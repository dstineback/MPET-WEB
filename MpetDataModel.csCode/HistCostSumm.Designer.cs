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

    public partial class HistCostSumm : XPLiteObject
    {
        int fUniqueID;
        [Key(true)]
        public int UniqueID
        {
            get { return fUniqueID; }
            set { SetPropertyValue<int>("UniqueID", ref fUniqueID, value); }
        }
        Jobs fn_HistoryID;
        [Association(@"HistCostSummReferencesJobs")]
        public Jobs n_HistoryID
        {
            get { return fn_HistoryID; }
            set { SetPropertyValue<Jobs>("n_HistoryID", ref fn_HistoryID, value); }
        }
        string fJobID;
        [Size(20)]
        public string JobID
        {
            get { return fJobID; }
            set { SetPropertyValue<string>("JobID", ref fJobID, value); }
        }
        decimal fTTLParts;
        public decimal TTLParts
        {
            get { return fTTLParts; }
            set { SetPropertyValue<decimal>("TTLParts", ref fTTLParts, value); }
        }
        decimal fTTLLabor;
        public decimal TTLLabor
        {
            get { return fTTLLabor; }
            set { SetPropertyValue<decimal>("TTLLabor", ref fTTLLabor, value); }
        }
        decimal fTTLEquipment;
        public decimal TTLEquipment
        {
            get { return fTTLEquipment; }
            set { SetPropertyValue<decimal>("TTLEquipment", ref fTTLEquipment, value); }
        }
        decimal fTTLOther;
        public decimal TTLOther
        {
            get { return fTTLOther; }
            set { SetPropertyValue<decimal>("TTLOther", ref fTTLOther, value); }
        }
    }

}
