using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Services
{
    public class ConfigurationWatch
    {
        public string PassportUrl { set; get; }

        public string PassportAppId { set; get; }

        public int ExcelMaxRow { set; get; }

        public string DMSDomainUrl { get; set; }

        public string TLSUser { get; set; }

        public string TLSPassword { get; set; }

        public string DMSUser { get; set; }

        public string DMSPassword { get; set; }
    }
}
