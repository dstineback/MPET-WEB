using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
namespace MpetDataModel.cs
{

    public partial class time_batch_items
    {
        public time_batch_items(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
