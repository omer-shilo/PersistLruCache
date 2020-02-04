using PersistLruCache.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace PersistLruCache.Cache
{
    public class LRUCache<T>: PersistDecorator<ConcurrentDictionary<string, LRUNode<T>>> where T : class
    {
        #region Properties
        public int Capacity { get; private set; }
        public int Count { get; private set; }
        private LRUDoubleLinkedList<T> DoubleLinkedList { get; set; }
        #endregion

        #region C'tor
        public LRUCache(string persistPrefix, string storeDirectory, double persistRefreshInterval, int maxSize = 50000)
        {
            Capacity = maxSize;
            PersistPrefix = persistPrefix;
            StoreDirectory = storeDirectory;
            Count = 0;

            DataSource = new ConcurrentDictionary<string, LRUNode<T>>(StringComparer.InvariantCultureIgnoreCase);
            DoubleLinkedList = new LRUDoubleLinkedList<T>();

            InitializeFromFile();

            PersistTimer = new Timer(persistRefreshInterval);
            PersistTimer.Elapsed += OnTimedEvent;
            PersistTimer.Enabled = true;
            PersistTimer.Start();
        }
        #endregion

        #region Methods
        public void Add(string key, T value)
        {
            if (DataSource.ContainsKey(key))
            {
                LRUNode<T> node = DataSource[key];

                DoubleLinkedList.RemoveNode(node);
                node.Value = value;
                DoubleLinkedList.AddToTop(node);
            }
            else
            {
                if (Count == Capacity)
                {
                    LRUNode<T> lru = DoubleLinkedList.RemoveLRUNode();
                    DataSource.TryRemove(lru.Key, out _);
                    Count--;
                }

                LRUNode<T> node = new LRUNode<T>(key, value);

                DoubleLinkedList.AddToTop(node);
                DataSource.TryAdd(key, node);
                Count++;
            }
        }
        public T Get(string key)
        {
            if (!DataSource.ContainsKey(key))
            {
                return null;
            }

            LRUNode<T> node = DataSource[key];
            DoubleLinkedList.RemoveNode(node);
            DoubleLinkedList.AddToTop(node);

            return node.Value;
        }
        public bool IsExistInCache(string key)
        {
            return DataSource.ContainsKey(key);
        }
        #endregion
    }
}
