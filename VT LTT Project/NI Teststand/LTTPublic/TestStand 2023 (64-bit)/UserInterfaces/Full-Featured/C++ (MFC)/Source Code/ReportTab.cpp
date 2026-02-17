// ReportTab.cpp : implementation file
//

#include "stdafx.h"
#include "TestExec.h"
#include "ReportTab.h"
#include "TestExecDlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CReportTab dialog


CReportTab::CReportTab(CWnd* pParent /*=NULL*/)
	: CDialog(CReportTab::IDD, pParent)
{
	//{{AFX_DATA_INIT(CReportTab)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
}


void CReportTab::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CReportTab)
		// NOTE: the ClassWizard will add DDX and DDV calls here
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CReportTab, CDialog)
	//{{AFX_MSG_MAP(CReportTab)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CReportTab message handlers

//////////////////////////////////////////////////////

BOOL CReportTab::OnInitDialog() 
{
	CDialog::OnInitDialog();

	// initialize CWnds for non-windowless activeX controls (refer to TestStand Reference Manual >> Chapter - Creating Custom Operator Interfaces >> Using TestStand UI Controls in Different Environments >> Visual C++)
	mReportViewCWnd.Attach(GetDlgItem(IDC_REPORTTAB_REPORT_VIEW)->m_hWnd);
	
	// initialize activeX control smart pointers for TestStand UI controls
	mReportView =	mReportViewCWnd.GetControlUnknown();
	
	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}

//////////////////////////////////////////////////////

// call the parent dialog so that menu accelerators will work and so you use the tab key to navigate out of this tab page if it contains an activeX control
BOOL CReportTab::PreTranslateMessage(MSG* pMsg) 
{
	if (pMsg->message == WM_KEYDOWN && pMsg->wParam == VK_TAB)
		return ParentDialog()->PreTranslateMessage(pMsg);	// let the parent handle tabs
	// everything else...
	else if (ParentDialog()->HandleAccelerators(pMsg))
		return TRUE;
	return CDialog::PreTranslateMessage(pMsg);

}