<%@ Page Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ProrrogacaoPrazoConfirmacao.aspx.cs" Inherits="Techne.Lyceum.Net.Matricula.ProrrogacaoPrazoConfirmacao"
    Title="Prorrogação Prazo de Confirmação de Matrícula" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript" language="javascript">
        $(document).ready(function() {
            $("#<%= this.txtDias.ClientID %>").numeric();
        });

        function Bloqueio() {
            var divBloqueio = document.getElementById("dvbloqueioTela");
            divBloqueio.className = "Bloqueado";
        } 
    </script>

    <div id="dvbloqueioTela" class="Desbloqueado">
    </div>
    <asp:Panel runat="server" ID="pnlFiltros" GroupingText="Informe os dados para pesquisa"
        Width="850px">
        <table width="100%">
            <tr>
                <td>
                    <asp:RadioButtonList ID="rblTipoFiltro" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"
                        OnSelectedIndexChanged="rblTipoFiltro_SelectedIndexChanged">
                        <asp:ListItem Text="Por Unidade de Ensino" Value="porUnidade">
                        </asp:ListItem>
                        <asp:ListItem Text="Por Município" Value="porMunicipio">
                        </asp:ListItem>
                        <asp:ListItem Text="Por Regional" Value="porRegional">
                        </asp:ListItem>
                        <asp:ListItem Text="Todos" Value="todos">
                        </asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="pnlUnidade" runat="server" Visible="false">
                        <tweb:TSearchBox ID="tseUnidade" runat="server" Caption="" Key="unidade_ens" MaxLength="20"
                            ArgumentColumns="50" Columns="10" Argument="nome_comp" SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao,nucleo,municipio,id_regional, ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO "
                            SqlWhere=" situacao = 'ESTADUAL'" GridWidth="850px" OnChanged="tseUnidade_Changed"
                            SqlOrder="nome_comp">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="12%" />
                                <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="10%" />
                                <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </asp:Panel>
                    <asp:Panel ID="pnlMunicipio" runat="server" Visible="false">
                        <tweb:TSearchBox ID="tseMunicipio" runat="server" SqlOrder="nome" SqlSelect=" select distinct codigo, nome, uf_sigla from VW_ZZCRO_UNIDADE_ENSINO u join municipio m on u.municipio = m.CODIGO "
                            GridWidth="600px" ArgumentColumns="50" OnChanged="tseMunicipio_Changed" Columns="10"
                            MaxLength="10">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="60%" />
                                <tweb:TSearchBoxColumn Caption="Estado" FieldName="uf_sigla" Width="20%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </asp:Panel>
                    <asp:Panel ID="pnlRegional" runat="server" Visible="false">
                        <tweb:TSearchBox ID="tseRegional" runat="server" Argument="regional" ArgumentColumns="50"
                            GridWidth="850px" MaxLength="20" Columns="10" AutoPostBack="True" Caption=""
                            OnChanged="tseRegional_Changed" Key="id_regional" SqlSelect="SELECT DISTINCT u.id_regional,regional FROM VW_ZZCRO_UNIDADE_ENSINO u JOIN municipio m ON u.municipio = m.CODIGO JOIN dbo.TCE_REGIONAL r ON r.ID_REGIONAL=u.ID_REGIONAL"
                            SqlOrder="regional" DataType="Number">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="id_regional" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="80%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click"
                        OnClientClick="Bloqueio();" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem" Visible="true"></asp:Label>
    <br />
    <asp:Panel ID="pnlProrrogacaoPrazoConfirmacao" runat="server" Visible="false" Width="60%">
        <table>
            <tr>
                <td align="right">
                    <asp:Label ID="lblDias" SkinID="lblObrigatorio" runat="server" Text="Dias úteis de prorrogação: *"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDias" runat="server" MaxLength="15" Width="60px" SkinID="numeroDocumento" />
                </td>
                <td>
                    &nbsp;
                </td>
                <td align="right">
                    <asp:Button ID="btnSalvar" Visible="false" runat="server" Text="Salvar" OnClick="btnSalvar_Click"
                        OnClientClick="Bloqueio();" />
                </td>
            </tr>
        </table>
        </br>
        <table width="100%">
            <tr>
                <td>
                    <dxwgv:ASPxGridView ID="grdProrrogacaoPrazoConfirmacao" runat="server" AutoGenerateColumns="False"
                        ClientInstanceName="grdProrrogacaoPrazoConfirmacao" KeyFieldName="PRAZORESPOSTA"
                        OnAfterPerformCallback="grdProrrogacaoPrazoConfirmacao_AfterPerformCallback"
                        OnPageIndexChanged="grdProrrogacaoPrazoConfirmacao_PageIndexChanged">
                        <SettingsEditing Mode="Inline" />
                        <ClientSideEvents EndCallback="function(s, e) { OnEndCallBack(s); }" />
                        <Columns>
                            <dxwgv:GridViewDataTextColumn Caption="Prazo de Resposta" FieldName="PRAZORESPOSTA"
                                ReadOnly="true" VisibleIndex="1">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Quantidade Convocados" FieldName="CONVOCADOS"
                                ReadOnly="true" Visible="true" VisibleIndex="2">
                            </dxwgv:GridViewDataTextColumn>
                        </Columns>
                        <Settings ShowFilterRow="False" ShowFilterRowMenu="False" />
                    </dxwgv:ASPxGridView>
                    <br />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
