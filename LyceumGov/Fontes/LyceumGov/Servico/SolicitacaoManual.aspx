<%@ Page Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true" 
CodeBehind="SolicitacaoManual.aspx.cs" Inherits="Techne.Lyceum.Net.Servico.SolicitacaoManual" %>


<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:UpdatePanel ID="UpdatePanelFiltro" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlFiltro" runat="server" GroupingText="Filtros" Width="720px">
                <table>
                    <tr>
                        <td style="text-align: right; width: 20%">
                            <asp:Label ID="lblAlunoTSearch" runat="server" Text="Aluno: "></asp:Label>
                        </td>
                        <td colspan="4">
                            <tweb:TSearch  ID="tseAluno" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAlunoSolicitacaoManual" 
                            AutoPostBack="true" OnTextChanged="tseAluno_Changed">
                            </tweb:TSearch>
                        </td>
                    </tr>
                </table>
            </asp:Panel>                    
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanelForm" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlForm" runat="server" GroupingText="Nova Solicitação" Width="720px" Visible="false">
                <table>                    
                    <tr>
                        <td style="text-align: right; width: 20%">
                            <asp:Label ID="lblOperadora" SkinID="lblObrigatorio" runat="server" Text="Operadora:"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList Height="20px" ID="ddlOperadora" runat="server" AutoPostBack="false" 
                                DataTextField="NomeOperadora" DataValueField="OperadoraId" SkinID="a" Width="250px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 20%">
                            <asp:Label ID="lblTipoSolicitacao" SkinID="lblObrigatorio" runat="server" Text="Tipo de Solicitação:"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList Height="20px" ID="ddlTipoSolicitacao" runat="server" AutoPostBack="false" 
                                DataTextField="Descricao" DataValueField="TipoSolicitacaoId" SkinID="a" Width="250px">
                            </asp:DropDownList>
                        </td>
                    </tr>                                        
                    <tr>
                        <td style="text-align: right; width: 20%">
                            <asp:Label ID="lblMotivo" runat="server" Text="Motivo:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMotivo" runat="server" TextMode="MultiLine" MaxLength="1000" Height="50px" Width="100%"></asp:TextBox> 
                        </td>
                        <td colspan="3" align="right">
                            <asp:ImageButton ID="btnCriar" runat="server" ValidationGroup="Criar" ImageUrl="~/Images/bt_salvar.png"
                                OnClick="btnCriar_Click"/>
                        </td>
                    </tr>
                    <tr>

                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="updPnlMensagem" runat="server">
        <ContentTemplate>
            <br />
                <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="updPnl" runat="server">
        <ContentTemplate>
            <dxwgv:ASPxGridView ClientInstanceName="grdSolicitacao" ID="grdSolicitacao" 
                runat="server" AutoGenerateColumns="False" KeyFieldName="Aluno" Width="100%"
                EnableCallBacks="false" OnPageIndexChanged="grdSolicitacao_PageIndexChanged" Visible="false">
                <SettingsBehavior AllowMultiSelection="False" AllowSort="False" />
                <SettingsCookies Enabled="false" />
                <SettingsText EmptyDataRow="Não existem dados."/>                                
                <Styles Header-HorizontalAlign="Center" Cell-HorizontalAlign="Center" Cell-VerticalAlign="Middle"/>
                <Columns>           
                    <dxwgv:GridViewDataTextColumn Caption="Matrícula" FieldName="Aluno" VisibleIndex="1" Width="10%">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Operadora" FieldName="NomeOperadora" VisibleIndex="2">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Tipo de Solicitação" FieldName="TipoSolicitacao" VisibleIndex="3">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Motivo" FieldName="Observacao" VisibleIndex="4">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Situação da Solicitação" FieldName="SituacaoSolicitacao" VisibleIndex="5">
                    <DataItemTemplate>
                        <%# Eval("SituacaoSolicitacao") == null ? String.Empty : Techne.Lyceum.Net.Util.Utils.GetEnumDescription((Techne.Lyceum.RN.CartaoEstudante.Enum.SituacaoSolicitacaoEnum)Convert.ToInt32(Eval("SituacaoSolicitacao")))%>
                    </DataItemTemplate>
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Data da Solicitação" FieldName="DataSolicitacao" VisibleIndex="6">
                    </dxwgv:GridViewDataTextColumn>                    
                    <dxwgv:GridViewDataTextColumn Caption="Usuário" FieldName="Usuario" VisibleIndex="7">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Situação do Retorno" FieldName="SituacaoRetorno" VisibleIndex="8">
                        <DataItemTemplate>
                            <%# Eval("SituacaoRetorno") == null ? String.Empty : Techne.Lyceum.Net.Util.Utils.GetEnumDescription((Techne.Lyceum.RN.CartaoEstudante.Enum.SituacaoProcessamentoEnum)Convert.ToInt32(Eval("SituacaoRetorno")))%>
                        </DataItemTemplate>
                    </dxwgv:GridViewDataTextColumn>                    
                    <dxwgv:GridViewDataTextColumn Caption="Data do Último Retorno" FieldName="DataUltimoRetorno" VisibleIndex="9">
                    </dxwgv:GridViewDataTextColumn>                                        
                </Columns>
            </dxwgv:ASPxGridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanelForcarSolicitacao" runat="server" UpdateMode="Always">
        <ContentTemplate>
        <script type="text/javascript">
            function abrirPopupGeracaoForcada() {
                window.setTimeout(function() {
                    ppcGeracaoForcada.Show();
                }, 1000);
            }
        </script>
            <dxpc:ASPxPopupControl ID="ppcGeracaoForcada" ClientInstanceName="ppcGeracaoForcada"
                runat="server" Modal="true" ShowShadow="true" AllowDragging="false" AllowResize="false"
                ShowCloseButton="false" ShowFooter="false" ShowHeader="false" HeaderText=""
                ShowSizeGrip="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                EnableAnimation="false" AutoUpdatePosition="true" Width="480px" Height="80px">
                <Border BorderColor="Gainsboro" BorderStyle="Ridge" BorderWidth="4px" />
                <ContentCollection>
                    <dxpc:PopupControlContentControl ID="ppConteudo" runat="server">      
                        <asp:UpdatePanel ID="updatePanelForcarGeracao" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnForcarGeracao" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancelarGeracao" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>
                                <table id="Table1" runat="server">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblMensagemPopup" runat="server"></asp:Label>
                                        </td>
                                    </tr>                                    
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center;">
                                            <asp:Button ID="btnForcarGeracao" runat="server" Text="Sim" OnClick="btnForcarGeracao_Click"
                                                OnClientClick="ppcGeracaoForcada.Hide(); return true;" />
                                            <asp:Button ID="btnCancelarGeracao" runat="server" Text="Não"
                                                OnClientClick="ppcGeracaoForcada.Hide(); return false;" />
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>                       
                    </dxpc:PopupControlContentControl>
                </ContentCollection>
                <ContentStyle>
                    <Paddings PaddingBottom="5px" />
                </ContentStyle>
            </dxpc:ASPxPopupControl>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <asp:Panel ID="Panel1" CssClass="overlay" runat="server">
                <asp:Panel ID="Panel2" CssClass="loader" runat="server">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/updateProgress.gif" AlternateText="Carregando..."
                        Height="48" Width="48" />
                </asp:Panel>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>

