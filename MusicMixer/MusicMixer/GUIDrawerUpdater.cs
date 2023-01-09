using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MusicMixer
{
    public interface IDrawVisitor
    {
        void DrawGui(GUIManager guimanager);
        void DrawButton(MonoButton button);
        void DrawNote(DrawableNote note);
    }

    public interface IUpdateVisitor
    {
        void UpdateGui(GUIManager guimanager, float dt);
        void UpdateButton(MonoButton button, float dt);
        void UpdateNote(DrawableNote note, float dt);
    }

    public class ConcreteDrawVisitor : IDrawVisitor
    {
        private IDrawManager DrawManager;
        public ConcreteDrawVisitor(IDrawManager draw_manager)
        {
            this.DrawManager = draw_manager;
        }
        public void DrawButton(MonoButton button)
        {
            DrawManager.DrawRectangle(button.TopLeftCorner, button.Width, button.Height, button.Color);
        }

        public void DrawGui(GUIManager guimanager)
        {
            while (guimanager.GuiElements.GetNext().visit<bool>((v) => { return true; }, () => { return false; }))
            {
                guimanager.GuiElements.GetCurrent().visit((v) => v.Draw(this), () => { });
            }
            guimanager.GuiElements.Reset();
        }

        public void DrawNote(DrawableNote note)
        {
            DrawManager.DrawRectangle(note.Pos,5,50,new Colour(255,255,255));
        }
    }

    public class ConcreteUpdateVisitor : IUpdateVisitor
    {
        private InputManager InputManager;
        public ConcreteUpdateVisitor(InputManager input_manager)
        {
            this.InputManager = input_manager;
        }
        public void UpdateButton(MonoButton button, float dt)
        {
            InputManager.GetTouchPosition().visit(
              (p) =>
              {
                  if (button.IsIntersecting(p)) button.action();
              }
              , () => { });
        }

        public void UpdateGui(GUIManager guimanager, float dt)
        {
            while (guimanager.GuiElements.GetNext().visit<bool>((v) => { return true; }, () => { return false; }))
            {
                guimanager.GuiElements.GetCurrent().visit((v) => v.Update(this, dt), () => { });
            }
            guimanager.GuiElements.Reset();
        }

        public void UpdateNote(DrawableNote note, float dt)
        {
            //No updates yet...
        }
    }
}