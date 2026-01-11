using SqlSugar;
using System;

namespace PublicInfos.Model
{
    [SplitTable(SplitType.Year)]
    [SugarTable("Record_{year}{month}{day}")]
    [SugarIndex("index_{split_table}_groupid_datetime", nameof(GroupID), OrderByType.Asc, nameof(DateTime), OrderByType.Desc)]
    public class Record
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }

        public long GroupID { get; set; }

        public long QQID { get; set; }

        public string Message { get; set; }

        [SplitField]
        public DateTime DateTime { get; set; }
    }
}
