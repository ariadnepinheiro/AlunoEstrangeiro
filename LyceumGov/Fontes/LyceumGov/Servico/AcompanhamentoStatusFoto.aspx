<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="AcompanhamentoStatusFoto.aspx.cs" Inherits="Techne.Lyceum.Net.Servico.AcompanhamentoStatusFoto" %>

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
    <asp:Panel ID="pnlFiltro" runat="server" GroupingText="Filtros" Width="70%">
        <table>
            <tr>
                <td colspan="3">
                    <asp:RadioButtonList ID="rblTipoFiltro" runat="server" RepeatDirection="Horizontal"
                        AutoPostBack="true" OnSelectedIndexChanged="rblTipoFiltro_IndexChanged">
                        <asp:ListItem Text="Aluno" Value="Aluno"></asp:ListItem>
                        <asp:ListItem Text="Escola" Value="Escola"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="pnlFiltroAluno" runat="server" Visible="false">
                        <table>
                            <tr>
                                <td style="text-align: right; ">
                                    <asp:Label ID="lblAlunoTSearch" runat="server" Text="Aluno:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td colspan="4">
                                    <tweb:TSearch ID="tseAluno" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAlunoRemessaRetornoCartao"
                                        OnTextChanged="tseAluno_Changed">
                                    </tweb:TSearch>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlFiltroEscola" runat="server" Visible="false">
                        <table>
                            <tr>
                                <td style="text-align: right; width: 20%">
                                    <asp:Label ID="lblRegional" runat="server" Font-Names="Verdana" Text="Regional:*"
                                        SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td colspan="4">
                                    <tweb:TSearchBox ID="tseRegional" runat="server" Argument="regional" ArgumentColumns="50"
                                        MaxLength="20" Columns="10" Caption="" OnChanged="tseRegional_Changed" Key="id_regional"
                                        SqlSelect="SELECT id_regional, regional FROM TCE_REGIONAL" DataType="Number">
                                        <GridColumns>
                                            <tweb:TSearchBoxColumn Caption="Código" FieldName="id_regional" Width="20%" />
                                            <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="80%" />
                                        </GridColumns>
                                    </tweb:TSearchBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 20%">
                                    <asp:Label Font-Names="Verdana" ID="lblMunicipio" runat="server" Text="Município:*"
                                        SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td colspan="4">
                                    <tweb:TSearchBox ID="tseMunicipio" runat="server" SqlOrder="nome" SqlSelect=" select distinct codigo, nome, uf_sigla from VW_ZZCRO_UNIDADE_ENSINO u join municipio m on u.municipio = m.CODIGO "
                                        SqlWhere="id_regional IS NOT NULL and id_regional = #tseRegional# " GridWidth="600px"
                                        ArgumentColumns="50" OnChanged="tseMunicipio_Changed" Columns="10" MaxLength="10">
                                        <GridColumns>
                                            <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                                            <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="60%" />
                                            <tweb:TSearchBoxColumn Caption="Estado" FieldName="uf_sigla" Width="20%" />
                                        </GridColumns>
                                    </tweb:TSearchBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 20%">
                                    <asp:Label ID="lblUnidadeEnsino" Text="Unidade de Ensino:*" runat="server" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td colspan="4">
                                    <tweb:TSearchBox ID="tseUnidadeEnsino" runat="server" Caption="" Key="unidade_ens"
                                        Argument="nome_comp" ColumnName="Faculdade" SqlSelect="SELECT unidade_ens, nome_comp, setor, cgc, situacao,id_regional,municipio,ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO"
                                        GridWidth="850px" MaxLength="20" FieldName="Unidade de Ensino" SqlWhere=" id_regional = #tseRegional# AND municipio = #tseMunicipio# "
                                        ArgumentColumns="60" Columns="10" OnChanged="tseUnidadeEnsino_Changed" SqlOrder="nome_comp">
                                        <GridColumns>
                                            <tweb:TSearchBoxColumn Caption="Censo" FieldName="unidade_ens" Width="12%" />
                                            <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="30%" />
                                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="30%" />
                                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="30%" />
                                            <tweb:TSearchBoxColumn Caption="CGC" FieldName="cgc" Width="15%" />
                                            <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                                            <tweb:TSearchBoxColumn Caption="Regional" FieldName="id_regional" Visible="false" />
                                        </GridColumns>
                                    </tweb:TSearchBox>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:ImageButton ID="btnPesquisar" runat="server" ValidationGroup="Pesquisar" ImageUrl="~/Images/bot_buscar.png"
                        OnClick="btnPesquisar_Click" Visible="false" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <dxwgv:ASPxGridView ClientInstanceName="grdProcessamentoRemessa" ID="grdProcessamentoRemessa"
        runat="server" AutoGenerateColumns="False" KeyFieldName="RemessaId" Width="100%"
        EnableCallBacks="false" OnPageIndexChanged="grdProcessamentoRemessa_PageIndexChanged"
        Visible="false">
        <SettingsBehavior AllowMultiSelection="False" AllowSort="False" />
        <SettingsCookies Enabled="false" />
        <SettingsText EmptyDataRow="Não existem dados." />
        <Styles Header-HorizontalAlign="Center" Cell-HorizontalAlign="Center" Cell-VerticalAlign="Middle" />
        <Columns>
            <dxwgv:GridViewDataTextColumn Caption="WSSTATUSFOTOID" FieldName="WSSTATUSFOTOID"
                VisibleIndex="1" Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="EXECUCAOINTEGRADORID" FieldName="EXECUCAOINTEGRADORID"
                VisibleIndex="2" Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="NUMEROREGISTRO" FieldName="NUMEROREGISTRO"
                VisibleIndex="3" Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="ID Beneficiário" FieldName="IDBENEFICIARIO"
                VisibleIndex="4">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Aluno" FieldName="MATRICULA" VisibleIndex="5"
                Width="10%">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Nome do Aluno" FieldName="NOME_ALUNO" VisibleIndex="6">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Número Cartão" FieldName="NUMEROCARTAO" VisibleIndex="7">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Número Chip" FieldName="NUMEROCHIP" VisibleIndex="8">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataCheckColumn Caption="Gerou Requisição?" FieldName="GEROUREQUISICAO"
                VisibleIndex="9" Width="120px">
                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="1"
                    ValueType="System.String" ValueUnchecked="0" DisplayTextUndefined="">
                </PropertiesCheckEdit>
            </dxwgv:GridViewDataCheckColumn>
            <dxwgv:GridViewDataTextColumn Caption="Crítica Foto" FieldName="CRITICAFOTO" VisibleIndex="10">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Tipo Requisição" FieldName="TIPOREQUISICAO"
                VisibleIndex="11">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Data Requisição" FieldName="DATAREQUISICAO"
                VisibleIndex="12">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Origem Foto" FieldName="ORIGEMFOTO" VisibleIndex="13">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Data Inclusão" FieldName="DATAINCLUCAO" VisibleIndex="14">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Status Foto" FieldName="STATUSFOTO" VisibleIndex="15">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Data Status" FieldName="DATASTATUS" VisibleIndex="16">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Motivo Rejeição" FieldName="MOTIVOREJEICAOFOTO"
                VisibleIndex="17">
            </dxwgv:GridViewDataTextColumn>
        </Columns>
    </dxwgv:ASPxGridView>
</asp:Content>
