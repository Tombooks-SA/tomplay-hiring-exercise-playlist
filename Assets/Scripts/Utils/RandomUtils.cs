using System;
using System.Collections.Generic;

namespace Utils
{
    public static class RandomUtils
    {
        public static T GetRandomElement<T>(this T[] array)
        {
            if (array == null || array.Length == 0)
            {
                throw new ArgumentException("The array cannot be null or empty.");
            }
            int index = UnityEngine.Random.Range(0, array.Length);
            return array[index];
        }
    }
}