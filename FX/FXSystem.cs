using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension.FX
{
    public class FXSystem : ICloneable
    {
        public FXSystem(FXModule mSystemSpawn, FXModule mSystemUpdate, List<FXEmitter> emitters)
        {
            MSystemSpawn = mSystemSpawn;
            MSystemUpdate = mSystemUpdate;
            Emitters = emitters;
        }

        // Variables

        public List<FXEmitter> Emitters { get; }

        // Modules

        public FXModule MSystemSpawn { get; }
        public FXModule MSystemUpdate { get; }

        // Parameters

        public float Age { get; set; }
        public float CurrentLoopDelay { get; set; }
        public float CurrentLoopDuration { get; set; }
        public int LoopCount { get; set; }
        public float LoopedAge { get; set; }
        public float NormalizedLoopedAge { get; set; }
        public bool Completed { get; set; }

        public virtual FXSystem Clone()
        {
            var system = new FXSystem(
                MSystemSpawn.Clone(),
                MSystemUpdate.Clone(),
                (from e in Emitters select e.Clone()).ToList()
                );

            system.Age = Age;
            system.CurrentLoopDelay = CurrentLoopDelay;
            system.CurrentLoopDuration = CurrentLoopDuration;
            system.LoopCount = LoopCount;
            system.LoopedAge = LoopedAge;
            system.NormalizedLoopedAge = NormalizedLoopedAge;
            system.Completed = Completed;

            return system;
        }

        public virtual void Spawn()
        {
            foreach(var script in MSystemSpawn.Scripts)
            {
                script.SystemSpawn();
            }

            SpawnEmitter();
        }
        private void SpawnEmitter()
        {
            foreach (var emitter in Emitters.AsParallel())
            {
                emitter.Spawn();
            }
        }

        public virtual void Update()
        {
            if (Completed)
            {
                return;
            }

            UpdateEmitter();
        }
        private void UpdateEmitter()
        {
            foreach (var script in MSystemSpawn.Scripts)
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

            foreach (var emitter in Emitters)
            {
                emitter.Render();
            }
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
