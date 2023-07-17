using System.Text.Json;

using MovieMatchMakerLib.Filters;

namespace MovieMatchMakerLibTests
{
    public class MovieConnectionListFilterTests
    {
        private const string ExpectedDerivedTypesInInterfaceTypedListSerializedJson = "[{\"$type\":\"Sort\",\"SortOrder\":1},{\"$type\":\"MaxMatchingTitleWords\",\"MaxMatchingTitleWords\":10},{\"$type\":\"MinConnectedRolesCount\",\"MinConnectedRolesCount\":10}]";
        private const string ExpectedDerivedTypeListSerializedJson = "[{\"MaxMatchingTitleWords\":1},{\"MaxMatchingTitleWords\":2},{\"MaxMatchingTitleWords\":3}]";        

        private readonly IMovieConnectionListFilter[] DerivedTypesInInterfaceTypedList = new IMovieConnectionListFilter[]
        { 
            new SortFilter(SortFilter.Order.Descending),
            new MaxMatchingTitleWordsFilter(10),
            new MinConnectedRolesCountFilter(10),
        };

        private readonly MaxMatchingTitleWordsFilter[] DerivedTypeList = new []
        {
            new MaxMatchingTitleWordsFilter(1),
            new MaxMatchingTitleWordsFilter(2),
            new MaxMatchingTitleWordsFilter(3),
        };

        [Fact]
        public void Test_DerivedTypeListSerialization()
        {
            var json = JsonSerializer.Serialize(DerivedTypeList);
            json.Should().NotBeNull();
            json.Should().NotBeEmpty();
            json.Should().BeEquivalentTo(ExpectedDerivedTypeListSerializedJson);
        }

        [Fact]
        public void Test_DerivedTypeListDeserialization()
        {
            var deserializedList = JsonSerializer.Deserialize<MaxMatchingTitleWordsFilter[]>(ExpectedDerivedTypeListSerializedJson);
            deserializedList.Should().NotBeNull();
            deserializedList.Should().NotBeEmpty();
            deserializedList.Should().BeEquivalentTo(DerivedTypeList);
        }

        [Fact]
        public void Test_DerivedTypesInInterfaceTypedListSerialization()
        {            
            var json = JsonSerializer.Serialize(DerivedTypesInInterfaceTypedList);
            json.Should().NotBeNull();
            json.Should().NotBeEmpty();
            json.Should().BeEquivalentTo(ExpectedDerivedTypesInInterfaceTypedListSerializedJson);
        }

        [Fact]
        public void Test_DerivedTypesInInterfaceTypedListDeserialization()
        {
            var deserializedList = JsonSerializer.Deserialize<IMovieConnectionListFilter[]>(ExpectedDerivedTypesInInterfaceTypedListSerializedJson);
            deserializedList.Should().NotBeNull();
            deserializedList.Should().NotBeEmpty();
            deserializedList.Should().HaveCount(3);
            deserializedList.Should().BeOfType(typeof(IMovieConnectionListFilter[]));
            deserializedList![0].Should().BeOfType(typeof(SortFilter));
            deserializedList[1].Should().BeOfType(typeof(MaxMatchingTitleWordsFilter));
            deserializedList[2].Should().BeOfType(typeof(MinConnectedRolesCountFilter));
        }

        [Fact]
        public void Test_DerivedTypesInInterfaceTypedListRoundTripSerialization()
        {
            var serializedJson = JsonSerializer.Serialize(DerivedTypesInInterfaceTypedList);
            serializedJson.Should().NotBeNull();
            serializedJson.Should().NotBeEmpty();
            serializedJson.Should().BeEquivalentTo(ExpectedDerivedTypesInInterfaceTypedListSerializedJson);

            var deserializedList = JsonSerializer.Deserialize<IMovieConnectionListFilter[]>(serializedJson);
            deserializedList.Should().NotBeNull();
            deserializedList.Should().NotBeEmpty();
            deserializedList.Should().HaveCount(3);
            deserializedList.Should().BeOfType(typeof(IMovieConnectionListFilter[]));
            deserializedList![0].Should().BeOfType(typeof(SortFilter));
            deserializedList[1].Should().BeOfType(typeof(MaxMatchingTitleWordsFilter));
            deserializedList[2].Should().BeOfType(typeof(MinConnectedRolesCountFilter));
        }

        [Fact]
        public void Test_DerivedTypesInInterfaceTypedListRoundTripDeserialization()
        {
            var deserializedList = JsonSerializer.Deserialize<IMovieConnectionListFilter[]>(ExpectedDerivedTypesInInterfaceTypedListSerializedJson);
            deserializedList.Should().NotBeNull();
            deserializedList.Should().NotBeEmpty();
            deserializedList.Should().HaveCount(3);
            deserializedList.Should().BeOfType(typeof(IMovieConnectionListFilter[]));
            deserializedList![0].Should().BeOfType(typeof(SortFilter));
            deserializedList[1].Should().BeOfType(typeof(MaxMatchingTitleWordsFilter));
            deserializedList[2].Should().BeOfType(typeof(MinConnectedRolesCountFilter));

            var serializedJson = JsonSerializer.Serialize(deserializedList);
            serializedJson.Should().NotBeNull();
            serializedJson.Should().NotBeEmpty();
            serializedJson.Should().BeEquivalentTo(ExpectedDerivedTypesInInterfaceTypedListSerializedJson);            
        }
    }
}
