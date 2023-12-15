using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDTests.Factory
{
    public abstract class BaseTest<T, T1> where T : class
                                            where T1 : class
    {
        protected abstract T CopyAndUpdate(T value);
        public abstract T1 Build();
    }
}
