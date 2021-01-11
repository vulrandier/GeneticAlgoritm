using System;

namespace GeneticAlgoritm.Utilities {
    public static class RandomNumGen {
        private static readonly Random _rand;

        static RandomNumGen() {
#if  const_random
            _rand = new Random(0);
#else
            _rand = new Random();
#endif
        }


        public static int getRandomIntInRange(int min, int max) {
            // +1 because Next returns a value greate or equal of min but less than max...
            return _rand.Next(min, max+1);
        }


        public static double getRandomDouble_0_to_1() {
            return _rand.NextDouble();
        }

        public static double getRandomDoubleInRange(double min, double max) {
            return min + (max - min) * _rand.NextDouble();
        }


        public static bool getRandomTrue(double probability = 0.5) {
            return probability >= _rand.NextDouble();
        }
    }
}