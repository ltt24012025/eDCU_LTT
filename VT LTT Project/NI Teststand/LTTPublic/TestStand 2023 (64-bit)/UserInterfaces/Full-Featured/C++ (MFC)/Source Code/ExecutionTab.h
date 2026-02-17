#if !defined(AFX_EXECUTIONTAB_H__F5749611_6CF3_48A8_AF35_E51515267709__INCLUDED_)
#define AFX_EXECUTIONTAB_H__F5749611_6CF3_48A8_AF35_E51515267709__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000


// TestStand API, TestStand UI ActiveX controls API, and utilities for using these APIs in C++
// The file is located in <TestStand>\API\VC
#include "tsutilCPP.h"	

class CTestExecDlg; // forward declaration

/////////////////////////////////////////////////////////////////////////////
// CExecutionTab dialog

class CExecutionTab : public CDialog
{
// Construction
public:
	CExecutionTab(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CExecutionTab)
	enum { IDD = IDD_EXECUTION_TAB_DIALOG };
		// NOTE: the ClassWizard will add data members here
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CExecutionTab)
	public:
	virtual BOOL PreTranslateMessage(MSG* pMsg);
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CExecutionTab)
	virtual BOOL OnInitDialog();
	void OnCreateContextMenuSequenceView(long menuHandle, long x, long y);
	void OnBorderDraggedExecutionStepList(long bordersChanged, long newX, long newY, long newWidth, long newHeight, BOOL finalResize);
	void BorderDraggedVariables(long bordersChanged, long newX, long newY, long newWidth, long newHeight, BOOL finalResize);
	void OnBorderDraggedCallstackList(long bordersChanged, long newX, long newY, long newWidth, long newHeight, BOOL finalResize);
	void OnConnectionActivityExecutionLabel(long activity);
	//}}AFX_MSG

	DECLARE_EVENTSINK_MAP()
	DECLARE_MESSAGE_MAP()

	CTestExecDlg * ParentDialog() {return (CTestExecDlg *)this->GetParent();}

public:
		// ActiveX control smart pointers (#import style)
		TSUI::IListBoxPtr			mThreads;
		TSUI::IListBoxPtr			mCallStack;
		TSUI::ISequenceViewPtr		mExecutionStepList;
		TSUI::IButtonPtr			mBreakResumeBtn;
		TSUI::IButtonPtr			mTerminateRestartBtn;
		TSUI::IVariablesViewPtr		mExecutionVariables;
		TSUI::ILabelPtr				mExecutionLabel;

		// CWnds for each ActiveX control
		CWnd						mThreadsCWnd;
		CWnd						mCallStackCWnd;
		CWnd						mExecutionStepListCWnd;
		CWnd						mBreakResumeBtnCWnd;
		CWnd						mTerminateRestartBtnCWnd;
		CWnd						mExecutionVariablesCWnd;
		CWnd						mExecutionLabelCWnd;
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_EXECUTIONTAB_H__F5749611_6CF3_48A8_AF35_E51515267709__INCLUDED_)
