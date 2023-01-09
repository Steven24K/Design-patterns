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
using Android.Util;

namespace MusicApp
{
    public interface IDrawVisitor
    {
        void DrawGui(GUIManager guimanager);
        void DrawButton(MonoButton button);
    }

    public interface IUpdateVisitor
    {
        void UpdateGui(GUIManager guimanager, float dt);
        void UpdateButton(MonoButton button, float dt);
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
            DrawManager.DrawRectangle(new Position(button.TopLeftCorner.X,button.TopLeftCorner.Y),button.Width,button.Height,button.Color);
        }

        public void DrawGui(GUIManager guimanager)
        {
            while (guimanager.GuiElements.GetNext().visit<bool>((v) => { return true; }, () => { return false; }))
            {
                guimanager.GuiElements.GetCurrent().visit((v) => v.Draw(this), () => { });
            }
            guimanager.GuiElements.Reset();
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
                 Thread.Sleep(1000);
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
                guimanager.GuiElements.GetCurrent().visit((v) => v.Update(this,dt), () => { });
            }
            guimanager.GuiElements.Reset();
        }
    }
}