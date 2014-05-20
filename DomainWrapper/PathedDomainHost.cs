using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DomainWrapper
{
    public class PathedDomainHost : CriticalFinalizerObject, IDisposable
    {
        private AppDomain _hostDomain;

        string _dllPath;

        public PathedDomainHost(string name, string path)
        {
            var setupInfo = new AppDomainSetup()
            {
                ApplicationBase = path,
                PrivateBinPath = path,
            };

            _dllPath = Path.Combine(path, Path.ChangeExtension(name, "exe"));
            _hostDomain = AppDomain.CreateDomain(name, AppDomain.CurrentDomain.Evidence, setupInfo);
        }
        ~PathedDomainHost()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_hostDomain != null)
            {
                AppDomain.Unload(_hostDomain);
                _hostDomain = null;
            }
        }

        public void Execute()
        {
            _hostDomain.ExecuteAssembly(_dllPath);
        }
    }
}