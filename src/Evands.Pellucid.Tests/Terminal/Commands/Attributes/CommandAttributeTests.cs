using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Evands.Pellucid.Terminal.Commands.Attributes
{
    public class CommandAttributeTests
    {
        [Test]
        public async Task CTor_Throws_ArgumentNull_When_Name_IsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {

                var f = new CommandAttribute(null, "help");
        
            });
        }

        [Test]
        public async Task CTor_WithAlias_Throws_ArgumentNull_When_Name_IsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {

                var f = new CommandAttribute(null, "alis", "help");
        
            });
        }

        [Test]
        public async Task CTor_Throws_ArgumentNull_When_Name_IsEmpty()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {

                var f = new CommandAttribute(string.Empty, "");
        
            });
        }
    }
}
