using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Helpers;

namespace TAM.LogisticSystem.Models
{
    public class DefectMaintenanceSearchResult : BasicSearchResult<Defect>
    {
        public int DefectId { get; set; }
        public string Name { get; set; }

        public DefectMaintenanceSearchResult(DefectMaintenanceSearchParameters search, int totalCount, IEnumerable<Defect> pagedItems) : base(search, totalCount, pagedItems)
        {
            this.DefectId = search.DefectId;
            this.Name = search.Name;
        }

    }
}
