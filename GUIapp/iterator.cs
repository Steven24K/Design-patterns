using System;

namespace GUIapp
{
    interface Iterator<T>
    {
        //The interator interface contains 3 methods:
        IOption<T> getCurrent(); //Gets the current value
        IOption<T> moveNext(); //Moves to the next value
        bool hasNext(); //Checks if there is a next value
    }

    class ArrayIteratior<T> : Iterator<T>
    {
        //Represents a concrete iterator for only Arrays
        private T[] _Array;
        private int Current;
        public ArrayIteratior(T[] array)
        {
            this._Array = array;
            this.Current = -1;
        }

        public IOption<T> getCurrent()
        { 
            //Returns an Option, if there is a next item return Some(CurrentITem) else return None() 
            if (this.hasNext()) return new Some<T>(this._Array[Current]);
            return new None<T>();
        }

        public IOption<T> moveNext()
        {
            //Increments the current by one, and returns the next value
            this.Current += 1;
            if (hasNext() && _Array[Current] != null)
            {
                return new Some<T>(this._Array[Current]);
            }
            return new None<T>();
        }

        public bool hasNext()
        {
            //Checks if the array is not out of range if so then return false
            if (Current < 0 | Current > _Array.Length) return false;
            return true;
        }
    }

    class LinkedListIterator<T> : Iterator<T>
    {
        //Concrete implementation of the iterator interface, for LinkedLists only.
        private ILinkedList<T> LinkedList;
        public LinkedListIterator(ILinkedList<T> linkedlist)
        {
            this.LinkedList = linkedlist;
        }

        public IOption<T> getCurrent()
        {
            //If there is a current value return some(value) else None()
            if (this.hasNext()) return new Some<T>(this.LinkedList.Value);
            return new None<T>();
        }

        public bool hasNext()
        {
            //Returns true if the list has a next value, if list is empty return false
            return this.LinkedList.Visit((value) => true, () => false);
        }

        public IOption<T> moveNext()
        {
            //Moves to the Tail from the current Linkedlist item wich contains a refrence to the next value
           this.LinkedList = this.LinkedList.Tail;
           return LinkedList.Visit<IOption<T>>(node => new Some<T>(this.LinkedList.Value), () => new None<T>());
        }
    }
}
