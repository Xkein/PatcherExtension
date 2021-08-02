using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension.FX.Scripts.Emitter
{
    public class FXSpawnBurstInstantaneous : FXSpawn
    {
        public FXSpawnBurstInstantaneous(FXSystem system, FXEmitter emitter) : base(system, emitter)
        {
        }

        public int SpawnCount { get; set; } = 1;
        public float SpawnTime { get; set; } = 0;
        public float SpawnProbability { get; set; } = 1;
        public float? Age { get; set; }


        public override FXScript Clone()
        {
            var spawnBurstInstantaneous = new FXSpawnBurstInstantaneous(System, Emitter);

            spawnBurstInstantaneous.SpawnInfo = SpawnInfo;

            spawnBurstInstantaneous.SpawnCount = SpawnCount;
            spawnBurstInstantaneous.SpawnTime = SpawnTime;
            spawnBurstInstantaneous.SpawnProbability = SpawnProbability;
            spawnBurstInstantaneous.Age = Age;

            return spawnBurstInstantaneous;
        }

        public override void EmitterUpdate()
        {
            var age = Age.GetValueOrDefault(Emitter.LoopedAge);

            SpawnInfo.InterpStartDt = SpawnTime + FXEngine.DeltaTime - age;

            bool shouldSpawn = SpawnInfo.InterpStartDt >= 0 && SpawnTime - age < 0;
            if (SpawnProbability < 1)
            {
                shouldSpawn &= SpawnProbability > FXEngine.CalculateRandomRange();
            }

            SpawnInfo.Count = shouldSpawn ? SpawnCount : 0;
            SpawnInfo.IntervalDt = 0;

            base.EmitterUpdate();
        }
    }
}
