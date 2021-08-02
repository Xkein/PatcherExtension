using Extension.FX.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension.FX
{
    public class FXEmitter : ICloneable
    {
        public FXEmitter(FXSystem system, FXParticle prototype)
        {
            System = system;
            MEmitterSpawn = new FXModule(system, this);
            MEmitterUpdate = new FXModule(system, this);
            MParticleSpawn = new FXModule(system, this);
            MParticleUpdate = new FXModule(system, this);
            MRender = new FXModule(system, this);

            Prototype = prototype;

            Particles = new List<FXParticle>();
        }

        protected FXEmitter(FXSystem system, FXModule mEmitterSpawn, FXModule mEmitterUpdate, FXModule mParticleSpawn, FXModule mParticleUpdate, FXModule mRender, FXParticle prototype, List<FXParticle> particles)
        {
            System = system;
            MEmitterSpawn = mEmitterSpawn;
            MEmitterUpdate = mEmitterUpdate;
            MParticleSpawn = mParticleSpawn;
            MParticleUpdate = mParticleUpdate;
            MRender = mRender;

            Prototype = prototype;

            Particles = particles;
        }

        // Variables

        public FXSystem System { get; }

        public List<FXParticle> Particles { get; }

        public FXParticle Prototype { get; set; }

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
        public FXExecutionState ExecutionState { get; set; }

        public Vector3 Position { get; set; }

        public virtual FXEmitter Clone()
        {
             var emitter = new FXEmitter(
                System,
                MEmitterSpawn.Clone(),
                MEmitterUpdate.Clone(),
                MParticleSpawn.Clone(),
                MParticleUpdate.Clone(),
                MRender.Clone(),
                Prototype.Clone(),
                (from p in Particles select p.Clone()).ToList()
                );

            emitter.Age = Age;
            emitter.CurrentLoopDelay = CurrentLoopDelay;
            emitter.CurrentLoopDuration = CurrentLoopDuration;
            emitter.LoopCount = LoopCount;
            emitter.LoopedAge = LoopedAge;
            emitter.NormalizedLoopedAge = NormalizedLoopedAge;
            emitter.ExecutionState = ExecutionState;

            emitter.Position = Position;

            return emitter;
        }

        public virtual void Spawn(Vector3 position)
        {
            Position = position;

            foreach (var script in MEmitterSpawn.Scripts)
            {
                script.EmitterSpawn(position);
            }
        }

        public virtual void Update()
        {
            if (ExecutionState != FXExecutionState.Active)
            {
                return;
            }

            foreach (var script in MEmitterUpdate.Scripts)
            {
                script.EmitterUpdate();
            }

            UpdateParticles();
        }
        private void UpdateParticles()
        {
            foreach (var script in MParticleUpdate.Scripts)
            {
                foreach (var particle in Particles.AsParallel())
                {
                    script.ParticleUpdate(particle);
                }
            }
        }

        public virtual void Render()
        {
            if (ExecutionState != FXExecutionState.Active)
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

        public FXParticle SpawnParticle()
        {
            FXParticle particle = Prototype.Clone();
            Particles.Add(particle);

            foreach (var script in MParticleSpawn.Scripts)
            {
                script.ParticleSpawn(particle);
            }

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
