using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Evands.Pellucid.Terminal.Formatting.DumpHelpers
{
    public class DumpFactoryTests
    {
        private TestConsoleWriter writer = new TestConsoleWriter();

        [Before(Test)]
        public void TestInitialize()
        {
            ConsoleBase.RegisterConsoleWriter(writer);
            Options.Instance.ColorizeConsoleOutput = false;
        }

        [After(Test)]
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

        [Test]
        public async Task GetNode_WithNull_Returns_DumpNode()
        {
            await Assert.That(DumpFactory.GetNode(null)).IsTypeOf<DumpNode>();
        }

        [Test]
        public async Task GetNode_WithValueType_Returns_DumpNode()
        {
            await Assert.That(DumpFactory.GetNode(123)).IsTypeOf<DumpNode>();
        }

        [Test]
        public async Task GetNode_WithStringType_Returns_DumpNode()
        {
            await Assert.That(DumpFactory.GetNode("123")).IsTypeOf<DumpNode>();
        }

        [Test]
        public async Task GetNode_WithIDictionary_Returns_DumpCollection()
        {
            await Assert.That(DumpFactory.GetNode(new Dictionary<string, string>())).IsTypeOf<DumpCollection>();
        }
        
        [Test]
        public async Task GetNode_WithIList_Returns_DumpCollection()
        {
            await Assert.That(DumpFactory.GetNode(new List<string>())).IsTypeOf<DumpCollection>();
        }

        [Test]
        public async Task GetNode_WithIEnumerable_Returns_DumpCollection()
        {
            IEnumerable<string> ienum = new List<string>().AsEnumerable<string>();
            await Assert.That(DumpFactory.GetNode(ienum)).IsTypeOf<DumpCollection>();
        }

        [Test]
        public async Task GetNode_WithOtherObject_Returns_DumpObject()
        {
            await Assert.That(DumpFactory.GetNode(new Object())).IsTypeOf<DumpObject>();
        }

        [Test]
        public async Task GetNode_WithException()
        {
            var ex = new InvalidOperationException("Invalid Operation");
            Crestron.SimplSharp.Reflection.ExtensionMethods.ExceptionToThrowOnGetCType = ex;
            Crestron.SimplSharp.Reflection.ExtensionMethods.QuantityToThrowOnGetCType = 1;
            var o = new object();
            var actual = DumpFactory.GetNode(o);

            await Assert.That(actual.ValueType).IsEqualTo(typeof(DumpObjectFailure));

            Crestron.SimplSharp.Reflection.ExtensionMethods.ExceptionToThrowOnGetCType = null;
            Crestron.SimplSharp.Reflection.ExtensionMethods.QuantityToThrowOnGetCType = 0;
        }

        [Test]
        public async Task GetNode_WithName_WithException()
        {
            var ex = new InvalidOperationException("Invalid Operation");
            Crestron.SimplSharp.Reflection.ExtensionMethods.ExceptionToThrowOnGetCType = ex;
            Crestron.SimplSharp.Reflection.ExtensionMethods.QuantityToThrowOnGetCType = 1;
            var o = new Dictionary<string, System.Reflection.MethodInfo>();
            var node = DumpFactory.GetNode(o, "Type");

            var content = node.ToString();

            await Assert.That(content.Contains("Invalid Operation")).IsTrue();

            Crestron.SimplSharp.Reflection.ExtensionMethods.ExceptionToThrowOnGetCType = null;
            Crestron.SimplSharp.Reflection.ExtensionMethods.QuantityToThrowOnGetCType = 0;
        }

        [Test]
        public async Task GetNode_WithName_WithType_WithException()
        {
            var ex = new InvalidOperationException("Invalid Operation");
            Crestron.SimplSharp.Reflection.ExtensionMethods.ExceptionToThrowOnGetCType = ex;
            Crestron.SimplSharp.Reflection.ExtensionMethods.QuantityToThrowOnGetCType = 1;
            var o = new object();
            var node = DumpFactory.GetNode(o, "Type", o.GetType());
            var content = node.ToString();

            await Assert.That(content.Contains("Invalid Operation")).IsTrue();

            Crestron.SimplSharp.Reflection.ExtensionMethods.ExceptionToThrowOnGetCType = null;
            Crestron.SimplSharp.Reflection.ExtensionMethods.QuantityToThrowOnGetCType = 0;
        }
    }
}
