using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
namespace Evands.Pellucid.Terminal.Formatting.Tables
{
    /// <summary>
    /// Summary description for ColumnCollectionTests
    /// </summary>
    public class ColumnCollectionTests
    {
        public ColumnCollectionTests()
        {
        }

        [Before(Test)]
        public void TestSetup()
        {
            Rows = new RowCollection();
            UnderTest = new ColumnCollection(Rows);
        }

        public RowCollection Rows { get; set; }

        public ColumnCollection UnderTest { get; set; }

        [Test]
        public async Task ColumnCollection_Ctor_Basic_Succeeds()
        {
            await Assert.That(UnderTest).IsNotNull();
        }

        [Test]
        public async Task ColumnCollection_Count_IsAccurate()
        {
            await Assert.That(UnderTest.Count == 0).IsTrue();
            Rows.AddRange(new Row[] { new Row(1), new Row(1), new Row(3) });
            await Assert.That(UnderTest.Count == 3).IsTrue();
        }

        [Test]
        public async Task ColumnCollection_Indexer_Get_Succeeds()
        {
            Rows.Add(new Row().AddCells("ItemA1"));
            Rows.Add(new Row().AddCells("ItemB1", "ItemB2"));

            var item1 = UnderTest[1][0];
            var comp1 = Rows[0][1];

            var item2 = UnderTest[1][1];
            var comp2 = Rows[1][1];

            await Assert.That(UnderTest[1][0] == Rows[0][1]).IsTrue();
            await Assert.That(UnderTest[1][1] == Rows[1][1]).IsTrue();
        }

        [Test]
        public async Task ColumnCollection_Expands_Automatically()
        {
            var expected = UnderTest[10];
            await Assert.That(expected).IsNotNull();
            await Assert.That(UnderTest.Count == 11).IsTrue();
            await Assert.That(Rows[0].Count == 11).IsTrue();
        }
    }
}
