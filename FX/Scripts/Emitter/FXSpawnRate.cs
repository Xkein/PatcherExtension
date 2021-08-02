using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension.FX.Scripts.Emitter
{
    public class FXSpawnRate : FXSpawn
    {
        public FXSpawnRate(FXSystem system, FXEmitter emitter) : base(system, emitter)
        {
        }

        public float SpawnRate { get; set; } = 0;
        public float SpawnProbability { get; set; } = 1;

        public override FXScript Clone()
        {
            var spawnRate = new FXSpawnRate(System, Emitter);

            spawnRate.SpawnInfo = SpawnInfo;

            spawnRate.SpawnProbability = SpawnProbability;

            return spawnRate;
        }

        public override void EmitterUpdate()
        {
            var spawnRate = SpawnRate;
            var intervalDT = 1 / spawnRate;
            var interpStartDT = intervalDT * (1 - SpawnRemainder);

            var spawnValue = spawnRate * (Emitter.LoopedAge >= 0 ? 1 : 0) * FXEngine.DeltaTime + SpawnRemainder;
            var spawnCount = (int)spawnValue;
            SpawnRemainder = spawnValue - spawnCount;

            SpawnInfo.Count = SpawnProbability >= 1 || SpawnProbability > FXEngine.CalculateRandomRange() ? spawnCount : 0;
            SpawnInfo.InterpStartDt = interpStartDT;
            SpawnInfo.IntervalDt = intervalDT;

            base.EmitterUpdate();
        }

        // Unique Attribute

        public float SpawnRemainder { get; set; }
    }
}
