using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTDClasses
{
    public class BoneYard : IEnumerable<Domino>
    {
        Random rand = new Random();

        public delegate void EmptyHandler(BoneYard by);
        public event EmptyHandler Empty;

        //fields
        private List<Domino> deck;
        private int maxDots;
        
        //How many dominos left
        public int DominosRemaining
        {
            get { return deck.Count; }
        }

        //constructor
        public BoneYard(int md)
        {
            maxDots = md;
            NewBonyYard();
            Empty = new EmptyHandler(HandleEmpty);
        }

        public void HandleEmpty(BoneYard by) { }

        //Creates new bony yard 
        public void NewBonyYard()
        {
            deck = new List<Domino>();
            for (int side1 = 0; side1 <= maxDots; side1++)
            {
                for(int side2 = side1; side2 <= maxDots; side2++)
                {
                    deck.Add(new Domino(side1, side2));
                }
            }
        }

        //complete this
        public void Shuffle()
        {
            //stuff we need
            Domino tempDomino;
            int randomDomino;

            //shuffle the deck
            for(int i = 0; i < DominosRemaining; i++)
            {
                //get a random domino
                randomDomino = rand.Next(DominosRemaining);
                //store current domino
                tempDomino = deck[i];
                //assign random domino to current domino
                deck[i] = deck[randomDomino];
                //assign temp domino to random domino
                deck[randomDomino] = tempDomino;
            }
        }

        //is the deck empty?
        public bool IsEmpty()
        {
            return deck.Count == 0 || deck == null;
        }

        //complete this
        public Domino Draw()
        {
            if (!IsEmpty())
            {
                //set last domino to a temp
                Domino tempDomino = deck[deck.Count - 1];
                //remove last domino
                deck.RemoveAt(deck.Count - 1);
                //return temp
                if (IsEmpty()) { Empty(this); }
                return tempDomino;
            }
            else
            {
                throw new Exception("BonyYard is empty");
            }
        }

        
        public Domino this[int index]
        {
            get { return deck[index]; }
        }

        public override string ToString()
        {
            string deckList = "";
            foreach(Domino d in deck)
            {
                deckList += d.Side1.ToString() + d.Side2.ToString() + " ";
            }
            return deckList;
        }

        public IEnumerator<Domino> GetEnumerator()
        {
            return deck.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
    
}
