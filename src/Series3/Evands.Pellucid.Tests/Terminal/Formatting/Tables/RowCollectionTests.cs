using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Evands.Pellucid.Terminal.Formatting.Tables
{
    /// <summary>
    /// Summary description for RowCollectionTests
    /// </summary>
    [TestClass]
    public class RowCollectionTests
    {
        public RowCollectionTests()
        {
        }

        private TestContext testContextInstance;

        [TestInitialize]
        public void TestSetup()
        {
            UnderTest = new RowCollection();
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

        public RowCollection UnderTest { get; set; }

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
        public void RowCollection_Ctor_Basic_Succeeds()
        {
            Assert.IsNotNull(UnderTest);
        }

        [TestMethod]
        public void RowCollection_Ctor_WithDefaults_Succeeds()
        {
            UnderTest = new RowCollection(new Row[] { new Row(), new Row() });
            Assert.IsNotNull(UnderTest);
        }

        [TestMethod]
        public void RowCollection_Count_IsAccurate()
        {
            Assert.IsTrue(UnderTest.Count == 0, "Count is Zero Failed");
            UnderTest.AddRange(new Row[] { new Row(), new Row(), new Row() });
            Assert.IsTrue(UnderTest.Count == 3);
        }

        [TestMethod]
        public void RowCollection_Add_Succeeds()
        {
            UnderTest.Add(new Row(10));
            Assert.IsTrue(UnderTest.Count == 1);
        }

        [TestMethod]
        public void RowCollection_AddRange_Succeeds()
        {
            UnderTest.AddRange(new Row[] { new Row(10), new Row(11), new Row(12) });
            Assert.IsTrue(UnderTest.Count == 3);
        }

        [TestMethod]
        public void RowCollection_Clear_Succeeds()
        {
            UnderTest.AddRange(new Row[] { new Row(), new Row() });
            Assert.IsTrue(UnderTest.Count == 2);
            UnderTest.Clear();
            Assert.IsTrue(UnderTest.Count == 0);
        }

        [TestMethod]
        public void RowCollection_Indexer_Get_Succeeds()
        {
            var expected = new Row(2);
            UnderTest.Add(new Row(1));
            UnderTest.Add(expected);

            Assert.IsTrue(UnderTest[1] == expected);
        }

        [TestMethod]
        public void RowCollection_Indexer_Set_Succeeds()
        {
            var expected = new Row(4);
            var index = 10;
            UnderTest[index] = expected;
            Assert.IsTrue(UnderTest[index] == expected);
        }

        [TestMethod]
        public void RowCollection_Expands_Automatically()
        {
            UnderTest[10] = new Row();
            Assert.IsTrue(UnderTest.Count == 11);
        }
    }
}
