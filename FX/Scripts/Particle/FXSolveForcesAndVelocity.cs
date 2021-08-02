using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension.FX.Scripts.Particle
{
    class FXSolveForcesAndVelocity : FXScript
    {
        public FXSolveForcesAndVelocity(FXSystem system, FXEmitter emitter) : base(system, emitter)
        {
        }

        public override FXScript Clone()
        {
            var solveForcesAndVelocity = new FXSolveForcesAndVelocity(System, Emitter);

            return solveForcesAndVelocity;
        }

        public override void ParticleUpdate(FXParticle particle)
        {

        }
    }
}
