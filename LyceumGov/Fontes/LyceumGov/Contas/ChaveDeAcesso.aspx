<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ChaveDeAcesso.aspx.cs" Inherits="Techne.Lyceum.Net.Contas.ChaveDeAcesso" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel runat="server" ID="pnlDefinicao" GroupingText="Informe os dados para pesquisa."
        Width="600px">
        <table>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblRegional" runat="server" Font-Names="Verdana" Text="Regional:*"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseRegional" runat="server" Argument="regional" ArgumentColumns="50"
                        GridWidth="500px" MaxLength="2" Columns="10" AutoPostBack="True" Caption="" OnChanged="tseRegional_Changed"
                        SqlOrder="regional" Key="id_regional" SqlSelect="SELECT DISTINCT S.id_regional AS id_regional, regional FROM VW_UNIDADE_ENSINO_SITUACAO S INNER JOIN TCE_REGIONAL R ON S.ID_REGIONAL=R.ID_REGIONAL"
                        DataType="Number">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="id_regional" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="80%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label Font-Names="Verdana" ID="lblMunicipio" runat="server" Text="Município:*"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseMunicipio" runat="server" SqlOrder="nome" SqlSelect=" select distinct codigo, nome, uf_sigla from VW_ZZCRO_UNIDADE_ENSINO u join municipio m on u.municipio = m.CODIGO "
                        SqlWhere="id_regional IS NOT NULL and id_regional = #tseRegional# " GridWidth="500px"
                        ArgumentColumns="50" OnChanged="tseMunicipio_Changed" Columns="10" MaxLength="8">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="60%" />
                            <tweb:TSearchBoxColumn Caption="Estado" FieldName="uf_sigla" Width="20%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    <asp:Label Font-Names="Verdana" ID="lblUnidadeResponsavel" runat="server" Text="Unidade de Ensino:*"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUnidadeResponsavel" runat="server" Caption="" Key="unidade_ens"
                        MaxLength="8" ArgumentColumns="50" Columns="10" Argument="nome_comp" GridWidth="500px"
                        OnChanged="tseUnidadeResponsavel_Changed" AutoPostBack="True" SqlOrder="nome_comp"
                        SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao, nucleo, municipio, id_regional, ua_atual  from VW_UNIDADE_ENSINO_SITUACAO "
                        SqlWhere=" id_regional = #tseRegional# AND municipio = #tseMunicipio# ">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="12%" />
                            <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="40%" />
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="setor" Width="8%" />							
                            <tweb:TSearchBoxColumn Caption="Regional" FieldName="id_regional" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="20%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <div class="divEditBlock" style="width: 1000px;">
        <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" OnClick="btnNovo_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcSalvar" OnClick="btnSalvar_Click"
            ValidationGroup="SalvarForm" />
        <asp:Label runat="server" ID="lblBloco" Text="Chave de Acesso - Nota Fiscal" SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsPrestacao" runat="server" ShowMessageBox="true" ValidationGroup="SalvarForm"
            ShowSummary="false" />
    </div>
    <br />
    <asp:Panel ID="pnlDados" runat="server" Width="900px" Visible="true">
        <table>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="Label6" runat="server" Text="Chave de Acesso:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtCodigoAcesso" runat="server" MaxLength="44" Width="300px" SkinID="numerico" />
                    <asp:HyperLink ID="hplValidaChaveAcesso" Font-Size="12px" runat="server" Target="_blank"
                        Text="Consultar Chave de Acesso" NavigateUrl="http://www4.fazenda.rj.gov.br/consultaDFe/paginas/resultadoDfeDetalhado.faces?cid=1"></asp:HyperLink>
                </td>
            </tr>
            <tr id="tr2" runat="server">
                <td style="text-align: right;">
                    <asp:Label ID="Label7" runat="server" Text="Número do Processo:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtProcesso" runat="server" MaxLength="35" Width="200px" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="Label12" runat="server" Text="Data do Processo:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="dtProcesso" runat="server" Width="100px" Enabled="true" EnableDefaultAppearance="true"
                        ClientInstanceName="dtProcesso" CalendarProperties-ClearButtonText="Limpar" CalendarProperties-TodayButtonText="Hoje">
                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                        </CalendarProperties>
                    </dxe:ASPxDateEdit>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblValido" runat="server" SkinID="lblObrigatorio" Text="Chave válida?:* "></asp:Label>
                </td>
                <td>
                    <asp:RadioButtonList ID="rblValido" runat="server" RepeatDirection="Horizontal" Width="150px">
                        <asp:ListItem Text="Sim" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Não" Value="0"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
        </table>
        <br />
    </asp:Panel>
    <asp:ObjectDataSource ID="odsContas" TypeName="Techne.Lyceum.Net.Contas.ChaveDeAcesso"
        runat="server" SelectMethod="Listar" UpdateMethod="Update" DeleteMethod="Delete">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseUnidadeResponsavel" DefaultValue="" Name="unidade_ens"
                PropertyName="DBValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:Panel ID="pnlGrid" runat="server" Visible="false">
        <table>
            <tr>
                <td>
                    <dxwgv:ASPxGridView ID="grdNotaFiscal" runat="server" AutoGenerateColumns="False"
                        Width="1000px" DataSourceID="odsContas" ClientInstanceName="grdNotaFiscal" EnableCallBacks="false"
                        KeyFieldName="ACOMPANHAMENTONOTAID" OnAfterPerformCallback="grdNotaFiscal_AfterPerformCallback"
                        OnRowUpdating="grdNotaFiscal_RowUpdating" OnRowDeleting="grdNotaFiscal_RowDeleting">
                        <SettingsText EmptyDataRow="Não existem dados." />
                        <SettingsEditing Mode="InLine" />
                        <Columns>
                            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="0px" ShowInCustomizationForm="true">
                                <CancelButton Visible="true" Text="Cancelar">
                                    <Image Url="~/img/bt_cancelar.png" />
                                </CancelButton>
                                <EditButton Visible="True" Text="Editar">
                                    <Image Url="../img/bt_editar.png" />
                                </EditButton>
                                <DeleteButton Visible="True" Text="Remover">
                                    <Image Url="../img/bt_exclui2.png" />
                                </DeleteButton>
                                <ClearFilterButton Text="Limpar" Visible="True">
                                    <Image Url="~/img/bt_limpa.png" />
                                </ClearFilterButton>
                                <UpdateButton Visible="true" Text="Alterar">
                                    <Image Url="../img/bt_salvar.png" />
                                </UpdateButton>
                            </dxwgv:GridViewCommandColumn>
                            <dxwgv:GridViewDataTextColumn Caption="ID" FieldName="ACOMPANHAMENTONOTAID" VisibleIndex="1"
                                Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Chave de Acesso*" FieldName="CHAVEACESSO"
                                VisibleIndex="2" Width="300px">
                                <PropertiesTextEdit MaxLength="44">
                                </PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Processo*" FieldName="PROCESSO" VisibleIndex="3"
                                Width="150px">
                                <PropertiesTextEdit MaxLength="35">
                                </PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataDateColumn Caption="Data do Processo*" FieldName="DATAPROCESSO"
                                VisibleIndex="4" Width="120px">
                                <PropertiesDateEdit Width="100px" EditFormat="Date">
                                    <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                    </CalendarProperties>
                                </PropertiesDateEdit>
                            </dxwgv:GridViewDataDateColumn>
                            <dxwgv:GridViewDataCheckColumn Caption="Válida?*" FieldName="VALIDO" VisibleIndex="5"
                                Width="60px">
                                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="1"
                                    ValueType="System.String" ValueUnchecked="0" DisplayTextUndefined="">
                                </PropertiesCheckEdit>
                            </dxwgv:GridViewDataCheckColumn>
                            <dxwgv:GridViewDataDateColumn Caption="Data Cadastro" FieldName="DATACADASTRO" ReadOnly="true"
                                VisibleIndex="6" Width="100px">
                                <PropertiesDateEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesDateEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataDateColumn>
                            <dxwgv:GridViewDataDateColumn Caption="Data Alteração" FieldName="DATAALTERACAO"
                                VisibleIndex="7" Visible="false">
                            </dxwgv:GridViewDataDateColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Usuário" FieldName="NOME" ReadOnly="true"
                                VisibleIndex="6" Width="150px">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                        </Columns>
                        <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                    </dxwgv:ASPxGridView>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
