using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Helpers
{
    public class BasicSearchResult<T> : IPagination
    {
        public int Page { set; get; }

        public int ItemsPerPage { set; get; }

        public int ItemsCount { get; set; }

        public IList<T> PagedItems { get; set; }

        public BasicSearchResult(BasicSearchParameters search, int totalCount, IEnumerable<T> pagedItems)
        {
            this.Page = search.Page;
            this.ItemsPerPage = search.ItemsPerPage;
            this.ItemsCount = totalCount;
            this.PagedItems = pagedItems.ToList();
        }

        public int TotalPages
        {
            get
            {
                if (ItemsCount == 0) return 1;
                return (int)Math.Ceiling(ItemsCount / (double)ItemsPerPage);
            }
        }

        public bool HasPreviousPage
        {
            get
            {
                return (Page > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (Page < TotalPages);
            }
        }
    }
}
