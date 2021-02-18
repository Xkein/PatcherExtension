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
    public partial class BulletExt : Extension<BulletClass>
    {
        public static Container<BulletExt, BulletClass> ExtMap = new Container<BulletExt, BulletClass>("BulletClass");

        internal Lazy<BulletScriptable> scriptable;
        public BulletScriptable Scriptable
        {
            get
            {
                if (Type.Script != null)
                {
                    return scriptable.Value;
                }
                return null;
            }
        }

        Lazy<BulletTypeExt> type;
        public BulletTypeExt Type { get => type.Value; }

        public BulletExt(Pointer<BulletClass> OwnerObject) : base(OwnerObject)
        {
            type = new Lazy<BulletTypeExt>(() => BulletTypeExt.ExtMap.Find(OwnerObject.Ref.Type));
            scriptable = new Lazy<BulletScriptable>(() => ScriptManager.GetScriptable(Type.Script, this) as BulletScriptable);
        }

        //[Hook(HookType.AresHook, Address = 0x4664BA, Size = 5)]
        static public unsafe UInt32 BulletClass_CTOR(REGISTERS* R)
        {
            var pItem = (Pointer<BulletClass>)R->ESI;

            BulletExt.ExtMap.FindOrAllocate(pItem);
            return 0;
        }

        //[Hook(HookType.AresHook, Address = 0x4665E9, Size = 0xA)]
        static public unsafe UInt32 BulletClass_DTOR(REGISTERS* R)
        {
            var pItem = (Pointer<BulletClass>)R->ESI;

            BulletExt.ExtMap.Remove(pItem);
            return 0;
        }

        //[Hook(HookType.AresHook, Address = 0x46AFB0, Size = 8)]
        //[Hook(HookType.AresHook, Address = 0x46AE70, Size = 5)]
        static public unsafe UInt32 BulletClass_SaveLoad_Prefix(REGISTERS* R)
        {
            var pItem = R->Stack<Pointer<BulletClass>>(0x4);
            var pStm = R->Stack<Pointer<IStream>>(0x8);
            IStream stream = Marshal.GetObjectForIUnknown(pStm) as IStream;

            BulletExt.ExtMap.PrepareStream(pItem, stream);
            return 0;
        }

        //[Hook(HookType.AresHook, Address = 0x46AF97, Size = 7)]
        //[Hook(HookType.AresHook, Address = 0x46AF9E, Size = 7)]
        static public unsafe UInt32 BulletClass_Load_Suffix(REGISTERS* R)
        {
            BulletExt.ExtMap.LoadStatic();
            return 0;
        }

        //[Hook(HookType.AresHook, Address = 0x46AFC4, Size = 3)]
        static public unsafe UInt32 BulletClass_Save_Suffix(REGISTERS* R)
        {
            BulletExt.ExtMap.SaveStatic();
            return 0;
        }
    }
}
