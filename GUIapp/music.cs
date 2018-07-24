using System;
using System.Threading;

namespace GUIapp
{
    class Music
    {
        public static ILinkedList<Note> BuildSong(int select)
        {
            NoteFactory SongBuilder = new ConcreteNoteFactory();
            ILinkedList<Note> Song = new Empty<Note>();
            switch (select) {
                case 1:
                    SongBuilder.Create("C", Duration.HALF).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));
                    SongBuilder.Create("F", Duration.QUARTER).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));
                    SongBuilder.Create("C", Duration.HALF).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));
                    SongBuilder.Create("G", Duration.QUARTER).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));

                    SongBuilder.Create("C", Duration.QUARTER).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));
                    SongBuilder.Create("A", Duration.EIGHTH).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));
                    SongBuilder.Create("G", Duration.QUARTER).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));
                    SongBuilder.Create("C", Duration.EIGHTH).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));

                    SongBuilder.Create("G", Duration.QUARTER).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));
                    SongBuilder.Create("C", Duration.EIGHTH).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));
                    SongBuilder.Create("G", Duration.HALF).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));
                    SongBuilder.Create("C", Duration.WHOLE).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));

                    SongBuilder.Create("G", Duration.QUARTER).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));
                    SongBuilder.Create("C", Duration.HALF).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));

                    SongBuilder.Create("rest", Duration.EIGHTH).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));
                    break;
                case 2:
                    //Couplet 1
                    SongBuilder.Create("D",Duration.HALF).Visit(()=>Do.Nothing(), (note)=> Song = Song.Add(note));
                    SongBuilder.Create("C",Duration.HALF).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));
                    SongBuilder.Create("E", Duration.HALF).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));

                    SongBuilder.Create("D", Duration.HALF).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));
                    SongBuilder.Create("C", Duration.QUARTER).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));
                    SongBuilder.Create("E", Duration.HALF).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));

                    SongBuilder.Create("D", Duration.HALF).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));
                    SongBuilder.Create("E", Duration.QUARTER).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));
                    SongBuilder.Create("C", Duration.HALF).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));

                    SongBuilder.Create("rest", Duration.HALF).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));
                    //Couplet 2
                    SongBuilder.Create("D", Duration.QUARTER).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));
                    SongBuilder.Create("C", Duration.EIGHTH).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));
                    SongBuilder.Create("E", Duration.QUARTER).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));

                    SongBuilder.Create("D", Duration.QUARTER).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));
                    SongBuilder.Create("C", Duration.EIGHTH).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));
                    SongBuilder.Create("E", Duration.QUARTER).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));

                    SongBuilder.Create("D", Duration.QUARTER).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));
                    SongBuilder.Create("E", Duration.EIGHTH).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));
                    SongBuilder.Create("C", Duration.QUARTER).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));

                    //Couplet 3
                    SongBuilder.Create("D", Duration.EIGHTH).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));
                    SongBuilder.Create("C", Duration.EIGHTH).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));
                    SongBuilder.Create("E", Duration.EIGHTH).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));

                    SongBuilder.Create("D", Duration.EIGHTH).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));
                    SongBuilder.Create("C", Duration.EIGHTH).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));
                    SongBuilder.Create("E", Duration.EIGHTH).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));

                    SongBuilder.Create("D", Duration.EIGHTH).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));
                    SongBuilder.Create("E", Duration.EIGHTH).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));
                    SongBuilder.Create("C", Duration.EIGHTH).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));

                    //Outro
                    SongBuilder.Create("D", Duration.EIGHTH).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));
                    SongBuilder.Create("C", Duration.QUARTER).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));
                    SongBuilder.Create("E", Duration.QUARTER).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));

                    SongBuilder.Create("F", Duration.EIGHTH).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));
                    SongBuilder.Create("G", Duration.QUARTER).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));
                    SongBuilder.Create("G#", Duration.QUARTER).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));

                    SongBuilder.Create("F#", Duration.HALF).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));
                    SongBuilder.Create("B", Duration.HALF).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));
                    SongBuilder.Create("A", Duration.HALF).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));
                    SongBuilder.Create("A#", Duration.HALF).Visit(() => Do.Nothing(), (note) => Song = Song.Add(note));
                    break;
        }

            return Song.Reverse();//The song needs to be reversed, cause the add function adds every item to the first place
        }

        public static void PlayNote(Note SingleNote)
        {
            SingleNote.Visit(()=>SingleNote.Play(),(duration)=>Thread.Sleep((int)duration));
        }

        public static void PlaySong(ILinkedList<Note> song)
        {
            Iterator<Note> SongIterartor = new LinkedListIterator<Note>(song);
            SongIterartor.getCurrent().Visit(() => Do.Nothing(), (note) => note.Visit(() => note.Play(), (duration) => Thread.Sleep(duration)));
            while (SongIterartor.hasNext())
            {
                SongIterartor.moveNext().Visit(() => Console.WriteLine("!!!______END__SONG______!!!"),
                    (note) => note.Visit(() => note.Play(),
                    (duration) => Thread.Sleep((int)duration)));
            }
        }
    }

    abstract class NoteFactory
    {
        public abstract IOption<Note> Create(string tone,Duration time);
    }

    class ConcreteNoteFactory : NoteFactory
    {
        public override IOption<Note> Create(string tone, Duration time)
        {
            switch (tone)
            {
                case "A":
                    return new Some<Note>(new Note(Tone.A,time));
                case "A#":
                    return new Some<Note>(new Note(Tone.Asharp, time));
                case "B":
                    return new Some<Note>(new Note(Tone.B, time));
                case "C":
                    return new Some<Note>(new Note(Tone.C, time));
                case "C#":
                    return new Some<Note>(new Note(Tone.Csharp, time));
                case "D":
                    return new Some<Note>(new Note(Tone.D, time));
                case "D#":
                    return new Some<Note>(new Note(Tone.Dsharp, time));
                case "E":
                    return new Some<Note>(new Note(Tone.E, time));
                case "F":
                    return new Some<Note>(new Note(Tone.F, time));
                case "F#":
                    return new Some<Note>(new Note(Tone.Fsharp, time));
                case "G":
                    return new Some<Note>(new Note(Tone.G, time));
                case "G#":
                    return new Some<Note>(new Note(Tone.Gsharp, time));
                case "GbelowC":
                    return new Some<Note>(new Note(Tone.GbelowC, time));
                case "rest":
                    return new Some<Note>(new Note(Tone.REST, time));
                default:
                    return new None<Note>();

            }
        }
    }

    public enum Tone
    {
        REST = 0,
        GbelowC = 196,
        A = 220,
        Asharp = 233,
        B = 247,
        C = 262,
        Csharp = 277,
        D = 294,
        Dsharp = 311,
        E = 330,
        F = 349,
        Fsharp = 370,
        G = 392,
        Gsharp = 415,
    }

   public enum Duration
    {
        WHOLE = 1600,
        HALF = WHOLE / 2,
        QUARTER = HALF / 2,
        EIGHTH = QUARTER / 2,
        SIXTEENTH = EIGHTH / 2,
    }
    class Note
    {
        private Tone Tone;
        private Duration Duration;
        public Note(Tone Frequency, Duration time)
        {
            this.Tone = Frequency;
            this.Duration = time;
        }

        public void Visit(Action OnPlay, Action<int> OnRest)
        {
            if (this.Tone == Tone.REST)
            {
                OnRest((int)this.Duration);
            }
            else
            {
                OnPlay();
            }
        }

        public void Play()
        {
            Console.Beep((int)this.Tone, (int)this.Duration);
        }
    }
}
