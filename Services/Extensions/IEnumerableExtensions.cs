using ServiceContracts.Enums;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> OrderByProperty<T>(this IEnumerable<T> source,
                                                        Func<T, object> keySelector,
                                                        SortOrderOptions sortOrder = SortOrderOptions.ASC,
                                                        IComparer<object>? customComparer = null)
        {
            return sortOrder switch
            {
                SortOrderOptions.ASC => source.OrderBy(keySelector, customComparer ?? Comparer<object>.Default),
                SortOrderOptions.DESC => source.OrderByDescending(keySelector, customComparer ?? Comparer<object>.Default),
                _ => source
            };
        }

        public static IEnumerable<T> OrderByProperty<T>(
           this IEnumerable<T> source,
           string propertyName,
           SortOrderOptions sortOrder = SortOrderOptions.ASC,
           (Type typeComparer, IComparer<object> comparerImpl)? customComparer = null)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (string.IsNullOrWhiteSpace(propertyName))
            {
                return source;
            }

            Func<T, object> keySelector = ExpressionHelpers.CreateAccessorProperty<T>(propertyName);

            var propertyType = ExpressionHelpers.GetAccessorPropertyType(propertyName);

            IComparer<object> defaultComparer = ComparerAdapter.GetDefaultComparer(propertyType);
            IComparer<object> res = defaultComparer;

            if (customComparer.HasValue)
            {
                (var typeComparer, var comparerImpl) = customComparer.Value;

                res = typeComparer == propertyType ? comparerImpl : defaultComparer;
            }

            return OrderByProperty(source, keySelector, sortOrder, res);
        }

        public static IEnumerable<T> OrderByProperty<T, TKey>(
           this IEnumerable<T> source,
           Func<T, TKey> keySelector,
           SortOrderOptions sortOrder = SortOrderOptions.ASC,
           IComparer<TKey>? customComparer = null)
        {
            return sortOrder switch
            {
                SortOrderOptions.ASC => customComparer switch
                {
                    null => source.OrderBy(keySelector),
                    _ => source.OrderBy(keySelector, customComparer),
                },

                SortOrderOptions.DESC => customComparer switch
                {
                    null => source.OrderByDescending(keySelector),
                    _ => source.OrderByDescending(keySelector, customComparer),
                }
            };
        }

    }
}
