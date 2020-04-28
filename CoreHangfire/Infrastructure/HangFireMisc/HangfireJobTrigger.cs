using CoreHangfire.Infrastructure.HangFireMisc.Interface;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreHangfire.Infrastructure.HangFireMisc
{
    public class HangfireJobTrigger : IHangfireJobTrigger
    {
        public void OnStart()
        {
            //-----------------------------------------------------------------
            // 定時執行任務 (Recurring)
            //
            // https://crontab.guru

            RecurringJob.AddOrUpdate<IHangfireJobs>
            (
                methodCall: s => s.CreateTask(null),
                cronExpression: "0/5 * * * *",
                timeZone: TimeZoneInfo.Local
            );
            RecurringJob.AddOrUpdate<IHangfireJobs>
       (
           methodCall: s => s.Customerchoicebank(null),
           cronExpression: "0 0 8 * * ? ",
           timeZone: TimeZoneInfo.Local
       );
        }
    }
}