<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="FechamentoMatricula.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.FechamentoMatricula" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<asp:Content ID="ctFechamentoMatricula" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">

        function validarDependencias() {
            var disciplinas = $("#<%= this.grdDependencia.ClientID %> input:checked").length;

            var totalDep = $('#<%=hdnQtdeDep.ClientID %>').val();

            if (disciplinas == 0
                || disciplinas > totalDep) {
                alert("É necessário selecionar de 1 a " + totalDep + " disciplinas para dependência.");

                return false;
            }

            return true;
        }

        function ConfirmaExecucao() {

            var alunos = $("#<%= this.grdMatriculas.ClientID %> input:checked").length;

            if (alunos == 0) {
                alert("Selecione os alunos.");

                return false;
            }

            var situacao = "";

            if ($("#<%= this.rbAprovar.ClientID %>_I").prop("checked")) {
                situacao = "aprovando";
            }
            else if ($("#<%= this.rbAprovarComDep.ClientID %>_I").prop("checked")) {
                situacao = "aprovando com dependência";
            }
            else if ($("#<%= this.rbReprovadoFalta.ClientID %>_I").prop("checked")) {
                situacao = "reprovando por falta";
            }
            else if ($("#<%= this.rbReprovadoNota.ClientID %>_I").prop("checked")) {
                situacao = "reprovando por nota";
            }
            else if ($("#<%= this.rbRetido.ClientID %>_I").prop("checked")) {
                situacao = "retido";
            }
            else if ($("#<%= this.rbPromovido.ClientID %>_I").prop("checked")) {
                situacao = "promovido";
            }

            var txtTurma = $("#<%= this.txtTurma.ClientID %>");

            if ($("#<%= this.rdSim.ClientID %>_I").prop("checked")) {
                var ddlTurma = $("#<%= ddlTurma.ClientID %>");

                if (!$(ddlTurma).is(":enabled")
                    || $(ddlTurma).val() == "") {
                    alert("Selecione todos os dados da nova matrícula.");

                    return false;
                }

                var turmaDestino = $(ddlTurma).val();
                //var txtUnidadeEns = $("#<%= txtUnidadeEns.ClientID %>");
                var ddlUnidadeEnsino = document.getElementById("<%=ddlUnidadeEnsino.ClientID%>");
                var unidadeDestino = ddlUnidadeEnsino.options[ddlUnidadeEnsino.selectedIndex].text;

                return confirm("Este processo finalizará o fechamento de " + alunos + " alunos da turma " + $(txtTurma).val() + ", " + situacao + " para a turma " + turmaDestino + " da unidade " + unidadeDestino + ".");
            }
            else {
                return confirm("Este processo finalizará o fechamento de " + alunos + " alunos da turma " + $(txtTurma).val() + ", " + situacao + ".");
            }
        }

        function ConfirmaExecucaoDependencia() {

            var alunos = $("#<%= this.grdProgressaoParcial.ClientID %> input:checked").length;

            if (alunos == 0) {
                alert("Selecione os alunos.");

                return false;
            }

            var situacao = "";

            if ($("#<%= this.rbAprovadoDependencia.ClientID %>_I").prop("checked")) {
                situacao = "aprovando";
            }
            else if ($("#<%= this.rbReprovadoDependencia.ClientID %>_I").prop("checked")) {
                situacao = "reprovando";
            }

            var txtTurma = $("#<%= this.txtTurma.ClientID %>");

            return confirm("Este processo finalizará o fechamento da Dependência de " + alunos + " alunos da turma " + $(txtTurma).val() + ", " + situacao + ".");

        }

        function VerificaSituacao(situacao, fieldcO) {

            var ano = $('#<%=txtAno.ClientID %>').val();
            var periodo = $('#<%=txtPeriodo.ClientID %>').val();
            if (ano == 2020 || ano == 2021) {
                if (situacao == "AprovadoDep" || situacao == "ReprovadoNota" || situacao == "ReprovadoFalta") {

                    alert('PARA ESTE ANO, EXCEPCIONALMENTE, NÃO PODERÁ SER INFORMADA ESTA SITUAÇÃO');
                    $(fieldcO).prop('checked', false);
                }
            }

        }

        function ConfirmaExecucaoEletiva() {

            var alunos = $("#<%= this.grdEletivas.ClientID %> input:checked").length;

            if (alunos == 0) {
                alert("Selecione os alunos.");

                return false;
            }

            var situacao = "";

            if ($("#<%= this.rbAprovadoEletiva.ClientID %>_I").prop("checked")) {
                situacao = "aprovando";
            }


            var txtTurma = $("#<%= this.txtTurma.ClientID %>");

            return confirm("Este processo finalizará o fechamento da Eletiva de " + alunos + " alunos da turma " + $(txtTurma).val() + ", " + situacao + ".");

        }
  
    </script>

    <asp:HiddenField ID="hdnEletiva" runat="server" />
    <asp:HiddenField ID="hdnQtdeDep" runat="server" />
    <asp:HiddenField ID="hdnSerieConcluinte" runat="server" />
    <asp:HiddenField ID="hdnRegional" runat="server" />
    <asp:ScriptManagerProxy ID="ScriptManagerProxy" runat="server" />
    <asp:Label ID="lblMensagemRelatorioTurmaInvalida" Visible="false" SkinID="lblMensagem"
        runat="server" />
    <asp:Panel ID="pnDadosTurma" runat="server" GroupingText="Dados da Turma" Width="850">
        <asp:TextBox ID="txtUnidadeEnsVal" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="txtTurmaTop" runat="server" Visible="false" Width="164px"></asp:TextBox>
        <asp:TextBox ID="txtAnoTop" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="txtPeriodoTop" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="txtEscolaridadeVal" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="txtTurnoVal" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="txtCurriculoTop" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="txtSerieVal" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="txtOptativaReforco" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="txtSituacaoTurma" runat="server" Visible="false"></asp:TextBox>
        <table>
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="lblUnidadeEns" Text="Unidade de Ensino: " runat="server"></asp:Label>
                </td>
                <td colspan="5">
                    <asp:TextBox ID="txtUnidadeEns" runat="server" ReadOnly="true" Width="390px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="lblTurma" Text="Turma: " runat="server"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtTurma" runat="server" ReadOnly="true" Width="164px"></asp:TextBox>
                </td>
                <td style="text-align: right;">
                    <asp:Label ID="lblAno" Text="Ano: " runat="server"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtAno" runat="server" ReadOnly="true"></asp:TextBox>
                </td>
                <td style="text-align: right;">
                    <asp:Label ID="lblPeriodo" Text="Período: " runat="server"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPeriodo" runat="server" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="lblEscolaridade" Text="Escolaridade: " runat="server"></asp:Label>
                </td>
                <td colspan="5">
                    <asp:TextBox ID="txtEscolaridade" runat="server" ReadOnly="true" Width="390px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="lblTurno" Text="Turno: " runat="server"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtTurno" runat="server" ReadOnly="true" Width="164px"></asp:TextBox>
                </td>
                <td style="text-align: right;">
                    <asp:Label ID="lblCurriculo" Text="Matriz Curricular: " runat="server"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtCurriculo" runat="server" ReadOnly="true"></asp:TextBox>
                </td>
                <td style="text-align: right;">
                    <asp:Label ID="lblSerie" Text="Ano de Escolaridade: " runat="server"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtSerie" runat="server" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="lblDataInicio" Text="Data Início: " runat="server"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDataInicio" runat="server" ReadOnly="true" Width="164px"></asp:TextBox>
                </td>
                <td style="text-align: right;">
                    <asp:Label ID="lblDataFim" Text="Data Fim: " runat="server"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDataFim" runat="server" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <asp:ObjectDataSource ID="odsFechamentoMatricula" runat="server" TypeName="" SelectMethod="">
        </asp:ObjectDataSource>
    </asp:Panel>
    <br />
    <dxe:ASPxButton ID="btnVoltar2" runat="server" Text="Voltar" OnClick="btnVoltar_Click">
    </dxe:ASPxButton>
    <asp:UpdatePanel ID="upnlMatriculas" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
            <br />
            <asp:Panel ID="pnlProgressaoParcial" GroupingText="Progressão Parcial" runat="server"
                ScrollBars="Auto" Width="850">
                <table>
                    <tr>
                        <td class="style1">
                            <asp:Panel ID="pnlGridProgressaoParcial" runat="server" ScrollBars="Auto" Width="600px"
                                Height="200px">
                                <asp:ObjectDataSource ID="odsProgressaoParcial" runat="server" TypeName="Techne.Lyceum.RN.FechamentoMatricula"
                                    SelectMethod="ListarAlunosProgressaoParcial">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="txtAnoTop" Name="ano" PropertyName="Text" />
                                        <asp:ControlParameter ControlID="txtPeriodoTop" Name="semestre" PropertyName="Text" />
                                        <asp:ControlParameter ControlID="txtEscolaridadeVal" Name="curso" PropertyName="Text" />
                                        <asp:ControlParameter ControlID="txtTurnoVal" Name="turno" PropertyName="Text" />
                                        <asp:ControlParameter ControlID="txtCurriculoTop" Name="curriculo" PropertyName="Text" />
                                        <asp:ControlParameter ControlID="txtSerieVal" Name="serie" PropertyName="Text" />
                                        <asp:ControlParameter ControlID="txtTurmaTop" Name="turma" PropertyName="Text" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                                <dxwgv:ASPxGridView ID="grdProgressaoParcial" ClientInstanceName="grdProgressaoParcial"
                                    Visible="true" DataSourceID="odsProgressaoParcial" KeyFieldName="CHAVE" runat="server"
                                    EnableRowsCache="true" EnableCallBacks="false" Width="544px" OnSelectionChanged="grdProgressaoParcial_SelectionChanged">
                                    <SettingsPager Mode="ShowAllRecords" />
                                    <SettingsBehavior ProcessSelectionChangedOnServer="true" AllowSort="false" AllowDragDrop="false"
                                        AllowMultiSelection="false" AllowGroup="false" />
                                    <Columns>
                                        <dxwgv:GridViewDataTextColumn Caption="" VisibleIndex="0" Name="selecionar" Width="30px">
                                            <DataItemTemplate>
                                                <dxe:ASPxCheckBox ID="chkBox" runat="server" Width="30px">
                                                </dxe:ASPxCheckBox>
                                            </DataItemTemplate>
                                            <CellStyle HorizontalAlign="Right">
                                            </CellStyle>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Nº" FieldName="num_chamada" VisibleIndex="1"
                                            Visible="false">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Matricula" FieldName="aluno" VisibleIndex="2">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Aluno" FieldName="nome_compl" VisibleIndex="3"
                                            Width="150px">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewCommandColumn VisibleIndex="4" ButtonType="Image" Width="7%" Caption="Foto">
                                            <SelectButton Text="Selecionar" Visible="True">
                                                <Image Url="~/Images/bt_foto.png" />
                                            </SelectButton>
                                        </dxwgv:GridViewCommandColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Situação" FieldName="sit_matgrade" VisibleIndex="5">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Disciplina" FieldName="DISCIPLINA" Visible="false"
                                            VisibleIndex="6">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Disciplina" FieldName="nomeDisciplina" VisibleIndex="7">
                                        </dxwgv:GridViewDataTextColumn>
                                    </Columns>
                                </dxwgv:ASPxGridView>
                            </asp:Panel>
                            <br />
                            <br />
                            <table>
                                <tr>
                                    <td>
                                        <dxe:ASPxButton ID="btnMarcarTodasDependencia" Text="Marcar Todos" runat="server"
                                            OnClick="btnMarcarTodasDependencia_Click">
                                        </dxe:ASPxButton>
                                    </td>
                                    <td>
                                        <dxe:ASPxButton ID="btnDesMarcarTodasDependencia" Text="Desmarcar Todos" runat="server"
                                            OnClick="btnDesMarcarTodasDependencia_Click">
                                        </dxe:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <asp:Label ID="lblSituacaoProgressaoParcial" runat="server" Text="Situação final dos alunos selecionados:"></asp:Label>
                            <dxe:ASPxRadioButton ID="rbAprovadoDependencia" runat="server" Text="Aprovado" AutoPostBack="false"
                                GroupName="executar">
                            </dxe:ASPxRadioButton>
                            <dxe:ASPxRadioButton ID="rbReprovadoDependencia" runat="server" Text="Reprovado"
                                AutoPostBack="false" GroupName="executar">
                            </dxe:ASPxRadioButton>
                            <asp:ImageButton ID="btnExecutarDependencia" runat="server" SkinID="Confirmar" ToolTip="Aprovar alunos selecionados"
                                OnClientClick="return ConfirmaExecucaoDependencia();" OnClick="btnExecutarDependencia_Click"
                                ImageUrl="~/Images/bot_confirmar.png" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <br />
            <asp:Panel ID="pnlEletivas" GroupingText="Eletivas" runat="server" ScrollBars="Auto"
                Width="850">
                <table>
                    <tr>
                        <td class="style1">
                            <asp:Panel ID="pnlGridEletivas" runat="server" ScrollBars="Auto" Width="600px" Height="200px">
                                <asp:ObjectDataSource ID="odsEletivas" runat="server" TypeName="Techne.Lyceum.Net.Academico.FechamentoMatricula"
                                    SelectMethod="ListarAlunosEletivas">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="txtAnoTop" Name="ano" PropertyName="Text" />
                                        <asp:ControlParameter ControlID="txtPeriodoTop" Name="semestre" PropertyName="Text" />
                                        <asp:ControlParameter ControlID="txtTurmaTop" Name="turma" PropertyName="Text" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                                <dxwgv:ASPxGridView ID="grdEletivas" ClientInstanceName="grdEletivas" Visible="true"
                                    DataSourceID="odsEletivas" KeyFieldName="CHAVE" runat="server" EnableRowsCache="true"
                                    EnableCallBacks="false" Width="544px" OnSelectionChanged="grdEletivas_SelectionChanged">
                                    <SettingsPager Mode="ShowAllRecords" />
                                    <SettingsBehavior ProcessSelectionChangedOnServer="true" AllowSort="false" AllowDragDrop="false"
                                        AllowMultiSelection="false" AllowGroup="false" />
                                    <Columns>
                                        <dxwgv:GridViewDataTextColumn Caption="" VisibleIndex="0" Name="selecionar" Width="30px">
                                            <DataItemTemplate>
                                                <dxe:ASPxCheckBox ID="chkBox" runat="server" Width="30px">
                                                </dxe:ASPxCheckBox>
                                            </DataItemTemplate>
                                            <CellStyle HorizontalAlign="Right">
                                            </CellStyle>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Nº" FieldName="num_chamada" VisibleIndex="1"
                                            Visible="false">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Matricula" FieldName="aluno" VisibleIndex="2">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Aluno" FieldName="nome_compl" VisibleIndex="3"
                                            Width="150px">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewCommandColumn VisibleIndex="4" ButtonType="Image" Width="7%" Caption="Foto">
                                            <SelectButton Text="Selecionar" Visible="True">
                                                <Image Url="~/Images/bt_foto.png" />
                                            </SelectButton>
                                        </dxwgv:GridViewCommandColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Situação" FieldName="sit_matgrade" VisibleIndex="5">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Disciplina" FieldName="DISCIPLINA" Visible="false"
                                            VisibleIndex="6">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Disciplina" FieldName="DISCIPLINAORIGINAL"
                                            Visible="false" VisibleIndex="6">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Disciplina" FieldName="nomeDisciplina" VisibleIndex="7">
                                        </dxwgv:GridViewDataTextColumn>
                                    </Columns>
                                </dxwgv:ASPxGridView>
                            </asp:Panel>
                            <br />
                            <br />
                            <table>
                                <tr>
                                    <td>
                                        <dxe:ASPxButton ID="btnMarcarTodasEletiva" Text="Marcar Todos" runat="server" OnClick="btnMarcarTodasEletiva_Click">
                                        </dxe:ASPxButton>
                                    </td>
                                    <td>
                                        <dxe:ASPxButton ID="btnDesMarcarTodasEletiva" Text="Desmarcar Todos" runat="server"
                                            OnClick="btnDesMarcarTodasEletiva_Click">
                                        </dxe:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <asp:Label ID="lblSituacaoEletivas" runat="server" Text="Situação final dos alunos selecionados:"></asp:Label>
                            <br />
                            <asp:RadioButton ID="rbAprovadoEletiva" runat="server" Text="Aprovado" Checked="true" />
                            <br />
                            <br />
                            <asp:ImageButton ID="btnExecutarEletiva" runat="server" SkinID="Confirmar" ToolTip="Aprovar alunos selecionados"
                                OnClientClick="return ConfirmaExecucaoEletiva();" OnClick="btnExecutarEletiva_Click"
                                ImageUrl="~/Images/bot_confirmar.png" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <br />
            <table>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top" class="style1">
                        <asp:Panel ID="pnlGridMatriculas" runat="server" ScrollBars="Auto" Height="300px"
                            Width="650px">
                            <asp:ObjectDataSource ID="odsMatriculas" runat="server" TypeName="Techne.Lyceum.RN.FechamentoMatricula"
                                SelectMethod="ListarAlunosMatriculados">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="txtAnoTop" Name="ano" PropertyName="Text" />
                                    <asp:ControlParameter ControlID="txtPeriodoTop" Name="semestre" PropertyName="Text" />
                                    <asp:ControlParameter ControlID="txtEscolaridadeVal" Name="curso" PropertyName="Text" />
                                    <asp:ControlParameter ControlID="txtTurnoVal" Name="turno" PropertyName="Text" />
                                    <asp:ControlParameter ControlID="txtCurriculoTop" Name="curriculo" PropertyName="Text" />
                                    <asp:ControlParameter ControlID="txtSerieVal" Name="serie" PropertyName="Text" />
                                    <asp:ControlParameter ControlID="txtTurmaTop" Name="turma" PropertyName="Text" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <dxwgv:ASPxGridView ID="grdMatriculas" ClientInstanceName="grdMatriculas" Visible="true"
                                DataSourceID="odsMatriculas" KeyFieldName="aluno" runat="server" EnableRowsCache="true"
                                EnableCallBacks="false" Width="600px" OnSelectionChanged="grdMatriculas_SelectionChanged">
                                <SettingsPager Mode="ShowAllRecords" />
                                <SettingsBehavior ProcessSelectionChangedOnServer="true" AllowSort="false" AllowDragDrop="false"
                                    AllowMultiSelection="false" AllowGroup="false" />
                                <Columns>
                                    <dxwgv:GridViewDataTextColumn Caption="" VisibleIndex="0" Name="selecionar" Width="30px">
                                        <DataItemTemplate>
                                            <dxe:ASPxCheckBox ID="chkBox" runat="server" Width="30px">
                                            </dxe:ASPxCheckBox>
                                        </DataItemTemplate>
                                        <CellStyle HorizontalAlign="Right">
                                        </CellStyle>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Nº" FieldName="num_chamada" VisibleIndex="1"
                                        Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Matricula" FieldName="aluno" VisibleIndex="2">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Aluno" FieldName="nome_compl" VisibleIndex="3">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Curso da Confirmação" FieldName="CURSO" VisibleIndex="4">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Situação" FieldName="sit_matricula" VisibleIndex="5">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewCommandColumn VisibleIndex="6" ButtonType="Image" Width="7%" Caption="Foto">
                                        <SelectButton Text="Selecionar" Visible="True">
                                            <Image Url="~/Images/bt_foto.png" />
                                        </SelectButton>
                                    </dxwgv:GridViewCommandColumn>
                                </Columns>
                            </dxwgv:ASPxGridView>
                        </asp:Panel>
                        <table>
                            <tr>
                                <td>
                                    <dxe:ASPxButton ID="btnMarcar" Text="Marcar Todos" runat="server" OnClick="btnMarcar_Click">
                                    </dxe:ASPxButton>
                                </td>
                                <td>
                                    <dxe:ASPxButton ID="btnDesmarcar" Text="Desmarcar Todos" runat="server" OnClick="btnDesmarcar_Click">
                                    </dxe:ASPxButton>
                                </td>
                            </tr>
                        </table>
                        <asp:Panel ID="pnlGridHistorico" runat="server" ScrollBars="Auto" Height="300px"
                            Width="450px">
                            <asp:ObjectDataSource ID="odsHistAlunos" runat="server" TypeName="Techne.Lyceum.RN.FechamentoMatricula"
                                SelectMethod="ListarHistoricoAlunos">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="txtAnoTop" Name="ano" PropertyName="Text" />
                                    <asp:ControlParameter ControlID="txtPeriodoTop" Name="semestre" PropertyName="Text" />
                                    <asp:ControlParameter ControlID="txtEscolaridadeVal" Name="curso" PropertyName="Text" />
                                    <asp:ControlParameter ControlID="txtTurnoVal" Name="turno" PropertyName="Text" />
                                    <asp:ControlParameter ControlID="txtCurriculoTop" Name="curriculo" PropertyName="Text" />
                                    <asp:ControlParameter ControlID="txtSerieVal" Name="serie" PropertyName="Text" />
                                    <asp:ControlParameter ControlID="txtTurmaTop" Name="turma" PropertyName="Text" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <dxwgv:ASPxGridView ID="grdHistorico" ClientInstanceName="grdHistorico" Visible="true"
                                DataSourceID="odsHistAlunos" KeyFieldName="aluno" runat="server" EnableRowsCache="true"
                                EnableCallBacks="false" Width="435px" OnSelectionChanged="grdHistorico_SelectionChanged">
                                <SettingsPager Mode="ShowAllRecords" />
                                <SettingsBehavior ProcessSelectionChangedOnServer="true" AllowSort="false" AllowDragDrop="false"
                                    AllowMultiSelection="false" AllowGroup="false" />
                                <Columns>                                    
                                    <dxwgv:GridViewDataTextColumn Caption="Matricula" FieldName="aluno" VisibleIndex="1">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Aluno" FieldName="nome_compl" VisibleIndex="2">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewCommandColumn VisibleIndex="3" ButtonType="Image" Width="7%" Caption="Foto">
                                        <SelectButton Text="Selecionar" Visible="True">
                                            <Image Url="~/Images/bt_foto.png" />
                                        </SelectButton>
                                    </dxwgv:GridViewCommandColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Situação" FieldName="situacao_hist" VisibleIndex="4">
                                    </dxwgv:GridViewDataTextColumn>
                                </Columns>
                            </dxwgv:ASPxGridView>
                        </asp:Panel>
                    </td>
                    <td style="vertical-align: top">
                        <br />
                        <br />
                        <br />
                        <br />
                        <dxe:ASPxRadioButton ID="rdNao" runat="server" Text="Não efetuar matrícula" AutoPostBack="true"
                            GroupName="efetuar" OnCheckedChanged="rdNao_CheckedChanged">
                        </dxe:ASPxRadioButton>
                        <dxe:ASPxRadioButton ID="rdSim" runat="server" Text="Efetuar nova matrícula" AutoPostBack="true"
                            GroupName="efetuar" Checked="true" OnCheckedChanged="rdSim_CheckedChanged">
                        </dxe:ASPxRadioButton>
                        <div id="divEfetuarMatricula" runat="server">
                            <asp:Label ID="lblSelecioneDados" runat="server" Text="Selecione os dados da nova turma:"></asp:Label>
                            <table>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblUniEnsino" runat="server"
                                            Text="Unidade de Ensino: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlUnidadeEnsino" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlUnidadeEnsino_SelectedIndexChanged"
                                            DataTextField="NOME_COMP" DataValueField="unidade_ens" Width="500px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">
                                        <asp:Label ID="lbAno" Text="Ano Letivo: " runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlAno" runat="server" AutoPostBack="True" DataTextField="ano"
                                            DataValueField="ano" Width="70px" DataSourceID="odsAno" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">
                                        <asp:Label ID="lbPeriodo" Text="Período Letivo:* " SkinID="lblObrigatorio" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlPeriodo" runat="server" DataTextField="periodo" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlPeriodo_SelectedIndexChanged" DataValueField="periodo"
                                            Width="70px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">
                                        <asp:Label ID="lbCurso" Text="Escolaridade:*" runat="server" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <tweb:TSearchBox ID="tseCurso" runat="server" Caption="" SqlSelect="SELECT distinct uec.curso as curso, nome FROM LY_CURSO c inner join LY_UNIDADE_ENSINO_CURSOS uec on c.CURSO = uec.CURSO"
                                            ArgumentColumns="60" Columns="10" MaxLength="20" GridWidth="800px" SqlOrder="nome"
                                            OnChanged="tseCurso_Changed">
                                            <GridColumns>
                                                <tweb:TSearchBoxColumn Caption="Código" FieldName="curso" Width="30%" />
                                                <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome" Width="70%" />
                                            </GridColumns>
                                        </tweb:TSearchBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">
                                        <asp:Label ID="lbTurno" Text="Turno:*" runat="server" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlTurno" runat="server" AutoPostBack="True" DataSourceID="odsTurno"
                                            OnSelectedIndexChanged="ddlTurno_SelectedIndexChanged" DataTextField="descricao"
                                            DataValueField="turno" Width="250px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">
                                        <asp:Label ID="lbSerie" Text="Ano de Escolaridade:*" runat="server" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlSerie" runat="server" DataTextField="serie" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlSerie_SelectedIndexChanged" DataValueField="serie"
                                            Width="250px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">
                                        <asp:Label ID="lbTurma" Text="Turma:*" runat="server" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlTurma" runat="server" DataTextField="turma" AutoPostBack="true"
                                            Enabled="False" DataValueField="turma" Width="250px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <br />
                        <div style="background-color: Black; height: 1px">
                        </div>
                        <br />
                        <asp:Label ID="lblSitFinal" runat="server" Text="Situação final dos alunos selecionados:"></asp:Label>
                        <br />
                        <br />
                        <asp:RadioButton ID="rbAprovar" runat="server" Text="Aprovado" AutoPostBack="false"
                            GroupName="executar" Checked="true" OnClick="return VerificaSituacao('Aprovado',this);" />
                        <br />
                        <asp:RadioButton ID="rbAprovarComDep" runat="server" Text="Aprovado com Dependência"
                            AutoPostBack="false" GroupName="executar" OnClick="return VerificaSituacao('AprovadoDep',this);" />
                        <br />
                        <asp:RadioButton ID="rbReprovadoNota" runat="server" Text="Reprovado por nota" AutoPostBack="false"
                            GroupName="executar" OnClick="return VerificaSituacao('ReprovadoNota',this);" />
                        <br />
                        <asp:RadioButton ID="rbReprovadoFalta" runat="server" Text="Reprovado por falta"
                            AutoPostBack="false" GroupName="executar" OnClick="return VerificaSituacao('ReprovadoFalta',this);" />
                        <br />
                        <asp:RadioButton ID="rbPromovido" runat="server" Text="Promovido com continuidade curricular"
                            AutoPostBack="false" GroupName="executar" OnClick="return VerificaSituacao('Promovido',this);" />
                        <br />
                        <asp:RadioButton ID="rbRetido" runat="server" Text="Retido" AutoPostBack="false"
                            GroupName="executar" OnClick="return VerificaSituacao('Retido',this);" />
                        <br />
                        <br />
                        <asp:ImageButton ID="btnExecutar" runat="server" SkinID="Confirmar" ToolTip="Aprovar alunos selecionados"
                            OnClientClick="return ConfirmaExecucao();" OnClick="btnExecutar_Click" ImageUrl="~/Images/bot_confirmar.png" />
                    </td>
                </tr>
            </table>
            <asp:ObjectDataSource ID="odsAno" runat="server" TypeName="Techne.Lyceum.RN.PeriodoLetivo"
                SelectMethod="ConsultarAno"></asp:ObjectDataSource>
            <asp:ObjectDataSource ID="odsPeriodo" runat="server" TypeName="Techne.Lyceum.RN.PeriodoLetivo"
                SelectMethod="ConsultarPeriodo">
                <SelectParameters>
                    <asp:ControlParameter ControlID="ddlAno" Name="ano" PropertyName="SelectedValue" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:ObjectDataSource ID="odsTurno" runat="server" TypeName="Techne.Lyceum.RN.Turno"
                SelectMethod="ComboConsultar"></asp:ObjectDataSource>
            <dxpc:ASPxPopupControl ID="pucInfoAluno" runat="server" CloseAction="CloseButton"
                Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                ClientInstanceName="pucInfoAluno" HeaderText="Detalhes do Aluno" AllowDragging="True"
                Width="250px" EnableAnimation="True" EnableViewState="False">
                <Border BorderColor="Gainsboro" BorderStyle="Ridge" BorderWidth="4px" />
                <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,12000); }" />
                <ContentCollection>
                    <dxpc:PopupControlContentControl ID="pucccInfoAluno" runat="server">
                        <div style="text-align: center">
                            <dxe:ASPxBinaryImage ID="bimgFotoPessoa" Width="150px" Height="150px" runat="server"
                                StoreContentBytesInViewState="True" AlternateText="sem foto" ClientInstanceName="bimgFotoPessoa"
                                ImageAlign="Middle">
                                <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
                                <EmptyImage AlternateText="sem foto" Url="~/Images/semfoto.jpg" />
                            </dxe:ASPxBinaryImage>
                        </div>
                        <br />
                        <asp:Label ID="Label3" Text="Nome: " Font-Bold="true" runat="server" />
                        <br />
                        <asp:Label ID="lblNome" runat="server" />
                        <br />
                        <br />
                        <asp:Label ID="Label4" Text="Nome do Pai: " Font-Bold="true" runat="server" />
                        <br />
                        <asp:Label ID="lblNomePai" runat="server" />
                        <br />
                        <br />
                        <asp:Label ID="Label5" Text="Nome da Mãe: " Font-Bold="true" runat="server" />
                        <br />
                        <asp:Label ID="lblNomeMae" runat="server" />
                        <br />
                        <br />
                        <asp:Label ID="Label6" Text="E-mail: " Font-Bold="true" runat="server" />
                        <br />
                        <asp:Label ID="lblEmail" runat="server" Text="não cadastrado" />
                        <asp:HyperLink ID="hlEmail" runat="server" Font-Underline="true" ForeColor="Blue"
                            Font-Size="Small" />
                        <br />
                    </dxpc:PopupControlContentControl>
                </ContentCollection>
            </dxpc:ASPxPopupControl>
            <dxpc:ASPxPopupControl ID="pcDependencia" runat="server" CloseAction="CloseButton"
                Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                ClientInstanceName="pcDependencia" HeaderText="Detalhes do Aluno" AllowDragging="True"
                Width="250px" EnableAnimation="True" EnableViewState="False" ShowPageScrollbarWhenModal="true">
                <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,14000); }" />
                <ContentCollection>
                    <dxpc:PopupControlContentControl ID="pccDependencia" runat="server">
                        <dxp:ASPxPanel ID="pnlDependencia" runat="server" DefaultButton="btnDependencia">
                            <PanelCollection>
                                <dxp:PanelContent ID="pncDependencia" runat="server">
                                    <asp:Label ID="lblDependencia" runat="server" />
                                    <br />
                                    <dxwgv:ASPxGridView ID="grdDependencia" runat="server" AutoGenerateColumns="False"
                                        EnableCallBacks="false" DataSourceID="" ClientInstanceName="grdDependencia" KeyFieldName="DISCIPLINA"
                                        Width="450px">
                                        <SettingsPager AlwaysShowPager="false" Mode="ShowAllRecords">
                                        </SettingsPager>
                                        <SettingsBehavior AllowFocusedRow="false" AutoExpandAllGroups="true" AllowGroup="false"
                                            AllowSort="false" AllowDragDrop="false" />
                                        <Columns>
                                            <dxwgv:GridViewDataColumn Caption="Disciplina" FieldName="DISCIPLINA" VisibleIndex="0">
                                            </dxwgv:GridViewDataColumn>
                                            <dxwgv:GridViewDataColumn Caption="Nome" FieldName="NOME_DISCIPLINA" VisibleIndex="1">
                                            </dxwgv:GridViewDataColumn>
                                            <dxwgv:GridViewDataColumn Caption="" Name="DEPENDENCIA" VisibleIndex="2">
                                                <DataItemTemplate>
                                                    <dxe:ASPxCheckBox ID="chkDependencia" runat="server">
                                                    </dxe:ASPxCheckBox>
                                                </DataItemTemplate>
                                            </dxwgv:GridViewDataColumn>
                                        </Columns>
                                    </dxwgv:ASPxGridView>
                                    <br />
                                    <div style="text-align: right">
                                        <asp:Button ID="btnCancelarDependencia" runat="server" Text="Cancelar" ToolTip="Cancelar"
                                            OnClick="btnCancelarDependencia_Click" />
                                        <asp:Button ID="btnDependencia" runat="server" Text="OK" ToolTip="OK" OnClick="btnDependencia_Click"
                                            OnClientClick="return validarDependencias();" />
                                    </div>
                                </dxp:PanelContent>
                            </PanelCollection>
                        </dxp:ASPxPanel>
                    </dxpc:PopupControlContentControl>
                </ContentCollection>
            </dxpc:ASPxPopupControl>
        </ContentTemplate>
    </asp:UpdatePanel>
    <dxe:ASPxButton ID="btnVoltar" runat="server" Text="Voltar" OnClick="btnVoltar_Click">
    </dxe:ASPxButton>
    <dxpc:ASPxPopupControl ID="pucConfirmar" ClientInstanceName="pucConfirmar" runat="server"
        Modal="true" ShowShadow="true" AllowDragging="false" AllowResize="false" ShowCloseButton="false"
        ShowFooter="false" ShowHeader="false" ShowSizeGrip="False" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" EnableAnimation="true" Width="150px">
        <Border BorderColor="Gainsboro" BorderStyle="Ridge" BorderWidth="4px" />
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,16000); }" />
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
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .style1
        {
            width: 589px;
        }
    </style>
</asp:Content>
