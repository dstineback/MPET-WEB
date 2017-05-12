<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Footer.ascx.cs" Inherits="UserControls.Common.Footer" %>
<style>
    .button {
        padding: 1px 1px !important;
    }
</style>
<div class="trialInfo">
    <dx:ASPxButton ID="SaveButton" 
        UseSubmitBehavior="False" runat="server" 
        CssClass="button" ToolTip="Save" >
        <HoverStyle CssClass="hover"></HoverStyle>
        <Image Url="~/Content/Images/save-26.png"></Image> 
    </dx:ASPxButton>    
    <dx:ASPxButton ID="NewButton" UseSubmitBehavior="false" AutoPostBack="True" runat="server"  CssClass="button" ToolTip="New"  >
        <HoverStyle CssClass="hover"></HoverStyle>
        <Image Url="~/Content/Images/add_file-26.png"></Image> 
    </dx:ASPxButton>
     <dx:ASPxButton ID="NewWRButton" UseSubmitBehavior="false" AutoPostBack="True" runat="server"  CssClass="button" ToolTip="Create a new Work Request"  >
        <HoverStyle CssClass="hover"></HoverStyle>
        <Image Url="~/Content/Images/add_file-26.png"></Image> 
    </dx:ASPxButton>
    <dx:ASPxButton ID="QuickPostButton" UseSubmitBehavior="false" AutoPostBack="true" runat="server" CssClass="button" ToolTip="Create a Quick Post" >
        <HoverStyle CssClass="hover"></HoverStyle>
        <Image Url="../../Content/Images/QuickPosticon.png" ></Image>
    </dx:ASPxButton>
    <dx:ASPxButton ID="NewNonStockPart" UseSubmitBehavior="false" AutoPostBack="True" runat="server"  CssClass="button" ToolTip="New Non-Stock Part"  >
        <HoverStyle CssClass="hover"></HoverStyle>
        <Image Url="~/Content/Images/add_file-26.png"></Image> 
    </dx:ASPxButton>
    <dx:ASPxButton ID="EditButton" UseSubmitBehavior="false" AutoPostBack="True" runat="server" CssClass="button" ToolTip="Edit Selected Record"  >
        <HoverStyle CssClass="hover"></HoverStyle>
        <Image Url="~/Content/Images/edit_file-26.png"></Image> 
    </dx:ASPxButton>
    <dx:ASPxButton ID="DeleteButton" UseSubmitBehavior="false" AutoPostBack="True" runat="server" CssClass="button" ToolTip="Delete Selected Record"  >
        <HoverStyle CssClass="hover"></HoverStyle>
        <Image Url="~/Content/Images/delete_file-26.png"></Image> 
    </dx:ASPxButton>
        <dx:ASPxButton ID="ViewButton" UseSubmitBehavior="false" AutoPostBack="True" runat="server" CssClass="button" ToolTip="View Selected Record"  >
        <HoverStyle CssClass="hover"></HoverStyle>
        <Image Url="~/Content/Images/view_file-26.png"></Image> 
    </dx:ASPxButton>
    <dx:ASPxButton ID="MapDisplay" UseSubmitBehavior="false" AutoPostBack="true" runat="server" CssClass="button" ToolTip="Map selected Items with GPS values">
        <HoverStyle CssClass="hover"></HoverStyle>
            <Image Url="~/Content/Images/map-icon-small.png"></Image>
    </dx:ASPxButton>
        
    <%-- WORK ORDER & REQUEST BUTTONS SPECIFIC --%>
        <dx:ASPxButton ID="PlanJob" UseSubmitBehavior="false" AutoPostBack="True" runat="server" CssClass="button" ToolTip="Plan Job"  >
        <HoverStyle CssClass="hover"></HoverStyle>
            <Image Url="~/Content/Images/calendar.png"></Image> 
    </dx:ASPxButton>
        <dx:ASPxButton ID="CopyJob" UseSubmitBehavior="false" AutoPostBack="True" runat="server" CssClass="button" ToolTip="Copy Job"  >
        <HoverStyle CssClass="hover"></HoverStyle>
            <Image Url="~/Content/Images/copy.png"></Image> 
    </dx:ASPxButton>
        <dx:ASPxButton ID="RoutineJob" UseSubmitBehavior="false" AutoPostBack="True" runat="server" CssClass="button" ToolTip="Routine Job"  >
        <HoverStyle CssClass="hover"></HoverStyle>
            <Image Url="~/Content/Images/workflow.png"></Image> 
    </dx:ASPxButton>
        <dx:ASPxButton ID="ForcePM" UseSubmitBehavior="false" AutoPostBack="True" runat="server" CssClass="button" ToolTip="Force PM Job"  >
        <HoverStyle CssClass="hover"></HoverStyle>
            <Image Url="~/Content/Images/todo_list_filled.png"></Image> 
    </dx:ASPxButton>
        <dx:ASPxButton ID="PostJob" UseSubmitBehavior="false" AutoPostBack="True" runat="server" CssClass="button" ToolTip="Post Job"  >
        <HoverStyle CssClass="hover"></HoverStyle>
            <Image Url="~/Content/Images/add_folder-26.png"></Image> 
    </dx:ASPxButton>
        <dx:ASPxButton ID="UnpostJob" UseSubmitBehavior="false" AutoPostBack="True" runat="server" CssClass="button" ToolTip="Unpost Job"  >
        <HoverStyle CssClass="hover"></HoverStyle>
            <Image Url="~/Content/Images/opened_folder_filled-25.png"></Image> 
    </dx:ASPxButton>
        <dx:ASPxButton ID="SetDefaults" UseSubmitBehavior="false" AutoPostBack="True" runat="server" CssClass="button" ToolTip="Set Defaults"  >
        <HoverStyle CssClass="hover"></HoverStyle>
            <Image Url="~/Content/Images/export-26.png"></Image> 
    </dx:ASPxButton>
        <dx:ASPxButton ID="IssueJob" UseSubmitBehavior="false" AutoPostBack="True" runat="server" CssClass="button" ToolTip="Issue Job"  >
        <HoverStyle CssClass="hover"></HoverStyle>
            <Image Url="~/Content/Images/planner-26.png"></Image> 
    </dx:ASPxButton>
    <dx:ASPxButton ID="PreviousStep" UseSubmitBehavior="false" AutoPostBack="True" runat="server" CssClass="button" ToolTip="Previous Job Step"  >
        <HoverStyle CssClass="hover"></HoverStyle>
            <Image Url="~/Content/Images/chevron_left_round.png"></Image> 
    </dx:ASPxButton>
    <dx:ASPxButton ID="NextStep" UseSubmitBehavior="false" AutoPostBack="True" runat="server" CssClass="button" ToolTip="Next Job Step"  >
        <HoverStyle CssClass="hover"></HoverStyle>
            <Image Url="~/Content/Images/right_round.png"></Image> 
    </dx:ASPxButton>
        <dx:ASPxButton ID="SetToStartDate" UseSubmitBehavior="false" AutoPostBack="True" runat="server" CssClass="button" ToolTip="Set To Start Date"  >
        <HoverStyle CssClass="hover"></HoverStyle>
            <Image Url="~/Content/Images/today-26.png"></Image> 
    </dx:ASPxButton>
        <dx:ASPxButton ID="SetToEndDate" UseSubmitBehavior="false" AutoPostBack="True" runat="server" CssClass="button" ToolTip="Set To End Date"  >
        <HoverStyle CssClass="hover"></HoverStyle>
            <Image Url="~/Content/Images/today-26.png"></Image> 
    </dx:ASPxButton>
        <dx:ASPxButton ID="BatchCrewAdd" UseSubmitBehavior="false" AutoPostBack="True" runat="server" CssClass="button" ToolTip="Batch Crew Add"  >
        <HoverStyle CssClass="hover"></HoverStyle>
            <Image Url="~/Content/Images/group.png"></Image> 
    </dx:ASPxButton>
        <dx:ASPxButton ID="BatchSupervisorAdd" UseSubmitBehavior="false" AutoPostBack="True" runat="server" CssClass="button" ToolTip="Batch Supervisor Add"  >
        <HoverStyle CssClass="hover"></HoverStyle>
            <Image Url="~/Content/Images/user.png"></Image> 
    </dx:ASPxButton>
        <dx:ASPxButton ID="BatchPartAdd" UseSubmitBehavior="false" AutoPostBack="True" runat="server" CssClass="button" ToolTip="Batch Part Add"  >
        <HoverStyle CssClass="hover"></HoverStyle>
            <Image Url="~/Content/Images/maintenance.png"></Image> 
    </dx:ASPxButton>
        <dx:ASPxButton ID="BatchEquipmentAdd" UseSubmitBehavior="false" AutoPostBack="True" runat="server" CssClass="button" ToolTip="Batch Equipment Add"  >
        <HoverStyle CssClass="hover"></HoverStyle>
            <Image Url="~/Content/Images/services.png"></Image> 
    </dx:ASPxButton>
