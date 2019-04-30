using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using MTDClasses;

namespace MTDTests
{
    [TestFixture]
    public class TrainTests
    {
        Hand hand1;
        Hand hand2;
        MexicanTrain aMexicanTrain;
        PlayerTrain aPlayerTrain;
        PlayerTrain anotherPlayerTrain;
        BoneYard reallyBigHand;

        [SetUp]
        public void SetUpAllTests()
        {
            hand1 = new Hand();
            hand2 = new Hand();
            aMexicanTrain = new MexicanTrain();
            aPlayerTrain = new PlayerTrain(hand1);
            anotherPlayerTrain = new PlayerTrain(hand2);
            reallyBigHand = new BoneYard(6);
        }

        [Test]
        public void TestSomething()
        {
            Assert.True(true);
        }

        [Test]
        public void TestDefaultConstructor()
        {
            MexicanTrain testTrain1 = new MexicanTrain();
            PlayerTrain testTrain2 = new PlayerTrain(hand1);
        }

        [Test]
        public void TestEngineConstructor()
        {
            MexicanTrain testTrain1 = new MexicanTrain(6);
            PlayerTrain testTrain2 = new PlayerTrain(hand1);
        }

        [Test]
        public void TestCount()
        {
            Assert.True(aMexicanTrain.Count == 0);
            Assert.True(aPlayerTrain.Count == 0);
            Assert.True(anotherPlayerTrain.Count == 0);
            aMexicanTrain.Add(reallyBigHand.Draw());
            aPlayerTrain.Add(reallyBigHand.Draw());
            anotherPlayerTrain.Add(reallyBigHand.Draw());
            Assert.True(aMexicanTrain.Count == 1);
            Assert.True(aPlayerTrain.Count == 1);
            Assert.True(anotherPlayerTrain.Count == 1);
        }

        [Test]
        public void TestEngineValue()
        {
            MexicanTrain a = new MexicanTrain(6);
            PlayerTrain b = new PlayerTrain(hand1, 5);
            Assert.True(a.EngineValue == 6);
            Assert.True(b.EngineValue == 5);
        }

        [Test]
        public void TestIsEmpty()
        {
            Assert.True(aMexicanTrain.IsEmpty);
            Assert.True(aPlayerTrain.IsEmpty);
            Assert.True(anotherPlayerTrain.IsEmpty);
            aMexicanTrain.Add(reallyBigHand.Draw());
            aPlayerTrain.Add(reallyBigHand.Draw());
            anotherPlayerTrain.Add(reallyBigHand.Draw());
            Assert.True(!aMexicanTrain.IsEmpty);
            Assert.True(!aPlayerTrain.IsEmpty);
            Assert.True(!anotherPlayerTrain.IsEmpty);
        }

        [Test]
        public void TestLastDomino()
        {
            Domino a = reallyBigHand.Draw();
            aMexicanTrain.Add(a);
            Assert.True(aMexicanTrain.LastDomino == a);
            Domino b = reallyBigHand.Draw();
            aMexicanTrain.Add(b);
            Assert.True(aMexicanTrain.LastDomino == b);
        }

        [Test]
        public void TestPlayableValue()
        {
            MexicanTrain a = new MexicanTrain(6);
            Assert.IsTrue(a.PlayableValue == 6);
            a.Add(new Domino(3, 3));
            Assert.IsTrue(a.PlayableValue == 3);
        }

        [Test]
        public void TestAdd()
        {
            //Significant evidence of add working in other unit tests
        }

        [Test]
        public void TestIndexer()
        {
            Domino a = reallyBigHand.Draw();
            aMexicanTrain.Add(a);
            Assert.True(aMexicanTrain[0] == a);
        }

        //This also covers IsOpen, Open, and Close since they are required for the testing
        [Test]
        public void TestIsPlayable()
        {
            bool flip;
            MexicanTrain a = new MexicanTrain(4);
            PlayerTrain b = new PlayerTrain(hand1, 5);
            PlayerTrain c = new PlayerTrain(hand2, 6);
            hand1.Add(new Domino(4, 5));
            hand1.Add(new Domino(5, 6));
            hand1.Add(new Domino(6, 4));
            hand2.Add(new Domino(4, 5));
            hand2.Add(new Domino(5, 6));
            hand2.Add(new Domino(6, 4));
            Assert.True(a.IsPlayable(hand1, hand1[0], out flip));
            Assert.True(!flip);
            Assert.True(a.IsPlayable(hand1, hand1[2], out flip));
            Assert.True(flip);
            Assert.True(b.IsPlayable(hand1, hand1[0], out flip));
            Assert.True(flip);
            Assert.True(b.IsPlayable(hand1, hand1[1], out flip));
            Assert.True(!flip);
            Assert.True(c.IsPlayable(hand2, hand2[1], out flip));
            Assert.True(flip);
            Assert.True(c.IsPlayable(hand2, hand2[2], out flip));
            Assert.True(!flip);
            Assert.True(!b.IsPlayable(hand2, hand1[0], out flip));
            Assert.True(!b.IsPlayable(hand2, hand1[1], out flip));
            Assert.True(!b.IsOpen);
            b.Open();
            Assert.True(b.IsOpen);
            Assert.True(b.IsPlayable(hand2, hand1[0], out flip));
            Assert.True(b.IsPlayable(hand2, hand1[1], out flip));
            b.Close();
            Assert.True(!b.IsOpen);
            Assert.True(!b.IsPlayable(hand2, hand1[0], out flip));
            Assert.True(!b.IsPlayable(hand2, hand1[1], out flip));
        }

        [Test]
        public void TestPlay()
        {
            MexicanTrain a = new MexicanTrain(4);
            hand1.Add(new Domino(4, 5));
            a.Play(hand1, hand1[0]);
            Assert.True(a.EngineValue == 4);
            Assert.True(a[0].Side1 == 4);
            Assert.True(a[0].Side2 == 5);
            a.Play(hand1, hand1[0]);
            Assert.True(a[1].Side1 == 5);
            Assert.True(a[0].Side2 == 4);
        }
    }
}
