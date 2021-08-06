using SqlSugar;
using System;

namespace PublicInfos.Model
{
    [SugarTable("Record")]
    public class Record
    {
        [SugarColumn(IsIdentity =true, IsPrimaryKey =true)]
        public int RowID { get; set; }
        public long GroupID { get; set; }
        public long QQID { get; set; }
        public string Message { get; set; }
        public DateTime DateTime { get; set; }
    }
}
