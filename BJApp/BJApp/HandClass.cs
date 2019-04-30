using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJApp
{
    public class Hand
    {
        #region data
        //fields
        private List<Card> cards;

        //properties
        public List<Card> Cards
        {
            get { return cards; }
            private set { cards = value; }
        }
        #endregion

        //default constructor
        public Hand() { cards = new List<Card> { }; }

        //constructor that draws
        public Hand(Deck d, int numCards)
        {
            cards = new List<Card> { };
            for (int i = 0; i < numCards; i++) { Cards.Add(d.DealCard()); }
        }

        #region methods
        //methods
        //adds the top card of the deck to hand, removes it from deck, returns that card
        public Card DealCard(Deck deck)
        {
            Card dealtCard = deck.GetTopCard();
            Cards.Add(dealtCard);
            deck.RemoveCard(dealtCard);
            return dealtCard;
        }

        //Display your hand
        public override string ToString()
        {
            string handstring = "";
            foreach (Card c in Cards)
            {
                handstring += c.ToString() + " ";
            }
            return handstring;
        }
        #endregion

        //how many cards in hand?
        public int NumCards { get { return Cards.Count; } }

        //add a card to hand
        public void AddCard(Card c) { Cards.Add(c); }

        //get card by index
        public Card GetCard(int index) { return Cards[index]; }

        //Get index of card by card
        public int GetIndexOf(Card c)
        {
            foreach(Card card in Cards)
            {
                if (c.Suit == card.Suit && c.Value == card.Value) { return Cards.IndexOf(card); }
            }
            return -1;
        }

        //get index of card by value
        public int GetIndexOf(string value)
        {
            foreach(Card card in Cards)
            {
                if(card.Value == value) { return Cards.IndexOf(card); }
            }
            return -1;
        }

        public int GetIndexOf(string value, string suit)
        {
            foreach(Card card in cards)
            {
                if (card.Value == value && card.Suit == suit ) { return Cards.IndexOf(card); }
            }
            return -1;
        }

        //check if card is in hand by card
        public bool HasCard(Card c) { return GetIndexOf(c) != -1; }

        //check if card is in hand by value
        public bool HasCard(string value) { return GetIndexOf(value) != -1; }

        //check if card is in hand by value and suit
        public bool HasCard(string value, string suit) { return GetIndexOf(value, suit) != -1; }

        //discard card by index
        public Card Discard(int index)
        {
            Card tempcard = cards[index];
            cards.Remove(cards[index]);
            return tempcard;
        }
    }

}
