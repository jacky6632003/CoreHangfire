using CoreHangfire.Infrastructure.HangFireMisc.Interface;
using CoreHangfire.Service.Interface;
using Hangfire.Console;
using Hangfire.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreHangfire.Infrastructure.HangFireMisc
{
    public class HangfireJobs : IHangfireJobs
    {
        private readonly ICustomerchoicebankService _customerchoicebankService;

        public HangfireJobs(ICustomerchoicebankService customerchoicebankService)
        {
            this._customerchoicebankService = customerchoicebankService;
        }

        public void CreateTask(PerformContext context)
        {
            var jobId = context.BackgroundJob.Id;

            var message = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss fff} 建立工作排程";

            // var data = _guaranteeBankDataCheckService.ExportIE();

            var endTime = DateTime.Now;

            message = $"{endTime:yyyy-MM-dd HH:mm:ss fff} 結束";
            context.WriteLine(message);
        }

        public async Task Customerchoicebank(PerformContext context)
        {
            var message = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss fff} 建立工作排程";
            context.WriteLine(message);

            var data = await _customerchoicebankService.CustomerchoicebankREAD();

            var endTime = DateTime.Now;
            message = $"{endTime:yyyy-MM-dd HH:mm:ss fff} 結束";
            context.WriteLine(message);
        }
    }
}