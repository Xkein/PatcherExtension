using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension.FX
{
    public interface IFXParameter : ICloneable
    {
        public string Name { get; }
        public object Value { get; set; }
    }

    public class FXParameter<T> : IFXParameter
    {
        public FXParameter(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public virtual T Value { get; set; }

        object IFXParameter.Value { get => Value; set => Value = (T)value; }

        public virtual FXParameter<T> Clone()
        {
            return new FXParameter<T>(Name) { Value = this.Value };
        }

        public override string ToString()
        {
            return GetType().FullName;
        }
        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
