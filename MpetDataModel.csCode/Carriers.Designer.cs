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

    public partial class Carriers : XPLiteObject
    {
        int fn_carrierid;
        [Key(true)]
        public int n_carrierid
        {
            get { return fn_carrierid; }
            set { SetPropertyValue<int>("n_carrierid", ref fn_carrierid, value); }
        }
        string fcarrierid;
        [Size(10)]
        public string carrierid
        {
            get { return fcarrierid; }
            set { SetPropertyValue<string>("carrierid", ref fcarrierid, value); }
        }
        string fdescription;
        [Size(254)]
        public string description
        {
            get { return fdescription; }
            set { SetPropertyValue<string>("description", ref fdescription, value); }
        }
        string faddress1;
        [Size(40)]
        public string address1
        {
            get { return faddress1; }
            set { SetPropertyValue<string>("address1", ref faddress1, value); }
        }
        string faddress2;
        [Size(30)]
        public string address2
        {
            get { return faddress2; }
            set { SetPropertyValue<string>("address2", ref faddress2, value); }
        }
        string fcity;
        [Size(15)]
        public string city
        {
            get { return fcity; }
            set { SetPropertyValue<string>("city", ref fcity, value); }
        }
        string fcontact;
        [Size(20)]
        public string contact
        {
            get { return fcontact; }
            set { SetPropertyValue<string>("contact", ref fcontact, value); }
        }
        string fcountry;
        [Size(10)]
        public string country
        {
            get { return fcountry; }
            set { SetPropertyValue<string>("country", ref fcountry, value); }
        }
        string fextention;
        [Size(10)]
        public string extention
        {
            get { return fextention; }
            set { SetPropertyValue<string>("extention", ref fextention, value); }
        }
        string ffax;
        [Size(20)]
        public string fax
        {
            get { return ffax; }
            set { SetPropertyValue<string>("fax", ref ffax, value); }
        }
        string fnotes;
        [Size(254)]
        public string notes
        {
            get { return fnotes; }
            set { SetPropertyValue<string>("notes", ref fnotes, value); }
        }
        string fphone;
        [Size(20)]
        public string phone
        {
            get { return fphone; }
            set { SetPropertyValue<string>("phone", ref fphone, value); }
        }
        string fstate;
        [Size(2)]
        public string state
        {
            get { return fstate; }
            set { SetPropertyValue<string>("state", ref fstate, value); }
        }
        decimal ftax_pct;
        public decimal tax_pct
        {
            get { return ftax_pct; }
            set { SetPropertyValue<decimal>("tax_pct", ref ftax_pct, value); }
        }
        string fzip;
        [Size(10)]
        public string zip
        {
            get { return fzip; }
            set { SetPropertyValue<string>("zip", ref fzip, value); }
        }
        MPetUsers fCreatedBy;
        [Association(@"CarriersReferencesMPetUsers")]
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
        [Association(@"CarriersReferencesMPetUsers1")]
        public MPetUsers ModifiedBy
        {
            get { return fModifiedBy; }
            set { SetPropertyValue<MPetUsers>("ModifiedBy", ref fModifiedBy, value); }
        }
        DateTime fLast_Modified;
        public DateTime Last_Modified
        {
            get { return fLast_Modified; }
            set { SetPropertyValue<DateTime>("Last_Modified", ref fLast_Modified, value); }
        }
        string fb_IsActive;
        [Size(1)]
        public string b_IsActive
        {
            get { return fb_IsActive; }
            set { SetPropertyValue<string>("b_IsActive", ref fb_IsActive, value); }
        }
        [Association(@"partxactionsReferencesCarriers", typeof(partxactions))]
        public XPCollection<partxactions> partxactionsCollection { get { return GetCollection<partxactions>("partxactionsCollection"); } }
        [Association(@"purchaseordersReferencesCarriers", typeof(purchaseorders))]
        public XPCollection<purchaseorders> purchaseordersCollection { get { return GetCollection<purchaseorders>("purchaseordersCollection"); } }
        [Association(@"ShippingReportsReferencesCarriers", typeof(ShippingReports))]
        public XPCollection<ShippingReports> ShippingReportsCollection { get { return GetCollection<ShippingReports>("ShippingReportsCollection"); } }
        [Association(@"VendorsReferencesCarriers", typeof(Vendors))]
        public XPCollection<Vendors> VendorsCollection { get { return GetCollection<Vendors>("VendorsCollection"); } }
    }

}
