<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ExigenciasEvento.aspx.cs" Inherits="Techne.Lyceum.Net.PrestacaoContas.ExigenciasEvento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Selecione as informações abaixo para filtrar"
        Width="70%">
        <table>
            <tr>
                <td align="right">
                    <asp:Label Font-Names="Verdana" ID="Label3" SkinID="lblObrigatorio" runat="server"
                        Text="Período de Prestação de Contas:*"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tsePeriodoReferencia" runat="server" Argument="DESCRICAO" ArgumentColumns="50"
                        MaxLength="20" Columns="10" AutoPostBack="True" Caption="" SqlOrder="ANO, MESINICIAL DESC"  OnChanged="tsePeriodoReferencia_Changed"
                        Key="PERIODOREFERENCIAID" SqlSelect=" SELECT ANO, MESINICIAL, MESFINAL, REFERENCIA FROM PrestacaoContas.VW_PERIODOREFERENCIA "
                        DataType="Number">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="PERIODOREFERENCIAID" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Período" FieldName="DESCRICAO" Width="80%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblUnidadeOrigem" SkinID="lblObrigatorio" runat="server" Text="Unidade de Ensino:*"></asp:Label>
                </td>
                <td colspan="3">
                    <tweb:TSearchBox ID="tseUnidadeEnsino" runat="server" Caption="" Key="unidade_ens" AutoPostBack="true"
                        Argument="nome_comp" ColumnName="Faculdade" SqlSelect="SELECT unidade_ens, nome_comp, setor, cgc, situacao from VW_UNIDADE_ENSINO_SITUACAO"
                        MaxLength="20" FieldName="Unidade de Ensino" SqlOrder="nome_comp" OnChanged="tseUnidadeEnsino_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="12%" />
                            <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="Label2" runat="server" Text="Finalidade:"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddlFinalidade" runat="server" OnSelectedIndexChanged="ddlFinalidade_SelectedIndexChanged" 
                    DataTextField="DESCRICAO" DataValueField="FINALIDADEID" AutoPostBack="true" >
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
               <td style="text-align: right; width: 15%">
                    <asp:Label ID="Label1" runat="server" Text="Despesa:"></asp:Label>
                </td>
                <td colspan="3">
                    <tweb:TSearchBox ID="tseEvento" runat="server" Caption="" Key="EVENTOID" MaxLength="20" Argument="NUMEROEVENTO"
                        ArgumentColumns="50" Columns="10" GridWidth="850px" SqlSelect=" SELECT UE.NOME_COMP AS ESCOLA, F.DESCRICAO AS FINALIDADE, E.DATAPAGAMENTO, E.PLANOTRABALHOID, E.NUMERONOTAFISCAL FROM [PRESTACAOCONTAS].[EVENTO] E INNER JOIN LY_UNIDADE_ENSINO UE (NOLOCK) ON E.CENSO = UE.UNIDADE_ENS INNER JOIN [PRESTACAOCONTAS].[PLANOTRABALHO] PT (NOLOCK) ON E.PLANOTRABALHOID = PT.PLANOTRABALHOID INNER JOIN PRESTACAOCONTAS.FINALIDADE F (NOLOCK) ON PT.FINALIDADEID = F.FINALIDADEID "
                        OnChanged="tseEvento_Changed" DataType="Number" SqlWhere=" NUMEROEVENTO IS NOT NULL ">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="EVENTOID" Width="5%" />
                            <tweb:TSearchBoxColumn Caption="Número" FieldName="NUMEROEVENTO" Width="15%" />                                                        
                            <tweb:TSearchBoxColumn Caption="Nota Fiscal" FieldName="NUMERONOTAFISCAL" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="Escola" FieldName="ESCOLA" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Finalidade" FieldName="FINALIDADE" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Dt.Pagamento" FieldName="DATAPAGAMENTO" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="Código Plano" FieldName="PLANOTRABALHOID" Width="5%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="right">
                    <asp:ImageButton ID="btnBuscar" runat="server" SkinID="BcBuscar" OnClick="btnBuscar_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <asp:Panel ID="pnlGrid" runat="server" Visible="false">
        <dxwgv:ASPxGridView ID="grdExigencias" runat="server" KeyFieldName="EXIGENCIAEVENTOID"
            ClientInstanceName="grdExigencias" AutoGenerateColumns="False" Width="80%" SkinID="NoConfirmDelete"
            EnableCallBacks="false" OnPageIndexChanged="grdExigencias_PageIndexChanged">
            <SettingsBehavior AllowMultiSelection="False" ProcessSelectionChangedOnServer="true" />
            <SettingsText EmptyDataRow="Não existem dados." />
            <Styles CommandColumn-Wrap="False" />
            <Columns>
                <dxwgv:GridViewDataTextColumn Caption=" " Name="btnDetalhes" VisibleIndex="1" Width="30px"
                    CellStyle-HorizontalAlign="Center" EditFormCaptionStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center">
                    <DataItemTemplate>
                        <asp:ImageButton ID="btnDetalhes" runat="server" EnableViewState="false" CommandArgument='<%# Eval("EVENTOID") + "," + Eval("TIPODESPESA") %>'
                            OnCommand="btnDetalhes_Command" ImageUrl="~/img/bt_busca.png" Height="20px" AlternateText="Ir para o evento">
                        </asp:ImageButton>
                    </DataItemTemplate>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="EXIGENCIAEVENTOID" VisibleIndex="2" Caption="EXIGENCIAEVENTOID"
                    Visible="false" CellStyle-HorizontalAlign="Center" Width="30">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="EVENTOID" VisibleIndex="3" Caption="EVENTOID"
                    Visible="false" CellStyle-HorizontalAlign="Center" Width="30">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="CENSO" VisibleIndex="4" Caption="Censo"
                    Visible="false" CellStyle-HorizontalAlign="Center">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="ESCOLA" VisibleIndex="5" Caption="Unidade Ensino">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="NUMEROEVENTO" VisibleIndex="6" Caption="Num. Despesa">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="FINALIDADE" VisibleIndex="7" Caption="Finalidade">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="TIPODESPESA" VisibleIndex="8" Caption="Tipo Despesa">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="DATAEVENTO" VisibleIndex="9" Caption="Data Despesa"
                    CellStyle-HorizontalAlign="Center" Width="100">
                    <PropertiesTextEdit DisplayFormatString="dd/MM/yyyy" MaxLength="100">
                    </PropertiesTextEdit>
                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                    <CellStyle HorizontalAlign="Left">
                    </CellStyle>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="DATANOTAFISCAL" VisibleIndex="10" Caption="Data NF"
                    CellStyle-HorizontalAlign="Center" Width="100">
                    <PropertiesTextEdit DisplayFormatString="dd/MM/yyyy" MaxLength="100">
                    </PropertiesTextEdit>
                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                    <CellStyle HorizontalAlign="Left">
                    </CellStyle>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="DATAPAGAMENTO" VisibleIndex="11" Caption="Data Pagamento"
                    CellStyle-HorizontalAlign="Center" Width="100">
                    <PropertiesTextEdit DisplayFormatString="dd/MM/yyyy" MaxLength="100">
                    </PropertiesTextEdit>
                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                    <CellStyle HorizontalAlign="Left">
                    </CellStyle>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="VALORPAGAMENTO" VisibleIndex="12" Caption="Valor Pagamento"
                    CellStyle-HorizontalAlign="Center">
                    <PropertiesTextEdit Width="60px" DisplayFormatString="c" ValidationSettings-Display="Dynamic"
                        MaxLength="9">
                        <MaskSettings Mask="$&lt;0..9999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="MOTIVO" VisibleIndex="13" Caption="Motivo da Exigência">
                </dxwgv:GridViewDataTextColumn>
            </Columns>
            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
        </dxwgv:ASPxGridView>
    </asp:Panel>
</asp:Content>
