// AboutBox.cpp : implementation file
//

#include "stdafx.h"
#include "TestExec.h"
#include "AboutBox.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CAboutBox dialog


CAboutBox::CAboutBox(TSUTIL::Localizer localizer, CWnd* pParent /*=NULL*/) :
	CDialog(CAboutBox::IDD, pParent),
	m_localizer(localizer)
{
	//{{AFX_DATA_INIT(CAboutBox)
	//}}AFX_DATA_INIT
}


void CAboutBox::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CAboutBox)
	DDX_Control(pDX, IDC_ENGINE_VERSION_STATIC, m_engineVersionStatic);
	DDX_Control(pDX, IDC_VERSION_STATIC, m_versionStatic);
	DDX_Control(pDX, IDC_LICENSE_STATIC, m_licenseStatic);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CAboutBox, CDialog)
	//{{AFX_MSG_MAP(CAboutBox)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CAboutBox message handlers

BOOL CAboutBox::OnInitDialog() 
{
	CString	label;

	CDialog::OnInitDialog();
	
	// localize controls

	// for strings that are different when we are an editor
	if (m_localizer.GetEngine()->ApplicationIsEditor) // check engine instead of applicationMgr because we have a convenient engine reference
		m_localizer.LocalizeWindow(this->m_hWnd, _T("TSUI_OI_EDITOR_ABOUT_BOX"), false);

	// for strings that are the same regardless of editor mode
	m_localizer.LocalizeWindow(this->m_hWnd, _T("TSUI_OI_ABOUT_BOX"), false);

		
	// add the version strings
	m_versionStatic.GetWindowText(label);
	label += _T(" ");
	label += _T("<YEAR> (<MAJOR>.<MINOR>.<PATCH>.<BUILD>)"); // <--- YOUR VERSION HERE.  (This is the version displayed in the about box.)
	m_versionStatic.SetWindowText(label);

	m_engineVersionStatic.GetWindowText(label);
	label += _T(" ");
	label += (const TCHAR *)m_localizer.GetEngine()->GetVersionString();
	m_engineVersionStatic.SetWindowText(label);

	// add license description
	m_licenseStatic.GetWindowText(label);
	label += (const TCHAR *)m_localizer.GetEngine()->GetLicenseDescription(0);
	m_licenseStatic.SetWindowText(label);

	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}
