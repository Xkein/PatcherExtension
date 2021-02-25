using Extension.Utilities;
using PatcherYRpp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension.Ext
{
    public partial class TechnoExt
    {
        public string MyExtensionTest = nameof(MyExtensionTest);
    }

    [INILoadAction("LoadINI")]
    public partial class TechnoTypeExt
    {
        [NonSerialized]
        public Pointer<SuperWeaponTypeClass> FireSuperWeapon;


        public void LoadINI(Pointer<CCINIClass> pINI)
        {
            INIReader reader = new INIReader(pINI);
            string section = OwnerObject.Ref.Base.Base.GetID();

            reader.ReadSuperWeapon(section, nameof(FireSuperWeapon), ref FireSuperWeapon);
        }
    }
}
