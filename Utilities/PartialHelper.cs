using Extension.Ext;
using PatcherYRpp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

    static class PartialHelper
    {
        public static void LoadINIConfig<T>(this Extension<T> ext, Pointer<CCINIClass> pINI)
        {
            Type type = ext.GetType();
            INILoadActionAttribute[] iniLoadActions = type.GetCustomAttributes(typeof(INILoadActionAttribute), false) as INILoadActionAttribute[];
            foreach (var iniLoadAction in iniLoadActions)
            {
                MethodInfo method = type.GetMethod(iniLoadAction.Name);
                method.Invoke(ext, new object[] { pINI });
            }
        }
    }
}
