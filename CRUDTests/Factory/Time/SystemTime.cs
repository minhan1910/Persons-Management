using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDTests.Factory.Time
{
    /// <summary>
    /// Used for getting DateTime.Now(), time is changeable for unit testing
    /// </summary>
    public sealed class SystemTime
    {
        /// <summary> Normally this is a pass-through to DateTime.Now, but it can be overridden with SetDateTime( .. ) for testing or debugging.
        /// </summary>
        private static Func<DateTime> NowFunc = () => DateTime.Now;

        /// <summary>
        /// For running parallel testing
        /// </summary>
        private static AsyncLocal<Func<DateTime>> _override = new AsyncLocal<Func<DateTime>>();

        /// <summary>
        /// Set time to return when SystemTime.Now is called.
        /// </summary>
        public static void SetDateTime(Func<DateTime> nowFunc)
        {           
            _override.Value = nowFunc;
        }

        /// <summary> 
        /// Resets SystemTime.Now to return DateTime.Now.
        /// </summary>
        public static void ResetDateTime()
        {
            //NowFunc = () => DateTime.Now;
            _override.Value = null;
        }

        public static DateTime Now => (_override.Value ?? NowFunc)();
    }
}
