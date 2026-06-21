<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="AndamentoGLP.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.AndamentoGLP" %>

<asp:Content ID="conAndamento" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel runat="server" ID="pnlFiltro" GroupingText="Informe os dados para pesquisa."
        Width="720px">
        <table>
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="lblUnidadeEnsino" runat="server" Text="Unidade de Ensino:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td colspan="3">
                    <tweb:TSearchBox ID="tseUnidade_Ensino" runat="server" Key="unidade_ens" Argument="nome_comp"
                        MaxLength="20" SqlSelect="SELECT unidade_ens, nome_comp, setor, cgc, situacao, ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO"
                        GridWidth="850px" SqlOrder="nome_comp" AutoPostBack="true" OnChanged="tseUnidade_Ensino_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="12%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome_comp" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="lblAnoTSearch" runat="server" Text="Ano:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlAno" runat="server" CssClass="ReadOnlyField" AutoPostBack="True"
                        DataTextField="ano" DataValueField="ano" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvAnoPesquisa" runat="server" ControlToValidate="ddlAno"
                        ErrorMessage="Ano: Preenchimento obrigatório." InitialValue="" ValidationGroup="ConfirmarForm"><img 
                                                alt="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                </td>
                <td style="text-align: right;">
                    <asp:Label ID="lblMes" runat="server" Text="Mês:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlMes" runat="server" CssClass="ReadOnlyField" AutoPostBack="True"
                        DataTextField="ano" DataValueField="ano" OnSelectedIndexChanged="ddlMes_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlMes"
                        ErrorMessage="Mês: Preenchimento obrigatório." InitialValue="" ValidationGroup="ConfirmarForm"><img 
                                                alt="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <table>
        <tr>
            <td>
                <asp:ObjectDataSource ID="odsAndamento" TypeName="Techne.Lyceum.Net.Basico.AndamentoGLP"
                    runat="server" SelectMethod="Listar">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="tseUnidade_Ensino" Name="unidade" PropertyName="DBValue" />
                        <asp:ControlParameter ControlID="ddlAno" Name="ano" PropertyName="SelectedValue" />
                        <asp:ControlParameter ControlID="ddlMes" Name="mes" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <dxwgv:ASPxGridView ID="grdAndamento" runat="server" ClientInstanceName="grdAndamento"
                    Visible="false" KeyFieldName="mes" OnAfterPerformCallback="grdAndamento_AfterPerformCallback"
                    DataSourceID="odsAndamento" OnHtmlDataCellPrepared="grdAndamento_HtmlDataCellPrepared"
                    OnHtmlRowPrepared="grdAndamento_HtmlRowPrepared" OnCustomColumnDisplayText="grdAndamento_CustomColumnDisplayText">
                    <SettingsText EmptyDataRow="Não existem dados." />
                    <Columns>
                        <dxwgv:GridViewDataTextColumn Caption="ID/Vínculo" FieldName="IDVINCULO" VisibleIndex="1">
                            <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                            </CellStyle>
                            <HeaderStyle HorizontalAlign="Center" />
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Matrícula" FieldName="matricula" VisibleIndex="1"
                            Width="10">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Nome" FieldName="nome" VisibleIndex="1" Width="150">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Disciplina" FieldName="disciplina" VisibleIndex="2"
                            Width="100">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Segmento de Atuação" FieldName="segmento"
                            VisibleIndex="3" Width="100">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="CH Solicitada" FieldName="quantidade" VisibleIndex="4"
                            Width="40">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataDateColumn FieldName="data" VisibleIndex="6" Caption="Data da Última Situação">
                        </dxwgv:GridViewDataDateColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Prazo" FieldName="prazo" VisibleIndex="7"
                            Width="40">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Solicitada" FieldName="GLP_SOLICITADA" VisibleIndex="8"
                            Width="40">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Alocada" FieldName="GLP_USADA" VisibleIndex="9"
                            Width="40">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Cancelada" FieldName="GLP_CANCELADA" VisibleIndex="10"
                            Width="40">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Situação do Pedido" FieldName="status" VisibleIndex="11"
                            Width="90">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataComboBoxColumn Caption="Motivo Reprovação" FieldName="motivo"
                            VisibleIndex="12" Width="400px">
                            <PropertiesComboBox ValueType="System.String" DataSourceID="tdsMotivo" TextField="DESCR"
                                ValueField="ITEM" Width="400px">
                            </PropertiesComboBox>
                        </dxwgv:GridViewDataComboBoxColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Solicitante" FieldName="usuario" VisibleIndex="13"
                            Width="100" Visible="false">
                        </dxwgv:GridViewDataTextColumn>
                    </Columns>
                    <Settings ShowFilterRow="false" />
                    <SettingsPager Mode="ShowAllRecords" />
                </dxwgv:ASPxGridView>
            </td>
        </tr>
    </table>
    <techne:TTableDataSource ID="tdsMotivo" runat="server" DataTableClassName="Techne.Lyceum.CR.Itemtabela"
        SqlColumns="ITEM, DESCR" SqlWhere="TAB = 'MotivoReprovarGLP'" SqlOrder="DESCR">
    </techne:TTableDataSource>
</asp:Content>