<%--        <dx:ASPxButton ID="ShowLocation" ClientInstanceName="ShowLocation" ToolTip="Show Location" UseSubmitBehavior="false" AutoPostBack="True" runat="server" CssClass="button" >
        <HoverStyle CssClass="hover"></HoverStyle>
            <Image Url="~/Content/Images/map_marker-26.png"></Image> 
    </dx:ASPxButton>--%>
        <dx:ASPxButton ID="AddCrewByLabor" UseSubmitBehavior="false" AutoPostBack="True" runat="server" CssClass="button" ToolTip="Add By Laborclass"  >
        <HoverStyle CssClass="hover"></HoverStyle>
            <Image Url="~/Content/Images/conference_call-26.png"></Image> 
    </dx:ASPxButton>
        <dx:ASPxButton ID="AdCrewByGroup" UseSubmitBehavior="false" AutoPostBack="True" runat="server" CssClass="button" ToolTip="Add By Group"  >
        <HoverStyle CssClass="hover"></HoverStyle>
            <Image Url="~/Content/Images/group-26.png"></Image> 
    </dx:ASPxButton>
<dx:ASPxButton ID="EmailButton" UseSubmitBehavior="false" AutoPostBack="True" runat="server" CssClass="button" ToolTip="Email Receipt"  >
        <HoverStyle CssClass="hover"></HoverStyle>
            <Image Url="~/Content/Images/new_post.png"></Image> 
    </dx:ASPxButton>
