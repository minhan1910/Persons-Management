using ServiceContracts.DTO.PersonDTO;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers
{
    [Obsolete]
    public static class PropertyCached<T> where T : class
    {
        private static readonly ConcurrentDictionary<string, Func<T, object>?> PropertyAccessorCache =
            new ConcurrentDictionary<string, Func<T, object>?>();

        private static Func<T, object>? CreatePropertyAccessor(string propertyName)
        {
            // Expression replace for using Reflection typeof(T).GetProperty()
            var parameter = Expression.Parameter(typeof(T));
            MemberExpression? property = null;
            try
            {
                property = Expression.Property(parameter, propertyName);
            }
            catch (Exception)
            {                
                return null;
            }
            var convert = Expression.Convert(property, typeof(object));
            var lambda = Expression.Lambda<Func<T, object>?>(convert, parameter);

            return lambda.Compile();
        }        

        public static Func<T, object>? GetOrAdd(string propertyName)        
            =>  PropertyAccessorCache.GetOrAdd(propertyName, CreatePropertyAccessor(propertyName));                

    }
}
