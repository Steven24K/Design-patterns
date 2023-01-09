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

using Microsoft.Xna.Framework.Audio;

namespace MusicMixer
{
    public class Note
    {
        private SoundEffectInstance Tone;
        private IStateMachine SM;
        public Note(SoundEffect tone, float duration)
        {
            this.Tone = tone.CreateInstance();
            this.Tone.IsLooped = false;
            this.SM = new Sequence(new Call(()=>this.Tone.Play()),new Sequence(new Timer(duration),new Call(()=>this.Tone.Stop())));
        }

        public void Play()
        {
            this.SM.Update(1.0f);
        }
    }
}