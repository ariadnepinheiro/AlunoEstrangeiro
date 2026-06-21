<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="FechamentoMatriculaLista.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.FechamentoMatriculaLista" %>

<asp:Content ID="cListarTurma" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel runat="server" ID="pnlFiltro" GroupingText="Informe os dados abaixo para pesquisar a turma"
        Width="700">
        <table>
            <tr>
                <td align="left" colspan="4">
                    <asp:Label ID="lblMensagem" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label Font-Names="Verdana" ID="lblAno" runat="server" Text="Ano:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlAno" runat="server" CssClass="ReadOnlyField" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlAno_SelectedIndexChanged" DataTextField="ano" DataValueField="ano"
                        Width="100px">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvAnoPesquisa" runat="server" ControlToValidate="ddlAno"
                        ErrorMessage="Ano: Preenchimento obrigatório." InitialValue="" ValidationGroup="ConfirmarForm"><img 
                                                title="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                </td>
                <td style="text-align: right">
                    <asp:Label ID="lblPeriodo" runat="server" Font-Names="Verdana" Text="Período:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlPeriodo" runat="server" DataTextField="id_reduzida" AutoPostBack="False"
                        DataValueField="periodo" Width="100px">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvPeriodoPesquisa" runat="server" ControlToValidate="ddlPeriodo"
                        ErrorMessage="Período: Preenchimento obrigatório." InitialValue="-1" ValidationGroup="ConfirmarForm"><img 
                                                title="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label Font-Names="Verdana" ID="lblUnidadeResponsavel" runat="server" Text="Unidade de Ensino:*"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUnidadeResponsavel" runat="server" Caption="" Key="unidade_ens"
                        MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao,ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO "
                        GridWidth="850px" OnChanged="tseUnidadeResponsavel_Changed" SqlOrder="nome_comp">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="12%" />
                            <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="30%" />                            
                            <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                    <asp:RequiredFieldValidator ID="rfvUnidadeResponsavelPesquisa" runat="server" ControlToValidate="tseUnidadeResponsavel"
                        Display="Dynamic" ErrorMessage="Unidade de Ensino: Preenchimento obrigatório."
                        InitialValue="" ValidationGroup="ConfirmarForm"><img title="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                </td>
                <td style="text-align: right">
                    <asp:Label ID="lblUA" runat="server" SkinID="lblObrigatorio" Text="U.A.:" Font-Names="Verdana"></asp:Label>
                </td>
                <td align="left">
                    <asp:Label ID="lblUAValor" runat="server" SkinID="lblObrigatorio" Font-Names="Verdana"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblCurso" runat="server" Text="Escolaridade: "></asp:Label>
                </td>
                <td colspan="3">
                    <tweb:TSearchBox ID="tseCurso" runat="server" Caption="" SqlSelect="SELECT distinct uec.curso as curso, nome FROM LY_CURSO c inner join LY_UNIDADE_ENSINO_CURSOS uec on (c.CURSO = uec.CURSO)"
                        ArgumentColumns="60" Columns="10" MaxLength="20" SqlWhere="unidade_ens = isnull(#tseUnidadeResponsavel#,'')"
                        SqlOrder="nome" GridWidth="800px">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="curso" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome" Width="70%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblTurno" runat="server" Text="Turno: "></asp:Label>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddlTurno" DataTextField="descricao" DataValueField="turno"
                        AutoPostBack="false" DataSourceID="odsTurno" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="right">
                    <asp:ImageButton ID="btnPesquisar" runat="server" ValidationGroup="ConfirmarForm"
                        ImageUrl="~/Images/bot_buscar.png" OnClick="btnPesquisar_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <dxwgv:ASPxGridView ID="grdGradeSerie" runat="server" KeyFieldName="grade_id" ClientInstanceName="grdGradeSerie"
        AutoGenerateColumns="False" OnAfterPerformCallback="grdGradeSerie_AfterPerformCallback"
        Width="90%" OnSelectionChanged="grdGradeSerie_SelectionChanged" SkinID="NoConfirmDelete">
        <SettingsBehavior AllowMultiSelection="False" ProcessSelectionChangedOnServer="True" />
        <SettingsText EmptyDataRow="Não existem dados." />
        <Columns>
            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="7%">
                <SelectButton Text="Selecionar" Visible="True">
                    <Image Url="~/img/bt_busca.png" />
                </SelectButton>
                <ClearFilterButton Text="Limpar" Visible="True">
                    <Image Url="~/img/bt_limpa.png" />
                </ClearFilterButton>
            </dxwgv:GridViewCommandColumn>
            <dxwgv:GridViewDataTextColumn FieldName="ano" VisibleIndex="1" Caption="Ano" Width="5%">
                <PropertiesTextEdit MaxLength="5">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn FieldName="semestre" VisibleIndex="2" Caption="Período"
                CellStyle-HorizontalAlign="Center">
                <PropertiesTextEdit MaxLength="5">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn FieldName="curso" VisibleIndex="3" Caption="Escolaridade"
                Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn FieldName="nomeCurso" VisibleIndex="4" Caption="Escolaridade">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn FieldName="turno" VisibleIndex="5" Caption="Turno"
                Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn FieldName="descricaoTurno" VisibleIndex="6" Caption="Turno">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn FieldName="serie" VisibleIndex="7" Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn FieldName="descricaoSerie" VisibleIndex="8" Caption="Ano de Escolaridade">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn FieldName="grade" VisibleIndex="9" Caption="Turma"
                Width="70px">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn FieldName="unidade_responsavel" VisibleIndex="10" Caption="Unidade de Ensino"
                Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn FieldName="nomeUnidadeResponsavel" VisibleIndex="11"
                Caption="Unidade de Ensino">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn FieldName="capacidade" VisibleIndex="12" Caption="Capacidade">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn FieldName="dependencia" VisibleIndex="13" Caption="Sala de Aula">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn FieldName="curriculo" VisibleIndex="14" Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn FieldName="faculdade" VisibleIndex="15" Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn FieldName="sit_turma" VisibleIndex="16" Caption="Situação Turma"
                CellStyle-HorizontalAlign="Center" Width="100">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn FieldName="optativareforco" VisibleIndex="16" Caption="Optativa/Reforço"
                CellStyle-HorizontalAlign="Center" Width="100" Visible = "false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn FieldName="eletiva" VisibleIndex="16" Caption="Eletiva"
                CellStyle-HorizontalAlign="Center" Width="100" Visible = "false">
            </dxwgv:GridViewDataTextColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
    <asp:ObjectDataSource ID="odsTurno" SelectMethod="ComboConsultar" TypeName="Techne.Lyceum.RN.Turno"
        runat="server"></asp:ObjectDataSource>
</asp:Content>
