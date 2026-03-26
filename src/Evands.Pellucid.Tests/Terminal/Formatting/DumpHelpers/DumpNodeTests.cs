using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Evands.Pellucid.Terminal.Formatting.DumpHelpers
{
    public class DumpNodeTests
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

        public DumpNodeTests()
        {
        }

        [Test]
        public async Task ToString_BasicValue_NoParameters_Writes_Correct()
        {
            var underTest = new DumpNode("TestValue", "TestName");
            var expected = "TestName = \"TestValue\"";

            var actual = underTest.ToString();
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToString_BasicValue_MaxDepth0_And_CurrentDepthNegative_Writes_Correct()
        {
            var underTest = new DumpNode("TestValue", "TestName");
            var expected = "TestName = \"TestValue\"";

            var actual = underTest.ToString(0, -1);
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToString_BasicValue_MaxDepthTwo_Writes_Correct()
        {
            var underTest = new DumpNode("TestValue", "TestName");
            var expected = "TestName = \"TestValue\"";

            var actual = underTest.ToString(2);
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToString_BasicValue_FullNamesFalse_Writes_Correct()
        {
            var underTest = new DumpNode("TestValue", "TestName");
            var expected = "TestName = \"TestValue\"";

            var actual = underTest.ToString(false);
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToString_BasicValue_MaxDepthOne_And_FullNamesFalse_Writes_Correct()
        {
            var underTest = new DumpNode("TestValue", "TestName");
            var expected = "TestName = \"TestValue\"";

            var actual = underTest.ToString(1, false);
            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToString_BasicValue_NoName_Writes_Correct()
        {
            var underTest = new DumpNode("TestValue", string.Empty);
            var expected = "\"TestValue\"";
            var actual = underTest.ToString();

            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToString_NullValue_NoName_Writes_Correct()
        {
            var underTest = new DumpNode(null, string.Empty);
            var expected = "<null>";
            var actual = underTest.ToString();

            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task ToString_NullValue_WithName_Writes_Correct()
        {
            var underTest = new DumpNode(null, "TestName");
            var expected = "TestName = <null>";
            var actual = underTest.ToString();

            await Assert.That(actual).IsEqualTo(expected);
        }

        [Test]
        public async Task CTor_WithNullName_Creates_WithEmptyName()
        {
            var underTest = new DumpNode("TestValue", null);
            await Assert.That(underTest.Name).IsEqualTo(string.Empty);
        }

        [Test]
        public async Task CTor_AssignsName_Correctly()
        {
            var expected = "TestName";
            var underTest = new DumpNode("TestValue", expected);
            await Assert.That(underTest.Name).IsEqualTo(expected);
        }

        [Test]
        public async Task CTor_AssignsStringValue_WithQuotes()
        {
            var expected = "TestValue";
            var underTest = new DumpNode(expected, "TestName");
            await Assert.That(underTest.Value).IsEqualTo("\"" + expected + "\"");
        }

        [Test]
        public async Task CTor_AssignsNonStringValue_Directly()
        {
            var expected = 1234;
            var underTest = new DumpNode(expected, "TestName");
            await Assert.That(underTest.Value).IsEqualTo(expected);
        }
    }
}
