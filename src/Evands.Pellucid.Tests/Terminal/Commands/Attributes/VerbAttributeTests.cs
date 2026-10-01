using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Evands.Pellucid.Terminal.Commands.Attributes
{
    public class VerbAttributeTests
    {
        [Test]
        public async Task CTor_Formats_AliasAndName_When_Name_IsNull()
        {
            var f = new VerbAttribute("Name", string.Empty, "help");
            await Assert.That(string.IsNullOrEmpty(f.Alias)).IsTrue();
            await Assert.That(f.HelpFormattedName == "Name").IsTrue();
        }
        
        [Test]
        public async Task Alias_Set_Formats_HelpFormattedName()
        {
            var f = new VerbAttribute("Name", string.Empty, "help");
            await Assert.That(f.HelpFormattedName == f.Name).IsTrue();
        }

        [Test]
        public async Task CTor_Throws_ArgumentNull_When_Name_IsEmpty()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {

                var f = new FlagAttribute(string.Empty, "");
        
            });
        }
    }
}
