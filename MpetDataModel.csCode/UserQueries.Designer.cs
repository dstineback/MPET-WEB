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

    public partial class UserQueries : XPLiteObject
    {
        int fQueryID;
        [Key(true)]
        public int QueryID
        {
            get { return fQueryID; }
            set { SetPropertyValue<int>("QueryID", ref fQueryID, value); }
        }
        MPetUsers fUserID;
        [Association(@"UserQueriesReferencesMPetUsers2")]
        public MPetUsers UserID
        {
            get { return fUserID; }
            set { SetPropertyValue<MPetUsers>("UserID", ref fUserID, value); }
        }
        string fQueryName;
        [Size(200)]
        public string QueryName
        {
            get { return fQueryName; }
            set { SetPropertyValue<string>("QueryName", ref fQueryName, value); }
        }
        string fFilter;
        [Size(1000)]
        public string Filter
        {
            get { return fFilter; }
            set { SetPropertyValue<string>("Filter", ref fFilter, value); }
        }
        string fDisplay;
        [Size(1000)]
        public string Display
        {
            get { return fDisplay; }
            set { SetPropertyValue<string>("Display", ref fDisplay, value); }
        }
        string fSql;
        [Size(1000)]
        public string Sql
        {
            get { return fSql; }
            set { SetPropertyValue<string>("Sql", ref fSql, value); }
        }
        MPetUsers fCreatedBy;
        [Association(@"UserQueriesReferencesMPetUsers")]
        public MPetUsers CreatedBy
        {
            get { return fCreatedBy; }
            set { SetPropertyValue<MPetUsers>("CreatedBy", ref fCreatedBy, value); }
        }
        DateTime fCreatedDate;
        public DateTime CreatedDate
        {
            get { return fCreatedDate; }
            set { SetPropertyValue<DateTime>("CreatedDate", ref fCreatedDate, value); }
        }
        MPetUsers fModifiedBy;
        [Association(@"UserQueriesReferencesMPetUsers1")]
        public MPetUsers ModifiedBy
        {
            get { return fModifiedBy; }
            set { SetPropertyValue<MPetUsers>("ModifiedBy", ref fModifiedBy, value); }
        }
        DateTime fLast_modified;
        public DateTime Last_modified
        {
            get { return fLast_modified; }
            set { SetPropertyValue<DateTime>("Last_modified", ref fLast_modified, value); }
        }
    }

}
