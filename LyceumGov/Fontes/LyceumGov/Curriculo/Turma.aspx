<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="Turma.aspx.cs" Inherits="Techne.Lyceum.Net.Curriculo.Turma" %>

<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<asp:Content ID="cTurma" ContentPlaceHolderID="cphFormulario" runat="server" EnableViewState="false">
    <style type="text/css">
        td.copied
        {
            background-color: PaleGreen;
            border-style: dashed;
            border-color: Green;
            border-width: 2.5px;
        }
        td.focusedCopied
        {
            background-color: SkyBlue;
            border-style: dashed;
            border-color: Green;
            border-width: 2.5px;
        }
        td.default
        {
            background-color: White;
            border-style: solid;
            border-color: White;
            border-width: 2.5px;
        }
        td.selected
        {
            background-color: Salmon;
            border-style: solid;
            border-color: Red;
            border-width: 2.5px;
        }
        td.focused
        {
            background-color: SkyBlue;
            border-style: solid;
            border-color: SkyBlue;
            border-width: 2.5px;
        }
        td.selectedFocused
        {
            background-color: SkyBlue;
            border-style: solid;
            border-color: Red;
            border-width: 2.5px;
        }
        .bordaVermelha
        {
            border-style: solid;
            border-color: Red;
            border-width: 1.8px;
        }
        .bordaBranca
        {
            border-style: solid;
            border-color: White;
            border-width: 1.8px;
        }
        .bordaHeader
        {
            border-width: thin;
            border-color: #0066CC;
            background-color: #0066CC;
            color: White;
            font-weight: bold;
        }
        .bordaHorario
        {
            background-color: #0066CC;
            color: White;
            font-weight: bold;
            border-right-width: 0px;
            border-width: 0px;
            text-align: center;
        }
        .PopUp
        {
            display: none;
            position: absolute;
            width: 100px;
            font-size: 11px;
            font-weight: normal;
            font-family: verdana;
            color: #E7E6E1;
            background-color: #E7E6E1;
        }
        .PopUpErro
        {
            display: none;
            position: absolute;
            font-size: 11px;
            font-weight: bold;
            font-family: verdana;
            color: Black;
            background-color: #EDF4FF;
        }
        .Imagem
        {
            cursor: pointer;
        }
        .Controle
        {
            cursor: pointer;
            color: Black;
            width: 100%;
            text-decoration: none;
            text-align: left;
        }
        .Controle:hover
        {
            text-decoration: none;
            background-color: #003366;
            color: White;
        }
        .ControleDesabilitado:visited
        {
            cursor: pointer;
            color: Gray;
            width: 100%;
            text-decoration: none;
            text-align: left;
        }
        .ControleDesabilitado:link
        {
            cursor: pointer;
            color: Gray;
            width: 100%;
            text-decoration: none;
            text-align: left;
        }
        .ControleDesabilitado:active
        {
            cursor: pointer;
            color: Gray;
            width: 100%;
            text-decoration: none;
            text-align: left;
        }
        .ControleDesabilitado:hover
        {
            background-color: #003366;
            color: Gray;
            text-decoration: none;
        }
        .textoAzulEscuro
        {
            border: 1px #ccc solid;
            font-family: Verdana;
            font-size: smaller;
            background-color: #000080;
        }
        .textoAzul
        {
            border: 1px #ccc solid;
            font-family: Verdana;
            font-size: smaller;
            background-color: #0F6BFF;
        }
        .textoAzulClaro
        {
            border: 1px #ccc solid;
            font-family: Verdana;
            font-size: smaller;
            background-color: #7FC9FF;
        }
        .textoAmarelo
        {
            border: 1px #ccc solid;
            padding: 1px 3px;
            font-family: Verdana;
            font-size: smaller;
            background-color: #FFFD80;
        }
        .textoAmareloEscuro
        {
            border: 1px #ccc solid;
            font-family: Verdana;
            font-size: smaller;
            background-color: #EAEA00;
        }
        .textoVermelho
        {
            border: 1px #ccc solid;
            font-family: Verdana;
            font-size: smaller;
            background-color: Red;
        }
        .textoLaranja
        {
            border: 1px #ccc solid;
            font-family: Verdana;
            font-size: smaller;
            background-color: #D7D7D7;
        }
        .textoCoral
        {
            border: 1px #ccc solid;
            font-family: Verdana;
            font-size: smaller;
            background-color: #ff0000;
        }
        .textoPreto
        {
            border: 1px #ccc solid;
            font-family: Verdana;
            font-size: smaller;
            background-color: LightGray;
        }
        .txtInput
        {
            border: 1px #ccc solid;
            font-family: Verdana;
            font-size: smaller;
            background-color: White;
        }
        .ImagemAviso
        {
            cursor: pointer;
        }
        .style1
        {
            width: 121px;
        }
        .style2
        {
            width: 5%;
        }
        .style3
        {
            width: 27%;
        }
        .style4
        {
            width: 7%;
        }
        .style5
        {
            width: 113px;
        }
    </style>

    <script type="text/javascript">

        function AtualizarCampoCelulasMarcadas() {
            var hidden = $("#<%=hdnTransferenciaSelecionada.ClientID %>");
            $(hidden).val("");

            var numFunc_temp = "";
            var disciplina_temp = "";

            var selectedTDCells = GetSelectedTDCells();
            for (var i = 0; i < selectedTDCells.length; i++) {
                var thisTD = $(selectedTDCells[i]);
                var content = GetTDContent(thisTD);
                if (content.length >= 4) {
                    var hiddenValue = $(hidden).val();

                    var matricula = content[0].split(' ')[0];
                    var num_func = content[2];
                    var disciplina = content[3];

                    if (disciplina_temp == "") disciplina_temp = disciplina;
                    else if (disciplina != disciplina_temp && disciplina != "") {
                        alert("É necessário selecionar alocações da mesma Disciplina.");
                        return false;
                    }

                    if (numFunc_temp == "") numFunc_temp = num_func;
                    else if (num_func != numFunc_temp && num_func != "") {
                        alert("É necessário selecionar alocações do mesmo Docente.");
                        return false;
                    }

                    if (matricula == "00000000" || matricula == "99999999") {
                        alert("Não é permitida a transferência de " + content[0]);
                        return false;
                    }

                    var controlID = $("input", thisTD)[0].id;
                    controlID = controlID.substr(controlID.indexOf("txtDocente"), controlID.length);
                    $(hidden).val(hiddenValue + "{" + num_func + ";" + disciplina + ";" + controlID + "}");
                }
            }

            if (numFunc_temp == "" && disciplina_temp == "") {
                alert("É necessário selecionar ao menos uma alocação.");
                return false;
            }
        }

        function VerificaSelecaoTransferencia() {
            var origemCount = grdTransferenciaSelecionada.GetVisibleRowsOnPage();
            var destinoCount = $("#<%=grdTransferencia.ClientID %> input:checkbox[checked=true]").length;
            var diff = origemCount - destinoCount;

            if (destinoCount == 0) {
                if (origemCount == 1) alert("É necessário selecionar 1 alocação.");
                else alert("É necessário selecionar " + origemCount + " alocações.");
                return false;
            } else if (diff > 0) {
                if (diff == 1) alert("Selecione mais 1 alocação.");
                else alert("Selecione mais " + diff + " alocações.");
                return false;
            } else if (diff < 0) {
                if (origemCount == 1) alert("Selecione apenas 1 alocação.");
                else alert("Selecione apenas " + origemCount + " alocações.");
                return false;
            }

            var carencia = $("#<%=rbtnCarencia.ClientID %> input:radio:checked");
            if (carencia.length == 0) {
                alert('Selecione Carência Real ou Carência Temporária.');
                return false;
            }

            $("#<%=lblTransferenciaMensagem.ClientID %>").text("");
        }

        function ValidaMarcacaoTransferencia(s, e) {
            var origemCount = grdTransferenciaSelecionada.GetVisibleRowsOnPage();
            var destinoCount = $("#<%=grdTransferencia.ClientID %> input:checkbox[checked=true]").length;

            if (destinoCount > origemCount)
                s.SetChecked(!s.GetChecked);
        }
    </script>

    <script type="text/javascript">
        function AutoUpdateCellsContent(number0or1) {
            var content = GetContentFromFields(number0or1);
            if (content == null) return;
            for (var i = 0; i < content.length; i++)
                if (content[i] == null || content[i] == "" || content[i] == "undefined")
                return;

            GetSelectedTDCells().each(function() {
                SetTDContent(this, content);
                SetAttrSelected(this, "false");
            });
        }

        function ValidarNomeTurma(s, e) {
            var prefixoSerie = $("#<%=txtPrefixoSerie.ClientID %>").val();
            var sufixoSerie = $("#<%=ddlSufixoSerie.ClientID %>").val();
            var turma = $("#<%=txtTurma.ClientID %>").val();
            var ua = $("#<%= hdnUA.ClientID %>").val();
            var nomeTurma = prefixoSerie + turma + sufixoSerie + "-" + ua;
            s.errormessage = "O código de turma gerado com Prefixo + Turma + Sufixo + '-' + UA (" + nomeTurma + ") não pode exceder o limite de 50 caracteres. " +
                "Atualmente contém " + nomeTurma.length + " caracteres.";
            e.IsValid = nomeTurma.length <= 50;
        }
        function abrirPopupTurmaDependencia() {
            window.setTimeout(function() {
                pucConfirmarTurma.Show();
            }, 1000);
        }   
    </script>

    <!-- DECLARAÇÃO DE VARIÁVEIS GLOBAIS-->

    <script type="text/javascript">
        var KeyCode = { "Left": 37, "Up": 38, "Right": 39, "Down": 40,
            "A": 65, "C": 67, "V": 86, "X": 88,
            "Control": 17, "Alt": 18, "Shift": 16, "Tab": 09,
            "SpaceBar": 32, "Enter": 13, "Escape": 27, "BackSpace": 08,
            "Delete": 46, "End": 35, "Home": 36, "Insert": 45,
            "PageUp": 33, "PageDown": 34
        };

        //Variável para armazenamento do pressionamento das teclas modificadoras.
        //OnKeyDown(KEY) => KeyPressed recebe TRUE;
        //OnKeyUp(KEY) => KeyPressed recebe FALSE.
        var ControlPressed = false, ShiftPressed = false, AltPressed = false;

        var ShiftStartRowIndex = -1, ShiftStartColumnIndex = -1;
    </script>

    <!-- REGISTRO DE EVENTOS-->

    <script type="text/javascript">
        $(document).ready(function() {
            // Cancela o registro de eventos caso a tela seja apenas de consulta
            if (!($("#<%=btnSalvar2.ClientID %>").length)) {
                return;
            }

            Sys.Application.add_load(BlurTSearch8Caracteres);

            GetAllTDCells().each(function(i) {
                // Calcula as coordenadas da célula
                var rowIndex = parseInt(i / 6);
                var columnIndex = parseInt(i % 6);

                // Seta atributos de iniciais da célula
                $(this).attr("rowIndex", rowIndex);
                $(this).attr("columnIndex", columnIndex);

                if ($(this).attr("somenteLeitura") == "true") {
                    return true;
                }

                RefreshClass(this);
                SetAttrFocused(this, "false");
                SetAttrCopied(this, "false");
                SetAttrCut(this, "false");

                $(this).mousedown(function() {
                    if (ControlPressed) {
                        InvertAttrSelected(this);
                    }
                    else {
                        GetSelectedTDCells().add(this).each(function() {
                            InvertAttrSelected(this);
                        });
                    }

                    FocusCellFirstInput($(this));
                }).mouseenter(function() {
                    ShowCellContent(this);
                }).mouseleave(function() {
                    ShowCellContent(null);
                }).focus(function() {
                    ShowCellContent(this);
                });

                $("input", this).keydown(function(event) {
                    var cell = $(this).parent()[0];
                    var code = IdentifyKeyDownCode(event);

                    switch (code) {
                        case KeyCode.Control:
                            ControlPressed = true;
                            break;
                        case KeyCode.Alt:
                            AltPressed = true;
                            break;
                        case KeyCode.Shift:
                            ShiftPressed = true;
                            ShiftStartRowIndex = $(GetFocusedTDCells()[0]).attr("rowIndex");
                            ShiftStartColumnIndex = $(GetFocusedTDCells()[0]).attr("columnIndex");
                            break;
                        case KeyCode.A:
                            if (ControlPressed) PressedControlA();
                            break;
                        case KeyCode.V:
                            if (ControlPressed) PressedControlV();
                            break;
                        case KeyCode.C:
                            if (ControlPressed) PressedControlC();
                            break;
                        case KeyCode.X:
                            if (ControlPressed) PressedControlX();
                            break;
                        case KeyCode.Escape:
                            PressedEscape();
                            break;
                        case KeyCode.Enter:
                            ShiftPressed ? PressedShiftEnter() : PressedEnter();
                            break;
                        case KeyCode.Delete:
                            ShiftPressed ? PressedShiftDelete() : PressedDelete();
                            break;
                        case KeyCode.SpaceBar:
                            PressedSpaceBar();
                            break;
                        case KeyCode.Tab:
                            ShiftPressed ? PressedShiftTab() : PressedTab();
                            break;
                        case KeyCode.Left:
                        case KeyCode.Up:
                        case KeyCode.Down:
                        case KeyCode.Right:
                            PressedArrows(rowIndex, columnIndex, code);
                            break;
                    }
                    return false;
                }).keyup(function(event) {
                    var code = IdentifyKeyDownCode(event);
                    switch (code) {
                        case KeyCode.Control: ControlPressed = false; break;
                        case KeyCode.Alt: AltPressed = false; break;
                        case KeyCode.Shift:
                            ShiftPressed = false;
                            ShiftStartColumnIndex = -1;
                            ShiftStartRowIndex = -1;
                            break;
                    }
                    return false;
                }).focus(function() {
                    var cell = $(this).parent()[0];
                    SetAttrFocused(cell, "true");
                }).blur(function() {
                    var cell = $(this).parent()[0];
                    SetAttrFocused(cell, "false");
                });
            });

            GetAllTDCellsWithSelectList().each(function(i) {
                // Calcula as coordenadas da célula
                var menuRowIndex = parseInt(i / 6);
                var menuColumnIndex = parseInt(i % 6);

                var div = $("div", this);
                div.append("<a href='#' class='Controle'>Copiar</a>");
                div.append("<br/>");
                div.append("<a href='#' class='Controle'>Colar</a>");
                div.append("<br/>");
                div.append("<a href='#' class='Controle'>Recortar</a>");
                div.append("<br/>");
                div.append("<a href='#' class='Controle'>Limpar</a>");

                div.mouseleave(function() {
                    $(this).slideUp("fast");
                }).hide();

                $("a", this).click(function() {
                    switch ($(this).text()) {
                        case "Copiar":
                            CopyCell(GetTDCell(menuRowIndex, menuColumnIndex));
                            break;
                        case "Colar":
                            PasteToCell(GetTDCell(menuRowIndex, menuColumnIndex));
                            break;
                        case "Recortar":
                            CutCell(GetTDCell(menuRowIndex, menuColumnIndex));
                            break;
                        case "Limpar":
                            CleanCell(GetTDCell(menuRowIndex, menuColumnIndex));
                            break;
                    }
                });

                $("img", this).click(function() {
                    div.slideDown("fast");
                });
            });
        });        
        
    </script>

    <!-- MÉTODOS DE PRESSIONAMENTO DE TECLA-->

    <script type="text/javascript">

        function CleanCells() {
            var permissao = $('#<%= this.hFPermissao.ClientID %>').val();
            if (permissao != "Parcial") {

                CleanSelectedCells();
            }
        }
        function PressedArrows(sourceRowIndex, sourceColumnIndex, keyCode) {
            Navigate(sourceRowIndex, sourceColumnIndex, keyCode);
        }
        function PressedControlC() {
            CopyFocusedCell();
        }
        function PressedControlV() {
            PasteToSelectedCells();
        }
        function PressedControlX() {
            var permissao = $('#<%= this.hFPermissao.ClientID %>').val();
            if (permissao != "Parcial") {
                CutCell(GetFocusedTDCells()[0]);
            }
        }
        function PressedEscape() {
            UnselectAllCells();
        }
        function PressedEnter() {
            CopyDataToSelectedCells(0);
        }
        function PressedShiftEnter() {
            CopyDataToSelectedCells(1);
        }
        function PressedDelete() {
            var permissao = $('#<%= this.hFPermissao.ClientID %>').val();
            if (permissao != "Parcial") {
                CleanSelectedCells();
            }
        }
        function PressedShiftDelete() {
            var permissao = $('#<%= this.hFPermissao.ClientID %>').val();
            if (permissao != "Parcial") {
                CleanAllCells();
                ShiftPressed = false;
            }
        }
        function PressedControlA() {
            SelectAllCells();
        }
        function PressedSpaceBar() {
            SelectCurrentFocusedCell();
        }
        function PressedTab() {
            FocusNeighbourCell(false);
        }
        function PressedShiftTab() {
            FocusNeighbourCell(true);
        }   
    </script>

    <!-- MÉTODOS DE AÇÃO NAS CÉLULAS DA TABELA-->

    <script type="text/javascript">
        //Navega através dos controles.
        function Navigate(sourceRowIndex, sourceColumnIndex, keyCode) {
            var sourceCell = GetTDCell(ShiftStartRowIndex, ShiftStartColumnIndex);

            //Calcula as coordenadas da próxima célula
            switch (keyCode) {
                case KeyCode.Left:
                    sourceColumnIndex--;
                    break;
                case KeyCode.Right:
                    sourceColumnIndex++;
                    break;
                case KeyCode.Up:
                    sourceRowIndex--;
                    break;
                case KeyCode.Down:
                    sourceRowIndex++;
                    break;
            }

            var nextCell = GetTDCell(sourceRowIndex, sourceColumnIndex);
            if (nextCell.length) {
                if ($(nextCell).attr("somenteLeitura") == "true") {
                    Navigate(sourceRowIndex, sourceColumnIndex, keyCode);
                } else {
                    FocusCellFirstInput(nextCell);
                    if (ShiftPressed)
                        SelectRangeOfCells(sourceCell, nextCell);
                }
            } else {
                switch (keyCode) {
                    case KeyCode.Left:
                        sourceColumnIndex = GetLastTDCell().attr("columnIndex");
                        break;
                    case KeyCode.Right:
                        sourceColumnIndex = GetFirstTDCell().attr("columnIndex");
                        break;
                    case KeyCode.Up:
                        sourceRowIndex = GetLastTDCell().attr("rowIndex");
                        break;
                    case KeyCode.Down:
                        sourceRowIndex = GetFirstTDCell().attr("rowIndex");
                        break;
                }
                nextCell = GetTDCell(sourceRowIndex, sourceColumnIndex);
                if (nextCell.length) {
                    if ($(nextCell).attr("somenteLeitura") == "true") {
                        Navigate(sourceRowIndex, sourceColumnIndex, keyCode);
                    } else {
                        FocusCellFirstInput(nextCell);
                        if (ShiftPressed)
                            SelectRangeOfCells(sourceCell, nextCell);
                    }
                }
            }
        }

        function SelectRangeOfCells(cell1, cell2) {
            var row1 = parseInt($(cell1).attr("rowIndex"));
            var row2 = parseInt($(cell2).attr("rowIndex"));
            var col1 = parseInt($(cell1).attr("columnIndex"));
            var col2 = parseInt($(cell2).attr("columnIndex"));

            var rowStart = row1 > row2 ? row2 : row1;
            var rowEnd = row1 < row2 ? row2 : row1;
            var colStart = col1 > col2 ? col2 : col1;
            var colEnd = col1 < col2 ? col2 : col1;

            UnselectAllCells();

            for (var row = rowStart; row <= rowEnd; row++) {
                for (var col = colStart; col <= colEnd; col++) {
                    var itCell = GetTDCell(row, col);
                    SetAttrSelected(itCell, "true");
                }
            }
            UpdateFilterControls();
        }

        //Marca a célula que está com o foco como origem da cópia.
        function CopyFocusedCell() {
            var focusedCells = GetFocusedTDCells();
            if (focusedCells.length == 0) {
                alert("Foque em uma célula.");
                return;
            } else if (focusedCells.length > 1) {
                alert("Erro: existem duas ou mais células focadas.");
                return;
            }
            SetAttrCopiedCells(GetCopiedCells(), "false");
            SetAttrCopied(focusedCells[0], "true");
            SetAttrCutCells(GetCutCells(), "false");
            SetAttrSelected(focusedCells[0], "false");
        }

        //Marca a célula como origem da cópia.
        function CopyCell(cell) {
            if (cell != null && cell.length) {
                SetAttrCopiedCells(GetCopiedCells(), "false");
                SetAttrCut(GetCutCells(), "false");
                SetAttrCopied(cell, "true");
                SetAttrSelected(cell, "false");
            }
        }

        //Marca a célula como origem da cópia (modo Recortar).
        function CutCell(cell) {
            CopyCell(cell);
            SetAttrCutCells(GetCutCells(), "false");
            SetAttrCopiedCells(GetCopiedCells(), "false");
            SetAttrCut(cell, "true");
            SetAttrSelected(cell, "false");
        }

        //Cola o conteúdo nas células selecionadas. Caso nenhuma célula esteja
        //selecionada, cola o conteúdo na célula que está com o foco.
        function PasteToSelectedCells() {
            var copiedCells = GetCopiedCells();
            if (copiedCells.length == 0) {
                copiedCells = GetCutCells();
                if (copiedCells.length == 0) {
                    alert("É necessário copiar antes de colar.");
                    return;
                }
            }
            var content = GetTDContent(copiedCells[0]);
            if (!(content instanceof Array)) {
                alert("Não há dados.");
                return;
            }
            var selectedCells = GetSelectedTDCells();
            if (selectedCells.length == 0) {
                var focusedCells = GetFocusedTDCells();
                if (focusedCells.length == 0) {
                    alert("Marque ao menos uma célula.");
                    return;
                } else {
                    selectedCells = focusedCells;
                }
            }
            selectedCells.each(function(i) {
                SetTDContent(this, content);
            });

            if (GetAttrCut(copiedCells[0]) == "true") {
                SetAttrCut(copiedCells[0], "false");
                CleanCell(copiedCells[0]);
            }
            SetAttrSelectedCells(GetSelectedTDCells(), "false");
        }

        //Cola o conteúdo na célula.
        function PasteToCell(cell) {
            if (cell != null && cell.length) {
                var copiedCells = GetCopiedCells();
                if (copiedCells.length == 0) {
                    copiedCells = GetCutCells();
                    if (copiedCells.length == 0) {
                        alert("É necessário copiar antes de colar.");
                        return;
                    }
                }
                var content = GetTDContent(copiedCells[0]);
                if (!(content instanceof Array)) {
                    alert("Não há dados.");
                    return;
                }
                SetTDContent(cell, content);

                if (GetAttrCut(copiedCells[0]) == "true") {
                    SetAttrCut(copiedCells[0], "false");
                    SetAttrCopied(copiedCells[0], "false");
                    CleanCell(copiedCells[0]);
                }
                SetAttrSelected(cell, "false");
            }
        }

        //Remove a seleção de todas as células.
        function UnselectAllCells() {
            SetAttrCopiedCells(GetAllTDCells(), "false");
            SetAttrSelectedCells(GetAllTDCells(), "false");
        }

        //Copia os dados dos campos Disciplina/Docente para as células
        //selecionadas. Caso nenhuma célula esteja selecionada, copia os dados
        //para a célula que está com o foco.
        function CopyDataToSelectedCells(number0or1) {
            var selectedCells = GetSelectedTDCells();
            if (selectedCells.length == 0) {
                var focusedCells = GetFocusedTDCells();
                if (focusedCells.length == 0) {
                    alert("Marque ao menos uma célula.");
                    return;
                } else {
                    selectedCells = focusedCells;
                }
            }
            var content = GetContentFromFields(number0or1);
            if (content != null) {
                selectedCells.each(function(i) {
                    SetTDContent(this, content);
                });
            }
            UnselectAllCells();
        }

        //Limpa as células selecionadas. Caso nenhuma célula esteja
        //selecionada, limpa a célula que está com o foco.
        function CleanSelectedCells() {
            var selectedCells = GetSelectedTDCells();
            if (selectedCells.length == 0) {
                selectedCells = GetFocusedTDCells();
                if (selectedCells.length == 0) {
                    alert("Selecione uma célula.");
                    return;
                }
                else
                    confirmation = confirm("A atual célula selecionada será limpa.\nDeseja continuar?");
            } else
                confirmation = confirm("As células selecionadas serão limpas.\nDeseja continuar?");
            if (!confirmation) return;

            selectedCells.each(function() {
                var emptyArray = new Array();
                emptyArray[0] = "";
                emptyArray[1] = "";
                emptyArray[2] = "";
                emptyArray[3] = "";
                SetTDContent(this, emptyArray);
            });
            PressedEscape();
        }

        //Limpa todas as células.
        function CleanAllCells() {
            var confirmation = confirm("Todas as células serão limpas.\nDeseja continuar?");
            if (!confirmation) return;

            var emptyArray = new Array();
            emptyArray[0] = "";
            emptyArray[1] = "";
            emptyArray[2] = "";
            emptyArray[3] = "";
            GetAllTDCells().each(function(i) {
                SetTDContent(this, emptyArray);
            });
        }

        //Limpa a célula.
        function CleanCell(cell) {
            if (cell == null) return;
            if (cell.length == 0) return;
            var emptyArray = new Array();
            emptyArray[0] = "";
            emptyArray[1] = "";
            emptyArray[2] = "";
            emptyArray[3] = "";
            SetTDContent(cell, emptyArray);
        }

        //Seleciona todas as células.
        function SelectAllCells() {
            SetAttrSelectedCells(GetAllTDCells(), "true");
        }

        //Seleciona a célula que está com o atual foco.
        function SelectCurrentFocusedCell() {
            GetFocusedTDCells().each(function(i) {
                InvertAttrSelected(this);
                SetAttrCopied(this, "false");
            });
        }

        //Foca no controle (próximo/anterior) em relação ao atual controle focado na tabela.
        // - previousCell == true   =>   Controle anterior.
        // - previousCell != true   =>   Próximo controle.
        function FocusNeighbourCell(previousCell) {
            if (previousCell != true) {
                var currentColumn = null, currentRow = null;
                var lastCell = GetLastTDCell();
                var lastColumn = parseInt($(lastCell).attr("columnIndex"));
                var lastRow = parseInt($(lastCell).attr("rowIndex"));

                GetFocusedTDCells().each(function(i) {
                    if (i == 0) {
                        currentColumn = parseInt($(this).attr("columnIndex"));
                        currentRow = parseInt($(this).attr("rowIndex"));
                    }
                });

                var totalCount = lastRow * 6 + lastColumn;
                var count = currentRow * 6 + currentColumn;

                if (count + 1 > totalCount)
                    FocusCellFirstInput(GetFirstTDCell());
                else {
                    var nextCount = count + 1;
                    var nextRow = parseInt(nextCount / 6);
                    var nextColumn = parseInt(nextCount % 6);
                    FocusCellFirstInput(GetTDCell(nextRow, nextColumn));
                }
            } else {
                var currentColumn = null, currentRow = null;
                GetFocusedTDCells().each(function(i) {
                    if (i == 0) {
                        currentColumn = parseInt($(this).attr("columnIndex"));
                        currentRow = parseInt($(this).attr("rowIndex"));
                    }
                });

                var count = currentRow * 6 + currentColumn;
                if (count - 1 < 0)
                    FocusCellFirstInput(GetLastTDCell());
                else {
                    var nextCount = count - 1;
                    var nextRow = parseInt(nextCount / 6);
                    var nextColumn = parseInt(nextCount % 6);
                    FocusCellFirstInput(GetTDCell(nextRow, nextColumn));
                }
            }
        }
    </script>

    <!-- MÉTODOS AUXILIARES-->

    <script type="text/javascript">
        function LimparMensagem() {
            $("#<%=lblMensagem.ClientID %>").text("");
            $("#<%=lblMensagemErro.ClientID %>").text("");
        }

        function VerificarAbaMatricula(s, e) {
            if (pcTurma.GetActiveTab().name == "Matricula") {
                btnBuscarMatriculas.DoClick();
            }
            VerificarAba(s, e);
        }

        function AutoMarcarCelulas() {
            var content = GetContentFromFields(-1);
            if (content == null || content[0] == "" || content[0] == null ||
                    content[1] == "" || content[1] == null || content[2] == "" ||
                    content[2] == null || content[3] == "" || content[3] == null)
                return;
            GetSelectedTDCells().each(function(i) {
                SetTDContent(this, content);
                //SetAttrSelected(this, "false");
            });
        }

        function BlurTSearch8Caracteres() {
            //Chama evento BLUR quando o campo de código da TSearch atinge 8 caracteres ou mais
            $("#<%=upnlQH1.ClientID %> input[originalValue]").keyup(function(e) {
                if ($(this).val().length >= 10)
                    $(this).blur();
            });
            $("#<%=upnlQH2.ClientID %> input[originalValue]").keyup(function(e) {
                if ($(this).val().length >= 10)
                    $(this).blur();
            });
        }

        function UpdateFilterControls() {
            var matriculaDocente = null, nomeDocente = null, codDisciplina = null, numFunc = null;
            var selectedCells = GetSelectedTDCells();
            if (selectedCells.length == 0) return;

            for (i = 0; i < selectedCells.length; i++) {
                var content = GetTDContent(selectedCells[i]);
                if (content != null && content.length >= 4) {
                    var tmp_matriculaDocente = content[0].split(' - ')[0];
                    var tmp_nomeDocente = content[0].split(' - ')[1];
                    var tmp_numFunc = content[2];
                    var tmp_codDisciplina = content[3];

                    if (tmp_matriculaDocente == null) tmp_matriculaDocente = "";
                    if (tmp_nomeDocente == null) tmp_nomeDocente = "";

                    if (matriculaDocente == null) {
                        matriculaDocente = tmp_matriculaDocente;
                        nomeDocente = tmp_nomeDocente;
                        numFunc = tmp_numFunc;
                    } else if (matriculaDocente != tmp_matriculaDocente) {
                        matriculaDocente = "";
                        nomeDocente = "";
                        numFunc = "";
                    }
                    if (codDisciplina == null) codDisciplina = tmp_codDisciplina;
                    else if (tmp_codDisciplina.indexOf(codDisciplina) < 0) codDisciplina = "";
                } else
                    return;
            }

            var tsDocente1 = tsearchControl("<%=tseDocente.ClientID %>");
            var ddlDisciplina1 = $("#<%=ddlDisciplinaQuadroHorario.ClientID %>");
            var hdnCodigoDocente = $("#<%=hCodigoDocente.ClientID %>");
            var hdnLimpaGrid = $("#<%=hdnLimpaGrid.ClientID %>");
            var hdnLimpaGrid2 = $("#<%=hdnLimpaGrid2.ClientID %>");

            if (tsDocente1 != null && ddlDisciplina1 != null && hdnCodigoDocente != null) {
                tsDocente1.setText(matriculaDocente);
                tsDocente1.setDescription(nomeDocente);
                tsDocente1.clearGrid(true);
                SelectDropDownListItem(document.getElementById("<%=ddlDisciplinaQuadroHorario.ClientID %>"), codDisciplina);
                hdnCodigoDocente.val(numFunc);
                hdnLimpaGrid.val("false");
            }

            var tsDocente2 = tsearchControl("<%=tseDocente2.ClientID %>");
            var ddlDisciplina2 = $("#<%=ddlDisciplinaQuadroHorario2.ClientID %>");
            if (tsDocente2 != null && ddlDisciplina2 != null && hdnCodigoDocente != null) {
                tsDocente2.setText(matriculaDocente);
                tsDocente2.setDescription(nomeDocente);
                tsDocente2.clearGrid(true);
                SelectDropDownListItem(document.getElementById("<%=ddlDisciplinaQuadroHorario2.ClientID %>"), codDisciplina);
                hdnCodigoDocente.val(numFunc);
                hdnLimpaGrid2.val("false");
            }

            //$("#<%=btnTrigger.ClientID %>").click();
        }

        function SelectDropDownListItem(ddl, partialKey) {
            if (partialKey == "" || partialKey == null) {
                for (i = 0; i < ddl.options.length; i++) {
                    if (ddl.options[i].value == "") {
                        ddl.selectedIndex = i;
                        return;
                    }
                }
            }
            for (i = 0; i < ddl.options.length; i++) {

                if (ddl.options[i].value == partialKey) {
                    ddl.selectedIndex = i;
                    return;
                }
            }
            for (i = 0; i < ddl.options.length; i++) {
                if (ddl.options[i].value.indexOf(partialKey) >= 0) {
                    ddl.selectedIndex = i;
                    return;
                }

            }
        }

        //Retorna o código da tecla de um evento KeyDown
        function IdentifyKeyDownCode(event) {
            return event.keyCode ? event.keyCode : event.charCode;
        }

        //Monta um Array contendo os dados das TSearches e DropDowns
        //  - number0or1 == 0, retorna os dados das TSearches/DropDowns superiores
        //  - number0or1 == 1, retorna os dados das TSearches/DropDowns inferiores
        function GetContentFromFields(number0or1) {
            var ddlDisciplinaQH, tseDocente, hCodigoDocente;

            if (number0or1 != 0 && number0or1 != 1) {
                if ($("#<%=ddlDisciplinaQuadroHorario.ClientID %>").attr("flag") == "true") {
                    number0or1 = 0;
                    $("#<%=ddlDisciplinaQuadroHorario.ClientID %>").attr("flag", "false");
                }
                else if ($("#<%=ddlDisciplinaQuadroHorario2.ClientID %>").attr("flag") == "true") {
                    number0or1 = 1;
                    $("#<%=ddlDisciplinaQuadroHorario2.ClientID %>").attr("flag", "false");
                }
            }

            if (number0or1 == 0) {
                ddlDisciplinaQH = document.getElementById("<%=ddlDisciplinaQuadroHorario.ClientID %>");
                tseDocente = tsearchControl("<%=tseDocente.ClientID %>");  // document.getElementById("<%=tseDocente.ClientID %>");
                hCodigoDocente = document.getElementById("<%=hCodigoDocente.ClientID %>");
            } else if (number0or1 == 1) {
                ddlDisciplinaQH = document.getElementById("<%=ddlDisciplinaQuadroHorario2.ClientID %>");
                tseDocente = tsearchControl("<%=tseDocente2.ClientID %>");  //document.getElementById("<%=tseDocente2.ClientID %>");
                hCodigoDocente = document.getElementById("<%=hCodigoDocente.ClientID %>");
            } else {
                return;
            }

            var disciplinaCodigo = ddlDisciplinaQH.value;
            var disciplinaDescricao = ddlDisciplinaQH.options[ddlDisciplinaQH.selectedIndex].text;

            var docenteCodigo = hCodigoDocente.value; //tseDocente.getRow().num_func;
            var docenteDescricao = tseDocente.getText() + " - " + tseDocente.getDescription(); //tseDocente.getRow().matricula + " - " + tseDocente.getRow().nome_compl; //tseDocente.children[3].value + ' - ' + tseDocente.children[4].value;

            if (tseDocente.getRow() != null) {
                docenteCodigo = tseDocente.getRow().num_func;
                docenteDescricao = tseDocente.getText() + " - " + tseDocente.getRow().nome_compl;
            }

            if (docenteCodigo == null || docenteCodigo == "" ||
               disciplinaCodigo == null || disciplinaCodigo == "" ||
               tseDocente.children[3].value == null || tseDocente.children[3].value == "" ||
               tseDocente.children[4].value == null || tseDocente.children[4].value == ""
               ) {
                return null;
            }

            var content = new Array();
            content[0] = docenteDescricao;
            content[1] = disciplinaDescricao;
            content[2] = docenteCodigo;
            content[3] = disciplinaCodigo;
            return content;
        }

        //Obtém o conteúdo dos controles de uma célula. Retorna un Array.
        function GetTDContent(cell) {
            var content = new Array();
            $("input", cell).each(function(i) {
                content[i] = $(this).val();
            });
            return content;
        }

        //Seta o conteúdo dos controles INPUT de uma célula. Content é um Array.
        function SetTDContent(cell, content) {
            if (!(content instanceof Array)) {
                alert("Conteúdo inválido.");
                return;
            }

            var oldContent = GetTDContent(cell);
            var contentChanged = false;

            $("input", cell).each(function(i) {
                if (oldContent[i] != content[i])
                    contentChanged = true;
                $(this).val(content[i]);
            });

            if (contentChanged) {
                UpdateCellControlsColor(cell, content[0].split(" - ")[0]);
            }
            ModifiedControlsIDs(cell);
        }

        function ModifiedControlsIDs(cell) {
            var hiddenField = $("#<%=hControleCelula.ClientID %>");
            var controlID = $("input", cell)[0].id;
            var hiddenFieldValue = $(hiddenField).val();

            controlID = controlID.substr(controlID.indexOf("txtDocente"), controlID.length);

            if (hiddenFieldValue.indexOf(controlID) == -1)
                $(hiddenField).val(hiddenFieldValue + "|" + controlID);
        }

        //Foca no primeiro controle INPUT de uma célula
        function FocusCellFirstInput(cell) {
            SetAttrFocusedCells(GetFocusedTDCells(), "false");
            if (cell != null && cell.length && $("input", cell).length) {
                var firstInput = $("input", cell)[0];
                if ($(firstInput).length && $(firstInput).attr("type") == "text")
                    $(firstInput).focus();
            }
        }

        //Atualiza as cores dos controles da célula, de acordo com o código de matrícula do docente.
        function UpdateCellControlsColor(cell, matriculaDocente) {
            var cellControls = $("input", cell);
            var controleDocente = cellControls[0];
            var controleDisciplina = cellControls[1];

            if (matriculaDocente == "00000000") {
                controleDocente.style.backgroundColor = "";
                controleDisciplina.style.backgroundColor = "";
                controleDocente.className = "textoAmarelo";
                controleDisciplina.className = "textoAmarelo";
                controleDocente.style.color = "Black";
                controleDisciplina.style.color = "Black";
            } else if (matriculaDocente == "99999999") {
                controleDocente.style.backgroundColor = "";
                controleDisciplina.style.backgroundColor = "";
                controleDocente.className = "textoAmareloEscuro";
                controleDisciplina.className = "textoAmareloEscuro";
                controleDocente.style.color = "Black";
                controleDisciplina.style.color = "Black";
            } else if (matriculaDocente == "66666666") {
                controleDocente.style.backgroundColor = "";
                controleDisciplina.style.backgroundColor = "";
                controleDocente.className = "textoAzulEscuro";
                controleDisciplina.className = "textoAzulEscuro";
                controleDocente.style.color = "White";
                controleDisciplina.style.color = "White";
            } else if (matriculaDocente == "55555551") {
                controleDocente.style.backgroundColor = "";
                controleDisciplina.style.backgroundColor = "";
                controleDocente.className = "textoCoral";
                controleDisciplina.className = "textoCoral";
                controleDocente.style.color = "WHITE";
                controleDisciplina.style.color = "WHITE";
            } else if (matriculaDocente == "88888888" || matriculaDocente == "11111111" || matriculaDocente == "22222222" || matriculaDocente == "44444444") {
                controleDocente.style.backgroundColor = "";
                controleDisciplina.style.backgroundColor = "";
                controleDocente.className = "textoLaranja";
                controleDisciplina.className = "textoLaranja";
                controleDocente.style.color = "Black";
                controleDisciplina.style.color = "Black";
            } else if (matriculaDocente == "55555555" || matriculaDocente == "77777777" || matriculaDocente == "88888880" || matriculaDocente == "88888881") {
                controleDocente.style.backgroundColor = "";
                controleDisciplina.style.backgroundColor = "";
                controleDocente.className = "textoPreto";
                controleDisciplina.className = "textoPreto";
                controleDocente.style.color = "Black";
                controleDisciplina.style.color = "Black";
            }
            else {
                controleDocente.style.backgroundColor = "White";
                controleDisciplina.style.backgroundColor = "White";
                controleDocente.style.color = "Black";
                controleDisciplina.style.color = "Black";
            }
        }

        //Exibe o conteúdo da célula no label.
        function ShowCellContent(cell) {
            var innerHTML = "";
            if (cell != null) {
                var content = GetTDContent(cell);
                if (content[0] != "" && content[1] != "") {
                    innerHTML = "Docente: " + content[0] + "<br/>Disciplina: " + content[1];  //+ "<br/>" + content[2] + "<br/>" + content[3];
                }
            }
            $("#dadosDocenteDisciplina").html(innerHTML);
            $("#dadosDocenteDisciplina2").html(innerHTML);
        }

        //Atualiza a classe da célula de acordo com seus atributos
        function RefreshClass(cell) {
            var cls = "";
            var s = GetAttrSelected(cell) == "true";
            var f = GetAttrFocused(cell) == "true";
            var c = GetAttrCopied(cell) == "true";
            var cut = GetAttrCut(cell) == "true";

            if (!s && !f && !c) cls = "default";
            else if (!s && !f && c) cls = "copied";
            else if (!s && f && !c) cls = "focused";
            else if (!s && f && c) cls = "focusedCopied";
            else if (s && !f && !c) cls = "selected";
            else if (s && !f && c) return;
            else if (s && f && !c) cls = "selectedFocused";
            else if (s && f && c) return;
            if (cut && !s) {
                if (f) cls = "focusedCopied";
                else cls = "copied";
            }
            $(cell).attr("class", cls);
        }   
    </script>

    <!-- SELEÇÃO DE CÉLULAS DA TABELA-->

    <script type="text/javascript">
        //Seleciona uma célula específica da tabela. 
        function GetTDCell(rowIndex, columnIndex) {
            return $("#<%=tQuadroHorario.ClientID %> td[columnIndex=" + columnIndex + "][rowIndex=" + rowIndex + "]:first");
        }
        //Seleciona a primeira célula da tabela.
        function GetFirstTDCell() {
            return $("#<%=tQuadroHorario.ClientID %> td:has(input[type=text]):first");
        }
        //Seleciona a última célula da tabela.
        function GetLastTDCell() {
            return $("#<%=tQuadroHorario.ClientID %> td:has(input[type=text]):last");
        }
        //Seleciona todas as células da tabela.
        function GetAllTDCells() {
            return $("#<%=tQuadroHorario.ClientID %> td:has(input[type=text])");
        }
        //Seleciona as células selecionadas da tabela.
        function GetSelectedTDCells() {
            return $("#<%=tQuadroHorario.ClientID %> td[somenteLeitura!=true][selecionado=true]:has(input[type=text])");
        }
        //Seleciona as células focadas da tabela.
        function GetFocusedTDCells() {
            return $("#<%=tQuadroHorario.ClientID %> td[somenteLeitura!=true][foco=true]:has(input[type=text])");
        }
        //Seleciona as células copiadas da tabela.
        function GetCopiedCells() {
            return $("#<%=tQuadroHorario.ClientID %> td[somenteLeitura!=true][copied=true]:has(input[type=text])");
        }
        //Seleciona as células recortadas da tabela.
        function GetCutCells() {
            return $("#<%=tQuadroHorario.ClientID %> td[somenteLeitura!=true][cut=true]:has(input[type=text])");
        }
        //Seleciona as células que possuem o menu de seleção de operação. (Copy/Paste/Cut)
        function GetAllTDCellsWithSelectList() {
            return $("#<%=tQuadroHorario.ClientID %> td[somenteLeitura!=true]:has(img)");
        }
        //Seleciona as células bloqueadas que possuem o menu de seleção de operação.
        function GetLockedTDCellsWithSelectList() {
            return $("#<%=tQuadroHorario.ClientID %> td[somenteLeitura=true]:has(img)");
        }
        //Seleciona as células bloqueadas
        function GetLockedTDCells() {
            return $("#<%=tQuadroHorario.ClientID %> td[somenteLeitura=true]:has(input[type=text])");
        }
    </script>

    <!-- GETTERS/SETTERS DOS ATRIBUTOS (selecionado/foco/copied/cut) DAS CÉLULAS-->

    <script type="text/javascript">
        //Retorna o atributo selectedo da célula
        function GetAttrSelected(cell) {
            return $(cell).attr("selecionado");
        }
        //Seta o atributo selected da célula
        function SetAttrSelected(cell, value) {
            $(cell).attr("selecionado", value);
            if (value == "true") SetAttrCut(cell, "false");
            RefreshClass(cell);
        }
        //Seta o atributo selected das células
        function SetAttrSelectedCells(cells, value) {
            $(cells).each(function(i) {
                if ($(this).attr("somenteLeitura") != "true") {
                    $(this).attr("selecionado", value);
                    RefreshClass(this);
                }
            });
        }
        //Inverte o valor do atributo selected da célula
        function InvertAttrSelected(cell) {
            $(cell).attr("selecionado", $(cell).attr("selecionado") == "true" ? "false" : "true");
            UpdateFilterControls();
            RefreshClass(cell);
        }

        //Retorna o atributo focused da célula
        function GetAttrFocused(cell) {
            return $(cell).attr("foco");
        }
        //Seta o atributo focused da célula
        function SetAttrFocused(cell, value) {
            $(cell).attr("foco", value);
            RefreshClass(cell);
        }
        //Seta o atributo focused das células
        function SetAttrFocusedCells(cells, value) {
            $(cells).each(function() {
                $(this).attr("foco", value);
                RefreshClass(this);
            });
        }
        //Inverte o valor do atributo focused da célula
        function InvertAttrFocused(cell) {
            $(cell).attr("foco", $(cell).attr("foco") == "true" ? "false" : "true");
            RefreshClass(cell);
        }

        //Retorna o atributo copied da célula
        function GetAttrCopied(cell) {
            return $(cell).attr("copied");
        }
        //Seta o atributo copied da célula        
        function SetAttrCopied(cell, value) {
            $(cell).attr("copied", value);
            RefreshClass(cell);
        }
        //Seta o atributo copied das células
        function SetAttrCopiedCells(cells, value) {
            $(cells).each(function(i) {
                $(this).attr("copied", value);
                RefreshClass(this);
            });
        }
        //Inverte o valor do atributo copied da célula
        function InvertAttrCopied(cell) {
            $(cell).attr("copied", $(cell).attr("copied") == "true" ? "false" : "true");
            RefreshClass(cell);
        }

        //Retorna o cut da célula
        function GetAttrCut(cell) {
            return $(cell).attr("cut");
        }
        //Seta o atributo cut da célula
        function SetAttrCut(cell, value) {
            $(cell).attr("cut", value);
            if (value == "true") SetAttrSelected(cell, "false");
            RefreshClass(cell);
        }
        //Seta o atributo cut das células
        function SetAttrCutCells(cells, value) {
            $(cells).each(function(i) {
                $(this).attr("cut", value);
                RefreshClass(this);
            });
        }
    </script>

    <!-- VALIDADORES/FORMATADORES -->

    <script type="text/javascript">
        function validaDecimais8(campo) {
            var regra = /^\d{1,3}(,\d{1,2})?$/i;
            var valida = regra.exec(campo.value);
            if (!valida) {
                campo.value = '0';
            }
        }

        //Usado para permitir apenas número ou vírgula
        function NumeroComVirgula(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if ((charCode > 47 && charCode <= 57) || (charCode == 8))
                return true;
            return false;
        }  
    
    </script>

    <asp:ScriptManagerProxy ID="scriptManager" runat="server" />

    <script type="text/javascript">
        var TipBoxID = "TipBox";
        var tip_box_id;
        var valorCopiado = "";
        var visaoCopiar = "";

        function OnValueChanged(s, e) {
            Page_ClientValidate(""); // validação do requiredfield
        }

        function validateDates(s, e) {
            var date1 = dtIni.GetDate();
            var date2 = dtFim.GetDate();

            if (date1 < date2)
                e.IsValid = true;
            else
                e.IsValid = false;


        }

        function VerificarControle(controle) {
            if (typeof (controle) != 'undefined' && controle != null)
                return true;

            return false;
        }

        function VerificarAba(s, e) {
            var botaoSalvar = document.getElementById('<%=btnSalvar.ClientID %>');
            var botaoCancelar = document.getElementById('<%=btnCancel.ClientID %>');

            if (VerificarControle(botaoSalvar) && VerificarControle(botaoCancelar)) {
                if (pcTurma.GetActiveTab().name == "Geral" || pcTurma.GetActiveTab().name == "Matricula") {
                    botaoCancelar.style.visibility = "visible";
                    botaoSalvar.style.visibility = "visible";
                }
                else {
                    botaoCancelar.style.visibility = "hidden";
                    botaoSalvar.style.visibility = "hidden";
                }
            }
        }

        function VerificaAlteracaoTurma(s, e) {
            var dados = $("#<%= hdnOriginalDadosTurma.ClientID %>").val();
            var dadosQuadro = $("#<%= hdnOriginalQuadro.ClientID %>").val();

            var alteracaoDados, alteracaoQuadro = false;

            //TURNO | DEPENDENCIA | NUM_MAX_ALUNOS | DATA_INICIO - YYYY-MM-DD | DATA_FIM - YYYY-MM-DD
            var orig_turno = dados.split("|")[0];
            var orig_dependencia = dados.split("|")[1];
            var orig_numMaxAlunos = dados.split("|")[2];
            var orig_dataInicio = dados.split("|")[3];
            var orig_dataFim = dados.split("|")[4];

            var turno = $("#<%= ddlTurno.ClientID %>").val();
            var dependencia = $("#<%= ddlDependencia.ClientID %>").val();
            var numMaxAlunos = $("#<%= txtNumMaxAluno.ClientID %>").val();
            var dataInicio = dtIni.GetDate().format("yyyy-MM-dd");
            var dataFim = dtFim.GetDate().format("yyyy-MM-dd");

            alteracaoDados = orig_turno != turno || orig_dependencia != dependencia ||
                orig_numMaxAlunos != numMaxAlunos || orig_dataInicio != dataInicio || orig_dataFim != dataFim;

            var valores = "";

            GetAllTDCells().each(function(i) {
                var inputs = $("input", this);
                var num_func = $(inputs[2]).val();
                var disciplina = $(inputs[3]).val();
                var dia_semana = "";
                var aula = "";
                var inputId = inputs[0].id;
                var valor;

                if (inputId.indexOf("Segunda") >= 0) {
                    dia_semana = 2;
                    aula = inputId.substr(inputId.indexOf("Segunda_") + 8);
                } else if (inputId.indexOf("Terca") >= 0) {
                    dia_semana = 3;
                    aula = inputId.substr(inputId.indexOf("Terca_") + 6);
                } else if (inputId.indexOf("Quarta") >= 0) {
                    dia_semana = 4;
                    aula = inputId.substr(inputId.indexOf("Quarta_") + 7);
                } else if (inputId.indexOf("Quinta") >= 0) {
                    dia_semana = 5;
                    aula = inputId.substr(inputId.indexOf("Quinta_") + 7);
                } else if (inputId.indexOf("Sexta") >= 0) {
                    dia_semana = 6;
                    aula = inputId.substr(inputId.indexOf("Sexta_") + 6);
                } else if (inputId.indexOf("Sabado") >= 0) {
                    dia_semana = 7;
                    aula = inputId.substr(inputId.indexOf("Sabado_") + 7);
                }

                aula = aula.substr(0, aula.indexOf("_"));

                if (num_func != "" && disciplina != "" && dia_semana != "" && aula != "") {
                    valor = aula + ";" + dia_semana + ";" + num_func + ";" + disciplina;
                    valores += valor + "|";
                    if (dadosQuadro.indexOf(valor) < 0) {
                        alteracaoQuadro = true;
                    }
                }
            });

            if (!alteracaoQuadro) {
                var splits = dadosQuadro.split("|");
                for (i = 0; i < splits.length; i++) {
                    if (valores.indexOf(splits[i]) < 0) {
                        alteracaoQuadro = true;
                        break;
                    }
                }
            }

            if (alteracaoDados || alteracaoQuadro) {

                if (!confirm('Deseja salvar o quadro de horários?')) {
                    e.processOnServer = true;
                } else {
                    e.processOnServer = false;
                    s.SetValue($("#<%= tbTurma2.ClientID %>").val());
                    btnSalvarClient.DoClick();
                }
            } else
                e.processOnServer = true;
        }
         
    </script>

    <br />
    <div id="TipBox" class="PopUp">
    </div>
    <asp:Label ID="lblTipoOperacao" runat="server" SkinID="lblNomePagina"></asp:Label>&nbsp;
    <asp:Label Font-Names="Verdana" ID="lblMensagem" SkinID="lblMensagem" runat="server"
        Font-Bold="true"></asp:Label>
    &nbsp;
    <br />
    <br />
    <asp:ValidationSummary ID="vsTurma" runat="server" ShowMessageBox="true" ValidationGroup="SalvarForm"
        ShowSummary="false" />
    <asp:ValidationSummary ID="vsTurmaPesquisa" runat="server" ShowMessageBox="true"
        ValidationGroup="ConfirmarForm" ShowSummary="false" />
    <input type="hidden" id="hControleCelula" runat="server" />
    <div class="divEditBlock" style="width: 90%;">
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:ImageButton ID="btnSalvar" runat="server" ImageAlign="Right" ImageUrl="~/Images/bt_salvar.png"
            OnClick="btnSalvar_Click" ValidationGroup="SalvarForm" />
        <asp:ImageButton ID="btnConfirmar" runat="server" SkinID="BcConfirmar" OnClick="btnConfirmar_Click" />
        <asp:ImageButton ID="btnVoltar" runat="server" ImageAlign="Right" SkinID="Voltar"
            OnClick="btnCancel_Click" />
        <asp:Label runat="server" ID="lblBloco" Text="Turmas" SkinID="BcTitulo" />
    </div>
    <asp:Panel runat="server" ID="pnlDefinicao" GroupingText="Características da turma selecionada"
        Width="90%">
        <table width="90%">
            <tr>
                <td style="text-align: right; width: 10%">
                    <asp:Label Font-Names="Verdana" ID="lblUnidadeResponsavel" runat="server" Text="Unidade Ensino:*"
                        SkinID="lblObrigatorio"> </asp:Label>
                </td>
                <td style="text-align: left; width: 50%">
                    <tweb:TSearchBox ID="tseUnidadeResponsavel" runat="server" Caption="" Key="unidade_ens"
                        EnableViewState="true" Argument="nome_comp" SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao,municipio,ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO "
                        ArgumentColumns="75" Columns="10" OnChanged="tseUnidadeResponsavel_Changed" GridWidth="850px"
                        SqlOrder="nome_comp">
                        <gridcolumns>
                            <tweb:TSearchBoxColumn Caption="Censo" FieldName="unidade_ens" Width="12%" />
                            <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="CGC" FieldName="cgc" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                            <tweb:TSearchBoxColumn Caption="Municipio" FieldName="municipio" Width="18%" />
                        </gridcolumns>
                    </tweb:TSearchBox>
                    <asp:RequiredFieldValidator ID="rfvUnidadeResponsavel" runat="server" ControlToValidate="tseUnidadeResponsavel"
                        ErrorMessage="Unidade Ensino: Preenchimento obrigatório." InitialValue="" ValidationGroup="SalvarForm"><img title="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                </td>
                <td align="right">
                    <asp:Label Font-Names="Verdana" ID="lblUA" runat="server" Text="U.A.:" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:Label Font-Names="Verdana" ID="lblUAValor" runat="server" SkinID="lblObrigatorio"></asp:Label>
                    <asp:HiddenField ID="hdnUA" runat="server" />
                </td>
                <td colspan="4">
                    <asp:LinkButton ID="btnPagHorOper" Text="Ir Para Página de Horário Operacional" runat="server"
                        OnClick="btnPagHorOper_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label Font-Names="Verdana" ID="Label7" runat="server" Text="Curso:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:Label Font-Names="Verdana" ID="lblCursoDesc" runat="server" SkinID="lblObrigatorio"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="text-align: right;" class="style2">
                    <asp:Label Font-Names="Verdana" ID="Label2" runat="server" Text="Turma:"></asp:Label>
                </td>
                <td style="text-align: left;" class="style3">
                    <dxe:ASPxComboBox ID="ddlTurmasUnidadeDev" Width="300px" ValueField="turma" TextField="turma_nome"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlTurmasUnidade_SelectedIndexChanged"
                        runat="server">
                        <clientsideevents valuechanged="function(s,e) { VerificaAlteracaoTurma(s,e); }" />
                    </dxe:ASPxComboBox>
                    <asp:HiddenField ID="tbTurma2" runat="server" />
                </td>
                <td style="text-align: right">
                    <asp:Label Font-Names="Verdana" ID="lblAno" runat="server" Text="Ano:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td align="left">
                    <asp:DropDownList Height="20px" ID="ddlAno" runat="server" DataTextField="ano" DataValueField="ano"
                        Enabled="false" Width="70px" AutoPostBack="True" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="rfvAno" runat="server" ControlToValidate="ddlAno"
                        ErrorMessage="Ano: Preenchimento obrigatório." InitialValue="" ValidationGroup="SalvarForm"><img title="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="rfvAnoPesquisa" runat="server" ControlToValidate="ddlAno"
                        ErrorMessage="Ano: Preenchimento obrigatório." InitialValue="" ValidationGroup="ConfirmarForm"><img title="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                </td>
                <td style="text-align: right">
                    <asp:Label Font-Names="Verdana" ID="lblPeriodo" runat="server" Text="Período:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td align="left">
                    <asp:DropDownList Height="20px" ID="ddlPeriodo" runat="server" DataTextField="id_reduzida"
                        DataValueField="periodo" Enabled="false" Width="100px" AutoPostBack="True" OnSelectedIndexChanged="ddlPeriodo_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="rfvPeriodo" runat="server" ControlToValidate="ddlPeriodo"
                        ErrorMessage="Período: Preenchimento obrigatório." InitialValue="" ValidationGroup="SalvarForm"><img title="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="rfvPeriodoPesquisa" runat="server" ControlToValidate="ddlPeriodo"
                        ErrorMessage="Período: Preenchimento obrigatório." InitialValue="" ValidationGroup="ConfirmarForm"><img title="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <techne:TTableDataSource ID="tdsTipoGestao" runat="server" DataTableClassName="Techne.Lyceum.CR.Itemtabela"
        SqlColumns="ITEM, DESCR" SqlWhere="TAB = 'TipoGestaoTurma'" SqlOrder="DESCR">
    </techne:TTableDataSource>
    <asp:ObjectDataSource ID="odsCurso" runat="server" TypeName="Techne.Lyceum.RN.Curso"
        SelectMethod="ConsultarCursosPorUnidadeEnsino">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseUnidadeResponsavel" Name="unidadeEns" PropertyName="Value" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <dxtc:ASPxPageControl ID="pcTurma" ClientInstanceName="pcTurma" runat="server" ActiveTabIndex="0"
        ClientSideEvents-ActiveTabChanged="function(s, e) { VerificarAba(s,e); }" Width="100%"
        ClientSideEvents-Init="function(s, e) { VerificarAba(s,e); }" ClientSideEvents-TabClick="function(s,e) { LimparMensagem(s,e); } ">
        <tabpages>
            <dxtc:TabPage Name="Geral" Text="Geral">
                <ContentCollection>
                    <dxw:ContentControl ID="ccGeral" runat="server">
                        <asp:Panel ID="pnlCadastro" runat="server">
                            <asp:Panel ID="pnlCaracteristica" runat="server" GroupingText="Características">
                                <asp:Panel runat="server" ID="pnlDefinicaoGeral">
                                    <table width="100%">
                                        <tr>
                                            <td style="text-align: right; vertical-align: middle; width: 15%">
                                                <asp:Label Font-Names="Verdana" ID="Label3" runat="server" Text="Tipo Gestão:*" SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList Height="20px" ID="ddlTipoGestao" runat="server" DataSourceID="tdsTipoGestao"
                                                    DataValueField="item" DataTextField="DESCR" Width="250px" AutoPostBack="True">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvTipoGestao" runat="server" ControlToValidate="ddlTipoGestao"
                                                    ErrorMessage="Turno: Preenchimento obrigatório." InitialValue="" ValidationGroup="SalvarForm"><img title="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvTipoGestaoPesquisa" runat="server" ControlToValidate="ddlTipoGestao"
                                                    ErrorMessage="Turno: Preenchimento obrigatório." InitialValue="" ValidationGroup="ConfirmarForm"><img title="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; width: 10%">
                                                <asp:Label Font-Names="Verdana" ID="lblCurso" runat="server" Text="Escolaridade:*"
                                                    SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td colspan="4" style="width: 90%">
                                                <table cellpadding="0px">
                                                    <tr>
                                                        <td>
                                                            <dxe:ASPxComboBox ID="ddlCurso" runat="server" DataSourceID="odsCurso" ValueField="curso"
                                                                AutoPostBack="true" TextFormatString="{0}" DropDownWidth="700px" OnSelectedIndexChanged="ddlCurso_SelectedIndexChanged"
                                                                Width="480px" CssClass="ReadOnlyField" Height="5px" ClientInstanceName="ddlCurso">
                                                                <Columns>
                                                                    <dxe:ListBoxColumn Caption="Nome" FieldName="nome" Width="30%" />
                                                                    <dxe:ListBoxColumn Caption="Curso" FieldName="curso" Width="10%" />
                                                                    <dxe:ListBoxColumn Caption="Nível" FieldName="descricao_nivel" Width="30%" />
                                                                    <dxe:ListBoxColumn Caption="Modalidade" FieldName="descricao_modalidade" Width="30%" />
                                                                </Columns>
                                                            </dxe:ASPxComboBox>
                                                        </td>
                                                        <td>
                                                            <asp:RequiredFieldValidator ID="rfvCurso" runat="server" ControlToValidate="ddlCurso"
                                                                ErrorMessage="Escolaridade: Preenchimento obrigatório." InitialValue="" ValidationGroup="SalvarForm"><img title="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                                                            <asp:RequiredFieldValidator ID="rfvCursoPesquisa" runat="server" ControlToValidate="ddlCurso"
                                                                ErrorMessage="Escolaridade: Preenchimento obrigatório." InitialValue="" ValidationGroup="ConfirmarForm"><img title="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; vertical-align: middle; width: 15%">
                                                <asp:Label Font-Names="Verdana" ID="lblTurno" runat="server" Text="Turno:*" SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList Height="20px" ID="ddlTurno" runat="server" DataTextField="descricao"
                                                    DataValueField="turno" Width="250px" AutoPostBack="True" OnSelectedIndexChanged="ddlTurno_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvTurno" runat="server" ControlToValidate="ddlTurno"
                                                    ErrorMessage="Turno: Preenchimento obrigatório." InitialValue="" ValidationGroup="SalvarForm"><img title="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvTurnoPesquisa" runat="server" ControlToValidate="ddlTurno"
                                                    ErrorMessage="Turno: Preenchimento obrigatório." InitialValue="" ValidationGroup="ConfirmarForm"><img title="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr id="trCurriculo" runat="server">
                                            <td style="text-align: right; width: 10%">
                                                <asp:Label Font-Names="Verdana" ID="lblCurriculo" runat="server" Text="Matriz Curricular:*"
                                                    SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList Height="20px" ID="ddlCurriculo" runat="server" DataTextField="curriculo"
                                                    DataValueField="curriculo" Width="250px" AutoPostBack="true" OnSelectedIndexChanged="ddlCurriculo_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvCurriculo" runat="server" ControlToValidate="ddlCurriculo"
                                                    ErrorMessage="Matriz Curricular: Preenchimento obrigatório." InitialValue=""
                                                    ValidationGroup="SalvarForm"><img title="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; width: 10%">
                                                <asp:Label Font-Names="Verdana" ID="lblSerie" runat="server" Text="Ano de Escolaridade:*"
                                                    SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td style="text-align: left;" colspan="3">
                                                <asp:DropDownList Height="20px" ID="ddlSerie" runat="server" AutoPostBack="True"
                                                    DataTextField="descricao" DataValueField="serie" OnSelectedIndexChanged="ddlSerie_SelectedIndexChanged"
                                                    Width="480px">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvSerie" runat="server" ControlToValidate="ddlSerie"
                                                    ErrorMessage="Ano de Escolaridade: Preenchimento obrigatório." ValidationGroup="SalvarForm"><img 
                                            title="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvSeriePesquisa" runat="server" ControlToValidate="ddlSerie"
                                                    ErrorMessage="Ano de Escolaridade: Preenchimento obrigatório." ValidationGroup="ConfirmarForm"><img 
                                            title="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                                            </td>
                                            <td style="text-align: right;" class="style4">
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr id="trTurma" runat="server">
                                            <td style="text-align: right; width: 10%">
                                                <asp:Label Font-Names="Verdana" ID="lblPrefixoSerie" runat="server" Text="Prefixo do Ano de Escolaridade:"></asp:Label>
                                            </td>
                                            <td style="text-align: left; margin-left: 0px">
                                                <table cellspacing="0px" border="0px">
                                                    <tr style="margin-bottom: 0px">
                                                        <td style="text-align: left;" class="style1">
                                                            <asp:TextBox ID="txtPrefixoSerie" runat="server" CssClass="ReadOnlyField" ReadOnly="true"></asp:TextBox>
                                                        </td>
                                                        <td style="text-align: right;" class="style2">
                                                            <asp:Label Font-Names="Verdana" ID="lblTurma" runat="server" Text="Turma:*" SkinID="lblObrigatorio"
                                                                Width="100px"></asp:Label>
                                                        </td>
                                                        <td style="text-align: left;" class="style3" id="tdTurma" runat="server">
                                                            <asp:DropDownList Height="20px" ID="ddlTurma" runat="server" AutoPostBack="True"
                                                                CssClass="ReadOnlyField" Enabled="false" DataTextField="turma" DataValueField="turma">
                                                            </asp:DropDownList>
                                                            <asp:TextBox ID="txtTurma" runat="server" CssClass="ReadOnlyField" MaxLength="50"
                                                                Height="16px" Width="70%"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvTurmaPesquisa" runat="server" ControlToValidate="ddlTurma"
                                                                ErrorMessage="Turma: Preenchimento obrigatório." ValidationGroup="ConfirmarForm"><img 
                                                                title="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                                                            <asp:RequiredFieldValidator ID="rfvTurma" runat="server" ControlToValidate="txtTurma"
                                                                ErrorMessage="Turma: Preenchimento obrigatório." ValidationGroup="SalvarForm"><img 
                                                                title="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                                                            <asp:CustomValidator ID="cvalTurma" runat="server" ErrorMessage="O código de turma gerado (Prefixo + Turma + Sufixo + '-' + UA) não pode exceder o limite de 50 caracteres."
                                                                ValidationGroup="SalvarForm" ClientValidationFunction="ValidarNomeTurma">
                                                            <img title="O código de turma gerado (Prefixo + Turma + Sufixo + '-' + UA) não pode exceder o limite de 50 caracteres." src="../Images/AlertaMens.gif" /></asp:CustomValidator>
                                                        </td>
                                                        <td style="text-align: right;" class="style4">
                                                            <asp:Label Font-Names="Verdana" ID="Label1" runat="server" Style="white-space: nowrap"
                                                                Text="Sufixo do Ano de Escolaridade:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList Height="20px" ID="ddlSufixoSerie" runat="server" DataTextField="descricao"
                                                                DataValueField="sufixo" Width="100px">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <table width="100%">
                                    <tr>
                                        <td style="text-align: right; width: 15%">
                                            <asp:Label Font-Names="Verdana" ID="lblUnidadeFisica" runat="server" Text="Unidade Física:*"
                                                SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <tweb:TSearchBox ID="tseUnidadeFisica" runat="server" Argument="nome_comp" AutoPostBack="true"
                                                Caption="" Key="unidade_fis" OnChanged="tseUnidadeFisica_Changed" SqlSelect="select unidade_fis, nome_comp from ly_unidade_fisica"
                                                SqlOrder="nome_comp" CssClass="ReadOnlyField">
                                                <GridColumns>
                                                    <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_fis" Width="20%" />
                                                    <tweb:TSearchBoxColumn Caption="Nome" FieldName="nome_comp" Width="80%" />
                                                </GridColumns>
                                            </tweb:TSearchBox>
                                            <asp:RequiredFieldValidator ID="rfvUnidadeFisica" runat="server" ControlToValidate="tseUnidadeFisica"
                                                ErrorMessage="Unidade Física: Preenchimento obrigatório." InitialValue="" ValidationGroup="SalvarForm"><img title="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; width: 15%">
                                            <asp:Label Font-Names="Verdana" ID="lblAmbienteExterno" runat="server" Text="Realiza atividade externa a unidade?"
                                                Visible="false"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox runat="server" ID="chkAmbienteExterno" Enabled="true" OnCheckedChanged="chkAmbienteExterno_CheckedChanged"
                                                AutoPostBack="true" Visible="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <!-- Anderson Wernek INÍCIO -->
                                        <td style="text-align: right; width: 15%">
                                            <asp:Label Font-Names="Verdana" ID="Label4" runat="server" Text="Optativa:*"
                                                SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList Height="20px" ID="ddlOptativaReforco" runat="server" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlOptativaReforco_SelectedIndexChanged" Width="350px"
                                                CssClass="ReadOnlyField">
                                                <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                                <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                                <asp:ListItem Value="R" Text="Ensino Religioso"></asp:ListItem>
                                                <asp:ListItem Value="L" Text="Língua Estrangeira Optativa"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <!-- Anderson Wernek FIM -->
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; width: 15%">
                                            <asp:Label Font-Names="Verdana" ID="lblDependencia" runat="server" Text="Sala de Aula:*"
                                                SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList Height="20px" ID="ddlDependencia" runat="server" DataTextField="DESCRICAO"
                                                DataValueField="DEPENDENCIA" Width="350px" CssClass="ReadOnlyField" AutoPostBack="True"
                                                OnSelectedIndexChanged="ddlDependencia_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; width: 15%">
                                            <asp:Label Font-Names="Verdana" ID="lblNumMaxAluno" runat="server" Text="Número Máximo de Alunos à Enturmar:*"
                                                SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtNumMaxAluno" MaxLength="3" CssClass="ReadOnlyField"
                                                Width="50px" SkinID="numerico"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvNumMaxAluno" runat="server" ControlToValidate="txtNumMaxAluno"
                                                ErrorMessage="Número Máximo de Alunos: Preenchimento obrigatório." InitialValue=""
                                                ValidationGroup="SalvarForm"><img title="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; width: 15%">
                                            <asp:Label Font-Names="Verdana" ID="lblCapacidadeSala" runat="server" Text="Capacidade da Sala:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtCapacidadeSala" CssClass="ReadOnlyField" Width="50px"
                                                Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; width: 15%">
                                            <asp:Label Font-Names="Verdana" ID="lblCapacidadeTurma" runat="server" Text="Capacidade da Turma (resolução):"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtCapacidadeTurma" CssClass="ReadOnlyField" Width="50px"
                                                Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; width: 15%">
                                            <asp:Label Font-Names="Verdana" ID="Label5" runat="server" Text="Eletiva:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkEletiva" runat="server" AutoPostBack="true" OnCheckedChanged="chkEletiva_CheckedChanged" />
                                        </td>
                                    </tr>
                                    <tr id="trTurmaReferencia" runat="server" visible="false">
                                        <td style="text-align: right; width: 15%">
                                            <asp:Label Font-Names="Verdana" ID="Label6" runat="server" Text="Turma Referência:" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlTurmaReferencia" runat="server" DataTextField="turma" DataValueField="turma">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trAlunosMatriculados" runat="server">
                                        <td style="text-align: right; width: 15%">
                                            <asp:Label Font-Names="Verdana" ID="lblAlunoMatricula" runat="server" Text="Alunos Matriculados:" />
                                        </td>
                                        <td>
                                            <asp:Label Font-Names="Verdana" ID="lblAlunoMatriculaValor" runat="server" />
                                        </td>
                                    </tr>
                                    <tr id="trAlunosMatriculadosProgressao" runat="server">
                                        <td style="text-align: right; width: 15%">
                                            <asp:Label Font-Names="Verdana" ID="lblAlunoMatriculaProgressao" runat="server" Text="Alunos Matriculados em Prog. Parcial:" />
                                        </td>
                                        <td>
                                            <asp:Label Font-Names="Verdana" ID="lblAlunoMatriculaProgressaoValor" runat="server" />
                                        </td>
                                    </tr>                                    
                                    <tr id="trAlunosMatriculadosEletiva1" runat="server">
                                        <td style="text-align: right; width: 15%">
                                            <asp:Label Font-Names="Verdana" ID="lblAlunoMatriculaEletiva1" runat="server" Text="Alunos Matriculados em Eletiva 1:" />
                                        </td>
                                        <td>
                                            <asp:Label Font-Names="Verdana" ID="lblAlunoMatriculaEletiva1Valor" runat="server" />
                                        </td>
                                    </tr>
                                    <tr id="trAlunosMatriculadosEletiva2" runat="server">
                                        <td style="text-align: right; width: 15%">
                                            <asp:Label Font-Names="Verdana" ID="lblAlunoMatriculaEletiva2" runat="server" Text="Alunos Matriculados em Eletiva 2:" />
                                        </td>
                                        <td>
                                            <asp:Label Font-Names="Verdana" ID="lblAlunoMatriculaEletiva2Valor" runat="server" />
                                        </td>
                                    </tr>
                                    <tr id="trAlunosMatriculadosEletiva3" runat="server">
                                        <td style="text-align: right; width: 15%">
                                            <asp:Label Font-Names="Verdana" ID="lblAlunoMatriculaEletiva3" runat="server" Text="Alunos Matriculados em Eletiva 3:" />
                                        </td>
                                        <td>
                                            <asp:Label Font-Names="Verdana" ID="lblAlunoMatriculaEletiva3Valor" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblMacros" runat="server"
                                                Text="Macrocampos:" Visible="false"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBoxList ID="chkMacros" runat="server" DataTextField="NOME_MACRO" DataValueField="ID_MACRO_CAMPOS"
                                                Visible="false" RepeatDirection="Horizontal">
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                    <tr id="trCriacao" runat="server">
                                        <td style="text-align: right; width: 15%">
                                            <asp:Label Font-Names="Verdana" ID="lblCriacao" runat="server" Text="Data de criação:" />
                                        </td>
                                        <td>
                                            <asp:Label Font-Names="Verdana" ID="lblCriacaoValor" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlPeriodoLetivo" runat="server" GroupingText="Período Letivo">
                                <table width="100%">
                                    <tr>
                                        <td style="text-align: right; width: 10%">
                                            <asp:Label Font-Names="Verdana" ID="lblIniAula" runat="server" Text="Início das Aulas:*"
                                                SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <dxe:ASPxDateEdit ID="dtIniAula" runat="server" CssClass="ReadOnlyField" ClientInstanceName="dtIni">
                                                            <ClientSideEvents ValueChanged="OnValueChanged" />
                                                        </dxe:ASPxDateEdit>
                                                    </td>
                                                    <td>
                                                        <asp:RequiredFieldValidator ID="rfvDtIniAula" runat="server" ControlToValidate="dtIniAula"
                                                            ErrorMessage="Início das Aulas: Preenchimento obrigatório." InitialValue="" ValidationGroup="SalvarForm"><img title="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; width: 15%">
                                            <asp:Label Font-Names="Verdana" ID="lblFimAula" runat="server" Text="Término das Aulas:*"
                                                SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <dxe:ASPxDateEdit ID="dtFimAula" runat="server" CssClass="ReadOnlyField" ClientInstanceName="dtFim"
                                                            AutoPostBack="true" OnDateChanged="dtFimAula_DateChanged">
                                                            <ClientSideEvents ValueChanged="OnValueChanged" />
                                                        </dxe:ASPxDateEdit>
                                                    </td>
                                                    <td>
                                                        <asp:RequiredFieldValidator ID="rfvDtFimAula" runat="server" ControlToValidate="dtFimAula"
                                                            ErrorMessage="Término das Aulas: Preenchimento obrigatório." InitialValue=""
                                                            ValidationGroup="SalvarForm"><img title="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                                                        <asp:CustomValidator ID="cvDt" runat="server" ClientValidationFunction="validateDates"
                                                            ValidationGroup="SalvarForm" ErrorMessage="Valor de Início das Aulas deve ser menor ou igual que o valor de Término das Aulas"><img title="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:CustomValidator>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </asp:Panel>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Name="QuadroHorario" Text="Quadro de Horários">
                <ContentCollection>
                    <dxw:ContentControl ID="ccQuadroHorario" runat="server">
                        <br />
                        <asp:Label Font-Names="Verdana" ID="lblMensagemQHI" runat="server" Text="Horário operacional não cadastrado."
                            Visible="false"></asp:Label>
                        <asp:Panel runat="server" ID="pnlQuadroHorario">
                            <asp:UpdatePanel ID="upnlQH1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnTrigger" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td style="text-align: right; width: 10%">
                                                <asp:Label Font-Names="Verdana" ID="lblDisciplinaQuadroHorario" runat="server" Text="Componente Curricular:"></asp:Label>
                                            </td>
                                            <td style="width: 30%">
                                                <asp:DropDownList Height="20px" ID="ddlDisciplinaQuadroHorario" runat="server" DataTextField="NOME"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlDisciplinaQuadroHorario_SelectedIndexChanged"
                                                    DataValueField="DISCIPLINA" Width="500px" CssClass="ReadOnlyField">
                                                </asp:DropDownList>
                                            </td>
                                            <td rowspan="2" valign="top" style="width: 60%">
                                                <input type="hidden" id="hCodigoDocente" runat="server" />
                                                <asp:HiddenField ID="hFPermissao" runat="server" />
                                                <span id="dadosDocenteDisciplina"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; width: 10%">
                                                <asp:Label Font-Names="Verdana" ID="lblMatriculaProfessor" runat="server" Text="Matrícula ou ID/Vínculo do Professor:"></asp:Label>
                                            </td>
                                            <td style="width: 30%">
                                                <tweb:TSearch ID="tseDocente" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryDocenteQHI"
                                                    MaxLength="20" AutoPostBack="true" ValidateText="true" OnSelecting="tseDocente_Selecting"
                                                    OnTextChanged="tseDocente_Changed">
                                                    <ClientSideEvents ValueChanged="AutoUpdateCellsContent(0);" />
                                                    <QueryParameters>
                                                        <asp:Parameter Name="disciplina" DefaultValue="''" DbType="String" />
                                                        <asp:Parameter Name="dtInicio" DbType="DateTime" />
                                                        <asp:Parameter Name="dtFim" DbType="DateTime" />
                                                        <asp:Parameter Name="voluntario" DbType="String" />
                                                    </QueryParameters>
                                                </tweb:TSearch>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:HiddenField ID="hdnLimpaGrid" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <table>
                                <tr>
                                    <td align="right" id="tdControleQH" runat="server" colspan="2">
                                        <asp:ImageButton ID="btnCancel2" runat="server" SkinID="Cancelar" OnClick="btnCancel_Click" />
                                        <asp:ImageButton ID="btnSalvar2" runat="server" ImageUrl="~/Images/bt_salvar.png"
                                            OnClick="btnSalvar_Click" ValidationGroup="SalvarForm" />
                                        <img id="btnMarcar2" src="../Images/bot_marcar.png" title="Marcar células selecionadas"
                                            border="0" style="cursor: pointer" onclick="javascript:CopyDataToSelectedCells(0);"
                                            runat="server" />
                                        <img id="btnLimpar2" src="../Images/bot_limpar.png" title="Limpar celulas selecionadas"
                                            border="0" style="cursor: pointer" onclick="javascript:CleanCells();" runat="server" />
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="btnTransferencia" runat="server" SkinID="BcTransferir" Visible="true"
                                            OnClientClick="javascript:return AtualizarCampoCelulasMarcadas();" OnClick="btnTransferencia_Click" />
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="btnImprimir" runat="server" Visible="false" SkinID="Imprimir" />
                                    </td>
                                </tr>
                            </table>
                            <asp:Image ID="aMensagemErro" runat="server" Visible="false" ImageUrl="~/Images/AlertaMens.gif"
                                CssClass="ImagemAviso" />
                            <asp:Label ID="lblMensagemErro" runat="server" Visible="false" SkinID="lblMensagem"></asp:Label>
                            <div id="TipErro" class="PopUpErro">
                            </div>
                            <br />
                            <asp:Table ID="tQuadroHorario" runat="server" CellSpacing="0" Width="95%" EnableViewState="true">
                                <asp:TableHeaderRow ID="trCabecalho" Height="20px" runat="server">
                                    <asp:TableHeaderCell CssClass="bordaHeader" Text="Início/Término" Width="10%"></asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="bordaHeader" Text="Segunda" Width="10%"></asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="bordaHeader" Width="2%"></asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="bordaHeader" Text="Terça" Width="10%"></asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="bordaHeader" Width="2%"></asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="bordaHeader" Text="Quarta" Width="10%"></asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="bordaHeader" Width="2%"></asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="bordaHeader" Text="Quinta" Width="10%"></asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="bordaHeader" Width="2%"></asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="bordaHeader" Text="Sexta" Width="10%"></asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="bordaHeader" Width="2%"></asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="bordaHeader" Text="Sábado" Width="10%"></asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="bordaHeader" Width="2%"></asp:TableHeaderCell>
                                </asp:TableHeaderRow>
                            </asp:Table>
                            <br />
                            <asp:UpdatePanel ID="upnlQH2" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnTrigger" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td style="text-align: right; width: 10%">
                                                <asp:Label Font-Names="Verdana" ID="lblDisciplinaQuadroHorario2" runat="server" Text="Componente Curricular:"></asp:Label>
                                            </td>
                                            <td style="width: 30%">
                                                <asp:DropDownList Height="20px" ID="ddlDisciplinaQuadroHorario2" runat="server" DataTextField="NOME"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlDisciplinaQuadroHorario2_SelectedIndexChanged"
                                                    DataValueField="DISCIPLINA" Width="500px" CssClass="ReadOnlyField">
                                                </asp:DropDownList>
                                            </td>
                                            <td rowspan="3" valign="top">
                                                <span id="dadosDocenteDisciplina2"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; width: 10%">
                                                <asp:Label Font-Names="Verdana" ID="lblMatriculaProfessor2" runat="server" Text="Matrícula do Professor:"></asp:Label>
                                            </td>
                                            <td style="width: 30%" colspan="2">
                                                <tweb:TSearch ID="tseDocente2" runat="server" MaxLength="20" SettingsTypeName="Techne.Lyceum.RN.Query.QueryDocenteQHI"
                                                    AutoPostBack="true" ValidateText="true" OnTextChanged="tseDocente2_Changed" OnSelecting="tseDocente2_Selecting">
                                                    <ClientSideEvents ValueChanged="AutoUpdateCellsContent(1);" />
                                                    <QueryParameters>
                                                        <asp:Parameter Name="disciplina" DefaultValue="''" DbType="String" />
                                                        <asp:Parameter Name="dtInicio" DbType="DateTime" />
                                                        <asp:Parameter Name="dtFim" DbType="DateTime" />
                                                    </QueryParameters>
                                                </tweb:TSearch>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:HiddenField ID="hdnLimpaGrid2" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <table>
                                <tr>
                                    <td runat="server" id="tdControleQH2" align="right" colspan="2">
                                        <asp:ImageButton ID="btnCancel3" runat="server" SkinID="Cancelar" OnClick="btnCancel_Click" />
                                        <asp:ImageButton ID="btnSalvar3" runat="server" ImageUrl="~/Images/bt_salvar.png"
                                            OnClick="btnSalvar_Click" ValidationGroup="SalvarForm" />
                                        <img id="btnMarcar3" src="../Images/bot_marcar.png" title="Marcar células selecionadas"
                                            border="0" style="cursor: pointer" onclick="javascript:CopyDataToSelectedCells(1);"
                                            runat="server" />
                                        <img id="btnLimpar3" src="../Images/bot_limpar.png" title="Limpar celulas selecionadas"
                                            border="0" style="cursor: pointer" onclick="javascript:CleanCells();" runat="server" />
                                        <asp:Button ID="btnTrigger" runat="server" Style="visibility: hidden" />
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="btnTransferencia2" runat="server" SkinID="BcTransferir" Visible="true"
                                            OnClientClick="javascript:return AtualizarCampoCelulasMarcadas();" OnClick="btnTransferencia_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="upnlTransferencia" runat="server" ChildrenAsTriggers="true"
                                            UpdateMode="Conditional">
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnTransferencia" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnTransferencia2" EventName="Click" />
                                            </Triggers>
                                            <ContentTemplate>
                                                <asp:HiddenField ID="hdnTransferenciaSelecionada" runat="server" />
                                                <dxpc:ASPxPopupControl ID="puTransferencia" runat="server" CloseAction="CloseButton"
                                                    Modal="true" PopupHorizontalAlign="WindowCenter" Top="150" ShowPageScrollbarWhenModal="true"
                                                    Width="800" HeaderText="Transferência de Alocações" AllowDragging="true" EnableAnimation="true">
                                                    <Border BorderColor="Gainsboro" BorderStyle="Ridge" BorderWidth="4px" />
                                                    <ContentCollection>
                                                        <dxpc:PopupControlContentControl>
                                                            <dxwgv:ASPxGridView ID="grdTransferenciaSelecionada" ClientInstanceName="grdTransferenciaSelecionada"
                                                                Width="100%" runat="server">
                                                                <SettingsPager Visible="false" />
                                                                <Columns>
                                                                    <dxwgv:GridViewDataTextColumn Caption="Matrícula" FieldName="Matricula" Width="75px" />
                                                                    <dxwgv:GridViewDataTextColumn Caption="Docente" FieldName="NomeDocente" />
                                                                    <dxwgv:GridViewDataTextColumn Caption="Disciplina" FieldName="DisciplinaDescr" />
                                                                    <dxwgv:GridViewDataTextColumn Caption="Dia da Semana" FieldName="DiaSemanaDescr"
                                                                        Width="80px" />
                                                                    <dxwgv:GridViewDataTextColumn Caption="Hora de Entrada" FieldName="HoraIni" Width="60px" />
                                                                    <dxwgv:GridViewDataTextColumn Caption="Hora de Saída" FieldName="HoraFim" Width="60px" />
                                                                </Columns>
                                                            </dxwgv:ASPxGridView>
                                                            <br />
                                                            <asp:Label ID="lblTransferenciaMensagem" runat="server" Visible="false" Font-Size="XX-Small"
                                                                SkinID="lblMensagem" />
                                                            <br />
                                                            <asp:Panel ID="pnlTransferenciaFiltro" GroupingText="Selecione os dados para a transferência de alocações"
                                                                runat="server" Wrap="false">
                                                                <table>
                                                                    <tr>
                                                                        <td align="right">
                                                                            <asp:Label Text="Turma destino:*" SkinID="lblObrigatorio" runat="server" />
                                                                        </td>
                                                                        <td>
                                                                            <tweb:TSearchBox ID="tseTurmaTransferencia" runat="server" Caption="" Key="grade_id"
                                                                                DataType="Number" MaxLength="50" ArgumentColumns="50" Columns="10" Argument="grade"
                                                                                SqlSelect=" SELECT DISTINCT GS.grade_id as grade_id, GS.grade as grade, t.descricao as turno_descr, t.turno as turno, s.DESCRICAO as serie, gs.ano as ano, gs.semestre as semestre FROM LY_GRADE_SERIE GS inner join ly_unidade_ensino US ON US.unidade_ens = GS.faculdade inner join ly_nucleo N ON N.nucleo = US.nucleo inner join ly_turno T ON T.turno = GS.turno inner join ly_unidade_ensino UE ON UE.UNIDADE_ENS = GS.UNIDADE_RESPONSAVEL inner join ly_serie S ON S.SERIE = GS.SERIE AND S.TURNO = GS.TURNO AND S.CURRICULO = GS.CURRICULO inner join LY_CURSO C ON C.CURSO = GS.CURSO inner join LY_TURMA TU ON TU.TURMA =  GS.GRADE AND TU.ANO = GS.ANO AND TU.SEMESTRE = GS.SEMESTRE AND TU.SIT_TURMA <> 'Desativada' AND TU.ESPECIAL = 'N' "
                                                                                SqlWhere="(GS.UNIDADE_RESPONSAVEL = #tseUnidadeResponsavel# or #tseUnidadeResponsavel# is null) "
                                                                                OnChanged="tseTurmaTransferencia_Changed" AutoPostBack="true" GridWidth="332px"
                                                                                SqlOrder="GRADE">
                                                                                <GridColumns>
                                                                                    <tweb:TSearchBoxColumn Caption="Código" FieldName="GRADE_ID" Width="50%" Visible="false" />
                                                                                    <tweb:TSearchBoxColumn Caption="Turma" FieldName="grade" Width="30%" />
                                                                                    <tweb:TSearchBoxColumn Caption="Turno" FieldName="turno_descr" Width="10%" />
                                                                                    <tweb:TSearchBoxColumn Caption="Série" FieldName="serie" Width="10%" />
                                                                                </GridColumns>
                                                                            </tweb:TSearchBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="right">
                                                                            <asp:Label Text="Disciplina destino:*" SkinID="lblObrigatorio" runat="server" />
                                                                        </td>
                                                                        <td>
                                                                            <tweb:TSearchBox ID="tseDisciplinaDestino" runat="server" Caption="" Key="disciplina"
                                                                                DataType="VarChar" MaxLength="20" ArgumentColumns="50" Columns="10" Argument="descricao"
                                                                                AutoPostBack="true" SqlSelect=" SELECT distinct tu.disciplina as disciplina, d.nome_compl as descricao
                                                                                    from ly_turma tu 
                                                                                    inner join ly_disciplina d on tu.disciplina = d.disciplina
                                                                                    inner join ly_grupo_habilitacao_disc ghdi on ghdi.disciplina = d.disciplina
                                                                                    inner join ly_grupo_habilitacao_doc ghdoc on ghdoc.agrupamento = ghdi.agrupamento"
                                                                                GridWidth="332px" SqlOrder="descricao" OnChanged="tseDisciplinaDestino_Changed">
                                                                                <GridColumns>
                                                                                    <tweb:TSearchBoxColumn Caption="Disciplina" FieldName="disciplina" Width="30%" />
                                                                                    <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="70%" />
                                                                                </GridColumns>
                                                                            </tweb:TSearchBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="right">
                                                                            <asp:Label Text="Carência:*" SkinID="lblObrigatorio" runat="server" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:RadioButtonList ID="rbtnCarencia" RepeatDirection="Horizontal" runat="server">
                                                                                <asp:ListItem Text="Real (99999999)" Value="99999999" />
                                                                                <asp:ListItem Text="Temporária (00000000)" Value="00000000" />
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <br />
                                                                <dxwgv:ASPxGridView ID="grdTransferencia" ClientInstanceName="grdTransferencia" Width="100%"
                                                                    DataSourceID="odsTransferencia" OnAfterPerformCallback="grdTransferencia_AfterPerformCallback"
                                                                    OnHtmlRowCreated="grdTransferencia_HtmlRowCreated" runat="server">
                                                                    <SettingsPager Mode="ShowAllRecords" AlwaysShowPager="false" />
                                                                    <Settings ShowVerticalScrollBar="true" />
                                                                    <Styles>
                                                                        <Header>
                                                                            <Paddings PaddingLeft="0px" PaddingRight="0px" />
                                                                        </Header>
                                                                    </Styles>
                                                                    <SettingsBehavior AllowSort="false" AllowDragDrop="false" AllowMultiSelection="false"
                                                                        AllowGroup="false" AllowFocusedRow="false" ColumnResizeMode="Disabled" />
                                                                    <Columns>
                                                                        <dxwgv:GridViewDataTextColumn Caption="Dia da Semana" FieldName="DiaSemanaDescr" />
                                                                        <dxwgv:GridViewDataTextColumn Caption="Hora de Entrada" FieldName="HoraIni" />
                                                                        <dxwgv:GridViewDataTextColumn Caption="Hora de Saída" FieldName="HoraFim" />
                                                                        <dxwgv:GridViewDataTextColumn Caption="Observação" FieldName="Observacao" />
                                                                        <dxwgv:GridViewDataCheckColumn Caption="Selecionar" Name="selecionar" Width="70px">
                                                                            <DataItemTemplate>
                                                                                <dxe:ASPxCheckBox ID="cbSelecionarAlocacao" runat="server">
                                                                                    <ClientSideEvents CheckedChanged="function (s,e) { ValidaMarcacaoTransferencia(s,e); }" />
                                                                                </dxe:ASPxCheckBox>
                                                                            </DataItemTemplate>
                                                                        </dxwgv:GridViewDataCheckColumn>
                                                                    </Columns>
                                                                </dxwgv:ASPxGridView>
                                                            </asp:Panel>
                                                            <br />
                                                            <asp:Panel Direction="RightToLeft" runat="server">
                                                                <asp:ImageButton ID="btnRealizarTransferencia" runat="server" SkinID="BcTransferir"
                                                                    Visible="true" OnClientClick="javascript:return VerificaSelecaoTransferencia();"
                                                                    OnClick="btnRealizarTransferencia_Click" />
                                                                <asp:HiddenField ID="hdnAulasDestino" runat="server" />
                                                                <dxe:ASPxButton ID="btnTransferenciaFechar" runat="server" Text="Fechar" Visible="false"
                                                                    OnClick="btnTransferenciaFechar_Click" />
                                                            </asp:Panel>
                                                            <asp:ObjectDataSource ID="odsTransferencia" runat="server" TypeName="Techne.Lyceum.RN.Turma"
                                                                SelectMethod="ObterAlocacoesDisponiveis">
                                                                <SelectParameters>
                                                                    <asp:Parameter Name="turmaDestino" />
                                                                    <asp:Parameter Name="ano" />
                                                                    <asp:Parameter Name="semestre" />
                                                                    <asp:Parameter Name="disciplinaDestino" />
                                                                </SelectParameters>
                                                            </asp:ObjectDataSource>
                                                        </dxpc:PopupControlContentControl>
                                                    </ContentCollection>
                                                </dxpc:ASPxPopupControl>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Name="Matricula" Text="Matrículas">
                <ContentCollection>
                    <dxw:ContentControl ID="ccMatricula" runat="server">
                        <asp:Panel runat="server" ID="pnlMatricula">
                            <dxwgv:ASPxGridView ID="grdMatriculaAluno" ClientInstanceName="grdMatriculaAluno"
                                AutoGenerateColumns="False" Visible="true" KeyFieldName="ALUNO1" runat="server"
                                EnableRowsCache="true" EnableCallBacks="false" Width="90%" OnSelectionChanged="grdMatriculaAluno_SelectionChanged"
                                Font-Names="Verdana" Font-Size="Small" SkinID="NoConfirmDelete" OnCommandButtonInitialize="grdMatriculaAluno_CommandButtonInitialize">
                                <SettingsPager Mode="ShowAllRecords" />
                                <SettingsBehavior ProcessSelectionChangedOnServer="true" AllowSort="false" AllowDragDrop="false"
                                    AllowMultiSelection="false" AllowGroup="false" />
                                <%--<SettingsText EmptyDataRow="Não existem dados." /> Tem q tirar isso para o popup funcionar--%>
                                <Columns>
                                    <dxwgv:GridViewDataTextColumn FieldName="ALUNO1" VisibleIndex="1" Caption="Aluno"
                                        Width="10%">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn FieldName="NOME_COMPL1" VisibleIndex="2" Caption="Nome">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn FieldName="SIT_MATRICULA" VisibleIndex="4" Caption="Situação">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn FieldName="DEPENDENCIA" ReadOnly="true" Visible="false"
                                        VisibleIndex="5">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewCommandColumn VisibleIndex="6" ButtonType="Link" Width="7%" Caption=" ">
                                        <SelectButton Text="Ver dep." Visible="true">
                                        </SelectButton>
                                    </dxwgv:GridViewCommandColumn>
                                </Columns>
                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                            </dxwgv:ASPxGridView>
                            <dxe:ASPxButton ID="btnBuscarMatriculas" Style="visibility: hidden" OnClick="btnBuscarMatriculas_Click"
                                ClientInstanceName="btnBuscarMatriculas" runat="server" ClientSideEvents-Click="function(s,e) { Bloqueio(); } " />
                            <dxwgv:ASPxGridView ID="grdMatriculaAlunoEletiva" ClientInstanceName="grdMatriculaAlunoEletiva"
                                AutoGenerateColumns="False" Visible="true" KeyFieldName="ALUNO1" runat="server"
                                EnableRowsCache="true" EnableCallBacks="false" Width="90%" 
                                Font-Names="Verdana" Font-Size="Small" SkinID="NoConfirmDelete" >
                                <SettingsPager Mode="ShowAllRecords" />
                                <SettingsBehavior ProcessSelectionChangedOnServer="true" AllowSort="false" AllowDragDrop="false"
                                    AllowMultiSelection="false" AllowGroup="false" />
                                <%--<SettingsText EmptyDataRow="Não existem dados." /> Tem q tirar isso para o popup funcionar--%>
                                <Columns>
                                    <dxwgv:GridViewDataTextColumn FieldName="ALUNO1" VisibleIndex="1" Caption="Aluno"
                                        Width="10%">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn FieldName="NOME_COMPL1" VisibleIndex="2" Caption="Nome">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn FieldName="SIT_MATRICULA" VisibleIndex="3" Caption="Situação">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn FieldName="ELETIVA1" VisibleIndex="4" Caption="Eletiva 1">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn FieldName="ELETIVA2" VisibleIndex="5" Caption="Eletiva 2">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn FieldName="ELETIVA3" VisibleIndex="6" Caption="Eletiva 3">
                                    </dxwgv:GridViewDataTextColumn>                                  
                                </Columns>
                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                            </dxwgv:ASPxGridView>
                            <dxpc:ASPxPopupControl ID="pucDependencia" runat="server" CloseAction="CloseButton"
                                Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                                ClientInstanceName="pucDependencia" HeaderText="Dependências do Aluno" AllowDragging="True"
                                Width="250px" EnableAnimation="True" EnableViewState="False">
                                <Border BorderColor="Gainsboro" BorderStyle="Ridge" BorderWidth="4px" />
                                <ContentCollection>
                                    <dxpc:PopupControlContentControl ID="pucccDependencia" runat="server">
                                        <asp:Label ID="lblMensagemPopup" Font-Bold="true" runat="server" />
                                        <br />
                                        <dxwgv:ASPxGridView ID="grdDependencia" runat="server" AutoGenerateColumns="False"
                                            EnableCallBacks="false" DataSourceID="" ClientInstanceName="grdDependencia" KeyFieldName="DISCIPLINA"
                                            Width="450px">
                                            <SettingsPager AlwaysShowPager="false" Mode="ShowAllRecords">
                                            </SettingsPager>
                                            <SettingsBehavior AllowFocusedRow="false" AutoExpandAllGroups="true" AllowGroup="false"
                                                AllowSort="false" AllowDragDrop="false" />
                                            <Columns>
                                                <dxwgv:GridViewDataColumn Caption="Disciplina" FieldName="DISCIPLINA_REFERENCIA"
                                                    VisibleIndex="0">
                                                </dxwgv:GridViewDataColumn>
                                                <dxwgv:GridViewDataColumn Caption="Nome" FieldName="NOME_DISCIPLINA" VisibleIndex="1">
                                                </dxwgv:GridViewDataColumn>
                                                <dxwgv:GridViewDataColumn Caption="Situação" FieldName="SITUACAO" VisibleIndex="2">
                                                </dxwgv:GridViewDataColumn>
                                            </Columns>
                                        </dxwgv:ASPxGridView>
                                    </dxpc:PopupControlContentControl>
                                </ContentCollection>
                            </dxpc:ASPxPopupControl>
                        </asp:Panel>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
        </tabpages>
        <clientsideevents activetabchanged="function(s, e) { VerificarAbaMatricula(s,e); }"
            init="function(s, e) { VerificarAba(s,e); }"></clientsideevents>
    </dxtc:ASPxPageControl>
    <asp:HiddenField ID="hdnOriginalDadosTurma" runat="server" />
    <asp:HiddenField ID="hdnOriginalQuadro" runat="server" />
    <dxe:ASPxButton ID="btnSalvarClient" Style="visibility: hidden" OnClick="btnSalvarClient_Click"
        ClientInstanceName="btnSalvarClient" runat="server" />
          <asp:HiddenField ID="hdnValidaDependTurma" runat="server" />
    <dxpc:ASPxPopupControl ID="pucConfirmarTurma" ClientInstanceName="pucConfirmarTurma"
        runat="server" Modal="true" ShowShadow="false" AllowDragging="false" AllowResize="false"
        ShowCloseButton="false" ShowFooter="false" ShowHeader="false" ShowSizeGrip="False"
        EnableAnimation="false" Width="300px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <border bordercolor="Gainsboro" borderstyle="Solid" borderwidth="2px" />
        <clientsideevents init="function(s,e){ OnInitASPxPopupControlSize(s,e,14000); }" />
        <contentcollection>
            <dxpc:PopupControlContentControl>
                <table>
                    <tr align="center">
                        <td>
                        <asp:Label ID="lblMensagemTurmaDependencia" runat="server"></asp:Label>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            <asp:Button ID="btnSimTurmaDependencia" runat="server" Text="Sim" OnClick="btnSimTurmaDependencia_Click" />
                            <asp:Button ID="btnNaoTurmaDependencia" runat="server" Text="Não" OnClick="btnNaoTurmaDependencia_Click" />
                        </td>
                    </tr>
                </table>
            </dxpc:PopupControlContentControl>
        </contentcollection>
    </dxpc:ASPxPopupControl>
</asp:Content>
