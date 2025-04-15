using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Effects;
using WheelRecognitionSystem.Models;
using WheelRecognitionSystem.Public;

namespace WheelRecognitionSystem.DataAccess
{
    public class SqlAccess
    {
        /// <summary>
        /// 系统数据访问
        /// </summary>
        public SqlSugarClient SystemDataAccess;
        /// <summary>
        /// 生产数据访问
        /// </summary>
        public SqlSugarClient ProductionDataAccess;
        

        public SqlAccess()
        {

            MySqlLink();
        }

        public void MySqlite()
        {

            //用于存储系统数据的数据库
            SystemDataAccess = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = @"Data Source=" + Environment.CurrentDirectory + @"\SystemData.db",

                DbType = DbType.Sqlite,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true
            });
            SystemDataAccess.DbMaintenance.CreateDatabase();

            //用于存储历史生产数据的数据库
            ProductionDataAccess = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = @"Data Source=" + Environment.CurrentDirectory + @"\ProductionData.db",
                DbType = DbType.Sqlite,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true
            });
            ProductionDataAccess.DbMaintenance.CreateDatabase();

           
        }

        public void MySqlLink()
        {
            //用于存储系统数据的数据库
            SystemDataAccess = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = "server=127.0.0.1;uid=root;pwd=Csdk@2025;database=csdk_zj",
                DbType = DbType.MySql,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true
            });
            SystemDataAccess.DbMaintenance.CreateDatabase();

            //用于存储历史生产数据的数据库
            ProductionDataAccess = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = "server=127.0.0.1;uid=root;pwd=123456;database=ProductionData",
                DbType = DbType.MySql,//设置数据库类型     
                InitKeyType = InitKeyType.Attribute, //从实体特性中读取主键自增列信息  
                IsAutoCloseConnection = true //自动释放数据务，如果存在事务，在事务结束后释放     
            });
            ProductionDataAccess.DbMaintenance.CreateDatabase();

           

        }
        /// <summary>
        /// 系统数据写入
        /// </summary>
        /// <param name="dataName">数据名</param>
        /// <param name="dataValue">数据值</param>
        public static void SystemDatasWrite(string name, string value)
        {
            var sDB = new SqlAccess().SystemDataAccess;
            SystemSettingsDataModel data = new SystemSettingsDataModel();
            data.Name = name;
            data.Value = value;
            sDB.Updateable(data).Where(x => x.Name == name).ExecuteCommand();
        }

        /// <summary>
        /// 初始化表格
        /// </summary>
        public void InitializeTable()
        {
            SystemDataAccess.CodeFirst.InitTables(typeof(sys_bd_Templatedatamodel));
            SystemDataAccess.CodeFirst.InitTables(typeof(SystemSettingsDataModel));
            SystemDataAccess.CodeFirst.InitTables(typeof(ActiveWheelTypeDataModel));
            SystemDataAccess.CodeFirst.InitTables(typeof(Sys_bd_camerainformation));
            SystemDataAccess.CodeFirst.InitTables(typeof(Processingtechnology));
            SystemDataAccess.CodeFirst.InitTables(typeof(tbl_defect_code));
            ProductionDataAccess.CodeFirst.InitTables(typeof(ProductionDataModel));
        }
    }
}
