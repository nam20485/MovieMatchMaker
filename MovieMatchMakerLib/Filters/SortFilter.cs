using MovieMatchMakerLib.Model;

namespace MovieMatchMakerLib.Filters
{
    public class SortFilter : MovieConnectionListFilterBase
    {
        public enum Order
        {
            Ascending,
            Descending
        }

        public Order SortOrder {  get; set; }

        public SortFilter()
            : this(Order.Descending)
        {
            // default to descending sort order
        }

        public SortFilter(Order sortOrder)
        {
            SortOrder = sortOrder;
        }

        protected override MovieConnection.List FilterList(MovieConnection.List list)
        {
            list.Sort((mc1, mc2) =>
            {
                return mc2.ConnectedRoles.Count.CompareTo(mc1.ConnectedRoles.Count);
            });
            return list;
        }
    }
}
