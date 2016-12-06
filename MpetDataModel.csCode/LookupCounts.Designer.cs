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

    public partial class LookupCounts : XPLiteObject
    {
        int fUniqueID;
        [Key(true)]
        public int UniqueID
        {
            get { return fUniqueID; }
            set { SetPropertyValue<int>("UniqueID", ref fUniqueID, value); }
        }
        MPetUsers fnUserID;
        [Association(@"LookupCountsReferencesMPetUsers")]
        public MPetUsers nUserID
        {
            get { return fnUserID; }
            set { SetPropertyValue<MPetUsers>("nUserID", ref fnUserID, value); }
        }
        int fItemsToShow;
        public int ItemsToShow
        {
            get { return fItemsToShow; }
            set { SetPropertyValue<int>("ItemsToShow", ref fItemsToShow, value); }
        }
        string fTableName;
        [Size(50)]
        public string TableName
        {
            get { return fTableName; }
            set { SetPropertyValue<string>("TableName", ref fTableName, value); }
        }
        char fModifiedOrder;
        public char ModifiedOrder
        {
            get { return fModifiedOrder; }
            set { SetPropertyValue<char>("ModifiedOrder", ref fModifiedOrder, value); }
        }
        string fLookupOrdering;
        [Size(SizeAttribute.Unlimited)]
        public string LookupOrdering
        {
            get { return fLookupOrdering; }
            set { SetPropertyValue<string>("LookupOrdering", ref fLookupOrdering, value); }
        }
        MPetUsers fCreatedBy;
        [Association(@"LookupCountsReferencesMPetUsers1")]
        public MPetUsers CreatedBy
        {
            get { return fCreatedBy; }
            set { SetPropertyValue<MPetUsers>("CreatedBy", ref fCreatedBy, value); }
        }
        DateTime fCreatedOn;
        public DateTime CreatedOn
        {
            get { return fCreatedOn; }
            set { SetPropertyValue<DateTime>("CreatedOn", ref fCreatedOn, value); }
        }
        MPetUsers fModifiedBy;
        [Association(@"LookupCountsReferencesMPetUsers2")]
        public MPetUsers ModifiedBy
        {
            get { return fModifiedBy; }
            set { SetPropertyValue<MPetUsers>("ModifiedBy", ref fModifiedBy, value); }
        }
        DateTime fModifiedOn;
        public DateTime ModifiedOn
        {
            get { return fModifiedOn; }
            set { SetPropertyValue<DateTime>("ModifiedOn", ref fModifiedOn, value); }
        }
    }

}
