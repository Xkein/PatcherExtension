using DynamicPatcher;
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

        internal Lazy<TechnoScriptable> scriptable;
        public TechnoScriptable Scriptable
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

        Lazy<TechnoTypeExt> type;
        public TechnoTypeExt Type { get => type.Value; }

        public TechnoExt(Pointer<TechnoClass> OwnerObject) : base(OwnerObject)
        {
            type = new Lazy<TechnoTypeExt>(() => TechnoTypeExt.ExtMap.Find(OwnerObject.Ref.Type));
            scriptable = new Lazy<TechnoScriptable>(() => ScriptManager.GetScriptable(Type.Script, this) as TechnoScriptable);
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
