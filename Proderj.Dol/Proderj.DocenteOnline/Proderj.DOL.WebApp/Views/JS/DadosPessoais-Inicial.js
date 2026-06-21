/*****************
DADOSPESSOAIS - INICIAL - INICIO
******************/

function listaPais(success, error) {
    return $.ajax({
        url: 'DadosPessoais/ListaPais',
        type: 'POST',
        success: success,
        error: error
    });
}

function listaUF(success, error) {
    return $.ajax({
        url: 'DadosPessoais/ListaUF',
        type: 'POST',
        success: success,
        error: error
    });
}

function obtemLogradouroPor(cep, success, error) {
    return $.ajax({
        url: 'DadosPessoais/ObtemLogradouroPor',
        type: 'POST',
        data: { cep: cep },
        success: success,
        error: error
    });
}

function dicionarioMunicipioPor(uf, success, error) {
    return $.ajax({
        url: 'DadosPessoais/DicionarioMunicipioPor',
        type: 'POST',
        data: { uf: uf },
        success: success,
        error: error
    });
}

function obtemUFPor(municipio, success, error) {
    return $.ajax({
        url: 'DadosPessoais/ObtemUFPor',
        type: 'POST',
        data: { municipio: municipio },
        success: success,
        error: error
    });
}

function obtemPessoaPorMatricula(matricula, success, error) {
    return $.ajax({
        url: 'DadosPessoais/ObtemPessoaPorMatricula',
        type: 'POST',
        data: { matricula: matricula },
        success: success,
        error: error
    });
}

function atualiza(dto, success, error) {
    return $.ajax({
        url: 'DadosPessoais/Atualiza',
        type: 'POST',
        data: dto,
        success: success,
        error: error
    });
}

function preencheCampos(data) {
    //console.log(data);

    $("#pessoa").val(data.PESSOA);

    $("#pais").val(data.END_PAIS);
    $("#cep").val(data.CEP);
    $("#endereco").val(data.ENDERECO);
    $("#numero").val(data.END_NUM);
    $("#complemento").val(data.END_COMPL);
    $("#bairro").val(data.BAIRRO);
    $("#uf").val(data.END_UF);
    $("#uf").trigger("change");
    $("#municipio-id").val(data.END_MUNICIPIO);
    $("#municipio-id").trigger("change");

    $("#localizacao-zona-rural").prop("checked", data.LY_FL_PESSOA ? data.LY_FL_PESSOA.FL_FIELD_01 == "Rural" : false);
    $("#localizacao-zona-urbana").prop("checked", data.LY_FL_PESSOA ? data.LY_FL_PESSOA.FL_FIELD_01 == "Urbana" : false);

    $("#localizacao-diferenciada-na").prop("checked", data.AREA_ASSENTAMENTO != "S" && data.AREA_QUILOMBOS != "S" && data.TERRA_INDIGENA != "S");
    $("#localizacao-diferenciada-assentamento").prop("checked", data.AREA_ASSENTAMENTO == "S");
    $("#localizacao-diferenciada-quilombos").prop("checked", data.AREA_QUILOMBOS == "S");
    $("#localizacao-diferenciada-indigena").prop("checked", data.TERRA_INDIGENA == "S");

    $("#telefone").val(data.FONE);
    $("#celular").val(data.CELULAR);
    $("#email-institucional").val(data.E_MAIL_INTERNO);
    $("#email-educa").val(data.E_MAIL_EDUCA);
    $("#email").val(data.E_MAIL);

    if (data.REL_CH_SERV_ANO1)
    {
        var docente = data.REL_CH_SERV_ANO1;

        $("#matricula").val(docente.MATRICULA);

        $("#docente-nome1").text(docente.NOME_COMPL);
        $("#docente-nomesocial1").text(docente.PRE_NOME_SOCIAL);
        $("#docente-cpf1").text(docente.CPF);
        $("#docente-matricula1").text(docente.MATRICULA);
		$("#docente-id1").text(docente.IDFUNCIONAL);
		$("#docente-vinculo1").text(docente.VINCULO);
		$("#docente-idvinculo1").text(docente.IDVINCULO);
        $("#docente-situacao1").text(docente.SITUACAO ? docente.SITUACAO : "");
        $("#docente-funcao1").text(docente.FUNCAO);
        $("#docente-disciplina-ingresso1").text(docente.DIS_INGRESS);
        $("#docente-cargo1").text(docente.CARGO);
        $("#docente-ch-regencia1").text(docente.CH_REGENCIA);
        $("#docente-hor-tur1").text(docente.HOR_TUR);
        $("#docente-tol-normal1").text(docente.TOL_NORMAL);
        $("#docente-tol-glp1").text(docente.TOL_GLP);
        $("#docente-regional1").text(docente.REGIONAL);
        $("#docente-municipio1").text(docente.MUNICIPIO);
        $("#docente-ua-lotacao1").text(docente.UA_DE_LOTACAO);
        $("#docente-unidade-adm1").text(docente.UNIDADE_ADMINISTRATIVA);
    }
                
    if (data.REL_CH_SERV_ANO2)
    {
        var docente = data.REL_CH_SERV_ANO2;

        $("#docente-nome2").text(docente.NOME_COMPL);
        $("#docente-nomesocial2").text(docente.PRE_NOME_SOCIAL);
        $("#docente-cpf2").text(docente.CPF);
        $("#docente-matricula2").text(docente.MATRICULA);
		$("#docente-id2").text(docente.IDFUNCIONAL);
		$("#docente-vinculo2").text(docente.VINCULO);
		$("#docente-idvinculo2").text(docente.IDVINCULO);
        $("#docente-situacao2").text(docente.SITUACAO ? docente.SITUACAO : "");
        $("#docente-funcao2").text(docente.FUNCAO);
        $("#docente-disciplina-ingresso2").text(docente.DIS_INGRESS);
        $("#docente-cargo2").text(docente.CARGO);
        $("#docente-ch-regencia2").text(docente.CH_REGENCIA);
        $("#docente-hor-tur2").text(docente.HOR_TUR);
        $("#docente-tol-normal2").text(docente.TOL_NORMAL);
        $("#docente-tol-glp2").text(docente.TOL_GLP);
        $("#docente-regional2").text(docente.REGIONAL);
        $("#docente-municipio2").text(docente.MUNICIPIO);
        $("#docente-ua-lotacao2").text(docente.UA_DE_LOTACAO);
        $("#docente-unidade-adm2").text(docente.UNIDADE_ADMINISTRATIVA);
    }
}

