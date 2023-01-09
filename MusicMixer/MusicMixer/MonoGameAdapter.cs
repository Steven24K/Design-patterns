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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace MusicMixer
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
            SpriteBatch.Draw(Pixel, new Rectangle(top_left_coordinate.X, top_left_coordinate.Y, width, height), MonoGameConverter.ColorConverter(color));
        }
    }

    public class MonoGameConverter
    {
        public static Color ColorConverter(Colour colour)
        {
            return new Color(colour.getRGB[0], colour.getRGB[1], colour.getRGB[2]);
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
        private Iterator<Note> MusicList;//used to play one single sound, is deleted after each play
        private Iterator<Note> SongList;//The actual song
        private ContentManager contentManager;
        private SoundEffect A, B, Bsharp, C, Csharp, D, E, Esharp, F, Fsharp, G, Gsharp;

        public Note[] Song { get { return SongList.GetCollection(); } }

        public int AmountOfNotes { get { return SongList.GetAmountOfItems; } }

        public Iterator<Note> GetSongList { get { return SongList; } }

        public MonoGameSoundAdapter(ContentManager content_manager)
        {
            this.MusicList = new ArrayItterator<Note>();
            this.SongList = new ArrayItterator<Note>();
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

        public void AddSoundToSong(string note)
        {
            Create(note).visit((snd) => SongList.Add(snd), () => { });
        }

        public void PlaySong()
        {
            while (SongList.GetNext().visit<bool>((snd) => true, () => false))
            {
                SongList.GetCurrent().visit((snd) => snd.Play(), () => { });
            }
            SongList.Reset();
        }

        public void PlayNote(string note)
        {
            Create(note).visit((snd) => MusicList.Add(snd), () => { });

            while (MusicList.GetNext().visit<bool>((snd) => true, () => false)) MusicList.GetCurrent().visit((snd) =>snd.Play(), () => { });
            MusicList.DeleteAll();
        }

        private IOption<Note> Create(string note)
        {
            IOption<Note> soundInstance;
            switch (note)
            {
                case "A":
                    soundInstance = new Some<Note>(new Note(this.A,1.0f));
                    break;
                case "B":
                    soundInstance = new Some<Note>(new Note(this.B, 1.0f));
                    break;
                case "B#":
                    soundInstance = new Some<Note>(new Note(this.Bsharp, 1.0f));
                    break;
                case "C":
                    soundInstance = new Some<Note>(new Note(this.C, 1.0f));
                    break;
                case "C#":
                    soundInstance = new Some<Note>(new Note(this.Csharp, 1.0f));
                    break;
                case "D":
                    soundInstance = new Some<Note>(new Note(this.D, 1.0f));
                    break;
                case "E":
                    soundInstance = new Some<Note>(new Note(this.E, 1.0f));
                    break;
                case "E#":
                    soundInstance = new Some<Note>(new Note(this.Esharp, 1.0f));
                    break;
                case "F":
                    soundInstance = new Some<Note>(new Note(this.F, 1.0f));
                    break;
                case "F#":
                    soundInstance = new Some<Note>(new Note(this.Fsharp, 1.0f));
                    break;
                case "G":
                    soundInstance = new Some<Note>(new Note(this.G, 1.0f));
                    break;
                case "G#":
                    soundInstance = new Some<Note>(new Note(this.Gsharp, 1.0f));
                    break;
                default:
                    soundInstance = new None<Note>();
                    break;
            }
            return soundInstance;
        }

        public void DeleteLastNote()
        {
            SongList.DeleteLast();
        }
    }
}