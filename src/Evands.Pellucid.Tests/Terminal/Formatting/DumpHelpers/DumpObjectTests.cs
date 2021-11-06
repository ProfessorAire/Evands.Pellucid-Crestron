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
        public void Ctor_Object_SetsValue()
        {
            var expected = new Object();
            var underTest = new DumpObject(expected);

            Assert.AreEqual(expected, underTest.Value);
        }

        [TestMethod]
        public void Ctor_Object_Name_SetsValue_And_SetsName()
        {
            var expectedValue = new Object();
            var expectedName = "Test Name";
            var underTest = new DumpObject(expectedValue, expectedName);

            Assert.AreEqual(expectedValue, underTest.Value);
            Assert.AreEqual(expectedName, underTest.Name);
        }

        [TestMethod]
        public void Ctor_NullValue_Name_SetsValue_And_SetsName()
        {
            object expectedValue = null;
            var expectedName = "Test Name";
            var underTest = new DumpObject(null, expectedName);

            Assert.AreEqual(expectedValue, underTest.Value);
            Assert.AreEqual(expectedName, underTest.Name);
        }

        [TestMethod]
        public void Ctor_Object_Name_Type_SetsValue_And_SetsName_And_SetsType()
        {
            var expectedValue = new Object();
            var expectedName = "Test Name";
            var expectedType = typeof(ArgumentNullException);
            var underTest = new DumpObject(expectedValue, expectedName, expectedType);

            Assert.AreEqual(expectedValue, underTest.Value);
            Assert.AreEqual(expectedName, underTest.Name);
            Assert.AreEqual(expectedType, underTest.ValueType);
        }

        [TestMethod]
        public void ToString_NullValueType_FullNames_Writes_Correct()
        {
            var underTest = new DumpObject(new Object(), null, null);
            var expected = @"
<unknown type> (0 Properties)
";

            var actual = "\r\n" + underTest.ToString(true);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_NullValueType_ShortNames_Writes_Correct()
        {
            var underTest = new DumpObject(new Object(), null, null);
            var expected = @"
<unknown type> (0 Properties)
";

            var actual = "\r\n" + underTest.ToString(false);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_MaxDepth_NegativeCurrentDepth_ShortNames_Writes_Correct()
        {
            var underTest = this.GetObject();
            var expected = @"
Evands.Pellucid.Terminal.Formatting.DumpHelpers.DumpObjectTests+TestClass (4 Properties)
----------------------------------------------------------------------------------------
| FirstProperty  = ""First""
| SecondProperty = ""Second""
| ThirdProperty  = 3
| FourthProperty = True
----------------------------------------------------------------------------------------
";

            var actual = "\r\n" + underTest.ToString(1, -10, true);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_WithNoPadding_WithBasicProperties_Writes_Correct()
        {
            var underTest = this.GetObject();
            var expected = @"
Evands.Pellucid.Terminal.Formatting.DumpHelpers.DumpObjectTests+TestClass (4 Properties)
----------------------------------------------------------------------------------------
| FirstProperty  = ""First""
| SecondProperty = ""Second""
| ThirdProperty  = 3
| FourthProperty = True
----------------------------------------------------------------------------------------
";

            var actual = "\r\n" + underTest.ToString(true);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_WithNoPadding_WithNestedProperties_Writes_Correct()
        {
            var underTest = new DumpObject(new TestClass2() { First = 2.5, Second = this.GetTestClass("First", "Second", 3, false) });
            var expected = @"
TestClass2 (2 Properties)
-------------------------
| First  = 2.5
| Second = TestClass (4 Properties)
|          ------------------------
|          | FirstProperty  = ""First""
|          | SecondProperty = ""Second""
|          | ThirdProperty  = 3
|          | FourthProperty = False
|          ------------------------
-------------------------
";

            var actual = "\r\n" + underTest.ToString();

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
            var expected = @"
Evands.Pellucid.Terminal.Formatting.DumpHelpers.DumpObjectTests+TestClass3 (4 Properties)
-----------------------------------------------------------------------------------------
| One    = True
| Two    = ""Two""
| Three  = 3
| Fourth = Evands.Pellucid.Terminal.Formatting.DumpHelpers.DumpObjectTests+TestClass2 (2 Properties)
|          -----------------------------------------------------------------------------------------
|          | First  = 2.5
|          | Second = Evands.Pellucid.Terminal.Formatting.DumpHelpers.DumpObjectTests+TestClass (4 Properties)
|          |          ----------------------------------------------------------------------------------------
|          |          | FirstProperty  = ""First""
|          |          | SecondProperty = ""Second""
|          |          | ThirdProperty  = 3
|          |          | FourthProperty = False
|          |          ----------------------------------------------------------------------------------------
|          -----------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------
";

            var actual = "\r\n" + underTest.ToString(true);

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

            var expected = @"
Evands.Pellucid.Terminal.Formatting.DumpHelpers.DumpObjectTests+TestClass3 (4 Properties)
-----------------------------------------------------------------------------------------
| One    = True
| Two    = ""Two""
| Three  = 3
| Fourth = Evands.Pellucid.Terminal.Formatting.DumpHelpers.DumpObjectTests+TestClass2 (2 Properties)
|          -----------------------------------------------------------------------------------------
|          | First  = 2.5
|          | Second = Evands.Pellucid.Terminal.Formatting.DumpHelpers.DumpObjectTests+TestClass (4 Properties)
|          -----------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------
";

            var actual = "\r\n" + underTest.ToString(2, true);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_With_ListProperty_Prints_Correct()
        {
            var underTest = new DumpObject(
                new TestClassWithList()
                {
                    IntList = new List<int>() { 0, 1, 2, 3 },
                    StringList = new List<string>() { "OneItem" } 
                });

            var expected = @"
TestClassWithList (2 Properties)
--------------------------------
| IntList    = List`1 (4 Items)
|              ----------------
|              | 0: 0
|              | 1: 1
|              | 2: 2
|              | 3: 3
|              ----------------
| StringList = List`1 (1 Item)
|              ---------------
|              | 0: ""OneItem""
|              ---------------
--------------------------------
";

            var actual = "\r\n" + underTest.ToString();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_With_GetPropertyValueFailures_AddsFailureObjects()
        {
            var underTest = new DumpObject(new TestFailureClass());

            var expected = @"
TestFailureClass (1 Property)
-----------------------------
| DumpFailures = List`1 (2 Items)
|                ----------------
|                | 0: DumpObjectFailure (4 Properties)
|                |    --------------------------------
|                |    | PropertyName     = ""InstancePropertyOne""
|                |    | ErrorMessage     = ""Exception encountered while dumping property value.""
|                |    | ExceptionMessage = ""Exception has been thrown by the target of an invocation.""
|                |    | ExceptionType    = ""System.Reflection.TargetInvocationException""
|                |    --------------------------------
|                | 1: DumpObjectFailure (4 Properties)
|                |    --------------------------------
|                |    | PropertyName     = ""StaticPropertyOne""
|                |    | ErrorMessage     = ""Exception encountered while dumping property value.""
|                |    | ExceptionMessage = ""Exception has been thrown by the target of an invocation.""
|                |    | ExceptionType    = ""System.Reflection.TargetInvocationException""
|                |    --------------------------------
|                ----------------
-----------------------------
";

            var actual = "\r\n" + underTest.ToString();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_With_GetProperties_Failures_AddsFailureObjects()
        {
            var q = new Queue<Exception>();
            q.Enqueue(new InvalidOperationException());
            q.Enqueue(new InvalidOperationException());

            Crestron.SimplSharp.Reflection.CType.GetPropertiesExceptions = q;

            var underTest = new DumpObject(new TestFailureClass());

            var expected = @"
TestFailureClass (1 Property)
-----------------------------
| DumpFailures = List`1 (2 Items)
|                ----------------
|                | 0: DumpObjectFailure (4 Properties)
|                |    --------------------------------
|                |    | PropertyName     = """"
|                |    | ErrorMessage     = ""Exception while attempting to get the public instance properties of this object.""
|                |    | ExceptionMessage = ""Operation is not valid due to the current state of the object.""
|                |    | ExceptionType    = ""System.InvalidOperationException""
|                |    --------------------------------
|                | 1: DumpObjectFailure (4 Properties)
|                |    --------------------------------
|                |    | PropertyName     = """"
|                |    | ErrorMessage     = ""Exception while attempting to get the public static properties of this object.""
|                |    | ExceptionMessage = ""Operation is not valid due to the current state of the object.""
|                |    | ExceptionType    = ""System.InvalidOperationException""
|                |    --------------------------------
|                ----------------
-----------------------------
";

            var actual = "\r\n" + underTest.ToString();

            Crestron.SimplSharp.Reflection.CType.GetPropertiesExceptions = null;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_With_ChildPropertySameAsSelf_Does_Not_DumpChild()
        {
            var underTest = new DumpObject(Options.Instance);
            Assert.IsFalse(underTest.ToString().Contains("Instance"));
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

        private class TestClassWithList
        {
            public List<int> IntList { get; set; }

            public List<string> StringList { get; set; }
        }

        private class TestFailureClass
        {
            public string InstancePropertyOne { get { throw new NotImplementedException("InstancePropertyOne has not been implemented."); } }

            public static string StaticPropertyOne { get { throw new NotImplementedException("StaticPropertyOne has not been implemented."); } }
        }
    }
}
