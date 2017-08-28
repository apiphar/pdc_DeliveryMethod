using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.TANGO.Helpers;
using TAM.TANGO.Entities;

namespace TAM.TANGO.Models
{
    public class RoutingGroupSearchResult : BasicSearchResult<RoutingGroup>
    {
        public string Query { get; set; }
        public int? RoutingGroupId { get; set; }

        public RoutingGroupSearchResult(RoutingGroupSearchParameter search, int totalCount, IEnumerable<RoutingGroup> pagedItems) : base(search, totalCount, pagedItems)
        {
            this.Query = search.Query;
        }
    }
}
