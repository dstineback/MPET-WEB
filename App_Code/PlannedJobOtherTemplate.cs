using System.Web.UI;
using DevExpress.Web;

/// <summary>
///     Summary description for PlannedJobOtherTemplate
/// </summary>
public class PlannedJobOtherTemplate : ITemplate
{
    private ASPxGridView _gridView;

    public ASPxGridView Grid
    {
        get { return _gridView; }

        set { _gridView = value; }
    }

    public void InstantiateIn(Control container)
    {
        int index = (container as GridViewEditFormTemplateContainer).VisibleIndex;

        var pc = new ASPxPageControl();
        pc.ID = "ASPxPageControl1";
        pc.TabPages.Add("CategoryName");
        pc.TabPages.Add("Description");

        var lab1 = new ASPxLabel();
        lab1.Text = "Description:";
        pc.TabPages[0].Controls.Add(lab1);

        var catTxt = new ASPxTextBox();
        catTxt.ID = "ASPxTextBox1";
        if (!_gridView.IsNewRowEditing)
        {
            catTxt.Text = _gridView.GetRowValues(index, "OtherDescr").ToString();
        }
        pc.TabPages[0].Controls.Add(catTxt);

        var lab2 = new ASPxLabel();
        lab2.Text = "Description:";
        pc.TabPages[1].Controls.Add(lab2);

        var descTxt = new ASPxTextBox();
        descTxt.ID = "ASPxTextBox2";
        if (!_gridView.IsNewRowEditing)
        {
            descTxt.Text = _gridView.GetRowValues(index, "OtherDescr").ToString();
        }
        pc.TabPages[1].Controls.Add(descTxt);


        container.Controls.Add(pc);

        var upd = new ASPxGridViewTemplateReplacement();
        upd.ReplacementType = GridViewTemplateReplacementType.EditFormUpdateButton;
        upd.ID = "Update";
        container.Controls.Add(upd);

        var can = new ASPxGridViewTemplateReplacement();
        can.ReplacementType = GridViewTemplateReplacementType.EditFormCancelButton;
        can.ID = "Cancel";
        container.Controls.Add(can);

    }
}