<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modulos/LyceumMaster.Master"
    CodeBehind="PainelFinanceiro.aspx.cs" Inherits="Techne.Lyceum.Net.PrestacaoContas.PainelFinanceiro" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script src="../Scripts/jquery-ui.js" type="text/javascript"></script>

    <script src="../Scripts/jquery.maskedinput-1.2.2.min.js" type="text/javascript"></script>

    <script type="text/javascript">

        $(document).ready(function() {
            $("#ctl00_cphFormulario_txtData").mask("99/99/9999", { placeholder: '' });
        });
    </script>

    <asp:Panel ID="pnBusca" runat="server" GroupingText="Informações do Painel Financeiro"
        Width="617px">
        <table style="width: 600px">
            <tr>
                <td style="text-align: right; width: 20%">
                    <asp:Label ID="lblData" runat="server" Text="Data:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtData" runat="server" Font-Names="Verdana" Enabled="false" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 20%">
                    <asp:Label ID="lblUnidade" runat="server" Text="Unidade Ensino*:" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUnidadeResponsavel" runat="server" Caption="" Key="unidade_ens"
                        EnableViewState="true" Argument="nome_comp" SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao,municipio, id_regional from VW_UNIDADE_ENSINO_SITUACAO "
                        ArgumentColumns="75" Columns="10" OnChanged="tseUnidadeResponsavel_Changed" GridWidth="850px"
                        SqlOrder="nome_comp">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Censo" FieldName="unidade_ens" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="setor" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="CGC" FieldName="cgc" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="Municipio" FieldName="municipio" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Regional" FieldName="id_regional" Width="15%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 20%">
                    <asp:Label Font-Names="Verdana" ID="Label2" SkinID="lblObrigatorio" runat="server"
                        Text="Período Referência:*"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tsePeriodoReferencia" runat="server" Argument="DESCRICAO" ArgumentColumns="50"
                        MaxLength="20" Columns="10" AutoPostBack="True" Caption="" SqlOrder="ANO, MESINICIAL DESC"
                        Key="PERIODOREFERENCIAID" SqlSelect=" SELECT ANO, MESINICIAL, MESFINAL, REFERENCIA FROM PrestacaoContas.VW_PERIODOREFERENCIA "
                        DataType="Number" OnChanged="tsePeriodoReferencia_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="PERIODOREFERENCIAID" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Período" FieldName="DESCRICAO" Width="80%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnBuscar" runat="server" Font-Names="Verdana" OnClick="btnBuscar_Click"
                        Text="Buscar"></asp:Button>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Label ID="lblMensagem" runat="server"></asp:Label>
    <asp:HiddenField ID="hdnSaldoInicialId" runat="server" />
    <asp:HiddenField ID="hdnDataInicio" runat="server" />
    <asp:HiddenField ID="hdnDataFim" runat="server" />
    <asp:HiddenField ID="hdnSaldoFinalMerenda" runat="server" />
    <br />
    <div class="divEditBlock" style="width: 950px;">
        <asp:Label runat="server" ID="lblBlocoAluno" Text="Painel Despesas Aprovadas" SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsPainel" runat="server" EnableClientScript="true" ShowMessageBox="true"
            ValidationGroup="SalvarForm" ShowSummary="false" />
    </div>
    <br />
    <asp:ObjectDataSource ID="odsMerenda" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.PainelFinanceiro"
        SelectMethod="ListaMerenda">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseUnidadeResponsavel" DefaultValue="" Name="censo"
                PropertyName="DBValue" />
            <asp:ControlParameter ControlID="hdnDataInicio" DefaultValue="" Name="inicio" PropertyName="Value" />
            <asp:ControlParameter ControlID="hdnDataFim" DefaultValue="" Name="fim" PropertyName="Value" />
            <asp:ControlParameter ControlID="tsePeriodoReferencia" DefaultValue="" Name="referenciaid" PropertyName="DBValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsManutencao" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.PainelFinanceiro"
        SelectMethod="ListaManutencao">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseUnidadeResponsavel" DefaultValue="" Name="censo"
                PropertyName="DBValue" />
            <asp:ControlParameter ControlID="hdnDataInicio" DefaultValue="" Name="inicio" PropertyName="Value" />
            <asp:ControlParameter ControlID="hdnDataFim" DefaultValue="" Name="fim" PropertyName="Value" />
            <asp:ControlParameter ControlID="tsePeriodoReferencia" DefaultValue="" Name="referenciaid" PropertyName="DBValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsOutrosProjetos" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.PainelFinanceiro"
        SelectMethod="ListaOutrosProjetos">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseUnidadeResponsavel" DefaultValue="" Name="censo"
                PropertyName="DBValue" />
            <asp:ControlParameter ControlID="hdnDataInicio" DefaultValue="" Name="inicio" PropertyName="Value" />
            <asp:ControlParameter ControlID="hdnDataFim" DefaultValue="" Name="fim" PropertyName="Value" />
            <asp:ControlParameter ControlID="tsePeriodoReferencia" DefaultValue="" Name="referenciaid" PropertyName="DBValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <table id="tblPainel" runat="server" visible="false">
        <tr>
            <td>
                <b>Merenda</b>
            </td>
            <td>
                <b>Manutenção</b>
            </td>
            <td>
                <b>Outros Projetos</b>
            </td>
        </tr>
        <tr>
            <td>
                <b>Saldo Anterior: </b>
                <asp:Label ID="lblSaldoAntMerenda" runat="server"> </asp:Label>
            </td>
            <td>
                <b>Saldo Anterior: </b>
                <asp:Label ID="lblSaldoAntManutencao" runat="server"> </asp:Label>
            </td>
            <td>
                <b>Saldo Anterior: </b>
                <asp:Label ID="lblSaldoAntOutroProjeto" runat="server"> </asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <dxwgv:ASPxGridView ID="grdMerenda" runat="server" DataSourceID="odsMerenda" KeyFieldName="FINALIDADEID"
                    AutoGenerateColumns="false" ClientInstanceName="grdMerenda" Width="80%">
                    <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                    <SettingsEditing Mode="Inline" />
                    <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                    <SettingsBehavior ConfirmDelete="true" />
                    <Columns>
                        <dxwgv:GridViewDataDateColumn VisibleIndex="1" Caption="Data" Name="DATAITEM" FieldName="DATAITEM"
                            Width="200px" Visible="true" ReadOnly="true">
                            <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" Width="150px">
                                <ValidationSettings>
                                    <RequiredField IsRequired="true" ErrorText="Campo Obrigatório." />
                                </ValidationSettings>
                            </PropertiesDateEdit>
                            <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                            </CellStyle>
                        </dxwgv:GridViewDataDateColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Item" Name="ITEM" VisibleIndex="2" FieldName="ITEM"
                            Width="400px">
                            <PropertiesTextEdit MaxLength="100">
                            </PropertiesTextEdit>
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Nota Fiscal" Name="NUMERONOTAFISCAL" VisibleIndex="3"
                            FieldName="NUMERONOTAFISCAL" Width="400px">
                            <PropertiesTextEdit MaxLength="100">
                            </PropertiesTextEdit>
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Valor" Name="VALORITEM" VisibleIndex="4" FieldName="VALORITEM"
                            Width="200px">
                            <PropertiesTextEdit MaxLength="100">
                            </PropertiesTextEdit>
                        </dxwgv:GridViewDataTextColumn>
                    </Columns>
                </dxwgv:ASPxGridView>
            </td>
            <td>
                <dxwgv:ASPxGridView ID="grdManutencao" runat="server" DataSourceID="odsManutencao"
                    KeyFieldName="FINALIDADEID" AutoGenerateColumns="false" ClientInstanceName="grdManutencao"
                    Width="80%">
                    <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                    <SettingsEditing Mode="Inline" />
                    <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                    <SettingsBehavior ConfirmDelete="true" />
                    <Columns>
                        <dxwgv:GridViewDataDateColumn VisibleIndex="1" Caption="Data" Name="DATAITEM" FieldName="DATAITEM"
                            Width="200px" Visible="true" ReadOnly="true">
                            <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" Width="150px">
                                <ValidationSettings>
                                    <RequiredField IsRequired="true" ErrorText="Campo Obrigatório." />
                                </ValidationSettings>
                            </PropertiesDateEdit>
                            <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                            </CellStyle>
                        </dxwgv:GridViewDataDateColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Item" Name="ITEM" VisibleIndex="2" FieldName="ITEM"
                            Width="400px">
                            <PropertiesTextEdit MaxLength="100">
                            </PropertiesTextEdit>
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Nota Fiscal" Name="NUMERONOTAFISCAL" VisibleIndex="3"
                            FieldName="NUMERONOTAFISCAL" Width="400px">
                            <PropertiesTextEdit MaxLength="100">
                            </PropertiesTextEdit>
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Valor" Name="VALORITEM" VisibleIndex="4" FieldName="VALORITEM"
                            Width="200px">
                            <PropertiesTextEdit MaxLength="100">
                            </PropertiesTextEdit>
                        </dxwgv:GridViewDataTextColumn>
                    </Columns>
                </dxwgv:ASPxGridView>
            </td>
            <td>
                <dxwgv:ASPxGridView ID="grdOutrosProjetos" runat="server" DataSourceID="odsOutrosProjetos"
                    KeyFieldName="FINALIDADEID" AutoGenerateColumns="false" ClientInstanceName="grdOutrosProjetos"
                    Width="80%">
                    <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                    <SettingsEditing Mode="Inline" />
                    <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                    <SettingsBehavior ConfirmDelete="true" />
                    <Columns>
                        <dxwgv:GridViewDataDateColumn VisibleIndex="1" Caption="Data" Name="DATAITEM" FieldName="DATAITEM"
                            Width="200px" Visible="true" ReadOnly="true">
                            <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" Width="150px">
                                <ValidationSettings>
                                    <RequiredField IsRequired="true" ErrorText="Campo Obrigatório." />
                                </ValidationSettings>
                            </PropertiesDateEdit>
                            <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                            </CellStyle>
                        </dxwgv:GridViewDataDateColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Item" Name="ITEM" VisibleIndex="2" FieldName="ITEM"
                            Width="400px">
                            <PropertiesTextEdit MaxLength="100">
                            </PropertiesTextEdit>
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Nota Fiscal" Name="NUMERONOTAFISCAL" VisibleIndex="3"
                            FieldName="NUMERONOTAFISCAL" Width="400px">
                            <PropertiesTextEdit MaxLength="100">
                            </PropertiesTextEdit>
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Valor" Name="VALORITEM" VisibleIndex="4" FieldName="VALORITEM"
                            Width="200px">
                            <PropertiesTextEdit MaxLength="100">
                            </PropertiesTextEdit>
                        </dxwgv:GridViewDataTextColumn>
                    </Columns>
                </dxwgv:ASPxGridView>
            </td>
        </tr>
        <tr>
            <td>
                <b>Saldo Final: </b>
                <asp:Label ID="lblSaldoFinalMerenda" runat="server"> </asp:Label>
            </td>
            <td>
                <b>Saldo Final: </b>
                <asp:Label ID="lblSaldoFinalManutencao" runat="server"> </asp:Label>
            </td>
            <td>
                <b>Saldo Final: </b>
                <asp:Label ID="lblSaldoFinalOutroProjeto" runat="server"> </asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <b>A</b>
            </td>
            <td>
                <b>B</b>
            </td>
            <td>
                C
            </td>
        </tr>
        <tr>
            <td>
                <b>&nbsp;</b>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <b>Saldo bancário teórico:</b><asp:Label ID="lblSaldoBancarioTeorico" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <b>A+B+C</b>
            </td>
        </tr>
    </table>
</asp:Content>
