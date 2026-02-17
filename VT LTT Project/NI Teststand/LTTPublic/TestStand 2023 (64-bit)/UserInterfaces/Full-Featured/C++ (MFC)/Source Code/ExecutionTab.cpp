// ExecutionTab.cpp : implementation file
//

#include "stdafx.h"
#include "TestExec.h"
#include "ExecutionTab.h"
#include "TestExecDlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CExecutionTab dialog


CExecutionTab::CExecutionTab(CWnd* pParent /*=NULL*/)
	: CDialog(CExecutionTab::IDD, pParent)
{
	//{{AFX_DATA_INIT(CExecutionTab)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
}


void CExecutionTab::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CExecutionTab)
	// NOTE: the ClassWizard will add DDX and DDV calls here
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CExecutionTab, CDialog)
	//{{AFX_MSG_MAP(CExecutionTab)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CExecutionTab message handlers

BOOL CExecutionTab::OnInitDialog() 
{
	CDialog::OnInitDialog();
	
	// initialize CWnds for non-windowless activeX controls (refer to TestStand Reference Manual >> Chapter - Creating Custom Operator Interfaces >> Using TestStand UI Controls in Different Environments >> Visual C++)
	mThreadsCWnd.Attach(GetDlgItem(IDC_EXECTAB_THREAD_LIST)->m_hWnd);
	mCallStackCWnd.Attach(GetDlgItem(IDC_EXECTAB_CALLSTACK_LIST)->m_hWnd);
	mExecutionStepListCWnd.Attach(GetDlgItem(IDC_EXECTAB_STEPLIST)->m_hWnd);
	mBreakResumeBtnCWnd.Attach(GetDlgItem(IDC_EXECTAB_BREAKRESUME_BTN)->m_hWnd);
	mTerminateRestartBtnCWnd.Attach(GetDlgItem(IDC_EXECTAB_TERMINATERESTART_BTN)->m_hWnd);
	mExecutionVariablesCWnd.Attach(GetDlgItem(IDC_EXECTAB_VARIABLES)->m_hWnd);
	mExecutionLabelCWnd.Attach(GetDlgItem(IDC_EXECTAB_EXECUTIONLABEL)->m_hWnd);

	// initialize activeX control smart pointers for TestStand UI controls
	mThreads =					mThreadsCWnd.GetControlUnknown();
	mCallStack =				mCallStackCWnd.GetControlUnknown();
	mExecutionStepList =		mExecutionStepListCWnd.GetControlUnknown();
	mBreakResumeBtn =			mBreakResumeBtnCWnd.GetControlUnknown();
	mTerminateRestartBtn =		mTerminateRestartBtnCWnd.GetControlUnknown();
	mExecutionVariables =		mExecutionVariablesCWnd.GetControlUnknown();
	mExecutionLabel =			mExecutionLabelCWnd.GetControlUnknown();

	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}

BEGIN_EVENTSINK_MAP(CExecutionTab, CDialog)
    //{{AFX_EVENTSINK_MAP(CExecutionTab)
	ON_EVENT(CExecutionTab, IDC_EXECTAB_SEQUENCE_VIEW, 4, OnCreateContextMenuSequenceView, VTS_I4 VTS_I4 VTS_I4)
	ON_EVENT(CExecutionTab, IDC_EXECTAB_SEQUENCE_VIEW, 5, CExecutionTab::OnBorderDraggedExecutionStepList, VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_BOOL)
	ON_EVENT(CExecutionTab, IDC_EXECTAB_VARIABLES, 1, CExecutionTab::BorderDraggedVariables, VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_BOOL)
	ON_EVENT(CExecutionTab, IDC_EXECTAB_CALLSTACK_LIST, 1, CExecutionTab::OnBorderDraggedCallstackList, VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_BOOL)
	ON_EVENT(CExecutionTab, IDC_EXECTAB_EXECUTIONLABEL, 2, CExecutionTab::OnConnectionActivityExecutionLabel, VTS_I4)
	//}}AFX_EVENTSINK_MAP
END_EVENTSINK_MAP()

//////////////////////////////////////////////////////

// call the parent dialog so that menu accelerators will work and so you use the tab key to navigate out of this tab page if it contains an activeX control
BOOL CExecutionTab::PreTranslateMessage(MSG* pMsg) 
{
	if (pMsg->message == WM_KEYDOWN && pMsg->wParam == VK_TAB)
		return ParentDialog()->PreTranslateMessage(pMsg);	// let the parent handle tabs
	// everything else...
	else if (ParentDialog()->HandleAccelerators(pMsg))
		return TRUE;
	return CDialog::PreTranslateMessage(pMsg);
}

//////////////////////////////////////////////////////

// create a right-click menu for the SequenceView control that displays execution steps
void CExecutionTab::OnCreateContextMenuSequenceView(long menuHandle, long x, long y) 
{
	ParentDialog()->BuildCommandSetMenu(TSUI::CommandKind_DefaultSequenceViewContextMenu_Set, menuHandle);
}

//////////////////////////////////////////////////////

// user dragged the horizontal bar on the execution steps lists that separates it from the call stack, thread list, and execution variables
void CExecutionTab::OnBorderDraggedExecutionStepList(long bordersChanged, long newX, long newY, long newWidth, long newHeight, BOOL finalResize)
{
	Splitters::DragSplitter(Control(mExecutionStepListCWnd, mExecutionStepList), 
							Control(mCallStackCWnd, mCallStack),
							newX, newY, newWidth, newHeight, bordersChanged);
	ParentDialog()->ArrangeControls();		
}

//////////////////////////////////////////////////////

// user dragged the vertical bar on the execution variables that separates it from the callstack
void CExecutionTab::BorderDraggedVariables(long bordersChanged, long newX, long newY, long newWidth, long newHeight, BOOL finalResize)
{
	Splitters::DragSplitter(Control(mExecutionVariablesCWnd, mExecutionVariables),
							Control(mCallStackCWnd, mCallStack),
							newX, newY, newWidth, newHeight, bordersChanged);
	ParentDialog()->ArrangeControls();		
}

//////////////////////////////////////////////////////

// user dragged the vertical bar on the callstack that separates it from the thread list
void CExecutionTab::OnBorderDraggedCallstackList(long bordersChanged, long newX, long newY, long newWidth, long newHeight, BOOL finalResize)
{
	Splitters::DragSplitter(Control(mCallStackCWnd, mCallStack), 
							Control(mThreadsCWnd, mThreads),
							newX, newY, newWidth, newHeight, bordersChanged);
	ParentDialog()->ArrangeControls();		
}

//////////////////////////////////////////////////////

// a hidden label control is connected to CaptionSource_CurrentExecution so we can get this event when that caption changes and thus update the title bar in case the title bar is showing the current execution
void CExecutionTab::OnConnectionActivityExecutionLabel(long activity)
{
	ParentDialog()->UpdateWindowTitle();
}
