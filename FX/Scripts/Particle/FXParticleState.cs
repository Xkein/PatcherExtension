﻿using Extension.FX.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension.FX.Scripts.Particle
{
    public class FXParticleState : FXScript
    {
        public FXParticleState(FXSystem system, FXEmitter emitter) : base(system, emitter)
        {
        }

        // Input Parameters

        public bool KillParticlesWhenLifetimeHasElapsed { get; set; } = true;
        public bool LoopParticlesLifetime { get; set; } = false;
        public bool LetInfinitelyLivedParticlesDieWhenEmitterDeactivates { get; set; } = false;
        // default to FXParticle.Lifetime
        public float? Lifetime { get; set; }
        public float DeltaTime { get; set; } = FXEngine.DeltaTime;

        public override FXScript Clone()
        {
            var state = new FXParticleState(System, Emitter);
            state.KillParticlesWhenLifetimeHasElapsed = KillParticlesWhenLifetimeHasElapsed;
            state.Lifetime = Lifetime;
            state.DeltaTime = DeltaTime;

            state.Alive = Alive;

            return state;
        }

        public override void ParticleUpdate(FXParticle particle)
        {
            bool shouldInactive = System.ExecutionState != FXExecutionState.Active || Emitter.ExecutionState != FXExecutionState.Active;
            shouldInactive &= LetInfinitelyLivedParticlesDieWhenEmitterDeactivates;

            var nextAge = particle.Age + DeltaTime;
            var safeLifetime = Math.Max(Lifetime.GetValueOrDefault(particle.Lifetime), 0.00001f);
            var safeLifetime_smaller = safeLifetime - 0.0001f;

            if (KillParticlesWhenLifetimeHasElapsed)
            {
                particle.Age = nextAge;

                if(nextAge >= safeLifetime_smaller)
                {
                    Alive = false;
                }
            }
            else
            {
                // UpdateAge for infinitely lived particles

                if (LoopParticlesLifetime && !shouldInactive)
                {
                    particle.Age = nextAge % safeLifetime;
                }
                else
                {
                    particle.Age = nextAge;
                }

                if (particle.Age > safeLifetime_smaller && shouldInactive)
                {
                    Alive = false;
                }
            }

            particle.NormalizedAge = particle.Age / safeLifetime;
        }

        // Unique Attribute

        public bool Alive { get; private set; } = true;
    }
}
