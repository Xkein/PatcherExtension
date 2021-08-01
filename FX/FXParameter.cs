using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension.FX
{
    public abstract class FXParameter : ICloneable
    {
        protected FXParameter(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public abstract object Value { get; set; }

        public abstract FXParameter Clone();

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
