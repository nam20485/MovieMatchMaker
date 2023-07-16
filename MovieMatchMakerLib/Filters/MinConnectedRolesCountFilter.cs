using MovieMatchMakerLib.Model;

namespace MovieMatchMakerLib.Filters
{
    public class MinConnectedRolesCountFilter : MovieConnectionListFilter
    {
        public int MinConnectedRolesCount { get; set; }

        public MinConnectedRolesCountFilter()
            : this(2)
        {
            // default to minimum of (greater than or equal to) 2 connected roles
        }

        public MinConnectedRolesCountFilter(int minConnectedRolesCount)
        {
            MinConnectedRolesCount = minConnectedRolesCount;
        }

        protected override MovieConnection.List FilterList(MovieConnection.List list)
        {
            var genericList = list.FindAll(mc =>
            {
                return mc.ConnectedRoles.Count >= MinConnectedRolesCount;
            });
            return new MovieConnection.List(genericList);          
        }
    }
}
