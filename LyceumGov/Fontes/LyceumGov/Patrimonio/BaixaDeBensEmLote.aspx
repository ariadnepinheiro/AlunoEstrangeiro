<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="BaixaDeBensEmLote.aspx.cs" Inherits="Techne.Lyceum.Net.Patrimonio.BaixaDeBensEmLote" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:HiddenField ID="hidPageInstance" runat="server" />
    
    <asp:Panel ID="pnGeral" runat="server" GroupingText="Localização do Patrimônio:" Width="100%" style="max-width: 1515px">
        <table width="900px" style="table-layout: fixed">
            <tr>
                <td width="150px" align="right">
                    <asp:Label Font-Names="Verdana" ID="lblUACedente" runat="server" Text="Unidade Administrativa:*"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td width="600px">
                    <tweb:TSearch ID="tseUACedente" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QuerySetorCedente"
                        AutoPostBack="true" OnTextChanged="tseUACedente_Changed" Width="575px">
                    </tweb:TSearch>
                </td>
                <td width="100px">&nbsp;</td>
            </tr>
            <tr>
                <td align="right">
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
                <td>
                    <asp:Button ID="btnLimpar" runat="server" Text="Limpar" OnClick="btnLimpar_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <asp:Panel ID="pnlGrid" runat="server"  Width="100%">
        <asp:ObjectDataSource ID="odsPatrimonio" runat="server" TypeName="Techne.Lyceum.Net.Patrimonio.BaixaDeBensEmLote" SelectMethod="Lista">
            <SelectParameters>
                <asp:ControlParameter ControlID="tseUACedente" Name="setor" PropertyName="DBValue" />
                <asp:ControlParameter ControlID="tseClassificacao" Name="conta" PropertyName="DBValue" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <table>
            <tr>
                <td>
                    <dxwgv:ASPxGridView ID="grdPatrimonio" runat="server" AutoGenerateColumns="False" Width="100%"
                        ClientInstanceName="grdPatrimonio" DataSourceID="odsPatrimonio" KeyFieldName="BEMID"
                        OnCustomUnboundColumnData="grdPatrimonio_CustomUnboundColumnData" >
                        <ClientSideEvents EndCallback="function(s, e) {  }"  />
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
                <td height="50px">
                    <asp:Button ID="btnAdicionar" runat="server" Text="Adicionar" OnClick="btnAdicionar_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:PlaceHolder ID="plaMensagemSelecionado" runat="server" Visible="false">
                        <asp:Label ID="lblMensagemSelecionado" runat="server" SkinID="lblMensagem"></asp:Label>
                        <br /><br />
                    </asp:PlaceHolder>
                
                    <dxwgv:ASPxGridView ID="grdSelecionado" runat="server" AutoGenerateColumns="False" Width="100%" 
                        ClientInstanceName="grdSelecionado" KeyFieldName="BEMID" OnCustomButtonCallback="grdSelecionado_CustomButtonCallback">
                        <ClientSideEvents EndCallback="function(s, e) { atualizaQtdItensSelecionados(); }" />
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
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="pnlBaixa" runat="server" GroupingText="Dados da Baixa:" Width="100%" style="max-width: 1515px">
                        <table width="600" style="table-layout: fixed;">
                            <tr>
                                <td width="100" align="right">
                                    <asp:Label Font-Names="Verdana" ID="Label12" SkinID="lblObrigatorio" runat="server" Text="Motivo:*"></asp:Label>
                                </td>
                                <td width="500">
                                    <asp:DropDownList ID="ddlMotivoBaixa" runat="server" DataTextField="descricao" DataValueField="motivobaixaid"
                                        OnSelectedIndexChanged="ddlMotivoBaixa_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label Font-Names="Verdana" ID="Label3" runat="server" Text="Processo:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlProcessoPrefixo" runat="server">
                                        <asp:ListItem Text="Selecione" Value="">
                                        </asp:ListItem>
                                        <asp:ListItem Text="E-03/" Value="E-03/">
                                        </asp:ListItem>
                                        <asp:ListItem Text="SEI-" Value="SEI-">
                                        </asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtProcesso" runat="server" MaxLength="20" Width="109px" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label Font-Names="Verdana" ID="Label5" SkinID="lblObrigatorio" runat="server"
                                        Text="Data da Baixa:*"></asp:Label>
                                </td>
                                <td>
                                    <dxe:ASPxDateEdit ID="dtDataBaixa" runat="server" Width="100px" Enabled="true" EnableDefaultAppearance="true"
                                        ClientInstanceName="dtDataBaixa" CalendarProperties-ClearButtonText="Limpar"
                                        CalendarProperties-TodayButtonText="Hoje">
                                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                        </CalendarProperties>
                                    </dxe:ASPxDateEdit>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label Font-Names="Verdana" ID="Label16" runat="server" Text="Observação:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtObservacao" runat="server" MaxLength="500" TextMode="MultiLine"
                                        Height="75px" Width="500px" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label Font-Names="Verdana" ID="lblBoletim" SkinID="lblObrigatorio" runat="server"
                                        Text="Boletim de Ocorrência:*" Visible="false"></asp:Label>
                                    <asp:Label Font-Names="Verdana" ID="lblCNPJ" SkinID="lblObrigatorio" runat="server"
                                        Text="CNPJ:*" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBoletimOcorrencia" runat="server" MaxLength="100" Visible="false" />
                                    <asp:TextBox ID="txtCNPJ" runat="server" MaxLength="18" Visible="false" onkeypress="formataCNPJ(this,event)" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label Font-Names="Verdana" ID="lblPrefeitura" SkinID="lblObrigatorio" runat="server"
                                        Text="Prefeitura/<br />Instituição:*" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPrefeituraInstituicao" runat="server" MaxLength="200" Visible="false" TextMode="MultiLine"
                                        Height="51px" Width="254px" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label Font-Names="Verdana" ID="lblMensagemBaixa" SkinID="lblMensagem" runat="server" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnEfetuarBaixa" runat="server" Text="Efetuar Baixa em Lote" OnClick="btnEfetuarBaixa_Click"
                        OnClientClick="return confirm('Tem certeza que confirma a baixa do bem?');" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    
    <script language="javascript">
        function atualizaQtdItensSelecionados() {
            let qtdItensSelecionados = grdSelecionado.GetVisibleRowsOnPage();
            let lblMensagemSelecionado = $("#<%= lblMensagemSelecionado.ClientID %>");
            if (lblMensagemSelecionado)            
                lblMensagemSelecionado.text("Existe(m) " + qtdItensSelecionados + " item(s) na Lista de Bens abaixo.");
        };
    </script>
    
</asp:Content>
