
using System;
using System.Collections.Generic;

namespace Engine
{
    class Utility
    {
        private static readonly Random random = new Random();

        public static int GetRandom(int min, int max)
        {
            return random.Next(min, max);
        }

        public static float GetRandom(float min, float max)
        {
            return (float)(random.NextDouble() * (max - min) + min);
        }


        public static int GetEnumLength<T>()
        {
            return Enum.GetNames(typeof(T)).Length;
        }

        public static T Choice<T>(T[] array)
        {
            return array[random.Next(0, array.Length)];
        }

        public static T Choice<T>(List<T> list)
        {
            return list[random.Next(0, list.Count)];
        }

        public static bool IsIndexValid<T>(T[] array, int index)
        {
            return index >= 0 && index < array.Length;
        }
    }
}

