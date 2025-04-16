﻿using SqlSugar;
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
       
        

        public SqlAccess()
        {

            MySqlLink();
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

            

           

        }
        /// <summary>
        /// 系统数据写入
        /// </summary>
        /// <param name="dataName">数据名</param>
        /// <param name="dataValue">数据值</param>
        public static void SystemDatasWrite(string name, string value)
        {
            var sDB = new SqlAccess().SystemDataAccess;
            sys_bd_systemsettingsdatamodel data = new sys_bd_systemsettingsdatamodel();
            data.Name = name;
            data.Value = value;
            sDB.Updateable(data).Where(x => x.Name == name).ExecuteCommand();
        }

        /// <summary>
        /// 初始化表格
        /// </summary>
        public void InitializeTable()
        {
            SystemDataAccess.CodeFirst.InitTables(typeof(sys_bd_Templatedatamodel)); //*
            SystemDataAccess.CodeFirst.InitTables(typeof(sys_bd_systemsettingsdatamodel));
            SystemDataAccess.CodeFirst.InitTables(typeof(Sys_bd_activewheeltypedatamodel));
            SystemDataAccess.CodeFirst.InitTables(typeof(Sys_bd_camerainformation)); //*
            //SystemDataAccess.CodeFirst.InitTables(typeof(Processingtechnology));
            SystemDataAccess.CodeFirst.InitTables(typeof(tbl_defect_code)); //*
            SystemDataAccess.CodeFirst.InitTables(typeof(Tbl_productiondatamodel));
        }
    }
}
