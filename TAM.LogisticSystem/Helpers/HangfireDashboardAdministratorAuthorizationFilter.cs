using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire.Dashboard;

namespace TAM.LogisticSystem.Helpers
{
    public class HangfireDashboardAdministratorAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var user = context.GetHttpContext().User;
            return user.Identity.IsAuthenticated;

            // TODO: Check role that can access the dashboard!
        }
    }
}
