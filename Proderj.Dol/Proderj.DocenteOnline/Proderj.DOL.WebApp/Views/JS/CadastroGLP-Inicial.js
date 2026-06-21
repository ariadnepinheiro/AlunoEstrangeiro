/*****************
CADASTROGLP - INICIAL - INICIO
******************/

function listaRegional(success, error, complete) {
    return $.ajax({
        url: '<%=Url.Action("ListaRegional","CadastroGLP")%>',
        type: 'POST',
        success: success,
        error: error,
        complete: complete
    });
}

function listaDisciplinaPor(success, error, complete) {
    return $.ajax({
        url: '<%=Url.Action("ListaDisciplinaPor","CadastroGLP")%>',
        type: 'POST',
        success: success,
        error: error,
        complete: complete
    });
}

function listaMunicipioRegional(codigoRegional, success, error, complete) {
    return $.ajax({
        url: '<%=Url.Action("ListaMunicipioPor","CadastroGLP")%>',
        data: { id_regional: codigoRegional },
        type: 'POST',
        success: success,
        error: error,
        complete: complete
    });
}

function listaUnidadeEnsinoPor (id_regional, municipio, success, error, complete) {
    return $.ajax({
        url: '<%=Url.Action("ListaUnidadeEnsinoPor","CadastroGLP")%>',
        data: { 
            id_regional: id_regional,
            municipio: municipio,
        },
        type: 'POST',
        success: success,
        error: error,
        complete: complete
    });
}

function alteraDocente(numeroTelefone, success, error, complete) {
    return $.ajax({
        url: '<%=Url.Action("AlteraDocente","CadastroGLP")%>',
        data: { NumeroTelefone: numeroTelefone },
        type: 'POST',
        success: success,
        error: error,
        complete: complete
    });
}

function incluiDisponibilidade (request, success, error, complete) {
    return $.ajax({
        url: '<%=Url.Action("IncluiDisponibilidade","CadastroGLP")%>',
        data: request, //$('#frmInclusaoDisponibilidade').serialize(),
        type: 'POST',
        success: success,
        error: error,
        complete: complete
    });
}

function excluiDisponibilidade(disponibilidadeGlpId, unidadeEns, success, error, complete) {
    return $.ajax({
        url: '<%=Url.Action("ExcluiDisponibilidade","CadastroGLP")%>',
        data: { 
            DISPONIBILIDADEGLPID: disponibilidadeGlpId,
            UNIDADE_ENS: unidadeEns,
        },
        type: 'POST',
        success: success,
        error: error,
        complete: complete
    });
}

function listaDisponibilidade(success, error, complete) {
    return $.ajax({
        url: '<%=Url.Action("ListaDisponibilidade", "CadastroGLP")%>',
        type: 'POST',
        success: success,
        error: error,
        complete: complete
    });
}

function obtemRegionalEMunicipioPor(unidadeEnsino, success, error, complete) {
    return $.ajax({
        url: '<%=Url.Action("ObtemRegionalEMunicipioPor", "CadastroGLP")%>',
        type: 'POST',
        data: { unidadeEnsino: unidadeEnsino },
        success: success,
        error: error,
        complete: complete
    });
}

function carregaCombosIniciais(success) {

    var cmbRegional = $('#cmbRegional');
    var txtRegional = $('#txtRegional');
    var cmbDisciplina = $('#cmbDisciplina');
    var txtDisciplina = $('#txtDisciplina');

    var defRegional = listaRegional(function(oJSON) {
        if (oJSON.Sucesso) {
            cmbRegional.empty();
            var oOption = document.createElement('OPTION');
            oOption.text = '-- Selecione aqui --';
            oOption.value = '';

            cmbRegional
						.append(oOption)
						.prop('disabled', false);

            txtRegional
						.val('')
						.css('background-color', '#FFF')
						.prop('disabled', false);

            $.each(oJSON.Combo, function () {
                var oOption = document.createElement('OPTION');
                oOption.value = this.Codigo;
                oOption.text = this.Descricao;
                cmbRegional.append(oOption);
            });
        }
    }, function (oXHTR) {
        oCombo.options[0].text = 'Ocorreu um erro inesperado... ';
    });

    var defDisciplina = listaDisciplinaPor(function (oJSON) {
        if (oJSON.Sucesso) {
            cmbDisciplina.empty();
            var oOption = document.createElement('OPTION');
            oOption.text = '-- Selecione aqui --';
            oOption.value = '';

            cmbDisciplina
							.append(oOption)
							.prop('disabled', false)

            txtDisciplina
							.val('')
							.css('background-color', '#FFF')
							.prop('disabled', false);

            $.each(oJSON.Combo, function () {
                var oOption = document.createElement('OPTION');
                oOption.value = this.Agrupamento;
                oOption.text = this.Descricao;
                cmbDisciplina.append(oOption);
            });
        }
    }, function (oXHTR) {
        oCombo.options[0].text = 'Ocorreu um erro inesperado... ';
    });

    $.when(defRegional, defDisciplina).done(success);
}

