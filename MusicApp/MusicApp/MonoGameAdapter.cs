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
using Android.Util;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace MusicApp
{
    public class MonoGameGraphicsAdapter : IGraphicsManager
    {
        private GraphicsDeviceManager Manager;
        public MonoGameGraphicsAdapter(GraphicsDeviceManager manager) { this.Manager = manager; }

        public int GetScreenHeight { get { return this.Manager.PreferredBackBufferHeight; } }
        public int GetScreenWidth { get { return this.Manager.PreferredBackBufferWidth; } }
    }

    public class MonoGameDrawAdapter : IDrawManager
    {
        private SpriteBatch SpriteBatch;
        private ContentManager ContentManager;
        private Texture2D Pixel;
        public MonoGameDrawAdapter(SpriteBatch spritebatch, ContentManager content_manager)
        {
            this.SpriteBatch = spritebatch;
            this.ContentManager = content_manager;
            this.Pixel = ContentManager.Load<Texture2D>("pixel");
        }

        public void DrawRectangle(Position top_left_coordinate, int width, int height, Colour color)
        {
            SpriteBatch.Draw(Pixel,new Rectangle(top_left_coordinate.X,top_left_coordinate.Y,width,height),MonoGameConverter.ColorConverter(color));
        }
    }

    public class MonoGameConverter
    {
        public static Color ColorConverter(Colour colour)
        {
            return new Color(colour.getRGB[0],colour.getRGB[1],colour.getRGB[2]);
        }
    }

    public class MonoGameInputAdapter : InputManager
    {
        public IOption<Position> GetTouchPosition()
        {
            TouchCollection touched = TouchPanel.GetState();
            if (touched.Count > 0)
                return new Some<Position>(new Position((int)touched[0].Position.X, (int)touched[0].Position.Y));
            return new None<Position>();
        }

        public bool isPressed()
        {
            TouchLocation PosTouched = new TouchLocation();
            if (PosTouched.State == TouchLocationState.Pressed) return true;
            return false;
        }
    }

    public class MonoGameSoundAdapter : ISoundManager
    {
        private Iterator<SoundEffectInstance> MusicList;
        private Iterator<SoundEffectInstance> Song;
        private ContentManager contentManager;
        private SoundEffect A, B, Bsharp, C, Csharp, D, E, Esharp, F, Fsharp, G, Gsharp;
        public MonoGameSoundAdapter(ContentManager content_manager)
        {
            this.MusicList = new ArrayItterator<SoundEffectInstance>();
            this.Song = new ArrayItterator<SoundEffectInstance>();
            this.contentManager = content_manager;
            this.A = contentManager.Load<SoundEffect>("piano_a");
            this.B = contentManager.Load<SoundEffect>("piano_b");
            this.Bsharp = contentManager.Load<SoundEffect>("piano_bb");
            this.C = contentManager.Load<SoundEffect>("piano_c");
            this.Csharp = contentManager.Load<SoundEffect>("piano_cs");
            this.D = contentManager.Load<SoundEffect>("piano_d");
            this.E = contentManager.Load<SoundEffect>("piano_e");
            this.Esharp = contentManager.Load<SoundEffect>("piano_eb");
            this.F = contentManager.Load<SoundEffect>("piano_f");
            this.Fsharp = contentManager.Load<SoundEffect>("piano_fs");
            this.G = contentManager.Load<SoundEffect>("piano_g");
            this.Gsharp = contentManager.Load<SoundEffect>("piano_gs");
        }

        public void PlaySingleNote(string note)
        {
            IOption<SoundEffectInstance> soundInstance = Create(note);
            soundInstance.visit((snd) => 
            {
                    snd.IsLooped = false;
                     snd.Play();
            }
            ,()=> { });
        }

        public void AddSoundToSong(string note)
        {
            Create(note).visit((snd) => Song.Add(snd), () => { });
        }

        public void PlaySong()
        {
            while (MusicList.GetNext().visit<bool>((snd) => true, () => false)) MusicList.GetCurrent().visit((snd) => { snd.Play(); }, () => { });
        }

        public void Play(string note)
        {
            Create(note).visit((snd)=>MusicList.Add(snd),()=> { });
            
            while(MusicList.GetNext().visit<bool>((snd)=>true,()=>false)) MusicList.GetCurrent().visit((snd) => { snd.Play();}, () => { });
            MusicList.DeleteAll();
        }

        private IOption<SoundEffectInstance> Create(string note)
        {
            IOption<SoundEffectInstance> soundInstance;
            switch (note)
            {
                case "A":
                    soundInstance = new Some<SoundEffectInstance>(this.A.CreateInstance());
                    break;
                case "B":
                    soundInstance = new Some<SoundEffectInstance>(this.B.CreateInstance());
                    break;
                case "B#":
                    soundInstance = new Some<SoundEffectInstance>(this.Bsharp.CreateInstance());
                    break;
                case "C":
                    soundInstance = new Some<SoundEffectInstance>(this.C.CreateInstance());
                    break;
                case "C#":
                    soundInstance = new Some<SoundEffectInstance>(this.Csharp.CreateInstance());
                    break;
                case "D":
                    soundInstance = new Some<SoundEffectInstance>(this.D.CreateInstance());
                    break;
                case "E":
                    soundInstance = new Some<SoundEffectInstance>(this.E.CreateInstance());
                    break;
                case "E#":
                    soundInstance = new Some<SoundEffectInstance>(this.Esharp.CreateInstance());
                    break;
                case "F":
                    soundInstance = new Some<SoundEffectInstance>(this.F.CreateInstance());
                    break;
                case "F#":
                    soundInstance = new Some<SoundEffectInstance>(this.Fsharp.CreateInstance());
                    break;
                case "G":
                    soundInstance = new Some<SoundEffectInstance>(this.G.CreateInstance());
                    break;
                case "G#":
                    soundInstance = new Some<SoundEffectInstance>(this.Gsharp.CreateInstance());
                    break;
                default:
                    soundInstance = new None<SoundEffectInstance>();
                    break;
            }
            return soundInstance;
        }


    }
}