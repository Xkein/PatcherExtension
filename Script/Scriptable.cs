using Extension.Ext;
using PatcherYRpp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Extension.Script
{
    public interface IScriptable
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
        public virtual void Save(IStream stream) { }
        public virtual void Load(IStream stream) { }
    }

    [Serializable]
    public class TechnoScriptable : Scriptable<TechnoExt>
    {
        public TechnoScriptable(TechnoExt owner) : base(owner)
        {
        }

        public virtual void OnUpdate() { }
        public virtual void OnFire(Pointer<AbstractClass> pTarget, int weaponIndex) { }
    }

    [Serializable]
    public class BulletScriptable : Scriptable<BulletExt>
    {
        public BulletScriptable(BulletExt owner) : base(owner)
        {
        }

        public virtual void OnUpdate() { }
    }
}
