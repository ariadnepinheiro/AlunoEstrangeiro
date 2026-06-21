<%@ Page Title="" Language="C#" AutoEventWireup="true" MasterPageFile="~/Modulos/LyceumMaster.Master"
    CodeBehind="AlteracaoRescisaoCHCT.aspx.cs" Inherits="Techne.Lyceum.Net.ProcessoSeletivo.AlteracaoRescisaoCHCT" %>

<asp:Content ID="conSolicitacaoAlteracaoRescisaoCHCT" ContentPlaceHolderID="cphFormulario"
    runat="server">

    <script type="text/javascript" src="../Scripts/ValidationControls.js"></script>

    <asp:Panel ID="pnBusca" GroupingText="Faça uma busca por processo seletivo e matrícula"
        runat="server" Width="700px">
        <table>
            <tr>
                <td align="right" style="width: 30%">
                    <asp:Label ID="lblBuscaConcurso" runat="server" Text="Processo Seletivo:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td style="width: 70%">
                    <tweb:TSearchBox ID="tseConcursoBusca" runat="server" Caption="" SqlSelect="SELECT distinct concurso, descricao FROM lY_concurso_docente"
                        ArgumentColumns="50" Columns="30" MaxLength="20" SqlOrder="descricao" GridWidth="800px"
                        SqlWhere="tipo = 'Contrato'" OnChanged="tseConcursoBusca_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Processo Seletivo" FieldName="concurso" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="70%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblBuscaCandidato" runat="server" Text="ID/Vínculo ou Matrícula Definitiva:* "
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseCandidatoBusca" runat="server" Caption="" SqlSelect="select CANDIDATO,CONCURSO,NOME,REGIONALID,REGIONAL,MATRICULA,CPF,NUM_FUNC,PESSOA,STATUS from VW_CANDIDATO_RESCISAO"
                        ArgumentColumns="50" Columns="30" Key="IDVINCULO" Argument="nome" SqlWhere="STATUS = '24' and concurso = #tseConcursoBusca#"
                        MaxLength="20" SqlOrder="nome" GridWidth="800px" OnChanged="tseCandidatoBusca_Changed"
                        AutoPostBack="true">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Matrícula Provisória" FieldName="candidato" Visible="false" />
                            <tweb:TSearchBoxColumn Caption="Matrícula Definitiva" FieldName="matricula" Width="25%" />
                            <tweb:TSearchBoxColumn Caption="Id/Vínculo ou Matrícula" FieldName="IDVINCULO" Width="25%" />
                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="nome" Width="25%" />
                            <tweb:TSearchBoxColumn Caption="CPF" FieldName="cpf" Width="25%" />
                            <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="25%" />
                            <tweb:TSearchBoxColumn Caption="Código Docente" FieldName="num_func" Visible="false" />
                            <tweb:TSearchBoxColumn Caption="pessoa" FieldName="pessoa" Visible="false" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblSelecionarOpcao" runat="server" Text="Selecionar:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlSelecionarOpcao" Height="20px" OnSelectedIndexChanged="ddlSelecionarOpcao_SelectedIndexChanged"
                        AutoPostBack="true">
                        <asp:ListItem Text="Selecione" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Rescisão de Contrato" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Alteração de Carga Horária" Value="2"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <div class="divEditBlock" id="divOpcao" runat="server" style="width: 850px;">
        <asp:ImageButton ID="btnEditar" runat="server" SkinID="BcEditar" Visible="false" />
        <asp:ImageButton ID="btnImprimir" runat="server" SkinID="Imprimir" ImageAlign="Right" />
        <asp:Label runat="server" ID="lblBlocoSolicitacao" Text="" SkinID="BcTitulo" />
    </div>
    <br />
    <asp:Panel runat="server" ID="pnCamposDocente" GroupingText="Dados" Width="850px"
        Visible="false">
        <table>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblDocente" runat="server" Text="Docente:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDocente" runat="server" ReadOnly="true" Width="500px" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblCPF" runat="server" Text="CPF:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtCPF" runat="server" ReadOnly="true" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblMatricula" runat="server" Text="Matrícula Definitiva:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtMatricula" runat="server" ReadOnly="true" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblIdFuncional" runat="server" Text="ID/Vínculo: "></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtIdFuncional" runat="server" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblCotas" runat="server" Text="Regime de Cotas: "></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtCotas" runat="server" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblDataProposta" runat="server" Text="Data de Admissão:"></asp:Label>
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="dtdataProposta" runat="server" MinDate="1901-01-01" CalendarProperties-ClearButtonText="Limpar"
                        CalendarProperties-TodayButtonText="Hoje" ReadOnly="true">
                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                        </CalendarProperties>
                    </dxe:ASPxDateEdit>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblSituacao" runat="server" Text="Situação:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtSituacao" runat="server" ReadOnly="true" Width="500px" Visible="false" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblCoordenadoria" runat="server" Font-Names="Verdana" Text="Coordenadoria:"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseRegional" runat="server" Argument="regional" ArgumentColumns="50"
                        MaxLength="20" Columns="10" Key="id_regional" DataType="Number" SqlSelect="SELECT id_regional,regional FROM TCE_REGIONAL">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="id_regional" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="80%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                    <tweb:TSearchBox ID="tseCoordenadoria" runat="server" Argument="descricao" ArgumentColumns="50"
                        MaxLength="20" Columns="10" Key="nucleo" SqlSelect="SELECT NUCLEO, DESCRICAO FROM LY_NUCLEO"
                        SqlOrder="nucleo">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="nucleo" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Coordenadoria" FieldName="descricao" Width="80%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label Font-Names="Verdana" ID="lblMunicipio" runat="server" Text="Município:"></asp:Label>
                </td>
                <td colspan="5">
                    <tweb:TSearchBox ID="tseMunicipio" runat="server" SqlOrder="nome" SqlSelect="SELECT DISTINCT codigo, nome, uf_sigla FROM VW_ZZCRO_UNIDADE_ENSINO U JOIN MUNICIPIO M ON U.MUNICIPIO = M.CODIGO"
                        GridWidth="600px" ArgumentColumns="50" Columns="10" MaxLength="10">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="60%" />
                            <tweb:TSearchBoxColumn Caption="Estado" FieldName="uf_sigla" Width="20%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label Font-Names="Verdana" ID="lblUnidadeResponsavel" runat="server" Text="Unidade Ensino:"></asp:Label>
                </td>
                <td colspan="3">
                    <tweb:TSearchBox ID="tseUnidadeResponsavel" runat="server" Caption="" Key="unidade_ens"
                        MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" SqlSelect="SELECT unidade_ens, nome_comp, setor, cgc, situacao, ua_atual,ua_antiga  FROM VW_UNIDADE_ENSINO_SITUACAO"
                        GridWidth="850px" SqlOrder="nome_comp">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="12%" />
                            <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblDisciplina" runat="server" Text="Disciplina: "></asp:Label>
                </td>
                <td colspan="5">
                    <tweb:TSearchBox ID="tseDisciplina" runat="server" Key="agrupamento" Argument="descricao"
                        MaxLength="50" SqlSelect="SELECT DISTINCT GH.agrupamento, GH.descricao FROM LY_CONCURSO_DOC_HABILITACAO CDH INNER JOIN LY_GRUPO_HABILITACAO GH ON CDH.AGRUPAMENTO = GH.AGRUPAMENTO"
                        SqlWhere=" GH.ATIVO='S' AND CDH.concurso = #tseConcursoBusca#" GridWidth="850px"
                        Columns="10" ArgumentColumns="50">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Disciplina" FieldName="agrupamento" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="70%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label runat="server" ID="lblCargo" Text="Cargo: "></asp:Label>
                </td>
                <td colspan="2">
                    <asp:TextBox runat="server" ID="txtCargo" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblCargaHoraria" runat="server" Text="Carga Horária:"></asp:Label>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtCargaHoraria" runat="server" MaxLength="3" Width="50px" ReadOnly="true" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblAulasAlocadas" runat="server" Text="Aulas Alocadas:"></asp:Label>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtAulasAlocadas" runat="server" Width="50px" ReadOnly="true" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 20%">
                    <asp:Label runat="server" Text="Data do último exercício: " ID="lblDtExercicio"></asp:Label>
                </td>
                <td colspan="2">
                    <dxe:ASPxDateEdit ID="dtExercicio" runat="server" MinDate="1901-01-01" CalendarProperties-ClearButtonText="Limpar"
                        CalendarProperties-TodayButtonText="Hoje">
                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                        </CalendarProperties>
                    </dxe:ASPxDateEdit>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label runat="server" ID="lblJustificativa" Text="Justificativa: "></asp:Label>
                </td>
                <td colspan="2">
                    <asp:TextBox runat="server" ID="txtJustificativa" Width="600px" onkeypress="return SomenteLetras(event);"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label runat="server" ID="lblNovaCargaHoraria" Text="Nova Carga Horária: "></asp:Label>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlNovaCargaHoraria" Width="100px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <table>
                        <tr>
                            <td>
                                <dxe:ASPxButton ID="btnAprovar" runat="server" Text="Rescindir" OnClick="btnAprovar_Click">
                                </dxe:ASPxButton>
                            </td>
                            <td>
                                <dxe:ASPxButton ID="btnAprovarAlteracao" runat="server" Text="Alterar Carga Horária"
                                    OnClick="btnAprovarAlteracao_Click" Visible="false">
                                </dxe:ASPxButton>
                            </td>
                            <td>
                                <dxe:ASPxButton ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click">
                                </dxe:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
