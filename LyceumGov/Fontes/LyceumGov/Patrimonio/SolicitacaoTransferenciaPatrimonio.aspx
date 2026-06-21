<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="SolicitacaoTransferenciaPatrimonio.aspx.cs" Inherits="Techne.Lyceum.Net.Patrimonio.SolicitacaoTransferenciaPatrimonio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <dxtc:ASPxPageControl ID="pcTransferencia" runat="server" ActiveTabIndex="0" OnTabClick="pcTransferencia_TabClick" Width="90%">
        <TabPages>
            <dxtc:TabPage Text="Solicitação de Transferência">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl1" runat="server">                        
                        <asp:Panel ID="pnGeral" runat="server" GroupingText="Localização do Patrimônio:" Width="100%">
                            <table style="width:100%">
                                <tr>
                                    <td style="text-align: right; width: 200px">
                                        <asp:Label Font-Names="Verdana" ID="lblUACedente" runat="server" Text="Unidade Administrativa Cedente:*"
                                            SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <tweb:TSearch ID="tseUACedente" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QuerySetorCedente"
                                            AutoPostBack="true" OnTextChanged="tseUACedente_Changed" Width="575px">
                                        </tweb:TSearch>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; width: 200px">
                                        <asp:Label Font-Names="Verdana" ID="lblClassificacao" SkinID="lblObrigatorio" runat="server"
                                            Text="Classificação:*"></asp:Label>
                                    </td>
                                    <td>
                                        <tweb:TSearchBox ID="tseClassificacao" runat="server" SqlSelect="SELECT CONTA, DESCRICAO FROM [LYCEUM].[Patrimonio].[CLASSIFICACAO]"
                                            SqlOrder="CONTA" ColumnName="conta" Caption="" MaxLength="15" SqlWhere=" ativo=1"
                                            DataType="Varchar" OnChanged="tseClassificacao_Changed">
                                            <GridColumns>
                                                <tweb:TSearchBoxColumn Caption="Conta" FieldName="CONTA" Width="20%" />
                                                <tweb:TSearchBoxColumn Caption="Nome" FieldName="DESCRICAO" Width="80%" />
                                            </GridColumns>
                                        </tweb:TSearchBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="Panel1" runat="server" GroupingText="Dados da Transferência:" Width="100%">
                            <table Width="100%">
                                <tr>
                                    <td style="text-align: right; width: 200px">
                                        <asp:Label Font-Names="Verdana" ID="Label2" SkinID="lblObrigatorio" runat="server"
                                            Text="Unidade Administrativa Destinatária:*"></asp:Label>
                                    </td>
                                    <td>
                                        <tweb:TSearchBox ID="tseUADestinataria" runat="server" SqlSelect="SELECT ua_atual, nomesetor,  ua_antiga, setor FROM hades..vw_setor"
                                            SqlOrder="setor" ColumnName="setor" Caption="" Connection="Hades" MaxLength="15"
                                            DataType="Varchar" OnChanged="tseUADestinataria_Changed">
                                            <GridColumns>                                               
                                                <tweb:TSearchBoxColumn Caption="Código" FieldName="setor" Width="10%" />
                                                <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="10%" />
                                                <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="10%" />                                               
                                                <tweb:TSearchBoxColumn Caption="Nome" FieldName="nomesetor" Width="70%" />
                                            </GridColumns>
                                        </tweb:TSearchBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:CheckBox ID="chkAtesto" runat="server" Text="Atesto que encaminhei a transferência do bem."
                                            SkinID="lblObrigatorio" />
                                    </td>                                   
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
                        <asp:Panel ID="pnlGrid" runat="server"  Width="100%">
                            <asp:ObjectDataSource ID="odsPatrimonio" runat="server" TypeName="Techne.Lyceum.Net.Patrimonio.SolicitacaoTransferenciaPatrimonio"
                                SelectMethod="Lista">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="tseUACedente" Name="setor" PropertyName="DBValue" />
                                    <asp:ControlParameter ControlID="tseClassificacao" Name="conta" PropertyName="DBValue" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <table >
                                <tr>
                                    <td>
                                        <dxwgv:ASPxGridView ID="grdPatrimonio" runat="server" AutoGenerateColumns="False" Width="100%"
                                            ClientInstanceName="grdPatrimonio" DataSourceID="odsPatrimonio" KeyFieldName="BEMID"
                                            OnCustomUnboundColumnData="grdPatrimonio_CustomUnboundColumnData" >
                                            <ClientSideEvents EndCallback="function(s, e) { OnEndCallBack(s); }"  />
                                            <Columns>
                                                <dxwgv:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" ButtonType="Image">
                                                    <HeaderTemplate>
                                                        <input type="checkbox" onclick="grdPatrimonio.SelectAllRowsOnPage(this.checked);"
                                                            title="Select/Unselect all rows on the page" />
                                                    </HeaderTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </dxwgv:GridViewCommandColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="BEMID" ReadOnly="true"
                                                    VisibleIndex="1" Visible="false">
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="Número" UnboundType="String" FieldName="NUMERO"
                                                    ReadOnly="true" VisibleIndex="2">
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn FieldName="DESCRICAO" Caption="Patrimônio" ReadOnly="true"
                                                    VisibleIndex="3">
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="Código Classificação" FieldName="CONTA" ReadOnly="true" VisibleIndex="4">
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="Classificação" FieldName="CLASSIFICACAO" ReadOnly="true"
                                                    VisibleIndex="5">
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="Conservação" FieldName="ESTADOCONSERVACAO"
                                                    ReadOnly="true" VisibleIndex="6">
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn FieldName="VALORCOMSIGLA" ReadOnly="true" Caption="Valor Atualizado"
                                                    VisibleIndex="7">
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn FieldName="DATAAQUISICAO" ReadOnly="true" Caption="Data Aquisição"
                                                    VisibleIndex="8">
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="Data Incorporação" FieldName="DATAINCORPORACAO"
                                                    ReadOnly="true" Visible="true" VisibleIndex="9">
                                                </dxwgv:GridViewDataTextColumn>
                                            </Columns>
                                            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                                        </dxwgv:ASPxGridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnAdicionar" runat="server" Text="Adicionar" OnClick="btnAdicionar_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblQtdSolicitacoes" runat="server" SkinID="lblMensagem" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dxwgv:ASPxGridView ID="grdSelecionado" runat="server" AutoGenerateColumns="False" Width="100%" 
                                            ClientInstanceName="grdSelecionado" KeyFieldName="BEMID" OnCustomButtonCallback="grdSelecionado_CustomButtonCallback">
                                            <ClientSideEvents EndCallback="function(s, e) { AtualizarLabelQtdSelecionado(); }" />
                                             <SettingsPager Mode="ShowAllRecords" />
                                            <Columns>
                                                <dxwgv:GridViewCommandColumn VisibleIndex="13" ButtonType="Link" Width="50px" Caption="Excluir">
                                                    <CustomButtons>
                                                        <dxwgv:GridViewCommandColumnCustomButton ID="btnExcluir" Text="Excluir" Visibility="AllDataRows">
                                                        </dxwgv:GridViewCommandColumnCustomButton>
                                                    </CustomButtons>
                                                </dxwgv:GridViewCommandColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="BEMID" ReadOnly="true" Visible="false"
                                                    VisibleIndex="1">
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="Número" UnboundType="String" FieldName="NUMERO"
                                                    ReadOnly="true" VisibleIndex="2">
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn FieldName="DESCRICAO" Caption="Patrimônio" ReadOnly="true"
                                                    VisibleIndex="3">
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="Código Classificação" FieldName="CONTA" ReadOnly="true" VisibleIndex="4">
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="Classificação" FieldName="CLASSIFICACAO" ReadOnly="true"
                                                    VisibleIndex="5">
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="Conservação" FieldName="ESTADOCONSERVACAO"
                                                    ReadOnly="true" VisibleIndex="6">
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn FieldName="VALORCOMSIGLA" ReadOnly="true" Caption="Valor Atualizado"
                                                    VisibleIndex="7">
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn FieldName="DATAAQUISICAO" ReadOnly="true" Caption="Data Aquisição"
                                                    VisibleIndex="8">
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="Data Incorporação" FieldName="DATAINCORPORACAO"
                                                    ReadOnly="true" Visible="true" VisibleIndex="9">
                                                </dxwgv:GridViewDataTextColumn>
                                            </Columns>
                                            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                                        </dxwgv:ASPxGridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnSalvar" runat="server" ValidationGroup="SalvarForm" Text="Encaminhar Transferência"
                                            OnClick="btnSalvar_Click" />
                                    </td>
                                </tr>
                            </table>                            
                        </asp:Panel>
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
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
        </TabPages>
    </dxtc:ASPxPageControl>
    
    <script language="javascript">
        function AtualizarLabelQtdSelecionado() {
            var qtd = grdSelecionado.GetVisibleRowsOnPage();

            if (qtd > 0)
                $("#<%= lblQtdSolicitacoes.ClientID %>").html("Existe(m) " + qtd + " item(s) na Lista de Transferência abaixo.<br /><br />");
            else
                $("#<%= lblQtdSolicitacoes.ClientID %>").html("");
        }

        AtualizarLabelQtdSelecionado();
    </script>
</asp:Content>
