using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Evands.Pellucid.Terminal.Commands.Attributes
{
    public class SampleAttributeTests
    {
        [Test]
        public async Task CTor_Throws_ArgumentNull_When_Sample_IsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {

                var f = new SampleAttribute(null, "help");
        
            });
        }

        [Test]
        public async Task CTor_Throws_ArgumentNull_When_Sample_IsEmpty()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {

                var f = new SampleAttribute(string.Empty, "");
        
            });
        }
    }
}
