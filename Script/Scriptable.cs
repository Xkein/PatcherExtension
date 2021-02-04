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
    public abstract class Scriptable<T> : IScriptable
    {
        public T Owner { get; protected set; }
        public Scriptable(T owner)
        {
            Owner = owner;
        }
    }
    public class TechnoScriptable : Scriptable<TechnoExt>
    {
        public TechnoScriptable(TechnoExt owner) : base(owner)
        {
        }
    }
}
