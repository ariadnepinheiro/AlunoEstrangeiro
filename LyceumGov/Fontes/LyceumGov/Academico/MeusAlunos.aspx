<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="MeusAlunos.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.MeusAlunos" %>

<asp:Content ID="ctMeusAlunos" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        function SelChanged(s, e) {
            if (e.isSelected)
                grdMeusAlunos.GetRowValues(e.visibleIndex, 'aluno', OnGridSelectionComplete);
        }

        function OnGridSelectionComplete(values) {
            var str = 'aluno=' + values;
            str = Base64.encode(str);
            window.location = 'Alunos.aspx?Chave=' + str;
        }
    </script>

    <asp:Panel ID="pnBusca" runat="server" GroupingText="Escolha o tipo de busca que deseja"
        Width="950px">
        <table width="100%">
            <tr>
                <td width="40px" align="center">
                    <asp:RadioButton ID="rbPrimeiraOpcao" GroupName="Busca" runat="server" Text=" " AutoPostBack="True"
                        OnCheckedChanged="rbPrimeiraOpcao_CheckedChanged" />
                </td>
                <td>
                    <table>
                        <tr>
                            <td align="right" width="114px">
                                <asp:Label ID="lblAluno" Text="Aluno:* " SkinID="lblObrigatorio" runat="server"></asp:Label>
                            </td>
                            <td>
                                <tweb:TSearch ID="tseAluno" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAluno"
                                    AutoPostBack="true" OnTextChanged="tseAluno_Changed">
                                    <QueryParameters>
                                        <asp:Parameter Name="ativo" DefaultValue="Ativo" DbType="String" />
                                    </QueryParameters>
                                </tweb:TSearch>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br />
        <div style="height: 1px; width: 100%; border-bottom: inset 1px Blue;">
        </div>
        <br />
        <table width="100%">
            <tr>
                <td width="40px" align="center">
                    <asp:RadioButton ID="rbSegundaOpcao" Checked="true" GroupName="Busca" runat="server"
                        Text=" " AutoPostBack="True" OnCheckedChanged="rbSegundaOpcao_CheckedChanged" />
                </td>
                <td>
                    <table>
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblCoordenadoria" Text="Regional:" runat="server"></asp:Label>
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
                            <td align="right">
                                <asp:Label ID="lblUnidadeEnsino" Text="Unidade de Ensino:" runat="server"></asp:Label>
                            </td>
                            <td>
                                <tweb:TSearchBox ID="tseUnidadeEnsino" runat="server" Caption="" Key="unidade_ens"
                                    OnChanged="tseUnidadeEnsino_Changed" Argument="nome_comp" ColumnName="Faculdade"
                                    SqlSelect="SELECT unidade_ens, nome_comp, setor, cgc, situacao,ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO"
                                    MaxLength="20" FieldName="Unidade de Ensino" GridWidth="850px" SqlOrder="nome_comp">
                                    <GridColumns>
                                        <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="12%" />
                                        <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="30%" />                                        
                                        <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="30%" />
                                        <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="30%" />
                                        <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="15%" />
                                        <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                                    </GridColumns>
                                </tweb:TSearchBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblTCurso" Text="Escolaridade:" runat="server"></asp:Label>
                            </td>
                            <td>
                                <tweb:TSearchBox ID="tseCurso" runat="server" Argument="nome" Caption="" Key="curso"
                                    SqlSelect="SELECT curso, nome FROM ly_curso" OnChanged="tseCurso_Changed" SqlOrder="nome">
                                    <GridColumns>
                                        <tweb:TSearchBoxColumn Caption="Código" FieldName="curso" Width="20%" />
                                        <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome" Width="80%" />
                                    </GridColumns>
                                </tweb:TSearchBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblTurno" Text="Turno:" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlTurno" OnSelectedIndexChanged="ddlTurno_SelectedIndexChanged"
                                    runat="server" AutoPostBack="true" Width="200px" DataTextField="descricao" DataValueField="turno">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblAnoEscolar" Text="Ano Escolar:" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSerie" OnSelectedIndexChanged="ddlSerie_SelectedIndexChanged"
                                    AutoPostBack="true" runat="server" Width="200px" DataTextField="descricao" DataValueField="serie">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; width: 15%">
                                <asp:Label Font-Names="Verdana" ID="lblTurmaTse" runat="server" Text="Turma:"></asp:Label>
                            </td>
                            <td>
                                <tweb:TSearchBox ID="tseTurma" runat="server" Caption="" Key="grade_id" DataType="Number"
                                    MaxLength="50" Argument="grade" AutoPostBack="false" SqlSelect=" SELECT DISTINCT GS.grade_id as grade_id, GS.grade as grade, GS.ANO as ano FROM LY_GRADE_SERIE  GS inner join ly_unidade_ensino UE ON UE.unidade_ens = GS.faculdade AND UE.UNIDADE_ENS = GS.UNIDADE_RESPONSAVEL inner join TCE_REGIONAL N ON N.id_regional = UE.id_regional inner join ly_turno T ON T.turno = GS.turno inner join ly_serie S ON S.SERIE = GS.SERIE AND S.TURNO = GS.TURNO AND S.CURRICULO = GS.CURRICULO inner join LY_CURSO C ON C.CURSO = GS.CURSO inner join LY_TURMA TU ON TU.TURMA = GS.GRADE AND TU.ANO = GS.ANO AND TU.SEMESTRE = GS.SEMESTRE "
                                    SqlWhere=" (UE.ID_REGIONAL = #tseRegional# or #tseRegional# is null) AND (GS.UNIDADE_RESPONSAVEL = #tseUnidadeEnsino# or #tseUnidadeEnsino# is null) AND (GS.CURSO = #tseCurso# or #tseCurso# is null)"
                                    GridWidth="850px" SqlOrder="ANO DESC, GRADE ASC">
                                    <GridColumns>
                                        <tweb:TSearchBoxColumn Caption="Código" FieldName="GRADE_ID" Width="0%" Visible="false" />
                                        <tweb:TSearchBoxColumn Caption="Turma" FieldName="GRADE" Width="50%" />
                                        <tweb:TSearchBoxColumn Caption="Ano" FieldName="ANO" Width="50%" />
                                    </GridColumns>
                                </tweb:TSearchBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right">
                                <asp:ImageButton ID="btnPesquisar" runat="server" ValidationGroup="Buscar" ImageUrl="~/Images/bot_buscar.png"
                                    OnClick="btnPesquisar_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <dxwgv:ASPxGridView ID="grdMeusAlunos" ClientInstanceName="grdMeusAlunos" AutoGenerateColumns="False"
        Visible="true" Width="950px" EnableViewState="false" runat="server" KeyFieldName="aluno"
        OnAfterPerformCallback="grdMeusAlunos_AfterPerformCallback">
        <SettingsBehavior AllowMultiSelection="False" AllowFocusedRow="false" />
        <SettingsCookies Enabled="false" />
        <Columns>
            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                <ClearFilterButton Text="Limpar" Visible="True">
                    <Image Url="~/img/bt_limpa.png" />
                </ClearFilterButton>
                <SelectButton Text="Selecionar" Visible="True">
                    <Image Url="../img/bt_copiar.png" />
                </SelectButton>
            </dxwgv:GridViewCommandColumn>
            <dxwgv:GridViewDataTextColumn Caption="Matrícula" FieldName="aluno" VisibleIndex="1">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Nome" FieldName="nome_compl" VisibleIndex="2">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Cód. Escolaridade" FieldName="curso" VisibleIndex="3">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Escolaridade" FieldName="nome_curso" VisibleIndex="4">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Cód. Turno" FieldName="turno" VisibleIndex="5">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Turno" FieldName="descricao_turno" VisibleIndex="6">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Cód. Ano Escolar" FieldName="serie" VisibleIndex="7">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Ano Escolar" FieldName="descricao_serie" VisibleIndex="8">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Cód. Unid. Ensino" FieldName="unidade_ensino"
                VisibleIndex="9">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Unidade Ensino" FieldName="nome_unidadeensino"
                VisibleIndex="10">
            </dxwgv:GridViewDataTextColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
        <ClientSideEvents SelectionChanged="function(s, e) { SelChanged(s, e); }" />
    </dxwgv:ASPxGridView>
</asp:Content>
