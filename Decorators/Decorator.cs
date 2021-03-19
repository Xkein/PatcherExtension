using PatcherYRpp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension.Decorators
{
    public abstract class Decorator
    {
        public string Description { get; set; }
        public DecoratorId ID { get; set; }
        public IDecorative Decorative { get; internal set; }
    }

    public abstract class PairDecorator : Decorator
    {
        public virtual object Key { get; set; }
        public virtual object Value { get; set; }
        internal PairDecorator(object key, object val)
        {
            Key = key;
            Value = val;
        }
    }

    public class PairDecorator<TKey, TValue> : PairDecorator
    {
        private ValueTuple<TKey, TValue> pair;
        internal PairDecorator(TKey key, TValue val) : base(key, val)
        {
        }
        public override object Key { get => pair.Item1; set => pair.Item1 = (TKey)value; }
        public override object Value { get => pair.Item2; set => pair.Item2 = (TValue)value; }
    }

    // simple version
    public abstract class EventDecorator : Decorator
    {
        public virtual void OnUpdate() { }
        public virtual void OnReceiveDamage(Pointer<int> pDamage, int DistanceFromEpicenter, Pointer<WarheadTypeClass> pWH,
            Pointer<ObjectClass> pAttacker, bool IgnoreDefenses, bool PreventPassengerEscape, Pointer<HouseClass> pAttackingHouse)
        { }
        public virtual void OnFire(Pointer<AbstractClass> pTarget, int weaponIndex) { }
    }
}
