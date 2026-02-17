// TestExecDlg.cpp : implementation file

// Note:	TestStand installs the source code files for the default user interfaces in the <TestStand>\UserInterfaces and <TestStand Public>\UserInterfaces directories. 
//			To modify the installed user interfaces or to create new user interfaces, modify the files in the <TestStand Public>\UserInterfaces directory. 
//			You can use the read-only source files for the default user interfaces in the <TestStand>\UserInterfaces directory as a reference. 
//			National Instruments recommends that you track the changes you make to the user interface source code files so you can integrate the changes with any enhancements in future versions of the TestStand User Interfaces.

#include "StdAfx.h"
#include "TestExec.h"
#include "TestExecDlg.h"

// TestStand API, TestStand UI ActiveX controls API, and utilities for using these APIs in C++
// The file is located in <TestStand>\API\VC 
#include "tsutilCPP.h"	

// using allows us to not type the namespace qualifier in front of each api call
using namespace TS;
using namespace TSUI;

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////

// MFC WIZARD CODE: 
CTestExecDlg::CTestExecDlg(CWnd* pParent /*=NULL*/) :
	CDialog(CTestExecDlg::IDD, pParent)
{
	//{{AFX_DATA_INIT(CTestExecDlg)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
	// Note that LoadIcon does not require a subsequent DestroyIcon in Win32
	mIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);	
	if (mIcon == NULL)
		AfxThrowResourceException();
}

// MFC WIZARD CODE: 
void CTestExecDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CTestExecDlg)
	//}}AFX_DATA_MAP
}

// MFC WIZARD CODE: 
BEGIN_MESSAGE_MAP(CTestExecDlg, CDialog)
	//{{AFX_MSG_MAP(CTestExecDlg)
	ON_WM_PAINT()
	ON_WM_CLOSE()
	ON_WM_QUERYDRAGICON()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CTestExecDlg message handlers

