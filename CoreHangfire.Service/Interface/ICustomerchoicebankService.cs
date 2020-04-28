using CoreHangfire.Service.ResultModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoreHangfire.Service.Interface
{
    public interface ICustomerchoicebankService
    {
        Task<IEnumerable<CustomerchoicebankResultModel>> CustomerchoicebankREAD();
    }
}