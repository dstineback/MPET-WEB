<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ObjectsList.aspx.cs" MasterPageFile="~/SiteBase.master" Inherits="Pages.Objects.ObjectsList" %>
<%@ MasterType VirtualPath="~/SiteBase.master" %>
<asp:Content runat="server" ContentPlaceHolderID="PageTitlePartPlaceHolder">Objects/Assets</asp:Content>
<asp:Content ID="ContentHolder" runat="server" ContentPlaceHolderID="ContentPlaceHolder">  
    <h1>OBJECT/ASSETS</h1> 
    
    <script type="text/javascript">

        function OnGetRowId(idValue) {
            
            Selection.Set('objectid', idValue[0].toString());
            Selection.Set('n_objectid', idValue[1].toString());
            Selection.Set('Longitude', idValue[2].toString());
            Selection.Set('Latitude', idValue[3].toString());
            Selection.Set('description', idValue[4].toString());
            Selection.Set('Area', idValue[5].toString());
            Selection.Set('AssetNumber', idValue[6].toString());
            Selection.Set('LocationID', idValue[7].toString());
        }

        function scrollPreview() {
            var previewDocBody = clientInstanceName.ObjectGrid().body;
            previewDocBody.scrollBottom = previewDocBody.scrollHeight;
        }
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />
    <dx:ASPxHiddenField ID="Selection" ViewStateMode="Enabled" ClientInstanceName="Selection" runat="server"></dx:ASPxHiddenField>
    <asp:UpdatePanel ID="UpdatePanel4" runat="server" OnUnload="UpdatePanel_Unload">
        <ContentTemplate>
            <dx:ASPxGridView
                ID="ObjectGrid"
                runat="server"
                Theme="Mulberry"
                KeyFieldName="objectid"
                Width="98%"
                KeyboardSupport="True" 
                ClientInstanceName="ObjectGrid"
                AutoPostBack="False"
                Settings-HorizontalScrollBarMode="Auto" Settings-VerticalScrollBarMode="Visible" Settings-VerticalScrollBarStyle="Standard"
                SettingsPager-Mode="ShowPager"
                SettingsBehavior-ProcessFocusedRowChangedOnServer="false"
                SettingsBehavior-AllowFocusedRow="True"
                SelectionMode="Multiple"
                DataSourceID="ObjectGridDataSource" 
                style="margin-bottom: 20px">
                <Styles Header-CssClass="gridViewHeader" Row-CssClass="gridViewRow" FocusedRow-CssClass="gridViewRowFocused"
                    RowHotTrack-CssClass="gridViewRow" FilterRow-CssClass="gridViewFilterRow">
                    <Header CssClass="gridViewHeader"></Header>
                    <Row CssClass="gridViewRow"></Row>
                    <RowHotTrack CssClass="gridViewRow"></RowHotTrack>
                    <FocusedRow CssClass="gridViewRowFocused"></FocusedRow>
                    <FilterRow CssClass="gridViewFilterRow"></FilterRow>
                </Styles>
                
                <ClientSideEvents RowClick="function(s, e) {
                        ObjectGrid.GetRowValues(e.visibleIndex, 'objectid;n_objectid;Longitude;Latitude;description;Area;Asset Number;LocationID', OnGetRowId);
                    }" />
                <Columns>
                    <dx:GridViewCommandColumn FixedStyle="Left" ShowSelectCheckbox="True" Visible="false" VisibleIndex="0" />
                    <dx:GridViewDataTextColumn FieldName="n_objectid" ReadOnly="True" Visible="false" VisibleIndex="1">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataHyperLinkColumn FixedStyle="Left"
                        FieldName="objectid"
                        SortOrder="Ascending"
                        Caption="Object"
                        Width="150px"
                        HeaderStyle-Font-Bold="True" 
                        VisibleIndex="4">
                        <CellStyle Wrap="False"></CellStyle>
                        <PropertiesHyperLinkEdit NavigateUrlFormatString="~/Pages/Objects/Objects.aspx?objectid={0}"></PropertiesHyperLinkEdit>
                    </dx:GridViewDataHyperLinkColumn>
                    <dx:GridViewDataTextColumn FieldName="description" Caption="Description" Width="300px" VisibleIndex="5">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FixedStyle="Left" FieldName="b_active" Caption="Active" Width="100px" VisibleIndex="2">
                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="b_chargeable" Caption="Chargeable" Width="100px" VisibleIndex="3">
                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Current Condition" Caption="Condition" Width="100px" VisibleIndex="6">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Reponsible Person" Caption="Resp. Person" Width="100px" VisibleIndex="7">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Storeroom" Caption="Storeroom" Width="100px" VisibleIndex="8">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Area" Caption="Area" Width="100px" VisibleIndex="9">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="TaskID" Caption="Task" Width="100px" ReadOnly="True" VisibleIndex="10">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="LocationID" Caption="Location" Width="100px" ReadOnly="True" VisibleIndex="11">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ObjectType" Caption="Obj. Type" Width="100px" ReadOnly="True" VisibleIndex="12">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ObjectClass" Caption="Obj. Class" Width="100px" ReadOnly="True" VisibleIndex="13">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Asset Number" Caption="Asset #" Width="100px" ReadOnly="True" VisibleIndex="14">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Misc. Ref." Caption="Misc. Ref." Width="100px" ReadOnly="True" VisibleIndex="15">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Manufacturer" Caption="Manufacturer" Width="100px" ReadOnly="True" VisibleIndex="16">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Model Number" Caption="Model #" Width="100px" ReadOnly="True" VisibleIndex="17">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Serial Number" Caption="Serial #" Width="100px" ReadOnly="True" VisibleIndex="18">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Build/Purchase" Settings-AllowHeaderFilter="True" Caption="Build/Purch." Width="120px" ReadOnly="True" VisibleIndex="19">
                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="In-Service" Settings-AllowHeaderFilter="True" Caption="In-Service" Width="120px" ReadOnly="True" VisibleIndex="20">
                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Warranty Exp." Settings-AllowHeaderFilter="True" Caption="Wnty. Exp." Width="120px" ReadOnly="True" VisibleIndex="21">
                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Life Cycle" Settings-AllowHeaderFilter="True" Caption="Life Cycle" Width="120px" ReadOnly="True" VisibleIndex="22">
                        <CellStyle HorizontalAlign="Center" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="RunUnit1Desc" Caption="Run Unit" Width="100px" ReadOnly="True" VisibleIndex="23">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="RunUnit1Reading" Caption="Reading" Width="100px" ReadOnly="True" VisibleIndex="24">
                        <CellStyle HorizontalAlign="Right" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="n_productlineid" ReadOnly="True" Visible="false" VisibleIndex="25">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ProductLineID" Caption="Product Line" Width="100px" ReadOnly="True" VisibleIndex="26">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="HighwayRouteID" Caption="Hwy. Route" Width="100px" ReadOnly="True" VisibleIndex="27">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="MileMarker" Caption="MM. From" Width="100px" ReadOnly="True" VisibleIndex="28">
                        <CellStyle HorizontalAlign="Right" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="MileMarkerTo" Caption="MM. To" Width="100px" ReadOnly="True" VisibleIndex="29">
                        <CellStyle HorizontalAlign="Right" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ChargeRate" Caption="Charge Rate" Width="100px" ReadOnly="True" VisibleIndex="30">
                        <CellStyle HorizontalAlign="Right" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="UnitsOfMeasureID" Caption="U.O.M." Width="100px" ReadOnly="True" VisibleIndex="31">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Quantity" Caption="Quantity" Width="100px" ReadOnly="True" VisibleIndex="32">
                        <CellStyle HorizontalAlign="Right" Wrap="False"></CellStyle>
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="MilePostDirection" Caption="Milepost Dir." Width="100px" ReadOnly="True" VisibleIndex="33">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ParentObjectID" Caption="Parent Obj." Width="100px" ReadOnly="True" VisibleIndex="34">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="costcodeid" Caption="Cost Code" Width="100px" ReadOnly="True" VisibleIndex="35">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="SupplementalCode" Caption="Suppl. Code" Width="100px" ReadOnly="True" VisibleIndex="36">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="FundSrcCodeID" Caption="Fund. Source" Width="100px" ReadOnly="True" VisibleIndex="37">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="WorkOrderCodeID" Caption="Work Order" Width="100px" ReadOnly="True" VisibleIndex="38">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="WorkOpID" Caption="Work Op" Width="100px" ReadOnly="True" VisibleIndex="39">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="OrganizationCodeID" Caption="Org. Code" Width="100px" ReadOnly="True" VisibleIndex="40">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="FundingGroupCodeID" Caption="Fund. Group" Width="100px" ReadOnly="True" VisibleIndex="41">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ControlSectionID" Caption="Ctl. Section" Width="100px" ReadOnly="True" VisibleIndex="42">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="EquipmentNumberID" Caption="Equip. Number" Width="100px" ReadOnly="True" VisibleIndex="43">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="VendorID" Caption="Vendor" Width="100px" ReadOnly="True" VisibleIndex="44">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Latitude" Caption="Latitude" Width="150px" ReadOnly="true" VisibleIndex="45" Settings-AllowSort="True" Settings-AllowGroup="True">
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Longitude" Caption="Longitude" Width="150px" ReadOnly="true" VisibleIndex="46" Settings-AllowSort="True" Settings-AllowGroup="True" >
                        <CellStyle Wrap="False"></CellStyle>
                    </dx:GridViewDataTextColumn>
                </Columns>
                <SettingsSearchPanel Visible="true" />
                <SettingsBehavior
                    EnableRowHotTrack="True" AllowGroup="true"
                    AllowFocusedRow="True" 
                    AllowClientEventsOnLoad="false" AllowSelectByRowClick="true" AllowSelectSingleRowOnly="false"
                    ColumnResizeMode="NextColumn" />
                <SettingsDataSecurity
                    AllowDelete="False"
                    AllowInsert="False" />
                <SettingsPopup HeaderFilter-Width="360" HeaderFilter-Height="360"></SettingsPopup>
                <Settings 
                    ShowFilterBar="Visible" ShowGroupPanel="true" ShowFilterRow="true" ShowFilterRowMenu="true"
                    VerticalScrollBarMode="Visible"
                    VerticalScrollBarStyle="Standard" 
                    VerticalScrollableHeight="450" />
                <SettingsFilterControl ViewMode="Visual" ShowAllDataSourceColumns="true"></SettingsFilterControl>
                <SettingsPager PageSize="20">
                    <PageSizeItemSettings Visible="true" />
                </SettingsPager>
            </dx:ASPxGridView>
            <asp:SqlDataSource ID="ObjectGridDataSource" 
                               runat="server" 
                               ConnectionString="<%$ ConnectionStrings:connection %>"  
                               SelectCommand="--Create/Set Null Date
                                DECLARE @NullDate DATETIME
                                SET @NullDate = CAST('1/1/1960 23:59:59' AS DATETIME)
                            
                            --Create Area Filering On Variable
                                DECLARE @areaFilteringOn VARCHAR(1)
                            
                            --Setup Area Filering Variable
                                IF ( ( SELECT   COUNT(dbo.UsersAreaFilter.RecordID)
                                       FROM     dbo.UsersAreaFilter WITH ( NOLOCK )
                                       WHERE    UsersAreaFilter.UserID = @UserID
                                                AND UsersAreaFilter.FilterActive = 'Y'
                                     ) <> 0 )
                                    BEGIN
                                        SET @areaFilteringOn = 'Y'
                                    END
                                ELSE
                                    BEGIN
                                        SET @areaFilteringOn = 'N'
                                    END
                            
                            ;
                                WITH    cte_MaintenanceObjects
              AS ( SELECT   tbl_MaintObj.n_objectid ,
                            tbl_MaintObj.objectid ,
                            tbl_MaintObj.description ,
                            -1 AS tasknum , -- tbl_MaintObj.tasknum ,
                            '' AS taskid , --tbl_Tasks.taskid ,
                            tbl_MaintObj.n_locationid ,
                            tbl_Locations.locationid ,
                            tbl_MaintObj.n_areaid ,
                            tbl_MaintObj.n_storeroomid ,
                            tbl_MaintObj.b_active ,
                            tbl_MaintObj.b_chargeable ,
                            tbl_MaintObj.n_ResponsiblePerson ,
                            tbl_MaintObj.n_prodlineid ,
                            tbl_MaintObj.CurrentConditionCode ,
                            tbl_ConditionCodes.CodeID ,
                            CASE tbl_RespPerson.UserID
                              WHEN 0 THEN ''
                              WHEN -1 THEN ''
                              ELSE tbl_RespPerson.FullName
                            END AS FullName ,
                            tbl_Storeroom.storeroomid ,
                            tbl_Areas.areaid ,
                            tbl_MaintObj.n_objtypeid ,
                            tbl_ObjectTypes.objtypeid ,
                            tbl_MaintObj.n_objclassid ,
                            tbl_ObjectClasses.objclassid ,
                            tbl_MaintObj.assetnumber ,
                            tbl_MaintObj.miscrefnum ,
                            tbl_MaintObj.n_mfgid ,
                            tbl_Manufactures.mfgid ,
                            tbl_MaintObj.mfgmodel ,
                            tbl_MaintObj.serialnumber ,
                            tbl_MaintObj.purchasedate ,
                            tbl_MaintObj.inservicedate ,
                            tbl_MaintObj.warrantyexpdate ,
                            tbl_MaintObj.ExpectedLifeExpirationDate ,
                            ISNULL(tbl_ObjRunUnits.DESCRIPTION, '') AS 'RunUnit1Desc' ,
                            ISNULL(tbl_ObjRunUnits.CurrentReading, 0) AS 'RunUnit1Reading' ,
                            tbl_MaintObj.n_prodlineid AS 'n_productlineid' ,
                            tbl_ProdLine.prodlineid ,
                            tbl_StateRoutes.n_StateRouteID ,
                            tbl_StateRoutes.StateRouteID ,
                            tbl_MaintObj.MilePost ,
                            tbl_MaintObj.MilePostTo ,
                            tbl_MaintObj.charge_rate ,
                            tbl_MaintObj.n_UnitsOfMeasureID ,
                            tbl_UOM.UnitsOfMeasure ,
                            tbl_MaintObj.Quantity ,
                            tbl_MilePostDirection.MilePostDirectionID ,
                            tbl_ParentObj.objectid AS 'ParentObjectID' ,
                            tbl_MaintObj.n_costcodeid ,
                            tbl_CostCodes.costcodeid ,
                            tbl_CostCodes.SupplementalCode ,
                            tbl_FundSource.n_FundSrcCodeID ,
                            tbl_FundSource.FundSrcCodeID ,
                            tbl_WorkOrders.n_WorkOrderCodeID ,
                            tbl_WorkOrders.WorkOrderCodeID ,
                            tbl_WorkOp.n_WorkOpID ,
                            tbl_WorkOp.WorkOpID ,
                            tbl_OrgCode.n_OrganizationCodeID ,
                            tbl_OrgCode.OrganizationCodeID ,
                            tbl_FundGroup.n_FundingGroupCodeID ,
                            tbl_FundGroup.FundingGroupCodeID ,
                            tbl_ControlSection.n_ControlSectionID ,
                            tbl_ControlSection.ControlSectionID ,
                            tbl_EquipNumber.n_EquipmentNumberID ,
                            tbl_EquipNumber.EquipmentNumberID ,
                            tbl_ParentObj.n_objectid AS 'n_ParentObjectID' ,
                            tbl_MaintObj.fundmtltype ,
                            tbl_MaintObj.PurchaseVendorID ,
                            tbl_Vendor.vendorid ,
                            tbl_MaintObj.n_MilePostDirection ,
                            tbl_MaintObj.GPS_X,
							tbl_MaintObj.GPS_Y,
                            tbl_MaintObj.b_oeefocus 
                   FROM     dbo.MaintenanceObjects tbl_MaintObj
                            INNER JOIN ( SELECT tblLocations.n_locationid ,
                                                tblLocations.locationid
                                         FROM   dbo.locations tblLocations
                                       ) tbl_Locations ON tbl_MaintObj.n_locationid = tbl_Locations.n_locationid
                            INNER JOIN ( SELECT tblConditionCode.ConditionCodeID ,
                                                tblConditionCode.CodeID
                                         FROM   dbo.MaintObjectsConditionCodes tblConditionCode
                                       ) tbl_ConditionCodes ON tbl_MaintObj.CurrentConditionCode = tbl_ConditionCodes.ConditionCodeID
                            INNER JOIN ( SELECT tblStorerooms.n_storeroomid ,
                                                tblStorerooms.storeroomid
                                         FROM   dbo.Storerooms tblStorerooms
                                       ) tbl_Storeroom ON tbl_MaintObj.n_storeroomid = tbl_Storeroom.n_storeroomid
                            INNER JOIN ( SELECT tblAreas.n_areaid ,
                                                tblAreas.areaid
                                         FROM   dbo.Areas tblAreas
                                         WHERE  ( ( @areaFilteringOn = 'Y'
                                                        AND EXISTS ( SELECT recordMatches.AreaFilterID
                                                                     FROM   dbo.UsersAreaFilter AS recordMatches
                                                                     WHERE  tblAreas.n_areaid = recordMatches.AreaFilterID
                                                                            AND recordMatches.UserID = @UserID
                                                                            AND recordMatches.FilterActive = 'Y' )
                                                      )
                                                      OR ( @areaFilteringOn = 'N' )
                                                    )
                                       ) tbl_Areas ON tbl_MaintObj.n_areaid = tbl_Areas.n_areaid
                            INNER JOIN ( SELECT tblObjectTypes.n_objtypeid ,
                                                tblObjectTypes.objtypeid
                                         FROM   dbo.objecttypes tblObjectTypes
                                       ) tbl_ObjectTypes ON tbl_MaintObj.n_objtypeid = tbl_ObjectTypes.n_objtypeid
                            INNER JOIN ( SELECT tblObjectClasses.n_objclassid ,
                                                tblObjectClasses.objclassid
                                         FROM   dbo.objectclasses tblObjectClasses
                                       ) tbl_ObjectClasses ON tbl_MaintObj.n_objclassid = tbl_ObjectClasses.n_objclassid
                            INNER JOIN ( SELECT tblMfg.n_mfgid ,
                                                tblMfg.mfgid
                                         FROM   dbo.Manufacturers tblMfg
                                       ) tbl_Manufactures ON tbl_MaintObj.n_mfgid = tbl_Manufactures.n_mfgid
                            INNER JOIN ( SELECT tblPRodLine.n_prodlineid ,
                                                tblPRodLine.prodlineid
                                         FROM   dbo.ProductLineTypes tblPRodLine
                                       ) tbl_ProdLine ON tbl_MaintObj.n_prodlineid = tbl_Prodline.n_prodlineid
                            INNER JOIN ( SELECT tblStateRoutes.n_StateRouteID ,
                                                tblStateRoutes.StateRouteID
                                         FROM   dbo.StateRoutes tblStateRoutes
                                       ) tbl_StateRoutes ON tbl_MaintObj.n_StateRouteID = tbl_StateRoutes.n_StateRouteID
                            INNER JOIN ( SELECT tblRespPerson.UserID ,
                                                tblRespPerson.LastName + ', ' + tblRespPerson.FirstName AS 'FullName'
                                         FROM   dbo.MPetUsers tblRespPerson
                                       ) tbl_RespPerson ON tbl_MaintObj.n_ResponsiblePerson = tbl_RespPerson.UserID
                            INNER JOIN ( SELECT tblUOM.n_UnitsOfMeasureID ,
                                                tblUOM.UnitsOfMeasure
                                         FROM   dbo.UnitsOfMeasure tblUOM
                                       ) tbl_UOM ON tbl_MaintObj.n_UnitsOfMeasureID = tbl_UOM.n_UnitsOfMeasureID
                            INNER JOIN ( SELECT tblMilePostDirection.n_MilePostDirectionID ,
                                                tblMilePostDirection.MilePostDirectionID
                                         FROM   dbo.MilePostDirections tblMilePostDirection
                                       ) tbl_MilePostDirection ON tbl_MaintObj.n_MilePostDirection = tbl_MilePostDirection.n_MilePostDirectionID
                            INNER JOIN ( SELECT tblParents.n_objectid ,
                                                tblParents.objectid
                                         FROM   dbo.MaintenanceObjects tblParents
                                       ) tbl_ParentObj ON tbl_MaintObj.n_parentobjectid = tbl_ParentObj.n_objectid
                            INNER JOIN ( SELECT tblCostCodes.n_costcodeid ,
                                                tblCostCodes.costcodeid ,
                                                tblCostCodes.SupplementalCode
                                         FROM   dbo.CostCodes tblCostCodes
                                       ) tbl_CostCodes ON tbl_MaintObj.n_costcodeid = tbl_CostCodes.n_costcodeid
                            INNER JOIN ( SELECT tblFundSource.n_FundSrcCodeID ,
                                                tblFundSource.FundSrcCodeID
                                         FROM   dbo.FundSrcCodes tblFundSource
                                       ) tbl_FundSource ON tbl_MaintObj.n_FundSrcCodeID = tbl_FundSource.n_FundSrcCodeID
                            INNER JOIN ( SELECT tblWorkOrders.n_WorkOrderCodeID ,
                                                tblWorkOrders.WorkOrderCodeID
                                         FROM   dbo.WorkOrderCodes tblWorkOrders
                                       ) tbl_WorkOrders ON tbl_MaintObj.n_WorkOrderCodeID = tbl_WorkOrders.n_WorkOrderCodeID
                            INNER JOIN ( SELECT tblWOrkOp.n_WorkOpID ,
                                                tblWOrkOp.WorkOpID
                                         FROM   dbo.WorkOperations tblWOrkOp
                                       ) tbl_WorkOp ON tbl_MaintObj.n_WorkOpID = tbl_WorkOp.n_WorkOpID
                            INNER JOIN ( SELECT tblOrgCode.n_OrganizationCodeID ,
                                                tblOrgCode.OrganizationCodeID
                                         FROM   dbo.OrganizationCodes tblOrgCode
                                       ) tbl_OrgCode ON tbl_MaintObj.n_OrganizationCodeID = tbl_OrgCode.n_OrganizationCodeID
                            INNER JOIN ( SELECT tblFundGroup.n_FundingGroupCodeID ,
                                                tblFundGroup.FundingGroupCodeID
                                         FROM   dbo.FundingGroupCodes tblFundGroup
                                       ) tbl_FundGroup ON tbl_MaintObj.n_FundingGroupCodeID = tbl_FundGroup.n_FundingGroupCodeID
                            INNER JOIN ( SELECT tblEquipNumber.n_EquipmentNumberID ,
                                                tblEquipNumber.EquipmentNumberID
                                         FROM   dbo.EquipmentNumber tblEquipNumber
                                       ) tbl_EquipNumber ON tbl_MaintObj.n_EquipmentNumberID = tbl_EquipNumber.n_EquipmentNumberID
                            INNER JOIN ( SELECT tblControlSection.n_ControlSectionID ,
                                                tblControlSection.ControlSectionID
                                         FROM   dbo.ControlSections tblControlSection
                                       ) tbl_ControlSection ON tbl_MaintObj.n_ControlSectionID = tbl_ControlSection.n_ControlSectionID
                            INNER JOIN ( SELECT tblVendor.n_vendorid ,
                                                tblVendor.vendorid
                                         FROM   dbo.Vendors tblVendor
                                       ) tbl_Vendor ON tbl_MaintObj.PurchaseVendorID = tbl_Vendor.n_vendorid
                            OUTER APPLY ( SELECT TOP 1
                                                    tblObjRunUnits.n_objectID ,
                                                    tblObjRunUnits.Description ,
                                                    tblObjRunUnits.CurrentReading
                                          FROM      dbo.ObjectRunUnits tblObjRunUnits
                                          WHERE     tblObjRunUnits.n_objectID = tbl_MaintObj.n_objectid
                                        ) AS tbl_ObjRunUnits
                            INNER JOIN ( SELECT dbo.MPetUsers.userid ,
                                                CASE dbo.MpetUsers.UserID
                                                  WHEN -1 THEN 'N/A'
                                                  WHEN 0 THEN 'IMPORTED'
                                                  ELSE dbo.MPetUsers.Username
                                                END AS Username
                                         FROM   dbo.mpetusers
                                       ) tbl_Creator ON tbl_MaintObj.CreatedBy = tbl_Creator.UserID
                            INNER JOIN ( SELECT dbo.MPetUsers.userid ,
                                                CASE dbo.MpetUsers.UserID
                                                  WHEN -1 THEN 'N/A'
                                                  WHEN 0 THEN 'IMPORTED'
                                                  ELSE dbo.MPetUsers.Username
                                                END AS Username
                                         FROM   dbo.MPetUsers
                                       ) tbl_Modifier ON tbl_MaintObj.ModifiedBy = tbl_Modifier.UserID
                   WHERE    tbl_MaintObj.n_objectid > 0
                 )
        SELECT  cte_MaintenanceObjects.n_objectid AS 'n_objectid' ,
                cte_MaintenanceObjects.objectid AS 'objectid' ,
                cte_MaintenanceObjects.description AS 'description' ,
                cte_MaintenanceObjects.b_active AS 'b_active' ,
                cte_MaintenanceObjects.b_chargeable AS 'b_chargeable' ,
                cte_MaintenanceObjects.CodeID AS 'Current Condition' ,
                cte_MaintenanceObjects.FullName AS 'Reponsible Person' ,
                cte_MaintenanceObjects.storeroomid AS 'Storeroom' ,
                cte_MaintenanceObjects.areaid AS 'Area' ,
                cte_MaintenanceObjects.TaskID AS 'TaskID' ,
                cte_MaintenanceObjects.LocationID AS 'LocationID' ,
                cte_MaintenanceObjects.objtypeid AS 'ObjectType' ,
                cte_MaintenanceObjects.objclassid AS 'ObjectClass' ,
                cte_MaintenanceObjects.AssetNumber AS 'Asset Number' ,
                cte_MaintenanceObjects.MiscRefNum AS 'Misc. Ref.' ,
                cte_MaintenanceObjects.MfgID 'Manufacturer' ,
                cte_MaintenanceObjects.mfgmodel 'Model Number' ,
                cte_MaintenanceObjects.serialnumber 'Serial Number' ,
                CASE cte_MaintenanceObjects.purchasedate
                  WHEN @NullDate THEN NULL
                  ELSE CAST(CAST(cte_MaintenanceObjects.purchasedate AS VARCHAR(11)) AS DATETIME)
                END AS 'Build/Purchase' ,
                CASE cte_MaintenanceObjects.InServiceDate
                  WHEN @NullDate THEN NULL
                  ELSE CAST(CAST(cte_MaintenanceObjects.InServiceDate AS VARCHAR(11)) AS DATETIME)
                END AS 'In-Service' ,
                CASE cte_MaintenanceObjects.warrantyexpdate
                  WHEN @NullDate THEN NULL
                  ELSE CAST(CAST(cte_MaintenanceObjects.warrantyexpdate AS VARCHAR(11)) AS DATETIME)
                END AS 'Warranty Exp.' ,
                CASE cte_MaintenanceObjects.ExpectedLifeExpirationDate
                  WHEN @NullDate THEN NULL
                  ELSE CAST(CAST(cte_MaintenanceObjects.ExpectedLifeExpirationDate AS VARCHAR(11)) AS DATETIME)
                END AS 'Life Cycle' ,
                cte_MaintenanceObjects.RunUnit1Desc AS 'RunUnit1Desc' ,
                cte_MaintenanceObjects.RunUnit1Reading AS 'RunUnit1Reading' ,
                cte_MaintenanceObjects.n_productlineid AS 'n_productlineid' ,
                cte_MaintenanceObjects.prodlineid AS 'ProductLineID' ,
                cte_MaintenanceObjects.StateRouteID AS 'HighwayRouteID' ,
                cte_MaintenanceObjects.MilePost AS 'MileMarker' ,
                cte_MaintenanceObjects.MilePostTo AS 'MileMarkerTo' ,
                cte_MaintenanceObjects.charge_rate AS 'ChargeRate' ,
                cte_MaintenanceObjects.UnitsOfMeasure AS 'UnitsOfMeasureID' ,
                cte_MaintenanceObjects.Quantity AS 'Quantity' ,
                cte_MaintenanceObjects.MilePostDirectionID AS 'MilePostDirection' ,
                cte_MaintenanceObjects.ParentObjectID AS 'ParentObjectID' ,
                cte_MaintenanceObjects.costcodeid AS 'costcodeid' ,
                cte_MaintenanceObjects.SupplementalCode AS 'SupplementalCode' ,
                cte_MaintenanceObjects.FundSrcCodeID AS 'FundSrcCodeID' ,
                cte_MaintenanceObjects.WorkOrderCodeID AS 'WorkOrderCodeID' ,
                cte_MaintenanceObjects.WorkOpID AS 'WorkOpID' ,
                cte_MaintenanceObjects.OrganizationCodeID AS 'OrganizationCodeID' ,
                cte_MaintenanceObjects.FundingGroupCodeID AS 'FundingGroupCodeID' ,
                cte_MaintenanceObjects.ControlSectionID AS 'ControlSectionID' ,
                cte_MaintenanceObjects.EquipmentNumberID AS 'EquipmentNumberID' ,
                cte_MaintenanceObjects.VendorID 'VendorID',
                cte_MaintenanceObjects.GPS_X AS 'Latitude',
				cte_MaintenanceObjects.GPS_Y AS 'Longitude'  
        FROM    cte_MaintenanceObjects">
                                                        <SelectParameters>
                                                            <asp:SessionParameter DefaultValue="-1" Name="UserID" SessionField="UserID" />
                                                        </SelectParameters>
                                                    </asp:SqlDataSource>
                                                                </ContentTemplate>

    </asp:UpdatePanel>
    <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="ObjectGrid"></dx:ASPxGridViewExporter>
</asp:Content>
