using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Evands.Pellucid.Terminal.Commands.Attributes
{
    public class TerminalCommandBaseOfTTests
    {
        [Test]
        public async Task CTor_TargetObject_MatchesConstructor()
        {
            var expected = "value";
            var tc3 = new TestCommand3(expected);
            await Assert.That(tc3.TargetObject == expected).IsTrue();
        }

        [Test]
        public async Task CTor_TargetObject_And_Name_MatchesConstructor()
        {
            var expectedTarget = "value";
            var expectedName = "TestCommand3Suffix";
            var tc3 = new TestCommand3(expectedTarget, "Suffix");
            await Assert.That(tc3.TargetObject == expectedTarget).IsTrue();
            await Assert.That(tc3.Name == expectedName).IsTrue();
        }
    }
}
