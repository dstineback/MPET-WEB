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

    public partial class FundSrcCodeLinks : XPLiteObject
    {
        int fn_FundSrcCodeLinkID;
        [Key(true)]
        public int n_FundSrcCodeLinkID
        {
            get { return fn_FundSrcCodeLinkID; }
            set { SetPropertyValue<int>("n_FundSrcCodeLinkID", ref fn_FundSrcCodeLinkID, value); }
        }
        FundSrcCodes fn_FundSrcCodeID;
        [Association(@"FundSrcCodeLinksReferencesFundSrcCodes")]
        public FundSrcCodes n_FundSrcCodeID
        {
            get { return fn_FundSrcCodeID; }
            set { SetPropertyValue<FundSrcCodes>("n_FundSrcCodeID", ref fn_FundSrcCodeID, value); }
        }
        WorkOrderCodes fn_WorkOrderCodeID;
        [Association(@"FundSrcCodeLinksReferencesWorkOrderCodes")]
        public WorkOrderCodes n_WorkOrderCodeID
        {
            get { return fn_WorkOrderCodeID; }
            set { SetPropertyValue<WorkOrderCodes>("n_WorkOrderCodeID", ref fn_WorkOrderCodeID, value); }
        }
        MPetUsers fCreatedBy;
        [Association(@"FundSrcCodeLinksReferencesMPetUsers")]
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
        [Association(@"FundSrcCodeLinksReferencesMPetUsers1")]
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
