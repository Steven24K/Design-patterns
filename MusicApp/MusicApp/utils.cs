using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MusicApp
{
    public interface IOption<T>
    {
        U visit<U>(Func<T,U> onSome, Func<U> onNone);
        void visit(Action<T> onSome, Action onNone);
    }

    public interface IPoint<T,U>
    {
        T X { get; set; }
        U Y { get; set; }
    }

    public interface Iterator<T>
    {
        IOption<T> GetNext();
        IOption<T> GetCurrent();
        bool hasNext();
        void Reset();
        void DeleteLast();
        void DeleteAll();
        void Add(T item);
        T[] GetCollection();
        int GetAmountOfItems { get; }
    }

    public interface IGraphicsManager
    {
        int GetScreenHeight { get; }
        int GetScreenWidth { get; }
    }

    public interface IDrawManager
    {
        void DrawRectangle(Position top_left_coordinate, int width, int height, Colour color);
    }

    public interface InputManager
    {
        bool isPressed();
        IOption<Position> GetTouchPosition();
    }

    public interface ISoundManager
    {
        void PlaySingleNote(string note);
        void Play(string note);
        void AddSoundToSong(string note);
        void PlaySong();
    }

    public class Some<T> : IOption<T>
    {
        private T Value;
        public Some(T value)
        {
            this.Value = value;
        }

        public U visit<U>(Func<T, U> onSome, Func<U> onNone)
        {
            return onSome(this.Value);
        }

        public void visit(Action<T> onSome, Action onNone)
        {
            onSome(this.Value);
        }
    }

    public class None<T> : IOption<T>
    {
        public U visit<U>(Func<T, U> onSome, Func<U> onNone)
        {
            return onNone();
        }

        public void visit(Action<T> onSome, Action onNone)
        {
            onNone();
        }
    }

    public class Position : IPoint<int, int>
    {
        private int _x,_y;
        public Position(int x, int y)
        {
            this._x = x;
            this._y = y;
        }

        public int X { get { return this._x; } set { this._x = value; } }

        public int Y { get { return this._y; } set { this._y = value; } }

    }

    public class ArrayItterator<T> : Iterator<T>
    {
        private T[] _Array;
        private int Current, Size, AmountOfItems;
        public ArrayItterator()
        {
            this.Size = 2;
            this._Array = new T[Size];
            AmountOfItems = 0;
            this.Current = -1;
        }

        public void Add(T item)
        {
            if (AmountOfItems >= Size)
            {
                Size *= 2;
                T[] new_array = new T[Size];
                for (int c = 0; c < _Array.Length; c++)
                {
                    new_array[c] = _Array[c];
                }
                _Array = new_array;
            }
            _Array[AmountOfItems] = item;
            AmountOfItems++;
        }

        public IOption<T> GetNext()
        {
            Current += 1;
            if (hasNext()) return new Some<T>(_Array[Current]);
            return new None<T>();
        }

        public bool hasNext()
        {
            if (Current < 0 | Current > AmountOfItems-1) return false;
            return true;
        }

        public T[] GetCollection()
        {
            return _Array;
        }

        public IOption<T> GetCurrent()
        {
            if (hasNext()) return new Some<T>(this._Array[Current]);
            return new None<T>();
        }

        public void Reset()
        {
            this.Current = -1;
        }

        public void DeleteLast()
        {
            this._Array[AmountOfItems] = default(T);
            AmountOfItems -= 1;
        }

        public void DeleteAll()
        {
            this.Size = 2;
            this._Array = new T[Size];
            AmountOfItems = 0;
            Reset();
        }

        public int GetAmountOfItems { get { return this.AmountOfItems; } }

    }

    public class Colour
    {
        private int R, G, B;
        public Colour(int r, int g, int b)
        {
            this.R = r;
            this.G = g;
            this.B = b;
        }

        public int[] getRGB { get { return new int[] {this.R,this.G,this.B }; } }
    }

    public abstract class GuiFactory
    {
        protected IGraphicsManager Manager;
        public GuiFactory(IGraphicsManager manager) { this.Manager = manager; }
        public abstract IOption<IComponent> Create(int id, Position top_left_corner, Action action);
    }

    public class ConcreteGuiFactory : GuiFactory
    {
        public ConcreteGuiFactory(IGraphicsManager manager):base(manager)
        {
        }

        public override IOption<IComponent> Create(int id, Position top_left_corner, Action action)
        {
            
            switch (id)
            {
                case 0://Create a new white button, used for the music actions
                    return new Some<IComponent>(new MonoButton(top_left_corner, new Colour(255, 255, 255), base.Manager.GetScreenWidth/12, base.Manager.GetScreenHeight/2, action));
                case 1: //Creates a play song button
                    return new Some<IComponent>(new MonoButton(top_left_corner, new Colour(0,255,50),50, 100,action));
                default:
                    return new None<IComponent>();
            }
        }
        }
}