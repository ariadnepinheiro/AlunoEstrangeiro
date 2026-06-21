<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true" CodeBehind="Protocolo.aspx.cs" Inherits="Techne.Lyceum.Net.Menu.Protocolo" %>
<%@ Register src="SiteMapControl.ascx" tagname="SiteMapControl" tagprefix="uc1" %>

<asp:Content ID="conAlunos" ContentPlaceHolderID="cphFormulario" runat="server">
<div class="main">
<uc1:SiteMapControl ID="SiteMapControl1" runat="server" />
</div>
</asp:Content>