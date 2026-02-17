#if !defined(AFX_ABOUTBOX_H__2B72971B_F96E_4FBD_AFFA_6CC329B02DDE__INCLUDED_)
#define AFX_ABOUTBOX_H__2B72971B_F96E_4FBD_AFFA_6CC329B02DDE__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

// TestStand API, TestStand UI ActiveX controls API, and utilities for using these APIs in C++
// The file is located in <TestStand>\API\VC
#include "tsutilCPP.h"	

// AboutBox.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CAboutBox dialog

class CAboutBox : public CDialog
{
// Construction
public:
	CAboutBox(TSUTIL::Localizer localizer, CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CAboutBox)
	enum { IDD = IDD_ABOUTBOX };
	CStatic	m_engineVersionStatic;
	CStatic	m_versionStatic;
	CStatic m_licenseStatic;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CAboutBox)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CAboutBox)
	virtual BOOL OnInitDialog();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
private:
	TSUTIL::Localizer	m_localizer;
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_ABOUTBOX_H__2B72971B_F96E_4FBD_AFFA_6CC329B02DDE__INCLUDED_)
