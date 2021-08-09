using PublicInfos.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PublicInfos
{
    public static class SQLHelper
    {
        private static SqlSugarClient GetInstance()
        {
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                //TODO: 插件发布时替换此处
                //ConnectionString = $"data source={Path.Combine(Environment.CurrentDirectory, "data.db")}",
                ConnectionString = $"data source={MainSave.DBPath}",
                DbType = DbType.Sqlite,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute,
            });
            return db;
        }
        /// <summary>
        /// 数据库不存在时将会创建
        /// </summary>
        public static void CreateDB()
        {
            using (var db = GetInstance())
            {
                //TODO: 插件发布时替换此处
                db.DbMaintenance.CreateDatabase(MainSave.DBPath);
                //db.DbMaintenance.CreateDatabase(Path.Combine(Environment.CurrentDirectory, "data.db"));
                db.CodeFirst.InitTables(typeof(Record));
            }
        }
        public static void AddRecord(long GroupID, long QQID, string Message)
        {
            AddRecord(new Record { GroupID = GroupID, QQID = QQID, Message = Message, DateTime = DateTime.Now });
        }
        public static void AddRecord(Record record)
        {
            using (var db = GetInstance())
            {
                db.Insertable(record).ExecuteCommand();
            }
        }
        public static List<Record> GetRecordsByDate(long groupID, DateTime dateTime)
        {
            using (var db = GetInstance())
            {
                var ls = db.Queryable<Record>()
                    .Where(x => x.GroupID == groupID).ToList();
                ls.ForEach(x => x.Message = Regex.Replace(x.Message, @"\[CQ.*\]", ""));
                string[] filter = CloudConfig.FilterWord.Split('|');
                if (filter.Length >= 1 && !string.IsNullOrWhiteSpace(filter[0]))
                    ls = ls.Where(x => !filter.Any(o => x.Message.Contains(o))).ToList();
                return ls.Where(x => x.DateTime.ToLongDateString() == dateTime.ToLongDateString()).ToList();
            }
        }
    }
}
