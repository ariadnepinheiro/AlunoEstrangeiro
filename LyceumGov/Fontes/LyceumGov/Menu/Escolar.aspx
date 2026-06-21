<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Escolar.aspx.cs" Inherits="Techne.Lyceum.Net.Menu.Escolar"
    MasterPageFile="~/Modulos/LyceumMaster.Master" %>

<%@ Register Src="SiteMapControl.ascx" TagName="SiteMapControl" TagPrefix="uc1" %>
<%@ Register Src="Popup.ascx" TagName="Popup" TagPrefix="uc1" %>
<%@ Register Src="PopupA.ascx" TagName="PopupA" TagPrefix="uc2" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<asp:Content ID="conAlunos" ContentPlaceHolderID="cphFormulario" runat="server" cssclass="centralizar">
    <div class="main">

        <script type="text/javascript">

            function abrirPopup() {
                window.setTimeout(function() {
                    pcPesquisaClima.Show();
                }, 1000);
            }

            function fecharPopup() {
                window.setTimeout(function() {

                    pcPesquisaClima.Hide();
                }, 1000);
            }

        </script>

        <dxpc:ASPxPopupControl ID="pcPesquisaClima" runat="server" Modal="True" Width="800"
            Height="150" ShowPageScrollbarWhenModal="True" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton" ClientInstanceName="pcPesquisaClima"
            HeaderStyle-HorizontalAlign="Center" HeaderText="Pesquisa Clima Organizacional"
            AllowDragging="True" EnableAnimation="False" EnableViewState="False">
            <ContentCollection>
                <dxpc:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
                    <dxp:ASPxPanel ID="ASPxPanel5" runat="server" DefaultButton="btnConfirmarPopUp">
                        <PanelCollection>
                            <dxp:PanelContent ID="PanelContent5" runat="server">
                                <contenttemplate>
                                    <table id="Table4" width="100%" runat="server">
                                        <tr>
                                        <td >
                                               A Secretaria de Estado de Educação está realizando a sua primeira Pesquisa de 
                                               Clima Organizacional destinada aos servidores da Rede Estadual de Educação.
                                               <br />
                                               <br />
                                               Seu objetivo é fazer um diagnóstico da organização com base na percepção de seus 
                                               servidores. Os dados são tratados de forma estatística, por uma entidade externa 
                                               à organização, e os respondentes não serão identificados.
                                               <br />
                                               <br />
                                               Basta acessar <a href="https://www5.cepuerj.uerj.br/cepuerj.pesquisadeclima/" target="_blank">https://www5.cepuerj.uerj.br/cepuerj.pesquisadeclima/</a>
                                               com o seu CPF e seguir as telas. A pesquisa é anônima.
                                               <br />
                                               <br />
                                               Não deixe para depois. O benefício é para todos.
                                               <br />
                                               <br />
                                               Eventuais dúvidas poderão ser sanadas junto à Central de Relacionamento.<br />
                                            </td>
                                               </tr>
                                               <tr>
                                            <td align="center">
                                                <dxe:ASPxButton runat="server" Text="Fechar" ID="btnConfirmarPopUp" OnClick="btnConfirmarPopUp_OnClick"></dxe:ASPxButton>

                                            </td>
                                            
                                        </tr>
                                       
                                    </table>
                                </contenttemplate>
                            </dxp:PanelContent>
                        </PanelCollection>
                    </dxp:ASPxPanel>
                </dxpc:PopupControlContentControl>
            </ContentCollection>
            <ContentStyle>
                <Paddings PaddingBottom="5px" />
            </ContentStyle>
            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
        </dxpc:ASPxPopupControl>
        <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
        <uc1:SiteMapControl ID="SiteMapControl1" CssClass="centralizar" runat="server" />
        <uc1:Popup ID="Popup1" runat="server"  />
        <% 
            bool exibir = false;
            bool.TryParse(System.Configuration.ConfigurationManager.AppSettings["ExibeSAEB"], out exibir);

            if (exibir)
            { %>
        <uc2:PopupA ID="Popup" runat="server" />
        <% }
        %>
    </div>
</asp:Content>
