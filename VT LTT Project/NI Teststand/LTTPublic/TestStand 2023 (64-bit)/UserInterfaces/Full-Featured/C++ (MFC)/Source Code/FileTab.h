#if !defined(AFX_FILETAB_H__BD83D338_6B69_4515_AFDC_4996CBD630AA__INCLUDED_)
#define AFX_FILETAB_H__BD83D338_6B69_4515_AFDC_4996CBD630AA__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

// TestStand API, TestStand UI ActiveX controls API, and utilities for using these APIs in C++
// The file is located in <TestStand>\API\VC
#include "tsutilCPP.h"	

class CTestExecDlg;	// forward declaration

/////////////////////////////////////////////////////////////////////////////
// CFileTab dialog

class CFileTab : public CDialog
{
// Construction
public:
	CFileTab(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CFileTab)
	enum { IDD = IDD_FILE_TAB_DIALOG };
	CStatic	mSequenceDescriptionLabel;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CFileTab)
	public:
	virtual BOOL PreTranslateMessage(MSG* pMsg);
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CFileTab)
	virtual BOOL OnInitDialog();
	void OnCreateContextMenuFileStepList(long menuHandle, long x, long y);
	void OnCreateContextMenuSequencesList(long menuHandle, long x, long y);
	void OnBorderDraggedFileStepList(long bordersChanged, long newX, long newY, long newWidth, long newHeight, BOOL finalResize);
	void OnBorderDraggedFileVariables(long bordersChanged, long newX, long newY, long newWidth, long newHeight, BOOL finalResize);
	void OnBorderDraggedInsertionPalette(long bordersChanged, long newX, long newY, long newWidth, long newHeight, BOOL finalResize);
	void OnConnectionActivitySequenceFileLabel(long activity);
	//}}AFX_MSG

	DECLARE_EVENTSINK_MAP()
	DECLARE_MESSAGE_MAP()

	CTestExecDlg * ParentDialog() {return (CTestExecDlg *)this->GetParent();}

public:
		// ActiveX control smart pointers (#import style)
		TSUI::IListBoxPtr			mSequencesList;
		TSUI::IButtonPtr			mEntryPoint1Btn;
		TSUI::IButtonPtr			mEntryPoint2Btn;
		TSUI::IButtonPtr			mRunSequenceBtn;
		TSUI::ISequenceViewPtr		mFileStepList;
		TSUI::IVariablesViewPtr		mFileVariables;
		TSUI::IInsertionPalettePtr	mInsertionPalette;
		TSUI::ILabelPtr				mSequenceFileLabel;

		// CWnds for each ActiveX control
		CWnd						mSequencesListCWnd;
		CWnd						mEntryPoint1BtnCWnd;
		CWnd						mEntryPoint2BtnCWnd;
		CWnd						mRunSequenceBtnCWnd;
		CWnd						mFileStepListCWnd;
		CWnd						mFileVariablesCWnd;
		CWnd						mInsertionPaletteCWnd;
		CWnd						mSequenceFileLabelCWnd;		
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_FILETAB_H__BD83D338_6B69_4515_AFDC_4996CBD630AA__INCLUDED_)
