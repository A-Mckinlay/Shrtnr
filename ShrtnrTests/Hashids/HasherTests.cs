using System;
using Shrtnr.HashIds;
using Xunit;

namespace ShrtnrTests.Hashids
{
    public class HasherTests
    {
        #region GetCode()
        [Fact]
        public void ItShouldReturnACodeWithAtLeast6Characters()
        {
            var hasher = new Hasher("Some salt");
            var code = hasher.GetCode();

            Assert.IsType<string>(code);
            Assert.True(code.Length >= 6);
        }

        #endregion
    }
}