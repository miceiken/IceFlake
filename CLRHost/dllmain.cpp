// dllmain.cpp : Defines the entry point for the DLL application.
#include <Windows.h>

#include <mscoree.h>
#include <metahost.h>
#include <string.h>
#include <wchar.h>
#include <fstream>

#pragma comment(lib, "mscoree.lib")

EXTERN_C IMAGE_DOS_HEADER __ImageBase;


DWORD WINAPI StartTheDotNetRuntime( LPVOID lpParameter)
{	
	ICLRMetaHost * lpMetaHost = NULL;
    HRESULT hr;

	 hr = CLRCreateInstance(
        CLSID_CLRMetaHost,
        IID_ICLRMetaHost,
        (LPVOID *)&lpMetaHost);
    if (FAILED(hr)) return 1;
    
    DWORD dwVersion = 0;
    DWORD dwImageVersion = 0;
    ICLRRuntimeInfo * lpRuntimeInfo = NULL;
    // Get a reference for the ICLRRuntimeInfo
    hr = lpMetaHost->GetRuntime(
        L"v4.0.30319", // 4.0
        IID_ICLRRuntimeInfo,
        (LPVOID *)&lpRuntimeInfo);
    if (FAILED(hr)) return 2;
    
    ICLRRuntimeHost * lpRuntimeHost = NULL;
    // Load the CLR.
    hr = lpRuntimeInfo->GetInterface(
        CLSID_CLRRuntimeHost,
        IID_ICLRRuntimeHost,
        (LPVOID *)&lpRuntimeHost);
    if (FAILED(hr)) return 3;
    
    // Start the CLR by using the hosting version 4
    hr = lpRuntimeHost->Start();
    if (FAILED(hr)) return 4;
    
	LPWSTR strDLLPath1 = new WCHAR[_MAX_PATH];
	::GetModuleFileNameW((HINSTANCE)&__ImageBase, strDLLPath1, _MAX_PATH);

	std::wstring tempPath = strDLLPath1;
	int index = tempPath.rfind('\\');
	tempPath.erase(index, tempPath.length() - index);
	tempPath += L"\\DomainManager.dll";

    DWORD dwRetCode = 0;
    // The function has to be: static int EntryPoint(string sz)
    hr = lpRuntimeHost->ExecuteInDefaultAppDomain(
        (LPWSTR)tempPath.c_str(),    // Executable path
        L"DomainManager.EntryPoint",
        L"Main",
        L"IceFlake.dll",
        &dwRetCode);
    if (FAILED(hr)) return 5;

	lpRuntimeHost->Release();

    return 0;
}


BOOL APIENTRY DllMain (HINSTANCE hInst /* Library instance handle. */ ,
					   DWORD reason    /* Reason this function is being called. */ ,
					   LPVOID reserved /* Not used. */ )
{
	switch (reason)
	{
		case DLL_PROCESS_ATTACH:
			{
				DWORD dwThreadId;
				HANDLE hThread = CreateThread(0, 0, StartTheDotNetRuntime, NULL, 0, &dwThreadId);
				break;
			}
		case DLL_PROCESS_DETACH:
			{
				
			}
	}
	
	return true;
}
