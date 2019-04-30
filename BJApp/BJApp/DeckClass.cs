using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJApp
{
    public class Deck
    {
        #region data
        private static Random rand = new Random();

        const int SUITS = 4;
        const int VALUES = 13;
        private static string[] suits = new string[4] { "S", "D", "C", "H" };
        private static string[] values = new string[13] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

        //fields
        private List<Card> cards = new List<Card> { };

        //properties
        public List<Card> Cards
        {
            get { return cards; }
            private set { cards = value; }
        }
        #endregion

        //constructor
        public Deck()
        {
            FillEmpty();
            NewDeck();
        }

        #region methods
        //fill the deck with new cards
        public void NewDeck()
        {
            int cardcount = 0;
            for (int i = 0; i < SUITS; i++)
            {
                for (int j = 0; j < VALUES; j++)
                {
                    Cards[cardcount] = new Card(suits[i], values[j]);
                    cardcount++;
                }
            }
        }

        //shuffle the deck by swapping each card with a random card
        public void ShuffleDeck()
        {
            Card tempcard;
            for (int i = 0; i < Cards.Count; i++)
            {
                int swapcard = rand.Next(0, cards.Count);
                tempcard = Cards[i];
                Cards[i] = Cards[swapcard];
                Cards[swapcard] = tempcard;
            }
        }
        //returns top card of deck
        public Card GetTopCard()
        {
            return Cards[cards.Count - 1];
        }
        //removes card from deck
        public void RemoveCard(Card card)
        {
            Cards.Remove(card);
        }
        //fills with empty cards so cards can be assigned
        public void FillEmpty()
        {
            for (int i = 0; i < 52; i++)
            {
                cards.Add(new Card());
            }
        }
        //deals a card (combo method)
        public Card DealCard()
        {
            Card tempCard = GetTopCard();
            RemoveCard(GetTopCard());
            return tempCard;
        }
        #endregion
    }

}
