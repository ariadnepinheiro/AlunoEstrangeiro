<%@ Page CodeFile="Relatorios.aspx.cs" Language="C#" AutoEventWireup="True" EnableViewState="true" MasterPageFile="~/Views/Shared/Relatorio.Master" Inherits="Proderj.DOL.WebApp.Relatorios" %>
<asp:Content ContentPlaceHolderID="Cabecalho" runat="server">
    <% Proderj.DOL.WebApp.Models.WebFormMVCUtil.RenderPartial("Cabecalho", cabecalhoModelo); %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadPagina" runat="server">
    <link rel="stylesheet" href="CSS/" />
	<script language="javascript" src="JS/jquery"></script>
	<script language="javascript">
		$(document).ready(function () {
			$('.cabecalho a.ico_sair').on('click', function () {
				if (opener) {
					window.close();
					return false;
				}
			})
		});
	</script>
</asp:Content>

<asp:Content ID="Conteudo1" ContentPlaceHolderID="Conteudo" runat="server">
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lblMsg" runat="server" BackColor="#FFC0C0"/>
        </div>
        <div>
            <rsweb:reportviewer id="rptViewer" runat="server" width="100%" font-names="Verdana"
                font-size="8pt" processingmode="Remote" borderstyle="None" 
                borderwidth="1px" Height="900px">
            </rsweb:reportviewer>
        </div>	
    </form>
</asp:Content>

