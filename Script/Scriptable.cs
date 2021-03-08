using Extension.Ext;
using Extension.Utilities;
using PatcherYRpp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Extension.Script
{
    public interface IAbstractScriptable
    {
        public void OnUpdate();
    }
    public interface IObjectScriptable : IAbstractScriptable
    {
        public void OnPut(CoordStruct coord, int faceDir);
        public void OnRemove();
        public void OnReceiveDamage(int Damage, int DistanceFromEpicenter, Pointer<WarheadTypeClass> pWH,
            Pointer<ObjectClass> pAttacker, bool IgnoreDefenses, bool PreventPassengerEscape, Pointer<HouseClass> pAttackingHouse);
    }
    public interface ITechnoScriptable : IObjectScriptable
    {
        public void OnFire(Pointer<AbstractClass> pTarget, int weaponIndex);
    }

    public interface IScriptable : IReloadable
    {
    }

    [Serializable]
    public abstract class Scriptable<T> : IScriptable
    {
        public T Owner { get; protected set; }
        public Scriptable(T owner)
        {
            Owner = owner;
        }
        public virtual void SaveToStream(IStream stream) { }
        public virtual void LoadFromStream(IStream stream) { }
    }

    [Serializable]
    public class TechnoScriptable : Scriptable<TechnoExt>, ITechnoScriptable
    {
        public TechnoScriptable(TechnoExt owner) : base(owner)
        {
        }

        public virtual void OnUpdate() { }
        public virtual void OnPut(CoordStruct coord, int faceDir) { }
        public virtual void OnRemove() { }
        public virtual void OnReceiveDamage(int Damage, int DistanceFromEpicenter, Pointer<WarheadTypeClass> pWH,
            Pointer<ObjectClass> pAttacker, bool IgnoreDefenses, bool PreventPassengerEscape, Pointer<HouseClass> pAttackingHouse)
        { }

        public virtual void OnFire(Pointer<AbstractClass> pTarget, int weaponIndex) { }
    }

    [Serializable]
    public class BulletScriptable : Scriptable<BulletExt>, IObjectScriptable
    {
        public BulletScriptable(BulletExt owner) : base(owner)
        {
        }

        public virtual void OnUpdate() { }
        public virtual void OnPut(CoordStruct coord, int faceDir) { }
        public virtual void OnRemove() { }
        public virtual void OnReceiveDamage(int Damage, int DistanceFromEpicenter, Pointer<WarheadTypeClass> pWH,
            Pointer<ObjectClass> pAttacker, bool IgnoreDefenses, bool PreventPassengerEscape, Pointer<HouseClass> pAttackingHouse)
        { }
    }
}
