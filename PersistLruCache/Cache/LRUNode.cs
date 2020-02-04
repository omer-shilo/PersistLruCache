using System;
using System.Collections.Generic;
using System.Text;

namespace PersistLruCache.Cache
{
    [Serializable]
    public class LRUNode<T> where T : class
    {
        #region Properties
        public string Key { get; set; }
        public T Value { get; set; }
        public LRUNode<T> Previous { get; set; }
        public LRUNode<T> Next { get; set; }
        #endregion

        #region C'tor
        public LRUNode()
        {

        }

        public LRUNode(string key, T value)
        {
            Key = key;
            Value = value;
        }
        #endregion
    }
}
