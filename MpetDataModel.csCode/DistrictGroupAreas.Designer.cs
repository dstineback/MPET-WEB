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

    public partial class DistrictGroupAreas : XPLiteObject
    {
        int fUniqueID;
        [Key(true)]
        public int UniqueID
        {
            get { return fUniqueID; }
            set { SetPropertyValue<int>("UniqueID", ref fUniqueID, value); }
        }
        DistrictGroups fDistrictGroupID;
        [Association(@"DistrictGroupAreasReferencesDistrictGroups")]
        public DistrictGroups DistrictGroupID
        {
            get { return fDistrictGroupID; }
            set { SetPropertyValue<DistrictGroups>("DistrictGroupID", ref fDistrictGroupID, value); }
        }
        Areas fAreaID;
        [Association(@"DistrictGroupAreasReferencesAreas")]
        public Areas AreaID
        {
            get { return fAreaID; }
            set { SetPropertyValue<Areas>("AreaID", ref fAreaID, value); }
        }
        MPetUsers fCreatedBy;
        [Association(@"DistrictGroupAreasReferencesMPetUsers")]
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
        [Association(@"DistrictGroupAreasReferencesMPetUsers1")]
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
