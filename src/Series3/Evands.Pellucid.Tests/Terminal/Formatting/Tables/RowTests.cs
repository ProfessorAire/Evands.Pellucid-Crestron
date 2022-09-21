using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Evands.Pellucid.Terminal.Formatting.Tables
{
    /// <summary>
    /// Summary description for RowTests
    /// </summary>
    [TestClass]
    public class RowTests
    {
        public RowTests()
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
        public void Row_Ctor_Basic_HasNoCells()
        {
            var r = new Row();
            Assert.IsTrue(r.Count == 0);
        }

        [TestMethod]
        public void Row_Ctor_WithDefaultSize_HasCorrectQuantityOfCells()
        {
            var r = new Row(10);
            Assert.IsTrue(r.Count == 10);
        }

        [TestMethod]
        public void Row_Count_IsAccurate()
        {
            var r = new Row();
            for (var i = 1; i < 10; i++)
            {
                r.AddCell(new Cell());
                Assert.IsTrue(r.Count == i);
            }
        }

        [TestMethod]
        public void Row_AutoExpands_WhenNewIndex_IsReferenced()
        {
            var r = new Row();
            r[0] = new Cell();

            Assert.IsTrue(r.Count == 1);
        }

        [TestMethod]
        public void Row_AutoExpands_WhenNewLargeIndex_IsReferenced()
        {
            var r = new Row();
            r[10] = new Cell();

            Assert.IsTrue(r.Count == 11);
        }

        [TestMethod]
        public void Row_AddCell_AddsProvidedCell()
        {
            var expected = new Cell("Value");
            var r = new Row().AddCell(expected);
            Assert.IsTrue(r[0] == expected);
        }

        [TestMethod]
        public void Row_AddCell_AsContent_AddsContentToNewCell()
        {
            var expectedValue = "Cell Value";
            var r = new Row().AddCell(expectedValue);
            Assert.IsTrue(r[0].Contents == expectedValue);
        }

        [TestMethod]
        public void Row_AddCells_AddsProvidedCells()
        {
            var expected = new Cell[] { new Cell("V1"), new Cell("V2"), new Cell("V3") };
            var r = new Row(1).AddCells(expected);
            for (var i = 1; i < r.Count; i++)
            {
                Assert.IsTrue(r[i] == expected[i - 1]);
            }
        }

        [TestMethod]
        public void Row_AddCells_UsingParams_AddsProvidedCells()
        {
            var expected = new Cell[] { new Cell("V1"), new Cell("V2"), new Cell("V3") };
            var r = new Row(1).AddCells(expected[0], expected[1], expected[2]);
            for (var i = 1; i < r.Count; i++)
            {
                Assert.IsTrue(r[i] == expected[i - 1]);
            }
        }

        [TestMethod]
        public void Row_AddCells_AsContent_UsingParams_AddsContentToNewCells()
        {
            var expectedValues = new string[] { "V1", "V2", "V3", "V4", "V5" };
            var r = new Row(1).AddCells("V1", "V2", "V3", "V4", "V5");
            Assert.IsTrue(string.IsNullOrEmpty(r[0].Contents));
            for (var i = 1; i < r.Count; i++)
            {
                Assert.IsTrue(r[i].Contents == expectedValues[i - 1]);
            }
        }

        [TestMethod]
        public void Row_AddCells_AsContent_AddsContentToNewCells()
        {
            var expectedValues = new string[] { "V1", "V2", "V3", "V4", "V5" };
            var r = new Row(1).AddCells(expectedValues);
            Assert.IsTrue(string.IsNullOrEmpty(r[0].Contents));
            for (var i = 1; i < r.Count; i++)
            {
                Assert.IsTrue(r[i].Contents == expectedValues[i - 1]);
            }
        }

        [TestMethod]
        public void Row_Indexer_Get_IsCorrect()
        {
            var expected = new Cell("Value");
            var r = new Row(10).AddCell(expected);
            Assert.IsTrue(r[10] == expected);
        }

        [TestMethod]
        public void Row_Indexer_Set_IsCorrect()
        {
            var expected = new Cell("Value");
            var r = new Row(10);
            r[3] = expected;
            Assert.IsTrue(r[3] == expected);
        }

        [TestMethod]
        public void Row_Clear_ClearsList()
        {
            var expected = 0;
            var r = new Row(10);
            Assert.IsTrue(r.Count == 10);
            r.Clear();
            Assert.IsTrue(r.Count == expected);
        }
    }
}
