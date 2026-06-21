<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="CardapioEletiva.aspx.cs" Inherits="Techne.Lyceum.Net.Curriculo.CardapioEletiva" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnGeral" runat="server" GroupingText="Informe os dados para inclusão/Consulta:"
        Width="800px">
        <asp:HiddenField runat="server" ID="hdnValida" />
        <asp:HiddenField runat="server" ID="hdnFinaliza" />
        <table>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label Font-Names="Verdana" ID="lblMunicipio" runat="server" Text="Município:"></asp:Label>
                </td>
                <td colspan="3">
                    <tweb:TSearchBox ID="tseMunicipio" runat="server" SqlOrder="nome" SqlSelect=" select distinct codigo, nome, uf_sigla from VW_ZZCRO_UNIDADE_ENSINO u join municipio m on u.municipio = m.CODIGO "
                        GridWidth="600px" ArgumentColumns="50" OnChanged="tseMunicipio_Changed" Columns="10"
                        MaxLength="10">
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
                        MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao,nucleo,municipio,ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO "
                        SqlWhere=" municipio = #tseMunicipio#" GridWidth="850px" OnChanged="tseUnidadeResponsavel_Changed"
                        SqlOrder="nome_comp">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Censo" FieldName="unidade_ens" Width="12%" />
                            <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblAno" runat="server" Text="Ano:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseAno" runat="server" SqlOrder="ano desc" SqlSelect=" select ANO,DESC_ANO from VW_ANOLETIVO "
                        GridWidth="200px" ArgumentColumns="50" OnChanged="tseAno_Changed" Columns="10"
                        DataType="Number" MaxLength="10">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="ANO" FieldName="ANO" Width="60%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblPeriodo" runat="server" Text="Período:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tsePeriodo" runat="server" Caption="" MaxLength="20" ArgumentColumns="50"
                        Columns="10" SqlSelect=" select distinct periodo,DESC_PERIODO from VW_PERIODOLETIVO  "
                        SqlWhere=" ano = #tseAno#" GridWidth="150px" OnChanged="tsePeriodo_Changed" SqlOrder="PERIODO"
                        DataType="Varchar">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Periodo" FieldName="periodo" Width="12%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="DESC_PERIODO" Width="12%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="right" style="width: 120px">
                    <asp:Label ID="lblCursoTSearch" runat="server" Text="Curso:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseCurso" runat="server" Caption="" MaxLength="20" ArgumentColumns="50"
                        Columns="10" SqlSelect=" SELECT DISTINCT C.CURSO,  C.NOME, TP.DESCRICAO,MC.DESCRICAO AS MODALIDADE
                                                                                                        FROM    LY_UNIDADE_ENSINO_CURSOS uc
                                                                                                                JOIN LY_CURSO c ON uc.CURSO = c.CURSO
                                                                                                                INNER JOIN LY_MODALIDADE_CURSO mc ON c.MODALIDADE = mc.MODALIDADE
                                                                                                                INNER JOIN LY_TIPO_CURSO tp ON tp.TIPO = c.TIPO  "
                        SqlWhere=" c.OFERTAELETIVA = 'S' AND C.CURSO <> '9999.80' AND UNIDADE_ENS = #tseUnidadeResponsavel#" GridWidth="850px"
                        OnChanged="tseCurso_Changed" SqlOrder="nome">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Curso" FieldName="curso" Width="12%" />
                            <tweb:TSearchBoxColumn Caption="Nome do Curso" FieldName="nome" Width="12%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="right" style="width: 120px">
                    <asp:Label ID="Label1" runat="server" Text="Série:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseSerie" runat="server" Caption="" MaxLength="20" DataType="Number"
                        ArgumentColumns="50" Columns="10" SqlSelect="SELECT distinct T.SERIE,S.DESCRICAO
                                                                                                    FROM LY_TURMA T
                                                                                                       INNER JOIN LY_SERIE S
                                       ON T.SERIE = S.SERIE AND T.CURSO = S.CURSO AND T.TURNO=S.TURNO AND T.CURRICULO=S.CURRICULO
                                                                                                           "
                        SqlWhere=" T.ANO = #tseAno# AND T.SEMESTRE =#tsePeriodo# AND T.CURSO = #tseCurso# AND T.FACULDADE = #tseUnidadeResponsavel# "
                        GridWidth="850px" OnChanged="tseSerie_Changed" SqlOrder="SERIE">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Serie" FieldName="serie" Width="12%" />
                            <tweb:TSearchBoxColumn Caption="Serie" FieldName="DESCRICAO" Width="12%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <asp:Label ID="lblMensagemFinalizacao" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <asp:Label ID="lblMensagemValidacao" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <div id="divEdit" class="divEditBlock" runat="server" style="width: 850px;">
        <asp:ImageButton ID="btnFinalizar" runat="server" SkinID="BcNovoFinalizar" OnClick="btnFinalizar_Click"
            OnClientClick="return confirm('Confirma a finalização do cardápio?');" />
        <asp:ImageButton ID="btnValidar" runat="server" SkinID="BcNovoValidar" OnClick="btnValidar_Click"
            OnClientClick="return confirm('Esta operação não salva as alterações das eletivas, apenas efetua a validação. Confirma a validação do cardápio?');" />
        <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcNovoSalvar" OnClick="btnSalvar_Click"
            ValidationGroup="SalvarForm" />
        <asp:ImageButton ID="btnLimpar" runat="server" SkinID="BcNovoLimpar" OnClick="btnLimpar_Click" ToolTip="Limpar Validação" />
        <asp:Label runat="server" ID="lblBloco" Text="Cardápio de Eletivas" SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsEletivas" runat="server" EnableClientScript="true" ShowMessageBox="true"
            ValidationGroup="SalvarForm" ShowSummary="false" />
    </div>
    <asp:Panel ID="pnlTurnos" runat="server" Visible="false" Width="800px">
        <asp:Panel ID="pnlManha" runat="server" Width="800px" GroupingText="Manhã" Enabled="false">
            <asp:HiddenField runat="server" ID="hdnEletivaManha1" />
            <asp:HiddenField runat="server" ID="hdnEletivaManha2" />
            <asp:HiddenField runat="server" ID="hdnEletivaTarde1" />
            <asp:HiddenField runat="server" ID="hdnEletivaTarde2" />
            <asp:HiddenField runat="server" ID="hdnEletivaNoite1" />
            <asp:HiddenField runat="server" ID="hdnEletivaNoite2" />
            <asp:HiddenField runat="server" ID="hdnEletivaIntegral1" />
            <asp:HiddenField runat="server" ID="hdnEletivaIntegral2" />
             <asp:HiddenField runat="server" ID="hdnEletivaAmpliado1" />
            <asp:HiddenField runat="server" ID="hdnEletivaAmpliado2" />
            <table>
                <tr>
                    <td>
                        <tweb:TSearchBox ID="tseEletivaManha_1" runat="server" Key="disciplina_multipla"
                            Argument="desc_disciplina_multipla" SqlOrder="desc_disciplina_multipla" SqlSelect="SELECT TURNO,ANO,periodo,SERIE FROM VW_DISCIPLINA_MULTIPLA   "
                            GridWidth="600px" ArgumentColumns="80" Columns="20" MaxLength="50" SqlWhere=" TURNO = 'M' AND ANO = #tseAno# AND periodo =#tsePeriodo# AND SERIE = #tseSerie#  "
                            OnLoad="tseEletivaManha_1_Load">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="disciplina_multipla" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="Disciplina" FieldName="desc_disciplina_multipla"
                                    Width="60%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <tweb:TSearchBox ID="tseEletivaManha_2" runat="server" Key="disciplina_multipla"
                            Argument="desc_disciplina_multipla" SqlOrder="desc_disciplina_multipla" SqlSelect="SELECT TURNO,ANO,periodo,SERIE FROM VW_DISCIPLINA_MULTIPLA   "
                            GridWidth="600px" ArgumentColumns="80" Columns="20" MaxLength="50" SqlWhere=" TURNO = 'M' AND ANO = #tseAno# AND periodo =#tsePeriodo# AND SERIE = #tseSerie#  ">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="disciplina_multipla" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="Disciplina" FieldName="desc_disciplina_multipla"
                                    Width="60%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Panel ID="pnlTarde" runat="server" Width="800px" GroupingText="Tarde" Enabled="false">
            <table>
                <tr>
                    <td>
                        <tweb:TSearchBox ID="tseEletivaTarde_1" runat="server" Key="disciplina_multipla"
                            Argument="desc_disciplina_multipla" SqlOrder="desc_disciplina_multipla" SqlSelect="SELECT TURNO,ANO,periodo,SERIE FROM VW_DISCIPLINA_MULTIPLA   "
                            GridWidth="600px" ArgumentColumns="80" Columns="20" MaxLength="50" SqlWhere=" TURNO = 'T' AND ANO = #tseAno# AND periodo =#tsePeriodo# AND SERIE = #tseSerie#  ">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="disciplina_multipla" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="Disciplina" FieldName="desc_disciplina_multipla"
                                    Width="60%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <tweb:TSearchBox ID="tseEletivaTarde_2" runat="server" Key="disciplina_multipla"
                            Argument="desc_disciplina_multipla" SqlOrder="desc_disciplina_multipla" SqlSelect="SELECT TURNO,ANO,periodo,SERIE FROM VW_DISCIPLINA_MULTIPLA   "
                            GridWidth="600px" ArgumentColumns="80" Columns="20" MaxLength="50" SqlWhere=" TURNO = 'T' AND ANO = #tseAno# AND periodo =#tsePeriodo# AND SERIE = #tseSerie#  ">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="disciplina_multipla" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="Disciplina" FieldName="desc_disciplina_multipla"
                                    Width="60%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Panel ID="pnlNoite" runat="server" Width="800px" GroupingText="Noite" Enabled="false">
            <table>
                <tr>
                    <td>
                        <tweb:TSearchBox ID="tseEletivaNoite_1" runat="server" Key="disciplina_multipla"
                            Argument="desc_disciplina_multipla" SqlOrder="desc_disciplina_multipla" SqlSelect="SELECT TURNO,ANO,periodo,SERIE FROM VW_DISCIPLINA_MULTIPLA   "
                            GridWidth="600px" ArgumentColumns="80" Columns="20" MaxLength="50" SqlWhere=" TURNO = 'N' AND ANO = #tseAno# AND periodo =#tsePeriodo# AND SERIE = #tseSerie#  ">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="disciplina_multipla" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="Disciplina" FieldName="desc_disciplina_multipla"
                                    Width="60%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <tweb:TSearchBox ID="tseEletivaNoite_2" runat="server" Key="disciplina_multipla"
                            Argument="desc_disciplina_multipla" SqlOrder="desc_disciplina_multipla" SqlSelect="SELECT TURNO,ANO,periodo,SERIE FROM VW_DISCIPLINA_MULTIPLA   "
                            GridWidth="600px" ArgumentColumns="80" Columns="20" MaxLength="50" SqlWhere=" TURNO = 'N' AND ANO = #tseAno# AND periodo =#tsePeriodo# AND SERIE = #tseSerie#  ">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="disciplina_multipla" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="Disciplina" FieldName="desc_disciplina_multipla"
                                    Width="60%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Panel ID="pnlIntegral" runat="server" Width="800px" GroupingText="Integral"
            Enabled="false">
            <table>
                <tr>
                    <td>
                        <tweb:TSearchBox ID="tseEletivaIntegral_1" runat="server" Key="disciplina_multipla"
                            Argument="desc_disciplina_multipla" SqlOrder="desc_disciplina_multipla" SqlSelect="SELECT TURNO,ANO,periodo,SERIE FROM VW_DISCIPLINA_MULTIPLA   "
                            GridWidth="600px" ArgumentColumns="80" Columns="20" MaxLength="50" SqlWhere=" TURNO = 'I' AND ANO = #tseAno# AND periodo =#tsePeriodo# AND SERIE = #tseSerie#  ">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="disciplina_multipla" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="Disciplina" FieldName="desc_disciplina_multipla"
                                    Width="60%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <tweb:TSearchBox ID="tseEletivaIntegral_2" runat="server" Key="disciplina_multipla"
                            Argument="desc_disciplina_multipla" SqlOrder="desc_disciplina_multipla" SqlSelect="SELECT TURNO,ANO,periodo,SERIE FROM VW_DISCIPLINA_MULTIPLA   "
                            GridWidth="600px" ArgumentColumns="80" Columns="20" MaxLength="50" SqlWhere=" TURNO = 'I' AND ANO = #tseAno# AND periodo =#tsePeriodo# AND SERIE = #tseSerie#  ">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="disciplina_multipla" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="Disciplina" FieldName="desc_disciplina_multipla"
                                    Width="60%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Panel ID="pnlAmpliado" runat="server" Width="800px" GroupingText="Ampliado"
            Enabled="false">
            <table>
                <tr>
                    <td>
                        <tweb:TSearchBox ID="tseEletivaAmpliado_1" runat="server" Key="disciplina_multipla"
                            Argument="desc_disciplina_multipla" SqlOrder="desc_disciplina_multipla" SqlSelect="SELECT TURNO,ANO,periodo,SERIE FROM VW_DISCIPLINA_MULTIPLA   "
                            GridWidth="600px" ArgumentColumns="80" Columns="20" MaxLength="50" SqlWhere=" TURNO = 'M' AND ANO = #tseAno# AND periodo =#tsePeriodo# AND SERIE = #tseSerie#  ">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="disciplina_multipla" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="Disciplina" FieldName="desc_disciplina_multipla"
                                    Width="60%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <tweb:TSearchBox ID="tseEletivaAmpliado_2" runat="server" Key="disciplina_multipla"
                            Argument="desc_disciplina_multipla" SqlOrder="desc_disciplina_multipla" SqlSelect="SELECT TURNO,ANO,periodo,SERIE FROM VW_DISCIPLINA_MULTIPLA   "
                            GridWidth="600px" ArgumentColumns="80" Columns="20" MaxLength="50" SqlWhere=" TURNO = 'M' AND ANO = #tseAno# AND periodo =#tsePeriodo# AND SERIE = #tseSerie#  ">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="disciplina_multipla" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="Disciplina" FieldName="desc_disciplina_multipla"
                                    Width="60%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
</asp:Content>
