<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ListarTurma.aspx.cs" Inherits="Techne.Lyceum.Net.Curriculo.ListarTurma" %>

<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
</asp:Content>
<asp:Content ID="cListarTurma" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:ScriptManagerProxy ID="scriptManager" runat="server" />
    <asp:UpdatePanel ID="upnlTurmas" UpdateMode="Always" runat="server" RenderMode="Block">
        <ContentTemplate>

            <script type="text/javascript">
                function Novo() {
                    if (typeof (grdGradeSerie) != 'undefined' && grdGradeSerie != null)
                        grdGradeSerie.AddNewRow();
                }
            </script>

            <asp:Panel runat="server" ID="pnlFiltro" GroupingText="Informe os dados para pesquisar turma">
                <div>
                    <table width="80%">
                        <tr>
                            <td align="left" colspan="4">
                                <asp:Label ID="lblMensagem" runat="server"></asp:Label>
                            </td>
                        </tr>
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
                            <td style="text-align: right">
                                <asp:Label Font-Names="Verdana" ID="lblSitTurma" runat="server" Text="Sit. Turma:*"
                                    SkinID="lblObrigatorio" />
                            </td>
                            <td>
                                <asp:DropDownList Height="20px" ID="ddlSitTurma" runat="server" OnSelectedIndexChanged="ddlSitTurma_SelectedIndexChanged"
                                    AutoPostBack="true">
                                    <asp:ListItem Text="Aberta" Value="Aberta" Selected="True" />
                                    <asp:ListItem Text="Desativada" Value="Desativada" Selected="False" />
                                    <asp:ListItem Text="Finalizada" Value="Finalizada" Selected="False" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; width: 15%">
                                <asp:Label ID="lblCoordenadoria" runat="server" Font-Names="Verdana" Text="Regional:"></asp:Label>
                            </td>
                            <td >
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
                              <td style="text-align: right">
                                <asp:Label Font-Names="Verdana" ID="Label1" runat="server" Text="Tipo Turma:*"
                                    SkinID="lblObrigatorio" />
                            </td>
                            <td>
                                <asp:DropDownList Height="20px" ID="ddlTipoTurma" runat="server" OnSelectedIndexChanged="ddlTipoTurma_SelectedIndexChanged"
                                    AutoPostBack="true">
                                    <asp:ListItem Text="Principais" Value="Principais" Selected="True" />
                                    <asp:ListItem Text="Alternativas" Value="Alternativas" Selected="False" />                                    
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; width: 15%">
                                <asp:Label Font-Names="Verdana" ID="lblMunicipio" runat="server" Text="Município:"></asp:Label>
                            </td>
                            <td >
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
                              <td style="text-align: right">
                                <asp:Label Font-Names="Verdana" ID="Label2" runat="server" Text="Situação de Funcionamento:*"
                                    SkinID="lblObrigatorio" />
                            </td>
                            <td>
                                <asp:DropDownList Height="20px" ID="ddlSituacaoFunc" runat="server" OnSelectedIndexChanged="ddlTipoTurma_SelectedIndexChanged"
                                    AutoPostBack="true"  DataTextField="descr" DataValueField="item">
                                                                  
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; width: 15%">
                                <asp:Label Font-Names="Verdana" ID="lblUnidadeResponsavel" runat="server" Text="Unidade Ensino:*"
                                    SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td width="20%">
                                <tweb:TSearchBox ID="tseUnidadeResponsavel" runat="server" Caption="" Key="unidade_ens"
                                    MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao, nucleo, municipio,id_regional, ua_atual,ua_antiga  from VW_UNIDADE_ENSINO_SITUACAO "
                                    SqlWhere=" id_regional = #tseRegional# AND municipio = #tseMunicipio# " GridWidth="850px"
                                    OnChanged="tseUnidadeResponsavel_Changed" SqlOrder="nome_comp">
                                    <GridColumns>
                                        <tweb:TSearchBoxColumn Caption="Censo" FieldName="unidade_ens" Width="12%" />
                                        <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="20%" />
                                        <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="20%" />
                                        <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="20%" />
                                        <tweb:TSearchBoxColumn Caption="CGC" FieldName="cgc" Width="10%" />
                                        <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
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
                            <td colspan="4" align="right">
                                <%-- <asp:ImageButton ID="btnPesquisar" runat="server" ValidationGroup="ConfirmarForm"
                            ImageUrl="~/Images/bot_buscar.png" OnClick="btnPesquisar_Click" />--%>
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
            <dxwgv:ASPxGridView ID="grdGradeSerie" runat="server" KeyFieldName="grade_id" ClientInstanceName="grdGradeSerie"
                AutoGenerateColumns="False" OnAfterPerformCallback="grdGradeSerie_AfterPerformCallback"
                OnHtmlDataCellPrepared="grdGradeSerie_HtmlDataCellPrepared" OnCustomButtonCallback="grdGradeSerie_CustomButtonCallback"
                OnRowDeleting="grdGradeSerie_RowDeleting" Width="90%" SkinID="NoConfirmDelete">
                <SettingsBehavior AllowMultiSelection="False" ProcessSelectionChangedOnServer="true" />
                <SettingsText EmptyDataRow="Não existem dados." />
                <ClientSideEvents CustomButtonClick="function(s,e) { 
                    if(e.buttonID == 'btnDesativar') { e.processOnServer = confirm('Tem certeza que deseja desativar a turma?'); return; }
                    else if(e.buttonID == 'btnReativar') { e.processOnServer = confirm('Tem certeza que deseja reativar a turma?'); return; }}" />
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
                        <DeleteButton Text="Remover" Visible="True">
                            <Image Url="~/img/bt_exclui2.png" />
                        </DeleteButton>
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
                            <dxwgv:GridViewCommandColumnCustomButton Text="Desativar" ID="btnDesativar" Visibility="AllDataRows"
                                Image-Url="../App_Themes/Blue/Editors/fcgroupremove.png" Image-Height="15px"
                                Image-AlternateText="Desativar">
                            </dxwgv:GridViewCommandColumnCustomButton>
                        </CustomButtons>
                        <CustomButtons>
                            <dxwgv:GridViewCommandColumnCustomButton Text="Reativar" ID="btnReativar" Visibility="AllDataRows"
                                Image-Url="../img/bt_cancelar.png" Image-Height="15px" Image-AlternateText="Reativar">
                            </dxwgv:GridViewCommandColumnCustomButton>
                        </CustomButtons>
                    </dxwgv:GridViewCommandColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="ano" VisibleIndex="1" Caption="Ano" CellStyle-HorizontalAlign="Center"
                        Width="30">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="semestre" VisibleIndex="2" Caption="Período"
                        CellStyle-HorizontalAlign="Center" Width="30">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="curso" VisibleIndex="3" Caption="Curso"
                        Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="nomeCurso" VisibleIndex="4" Caption="Escolaridade"
                        CellStyle-HorizontalAlign="Center" Width="100">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="turno" VisibleIndex="5" Caption="Turno"
                        Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="descricaoTurno" VisibleIndex="6" Caption="Turno"
                        CellStyle-HorizontalAlign="Center" Width="40">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="serie" VisibleIndex="7" Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="descricaoSerie" VisibleIndex="8" Caption="Ano de Escolaridade"
                        CellStyle-HorizontalAlign="Center" Width="40">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="grade_token" VisibleIndex="9" Caption="Turma"
                        CellStyle-HorizontalAlign="Center" Width="80">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="unidade_responsavel" VisibleIndex="10" Caption="Unidade Ensino"
                        Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="nomeUnidadeResponsavel" VisibleIndex="11"
                        Caption="Unidade Ensino" CellStyle-HorizontalAlign="Center" Width="150">
                    </dxwgv:GridViewDataTextColumn>
                    
                    <dxwgv:GridViewDataTextColumn FieldName="dependencia" VisibleIndex="13" Caption="Sala de Aula"
                        CellStyle-HorizontalAlign="Center" Width="40">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="curriculo" VisibleIndex="14" Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="faculdade" VisibleIndex="15" Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="em_elaboracao" VisibleIndex="16" Caption="Turma Sem Alocação"
                        CellStyle-HorizontalAlign="Center" Width="100">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="grade_id" VisibleIndex="17" Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="sufixo" VisibleIndex="18" Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="tipo_gestao" VisibleIndex="19" Caption="Tipo de Gestão"
                        CellStyle-HorizontalAlign="Center" Width="100">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="tipo_alternativa" VisibleIndex="20" Caption="Tipo Alternativa"
                        CellStyle-HorizontalAlign="Center" Width="100">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="turmareferencia" VisibleIndex="20" Caption="Turma Referência"
                        CellStyle-HorizontalAlign="Center" Width="100">
                    </dxwgv:GridViewDataTextColumn>    
                    <dxwgv:GridViewDataTextColumn FieldName="capacidadeSala" VisibleIndex="20" Caption="Capacidade da Sala"
                        CellStyle-HorizontalAlign="Center" Width="30">
                    </dxwgv:GridViewDataTextColumn>
                     <dxwgv:GridViewDataTextColumn FieldName="num_alunos" VisibleIndex="20" Caption="Número Máximo Alunos"
                        CellStyle-HorizontalAlign="Center" Width="100">
                    </dxwgv:GridViewDataTextColumn>                 
                    <dxwgv:GridViewDataTextColumn FieldName="matriculadosprincipal" VisibleIndex="21" Caption="Alunos Matriculados"
                        CellStyle-HorizontalAlign="Center" Width="100">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="matriculadoseletivas" VisibleIndex="22" Caption="Alunos Matriculados Eletivas"
                        CellStyle-HorizontalAlign="Center" Width="100">
                    </dxwgv:GridViewDataTextColumn>
                    
                </Columns>
                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
            </dxwgv:ASPxGridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
