<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="TransferenciaTurma.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.TransferenciaTurma" %>

<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Informe a matrícula ou o nome do aluno"
        Height="45px" Width="580px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblAlunoTSearch" runat="server" Text="Aluno:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseAluno" runat="server" OnTextChanged="tseAluno_Changed" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAlunoTransfTurma"
                        AutoPostBack="true">
                        <QueryParameters>
                            <asp:Parameter Name="ativo" DefaultValue="Ativo" DbType="String" />
                        </QueryParameters>
                    </tweb:TSearch>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagemBloqueio" runat="server" SkinID="lblMensagem"></asp:Label>
    <asp:ScriptManagerProxy ID="manager" runat="server" />
    <asp:UpdatePanel ID="upTransfTurma" runat="server">
        <ContentTemplate>
            <asp:ValidationSummary ID="vsTransfTurma" runat="server" ShowMessageBox="true" ValidationGroup="SalvarForm"
                ShowSummary="false" />
            <br />
            <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
            <table>
                <tr>
                    <td>
                        <asp:Panel ID="pnDados" GroupingText="Dados Atuais do Aluno" runat="server">
                            <table>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblUniEnsino" runat="server" Text="Unidade de Ensino:"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtUniEnsino" runat="server" MaxLength="100" Width="600px" ReadOnly="true"
                                            Visible="false" />
                                        <asp:TextBox ID="txtNomeUniEnsino" runat="server" MaxLength="100" Width="600px" ReadOnly="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblSituacao" runat="server" Text="Situação:"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtSituacao" runat="server" MaxLength="15" Width="600px" ReadOnly="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblCurso" runat="server" Text="Escolaridade:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNomeCurso" runat="server" MaxLength="20" Width="200px" ReadOnly="true" />
                                        <asp:TextBox ID="txtCurso" runat="server" MaxLength="20" Width="50px" ReadOnly="true"
                                            Visible="false" />
                                        <asp:TextBox ID="txtCobran" runat="server" MaxLength="20" Width="50px" ReadOnly="true"
                                            Visible="false" />
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblTurno" runat="server" Text="Turno:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurno" runat="server" MaxLength="20" Width="50px" ReadOnly="true"
                                            Visible="false" />
                                        <asp:TextBox ID="txtNomeTurno" runat="server" MaxLength="20" Width="200px" ReadOnly="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblCurriculo" runat="server" Text="Currículo:" Visible="false"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtCurriculo" runat="server" MaxLength="20" Width="600px" ReadOnly="true"
                                            Visible="false" />
                                        <asp:TextBox ID="txtAno" runat="server" Visible="false" />
                                        <asp:TextBox ID="txtPeriodo" runat="server" Visible="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblSerie" runat="server" Text="Ano Escolar:"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtSerie" runat="server" MaxLength="3" Width="100px" ReadOnly="true"
                                            Visible="false" />
                                        <asp:TextBox ID="txtNomeSerie" runat="server" MaxLength="3" Width="600px" ReadOnly="true"
                                            Visible="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblTurmaAtual" runat="server" Text="Turma Atual: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurmaAtual" runat="server" MaxLength="50" Width="200px" ReadOnly="true" />
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblAnoLetivo" runat="server" Text="Ano Letivo: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAnoLetivo" runat="server" MaxLength="20" Width="200px" ReadOnly="true" />
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblPeriodoLetivo" runat="server" Text="Período Letivo: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPeriodoLetivo" runat="server" MaxLength="20" Width="200px" ReadOnly="true" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnGrid" runat="server" GroupingText="Dados da transferência">
                            <table>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblPeriodoDestino" Text="Periodo de destino:* " runat="server" Visible="false"
                                            SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:DropDownList ID="ddlPeriodoDestino" runat="server" AutoPostBack="True" DataTextField="PERIODO"
                                            DataValueField="PERIODO" Width="250px" Visible="false" OnSelectedIndexChanged="ddlPeriodoDestino_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblCursotse" Text="Curso:* " runat="server" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <tweb:TSearchBox ID="tseCurso" runat="server" Caption="" AutoPostBack="true" ArgumentColumns="60"
                                            Columns="10" OnChanged="tseCurso_Changed" MaxLength="20" GridWidth="800px" SqlOrder="nome" >
                                            <GridColumns>
                                                <tweb:TSearchBoxColumn Caption="Código" FieldName="curso" Width="20%" />
                                                <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome" Width="60%" />
                                                <tweb:TSearchBoxColumn Caption="Tipo Curso" FieldName="tipo_curso" Width="20%" />
                                            </GridColumns>
                                        </tweb:TSearchBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblDDLTurno" Text="Turno:* " runat="server" Visible="false" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:DropDownList ID="ddlTurno" runat="server" AutoPostBack="True" DataTextField="descricao"
                                            DataValueField="turno" Width="250px" Visible="false" OnSelectedIndexChanged="ddlTurno_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblDDLSerie" Text="Ano de Escolaridade:* " runat="server" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlSerie" runat="server" DataTextField="serie" AutoPostBack="true"
                                            DataValueField="serie" Width="250px" OnSelectedIndexChanged="ddlSerie_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblTurmaDestino" runat="server" Text="Turma de destino:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlTurmaDestino" runat="server" DataTextField="turma" DataValueField="grade_id"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlTurmaDestino_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblTipoCurso" runat="server" Text="Tipo Ens. Profissionalizante: "
                                            Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlTipoCurso" runat="server" Visible="false">
                                            <asp:ListItem Text="Selecione" Value=""></asp:ListItem>
                                            <asp:ListItem Text="Concomitante" Value="Concomitante"></asp:ListItem>
                                            <asp:ListItem Text="Subsequente" Value="Subsequente"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr visible="false">
                                    <td style="text-align: right">
                                        <asp:Label ID="lblGradeIDDestino" runat="server" Text="Grade ID: " Visible="false"></asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:TextBox ID="txtGradeIdDestino" runat="server" MaxLength="50" Width="200px" ReadOnly="true"
                                            Visible="false" />
                                        <asp:HiddenField ID="hdnCurriculo" runat="server" />
                                        <asp:HiddenField ID="hdnEnsinoReligioso" runat="server" />
                                        <asp:HiddenField ID="hdnLinguaEstrangeiraFacultativa" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label8" runat="server" Text="Disciplinas Optativas: " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:CheckBox ID="chkEnsReligioso" runat="server" Text="Ensino Religioso" Width="140px"
                                            Enabled="false" />
                                        <asp:CheckBox ID="chkLinguaEstrangeira" runat="server" Text="Língua Estrangeira Facultativa"
                                            Enabled="false" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:Panel ID="pnlConfirmacao" runat="server" GroupingText="Existe algum aluno para colocar na vaga que está sendo liberada?(Somente para o Curso/Série/Turno que participa da matrícula fácil)"  Visible="false">
                                <table>
                                    <%--<tr>
                                        <td>
                                            <asp:Label ID="Label9" Text="Existe algum aluno para colocar na vaga que está sendo liberada?(Somente para o Curso/Série/Turno que participa da matrícula fácil)"
                                                runat="server" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td>
                                            <asp:RadioButtonList ID="rblConfirmacao" runat="server" RepeatDirection="Horizontal"
                                                AutoPostBack="true" OnSelectedIndexChanged="rblConfirmacao_SelectedIndexChanged">
                                                <asp:ListItem Text="Sim" Value="Sim"></asp:ListItem>
                                                <asp:ListItem Text="Não" Value="Nao"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Panel ID="pnlPermuta" runat="server" GroupingText="Informe a matrícula ou o nome do aluno para colocar na vaga"
                                                Width="650px" Visible="false">
                                                <table>
                                                    <tr>
                                                        <td style="text-align: right">
                                                            <asp:Label ID="Label10" runat="server" Text="Aluno:* " SkinID="lblObrigatorio"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <tweb:TSearch ID="tseAlunoPermuta" runat="server" OnTextChanged="tseAlunoPermuta_Changed"
                                                                SettingsTypeName="Techne.Lyceum.RN.Query.QueryAlunoTransfTurma" AutoPostBack="true">
                                                                <QueryParameters>
                                                                    <asp:Parameter Name="ativo" DefaultValue="Ativo" DbType="String" />
                                                                </QueryParameters>
                                                            </tweb:TSearch>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right">
                                                            <asp:Label ID="Label11" runat="server" Text="Disciplinas Optativas: "></asp:Label>
                                                        </td>
                                                        <td colspan="5">
                                                            <asp:CheckBox ID="chkEnsReligiosoPermuta" runat="server" Text="Ensino Religioso"
                                                                Width="140px" Enabled="false" />
                                                            <asp:CheckBox ID="chkLinEstrangeiraPermuta" runat="server" Text="Língua Estrangeira Facultativa"
                                                                Enabled="false" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right">
                                                            <asp:Label ID="Label12" runat="server" Text="Motivo:* " SkinID="lblObrigatorio"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlMotivoPermuta" runat="server" DataValueField="motivo_transf"
                                                                DataTextField="descricao" Width="200px">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlTurmaConcomitanteOrigem" runat="server" GroupingText="Turma Profissionalizante (Concomitante) - Origem">
                            <table>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="Label7" Text="Unidade de Ensino: " runat="server"></asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:Label ID="lblUnidadeEnsinoOrigem" runat="server"></asp:Label>
                                        -
                                        <asp:Label ID="lblNomeUnidadeEnsinoOrigem" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="Label2" Text="Ano: " runat="server"></asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:Label ID="lblAnoConcomitanteOrigem" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="Label1" Text="Período: " runat="server"></asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:Label ID="lblPeriodoConcomitanteOrigem" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="Label3" Text="Curso: " runat="server"></asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:Label ID="lblCursoConcomitanteOrigem" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="Label4" Text="Turno: " runat="server"></asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:Label ID="lblTurnoConcomitanteOrigem" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="Label5" Text="Ano de Escolaridade: " runat="server"></asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:Label ID="lblAnoEscolaridadeConcomitanteOrigem" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label6" runat="server" Text="Turma: "></asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:Label ID="lblTurmaConcomitanteOrigem" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlTurmaConcomitante" runat="server" GroupingText="Turma Profissionalizante (Concomitante) - Destino">
                            <table>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblTituloCurso" Text="Curso de destino: " runat="server"></asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <tweb:TSearchBox ID="tseCursoConcomitante" runat="server" Caption="" AutoPostBack="true"
                                            ArgumentColumns="60" Columns="10" OnChanged="tseCursoConcomitante_Changed" MaxLength="20"
                                            GridWidth="800px" SqlOrder="nome">
                                            <GridColumns>
                                                <tweb:TSearchBoxColumn Caption="Código" FieldName="curso" Width="30%" />
                                                <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome" Width="70%" />
                                            </GridColumns>
                                        </tweb:TSearchBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblDDLTurnoConcomitante" Text="Turno de destino: " runat="server"
                                            Visible="false"></asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:DropDownList ID="ddlTurnoConcomitante" runat="server" AutoPostBack="True" DataTextField="descricao"
                                            DataValueField="turno" Width="250px" Visible="false" OnSelectedIndexChanged="ddlTurnoConcomitante_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblDDLSerieConcomitante" Text="Ano de Escolaridade de destino: " runat="server"></asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:DropDownList ID="ddlSerieConcomitante" runat="server" DataTextField="serie"
                                            AutoPostBack="true" DataValueField="serie" Width="250px" OnSelectedIndexChanged="ddlSerieConcomitante_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblTurmaConcomitante" runat="server" Text="Turma de destino:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlTurmaConcomitante" runat="server" DataTextField="turma"
                                            DataValueField="grade_id" AutoPostBack="false">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Button ID="btnSalvarConcomitante" runat="server" Text="Salvar Concomitante"
                                            OnClick="btnSalvarConcomitante_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlTurmaEducacaoEspecialOrigem" runat="server" GroupingText="Turma Educação Especial - Origem">
                            <table>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblDscUnidadeEnsinoEducEspecialOrigem" Text="Unidade de Ensino: "
                                            runat="server"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:Label ID="lblCensoEducEspecialOrigem" runat="server"></asp:Label>
                                        -
                                        <asp:Label ID="lblEscolaEducEspecialOrigem" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblDscCursoEducEspecialOrigem" Text="Curso: " runat="server"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:Label ID="lblCursoEducEspecialOrigem" runat="server"></asp:Label>
                                        -
                                        <asp:Label ID="lblNomeCursoEspecialOrigem" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblDscAnoEducEspecialOrigem" Text="Ano: " runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblAnoEducEspecialOrigem" runat="server"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblDscPeriodoEducEspecialOrigem" Text="Período: " runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPeriodoEducEspecialOrigem" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblDscTurnoEducEspecialOrigem" Text="Turno: " runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTurnoEducEspecialOrigem" runat="server"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblDscSerieEducEspecialOrigem" Text="Ano de Escolaridade: " runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblSerieEducEspecialOrigem" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblDscTurmaEducEspecialOrigem" runat="server" Text="Sala de Recuros: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTurmaEducEspecialOrigem" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblDscHorarioEducEspecialOrigem" runat="server" Text="Horário de Atendimento: "></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:ListBox ID="lblHorariosOrigem" runat="server" Width="700px"></asp:ListBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlTurmaEducacaoEspecialDestino" runat="server" GroupingText="Turma Educação Especial - Destino">
                            <table>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblDscTurnoEducEspecialDestino" Text="Turno de destino: " runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlTurnoEducEspecialDestino" runat="server" AutoPostBack="True"
                                            DataTextField="descricao" DataValueField="turno" Width="250px" OnSelectedIndexChanged="ddlTurnoEducEspecialDestino_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblDscSerieEducEspecialDestino" Text="Ano de Escolaridade de destino: "
                                            runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlSerieEducEspecialDestino" runat="server" DataTextField="serie"
                                            AutoPostBack="true" DataValueField="serie" Width="250px" OnSelectedIndexChanged="ddlSerieEducEspecialDestino_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblDscTurmaEducEspecialDestino" runat="server" Text="Sala de Recurso de destino:* "
                                            SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlTurmaEducEspecialDestino" runat="server" DataTextField="turma"
                                            DataValueField="turma" AutoPostBack="true" OnSelectedIndexChanged="ddlTurmaEducEspecialDestino_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblDscHorarioEducEspecialDestino" runat="server" Text="Horário de Atendimento: "
                                            SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlHorarioEducEspecialDestino" runat="server" DataTextField="HORARIO"
                                            DataValueField="DISCIPLINA" AutoPostBack="false">
                                        </asp:DropDownList>
                                    </td>
                                    <tr>
                                        <td>
                                            Horários adicionados<span style="color: #FF0000">*</span>:
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Button ID="btnAdicionar" runat="server" Text="Adicionar" OnClick="btnAdicionar_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:ListBox ID="lbHorariosDestino" runat="server" Width="700px"></asp:ListBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="text-align: right">
                                            <asp:Button ID="btnRemover" runat="server" Text="Remover" OnClick="btnRemover_Click" />
                                        </td>
                                    </tr>
                                </tr>
                                <tr align="left">
                                    <td>
                                        <asp:Button ID="btnTransferirEducEspecial" runat="server" Text="Transferir Educação Especial"
                                            OnClick="btnTransferirEducEspecial_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlTurmaMaisEducacaoOrigem" runat="server" GroupingText="Turma Mais Educação - Origem">
                            <table>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblDscEscolaMaisEducacao" Text="Unidade de Ensino: " runat="server"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:Label ID="lblCensoMaisEducacao" runat="server"></asp:Label>
                                        -
                                        <asp:Label ID="lblEscolaMaisEducacao" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblDscCursoMaisEducacaoOrigem" Text="Curso: " runat="server"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:Label ID="lblCursoMaisEducacaoOrigem" runat="server"></asp:Label>
                                        -
                                        <asp:Label ID="lblNomeCursoMaisEducacaoOrigem" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblDscAnoMaisEducacao" Text="Ano: " runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblAnoMaisEducacao" runat="server"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblDscPeriodoMaisEducacao" Text="Período: " runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPeriodoMaisEducacao" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblDscTurnoMaisEducacaoOrigem" Text="Turno: " runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTurnoMaisEducacaoOrigem" runat="server"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblDscSerieMaisEducacaoOrigem" Text="Ano de Escolaridade: " runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblSerieMaisEducacaoOrigem" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblDscTurmaMaisEducacaoOrigem" runat="server" Text="Turma: "></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:Label ID="lblTurmaMaisEducacaoOrigem" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlTurmaMaisEducacaoDestino" runat="server" GroupingText="Turma Mais Educação - Destino">
                            <table>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblDscTurnoMaisEducacaoDestino" Text="Turno de destino: " runat="server"></asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:DropDownList ID="ddlTurnoMaisEducacaoDestino" runat="server" AutoPostBack="True"
                                            DataTextField="descricao" DataValueField="turno" Width="250px" OnSelectedIndexChanged="ddlTurnoMaisEducacaoDestino_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lbldscSerieMaisEducacaoDestino" Text="Ano de Escolaridade de destino: "
                                            runat="server"></asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:DropDownList ID="ddlSerieMaisEducacaoDestino" runat="server" DataTextField="serie"
                                            AutoPostBack="true" DataValueField="serie" Width="250px" OnSelectedIndexChanged="ddlSerieMaisEducacaoDestino_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblDscTurmaMaisEducacaoDestino" runat="server" Text="Turma de destino:* "
                                            SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlTurmaMaisEducacaoDestino" runat="server" DataTextField="turma"
                                            DataValueField="grade_id" AutoPostBack="false">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td>
                                        <asp:Button ID="btnTransferirMaisEducacao" runat="server" Text="Transferir Mais Educação"
                                            OnClick="btnTransferirMaisEducacao_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlProgressao" runat="server" GroupingText="Progressão Parcial">
                            <table>
                                <tr>
                                    <td>
                                        <dxwgv:ASPxGridView ID="grdProgressao" runat="server" AutoGenerateColumns="False"
                                            ClientInstanceName="grdProgressao" DataSourceID="odsProgressao" KeyFieldName="ALUNO;DISCIPLINA;TURMA;ANO;SEMESTRE"
                                            OnStartRowEditing="grdProgressao_StartRowEditing" OnInitNewRow="grdProgressao_InitNewRow"
                                            OnCellEditorInitialize="grdProgressao_CellEditorInitialize" OnAfterPerformCallback="grdProgressao_AfterPerformCallback">
                                            <SettingsEditing Mode="Inline" />
                                            <Columns>
                                                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                                    <UpdateButton Text="Salvar">
                                                        <Image Url="~/img/bt_salvar.png" />
                                                    </UpdateButton>
                                                    <EditButton Text="Editar" Visible="True">
                                                        <Image Url="~/img/bt_editar.png" />
                                                    </EditButton>
                                                    <CancelButton Text="Cancelar">
                                                        <Image Url="~/img/bt_cancelar.png" />
                                                    </CancelButton>
                                                    <ClearFilterButton Text="Limpar" Visible="True">
                                                        <Image Url="~/img/bt_limpa.png" />
                                                    </ClearFilterButton>
                                                </dxwgv:GridViewCommandColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="Aluno" FieldName="ALUNO" ReadOnly="true" VisibleIndex="1"
                                                    Visible="FALSE">
                                                    <PropertiesTextEdit>
                                                        <ReadOnlyStyle>
                                                            <Border BorderStyle="None"></Border>
                                                        </ReadOnlyStyle>
                                                    </PropertiesTextEdit>
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="Ano" FieldName="ANO" VisibleIndex="2" ReadOnly="true"
                                                    Width="30px">
                                                    <PropertiesTextEdit>
                                                        <ReadOnlyStyle>
                                                            <Border BorderStyle="None"></Border>
                                                        </ReadOnlyStyle>
                                                    </PropertiesTextEdit>
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="Período" FieldName="SEMESTRE" VisibleIndex="3"
                                                    ReadOnly="true" Width="50px">
                                                    <PropertiesTextEdit>
                                                        <ReadOnlyStyle>
                                                            <Border BorderStyle="None"></Border>
                                                        </ReadOnlyStyle>
                                                    </PropertiesTextEdit>
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="Turno" FieldName="DSC_TURNO" VisibleIndex="4"
                                                    ReadOnly="true" Width="100px">
                                                    <PropertiesTextEdit>
                                                        <ReadOnlyStyle>
                                                            <Border BorderStyle="None"></Border>
                                                        </ReadOnlyStyle>
                                                    </PropertiesTextEdit>
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="Turma" FieldName="TURMA" VisibleIndex="5"
                                                    ReadOnly="true">
                                                    <PropertiesTextEdit>
                                                        <ReadOnlyStyle>
                                                            <Border BorderStyle="None"></Border>
                                                        </ReadOnlyStyle>
                                                    </PropertiesTextEdit>
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="Disciplina" FieldName="DSC_DISCIPLINA" VisibleIndex="6"
                                                    ReadOnly="true">
                                                    <PropertiesTextEdit>
                                                        <ReadOnlyStyle>
                                                            <Border BorderStyle="None"></Border>
                                                        </ReadOnlyStyle>
                                                    </PropertiesTextEdit>
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="Série Referência" FieldName="SERIE_REFERENCIA"
                                                    ReadOnly="true" VisibleIndex="7" Width="100px">
                                                    <PropertiesTextEdit>
                                                        <ReadOnlyStyle>
                                                            <Border BorderStyle="None"></Border>
                                                        </ReadOnlyStyle>
                                                    </PropertiesTextEdit>
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="Disciplina Referência" FieldName="DISCIPLINA_REFERENCIA"
                                                    ReadOnly="true" VisibleIndex="8" Visible="FALSE">
                                                    <PropertiesTextEdit>
                                                        <ReadOnlyStyle>
                                                            <Border BorderStyle="None"></Border>
                                                        </ReadOnlyStyle>
                                                    </PropertiesTextEdit>
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="Disciplina Referência" FieldName="DSC_DISCIPLINA_REFERENCIA"
                                                    ReadOnly="true" VisibleIndex="9">
                                                    <PropertiesTextEdit>
                                                        <ReadOnlyStyle>
                                                            <Border BorderStyle="None"></Border>
                                                        </ReadOnlyStyle>
                                                    </PropertiesTextEdit>
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataComboBoxColumn VisibleIndex="10" Caption="Turma de Destino*" FieldName="NOVATURMA">
                                                    <PropertiesComboBox EnableSynchronization="False" EnableIncrementalFiltering="True"
                                                        ClientInstanceName="NOVATURMA">
                                                    </PropertiesComboBox>
                                                </dxwgv:GridViewDataComboBoxColumn>
                                            </Columns>
                                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                                        </dxwgv:ASPxGridView>
                                    </td>
                                </tr>
                            </table>
                            <asp:ObjectDataSource ID="odsProgressao" TypeName="Techne.Lyceum.Net.Academico.TransferenciaTurma"
                                runat="server" SelectMethod="ListarProgressao" OnUpdating="odsProgressao_Updating"
                                UpdateMethod="Update">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="tseAluno" PropertyName="DBValue" Name="aluno" />
                                    <asp:ControlParameter ControlID="txtAnoLetivo" PropertyName="Text" Name="ano" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlTurmaOptativaReforco" runat="server" GroupingText="Optativas/Reforço">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Panel ID="Panel3" runat="server">
                                            <dxwgv:ASPxGridView ID="grdOptativaReforco" runat="server" AutoGenerateColumns="False"
                                                ClientInstanceName="grdOptativaReforco" DataSourceID="odsOptativaReforco" KeyFieldName="ALUNO;ANO;SEMESTRE;TURMA"
                                                OnStartRowEditing="grdOptativaReforco_StartRowEditing" OnInitNewRow="grdOptativaReforco_InitNewRow"
                                                OnCellEditorInitialize="grdOptativaReforco_CellEditorInitialize" OnAfterPerformCallback="grdOptativaReforco_AfterPerformCallback">
                                                <SettingsEditing Mode="Inline" />
                                                <Columns>
                                                    <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                                        <UpdateButton Text="Salvar">
                                                            <Image Url="~/img/bt_salvar.png" />
                                                        </UpdateButton>
                                                        <EditButton Text="Editar" Visible="True">
                                                            <Image Url="~/img/bt_editar.png" />
                                                        </EditButton>
                                                        <CancelButton Text="Cancelar">
                                                            <Image Url="~/img/bt_cancelar.png" />
                                                        </CancelButton>
                                                        <ClearFilterButton Text="Limpar" Visible="True">
                                                            <Image Url="~/img/bt_limpa.png" />
                                                        </ClearFilterButton>
                                                    </dxwgv:GridViewCommandColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Aluno" FieldName="ALUNO" ReadOnly="true" VisibleIndex="1"
                                                        Visible="FALSE">
                                                        <PropertiesTextEdit>
                                                            <ReadOnlyStyle>
                                                                <Border BorderStyle="None"></Border>
                                                            </ReadOnlyStyle>
                                                        </PropertiesTextEdit>
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Ano" FieldName="ANO" VisibleIndex="2" ReadOnly="true"
                                                        Width="30px">
                                                        <PropertiesTextEdit>
                                                            <ReadOnlyStyle>
                                                                <Border BorderStyle="None"></Border>
                                                            </ReadOnlyStyle>
                                                        </PropertiesTextEdit>
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Período" FieldName="SEMESTRE" VisibleIndex="3"
                                                        ReadOnly="true" Width="50px">
                                                        <PropertiesTextEdit>
                                                            <ReadOnlyStyle>
                                                                <Border BorderStyle="None"></Border>
                                                            </ReadOnlyStyle>
                                                        </PropertiesTextEdit>
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Turma" FieldName="TURMA" VisibleIndex="4"
                                                        ReadOnly="true">
                                                        <PropertiesTextEdit>
                                                            <ReadOnlyStyle>
                                                                <Border BorderStyle="None"></Border>
                                                            </ReadOnlyStyle>
                                                        </PropertiesTextEdit>
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Disciplina" FieldName="NOME" VisibleIndex="5"
                                                        ReadOnly="true">
                                                        <PropertiesTextEdit>
                                                            <ReadOnlyStyle>
                                                                <Border BorderStyle="None"></Border>
                                                            </ReadOnlyStyle>
                                                        </PropertiesTextEdit>
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Data de Início" FieldName="DT_MATRICULA" ReadOnly="true"
                                                        VisibleIndex="7" Width="100px">
                                                        <PropertiesTextEdit>
                                                            <ReadOnlyStyle>
                                                                <Border BorderStyle="None"></Border>
                                                            </ReadOnlyStyle>
                                                        </PropertiesTextEdit>
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataComboBoxColumn VisibleIndex="10" Caption="Turma de Destino*" FieldName="NOVATURMA">
                                                        <PropertiesComboBox EnableSynchronization="False" EnableIncrementalFiltering="True"
                                                            ClientInstanceName="NOVATURMA">
                                                        </PropertiesComboBox>
                                                    </dxwgv:GridViewDataComboBoxColumn>
                                                </Columns>
                                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                                            </dxwgv:ASPxGridView>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                            <asp:ObjectDataSource ID="odsOptativaReforco" TypeName="Techne.Lyceum.Net.Academico.TransferenciaTurma"
                                runat="server" SelectMethod="ListaMatriculaOptativaReforcoPor" OnUpdating="odsOptativaReforco_Updating"
                                UpdateMethod="AtualizaTurmaDaMatriculaOptativaReforco">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="tseAluno" PropertyName="DBValue" Name="aluno" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnBotoesTransferencia" GroupingText="" runat="server">
                            <table>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblMotivo" runat="server" Text="Motivo:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlMotivo" runat="server" DataValueField="motivo_transf" DataTextField="descricao"
                                            Width="200px">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ErrorMessage="Motivo: Preenchimento obrigatório." ID="rfvMotivo"
                                            runat="server" ControlToValidate="ddlMotivo" InitialValue="" ValidationGroup="SalvarForm"
                                            Display="Dynamic"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dxe:ASPxButton ID="btnTransferir" runat="server" Text="Transferir" OnClick="btnTransferir_Click"
                                            ValidationGroup="SalvarForm" Visible="true">
                                        </dxe:ASPxButton>
                                    </td>
                                    <td>
                                        <dxe:ASPxButton ID="btnVoltar" runat="server" Text="Voltar" OnClick="btnVoltar_Click">
                                        </dxe:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <dxpc:ASPxPopupControl ID="ppcTransfTurma" runat="server" Modal="True" Width="700px"
                Height="500px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                ClientInstanceName="ppcTransfTurma" HeaderText="Aviso" ShowPageScrollbarWhenModal="true">
                <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,14000); }" />
                <ContentCollection>
                    <dxpc:PopupControlContentControl ID="pccTransfTurma" runat="server">
                        <asp:Panel ID="pnlScroll" runat="server" ScrollBars="auto" Width="700px" Height="500px">
                            <dxp:ASPxPanel ID="pnlTransfTurma" runat="server" DefaultButton="btnCancelar">
                                <PanelCollection>
                                    <dxp:PanelContent ID="pnFreqProvas" runat="server">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td align="center" valign="middle">
                                                    <asp:Label ID="lblMensagemTurma" Text="" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td align="center" valign="middle">
                                                    <asp:Label ID="lblMensagemTurmaPermuta" Text="" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:ImageButton ID="btnOK" SkinID="BcSalvar" runat="server" OnClick="btnOK_Click" />
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="btnCancelar" runat="server" SkinID="BcCancelar" OnClick="btnCancelar_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <br />
                                                </td>
                                            </tr>
                                        </table>
                                    </dxp:PanelContent>
                                </PanelCollection>
                            </dxp:ASPxPanel>
                        </asp:Panel>
                    </dxpc:PopupControlContentControl>
                </ContentCollection>
            </dxpc:ASPxPopupControl>
             <dxpc:ASPxPopupControl ID="pucConfirmar" ClientInstanceName="pucConfirmar" runat="server"
                Modal="true" ShowShadow="true" AllowDragging="false" AllowResize="false" ShowCloseButton="false"
                ShowFooter="false" ShowHeader="false" ShowSizeGrip="False" PopupHorizontalAlign="WindowCenter"
                PopupVerticalAlign="WindowCenter" EnableAnimation="true" Width="400px" ShowPageScrollbarWhenModal="true">
                <Border BorderColor="Gainsboro" BorderStyle="Ridge" BorderWidth="4px" />
                <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,15000); }" />
                <ContentCollection>
                    <dxpc:PopupControlContentControl>
                        <table>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblConfirmar" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    <asp:Button ID="btConfirma" runat="server" Text="Confirma" OnClick="btConfirma_Click" />
                                </td>
                                <td style="text-align: left;">
                                    <asp:Button ID="btCancelar" runat="server" Text="Cancelar" OnClientClick="pucConfirmar.Hide();" />
                                </td>
                            </tr>
                        </table>
                    </dxpc:PopupControlContentControl>
                </ContentCollection>
            </dxpc:ASPxPopupControl>

        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:HiddenField runat="server" ID="hdnAnoLetivo" />
    <asp:HiddenField runat="server" ID="hdnPeriodoLetivo" />
    <asp:HiddenField runat="server" ID="hdnGradeId" />
</asp:Content>
