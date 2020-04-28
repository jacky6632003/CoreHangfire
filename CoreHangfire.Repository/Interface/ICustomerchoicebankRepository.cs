using CoreHangfire.Repository.DataModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoreHangfire.Repository.Interface
{
    public interface ICustomerchoicebankRepository
    {
        /// <summary>
        /// 讀取銀行資料
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<CustomerchoicebankDataModel>> CustomerchoicebankREAD();

        /// <summary>
        /// 修改銀行資料格式
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<CustomerchoicebankDataModel>> CustomerchoicebankREADNext();

        /// <summary>
        /// 寫入金融資訊
        /// </summary>
        /// <param name="SQL"></param>
        /// <returns></returns>
        Task<IEnumerable<string>> InsertCustomerchoicebank(string SQL);

        /// <summary>
        /// 刪除金融資訊
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<string>> deleteCustomerchoicebank();
    }
}