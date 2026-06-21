<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ListaAvisoTransferenciaBens.aspx.cs" Inherits="Techne.Lyceum.Net.Patrimonio.ListaAvisoTransferenciaBens" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
     <asp:Panel runat="server" ID="pnlFiltro" GroupingText="Informe os dados para pesquisar as transferências"
        Width="80%">
        <table style="width: 100%">
            <tr>
                <td style="text-align: right; width: 200px">
                    <asp:Label Font-Names="Verdana" ID="lblUACedente" runat="server" Text="Unidade Administrativa Cedente:*"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseUACedente" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QuerySetorCedente"
                        AutoPostBack="true" OnTextChanged="tseUACedente_Changed" >
                    </tweb:TSearch>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 200px">
                    <asp:Label Font-Names="Verdana" ID="Label2" SkinID="lblObrigatorio" runat="server"
                        Text="Unidade Administrativa Destinatária:*"></asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseUADestinataria" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QuerySetor"
                        AutoPostBack="true" OnTextChanged="tseUADestinataria_Changed">
                    </tweb:TSearch>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <asp:Panel ID="pnlGrid" runat="server" Width="90%" Visible="false">
        <asp:HiddenField ID="hidSetorOrigem" runat="server" />
        <asp:HiddenField ID="hidSetorDestino" runat="server" />
        
        <asp:ObjectDataSource ID="odsATBM" runat="server" TypeName="Techne.Lyceum.Net.Patrimonio.ListaAvisoTransferenciaBens"
            SelectMethod="Lista">
            <SelectParameters>
                <asp:ControlParameter ControlID="hidSetorOrigem" Name="setorCedente" PropertyName="Value" />
                <asp:ControlParameter ControlID="hidSetorDestino" Name="setorDestino" PropertyName="Value" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <table>
            <tr>
                <td>
                    <dxwgv:ASPxGridView ID="grdATBM" runat="server" AutoGenerateColumns="False" ClientInstanceName="grdATBM"
                        DataSourceID="odsATBM" KeyFieldName="LOTE" Width="100%" OnAfterPerformCallback="grdATBM_AfterPerformCallback"
                        OnCustomButtonCallback="grdATBM_CustomButtonCallback">
                        <SettingsBehavior AllowMultiSelection="False" ProcessSelectionChangedOnServer="true" />
                        <SettingsText EmptyDataRow="Não existem dados." />
                        <Columns>
                            <dxwgv:GridViewCommandColumn VisibleIndex="9" ButtonType="Image" Width="50px" Caption="Gerar ATBM">
                                <CustomButtons>
                                    <dxwgv:GridViewCommandColumnCustomButton ID="btnImprimir" Visibility="AllDataRows"
                                        Image-Url="../Images/ico_imprimir.png">
                                    </dxwgv:GridViewCommandColumnCustomButton>
                                </CustomButtons>
                            </dxwgv:GridViewCommandColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="LOTE" ReadOnly="true" Visible="false"
                                VisibleIndex="0">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="DATASOLICITACAO" Caption="Data Solicitação"
                                ReadOnly="true" VisibleIndex="1">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="U.A. Destino" FieldName="SETORDESTINO" ReadOnly="true"
                                VisibleIndex="2">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Destinatária" FieldName="SETORDESTINODESCRICAO"
                                ReadOnly="true" VisibleIndex="3">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="U.A. Cedente" FieldName="SETORORIGEM" ReadOnly="true"
                                VisibleIndex="4">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="SETORORIGEMDESCRICAO" ReadOnly="true" Caption="Cedente"
                                VisibleIndex="5">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Aceitos" FieldName="QUANTIDADEITENSACEITOS"
                                HeaderStyle-HorizontalAlign="Center" ReadOnly="true" VisibleIndex="6">
                                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                </CellStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Recusados" FieldName="QUANTIDADEITENSRECUSADOS"
                                HeaderStyle-HorizontalAlign="Center" ReadOnly="true" VisibleIndex="7">
                                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                </CellStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Pendentes" FieldName="QUANTIDADEITENSPENDENTES"
                                HeaderStyle-HorizontalAlign="Center" ReadOnly="true" VisibleIndex="8">
                                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                </CellStyle>
                            </dxwgv:GridViewDataTextColumn>
                        </Columns>
                        <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                    </dxwgv:ASPxGridView>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
