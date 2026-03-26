using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Evands.Pellucid.Terminal.Commands
{
    public class ManagerTests
    {
        [Test]
        public async Task Unregister_WhenNoCommandContain_Returns_False()
        {
            var result = Manager.Unregister("noCommandExists", new TestCommand());

            await Assert.That(result).IsFalse();
        }
    }
}
