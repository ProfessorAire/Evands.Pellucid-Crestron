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
            Options.UseDefault();
            ConsoleBase.NewLine = Environment.NewLine;
            writer.Messages.Clear();
            ConsoleBase.UnregisterConsoleWriter(writer);
            ConsoleBase.OptionalHeader = string.Empty;
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

    }
}
