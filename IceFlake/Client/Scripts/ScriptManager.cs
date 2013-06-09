using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Microsoft.CSharp;

namespace IceFlake.Client.Scripts
{
    public static class ScriptManager
    {
        static ScriptManager()
        {
            RegistrationQueue = new Queue<Type>();
            Scripts = new List<Script>();
        }

        private static readonly object SynchronizeLock = new object();

        public static string ScriptFolder
        {
            get;
            private set;
        }

        private static Queue<Type> RegistrationQueue
        {
            get;
            set;
        }

        public static List<Script> Scripts
        {
            get;
            private set;
        }

        private static Script CurrentScript
        {
            get;
            set;
        }

        public static void Initialize()
        {
            ScriptFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts");
            //CompileAsync();
            lock (SynchronizeLock)
            {
                var typeScripts = Assembly.GetExecutingAssembly().GetTypes()
                                    .Where(t => t.IsSubclassOf(typeof(Script)));
                foreach (var t in typeScripts)
                    RegistrationQueue.Enqueue(t);
            }
        }

        public static void Pulse()
        {
            lock (SynchronizeLock)
            {
                while (RegistrationQueue.Count > 0)
                    Register(RegistrationQueue.Dequeue());

                foreach (var script in Scripts)
                {
                    CurrentScript = script;
                    script.Tick();
                }

                CurrentScript = null;
            }
        }

        public static void CompileAsync()
        {
            ThreadPool.QueueUserWorkItem((state) => Compile());
        }

        private static void Compile()
        {
            try
            {
                OnCompilerStarted();

                //lock (SynchronizeLock)
                //    ScriptPool.Clear();

                string[] files = Directory.GetFiles(ScriptFolder, "*.cs", SearchOption.AllDirectories);
                Log.WriteLine("Found {0} files in Scripts folder", files.Length);

                var parameters = new CompilerParameters()
                {
                    CompilerOptions = string.Format("/lib:\"{0}\"", AppDomain.CurrentDomain.BaseDirectory),
                    GenerateExecutable = false,
                    GenerateInMemory = true,
                    IncludeDebugInformation = false,
                    OutputAssembly = "Scripts"
                };

                parameters.ReferencedAssemblies.Add("System.dll");
                parameters.ReferencedAssemblies.Add("System.Core.dll");
                //parameters.ReferencedAssemblies.Add("cleanLayer.dll");
                parameters.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);

                var provider = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v4.0" } });
                var results = provider.CompileAssemblyFromFile(parameters, files);

                var errors = results.Errors;

                Log.WriteLine("Errors:");
                foreach (CompilerError error in errors)
                    if (!error.IsWarning)
                        Log.WriteLine("\t({0}:{1}) {2}", error.Line, error.Column, error.ErrorText);
                Log.WriteLine("");

                Log.WriteLine("Warnings:");
                foreach (CompilerError warning in errors)
                    if (warning.IsWarning)
                        Log.WriteLine("\t({0}:{1}) {2}", warning.Line, warning.Column, warning.ErrorText);
                Log.WriteLine("");

                if (errors.HasErrors)
                {
                    Log.WriteLine("Compiler terminated due to errors");
                    return;
                }

                AnalyzeAssembly(results.CompiledAssembly);

            }
            catch (Exception ex)
            {
                Log.WriteLine("Compiler error: {0}", ex.Message);
            }
            finally
            {
                OnCompilerFinished();
            }
        }

        private static void AnalyzeAssembly(Assembly asm)
        {
            Log.WriteLine("Analyzing compiled assembly {0}", asm.FullName);

            var types = asm.GetTypes();
            Log.WriteLine("Found {0} types", types.Length);
            lock (SynchronizeLock)
            {
                foreach (var type in types)
                {
                    if (!type.IsClass || !type.IsSubclassOf(typeof(Script)))
                    {
                        Log.WriteLine("Ignoring {0}", type.Name);
                        continue;
                    }

                    RegistrationQueue.Enqueue(type);
                }
            }
        }

        private static void Register(Type type)
        {
            //Log.WriteLine("Registering {0} as script", type.Name);
            try
            {
                var ctor = type.GetConstructor(new Type[] { });
                if (ctor == null)
                    throw new Exception("Constructor not found");

                var script = (Script)ctor.Invoke(new object[] { });
                if (script == null)
                    throw new Exception("Unable to instantiate script");

                Scripts.Add(script);
                OnScriptRegistered(script);
            }
            catch (Exception ex)
            {
                Log.WriteLine("Failed to compile: {0}", ex.Message);
            }
        }

        private static void OnCompilerStarted()
        {
            if (CompilerStarted != null)
                CompilerStarted(null, new EventArgs());
        }

        private static void OnCompilerFinished()
        {
            if (CompilerFinished != null)
                CompilerFinished(null, new EventArgs());
        }

        private static void OnScriptRegistered(Script script)
        {
            if (ScriptRegistered != null)
                ScriptRegistered(script, new EventArgs());
        }

        public static event EventHandler CompilerStarted;
        public static event EventHandler CompilerFinished;
        public static event EventHandler ScriptRegistered;
    }
}
