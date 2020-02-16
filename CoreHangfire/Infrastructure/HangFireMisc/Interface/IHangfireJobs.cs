using Hangfire;
using Hangfire.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreHangfire.Infrastructure.HangFireMisc.Interface
{
    public interface IHangfireJobs
    {
        /// <summary>
        /// 建立工作排程.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        [JobDisplayName("CreateTaskAsync - 建立工作排程")]
        void CreateTask(PerformContext context);
    }
}