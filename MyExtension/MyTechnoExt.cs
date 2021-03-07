using Extension.Utilities;
using PatcherYRpp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Extension.Ext
{
    public partial class TechnoExt
    {
        public string MyExtensionTest = nameof(MyExtensionTest);
    }

    public partial class TechnoTypeExt
    {
        [NonSerialized]
        public PointerHandle<SuperWeaponTypeClass> FireSuperWeapon = new PointerHandle<SuperWeaponTypeClass>();


        [INILoadAction]
        public void LoadINI(Pointer<CCINIClass> pINI)
        {
            INIReader reader = new INIReader(pINI);
            string section = OwnerObject.Ref.Base.Base.GetID();

            reader.ReadSuperWeapon(section, nameof(FireSuperWeapon), ref FireSuperWeapon.Pointer);
        }

        [LoadAction]
        public void Load(IStream stream)
        {
            this.Load(stream, ref FireSuperWeapon);
        }
        [SaveAction]
        public void Save(IStream stream)
        {
            this.Save(stream, FireSuperWeapon);
        }
    }
}
