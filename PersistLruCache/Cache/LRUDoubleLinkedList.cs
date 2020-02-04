using System;
using System.Collections.Generic;
using System.Text;

namespace PersistLruCache.Cache
{
    [Serializable]
    public class LRUDoubleLinkedList<T> where T : class
    {
        #region Properties
        private readonly LRUNode<T> Head;
        private readonly LRUNode<T> Tail;
        #endregion

        #region C'tor
        public LRUDoubleLinkedList()
        {
            Head = new LRUNode<T>();
            Tail = new LRUNode<T>();

            Head.Next = Tail;
            Tail.Previous = Head;
        }
        #endregion

        #region Methods
        public void AddToTop(LRUNode<T> node)
        {
            node.Next = Head.Next;
            Head.Next.Previous = node;
            node.Previous = Head;
            Head.Next = node;
        }
        public void RemoveNode(LRUNode<T> node)
        {
            node.Previous.Next = node.Next;
            node.Next.Previous = node.Previous;
            node.Next = null;
            node.Previous = null;
        }
        public LRUNode<T> RemoveLRUNode()
        {
            LRUNode<T> target = Tail.Previous;
            RemoveNode(target);

            return target;
        }
        #endregion
    }
}
