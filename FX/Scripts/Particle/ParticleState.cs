using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension.FX.Scripts.Particle
{
    class ParticleState : FXScript
    {
        public ParticleState(FXSystem system, FXEmitter particle) : base(system, particle)
        {
        }

        // Input Parameters

        public bool DelayFirstLoopOnly { get; set; }
        public int LoopCount { get; set; }
        public float LoopDelay { get; set; }
        public float LoopDuration { get; set; }
        public bool RecalculateDurationEachLoop { get; set; }

        public override FXScript Clone()
        {
            var state = new ParticleState(System, Emitter);
            state.DelayFirstLoopOnly = DelayFirstLoopOnly;
            state.LoopCount = LoopCount;
            state.LoopDelay = LoopDelay;
            state.LoopDuration = LoopDuration;
            state.RecalculateDurationEachLoop = RecalculateDurationEachLoop;
            return state;
        }

        public override void ParticleUpdate(FXParticle particle)
        {
            bool loopCountIncreased;
            // DELAY: Copy first round of loop duration and delay into LoopedAge, CurrentLoopDuration, and CurrentLoopDelay
            if (particle.Age == 0)
            {
                particle.LoopedAge = -LoopDelay;
                particle.CurrentLoopDuration = Math.Max(LoopDuration, FXEngine.DeltaTime);
                particle.CurrentLoopDelay = LoopDelay;
            }

            if (LoopCount > 1)
            {
                // If LoopedAge > LoopDuration then increment loop count and store the remainder in LoopedAge. 
                // The particle is still delayed if LoopedAge < 0.0.
                particle.Age += FXEngine.DeltaTime;

                var nextLoopedAge = particle.LoopedAge + FXEngine.DeltaTime;
                loopCountIncreased = Math.Max((int)(nextLoopedAge / particle.CurrentLoopDuration), 0) > 0;

                if (loopCountIncreased)
                {
                    particle.LoopCount++;
                    particle.LoopedAge = 0;
                }
                else
                {
                    particle.LoopedAge = nextLoopedAge;
                }
            }
            else
            {
                // Loop Once behavior, feed the looped age variable for stack behavior consistency
                particle.Age += FXEngine.DeltaTime;
                particle.LoopedAge += FXEngine.DeltaTime;
                loopCountIncreased = particle.LoopedAge >= particle.CurrentLoopDuration;
            }


            if (loopCountIncreased)
            {
                if (LoopCount > 1)
                {
                    // DELAY: If the loop count really did go up, we need to factor in delays, decide on the new loop variables
                    if (RecalculateDurationEachLoop)
                    {
                        particle.CurrentLoopDuration = LoopDuration;
                    }
                    particle.CurrentLoopDelay = DelayFirstLoopOnly ? 0 : LoopDelay;
                    particle.LoopedAge -= particle.CurrentLoopDelay;
                }
                else
                {
                    // LOOP ONCE Age variables
                    particle.CurrentLoopDuration = LoopDuration;
                    particle.LoopedAge = 0;

                }
            }

            particle.NormalizedLoopedAge = particle.LoopedAge / particle.CurrentLoopDuration;

            if (particle.LoopCount >= LoopCount)
            {
                particle.Completed = true;
            }
        }
    }
}
