using DynamicPatcher;
using Extension.Ext;
using PatcherYRpp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension.Script
{
    public partial class ScriptManager
    {
        //[Hook(HookType.AresHook, Address = 0x4666F7, Size = 6)]
        static public unsafe UInt32 BulletClass_Update_Script(REGISTERS* R)
        {
            Pointer<BulletClass> pBullet = (IntPtr)R->EBP;

            BulletExt ext = BulletExt.ExtMap.Find(pBullet);
            ext.Scriptable?.OnUpdate();

            return 0;
        }
    }
}
