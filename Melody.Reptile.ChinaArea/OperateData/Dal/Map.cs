using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melody.Reptile.ChinaArea.OperateData.Model;
using Dapper;
namespace Melody.Reptile.ChinaArea.OperateData.Dal
{
  public  class Map
    {
        private static  readonly  string connectionString ="Data Source=.;Initial Catalog=ChinaArea;User Id=sa;Password=123;";

        IDbConnection conn=new SqlConnection(connectionString);

        public int Insert(List<Model.Map> maps)
        {
            string insertSql = "insert into map values(@Code,@Name,@Depth,@ParentCode)";

            return conn.Execute(insertSql, maps);
        }
    }
}
