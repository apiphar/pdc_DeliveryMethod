using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAM.LogisticSystem.Models
{
    public class UserClaims
    {
        public string Username { set; get; }

        public string Name { set; get; }

        public List<string> Roles { set; get; }

        public Guid SessionId { set; get; }

        public string LocationCode { set; get; }
    }
}
