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

        //The hand
        private List<Domino> hand;

        /// <summary>
        /// Creates an empty hand
        /// </summary>
        public Hand()
        {
            hand = new List<Domino>();
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
            //how much do you draw?
            int dominoesDrawn;
            //let's find out
            if(numPlayers < 2) { throw new ArgumentException("You need at least 2 players"); }
            else if (numPlayers < 5) { dominoesDrawn = 10; }
            else if (numPlayers < 7) { dominoesDrawn = 9; }
            else { dominoesDrawn = 7; }

            //draw those dominoes!
            for(int i = 0; i < dominoesDrawn; i++) { hand.Add(by.Draw()); }
        }

        public void Add(Domino d)
        {
            //add a domino to hand
            hand.Add(d);
        }


        public int Count
        {
            //how many dominos in the hand?
            get { return hand.Count; }
        }

        public bool IsEmpty
        {
            //is the hand empty?
            get { return hand == null || Count < 1; }
        }

        /// <summary>
        /// Sum of the score of each domino in the hand
        /// </summary>
        public int Score
        {
            //add up the scores and return them
            get
            {
                int score = 0;
                foreach(Domino d in hand) { score += d.Score; }
                return score;
            }
        }

        /// <summary>
        /// Does the hand contain a domino with value in side 1 or side 2?
        /// </summary>
        /// <param name="value">The number of dots on one side of the domino that you're looking for</param>
        public bool HasDomino(int value)
        {
            //if a domino in hand matches, return true
            foreach(Domino d in hand)
            {
                if (d.Side1 == value || d.Side2 == value) { return true; }
            }
            return false;
        }

        /// <summary>
        ///  DOes the hand contain a double of a certain value?
        /// </summary>
        /// <param name="value">The number of (double) dots that you're looking for</param>
        public bool HasDoubleDomino(int value)
        {
            //if both sides on a domino match, return true
            foreach (Domino d in hand)
            {
                if (d.Side1 == value && d.Side2 == value) { return true; }
            }
            return false;
        }

        /// <summary>
        /// The index of a domino with a value in the hand
        /// </summary>
        /// <param name="value">The number of dots on one side of the domino that you're looking for</param>
        /// <returns>-1 if the domino doesn't exist in the hand</returns>
        public int IndexOfDomino(int value)
        {
            foreach (Domino d in hand)
            {
                if (d.Side1 == value || d.Side2 == value) { return hand.IndexOf(d); }
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
            foreach (Domino d in hand)
            {
                if (d.Side1 == value && d.Side2 == value) { return hand.IndexOf(d); }
            }
            return -1;
        }

        /// <summary>
        /// The index of the highest double domino in the hand
        /// </summary>
        /// <returns>-1 if there isn't a double in the hand</returns>
        public int IndexOfHighDouble()
        {
            int highest = -1;
            int indexof = -1;
            foreach (Domino d in hand)
            {
                if (d.Side1 == d.Side2 && d.Side1 > highest)
                {
                    highest = d.Side1;
                    indexof = hand.IndexOf(d);
                }
            }
            return indexof;
        }

        //indexer
        public Domino this[int index]
        {
            get { return hand[index]; }
            set { hand[index] = value; }
        }

        //removes a domino at given index
        public void RemoveAt(int index)
        {
            hand.RemoveAt(index);
        }
        

        /*


        /// <summary>
        /// Finds a domino with a certain number of dots in the hand.
        /// If it can find the domino, it removes it from the hand and returns it.
        /// Otherwise it returns null
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Domino GetDomino(int value)
        {
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
        }

        /// <summary>
        /// Draws a domino from the boneyard and adds it to the hand
        /// </summary>
        /// <param name="by"></param>
        public void Draw(BoneYard by)
        {
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
        }

        public override string ToString()
        {
        }
        */
    }
}
