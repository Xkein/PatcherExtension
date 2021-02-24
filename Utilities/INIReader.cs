using PatcherYRpp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension.Utilities
{
    class INIReader
    {
        INI_EX parser;

        public INIReader(INI_EX exINI)
        {
            parser = exINI;
        }

        public INIReader(Pointer<CCINIClass> pINI)
        {
            parser = new INI_EX(pINI);
        }

        //public T Read<T>(string section, string key)
        //{
        //    T buffer = default;

        //    switch (buffer)
        //    {
        //        case string:
        //        case bool:
        //        case int:
        //        case byte:
        //        case float:
        //        case double:
        //            return ReadNormal<T>(section, key);
        //        case Pointer<SuperWeaponTypeClass>:
        //            return ReadSuperWeapon(section, key);
        //        default:
        //            break;
        //    }
        //}

        public T ReadNormal<T>(string section, string key)
        {
            T buffer = default;
            parser.Read(section, key, ref buffer);
            return buffer;
        }

        public Pointer<SuperWeaponTypeClass> ReadSuperWeapon(string section, string key)
        {
            string val = ReadNormal<string>(section, key);
            return SuperWeaponTypeClass.ABSTRACTTYPE_ARRAY.Find(val);
        }
    }
}
