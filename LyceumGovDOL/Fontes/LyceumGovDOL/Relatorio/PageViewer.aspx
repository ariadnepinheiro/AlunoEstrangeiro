<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PageViewer.aspx.cs" Inherits="webReportsHost._PageViewer" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Lyceum Relatórios</title>
</head>
<body>
    <form id="frmMain" runat="server">
    
    <div>
        &nbsp;
    &nbsp;<asp:Label ID="lblMsg" runat="server" BackColor="#FFC0C0"></asp:Label>
      &nbsp;
    </div>
    <div>
        <rsweb:ReportViewer ID="rptViewer" runat="server" Font-Names="Verdana" Font-Size="8pt" ProcessingMode="Remote" BorderStyle="None" BorderWidth="1px" ShowExportControls="true" >
        </rsweb:ReportViewer>
    </div>
    </form>
</body>
</html>
