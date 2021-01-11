using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GeneticAlgoritm.DNA;
using static  GeneticAlgoritm.Utilities.RandomNumGen;
namespace GeneticAlgoritm.Utilities {
    public static class Extensions {
        public static T selectRandomItem<T>(this List<T> list) {
            return list[getRandomIntInRange(0, list.Count-1)];
        }

        public static T minmax<T>(this T value, T min, T max) where  T: IComparable{
            if (value.CompareTo(min) == Decimal.MinusOne)
                return min;
            if (value.CompareTo(max) == Decimal.One)
                return max;
            return value;
        }
        
    }
}