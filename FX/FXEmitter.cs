using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension.FX
{
    public class FXEmitter : ICloneable
    {
        public FXEmitter(FXSystem system, FXModule mEmitterSpawn, FXModule mEmitterUpdate, FXModule mParticleSpawn, FXModule mParticleUpdate, FXModule mRender, List<FXParticle> particles)
        {
            System = system;
            MEmitterSpawn = mEmitterSpawn;
            MEmitterUpdate = mEmitterUpdate;
            MParticleSpawn = mParticleSpawn;
            MParticleUpdate = mParticleUpdate;
            MRender = mRender;

            Particles = particles;
        }

        // Variables

        public FXSystem System { get; }

        public List<FXParticle> Particles { get; }

        // Modules

        public FXModule MEmitterSpawn { get; }
        public FXModule MEmitterUpdate { get; }
        public FXModule MParticleSpawn { get; }
        public FXModule MParticleUpdate { get; }
        public FXModule MRender { get; }

        // Parameters

        public float Age { get; set; }
        public float CurrentLoopDelay { get; set; }
        public float CurrentLoopDuration { get; set; }
        public int LoopCount { get; set; }
        public float LoopedAge { get; set; }
        public float NormalizedLoopedAge { get; set; }
        public bool Completed { get; set; }

        public virtual FXEmitter Clone()
        {
             var emitter = new FXEmitter(
                System,
                MEmitterSpawn.Clone(),
                MEmitterUpdate.Clone(),
                MParticleSpawn.Clone(),
                MParticleUpdate.Clone(),
                MRender.Clone(),
                (from p in Particles select p.Clone()).ToList()
                );

            emitter.Age = Age;
            emitter.CurrentLoopDelay = CurrentLoopDelay;
            emitter.CurrentLoopDuration = CurrentLoopDuration;
            emitter.LoopCount = LoopCount;
            emitter.LoopedAge = LoopedAge;
            emitter.NormalizedLoopedAge = NormalizedLoopedAge;
            emitter.Completed = Completed;

            return emitter;
        }

        public virtual void Spawn()
        {
            foreach (var script in MEmitterSpawn.Scripts)
            {
                script.SystemSpawn();
            }
        }

        public virtual void Update()
        {
            if (Completed)
            {
                return;
            }

            foreach (var script in MEmitterUpdate.Scripts)
            {
                script.SystemUpdate();
            }
        }

        public virtual void Render()
        {
            if (Completed)
            {
                return;
            }

            foreach (var script in MRender.Scripts)
            {
                var render = script as FXRender;
                foreach (var particle in Particles.AsParallel())
                {
                    render.ParticleRender(particle);
                }
            }
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
