using BT.Database.DbTools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.Database.Extensions
{
    public static class CollectionExtension
    {
        public static bool IsNullOrEmpty(this ICollection list)
        {
            bool result = list == null || list.Count == 0;
            return result;
        }

        public static void ReOrder(this IList<Column> list)
        {
            List<int> funcs = new List<int>();
            int i = 0;
            for (i = 0; i < list.Count; i++)
            {
                if (list[i].Functions != null)
                    funcs.Add(i);
            }
            for (i = 0; i < funcs.Count; i++)
            {
                //indice fonction : funcs[i]
                if (i != funcs[i])
                    list.Permute(i, funcs[i]);
            }
        }

        public static void Permute<T>(this IList<T> array, int i, int j)
        {
            var temp = array[j];
            array[j] = array[i];
            array[i] = temp;
        }

        public static T[] RemoveNulls<T>(this T[] array)
        {
            List<T> list = new List<T>();
            foreach (T item in array)
            {
                if (item != null)
                    list.Add(item);
            }
            return list.ToArray();
        }

        public static object GetFirstKey<T1, T2>(this Dictionary<T1, T2> dictionnary)
        {
            foreach (T1 key in dictionnary.Keys)
                return key;
            return null;
        }


    }
}
