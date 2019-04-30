using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTDClasses
{
    /// <summary>
    /// Represents a generic Train for MTD
    /// </summary>
    public abstract class Train
    {
        private List<Domino> dominos;
        private int engineValue;
        
        //new empty train
        public Train() { dominos = new List<Domino>(); }

        public Train(int engValue)
        {
            dominos = new List<Domino>();
            engineValue = engValue;
        }

        //how many dominos in the train?
        public int Count
        {
            get { return dominos.Count; }
        }

        /// <summary>
        /// The first domino value that must be played on a train
        /// </summary>
        public int EngineValue
        {
            get { return engineValue; }
        }

        //check if the train is null or empty
        public bool IsEmpty
        {
            get { return dominos == null || Count < 1; }
        }

        //check the last domino in the train
        public Domino LastDomino
        {
            get { return dominos[Count - 1]; }
        }

        /// <summary>
        /// Side2 of the last domino in the train.  It's the value of the next domino that can be played.
        /// </summary>
        public int PlayableValue
        {
            get
            {
                if (IsEmpty) { return engineValue; }
                else { return LastDomino.Side2; }
            }
        }

        public void Add(Domino d)
        {
            dominos.Add(d);
        }

        public Domino this[int index]
        {
            get { return dominos[index]; }
        }

        /// <summary>
        /// Determines whether a hand can play a specific domino on this train and if the domino must be flipped.
        /// Because the rules for playing are different for Mexican and Player trains, this method is abstract.
        /// </summary>
        public abstract bool IsPlayable(Hand h, Domino d, out bool mustFlip);

        /// <summary>
        /// A helper method that determines whether a specific domino can be played on this train.
        /// It can be called in the Mexican and Player train class implementations of the abstract method
        /// </summary>
        protected bool IsPlayable(Domino d, out bool mustFlip)
        {
            mustFlip = false;
            if(d.Side1 == PlayableValue) { return true; }
            else if (d.Side2 == PlayableValue) { mustFlip = true; return true; }
            return false;
        }

        // assumes the domino has already been removed from the hand
        public void Play(Hand h, Domino d)
        {
            //add it if you can play it
            if(IsPlayable(h, d, out bool mustFlip)) { dominos.Add(d); }
            //flip it if you gotta
            if (mustFlip) { LastDomino.Flip(); }
        }
        
        //ToString it
        public override string ToString()
        {
            string dominoString = "";
            foreach (Domino d in dominos) { dominoString += d.ToString() + " "; }
            return dominoString;
        }
    }
}