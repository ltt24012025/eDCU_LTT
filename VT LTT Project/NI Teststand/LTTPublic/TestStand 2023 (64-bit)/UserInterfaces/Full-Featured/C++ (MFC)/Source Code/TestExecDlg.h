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
using namespace TSUTIL;

// more convenient than GetWindowRect because the rectangle is the return value and because it can return client coordinates
inline CRect RectOf(CWnd &cWnd, BOOL returnClientCoordinates = TRUE)  {return DialogHelper::RectOf(cWnd, returnClientCoordinates);} 

// using allows us to not type the namespace qualifier in front of each api call
using namespace TS;
using namespace TSUI;

#include "FileTab.h"
#include "ExecutionTab.h"
#include "ReportTab.h"

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
	CStatic	mMenuDividerBar;
	CTabCtrl	m_tabCtrl;
	//}}AFX_DATA

		// ClassWizard generated virtual function overrides
		//{{AFX_VIRTUAL(CTestExecDlg)
	public:
		virtual BOOL PreTranslateMessage(MSG* pMsg);
	protected:
		virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support
	//}}AFX_VIRTUAL

	// Implementation
	protected:
		HICON	mIcon;
		HACCEL	mAccelTable;		// accelerator table

		// Generated message map functions
		//{{AFX_MSG(CTestExecDlg)
		virtual BOOL OnInitDialog();
		afx_msg void OnPaint();
		afx_msg void OnClose();
		afx_msg HCURSOR OnQueryDragIcon();
		afx_msg void OnExitApplicationApplicationMgr();
		afx_msg void OnInitMenu(CMenu* pMenu);
		afx_msg void OnWaitApplicationMgr(BOOL showWaitVal);
		afx_msg void OnReportErrorApplicationMgr(long errorCode, LPCTSTR errorMessage);
		afx_msg void OnDisplayExecutionApplicationMgr(LPDISPATCH exec, ExecutionDisplayReasons reason);
		afx_msg void OnDisplaySequenceFileApplicationMgr(LPDISPATCH file, SequenceFileDisplayReasons reason);
		afx_msg void OnSelchangeTab(NMHDR* pNMHDR, LRESULT* pResult);
		afx_msg void OnCurPageChangedListbar(long CurrentPage);
		afx_msg void OnDisplayReportApplicationMgr(LPDISPATCH exec);
		afx_msg void OnStartExecutionApplicationMgr(LPDISPATCH exec, LPDISPATCH thrd, BOOL initiallyHidden);
		afx_msg void OnExecutionChangedExecutionViewMgr(LPDISPATCH exec);
		afx_msg void OnHelpAbout();
		virtual void OnOK();
		virtual void OnCancel();
		afx_msg void OnSize(UINT nType, int cx, int cy);
		afx_msg void OnCreateContextMenuListbar(long menuHandle, long x, long y);
		afx_msg void OnBorderDraggedListbar(long bordersChanged, long newX, long newY, long newWidth, long newHeight, BOOL finalResize);
		afx_msg void OnQueryShutdownApplicationMgr(long* opt);
		afx_msg void EditModeChangedApplicationMgr();
		afx_msg void PostCommandExecuteApplicationMgr(LPDISPATCH Command);

		DECLARE_EVENTSINK_MAP()
	//}}AFX_MSG
		DECLARE_MESSAGE_MAP()

	public: 
		BOOL					HandleAccelerators(MSG* pMsg);
		void					ArrangeControls(void);
		IApplicationMgrPtr		GetApplicationMgr() {return mApplicationMgr;}
		ISequenceFileViewMgrPtr	GetSequenceFileViewMgr() {return mSequenceFileViewMgr;}
		IExecutionViewMgrPtr	GetExecutionViewMgr() {return mExecutionViewMgr;}
		IDispatchPtr			GetActiveViewManager(void);
		void					BuildCommandSetMenu(TSUI::CommandKinds commandSet, long menuHandle);
		void					RebuildMenuBar(void);
		void					UpdateWindowTitle();

	private:
		void SetTabPageBounds(CTabCtrl &tabCtrl, CDialog &tabPage);
		void InitializeTabPages(IEnginePtr engine);
		void ShowReport(ExecutionPtr execution);
		void ConnectListBarPages();
		void ConnectStatusBarPanes();
		void ShowAppropriateTabs();
		void ShowAppropriateTabWindow();
		void ShowAppropriateStatusBarPanes();
		void GetAdditionalLocalizedStrings(TSUTIL::Localizer &localizer);
		
		enum TabPageIdentifier  // need a way to identify the different tab pages. Index won't work because not all pages are in the tab control at the same time
		{
			TabPageIdentifier_NotATab,
			TabPageIdentifier_SequenceFile,
			TabPageIdentifier_Execution,
			TabPageIdentifier_Report
		};

		// ActiveX control smart pointers (#import style)
		IApplicationMgrPtr				mApplicationMgr;
		ISequenceFileViewMgrPtr			mSequenceFileViewMgr;
		IExecutionViewMgrPtr			mExecutionViewMgr;
		IListBarPtr						mListBar;
		IStatusBarPtr					mStatusBar;

		// CWnds for each ActiveX control
		CWnd							mApplicationMgrCWnd;
		CWnd							mSequenceFileViewMgrCWnd;
		CWnd							mExecutionViewMgrCWnd;
		CWnd							mListBarCWnd;
		CWnd							mStatusBarCWnd;

		// the MenuBuilder makes it simple to create menu items for common TestStand commands
		TSUTIL::MenuBuilder				mMenuBuilder;		

		// child dialog window with the control for each tab page
		CFileTab						mFileTab;
		CExecutionTab					mExecutionTab;
		CReportTab						mReportTab;

		// various strings that require explicit localization
		_bstr_t							mErrorDlgTitle;
		_bstr_t							mFileTabTitle;
		_bstr_t							mExecutionTabTitle;
		_bstr_t							mReportTabTitle;

		bool							m_ProgrammaticallyUpdatingTabPages;
		TSUTIL::Controls				mWindowsToPersistSizesFor;
		TSUTIL::Controls				mWindowsToPersistBoundsFor;

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.
};
#endif // !defined(AFX_TESTEXECDLG_H__2D42D297_84D9_11D3_AEA3_8D61D959765A__INCLUDED_)
