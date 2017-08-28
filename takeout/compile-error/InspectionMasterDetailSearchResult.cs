using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.TANGO.Entities;
using TAM.TANGO.Helpers;

namespace TAM.TANGO.Models
{
    public class InspectionMasterDetailSearchResult : BasicSearchResult<InspectionMasterDetail>
    {
        public string Query { set; get; }

        public int? Type { set; get; }

        public InspectionMasterDetailSearchResult(InspectionMasterDetailSearchParameters search, int totalCount, IEnumerable<InspectionMasterDetail> pagedItems) : base(search, totalCount, pagedItems)
        {
            this.Query = search.Query;
            this.Type = search.Type;            
        }
    }
}
