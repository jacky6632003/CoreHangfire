using Hangfire.Annotations;
using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreHangfire.Infrastructure.HangFireMisc
{
    public class HangfireAuthorizeFilter : IDashboardAuthorizationFilter

    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            return true;
        }
    }
}