<%@ Page Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="SuspenderAluno.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.SuspenderAluno"
    Title="Suspender Aluno" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript" language="javascript">

        function ConfirmaSuspensao() {
            if (confirm("Deseja suspender o(s) aluno(s)selecionados?")) {
                return true;
            }
            return false;
        }
    </script>

    <asp:Panel runat="server" ID="pnlFiltros" GroupingText="Informe os dados para pesquisa / inclusão"
        Width="850px">
        <table width="100%">
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label Font-Names="Verdana" ID="lblAno" runat="server" Text="Ano:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td width="35%">
                    <asp:DropDownList ID="ddlAno" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged"
                        DataTextField="ano" DataValueField="ano" Width="70px">
                    </asp:DropDownList>
                </td>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblPeriodo" runat="server" Font-Names="Verdana" Text="Período:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlPeriodo" runat="server" DataTextField="id_reduzida" DataValueField="periodo"
                        Width="100px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="Label2" runat="server" Font-Names="Verdana" Text="Tipo:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td colspan="2">
                    <asp:RadioButtonList ID="rblTipoFiltro" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"
                        OnSelectedIndexChanged="rblTipoFiltro_SelectedIndexChanged">
                        <asp:ListItem Text="Por Regional" Value="porRegional">
                        </asp:ListItem>
                        <asp:ListItem Text="Por Unidade de Ensino" Value="porUnidade">
                        </asp:ListItem>
                        <asp:ListItem Text="Todos" Value="Todos">
                        </asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td colspan="2">
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
                </td>
            </tr>
             <tr align="left">
                <td  colspan="4">
                    <asp:ImageButton ID="btnBuscar" runat="server" SkinID="BcBuscar" OnClick="btnBuscar_Click" />
                </td>
            </tr>
        </table>     
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem" Visible="true"></asp:Label>
    <div class="divEditBlock" style="width: 850px;">
        <asp:ImageButton ID="btnSuspender" runat="server" SkinID="BcSuspender" OnClick="btnSuspender_Click"
            OnClientClick="return ConfirmaSuspensao();" />
        <asp:Label runat="server" ID="lblBlocoAluno" Text="Alunos" SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsAlunos" runat="server" EnableClientScript="true" ShowMessageBox="true"
            ValidationGroup="SalvarForm" ShowSummary="false" />
    </div>
    <asp:Panel ID="pnlAlunos" runat="server" Visible="false" Width="80%">
        <br />
        <table>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" SkinID="lblObrigatorio" Text=" TOTAL DE ALUNO(S) A SER(EM) SUSPENSO(S):"></asp:Label>
                    <asp:Label ID="lblTotalAlunos" runat="server" SkinID="lblObrigatorio" Visible="true"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <table width="100%">
            <tr>
                <td>
                    <dxwgv:ASPxGridView ID="grdAlunos" runat="server" AutoGenerateColumns="False" Width="100%"
                        ClientInstanceName="grdAlunos" KeyFieldName="HISTORICOSUSPENSAOID" OnAfterPerformCallback="grdAlunos_AfterPerformCallback"
                        OnPageIndexChanged="grdAlunos_PageIndexChanged">
                        <SettingsEditing Mode="Inline" />
                        <ClientSideEvents EndCallback="function(s, e) { OnEndCallBack(s); }" />
                        <Columns>
                            <dxwgv:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" ButtonType="Image"
                                Width="5%">
                                <HeaderTemplate>
                                    <input type="checkbox" onclick="grdAlunos.SelectAllRowsOnPage(this.checked);" title="Select/Unselect all rows on the page" />
                                </HeaderTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </dxwgv:GridViewCommandColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="HISTORICOSUSPENSAOID" ReadOnly="true"
                                VisibleIndex="1" Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Regional" FieldName="REGIONAL" ReadOnly="true"
                                Visible="true" VisibleIndex="2">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Município" FieldName="MUNICIPIO" ReadOnly="true"
                                Visible="true" VisibleIndex="3">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Escola" FieldName="ESCOLA" ReadOnly="true"
                                Visible="true" VisibleIndex="4">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Turma" FieldName="TURMA" ReadOnly="true" VisibleIndex="5"
                                CellStyle-HorizontalAlign="Center">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Aluno" FieldName="ALUNO" ReadOnly="true" VisibleIndex="6"
                                CellStyle-HorizontalAlign="Center">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Nome" FieldName="NOME_COMPL" ReadOnly="true"
                                VisibleIndex="7">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Dias de Faltas" FieldName="DIASFALTASSUSPENSAO"
                                ReadOnly="true" VisibleIndex="8" CellStyle-HorizontalAlign="Center">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Data Suspensão" FieldName="DATA_EM_SUSPENSAO"
                                ReadOnly="true" VisibleIndex="9" CellStyle-HorizontalAlign="Center">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Inicio Falta" FieldName="INICIOFALTASUSPENSAO"
                                ReadOnly="true" VisibleIndex="10" CellStyle-HorizontalAlign="Center">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Fim Falta" FieldName="FIMFALTASUSPENSAO" ReadOnly="true"
                                VisibleIndex="11" CellStyle-HorizontalAlign="Center">
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
