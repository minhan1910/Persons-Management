using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public class ExpressionHelpers
    {
        private static readonly Dictionary<string, object> _accessorCache = new Dictionary<string, object>();
        private static readonly Dictionary<string, MemberExpression> _propretyCache = new Dictionary<string, MemberExpression>();

        public static Type GetAccessorPropertyType(string propertyName) => _propretyCache[propertyName].Type;

        public static Func<T, object> CreateAccessorProperty<T>(string propertyName)
        {

            if (_accessorCache.TryGetValue(propertyName, out var accessorObj) == false)
            {
                ParameterExpression param = Expression.Parameter(typeof(T), "x");

                if (_propretyCache.TryGetValue(propertyName, out var property) == false)
                {
                    property = Expression.Property(param, propertyName);
                }

                Type objectType = typeof(object);

                var conversion = Expression.Convert(property, objectType);

                Type funcType = typeof(Func<,>).MakeGenericType(typeof(T), objectType);

                LambdaExpression lambda = Expression.Lambda(funcType, conversion, param);

                var accessor = lambda.Compile();

                // caching accessor and property type
                _accessorCache[propertyName] = accessor;
                _propretyCache[propertyName] = property;

                accessorObj = accessor;
            }

            return (Func<T, object>)accessorObj;
        }
    }
}