function carregaEventos() {

    $('#cmbRegional').on('change', function () {

        var cmbMunicipio = $('#cmbMunicipio');
        var txtMunicipio = $("#txtMunicipio");
        var cmbUE = $("#cmbUE");
        var txtUE = $("#txtUE");

        cmbMunicipio
			.empty()
			.append('<option value="">-- Selecione uma regional -- </option>')
            .trigger("change");

        txtMunicipio
			.val('');

        if (this.selectedIndex > 0) {

            var oCombo = cmbMunicipio.get(0);
            oCombo.options[0].text = 'Carregando...';

            listaMunicipioRegional(this.value, function (oJSON) {
                if (oJSON.Sucesso) {
                    cmbMunicipio.empty();
                    var oOption = document.createElement('OPTION');
                    oOption.text = '-- Selecione aqui --';
                    oOption.value = '';

                    cmbMunicipio
							.append(oOption);

                    txtMunicipio
							.val('');

                    $.each(oJSON.Combo, function () {
                        var oOption = document.createElement('OPTION');
                        oOption.value = this.Codigo;
                        oOption.text = this.Descricao;
                        cmbMunicipio.append(oOption);
                    });
                }
            }, function (oXHTR) {
                oCombo.options[0].text = 'Ocorreu um erro inesperado... ';
            });
        }

        $("#txtRegional").val(this.value);
    });

    $('#txtRegional').on("change", function () {
        var id = this.value;
        if ($("#cmbRegional").find("[value='" + this.value + "']").length) {
            $("#cmbRegional").val(this.value);
            $("#cmbRegional").trigger("change");
        } else {
            this.value = $("#cmbRegional").val();
        }
    });

    $('#cmbMunicipio').on('change', function () {

        var cmbUE = $('#cmbUE');
        var txtUE = $('#txtUE');

        cmbUE
			.empty()
			.append('<option value="">-- Selecione um município -- </option>')
            .trigger("change");

        txtUE
			.val('');

        if (this.selectedIndex > 0) {

            var oCombo = cmbUE.get(0);
            oCombo.options[0].text = 'Carregando...';

            listaUnidadeEnsinoPor($("#cmbRegional").val(), this.value, function (oJSON) {
                if (oJSON.Sucesso) {

                    cmbUE.empty();

                    var oOption = document.createElement('OPTION');
                    oOption.text = '-- Selecione aqui --';
                    oOption.value = '';
                    cmbUE.append(oOption);

                    var oOptionTodas = document.createElement('OPTION');
                    oOptionTodas.text = '-- TODAS AS UNIDADES --';
                    oOptionTodas.value = '1';
                    cmbUE.append(oOptionTodas);

                    txtUE.val('');

                    $.each(oJSON.Combo, function (i, el) {
                        var oOption = document.createElement('OPTION');
                        oOption.value = el.UNIDADE_ENS;
                        oOption.text = el.NOME_COMP;
                        cmbUE.append(oOption);
                    });
                }
            }, function (oXHTR) {
                oCombo.options[0].text = 'Ocorreu um erro inesperado... ';
            });
        }

        $("#txtMunicipio").val(this.value);
    });

    $('#txtMunicipio').on("change", function () {
        var id = this.value;
        if ($("#cmbMunicipio").find("[value='" + this.value + "']").length) {
            $("#cmbMunicipio").val(this.value);
            $("#cmbMunicipio").trigger("change");
        } else {
            this.value = $("#cmbMunicipio").val();
        }
    });

    $("#cmbUE").on("change", function () {
        $("#txtUE").val(this.value);
    });

    $('#txtUE').on("change", function () {
        var id = this.value;
        if ($("#cmbUE").find("[value='" + this.value + "']").length) {
            $("#cmbUE").val(this.value);
            $("#cmbUE").trigger("change");
        } else {
            obtemRegionalEMunicipioPor(this.value, function (data) {
                if (data) {
                    $("#txtRegional,#cmbRegional").val(data.ID_REGIONAL);
                    listaMunicipioRegional(data.ID_REGIONAL, function(oJSON) {
                        if (oJSON.Sucesso) {
                            $("#cmbMunicipio").empty();
                            var oOption = document.createElement('OPTION');
                            oOption.text = '-- Selecione aqui --';
                            oOption.value = '';

                            $("#cmbMunicipio").append(oOption);

                            $("#txtMunicipio").val('');

                            $.each(oJSON.Combo, function () {
                                var oOption = document.createElement('OPTION');
                                oOption.value = this.Codigo;
                                oOption.text = this.Descricao;
                                $("#cmbMunicipio").append(oOption);
                            });

                            $("#txtMunicipio,#cmbMunicipio").val(data.MUNICIPIO);

                            listaUnidadeEnsinoPor(data.ID_REGIONAL, data.MUNICIPIO, function(oJSON){
                                if (oJSON.Sucesso) {
                                    $("#cmbUE").empty();

                                    var oOption = document.createElement('OPTION');
                                    oOption.text = '-- Selecione aqui --';
                                    oOption.value = '';
                                    $("#cmbUE").append(oOption);

                                    var oOptionTodas = document.createElement('OPTION');
                                    oOptionTodas.text = '-- TODAS AS UNIDADES --';
                                    oOptionTodas.value = '1';
                                    $("#cmbUE").append(oOptionTodas);

                                    $.each(oJSON.Combo, function (i, el) {
                                        var oOption = document.createElement('OPTION');
                                        oOption.value = el.UNIDADE_ENS;
                                        oOption.text = el.NOME_COMP;
                                        $("#cmbUE").append(oOption);
                                    });

                                    $("#cmbUE").val(id);
                                }
                            }, function(oXHTR){
                                oCombo.options[0].text = 'Ocorreu um erro inesperado... ';
                            });
                        }
                    }, function(oXHTR){
                        oCombo.options[0].text = 'Ocorreu um erro inesperado... ';
                    });
                } else {
                    $('#txtUE').val($("#cmbUE").val());
                }
            }, function (err) {
                oCombo.options[0].text = 'Ocorreu um erro inesperado... ';
            });
        }
    });

    $("#cmbDisciplina").on("change", function() {
        $("#txtDisciplina").val(this.value);
    });

    $('#txtDisciplina').on("change", function () {
        var id = this.value;
        if ($("#cmbDisciplina").find("[value='" + this.value + "']").length) {
            $("#cmbDisciplina").val(this.value);
            $("#cmbDisciplina").trigger("change");
        } else {
            this.value = $("#cmbDisciplina").val();
        }
    });

    $("#modalidade-medio,#modalidade-fundamental-finais,#modalidade-fundamental-iniciais").on("change", function () {
        if ($(this).data("modalidadeOpcao") == "1")
            $("input:checkbox[data-modalidade-opcao='2']").prop("checked", false);

        if ($(this).data("modalidadeOpcao") == "2")
            $("input:checkbox[data-modalidade-opcao='1']").prop("checked", false);
    });

    $('#btIncluirDisponibilidade').on('click', function () {
        var oBotao = this;

        oBotao.disabled = true;
        oBotao.value = 'Incluindo...';

        incluiDisponibilidade($('#frmInclusaoDisponibilidade').serialize(), function (oJSON) {
            if (oJSON.Sucesso) {
                mostraSucesso();
                $("#txtRegional,#cmbRegional").val("");
                $("#cmbRegional, #cmbMunicipio").trigger("change");
                $("#txtDisciplina,#cmbDisciplina").val("");
                $("#modalidade-medio,#modalidade-fundamental-finais,#modalidade-fundamental-iniciais,#dia-segunda,#dia-terca,#dia-quarta,#dia-quinta,#dia-sexta,#dia-sabado,#turno-manha,#turno-tarde,#turno-noite").prop("checked", false);
                carregaListagem();
            }
            else {
                var msgErro = "";
                if (oJSON.ListaMensagem) {
                    $.each(oJSON.ListaMensagem, function () {
			            msgErro += '<br />- ' + this;
		            });
                }
                else if (oJSON.Mensagem) {
                    msgErro = oJSON.Mensagem;
                }
                else {
                    msgErro = "Erro não especificado";
                }
                mostraErro(msgErro);
            }
        }, function (oXHTR) {
            alert('Ocorreu um erro inesperado ao incluir disponibilidade');
        }, function () {
            oBotao.disabled = false;
            oBotao.value = 'Incluir';
        });
    });

    $('form input.hora')
		.setMask('time')
		.on('drag dragstart dragend', function () { return false; });

    $('#tabela-disponibilidade').on('click', 'input:image', function () {

        var disponibilidadeGlpId = $(this).data('disponibilidadeGlpId');
        var unidadeEns = $(this).data('unidadeEns');

        if (confirm("Deseja realmente excluir esta disponibilidade?")) {
            var jqBotao = $(this);
            jqBotao.fadeTo(0, 0.5);

            excluiDisponibilidade(disponibilidadeGlpId, unidadeEns, function (oJSON) {
                if (oJSON.Sucesso) {
                    mostraSucessoGrid(oJSON.Mensagem);
                    carregaListagem();
                }
                else {
                    ExibeErroNegocio(oJSON);
                }
            }, function (oXHTR) {
                mostraErroGrid('Ocorreu um erro inesperado ao excluir disponibilidade');
            }, function () {
                jqBotao.fadeTo(0, 1);
            });
        }
    });
}

