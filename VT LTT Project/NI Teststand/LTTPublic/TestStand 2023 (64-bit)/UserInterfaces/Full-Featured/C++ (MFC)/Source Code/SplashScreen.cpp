// SplashScreen.cpp : implementation file
//

#include "stdafx.h"
#include "TestExec.h"
#include "SplashScreen.h"

using TSUTIL::DialogHelper;

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CSplashScreen dialog


CSplashScreen::CSplashScreen(CWnd* pParent /*=NULL*/)
	: CDialog(CSplashScreen::IDD, pParent)
{
	//{{AFX_DATA_INIT(CSplashScreen)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
}


void CSplashScreen::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CSplashScreen)
	DDX_Control(pDX, IDC_SPLASHBITMAP, m_splashBitmap);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CSplashScreen, CDialog)
	//{{AFX_MSG_MAP(CSplashScreen)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CSplashScreen message handlers

BOOL CSplashScreen::OnInitDialog() 
{
	CDialog::OnInitDialog();
	
	// When the video driver large fonts option is set, it changes the mapping of pixels to dialog units and the splash screen dialog no longer matches the size
	// of the bitmap it displays.  The following code resets the splash screen size to the size of the bitmap while keeping it centered.
	CRect	bitmapRect = DialogHelper::RectOf(m_splashBitmap);
	CRect	windowRect = DialogHelper::RectOf(this->m_hWnd, FALSE);
	POINT	offset;
	offset.x = (windowRect.Width() - bitmapRect.Width()) / 2;
	offset.y = (windowRect.Height() - bitmapRect.Height()) / 2;

	DialogHelper::SetPos(this->m_hWnd, windowRect.left + offset.x, windowRect.top + offset.y, bitmapRect.Width(), bitmapRect.Height());
			
	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}
