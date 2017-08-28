using System.Collections.Generic;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Helpers;

namespace TAM.LogisticSystem.Models
{
    public class DealerSearchResult : BasicSearchResult<Dealer>
    {
        public string Query { set; get; }

        public DealerSearchResult(DealerSearchParameters search, int totalCount, IEnumerable<Dealer> pagedItems) : base(search, totalCount, pagedItems)
        {
            this.Query = search.Query;
        }
    }
}
