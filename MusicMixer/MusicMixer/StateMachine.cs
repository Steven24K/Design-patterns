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

namespace MusicMixer
{
    public interface IStateMachine
    {
        bool Done { get; }
        void Update(float dt);
        void Reset();
    }

    public class Timer : IStateMachine
    {
        private float Time, Init_Time;
        private bool done;
        public Timer(float time)
        {
            this.Time = time;
            this.Init_Time = time;
            this.done = false;
        }

        public bool Done { get { return this.done; } }

        public void Reset()
        {
            this.Time = this.Init_Time;
            this.done = false;
        }

        public void Update(float dt)
        {
            this.Time -= dt;
            if (this.Time <= 0) this.done = true;
        }
    }

    public class Wait : IStateMachine
    {
        private bool done;
        private Func<bool> Condition;
        public Wait(Func<bool> cond)
        {
            this.Condition = cond;
            this.done = false;
        }

        public bool Done { get { return this.done; } }

        public void Reset()
        {
            this.done = false;
        }

        public void Update(float dt)
        {
            if (this.Condition()) this.done = true;
        }
    }

    public class Sequence : IStateMachine
    {
        private bool done;
        private IStateMachine Current, Next, S1, S2;
        public Sequence(IStateMachine s1, IStateMachine s2)
        {
            this.Current = s1;
            this.Next = s2;
            this.S1 = s1;
            this.S2 = s2;
            this.done = false;
        }

        public bool Done { get { return this.done; } }

        public void Reset()
        {
            this.S1.Reset();
            this.S2.Reset();
            this.Current = this.S1;
            this.Next = this.S2;
            this.done = false;
        }

        public void Update(float dt)
        {
            if (!this.Done)
            {
                this.Current.Update(dt);
                if (Current.Done) this.Current = this.Next;
                if (Next.Done) this.done = true;
            }
        }
    }

    public class Repeat : IStateMachine
    {
        private IStateMachine Action;
        public Repeat(IStateMachine action) { this.Action = action; }

        public bool Done { get { return true; } }

        public void Reset()
        {
            this.Action.Reset();
        }

        public void Update(float dt)
        {
            if (this.Action.Done) Reset();
            this.Action.Update(dt);
        }
    }

    public class Call : IStateMachine
    {
        private Action Action;
        private bool done;
        public Call(Action action)
        {
            this.Action = action;
            this.done = false;
        }

        public bool Done { get { return this.done; } }

        public void Reset()
        {
            this.done = false;
        }

        public void Update(float dt)
        {
            this.Action();
            this.done = true;
        }
    }

    public class Callif : IStateMachine
    {
        private bool done;
        private Func<bool> Guard;
        private Action Action;
        public Callif(Func<bool> guard, Action action)
        {
            this.Guard = guard;
            this.Action = action;
            this.done = false;
        }
        public bool Done { get { return this.done; } }

        public void Reset()
        {
            this.done = false;
        }

        public void Update(float dt)
        {
            if (this.Guard()) this.Action();
            this.done = true;
        }
    }
}