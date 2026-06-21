<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true" CodeBehind="InspecaoEscolar.aspx.cs" Inherits="Techne.Lyceum.Net.Menu.InspecaoEscolar" %>
<%@ Register src="SiteMapControl.ascx" tagname="SiteMapControl" tagprefix="uc1" %>
<%@ Register src="Popup.ascx" tagname="Popup" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphFormulario" runat="server">
<div class="main">
<uc1:SiteMapControl ID="SiteMapControl1" runat="server" />
<uc1:Popup ID="Popup1" runat="server" />
</div>
</asp:Content>

