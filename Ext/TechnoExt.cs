﻿using DynamicPatcher;
using Extension.Script;
using PatcherYRpp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Extension.Ext
{
    [Serializable]
    public partial class TechnoExt : Extension<TechnoClass>
    {
        public static Container<TechnoExt, TechnoClass> ExtMap = new Container<TechnoExt, TechnoClass>("TechnoClass");

        // Lazy<TechnoScriptable> scriptable;
        internal TechnoScriptable scriptable;
        public TechnoScriptable Scriptable
        {
            get
            {
                if (scriptable == null)
                {
                    if (Type.Script != null)
                    {
                        scriptable = ScriptManager.GetScriptable(Type.Script, this) as TechnoScriptable;
                    }
                }
                return scriptable;
            }
        }

        public TechnoTypeExt Type { get => TechnoTypeExt.ExtMap.Find(OwnerObject.Ref.Type); }

        public TechnoExt(Pointer<TechnoClass> OwnerObject) : base(OwnerObject)
        {
        }

        //[Hook(HookType.AresHook, Address = 0x6F3260, Size = 5)]
        static public unsafe UInt32 TechnoClass_CTOR(REGISTERS* R)
        {
            var pItem = (Pointer<TechnoClass>)R->ESI;

            TechnoExt.ExtMap.FindOrAllocate(pItem);
            return 0;
        }

        //[Hook(HookType.AresHook, Address = 0x6F4500, Size = 5)]
        static public unsafe UInt32 TechnoClass_DTOR(REGISTERS* R)
        {
            var pItem = (Pointer<TechnoClass>)R->ECX;

            TechnoExt.ExtMap.Remove(pItem);
            return 0;
        }

        //[Hook(HookType.AresHook, Address = 0x70C250, Size = 8)]
        //[Hook(HookType.AresHook, Address = 0x70BF50, Size = 5)]
        static public unsafe UInt32 TechnoClass_SaveLoad_Prefix(REGISTERS* R)
        {
            var pItem = R->Stack<Pointer<TechnoClass>>(0x4);
            var pStm = R->Stack<Pointer<IStream>>(0x8);
            IStream stream = Marshal.GetObjectForIUnknown(pStm) as IStream;

            TechnoExt.ExtMap.PrepareStream(pItem, stream);
            return 0;
        }

        //[Hook(HookType.AresHook, Address = 0x70C249, Size = 5)]
        static public unsafe UInt32 TechnoClass_Load_Suffix(REGISTERS* R)
        {
            TechnoExt.ExtMap.LoadStatic();
            return 0;
        }

        //[Hook(HookType.AresHook, Address = 0x70C264, Size = 5)]
        static public unsafe UInt32 TechnoClass_Save_Suffix(REGISTERS* R)
        {
            TechnoExt.ExtMap.SaveStatic();
            return 0;
        }
    }

}
