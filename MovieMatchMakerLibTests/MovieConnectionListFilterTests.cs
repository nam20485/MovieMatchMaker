using System.Text.Json;

using FluentAssertions;

using MovieMatchMakerLib.Filters;

namespace MovieMatchMakerLibTests
{
    public class MovieConnectionListFilterTests
    {
        private const string ExpectedSerializedJson = "";

        private readonly IMovieConnectionListFilter[] DerivedTypesInInterfaceTypedList = new IMovieConnectionListFilter[]
        { 
            new SortFilter(SortFilter.Order.Descending),
            new MaxMatchingTitleWordsFilter(10),
            new MinConnectedRolesCountFilter(10),
        };

    [Fact]
        public void Test_DerivedTypesInInterfaceTypedSerialization()
        {            
            var json = JsonSerializer.Serialize(DerivedTypesInInterfaceTypedList);
            json.Should().NotBeNull();
            json.Should().NotBeEmpty();
            json.Should().BeEquivalentTo(ExpectedSerializedJson);
        }

        [Fact]
        public void Test_DerivedTypesInInterfaceTypedDeserialization()
        {
            var deserializedList = JsonSerializer.Deserialize<IMovieConnectionListFilter[]>(ExpectedSerializedJson);
            deserializedList.Should().NotBeNull();
            deserializedList.Should().NotBeEmpty();
            deserializedList.Should().BeEquivalentTo(DerivedTypesInInterfaceTypedList);
        }

        [Fact]
        public void Test_DerivedTypesInInterfaceTypedRoundTripSerialization()
        {
            var serializedJson = JsonSerializer.Serialize(DerivedTypesInInterfaceTypedList);
            serializedJson.Should().NotBeNull();
            serializedJson.Should().NotBeEmpty();
            serializedJson.Should().BeEquivalentTo(ExpectedSerializedJson);

            var deserializedList = JsonSerializer.Deserialize<IMovieConnectionListFilter[]>(serializedJson);
            deserializedList.Should().NotBeNull();
            deserializedList.Should().NotBeEmpty();
            deserializedList.Should().BeEquivalentTo(DerivedTypesInInterfaceTypedList);
        }

        [Fact]
        public void Test_DerivedTypesInInterfaceTypedRoundTripDeserialization()
        {
            var deserializedList = JsonSerializer.Deserialize<IMovieConnectionListFilter[]>(ExpectedSerializedJson);
            deserializedList.Should().NotBeNull();
            deserializedList.Should().NotBeEmpty();
            deserializedList.Should().BeEquivalentTo(DerivedTypesInInterfaceTypedList);

            var serializedJson = JsonSerializer.Serialize(deserializedList);
            serializedJson.Should().NotBeNull();
            serializedJson.Should().NotBeEmpty();
            serializedJson.Should().BeEquivalentTo(ExpectedSerializedJson);            
        }
    }
}
