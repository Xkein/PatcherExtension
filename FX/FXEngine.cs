using Extension.FX.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Extension.FX
{
    public class FXEngine
    {
        #region Constant
        public static float DeltaTime = 1f / 60.0f; // 60fps
        #endregion

        #region Math
        public static float Clamp(float a, float min, float max)
        {
            if (a < min)
            {
                return min;
            }

            if (a > max)
            {
                return max;
            }

            return a;
        }

        private static Random _random = new Random(60);
        private static object _random_locker = new object();
        public static float CalculateRandomRange(float min = 0.0f, float max = 1.0f)
        {
            if (min == max)
            {
                return min;
            }

            lock (_random_locker)
            {
                float length = max - min;
                return min + (float)_random.NextDouble() * length;
            }
        }

        public static Vector3 CalculateRandomUnitVector()
        {
            lock (_random_locker)
            {
                const float r = 1;
                const float PI2 = (float)(Math.PI * 2);

                float azimuth = (float)(_random.NextDouble() * PI2);
                float elevation = (float)(_random.NextDouble() * PI2);

                return new Vector3(
                    (float)(r * Math.Cos(elevation) * Math.Cos(azimuth)),
                    (float)(r * Math.Cos(elevation) * Math.Sin(azimuth)),
                    (float)(r * Math.Sin(elevation))
                    );
            }
        }
        public static Vector3 CalculateRandomPointInSphere(float innerRadius, float outerRadius)
        {
            return CalculateRandomUnitVector() * CalculateRandomRange(innerRadius, outerRadius);
        }

        public static Vector3 CalculateRandomPointInBox(Vector3 size)
        {
            return new Vector3(
                CalculateRandomRange(0, size.X) - size.X / 2f,
                CalculateRandomRange(0, size.Y) - size.Y / 2f,
                CalculateRandomRange(0, size.Z) - size.Z / 2f
                );
        }
        #endregion

        #region Work
        public static bool EnableParallelSpawn { get; set; } = true;
        public static bool EnableParallelUpdate { get; set; } = true;
        public static bool EnableParallelRender { get; set; } = true;
        public static bool EnableParallel
        {
            get => EnableParallelSpawn && EnableParallelUpdate && EnableParallelRender;
            set => EnableParallelSpawn = EnableParallelUpdate = EnableParallelRender = value;
        }

        public static List<FXSystem> WorkSystems { get; private set; } = new List<FXSystem>();
        public static ReaderWriterLockSlim WorkListRWLock = new ReaderWriterLockSlim();

        /// <summary>
        /// Add FXSystem to work list next frame. Adding when updating is allowed.
        /// </summary>
        public static void AddSystem(FXSystem system)
        {
            WorkListRWLock.EnterWriteLock();

            WorkSystems.Add(system);

            WorkListRWLock.ExitWriteLock();
        }
        /// <summary>
        /// Remove FXSystem from work list next frame. Removing when updating is allowed..
        /// </summary>
        public static void RemoveSystem(FXSystem system)
        {
            WorkListRWLock.EnterWriteLock();

            WorkSystems.Remove(system);

            WorkListRWLock.ExitWriteLock();
        }

        public static void Restart()
        {
            WorkSystems.Clear();
        }

        public static void Update()
        {
            WorkListRWLock.EnterWriteLock();

            var list = WorkSystems.ToArray();

            if (FXEngine.EnableParallelUpdate)
            {
                Parallel.ForEach(WorkSystems, system => system.Update());
            }
            else
            {
                foreach (var system in list)
                {
                    system.Update();
                }
            }

            foreach (var system in list)
            {
                switch (system.ExecutionState)
                {
                    case FXExecutionState.InactiveClear:
                    case FXExecutionState.Complete:
                        RemoveSystem(system);
                        system.ExecutionState = FXExecutionState.Complete;
                        break;
                }
            }

            WorkListRWLock.ExitWriteLock();
        }
        public static void Render()
        {
            WorkListRWLock.EnterReadLock();

            if (FXEngine.EnableParallelRender)
            {
                Parallel.ForEach(WorkSystems, system => system.Render());
            }
            else
            {
                foreach (var system in WorkSystems)
                {
                    system.Render();
                }
            }

            WorkListRWLock.ExitReadLock();
        }
        #endregion

        #region Graphic

        #endregion
    }
}
