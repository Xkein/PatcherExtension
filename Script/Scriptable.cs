using Extension.Ext;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public Script Script { get; internal set; }
        public Scriptable(T owner)
        {
            Owner = owner;
        }

        // this is slow, so use virtual function later
        public object Invoke(ScriptEventType scriptEventType, params object[] parameters)
        {
            return Script[scriptEventType]?.Invoke(this, parameters);
        }
    }

    [Serializable]
    public class TechnoScriptable : Scriptable<TechnoExt>
    {
        public TechnoScriptable(TechnoExt owner) : base(owner)
        {
        }
    }

    [Serializable]
    public class BulletScriptable : Scriptable<BulletExt>
    {
        public BulletScriptable(BulletExt owner) : base(owner)
        {
        }
    }
}