BOOL CTestExecDlg::OnInitDialog()
{
	bool startUpSuccessful = false;

	TS_MFC_TRY
		{
		CDialog::OnInitDialog();

		// set the icon for this dialog.  the framework does this automatically
		// when the application's main window is not a dialog
		SetIcon(mIcon, TRUE);		// Set big icon
		SetIcon(mIcon, FALSE);		// Set small icon

		// initialize CWnds for ActiveX controls
		mApplicationMgrCWnd.Attach(GetDlgItem(IDC_APPLICATIONMGR)->m_hWnd);
		mSequenceFileViewMgrCWnd.Attach(GetDlgItem(IDC_SEQUENCEFILEVIEWMGR)->m_hWnd);
		mExecutionViewMgrCWnd.Attach(GetDlgItem(IDC_EXECUTIONVIEWMGR)->m_hWnd);
		mFilesComboCWnd.Attach(GetDlgItem(IDC_FILESCOMBO)->m_hWnd);
		mOpenFileBtnCWnd.Attach(GetDlgItem(IDC_OPENFILEBTN)->m_hWnd);
		mSequencesComboCWnd.Attach(GetDlgItem(IDC_SEQUENCESCOMBO)->m_hWnd);
		mCloseFileBtnCWnd.Attach(GetDlgItem(IDC_CLOSEFILEBTN)->m_hWnd);
		mEntryPoint1BtnCWnd.Attach(GetDlgItem(IDC_ENTRYPOINT1BTN)->m_hWnd);
		mEntryPoint2BtnCWnd.Attach(GetDlgItem(IDC_ENTRYPOINT2BTN)->m_hWnd);
		mRunSelectedBtnCWnd.Attach(GetDlgItem(IDC_RUNSELECTEDSEQUENCEBTN)->m_hWnd);
		mExecutionsComboCWnd.Attach(GetDlgItem(IDC_EXECUTIONSCOMBO)->m_hWnd);
		mCloseExecutionBtnCWnd.Attach(GetDlgItem(IDC_CLOSEEXECUTIONBTN)->m_hWnd);
		mSequenceViewCWnd.Attach(GetDlgItem(IDC_SEQUENCEVIEW)->m_hWnd);
		mBreakResumeBtnCWnd.Attach(GetDlgItem(IDC_BREAKRESUMEBTN)->m_hWnd);
		mTerminateRestartBtnCWnd.Attach(GetDlgItem(IDC_TERMINATERESTARTBTN)->m_hWnd);
		mTerminateAllBtnCWnd.Attach(GetDlgItem(IDC_TERMINATEALLBTN)->m_hWnd);
		mLoginLogoutBtnCWnd.Attach(GetDlgItem(IDC_LOGINLOGOUTBTN)->m_hWnd);
		mExitBtnCWnd.Attach(GetDlgItem(IDC_EXITBTN)->m_hWnd);
		mReportViewCWnd.Attach(GetDlgItem(IDC_TSREPORTVIEW)->m_hWnd);

		// initialize activeX control smart pointers for TestStand UI controls
		mApplicationMgr =		mApplicationMgrCWnd.GetControlUnknown();
		mSequenceFileViewMgr =	mSequenceFileViewMgrCWnd.GetControlUnknown();
		mExecutionViewMgr =		mExecutionViewMgrCWnd.GetControlUnknown();
		mFilesCombo =			mFilesComboCWnd.GetControlUnknown();
		mOpenFileBtn =			mOpenFileBtnCWnd.GetControlUnknown();
		mSequencesCombo =		mSequencesComboCWnd.GetControlUnknown();
		mCloseFileBtn =			mCloseFileBtnCWnd.GetControlUnknown();
		mEntryPoint1Btn =		mEntryPoint1BtnCWnd.GetControlUnknown();
		mEntryPoint2Btn =		mEntryPoint2BtnCWnd.GetControlUnknown();
		mRunSelectedBtn =		mRunSelectedBtnCWnd.GetControlUnknown();
		mExecutionsCombo =		mExecutionsComboCWnd.GetControlUnknown();
		mCloseExecutionBtn =	mCloseExecutionBtnCWnd.GetControlUnknown();
		mSequenceView =			mSequenceViewCWnd.GetControlUnknown();
		mBreakResumeBtn =		mBreakResumeBtnCWnd.GetControlUnknown();
		mTerminateRestartBtn =	mTerminateRestartBtnCWnd.GetControlUnknown();
		mTerminateAllBtn =		mTerminateAllBtnCWnd.GetControlUnknown();
		mLoginLogoutBtn =		mLoginLogoutBtnCWnd.GetControlUnknown();
		mExitBtn =				mExitBtnCWnd.GetControlUnknown();
		mReportView =			mReportViewCWnd.GetControlUnknown();
	
		// connect controls	

		// connect TestStand comboboxes 
		mSequenceFileViewMgr->ConnectSequenceFileList(mFilesCombo, VARIANT_TRUE);
		mSequenceFileViewMgr->ConnectSequenceList(mSequencesCombo);
		ExecutionListConnectionPtr connection = mExecutionViewMgr->ConnectExecutionList(mExecutionsCombo);
			// specify what information to display in each execution list combobox entry (the expression string looks extra complicated here because we have to escape the quotes for the C++ compiler.)
		connection->PutDisplayExpression(_T("\"%CurrentExecution% - \" + (\"%UUTSerialNumber%\" == \"\" ? \"\" : (ResStr(\"TSUI_OI_MAIN_PANEL\",\"SERIAL_NUMBER\") + \" %UUTSerialNumber% - \")) + (\"%TestSocketIndex%\" == \"\" ? \"\" : (ResStr(\"TSUI_OI_MAIN_PANEL\",\"SOCKET_NUMBER\") + \" %TestSocketIndex% - \")) + \"%ModelState%\""));

		// connect sequence view to execution view manager									  
		mExecutionViewMgr->ConnectExecutionView(mSequenceView, ExecutionViewConnection_NoOptions);

		// connect report view to execution view manager									  
		mExecutionViewMgr->ConnectReportView(mReportView);

		// connect TestStand buttons to commands
		mApplicationMgr->ConnectCommand(mTerminateAllBtn, CommandKind_TerminateAll, 0, CommandConnection_EnableImage);
		mApplicationMgr->ConnectCommand(mLoginLogoutBtn, CommandKind_LoginLogout, 0, CommandConnection_EnableImage);
		mApplicationMgr->ConnectCommand(mExitBtn, CommandKind_Exit, 0, CommandConnection_EnableImage);
		mSequenceFileViewMgr->ConnectCommand(mOpenFileBtn, CommandKind_OpenSequenceFiles, 0, CommandConnection_EnableImage);
		mSequenceFileViewMgr->ConnectCommand(mCloseFileBtn, CommandKind_Close, 0, CommandConnection_EnableImage);
		mSequenceFileViewMgr->ConnectCommand(mEntryPoint1Btn, CommandKind_ExecutionEntryPoints_Set, 0, CommandConnection_EnableImage);
		mSequenceFileViewMgr->ConnectCommand(mEntryPoint2Btn, CommandKind_ExecutionEntryPoints_Set, 1, CommandConnection_EnableImage);
		mSequenceFileViewMgr->ConnectCommand(mRunSelectedBtn, CommandKind_RunCurrentSequence, 0, CommandConnection_EnableImage);
		mExecutionViewMgr->ConnectCommand(mCloseExecutionBtn, CommandKind_Close, 0, CommandConnection_EnableImage);
		mExecutionViewMgr->ConnectCommand(mBreakResumeBtn, CommandKind_BreakResume, 0, CommandConnection_EnableImage);
		mExecutionViewMgr->ConnectCommand(mTerminateRestartBtn, CommandKind_TerminateRestart, 0, CommandConnection_EnableImage);

		// show all step groups at once in the sequence view
		mExecutionViewMgr->StepGroupMode = TSUI::StepGroupMode_AllGroups; 

		// start up the TestStand User Interface Components. this also logs in the user
		mApplicationMgr->Start();

		startUpSuccessful = true;
		}
	TS_MFC_CATCH_AND_DISPLAY

	if (!startUpSuccessful)
		EndDialog(0);

	return TRUE;  // return TRUE  unless you set the focus to a control
}

