using CoreHangfire.Repository.DataModel;
using CoreHangfire.Repository.Helper;
using CoreHangfire.Repository.Interface;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CoreHangfire.Repository.Implement
{
    public class CustomerchoicebankRepository : ICustomerchoicebankRepository
    {
        private readonly IDatabaseHelper DatabaseHelper;

        public CustomerchoicebankRepository(IDatabaseHelper databaseHelper)
        {
            this.DatabaseHelper = databaseHelper;
        }

        /// <summary>
        /// 讀取銀行資料
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CustomerchoicebankDataModel>> CustomerchoicebankREAD()
        {
            if (System.IO.File.Exists(@"bank.txt"))
            {
                // Use a try block to catch IOExceptions, to handle the case of the file already
                // being opened by another process.

                System.IO.File.Delete(@"bank.txt");
            }
            WebClient wc = new WebClient();
            ServicePointManager.ServerCertificateValidationCallback =
                       delegate { return true; };
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;//SecurityProtocolType.Tls
            wc.DownloadFile("https://www.fisc.com.tw/tc/download/twd.txt", @"bank.txt");
            wc.Dispose();
            System.Text.Encoding encode = System.Text.Encoding.GetEncoding("big5");
            StreamReader sr = new StreamReader(@"bank.txt", encode);
            string line;

            List<CustomerchoicebankDataModel> listAll = new List<CustomerchoicebankDataModel>();

            var toatlname = "";
            while ((line = sr.ReadLine()) != null)
            {
                //開始一行一行讀取囉!!
                var t = line.ToString().Split(new Char[] { });
                List<string> list2 = new List<string>();

                for (int i = 0; i <= t.Length - 1; i++)
                {
                    if (t[i] != "")
                    {
                        list2.Add(t[i]);
                    }
                }

                var bankno = list2[0].Length == 3 ? list2[0] : list2[0].Substring(0, 3);
                var bankname = list2[0].Length == 3 ? list2[1] : toatlname;
                var bankno1 = list2[0].Length == 3 ? " " : list2[0].Substring(3, 4);
                var bankname1 = list2[0].Length == 3 ? " " : list2[1];
                if (list2[0].Length == 3)
                {
                    toatlname = list2[1];
                }

                listAll.Add(new CustomerchoicebankDataModel { bankno = bankno, bankname = bankname, bankno1 = bankno1, bankname1 = bankname1, BankNo1Short = "123" });
            };
            Writetxt(listAll);
            return listAll;
        }

        /// <summary>
        /// 銀行寫入TXT
        /// </summary>
        /// <param name="data"></param>
        private void Writetxt(IEnumerable<CustomerchoicebankDataModel> data)
        {
            if (System.IO.File.Exists(@"bank3.txt"))
            {
                // Use a try block to catch IOExceptions, to handle the case of the file already
                // being opened by another process.

                System.IO.File.Delete(@"bank3.txt");
            }
            FileStream fs = new FileStream("bank3.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("big5"));
            foreach (var a2 in data)
            {
                sw.Write(a2.bankno + "   " + a2.bankname + "   " + a2.bankno1 + "   " + a2.bankname1 + "\r\n");
                // sw.WriteLine(Environment.NewLine);
            }

            sw.Flush();
            sw.Close();
            fs.Close();
        }

        /// <summary>
        /// 修改銀行資料格式
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CustomerchoicebankDataModel>> CustomerchoicebankREADNext()
        {
            System.Text.Encoding encode = System.Text.Encoding.GetEncoding("big5");
            StreamReader sr = new StreamReader(@"bank3.txt", encode);
            string line;
            List<CustomerchoicebankDataModel> listAll2 = new List<CustomerchoicebankDataModel>();

            var toatlname = "";
            while ((line = sr.ReadLine()) != null)
            {
                //開始一行一行讀取囉!!
                var t = line.ToString().Split(new Char[] { });
                List<string> list2 = new List<string>();

                for (int i = 0; i <= t.Length - 1; i++)
                {
                    if (t[i] != "")
                    {
                        list2.Add(t[i]);
                    }
                }

                var bankno = list2[0];
                var bankname = list2[1];
                var bankno1 = list2[list2.Count - 2] == list2[0] ? "" : list2[list2.Count - 2];
                var bankname1 = list2[list2.Count - 1] == list2[1] ? "" : list2[list2.Count - 1];
                var BankNo1Short = (bankno + bankno1).Length > 5 ? (bankno + bankno1).Substring(3, 3) : "";
                if ((bankno + bankno1).Length > 5)
                {
                    if (bankname1.Contains("農會") || bankname1.Contains("漁會"))
                    {
                        toatlname = bankname1;
                        bankname = toatlname;
                    }
                    else if (bankname1.Contains("分部") || bankname1.Contains("辦事處"))
                    {
                        bankname = toatlname;
                    }
                }
                if (bankname == "財團法人全國農漁業及金融資訊中心" || bankname == "財團法人農漁會聯合資訊中心" || bankname == "板橋區農會電腦共用中心" || bankname == "財團法人農漁會南區資訊中心" || bankname == "中華民國信用合作社聯合社南區聯合資訊處理")
                {
                }
                else
                {
                    listAll2.Add(new CustomerchoicebankDataModel { bankno = bankno, bankname = bankname, bankno1 = bankno1, bankname1 = bankname1, BankNo1Short = BankNo1Short });
                }
            };

            return listAll2;
        }

        /// <summary>
        /// 寫入金融資訊
        /// </summary>
        /// <param name="SQL"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> InsertCustomerchoicebank(string SQL)
        {
            using (System.Data.SqlClient.SqlConnection conn = DatabaseHelper.GetConnection(this.DatabaseHelper.HangfireConnectionString) as SqlConnection)
            {
                var result = await conn.QueryAsync<string>(SQL);

                return result;
            }
        }

        /// <summary>
        /// 刪除金融資訊
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<string>> deleteCustomerchoicebank()
        {
            using (System.Data.SqlClient.SqlConnection conn = DatabaseHelper.GetConnection(this.DatabaseHelper.HangfireConnectionString) as SqlConnection)
            {
                var result = await conn.QueryAsync<string>(deleteCustomerchoicebankSQL());

                return result;
            }
        }

        /// <summary>
        /// 刪除金融資訊SQL
        /// </summary>
        /// <returns></returns>
        private string deleteCustomerchoicebankSQL()
        {
            string sql = $@"delete customerchoicebank";

            return sql;
        }
    }
}