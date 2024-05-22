using System.Collections.Generic;

namespace MonoGameEngine
{
    public sealed class FixedSizeLinkedList<T> : LinkedList<T> where T : struct
    {
        private int _maxSize = 8192;

        public FixedSizeLinkedList(int size) : base()
        {
            _maxSize = size;
        }

        public void Push(T item)
        {
            if(Count >= _maxSize)
            {
                RemoveFirst();
            }
            AddLast(item);
        }

        public T Pop()
        {
            if (Count > 0)
            {
                T item = Last.Value;
                RemoveLast();
                return item;
            }
            else
            {
                return default;
            }
        }

        public void SetSize(int size)
        {
            _maxSize = size;
            while (Count > _maxSize)
            {
                RemoveFirst();
            }
        }
    }
}
