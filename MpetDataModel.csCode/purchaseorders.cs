using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
namespace MpetDataModel.cs
{

    public partial class purchaseorders
    {
        public purchaseorders(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
