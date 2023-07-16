using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;

using MovieMatchMakerLib;
using MovieMatchMakerLib.Model;

namespace MovieMatchMakerLibTests
{
    public class MovieConnectionListTests
    {
        [Fact]
        public void Test_FilePath()
        {
            MovieConnectionBuilderBase.FilePath.Should().NotBeNull();
            MovieConnectionBuilderBase.FilePath.Should().NotBeEmpty();
            File.Exists(MovieConnectionBuilderBase.FilePath).Should().BeTrue();
        }

        [Fact]
        public void Test_LoadFromFile()
        {
            File.Exists(MovieConnectionBuilderBase.FilePath).Should().BeTrue();
            var loaded = MovieConnection.List.LoadFromFile(MovieConnectionBuilderBase.FilePath);
            loaded.Should().NotBeNull();
            loaded.Should().NotBeEmpty();
            loaded.Should().HaveCount(17413);
        }

        [Fact]
        public void Test_SaveToFile()
        {
            File.Exists(MovieConnectionBuilderBase.FilePath).Should().BeTrue();
            var loaded = MovieConnection.List.LoadFromFile(MovieConnectionBuilderBase.FilePath);
            loaded.Should().NotBeNull();
            loaded.Should().NotBeEmpty();
            loaded.Should().HaveCount(17413);

            const string filePath = "movieconnections_test_savetofile.json";
            loaded.SaveToFile(filePath);
            File.Exists(filePath).Should().BeTrue();
        }

        [Fact]
        public void Test_LoadSaveToFileRoundTrip()
        {
            File.Exists(MovieConnectionBuilderBase.FilePath).Should().BeTrue();
            var loaded = MovieConnection.List.LoadFromFile(MovieConnectionBuilderBase.FilePath);
            loaded.Should().NotBeNull();
            loaded.Should().NotBeEmpty();
            loaded.Should().HaveCount(17413);

            const string filePath = "movieconnections_test_savetofile.json";
            loaded.SaveToFile(filePath);
            File.Exists(filePath).Should().BeTrue();

            var loadedAgain = MovieConnection.List.LoadFromFile(filePath);
            loadedAgain.Should().NotBeNull();
            loadedAgain.Should().NotBeEmpty();
            loadedAgain.Should().HaveCount(loaded.Count);
            loadedAgain.Should().BeEquivalentTo(loaded);
        }
    }
}
