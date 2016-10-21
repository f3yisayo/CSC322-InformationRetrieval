using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSC322_InformationRetrieval;

namespace CSC322_InformationRetrieval.Tests
{
    [TestClass]
    public class QueryTests
    {
        [TestMethod()]
        public void QueryTest()
        {
            //Assert.Fail();
        }

        [TestMethod]
        public void Check_Get_Inverted_Index_Returns_An_InvertedIndex()
        {
            var expected = new Query("License", @"C:\Users\Feyisayo\Documents\Visual Studio 2015\Projects\CSC322-InformationRetrieval\CSC322-InformationRetrieval\bin\Debug\index.dat")
                .GetInvertedIndex();

            Assert.IsInstanceOfType(expected, typeof(InvertedIndex));
        }

        [TestMethod]
        public void Check_Stemming_Works_Well()
        {
            var actual = new Query("License",
                @"C:\Users\Feyisayo\Documents\Visual Studio 2015\Projects\CSC322-InformationRetrieval\CSC322-InformationRetrieval\bin\Debug\index.dat")
                .WordToTerm("working");

            Assert.AreEqual("work", actual);
        }
    }
}