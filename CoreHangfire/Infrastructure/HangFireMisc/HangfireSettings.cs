using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreHangfire.Infrastructure.HangFireMisc
{
    public class HangfireSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HangfireSettings"/> class.
        /// </summary>
        public HangfireSettings()
        {
            this.EnableServer = false;
            this.EnableDashboard = false;
            this.ServerName = string.Empty;
            this.WorkerCount = 10;
            this.Queues = new List<string>().ToArray();
            this.SchemaName = "HangFire";
            this.PrepareSchemaIfNecessary = false;
        }

        /// <summary>
        /// EnableServer.
        /// </summary>
        public bool EnableServer { get; set; }

        /// <summary>
        /// EnableDashboard.
        /// </summary>
        public bool EnableDashboard { get; set; }

        /// <summary>
        /// ServerName.
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// Hangfire's WorkerCount.
        /// </summary>
        public int WorkerCount { get; set; }

        /// <summary>
        /// Hangfire's queues.
        /// </summary>
        public string[] Queues { get; set; }

        /// <summary>
        /// Hangfire Table Schema Name (default: Hangfire).
        /// </summary>
        public string SchemaName { get; set; }

        /// <summary>
        /// PrepareSchemaIfNecessary (default: false).
        /// </summary>
        public bool PrepareSchemaIfNecessary { get; set; }
    }
}