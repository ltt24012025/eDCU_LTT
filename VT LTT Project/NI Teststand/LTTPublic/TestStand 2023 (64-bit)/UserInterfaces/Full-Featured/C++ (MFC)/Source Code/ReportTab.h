#if !defined(AFX_REPORTTAB_H__F6D38F07_9078_45B1_B21A_78FD136E24C2__INCLUDED_)
#define AFX_REPORTTAB_H__F6D38F07_9078_45B1_B21A_78FD136E24C2__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

// TestStand API, TestStand UI ActiveX controls API, and utilities for using these APIs in C++
// The file is located in <TestStand>\API\VC
#include "tsutilCPP.h"	

class CTestExecDlg;	// forward declaration

/////////////////////////////////////////////////////////////////////////////
// CReportTab dialog

class CReportTab : public CDialog
{
// Construction
public:
	CReportTab(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CReportTab)
	enum { IDD = IDD_REPORT_TAB_DIALOG };
		// NOTE: the ClassWizard will add data members here
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CReportTab)
	public:
	virtual BOOL PreTranslateMessage(MSG* pMsg);
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CReportTab)
	virtual BOOL OnInitDialog();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

	CTestExecDlg * ParentDialog() {return (CTestExecDlg *)this->GetParent();}

public:
		// ActiveX control smart pointers (#import style)
		TSUI::IReportViewPtr			mReportView;

		// CWnds for each ActiveX control
		CWnd							mReportViewCWnd;
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_REPORTTAB_H__F6D38F07_9078_45B1_B21A_78FD136E24C2__INCLUDED_)
