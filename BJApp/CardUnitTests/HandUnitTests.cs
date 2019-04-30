using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using BJApp;

namespace CardUnitTests
{
    [TestFixture]
    class HandUnitTests
    {
        Hand hand1;
        Hand hand2;
        BJHand hand3;
        BJHand hand4;
        Deck d;

        [SetUp]
        public void SetUpAllTests()
        {
            d = new Deck();
            d.NewDeck();
            hand1 = new Hand();
            hand2 = new Hand(d, 5);
            hand3 = new BJHand();
            hand4 = new BJHand(d, 5);
        }

        [Test]
        public void TestConstructors()
        {
            Assert.True(hand1 != null);
            Assert.True(hand2 != null);
            Assert.True(hand3 != null);
            Assert.True(hand4 != null);
        }

        [Test]
        public void TestNumCards()
        {
            Assert.True(hand1.NumCards == 0);
            Assert.True(hand2.NumCards == 5);
            Assert.True(hand3.NumCards == 0);
            Assert.True(hand4.NumCards == 5);
        }

        [Test]
        public void TestDealCard()
        {
            Card a = d.GetTopCard();
            hand1.DealCard(d);
            Card b = d.GetTopCard();
            hand2.DealCard(d);
            Card c = d.GetTopCard();
            hand3.DealCard(d);
            Card e = d.GetTopCard();
            hand4.DealCard(d);
            Assert.True(hand1.NumCards == 1);
            Assert.True(hand2.NumCards == 6);
            Assert.True(hand3.NumCards == 1);
            Assert.True(hand4.NumCards == 6);
            Assert.True(hand1.GetCard(0) == a);
            Assert.True(hand2.GetCard(5) == b);
            Assert.True(hand3.GetCard(0) == c);
            Assert.True(hand4.GetCard(5) == e);
        }

        [Test]
        public void TestAddCard()
        {
            hand1.AddCard(d.DealCard());
            hand2.AddCard(d.DealCard());
            hand3.AddCard(d.DealCard());
            hand4.AddCard(d.DealCard());
            Assert.True(hand1.NumCards == 1);
            Assert.True(hand2.NumCards == 6);
            Assert.True(hand3.NumCards == 1);
            Assert.True(hand4.NumCards == 6);
        }

        [Test]
        public void TestGetCard()
        {
            Card a = d.DealCard();
            hand1.AddCard(a);
            Assert.True(hand1.GetCard(0) == a);
        }

        [Test]
        public void TestGetIndexOf()
        {
            Card a = d.DealCard();
            Card b = d.DealCard();
            hand1.AddCard(a);
            hand1.AddCard(b);
            Assert.True(hand1.GetIndexOf(a) == 0);
            Assert.True(hand1.GetIndexOf(b) == 1);
            string avalue = a.Value;
            string bvalue = b.Value;
            Assert.True(hand1.GetIndexOf(avalue) == 0);
            Assert.True(hand1.GetIndexOf(bvalue) == 1);
            string asuit = a.Suit;
            string bsuit = b.Suit;
            Assert.True(hand1.GetIndexOf(avalue, asuit) == 0);
            Assert.True(hand1.GetIndexOf(bvalue, bsuit) == 1);
        }

        [Test]
        public void TestHasCard()
        {
            Card a = d.DealCard();
            Card b = d.DealCard();
            hand1.AddCard(a);
            hand1.AddCard(b);
            string avalue = a.Value;
            string bvalue = b.Value;
            string asuit = a.Suit;
            string bsuit = b.Suit;
            Assert.True(hand1.HasCard(a));
            Assert.True(hand1.HasCard(b));
            Assert.True(hand1.HasCard(avalue));
            Assert.True(hand1.HasCard(bvalue));
            Assert.True(hand1.HasCard(avalue, asuit));
            Assert.True(hand1.HasCard(bvalue, bsuit));
        }

        [Test]
        public void TestDiscard()
        {
            Card a = hand2.GetCard(3);
            Card b = hand2.Discard(3);
            Assert.True(a == b);
            Assert.True(a != hand2.GetCard(3));
            Assert.True(hand2.NumCards == 4);
        }

        [Test]
        public void TestTotalHand()
        {
            Assert.True(hand3.TotalHand() == 0);
            int hand4Total = int.Parse(hand4.GetCard(0).Value) + 
                int.Parse(hand4.GetCard(1).Value) + 
                int.Parse(hand4.GetCard(2).Value) + 
                int.Parse(hand4.GetCard(3).Value) + 
                int.Parse(hand4.GetCard(4).Value);
            if (hand4.HasAce) { hand4Total -= 10; }
            Assert.True(hand4.TotalHand() == hand4Total);
        }

        [Test]
        public void TestHasAce()
        {
            hand3.AddCard(new Card("S", "A"));
            Assert.True(hand4.HasCard("A"));
            Assert.True(hand4.HasAce);
        }

        [Test]
        public void TestIsBust()
        {
            hand3.AddCard(new Card("S", "A"));
            hand3.AddCard(new Card("H", "J"));
            hand3.AddCard(new Card("H", "Q"));
            Assert.True(!hand3.IsBust);
            hand3.AddCard(new Card("H", "A"));
            Assert.True(hand3.IsBust);
        }
    }
}
