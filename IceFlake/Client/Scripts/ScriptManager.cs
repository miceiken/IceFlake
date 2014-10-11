using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using IceFlake.DirectX;
using Microsoft.CSharp;

namespace IceFlake.Client.Scripts
{
    public class ScriptManager : IPulsable
    {
        public ScriptManager()
        {
            Scripts = new List<Script>();

            ScriptFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts");
            CompileInternal();
        }

        public string ScriptFolder { get; private set; }

        public List<Script> Scripts { get; private set; }

        private Script CurrentScript { get; set; }

        public void Direct3D_EndScene()
        {
            foreach (Script script in Scripts)
            {
                CurrentScript = script;
                script.Tick();
            }

            CurrentScript = null;
        }

        public void CompileAsync()
        {
            ThreadPool.QueueUserWorkItem(state => CompileExternal());
        }

        private void CompileInternal()
        {
            OnCompilerStarted();

            Scripts.AddRange(
                Assembly.GetExecutingAssembly().GetTypes()
                    .Where(t => t.IsSubclassOf(typeof (Script)))
                    .Select(s => Register(s))
                    .Where(s => s != null)
                );

            OnCompilerFinished();
        }

        private void CompileExternal()
        {
            try
            {
                OnCompilerStarted();

                string[] files = Directory.GetFiles(ScriptFolder, "*.cs", SearchOption.AllDirectories);
                Log.WriteLine("Found {0} files in Scripts folder", files.Length);

                var parameters = new CompilerParameters
                {
                    CompilerOptions = string.Format("/lib:\"{0}\"", AppDomain.CurrentDomain.BaseDirectory),
                    GenerateExecutable = false,
                    GenerateInMemory = true,
                    IncludeDebugInformation = false,
                    OutputAssembly = "Scripts"
                };

                IEnumerable<string> assemblies =
                    AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic).Select(a => a.Location);
                parameters.ReferencedAssemblies.AddRange(assemblies.ToArray());
                parameters.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);

                var provider = new CSharpCodeProvider(new Dictionary<string, string> {{"CompilerVersion", "v4.0"}});
                CompilerResults results = provider.CompileAssemblyFromFile(parameters, files);

                CompilerErrorCollection errors = results.Errors;

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

        private void AnalyzeAssembly(Assembly asm)
        {
            Log.WriteLine("Analyzing compiled assembly {0}", asm.FullName);

            Type[] types = asm.GetTypes();

            Log.WriteLine("Found {0} types", types.Length);
            foreach (Type type in types)
            {
                if (!type.IsClass || !type.IsSubclassOf(typeof (Script)))
                {
                    //Log.WriteLine("Ignoring {0}", type.Name);
                    continue;
                }

                Scripts.Add(Register(type));
            }
        }

        private Script Register(Type type)
        {
            try
            {
                ConstructorInfo ctor = type.GetConstructor(new Type[] {});
                if (ctor == null)
                    throw new Exception("Constructor not found");

                var script = (Script) ctor.Invoke(new object[] {});
                if (script == null)
                    throw new Exception("Unable to instantiate script");

                OnScriptRegistered(script);
                return script;
            }
            catch (Exception ex)
            {
                Log.WriteLine("Failed to compile: {0}", ex.Message);
            }
            return null;
        }

        private void OnCompilerStarted()
        {
            if (CompilerStarted != null)
                CompilerStarted(null, new EventArgs());
        }

        private void OnCompilerFinished()
        {
            if (CompilerFinished != null)
                CompilerFinished(null, new EventArgs());
        }

        private void OnScriptRegistered(Script script)
        {
            if (ScriptRegistered != null)
                ScriptRegistered(script, new EventArgs());
        }

        public event EventHandler CompilerStarted;
        public event EventHandler CompilerFinished;
        public event EventHandler ScriptRegistered;
    }
}