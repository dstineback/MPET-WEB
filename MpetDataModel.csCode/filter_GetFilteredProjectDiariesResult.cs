//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
namespace MpetDataModel.cs
{

    [NonPersistent]
    public partial class filter_GetFilteredProjectDiariesResult
    {
        public int n_ProjectDiaryID { get; set; }
        public int n_ProjectID { get; set; }
        public string ProjectID { get; set; }
        public int n_EngineerID { get; set; }
        public string EngineerID { get; set; }
        public int n_weatherid { get; set; }
        public string WeatherID { get; set; }
        public int n_ProjectCondition { get; set; }
        public string ProjectCondition { get; set; }
        public DateTime WorkDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string DiaryNum { get; set; }
        public int FlaggedRecordID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }

}
