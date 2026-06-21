<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ListarRegistro.aspx.cs" Inherits="Techne.Lyceum.Net.Ocorrencia.ListarRegistro" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
</asp:Content>
<asp:Content ID="cListarRegistro" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:ScriptManagerProxy ID="scriptManager" runat="server" />
    <asp:UpdatePanel ID="upnlTurmas" UpdateMode="Always" runat="server" RenderMode="Block">
        <ContentTemplate>

            <script type="text/javascript">
                function Novo() {
                    if (typeof (grdRegistro) != 'undefined' && grdRegistro != null)
                        grdRegistro.AddNewRow();
                }
            </script>

            <asp:Panel runat="server" ID="pnlFiltro" GroupingText="Informe os dados para pesquisar a ocorrência"
                Width="70%">
                <div>
                    <table width="60%">
                        <tr>
                            <td style="text-align: right; width: 15%">
                                <asp:Label Font-Names="Verdana" ID="lblAno" runat="server" Text="Ano:*" SkinID="a"
                                    Font-Bold="true">                                   
                                </asp:Label>
                            </td>
                            <td width="20%">
                                <asp:DropDownList Height="20px" ID="ddlAno" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged"
                                    DataTextField="ano" DataValueField="ano" SkinID="a" Width="100px">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvAnoPesquisa" runat="server" ControlToValidate="ddlAno"
                                    ErrorMessage="Ano: Preenchimento obrigatório." InitialValue="" ValidationGroup="ConfirmarForm"><img 
                                                title="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; width: 15%">
                                <asp:Label ID="lblCoordenadoria" runat="server" Font-Names="Verdana" Text="Regional:"></asp:Label>
                            </td>
                            <td colspan="3">
                                <tweb:TSearchBox ID="tseRegional" runat="server" Argument="descricao" ArgumentColumns="50"
                                    DataType="Number" MaxLength="20" Columns="10" AutoPostBack="True" Caption=""
                                    OnChanged="tseRegional_Changed" Key="id_regional" SqlSelect="select distinct ID_REGIONAL, descricao from (select distinct ue.ID_REGIONAL, n.regional as descricao from VW_UNIDADE_ENSINO_SITUACAO uuf
                                join LY_UNIDADE_ENSINO ue on uuf.UNIDADE_ENS = ue.UNIDADE_ENS
                                join TCE_REGIONAL n on n.ID_REGIONAL = ue.ID_REGIONAL) as tabela" SqlOrder="descricao, id_regional">
                                    <GridColumns>
                                        <tweb:TSearchBoxColumn Caption="Código" FieldName="id_regional" Width="20%" />
                                        <tweb:TSearchBoxColumn Caption="Regional" FieldName="descricao" Width="80%" />
                                    </GridColumns>
                                </tweb:TSearchBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; width: 15%">
                                <asp:Label Font-Names="Verdana" ID="lblMunicipio" runat="server" Text="Município:"></asp:Label>
                            </td>
                            <td colspan="3">
                                <tweb:TSearchBox ID="tseMunicipio" runat="server" SqlOrder="nome" SqlSelect=" select distinct codigo, nome, uf_sigla from VW_ZZCRO_UNIDADE_ENSINO u join municipio m on u.municipio = m.CODIGO "
                                    SqlWhere=" id_regional = #tseRegional# " GridWidth="600px" ArgumentColumns="50"
                                    OnChanged="tseMunicipio_Changed" Columns="10" MaxLength="10">
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
                                <asp:Label Font-Names="Verdana" ID="lblUnidadeResponsavel" runat="server" Text="Unidade Ensino:*"
                                    SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td width="20%">
                                <tweb:TSearchBox ID="tseUnidadeResponsavel" runat="server" Caption="" Key="unidade_ens"
                                    MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao, nucleo, municipio,id_regional, ua_atual, bairro, ua_antiga    from VW_UNIDADE_ENSINO_SITUACAO "
                                    SqlWhere=" id_regional = #tseRegional# AND municipio = #tseMunicipio# " GridWidth="850px"
                                    OnChanged="tseUnidadeResponsavel_Changed" SqlOrder="nome_comp">
                                    <GridColumns>
                                        <tweb:TSearchBoxColumn Caption="Censo" FieldName="unidade_ens" Width="12%" />
                                        <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="20%" />
                                        <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="20%" />
                                        <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="20%" />
                                        <tweb:TSearchBoxColumn Caption="CGC" FieldName="cgc" Width="10%" />
                                        <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                                        <tweb:TSearchBoxColumn Caption="Bairro" FieldName="bairro" Width="18%" />
                                        <tweb:TSearchBoxColumn Caption="id_regional" FieldName="id_regional" Width="18%"
                                            Visible="false" />
                                    </GridColumns>
                                </tweb:TSearchBox>
                                <asp:RequiredFieldValidator ID="rfvUnidadeResponsavelPesquisa" runat="server" ControlToValidate="tseUnidadeResponsavel"
                                    Display="Dynamic" ErrorMessage="Unidade de Ensino: Preenchimento obrigatório."
                                    InitialValue="" ValidationGroup="ConfirmarForm"><img title="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                            </td>
                            <td style="text-align: right; width: 15%">
                                <asp:Label ID="lblUA" runat="server" SkinID="lblObrigatorio" Text="U.A.:" Font-Names="Verdana"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblUAValor" runat="server" SkinID="lblObrigatorio" Font-Names="Verdana"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; width: 15%">
                            </td>
                            <td colspan="3">
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; width: 15%">
                                &nbsp;
                            </td>
                            <td colspan="3">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
            <br />
            <br />
            <table>
                <tr>
                    <td align="left" colspan="4">
                        <asp:Label ID="lblMensagem" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <br />
            <dxwgv:ASPxGridView ID="grdRegistro" runat="server" KeyFieldName="OCORRENCIAID" ClientInstanceName="grdRegistro"
                AutoGenerateColumns="False" OnAfterPerformCallback="grdRegistro_AfterPerformCallback"
                OnHtmlDataCellPrepared="grdRegistro_HtmlDataCellPrepared" OnCustomButtonCallback="grdRegistro_CustomButtonCallback"
                OnRowDeleting="grdRegistro_RowDeleting" Width="70%" SkinID="NoConfirmDelete"
                OnCommandButtonInitialize="grdRegistro_CommandButtonInitialize">
                <SettingsBehavior AllowMultiSelection="False" ProcessSelectionChangedOnServer="true" />
                <SettingsText EmptyDataRow="Não existem dados." />
                <ClientSideEvents CustomButtonClick="function(s,e) { 
                    if(e.buttonID == 'btnDesativar') { e.processOnServer = confirm('Tem certeza que deseja desativar uma ocorrência?'); return; }
                    else if(e.buttonID == 'btnReativar') { e.processOnServer = confirm('Tem certeza que deseja reativar a ocorrência?'); return; }}" />
                <Styles CommandColumn-Wrap="False" />
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
                        <CustomButtons>
                            <dxwgv:GridViewCommandColumnCustomButton Text="Desativar" ID="btnDesativar" Visibility="Invisible"
                                Image-Url="../App_Themes/Blue/Editors/fcgroupremove.png" Image-Height="15px"
                                Image-AlternateText="Desativar">
                            </dxwgv:GridViewCommandColumnCustomButton>
                        </CustomButtons>
                    </dxwgv:GridViewCommandColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="OCORRENCIAID" VisibleIndex="3" Caption="Id"
                        Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="CENSO" VisibleIndex="1" Caption="Censo"
                        CellStyle-HorizontalAlign="Center" Width="30" Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataColumn Caption="Data da Ocorrência" FieldName="DATAOCORRENCIA"
                        VisibleIndex="2" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                        Width="40">
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="SUBCLASSE" VisibleIndex="3" Caption="SubClasse"
                        Visible="true" Width="100" CellStyle-HorizontalAlign="Justify" HeaderStyle-HorizontalAlign="Center">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="CLASSE" VisibleIndex="3" Caption="Classe"
                        Visible="true" Width="100" CellStyle-HorizontalAlign="Justify" HeaderStyle-HorizontalAlign="Center">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="BATALHAO" VisibleIndex="5" Caption="Batalhão"
                        Visible="true" Width="100" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="DELEGACIA" VisibleIndex="6" Caption="Delegacia"
                        CellStyle-HorizontalAlign="Center" Width="100" HeaderStyle-HorizontalAlign="Center">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="SITUACAO" VisibleIndex="7" Caption="Situação"
                        CellStyle-HorizontalAlign="Center" Width="100" HeaderStyle-HorizontalAlign="Center">
                    </dxwgv:GridViewDataTextColumn>
                </Columns>
                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
            </dxwgv:ASPxGridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
