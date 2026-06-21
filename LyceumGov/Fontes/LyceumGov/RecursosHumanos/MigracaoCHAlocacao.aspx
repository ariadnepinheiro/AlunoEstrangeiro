<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="MigracaoCHAlocacao.aspx.cs" Inherits="Techne.Lyceum.Net.RecursosHumanos.MigracaoCHAlocacao" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%-- <script type="text/javascript">
        $(document).ready(function() {
            AtualizarLabelQtdSelecionado();

        });
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnlBusca" runat="server" GroupingText="Informe a Id/Vínculo ou o nome do docente e o ano das alocações"
        Height="65px" Width="710px">
        <asp:ObjectDataSource ID="odsAno" TypeName="Techne.Lyceum.RN.PeriodoLetivo" SelectMethod="ConsultarProximosAnos"
            runat="server" />
        <table>
            <tr>
                <td align="right">
                    <asp:Label ID="lblBuscaConcurso" runat="server" Text="Processo Seletivo:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseConcursoBusca" runat="server" Caption="" SqlSelect="SELECT distinct concurso, descricao, indigena, ano FROM lY_concurso_docente"
                        ArgumentColumns="50" Columns="30" MaxLength="20" SqlWhere="TIPO = 'Migracao' "
                        SqlOrder=" ano desc" GridWidth="800px" OnChanged="tseConcursoBusca_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Processo Seletivo" FieldName="concurso" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="70%" />
                            <tweb:TSearchBoxColumn Caption="Indigena" FieldName="indigena" Visible="false" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblDocenteTSearch" runat="server" Text="Id/Vínculo do Docente*: "
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseDocente" MaxLength="20" runat="server" ValidateText="true" SettingsTypeName="Techne.Lyceum.RN.Query.QueryCandidatoDocenteMigracao"
                        AutoPostBack="true" OnChanged="tseDocente_Changed" OnTextChanged="tseDocente_Changed">
                        <QueryParameters>
                            <asp:ControlParameter Name="Concurso" ControlID="tseConcursoBusca" PropertyName="DBValue" />
                        </QueryParameters>
                    </tweb:TSearch>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblAno" Text="Ano Letivo:" runat="server" SkinID="lblObrigatorio" />
                </td>
                <td>
                    <asp:DropDownList ID="ddlAno" AutoPostBack="true" DataSourceID="odsAno" DataTextField="ano"
                        DataValueField="ano" runat="server" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:Label ID="lblMensagem" SkinID="lblMensagem" runat="server" />
    <br />
    <br />
    <asp:Panel ID="pnlDados" runat="server">
        <asp:Panel ID="pnlDocente" runat="server" GroupingText="Dados do docente" Width="784px">
            <table style="width: 100%">
                <tr>
                    <td align="right">
                        <asp:Label ID="lblNome" Text="Nome:" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtNome" runat="server" ReadOnly="true" Width="100%" />
                    </td>
                    <td align="right">
                        <asp:Label ID="lblCPF" Text="CPF:" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtCPF" runat="server" ReadOnly="true" Width="100%" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblMatricula" Text="Matricula:" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtMatricula" runat="server" ReadOnly="true" Width="100%" />
                        <asp:HiddenField ID="hdnNumFunc" runat="server" />
                    </td>
                    <td align="right">
                        <asp:Label ID="lblIDVinculo" Text="Id/Vinculo:" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtIDVinculo" runat="server" ReadOnly="true" Width="100%" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblCargo" Text="Cargo:" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtCargo" runat="server" ReadOnly="true" Width="100%" />
                    </td>
                    <td align="right">
                        <asp:Label ID="lblFuncao" Text="Função:" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtFuncao" runat="server" ReadOnly="true" Width="100%" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="Label4" Text="Situação:" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtSituacao" runat="server" ReadOnly="true" Width="100%" />
                    </td>
                    <td align="right">
                        <asp:Label ID="Label5" Text="Readaptado:" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtReadaptado" runat="server" ReadOnly="true" Width="100%" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblDisciplinaIngresso" Text="Disciplina de ingresso:" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtDisciplinaIngresso" ReadOnly="true" runat="server" Width="100%" />
                    </td>
                    <td align="right">
                        <asp:Label ID="lblSegundaMatricula" Text="Segunda Matrícula:" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtSegundaMatricula" runat="server" ReadOnly="true" Width="100%" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblCHIngresso" Text="CH de regência:" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtCHIngresso" runat="server" ReadOnly="true" Width="100%" />
                    </td>
                    <td align="right">
                        <asp:Label ID="lblCHTurma" Text="CH em turma:" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtCHTurma" runat="server" ReadOnly="true" Width="100%" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblSituacao" Text="CH Normal:" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtCHNormal" runat="server" ReadOnly="true" Width="100%" />
                    </td>
                    <td align="right">
                        <asp:Label ID="Label6" Text="CH GLP:" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtCHGLP" runat="server" ReadOnly="true" Width="100%" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="Label7" Text="Regional:" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtRegional" runat="server" ReadOnly="true" Width="100%" />
                    </td>
                    <td align="right">
                        <asp:Label ID="Label8" Text="Município:" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtMunicipio" runat="server" ReadOnly="true" Width="100%" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="Label9" Text="UA de Lotação:" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtUALotacao" runat="server" ReadOnly="true" Width="100%" />
                    </td>
                    <td align="right">
                        <asp:Label ID="Label10" Text="Unidade Adminsitrativa:" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtUANome" runat="server" ReadOnly="true" Width="100%" />
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hdnCenso" runat="server" />
            <asp:HiddenField ID="hdnFuncaoNaoRegenteComGLP" runat="server" />
        </asp:Panel>
        <br />
        <asp:Panel ID="pnlFuncao" runat="server" GroupingText="Função e Cargo" Width="784px">
            <table style="width: 100%">
                <tr>
                    <td align="right">
                        <asp:Label ID="Label3" Text="Cargo:*" runat="server" SkinID="lblObrigatorio" />
                    </td>
                    <td>
                        <tweb:TSearchBox ID="tseCargo" runat="server" Argument="nome" ArgumentColumns="70"
                            FollowContainerMode="false" MaxLength="20" Columns="10" DataType="VarChar" Key="categoria"
                            SqlOrder="nome" SqlSelect="Select distinct categoria,nome from RecursosHumanos.VW_CATEGORIA_DOCENTE ">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="categoria" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome" Width="80%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="Label1" Text="Data D.O. Convocação/Migração:*" runat="server" SkinID="lblObrigatorio" />
                    </td>
                    <td>
                        <dxe:ASPxDateEdit ID="dtConvocacao" runat="server" MinDate="01/01/1900" CalendarProperties-ClearButtonText="Limpar"
                            CalendarProperties-TodayButtonText="Hoje">
                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                            </CalendarProperties>
                        </dxe:ASPxDateEdit>
                    </td>
                </tr>
            </table>
            <br />
            <table style="width: 92%" runat="server" id="tbFuncao">
                <tr>
                    <td align="right">
                        <asp:Label ID="Label2" Text="Função:*" runat="server" SkinID="lblObrigatorio" />
                    </td>
                    <td>
                        <tweb:TSearchBox ID="tseFuncaoLotacao" runat="server" Argument="descricao" ArgumentColumns="70"
                            FollowContainerMode="false" MaxLength="20" AutoPostBack="true" Columns="10" DataType="VarChar"
                            Key="funcao" OnChanged="tseFuncaoLotacao_Changed" SqlOrder="descricao" SqlSelect="SELECT DISTINCT F.funcao, F.descricao FROM Ly_funcao F ">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="funcao" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="80%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
            </table>
            <br />
            <table>
                <tr>
                    <td style="width: 50px">
                        <asp:Button ID="btnAlterarCargo" runat="server" ValidationGroup="SalvarForm" Text="Alterar Cargo"
                            OnClick="btnAlterarCargo_Click" OnClientClick="Bloqueio();" />
                    </td>
                </tr>
            </table>
            <br />
        </asp:Panel>
        <br />
        <asp:Panel ID="Panel1" runat="server" GroupingText="Observação" Width="784px">
            <asp:TextBox ID="txtObservacao" runat="server" Width="100%" TextMode="MultiLine"
                MaxLength="500" />
        </asp:Panel>
        <br />
        <br />
        <asp:Panel ID="pnlAulas" runat="server">
            <dxwgv:ASPxGridView ID="grdAulas" DataSourceID="odsAulas" ClientInstanceName="grdAulas"
                runat="server">
                <SettingsPager Mode="ShowAllRecords" />
                <SettingsText EmptyDataRow="O docente não possui alocações ativas." />
                <Columns>
                    <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="0px">
                        <ClearFilterButton Text="Limpar" Visible="True">
                            <Image Url="~/img/bt_limpa.png" />
                        </ClearFilterButton>
                    </dxwgv:GridViewCommandColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Ano" FieldName="ano" Name="ano">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Período" FieldName="semestre" Name="semestre">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Regional" FieldName="regional" Name="regional">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Município" FieldName="municipio_descr" Name="municipio_descr">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Unidade de Ensino" FieldName="ue_descr" Name="ue_descr">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="U.A." FieldName="NOVOSETOR" Name="NOVOSETOR">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Censo" FieldName="censo" Name="censo" Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Turma" FieldName="turma" Name="turma">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Turno" FieldName="turno_descr" Name="turno_descr">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Dia da Aula" FieldName="dia_semana_descr"
                        Name="dia_semana_descr">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Hora de Entrada" FieldName="hora_inicio" Name="hora_inicio">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Hora de Saída" FieldName="hora_fim" Name="hora_fim">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Componente Curricular" FieldName="disciplina_descr"
                        Name="disciplina_descr">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Tipo Aula" FieldName="tipo" Name="tipo">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Tipo GLP" FieldName="tipoglp" Name="tipoglp">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Matrícula" FieldName="matricula" Name="matricula">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="ID/Vínculo" FieldName="idvinculo" Name="idvinculo">
                    </dxwgv:GridViewDataTextColumn>
                </Columns>
                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
            </dxwgv:ASPxGridView>
            <asp:ObjectDataSource ID="odsAulas" runat="server" SelectMethod="ObterAulasDoDocente"
                TypeName="Techne.Lyceum.RN.Turma">
                <SelectParameters>
                    <asp:ControlParameter ControlID="hdnNumFunc" Name="numfunc" PropertyName="Value" />
                    <asp:ControlParameter ControlID="ddlAno" Name="ano" PropertyName="SelectedValue" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <br />
        </asp:Panel>
    </asp:Panel>
    <br />
    <br />
    <asp:Panel ID="pnPrincipal" runat="server" GroupingText="Informe os dados para migração:"
        Visible="false">
        <table>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblRegional" runat="server" Font-Names="Verdana" Text="Regional:"></asp:Label>
                </td>
                <td>
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
                    <asp:Label ID="lblMunicipio" runat="server" Text="Município:"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseMunicipio" runat="server" SqlOrder="nome" SqlSelect=" select distinct codigo, nome, uf_sigla from VW_ZZCRO_UNIDADE_ENSINO u join municipio m on u.municipio = m.CODIGO "
                        GridWidth="600px" SqlWhere=" id_regional = #tseRegional# " ArgumentColumns="50"
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
                    <asp:Label ID="lblUnidadeEnsino" runat="server" Text="Unidade de Ensino:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUnidadeEnsino" runat="server" Key="unidade_ens" Argument="nome_comp"
                        MaxLength="20" SqlSelect="SELECT unidade_ens, nome_comp, setor, cgc, situacao, municipio,id_regional, ua_atual,ua_antiga  from VW_UNIDADE_ENSINO_SITUACAO"
                        GridWidth="850px" SqlOrder="nome_comp" SqlWhere=" municipio = #tseMunicipio# and id_regional = #tseRegional# "
                        OnChanged="tseUnidadeEnsino_Changed" AutoPostBack="true">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome_comp" Width="55%" />
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="15%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label Font-Names="Verdana" ID="lblDisciplina" runat="server" Text="Disciplina:*"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlDisciplina" runat="server" DataTextField="DESCRICAO" DataValueField="AGRUPAMENTO"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlDisciplina_SelectedIndexChanged"
                        onchange="Bloqueio()">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <asp:Panel ID="pnlGridTurmas" Visible="false" runat="server">
            <table>
                <tr>
                    <td>
                        <asp:ObjectDataSource ID="odsTurmaCarenciaContratoGLP" TypeName="Techne.Lyceum.Net.RecursosHumanos.MigracaoCHAlocacao"
                            runat="server" SelectMethod="ListarTurmaCarenciaContratoGLP">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="tseUnidadeEnsino" DefaultValue="" Name="unidadeEnsino"
                                    PropertyName="DBValue" />
                                <asp:ControlParameter ControlID="hdnNumFunc" DefaultValue="" Name="numFunc" PropertyName="Value" />
                                <asp:ControlParameter ControlID="ddlDisciplina" DefaultValue="" Name="agrupamentoDisciplina"
                                    PropertyName="SelectedValue" />
                                <asp:ControlParameter ControlID="ddlAno" DefaultValue="" Name="ano" PropertyName="SelectedValue" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <dxwgv:ASPxGridView ID="grdTurmaCarenciaContratoGLP" runat="server" AutoGenerateColumns="False"
                            ClientInstanceName="grdTurmaCarenciaContratoGLP" DataSourceID="odsTurmaCarenciaContratoGLP"
                            KeyFieldName="CompositeKey" OnAfterPerformCallback="grdTurmaCarenciaContratoGLP_AfterPerformCallback"
                            OnCustomUnboundColumnData="grdTurmaCarenciaContratoGLP_CustomUnboundColumnData"
                            Styles-AlternatingRow-BackColor="#F7C3B5" Styles-Row-BackColor="#F4AD9D" Styles-SelectedRow-BackColor="#A4A4A4">
                            <SettingsPager Mode="ShowAllRecords" />
                            <Columns>
                                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="0px">
                                    <ClearFilterButton Text="Limpar" Visible="True">
                                        <Image Url="~/img/bt_limpa.png" />
                                    </ClearFilterButton>
                                </dxwgv:GridViewCommandColumn>
                                <dxwgv:GridViewCommandColumn ShowSelectCheckbox="true" VisibleIndex="0" />
                                <dxwgv:GridViewDataTextColumn Caption="CompositeKey" FieldName="CompositeKey" Visible="False"
                                    VisibleIndex="0" UnboundType="String">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Regional" FieldName="REGIONAL" ReadOnly="true"
                                    VisibleIndex="1">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="MUNICIPIO" FieldName="MUNICIPIO" ReadOnly="true"
                                    VisibleIndex="2" Visible="False">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Municipio" FieldName="DESCRICAOMUNICIPIO"
                                    ReadOnly="true" VisibleIndex="3">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Censo" FieldName="CENSO" ReadOnly="true" VisibleIndex="4">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="UA" FieldName="UA_ATUAL" ReadOnly="true" VisibleIndex="5">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Escola" FieldName="ESCOLA" ReadOnly="true"
                                    VisibleIndex="6">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Ano" FieldName="ANO" ReadOnly="true" VisibleIndex="7"
                                    Width="50">
                                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Semestre" FieldName="SEMESTRE" ReadOnly="true"
                                    VisibleIndex="8" Width="50">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Turma" FieldName="TURMA" ReadOnly="true" VisibleIndex="9">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Turno" FieldName="TURNO" ReadOnly="true" VisibleIndex="10">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Disciplina" FieldName="DISCIPLINA" ReadOnly="true"
                                    VisibleIndex="11" Width="90" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Componente Curricular" FieldName="NOMEDISCIPLINA"
                                    ReadOnly="true" VisibleIndex="12" Visible="true" Width="90">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Dia da semana" FieldName="DIA_SEMANA" ReadOnly="true"
                                    VisibleIndex="13" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Dia da semana" FieldName="DIA_SEMANA_DESCRICAO"
                                    ReadOnly="true" VisibleIndex="14">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Início" FieldName="HORA_INICIO" ReadOnly="true"
                                    VisibleIndex="15">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Fim" FieldName="HORA_FIM" ReadOnly="true"
                                    VisibleIndex="16">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Aula" FieldName="AULA" ReadOnly="true" VisibleIndex="17"
                                    Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Tipo Cârencia" FieldName="TIPO" ReadOnly="true"
                                    VisibleIndex="18" Width="150">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="DATA_INICIO" FieldName="DATA_INICIO" ReadOnly="true"
                                    VisibleIndex="19" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="DATA_FIM" FieldName="DATA_FIM" ReadOnly="true"
                                    VisibleIndex="20" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="NUM_FUNC_AULA" FieldName="NUM_FUNC_AULA" ReadOnly="true"
                                    VisibleIndex="21" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="TIPO_GESTAO" FieldName="TIPO_GESTAO" ReadOnly="true"
                                    VisibleIndex="22" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="DEPENDENCIA" FieldName="DEPENDENCIA" ReadOnly="true"
                                    VisibleIndex="23" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="CURSO" FieldName="CURSO" ReadOnly="true" VisibleIndex="24"
                                    Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="SERIE" FieldName="SERIE" ReadOnly="true" VisibleIndex="25"
                                    Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="OPTATIVAREFORCO" FieldName="OPTATIVAREFORCO"
                                    ReadOnly="true" VisibleIndex="26" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="CURRICULO" FieldName="CURRICULO" ReadOnly="true"
                                    VisibleIndex="27" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="DISCIPLINA_MULTIPLA" FieldName="DISCIPLINA_MULTIPLA"
                                    ReadOnly="true" VisibleIndex="28" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="TIPODOCENTE" FieldName="TIPODOCENTE" ReadOnly="true"
                                    VisibleIndex="29" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                            </Columns>
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                        </dxwgv:ASPxGridView>
                    </td>
                </tr>
            </table>
            <br />
            <table>
                <tr>
                    <td align="left">
                        <asp:Button ID="btnIncluir" runat="server" ValidationGroup="SalvarForm" Text="Incluir"
                            OnClick="btnIncluir_Click" OnClientClick="Bloqueio();" />
                    </td>
                </tr>
            </table>
            <br />
            <asp:Label ID="lblMensagemMigracao" SkinID="lblMensagem" runat="server" />
            <br />
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblQtdSolicitacoes" runat="server" SkinID="lblMensagem" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <dxwgv:ASPxGridView ID="grdSelecionado" runat="server" AutoGenerateColumns="False"
                            ClientInstanceName="grdSelecionado" KeyFieldName="CompositeKey" OnCustomButtonCallback="grdSelecionado_CustomButtonCallback">
                            <ClientSideEvents EndCallback="function(s, e) { AtualizarLabelQtdSelecionado(); }" />
                            <SettingsPager Mode="ShowAllRecords" />
                            <Columns>
                                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Link" Width="50px" Caption="Excluir">
                                    <CustomButtons>
                                        <dxwgv:GridViewCommandColumnCustomButton ID="btnExcluir" Text="Excluir" Visibility="AllDataRows">
                                        </dxwgv:GridViewCommandColumnCustomButton>
                                    </CustomButtons>
                                </dxwgv:GridViewCommandColumn>
                                <dxwgv:GridViewDataTextColumn Caption="CompositeKey" FieldName="CompositeKey" Visible="False"
                                    VisibleIndex="0" UnboundType="String">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Regional" FieldName="REGIONAL" ReadOnly="true"
                                    VisibleIndex="1">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="MUNICIPIO" FieldName="MUNICIPIO" ReadOnly="true"
                                    VisibleIndex="2" Visible="False">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Municipio" FieldName="DESCRICAOMUNICIPIO"
                                    ReadOnly="true" VisibleIndex="3">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Censo" FieldName="CENSO" ReadOnly="true" VisibleIndex="4">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="UA" FieldName="UA_ATUAL" ReadOnly="true" VisibleIndex="5">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Escola" FieldName="ESCOLA" ReadOnly="true"
                                    VisibleIndex="6">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Ano" FieldName="ANO" ReadOnly="true" VisibleIndex="7">
                                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Semestre" FieldName="SEMESTRE" ReadOnly="true"
                                    VisibleIndex="8">
                                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Turma" FieldName="TURMA" ReadOnly="true" VisibleIndex="9">
                                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Turno" FieldName="TURNO" ReadOnly="true" VisibleIndex="10">
                                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Disciplina" FieldName="DISCIPLINA" ReadOnly="true"
                                    VisibleIndex="11" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Componente Curricular" FieldName="NOMEDISCIPLINA"
                                    ReadOnly="true" VisibleIndex="12" Visible="true">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Dia da semana" FieldName="DIA_SEMANA" ReadOnly="true"
                                    VisibleIndex="13" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Dia da semana" FieldName="DIA_SEMANA_DESCRICAO"
                                    ReadOnly="true" VisibleIndex="14">
                                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Início" FieldName="HORA_INICIO" ReadOnly="true"
                                    VisibleIndex="15">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Fim" FieldName="HORA_FIM" ReadOnly="true"
                                    VisibleIndex="16">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Aula" FieldName="AULA" ReadOnly="true" VisibleIndex="17"
                                    Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Tipo Cârencia" FieldName="TIPO" ReadOnly="true"
                                    VisibleIndex="18">
                                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="DATA_INICIO" FieldName="DATA_INICIO" ReadOnly="true"
                                    VisibleIndex="19" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="DATA_FIM" FieldName="DATA_FIM" ReadOnly="true"
                                    VisibleIndex="20" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="NUM_FUNC_AULA" FieldName="NUM_FUNC_AULA" ReadOnly="true"
                                    VisibleIndex="21" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="TIPO_GESTAO" FieldName="TIPO_GESTAO" ReadOnly="true"
                                    VisibleIndex="22" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="DEPENDENCIA" FieldName="DEPENDENCIA" ReadOnly="true"
                                    VisibleIndex="23" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="CURSO" FieldName="CURSO" ReadOnly="true" VisibleIndex="24"
                                    Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="SERIE" FieldName="SERIE" ReadOnly="true" VisibleIndex="25"
                                    Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="OPTATIVAREFORCO" FieldName="OPTATIVAREFORCO"
                                    ReadOnly="true" VisibleIndex="26" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="CURRICULO" FieldName="CURRICULO" ReadOnly="true"
                                    VisibleIndex="27" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="DISCIPLINA_MULTIPLA" FieldName="DISCIPLINA_MULTIPLA"
                                    ReadOnly="true" VisibleIndex="28" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="TIPODOCENTE" FieldName="TIPODOCENTE" ReadOnly="true"
                                    VisibleIndex="29" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                            </Columns>
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                        </dxwgv:ASPxGridView>
                    </td>
                </tr>
            </table>
            <br />
            <br />
        </asp:Panel>
    </asp:Panel>
    <div id="dvMigrar" runat="server">
        <table>
            <tr>
                <td align="left">
                    <asp:Button ID="btnMigrar" runat="server" ValidationGroup="SalvarForm" Text="Migrar e Alocar"
                        OnClick="btnMigrar_Click" OnClientClick="Bloqueio();" />
                </td>
            </tr>
        </table>
    </div>
    <div id="dvRelatorio" runat="server" visible="false">
        <table>
            <tr>
                <td align="left">
                    <asp:Button ID="btnRelDesalocaProfessor" runat="server" ValidationGroup="SalvarForm"
                        Text="GLPs e Contratos Cancelados --> Clique aqui" OnClientClick="Bloqueio();" />
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript" language="javascript">
        function AtualizarLabelQtdSelecionado() {
            var qtd = grdSelecionado.GetVisibleRowsOnPage();
            var dvMigracao = document.getElementById('<%=dvMigrar.ClientID %>');
            dvMigracao.style.visibility = "hidden";

            if (qtd > 0) {
                $("#<%= lblQtdSolicitacoes.ClientID %>").html("Existe(m) " + qtd + " aula(s) na Lista de Migração abaixo.<br /><br />");

                if (qtd >= 8) {

                    dvMigracao.style.visibility = "visible";
                }
                else {
                    dvMigracao.style.visibility = "hidden";
                }
            }
            else
                $("#<%= lblQtdSolicitacoes.ClientID %>").html("");
        }
        AtualizarLabelQtdSelecionado();
    </script>

</asp:Content>
