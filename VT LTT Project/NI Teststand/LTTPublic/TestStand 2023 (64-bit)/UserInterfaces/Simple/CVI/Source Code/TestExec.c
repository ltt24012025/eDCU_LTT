#include <cvirte.h>		
#include <userint.h>
#include "TestExec.h"	// UIR header
#include "tsutil.h"		// Helpful CVI functions for TestStand
#include "tsui.h"		// API's for the TestStand ActiveX controls
#include "tsuisupp.h"	

// Note:	TestStand installs the source code files for the default user interfaces in the <TestStand>\UserInterfaces and <TestStand Public>\UserInterfaces directories. 
//			To modify the installed user interfaces or to create new user interfaces, modify the files in the <TestStand Public>\UserInterfaces directory. 
//			You can use the read-only source files for the default user interfaces in the <TestStand>\UserInterfaces directory as a reference. 
//			National Instruments recommends that you track the changes you make to the user interface source code files so you can integrate the changes with any enhancements in future versions of the TestStand User Interfaces.

// this structure holds the handles to the objects that make up an application window
typedef struct 
{
	int			panel;
	CAObjHandle	engine;
	
	// ActiveX control handles:
	CAObjHandle applicationMgr;
	CAObjHandle sequenceFileViewMgr;
  	CAObjHandle executionViewMgr;
	CAObjHandle reportView;
	CAObjHandle filesCombo;
	CAObjHandle openFileBtn;
	CAObjHandle sequencesCombo;
	CAObjHandle closeFileBtn;
	CAObjHandle entryPoint1Btn;
	CAObjHandle entryPoint2Btn;
	CAObjHandle runSelectedBtn;
	CAObjHandle executionsCombo;
	CAObjHandle closeExecutionBtn;
	CAObjHandle sequenceView;
	CAObjHandle breakResumeBtn;
	CAObjHandle terminateRestartBtn;
	CAObjHandle terminateAllBtn;
	CAObjHandle loginLogoutBtn;
	CAObjHandle exitBtn;
} MainPanel;


static MainPanel 	gMainWindow;	// the application only has one window


// the presence of these two variables is expected by the tsErrChk macro from tsutil.h.  Usually you declare these variables as locals
// in each function that uses tsErrChk. However, since all the code in this file runs in a single thread, they can be globals for convenience
ERRORINFO	errorInfo = {0, 0, "", "", "", 0, 0};
ErrMsg		errMsg = "";

static int 	SetupActiveXControls(void);
static int	ExitApplication(void);
static void DisplayError(int errorCode);
static void ClearErrorMessage(void);
static int 	MainCallback(int panelOrMenuBarHandle, int controlOrMenuItemID, int event, void *callbackData, int eventData1, int eventData2);

///////////////////////////////////////////////////////////////////////////

int main(int argc, char *argv[])
{
	int		error = 0;
	long	exitCode = 0;
	
	nullChk( InitCVIRTE(0, argv, 0));	// initialize CVI runtime engine

	// load the panel for the main window from the .UIR file
	errChk( gMainWindow.panel = LoadPanelEx (0, "TestExec.uir", MAINPANEL, __CVIUserHInst));
	
	// get ActiveX ctrl handles, register ActiveX event callbacks, and connect TestStand controls
	errChk( SetupActiveXControls());
	
	errChk( InstallMainCallback(MainCallback, 0, 0));	// handle the EVENT_END_TASK event

	// make a handle to engine conveniently accessible
	tsErrChk( TSUI_ApplicationMgrGetEngine(gMainWindow.applicationMgr, &errorInfo, &gMainWindow.engine));	

	// start up the TestStand User Interface Components. this also logs in the user
	tsErrChk( TSUI_ApplicationMgrStart(gMainWindow.applicationMgr, &errorInfo));
	
	// display window and process user input until application exits
	errChk( DisplayPanel(gMainWindow.panel));
	errChk( RunUserInterface());

	errChk( TSUI_ApplicationMgrGetExitCode(gMainWindow.applicationMgr, &errorInfo, &exitCode));
	
Error:	
	if (gMainWindow.panel > 0)
		DiscardPanel(gMainWindow.panel);
		
	if (UIEActiveXCtrlNotRegistered == error)
	{
		// This usually means the current active version of TestStand does not match the version
		// this application was built with.
#ifndef _WIN64
		const char msg[] = "This TestStand User Interface was built for a version of TestStand different than the version that is currently active.\n"
						   "Use the TestStand Version Selector to activate the correct version and ensure that 32-bit TestStand is installed.";
#else
		const char msg[] = "This TestStand User Interface was built for a version of TestStand different than the version that is currently active.\n"
						   "Use the TestStand Version Selector to activate the correct version and ensure that 64-bit TestStand is installed.";
#endif
		MessagePopup("Error", msg);
	}
	else
	{
		DisplayError(error);	
	}

	return exitCode;
}

