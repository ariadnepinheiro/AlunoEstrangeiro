<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="HistoricoTransferenciaPatrimonio.aspx.cs" Inherits="Techne.Lyceum.Net.Patrimonio.HistoricoTransferenciaPatrimonio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <dxtc:ASPxPageControl ID="pcTransferencia" runat="server" ActiveTabIndex="2" OnTabClick="pcTransferencia_TabClick"
        Width="90%">
        <TabPages>
            <dxtc:TabPage Text="Solicitação de Transferência">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl1" runat="server">
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Acompanhamento de Solicitações">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl2" runat="server">
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Histórico">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl3" runat="server">
                        <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
                        <asp:HiddenField ID="hdnBem" runat="server" />
                        <asp:Panel ID="pnGeral" runat="server" GroupingText="Localização do Patrimônio:"
                            Width="100%">
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
                                    <td style="text-align: right;">
                                        <asp:Label Font-Names="Verdana" ID="lblClassificacao" runat="server" Text="Classificação:"></asp:Label>
                                    </td>
                                    <td>
                                        <tweb:TSearchBox ID="tseClassificacao" runat="server" SqlSelect="SELECT CONTA, DESCRICAO,CLASSIFICACAOID FROM [LYCEUM].[Patrimonio].[CLASSIFICACAO]"
                                            SqlOrder="CONTA" ColumnName="conta" Caption="" MaxLength="15" SqlWhere=" ativo=1"
                                            DataType="Varchar" OnChanged="tseClassificacao_Changed">
                                            <GridColumns>
                                                <tweb:TSearchBoxColumn Caption="Conta" FieldName="CONTA" Width="20%" />
                                                <tweb:TSearchBoxColumn Caption="Nome" FieldName="DESCRICAO" Width="80%" />
                                            </GridColumns>
                                        </tweb:TSearchBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" ID="Label1" SkinID="lblObrigatorio" runat="server"
                                            Text="Patrimônio:*"></asp:Label>
                                    </td>
                                    <td>
                                        <tweb:TSearchBox ID="tseBem" runat="server" SqlOrder="numeroformatado" Caption=""
                                            AutoPostBack="true" OnChanged="tseBem_Changed" Key="numeroformatado">
                                            <GridColumns>
                                                <tweb:TSearchBoxColumn Caption="Nº Patrimônio" FieldName="numeroformatado" Width="20%" />
                                                <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="80%" />
                                            </GridColumns>
                                        </tweb:TSearchBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pnlGrid" runat="server" Width="100%">
                            <asp:ObjectDataSource ID="odsHistorico" runat="server" TypeName="Techne.Lyceum.Net.Patrimonio.HistoricoTransferenciaPatrimonio"
                                SelectMethod="Lista">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="hdnBem" Name="bem" PropertyName="Value" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <table>
                                <tr>
                                    <td>
                                        <dxwgv:ASPxGridView ID="grdHistorico" runat="server" AutoGenerateColumns="False"
                                            ClientInstanceName="grdHistorico" DataSourceID="odsHistorico" KeyFieldName="TRANSFERENCIAITEMID"
                                            Width="100%" OnCustomUnboundColumnData="grdHistorico_CustomUnboundColumnData">
                                            <ClientSideEvents EndCallback="function(s, e) { OnEndCallBack(s); }" />
                                            <Columns>
                                                <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="TRANSFERENCIAITEMID" ReadOnly="true"
                                                    VisibleIndex="1" Visible="false">
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="Número de Origem" UnboundType="String" FieldName="NUMERO"
                                                    ReadOnly="true" VisibleIndex="2">
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn FieldName="BEM" Caption="Patrimônio" ReadOnly="true"
                                                    VisibleIndex="3">
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="Código Classificação" FieldName="CONTA" ReadOnly="true"
                                                    VisibleIndex="4">
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="Classificação" FieldName="CLASSIFICACAO" ReadOnly="true"
                                                    VisibleIndex="5">
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="Conservação" FieldName="ESTADOCONSERVACAO"
                                                    ReadOnly="true" VisibleIndex="6">
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="Situação" FieldName="SITUACAO" ReadOnly="true"
                                                    VisibleIndex="7">
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn FieldName="VALORCOMSIGLA" ReadOnly="true" Caption="Valor"
                                                    VisibleIndex="8" >
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="Valor" FieldName="VALORFORMAT" UnboundType="String"
                                                    VisibleIndex="8" Name="Valor">
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="Unidade de Origem" FieldName="SETORORIGEMDESCRICAO"
                                                    ReadOnly="true" VisibleIndex="9">
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="Unidade de Destino" FieldName="SETORDESTINODESCRICAO"
                                                    ReadOnly="true" VisibleIndex="10">
                                                </dxwgv:GridViewDataTextColumn>
                                               
                                                <dxwgv:GridViewDataTextColumn Caption="Justificativa" FieldName="JUSTIFICATIVA" ReadOnly="true"
                                                    Visible="true" VisibleIndex="12">
                                                </dxwgv:GridViewDataTextColumn>
                                                 <dxwgv:GridViewDataTextColumn FieldName="DATAMOVIMENTACAO" ReadOnly="true" Caption="Data da Transferência"
                                                    VisibleIndex="13">
                                                </dxwgv:GridViewDataTextColumn>
                                                 <dxwgv:GridViewDataTextColumn FieldName="DATASOLICITACAO" ReadOnly="true" Caption="Data da Solicitação"
                                                    VisibleIndex="14">
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="Data Andamento" FieldName="DATAANDAMENTO"
                                                    ReadOnly="true" Visible="true" VisibleIndex="15">
                                                </dxwgv:GridViewDataTextColumn>
                                            </Columns>
                                            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                                        </dxwgv:ASPxGridView>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
        </TabPages>
    </dxtc:ASPxPageControl>
</asp:Content>
