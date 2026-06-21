<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Matricula.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.Matricula" %>

<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<asp:Content ID="cTurma" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:UpdatePanel ID="updatePanel3" runat="server" UpdateMode="Always">
        <ContentTemplate>

            <script type="text/javascript">

                var ASPxClientMenuBase;
                var ASPxClientPopupMenu;

                function abrirPopupDisciplina() {
                    window.setTimeout(function() {
                        pcDisciplinasMatricula.Show();
                    }, 1000);
                }
                function abrirPopupDisciplinaMaisEducacao() {
                    window.setTimeout(function() {
                        pcDisciplinasMaisEducacao.Show();
                    }, 1000);
                }
                function Bloqueio() {

                    var divBloqueio = document.getElementById("dvbloqueioMatricula");
                    divBloqueio.className = "Bloqueado";
                }     
     
            </script>

            <div id="dvbloqueioMatricula" class="Desbloqueado">
            </div>
            <table>
                <tr>
                    <td>
                        <asp:Panel ID="pnBusca" runat="server" GroupingText="Informe a matrícula ou o nome do aluno"
                            Width="620px">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblMatriculaTSearch" runat="server" Text="Matrícula:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <tweb:TSearch ID="tseAluno" runat="server" ValidateText="true" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAluno"
                                            AutoPostBack="true" OnTextChanged="tseAluno_Changed">
                                            <QueryParameters>
                                                <asp:Parameter Name="ativo" DefaultValue="Ativo" DbType="String" />
                                            </QueryParameters>
                                        </tweb:TSearch>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
                        <div class="divEditBlock" style="width: 740px;">
                            <asp:ImageButton ID="btnExcluir" runat="server" SkinID="BcDeletar" OnClick="btnExcluir_Click"
                                OnClientClick="return confirm('Confirma a remoção?');" />
                            <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" OnClick="btnNovo_Click" />
                            <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
                            <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcSalvar" OnClick="btnSalvar_Click"
                                OnClientClick="Bloqueio()" ValidationGroup="SalvarForm" />
                            <asp:Label runat="server" ID="lblBloco" Text="Matrículas" SkinID="BcTitulo" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlConcomitante" GroupingText="Educação Profissional Concomitante"
                            runat="server" Visible="false" Width="740px">
                            <table>
                                <tr>
                                    <td colspan="2">
                                        <dxe:ASPxCheckBox ID="chkConcomitante" ValueChecked="S" ValueUnchecked="N" ValueType="System.String"
                                            runat="server" Checked="false" Text="Liberado para Educação Profissional Concomitante"
                                            OnCheckedChanged="chkConcomitante_CheckedChanged" AutoPostBack="true">
                                        </dxe:ASPxCheckBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 70px">
                                        <asp:Label ID="lblUnidadeProfissionalizante" runat="server" Text="Unidade de Ensino Profissionalizante: "></asp:Label>
                                    </td>
                                    <td>
                                        <tweb:TSearchBox ID="tseUnidadeProfissionalizante" runat="server" Argument="nome_comp"
                                            Key="unidade_ens" SqlSelect="SELECT DISTINCT u.UNIDADE_ENS, u.NOME_COMP FROM LY_UNIDADE_ENSINO u INNER JOIN LY_UNIDADE_ENSINO_CURSOS uc ON uc.UNIDADE_ENS = u.UNIDADE_ENS INNER JOIN LY_CURSO c ON uc.CURSO = c.CURSO AND c.CONCOMITANTE = 'S' "
                                            MaxLength="8" SqlOrder="nome_comp">
                                            <GridColumns>
                                                <tweb:TSearchBoxColumn Caption="Código" FieldName="UNIDADE_ENS" Width="20%" />
                                                <tweb:TSearchBoxColumn Caption="Descrição" FieldName="NOME_COMP" Width="80%" />
                                            </GridColumns>
                                        </tweb:TSearchBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right">
                                        <asp:Button ID="btnSalvarUnidadeProfissionalizante" runat="server" Text="Salvar"
                                            OnClientClick="return confirm('Confirmo que recebi a declaração de vaga da unidade informada para módulo e curso solicitado.'); "
                                            OnClick="btnSalvarUnidadeProfissionalizante_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnSerieTurma" GroupingText="Dados Escolares" runat="server" Visible="false"
                            Width="740px">
                            <table>
                                <tr>
                                    <td align="right" style="width: 100px">
                                        <asp:Label ID="lblCurso" runat="server" Text="Escolaridade: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCurso" Enabled="false" runat="server" Width="150px" Visible="false"></asp:TextBox><asp:TextBox
                                            ID="txtNomeCurso" ReadOnly="true" runat="server" Width="250px"></asp:TextBox>
                                    </td>
                                    <td align="right" style="width: 70px">
                                        <asp:Label ID="lblCurriculo" runat="server" Text="Matriz Curricular: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCurriculo" ReadOnly="true" Width="230px" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblTurno" runat="server" Text="Turno: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurno" ReadOnly="true" runat="server" Width="150px" Visible="false"></asp:TextBox><asp:TextBox
                                            ID="txtNomeTurno" ReadOnly="true" runat="server" Width="250px"></asp:TextBox>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblSerie" runat="server" Text="Ano Escolar: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSerie" ReadOnly="true" Width="230px" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblUnidadeFisica" runat="server" Text="Unidade Física: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtUnidadeFisica" ReadOnly="true" runat="server" Width="250px"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtUnidadeFisicaDescr" ReadOnly="true" runat="server" Width="304px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblCoordenadoria" runat="server" Text="Regional: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNucleo" ReadOnly="true" runat="server" Width="250px"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtDecricaoNucleo" ReadOnly="true" runat="server" Width="304px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblFaculdade" runat="server" Text="Unidade de Ensino: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFaculdade" ReadOnly="true" runat="server" Width="250px"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtFaculdadeDescr" ReadOnly="true" runat="server" Width="304px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnAnoPeriodo" runat="server" GroupingText="Turma" Visible="false"
                            Width="740px">
                            <table>
                                <tr>
                                    <td align="right" style="width: 100px">
                                        <asp:Label ID="lblAno" runat="server" Text="Ano:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlAno" runat="server" DataValueField="ano" AutoPostBack="true"
                                            DataTextField="ano" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged" Width="250px">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvAno" runat="server" ControlToValidate="ddlAno"
                                            InitialValue="-1" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                                    </td>
                                    <td align="right" style="width: 70px">
                                        <asp:Label ID="lblSemestre" runat="server" Text="Período:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlSemestre" runat="server" DataValueField="periodo" AutoPostBack="true"
                                            DataTextField="id_reduzida" OnSelectedIndexChanged="ddlSemestre_SelectedIndexChanged"
                                            Width="150px">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvSemestre" runat="server" ControlToValidate="ddlSemestre"
                                            InitialValue="-1" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 70px">
                                        <asp:Label ID="Label2" runat="server" Text="Curso:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <tweb:TSearchBox ID="tseCurso" runat="server" Caption="" Key="curso" ArgumentColumns="60"
                                            Columns="10" OnChanged="tseCurso_Changed" MaxLength="20" Argument="nome" GridWidth="800px">
                                            <GridColumns>
                                                <tweb:TSearchBoxColumn Caption="Código" FieldName="curso" Width="20%" />
                                                <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome" Width="60%" />
                                                <tweb:TSearchBoxColumn Caption="Tipo Curso" FieldName="tipo_curso" Width="20%" />
                                            </GridColumns>
                                        </tweb:TSearchBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 70px">
                                        <asp:Label ID="Label3" runat="server" Text="Turno:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="ddlTurno" runat="server" AutoPostBack="True" DataTextField="descricao"
                                            DataValueField="turno" Width="100px" OnSelectedIndexChanged="ddlTurno_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlTurno"
                                            InitialValue="-1" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="width: 70px">
                                        <asp:Label ID="Label4" runat="server" Text="Ano de escolaridade:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="ddlSerie" runat="server" DataTextField="serie" AutoPostBack="true"
                                            DataValueField="serie" Width="100px" onchange="Bloqueio()" OnSelectedIndexChanged="ddlSerie_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlSerie"
                                            InitialValue="-1" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblTurma" runat="server" Text="Turma:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlTurma" runat="server" AutoPostBack="true" DataValueField="turma"
                                            DataTextField="turma" OnSelectedIndexChanged="ddlTurma_SelectedIndexChanged"
                                            Width="150px">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvTurma" runat="server" ControlToValidate="ddlTurma"
                                            InitialValue="-1" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                                        <asp:LinkButton ID="lnkVisualizarDisciplina" runat="server" OnClick="lnkVisualizarDisciplina_Click"
                                            Visible="false">Visualizar Disciplinas</asp:LinkButton>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblTipoCurso" runat="server" Text="Tipo Ens. Profissionalizante: "
                                            Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlTipoCurso" runat="server" Visible="false">
                                            <asp:ListItem Text="Selecione" Value=""></asp:ListItem>
                                            <asp:ListItem Text="Subsequente" Value="Subsequente"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlTurmaProfissionalizante" GroupingText="Turma Profissionalizante Concomitante"
                            runat="server" Visible="false" Width="740px">
                            <table>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblUnidadeEnsinoConcomitanteTitulo" runat="server" Text="Unidade de Ensino: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblUnidadeEnsinoConcomitante" runat="server" Width="250px"></asp:Label>
                                        <asp:Label ID="lblNomeUnidadeEnsinoConcomitante" runat="server" Width="250px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblAnoConcomitanteTitulo" runat="server" Text="Ano: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblAnoConcomitante" ReadOnly="true" runat="server" Width="250px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblPeriodoConcomitante" Text="Período: " runat="server"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlPeriodoConcomitante" runat="server" DataTextField="PERIODO"
                                            DataValueField="PERIODO" Width="250px" AutoPostBack="true" OnSelectedIndexChanged="ddlPeriodoConcomitante_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblCursoConcomitanteTse" Text="Curso: " runat="server"></asp:Label>
                                    </td>
                                    <td colspan="2">
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
                                        <asp:Label ID="lblTurnoConcomitante" Text="Turno: " runat="server"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlTurnoConcomitante" runat="server" AutoPostBack="True" DataTextField="descricao"
                                            DataValueField="turno" Width="250px" OnSelectedIndexChanged="ddlTurnoConcomitante_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblSerieConcomitante" Text="Ano Escolar: " runat="server"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlSerieConcomitante" runat="server" DataTextField="serie"
                                            AutoPostBack="true" onchange="Bloqueio()" DataValueField="serie" Width="250px"
                                            OnSelectedIndexChanged="ddlSerieConcomitante_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblTurmaConcomitante" runat="server" Text="Turma:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlTurmaConcomitante" runat="server" DataTextField="turma"
                                            DataValueField="turma" AutoPostBack="false">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right">
                                        <asp:Button ID="btnSalvarTurmaProfissionalizante" runat="server" Text="Salvar Turma"
                                            OnClick="btnSalvarTurmaProfissionalizante_Click" OnClientClick="Bloqueio()" />
                                    </td>
                                    <td align="right">
                                        <asp:Button ID="btnExcluirTurmaProfissionalizante" runat="server" Text="Excluir Turma"
                                            OnClick="btnExcluirTurmaProfissionalizante_Click" OnClientClick="Bloqueio()" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlTurmaMaisEducacao" Visible="false" runat="server" GroupingText="Turma Mais Educação"
                            Width="740px">
                            <table>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblDscCursoMaisEducacao" Text="Curso: " runat="server"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="lblCursoMaisEducacao" runat="server"></asp:Label>
                                        -
                                        <asp:Label ID="lblNomeCursoMaisEducacao" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblDscAnoMaisEducacao" Text="Ano: " runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblAnoMaisEducacao" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblDscPeriodoMaisEducacao" Text="Período: " runat="server"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlPeriodoMaisEducacao" runat="server" DataTextField="PERIODO"
                                            DataValueField="PERIODO" Enabled="false" Width="250px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblDscTurnoMaisEducacao" Text="Turno: " runat="server"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlTurnoMaisEducacao" runat="server" AutoPostBack="True" DataTextField="descricao"
                                            DataValueField="turno" Width="250px" OnSelectedIndexChanged="ddlTurnoMaisEducacao_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lbldscSerieMaisEducacao" Text="Ano de Escolaridade: " runat="server"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlSerieMaisEducacao" runat="server" DataTextField="serie"
                                            AutoPostBack="true" DataValueField="serie" Width="250px" onchange="Bloqueio()"
                                            OnSelectedIndexChanged="ddlSerieMaisEducacao_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblDscTurmaMaisEducacao" runat="server" Text="Turma:* " SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlTurmaMaisEducacao" runat="server" DataTextField="turma"
                                                DataValueField="turma" AutoPostBack="true" OnSelectedIndexChanged="ddlTurmaMaisEducacao_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:LinkButton ID="lnkVisualizarDisciplinaMaisEducacao" runat="server" OnClick="lnkVisualizarDisciplinaMaisEducacao_Click"
                                                Visible="false">Visualizar Disciplinas</asp:LinkButton>
                                        </td>
                                    </tr>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right">
                                        <asp:Button ID="btnSalvarTurmaMaisEducacao" runat="server" Text="Salvar Turma" OnClick="btnSalvarTurmaMaisEducacao_Click"
                                            OnClientClick="Bloqueio()" />
                                    </td>
                                    <td align="right">
                                        <asp:Button ID="btnExcluirTurmaMaisEducacao" runat="server" Text="Excluir Turma"
                                            OnClick="btnExcluirTurmaMaisEducacao_Click" OnClientClick="Bloqueio()" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlEletivas" runat="server" GroupingText="Eletivas" Visible="false"
                            Width="740px">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Panel ID="Panel3" runat="server">
                                            <dxwgv:ASPxGridView ID="grdEletivas" runat="server" AutoGenerateColumns="False" ClientInstanceName="grdEletivas"
                                                DataSourceID="odsEletivas" KeyFieldName="ALUNO;TURMA;ANO;SEMESTRE" OnAfterPerformCallback="grdEletivaso_AfterPerformCallback">
                                                <SettingsBehavior ConfirmDelete="True" />
                                                <SettingsEditing Mode="Inline" />
                                                <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                                <Columns>
                                                    <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                                        <CancelButton Text="Cancelar">
                                                            <Image Url="~/img/bt_cancelar.png" />
                                                        </CancelButton>
                                                        <ClearFilterButton Text="Limpar" Visible="True">
                                                            <Image Url="~/img/bt_limpa.png" />
                                                        </ClearFilterButton>
                                                    </dxwgv:GridViewCommandColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Aluno" FieldName="ALUNO" ReadOnly="true" VisibleIndex="1"
                                                        Visible="FALSE">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Ano" FieldName="ANO" VisibleIndex="2" Width="30px">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Período" FieldName="SEMESTRE" VisibleIndex="3"
                                                        Width="50px">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Turno" FieldName="DESCRICAOTURNO" VisibleIndex="4">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Turma" FieldName="TURMA" VisibleIndex="5">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Disciplina" FieldName="DISCIPLINA" VisibleIndex="6">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Grupo Eletiva" FieldName="GRUPO" VisibleIndex="7"
                                                        Width="50px">
                                                    </dxwgv:GridViewDataTextColumn>
                                                </Columns>
                                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                                            </dxwgv:ASPxGridView>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="btnExluirEletivas" runat="server" Text="Excluir Eletivas" OnClick="btnExluirEletivas_Click"
                                            OnClientClick="Bloqueio()" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:ObjectDataSource ID="odsEletivas" TypeName="Techne.Lyceum.Net.Academico.Matricula"
                                runat="server" SelectMethod="ListarEletivas">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="tseAluno" PropertyName="DBValue" Name="aluno" />
                                    <asp:ControlParameter ControlID="pnlEletivas" PropertyName="Visible" Name="painel" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlProgressao" runat="server" GroupingText="Progressão Parcial" Visible="false"
                            Width="740px">
                            <table>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblAnoProgressao" runat="server" Text="Ano:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlAnoProgressao" runat="server" DataValueField="ano" AutoPostBack="true"
                                            DataTextField="ano" OnSelectedIndexChanged="ddlAnoProgressao_SelectedIndexChanged"
                                            Width="250px" Enabled="false">
                                        </asp:DropDownList>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblPeridoProgressao" runat="server" Text="Período:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlPeriodoProgressao" runat="server" DataValueField="id_reduzida"
                                            AutoPostBack="true" DataTextField="id_reduzida" OnSelectedIndexChanged="ddlPeriodoProgressao_SelectedIndexChanged"
                                            Width="150px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblTurnoProgressao" runat="server" Text="Turno:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlTurnoProgressao" runat="server" AutoPostBack="true" DataValueField="TURNO"
                                            DataTextField="DESCRICAO" OnSelectedIndexChanged="ddlTurnoProgressao_SelectedIndexChanged"
                                            Width="150px" onchange="Bloqueio()">
                                        </asp:DropDownList>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblTurmaProgressao" runat="server" Text="Turma:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlTurmaProgressao" runat="server" AutoPostBack="true" DataValueField="turma"
                                            DataTextField="turma" OnSelectedIndexChanged="ddlTurmaProgressao_SelectedIndexChanged"
                                            Width="150px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblDisciplina" Text="Disciplina:* " runat="server" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <tweb:TSearchBox ID="tseDisciplinaProgressao" runat="server" Caption="" AutoPostBack="true"
                                            ArgumentColumns="60" Columns="20" OnChanged="tseDisciplinaProgressao_Changed"
                                            MaxLength="20" GridWidth="800px" SqlOrder="NOME_COMPL">
                                            <GridColumns>
                                                <tweb:TSearchBoxColumn Caption="Código" FieldName="DISCIPLINA" Width="15%" />
                                                <tweb:TSearchBoxColumn Caption="Disciplina" FieldName="NOME_COMPL" Width="35%" />
                                                <tweb:TSearchBoxColumn Caption="Matriz Curricular" FieldName="CURRICULO" Width="20%" />
                                            </GridColumns>
                                        </tweb:TSearchBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblSerieProgressao" runat="server" Text="Série Referência:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlSerieProgressao" runat="server" AutoPostBack="true" DataValueField="serie"
                                            DataTextField="serie" OnSelectedIndexChanged="ddlSerieProgressao_SelectedIndexChanged"
                                            Width="150px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="Label1" Text="Disciplina Referência:* " runat="server" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <tweb:TSearchBox ID="tseDisciplinaReferencia" runat="server" Caption="" AutoPostBack="true"
                                            ArgumentColumns="60" Columns="10" MaxLength="20" GridWidth="800px" SqlOrder="NOME_COMPL"
                                            Key="disciplina">
                                            <GridColumns>
                                                <tweb:TSearchBoxColumn Caption="Código" FieldName="disciplina" Width="10%" />
                                                <tweb:TSearchBoxColumn Caption="Disciplina" FieldName="NOME_COMPL" Width="50%" />
                                            </GridColumns>
                                        </tweb:TSearchBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" style="text-align: right;">
                                        <asp:Button ID="btnSalvarProgressao" runat="server" OnClick="btnSalvarProgressao_Click"
                                            Text="Salvar" ValidationGroup="SalvarForm" OnClientClick="Bloqueio()" />
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnGrid" runat="server">
                                            <dxwgv:ASPxGridView ID="grdProgressao" runat="server" AutoGenerateColumns="False"
                                                ClientInstanceName="grdProgressao" DataSourceID="odsProgressao" KeyFieldName="ALUNO;DISCIPLINA;TURMA;ANO;SEMESTRE"
                                                OnAfterPerformCallback="grdProgressao_AfterPerformCallback" OnStartRowEditing="grdProgressao_StartRowEditing"
                                                OnInitNewRow="grdProgressao_InitNewRow">
                                                <SettingsBehavior ConfirmDelete="True" />
                                                <SettingsEditing Mode="Inline" />
                                                <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                                <Columns>
                                                    <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                                        <CancelButton Text="Cancelar">
                                                            <Image Url="~/img/bt_cancelar.png" />
                                                        </CancelButton>
                                                        <DeleteButton Text="Remover" Visible="True">
                                                            <Image Url="~/img/bt_exclui2.png" />
                                                        </DeleteButton>
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
                                                    <dxwgv:GridViewDataTextColumn Caption="Ano" FieldName="ANO" VisibleIndex="2" Width="30px">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Período" FieldName="SEMESTRE" VisibleIndex="3"
                                                        Width="50px">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Turno" FieldName="DSC_TURNO" VisibleIndex="4">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Turma" FieldName="TURMA" VisibleIndex="5">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Codigo_disc" FieldName="DISCIPLINA" VisibleIndex="6"
                                                        Visible="false">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Disciplina" FieldName="DSC_DISCIPLINA" VisibleIndex="6">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Série Referência" FieldName="SERIE_REFERENCIA"
                                                        VisibleIndex="7">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Disciplina Referência" FieldName="DSC_DISCIPLINA_REFERENCIA"
                                                        VisibleIndex="8">
                                                    </dxwgv:GridViewDataTextColumn>
                                                </Columns>
                                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                                            </dxwgv:ASPxGridView>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:ObjectDataSource ID="odsProgressao" TypeName="Techne.Lyceum.Net.Academico.Matricula"
                                runat="server" SelectMethod="ListarProgressao" OnDeleting="odsProgressao_Deleting"
                                DeleteMethod="Delete">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="tseAluno" PropertyName="DBValue" Name="aluno" />
                                    <asp:ControlParameter ControlID="pnlProgressao" PropertyName="Visible" Name="painel" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblMensagem2" runat="server" SkinID="lblMensagem"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlTurmaOptativaReforco" Visible="false" runat="server" GroupingText="Turma Optativa / Reforço"
                            Width="740px">
                            <table>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblAnoOptativaReforcoTitulo" Text="Ano: " runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblAnoOptativaReforco" runat="server" Width="250px"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblPeriodoOptativaReforcoTitulo" Text="Período: " runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPeriodoOptativaReforco" runat="server" Width="150px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblTurnoOptativaReforcoTitulo" Text="Turno:* " runat="server" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlTurnoOptativaReforco" runat="server" AutoPostBack="True"
                                            DataTextField="descricao" DataValueField="turno" Width="150px" OnSelectedIndexChanged="ddlTurnoOptativaReforco_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblTurmaOptativaReforcoTitulo" Text="Turma:* " runat="server" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlTurmaOptativaReforco" runat="server" AutoPostBack="True"
                                            DataTextField="turma" DataValueField="turma" Width="150px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="right">
                                        <br />
                                        <asp:Button ID="btnSalvarTurmaOptativaReforco" runat="server" Text="Salvar Turma"
                                            OnClick="btnSalvarTurmaOptativaReforco_Click" />
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Panel ID="Panel2" runat="server">
                                            <dxwgv:ASPxGridView ID="grdOptativaReforco" runat="server" AutoGenerateColumns="False"
                                                ClientInstanceName="grdOptativaReforco" DataSourceID="odsOptativaReforco" KeyFieldName="ALUNO;ANO;SEMESTRE;TURMA"
                                                OnAfterPerformCallback="grdOptativaReforco_AfterPerformCallback" OnStartRowEditing="grdOptativaReforco_StartRowEditing"
                                                OnInitNewRow="grdOptativaReforco_InitNewRow">
                                                <SettingsBehavior ConfirmDelete="True" />
                                                <SettingsEditing Mode="Inline" />
                                                <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                                <Columns>
                                                    <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                                        <CancelButton Text="Cancelar">
                                                            <Image Url="~/img/bt_cancelar.png" />
                                                        </CancelButton>
                                                        <DeleteButton Text="Remover" Visible="True">
                                                            <Image Url="~/img/bt_exclui2.png" />
                                                        </DeleteButton>
                                                        <ClearFilterButton Text="Limpar" Visible="True">
                                                            <Image Url="~/img/bt_limpa.png" />
                                                        </ClearFilterButton>
                                                    </dxwgv:GridViewCommandColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Aluno" FieldName="ALUNO" ReadOnly="true" VisibleIndex="1"
                                                        Visible="false">
                                                        <PropertiesTextEdit>
                                                            <ReadOnlyStyle>
                                                                <Border BorderStyle="None"></Border>
                                                            </ReadOnlyStyle>
                                                        </PropertiesTextEdit>
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Ano" FieldName="ANO" VisibleIndex="2" Width="30px">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Período" FieldName="SEMESTRE" VisibleIndex="3"
                                                        Width="50px">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Turma" FieldName="TURMA" VisibleIndex="4">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Disciplina" FieldName="NOME" VisibleIndex="5">
                                                    </dxwgv:GridViewDataTextColumn>
                                                    <dxwgv:GridViewDataTextColumn Caption="Data de Início" FieldName="DT_MATRICULA" VisibleIndex="6">
                                                    </dxwgv:GridViewDataTextColumn>
                                                </Columns>
                                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                                            </dxwgv:ASPxGridView>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:ObjectDataSource ID="odsOptativaReforco" TypeName="Techne.Lyceum.Net.Academico.Matricula"
                                runat="server" SelectMethod="ListaMatriculaOptativaReforcoPor" OnDeleting="odsOptativaReforco_Deleting"
                                DeleteMethod="RemoveMatriculaOptativaReforco">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="tseAluno" PropertyName="DBValue" Name="aluno" />
                                    <asp:ControlParameter ControlID="ddlAno" DefaultValue="" Name="ano" PropertyName="SelectedValue" />
                                    <asp:ControlParameter ControlID="ddlSemestre" DefaultValue="" Name="semestre" PropertyName="SelectedValue" />
                                    <asp:ControlParameter ControlID="pnlTurmaOptativaReforco" PropertyName="Visible"
                                        Name="painel" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </asp:Panel>
                    </td>
                </tr>                
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <dxpc:ASPxPopupControl ID="pcDisciplinasMatricula" runat="server" Modal="True" Width="600"
        Height="350" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        CloseAction="CloseButton" ClientInstanceName="pcDisciplinasMatricula" HeaderText="Disciplinas Ativas"
        AllowDragging="True" EnableAnimation="False" EnableViewState="True">
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,12000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl ID="ppDisciplinasMatricula" runat="server">
                <asp:UpdatePanel ID="updatePanel9" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <dxp:ASPxPanel ID="ASPxPanel2" runat="server" DefaultButton="btnRemoveDisciplinasMatricula">
                            <PanelCollection>
                                <dxp:PanelContent ID="PanelContent3" runat="server">
                                    <table cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td align="center">
                                                <dxwgv:ASPxGridView ID="grdDisciplinasAtivas" runat="server" KeyFieldName="DISCIPLINA"
                                                    ClientInstanceName="grdDisciplinasAtivas" EnableCallBacks="false" AutoGenerateColumns="False"
                                                    Width="95%" Font-Names="Verdana" Font-Size="Small" OnPageIndexChanged="grdDisciplinasAtivas_PageIndexChanged">
                                                    <SettingsText EmptyDataRow="Não existem dados." />
                                                    <SettingsPager PageSize="15" />
                                                    <Columns>
                                                        <dxwgv:GridViewDataColumn FieldName="DISCIPLINA" Caption="Código" VisibleIndex="0" />
                                                        <dxwgv:GridViewDataColumn FieldName="NOME" Caption="Disciplina" VisibleIndex="0" />
                                                    </Columns>
                                                </dxwgv:ASPxGridView>
                                            </td>
                                        </tr>
                                    </table>
                                </dxp:PanelContent>
                            </PanelCollection>
                        </dxp:ASPxPanel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
        <ContentStyle>
            <Paddings PaddingBottom="5px" />
        </ContentStyle>
    </dxpc:ASPxPopupControl>
    <dxpc:ASPxPopupControl ID="pcDisciplinasMaisEducacao" runat="server" Modal="True"
        Width="600" Height="350" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        CloseAction="CloseButton" ClientInstanceName="pcDisciplinasMaisEducacao" HeaderText="Disciplinas Ativas"
        AllowDragging="True" EnableAnimation="False" EnableViewState="True">
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,13000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <dxp:ASPxPanel ID="ASPxPanel1" runat="server" DefaultButton="btnRemoveDisciplinasMaisEducacao">
                            <PanelCollection>
                                <dxp:PanelContent ID="PanelContent1" runat="server">
                                    <table cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td align="center">
                                                <dxwgv:ASPxGridView ID="grdDisciplinasMaisEducacaoAtivas" runat="server" ClientInstanceName="grdDisciplinasMaisEducacaoAtivas"
                                                    KeyFieldName="disciplina" AutoGenerateColumns="False" Width="95%" Font-Names="Verdana"
                                                    Font-Size="Small" OnPageIndexChanged="grdDisciplinasMaisEducacaoAtivas_PageIndexChanged">
                                                    <SettingsText EmptyDataRow="Não existem dados." />
                                                    <SettingsPager PageSize="15" />
                                                    <Columns>
                                                        <dxwgv:GridViewDataColumn FieldName="disciplina" Caption="Código" VisibleIndex="0" />
                                                        <dxwgv:GridViewDataColumn FieldName="nome" Caption="Disciplina" VisibleIndex="0" />
                                                    </Columns>
                                                </dxwgv:ASPxGridView>
                                            </td>
                                        </tr>
                                    </table>
                                </dxp:PanelContent>
                            </PanelCollection>
                        </dxp:ASPxPanel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
        <ContentStyle>
            <Paddings PaddingBottom="5px" />
        </ContentStyle>
    </dxpc:ASPxPopupControl>
</asp:Content>
