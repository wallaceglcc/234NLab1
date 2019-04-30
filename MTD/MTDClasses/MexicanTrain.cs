using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTDClasses
{
    public class MexicanTrain : Train
    {
        //constructor calling base constructor
        public MexicanTrain() : base() { }

        //constructor calling base constructor
        public MexicanTrain(int engValue) : base(engValue) { }

        //override bool for mexican train
        public override bool IsPlayable(Hand h, Domino d, out bool mustFlip)
        {
            return IsPlayable(d, out mustFlip);
        }
    }
}
