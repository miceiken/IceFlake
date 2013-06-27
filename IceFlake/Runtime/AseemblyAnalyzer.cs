using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using IceFlake.DirectX;

namespace IceFlake.Runtime
{
    public static class AssemblyAnalyzer
    {
        private static readonly Dictionary<Type, object> targets = new Dictionary<Type, object>();

        public static void Analyze(Assembly assembly)
        {
            var types = assembly.GetTypes();

            foreach (var type in types)
                AnalyzeType(type);
        }

        public static void AnalyzeType(Type type)
        {
            AnalyzeAttributes(type, type.GetCustomAttributes(true));

            foreach (
                MethodInfo method in
                    type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                AnalyzeAttributes(method, method.GetCustomAttributes(true));

            foreach (
                PropertyInfo property in
                    type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                AnalyzeAttributes(property, property.GetCustomAttributes(true));
        }

        private static void AnalyzeAttributes(MemberInfo info, object[] attributes)
        {
            foreach (object item in attributes)
            {
                if (item is EndSceneHandler)
                {
                    object target = info is Type ? GetTarget(info as Type) : GetTarget(info.DeclaringType);
                    if (target == null)
                        continue;

                    Direct3D.CallbackManager.Register(
                        (EndSceneCallback)Delegate.CreateDelegate(typeof(EndSceneCallback), target, info.Name));
                }
            }
        }

        public static object GetTarget(Type type)
        {
            if (targets.ContainsKey(type))
                return targets[type];

            object target = null;
            try
            {
                target = Activator.CreateInstance(type);
                targets.Add(type, target);
            }
            catch (Exception e)
            {
                Log.WriteLine(e.ToLongString());
            }

            return target;
        }

        public static void RegisterTarget(object target)
        {
            targets.Add(target.GetType(), target);
        }

        public static void RegisterTargets(params object[] targets)
        {
            foreach (var target in targets)
                RegisterTarget(target);
        }
    }
}
