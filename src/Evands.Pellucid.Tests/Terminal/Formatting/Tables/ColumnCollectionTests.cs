using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Evands.Pellucid.Terminal.Formatting.Tables
{
    /// <summary>
    /// Summary description for ColumnCollectionTests
    /// </summary>
    [TestClass]
    public class ColumnCollectionTests
    {
        public ColumnCollectionTests()
        {
        }

        private TestContext testContextInstance;

        [TestInitialize]
        public void TestSetup()
        {
            Rows = new RowCollection();
            UnderTest = new ColumnCollection(Rows);
        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        public RowCollection Rows { get; set; }

        public ColumnCollection UnderTest { get; set; }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void ColumnCollection_Ctor_Basic_Succeeds()
        {
            Assert.IsNotNull(UnderTest);
        }

        [TestMethod]
        public void ColumnCollection_Count_IsAccurate()
        {
            Assert.IsTrue(UnderTest.Count == 0, "Count is Zero Failed");
            Rows.AddRange(new Row[] { new Row(1), new Row(1), new Row(3) });
            Assert.IsTrue(UnderTest.Count == 3, "Count is 3 Failed");
        }

        [TestMethod]
        public void ColumnCollection_Indexer_Get_Succeeds()
        {
            Rows.Add(new Row().AddCells("ItemA1"));
            Rows.Add(new Row().AddCells("ItemB1", "ItemB2"));

            var item1 = UnderTest[1][0];
            var comp1 = Rows[0][1];

            var item2 = UnderTest[1][1];
            var comp2 = Rows[1][1];

            Assert.IsTrue(UnderTest[1][0] == Rows[0][1]);
            Assert.IsTrue(UnderTest[1][1] == Rows[1][1]);
        }

        [TestMethod]
        public void ColumnCollection_Expands_Automatically()
        {
            var expected = UnderTest[10];
            Assert.IsNotNull(expected);
            Assert.IsTrue(UnderTest.Count == 11);
            Assert.IsTrue(Rows[0].Count == 11);
        }
    }
}
