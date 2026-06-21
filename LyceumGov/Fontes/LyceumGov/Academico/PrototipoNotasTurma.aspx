<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="PrototipoNotasTurma.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.PrototipoNotasTurma" %>

<asp:Content ID="cNotasTurma" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">

        
        $(document).ready(function() {
            AddEvents();
        });

        function AddEvents() {
            //registra eventos keydown para controles que possuem attributo "navegar"
            $("#<%=grdMatriculas.ClientID %> *[navegar=true]").each(function(i) {
                var col = parseInt($("#" + this.id).attr("columnIndex"));
                var row = parseInt($("#" + this.id).attr("rowIndex"));
                $("#" + this.id).keydown(function(e) {
                    nav(e, row, col);
                });
            });

            //registra eventos para validação numérica
            $("#<%=grdMatriculas.ClientID %> *[validar=true]").each(function(i) {
                $(this).keypress(function(e) {
                    return (ValidaKeyPress(this, e));
                }).change(function(e) {
                    ValidaCorrige(this);
                }).select(function(e) {
                    ValidaCorrige(this);
                });
            });
        }

        //Constantes e Enumerações
        var KeyCode = { "Left": 37, "Up": 38, "Right": 39, "Down": 40, "Enter": 13 };
        var ControlType = { "button": {}, "checkbox": {}, "file": {}, "hidden": {}, "image": {},
            "password": {}, "radio": {}, "reset": {}, "select-one": {},
            "select-multiple": {}, "submit": {}, "text": {}, "textarea": {}
        };

        //função para navegação entre controles através das setas
        function nav(e, row, column) {
            var unicode = e.keyCode ? e.keyCode : e.charCode;

            //atualiza índices row, column do próximo controle, conforme seta pressionada
            switch (unicode) {
                case KeyCode.Up: row -= 1; break;
                case KeyCode.Down: row += 1; break;
                case KeyCode.Left: column -= 1; break;
                case KeyCode.Right: column += 1; break;
                case KeyCode.Enter: row += 1; break;
            }

            //busca o próximo controle
            var ob = $("#<%=grdMatriculas.ClientID %> *[navegar][columnIndex=" + column + "][rowIndex=" + row + "]");
            //se não nulo, foca no controle ou avança para o próximo controle caso readOnly || disabled
            if (ob.length) {
                //verifica se controle é tipo text
                if (ob.attr("type") == "text") {
                    //se controle readOnly ou disabled, chama evento para avançar para próximo controle
                    if (ob.attr("disabled") || ob.attr("readOnly")) {
                        nav(e, row, column);
                        return;
                    }
                    //se controle enabled e !readonly, foca no controle
                    else
                        ob.focus();
                }
                //se nulo, volta para início ou fim da linha ou coluna, conforme tecla pressionada
            } else {
                var coltemp = 0, rowtemp = 0;
                if (unicode == KeyCode.Up || unicode == KeyCode.Down || unicode == KeyCode.Enter)
                    coltemp = column;
                if (unicode == KeyCode.Left || unicode == KeyCode.Right)
                    rowtemp = row;

                switch (unicode) {
                    case KeyCode.Up:
                        $("#<%=grdMatriculas.ClientID %> *[navegar][columnIndex=" + coltemp + "]:last").focus();
                        break;
                    case KeyCode.Down:
                    case KeyCode.Enter:
                        $("#<%=grdMatriculas.ClientID %> *[navegar][columnIndex=" + coltemp + "]:first").focus();
                        break;
                    case KeyCode.Left:
                        $("#<%=grdMatriculas.ClientID %> *[navegar][rowIndex=" + rowtemp + "]:last").focus();
                        break;
                    case KeyCode.Right:
                        $("#<%=grdMatriculas.ClientID %> *[navegar][rowIndex=" + rowtemp + "]:first").focus();
                        break;
                }
            }
        }

        //Função para validação numérica
        var Keys = { "Backspace": 8, "Tab": 9, "Left": 37, "Up": 38, "Right": 39, "Down": 40, "Del": 46, "End": 35, "Home": 36, "Shift": 16 };

        function ValidaKeyPress(jqObject, e) {
            var keyCode = e.keyCode;
            var new_char = String.fromCharCode(keyCode);
            var old_value = $(jqObject).val().replace(",", ".");
            var notaMax = $(jqObject).attr("notaMax");
            var numCasasDec = $("#<%=hdnNCasasDec.ClientID %>").val();

            if (new_char == ",") new_char = ".";

            if (new_char == "." && old_value.indexOf('.') >= 0) // Bloqueia inserção de mais de um separador decimal
                return (false);

            if (new_char == "." && old_value.length == 0)
                old_value = "0";

            if ("0123456789.".indexOf(new_char) >= 0) {
                var fut_value = old_value + new_char;

                if (parseFloat(fut_value) > parseFloat(notaMax)) return (false); // Valor máximo atingido 

                if (fut_value.indexOf('.') >= 0) // Número máximo de casas decimais atingido
                    if (fut_value.substring(fut_value.indexOf('.'), fut_value.length - 1).length > parseInt(numCasasDec)) return (false);

                $(jqObject).val(fut_value.replace(".", ","));
                return (false);
            } else if (keyCode == Keys.Backspace || keyCode == Keys.Tab || keyCode == Keys.Home || keyCode == Keys.End || keyCode == Keys.Del || keyCode == Keys.Shift) {
                return (true);
            } else {
                return (false);
            }
        }

        function ValidaCorrige(jqObject) {
            var notaMax = $(jqObject).attr("notaMax");
            var numCasasDec = $("#<%=hdnNCasasDec.ClientID %>").val();
            var val = "";
            var oval = $(jqObject).val().replace(",", ".");
            for (i = 0; i < oval.length; i++) {
                if ("01234567890.".indexOf(oval.charAt(i)) >= 0)
                    if (val.indexOf(".") < 0 || oval.charAt(i) != '.')
                    val = val + oval.charAt(i);
            }
            $(jqObject).val(val.replace(".", ","));
            oval = val;
            if (oval.indexOf('.') >= 0) { // Número máximo de casas decimais atingido
                if (oval.substring(oval.indexOf('.'), oval.length - 1).length > parseInt(numCasasDec)) {
                    $(jqObject).val(oval.substring(0, oval.length - 1).replace(".", ","));
                    ValidaCorrige(jqObject);
                }
            }
            oval = $(jqObject).val();
            if (parseFloat(val) > parseFloat(notaMax)) {
                $(jqObject).val(val.substring(0, val.length - 1).replace(".", ","));
                ValidaCorrige(jqObject);
            }
        }
  
    </script>

    <div style="visibility: hidden">
        <asp:HiddenField ID="hdnGradeID" runat="server" />
        <asp:HiddenField ID="hdnGrupoNota" runat="server" />
        <asp:HiddenField ID="hdnNCasasDec" runat="server" />
    </div>
    <asp:ScriptManagerProxy ID="scriptManager" runat="server" />
    <asp:Panel ID="pnlDadosTurma" GroupingText="Dados da turma" runat="server" Width="650px"
        HorizontalAlign="Left">
        <table width="90%">
            <tr>
                <td align="right">
                    <asp:Label ID="lblUnidadeEnsino" Text="Unidade de Ensino:" runat="server" />
                </td>
                <th colspan="3" align="left">
                    <asp:TextBox ID="tbUnidadeEnsino" ReadOnly="true" runat="server" Width="100%" />
                </th>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblTurma" Text="Turma:" runat="server" />
                </td>
                <td align="left">
                    <asp:TextBox ID="tbTurma" ReadOnly="true" runat="server" />
                </td>
                <td align="right">
                    <asp:Label ID="lblAno" Text="Ano:" runat="server" />
                </td>
                <td align="left">
                    <asp:TextBox ID="tbAno" runat="server" ReadOnly="true" Style="text-align: center"
                        Width="100%" />
                </td>
                <td align="right">
                    <asp:Label ID="lblPeriodo" Text="Período:" runat="server" />
                </td>
                <td align="left">
                    <asp:TextBox ID="tbPeriodo" runat="server" ReadOnly="true" Style="text-align: center"
                        Width="50px" />
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblEscolaridade" Text="Escolaridade:" runat="server" />
                </td>
                <th colspan="3" align="left">
                    <asp:TextBox ID="tbEscolaridade" ReadOnly="true" runat="server" Width="100%" />
                </th>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblTurno" Text="Turno:" runat="server" />
                </td>
                <td align="left">
                    <asp:TextBox ID="tbTurno" ReadOnly="true" runat="server" />
                </td>
                <td align="right">
                    <asp:Label ID="lblMatrizCurricular" Text="Matriz Curricular:" runat="server" />
                </td>
                <td align="right">
                    <asp:TextBox ID="tbMatrizCurricular" ReadOnly="true" Style="text-align: center" runat="server"
                        Width="100%" />
                </td>
                <td align="right" style="width: 85px">
                    <asp:Label ID="lblAnoEscolar" Text="Ano de Escolaridade:" runat="server" Width="100%" />
                </td>
                <td align="left">
                    <asp:TextBox ID="tbAnoEscolar" ReadOnly="true" Style="text-align: center" runat="server"
                        Width="50px" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlDisciplina" runat="server" GroupingText="Informe os dados para pesquisar notas"
        Width="650px">
        <table>
            <tr>
                <td align="right">
                    <asp:Label ID="lblDisciplinas" Text="Disciplina:*" SkinID="lblObrigatorio" runat="server"
                        Width="120px" />
                </td>
                <td colspan="3" align="left">
                    <tweb:TSearchBox ID="tseDisciplina" runat="server" Argument="nome" DataType="VarChar"
                        Key="disciplina" SqlOrder="nome" SqlSelect="SELECT gt.grade_id as grade_id, gt.disciplina as disciplina, d.nome as nome FROM ly_grade_turma gt 
                            inner join LY_DISCIPLINA d on d.DISCIPLINA = gt.DISCIPLINA" MaxLength="20"
                        AutoPostBack="true" Columns="15">
                        <gridcolumns>
                            <tweb:TSearchBoxColumn Caption="Disciplina" FieldName="disciplina" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="nome" Width="80%" />
                        </gridcolumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblPeriodoEscolar" Text="Período Letivo:" runat="server" />
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlPeriodoEscolar" DataTextField="descricao" DataValueField="subperiodo"
                        runat="server" />
                </td>
                <td style="width: 25px" />
                <td align="right">
                    <asp:ImageButton ID="btnBuscar" OnClick="btnBuscar_Click" ImageUrl="~/Images/bot_buscar.png"
                        runat="server" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" Visible="false" EnableViewState="true" SkinID="lblMensagem"
        runat="server" />
    <br />
    <asp:UpdatePanel ID="upnlMatriculas" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlGridMatriculas" runat="server" ScrollBars="Auto" Height="480px">
                <dxwgv:ASPxGridView ID="grdMatriculas" ClientInstanceName="grdMatriculas" Visible="true"
                    KeyFieldName="aluno" runat="server" EnableRowsCache="true" EnableCallBacks="false"
                    OnHtmlRowCreated="grdMatriculas_HtmlRowCreated" OnSelectionChanged="grdMatriculas_SelectionChanged"
                    OnBeforeColumnSortingGrouping="grdMatriculas_BeforeColumnSortingGrouping">
                    <SettingsPager Mode="ShowAllRecords" />
                    <SettingsBehavior ProcessSelectionChangedOnServer="true" AllowSort="false" AllowDragDrop="false"
                        AllowMultiSelection="false" AllowGroup="false" />
                    <Columns />
                </dxwgv:ASPxGridView>
                <dxpc:ASPxPopupControl ID="pucInfoAluno" runat="server" CloseAction="CloseButton"
                    Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                    ClientInstanceName="pucInfoAluno" HeaderText="Detalhes do Aluno" AllowDragging="True"
                    Width="250px" EnableAnimation="True" EnableViewState="False">
                    <Border BorderColor="Gainsboro" BorderStyle="Ridge" BorderWidth="4px" />
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
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <asp:ImageButton ID="btnFecharNotas" OnClick="btnFecharNotas_Click" Visible="false"
        SkinID="BcFecharNotas" runat="server" />
    <asp:ImageButton ID="btnSalvar" OnClick="btnSalvar_Click" Visible="false" SkinID="BcSalvar"
        runat="server" />
</asp:Content>
