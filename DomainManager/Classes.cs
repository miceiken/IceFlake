using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace DomainManager
{
    public interface IAssemblyLoader
    {
        void LoadAndRun(string file);
    }

    public class EntryPoint
    {
        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);

        [STAThread]
        public static int Main(string args)
        {
            DomainManager.Arguments = args;
            bool firstLoaded = false;
            while (true)
            {
                if (!firstLoaded)
                {
                    firstLoaded = true;
                    new CleanDomain("IceFlake.dll");
                }

                if ((GetAsyncKeyState(Keys.F11) & 1) == 1)
                {
                    new CleanDomain("IceFlake.dll");
                }

                Thread.Sleep(10);
            }
            return 0;
        }
    }

    public static class DomainManager
    {
        public static AppDomain CurrentDomain { get; set; }
        public static CleanAssemblyLoader CurrentAssemblyLoader { get; set; }
        public static string Arguments { get; set; }
    }

    public class CleanAssemblyLoader : MarshalByRefObject, IAssemblyLoader
    {
        public CleanAssemblyLoader()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        #region IAssemblyLoader Members

        public void LoadAndRun(string file)
        {
            Assembly asm = Assembly.Load(file);
            MethodInfo entry = asm.GetType("IceFlake.EntryPoint").GetMethod("Main");
            entry.Invoke(null, new string[] { DomainManager.Arguments });
        }

        #endregion

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name == Assembly.GetExecutingAssembly().FullName)
                return Assembly.GetExecutingAssembly();

            string appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string shortAsmName = Path.GetFileName(args.Name);
            string fileName = Path.Combine(appDir, shortAsmName);

            if (File.Exists(fileName))
            {
                return Assembly.LoadFrom(fileName);
            }
            return Assembly.GetExecutingAssembly().FullName == args.Name ? Assembly.GetExecutingAssembly() : null;
        }
    }

    public class CleanDomain
    {
        private readonly Random _rand = new Random();

        public CleanDomain(string assemblyName)
        {
            try
            {
                string appBase = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var ads = new AppDomainSetup {ApplicationBase = appBase, PrivateBinPath = appBase};
                DomainManager.CurrentDomain = AppDomain.CreateDomain("CleanDomain_Internal_" + _rand.Next(0, 100000),
                                                                     null, ads);
                AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
                DomainManager.CurrentAssemblyLoader =
                    (CleanAssemblyLoader)
                    DomainManager.CurrentDomain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName,
                                                                        typeof (CleanAssemblyLoader).FullName);

                DomainManager.CurrentAssemblyLoader.LoadAndRun(
                    Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), assemblyName));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                DomainManager.CurrentAssemblyLoader = null;
                AppDomain.Unload(DomainManager.CurrentDomain);
            }
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            try
            {
                Assembly assembly = Assembly.Load(args.Name);
                if (assembly != null)
                    return assembly;
            }
            catch
            {
            }

            string[] Parts = args.Name.Split(',');
            string File = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + Parts[0].Trim() +
                          ".dll";

            return Assembly.LoadFrom(File);
        }
    }
}