function carregaListagem() {
    $('#tabela-disponibilidade').fadeTo(0, 0.5);

    listaDisponibilidade(function (sHTML) {
        $('#tabela-disponibilidade').html(sHTML);
    }, function () {
        alert('Erro inesperado ao listar disponibilidade');
    }, function () {
        $('#tabela-disponibilidade').fadeTo(0, 1);
    });
}

function mostraErro(erro) {
    $("#msg-erro-grid").css("color", "#f00", "!important").html("").hide();

    if (erro)
        $("#msg-erro").css("color", "#f00", "!important").html(erro).show();
    else
        $("#msg-erro").css("color", "#f00", "!important").html("").hide();
}

function mostraErroGrid(erro) {
    $("#msg-erro").css("color", "#f00", "!important").html("").hide();

    if (erro)
        $("#msg-erro-grid").css("color", "#f00", "!important").html(erro).show();
    else
        $("#msg-erro-grid").css("color", "#f00", "!important").html("").hide();
}

function mostraSucesso() {
    $("#msg-erro-grid").css("color", "#0a0", "!important").html("").hide();

    var sucesso = "Prezado(a) Professor(a),<br />Agradecemos o seu cadastro e informamos que o Coordenador de Gestão de Pessoas da Regional Cadastrada entrará em contato, em caso de surgimento de vagas considerando a sua disponibilidade.";
    $("#msg-erro").css("color", "#0a0", "important").html(sucesso).show();
}

function mostraSucessoGrid(msg) {
    $("#msg-erro").css("color", "#0a0", "!important").html("").hide();
    
    if (msg)
        $("#msg-erro-grid").css("color", "#0a0", "!important").html(msg).show();
    else
        $("#msg-erro-grid").css("color", "#0a0", "!important").html("").hide();
}

$(document).ready(function () {
    carregaCombosIniciais(function () {
        carregaEventos();
        carregaListagem();
    });
});

/*****************
CADASTROGLP - INICIAL - FIM
******************/
