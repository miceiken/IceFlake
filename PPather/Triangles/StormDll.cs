/*
 *  Part of PPather
 *  Copyright Pontus Borg 2008
 * 
 */

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using PatherPath.Graph;
using IOPath = System.IO.Path;

namespace StormDll
{
    internal unsafe class StormDll
    {
        /*
         * 
typedef unsigned long       DWORD;
typedef DWORD LCID;  

// Archive opening/closing
LCID  WINAPI SFileSetLocale(LCID lcNewLocale);
LCID  WINAPI SFileGetLocale();
BOOL  WINAPI SFileOpenArchive(const char * szMpqName, DWORD dwPriority, DWORD dwFlags, HANDLE * phMpq);
BOOL  WINAPI SFileCloseArchive(HANDLE hMpq);

// File opening/closing
BOOL  WINAPI SFileOpenFileEx(HANDLE hMpq, const char * szFileName, DWORD dwSearchScope, HANDLE * phFile);
BOOL  WINAPI SFileCloseFile(HANDLE hFile);

// File I/O
DWORD WINAPI SFileGetFilePos(HANDLE hFile, DWORD * pdwFilePosHigh = NULL);
DWORD WINAPI SFileGetFileSize(HANDLE hFile, DWORD * pdwFileSizeHigh = NULL);
DWORD WINAPI SFileSetFilePointer(HANDLE hFile, LONG lFilePos, LONG * pdwFilePosHigh, DWORD dwMethod);
BOOL  WINAPI SFileReadFile(HANDLE hFile, VOID * lpBuffer, DWORD dwToRead, 
                           DWORD * pdwRead = NULL, LPOVERLAPPED lpOverlapped = NULL);

// Adds another listfile into MPQ. The currently added listfile(s) remain,
// so you can use this API to combining more listfiles.
// Note that this function is internally called by SFileFindFirstFile
int   WINAPI SFileAddListFile(HANDLE hMpq, const char * szListFile);

          
         */

        [DllImport("PPather\\StormLib.dll")]
        public static extern uint SFileGetLocale();

        [DllImport("PPather\\StormLib.dll")]
        public static extern bool SFileOpenArchive([MarshalAs(UnmanagedType.LPStr)] string szMpqName,
                                                   uint dwPriority, uint dwFlags,
                                                   void** phMpq);

        [DllImport("PPather\\StormLib.dll")]
        public static extern bool SFileCloseArchive(void* hMpq);


        [DllImport("PPather\\StormLib.dll")]
        public static extern bool SFileOpenFileEx(void* hMpq,
                                                  [MarshalAs(UnmanagedType.LPStr)] string szFileName,
                                                  uint dwSearchScope,
                                                  void** phFile);

        [DllImport("PPather\\StormLib.dll")]
        public static extern bool SFileCloseFile(void* hFile);

        [DllImport("PPather\\StormLib.dll")]
        public static extern uint SFileGetFilePos(void* hFile, uint* pdwFilePosHigh);

        [DllImport("PPather\\StormLib.dll")]
        public static extern uint SFileGetFileSize(void* hFile, uint* pdwFileSizeHigh);

        [DllImport("PPather\\StormLib.dll")]
        public static extern uint SFileSetFilePointer(void* hFile,
                                                      int lFilePos, int* pdwFilePosHigh, uint dwMethod);

        [DllImport("PPather\\StormLib.dll")]
        public static extern bool SFileReadFile(void* hFile, void* lpBuffer, uint dwToRead,
                                                uint* pdwRead, void* lpOverlapped);

        [DllImport("PPather\\StormLib.dll")]
        public static extern bool SFileExtractFile(void* hMpq,
                                                   [MarshalAs(UnmanagedType.LPStr)] string szToExtract,
                                                   [MarshalAs(UnmanagedType.LPStr)] string szExtracted);

        [DllImport("PPather\\StormLib.dll")]
        public static extern bool SFileHasFile(void* hMpq,
                                               [MarshalAs(UnmanagedType.LPStr)] string szFileName);

        /*
        [DllImport("user32.dll")]
        public static extern int MessageBoxA(int h,
                    [MarshalAs(UnmanagedType.LPStr)]string m,
                    [MarshalAs(UnmanagedType.LPStr)]string c,
                    int type); 
         * */

        public static uint GetLocale()
        {
            return SFileGetLocale();
        }
    }

    public class ArchiveSet
    {
        private readonly List<Archive> archives = new List<Archive>();
        private string GameDir = ".\\";

        public void SetGameDir(string dir)
        {
            GameDir = dir;
        }

        public string SetGameDirFromReg()
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Blizzard Entertainment\\World of Warcraft");
            if (key == null)
                return null;
            Object val = key.GetValue("InstallPath");
            if (val == null)
                return null;
            string s = val.ToString();
            SetGameDir(s + "Data\\");
            return s;
        }

        public bool AddArchive(string file)
        {
            var a = new Archive(GameDir + file, 0, 0);
            if (a.IsOpen())
            {
                archives.Add(a);
                PathGraph.Log("Add archive " + file);
                return true;
            }
            return false;
        }

        public int AddArchives(string[] files)
        {
            int n = 0;
            foreach (string s in files)
            {
                if (AddArchive(s))
                    n++;
            }
            return n;
        }

        public bool HasFile(string name)
        {
            foreach (Archive a in archives)
            {
                if (a.HasFile(name))
                    return true;
            }
            return false;
        }

        public bool ExtractFile(string from, string to)
        {
            foreach (Archive a in archives)
            {
                if (a.HasFile(from))
                {
                    //PathGraph.Log("Extract " + from);
                    bool ok = a.ExtractFile(from, to);
                    //PathGraph.Log("  result: " + ok);
                    return ok;
                }
            }
            return false;
        }

        public void Close()
        {
            foreach (Archive a in archives)
            {
                a.Close();
            }
            archives.Clear();
        }
    }

    public unsafe class Archive
    {
        private void* handle = null;

        public Archive(string file, uint Prio, uint Flags)
        {
            bool r = Open(file, Prio, Flags);
        }

        public bool IsOpen()
        {
            return handle != null;
        }

        private bool Open(string file, uint Prio, uint Flags)
        {
            void* h;
            void** hp = &h;
            bool r = StormDll.SFileOpenArchive(file, Prio, Flags, hp);
            handle = h;
            return r;
        }

        public bool Close()
        {
            bool r = StormDll.SFileCloseArchive(handle);
            if (r)
                handle = null;
            return r;
        }

        public File OpenFile(string szFileName, uint dwSearchScope)
        {
            void* h;
            void** hp = &h;
            bool r = StormDll.SFileOpenFileEx(handle, szFileName, dwSearchScope, hp);
            if (!r)
                return null;
            return new File(this, h);
        }

        public bool HasFile(string name)
        {
            bool r = StormDll.SFileHasFile(handle, name);
            return r;
        }

        public bool ExtractFile(string from, string to)
        {
            bool r = StormDll.SFileExtractFile(handle, from, to);
            return r;
        }
    }

    public unsafe class File
    {
        private Archive archive;
        private void* handle;

        public File(Archive a, void* h)
        {
            archive = a;
            handle = h;
        }

        public bool Close()
        {
            bool r = StormDll.SFileCloseFile(handle);
            if (r)
                handle = null;
            return r;
        }

        public ulong GetSize()
        {
            uint high;
            uint* phigh = &high;
            uint low = StormDll.SFileGetFileSize(handle, phigh);
            //if (low == 0xffffffff)
            //    return 0;
            return low;
        }
    }
}