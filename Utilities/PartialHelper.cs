using Extension.Ext;
using PatcherYRpp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Extension.Utilities
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    sealed class INILoadActionAttribute : Attribute
    {
        public INILoadActionAttribute(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
    }

    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    sealed class SaveActionAttribute : Attribute
    {
        public SaveActionAttribute(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
    }

    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    sealed class LoadActionAttribute : Attribute
    {
        public LoadActionAttribute(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
    }

    static class PartialHelper
    {
        public static void PartialLoadINIConfig<T>(this Extension<T> ext, Pointer<CCINIClass> pINI)
        {
            Type type = ext.GetType();
            INILoadActionAttribute[] iniLoadActions = type.GetCustomAttributes(typeof(INILoadActionAttribute), false) as INILoadActionAttribute[];
            foreach (var iniLoadAction in iniLoadActions)
            {
                MethodInfo method = type.GetMethod(iniLoadAction.Name);
                method.Invoke(ext, new object[] { pINI });
            }
        }
        public static void PartialSaveToStream<T>(this Extension<T> ext, IStream stream)
        {
            Type type = ext.GetType();
            SaveActionAttribute[] saveActions = type.GetCustomAttributes(typeof(SaveActionAttribute), false) as SaveActionAttribute[];
            foreach (var saveAction in saveActions)
            {
                MethodInfo method = type.GetMethod(saveAction.Name);
                method.Invoke(ext, new object[] { stream });
            }
        }
        public static void PartialLoadFromStream<T>(this Extension<T> ext, IStream stream)
        {
            Type type = ext.GetType();
            LoadActionAttribute[] loadActions = type.GetCustomAttributes(typeof(LoadActionAttribute), false) as LoadActionAttribute[];
            foreach (var loadAction in loadActions)
            {
                MethodInfo method = type.GetMethod(loadAction.Name);
                method.Invoke(ext, new object[] { stream });
            }
        }
    }
}
