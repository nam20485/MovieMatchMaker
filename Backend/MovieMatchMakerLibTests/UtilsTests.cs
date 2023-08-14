using MovieMatchMakerLib.Utils;

namespace MovieMatchMakerLibTests
{
    public class UtilsTests
    {
        [Fact]
        public void Test_IsDebugOrReleaseBuild()
        {
#if DEBUG
            Macros.IsDebugBuild().Should().BeTrue();            
            Macros.IsReleaseBuild().Should().BeFalse();
#else
            Macros.IsDebugBuild().Should().BeFalse();
            Macros.IsReleaseBuild().Should().BeTrue();
#endif
        }

        // TODO: Test convienience methods in MovieMatchMakerLibTests.Utils.cs class
    }
}
