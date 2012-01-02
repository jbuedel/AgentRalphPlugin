using System.Collections.Generic;
using System.Linq;

namespace AgentRalph
{
    public static class Sets<T>
    {
        public static IEnumerable<IEnumerable<T>> CartesianProduct(IEnumerable<IEnumerable<T>> list_of_lists, int size)
        {
            IEnumerable<IEnumerable<T>> en = new IEnumerable<T>[0];
            foreach (var list in list_of_lists)
            {
                en = FoldInCartesianProduct(en, list, size);
            }

            if(size > -1)
                return en.Where(x=>x.Count() == size);
            return en;
        }

        public static IEnumerable<IEnumerable<T>> CartesianProduct(IEnumerable<IEnumerable<T>> listOfLists)
        {
            return CartesianProduct(listOfLists, -1);
        }

        public static IEnumerable<IEnumerable<T>> CartesianProduct(IEnumerable<T> first, IEnumerable<T> second)
        {
            return CartesianProduct(new[] {first, second});
        }

        private static IEnumerable<IEnumerable<T>> FoldInCartesianProduct(IEnumerable<IEnumerable<T>> enumerable, IEnumerable<T> list3, int size)
        {
            foreach (var c in list3)
            {
                if(size < 0 || 1 <= size)
                    yield return new[] {c};
            }
            foreach (var e in enumerable)
            {
                if (size < 0 || e.Count() <= size)
                    yield return e;

                foreach (var c in list3)
                {
                    if (size < 0 || e.Count() + 1 <= size)
                        yield return e.Concat(new[] {c});
                }
            }
        }

        public static IEnumerable<IList<T>> PowerSet(IList<T> currentGroupList)
        {
            int count = currentGroupList.Count;
            Dictionary<long, T> powerToIndex = new Dictionary<long, T>();
            long mask = 1L;
            for (int i = 0; i < count; i++)
            {
                powerToIndex[mask] = currentGroupList[i];
                mask <<= 1;
            }

            Dictionary<long, T> result = new Dictionary<long, T>();
            yield return result.Values.ToArray();

            long max = 1L << count;
            for (long i = 1L; i < max; i++)
            {
                long key = i & -i;
                if (result.ContainsKey(key))
                    result.Remove(key);
                else
                    result[key] = powerToIndex[key];
                yield return result.Values.ToArray();
            }
        }


        public static int[] ZeroTo(int count)
        {
            var list = new List<int>();
            for (int i = 0; i < count; i++)
            {
                list.Add(i);
            }
            return list.ToArray();
        }
    }
}