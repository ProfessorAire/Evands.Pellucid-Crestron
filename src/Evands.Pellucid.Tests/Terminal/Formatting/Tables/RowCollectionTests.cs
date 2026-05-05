using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
namespace Evands.Pellucid.Terminal.Formatting.Tables
{
    /// <summary>
    /// Summary description for RowCollectionTests
    /// </summary>
    public class RowCollectionTests
    {
        public RowCollectionTests()
        {
        }

        [Before(Test)]
        public void TestSetup()
        {
            UnderTest = new RowCollection();
        }

        public RowCollection UnderTest { get; set; }

        [Test]
        public async Task RowCollection_Ctor_Basic_Succeeds()
        {
            await Assert.That(UnderTest).IsNotNull();
        }

        [Test]
        public async Task RowCollection_Ctor_WithDefaults_Succeeds()
        {
            var underTest = new RowCollection(new Row[] { new Row(), new Row() });
            await Assert.That(underTest).IsNotNull();
        }

        [Test]
        public async Task RowCollection_Count_IsAccurate()
        {
            await Assert.That(UnderTest.Count == 0).IsTrue();
            UnderTest.AddRange(new Row[] { new Row(), new Row(), new Row() });
            await Assert.That(UnderTest.Count == 3).IsTrue();
        }

        [Test]
        public async Task RowCollection_Add_Succeeds()
        {
            UnderTest.Add(new Row(10));
            await Assert.That(UnderTest.Count == 1).IsTrue();
        }

        [Test]
        public async Task RowCollection_AddRange_Succeeds()
        {
            UnderTest.AddRange(new Row[] { new Row(10), new Row(11), new Row(12) });
            await Assert.That(UnderTest.Count == 3).IsTrue();
        }

        [Test]
        public async Task RowCollection_Clear_Succeeds()
        {
            UnderTest.AddRange(new Row[] { new Row(), new Row() });
            await Assert.That(UnderTest.Count == 2).IsTrue();
            UnderTest.Clear();
            await Assert.That(UnderTest.Count == 0).IsTrue();
        }

        [Test]
        public async Task RowCollection_Indexer_Get_Succeeds()
        {
            var expected = new Row(2);
            UnderTest.Add(new Row(1));
            UnderTest.Add(expected);

            await Assert.That(UnderTest[1] == expected).IsTrue();
        }

        [Test]
        public async Task RowCollection_Indexer_Set_Succeeds()
        {
            var expected = new Row(4);
            var index = 10;
            UnderTest[index] = expected;
            await Assert.That(UnderTest[index] == expected).IsTrue();
        }

        [Test]
        public async Task RowCollection_Expands_Automatically()
        {
            UnderTest[10] = new Row();
            await Assert.That(UnderTest.Count == 11).IsTrue();
        }
    }
}
