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
            if (Scripts.TryGetValue(filename, out Script script))
            {
                return script as TScript;
            }
            else
            {
                TScript newScript = Activator.CreateInstance(typeof(TScript), filename) as TScript;
                try
                {
                    var pair = Program.Patcher.FileAssembly.First((pair) => Path.GetFileNameWithoutExtension(pair.Key) == filename);
                    Assembly assembly = pair.Value;

                    RefreshScript(newScript, assembly);

                    Scripts.Add(filename, newScript);
                    return newScript;
                }
                catch (Exception e)
                {
                    Logger.Log("ScriptManager could not find script: {0}", filename);
                    Helpers.PrintException(e);
                    return null;
                }
            }
        }

        static public Scriptable<T> GetScriptable<T>(Script script, T owner)
        {
            return script != null ? GetScriptable(script.ScriptableType, owner) : null;
        }

        static public Scriptable<T> GetScriptable<T>(Type scriptableType, T owner)
        {
            var scriptable = Activator.CreateInstance(scriptableType, owner) as Scriptable<T>;
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
            if (Scripts.TryGetValue(args.FileName, out Script script))
            {
                RefreshScript(script, args.RefreshedAssembly);

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

        #region TechnoScript
        //[Hook(HookType.AresHook, Address = 0x6F9E50, Size = 5)]
        static public unsafe UInt32 TechnoClass_Update_Script(REGISTERS* R)
        {
            Pointer<TechnoClass> pTechno = (IntPtr)R->ESI;

            TechnoExt ext = TechnoExt.ExtMap.Find(pTechno);
            ext.Scriptable?.OnUpdate();

            return 0;
        }
        //[Hook(HookType.AresHook, Address = 0x6F6CA0, Size = 7)]
        static public unsafe UInt32 TechnoClass_Put_Script(REGISTERS* R)
        {
            Pointer<TechnoClass> pTechno = (IntPtr)R->ECX;
            var pCoord = R->Stack<Pointer<CoordStruct>>(0x4);
            var faceDir = R->Stack<int>(0x8);

            TechnoExt ext = TechnoExt.ExtMap.Find(pTechno);
            ext.Scriptable?.OnPut(pCoord.Data, faceDir);

            return 0;
        }
        //[Hook(HookType.AresHook, Address = 0x6F6AC0, Size = 5)]
        static public unsafe UInt32 TechnoClass_Remove_Script(REGISTERS* R)
        {
            Pointer<TechnoClass> pTechno = (IntPtr)R->ECX;

            TechnoExt ext = TechnoExt.ExtMap.Find(pTechno);
            ext.Scriptable?.OnRemove();

            return 0;
        }
        //[Hook(HookType.AresHook, Address = 0x701900, Size = 6)]
        static public unsafe UInt32 TechnoClass_ReceiveDamage_Script(REGISTERS* R)
        {
            Pointer<TechnoClass> pTechno = (IntPtr)R->ECX;
            var pDamage = R->Stack<Pointer<int>>(0x4);
            var distanceFromEpicenter = R->Stack<int>(0x8);
            var pWH = R->Stack<Pointer<WarheadTypeClass>>(0xC);
            var pAttacker = R->Stack<Pointer<ObjectClass>>(0x10);
            var ignoreDefenses = R->Stack<bool>(0x14);
            var preventPassengerEscape = R->Stack<bool>(0x18);
            var pAttackingHouse = R->Stack<Pointer<HouseClass>>(0x1C);

            TechnoExt ext = TechnoExt.ExtMap.Find(pTechno);
            ext.Scriptable?.OnReceiveDamage(pDamage.Data, distanceFromEpicenter, pWH, pAttacker, ignoreDefenses, preventPassengerEscape, pAttackingHouse);

            return 0;
        }
        //[Hook(HookType.AresHook, Address = 0x6FDD50, Size = 6)]
        static public unsafe UInt32 TechnoClass_Fire_Script(REGISTERS* R)
        {
            Pointer<TechnoClass> pTechno = (IntPtr)R->ECX;
            var pTarget = R->Stack<Pointer<AbstractClass>>(0x4);
            var nWeaponIndex = R->Stack<int>(0x8);

            TechnoExt ext = TechnoExt.ExtMap.Find(pTechno);
            ext.Scriptable?.OnFire(pTarget, nWeaponIndex);

            return 0;
        }
        #endregion

        #region BulletScript
        //[Hook(HookType.AresHook, Address = 0x4666F7, Size = 6)]
        static public unsafe UInt32 BulletClass_Update_Script(REGISTERS* R)
        {
            Pointer<BulletClass> pBullet = (IntPtr)R->EBP;

            BulletExt ext = BulletExt.ExtMap.Find(pBullet);
            ext.Scriptable?.OnUpdate();

            return 0;
        }
        #endregion
    }
}
