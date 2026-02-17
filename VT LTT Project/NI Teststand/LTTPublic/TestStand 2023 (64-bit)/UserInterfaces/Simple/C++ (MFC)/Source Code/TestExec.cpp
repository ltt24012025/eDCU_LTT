// TestExec.cpp : Defines the class behaviors for the application.
//

#include "stdafx.h"
#include "TestExec.h"
#include "TestExecDlg.h"

#include <sys/stat.h>

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

//This value identifies the registry key where the path to the TestStand engine
//is stored. We can use it to check whether the engine is available.
#define REGISTRY_SUBKEY_WITH_ENGINE_PATH _T("CLSID\\{B2794EF6-C0B6-11D0-939C-0020AF68E893}\\InprocServer32")

/////////////////////////////////////////////////////////////////////////////
// CTestExecApp
BEGIN_MESSAGE_MAP(CTestExecApp, CWinApp)
	//{{AFX_MSG_MAP(CTestExecApp)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CTestExecApp construction
CTestExecApp::CTestExecApp()
{
}

/////////////////////////////////////////////////////////////////////////////
// The one and only CTestExecApp object
CTestExecApp g_theApp;

/////////////////////////////////////////////////////////////////////////////
// CTestExecApp initialization

BOOL CTestExecApp::InitInstance()
{
	//Check whether the engine is available, and display an
	//error message if not.
	bool engineIsAvailable = ConfirmPresenceOfRequiredEngine();
	
	//If the engine is not available, don't do anything else.
	if(engineIsAvailable)
	{
		// Initialize OLE libraries
		if (!AfxOleInit())
		{
			AfxMessageBox(IDP_OLE_INIT_FAILED);
			return FALSE;
		}

		AfxEnableControlContainer();

#if _MFC_VER < 0x0700  // functions obsoleted in newer MFC
#ifdef _AFXDLL
		Enable3dControls();			// Call this when using MFC in a shared DLL
#else
		Enable3dControlsStatic();	// Call this when linking to MFC statically
#endif
#endif

		CTestExecDlg dlg;
		m_pMainWnd = &dlg;

		const INT_PTR exitCode = dlg.DoModal();
	
		if (AfxGetCurrentMessage())
			AfxGetCurrentMessage()->wParam = exitCode; // MFC returns this value as the application exit code. 
	}

	// Since the dialog has been closed, return FALSE so that we exit the
	//  application, rather than start the application's message pump.
	return FALSE;
}

/////////////////////////////////////////////////////////////////////////////
//Gets a key to read from the registry and places it in newKey if the key exists.
//If the key cannot be obtained, this function will return false. The key must 
//be a subkey of HKEY_CLASSES_ROOT. registrySubKeyPath should be the path to 
//the key after HKEY_CLASSES_ROOT. The function calling this takes responsibility
//for closing the result key by using RegCloseKey(HKEY hKey).
bool CTestExecApp::GetRegistryKeyFromHkeyClassesRootIfExists(const TCHAR * registrySubKeyPath, HKEY& newKey)
{
	bool obtainedKey = false;

	LONG result = RegOpenKeyEx(HKEY_CLASSES_ROOT, registrySubKeyPath, 0, KEY_READ, &newKey);
	
	if(result != ERROR_SUCCESS)
	{
		RegCloseKey(newKey);
		newKey = NULL;
	}
	else
	{
		obtainedKey = true;
	}
	

	return obtainedKey;
}

/////////////////////////////////////////////////////////////////////////////
//Gets a string value from a registry key provided that:
//1. That registry key is a subkey of HKEY_CLASSES_ROOT
//2. The subkey exists
//3. The value with the requested name exists
//4. The value with the requested name is a string value
//If any of these conditions is not met, the function returns null.
//Otherwise, it returns a pointer to the string value from the registry key.
//That string value will be encoded in ANSI. The code that calls this
//function takes responsibility for deleting the string returned using delete [].
TCHAR * CTestExecApp::GetStringValueFromHkeyClassesRootIfExists(TCHAR * registrySubKeyPath, TCHAR * valueName)
{
	LPBYTE b = 0;
	DWORD bufSize = 0, valueType = 0;
	HKEY key = NULL;
	bool gotKey = GetRegistryKeyFromHkeyClassesRootIfExists(registrySubKeyPath,key);
	TCHAR *resultString = NULL;

	if(gotKey)
	{
		LONG result = RegQueryValueEx(key, valueName, NULL, &valueType, NULL, &bufSize);

		if (result == ERROR_SUCCESS && (valueType == REG_SZ /*type for a string with no references to environment variables*/))
		{
			const size_t stringLength = bufSize / sizeof(TCHAR);
			// Per Microsoft documentation, in some cases the string may have been stored without the terminating
			// null, so allocate an extra character to stick a terminator in.
			// https://msdn.microsoft.com/en-us/library/windows/desktop/ms724911.aspx
			resultString = new TCHAR[stringLength + 1 /*+1 is for null terminator*/];
			result = RegQueryValueEx(key, valueName, NULL, &valueType, LPBYTE(resultString), &bufSize);

			if (result == ERROR_SUCCESS && (valueType == REG_SZ))
			{
				// Force null termination, just in case.
				resultString[stringLength] = _T('\0');
			}
			else
			{
				delete [] resultString;
				resultString = NULL;
			}
		}

		RegCloseKey(key);
		key = NULL;
	}

	return resultString;
}

/////////////////////////////////////////////////////////////////////////////
//Checks whether the TestStand engine is registered. If
//it is registered, checks whether the engine dll is
//present. Returns a bool indicating whether the engine 
//is accessible.
bool CTestExecApp::IsEngineAccessible()
{
	bool engineDllExists = false;

	//Get a path to the engine dll. We take responsibility for de-allocating the string that's returned.
	const TCHAR * engineDllPath = GetStringValueFromHkeyClassesRootIfExists(REGISTRY_SUBKEY_WITH_ENGINE_PATH, NULL);
	
	//check if the engine dll file exists.
	if(engineDllPath)
	{
		struct _stat unusedStatBuffer;
		engineDllExists = (_tstat(engineDllPath, &unusedStatBuffer) == 0);
		
		delete [] engineDllPath;
		engineDllPath = NULL;
	}
	
	return engineDllExists;
}

/////////////////////////////////////////////////////////////////////////////
//Checks whether the engine with bitness matching the
//current process is accessible. Displays an error
//message if not.
bool CTestExecApp::ConfirmPresenceOfRequiredEngine()
{
	//Check for the engine whose bitness matches this process, since that's
	//the one that will be needed to run the UI.
	bool engineIsAccessible = IsEngineAccessible();

	if(!engineIsAccessible)
	{
		
		//The user built the UI with a different version than is active or a different bitness than is installed.
		//Use a string literal for the message- since we don't have an engine, we can't localize.
#ifdef _WIN64
		const TCHAR errorMsg[] = _T("This TestStand User Interface was built for a version of TestStand different than the version that is currently active.\n")
							_T("Use the TestStand Version Selector to activate the correct version and ensure that 64-bit TestStand is installed.");
#else
		const TCHAR errorMsg[] = _T("This TestStand User Interface was built for a version of TestStand different than the version that is currently active.\n")
							_T("Use the TestStand Version Selector to activate the correct version and ensure that 32-bit TestStand is installed.");
#endif
		MessageBox(NULL, errorMsg, NULL, MB_ICONERROR);
	}
	
	return engineIsAccessible;
}
