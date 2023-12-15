using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public class ComparerAdapter
    {
        public static IComparer<object> GetDefaultComparer(Type propertyType)
        {
            if (propertyType == typeof(string))
            {
                return Comparer<object>.Create((x, y) =>
                {
                    string s1 = x.ToString()!;
                    string s2 = y.ToString()!;

                    return string.Compare(s1, s2, StringComparison.OrdinalIgnoreCase);
                });
            }

            return Comparer<object>.Default;
        }

        //public static IComparer<object> Adapt<T>(IComparer<T> comparer)
        //{
        //    return Comparer<object>.Create((x, y) =>
        //    {
        //        if (x is T arg1 && y is T arg2)
        //        {
        //            return comparer.Compare(arg1, arg2);
        //        }

        //        throw new ArgumentException("Both objects invalid type.");
        //    });
        //}

        public static (Type, IComparer<object>) Adapt<T>(IComparer<T> comparer)
        {
            Type typeT = typeof(T);

            Comparer<object> adaptedComparer = Comparer<object>.Create((x, y) =>
            {
                if (x is T arg1 && y is T arg2)
                {
                    return comparer.Compare(arg1, arg2);
                }

                throw new ArgumentException("Both objects invalid type.");
            });

            return (typeT, adaptedComparer);
        }
    }
}
