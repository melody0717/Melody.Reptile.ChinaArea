using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using HtmlAgilityPack;
using Melody.Reptile.ChinaArea.OperateData.Bll;
using Melody.Reptile.ChinaArea.OperateData.Model;

namespace Melody.Reptile.ChinaArea
{
    class Program
    {

        static void Main(string[] args)
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            OperateData.Bll.GetChinaArea bllGetChinaArea = new GetChinaArea();
            var maps= bllGetChinaArea.GetMaps();
          

            OperateData.Bll.Map bllMap=new OperateData.Bll.Map();

            System.Diagnostics.Stopwatch stopwatchInsertDatabase = new System.Diagnostics.Stopwatch();
            stopwatchInsertDatabase.Start();

            var count= bllMap.Insert(maps);
          

            stopwatchInsertDatabase.Stop();
            stopwatch.Stop();

            Console.WriteLine("数据行数：" + maps.Count);
            Console.WriteLine("成功插入数据库记录数：" + count);
            Console.WriteLine("程序总耗时："+stopwatch.Elapsed.TotalSeconds);
            Console.WriteLine("其中抓取数据耗时：" + (stopwatch.Elapsed.TotalSeconds - stopwatchInsertDatabase.Elapsed.TotalSeconds));
            Console.WriteLine("其中插入数据库耗时：" + stopwatchInsertDatabase.Elapsed.TotalSeconds);
           
            Console.Read();
        }
    }
}