function loadPage(matricula) {

    var lp = listaPais(function (data) {
        $(data).each(function (i, el) {
            $("#pais").append(new Option((el.Nome == "NÃO INFORMADO" ? "" : el.Nome), el.Pais));
        });
    });

    var luf = listaUF(function (data) {
        $("#uf").append(new Option("", ""));
        $(data).each(function (i, el) {
            $("#uf").append(new Option(el, el));
        });
    });

    $.when(lp, luf).then(function() {

        $('#cep').inputmask({ mask: "99999-999", autoUnmask : true });
        $('#telefone').inputmask("(99) 9999-9999");
        $('#celular').inputmask("(99) 99999-9999");

        Inputmask.extendAliases({
          'email-caseinsensitive': {
            mask: "*{1,64}[.*{1,64}][.*{1,64}][.*{1,63}]@-{1,63}.-{1,63}.-{1,63}.-{1,63}.-{1,63}", 
            greedy: !1, 
            onBeforePaste: function (e, t) { 
                return e = e.replace(/mailto:/ig, "") 
            }, 
            definitions: { 
                "*": { 
                    validator: "[0-9A-Za-z!#$%&'*+/=?^_`{|}~-]", 
                    cardinality: 1
                }, 
                "-": { 
                    validator: "[0-9A-Za-z-]", 
                    cardinality: 1
                } 
            }, 
            onUnMask: function (e, t, i) { 
                return e 
            }, 
            inputmode: "email"
          }
        });

        $('#email-institucional').inputmask({ alias: "email-caseinsensitive" });
        $('#email-educa').inputmask({ alias: "email-caseinsensitive" });
        $('#email').inputmask({ alias: "email-caseinsensitive" });

        $("#uf").on("change", function () {

            $("#municipio-id").val("");
            $("#municipio").empty();

            dicionarioMunicipioPor($("#uf").val(), function (dataMunicipio) {
                if (!dataMunicipio)
                    return;

                $("#municipio").append(new Option("", ""));
                $(dataMunicipio).each(function (i, el) {
                    $("#municipio").append(new Option(el.NOME, el.ID_MUNICIPIO));
                });
            });

        });

        $("#municipio").on("change", function () {
            $("#municipio-id").val(this.value);
        });

        $("#municipio-id").on("change", function () {
            var id = this.value;
            if ($("#municipio").find("[value*='" + this.value + "']").length) {
                $("#municipio").val(this.value);
            } else {
                obtemUFPor($("#municipio-id").val(), function (data) {
                    if (data) {
                        $("#uf").val(data);

                        dicionarioMunicipioPor(data, function (dataMunicipio) {
                            if (!dataMunicipio)
                                return;
                            $("#municipio").append(new Option("", ""));
                            $(dataMunicipio).each(function (i, el) {
                                $("#municipio").append(new Option(el.NOME, el.ID_MUNICIPIO));
                            });
                            $("#municipio").val(id);
                            $("#municipio-id").val(id);
                        });
                    }
                });
            }
        });

        $("#cep").on("blur", function () {

            $("#endereco").val("");
            $("#uf").val("");
            $("#municipio-id").val("");
            $("#municipio").empty();
            $("#numero").val("");
            $("#complemento").val("");
            $("#bairro").val("");

            if ($("#cep").val().trim() == "")
                return;

            obtemLogradouroPor(this.value, function (data) {
                if (!data)
                    return;

                $("#endereco").val(data.NOME.toUpperCase());
                $("#municipio-id").val(data.ID_MUNICIPIO);
                $("#uf").val(data.Municipio.UF);

                dicionarioMunicipioPor(data.Municipio.UF, function (dataMunicipio) {
                    if (!dataMunicipio)
                        return;

                    $("#municipio").append(new Option("", ""));
                    $(dataMunicipio).each(function (i, el) {
                        $("#municipio").append(new Option(el.NOME, el.ID_MUNICIPIO));
                    });
                    $("#municipio").val(data.ID_MUNICIPIO);
                });
            });
        });

        obtemPessoaPorMatricula(matricula, function (data) {
         
            preencheCampos(data);
        });

        $("#localizacao-diferenciada-na").on("change", function() {
            if ($(this).prop("checked")) {
                $("#localizacao-diferenciada-assentamento").prop("checked", false);
                $("#localizacao-diferenciada-quilombos").prop("checked", false);
                $("#localizacao-diferenciada-indigena").prop("checked", false);
            }
        });
    
        $("#localizacao-diferenciada-assentamento,#localizacao-diferenciada-quilombos,#localizacao-diferenciada-indigena").on("change", function() {
            if ($(this).prop("checked")) {
                $("#localizacao-diferenciada-na").prop("checked", false);
            }
        });

        $("#endereco,#numero,#complemento,#bairro").bind('input', function (e) {
            if (e.which >= 97 && e.which <= 122) {
                var newKey = e.which - 32;
                e.keyCode = newKey;
                e.charCode = newKey;
            }
            $(this).val(($(this).val()).toUpperCase());
            $("#msg-erro-dados-pessoais").html("").hide();
        });

        $("#btAlterarDocente").on("click", function() {
        
            $("#msg-erro-dados-pessoais").html("").hide();

            $("input[type='text']").val(function() {
                return $(this).val().trim();
            });

            var erros = [];

            if ($("#pais").val() == "0000000000")
                erros.push("PAÍS não preenchido");

            if ($("#cep").val() == "")
                erros.push("CEP não preenchido");

            if ($("#endereco").val() == "")
                erros.push("ENDEREÇO não preenchido");

            if ($("#numero").val() == "")
                erros.push("NÚMERO não preenchido");

    //        if ($("#complemento").val() == "")
    //            erros.push("COMPLEMENTO não preenchido");

            if ($("#bairro").val() == "")
                erros.push("BAIRRO não preenchido");

            if (!$("#municipio").val())
                erros.push("MUNICÍPIO não selecionado");

            if ($("#uf").val() == "")
                erros.push("UF não selecionado");

            if (
                !$("#localizacao-zona-rural").prop("checked") &&
                !$("#localizacao-zona-urbana").prop("checked")
            )
                erros.push("LOCALIZAÇÃO/ZONA DE RESIDÊNCIA não marcada");

            if (
                !$("#localizacao-diferenciada-na").prop("checked") &&
                !$("#localizacao-diferenciada-assentamento").prop("checked") &&
                !$("#localizacao-diferenciada-quilombos").prop("checked") &&
                !$("#localizacao-diferenciada-indigena").prop("checked")
            )
                erros.push("LOCALIZAÇÃO DIFERENCIADA não marcada");

            $(erros).each(function(i, el) {
                el = "<p>" + el + "</p>";
                erros[i] = el;
            });
            erros = erros.join("");

            if (erros != "") {
                $("#msg-erro-dados-pessoais").html(erros).show();
                return;
            }
        
            var dto = {
                PESSOA: $("#pessoa").val(),
                END_PAIS: $("#pais").val(),
                CEP: $("#cep").val(),
                ENDERECO: $("#endereco").val(),
                END_NUM: $("#numero").val(),
                END_COMPL: $("#complemento").val(),
                BAIRRO: $("#bairro").val(),
                END_MUNICIPIO: $("#municipio-id").val(),
                AREA_QUILOMBOS: $("#localizacao-diferenciada-quilombos").prop("checked") ? "S" : "N",
                TERRA_INDIGENA: $("#localizacao-diferenciada-indigena").prop("checked") ? "S" : "N",
                AREA_ASSENTAMENTO: $("#localizacao-diferenciada-assentamento").prop("checked") ? "S" : "N",
                FONE: $("#telefone").val(),
                CELULAR: $("#celular").val(),
                E_MAIL_INTERNO: $("#email-institucional").val(),
                E_MAIL_EDUCA: $("#email-educa").val(),
                E_MAIL: $("#email").val(),
                FL_FIELD_01: $("#localizacao-zona-rural").prop("checked") ? "Rural" : ($("#localizacao-zona-urbana").prop("checked") ? "Urbana" : null),
            };
           
            atualiza(dto, function(data) {
                preencheCampos(data);
                $("#msg-erro-dados-pessoais").html("<p style='color: #0a0 !important;'>DADOS SALVOS COM SUCESSO</p>").show();
            }, function(err) {
                console.log("deu erro");
            });
        });

    });
}

/*****************
DADOSPESSOAIS - INICIAL - FIM
******************/