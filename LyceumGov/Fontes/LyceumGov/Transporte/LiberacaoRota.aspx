<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="LiberacaoRota.aspx.cs" Inherits="Techne.Lyceum.Net.Transporte.LiberacaoRota" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel runat="server" ID="pnlPesquisa" GroupingText="Informe os dados para pesquisa."
        Width="80%">
        <table>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblRegional" runat="server" Font-Names="Verdana" Text="Regional:"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseRegional" runat="server" Argument="regional" ArgumentColumns="50"
                        MaxLength="20" Columns="10" AutoPostBack="True" Caption="" OnChanged="tseRegional_Changed"
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
                    <asp:Label Font-Names="Verdana" ID="lblMunicipio" runat="server" Text="Município:"></asp:Label>
                </td>
                <td>
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
                <td style="text-align: right; width: 15%">
                    <asp:Label Font-Names="Verdana" ID="lblUnidadeResponsavel" SkinID="lblObrigatorio"
                        runat="server" Text="Unidade de Ensino:*"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUnidadeResponsavel" runat="server" Caption="" Key="unidade_ens"
                        MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" GridWidth="850px"
                        OnChanged="tseUnidadeResponsavel_Changed" AutoPostBack="True" SqlOrder="nome_comp"
                        SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao,nucleo,municipio,id_regional,ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO ">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="20%" />                            
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Regional" FieldName="id_regional" Width="30%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <asp:Panel ID="pnlRotas" runat="server" Visible="false">
        <asp:ObjectDataSource ID="odsRota" TypeName="Techne.Lyceum.Net.Transporte.LiberacaoRota"
            runat="server" SelectMethod="ListaRota">
            <SelectParameters>
                <asp:ControlParameter ControlID="tseUnidadeResponsavel" Name="unidade_ens" PropertyName="DBValue" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <br />
        <dxwgv:ASPxGridView ID="grdRota" runat="server" AutoGenerateColumns="False" Width="80%"
            Visible="true" ClientInstanceName="grdRota" DataSourceID="odsRota" KeyFieldName="ROTAID"
            OnAfterPerformCallback="grdRota_AfterPerformCallback" OnCustomButtonCallback="grdRota_CustomButtonCallback"
            OnCustomButtonInitialize="grdRota_CustomButtonInitialize" EnableCallBacks="false">
            <SettingsBehavior ConfirmDelete="True" />
            <SettingsEditing Mode="Inline" />
            <SettingsText EmptyDataRow="Não existem dados." />
            <Columns>
                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                    <ClearFilterButton Text="Limpar" Visible="True">
                        <Image Url="~/img/bt_limpa.png" />
                    </ClearFilterButton>
                    <CustomButtons>
                        <dxwgv:GridViewCommandColumnCustomButton ID="btnAprovar" Text="Aprovar">
                            <Image Url="../Images/sel.png" />
                        </dxwgv:GridViewCommandColumnCustomButton>
                    </CustomButtons>
                    <CustomButtons>
                        <dxwgv:GridViewCommandColumnCustomButton ID="btnLiberarAluno" Text="Liberar Associação Alunos">
                            <Image Url="../Images/Edit.png" />
                        </dxwgv:GridViewCommandColumnCustomButton>
                    </CustomButtons>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataTextColumn Caption="Codigo" FieldName="ROTAID" VisibleIndex="1"
                    Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Codigo" FieldName="CODIGO" VisibleIndex="2"
                    Visible="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Turno" FieldName="TURNO" VisibleIndex="3"
                    Visible="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Tipo" FieldName="TIPOCALCULOPAGAMENTO" VisibleIndex="4"
                    Visible="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Situação" FieldName="SITUACAO" VisibleIndex="5"
                    Visible="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataDateColumn Caption="Data Limite edição*" FieldName="DATALIMITEEDICAO"
                    VisibleIndex="6" Visible="true">
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataDateColumn Caption="Data Limite alteração alunos*" FieldName="DATALIMITEEDICAOALUNO"
                    VisibleIndex="7" Visible="true">
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewCommandColumn VisibleIndex="8" ButtonType="Link" Width="50px" Caption="Dados Rota"
                    Name="DadosRota">
                    <CustomButtons>
                        <dxwgv:GridViewCommandColumnCustomButton ID="btnDados" Text="Dados Rota" Image-Width="50px"
                            Visibility="AllDataRows">
                        </dxwgv:GridViewCommandColumnCustomButton>
                    </CustomButtons>
                </dxwgv:GridViewCommandColumn>
            </Columns>
            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
        </dxwgv:ASPxGridView>
    </asp:Panel>
    <dxpc:ASPxPopupControl ID="pucRota" ClientInstanceName="pucRota" runat="server" Modal="true"
        ShowShadow="true" AllowDragging="false" AllowResize="false" ShowCloseButton="false"
        ShowFooter="false" ShowHeader="false" ShowSizeGrip="False" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" EnableAnimation="true" Width="600">
        <Border BorderColor="Gainsboro" BorderStyle="Ridge" BorderWidth="4px" />
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,15000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl>
                <table>
                    <tr>
                        <td style="font-weight: bold; font-size: 13px;">
                            <asp:Label ID="Label1" runat="server" Text="Codigo Rota:"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblCodigo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold; font-size: 13px;">
                            <asp:Label ID="Label5" runat="server" Text="Escola:"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblEscola" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold; font-size: 13px;">
                            <asp:Label ID="Label2" runat="server" Text="Regional:"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblRegionalRota" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold; font-size: 13px;">
                            <asp:Label ID="Label3" runat="server" Text="Municipio:"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblMunicipioRota" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold; font-size: 13px;">
                            <asp:Label ID="Label4" runat="server" Text="Região Financeira:"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblRegiaoFinanceira" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold; font-size: 13px;">
                            <asp:Label ID="Label7" runat="server" Text="CNPJ:"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblCnpj" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold; font-size: 13px;">
                            <asp:Label ID="Label6" runat="server" Text="Turno:"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblTurno" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold; font-size: 13px;">
                            <asp:Label ID="Label8" runat="server" Text="Tipo Calculo Pagamento:"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblTipoCalculoPagamento" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Panel ID="pnlIda" runat="server" GroupingText="Trajeto de Ida" Width="100%">
                    <table>
                        <tr>
                            <td style="font-weight: bold; font-size: 13px;">
                                <asp:Label ID="Label12" runat="server" Text="Contratação:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblTipoContratacaoIda" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold; font-size: 13px;">
                                <asp:Label ID="Label13" runat="server" Text="Valor:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblValorRotaIda" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold; font-size: 13px;">
                                <asp:Label ID="Label14" runat="server" Text="Quantidade Km:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblQuantidadeKmIda" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold; font-size: 13px;">
                                <asp:Label ID="Label15" runat="server" Text="Quantidade Alunos:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblQuantidadeAlunoIda" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold; font-size: 13px;">
                                <asp:Label ID="Label9" runat="server" Text="Prestador:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPrestadorIda" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold; font-size: 13px;">
                                <asp:Label ID="Label10" runat="server" Text="Condutor:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCondutorIda" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold; font-size: 13px;">
                                <asp:Label ID="Label11" runat="server" Text="Veiculo:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblVeiculoIda" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <br />
                <asp:Panel ID="pnlVolta" runat="server" GroupingText="Trajeto de Volta" Width="100%">
                    <table>
                        <tr>
                            <td style="font-weight: bold; font-size: 13px;">
                                <asp:Label ID="Label16" runat="server" Text="Contratação:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblTipoContratacaoVolta" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold; font-size: 13px;">
                                <asp:Label ID="Label17" runat="server" Text="Valor:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblValorRotaVolta" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold; font-size: 13px;">
                                <asp:Label ID="Label18" runat="server" Text="Quantidade Km:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblQuantidadeKmVolta" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold; font-size: 13px;">
                                <asp:Label ID="Label19" runat="server" Text="Quantidade Alunos:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblQuantidadeAlunoVolta" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold; font-size: 13px;">
                                <asp:Label ID="Label20" runat="server" Text="Prestador:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPrestadorVolta" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold; font-size: 13px;">
                                <asp:Label ID="Label21" runat="server" Text="Condutor:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCondutorVolta" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold; font-size: 13px;">
                                <asp:Label ID="Label22" runat="server" Text="Veiculo:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblVeiculoVolta" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <br />
                <table width="100%">
                    <tr>
                        <td align="right">
                            <asp:Button ID="btCancelar" runat="server" Text="Cancelar" OnClientClick="pucRota.Hide();" />
                        </td>
                    </tr>
                </table>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
</asp:Content>
