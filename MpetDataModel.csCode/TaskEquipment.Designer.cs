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

    public partial class TaskEquipment : XPLiteObject
    {
        int fn_taskequipmentid;
        [Key(true)]
        public int n_taskequipmentid
        {
            get { return fn_taskequipmentid; }
            set { SetPropertyValue<int>("n_taskequipmentid", ref fn_taskequipmentid, value); }
        }
        Tasks fn_taskid;
        [Association(@"TaskEquipmentReferencesTasks")]
        public Tasks n_taskid
        {
            get { return fn_taskid; }
            set { SetPropertyValue<Tasks>("n_taskid", ref fn_taskid, value); }
        }
        tasksteps fn_taskstepid;
        [Association(@"TaskEquipmentReferencestasksteps")]
        public tasksteps n_taskstepid
        {
            get { return fn_taskstepid; }
            set { SetPropertyValue<tasksteps>("n_taskstepid", ref fn_taskstepid, value); }
        }
        MaintenanceObjects fn_objectid;
        [Association(@"TaskEquipmentReferencesMaintenanceObjects")]
        public MaintenanceObjects n_objectid
        {
            get { return fn_objectid; }
            set { SetPropertyValue<MaintenanceObjects>("n_objectid", ref fn_objectid, value); }
        }
        decimal fqtyplanned;
        public decimal qtyplanned
        {
            get { return fqtyplanned; }
            set { SetPropertyValue<decimal>("qtyplanned", ref fqtyplanned, value); }
        }
        MPetUsers fCreatedBy;
        [Association(@"TaskEquipmentReferencesMPetUsers")]
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
        [Association(@"TaskEquipmentReferencesMPetUsers1")]
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
