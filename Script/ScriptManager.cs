using DynamicPatcher;
using Extension.Ext;
using PatcherYRpp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Extension.Script
{
    public class ScriptManager
    {
        static Dictionary<string, Script> Scripts = new Dictionary<string, Script>();

        // create script or get a exist script
        static public TScript GetScript<TScript>(string filename) where TScript : Script
        {
            if (Scripts.ContainsKey(filename))
            {
                return Scripts[filename] as TScript;
            }
            else
            {
                TScript script = Activator.CreateInstance(typeof(TScript), filename) as TScript;

                var pair = Program.Patcher.FileAssembly.First((pair) => Path.GetFileNameWithoutExtension(pair.Key) == filename);
                Assembly assembly = pair.Value;

                RefreshScript(script, assembly);

                Scripts.Add(filename, script);
                return script;
            }
        }

        static public Scriptable<T> GetScriptable<T>(Script script, T owner)
        {
            var scriptable = Activator.CreateInstance(script.ScriptableType, owner) as Scriptable<T>;
            scriptable.Script = script;
            return scriptable;
        }

        private static void RefreshScript(Script script, Assembly assembly)
        {
            Type[] types = assembly.GetTypes();
            foreach (Type type in types)
            {
                if (typeof(IScriptable).IsAssignableFrom(type))
                {
                    script.SetEvents(type);
                    break;
                }
            }
        }


        private static void Patcher_AssemblyRefresh(object sender, AssemblyRefreshEventArgs args)
        {
            if (Scripts.ContainsKey(args.FileName))
            {
                RefreshScript(Scripts[args.FileName], args.RefreshedAssembly);

                // [warning!] unsafe change to scriptable
                unsafe
                {
                    ref var technoArray = ref TechnoClass.Array;
                    for (int i = 0; i < technoArray.Count; i++)
                    {
                        var pItem = technoArray[i];
                        var ext = TechnoExt.ExtMap.Find(pItem);
                        if (ext.Scriptable != null)
                        {
                            var extType = ext.Type;
                            ext.scriptable = new Lazy<TechnoScriptable>(() => GetScriptable(extType.Script, ext) as TechnoScriptable);
                        }
                    }

                    ref var bulletArray = ref BulletClass.Array;
                    for (int i = 0; i < bulletArray.Count; i++)
                    {
                        var pItem = bulletArray[i];
                        var ext = BulletExt.ExtMap.Find(pItem);
                        if (ext.Scriptable != null)
                        {
                            var extType = ext.Type;
                            ext.scriptable = new Lazy<BulletScriptable>(() => GetScriptable(extType.Script, ext) as BulletScriptable);
                        }
                    }
                }
            }
        }

        static ScriptManager()
        {
            Program.Patcher.AssemblyRefresh += Patcher_AssemblyRefresh;
        }


        //[Hook(HookType.AresHook, Address = 0x6F9E50, Size = 5)]
        static public unsafe UInt32 TechnoClass_Update_Script(REGISTERS* R)
        {
            Pointer<TechnoClass> pTechno = (IntPtr)R->ESI;
            TechnoExt ext = TechnoExt.ExtMap.Find(pTechno);

            ext.Scriptable?.Invoke(ScriptEventType.OnUpdate, null);

            return 0;
        }

        //[Hook(HookType.AresHook, Address = 0x4666F7, Size = 6)]
        static public unsafe UInt32 BulletClass_Update_Script(REGISTERS* R)
        {
            Pointer<BulletClass> pBullet = (IntPtr)R->EBP;
            BulletExt ext = BulletExt.ExtMap.Find(pBullet);

            ext.Scriptable?.Invoke(ScriptEventType.OnUpdate, null);

            return 0;
        }
    }
}
