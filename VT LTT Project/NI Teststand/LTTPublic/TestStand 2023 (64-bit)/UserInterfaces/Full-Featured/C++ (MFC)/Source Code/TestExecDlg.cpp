// Note:	This example can function as an editor or an operator interface. The user can change the edit mode with a keystroke (ctrl-alt-shift-insert) or with a command line 
//			argument. For more information and for instructions on how prevent the user from changing the edit mode, refer to the TestStand Reference Manual>>Creating Custom 
//			User Interfaces>>Editor versus Operator Interface Applications>>Creating Editor Applications.
 
// Note:	TestStand installs the source code files for the default user interfaces in the <TestStand>\UserInterfaces and <TestStand Public>\UserInterfaces directories. 
//			To modify the installed user interfaces or to create new user interfaces, modify the files in the <TestStand Public>\UserInterfaces directory. 
//			You can use the read-only source files for the default user interfaces in the <TestStand>\UserInterfaces directory as a reference. 
//			National Instruments recommends that you track the changes you make to the user interface source code files so you can integrate the changes with any enhancements in future versions of the TestStand User Interfaces.

// TestExecDlg.cpp : implementation file

#include "StdAfx.h"
#include "TestExec.h"
#include "TestExecDlg.h"
#include "SplashScreen.h"
#include "AboutBox.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

// list bar page indices
const int	SEQUENCE_FILES_PAGE_INDEX = 0;	// first page in list bar
const int	EXECUTIONS_PAGE_INDEX = 1;		// second page in list bar

/////////////////////////////////////////////////////////////////////////////

// MFC WIZARD CODE: 
CTestExecDlg::CTestExecDlg(CWnd* pParent /*=NULL*/) :
	CDialog(CTestExecDlg::IDD, pParent)
{
	mErrorDlgTitle = _T("Error"); // localized in OnInitDialog
	
	//{{AFX_DATA_INIT(CTestExecDlg)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
	// Note that LoadIcon does not requizre a subsequent DestroyIcon in Win32
	mIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);	
	if (mIcon == NULL)
		AfxThrowResourceException();
}

///////////////////////////////////////////////////////////

// MFC WIZARD CODE: 
void CTestExecDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CTestExecDlg)
	DDX_Control(pDX, IDC_MENUDIVIDERBAR, mMenuDividerBar);
	DDX_Control(pDX, IDC_TAB, m_tabCtrl);
	//}}AFX_DATA_MAP
}

///////////////////////////////////////////////////////////

// MFC WIZARD CODE: 
BEGIN_MESSAGE_MAP(CTestExecDlg, CDialog)
	//{{AFX_MSG_MAP(CTestExecDlg)
	ON_WM_PAINT()
	ON_WM_CLOSE()
	ON_WM_QUERYDRAGICON()
	ON_WM_INITMENU()
	ON_NOTIFY(TCN_SELCHANGE, IDC_TAB, OnSelchangeTab)
	ON_COMMAND(ID_HELP_ABOUT, OnHelpAbout)
	ON_WM_INITMENUPOPUP()
	ON_WM_SIZE()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

///////////////////////////////////////////////////////////

// MFC WIZARD CODE:
BEGIN_EVENTSINK_MAP(CTestExecDlg, CDialog)
    //{{AFX_EVENTSINK_MAP(CTestExecDlg)
	ON_EVENT(CTestExecDlg, IDC_APPLICATIONMGR, 1, OnExitApplicationApplicationMgr, VTS_NONE)
	ON_EVENT(CTestExecDlg, IDC_APPLICATIONMGR, 10, OnWaitApplicationMgr, VTS_BOOL)
	ON_EVENT(CTestExecDlg, IDC_APPLICATIONMGR, 11, OnReportErrorApplicationMgr, VTS_I4 VTS_BSTR)
	ON_EVENT(CTestExecDlg, IDC_APPLICATIONMGR, 22, OnDisplayExecutionApplicationMgr, VTS_DISPATCH VTS_I4)
	ON_EVENT(CTestExecDlg, IDC_APPLICATIONMGR, 21, OnDisplaySequenceFileApplicationMgr, VTS_DISPATCH VTS_I4)
	ON_EVENT(CTestExecDlg, IDC_LISTBAR, 2, OnCurPageChangedListbar, VTS_I4)
	ON_EVENT(CTestExecDlg, IDC_APPLICATIONMGR, 23, OnDisplayReportApplicationMgr, VTS_DISPATCH)
	ON_EVENT(CTestExecDlg, IDC_APPLICATIONMGR, 5, OnStartExecutionApplicationMgr, VTS_DISPATCH VTS_DISPATCH VTS_BOOL)
	ON_EVENT(CTestExecDlg, IDC_EXECUTIONVIEWMGR, 1, OnExecutionChangedExecutionViewMgr, VTS_DISPATCH)
	ON_EVENT(CTestExecDlg, IDC_LISTBAR, 3, OnCreateContextMenuListbar, VTS_I4 VTS_I4 VTS_I4)
	ON_EVENT(CTestExecDlg, IDC_LISTBAR, 4, OnBorderDraggedListbar, VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_BOOL)
	ON_EVENT(CTestExecDlg, IDC_APPLICATIONMGR, 16 , CTestExecDlg::OnQueryShutdownApplicationMgr, VTS_PI4)
	ON_EVENT(CTestExecDlg, IDC_APPLICATIONMGR, 42, CTestExecDlg::EditModeChangedApplicationMgr, VTS_NONE)
	ON_EVENT(CTestExecDlg, IDC_APPLICATIONMGR, 24, CTestExecDlg::PostCommandExecuteApplicationMgr, VTS_DISPATCH)
	//}}AFX_EVENTSINK_MAP
END_EVENTSINK_MAP()

///////////////////////////////////////////////////////////

// converts a CWnd to a TSUTIL::Control. This would be a constructor overload in TSUTIL::Control except that TSUTIL does not use any MFC classes
Control CWndToControl(CWnd &cWnd)
{
	return Control(cWnd.m_hWnd, cWnd.GetControlUnknown());
}

///////////////////////////////////////////////////////////

