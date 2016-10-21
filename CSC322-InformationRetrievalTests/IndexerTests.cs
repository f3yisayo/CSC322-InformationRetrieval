using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace CSC322_InformationRetrieval.Tests
{
    [TestClass]
    public class IndexerTests
    {
        [TestMethod]
        public void Verify_Index_Path_Does_NOT_Match()
        {
            string path = " ";
            Indexer indexer = new Indexer(path);
            Assert.IsFalse(Regex.IsMatch(indexer.pathToIndexTo, @"^(?:[a-zA-Z]\:|\\\\[\w\.]+\\[\w.$]+)\\(?:[\w]+\\)*\w([\w.])+$") );
        }

        [TestMethod]
        public void Verify_Index_Path_Matches()
        {
            string path = @"C:\Users\Test";
            Indexer indexer = new Indexer(path);
            Assert.IsTrue(Regex.IsMatch(indexer.pathToIndexTo, @"^(?:[a-zA-Z]\:|\\\\[\w\.]+\\[\w.$]+)\\(?:[\w]+\\)*\w([\w.])+$"));
        }

        [TestMethod]
        public void Test_Tag_Stripping_Works_For_File_With_Tags()
        {
            var testString = "<p>Hello World</p>";
            var testIndexer = new Indexer("").RemoveAllTags(testString);
            var expected = "Hello World";

            Assert.AreEqual(expected, testIndexer);
        }

        [TestMethod]
        public void Test_PPT_Extraction_Returns_A_String()
        {
            var pptFile = new FileInfo(@"C:\Users\Feyisayo\Desktop\Test\Breadth First Search.pptx");
            var pptContent = new Indexer("").ExtractPptxText(pptFile);

            Assert.IsInstanceOfType(pptContent, typeof(string));
        }

        [TestMethod]
        [ExpectedException(typeof (FileNotFoundException))]
        public void Try_To_Parse_A_File_We_DoNot_Accept()
        {
            var crazyFile = new FileInfo(@"C:\Users\Feyisayo\Pictures\Tumblr\8070685_16515551_lz.jpg");
            new Indexer("").ProcessFile(crazyFile);
        }

        [TestMethod]
        [ExpectedException(typeof (FileNotFoundException))]
        public void Try_To_Parse_A_File_That_Does_Not_Exist()
        {
            var crazyFile = new FileInfo(@"C:\Users\Feyisayo\foo.bar");
            new Indexer("").ProcessFile(crazyFile);
        }

        [TestMethod]
        public void Index_File_Test()
        {
           
            var indexer = new Indexer(@"C:\Users\Feyisayo\Documents\Visual Studio 2015\Projects\CSC322-InformationRetrieval\CSC322-InformationRetrieval\bin\Debug\index.dat");
            var expected = indexer.IndexDoc(new FileInfo(@"C:\Users\Feyisayo\Desktop\Test\test3\blah.txt"));
            /*Debug.WriteLine(expected);

            var actual = InvertedIndex.GetInstance().ToString();
            Assert.AreEqual(expected, actual);*/
        }

     }
}