////////////////////////////////////////////////////////////////////////////////////

int CVICALLBACK MainPanelCallback(int panel, int event, void *callbackData, int eventData1, int eventData2)
{
	switch (event)
	{
		case EVENT_CLOSE:		// EVENT_CLOSE == user clicked on window close box
			ExitApplication(); 	// this function displays error, if any
	}
		
	return 0;
}

///////////////////////////////////////////////////////////////////////////

static int MainCallback(int panelOrMenuBarHandle, int controlOrMenuItemID, int event, void *callbackData, int eventData1, int eventData2)
{
	switch (event)
	{
		case EVENT_END_TASK:	// EVENT_END_TASK can occur when windows shuts down or when the user selects Close from the context menu for the application's task bar item
			if (!ExitApplication() && !eventData1) 	
				return 1; //  don't immediately exit if we have cleanup to do and the whole computer is not shutting down
			break;
	}

	return 0;
}

///////////////////////////////////////////////////////////////////////////

// the ApplicationMgr control sends this event when it is ok to exit the application.
// discard any handles to TestStand objects here at the latest
HRESULT CVICALLBACK ApplicationMgr_OnExitApplication(CAObjHandle caServerObjHandle, void *caCallbackData)
{
	CA_DiscardObjHandle(gMainWindow.engine);
	gMainWindow.engine = 0;

	ExitApplication();		
	return S_OK;
}

///////////////////////////////////////////////////////////////////////////

// the ApplicationMgr control sends this event when it's busy doing something so we know to display a hourglass cursor or equivalent
HRESULT CVICALLBACK ApplicationMgr_OnWait(CAObjHandle caServerObjHandle, void *caCallbackData, VBOOL  bShowWait)
{
	SetMouseCursor(bShowWait ? VAL_HOUR_GLASS_CURSOR : VAL_DEFAULT_CURSOR);
	return S_OK;
}

///////////////////////////////////////////////////////////////////////////

// the ApplicationMgr sends this event when the TestStand UI Controls need to display an error
HRESULT CVICALLBACK ApplicationMgr_OnReportError(CAObjHandle caServerObjHandle, void *caCallbackData, long  errorCode, char *errorMessage)
{
	int	error = 0;
	
	if(!gMainWindow.engine)
	{
		strncpy(errMsg, errorMessage, sizeof(ERRMSG_SIZE));   // update global errMsg buffer
		errMsg[ERRMSG_SIZE - 1] = '\0';
		DisplayError(errorCode);
	}
	else
	{
		tsErrChk(TS_EngineDisplayErrorDialog(gMainWindow.engine, &errorInfo, "Error", errorMessage, errorCode, TS_CommonDlgOption_DisableGotoLocation));
	}
	
Error:
	
	return S_OK;
}

///////////////////////////////////////////////////////////////////////////

// the ApplicationMgr sends this event to request that the UI display a particular execution
HRESULT CVICALLBACK ApplicationMgr_OnDisplayExecution(CAObjHandle caServerObjHandle, void *caCallbackData, TSUIObj_Execution execution, enum TSUIEnum_ExecutionDisplayReasons reason)
{
	int	error = 0;

	// bring application to front if we hit a breakpoint
	if (reason == TSUIConst_ExecutionDisplayReason_Breakpoint || reason == TSUIConst_ExecutionDisplayReason_BreakOnRunTimeError)
		errChk( SetActivePanel(gMainWindow.panel));

	tsErrChk( TSUI_ExecutionViewMgrSetByRefExecution(gMainWindow.executionViewMgr, &errorInfo, execution));

Error:
	DisplayError(error);
	return error < 0 ? E_FAIL : S_OK;
}

