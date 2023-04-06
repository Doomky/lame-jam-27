using System.Collections.Generic;

namespace Framework.Extensions
{
    public static class ListExtensions
    {
        public static bool TryGetRandom<T>(this IList<T> list, out T t)
        {
            int length = list.Count;
            t = default(T);

            if (length == 0)
            {
                return false;
            }

            t = list[UnityEngine.Random.Range(0, length)];

            return true;
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = UnityEngine.Random.Range(0, n + 1);
                (list[n], list[k]) = (list[k], list[n]);
            }
        }
    }
}