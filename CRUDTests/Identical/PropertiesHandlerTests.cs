using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace CRUDTests.Identical
{
    public class PropertiesHandlerTests<T1, T2> where T1 : class
                                                     where T2 : class
    {

        private static ITestOutputHelper _outputHelper;
        public PropertiesHandlerTests(ITestOutputHelper testOutputHelper)
        {
            _outputHelper = testOutputHelper;   
        }

        /// <summary>
        /// Copy src into dest
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>        
        public static T2? Copy(T1? src, T2? dest)
        {
            if (src is null || dest is null)
            {
                return null;
            }

            var p1Props = src.GetType().GetProperties();
            var p2Props = dest.GetType().GetProperties();

            foreach (PropertyInfo p1PropInfo in p1Props)
            {
                foreach (PropertyInfo p2PropInfo in p2Props)
                {
                    var v1 = p1PropInfo.GetValue(src);

                            _outputHelper.WriteLine(" same type");
                    if (p1PropInfo.Name == p2PropInfo.Name)
                    {
                        if (p1PropInfo.PropertyType == p2PropInfo.PropertyType)
                        {
                            _outputHelper.WriteLine(" same type");
                            if (v1 is not null)
                            {
                                p2PropInfo.SetValue(dest, v1);
                            }
                        }
                        else
                        {
                            _outputHelper.WriteLine("Not the same type");
                            if (v1 is not null)
                            {
                                if (p2PropInfo.PropertyType == typeof(GenderOptions))
                                {
                                    p2PropInfo.SetValue(dest, Enum.Parse(typeof(GenderOptions), v1.ToString()!, true));
                                }
                            }
                        }
                    }
                }
            }

            return dest;
        }
    }
}
