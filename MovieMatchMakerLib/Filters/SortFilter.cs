using System;
using System.Collections.Generic;
using System.Text;

namespace MovieMatchMakerLib.Filters
{
    public class SortFilter : MovieConnectionListFilter
    {
        public enum Order
        {
            Ascending,
            Descending
        }

        public Order SortOrder {  get; private set; }

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
