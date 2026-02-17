#if !defined(AFX_SPLASHSCREEN_H__A5870398_9939_4075_9027_B4111599A7CF__INCLUDED_)
#define AFX_SPLASHSCREEN_H__A5870398_9939_4075_9027_B4111599A7CF__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// SplashScreen.h : header file
//

// TestStand API, TestStand UI ActiveX controls API, and utilities for using these APIs in C++
// The file is located in <TestStand>\API\VC
#include "tsutilCPP.h"	

// using allows us to not type the namespace qualifier in front of each api call
using namespace TS;
using namespace TSUI;

/////////////////////////////////////////////////////////////////////////////
// CSplashScreen dialog

class CSplashScreen : public CDialog
{
// Construction
public:
	CSplashScreen(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CSplashScreen)
	enum { IDD = IDD_SPLASHSCREEN };
	CStatic	m_splashBitmap;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CSplashScreen)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CSplashScreen)
	virtual BOOL OnInitDialog();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_SPLASHSCREEN_H__A5870398_9939_4075_9027_B4111599A7CF__INCLUDED_)
