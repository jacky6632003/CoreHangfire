using AutoMapper;
using CoreHangfire.Repository.DataModel;
using CoreHangfire.Repository.Interface;
using CoreHangfire.Service.Interface;
using CoreHangfire.Service.ResultModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoreHangfire.Service.Implement
{
    public class CustomerchoicebankService : ICustomerchoicebankService
    {
        private readonly IMapper _mapper;

        private readonly ICustomerchoicebankRepository _customerchoicebankRepository;

        public CustomerchoicebankService(IMapper mapper, ICustomerchoicebankRepository customerchoicebankRepository)
        {
            this._mapper = mapper;
            this._customerchoicebankRepository = customerchoicebankRepository;
        }

        /// <summary>
        /// 抓取銀行資料
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CustomerchoicebankResultModel>> CustomerchoicebankREAD()
        {
            //下載資料
            var data = await _customerchoicebankRepository.CustomerchoicebankREAD();

            //修改格式
            var dataNext = await _customerchoicebankRepository.CustomerchoicebankREADNext();

            string sql = @"
INSERT INTO [dbo].[customerchoicebank]
           ([bankno]
           ,[bankname]
           ,[bankno1]
           ,[bankname1]
           ,[BankNo1Short]
           ,[listed]
           )
     VALUES ";

            int j = 1;
            foreach (var a in dataNext)
            {
                if (j % 500 == 0)
                {
                    if (!string.IsNullOrWhiteSpace(sql))
                    {
                        sql = sql.Substring(0, sql.Length - 1);
                    }
                    sql += @"

INSERT INTO [dbo].[customerchoicebank]
           ([bankno]
           ,[bankname]
           ,[bankno1]
           ,[bankname1]
           ,[BankNo1Short]
           ,[listed]
           )
     VALUES " + "('" + a.bankno + "','" + a.bankname + "','" + a.bankno1 + "','" + a.bankname1 + "','" + (a.bankno1.Length > 3 ? a.bankno1.Substring(0, 3) : " ") + "','0'),";
                }
                else
                {
                    sql += "('" + a.bankno + "','" + a.bankname + "','" + a.bankno1 + "','" + a.bankname1 + "','" + (a.bankno1.Length > 3 ? a.bankno1.Substring(0, 3) : " ") + "','0'),";
                }
                j++;
            }

            if (!string.IsNullOrWhiteSpace(sql))
            {
                sql = sql.Substring(0, sql.Length - 1);
            }
            //刪除全部資訊
            await _customerchoicebankRepository.deleteCustomerchoicebank();
            //寫入全部資料
            await _customerchoicebankRepository.InsertCustomerchoicebank(sql);

            return null;
        }
    }
}