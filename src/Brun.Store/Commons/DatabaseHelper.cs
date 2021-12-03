using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Store.Commons
{
    /// <summary>
    /// 检测数据库，创建数据库
    /// </summary>
    public class DatabaseHelper
    {
        public static bool ExistDatabase(string connectionString, DbType dbType)
        {
            //TODO 检测数据库
            //if()
            SqlSugar.ConnectionConfig connectionConfig = new ConnectionConfig()
            {
                ConnectionString = connectionString,
                DbType = (SqlSugar.DbType)dbType,
                IsAutoCloseConnection = false,
            };
            using (SqlSugarClient db = new SqlSugarClient(connectionConfig))
            {
                var isc = db.DbMaintenance.CreateDatabase();
                db.CodeFirst.SetStringDefaultLength(200).InitTables(typeof(Store.Entities.WorkerEntity));
                db.CodeFirst.SetStringDefaultLength(200).InitTables(typeof(Store.Entities.OnceBrunEntity));
            }
            return true;

            //using (SqlSugarClient db = new SqlSugarClient(connectionConfig))
            //{
            //    var conn = db.Ado.Connection;
            //    string dbName = db.Ado.Connection.Database;
            //    //conn.Open();
            //    //conn.ChangeDatabase("");
            //    int i = db.Ado.SqlQuery<int>($"select count(*) from pg_catalog.pg_database where pg_catalog.pg_database.datname='{dbName}'").FirstOrDefault();
            //    if (i <= 0)
            //    {
            //        Console.WriteLine("创建数据库");
            //        var isc = db.DbMaintenance.CreateDatabase();
            //        Console.WriteLine("是否创建了数据库？{0}", isc);
            //        db.CodeFirst.SetStringDefaultLength(200).InitTables(typeof(Store.Entities.WorkerEntity));
            //        db.CodeFirst.SetStringDefaultLength(200).InitTables(typeof(Store.Entities.OnceBrunEntity));
            //    }
            //}
        }
    }
}
