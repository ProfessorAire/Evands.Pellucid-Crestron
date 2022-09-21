using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Evands.Pellucid.Terminal.Formatting.DumpHelpers
{
    [TestClass]
    public class DumpFactoryTests
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

        public DumpFactoryTests()
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

        [TestMethod]
        public void GetNode_WithNull_Returns_DumpNode()
        {
            Assert.IsInstanceOfType(DumpFactory.GetNode(null), typeof(DumpNode));
        }

        [TestMethod]
        public void GetNode_WithValueType_Returns_DumpNode()
        {
            Assert.IsInstanceOfType(DumpFactory.GetNode(123), typeof(DumpNode));
        }

        [TestMethod]
        public void GetNode_WithStringType_Returns_DumpNode()
        {
            Assert.IsInstanceOfType(DumpFactory.GetNode("123"), typeof(DumpNode));
        }

        [TestMethod]
        public void GetNode_WithIDictionary_Returns_DumpCollection()
        {
            Assert.IsInstanceOfType(DumpFactory.GetNode(new Dictionary<string, string>()), typeof(DumpCollection));
        }
        
        [TestMethod]
        public void GetNode_WithIList_Returns_DumpCollection()
        {
            Assert.IsInstanceOfType(DumpFactory.GetNode(new List<string>()), typeof(DumpCollection));
        }

        [TestMethod]
        public void GetNode_WithIEnumerable_Returns_DumpCollection()
        {
            IEnumerable<string> ienum = new List<string>().AsEnumerable<string>();
            Assert.IsInstanceOfType(DumpFactory.GetNode(ienum), typeof(DumpCollection));
        }

        [TestMethod]
        public void GetNode_WithOtherObject_Returns_DumpObject()
        {
            Assert.IsInstanceOfType(DumpFactory.GetNode(new Object()), typeof(DumpObject));
        }

#if !SERIES4
        [TestMethod]
        public void GetNode_WithException()
        {
            var ex = new InvalidOperationException("Invalid Operation");
            Crestron.SimplSharp.Reflection.ExtensionMethods.ExceptionToThrowOnGetCType = ex;
            Crestron.SimplSharp.Reflection.ExtensionMethods.QuantityToThrowOnGetCType = 1;
            var o = new object();
            var actual = DumpFactory.GetNode(o);

            Assert.AreEqual(typeof(DumpObjectFailure), actual.ValueType);

            Crestron.SimplSharp.Reflection.ExtensionMethods.ExceptionToThrowOnGetCType = null;
            Crestron.SimplSharp.Reflection.ExtensionMethods.QuantityToThrowOnGetCType = 0;
        }
#endif

#if !SERIES4
        [TestMethod]
        public void GetNode_WithName_WithException()
        {
            var ex = new InvalidOperationException("Invalid Operation");
            Crestron.SimplSharp.Reflection.ExtensionMethods.ExceptionToThrowOnGetCType = ex;
            Crestron.SimplSharp.Reflection.ExtensionMethods.QuantityToThrowOnGetCType = 1;
            var o = new Dictionary<string, System.Reflection.MethodInfo>();
            var node = DumpFactory.GetNode(o, "Type");

            var content = node.ToString();

            Assert.IsTrue(content.Contains("Invalid Operation"));

            Crestron.SimplSharp.Reflection.ExtensionMethods.ExceptionToThrowOnGetCType = null;
            Crestron.SimplSharp.Reflection.ExtensionMethods.QuantityToThrowOnGetCType = 0;
        }
#endif

#if !SERIES4
        [TestMethod]
        public void GetNode_WithName_WithType_WithException()
        {
            var ex = new InvalidOperationException("Invalid Operation");
            Crestron.SimplSharp.Reflection.ExtensionMethods.ExceptionToThrowOnGetCType = ex;
            Crestron.SimplSharp.Reflection.ExtensionMethods.QuantityToThrowOnGetCType = 1;
            var o = new object();
            var node = DumpFactory.GetNode(o, "Type", o.GetType());
            var content = node.ToString();

            Assert.IsTrue(content.Contains("Invalid Operation"));

            Crestron.SimplSharp.Reflection.ExtensionMethods.ExceptionToThrowOnGetCType = null;
            Crestron.SimplSharp.Reflection.ExtensionMethods.QuantityToThrowOnGetCType = 0;
        }
#endif
    }
}
