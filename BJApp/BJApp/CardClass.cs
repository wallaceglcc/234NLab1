using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJApp
{
    public class Card
    {
        #region data

        private static string[] values = new string[13] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
        //fields
        private string s;
        private string v;

        //properties
        public String Suit
        {
            get { return s; }
            private set { s = value; }
        }

        public String Value
        {
            get { return v; }
            private set { v = value; }
        }
        #endregion

        //constructors
        public Card() { }
        public Card(string suit, string value) { Suit = suit; Value = value; }

        #region methods
        //methods, do we need anything besides value for blackjack?
        public int GetValue()
        {
            foreach (string v in values)
            {
                if (Value == v && char.IsDigit(Value, 0))
                {
                    return int.Parse(Value);
                }
                else if (Value == "A") { return 11; }
                else if (Value == "J" || Value == "Q" || Value == "K") { return 10; }
            }
            return 0;
        }

        //To String
        public override string ToString()
        {
            return Value + Suit;
        }
        #endregion
    }
}
