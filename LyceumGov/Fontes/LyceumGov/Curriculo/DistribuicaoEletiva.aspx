<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="DistribuicaoEletiva.aspx.cs" Inherits="Techne.Lyceum.Net.Curriculo.DistribuicaoEletiva" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnGeral" runat="server" GroupingText="Informe os dados para inclusão/Consulta:"
        Width="800px">
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
                        SqlWhere=" c.OFERTAELETIVA = 'S' AND C.CURSO <> '9999.80' AND UNIDADE_ENS = #tseUnidadeResponsavel#"
                        GridWidth="850px" OnChanged="tseCurso_Changed" SqlOrder="nome">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Curso" FieldName="curso" Width="12%" />
                            <tweb:TSearchBoxColumn Caption="Nome do Curso" FieldName="nome" Width="12%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="right" style="width: 120px">
                    <asp:Label ID="Label1" runat="server" Text="Turno:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseTurno" runat="server" Caption="" MaxLength="1" DataType="VarChar"
                        ArgumentColumns="50" Columns="10" SqlSelect="SELECT distinct TU.TURNO, TU.DESCRICAO AS NOME_TURNO
                                                                                                    FROM LY_TURMA T     
                                                                                                    INNER JOIN LY_TURNO TU ON TU.TURNO = T.TURNO                                                                                                 
                                                                                                           "
                        SqlWhere=" T.ANO = #tseAno# AND T.SEMESTRE =#tsePeriodo# AND T.CURSO = #tseCurso# AND T.FACULDADE = #tseUnidadeResponsavel# "
                        GridWidth="850px" OnChanged="tseTurno_Changed" SqlOrder="NOME_TURNO">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Turno" FieldName="TURNO" Width="12%" />
                            <tweb:TSearchBoxColumn Caption="Descrição Turno" FieldName="NOME_TURNO" Width="12%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <div id="divEdit" runat="server" class="divEditBlock" style="width: 850px;">
        <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcNovoSalvar" OnClick="btnSalvar_Click"
            ValidationGroup="SalvarForm" />
        <asp:Label runat="server" ID="lblBloco" Text="Distribuição de Eletivas" SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsEletivas" runat="server" EnableClientScript="true" ShowMessageBox="true"
            ValidationGroup="SalvarForm" ShowSummary="false" />
    </div>
    <br />
    <div id="dvTurmas" runat="server" visible="false">
        <asp:Panel GroupingText="Informações" runat="server">
            <ul style="list-style-type: square">
                <li>Na descrição da turma a expressão <b>"Referência"</b> indica que é uma turma apenas de eletiva para escolas que só possuem uma turma no curso / serie / turno.</li>
            </ul>
        </asp:Panel>
        <br />
        <br />
        <table>
            <asp:Repeater ID="rpTurmas" runat="server" OnItemDataBound="rpTurmas_ItemDataBound">
                <HeaderTemplate>
                    <tr>
                        <td>
                            <asp:Label ID="lblTurma" runat="server" Text="Turma" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Grupo 1" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="Grupo 2" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="Grupo 3" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblTurmaRef" runat="server" Text="Turma Referência" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:Label ID="lblTurma" runat="server" Text='<%#Eval("TURMA")%>' SkinID="lblObrigatorio"></asp:Label>
                            <asp:HiddenField ID="hdnSerie" runat="server" Value='<%# Eval("SERIE") %>' />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlGrupo1" DataTextField="NOME" DataValueField="DISCIPLINA"
                                runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlGrupo2" DataTextField="NOME" DataValueField="DISCIPLINA"
                                runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlGrupo3" DataTextField="NOME" DataValueField="DISCIPLINA"
                                runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblTurmaReferencia" runat="server" Text='<%#Eval("DESCRICAOTURMA")%>'
                                SkinID="lblObrigatorio"></asp:Label>
                            <asp:HiddenField ID="hdnReferencia" runat="server" Value='<%# Eval("REFERENCIA") %>' />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>
</asp:Content>
