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

    public partial class MaintenanceObjects : XPLiteObject
    {
        int fn_objectid;
        [Key(true)]
        public int n_objectid
        {
            get { return fn_objectid; }
            set { SetPropertyValue<int>("n_objectid", ref fn_objectid, value); }
        }
        string fobjectid;
        [Indexed(Name = @"uniqueObjectIds", Unique = true)]
        [Size(30)]
        public string objectid
        {
            get { return fobjectid; }
            set { SetPropertyValue<string>("objectid", ref fobjectid, value); }
        }
        string fdescription;
        [Size(254)]
        public string description
        {
            get { return fdescription; }
            set { SetPropertyValue<string>("description", ref fdescription, value); }
        }
        string fAssignedGUID;
        [Size(50)]
        public string AssignedGUID
        {
            get { return fAssignedGUID; }
            set { SetPropertyValue<string>("AssignedGUID", ref fAssignedGUID, value); }
        }
        Tasks ftasknum;
        [Association(@"MaintenanceObjectsReferencesTasks")]
        public Tasks tasknum
        {
            get { return ftasknum; }
            set { SetPropertyValue<Tasks>("tasknum", ref ftasknum, value); }
        }
        MaintenanceObjects fn_parentobjectid;
        [Association(@"MaintenanceObjectsReferencesMaintenanceObjects")]
        public MaintenanceObjects n_parentobjectid
        {
            get { return fn_parentobjectid; }
            set { SetPropertyValue<MaintenanceObjects>("n_parentobjectid", ref fn_parentobjectid, value); }
        }
        Areas fn_areaid;
        [Association(@"MaintenanceObjectsReferencesAreas")]
        public Areas n_areaid
        {
            get { return fn_areaid; }
            set { SetPropertyValue<Areas>("n_areaid", ref fn_areaid, value); }
        }
        CostCodes fn_costcodeid;
        [Association(@"MaintenanceObjectsReferencesCostCodes")]
        public CostCodes n_costcodeid
        {
            get { return fn_costcodeid; }
            set { SetPropertyValue<CostCodes>("n_costcodeid", ref fn_costcodeid, value); }
        }
        locations fn_locationid;
        [Association(@"MaintenanceObjectsReferenceslocations")]
        public locations n_locationid
        {
            get { return fn_locationid; }
            set { SetPropertyValue<locations>("n_locationid", ref fn_locationid, value); }
        }
        Manufacturers fn_mfgid;
        [Association(@"MaintenanceObjectsReferencesManufacturers")]
        public Manufacturers n_mfgid
        {
            get { return fn_mfgid; }
            set { SetPropertyValue<Manufacturers>("n_mfgid", ref fn_mfgid, value); }
        }
        objectclasses fn_objclassid;
        [Association(@"MaintenanceObjectsReferencesobjectclasses")]
        public objectclasses n_objclassid
        {
            get { return fn_objclassid; }
            set { SetPropertyValue<objectclasses>("n_objclassid", ref fn_objclassid, value); }
        }
        objecttypes fn_objtypeid;
        [Association(@"MaintenanceObjectsReferencesobjecttypes")]
        public objecttypes n_objtypeid
        {
            get { return fn_objtypeid; }
            set { SetPropertyValue<objecttypes>("n_objtypeid", ref fn_objtypeid, value); }
        }
        ProductLineTypes fn_prodlineid;
        [Association(@"MaintenanceObjectsReferencesProductLineTypes")]
        public ProductLineTypes n_prodlineid
        {
            get { return fn_prodlineid; }
            set { SetPropertyValue<ProductLineTypes>("n_prodlineid", ref fn_prodlineid, value); }
        }
        Storerooms fn_storeroomid;
        [Association(@"MaintenanceObjectsReferencesStorerooms")]
        public Storerooms n_storeroomid
        {
            get { return fn_storeroomid; }
            set { SetPropertyValue<Storerooms>("n_storeroomid", ref fn_storeroomid, value); }
        }
        string fNotes;
        [Size(SizeAttribute.Unlimited)]
        public string Notes
        {
            get { return fNotes; }
            set { SetPropertyValue<string>("Notes", ref fNotes, value); }
        }
        string fassetnumber;
        [Size(21)]
        public string assetnumber
        {
            get { return fassetnumber; }
            set { SetPropertyValue<string>("assetnumber", ref fassetnumber, value); }
        }
        string fb_active;
        [Size(1)]
        public string b_active
        {
            get { return fb_active; }
            set { SetPropertyValue<string>("b_active", ref fb_active, value); }
        }
        string fb_chargeable;
        [Size(1)]
        public string b_chargeable
        {
            get { return fb_chargeable; }
            set { SetPropertyValue<string>("b_chargeable", ref fb_chargeable, value); }
        }
        decimal fcharge_rate;
        public decimal charge_rate
        {
            get { return fcharge_rate; }
            set { SetPropertyValue<decimal>("charge_rate", ref fcharge_rate, value); }
        }
        string fb_oeefocus;
        [Size(1)]
        public string b_oeefocus
        {
            get { return fb_oeefocus; }
            set { SetPropertyValue<string>("b_oeefocus", ref fb_oeefocus, value); }
        }
        string fb_route;
        [Size(1)]
        public string b_route
        {
            get { return fb_route; }
            set { SetPropertyValue<string>("b_route", ref fb_route, value); }
        }
        string ffundmtltype;
        [Size(10)]
        public string fundmtltype
        {
            get { return ffundmtltype; }
            set { SetPropertyValue<string>("fundmtltype", ref ffundmtltype, value); }
        }
        decimal fGPS_X;
        public decimal GPS_X
        {
            get { return fGPS_X; }
            set { SetPropertyValue<decimal>("GPS_X", ref fGPS_X, value); }
        }
        decimal fGPS_Y;
        public decimal GPS_Y
        {
            get { return fGPS_Y; }
            set { SetPropertyValue<decimal>("GPS_Y", ref fGPS_Y, value); }
        }
        decimal fGPS_Z;
        public decimal GPS_Z
        {
            get { return fGPS_Z; }
            set { SetPropertyValue<decimal>("GPS_Z", ref fGPS_Z, value); }
        }
        int flogicalorder;
        public int logicalorder
        {
            get { return flogicalorder; }
            set { SetPropertyValue<int>("logicalorder", ref flogicalorder, value); }
        }
        int fidealcycle;
        public int idealcycle
        {
            get { return fidealcycle; }
            set { SetPropertyValue<int>("idealcycle", ref fidealcycle, value); }
        }
        DateTime finservicedate;
        public DateTime inservicedate
        {
            get { return finservicedate; }
            set { SetPropertyValue<DateTime>("inservicedate", ref finservicedate, value); }
        }
        string fmfgid;
        [Size(20)]
        public string mfgid
        {
            get { return fmfgid; }
            set { SetPropertyValue<string>("mfgid", ref fmfgid, value); }
        }
        string fmfgmodel;
        [Size(30)]
        public string mfgmodel
        {
            get { return fmfgmodel; }
            set { SetPropertyValue<string>("mfgmodel", ref fmfgmodel, value); }
        }
        string fmiscrefnum;
        [Size(30)]
        public string miscrefnum
        {
            get { return fmiscrefnum; }
            set { SetPropertyValue<string>("miscrefnum", ref fmiscrefnum, value); }
        }
        int fobjectcount;
        public int objectcount
        {
            get { return fobjectcount; }
            set { SetPropertyValue<int>("objectcount", ref fobjectcount, value); }
        }
        DateTime fpurchasedate;
        public DateTime purchasedate
        {
            get { return fpurchasedate; }
            set { SetPropertyValue<DateTime>("purchasedate", ref fpurchasedate, value); }
        }
        decimal fpurchaseprice;
        public decimal purchaseprice
        {
            get { return fpurchaseprice; }
            set { SetPropertyValue<decimal>("purchaseprice", ref fpurchaseprice, value); }
        }
        string fremarks;
        [Size(127)]
        public string remarks
        {
            get { return fremarks; }
            set { SetPropertyValue<string>("remarks", ref fremarks, value); }
        }
        string fserialnumber;
        [Size(30)]
        public string serialnumber
        {
            get { return fserialnumber; }
            set { SetPropertyValue<string>("serialnumber", ref fserialnumber, value); }
        }
        DateTime fstatusdate;
        public DateTime statusdate
        {
            get { return fstatusdate; }
            set { SetPropertyValue<DateTime>("statusdate", ref fstatusdate, value); }
        }
        DateTime fwarrantyexpdate;
        public DateTime warrantyexpdate
        {
            get { return fwarrantyexpdate; }
            set { SetPropertyValue<DateTime>("warrantyexpdate", ref fwarrantyexpdate, value); }
        }
        OverheadRates fn_OverheadRateID;
        [Association(@"MaintenanceObjectsReferencesOverheadRates")]
        public OverheadRates n_OverheadRateID
        {
            get { return fn_OverheadRateID; }
            set { SetPropertyValue<OverheadRates>("n_OverheadRateID", ref fn_OverheadRateID, value); }
        }
        MPetUsers fn_ResponsiblePerson;
        [Association(@"MaintenanceObjectsReferencesMPetUsers")]
        public MPetUsers n_ResponsiblePerson
        {
            get { return fn_ResponsiblePerson; }
            set { SetPropertyValue<MPetUsers>("n_ResponsiblePerson", ref fn_ResponsiblePerson, value); }
        }
        MaintObjectsConditionCodes fCurrentConditionCode;
        [Association(@"MaintenanceObjectsReferencesMaintObjectsConditionCodes")]
        public MaintObjectsConditionCodes CurrentConditionCode
        {
            get { return fCurrentConditionCode; }
            set { SetPropertyValue<MaintObjectsConditionCodes>("CurrentConditionCode", ref fCurrentConditionCode, value); }
        }
        DateTime fExpectedLifeExpirationDate;
        public DateTime ExpectedLifeExpirationDate
        {
            get { return fExpectedLifeExpirationDate; }
            set { SetPropertyValue<DateTime>("ExpectedLifeExpirationDate", ref fExpectedLifeExpirationDate, value); }
        }
        Vendors fPurchaseVendorID;
        [Association(@"MaintenanceObjectsReferencesVendors")]
        public Vendors PurchaseVendorID
        {
            get { return fPurchaseVendorID; }
            set { SetPropertyValue<Vendors>("PurchaseVendorID", ref fPurchaseVendorID, value); }
        }
        decimal fMilePost;
        public decimal MilePost
        {
            get { return fMilePost; }
            set { SetPropertyValue<decimal>("MilePost", ref fMilePost, value); }
        }
        MilePostDirections fn_MilePostDirection;
        [Association(@"MaintenanceObjectsReferencesMilePostDirections")]
        public MilePostDirections n_MilePostDirection
        {
            get { return fn_MilePostDirection; }
            set { SetPropertyValue<MilePostDirections>("n_MilePostDirection", ref fn_MilePostDirection, value); }
        }
        StateRoutes fn_StateRouteID;
        [Association(@"MaintenanceObjectsReferencesStateRoutes")]
        public StateRoutes n_StateRouteID
        {
            get { return fn_StateRouteID; }
            set { SetPropertyValue<StateRoutes>("n_StateRouteID", ref fn_StateRouteID, value); }
        }
        decimal fEasting;
        public decimal Easting
        {
            get { return fEasting; }
            set { SetPropertyValue<decimal>("Easting", ref fEasting, value); }
        }
        decimal fNorthing;
        public decimal Northing
        {
            get { return fNorthing; }
            set { SetPropertyValue<decimal>("Northing", ref fNorthing, value); }
        }
        int fWarrantyIntervalDays;
        public int WarrantyIntervalDays
        {
            get { return fWarrantyIntervalDays; }
            set { SetPropertyValue<int>("WarrantyIntervalDays", ref fWarrantyIntervalDays, value); }
        }
        int fLifeCycleIntervalDays;
        public int LifeCycleIntervalDays
        {
            get { return fLifeCycleIntervalDays; }
            set { SetPropertyValue<int>("LifeCycleIntervalDays", ref fLifeCycleIntervalDays, value); }
        }
        MPetUsers fCreatedBy;
        [Association(@"MaintenanceObjectsReferencesMPetUsers1")]
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
        [Association(@"MaintenanceObjectsReferencesMPetUsers2")]
        public MPetUsers ModifiedBy
        {
            get { return fModifiedBy; }
            set { SetPropertyValue<MPetUsers>("ModifiedBy", ref fModifiedBy, value); }
        }
        DateTime fLastModified;
        public DateTime LastModified
        {
            get { return fLastModified; }
            set { SetPropertyValue<DateTime>("LastModified", ref fLastModified, value); }
        }
        UnitsOfMeasure fn_UnitsOfMeasureID;
        [Association(@"MaintenanceObjectsReferencesUnitsOfMeasure")]
        public UnitsOfMeasure n_UnitsOfMeasureID
        {
            get { return fn_UnitsOfMeasureID; }
            set { SetPropertyValue<UnitsOfMeasure>("n_UnitsOfMeasureID", ref fn_UnitsOfMeasureID, value); }
        }
        decimal fMilePostTo;
        public decimal MilePostTo
        {
            get { return fMilePostTo; }
            set { SetPropertyValue<decimal>("MilePostTo", ref fMilePostTo, value); }
        }
        decimal fQuantity;
        public decimal Quantity
        {
            get { return fQuantity; }
            set { SetPropertyValue<decimal>("Quantity", ref fQuantity, value); }
        }
        decimal favailablehours;
        public decimal availablehours
        {
            get { return favailablehours; }
            set { SetPropertyValue<decimal>("availablehours", ref favailablehours, value); }
        }
        decimal fpmhours;
        public decimal pmhours
        {
            get { return fpmhours; }
            set { SetPropertyValue<decimal>("pmhours", ref fpmhours, value); }
        }
        decimal ftotalavailablehours;
        public decimal totalavailablehours
        {
            get { return ftotalavailablehours; }
            set { SetPropertyValue<decimal>("totalavailablehours", ref ftotalavailablehours, value); }
        }
        FundSrcCodes fn_FundSrcCodeID;
        [Association(@"MaintenanceObjectsReferencesFundSrcCodes")]
        public FundSrcCodes n_FundSrcCodeID
        {
            get { return fn_FundSrcCodeID; }
            set { SetPropertyValue<FundSrcCodes>("n_FundSrcCodeID", ref fn_FundSrcCodeID, value); }
        }
        WorkOrderCodes fn_WorkOrderCodeID;
        [Association(@"MaintenanceObjectsReferencesWorkOrderCodes")]
        public WorkOrderCodes n_WorkOrderCodeID
        {
            get { return fn_WorkOrderCodeID; }
            set { SetPropertyValue<WorkOrderCodes>("n_WorkOrderCodeID", ref fn_WorkOrderCodeID, value); }
        }
        WorkOperations fn_WorkOpID;
        [Association(@"MaintenanceObjectsReferencesWorkOperations")]
        public WorkOperations n_WorkOpID
        {
            get { return fn_WorkOpID; }
            set { SetPropertyValue<WorkOperations>("n_WorkOpID", ref fn_WorkOpID, value); }
        }
        OrganizationCodes fn_OrganizationCodeID;
        [Association(@"MaintenanceObjectsReferencesOrganizationCodes")]
        public OrganizationCodes n_OrganizationCodeID
        {
            get { return fn_OrganizationCodeID; }
            set { SetPropertyValue<OrganizationCodes>("n_OrganizationCodeID", ref fn_OrganizationCodeID, value); }
        }
        FundingGroupCodes fn_FundingGroupCodeID;
        [Association(@"MaintenanceObjectsReferencesFundingGroupCodes")]
        public FundingGroupCodes n_FundingGroupCodeID
        {
            get { return fn_FundingGroupCodeID; }
            set { SetPropertyValue<FundingGroupCodes>("n_FundingGroupCodeID", ref fn_FundingGroupCodeID, value); }
        }
        ControlSections fn_ControlSectionID;
        [Association(@"MaintenanceObjectsReferencesControlSections")]
        public ControlSections n_ControlSectionID
        {
            get { return fn_ControlSectionID; }
            set { SetPropertyValue<ControlSections>("n_ControlSectionID", ref fn_ControlSectionID, value); }
        }
        EquipmentNumber fn_EquipmentNumberID;
        [Association(@"MaintenanceObjectsReferencesEquipmentNumber")]
        public EquipmentNumber n_EquipmentNumberID
        {
            get { return fn_EquipmentNumberID; }
            set { SetPropertyValue<EquipmentNumber>("n_EquipmentNumberID", ref fn_EquipmentNumberID, value); }
        }
        [Association(@"AccumulatorsReferencesMaintenanceObjects", typeof(Accumulators))]
        public XPCollection<Accumulators> AccumulatorsCollection { get { return GetCollection<Accumulators>("AccumulatorsCollection"); } }
        [Association(@"AttachmentsReferencesMaintenanceObjects", typeof(Attachments))]
        public XPCollection<Attachments> AttachmentsCollection { get { return GetCollection<Attachments>("AttachmentsCollection"); } }
        [Association(@"FieldDataCollectionReferencesMaintenanceObjects", typeof(FieldDataCollection))]
        public XPCollection<FieldDataCollection> FieldDataCollections { get { return GetCollection<FieldDataCollection>("FieldDataCollections"); } }
        [Association(@"JobEquipmentReferencesMaintenanceObjects", typeof(JobEquipment))]
        public XPCollection<JobEquipment> JobEquipments { get { return GetCollection<JobEquipment>("JobEquipments"); } }
        [Association(@"JobsReferencesMaintenanceObjects", typeof(Jobs))]
        public XPCollection<Jobs> JobsCollection { get { return GetCollection<Jobs>("JobsCollection"); } }
        [Association(@"JobsReferencesMaintenanceObjects1", typeof(Jobs))]
        public XPCollection<Jobs> JobsCollection1 { get { return GetCollection<Jobs>("JobsCollection1"); } }
        [Association(@"JobstepsReferencesMaintenanceObjects", typeof(Jobsteps))]
        public XPCollection<Jobsteps> JobstepsCollection { get { return GetCollection<Jobsteps>("JobstepsCollection"); } }
        [Association(@"MaintenanceObjectsReferencesMaintenanceObjects", typeof(MaintenanceObjects))]
        public XPCollection<MaintenanceObjects> MaintenanceObjectsCollection { get { return GetCollection<MaintenanceObjects>("MaintenanceObjectsCollection"); } }
        [Association(@"ObjectPartsReferencesMaintenanceObjects", typeof(ObjectParts))]
        public XPCollection<ObjectParts> ObjectPartsCollection { get { return GetCollection<ObjectParts>("ObjectPartsCollection"); } }
        [Association(@"ObjectRunUnitsReferencesMaintenanceObjects", typeof(ObjectRunUnits))]
        public XPCollection<ObjectRunUnits> ObjectRunUnitsCollection { get { return GetCollection<ObjectRunUnits>("ObjectRunUnitsCollection"); } }
        [Association(@"ObjectTasksReferencesMaintenanceObjects", typeof(ObjectTasks))]
        public XPCollection<ObjectTasks> ObjectTasksCollection { get { return GetCollection<ObjectTasks>("ObjectTasksCollection"); } }
        [Association(@"RouteMembersReferencesMaintenanceObjects", typeof(RouteMembers))]
        public XPCollection<RouteMembers> RouteMembersCollection { get { return GetCollection<RouteMembers>("RouteMembersCollection"); } }
        [Association(@"TaskEquipmentReferencesMaintenanceObjects", typeof(TaskEquipment))]
        public XPCollection<TaskEquipment> TaskEquipments { get { return GetCollection<TaskEquipment>("TaskEquipments"); } }
        [Association(@"TestpointReadingsReferencesMaintenanceObjects", typeof(TestpointReadings))]
        public XPCollection<TestpointReadings> TestpointReadingsCollection { get { return GetCollection<TestpointReadings>("TestpointReadingsCollection"); } }
        [Association(@"TestpointsReferencesMaintenanceObjects", typeof(Testpoints))]
        public XPCollection<Testpoints> TestpointsCollection { get { return GetCollection<Testpoints>("TestpointsCollection"); } }
        [Association(@"time_batch_equipReferencesMaintenanceObjects", typeof(time_batch_equip))]
        public XPCollection<time_batch_equip> time_batch_equips { get { return GetCollection<time_batch_equip>("time_batch_equips"); } }
        [Association(@"UsersFlaggedRecordsReferencesMaintenanceObjects", typeof(UsersFlaggedRecords))]
        public XPCollection<UsersFlaggedRecords> UsersFlaggedRecordsCollection { get { return GetCollection<UsersFlaggedRecords>("UsersFlaggedRecordsCollection"); } }
        [Association(@"WorkEventsReferencesMaintenanceObjects", typeof(WorkEvents))]
        public XPCollection<WorkEvents> WorkEventsCollection { get { return GetCollection<WorkEvents>("WorkEventsCollection"); } }
    }

}