BOOL CTestExecDlg::OnInitDialog()
{
	bool	startUpSuccessful = false;

	TS_MFC_TRY
	{		
		CDialog::OnInitDialog();

		// initialize CWnds for ActiveX controls
		mApplicationMgrCWnd.Attach(GetDlgItem(IDC_APPLICATIONMGR)->m_hWnd);
		mSequenceFileViewMgrCWnd.Attach(GetDlgItem(IDC_SEQUENCEFILEVIEWMGR)->m_hWnd);
		mExecutionViewMgrCWnd.Attach(GetDlgItem(IDC_EXECUTIONVIEWMGR)->m_hWnd);
		mListBarCWnd.Attach(GetDlgItem(IDC_LISTBAR)->m_hWnd);
		mStatusBarCWnd.Attach(GetDlgItem(IDC_STATUSBAR)->m_hWnd);

		// initialize activeX control smart pointers for TestStand UI controls in the parent dialog	window 
		mApplicationMgr =		mApplicationMgrCWnd.GetControlUnknown();
		mSequenceFileViewMgr =	mSequenceFileViewMgrCWnd.GetControlUnknown();
		mExecutionViewMgr =		mExecutionViewMgrCWnd.GetControlUnknown();
		mListBar =				mListBarCWnd.GetControlUnknown();
		mStatusBar =			mStatusBarCWnd.GetControlUnknown();

		CSplashScreen	splashScreen;

		if (!mApplicationMgr->ApplicationWillExitOnStart)
		{
			// show splash screen 
			splashScreen.Create(IDD_SPLASHSCREEN);
			splashScreen.ShowWindow(SW_SHOWNORMAL);
		}

		// load the child window for each tab page
		InitializeTabPages(mApplicationMgr->GetEngine());		

		// connect controls that are always visible
		ConnectListBarPages();
		ConnectStatusBarPanes();

		// connect controls on the Sequence File tab
		mSequenceFileViewMgr->ConnectSequenceView(mFileTab.mFileStepList);
		mSequenceFileViewMgr->ConnectCommand(mFileTab.mEntryPoint1Btn, CommandKind_ExecutionEntryPoints_Set, 0, CommandConnection_NoOptions);
		mSequenceFileViewMgr->ConnectCommand(mFileTab.mEntryPoint2Btn, CommandKind_ExecutionEntryPoints_Set, 1, CommandConnection_NoOptions);
		mSequenceFileViewMgr->ConnectCommand(mFileTab.mRunSequenceBtn, CommandKind_RunCurrentSequence, 0, CommandConnection_NoOptions);
		mSequenceFileViewMgr->ConnectCaption(mFileTab.mSequenceFileLabel, CaptionSource_CurrentSequenceFile, VARIANT_FALSE);
        mSequenceFileViewMgr->ConnectVariables(mFileTab.mFileVariables);
        mSequenceFileViewMgr->ConnectInsertionPalette(mFileTab.mInsertionPalette);
		mSequenceFileViewMgr->ConnectSequenceList(mFileTab.mSequencesList)->SetColumnVisible(SeqListConnectionColumn_Comments, true);

		// connect controls on the Execution tab
		mExecutionViewMgr->ConnectExecutionView(mExecutionTab.mExecutionStepList, ExecutionViewConnection_NoOptions);
        mExecutionViewMgr->ConnectVariables(mExecutionTab.mExecutionVariables);
		mExecutionViewMgr->ConnectCallStack(mExecutionTab.mCallStack);
		mExecutionViewMgr->ConnectThreadList(mExecutionTab.mThreads);
		mExecutionViewMgr->ConnectCommand(mExecutionTab.mBreakResumeBtn, CommandKind_BreakResume, 0, CommandConnection_NoOptions);
		mExecutionViewMgr->ConnectCommand(mExecutionTab.mTerminateRestartBtn, CommandKind_TerminateRestart, 0, CommandConnection_NoOptions);
		mExecutionViewMgr->ConnectCaption(mExecutionTab.mExecutionLabel, CaptionSource_CurrentExecution, VARIANT_FALSE);

		// connect controls on the Report tab
		mExecutionViewMgr->ConnectReportView(mReportTab.mReportView);

		// set the icon for this dialog.  the framework does this automatically
		// when the application's main window is not a dialog
		SetIcon(mIcon, TRUE);		// Set big icon
		SetIcon(mIcon, FALSE);		// Set small icon
	
		// normally, the CFrameWnd class would load the accelerator table. Since this example doesn't use the CFrameWnd class, we load the accelerators here.
		mAccelTable = LoadAccelerators(AfxGetResourceHandle(), _T("IDR_ACCELERATOR"));
		if (!mAccelTable)
			AfxThrowResourceException();

		// this application allows setting of breakpoints on sequences files, so let them persist
		mApplicationMgr->GetEngine()->PutPersistBreakpoints(VARIANT_TRUE);

		// localize strings in top level menu items and controls
		TSUTIL::Localizer	localizer(mApplicationMgr->GetEngine());
		localizer.LocalizeWindow(m_hWnd, _T("TSUI_OI_MAIN_PANEL"), true);	// localize windows controls and windows menu items
		mApplicationMgr->LocalizeAllControls(_T("TSUI_OI_MAIN_PANEL"));		// localize TestStand UI Controls
		GetAdditionalLocalizedStrings(localizer);

		// start up the TestStand User Interface Components. this also logs in the user
		mApplicationMgr->Start();

		// remember window and control positions from last time
		mWindowsToPersistSizesFor.push_back(CWndToControl(mListBarCWnd));
		mWindowsToPersistSizesFor.push_back(CWndToControl(m_tabCtrl));		
		mWindowsToPersistSizesFor.push_back(CWndToControl(mFileTab.mFileStepListCWnd));
		mWindowsToPersistSizesFor.push_back(CWndToControl(mFileTab.mSequencesListCWnd));
		mWindowsToPersistSizesFor.push_back(CWndToControl(mFileTab.mFileVariablesCWnd));
		mWindowsToPersistSizesFor.push_back(CWndToControl(mFileTab.mInsertionPaletteCWnd));
		mWindowsToPersistSizesFor.push_back(CWndToControl(mExecutionTab.mExecutionStepListCWnd));
		mWindowsToPersistSizesFor.push_back(CWndToControl(mExecutionTab.mCallStackCWnd));
		mWindowsToPersistSizesFor.push_back(CWndToControl(mExecutionTab.mThreadsCWnd));
		mWindowsToPersistSizesFor.push_back(CWndToControl(mExecutionTab.mExecutionVariablesCWnd));

		mWindowsToPersistBoundsFor.push_back(CWndToControl(*this));

		LayoutPersister::LoadSizes(mApplicationMgr, mWindowsToPersistSizesFor);
		LayoutPersister::LoadBounds(mApplicationMgr, mWindowsToPersistBoundsFor);

		ArrangeControls();  // arrange controls for the first time

		// setup menu bar
		mMenuBuilder.SetWindow(m_hWnd);		// let the TSUTIL::MenuBuilder know which menubar to act on by specifing the window that contains the menubar
		RebuildMenuBar();					// build menu bar contents for the first time

		// decide which tab pages to initially show
		ShowAppropriateTabs();
		
		startUpSuccessful = true;
	}
	TS_MFC_CATCH_AND_DISPLAY

	if (!startUpSuccessful)
		EndDialog(0);	

	return TRUE;  // return TRUE  unless you set the focus to a control
}

