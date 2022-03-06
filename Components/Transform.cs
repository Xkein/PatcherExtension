using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Extension.Components
{
    [Serializable]
    public class Transform : Component
    {
        public Transform() : base(true)
        {
            _transform = this;
        }

        public virtual Vector3 Location { get; set; }
        public virtual Vector3 Rotation { get; set; }
        public virtual Vector3 Scale { get; set; }

        public Transform GetParent()
        {
            return Parent.Parent.Transform;
        }

        public new Transform GetRoot()
        {
            return base.GetRoot().Transform;
        }
    }
}
