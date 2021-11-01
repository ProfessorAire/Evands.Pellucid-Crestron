using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Evands.Pellucid.Terminal.Formatting.DumpHelpers
{
    [TestClass]
    public class DumpObjectTests
    {
        private TestConsoleWriter writer = new TestConsoleWriter();

        [TestInitialize]
        public void TestInitialize()
        {
            ConsoleBase.RegisterConsoleWriter(writer);
            Options.Instance.ColorizeConsoleOutput = false;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            writer.Messages.Clear();
            ConsoleBase.UnregisterConsoleWriter(writer);
            ConsoleBase.OptionalHeader = string.Empty;
            Options.Instance.ColorizeConsoleOutput = true;
        }

        public DumpObjectTests()
        {
        }

        private TestContext testContextInstance;

        private class TestClass
        {
            public string FirstProperty { get; set; }

            public string SecondProperty { get; set; }

            public int ThirdProperty { get; set; }

            public bool FourthProperty { get; set; }
        }

        private class TestClass2
        {
            public double First { get; set; }

            public TestClass Second { get; set; }
        }

        private class TestClass3
        {
            public bool One { get; set; }

            public string Two { get; set; }

            public int Three { get; set; }

            public TestClass2 Fourth { get; set; }
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

        [TestMethod]
        public void ToString_WithNoPadding_WithBasicProperties_Writes_Correct()
        {
            var underTest = this.GetObject();
            var expected = @"Evands.Pellucid.Terminal.Formatting.DumpHelpers.DumpObjectTests+TestClass
-------------------------------------------------------------------------
FirstProperty  = ""First""
SecondProperty = ""Second""
ThirdProperty  = 3
FourthProperty = True";

            var actual = underTest.ToString();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_WithNoPadding_WithNestedProperties_Writes_Correct()
        {
            var underTest = new DumpObject(new TestClass2() { First = 2.5, Second = this.GetTestClass("First", "Second", 3, false) });
            var expected = @"Evands.Pellucid.Terminal.Formatting.DumpHelpers.DumpObjectTests+TestClass2
--------------------------------------------------------------------------
First  = 2.5
Second = Evands.Pellucid.Terminal.Formatting.DumpHelpers.DumpObjectTests+TestClass
         -------------------------------------------------------------------------
         FirstProperty  = ""First""
         SecondProperty = ""Second""
         ThirdProperty  = 3
         FourthProperty = False";

            var actual = underTest.ToString();

            Assert.AreEqual(expected, actual);
        }
        
        [TestMethod]
        public void ToString_WithNoPadding_WithDeepNestedProperties_Writes_Correct()
        {
            var underTest = new DumpObject(
                new TestClass3()
                {
                    One = true,
                    Two = "Two",
                    Three = 3,
                    Fourth = new TestClass2() { First = 2.5, Second = this.GetTestClass("First", "Second", 3, false) }
                });
            var expected = @"Evands.Pellucid.Terminal.Formatting.DumpHelpers.DumpObjectTests+TestClass3
--------------------------------------------------------------------------
One    = True
Two    = ""Two""
Three  = 3
Fourth = Evands.Pellucid.Terminal.Formatting.DumpHelpers.DumpObjectTests+TestClass2
         --------------------------------------------------------------------------
         First  = 2.5
         Second = Evands.Pellucid.Terminal.Formatting.DumpHelpers.DumpObjectTests+TestClass
                  -------------------------------------------------------------------------
                  FirstProperty  = ""First""
                  SecondProperty = ""Second""
                  ThirdProperty  = 3
                  FourthProperty = False";

            var actual = underTest.ToString();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_WithNoPadding_WithDeepNestedProperties_WritesTwoLevels_Correct()
        {
            var underTest = new DumpObject(
                new TestClass3()
                {
                    One = true,
                    Two = "Two",
                    Three = 3,
                    Fourth = new TestClass2() { First = 2.5, Second = this.GetTestClass("First", "Second", 3, false) }
                });

            var expected = @"Evands.Pellucid.Terminal.Formatting.DumpHelpers.DumpObjectTests+TestClass3
--------------------------------------------------------------------------
One    = True
Two    = ""Two""
Three  = 3
Fourth = Evands.Pellucid.Terminal.Formatting.DumpHelpers.DumpObjectTests+TestClass2
         --------------------------------------------------------------------------
         First  = 2.5
         Second = Evands.Pellucid.Terminal.Formatting.DumpHelpers.DumpObjectTests+TestClass...";

            var actual = underTest.ToString(2);

            Assert.AreEqual(expected, actual);
        }

        private DumpObject GetObject()
        {
            return this.GetObject("First", "Second", 3, true);
        }

        private DumpObject GetObject(string one, string two, int three, bool four)
        {
            return new DumpObject(this.GetTestClass(one, two, three, four));
        }

        private TestClass GetTestClass(string one, string two, int three, bool four)
        {
            return new TestClass()
            {
                FirstProperty = one,
                SecondProperty = two,
                ThirdProperty = three,
                FourthProperty = four
            };
        }
    }
}
