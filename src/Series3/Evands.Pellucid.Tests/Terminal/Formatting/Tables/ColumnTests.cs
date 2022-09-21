using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Evands.Pellucid.Terminal.Formatting.Tables
{
    /// <summary>
    /// Summary description for ColumnTests
    /// </summary>
    [TestClass]
    public class ColumnTests
    {
        public ColumnTests()
        {
        }

        private TestContext testContextInstance;

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

        public Column UnderTest { get; set; }

        [TestInitialize]
        public void TestSetup()
        {
            Rows = new RowCollection(new Row[] { new Row().AddCells("R1C1", "R1C2", "R1C3"), new Row().AddCells("R2C1", "R2C2", "R2C3") });
            UnderTest = new Column(Rows, 2);
        }

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
        [ExpectedException(typeof(ArgumentNullException))]
        public void Column_Ctor_Throws_ArgumentNull_WhenRowIsNull()
        {
            RowCollection r = null;
            var c = new Column(r, 0);
        }

        [TestMethod]
        public void Column_Ctor_Throws_Nothing()
        {
            var c = new Column(Rows, 0);
        }

        [TestMethod]
        public void Column_Count_IsAccurate()
        {
            var expected = Rows.Count;
            Assert.IsTrue(UnderTest.Count == expected);
        }

        [TestMethod]
        public void Column_Count_AfterExpansion_IsAccurate()
        {
            var expectedCount = 16;
            var index = 15;
            UnderTest[index].Contents = "Does not matter.";
            Assert.IsTrue(UnderTest.Count == expectedCount);
            Assert.IsTrue(Rows.Count == expectedCount);
        }

        [TestMethod]
        public void Indexer_Returns_Expected()
        {
            var expectedItems = Rows.Select(r => r[2]).ToArray();
            for (var i = 0; i < expectedItems.Length; i++)
            {
                Assert.IsTrue(UnderTest[i] == expectedItems[i]);
            }
        }

        [TestMethod]
        public void Indexer_Expands_IfNeeded()
        {
            var expected = string.Empty;
            Assert.IsTrue(UnderTest[3].Contents == expected);
            Assert.IsTrue(Rows[0][3].Contents == expected);
        }

        [TestMethod]
        public void Indexer_SetsValue_InRowAndColumn()
        {
            var expected = "Testing Value";
            UnderTest[0].Contents = expected;
            Assert.IsTrue(UnderTest[0].Contents == expected);
            Assert.IsTrue(Rows[0][2].Contents == expected);
        }

        [TestMethod]
        public void Indexer_SetsValue_Expands_InRowAndColumn()
        {
            var expected = new Cell("Testing Value");
            UnderTest[15] = expected;
            Assert.IsTrue(UnderTest[15] == expected);
            Assert.IsTrue(Rows[15][2] == expected);
        }
    }
}
