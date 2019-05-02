using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTDClasses
{
    /// <summary>
    /// Represents a hand of dominos
    /// </summary>
    public class Hand
    {
        /// <summary>
        /// The list of dominos in the hand
        /// </summary>
        private List<Domino> handOfDominos;

        public delegate void EmptyHandler(Hand j);
        public event EmptyHandler Empty;

        public void HandleEmpty(Hand h)
        {
        }

        /// <summary>
        /// Creates an empty hand
        /// </summary>
        public Hand()
        {
            handOfDominos = new List<Domino>();
            Empty = new EmptyHandler(HandleEmpty);
        }

        /// <summary>
        /// Creates a hand of dominos from the boneyard.
        /// The number of dominos is based on the number of players
        /// 2–4 players: 10 dominoes each
        /// 5–6 players: 9 dominoes each
        /// 7–8 players: 7 dominoes each
        /// </summary>
        /// <param name="by"></param>
        /// <param name="numPlayers"></param>
        public Hand(BoneYard by, int numPlayers)
        {
            Empty = new EmptyHandler(HandleEmpty);
            handOfDominos = new List<Domino>();
            int numDominos;
            switch (numPlayers)
            {
                case 2:
                case 3:
                case 4:
                    numDominos = 10;
                    break;
                case 5:
                case 6:
                    numDominos = 9;
                    break;
                case 7:
                case 8:
                    numDominos = 7;
                    break;
                default:
                    numDominos = 5;
                    break;
            }
            for (int i = 0; i < numDominos; i++)
                handOfDominos.Add(by.Draw());
        }

        public void Add(Domino d)
        {
            handOfDominos.Add(d);
        }


        public int Count
        {
            get
            {
                return handOfDominos.Count;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return Count == 0;
            }
        }

        /// <summary>
        /// Sum of the score of each domino in the hand
        /// </summary>
        public int Score
        {
            get
            {
                int score = 0;
                foreach (Domino d in handOfDominos)
                    score += d.Score;
                return score;

            }
        }

        /// <summary>
        /// Does the hand contain a domino with value in side 1 or side 2?
        /// </summary>
        /// <param name="value">The number of dots on one side of the domino that you're looking for</param>
        public bool HasDomino(int value)
        {
            foreach (Domino d in handOfDominos)
                if (d.Side1 == value || d.Side2 == value)
                    return true;
            return false;
        }

        /// <summary>
        ///  DOes the hand contain a double of a certain value?
        /// </summary>
        /// <param name="value">The number of (double) dots that you're looking for</param>
        public bool HasDoubleDomino(int value)
        {
            foreach (Domino d in handOfDominos)
                if (d.Side1 == value && d.Side2 == value)
                    return true;
            return false;
        }

        /// <summary>
        /// The index of a domino with a value in the hand
        /// </summary>
        /// <param name="value">The number of dots on one side of the domino that you're looking for</param>
        /// <returns>-1 if the domino doesn't exist in the hand</returns>
        public int IndexOfDomino(int value)
        {
            int i = 0;
            foreach (Domino d in handOfDominos)
            {
                if (d.Side1 == value || d.Side2 == value)
                    return i;
                i++;
            }
            return -1;
        }

        /// <summary>
        /// The index of the do
        /// </summary>
        /// <param name="value">The number of (double) dots that you're looking for</param>
        /// <returns>-1 if the domino doesn't exist in the hand</returns>
        public int IndexOfDoubleDomino(int value)
        {
            int i = 0;
            foreach (Domino d in handOfDominos)
            {
                if (d.Side1 == value && d.Side2 == value)
                    return i;
                i++;
            }
            return -1;
        }

        /// <summary>
        /// The index of the highest double domino in the hand
        /// </summary>
        /// <returns>-1 if there isn't a double in the hand</returns>
        public int IndexOfHighDouble()
        {
            for (int i = 12; i > 0; i--)
            {
                int indexOfHigh = IndexOfDoubleDomino(i);
                if (indexOfHigh != -1)
                    return indexOfHigh;
            }
            return -1;
        }

        public Domino this[int index]
        {
            get
            {
                return handOfDominos[index];
            }

        }

        public void RemoveAt(int index)
        {
            handOfDominos.RemoveAt(index);
            if (handOfDominos.Count == 0)
                Empty(this);
        }

        /// <summary>
        /// Finds a domino with a certain number of dots in the hand.
        /// If it can find the domino, it removes it from the hand and returns it.
        /// Otherwise it returns null
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Domino GetDomino(int value)
        {
            int position = IndexOfDomino(value);
            if (position == -1)
                return null;
            else
            {
                Domino d = handOfDominos[position];
                handOfDominos.RemoveAt(position);
                if (d.Side1 == value)
                    return d;
                else
                {
                    d.Flip();
                    return d;
                }
            }
        }

        /// <summary>
        /// Finds a domino with a certain number of double dots in the hand.
        /// If it can find the domino, it removes it from the hand and returns it.
        /// Otherwise it returns null
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Domino GetDoubleDomino(int value)
        {
            int position = IndexOfDoubleDomino(value);
            if (position == -1)
                return null;
            else
            {
                Domino d = handOfDominos[position];
                handOfDominos.RemoveAt(position);
                return d;
            }
        }

        /// <summary>
        /// Draws a domino from the boneyard and adds it to the hand
        /// </summary>
        /// <param name="by"></param>
        public void Draw(BoneYard by)
        {
            Add(by.Draw());
        }

        /// <summary>
        /// Plays the domino at the index on the train.
        /// Flips the domino if necessary before playing.
        /// Removes the domino from the hand.
        /// Throws an exception if the domino at the index
        /// is not playable.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="t"></param>
        private void Play(int index, Train t)
        {
            bool mustFlip = false;
            Domino d = handOfDominos[index];
            if (t.IsPlayable(this, d, out mustFlip))
            {
                handOfDominos.RemoveAt(index);
                if (mustFlip)
                    d.Flip();
                t.Play(this, d);
            }
            else
            {
                throw new Exception("Domino " + d.ToString() + " cannot be played on this train.");
            }
        }

        /// <summary>
        /// Plays the domino from the hand on the train.
        /// Flips the domino if necessary before playing.
        /// Removes the domino from the hand.
        /// Throws an exception if the domino is not in the hand
        /// or is not playable.
        /// </summary>
        public void Play(Domino d, Train t)
        {
            int index = handOfDominos.IndexOf(d);
            if (index != -1)
            {
                Play(index, t);
            }
        }

        /// <summary>
        /// Plays the first playable domino in the hand on the train
        /// Removes the domino from the hand.
        /// Returns the domino.
        /// Throws an exception if no dominos in the hand are playable.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public Domino Play(Train t)
        {
            int playableValue = t.PlayableValue;
            int index = IndexOfDomino(playableValue);
            if (index != -1)
            {
                Domino playable = this[index];
                Play(index, t);
                return playable;
            }
            else
            {
                throw new Exception("No play from this hand on this train.");
            }
        }

        public override string ToString()
        {
            string output = "";
            int index = 0;
            foreach (Domino d in handOfDominos)
            {
                output += index + ": " + d.ToString() + "\n";
                index++;
            }
            output += "\n";
            return output;
        }
    }
}