///////////////////////////////////////////////////////////

// called when Close 'X' in the upper right-hand corner is pressed
void CTestExecDlg::OnClose() 
{
	TS_MFC_TRY
		{
			// After the ApplicationMgr shuts down by closing all files and logging out the user, it sends an OnExitApplication
			// event.  The handler for that event exits the dialog
		mApplicationMgr->Shutdown();
		}
	TS_MFC_CATCH_AND_DISPLAY
}

///////////////////////////////////////////////////////////

// the ApplicationMgr control sends this event after the processing initiated by the
// first call to IApplicationMgr.ShutDown completes.
void CTestExecDlg::OnExitApplication_ApplicationMgr()
{
	EndDialog(mApplicationMgr->ExitCode);
}

///////////////////////////////////////////////////////////

// the ApplicationMgr control sends this event when it's busy doing something so we know to display a hourglass cursor or equivalent
void CTestExecDlg::OnWait_ApplicationMgr(BOOL showWaitVal) 
{
	if (showWaitVal)
		BeginWaitCursor();
	else
		EndWaitCursor();	
}

///////////////////////////////////////////////////////////

// the ApplicationMgr sends this event when the TestStand UI Controls need to display an error
void CTestExecDlg::OnHandleError_ApplicationMgr(long errorCode, LPCTSTR errorMessage) 
{
	this->mApplicationMgr->GetEngine()->DisplayErrorDialog("Error", errorMessage, errorCode, CommonDlgOption_DisableGotoLocation);
}

///////////////////////////////////////////////////////////

// the ApplicationMgr sends this event to request that the UI display a particular execution
void CTestExecDlg::OnDisplayExecutionApplicationMgr(LPDISPATCH exec, ExecutionDisplayReasons reason) 
{
	// bring application to front if we hit a breakpoint
	if (reason == TSUI::ExecutionDisplayReason_Breakpoint || reason == TSUI::ExecutionDisplayReason_BreakOnRunTimeError)
		this->SetForegroundWindow();

	mExecutionViewMgr->PutRefExecution(TS::ExecutionPtr(exec));
}

///////////////////////////////////////////////////////////

// the ApplicationMgr sends this event to request that the UI display a particular sequence file
void CTestExecDlg::OnDisplaySequenceFileApplicationMgr(LPDISPATCH file, SequenceFileDisplayReasons reason) 
{
	mSequenceFileViewMgr->PutRefSequenceFile(TS::SequenceFilePtr(file));
}

///////////////////////////////////////////////////////////

//  MFC WIZARD CODE: If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model, this is automatically done for you by the framework.
void CTestExecDlg::OnPaint() 
{
	if (IsIconic())
		{
		CPaintDC dc(this); // device context for painting

		SendMessage(WM_ICONERASEBKGND, (WPARAM) dc.GetSafeHdc(), 0);

		// Center icon in client rectangle
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		dc.DrawIcon(x, y, mIcon);
		}
	else
		CDialog::OnPaint();
}

///////////////////////////////////////////////////////////

// MFC WIZARD CODE: The system calls this to obtain the cursor to display while the user drags the minimized window.
HCURSOR CTestExecDlg::OnQueryDragIcon()
{
	return (HCURSOR)mIcon;
}

///////////////////////////////////////////////////////////

// MFC WIZARD CODE:
BEGIN_EVENTSINK_MAP(CTestExecDlg, CDialog)
    //{{AFX_EVENTSINK_MAP(CTestExecDlg)
	ON_EVENT(CTestExecDlg, IDC_APPLICATIONMGR, 1 /* ExitApplication */, OnExitApplication_ApplicationMgr, VTS_NONE)
	ON_EVENT(CTestExecDlg, IDC_APPLICATIONMGR, 10 /* Wait */, OnWait_ApplicationMgr, VTS_BOOL)
	ON_EVENT(CTestExecDlg, IDC_APPLICATIONMGR, 11 /* HandleError */, OnHandleError_ApplicationMgr, VTS_I4 VTS_BSTR)
	ON_EVENT(CTestExecDlg, IDC_APPLICATIONMGR, 22 /* DisplayExecution */, OnDisplayExecutionApplicationMgr, VTS_DISPATCH VTS_I4)
	ON_EVENT(CTestExecDlg, IDC_APPLICATIONMGR, 21 /* DisplaySequenceFile */, OnDisplaySequenceFileApplicationMgr, VTS_DISPATCH VTS_I4)
	//}}AFX_EVENTSINK_MAP
END_EVENTSINK_MAP()


