using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Evands.Pellucid.Terminal.Commands.Attributes
{
    public class OperandAttributeTests
    {
        [Test]
        public async Task CTor_Throws_ArgumentNull_When_Name_IsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {

                var f = new OperandAttribute(null, "help");
        
            });
        }

        [Test]
        public async Task CTor_Throws_ArgumentNull_When_Name_IsEmpty()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {

                var f = new OperandAttribute(string.Empty, "");
        
            });
        }
    }
}
