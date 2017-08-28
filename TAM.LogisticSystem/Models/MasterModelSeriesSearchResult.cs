using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TAM.LogisticSystem.Entities;
using TAM.LogisticSystem.Helpers;

namespace TAM.LogisticSystem.Models
{
    public class MasterModelSeriesSearchResult //: BasicSearchResult<dynamic>
    {
        public string Query { set; get; }
        
        public string BrandId { set; get; }
        
        public string CarModelCode { set; get; }
      
        public string CarSeriesCode { set; get; }
        public string CarSeriesName { set; get; }
        public string CarModelName { set; get; }
        
        public string Name { set; get; }
        //public MasterModelSeriesSearchResult(MasterModelSeriesSearchParameters search, int totalCount, IEnumerable<dynamic> pagedItems): base(search, totalCount, pagedItems)
        //{
        //    this.Query = search.Query;
        //}

    }
}
