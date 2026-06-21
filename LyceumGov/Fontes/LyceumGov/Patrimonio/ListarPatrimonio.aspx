<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ListarPatrimonio.aspx.cs" Inherits="Techne.Lyceum.Net.Patrimonio.ListarPatrimonio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript" language="javascript">
        function Novo() {
            if (typeof (grdPatrimonio) != 'undefined' && grdPatrimonio != null)
                grdPatrimonio.AddNewRow();
        }
    </script>

    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <asp:Panel ID="pnGeral" runat="server" GroupingText="Localização do Patrimônio:"
        Width="90%">
        <table style="width: 100%">
            <tr>
                <td style="text-align: right; width: 150px">
                    <asp:Label Font-Names="Verdana" ID="lblUA" runat="server" Text="Unidade Administrativa:*"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseUA" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QuerySetorCedente"
                        AutoPostBack="true" OnTextChanged="tseUA_Changed" Width="575px">
                    </tweb:TSearch>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 150px">
                    <asp:Label Font-Names="Verdana" ID="Label1" runat="server" Text="Classificação:"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseClassificacao" runat="server" SqlSelect="SELECT CONTA, DESCRICAO,CLASSIFICACAOID FROM [LYCEUM].[Patrimonio].[CLASSIFICACAO]"
                        SqlOrder="CONTA" ColumnName="conta" Caption="" MaxLength="15" DataType="Varchar"
                        OnChanged="tseClassificacao_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Conta" FieldName="CONTA" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="DESCRICAO" Width="80%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlGrid" runat="server" Width="90%">
        <asp:ObjectDataSource ID="odsPatrimonio" runat="server" TypeName="Techne.Lyceum.Net.Patrimonio.ListarPatrimonio"
            SelectMethod="Lista">
            <SelectParameters>
                <asp:ControlParameter ControlID="tseUA" Name="setor" PropertyName="DBValue" />
                <asp:ControlParameter ControlID="tseClassificacao" Name="classificacao" PropertyName="DBValue" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <table>
            <tr>
                <td>
                    <dxwgv:ASPxGridView ID="grdPatrimonio" runat="server" AutoGenerateColumns="False"
                        ClientInstanceName="grdPatrimonio" DataSourceID="odsPatrimonio" KeyFieldName="BEMID"
                        OnCommandButtonInitialize="grdPatrimonio_CommandButtonInitialize" Width="100%"
                        OnAfterPerformCallback="grdPatrimonio_AfterPerformCallback" OnCustomUnboundColumnData="grdPatrimonio_CustomUnboundColumnData"
                        OnHtmlDataCellPrepared="grdPatrimonio_HtmlDataCellPrepared">
                        <SettingsBehavior AllowMultiSelection="False" ProcessSelectionChangedOnServer="true" />
                        <SettingsText EmptyDataRow="Não existem dados." />
                        <Columns>
                            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="0px">
                                <HeaderCaptionTemplate>
                                    <div style="text-align: center">
                                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                                            onclick="Novo();" title="Novo" />
                                    </div>
                                </HeaderCaptionTemplate>
                                <EditButton Text="Editar" Visible="True">
                                    <Image Url="~/img/bt_editar.png" />
                                </EditButton>
                                <SelectButton Text="Selecionar" Visible="True">
                                    <Image Url="~/img/bt_busca.png" />
                                </SelectButton>
                                <CancelButton Text="Cancelar">
                                    <Image Url="~/img/bt_cancelar.png" />
                                </CancelButton>
                                <UpdateButton Text="Alterar">
                                    <Image Url="~/img/bt_salvar.png" />
                                </UpdateButton>
                                <ClearFilterButton Text="Limpar" Visible="True">
                                    <Image Url="~/img/bt_limpa.png" />
                                </ClearFilterButton>
                            </dxwgv:GridViewCommandColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="BEMID" ReadOnly="true"
                                Visible="false" VisibleIndex="0">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Número" UnboundType="String" FieldName="NUMERO"
                                ReadOnly="true" VisibleIndex="1">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="DESCRICAO" Caption="Patrimônio" ReadOnly="true"
                                VisibleIndex="2">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Código Classificação" FieldName="CONTA" ReadOnly="true"
                                VisibleIndex="3">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Classificação" FieldName="CLASSIFICACAO" ReadOnly="true"
                                VisibleIndex="4">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Conservação" FieldName="ESTADOCONSERVACAO"
                                ReadOnly="true" VisibleIndex="5">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="SIGLA" ReadOnly="true" Visible="false" VisibleIndex="6">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="ULTIMOVALOR" ReadOnly="true" VisibleIndex="7"
                                Caption="Valor" Visible="false" Width="20px" Name="ULTIMOVALOR">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Valor" FieldName="ULTIMOVALORFORMATADO" UnboundType="String"
                                VisibleIndex="7" Name="UltimoValor">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="VALORATUALIZADO" ReadOnly="true" VisibleIndex="8"
                                Caption="Valor Atualizado" Width="20px" Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="VALORATUALIZADOFORMAT" ReadOnly="true" VisibleIndex="8"
                                Caption="Valor Atualizado" UnboundType="String" Name="VALORATUALIZADOFORMAT">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="DATAAQUISICAO" ReadOnly="true" Caption="Data Aquisição"
                                VisibleIndex="9">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Data Incorporação" FieldName="DATAINCORPORACAO"
                                ReadOnly="true" Visible="true" VisibleIndex="10">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Data Baixa" FieldName="DATABAIXA" ReadOnly="true"
                                Visible="true" VisibleIndex="11" Name="DATABAIXA">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="BAIXA" FieldName="BAIXA" ReadOnly="true" Visible="false"
                                VisibleIndex="11">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataCheckColumn Caption="Precisa Reavaliar" FieldName="PRECISAREAVALIAR"
                                Name="PRECISAREAVALIAR" VisibleIndex="12">
                                <PropertiesCheckEdit ValueChecked="S" ValueType="System.String" ValueUnchecked="N">
                                </PropertiesCheckEdit>
                            </dxwgv:GridViewDataCheckColumn>
                        </Columns>
                        <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                    </dxwgv:ASPxGridView>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
