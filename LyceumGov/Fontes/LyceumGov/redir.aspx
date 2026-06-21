<%@ Page language="c#" Codebehind="redir.aspx.cs" MasterPageFile="~/Modulos/PublicMaster.Master" AutoEventWireup="True" Inherits="Techne.Lyceum.Net._redir" %>
<asp:Content ContentPlaceHolderID="cphFormulario" ID="Content1" runat="server">
    <script type="text/javascript">
        $().ready(function() {
            {
                if (window.name != 'Seeduc') {
                    window.open('Default.aspx', 'Seeduc', 'toolbar=no,location=auto,directories=0,status=0,menubar=no,scrollbars=no,resizable=no,fullscreen=yes');
                }
                else {
                    window.location = "Default.aspx";
                    self.close();
                }

            }
        });
</script>
    <!-- Início da área de conteúdo -->
    <div class="login">
      <h3>&nbsp;<img src="Images/sel.png" alt="" width="18" height="15" align="absmiddle" />&nbsp;Bloqueador de Pop-Up:</h3>
      <p align="center" style=" margin:0px; padding:0px;"><strong><img src="Images/img_interrog.png" width="128" height="128" /><br />
        <br />
        <a href="javascript:void(window.open('Default.aspx', '', 'toolbar=no,location=auto,directories=0,status=0,menubar=no,scrollbars=no,resizable=no,fullscreen=yes')); self.close();">Caso não
      consiga abrir a página, clique aqui</a></strong></p>
    </div>
    <!-- Fim da área de conteúdo -->     
</asp:Content>