using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSC322_InformationRetrieval;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSC322_InformationRetrieval.Tests
{
    [TestClass()]
    public class InvertedIndexTests
    {
        InvertedIndex test;

        [TestInitialize]
        public void Initialize()
        {
            test = InvertedIndex.GetInstance();
        }

        [TestMethod()]
        public void AddTest()
        {
            test.Add("boy", new InvertedIndex.Tuple(1, 2));
            test.Add("boy", new InvertedIndex.Tuple(1, 5));
            test.Add("boy", new InvertedIndex.Tuple(1, 6));
            test.Add("dog", new InvertedIndex.Tuple(1, 102));
            test.Add("boy", new InvertedIndex.Tuple(2, 1));
            test.Add("boy", new InvertedIndex.Tuple(3, 1));
            test.Add("dog", new InvertedIndex.Tuple(1, 3));
            test.Add("cow", new InvertedIndex.Tuple(1, 4));
            List<InvertedIndex.Tuple> actual = test["boy"];
            Assert.AreEqual(1, actual[0].DocumentId);
            Assert.AreEqual(2, actual[0].Position);
            Assert.AreEqual(1, actual[3].Position);
            Assert.AreEqual(2, actual[3].DocumentId);
        }

        [TestMethod()]
        public void ContainsTermTrueTest()
        {
            Assert.AreEqual(true, test.ContainTerm("boy"));
        }


        [TestMethod()]
        public void ContainsTermFalseTest()
        {
            Assert.AreEqual(false, test.ContainTerm("girl"));
        }

        [TestMethod()]
        public void TermFrequencyTest()
        {
            int actual = test.TermFrequency("boy", 1);

            Assert.AreEqual(3, actual);
        }

        [TestMethod()]
        public void DocumentFrequencyTest()
        {
            int actual = test.DocumentFrequency("boy");

            Assert.AreEqual(3, actual);
        }

        [TestMethod()]
        public void NumberOfTermsTest()
        {
            int actual = test.NumberOfDocuments();

            Assert.AreEqual(3, actual);
        }

        [TestMethod()]
        public void RemoveTrueTest()
        {
            Assert.AreEqual(true, test.Remove("boy"));
            Assert.AreEqual(false, test.ContainTerm("boy"));
        }

        [TestMethod()]
        public void RemoveFalseeTest()
        {
            Assert.AreEqual(false, test.Remove("girl"));
        }
    }
}