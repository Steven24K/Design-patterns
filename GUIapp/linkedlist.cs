using System;

namespace GUIapp
{
    interface ILinkedList<T>
    {
        //Linkedlist interface
        bool IsEmpty { get; }
        T Value { get; set; }
        ILinkedList<T> Tail { get; set; }
        //LinkedList items can be accesed through a visitor
        U Visit<U>(Func<T, U> onNode, Func<U> onEmpty);
        void Visit(Action<T> onNode, Action onEmpty);
        ILinkedList<T> Add(T _object); //To add a new element
        ILinkedList<T> Reverse(); //Reverses the list
        ILinkedList<T> Filter(Func<T,bool> Predicate);
        int Length();
    }

    class Node<T> : ILinkedList<T>
    {
        //Concrete implementation of linkedlist
        private T item;
        private ILinkedList<T> next;
        private bool isempty;

        public Node(T value, ILinkedList<T> tail)
        {
            this.item = value;
            this.next = tail;
            this.isempty = false;
        }

        public bool IsEmpty { get { return this.isempty; } }
        public T Value { get { return this.item; } set { this.item = value; } }
        public ILinkedList<T> Tail { get { return this.next; } set { this.next = value; } }

        public int Length()
        {
            return 1 + this.Tail.Length();
        }

        public ILinkedList<T> Reverse()
        {
            ILinkedList<T> tmp = this;
            ILinkedList<T> ret = new Empty<T>();
            while (!tmp.IsEmpty)
            {
                ret = new Node<T>(tmp.Value, ret);
                tmp = tmp.Tail;
            }
            return ret;
        }

        public ILinkedList<T> Add(T _object)
        {
            return new Node<T>(_object, this);
        }

        public U Visit<U>(Func<T, U> onNode, Func<U> onEmpty)
        {
            return onNode(this.Value);
        }

        public void Visit(Action<T> onNode, Action onEmpty)
        {
            onNode(this.Value);
        }

        public ILinkedList<T> Filter(Func<T, bool> Predicate)
        {
            if (Predicate(this.Value))
            {
                return new Node<T>(this.Value,this.Tail.Filter(Predicate));
            }
            else
            {
                return this.Tail.Filter(Predicate);
            }
        }
    }

    class Empty<T> : ILinkedList<T>
    {
        //Concrete implementation of LinkedList
        private bool isempty;
        public Empty()
        {
            this.isempty = true;
        }

        public string Print()
        {
            return ("");
        }

        public ILinkedList<T> Add(T _object)
        {
            return new Node<T>(_object, this);
        }

        public U Visit<U>(Func<T, U> onNode, Func<U> onEmpty)
        {
            return onEmpty();
        }

        public int Length()
        {
            return 0;
        }

        public void Visit(Action<T> onNode, Action onEmpty)
        {
            onEmpty();
        }

        public ILinkedList<T> Reverse()
        {
            return this;
        }

        public ILinkedList<T> Filter(Func<T, bool> Predicate)
        {
            return this;
        }

        public bool IsEmpty { get { return this.isempty; } }
        public T Value { get { return default(T); } set { value = default(T); } }
        public ILinkedList<T> Tail { get { return default(ILinkedList<T>); } set { value = default(ILinkedList<T>); } }
    }
}
