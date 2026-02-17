// TestExec.h : main header file for the TESTEXEC application
//

#if !defined(AFX_TESTEXEC_H__2D42D295_84D9_11D3_AEA3_8D61D959765A__INCLUDED_)
#define AFX_TESTEXEC_H__2D42D295_84D9_11D3_AEA3_8D61D959765A__INCLUDED_

#pragma once

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"		// main symbols

/////////////////////////////////////////////////////////////////////////////
// CTestExecApp:
// See TestExec.cpp for the implementation of this class
//

class CTestExecApp : public CWinApp
{
public:
	CTestExecApp();

	private:

		//Functions for helping to check that the engine can be used
		bool IsEngineAccessible();
		bool ConfirmPresenceOfRequiredEngine();
		bool GetRegistryKeyFromHkeyClassesRootIfExists(const TCHAR * registrySubKeyPath, HKEY& newKey);
		TCHAR * GetStringValueFromHkeyClassesRootIfExists(TCHAR * registrySubKeyPath, TCHAR * valueName);

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTestExecApp)
	public:
	virtual BOOL InitInstance();
	//}}AFX_VIRTUAL

	//{{AFX_MSG(CTestExecApp)
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TESTEXEC_H__2D42D295_84D9_11D3_AEA3_8D61D959765A__INCLUDED_)
