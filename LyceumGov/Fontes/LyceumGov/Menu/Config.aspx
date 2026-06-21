<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Config.aspx.cs" Inherits="Techne.Lyceum.Net.Menu.Config" 
        MasterPageFile="~/Modulos/LyceumMaster.Master" %>
<%@ Register src="SiteMapControl.ascx" tagname="SiteMapControl" tagprefix="uc1" %>

<asp:Content ID="conAlunos" ContentPlaceHolderID="cphFormulario" runat="server">
<div class="main">
<uc1:SiteMapControl ID="SiteMapControl1" runat="server" />
</div>
</asp:Content>