///////////////////////////////////////////////////////////////////////////

// the ApplicationMgr sends this event to request that the UI display a particular sequence file
HRESULT CVICALLBACK ApplicationMgr_OnDisplaySequenceFile(CAObjHandle caServerObjHandle, void *caCallbackData, TSUIObj_SequenceFile file, enum TSUIEnum_SequenceFileDisplayReasons reason)
{
	int	error = 0;

	tsErrChk( TSUI_SequenceFileViewMgrSetByRefSequenceFile(gMainWindow.sequenceFileViewMgr, &errorInfo, file));

Error:
	DisplayError(error);
	return error < 0 ? E_FAIL : S_OK;
}

///////////////////////////////////////////////////////////////////////////

// call this function to exit the program
static int ExitApplication(void)
{
	int		error = 0; 
	VBOOL	canExitNow;
	
	// The first call to ApplicationMgrShutDown unloads files, logs out, runs unload callbacks, and finally triggers an OnApplicationCanExit event, which calls  
	// this function again. When the second call ApplicationMgrShutdown returns true for canExitNow, we call QuitUserInterface, which causes the RunUserInterface call to return.
	tsErrChk( TSUI_ApplicationMgrShutdown(gMainWindow.applicationMgr, &errorInfo, &canExitNow));
	if (canExitNow)
		QuitUserInterface(0);
	
Error:
	DisplayError(error);
	return canExitNow ? TRUE : FALSE;
}

///////////////////////////////////////////////////////////////////////////

// call this function after you handle an error, unless you handle the error by calling DisplayError, which also calls this function
static void ClearErrorMessage(void)
{
		// clear out error message globals so that a future error that lacks an error description does not
		// unintentionally use the error description from a prior error.
	*errMsg = '\0';
	memset(&errorInfo, 0, sizeof(ERRORINFO)); 
}

//////////////////////////////////////////////

// displays a message box with the error code, the error message associated with the code, and any error description details 
// does nothing if errorCode is not negative
static void DisplayError(int errorCode)
{
	if (errorCode < 0)
	{
		TS_DisplayError(errorCode, errMsg, gMainWindow.engine);	// errMsg is a global, see top of file
		ClearErrorMessage();	
	}
}

///////////////////////////////////////////////////////////////////////////

