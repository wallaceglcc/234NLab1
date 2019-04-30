using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTDClasses
{
    [Serializable()]
    public class Domino : IComparable<Domino>
    {
        //fields
        private int side1;
        private int side2;
        private int score;

        public Domino()
        {
            side1 = 0;
            side2 = 0;
        }

        public Domino(int p1, int p2)
        {
            //Set domino to given numbers
            Side1 = p1;
            Side2 = p2;
        }

        // don't use an auto implemented property because of the validation in the setter - p 390
        public int Side1
        {
            get { return side1; }
            set
            {
                if (value >= 0 && value <= 12) { side1 = value; }
                else { throw new ArgumentException("Dots must be between 0 and 12"); }
            }
        }


        public int Side2
        {
            get { return side2; }
            set
            {
                if (value >= 0 && value <= 12) { side2 = value; }
                else { throw new ArgumentException("Dots must be between 0 and 12"); }
            }
        }

        public void Flip()
        {
            //store 1 in a temp int
            int temp = Side1;
            //set 1 to 2
            Side1 = Side2;
            //set 2 to temp
            Side2 = temp;
        }

        /// This is how I would have done this in 233N
        public int Score
        {
            //return total of both sides
            get { return Side1 + Side2; }
        }

        // because it's a read only property, I can use the "expression bodied syntax" or a lamdba expression - p 393
        //public int Score => side1 + side2;

        //ditto for the first version of this method and the next one
        public bool IsDouble()
        {
            //true if sides are equal
            return Side1 == Side2;
        }

        // could you do this one using a lambda expression?
        public string Filename
        {
            get
            {
                return String.Format("d{0}{1}.png", side1, side2);
            }
        }

        //public bool IsDouble() => (side1 == side2) ? true : false;

        public override string ToString()
        {
            return String.Format("{0} {1}", side1, side2);
        }

        // could you overload the == and != operators?
        //sure

        // == override
        public static bool operator == (Domino a, Domino b)
        {
            //true if equal
            return a.Equals(b);
        }

        // != override
        public static bool operator !=(Domino a, Domino b)
        {
            //false if equal
            //return !a.Equals(b);
            return !(a == b);
        }


        public override bool Equals(object obj)
        {
            //check if obj is null or wrong type
            if(obj == null || this.GetType() != obj.GetType()) { return false; }
            //cast to domino
            Domino b = (Domino)obj;
            //compare
            return (Side1 == b.Side1 && Side2 == b.Side2 || Side1 == b.Side2 && Side2 == b.Side1);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public int CompareTo(Domino other)
        {
            return Score.CompareTo(other.Score);
        }
    }
}
