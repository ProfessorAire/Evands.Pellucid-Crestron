using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Evands.Pellucid.Terminal.Formatting.Tables
{
    /// <summary>
    /// Summary description for ColumnTests
    /// </summary>
    public class ColumnTests
    {
        public ColumnTests()
        {
        }

        public RowCollection Rows { get; set; }

        public Column UnderTest { get; set; }

        [Before(Test)]
        public void TestSetup()
        {
            Rows = new RowCollection(new Row[] { new Row().AddCells("R1C1", "R1C2", "R1C3"), new Row().AddCells("R2C1", "R2C2", "R2C3") });
            UnderTest = new Column(Rows, 2);
        }

        [Test]
        public async Task Column_Ctor_Throws_ArgumentNull_WhenRowIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {

                RowCollection r = null;
                var c = new Column(r, 0);
        
            });
        }

        [Test]
        public async Task Column_Ctor_Throws_Nothing()
        {
            var c = new Column(Rows, 0);
        }

        [Test]
        public async Task Column_Count_IsAccurate()
        {
            var expected = Rows.Count;
            await Assert.That(UnderTest.Count == expected).IsTrue();
        }

        [Test]
        public async Task Column_Count_AfterExpansion_IsAccurate()
        {
            var expectedCount = 16;
            var index = 15;
            UnderTest[index].Contents = "Does not matter.";
            await Assert.That(UnderTest.Count == expectedCount).IsTrue();
            await Assert.That(Rows.Count == expectedCount).IsTrue();
        }

        [Test]
        public async Task Indexer_Returns_Expected()
        {
            var expectedItems = Rows.Select(r => r[2]).ToArray();
            for (var i = 0; i < expectedItems.Length; i++)
            {
                await Assert.That(UnderTest[i] == expectedItems[i]).IsTrue();
            }
        }

        [Test]
        public async Task Indexer_Expands_IfNeeded()
        {
            var expected = string.Empty;
            await Assert.That(UnderTest[3].Contents == expected).IsTrue();
            await Assert.That(Rows[0][3].Contents == expected).IsTrue();
        }

        [Test]
        public async Task Indexer_SetsValue_InRowAndColumn()
        {
            var expected = "Testing Value";
            UnderTest[0].Contents = expected;
            await Assert.That(UnderTest[0].Contents == expected).IsTrue();
            await Assert.That(Rows[0][2].Contents == expected).IsTrue();
        }

        [Test]
        public async Task Indexer_SetsValue_Expands_InRowAndColumn()
        {
            var expected = new Cell("Testing Value");
            UnderTest[15] = expected;
            await Assert.That(UnderTest[15] == expected).IsTrue();
            await Assert.That(Rows[15][2] == expected).IsTrue();
        }
    }
}
