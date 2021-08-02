using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension.FX
{
    public class FXEngine
    {
        public static float DeltaTime = 1 / 60; // 60fps

        private static Random _random = new Random(60);
        private static object _random_locker = new object();
        public static float CalculateRandomRange(float min = 0.0f, float max = 1.0f)
        {
            lock (_random_locker)
            {
                float length = max - min;
                return min + (float)_random.NextDouble() * length;
            }
        }
    }
}
