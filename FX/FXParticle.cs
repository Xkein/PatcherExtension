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

        public Vector3 Location { get; set; }
        public Vector3 Velocity { get; set; }


        public float Age { get; set; }
        public float CurrentLoopDelay { get; set; }
        public float CurrentLoopDuration { get; set; }
        public int LoopCount { get; set; }
        public float LoopedAge { get; set; }
        public float NormalizedLoopedAge { get; set; }
        public bool Completed { get; set; }

        public virtual FXParticle Clone()
        {
            FXParticle particle = new FXParticle();

            particle.Location = this.Location;
            particle.Velocity = this.Velocity;

            particle.Age = Age;
            particle.CurrentLoopDelay = CurrentLoopDelay;
            particle.CurrentLoopDuration = CurrentLoopDuration;
            particle.LoopCount = LoopCount;
            particle.LoopedAge = LoopedAge;
            particle.NormalizedLoopedAge = NormalizedLoopedAge;
            particle.Completed = Completed;

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
