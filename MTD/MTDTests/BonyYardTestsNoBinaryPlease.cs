using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MTDClasses;
using NUnit.Framework;

namespace MTDTests
{
    [TestFixture]
    public class BonyYardTestsNoBinaryPlease
    {
        Domino def;
        Domino d12;
        Domino d21;
        Domino d33;
        BoneYard b6;
        BoneYard b62;

        [SetUp]
        public void SetUpAllTests()
        {
            def = new Domino();
            d12 = new Domino(1, 2);
            d21 = new Domino(2, 1);
            d33 = new Domino(3, 3);
            b6 = new BoneYard(6);
            b62 = new BoneYard(6);
        }

        [Test]
        public void TestForEach()
        {
            foreach(Domino d in b6)
            {
                Console.WriteLine(d);
            }
        }
    }
}
