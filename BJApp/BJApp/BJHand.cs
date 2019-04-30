using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJApp
{
    public class BJHand : Hand
    {
        public BJHand() : base() { }

        public BJHand(Deck d, int numCards) :base(d, numCards) { }
        //totals the hand
        public int TotalHand()
        {
            int handTotal = 0;
            bool ace = false;
            //add each card
            foreach (Card c in Cards)
            {
                handTotal += c.GetValue();
                if (c.GetValue() == 11) { ace = true; }
            }
            //if you're over 21 subtract 10 for each ace
            if (handTotal > 21 && ace)
            {
                handTotal -= 10;
            }
            return handTotal;
        }

        //is there an ace?
        public bool HasAce { get { return HasCard("A"); } }

        //is the hand a bust?
        public bool IsBust { get { return TotalHand() > 21; } }

        //Display Dealer hand
        public string DealerToString()
        {
            string handstring = "";
            bool firstcard = true;
            foreach (Card c in Cards)
            {
                if (firstcard) { handstring += "??" + " "; firstcard = false; }
                else { handstring += c.ToString() + " "; }
            }
            return handstring;
        }
    }
}
