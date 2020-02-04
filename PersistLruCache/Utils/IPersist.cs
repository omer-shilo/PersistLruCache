using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace PersistLruCache.Utils
{
    public interface IPersist<T> where T : class
    {
        Timer PersistTimer { get; set; }
        string PersistPrefix { get; set; }
        string StoreDirectory { get; set; }
        T DataSource { get; set; }

        void InitializeFromFile();
        void OnTimedEvent(Object source, ElapsedEventArgs e);
    }
}