// obtain ActiveX control handles and register ActiveX event callbacks
static int SetupActiveXControls(void)
{
	int			error = 0;
	CAObjHandle	connection = 0;
	
	// get handles to ActiveX controls
	errChk( GetObjHandleFromActiveXCtrl(gMainWindow.panel, MAINPANEL_APPLICATIONMGR, 		&gMainWindow.applicationMgr));
	errChk( GetObjHandleFromActiveXCtrl(gMainWindow.panel, MAINPANEL_SEQUENCEFILEVIEWMGR,	&gMainWindow.sequenceFileViewMgr));
	errChk( GetObjHandleFromActiveXCtrl(gMainWindow.panel, MAINPANEL_EXECUTIONVIEWMGR, 		&gMainWindow.executionViewMgr));
	errChk( GetObjHandleFromActiveXCtrl(gMainWindow.panel, MAINPANEL_FILESCOMBO, 			&gMainWindow.filesCombo));
	errChk( GetObjHandleFromActiveXCtrl(gMainWindow.panel, MAINPANEL_OPENFILEBTN, 			&gMainWindow.openFileBtn));
	errChk( GetObjHandleFromActiveXCtrl(gMainWindow.panel, MAINPANEL_SEQUENCESCOMBO,		&gMainWindow.sequencesCombo));
	errChk( GetObjHandleFromActiveXCtrl(gMainWindow.panel, MAINPANEL_CLOSEFILEBTN, 			&gMainWindow.closeFileBtn));
	errChk( GetObjHandleFromActiveXCtrl(gMainWindow.panel, MAINPANEL_ENTRYPOINT1BTN, 		&gMainWindow.entryPoint1Btn));
	errChk( GetObjHandleFromActiveXCtrl(gMainWindow.panel, MAINPANEL_ENTRYPOINT2BTN, 		&gMainWindow.entryPoint2Btn));
	errChk( GetObjHandleFromActiveXCtrl(gMainWindow.panel, MAINPANEL_RUNSELECTEDBTN,		&gMainWindow.runSelectedBtn));
	errChk( GetObjHandleFromActiveXCtrl(gMainWindow.panel, MAINPANEL_EXECUTIONSCOMBO, 		&gMainWindow.executionsCombo));
	errChk( GetObjHandleFromActiveXCtrl(gMainWindow.panel, MAINPANEL_CLOSEEXECUTIONBTN, 	&gMainWindow.closeExecutionBtn));
	errChk( GetObjHandleFromActiveXCtrl(gMainWindow.panel, MAINPANEL_SEQUENCEVIEW, 			&gMainWindow.sequenceView));
	errChk( GetObjHandleFromActiveXCtrl(gMainWindow.panel, MAINPANEL_BREAKRESUMEBTN, 		&gMainWindow.breakResumeBtn));
	errChk( GetObjHandleFromActiveXCtrl(gMainWindow.panel, MAINPANEL_TERMINATERESTARTBTN, 	&gMainWindow.terminateRestartBtn));
	errChk( GetObjHandleFromActiveXCtrl(gMainWindow.panel, MAINPANEL_TERMINATEALLBTN, 		&gMainWindow.terminateAllBtn));
	errChk( GetObjHandleFromActiveXCtrl(gMainWindow.panel, MAINPANEL_LOGINLOGOUTBTN, 		&gMainWindow.loginLogoutBtn));
	errChk( GetObjHandleFromActiveXCtrl(gMainWindow.panel, MAINPANEL_EXITBTN, 				&gMainWindow.exitBtn));
	errChk( GetObjHandleFromActiveXCtrl(gMainWindow.panel, MAINPANEL_REPORTVIEW, 			&gMainWindow.reportView));
	
	// register ActiveX control event callbacks
	errChk( TSUI__ApplicationMgrEventsRegOnExitApplication(gMainWindow.applicationMgr, 		ApplicationMgr_OnExitApplication, NULL, 1, NULL));
	errChk( TSUI__ApplicationMgrEventsRegOnWait(gMainWindow.applicationMgr, 				ApplicationMgr_OnWait, NULL, 1, NULL));
	errChk( TSUI__ApplicationMgrEventsRegOnReportError(gMainWindow.applicationMgr, 			ApplicationMgr_OnReportError, NULL, 1, NULL));
	errChk( TSUI__ApplicationMgrEventsRegOnDisplaySequenceFile(gMainWindow.applicationMgr,	ApplicationMgr_OnDisplaySequenceFile, NULL, 1, NULL));
	errChk( TSUI__ApplicationMgrEventsRegOnDisplayExecution(gMainWindow.applicationMgr, 	ApplicationMgr_OnDisplayExecution, NULL, 1, NULL));
	
	// connect TestStand comboboxes 
		
	tsErrChk( TSUI_SequenceFileViewMgrConnectSequenceFileList(gMainWindow.sequenceFileViewMgr, &errorInfo, gMainWindow.filesCombo, VTRUE, NULL));
	
	tsErrChk( TSUI_SequenceFileViewMgrConnectSequenceList(gMainWindow.sequenceFileViewMgr, &errorInfo, gMainWindow.sequencesCombo, NULL));

	tsErrChk( TSUI_ExecutionViewMgrConnectExecutionList(gMainWindow.executionViewMgr, &errorInfo, gMainWindow.executionsCombo, &connection));
	
	// specify what information to display in each execution list combobox entry (the expression string looks extra complicated here because we have to escape the quotes for the C compiler.)
	tsErrChk( TSUISUPP_ExecutionListConnectionSetDisplayExpression(connection, &errorInfo, "\"%CurrentExecution% - \" + (\"%UUTSerialNumber%\" == \"\" ? \"\" : (ResStr(\"TSUI_OI_MAIN_PANEL\",\"SERIAL_NUMBER\") + \" %UUTSerialNumber% - \")) + (\"%TestSocketIndex%\" == \"\" ? \"\" : (ResStr(\"TSUI_OI_MAIN_PANEL\",\"SOCKET_NUMBER\") + \" %TestSocketIndex% - \")) + \"%ModelState%\""));

	// connect sequence view to execution view manager									  
	tsErrChk( TSUI_ExecutionViewMgrConnectExecutionView(gMainWindow.executionViewMgr, &errorInfo, gMainWindow.sequenceView, TSUIConst_ExecutionViewConnection_NoOptions, NULL));

	// connect report view to execution view manager									  
	tsErrChk( TSUI_ExecutionViewMgrConnectReportView(gMainWindow.executionViewMgr, &errorInfo, gMainWindow.reportView, NULL));

	// connect TestStand buttons to commands
	tsErrChk( TSUI_ApplicationMgrConnectCommand(gMainWindow.applicationMgr, &errorInfo, gMainWindow.terminateAllBtn, TSUIConst_CommandKind_TerminateAll, 0, TSUIConst_CommandConnection_EnableImage, NULL));

	tsErrChk( TSUI_ApplicationMgrConnectCommand(gMainWindow.applicationMgr, &errorInfo, gMainWindow.loginLogoutBtn, TSUIConst_CommandKind_LoginLogout, 0, TSUIConst_CommandConnection_EnableImage, NULL));

	tsErrChk( TSUI_ApplicationMgrConnectCommand(gMainWindow.applicationMgr, &errorInfo, gMainWindow.exitBtn, TSUIConst_CommandKind_Exit, 0, TSUIConst_CommandConnection_EnableImage, NULL));

	tsErrChk( TSUI_SequenceFileViewMgrConnectCommand(gMainWindow.sequenceFileViewMgr, &errorInfo, gMainWindow.openFileBtn, TSUIConst_CommandKind_OpenSequenceFiles, 0, TSUIConst_CommandConnection_EnableImage, NULL));
	
	tsErrChk( TSUI_SequenceFileViewMgrConnectCommand(gMainWindow.sequenceFileViewMgr, &errorInfo, gMainWindow.closeFileBtn, TSUIConst_CommandKind_Close, 0, TSUIConst_CommandConnection_EnableImage, NULL));

	tsErrChk( TSUI_SequenceFileViewMgrConnectCommand(gMainWindow.sequenceFileViewMgr, &errorInfo, gMainWindow.entryPoint1Btn, TSUIConst_CommandKind_ExecutionEntryPoints_Set, 0, TSUIConst_CommandConnection_EnableImage, NULL));

	tsErrChk( TSUI_SequenceFileViewMgrConnectCommand(gMainWindow.sequenceFileViewMgr, &errorInfo, gMainWindow.entryPoint2Btn, TSUIConst_CommandKind_ExecutionEntryPoints_Set, 1, TSUIConst_CommandConnection_EnableImage, NULL));

	tsErrChk( TSUI_SequenceFileViewMgrConnectCommand(gMainWindow.sequenceFileViewMgr, &errorInfo, gMainWindow.runSelectedBtn, TSUIConst_CommandKind_RunCurrentSequence, 0, TSUIConst_CommandConnection_EnableImage, NULL));

	tsErrChk( TSUI_ExecutionViewMgrConnectCommand(gMainWindow.executionViewMgr, &errorInfo, gMainWindow.closeExecutionBtn, TSUIConst_CommandKind_Close, 0, TSUIConst_CommandConnection_EnableImage, NULL));

	tsErrChk( TSUI_ExecutionViewMgrConnectCommand(gMainWindow.executionViewMgr, &errorInfo, gMainWindow.breakResumeBtn, TSUIConst_CommandKind_BreakResume, 0, TSUIConst_CommandConnection_EnableImage, NULL));
	
	tsErrChk( TSUI_ExecutionViewMgrConnectCommand(gMainWindow.executionViewMgr, &errorInfo, gMainWindow.terminateRestartBtn, TSUIConst_CommandKind_TerminateRestart, 0, TSUIConst_CommandConnection_EnableImage, NULL));

	// show all step groups at once in the sequence view
	tsErrChk( TSUI_ExecutionViewMgrSetStepGroupMode (gMainWindow.executionViewMgr, &errorInfo, TSUIConst_StepGroupMode_AllGroups));

Error:
	CA_DiscardObjHandle(connection);
	return error;
}

