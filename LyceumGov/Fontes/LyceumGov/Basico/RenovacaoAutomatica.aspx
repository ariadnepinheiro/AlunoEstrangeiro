<%@ Page Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="RenovacaoAutomatica.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.RenovacaoAutomatica"
    Title="Renovacao Automatica" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript" language="javascript">

        function ConfirmaExclusaoRenovacao() {
            if (confirm("Deseja excluir as unidades de ensino selecionadas?")) {
                return true;
            }
            return false;
        }
    </script>

    <asp:Panel runat="server" ID="pnlFiltros" GroupingText="Informe os dados para pesquisa / inclusão" Width="850px">
        <table  Width="100%">
            <tr>
                <td>
                    <asp:RadioButtonList ID="rblTipoFiltro" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"
                        OnSelectedIndexChanged="rblTipoFiltro_SelectedIndexChanged">
                        <asp:ListItem Text="Por Regional" Value="porRegional">
                        </asp:ListItem>
                        <asp:ListItem Text="Por Unidade de Ensino" Value="porUnidade">
                        </asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="pnlRegional" runat="server" Visible="false">
                        <tweb:TSearchBox ID="tseRegional" runat="server" Argument="regional" ArgumentColumns="50" GridWidth="850px" 
                            MaxLength="20" Columns="10" AutoPostBack="True" Caption="" OnChanged="tseRegional_Changed"
                            Key="id_regional" SqlSelect="SELECT DISTINCT u.id_regional,regional FROM VW_ZZCRO_UNIDADE_ENSINO u JOIN municipio m ON u.municipio = m.CODIGO JOIN dbo.TCE_REGIONAL r ON r.ID_REGIONAL=u.ID_REGIONAL"
                            SqlOrder="regional" DataType="Number">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="id_regional" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="80%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </asp:Panel>
                    <asp:Panel ID="pnlUnidade" runat="server" Visible="false">
                        <tweb:TSearchBox ID="tseUnidade" runat="server" Caption="" Key="unidade_ens" MaxLength="20"
                            ArgumentColumns="50" Columns="10" Argument="nome_comp" SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao,nucleo,municipio,id_regional, ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO "
                            SqlWhere=" situacao = 'ESTADUAL'"  GridWidth="850px" OnChanged="tseUnidade_Changed"
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
                </td>
            </tr>
            <tr>
                <td align="right">
                    <br />
                    <asp:Button ID="btnSalvar" Visible="false" runat="server" Text="Salvar" OnClick="btnSalvar_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem" Visible="true"></asp:Label>
    <br />
    <br />
    <br />
    <asp:Panel ID="pnlRenovacaoAutomatica" runat="server" Visible="false" Width="60%">
        <table Width="100%">
            <tr>
                <td>
                    <dxwgv:ASPxGridView ID="grdRenovacaoAutomatica" runat="server" AutoGenerateColumns="False" Width="100%"
                        ClientInstanceName="grdRenovacaoAutomatica" KeyFieldName="UNIDADEENSINORENOVACAOAUTOMATICAID"
                        OnAfterPerformCallback="grdRenovacaoAutomatica_AfterPerformCallback" OnPageIndexChanged="grdRenovacaoAutomatica_PageIndexChanged">
                        <SettingsEditing Mode="Inline" />
                        <ClientSideEvents EndCallback="function(s, e) { OnEndCallBack(s); }" />
                        <Columns>
                            <dxwgv:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" ButtonType="Image" Width="5%">
                                <HeaderTemplate>
                                    <input type="checkbox" onclick="grdRenovacaoAutomatica.SelectAllRowsOnPage(this.checked);"
                                        title="Select/Unselect all rows on the page" />
                                </HeaderTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </dxwgv:GridViewCommandColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="UNIDADEENSINORENOVACAOAUTOMATICAID"
                                ReadOnly="true" VisibleIndex="1" Visible="false" >                              
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Unidade de Ensino" FieldName="ESCOLA" ReadOnly="true"
                                Visible="true" VisibleIndex="2" Width="80%">                            
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Data de Cadastro" FieldName="DATACADASTRO"
                                ReadOnly="true" Visible="true" VisibleIndex="3" Width="15%">
                            </dxwgv:GridViewDataTextColumn>
                        </Columns>
                        <Settings ShowFilterRow="False" ShowFilterRowMenu="False" />
                    </dxwgv:ASPxGridView>
                    <br />
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Button ID="btnExcluir" Visible="false" runat="server" Text="Excluir" OnClick="btnExcluir_Click"
                        OnClientClick="return ConfirmaExclusaoRenovacao();" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