///////////////////////////////////////////////////////////

// load the child windows for the three tab pages
void CTestExecDlg::InitializeTabPages(IEnginePtr engine)
{
	// load the tab page child windows from their dialog resources
	mReportTab.Create(IDD_REPORT_TAB_DIALOG, this);
	mExecutionTab.Create(IDD_EXECUTION_TAB_DIALOG, this);
	mFileTab.Create(IDD_FILE_TAB_DIALOG, this);

	// move tabs to the front
	mFileTab.SetWindowPos(&wndTop, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
	mExecutionTab.SetWindowPos(&wndTop, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
	mReportTab.SetWindowPos(&wndTop, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);

	// enable xp tab themes for child dialogs used as tab pages
	TSUTIL::DialogHelper	dialogHelper;
	dialogHelper.EnableXPTabTheme(mFileTab.m_hWnd);
	dialogHelper.EnableXPTabTheme(mExecutionTab.m_hWnd);
	dialogHelper.EnableXPTabTheme(mReportTab.m_hWnd);
}

///////////////////////////////////////////////////////////

// if the execution has a report, switch to the report tab either immediately if the execution is visible, or, whenever the execution is viewed next
void CTestExecDlg::ShowReport(ExecutionPtr execution)
{
	// switch to report view when this execution is next viewed
	execution->AsPropertyObject()->SetValNumber(_T("NIUI.LastActiveTab"), PropOption_InsertIfMissing, (double)TabPageIdentifier_Report); // activate the reportTab when the user views this execution

	if (execution->GetId() == mExecutionViewMgr->GetExecution()->GetId()) // is this execution the currently displayed execution?			
		ShowAppropriateTabs();  // switch to report view tab now
}

///////////////////////////////////////////////////////////

// show the sequence file list and execution list in list bar pages
void CTestExecDlg::ConnectListBarPages()
{
	// connect listbar page 0 to SequenceFileList
	mSequenceFileViewMgr->ConnectSequenceFileList(mListBar->GetPages()->Item[0L], false);

	// connect listbar page 1 to ExecutionList
	ExecutionListConnectionPtr connection = mExecutionViewMgr->ConnectExecutionList(mListBar->GetPages()->Item[1L]);
	// display the execution name on the first line, the serial number (if any) on the next line, and the model execution state on the last line (the expression string looks complicated here because we have to escape the quotes and backslashes for the C++ compiler.)
	connection->DisplayExpression = _T("\"%CurrentExecution%\\n\" + (\"%UUTSerialNumber%\" == \"\" ? \"\" : (ResStr(\"TSUI_OI_MAIN_PANEL\",\"SERIAL_NUMBER\") + \" %UUTSerialNumber%\\n\")) + (\"%TestSocketIndex%\" == \"\" ? \"\" : ResStr(\"TSUI_OI_MAIN_PANEL\",\"SOCKET_NUMBER\") + \" %TestSocketIndex%\\n\") + \"%ModelState%\"");
}

///////////////////////////////////////////////////////////

void CTestExecDlg::ConnectStatusBarPanes()
{
	StatusBarPanesPtr	panes = mStatusBar->GetPanes();

	// User
	mApplicationMgr->ConnectCaption(panes->GetItem(_T("User")), CaptionSource_UserName, false);
	// Engine Environment
	mApplicationMgr->ConnectCaption(panes->GetItem(_T("EngineEnvironment")), CaptionSource_EngineEnvironment, false);
	// File Process Model
	mSequenceFileViewMgr->ConnectCaption(panes->GetItem(_T("FileModel")), CaptionSource_CurrentProcessModelFile, false)->PutLongName(VARIANT_FALSE);
	// Execution Process Model
	mExecutionViewMgr->ConnectCaption(panes->GetItem(_T("ExecutionModel")), CaptionSource_CurrentProcessModelFile, false)->PutLongName(VARIANT_FALSE);
	// File Selected Steps
	mSequenceFileViewMgr->ConnectCaption(panes->GetItem(_T("FileSelectedSteps")), CaptionSource_SelectedSteps_ZeroBased, false);
	// File Number of Steps
	mSequenceFileViewMgr->ConnectCaption(panes->GetItem(_T("FileNumberOfSteps")), CaptionSource_NumberOfSteps, false);
	// Execution Selected Steps
	mExecutionViewMgr->ConnectCaption(panes->GetItem(_T("ExecutionSelectedSteps")), CaptionSource_SelectedSteps_ZeroBased, false);
	// Execution Number of Steps
	mExecutionViewMgr->ConnectCaption(panes->GetItem(_T("ExecutionNumberOfSteps")), CaptionSource_NumberOfSteps, false);
	// Report Location
	mExecutionViewMgr->ConnectCaption(panes->GetItem(_T("ReportLocation")), CaptionSource_ReportLocation, true);
	// Progress Text
	mExecutionViewMgr->ConnectCaption(panes->GetItem(_T("ProgressText")), CaptionSource_ProgressText, false);
	// Progress Percent Text
	mExecutionViewMgr->ConnectCaption(panes->GetItem(_T("ProgressPercent")), CaptionSource_ProgressPercent, false);
	// Progress Percent Bar
	mExecutionViewMgr->ConnectNumeric(panes->GetItem(_T("ProgressPercent")), NumericSource_ProgressPercent);
}

///////////////////////////////////////////////////////////

// the selected tab in the tab control has changed
void CTestExecDlg::OnSelchangeTab(NMHDR* pNMHDR, LRESULT* pResult) 
{
	if (!m_ProgrammaticallyUpdatingTabPages)	// filter out programmatically triggered activation events that might be due only to hidden tabs being made visible again
	{
		// remember which tab is active so when execution is revisited in the future, we can activate the same tab
		// is the new tab an execution tab and there is a current execution?
		if (mListBar->GetCurrentPage() == EXECUTIONS_PAGE_INDEX && mExecutionViewMgr->GetExecution() != NULL) 
		{
			TabPageIdentifier	thisTab = m_tabCtrl.GetCurSel() == 0 ? TabPageIdentifier_Execution : TabPageIdentifier_Report;
			
			// store the activated tab index in a custom property added to the execution
			mExecutionViewMgr->GetExecution()->AsPropertyObject()->SetValNumber(_T("NIUI.LastActiveTab"), PropOption_InsertIfMissing, (double)thisTab);
		}

		ShowAppropriateTabWindow();	// MFC doesn't show the window for the selected tab for you, you do it yourself
	}	

	ShowAppropriateStatusBarPanes();				
	
	*pResult = 0;
}

///////////////////////////////////////////////////////////

// In MFC, the tab control doesn't show and hide the tab pages for you when the selected page changes
void CTestExecDlg::ShowAppropriateTabWindow()
{
	TabPageIdentifier	tabPageToShow;

	if (mListBar->GetCurrentPage() == SEQUENCE_FILES_PAGE_INDEX)
		tabPageToShow = TabPageIdentifier_SequenceFile;
	else
	if (m_tabCtrl.GetCurSel() == 0)
		tabPageToShow = TabPageIdentifier_Execution;
	else
		tabPageToShow = TabPageIdentifier_Report;

    mFileTab.ShowWindow(tabPageToShow == TabPageIdentifier_SequenceFile ? SW_SHOWNORMAL : SW_HIDE);
    mExecutionTab.ShowWindow(tabPageToShow == TabPageIdentifier_Execution ? SW_SHOWNORMAL : SW_HIDE);
    mReportTab.ShowWindow(tabPageToShow == TabPageIdentifier_Report ? SW_SHOWNORMAL : SW_HIDE);

	// if we hide a tab page that contains a control with the focus, we must set the focus to 
	// a control on the visible tab page. otherwise, the focus will be stuck and keyboard input will
	// continue to go to the control on the hidden window. Also, MFC will hang in an infinite loop when processing shortcut keys.
	if (!DialogHelper::IsValidFocusWindow(::GetFocus()))
	{
		HWND dialogWnd = NULL;
		if(tabPageToShow == TabPageIdentifier_SequenceFile)
			dialogWnd = mFileTab.GetSafeHwnd();
		else
		if(tabPageToShow == TabPageIdentifier_Execution )
			dialogWnd = mExecutionTab.GetSafeHwnd();
		else
		if(tabPageToShow == TabPageIdentifier_Report )
			dialogWnd = mReportTab.GetSafeHwnd();

		DialogHelper::UpdateChildWndFocus(dialogWnd, (HWND)LongToHandle(mListBar->hWnd));
	}
}

///////////////////////////////////////////////////////////

// based on the current listbar page, show and hide the tabs that appear in the space to the right of the listbar
void CTestExecDlg::ShowAppropriateTabs()
{
	m_ProgrammaticallyUpdatingTabPages = true;	

	// show and hide tabs as appropriate (Windows doesn't let you hide a tab page, so we remove all pages and add back the ones we want to show)
	m_tabCtrl.DeleteAllItems();

	if (mListBar->GetCurrentPage() == SEQUENCE_FILES_PAGE_INDEX)
		m_tabCtrl.InsertItem(TCIF_TEXT, 0, mFileTabTitle, -1, 0);
	else
	{
		m_tabCtrl.InsertItem(TCIF_TEXT, 0, mExecutionTabTitle, -1, 0);
		m_tabCtrl.InsertItem(TCIF_TEXT, 1, mReportTabTitle, -1, 0);
	}

	// if we are viewing an execution...
	if (mListBar->GetCurrentPage() == EXECUTIONS_PAGE_INDEX)
	{
		TabPageIdentifier	lastActiveExecutionTab = TabPageIdentifier_NotATab; 

		// determine which tab page we last displayed for this execution
		ExecutionPtr execution = mExecutionViewMgr->GetExecution();
		if (execution != NULL)
			lastActiveExecutionTab = (TabPageIdentifier)(unsigned long)execution->AsPropertyObject()->GetValNumber(_T("NIUI.LastActiveTab"), PropOption_InsertIfMissing);

		// re-activate previously active tab for the execution
		if (lastActiveExecutionTab == TabPageIdentifier_Execution)
			m_tabCtrl.SetCurSel(0);
		if (lastActiveExecutionTab == TabPageIdentifier_Report)
			m_tabCtrl.SetCurSel(1);
	}

	m_ProgrammaticallyUpdatingTabPages = false;

	ShowAppropriateTabWindow();
	ShowAppropriateStatusBarPanes();
}

///////////////////////////////////////////////////////////

void CTestExecDlg::ShowAppropriateStatusBarPanes()
{
	if (m_tabCtrl.GetCurSel() >= 0)
	{
		if (mListBar->GetCurrentPage() == SEQUENCE_FILES_PAGE_INDEX) // if only the files tab is visible
			mStatusBar->ShowPanes(_T("User, EngineEnvironment, FileModel, FileSelectedSteps, FileNumberOfSteps"));
		else		
		if (m_tabCtrl.GetCurSel() == 0)	// execution tab is selected
			mStatusBar->ShowPanes(_T("User, EngineEnvironment, ExecutionModel, ExecutionSelectedSteps, ExecutionNumberOfSteps, ProgressText, ProgressPercent"));
		else							// report tab is selected
			mStatusBar->ShowPanes(_T("User, EngineEnvironment, ExecutionModel, ReportLocation, ReportModel, ProgressText, ProgressPercent"));
	}
}

///////////////////////////////////////////////////////////

// get localized text for strings that are not otherwise localized
void CTestExecDlg::GetAdditionalLocalizedStrings(TSUTIL::Localizer &localizer)
{
	_variant_t unused;
	mErrorDlgTitle = localizer.LocalizeString(_T("TSUI_OI_MAIN_PANEL"), _T("ERR_BOX_TITLE"));
	mFileTabTitle = localizer.LocalizeString(_T("TSUI_OI_MAIN_PANEL"), _T("SEQUENCE_FILE"));
	mExecutionTabTitle = localizer.LocalizeString(_T("TSUI_OI_MAIN_PANEL"), _T("EXECUTION"));
	mReportTabTitle = localizer.LocalizeString(_T("TSUI_OI_MAIN_PANEL"), _T("REPORT"));
}

///////////////////////////////////////////////////////////

// called when Close 'X' in the upper right-hand corner is pressed
void CTestExecDlg::OnClose() 
{
	// After the ApplicationMgr shuts down by closing all files and logging out the user, it sends an OnExitApplication
	// event.  The handler for that event exits the dialog
	mApplicationMgr->Shutdown();
}

///////////////////////////////////////////////////////////

// release any TestStand objects and save any settings here
void CTestExecDlg::OnQueryShutdownApplicationMgr(long* opt)
{
	LayoutPersister::SaveSizes(mApplicationMgr, mWindowsToPersistSizesFor);
	LayoutPersister::SaveBounds(mApplicationMgr, mWindowsToPersistBoundsFor);
}

///////////////////////////////////////////////////////////

// the ApplicationMgr ActiveX control sends this event after the processing initiated by the first call to IApplicationMgr.ShutDown completes.
void CTestExecDlg::OnExitApplicationApplicationMgr()
{
	// discard any TSUTIL::MenuBuilder menu items. These menu items might refer to TestStand objects, so delete them before the engine is destroyed. Note that it is too early to do this in QueryShutdown because a menu might be used after QueryShutdown returns, particularly if an unload callback runs.
	mMenuBuilder.RemoveMenuCommands(false);

	EndDialog(mApplicationMgr->ExitCode);
}

///////////////////////////////////////////////////////////

// the ApplicationMgr ActiveX control sends this event when it's busy doing something so we know to display a hourglass cursor or equivalent
void CTestExecDlg::OnWaitApplicationMgr(BOOL showWaitVal) 
{
	if (showWaitVal)
		BeginWaitCursor();
	else
		EndWaitCursor();	
}

///////////////////////////////////////////////////////////

// the ApplicationMgr sends this event when the TestStand UI Controls need to display an error
void CTestExecDlg::OnReportErrorApplicationMgr(long errorCode, LPCTSTR errorMessage) 
{
	this->mApplicationMgr->GetEngine()->DisplayErrorDialog(mErrorDlgTitle, errorMessage, errorCode, 0);
}

///////////////////////////////////////////////////////////

// the ApplicationMgr sends this event to request that the UI display the report for a particular execution
void CTestExecDlg::OnDisplayReportApplicationMgr(LPDISPATCH exec) 
{
	ShowReport(exec);	
}

///////////////////////////////////////////////////////////

// the ApplicationMgr sends this event to request that the UI display a particular execution
void CTestExecDlg::OnDisplayExecutionApplicationMgr(LPDISPATCH exec, ExecutionDisplayReasons reason) 
{
	// bring application to front if we hit a breakpoint
	if (reason == TSUI::ExecutionDisplayReason_Breakpoint || reason == TSUI::ExecutionDisplayReason_BreakOnRunTimeError)
		this->SetForegroundWindow();

	// show this execution
	mExecutionViewMgr->PutRefExecution(ExecutionPtr(exec));

	// show the executions page in the list bar
	mListBar->PutCurrentPage(EXECUTIONS_PAGE_INDEX);
			
	// in case we are already showing the executions page, ensure we switch to steps or report tab as appropriate
	ShowAppropriateTabs(); 
}

///////////////////////////////////////////////////////////

// the ApplicationMgr sends this event to request that the UI display a particular sequence file
void CTestExecDlg::OnDisplaySequenceFileApplicationMgr(LPDISPATCH file, SequenceFileDisplayReasons reason) 
{
	// show this sequence file
	mSequenceFileViewMgr->PutRefSequenceFile(SequenceFilePtr(file));

	// show the sequence files page in the list bar
	mListBar->PutCurrentPage(SEQUENCE_FILES_PAGE_INDEX);
}

///////////////////////////////////////////////////////////

// the ApplicationMgr sends this event whenever an execution starts
void CTestExecDlg::OnStartExecutionApplicationMgr(LPDISPATCH exec, LPDISPATCH thrd, BOOL initiallyHidden) 
{
	// add a custom property to the execution to store which tab we are displaying for this execution. Initially show the execution tab
	ExecutionPtr(exec)->AsPropertyObject()->SetValNumber(_T("NIUI.LastActiveTab"), PropOption_InsertIfMissing, (double)TabPageIdentifier_Execution);			
}

///////////////////////////////////////////////////////////

// the ExecutionViewMgr sends this event whenever a new execution is selected
void CTestExecDlg::OnExecutionChangedExecutionViewMgr(LPDISPATCH exec) 
{
	// switch to report or steps tab depending on what the execution displayed last
	ShowAppropriateTabs();	
}

///////////////////////////////////////////////////////////

// the ListBar sends this event when the listbar switches to a new page
void CTestExecDlg::OnCurPageChangedListbar(long CurrentPage) 
{
	ShowAppropriateTabs();
	UpdateWindowTitle();
}

///////////////////////////////////////////////////////////

void CTestExecDlg::OnInitMenu(CMenu* pMenu) 
{	
	TS_MFC_TRY
		{
		// whenever a menu opens, rebuild the menubar to make sure it has the correct items and dimming for the current application state
		// if we wanted, we could just rebuild the menu that is actually opening, but our menu bar is small enough that we can rebuild it all
		RebuildMenuBar();  

		CDialog::OnInitMenu(pMenu);
		}	
	TS_MFC_CATCH_AND_DISPLAY	
}

///////////////////////////////////////////////////////////

// return the SequenceFileViewMgr or the ExecutionViewMgr depending on whether we are displaying sequence files or executions
IDispatchPtr CTestExecDlg::GetActiveViewManager(void)
{
	if (mListBar->GetCurrentPage() == SEQUENCE_FILES_PAGE_INDEX)	// sequence files are visible, sequence file menu commands apply
		return mSequenceFileViewMgr;
	else
	if (mListBar->CurrentPage == EXECUTIONS_PAGE_INDEX)				// executions are visible, execution menu commands apply
		return mExecutionViewMgr;	
	else
		return NULL;
}

void CTestExecDlg::RebuildMenuBar(void)
{
	IDispatchPtr	viewMgr = NULL;

	// determine which view manager menu commands apply to
	viewMgr = GetActiveViewManager();
	
	// remove any TSUTIL::MenuBuilder items so they can be reinserted as 
	// appropriate for the current state of the application
	mMenuBuilder.RemoveMenuCommands(false);

	// rebuild the File menu
	CommandKinds fileMenuCommands[] = 
		{
		CommandKind_DefaultFileMenu_Set,		// add all the usual commands in a File menu
		CommandKind_NotACommand					// list terminator
		};

	mMenuBuilder.InsertCommandsInMenu(fileMenuCommands, mMenuBuilder.GetMenuHandle(0), -1, true, viewMgr);

	// rebuild the Edit menu
	CommandKinds editMenuCommands[] = 
		{
		CommandKind_DefaultEditMenu_Set,		// add all the usual commands in an Edit menu
		CommandKind_NotACommand					// list terminator
		};

	mMenuBuilder.InsertCommandsInMenu(editMenuCommands, mMenuBuilder.GetMenuHandle(1), -1, true, viewMgr);

	// rebuild the Execute menu
	CommandKinds executeMenuCommands[] = 
		{
		CommandKind_DefaultExecuteMenu_Set,		// add all the usual commands in an Execute menu
		CommandKind_NotACommand					// list terminator
		};

	mMenuBuilder.InsertCommandsInMenu(executeMenuCommands, mMenuBuilder.GetMenuHandle(2), -1, true, viewMgr);

	// rebuild the Debug menu
	CommandKinds debugMenuCommands[] = 
		{
		CommandKind_DefaultDebugMenu_Set,		// add all the usual commands in a Debug menu
		CommandKind_NotACommand					// list terminator
		};

	mMenuBuilder.InsertCommandsInMenu(debugMenuCommands, mMenuBuilder.GetMenuHandle(3), -1, true, viewMgr);


	// rebuild the Configure menu
	CommandKinds configureMenuCommands[] = 
		{
		CommandKind_DefaultConfigureMenu_Set,	// add all the usual commands in a Configure menu
		CommandKind_NotACommand					// list terminator
		};


	mMenuBuilder.InsertCommandsInMenu(configureMenuCommands, mMenuBuilder.GetMenuHandle(4), -1, true, viewMgr);
	if(mApplicationMgr->IsEditor)
	{
		CommandKinds ConfigureMenuCommands_EditMode[] = 
		{
		CommandKind_ConfigureEngineEnvironment,	// add all the usual commands in a Configure menu
		CommandKind_NotACommand					// list terminator
		};

		mMenuBuilder.InsertCommandsInMenu(ConfigureMenuCommands_EditMode, mMenuBuilder.GetMenuHandle(4), -1, true, viewMgr);
	}

	// rebuild the Tools menu
	CommandKinds toolsMenuCommands[] = 
		{
		CommandKind_DefaultToolsMenu_Set,		// add all the usual commands in the Tools menu
		CommandKind_NotACommand					// list terminator
		};

	mMenuBuilder.InsertCommandsInMenu(toolsMenuCommands, mMenuBuilder.GetMenuHandle(5), -1, true, viewMgr);

	// rebuild the Help menu. Note that the help menu already contains an "About..." item, which is not a TestStand command item
	CommandKinds helpMenuCommands[] = 
		{
		CommandKind_Separator,					// separates the existing About... item
		CommandKind_DefaultHelpMenu_Set,		// add all the usual commands in a Help menu. Note that most help items appear only when in Edit mode.
		CommandKind_NotACommand					// list terminator
		};

	mMenuBuilder.InsertCommandsInMenu(helpMenuCommands, mMenuBuilder.GetMenuHandle(6), -1, true, viewMgr);

	// remove any duplicate shortcuts or separators
	mMenuBuilder.CleanupMenu();
}

///////////////////////////////////////////////////////////

BOOL CTestExecDlg::HandleAccelerators(MSG* pMsg)
{
	TS_MFC_TRY
		{	
		if (pMsg->message >= WM_KEYFIRST && pMsg->message <= WM_KEYLAST)
			{
			// check if the key is an accelerator for a menu item inserted by the TSUTIL::MenuBuilder class
			if (mMenuBuilder.TranslateAccelerator(pMsg))
				return TRUE;
			
			HWND currentFocussedWindow = ::GetFocus();
			//The focus might be gone if a file or execution or a window having the focus was closed.
			//Setting the focus back to one of the child windows to get the menu accelerators working.
			if(!currentFocussedWindow || currentFocussedWindow == this->m_hWnd)			  
				this->mListBarCWnd.SetFocus();
			// check for regular MFC accelerators:

			// Normally, the CFrameWnd class would automatically process the accelerator table.
			// Since this example doesn't use the CFrameWnd class, the accelerators are checked.
			if (mAccelTable && ::TranslateAccelerator(m_hWnd, mAccelTable, pMsg))
				return TRUE;
			}
		}
	TS_MFC_CATCH_AND_DISPLAY	
	return FALSE;
}

///////////////////////////////////////////////////////////

// handle menu shortcut keys
BOOL CTestExecDlg::PreTranslateMessage(MSG* pMsg) 
{
	if (HandleAccelerators(pMsg))
		return TRUE;
	else
		return CDialog::PreTranslateMessage(pMsg);
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

void CTestExecDlg::OnHelpAbout() 
{
	CAboutBox	aboutBox(TSUTIL::Localizer(mApplicationMgr->GetEngine()));

	aboutBox.DoModal();	
}

///////////////////////////////////////////////////////////

// We are a mode-less dialog app, don't perform default modal dialog OnOk handling or we will exit when the user hits the Enter key
void CTestExecDlg::OnOK() 
{
}

///////////////////////////////////////////////////////////

// We are a mode-less dialog app, don't perform default modal dialog OnCancel handling or we will exit when the user hits the ESC key
void CTestExecDlg::OnCancel() 
{
}

///////////////////////////////////////////////////////////

// build a context menu for a control that has been right-clicked
void CTestExecDlg::BuildCommandSetMenu(TSUI::CommandKinds commandSet, long menuHandle)
{
	TSUI::CommandsPtr	cmds = this->mApplicationMgr->NewCommands();
	long				unused = 0;

	// insert items for default sequence view context menu in the context menu
	cmds->InsertKind(commandSet, GetActiveViewManager(), -1, _T(""), _T(""), &unused);
	cmds->InsertIntoWin32Menu(menuHandle, -1, VARIANT_TRUE, VARIANT_TRUE);  // we are using the context menu that the control provides because it requires fewer lines of code. We could have displayed a MFC context menu in response to an ActiveX right-mouse-click event instead. 
}

///////////////////////////////////////////////////////////

// the user right clicked on the list bar, create a context menu to display
void CTestExecDlg::OnCreateContextMenuListbar(long menuHandle, long x, long y) 
{
	BuildCommandSetMenu(CommandKind_DefaultListBarContextMenu_Set, menuHandle);
}

///////////////////////////////////////////////////////////

// window size changed
void CTestExecDlg::OnSize(UINT nType, int cx, int cy) 
{
	CDialog::OnSize(nType, cx, cy);	

	if (this->IsWindowVisible())	// ignore size events that occur before the window is fully constructed and displayed
		ArrangeControls();
}

///////////////////////////////////////////////////////////

// adjust controls to fit within current window size
void CTestExecDlg::ArrangeControls()
{
	if (IsIconic())	// don't shrink controls and inadvertently close subpanes when the window is minimized
		return;

    const int buttonVerticalMargin = 6;
    const int buttonHorizontalSpacing = 12;

	// combine ActiveX interfaces and window handles into Control objects for passing to Splitters::DivideSpaceBetweenPanes and for more convenient resizing
	Control	tabControl(m_tabCtrl, NULL);
	Control	listBar(mListBarCWnd, mListBar);

	// resize controls to fit current window height
	CRect	clientRect;
	this->GetClientRect(&clientRect);
	listBar.SetHeight(clientRect.Height() - DialogHelper::Height(mStatusBarCWnd) - DialogHelper::Height(mMenuDividerBar));
	tabControl.SetHeight(listBar.Height());

    // there is a vertical drag bar between listbar and tab control
	Splitters::DivideSpaceBetweenPanes(true, 0, clientRect.Width(), Controls(listBar, tabControl));

	// obtain new size of tab area interior
	CRect	tabInteriorRect = tabControl.Rect(FALSE);
	m_tabCtrl.AdjustRect(FALSE, &tabInteriorRect);
	ScreenToClient(&tabInteriorRect);

	// set tab page sizes to fit interior area
	DialogHelper::SetRect(mFileTab, tabInteriorRect);
	DialogHelper::SetRect(mExecutionTab, tabInteriorRect);
	DialogHelper::SetRect(mReportTab, tabInteriorRect);

	// make menu divider bar stretch across entire window
	DialogHelper::SetWidth(mMenuDividerBar, DialogHelper::Width(*this));

    // File and Execution Tabs have buttons at the bottom. figure out how much space is left after the buttons are placed
	int buttonHeight = DialogHelper::Height(mExecutionTab.mBreakResumeBtnCWnd);
	int buttonTop = max(tabInteriorRect.Height() - buttonHeight - buttonVerticalMargin, 40 + buttonVerticalMargin);

	// compute the size of the tab control interior area minus space for the buttons. this area is shared by controls that be resized
    // don't shrink control area to less than 40 by 40 pixels so that if the window is resized to have no visible area, the controls aren't made so small that we forget their relative sizes
	int resizableTabControlAreaHeight = max(buttonTop - buttonVerticalMargin, 40);	
	int	resizableTabControlAreaWidth = max(tabInteriorRect.Width(), 40);				

    // FILE TAB:

	// combine ActiveX interfaces and window handles into Control objects for passing to Splitters::DivideSpaceBetweenPanes and for more convenient resizing
	Control	fileSteps(mFileTab.mFileStepListCWnd, mFileTab.mFileStepList);
	Control	sequencesList(mFileTab.mSequencesListCWnd, mFileTab.mSequencesList);
	Control insertionPalette(mFileTab.mInsertionPaletteCWnd, mFileTab.mInsertionPalette);
	Control fileVariables(mFileTab.mFileVariablesCWnd, mFileTab.mFileVariables);

    // place the file tab buttons
	DialogHelper::SetTop(mFileTab.mEntryPoint1BtnCWnd, buttonTop);
	DialogHelper::SetTop(mFileTab.mEntryPoint2BtnCWnd, buttonTop);
	DialogHelper::SetTop(mFileTab.mRunSequenceBtnCWnd, buttonTop);
	DialogHelper::SetLeft(mFileTab.mEntryPoint2BtnCWnd, DialogHelper::Right(mFileTab.mEntryPoint1BtnCWnd)+ buttonHorizontalSpacing);
	DialogHelper::SetLeft(mFileTab.mRunSequenceBtnCWnd, DialogHelper::Right(mFileTab.mEntryPoint2BtnCWnd) + buttonHorizontalSpacing);

    // hide editor-only controls if not an editor
	VARIANT_BOOL isEditor = mApplicationMgr->IsEditor;
	mFileTab.mInsertionPaletteCWnd.ShowWindow(isEditor ? SW_SHOWNOACTIVATE : SW_HIDE);
   
    // there is a horizontal drag bar between FileSteps and FileVariables/SequenceList
	Splitters::DivideSpaceBetweenPanes(false, 0, resizableTabControlAreaHeight, Controls(fileSteps, sequencesList));

	// file variables has same height and top as the sequences list
	fileVariables.SetTop(sequencesList.Top());
	fileVariables.SetHeight(sequencesList.Height());

    // there is a vertical drag bar between the FileSteps and the Insertion Palette
    if (!isEditor)
		fileSteps.SetWidth(resizableTabControlAreaWidth); // Insertion Palette is not visible, nothing to split
    else
		Splitters::DivideSpaceBetweenPanes(true, 0, resizableTabControlAreaWidth, Controls(fileSteps, insertionPalette));

	insertionPalette.SetHeight(resizableTabControlAreaHeight);

    // there is a vertical drag bar between the FileVariables and the SequencesList
	Splitters::DivideSpaceBetweenPanes(true, 0, fileSteps.Width(), Controls(fileVariables, sequencesList));

    // EXECUTION TAB:

    // place the execution tab buttons
	DialogHelper::SetTop(mExecutionTab.mBreakResumeBtnCWnd, buttonTop);
	DialogHelper::SetTop(mExecutionTab.mTerminateRestartBtnCWnd, buttonTop);
	DialogHelper::SetLeft(mExecutionTab.mTerminateRestartBtnCWnd, DialogHelper::Right(mExecutionTab.mBreakResumeBtnCWnd) + buttonHorizontalSpacing);

	// combine ActiveX interfaces and window handles into Control objects for passing to Splitters::DivideSpaceBetweenPanes and for more convenient resizing
	Control	executionSteps(mExecutionTab.mExecutionStepListCWnd, mExecutionTab.mExecutionStepList);
	Control	callStack(mExecutionTab.mCallStackCWnd, mExecutionTab.mCallStack);
	Control	threads(mExecutionTab.mThreadsCWnd, mExecutionTab.mThreads);
	Control	executionVariables(mExecutionTab.mExecutionVariablesCWnd, mExecutionTab.mExecutionVariables);

	// size ExecutionSteps to tab control interior width
	DialogHelper::SetWidth(mExecutionTab.mExecutionStepListCWnd, resizableTabControlAreaWidth);

    // there is a horizontal drag bar between ExecutionSteps and CallStack/Threads/ExecutionVariables
	Splitters::DivideSpaceBetweenPanes(false, 0, resizableTabControlAreaHeight, Controls(executionSteps, callStack));

	// execution variables and threads have the same height and top as the callstack
    executionVariables.SetHeight(callStack.Height());
	threads.SetHeight(callStack.Height());
    executionVariables.SetTop(callStack.Top());
	threads.SetTop(callStack.Top());

    // there are vertical drag bars between the ExecutionVariables, CallStack, and Threads
	Splitters::DivideSpaceBetweenPanes(true, 0, executionSteps.Width(), Controls(executionVariables, callStack, threads));

    // REPORT TAB:
	CRect	reportBounds;
	mReportTab.GetClientRect(&reportBounds);
	DialogHelper::SetRect(mReportTab.mReportViewCWnd, reportBounds);

	UpdateWindow();

	// for some reason, MFC does not update the clipped rect of the control when the dialog is resized and it needs to be explicitly reset
	DialogHelper::CorrectControlClipping(mFileTab.mEntryPoint1BtnCWnd);
	DialogHelper::CorrectControlClipping(mFileTab.mEntryPoint2BtnCWnd);
	DialogHelper::CorrectControlClipping(mFileTab.mRunSequenceBtnCWnd);
	DialogHelper::CorrectControlClipping(mFileTab.mSequencesListCWnd);
	DialogHelper::CorrectControlClipping(mFileTab.mFileVariablesCWnd);
	DialogHelper::CorrectControlClipping(mFileTab.mEntryPoint1BtnCWnd);
	DialogHelper::CorrectControlClipping(mFileTab.mInsertionPaletteCWnd);
	DialogHelper::CorrectControlClipping(mExecutionTab.mCallStackCWnd);
	DialogHelper::CorrectControlClipping(mExecutionTab.mThreadsCWnd);
	DialogHelper::CorrectControlClipping(mExecutionTab.mExecutionVariablesCWnd);
	DialogHelper::CorrectControlClipping(mStatusBarCWnd);
	DialogHelper::CorrectControlClipping(mListBarCWnd);
}

///////////////////////////////////////////////////////////

// splitter bar moved
void CTestExecDlg::OnBorderDraggedListbar(long bordersChanged, long newX, long newY, long newWidth, long newHeight, BOOL finalResize) 
{
	Splitters::DragSplitter(Control(mListBarCWnd, mListBar), Control(m_tabCtrl, NULL), newX, newY, newWidth, newHeight, bordersChanged);
	ArrangeControls();		
}

///////////////////////////////////////////////////////////

// append the caption for the selected file or execution to the application window title
void CTestExecDlg::UpdateWindowTitle()
{
	_variant_t	unused;
	_bstr_t title = mApplicationMgr->GetEngine()->GetResourceString(_T("TSUI_OI_MAIN_PANEL"), _T("TESTSTAND_USER_INTERFACE"), _T(""), &unused);
	_bstr_t	documentDescription;

	if (mListBar->GetCurrentPage() == SEQUENCE_FILES_PAGE_INDEX)	// sequence files are visible
		documentDescription += mSequenceFileViewMgr->GetCaptionText(CaptionSource_CurrentSequenceFile, VARIANT_FALSE, _T(""));
	else	// executions are visible
		documentDescription += mExecutionViewMgr->GetCaptionText(CaptionSource_CurrentExecution, VARIANT_FALSE, _T(""));
	
	if (documentDescription.length() > 0)
		title += _T(" - ") + documentDescription;

	SetWindowText(title);
}

///////////////////////////////////////////////////////////

// user toggled edit mode. the only way to do that in this application is to type ctrl-shift-alt-insert, which is the edit mode toggle key this application specifies in designer for the ApplicationMgr control. 
// to prevent edit mode from being toggled with a hotkey, set ApplicationMgr.EditModeShortcutKey to ShortcutKey_VK_NOT_A_KEY
void CTestExecDlg::EditModeChangedApplicationMgr()
{	
	DialogHelper::SetWidth(mFileTab.mInsertionPaletteCWnd, max(DialogHelper::Width(mFileTab.mInsertionPaletteCWnd), 260));	// make sure the palette is wide enough in case it is going to be shown
	ArrangeControls();	// relayout the controls to reflect the change in edit mode
}

///////////////////////////////////////////////////////////

void CTestExecDlg::PostCommandExecuteApplicationMgr(LPDISPATCH Command)
{
	if (CommandPtr(Command)->GetKind() == CommandKind_CloseCompletedExecutions)
	{
		// if we closed all the executions, switch to the files page instead of showing an empty executions page
		if (this->mApplicationMgr->Executions->Count == 0)
			mListBar->PutCurrentPage(SEQUENCE_FILES_PAGE_INDEX);
	}
}
