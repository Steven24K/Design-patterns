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
    public interface IDrawable
    {
        void Draw(IDrawVisitor visitor);
    }

    public interface IUpdateable
    {
        void Update(IUpdateVisitor visitor, float dt);
    }

    public interface IComponent : IDrawable, IUpdateable { }

    public class MonoButton : IComponent
    {
        public int Width, Height;
        public Action action;
        public Colour Color;
        public Position TopLeftCorner;
        public MonoButton(Position top_left_corner, Colour color, int width, int height, Action action)
        {
            this.TopLeftCorner = top_left_corner;
            this.Color = color;
            this.action = action;
            this.Width = width;
            this.Height = height;
        }

        public bool IsIntersecting(Position point)
        {
            return point.X > TopLeftCorner.X && point.Y > TopLeftCorner.Y &&
                  point.X < TopLeftCorner.X + Width && point.Y < TopLeftCorner.Y + Height;
        }

        public void Draw(IDrawVisitor visitor)
        {
            visitor.DrawButton(this);
        }

        public void Update(IUpdateVisitor visitor, float dt)
        {
            visitor.UpdateButton(this, dt);
        }
    }

    public class DrawableNote : IComponent
    {
        public Position Pos;
        public DrawableNote(Position pos)
        {
            this.Pos = pos;
        }

        public void Draw(IDrawVisitor visitor)
        {
            visitor.DrawNote(this);
        }

        public void Update(IUpdateVisitor visitor, float dt)
        {
            visitor.UpdateNote(this, dt);
        }
    }

    public class GUIManager : IDrawable, IUpdateable
    {
        public Iterator<IComponent> GuiElements;
        private ISoundManager SndManager;
        private IGraphicsManager GManager;
        private GuiFactory Factory;
        private string[] notes;
        private int xPos, yPos;
        public GUIManager(ISoundManager snd_manager, IGraphicsManager g_manager, Action exit)
        {
            this.GuiElements = new ArrayItterator<IComponent>();
            this.SndManager = snd_manager;
            this.GManager = g_manager;
            this.Factory = new ConcreteGuiFactory(GManager);

            this.notes = new string[] { "C", "C#", "D", "E", "E#", "F", "F#", "G", "G#", "A", "B", "B#" };
            this.xPos = -10;
            this.yPos = GManager.GetScreenHeight / 2;
            foreach (var note in notes)
            {
                Factory.Create(0, new Position(xPos, yPos), () => { AddMusicNote(); SndManager.PlayNote(note); SndManager.AddSoundToSong(note); }).visit((elem) => GuiElements.Add(elem), () => { });

                xPos += GManager.GetScreenWidth / 12 + 5;
            }

            Factory.Create(1, new Position(1000, 150), () => SndManager.PlaySong()).visit((elem) => GuiElements.Add(elem), () => { });
            Factory.Create(3, new Position(1000, 230), () => DeleteMusicNote()).visit((elem) => GuiElements.Add(elem),()=> { });
        }

        public void Draw(IDrawVisitor visitor)
        {
            visitor.DrawGui(this);
        }

        public void Update(IUpdateVisitor visitor, float dt)
        {
            visitor.UpdateGui(this, dt);
        }
        private void DeleteMusicNote()
        {
            if (SndManager.GetSongList.GetLast().visit<bool>((n)=>true,()=>false)) {
                SndManager.DeleteLastNote();
                GuiElements.DeleteLast();
            }
        }

        private void AddMusicNote()
        {
            int xNote = 30;
            int yNote = 100;
            while(SndManager.GetSongList.GetNext().visit<bool>((e)=>true,()=>false))
            {
                Factory.Create(2, new Position(xNote, yNote), () => { }).visit((btn) => GuiElements.Add(btn), () => { });
                xNote += 50;
            }
            SndManager.GetSongList.Reset();
        }
    }
}