<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/PublicMaster.Master" AutoEventWireup="true"
    CodeBehind="AceiteTermoCompromissoGestao.aspx.cs" Inherits="Techne.Lyceum.Net.Termos.AceiteTermoCompromissoGestao" %>

<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">

        function abrirPopup() {
            window.setTimeout(function() {
                pcPesquisaClima.Show();
            }, 1000);
        }
    </script>

    <asp:Panel ID="pnAceite" runat="server" Width="100%">
        <asp:Panel ID="pnTexto" runat="server">
            <iframe id="ifpanel" runat="server" width="100%" height="300px"></iframe>
        </asp:Panel>
        <br />
        <br />
        <asp:HiddenField ID="hdnAnoTermo" runat="server" />
        <asp:HiddenField ID="hdnIDTermo" runat="server" />
        <table style="width: 800px">
            <tr>
                <td>
                    <asp:CheckBox runat="server" ID="chkAceite" Text="Declaro que li e aceito o termo de compromisso"
                        OnCheckedChanged="chkAceite_CheckedChanged" AutoPostBack="true" />
                </td>
                <td>
                    <asp:Button ID="btnSalvarAceite" runat="server" OnClick="btnSalvarAceite_Click" Text="Acessar Sistema"
                        ValidationGroup="SalvarForm" Width="115px" Visible="false" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
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
</asp:Content>
