using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreHangfire.Infrastructure.HangFireMisc.Interface
{
    public interface IHangfireJobTrigger
    {
        void OnStart();
    }
}