<dx:ASPxButton ID="PrintButton" UseSubmitBehavior="false" AutoPostBack="True" runat="server" CssClass="button" ToolTip="Print Selected Record"  >
        <HoverStyle CssClass="hover"></HoverStyle>
        <Image Url="~/Content/Images/printer-26.png"></Image> 
    </dx:ASPxButton>
    <dx:ASPxButton ID="ExportPDF" UseSubmitBehavior="false" AutoPostBack="True" runat="server" CssClass="button" ToolTip="Export To PDF"  >
        <HoverStyle CssClass="hover"></HoverStyle>
        <Image Url="~/Content/Images/pdf-2-26.png"></Image> 
    </dx:ASPxButton>
        <dx:ASPxButton ID="ExportXLS" UseSubmitBehavior="false" AutoPostBack="True" runat="server" CssClass="button" ToolTip="Export To XLS"  >
        <HoverStyle CssClass="hover"></HoverStyle>
            <Image Url="~/Content/Images/excel_logo-26.png"></Image> 
    </dx:ASPxButton>
    <dx:ASPxButton ID="MultiSelect" UseSubmitBehavior="false" AutoPostBack="True" runat="server" CssClass="button" ToolTip="Enable/Disable Grid Multi-Select"  >
        <HoverStyle CssClass="hover"></HoverStyle>
            <Image Url="~/Content/Images/layers-26.png"></Image> 
    </dx:ASPxButton>
    
</div>
