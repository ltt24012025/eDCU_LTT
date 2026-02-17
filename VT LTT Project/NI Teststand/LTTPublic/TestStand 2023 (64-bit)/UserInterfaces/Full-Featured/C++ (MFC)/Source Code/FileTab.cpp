// FileTab.cpp : implementation file
//

#include "stdafx.h"
#include "TestExec.h"
#include "FileTab.h"
#include "TestExecDlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CFileTab dialog


CFileTab::CFileTab(CWnd* pParent /*=NULL*/)
	: CDialog(CFileTab::IDD, pParent)
{
	//{{AFX_DATA_INIT(CFileTab)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
}


void CFileTab::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CFileTab)
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CFileTab, CDialog)
	//{{AFX_MSG_MAP(CFileTab)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CFileTab message handlers

BOOL CFileTab::OnInitDialog() 
{
	CDialog::OnInitDialog();
	
	// initialize CWnds for non-windowless activeX controls (refer to TestStand Reference Manual >> Chapter - Creating Custom Operator Interfaces >> Using TestStand UI Controls in Different Environments >> Visual C++)
	mSequencesListCWnd.Attach(GetDlgItem(IDC_FILETAB_SEQUENCES_LIST)->m_hWnd);
	mEntryPoint1BtnCWnd.Attach(GetDlgItem(IDC_FILETAB_ENTRYPOINT1_BTN)->m_hWnd);
	mEntryPoint2BtnCWnd.Attach(GetDlgItem(IDC_FILETAB_ENTRYPOINT2_BTN)->m_hWnd);
	mRunSequenceBtnCWnd.Attach(GetDlgItem(IDC_FILETAB_RUN_SEQUENCE_BTN)->m_hWnd);
	mFileStepListCWnd.Attach(GetDlgItem(IDC_FILETAB_STEPLIST)->m_hWnd);
	mFileVariablesCWnd.Attach(GetDlgItem(IDC_FILETAB_VARIABLES)->m_hWnd);
	mInsertionPaletteCWnd.Attach(GetDlgItem(IDC_FILETAB_INSERTIONPALETTE)->m_hWnd);
	mSequenceFileLabelCWnd.Attach(GetDlgItem(IDC_FILETAB_SEQUENCEFILELABEL)->m_hWnd);
	
	// initialize activeX control smart pointers for TestStand UI controls
	mSequencesList =			mSequencesListCWnd.GetControlUnknown();
	mEntryPoint1Btn =			mEntryPoint1BtnCWnd.GetControlUnknown();
	mEntryPoint2Btn =			mEntryPoint2BtnCWnd.GetControlUnknown();
	mRunSequenceBtn =			mRunSequenceBtnCWnd.GetControlUnknown();
	mFileStepList =				mFileStepListCWnd.GetControlUnknown();
	mFileVariables =			mFileVariablesCWnd.GetControlUnknown();
	mInsertionPalette =			mInsertionPaletteCWnd.GetControlUnknown();
	mSequenceFileLabel =		mSequenceFileLabelCWnd.GetControlUnknown();

	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}

BEGIN_EVENTSINK_MAP(CFileTab, CDialog)
    //{{AFX_EVENTSINK_MAP(CFileTab)
	ON_EVENT(CFileTab, IDC_FILETAB_SEQUENCE_VIEW, 4, OnCreateContextMenuFileStepList, VTS_I4 VTS_I4 VTS_I4)
	ON_EVENT(CFileTab, IDC_FILETAB_SEQUENCE_VIEW, 5, CFileTab::OnBorderDraggedFileStepList, VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_BOOL)
	ON_EVENT(CFileTab, IDC_FILETAB_VARIABLES, 1, CFileTab::OnBorderDraggedFileVariables, VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_BOOL)
	ON_EVENT(CFileTab, IDC_FILETAB_SEQUENCES_LIST, 2, CFileTab::OnCreateContextMenuSequencesList, VTS_I4 VTS_I4 VTS_I4)
	ON_EVENT(CFileTab, IDC_FILETAB_INSERTIONPALETTE, 1, CFileTab::OnBorderDraggedInsertionPalette, VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_BOOL)
	ON_EVENT(CFileTab, IDC_FILETAB_SEQUENCEFILELABEL, 2, CFileTab::OnConnectionActivitySequenceFileLabel, VTS_I4)
	//}}AFX_EVENTSINK_MAP
END_EVENTSINK_MAP()

//////////////////////////////////////////////////////

BOOL CFileTab::PreTranslateMessage(MSG* pMsg) 
{	
	if (pMsg->message == WM_KEYDOWN && pMsg->wParam == VK_TAB)
		return ParentDialog()->PreTranslateMessage(pMsg);	// let the parent handle tabs
	// everything else...
	else if (ParentDialog()->HandleAccelerators(pMsg))
		return TRUE;
	return CDialog::PreTranslateMessage(pMsg);
}

//////////////////////////////////////////////////////

// create a right-click menu for the SequenceView control that displays sequence file steps
void CFileTab::OnCreateContextMenuFileStepList(long menuHandle, long x, long y) 
{
	ParentDialog()->BuildCommandSetMenu(TSUI::CommandKind_DefaultSequenceViewContextMenu_Set, menuHandle);
}

//////////////////////////////////////////////////////

// create a right-click menu for the list control that displays the list of sequence files
void CFileTab::OnCreateContextMenuSequencesList(long menuHandle, long x, long y)
{
	ParentDialog()->BuildCommandSetMenu(TSUI::CommandKind_DefaultSequenceListContextMenu_Set, menuHandle);
}

//////////////////////////////////////////////////////

// user dragged horizontal bar that separates the step list from the sequence list and file variables
void CFileTab::OnBorderDraggedFileStepList(long bordersChanged, long newX, long newY, long newWidth, long newHeight, BOOL finalResize)
{
	Splitters::DragSplitter(Control(mFileStepListCWnd, mFileStepList), 
							Control(mSequencesListCWnd, mSequencesList),
							newX, newY, newWidth, newHeight, bordersChanged);
	ParentDialog()->ArrangeControls();		
}

//////////////////////////////////////////////////////

// user dragged vertical bar on the file variables that separates it from the sequences list 
void CFileTab::OnBorderDraggedFileVariables(long bordersChanged, long newX, long newY, long newWidth, long newHeight, BOOL finalResize)
{
	Splitters::DragSplitter(Control(mFileVariablesCWnd, mFileVariables),
							Control(mSequencesListCWnd, mSequencesList), 
							newX, newY, newWidth, newHeight, bordersChanged);
	ParentDialog()->ArrangeControls();		
}

//////////////////////////////////////////////////////

// user dragged the vertical bar on the insertion palette that separates it from the file step list
void CFileTab::OnBorderDraggedInsertionPalette(long bordersChanged, long newX, long newY, long newWidth, long newHeight, BOOL finalResize)
{
	Splitters::DragSplitter(Control(mInsertionPaletteCWnd, mInsertionPalette), 
							Control(mFileStepListCWnd, mFileStepList),
							newX, newY, newWidth, newHeight, bordersChanged);
	ParentDialog()->ArrangeControls();		
}

//////////////////////////////////////////////////////

// a hidden label control is connected to CaptionSource_CurrentSequenceFile so we can get this event when that caption changes and thus update the title bar in case the title bar is showing the current file
void CFileTab::OnConnectionActivitySequenceFileLabel(long activity)
{
	ParentDialog()->UpdateWindowTitle();
}

