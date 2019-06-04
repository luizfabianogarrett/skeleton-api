using SkeletonApi.Util;
using Xunit;

namespace SkeletonApiTest._05.Util
{
    public class HelperTest
    {
        [Fact]
        public void GenerateKey_Ok()
        {
            Assert.NotEmpty(Helper.GenerateKey());
        }
    }
}
