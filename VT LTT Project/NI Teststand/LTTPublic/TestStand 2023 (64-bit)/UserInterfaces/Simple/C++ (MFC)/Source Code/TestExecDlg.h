// TestExecDlg.h : header file
//

#if !defined(AFX_TESTEXECDLG_H__2D42D297_84D9_11D3_AEA3_8D61D959765A__INCLUDED_)
#define AFX_TESTEXECDLG_H__2D42D297_84D9_11D3_AEA3_8D61D959765A__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

// TestStand API, TestStand UI ActiveX controls API, and utilities for using these APIs in C++
// The file is located in <TestStand>\API\VC
#include "tsutilCPP.h"	


/////////////////////////////////////////////////////////////////////////////
// CTestExecDlg dialog

class CTestExecDlg : public CDialog
	{
		// Construction
	public:
		CTestExecDlg(CWnd* pParent = NULL);	// standard constructor		

	// Dialog Data
		//{{AFX_DATA(CTestExecDlg)
	enum { IDD = IDD_TESTEXEC_DIALOG };
	//}}AFX_DATA

		// ClassWizard generated virtual function overrides
		//{{AFX_VIRTUAL(CTestExecDlg)
	protected:
		virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support
	//}}AFX_VIRTUAL

	// Implementation
	protected:
		HICON	mIcon;

		// Generated message map functions
		//{{AFX_MSG(CTestExecDlg)
		virtual BOOL OnInitDialog();
		afx_msg void OnPaint();
		afx_msg void OnClose();
		afx_msg HCURSOR OnQueryDragIcon();
		afx_msg void OnExitApplication_ApplicationMgr();
		afx_msg void OnWait_ApplicationMgr(BOOL showWaitVal);
		afx_msg void OnHandleError_ApplicationMgr(long errorCode, LPCTSTR errorMessage);
		afx_msg void OnDisplayExecutionApplicationMgr(LPDISPATCH exec, TSUI::ExecutionDisplayReasons reason);
		afx_msg void OnDisplaySequenceFileApplicationMgr(LPDISPATCH file, TSUI::SequenceFileDisplayReasons reason);
		DECLARE_EVENTSINK_MAP()
	//}}AFX_MSG
		DECLARE_MESSAGE_MAP()

	private:
		// ActiveX control smart pointers (#import style)
		TSUI::IApplicationMgrPtr		mApplicationMgr;
		TSUI::ISequenceFileViewMgrPtr	mSequenceFileViewMgr;
		TSUI::IExecutionViewMgrPtr		mExecutionViewMgr;
		TSUI::IComboBoxPtr				mFilesCombo;
		TSUI::IButtonPtr				mOpenFileBtn;
		TSUI::IComboBoxPtr				mSequencesCombo;
		TSUI::IButtonPtr				mCloseFileBtn;
		TSUI::IButtonPtr				mEntryPoint1Btn;
		TSUI::IButtonPtr				mEntryPoint2Btn;
		TSUI::IButtonPtr				mRunSelectedBtn;
		TSUI::IComboBoxPtr				mExecutionsCombo;
		TSUI::IButtonPtr				mCloseExecutionBtn;
		TSUI::ISequenceViewPtr			mSequenceView;
		TSUI::IButtonPtr				mBreakResumeBtn;
		TSUI::IButtonPtr				mTerminateRestartBtn;
		TSUI::IButtonPtr				mTerminateAllBtn;
		TSUI::IButtonPtr				mLoginLogoutBtn;
		TSUI::IButtonPtr				mExitBtn;
		TSUI::IReportViewPtr			mReportView;

		// CWnds for each ActiveX control
		CWnd							mApplicationMgrCWnd;
		CWnd							mSequenceFileViewMgrCWnd;
		CWnd							mExecutionViewMgrCWnd;
		CWnd							mFilesComboCWnd;
		CWnd							mOpenFileBtnCWnd;
		CWnd							mSequencesComboCWnd;
		CWnd							mCloseFileBtnCWnd;
		CWnd							mEntryPoint1BtnCWnd;
		CWnd							mEntryPoint2BtnCWnd;
		CWnd							mRunSelectedBtnCWnd;
		CWnd							mExecutionsComboCWnd;
		CWnd							mCloseExecutionBtnCWnd;
		CWnd							mSequenceViewCWnd;
		CWnd							mBreakResumeBtnCWnd;
		CWnd							mTerminateRestartBtnCWnd;
		CWnd							mTerminateAllBtnCWnd;
		CWnd							mLoginLogoutBtnCWnd;
		CWnd							mExitBtnCWnd;
		CWnd							mReportViewCWnd;
	};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TESTEXECDLG_H__2D42D297_84D9_11D3_AEA3_8D61D959765A__INCLUDED_)
