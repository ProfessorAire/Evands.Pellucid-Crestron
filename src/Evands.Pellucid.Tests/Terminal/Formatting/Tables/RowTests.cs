using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
namespace Evands.Pellucid.Terminal.Formatting.Tables
{
    /// <summary>
    /// Summary description for RowTests
    /// </summary>
    public class RowTests
    {
        public RowTests()
        {
        }

        [Test]
        public async Task Row_Ctor_Basic_HasNoCells()
        {
            var r = new Row();
            await Assert.That(r.Count == 0).IsTrue();
        }

        [Test]
        public async Task Row_Ctor_WithDefaultSize_HasCorrectQuantityOfCells()
        {
            var r = new Row(10);
            await Assert.That(r.Count == 10).IsTrue();
        }

        [Test]
        public async Task Row_Count_IsAccurate()
        {
            var r = new Row();
            for (var i = 1; i < 10; i++)
            {
                r.AddCell(new Cell());
                await Assert.That(r.Count == i).IsTrue();
            }
        }

        [Test]
        public async Task Row_AutoExpands_WhenNewIndex_IsReferenced()
        {
            var r = new Row();
            r[0] = new Cell();

            await Assert.That(r.Count == 1).IsTrue();
        }

        [Test]
        public async Task Row_AutoExpands_WhenNewLargeIndex_IsReferenced()
        {
            var r = new Row();
            r[10] = new Cell();

            await Assert.That(r.Count == 11).IsTrue();
        }

        [Test]
        public async Task Row_AddCell_AddsProvidedCell()
        {
            var expected = new Cell("Value");
            var r = new Row().AddCell(expected);
            await Assert.That(r[0] == expected).IsTrue();
        }

        [Test]
        public async Task Row_AddCell_AsContent_AddsContentToNewCell()
        {
            var expectedValue = "Cell Value";
            var r = new Row().AddCell(expectedValue);
            await Assert.That(r[0].Contents == expectedValue).IsTrue();
        }

        [Test]
        public async Task Row_AddCells_AddsProvidedCells()
        {
            var expected = new Cell[] { new Cell("V1"), new Cell("V2"), new Cell("V3") };
            var r = new Row(1).AddCells(expected);
            for (var i = 1; i < r.Count; i++)
            {
                await Assert.That(r[i] == expected[i - 1]).IsTrue();
            }
        }

        [Test]
        public async Task Row_AddCells_UsingParams_AddsProvidedCells()
        {
            var expected = new Cell[] { new Cell("V1"), new Cell("V2"), new Cell("V3") };
            var r = new Row(1).AddCells(expected[0], expected[1], expected[2]);
            for (var i = 1; i < r.Count; i++)
            {
                await Assert.That(r[i] == expected[i - 1]).IsTrue();
            }
        }

        [Test]
        public async Task Row_AddCells_AsContent_UsingParams_AddsContentToNewCells()
        {
            var expectedValues = new string[] { "V1", "V2", "V3", "V4", "V5" };
            var r = new Row(1).AddCells("V1", "V2", "V3", "V4", "V5");
            await Assert.That(string.IsNullOrEmpty(r[0].Contents)).IsTrue();
            for (var i = 1; i < r.Count; i++)
            {
                await Assert.That(r[i].Contents == expectedValues[i - 1]).IsTrue();
            }
        }

        [Test]
        public async Task Row_AddCells_AsContent_AddsContentToNewCells()
        {
            var expectedValues = new string[] { "V1", "V2", "V3", "V4", "V5" };
            var r = new Row(1).AddCells(expectedValues);
            await Assert.That(string.IsNullOrEmpty(r[0].Contents)).IsTrue();
            for (var i = 1; i < r.Count; i++)
            {
                await Assert.That(r[i].Contents == expectedValues[i - 1]).IsTrue();
            }
        }

        [Test]
        public async Task Row_Indexer_Get_IsCorrect()
        {
            var expected = new Cell("Value");
            var r = new Row(10).AddCell(expected);
            await Assert.That(r[10] == expected).IsTrue();
        }

        [Test]
        public async Task Row_Indexer_Set_IsCorrect()
        {
            var expected = new Cell("Value");
            var r = new Row(10);
            r[3] = expected;
            await Assert.That(r[3] == expected).IsTrue();
        }

        [Test]
        public async Task Row_Clear_ClearsList()
        {
            var expected = 0;
            var r = new Row(10);
            await Assert.That(r.Count == 10).IsTrue();
            r.Clear();
            await Assert.That(r.Count == expected).IsTrue();
        }
    }
}
