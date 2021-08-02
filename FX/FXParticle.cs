using Extension.FX.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension.FX
{
    public class FXParticle : ICloneable
    {
        // Parameters

        public Vector3 Position { get; set; }
        public Vector3 Velocity { get; set; }


        public float Age { get; set; }
        public float Lifetime { get; set; }
        public float NormalizedAge { get; set; }
        public float Mass { get; set; }

        // Data 

        public bool Alive { get; set; } = true;

        // Transient
        public Vector3 PhysicsForce;
        public float PhysicsDrag;

        public virtual FXParticle Clone()
        {
            FXParticle particle = new FXParticle();

            particle.Position = this.Position;
            particle.Velocity = this.Velocity;

            particle.Age = Age;
            particle.Lifetime = Lifetime;
            particle.NormalizedAge = NormalizedAge;
            particle.Mass = Mass;

            particle.Alive = Alive;

            return particle;
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
