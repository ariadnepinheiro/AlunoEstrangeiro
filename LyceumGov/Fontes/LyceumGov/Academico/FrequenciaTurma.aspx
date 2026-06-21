<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modulos/LyceumMaster.Master"
    CodeBehind="FrequenciaTurma.aspx.cs" EnableEventValidation="false" Inherits="Techne.Lyceum.Net.Academico.FrequenciaTurma" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<asp:Content ID="cFrequenciaTurma" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">


        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

        function EndRequestHandler(sender, args) {
            AddEvents();
        }

        function BloquearCtrl() {
            if (event.keyCode == 17) {
                alert("Proibido utilizar o Ctrl neste campo");
            }
        }

        function desabilitaBotaoDireito() {
            if (event.button == 2) {
                alert("Proibido utilizar o botao direito neste campo");
            }
        }


        $(document).ready(function() {
            AddEvents();
            ControlaBloqCampos();
        });

        // adiciona eventos nas células de notas da Grid para:
        // 1. navegação com direcionais do teclado
        // 2. validação dos caracteres digitados na célula

        // constantes e enumerações usadas para o controle de navegação das teclas pressinadas
        var KeyCode = { "Left": 37, "Up": 38, "Right": 39, "Down": 40, "Enter": 13 };
        var ControlType = { "button": {}, "checkbox": {}, "file": {}, "hidden": {}, "image": {},
            "password": {}, "radio": {}, "reset": {}, "select-one": {},
            "select-multiple": {}, "submit": {}, "text": {}, "textarea": {}
        };

        // função para navegação entre controles através das setas
        function nav(e, row, column) {
            var unicode = getKeyCode(e);
            var direcional = false;

            // atualiza índices row, column do próximo controle, conforme seta pressionada
            switch (unicode) {
                case KeyCode.Up:
                    row -= 1;
                    direcional = true;
                    break;

                case KeyCode.Down: row += 1;
                    direcional = true;
                    break;

                case KeyCode.Left: column -= 1;
                    direcional = true;
                    break;

                case KeyCode.Right: column += 1;
                    direcional = true;
                    break;

                case KeyCode.Enter: row += 1;
                    direcional = true;
                    break;
            }

            if (!direcional) {
                return;
            }
        }

        // constantes e enumerações usadas para a validação dos caracteres digitados pelo usuário
        var Keys = { "Backspace": 8, "Tab": 9, "Left": 37, "Up": 38, "Right": 39, "Down": 40, "Del": 46, "End": 35, "Home": 36, "Shift": 16 };

        function ValidaKeyPress(jqObject, e) {

            $(jqObject).replaceSelection('', true);

            var keyCode = getKeyCode(e);
            var new_char = String.fromCharCode(keyCode);
            var old_value = $(jqObject).val().replace(",", ".");
            var notaMax = $(jqObject).attr("notaMax");


            if (new_char == ",") {
                new_char = ".";
            }

            // Bloqueia inserção de mais de um separador decimal
            if (new_char == "." && old_value.indexOf('.') >= 0) {
                return (false);
            }

            if (new_char == "." && old_value.length == 0) {
                old_value = "0";
            }
            else if ("0123456789.".indexOf(new_char) >= 0) {
                var fut_value = old_value + new_char;

                // Valor máximo atingido
                if (parseFloat(fut_value) > parseFloat(notaMax.replace(",", "."))) {
                    return (false);
                }
                else if (parseFloat(fut_value) < parseFloat(parseFloat(0.5) * parseFloat(notaMax.replace(",", ".")))) {
                    $(jqObject).css({ "color": "Red" });
                }
                else {
                    $(jqObject).css({ "color": "Blue" });
                }

                $(jqObject).val(fut_value.replace(".", ","));

                return (false);
            } else if (keyCode == Keys.Tab || keyCode == Keys.Home || keyCode == Keys.End || keyCode == Keys.Shift) {
                return (true);
            } else if (keyCode == Keys.Backspace || keyCode == Keys.Del) {
                return (true);
            } else {
                return (false);
            }
        }

        function ValidaCorrige(jqObject) {

            //POG
            try {
                var ok = $(jqObject);
            } catch (e) {
                return;
            }

            // obtém atributos da célula referente à nota do aluno
            var notaMax = parseFloat($(jqObject).attr("notaMax").replace(",", "."));
            var notaCelula = $(jqObject).val().replace(",", ".");

            if ("sS".indexOf(notaCelula) >= 0)
                if ("0123456789.".indexOf(notaCelula) >= 0)
                $(jqObject).val("");
            else
                $(jqObject).val("SN");

            // 3. Número Máximo de Casas Decimais:
            // A partir da ocorrência de ponto na célula, verifica-se se o número de casas decimais permitido foi atingigo.
            // Caso positivo, os caracteres excedentes devem ser eliminados na célula.
            var ocorrenciaPonto = notaCelula.indexOf('.');
            var casasDecimaisCelula = notaCelula.substring(ocorrenciaPonto, notaCelula.length - 1).length;



            var valor = parseFloat($(jqObject).val().replace(",", "."));

            $(jqObject).css({ "color": (valor >= notaMax / 2.0) ? "Blue" : "Red" });
        }

		
    </script>

    <div style="visibility: hidden">
        <asp:HiddenField ID="hdnHorarios" runat="server" Value="" />
        <asp:HiddenField ID="hdnPossuiBasico" runat="server" />
        <asp:HiddenField ID="hdnPossuiEssencial" runat="server" />
        <asp:HiddenField ID="hdnPossuiRecomposicao" runat="server" />
    </div>
    <asp:ScriptManagerProxy ID="scriptManager" runat="server" />
    <div style="width: 50%">
        <asp:Panel ID="pnlDadosTurma" GroupingText="Dados da turma" runat="server" Width="100%"
            Wrap="false" HorizontalAlign="Left">
            <table width="90%">
                <tr>
                    <td align="right">
                        <asp:Label ID="Label1" Text="Regional:" runat="server" SkinID="lblObrigatorio" />
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbRegional" ReadOnly="true" runat="server" />
                    </td>
                    <td align="right">
                        <asp:Label ID="Label2" Text="Municipio:" runat="server" SkinID="lblObrigatorio" />
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbMunicipio" runat="server" ReadOnly="true" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblUnidadeEnsino" Text="Censo:" runat="server" SkinID="lblObrigatorio" />
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbCenso" ReadOnly="true" runat="server" />
                    </td>
                    <td align="right">
                        <asp:Label ID="Label5" Text="Escola:" runat="server" SkinID="lblObrigatorio" />
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbUnidadeEnsino" ReadOnly="true" runat="server" Width="350px" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblTurma" Text="Turma:" runat="server" SkinID="lblObrigatorio" />
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbTurma" ReadOnly="true" runat="server" />
                    </td>
                    <td align="right">
                        <asp:Label ID="lblAno" Text="Ano:" runat="server" SkinID="lblObrigatorio" />
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbAno" runat="server" ReadOnly="true" />
                    </td>
                    <td align="right">
                        <asp:Label ID="lblPeriodo" Text="Período:" runat="server" SkinID="lblObrigatorio" />
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbPeriodo" runat="server" ReadOnly="true" Width="50px" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblEscolaridade" Text="Escolaridade:" runat="server" SkinID="lblObrigatorio" />
                    </td>
                    <th colspan="3" align="left">
                        <asp:TextBox ID="tbEscolaridade" ReadOnly="true" runat="server" Width="350px" />
                        <asp:HiddenField ID="hdnCurso" runat="server" />
                    </th>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblTurno" Text="Turno:" runat="server" SkinID="lblObrigatorio" />
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbTurno" ReadOnly="true" runat="server" />
                        <asp:HiddenField ID="hdnTurno" runat="server" />
                    </td>
                    <td align="right">
                        <asp:Label ID="lblMatrizCurricular" Text="Matriz Curricular:" runat="server" SkinID="lblObrigatorio" />
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbMatrizCurricular" ReadOnly="true" runat="server" />
                    </td>
                    <td align="right" style="width: 85px">
                        <asp:Label ID="lblAnoEscolar" Text="Série:" runat="server" SkinID="lblObrigatorio" />
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbSerie" ReadOnly="true" runat="server" Width="50px" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Panel ID="pnlDadosFrequencia" runat="server" GroupingText="Escolha os dados da frequência"
            Width="100%">
            <table>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblDisciplinas" Text="Disciplina:*" SkinID="lblObrigatorio" runat="server" />
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlDisciplina" runat="server" AutoPostBack="True" DataTextField="nome"
                            DataValueField="disciplina" OnSelectedIndexChanged="ddlDisciplina_SelectedIndexChanged"
                            AppendDataBoundItems="true">
                        </asp:DropDownList>
                    </td>
                    <td align="right">
                        <asp:Label ID="lblProfessor" Text="Professor:" SkinID="lblObrigatorio" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lblProfessorAlocado" SkinID="lblMensagem" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="Label3" Text="Mês:*" SkinID="lblObrigatorio" runat="server" />
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlMes" runat="server" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlMes_SelectedIndexChanged"
                            AutoPostBack="true">
                            <asp:ListItem Text="Selecione" Value=""></asp:ListItem>
                            <asp:ListItem Text="Janeiro" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Fevereiro" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Março" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Abril" Value="4"></asp:ListItem>
                            <asp:ListItem Text="Maio" Value="5"></asp:ListItem>
                            <asp:ListItem Text="Junho" Value="6"></asp:ListItem>
                            <asp:ListItem Text="Julho" Value="7"></asp:ListItem>
                            <asp:ListItem Text="Agosto" Value="8"></asp:ListItem>
                            <asp:ListItem Text="Setembro" Value="9"></asp:ListItem>
                            <asp:ListItem Text="Outubro" Value="10"></asp:ListItem>
                            <asp:ListItem Text="Novembro" Value="11"></asp:ListItem>
                            <asp:ListItem Text="Dezembro" Value="12"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="right">
                        <asp:Label ID="Label4" Text="Data da Frequência:*" SkinID="lblObrigatorio" runat="server" />
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlDataFrequencia" runat="server" DataTextField="data" DataValueField="data"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlDataFrequencia_SelectedIndexChanged"
                            AppendDataBoundItems="true">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <br />
            <table>
                <tr>
                    <td align="left">
                        <asp:ImageButton ID="btnBuscar" runat="server" SkinID="BcBuscar" OnClick="btnBuscar_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <br />
        <asp:Label ID="lblMensagemFinalizacao" SkinID="lblMensagem" runat="server" Width="800px" />
        <br />
        <asp:Label ID="lblMensagemReposicao" SkinID="lblMensagem" runat="server" Width="800px" />
        <br />
        <asp:Panel ID="pnlPlanoAula" runat="server" GroupingText="Registro de Aula - Temática Desenvolvida (Assunto, Exercício e outros)"
            Width="100%" Visible="false">
            <table width="100%">
                <tr>
                    <td align="right">
                        <asp:TextBox ID="txtPlanoAula" runat="server" TextMode="MultiLine" MaxLength="5000"
                            Width="100%" Height="100px"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Panel ID="pnlCurriculo" runat="server" Width="100%" GroupingText="Currículo">
            <table width="100%">
                <tr style="font-weight: bold; background-color: #6CA6EA; text-align: center;">
                    <td>
                        <asp:LinkButton ID="lnkBasico" runat="server" Text="BÁSICO" OnClick="lnkBasico_Click"
                            Style="color: Black"></asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlBasico" runat="server">
                            <asp:Repeater ID="rpCurriculoBasico" runat="Server">
                                <ItemTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <%-- Grupo--%>
                                                <asp:Label ID="lblGrupoBasico" runat="server" Text='<%# Eval("Grupo") %>' Style="font-weight: bold;
                                                    color: #000000;"></asp:Label>
                                                <asp:HiddenField ID="hdnGrupoIdBasico" Value='<%# Eval("GrupoId") %>' runat="server" />
                                                <%-- Item--%>
                                                <asp:CheckBoxList ID="chkCompetenciaItemBasico" runat="server" DataTextField="CompetenciaHabilidade"
                                                    RepeatDirection="Vertical" DataValueField="ItemId" DataSource='<%#Eval("ListaItem")%>'>
                                                </asp:CheckBoxList>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:Repeater>
                        </asp:Panel>
                    </td>
                </tr>
                <tr style="font-weight: bold; background-color: #6CA6EA; text-align: center;">
                    <td>
                        <asp:LinkButton ID="lnkEssencial" runat="server" Text="ESSENCIALIZADO" OnClick="lnkEssencial_Click"
                            Style="color: Black"></asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlEssencial" runat="server">
                            <asp:Repeater ID="rpCurriculoEssencial" runat="Server">
                                <ItemTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <%-- Grupo--%>
                                                <asp:Label ID="lblGrupoEssencial" runat="server" Text='<%# Eval("Grupo") %>' Style="font-weight: bold;
                                                    color: #000000;"></asp:Label>
                                                <asp:HiddenField ID="hdnGrupoIdEssencial" Value='<%# Eval("GrupoId") %>' runat="server" />
                                                <%-- Item--%>
                                                <asp:CheckBoxList ID="chkCompetenciaItemEssencial" runat="server" DataTextField="CompetenciaHabilidade"
                                                    RepeatDirection="Vertical" DataValueField="ItemId" DataSource='<%#Eval("ListaItem")%>'>
                                                </asp:CheckBoxList>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:Repeater>
                        </asp:Panel>
                    </td>
                </tr>
                <tr style="font-weight: bold; background-color: #6CA6EA; color: White; text-align: center;">
                    <td>
                        <asp:LinkButton ID="lnkRecomposicao" runat="server" Text="RECOMPOSIÇÃO" OnClick="lnkRecomposicao_Click"
                            Style="color: Black"></asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlRecomposicao" runat="server">
                            <asp:Repeater ID="rpCurriculoRecomposicao" runat="Server">
                                <ItemTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <%-- Grupo--%>
                                                <asp:Label ID="lblGrupoRecomposicao" runat="server" Text='<%# Eval("Grupo") %>' Style="font-weight: bold;
                                                    color: #000000;"></asp:Label>
                                                <asp:HiddenField ID="hdnGrupoIdRecomposicao" Value='<%# Eval("GrupoId") %>' runat="server" />
                                                <%-- Item--%>
                                                <asp:CheckBoxList ID="chkCompetenciaItemRecomposicao" runat="server" DataTextField="CompetenciaHabilidade"
                                                    RepeatDirection="Vertical" DataValueField="ItemId" DataSource='<%#Eval("ListaItem")%>'>
                                                </asp:CheckBoxList>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:Repeater>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Label ID="lblMensagem" SkinID="lblMensagem" runat="server" Width="800px" />
        <br />
        <asp:Panel ID="pnlGridMatriculas" runat="server" Visible="false" Width="100%">
            <asp:Panel ID="pnlNotasConsolidada" runat="server">
                <dxwgv:ASPxGridView ID="grdMatriculas" ClientInstanceName="grdMatriculas" Visible="true"
                    EnableViewState="false" KeyFieldName="aluno" runat="server" EnableCallBacks="false"
                    OnHtmlRowCreated="grdMatriculas_HtmlRowCreated" OnHtmlDataCellPrepared="grdMatriculas_HtmlDataCellPrepared">
                    <SettingsPager Mode="ShowAllRecords" />
                    <SettingsBehavior ProcessSelectionChangedOnServer="true" AllowSort="false" AllowDragDrop="false"
                        AllowMultiSelection="false" AllowGroup="false" />
                    <Columns />
                </dxwgv:ASPxGridView>
            </asp:Panel>
        </asp:Panel>
        <br />
        <asp:Panel ID="botoes" runat="server">
            <table>
                <tr>
                    <td align="left">
                        <asp:ImageButton ID="btnImprimirComp" runat="server" SkinID="Imprimir" ImageAlign="Right" />
                    </td>
                    <td align="left">
                        <asp:ImageButton ID="btnSalvar" OnClick="btnSalvar_Click" SkinID="BcSalva" runat="server"
                            Visible="true" OnClientClick="return ValidaPreenchimentoTotal();" />
                    </td>
                    <td align="left">
                        <asp:ImageButton ID="btnCancelar" runat="server" SkinID="Cancelar" OnClick="btnCancelarVoltar_Click"
                            OnClientClick="return ConfirmaCancelarLancamentoNotas();" Visible="false" />
                    </td>
                    <td align="left">
                        <asp:ImageButton ID="btnVoltar" runat="server" SkinID="Voltar" OnClick="btnCancelarVoltar_Click"
                            Visible="false" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .style1
        {
            width: 512px;
        }
    </style>
</asp:Content>
