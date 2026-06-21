var __aspxAgent = navigator.userAgent.toLowerCase();
var __aspxOpera = (__aspxAgent.indexOf("opera") > -1);
var __aspxSafari = __aspxAgent.indexOf("safari") > -1;
var __aspxIE = (__aspxAgent.indexOf("msie") > -1 && !__aspxOpera);
var __aspxNotIEOperaSafari = !__aspxSafari && !__aspxIE && !__aspxOpera;
var __aspxFirefox = (__aspxAgent.indexOf("firefox") > -1) && __aspxNotIEOperaSafari;
var __aspxMozilla = (__aspxAgent.indexOf("mozilla") > -1) && __aspxNotIEOperaSafari;
var __aspxNetscape = (__aspxAgent.indexOf("netscape") > -1) && __aspxNotIEOperaSafari;
var __aspxNS = __aspxFirefox  || __aspxMozilla || __aspxNetscape; 


var itensPermitidos = new RegExp("[0-9]");
var itensPermitidosNumeros = new RegExp("[0-9]");
var itensPermitidosLetras = new RegExp("[A-Za-z]");
var itensPermitidosCodigo = new RegExp("[A-Za-z0-9]");
var itensPermitidosCodigoEspaco = new RegExp("[A-Za-z0-9_ ]");
var itensPermitidosDescricao = new RegExp("[A-Za-zÁáÀàÂâÃãÉéÈèÊêÍíÌìÎîÓóÒòÔôÕõÚúÙùÛûÇçÑñ0-9_ ]");


function ItemPermitido(symbol) { return itensPermitidos.test(symbol); }
function ItemPermitidoNumeros(symbol) { return itensPermitidosNumeros.test(symbol); }
function ItemPermitidoLetras(symbol) { return itensPermitidosLetras.test(symbol); }
function ItemPermitidoCodigo(symbol) { return itensPermitidosCodigo.test(symbol); }
function ItemPermitidoCodigoEspaco(symbol) { return itensPermitidosCodigoEspaco.test(symbol); }
function ItemPermitidoDescricao(symbol) { return itensPermitidosDescricao.test(symbol); }


function SomentePermitirNumeros(campo, evt) 
{
    var vr = campo.inputElement.value;
    var tam = vr.length;
    
    if(tam >= 10)
    {
        _aspxPreventEvent(evt)
    }
    else
    {
     var codigo = ResgatarChave(evt);
     var itemPressionado = String.fromCharCode(codigo);
     if (!ItemPermitidoNumeros(itemPressionado))
        _aspxPreventEvent(evt);
    
    }
}


function SomentePermitirCodigo(campo, evt) 
{
    var vr = campo.inputElement.value;
     var codigo = ResgatarChave(evt);
     var itemPressionado = String.fromCharCode(codigo);
     if (!ItemPermitidoCodigo(itemPressionado))
        _aspxPreventEvent(evt);
}

function SomentePermitirCodigoEspaco(campo, evt) 
{
    var vr = campo.inputElement.value;
     var codigo = ResgatarChave(evt);
     var itemPressionado = String.fromCharCode(codigo);
     if (!ItemPermitidoCodigoEspaco(itemPressionado))
        _aspxPreventEvent(evt);
}

function SomentePermitirDescricao(campo, evt) 
{
    var vr = campo.inputElement.value;
     var codigo = ResgatarChave(evt);
     var itemPressionado = String.fromCharCode(codigo);
     if (!ItemPermitidoDescricao(itemPressionado))
        _aspxPreventEvent(evt);
}







function ResgatarChave(srcEvt) { return __aspxNS ? srcEvt.which : srcEvt.keyCode; }
function _aspxPreventEvent(evt)
{
   if (__aspxNS)
      evt.preventDefault();
   else
      evt.returnValue = false;
 return false;
}            


// Formata o campo valor monetário
function formataValor(campo, evt) {
    //1.000.000,00
    evt = getEvent(evt);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla))
        return;
    vr = campo.value = filtraNumeros(filtraCampo(campo));
    tam = vr.length;
    if (tam <= 2) {
        campo.value = vr;
    }
    if ((tam > 2) && (tam <= 5)) {
        campo.value = vr.substr(0, tam - 2) + ',' + vr.substr(tam - 2, tam);
    }
    if ((tam >= 6) && (tam <= 8)) {
        campo.value = vr.substr(0, tam - 5) + '.' + vr.substr(tam - 5, 3) + ',' + vr.substr(tam - 2, tam);
    }
    if ((tam >= 9) && (tam <= 11)) {
        campo.value = vr.substr(0, tam - 8) + '.' + vr.substr(tam - 8, 3) + '.' + vr.substr(tam - 5, 3) + ',' + vr.substr(tam - 2, tam);
    }
    if ((tam >= 12) && (tam <= 14)) {
        campo.value = vr.substr(0, tam - 11) + '.' + vr.substr(tam - 11, 3) + '.' + vr.substr(tam - 8, 3) + '.' + vr.substr(tam - 5, 3) + ',' + vr.substr(tam - 2, tam);
    }
    if ((tam >= 15) && (tam <= 18)) {
        campo.value = vr.substr(0, tam - 14) + '.' + vr.substr(tam - 14, 3) + '.' + vr.substr(tam - 11, 3) + '.' + vr.substr(tam - 8, 3) + '.' + vr.substr(tam - 5, 3) + ',' + vr.substr(tam - 2, tam);
    }
}
// Formata data no padrão DD/MM/YYYY
function formataData(campo, evt) {
    //dd/MM/yyyy
    evt = getEvent(evt);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla))
        return;
    vr = campo.value = filtraNumeros(filtraCampo(campo));
    tam = vr.length;
    if (tam >= 2 && tam < 4)
        campo.value = vr.substr(0, 2) + '/' + vr.substr(2);
    if (tam == 4)
        campo.value = vr.substr(0, 2) + '/' + vr.substr(2, 2) + '/';
    if (tam > 4)
        campo.value = vr.substr(0, 2) + '/' + vr.substr(2, 2) + '/' + vr.substr(4);
    //if (tam >= 5 && tam <= 10)
    // campo.value = vr.substr(0, 2) + '/' + vr.substr(2, 2) + '/' + vr.substr(4, 4);
}
// Formata só números
function formataInteiro(campo, evt) {
    //1234567890
    evt = getEvent(evt);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla))
        return false;
    campo.value = filtraNumeros(filtraCampo(campo));
    return;
}
// Formata hora no padrao HH:MM
function formataHora(campo, evt) {
    //HH:mm
    evt = getEvent(evt);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla))
        return;
    vr = campo.value = filtraNumeros(filtraCampo(campo));
    if (tam == 2)
        campo.value = vr.substr(0, 2) + ':';
    if (tam > 2 && tam < 5)
        campo.value = vr.substr(0, 2) + ':' + vr.substr(2);
}
// Formata o campo quando é digitado somente o mês e o ano
function formataMesAno(campo, evt) {
    //MM/yyyy
    evt = getEvent(evt);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla))
        return;
    vr = campo.value = filtraNumeros(filtraCampo(campo));
    tam = vr.length;
    if (tam > 2 && tam < 5)
        campo.value = vr.substr(0, tam - 2) + '/' + vr.substr(tam - 2, tam);
    if (tam >= 5 && tam <= 10)
        campo.value = vr.substr(0, 2) + '/' + vr.substr(2, 4);
}
// Formata o campo CNPJ
function formataCNPJ(campo, evt) {
    //99.999.999/9999-99
    evt = getEvent(evt);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla))
        return;

    vr = campo.value = filtraNumeros(filtraCampo(campo));
    if (vr.length > 14)
        vr = vr.substr(0, 14);

    tam = vr.length;

    if (tam >= 2 && tam < 5)
        campo.value = vr.substr(0, 2) + '.' + vr.substr(2);
    else if (tam >= 5 && tam < 8)
        campo.value = vr.substr(0, 2) + '.' + vr.substr(2, 3) + '.' + vr.substr(5);
    else if (tam >= 8 && tam < 12)
        campo.value = vr.substr(0, 2) + '.' + vr.substr(2, 3) + '.' + vr.substr(5, 3) + '/' + vr.substr(8);
    else if (tam >= 12)
        campo.value = vr.substr(0, 2) + '.' + vr.substr(2, 3) + '.' + vr.substr(5, 3) + '/' + vr.substr(8, 4) + '-' + vr.substr(12, 2);
}
// Formata o campo CPF
function formataCPF(campo, evt) {
    //999.999.999-99
    evt = getEvent(evt);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla))
        return;
    vr = campo.value = filtraNumeros(filtraCampo(campo));
    if (vr.length > 11)
        vr = vr.substr(0, 11);
    tam = vr.length;
    if (tam <= 2) {
        campo.value = vr;
    }
    if (tam > 2 && tam <= 5) {
        campo.value = vr.substr(0, tam - 2) + '-' + vr.substr(tam - 2, tam);
    }
    if (tam >= 6 && tam <= 8) {
        campo.value = vr.substr(0, tam - 5) + '.' + vr.substr(tam - 5, 3) + '-' + vr.substr(tam - 2, tam);
    }
    if (tam >= 9 && tam <= 11) {
        campo.value = vr.substr(0, tam - 8) + '.' + vr.substr(tam - 8, 3) + '.' + vr.substr(tam - 5, 3) + '-' + vr.substr(tam - 2, tam);
    }
}

// Formata o campo RG
function formataRG(campo, evt) {
    //99.999.999-9
    evt = getEvent(evt);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla))
        return;
    vr = campo.value = filtraNumeros(filtraCampo(campo));
    if (vr.length > 9)
        vr = vr.substr(0, 9);
    tam = vr.length;
    if (tam <= 1) {
        campo.value = vr;
    }
    if (tam > 1 && tam <= 5) {
        campo.value = vr.substr(0, tam - 1) + '-' + vr.substr(tam - 1, tam);
    }
    if (tam >= 6 && tam <= 7) {
        campo.value = vr.substr(0, tam - 5) + '.' + vr.substr(tam - 5, 3) + '-' + vr.substr(tam - 2, tam);
    }
    if (tam >= 8 && tam <= 11) {
        campo.value = vr.substr(0, tam - 7) + '.' + vr.substr(tam - 7, 3) + '.' + vr.substr(tam - 4, 3) + '-' + vr.substr(tam - 1, tam);
    }
}

// Formata campo flutuante, permite números e somente uma vírgula
function formataDouble(campo, evt) {
    //18,53012
    evt = getEvent(evt);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla))
        return;
    campo.value = filtraNumerosComVirgula(campo.value);
}
// Formata campo telefone
function formataTelefone(campo, evt) {
    //0000-0000
    evt = getEvent(evt);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla))
        return;
    vr = campo.value = filtraNumeros(filtraCampo(campo));
    tam = vr.length;
    if (tam <= 4)
    {
        campo.value = vr;
        return;
    }
    campo.value = vr.substr(0, 4) + '-' + vr.substr(4, 4);
    return;
}

// Formata campo telefone com DDD
function formataTelefoneDDD(campo, evt) {
    //(00)0000-0000
    evt = getEvent(evt);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla))
        return;
    vr = campo.value = filtraNumeros(filtraCampo(campo));
    if (vr.length > 10)
        vr = vr.substr(0, 10);
    tam = vr.length;
    if (tam <= 2)
        campo.value = '(' + vr + ')';
    if (tam <= 6)
        campo.value = '(' + vr.substr(0, 2) + ')' + vr.substr(2, 4);
    if (tam > 6)
        campo.value = '(' + vr.substr(0, 2) + ')' + vr.substr(2, 4) + '-' + vr.substr(6, 4);
}
// Formata o campo CEP
function formataCEP(campo, evt) {
    //31255-650
    evt = getEvent(evt);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla))
        return false;
    vr = campo.value = filtraNumeros(filtraCampo(campo));
    tam = vr.length;
    if (tam <= 3)
        campo.value = vr;
    if (tam > 3)
        campo.value = vr.substr(0, tam - 3) + '-' + vr.substr(tam - 3, tam);
}
// Formata o campo Cartão de Crédito
function formataCartaoCredito(campo, evt) {
    //0000.0000.0000.0000
    evt = getEvent(evt);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla))
        return;
    var vr = campo.value = filtraNumeros(filtraCampo(campo));
    var tammax = 16;
    var tam = vr.length;
    if (tam < tammax && tecla != 8)
    { tam = vr.length + 1; }
    if (tam < 5)
    { campo.value = vr; }
    if ((tam > 4) && (tam < 9))
    { campo.value = vr.substr(0, 4) + '.' + vr.substr(4, tam - 4); }
    if ((tam > 8) && (tam < 13))
    { campo.value = vr.substr(0, 4) + '.' + vr.substr(4, 4) + '.' + vr.substr(8, tam - 4); }
    if (tam > 12)
    { campo.value = vr.substr(0, 4) + '.' + vr.substr(4, 4) + '.' + vr.substr(8, 4) + '.' + vr.substr(12, tam - 4); }
}
// limpa todos os caracteres especiais do campo solicitado, usar só aqui dentro!!!!!!
function filtraCampo(campo) {
    var s = "";
    var cp = "";
    vr = campo.value;
    tam = vr.length;
    for (i = 0; i < tam; i++) {
        if (vr.substring(i, i + 1) != "/"
&& vr.substring(i, i + 1) != "-"
&& vr.substring(i, i + 1) != "."
&& vr.substring(i, i + 1) != ":"
&& vr.substring(i, i + 1) != ",") {
            s = s + vr.substring(i, i + 1);
        }
    }
    return s;
    //return campo.value.replace("/", "").replace("-", "").replace(".", "").replace(",", "")
}
// limpa todos caracteres que não são números, usar só aqui dentro!!!!!!
function filtraNumeros(campo) {
    var s = "";
    var cp = "";
    vr = campo;
    tam = vr.length;
    for (i = 0; i < tam; i++) {
        if (vr.substring(i, i + 1) == "0" ||
vr.substring(i, i + 1) == "1" ||
vr.substring(i, i + 1) == "2" ||
vr.substring(i, i + 1) == "3" ||
vr.substring(i, i + 1) == "4" ||
vr.substring(i, i + 1) == "5" ||
vr.substring(i, i + 1) == "6" ||
vr.substring(i, i + 1) == "7" ||
vr.substring(i, i + 1) == "8" ||
vr.substring(i, i + 1) == "9") {
            s = s + vr.substring(i, i + 1);
        }
    }
    return s;
    //return campo.value.replace("/", "").replace("-", "").replace(".", "").replace(",", "")
}


// limpa todos caracteres que não são números, menos a vírgula
function filtraNumerosComVirgula(campo) {
    var s = "";
    var cp = "";
    vr = campo;
    tam = vr.length;
    var complemento = 0; //flag paga contar o número de virgulas
    for (i = 0; i < tam; i++) {
        if ((vr.substring(i, i + 1) == "," && complemento == 0 && s != "") ||
vr.substring(i, i + 1) == "0" ||
vr.substring(i, i + 1) == "1" ||
vr.substring(i, i + 1) == "2" ||
vr.substring(i, i + 1) == "3" ||
vr.substring(i, i + 1) == "4" ||
vr.substring(i, i + 1) == "5" ||
vr.substring(i, i + 1) == "6" ||
vr.substring(i, i + 1) == "7" ||
vr.substring(i, i + 1) == "8" ||
vr.substring(i, i + 1) == "9") {
            if (vr.substring(i, i + 1) == ",")
                complemento = complemento + 1;
            s = s + vr.substring(i, i + 1);
        }
    }
    return s;
}
//recupera tecla
//evita criar mascara quando as teclas são pressionadas
function teclaValida(tecla) {
    if (tecla == 8 //backspace
|| tecla == 45 //insert
|| tecla == 46 //delete
|| tecla == 36 //home
|| tecla == 37 //esquerda
|| tecla == 38 //cima
|| tecla == 39 //direita
|| tecla == 40)//baixo
        return false;
    else
        return true;
}
// recupera o evento do form
function getEvent(evt) {
    if (!evt) evt = window.event; //IE
    return evt;
}

//Recupera o código da tecla que foi pressionado
function getKeyCode(evt) {
    var code;
    if (evt.keyCode != 'undefined' && evt.keyCode != '0')
        code = evt.keyCode;
    else if (evt.which != 'undefined' && evt.which != '0')
        code = evt.which;
    else if (evt.charCode != 'undefined' && evt.charCode != '0')
        code = evt.charCode;
    else
        return 0;
    return code;
}

function ValidEmail(oSrc, args) {

    vr = args.Value;

    var reg = new RegExp("^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
    //var reg = new RegExp("^(([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}){1,25})+([;.](([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}){1,25})+)*$");

    if (vr.lastIndexOf(reg) == -1) {
        args.IsValid = false;
        return;
    }
    args.IsValid = true;

}

// Formata campo Email
function formataEmail(campo, evt) {
    evt = getEvent(evt);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla) || !teclaValidaEmail(tecla))
        return;
}

function teclaValidaEmail(tecla) {
    if (tecla == 32) //space
        return false;
    else
        return true;
}

function vcnpj(oSrc, args) {

    var numeros, digitos, soma, i, resultado, pos, tamanho, digitos_iguais, cnpj = args.Value.replace(/\D+/g, '');
    digitos_iguais = 1;
    if (cnpj.length != 14) {
        args.IsValid = false;
    }

    for (i = 0; i < cnpj.length - 1; i++)
        if (cnpj.charAt(i) != cnpj.charAt(i + 1)) {
        digitos_iguais = 0;
        break;
    }
    if (!digitos_iguais) {
        tamanho = cnpj.length - 2
        numeros = cnpj.substring(0, tamanho);
        digitos = cnpj.substring(tamanho);
        soma = 0;
        pos = tamanho - 7;
        for (i = tamanho; i >= 1; i--) {
            soma += numeros.charAt(tamanho - i) * pos--;
            if (pos < 2)
                pos = 9;
        }
        resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
        if (resultado != digitos.charAt(0)) {
            args.IsValid = false;
        }

        tamanho = tamanho + 1;
        numeros = cnpj.substring(0, tamanho);
        soma = 0;
        pos = tamanho - 7;
        for (i = tamanho; i >= 1; i--) {
            soma += numeros.charAt(tamanho - i) * pos--;
            if (pos < 2)
                pos = 9;
        }
        resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
        if (resultado != digitos.charAt(1)) {
            args.IsValid = false;
        }
        else {
            args.IsValid = true;
        }
    }
    else {
        args.IsValid = false;
    }
}

function vcpf(oSrc, args) {
    var numeros, digitos, soma, i, resultado, digitos_iguais, cpf = args.Value.replace(/\D+/g, '');
    digitos_iguais = 1;
    if (cpf.length < 11)
        args.IsValid = false;
    for (i = 0; i < cpf.length - 1; i++)
        if (cpf.charAt(i) != cpf.charAt(i + 1)) {
        digitos_iguais = 0;
        break;
    }
    if (!digitos_iguais) {
        numeros = cpf.substring(0, 9);
        digitos = cpf.substring(9);
        soma = 0;
        for (i = 10; i > 1; i--)
            soma += numeros.charAt(10 - i) * i;
        resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
        if (resultado != digitos.charAt(0))
            args.IsValid = false;
        numeros = cpf.substring(0, 10);
        soma = 0;
        for (i = 11; i > 1; i--)
            soma += numeros.charAt(11 - i) * i;
        resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
        if (resultado != digitos.charAt(1))
            args.IsValid = false;
        args.IsValid = true;
    }
    else
        args.IsValid = false;
}

// permite apenas números
function isNumberKey(evt) {
	var charCode = getKeyCode(evt);
	if ((charCode > 31 && charCode < 48) || (charCode > 57 && charCode != 127))
		return false;
	return true;
}

// permite apenas números e ponto
function isNumberDotKey(evt) {
    var charCode = getKeyCode(evt);
    if ((charCode > 31 && charCode < 48 && charCode != 46) || (charCode > 57 && charCode != 127))
        return false;
    return true;
}

//permite apenas números e vírgula
function isNumberCommaKey(evt) {
    var charCode = getKeyCode(evt);
    if ((charCode > 31 && charCode < 48 && charCode != 44) || (charCode > 57 && charCode != 127))
        return false;
    return true;
}

// permite apenas números entre 1 e 5
// Obs.:Usado para a quantidade de dígitos permitida para o Número de Inscrição
function isOneToFiveKey(evt) {
	var charCode = getKeyCode(evt);
	if ((charCode > 31 && charCode < 49) || (charCode > 53 && charCode != 127))
		return false;
	return true;
}

//permite apenas números, vírgula, e verifica quantidade de casas decimais e valor máximo
function isValidDecimal(keypressevent, n_casas_dec, maxVal) {
    validkey = isNumberCommaKey(event);
    if (validkey) {
        val = event.srcElement.value.replace(",", ".");
        newchar = String.fromCharCode(getKeyCode(event)).replace(",", ".");

        if (validkey && ".".indexOf(newchar) >= 0 && ".".indexOf(val[val.length - 1]) >= 0)
            validkey = false;

        if (validkey && val.split(".").length > 1 && val.split(".")[1].length >= n_casas_dec)
            validkey = false;

        if (validkey) {                    
            newval = parseFloat(val + newchar);
            if (newval > maxVal) validkey = false;
        }
    }
    return validkey;
}

function formatScoreColor(control, val_approv) {
    strVal = control.value.replace(",",".");
    val = parseFloat(strVal);    
    if(val < parseFloat(val_approv))
        $(control).css({ "color": "Red" });
    else 
        $(control).css({ "color": "Blue" });      
}

function formatDecimal(blurevent, n_casas_dec, max_val) {
    strVal = blurevent.srcElement.value.replace(",",".");
    if(strVal != "") {
        //filtra apenas os números
        v1 = filtraNumeros(strVal.split(".")[0]); //parte inteira
        v2 = strVal.split(".").length > 1 ? filtraNumeros(strVal.split(".")[1]) : ""; //parte decimal
        
        while(v1.length > 0 && v1[0] == "0")
            v1 = v1.substring(1, v1.length);        
        if(v1.length == 0) v1 = "0";
        while(v2.length > 0 && v2[v2.length-1] == "0")
            v2 = v2.substring(0, v2.length - 1);
        while(v2.length < n_casas_dec)
            v2 += "0";
        
        if(parseFloat(v1 + "." + v2) > max_val) {
            blurevent.srcElement.value = max_val.toString().replace(".",",");
            formatDecimal(blurevent, n_casas_dec, max_val);
        } else {
            if(n_casas_dec == 0)
                blurevent.srcElement.value = v1;
            else 
                blurevent.srcElement.value = v1 + "," + v2;
        }
    }
}

//by Debora Matteo:

//usado para Nomes, permite caracteres, ç, acentos COM números
function alphanumeric_only(e) {
    var keycode;
    if (window.event) keycode = window.event.keyCode;
    else if (event) keycode = event.keyCode;
    else if (e) keycode = e.which;
    else return true;
    if ((keycode > 47 && keycode <= 57) || (keycode >= 65 && keycode <= 90) || (keycode >= 97 && keycode <= 122) || (keycode >= 192 && keycode <= 255) || (keycode == 32)) {
        return true;
    }
    else {
        return false;
    }
    return true;
}

//Usado para RG, permite números e letras, sem acento!!
function rg(e) {
    var keycode;
    if (window.event) keycode = window.event.keyCode;
    else if (event) keycode = event.keyCode;
    else if (e) keycode = e.which;
    else return true;
    if ((keycode > 47 && keycode <= 57) || (keycode >= 65 && keycode <= 90) || (keycode >= 97 && keycode <= 122)) {
        return true;
    }
    else {
        return false;
    }
    return true;
}

//usado para nome, permite caracteres, ç, acentos, SEM números
function nomeSemNum(e) {
    var keycode;
    if (window.event) keycode = window.event.keyCode;
    else if (event) keycode = event.keyCode;
    else if (e) keycode = e.which;
    else return true;
    if ((keycode >= 65 && keycode <= 90) || (keycode >= 97 && keycode <= 122) || (keycode >= 192 && keycode <= 255) || (keycode == 32)) {
        return true;
    }
    else {
        return false;
    }
    return true;
}

//usado para Endereço, bairro, complemento, permite caracteres, ç, acentos, números e "." (ponto)
function endereco(e) {
    var keycode;
    if (window.event) keycode = window.event.keyCode;
    else if (event) keycode = event.keyCode;
    else if (e) keycode = e.which;
    else return true;
    if ((keycode >= 48 && keycode <= 57) || (keycode >= 65 && keycode <= 90) || (keycode >= 97 && keycode <= 122) || (keycode >= 192 && keycode <= 255) || (keycode == 32) || (keycode == 46)) {
        return true;
    }
    else {
        return false;
    }
    return true;
}


function Count(text, longNum) {

    var maxlength = new Number(longNum); // Change number to your max length.

    if (text.value.length > maxlength) {

        text.value = text.value.substring(0, maxlength);

    }
}

function MontaDataSemHora(strdata)
{
    //alert(dateFormat(new Date(strdata), "dd, mm, yyyy"));

    return getDateFromFormat(strdata,"dd/mm/yyyy");
}


// ------------------------------------------------------------------
// Utility functions for parsing in getDateFromFormat()
// ------------------------------------------------------------------
function _isInteger(val) {
	var digits = "1234567890";
	for (var i=0; i < val.length; i++) {
		if (digits.indexOf(val.charAt(i)) == -1) { return false; }
		}
	return true;
	}
function _getInt(str,i,minlength,maxlength) {
	for (x=maxlength; x>=minlength; x--) {
		var token = str.substring(i,i+x);
		if (token.length < minlength) {
			return null;
			}
		if (_isInteger(token)) { 
			return token;
			}
		}
	return null;
	}


function getDateFromFormat(val,format) {
    var MONTH_NAMES = new Array('Janeiro','Fevereiro','Março','Abril','Maio','Junho','Julho','Agosto','Setembro','Outubro','Novembro','Dezembro','Jan','Fev','Mar','Abr','Mai','Jun','Jul','Ago','Set','Out','Nov','Dez');
    var DAYS_OF_WEEK = new Array('Domingo', 'Segunda-feira','Terça-feira','Quarta-feira','Quinta-feira','Sexta-feira','Sábado','Dom','Seg','Ter','Qua','Qui','Sex','Sab');
    
                // val = obj.value+"";
  
                val = val+"";
                format = format+"";
                var i_val = 0;
                var i_format = 0;
                var c = "";
                var token = "";
                var token2= "";
                var x,y;
                var now   = new Date();
                var year  = now.getYear();
                var month = now.getMonth()+1;
                var date  = now.getDate();
                var hh    = now.getHours();
                var MM    = now.getMinutes();
                var ss    = now.getSeconds();
                var ampm  = "";
                
                while (i_format < format.length) {
                               // Get next token from format string
                               c = format.charAt(i_format);
                               token = "";
                               while ((format.charAt(i_format) == c) && (i_format < format.length)) {
                                               token += format.charAt(i_format);
                                               i_format++;
                                               }
                               // Extract contents of value based on format token
                               
                               if (token=="yyyy" || token=="yy" || token=="y") {
                                               if (token=="yyyy") { x=4;y=4; }// 4-digit year
                                               if (token=="yy")   { x=2;y=2; }// 2-digit year
                                               if (token=="y")    { x=1;y=2; }// 2-or-4-digit year
                                               year = _getInt(val,i_val,x,y);
                                               //alert(year);
                                               if (year == null) 
                                                {  x=1; y=1;
                                                   year = _getInt(val,i_val,x,y);         
                                                }
                                               if (year == null) { return 0; }
                                               
                                               i_val += year.length;
                                               if (year.length == 2) {
                                                               if (year > 70) {
                                                                              year = 1900+(year-0);
                                                                              }
                                                               else {
                                                                              year = 2000+(year-0);
                                                                              }
                                                               }
                                               }
                               else if (token=="mmm"){// Month name abbreviaton
                                               month = 0;
                                               
                                               for (var i=0; i<MONTH_NAMES.length; i++) {
                                                               var month_name = MONTH_NAMES[i];
                                                               if (val.substring(i_val,i_val+month_name.length).toLowerCase() == month_name.toLowerCase()) {
                                                                   month = i+1;
                                                                              if (month>12) { month -= 12; }
                                                                              i_val += month_name.length;
                                                                              break;
                                                                              }
                                                               }
                                               if (month == 0) { return 0; }
                                               if ((month < 1) || (month>12)) { return 0; }
                                               
                                               }
                               else if (token=="mmmm"){// Month name long
                                               month = 0;
                                               
                                               for (var i=0; i<MONTH_NAMES.length; i++) {
                                                               var month_name = MONTH_NAMES[i];
                                                               if (val.substring(i_val,i_val+month_name.length).toLowerCase() == month_name.toLowerCase()) {
                                                                   month = i+1;
                                                                              if (month>12) { month -= 12; }
                                                                              i_val += month_name.length;
                                                                              break;
                                                                              }
                                                               }
                                               if (month == 0) { return 0; }
                                               if ((month < 1) || (month>12)) { return 0; }
                                               
                                               }

                               else if (token=="mm" || token=="m") {
                                               x=token.length; y=2;
                                               month = _getInt(val,i_val,x,y);
                                               if (month == null) 
                                                {  x=1; y=1;
                                                   month = _getInt(val,i_val,x,y);         
                                                }
                                               if (month == null) { return 0; }
                                               if ((month < 1) || (month > 12)) { return 0; }
                                               i_val += month.length;
                                   
                                               }
                               else if (token=="dd"  || token=="d") {
                                               x=token.length; y=2;
                                               date = _getInt(val,i_val,x,y);
                                               
                                               if (date == null) 
                                                {  x=1; y=1;
                                                   date = _getInt(val,i_val,x,y);         
                                                }
                                               
                                               if (date == null) 
                                                { return 0; }
                                               
                                               if ((date < 1) || (date>31)) { return 0; }
                                               i_val += date.length;
                                               }
                                               
                               else if (token=="ddd"){// day name abbreviaton
                                               nday = 0;
                                               
                                               for (var i=7; i<DAYS_OF_WEEK.length; i++) {
                                                               var day_name = DAYS_OF_WEEK[i];
                                                               if (val.substring(i_val,i_val+day_name.length).toLowerCase() == day_name.toLowerCase()) {
                                                                   nday = i+1;
                                                                              if (nday>14) { nday -= 14; }
                                                                              i_val += day_name.length;
                                                                              //alert(day_name);
                                                                              break;
                                                                              }
                                                               }
                                               if (nday == 0) { return 0; }
                                               if ((nday < 1) || (nday>14)) { return 0; }
                                               
                                               }
                               else if (token=="dddd"){// day name long
                                               nday = 0;
                                               
                                               for (var i=0; i<DAYS_OF_WEEK.length; i++) {
                                                               var day_name = DAYS_OF_WEEK[i];
                                                               if (val.substring(i_val,i_val+day_name.length).toLowerCase() == day_name.toLowerCase()) {
                                                                   nday = i+1;
                                                                              if (nday>7) { nday -= 7; }
                                                                              i_val += day_name.length;
                                                                              break;
                                                                              }
                                                               }
                                               if (nday == 0) { return 0; }
                                               if ((nday < 1) || (nday>7)) { return 0; }
                                               
                                               }              
                               else if (token=="hh" || token=="h") {
                                               x=token.length; y=2;
                                               hh = _getInt(val,i_val,x,y);
                                               if (hh == null) { return 0; }
                                               if ((hh < 1) || (hh > 12)) { return 0; }
                                               i_val += hh.length;
                                               hh--;
                                               }
                               else if (token=="HH" || token=="H") {
                                               x=token.length; y=2;
                                               hh = _getInt(val,i_val,x,y);
                                               if (hh == null) { return 0; }
                                               if ((hh < 0) || (hh > 23)) { return 0; }
                                               i_val += hh.length;
                                               }
                               else if (token=="KK" || token=="K") {
                                               x=token.length; y=2;
                                               hh = _getInt(val,i_val,x,y);
                                               if (hh == null) { return 0; }
                                               if ((hh < 0) || (hh > 11)) { return 0; }
                                               i_val += hh.length;
                                               }
                               else if (token=="kk" || token=="k") {
                                               x=token.length; y=2;
                                               hh = _getInt(val,i_val,x,y);
                                               if (hh == null) { return 0; }
                                               if ((hh < 1) || (hh > 24)) { return 0; }
                                               i_val += hh.length;
                                               h--;
                                               }
                               else if (token=="MM" || token=="M") {
                                               x=token.length; y=2;
                                               mm = _getInt(val,i_val,x,y);
                                               if (mm == null) { return 0; }
                                               if ((mm < 0) || (mm > 59)) { return 0; }
                                               i_val += mm.length;
                                               }
                               else if (token=="ss" || token=="s") {
                                               x=token.length; y=2;
                                               ss = _getInt(val,i_val,x,y);
                                               if (ss == null) { return 0; }
                                               if ((ss < 0) || (ss > 59)) { return 0; }
                                               i_val += ss.length;
                                               }
                               else if (token=="a") {
                                               if (val.substring(i_val,i_val+2).toLowerCase() == "am") {
                                                               ampm = "AM";
                                                               }
                                               else if (val.substring(i_val,i_val+2).toLowerCase() == "pm") {
                                                               ampm = "PM";
                                                               }
                                               else {
                                                               return 0;
                                                               }
                                               }
                               else {
                                               if (val.substring(i_val,i_val+token.length) != token) {
                                                               return 0;
                                                               }
                                               else {
                                                               i_val += token.length;
                                                               }
                                               }
                               }

                // Is date valid for month?
                if (month == 2) {
                               // Check for leap year
                               if ( ( (year%4 == 0)&&(year%100 != 0) ) || (year%400 == 0) ) { // leap year
                                               if (date > 29){ return false; }
                                               }
                               else {
                                               if (date > 28) { return false; }
                                               }
                               }
                if ((month==4)||(month==6)||(month==9)||(month==11)) {
                               if (date > 30) { return false; }
                               }
                // Correct hours value
                if (hh<12 && ampm=="PM") {
                               hh+=12;
                               }
                else if (hh>11 && ampm=="AM") {
                               hh-=12;
                
                               }
                var newdate = new Date(year,month-1,date,0,0,0);
                return newdate;
                // alert (newdate.getTime());
                
                
                }

// ------------------------------------------------------------------
// formatDate (date_object, format)
//
// Returns a date in the output format specified.
// The format string uses the same abbreviations as in getDateFromFormat()
// ------------------------------------------------------------------
function formatDateMR(date,format) {
    var MONTH_NAMES = new Array('Janeiro','Fevereiro','Março','Abril','Maio','Junho','Julho','Agosto','Setembro','Outubro','Novembro','Dezembro','Jan','Fev','Mar','Abr','Mai','Jun','Jul','Ago','Set','Out','Nov','Dez');
    var DAYS_OF_WEEK = new Array('Domingo', 'Segunda-feira','Terça-feira','Quarta-feira','Quinta-feira','Sexta-feira','Sábado','Dom','Seg','Ter','Qua','Qui','Sex','Sab');

	if((date == null) || (date.getYear == undefined))  return "";
	format = format+"";
	var result = "";
	var i_format = 0;
	var c = "";
	var token = "";
	var y = date.getYear()+"";
	var m = date.getMonth()+1;
	var d = date.getDate();
	var nday = date.getDay();
	var H = date.getHours();
	var M = date.getMinutes();
	var s = date.getSeconds();
	var yyyy,yy,mmm,mm,dd,hh,h,MM,ss,ampm,HH,H,KK,K,kk,k;
	// Convert real date parts into formatted versions
	// Year
	if (y.length < 4) {
  		y = y-0+1900;
	  }
	
	yyyy = ""+y;
	y = ""+y;
	//yy = ""+y;
   	if (y < 10) { yy = "0"+y; }
		else { yy = y; }
    
	
	y = y.substring(2,4);
	yy = yy.substring(2,4);

	// Month
	if (m < 10) { mm = "0"+m; }
		else { mm = m; }
	
	ddd = DAYS_OF_WEEK[nday+7];
	dddd = DAYS_OF_WEEK[nday];
		
	mmm = MONTH_NAMES[m-1+12];
	mmmm = MONTH_NAMES[m-1];
	
	// Date
	if (d < 10) { dd = "0"+d; }
		else { dd = d; }

	// Hour
	h=H+1;
	K=H;
	k=H+1;
	if (h > 12) { h-=12; }
	if (h == 0) { h=12; }
	if (h < 10) { hh = "0"+h; }
		else { hh = h; }
	if (H < 10) { HH = "0"+K; }
		else { HH = H; }
	if (K > 11) { K-=12; }
	if (K < 10) { KK = "0"+K; }
		else { KK = K; }
	if (k < 10) { kk = "0"+k; }
		else { kk = k; }
	// AM/PM
	if (H > 11) { ampm="PM"; }
	else { ampm="AM"; }
	// Minute
	if (M < 10) { MM = "0"+M; }
		else { MM = M; }
	// Second
	if (s < 10) { ss = "0"+s; }
		else { ss = s; }
	// Now put them all into an object!
	var value = new Object();
	value["yyyy"] = yyyy;
	value["yy"] = yy;
	value["y"] = parseInt(y);
	value["mmm"] = mmm;
	value["mmmm"] = mmmm;
	value["mm"] = mm;
	value["m"] = m;
	value["dddd"] = dddd;
	value["ddd"] = ddd;
	value["dd"] = dd;
	value["d"] = d;
	value["hh"] = hh;
	value["h"] = h;
	value["HH"] = HH;
	value["H"] = H;
	value["KK"] = KK;
	value["K"] = K;
	value["kk"] = kk;
	value["k"] = k;
	value["MM"] = MM;
	value["M"] = M;
	value["ss"] = ss;
	value["s"] = s;
	value["a"] = ampm;
	while (i_format < format.length) {
		// Get next token from format string
		c = format.charAt(i_format);
		token = "";
		while ((format.charAt(i_format) == c) && (i_format < format.length)) {
			token += format.charAt(i_format);
			i_format++;
			}
		if (value[token] != null) {
			result = result + value[token];
			}
		else {
			result = result + token;
			}
		}
	return result;
}

function ASPxDateEditOnParseDate(editor, args) {
    editor.ASPxDateEditLastValue = '';
    if(args.value.length == 0)
    {
        return true;
    }
    if (ASPxDateEditvalidarData(args.value) && ASPxDateEditvalidarDataLimites(args.value,formatDateMR(editor.GetCalendar().minDate, "dd/mm/yyyy"),formatDateMR(editor.GetCalendar().maxDate, "dd/mm/yyyy"))) {
        args.date = MontaDataSemHora(args.value);
        args.handled = true;
        return true;
    }
    else {
        editor.ASPxDateEditLastValue = editor.inputElement.value;
        args.date = null;
        args.handled = true;
        return false;
    }
}

function checkleapyear(datea)
{
	datea = parseInt(datea);
	if(datea%4 == 0)
	{
		if(datea%100 != 0)
		{
			return true;
		}
		else
		{
			if(datea%400 == 0)
				return true;
			else
                return false;
		}
	}
return false;
}

function ASPxDateEditvalidarDataLimites(newdata, mindata, maxdata)
{
    if((mindata != null) && (mindata.length > 0))
    {
        if(MontaDataSemHora(newdata) < MontaDataSemHora(mindata))
        {
            alert("Data Informada menor que data mínima: " + mindata);
            return false;
        }
    }    
    if((maxdata != null) && (maxdata.length > 0))
    {
        if(MontaDataSemHora(newdata) > MontaDataSemHora(maxdata))
        {
            alert("Data Informada maior que data máxima: " + maxdata);
            return false;
        }
    }    
    return true;
}
 
 function ASPxDateEditvalidarData(campo) {
    //var expReg = /^(([0-2]\d|[3][0-1])\/([0]\d|[1][0-2])\/[1-2][0-9]\d{2})$/;

    var expReg = /^([0]?[1-9]|[1|2][0-9]|[3][0|1])[/]([0]?[1-9]|[1][0-2])[/]([0-9]{4}|[0-9]{2})$/;
    
    var msgErro = 'Formato inválido de data.';
    if ((campo.match(expReg)) && (campo != '')) {
//                var dia = campo.substring(0, 2);
//                var mes = campo.substring(3, 5);
//                var ano = campo.substring(6, 10);

        var dia = campo.split('/')[0];
        var mes = campo.split('/')[1];
        var ano = campo.split('/')[2];
        
        if ((mes == 4 || mes == 6 || mes == 9 || mes == 11) && dia > 30) {
            alert("Dia incorreto !!! O mês especificado contém no máximo 30 dias.");
            return false;
        } else {
            if ((!checkleapyear(ano) && mes == 2) && dia > 28) {
                alert("Data incorreta!! O mês especificado contém no máximo 28 dias.");
                return false;
            } else {
                if ((checkleapyear(ano) && mes == 2) && dia > 29) {
                    alert("Data incorreta!! O mês especificado contém no máximo 29 dias.");
                    return false;
                } else {
                    //alert("Data correta!");
                    return true;
                }
            }
        }
    } else {
        alert(msgErro);
        return false;
    }
}

function MRAcceptNumber(s, e, max)
{
  //  alert(e.htmlEvent.keyCode);
    if(e.htmlEvent.keyCode >= 48 && e.htmlEvent.keyCode <= 57)  
    {   
        if(max <= 0) return true;
        if(s.inputElement.value.length < max)
                return true;
    }    
    e.htmlEvent.keyCode = 0;
    return false;
}




function validDate(obj)
{

date=obj.value
if (/[^\d/]|(\/\/)/g.test(date))  {obj.value=obj.value.replace(/[^\d/]/g,'');obj.value=obj.value.replace(/\/{2}/g,'/'); return }
if (/^\d{2}$/.test(date)){obj.value=obj.value+'/'; return }
if (/^\d{2}\/\d{2}$/.test(date)){obj.value=obj.value+'/'; return }
if (!/^\d{1,2}\/\d{1,2}\/\d{4}$/.test(date)) return

 test1=(/^\d{1,2}\/?\d{1,2}\/\d{4}$/.test(date))
 date=date.split('/')
 d=new Date(date[2],date[1]-1,date[0])
 test2=(1*date[0]==d.getDate() && 1*date[1]==(d.getMonth()+1) && 1*date[2]==d.getFullYear())
 if (test1) return true
 
 obj.select();
 obj.focus()
 return false

}



function ASPxDateOnlyNumber(e)
{
   return (isNumberKey(e));
}

function ASPxDateEditMaskData(s, e)
{
    validDate(s.inputElement);
}


function ASPxDateEditformataData(campovalor, tecla) {
    //dd/MM/yyyy
    if(campovalor == undefined)  return '';
    vr = ASPxDateEditfiltraNumeros(ASPxDateEditfiltraCampo(campovalor));
    tam = vr.length;
    if (tam >= 2 && tam < 4)
        return vr.substr(0, 2) + '/' + vr.substr(2);
    if (tam == 4)
        return vr.substr(0, 2) + '/' + vr.substr(2, 2) + '/';
    if (tam > 4)
        return vr.substr(0, 2) + '/' + vr.substr(2, 2) + '/' + vr.substr(4);
    return vr;    
}


function ASPxDateEditfiltraNumeros(campo) {
    var s = "";
    var cp = "";
    vr = campo;
    tam = vr.length;
    for (i = 0; i < tam; i++) {
        if (vr.substring(i, i + 1) == "0" || vr.substring(i, i + 1) == "1" || vr.substring(i, i + 1) == "2" ||
            vr.substring(i, i + 1) == "3" || vr.substring(i, i + 1) == "4" || vr.substring(i, i + 1) == "5" ||
            vr.substring(i, i + 1) == "6" || vr.substring(i, i + 1) == "7" || vr.substring(i, i + 1) == "8" ||
            vr.substring(i, i + 1) == "9") {    
            s = s + vr.substring(i, i + 1);
        }
    }
    return s;
    //return campo.value.replace("/", "").replace("-", "").replace(".", "").replace(",", "")
}
function ASPxDateEditfiltraCampo(campo) {
    var s = "";
    var cp = "";
    vr = campo;
    tam = vr.length;
    for (i = 0; i < tam; i++) {
        if (vr.substring(i, i + 1) != "/" && vr.substring(i, i + 1) != "-" && vr.substring(i, i + 1) != "."
            && vr.substring(i, i + 1) != ":" && vr.substring(i, i + 1) != ",") {
            s = s + vr.substring(i, i + 1);
        }
    }
    return s;
    //return campo.value.replace("/", "").replace("-", "").replace(".", "").replace(",", "")
}



function ASPxDateEditLostFocus(editor, args) {
    if(editor.ASPxDateEditLastValue != undefined && editor.ASPxDateEditLastValue.length > 0)
    {
            editor.SetFocus();
            editor.ASPxDateEditLastValue = '';
            return false; 
    }        
    return true;    
}





function ASPxDateEditGotFocusOld(editor, args) {
    document.getElementById("teste").value = document.getElementById("teste").value + "gotfocus;";
    if(editor.ASPxDateEditLastValue != undefined && editor.ASPxDateEditLastValue.length > 0)
    {
            document.getElementById("teste").value = document.getElementById("teste").value + editor.ASPxDateEditLastValue;
            editor.inputElement.value = editor.ASPxDateEditLastValue;
            editor.SkipParse =true;
            editor.SetText(editor.ASPxDateEditLastValue);
            editor.inputElement.value = editor.ASPxDateEditLastValue;
            editor.SkipParse = false;
            editor.ASPxDateEditLastValue = '';
            return true; 
    }        
    return true;    
}

function moveRelogio() {
    var now = new Date();
    var divHoraData = document.getElementById("divHoraData");
    if (divHoraData!=null && typeof divHoraData != 'undefined') {
        divHoraData.innerHTML = formatDateMR(now, "dddd, d") + "&nbsp;de&nbsp;" + formatDateMR(now, "mmmm") + "&nbsp;de&nbsp;" + formatDateMR(now, "yyyy") + "&nbsp;-&nbsp;" +  formatDateMR(now,"HH:MM:ss");
        setTimeout("moveRelogio()", 1000)
    }
}


	function resizeConteudo()
	{
		var conteudo=document.getElementById('conteudo');
		if(typeof conteudo != 'undefined' && conteudo!=null)
		{
			var h= window.innerHeight ? window.innerHeight - 110 : /* For non-IE */  
             document.documentElement ? document.documentElement.clientHeight  - 110 : /* IE 6+ (Standards Compilant Mode) */  
             document.body ? document.body.clientHeight  - 110 : /* IE 4 Compatible */  
             window.screen.height; /* Others (It is not browser window size, but screen size) */  
            if(h<0) h=0;
			conteudo.style.height=h.toString()+"px";
		}
	}

//Função para centralizar tratamento de CEP)
function trataCep(controles) {
    var copiaValor = function(id, val) {
        if (typeof id != 'undefined' && id!=null && typeof val != 'undefined') {
            $('#'+id).val(val);
        }
    }

    var $tscep=$('#'+controles.tscep);
    var $cep=$('#'+controles.cep);
    $tscep.bind('onButtonClick', function() {
        if ($cep.val() != '') {
            $tscep[0].setFilter([{ name: 'cep', controlid: controles.cep}]);
            $tscep[0].setAutoExecute(true);
        }
        else {
            $tscep[0].setAutoExecute(false);
        }
    });
    $tscep.bind('onValueChanged', function() {
        var rowData = $tscep[0].getRow();
        copiaValor(controles.cep,rowData.cep);
        copiaValor(controles.nomeLogradouro, rowData.nomeLogradouro);
        copiaValor(controles.nomeBairro, rowData.nomeBairro);
        copiaValor(controles.codigoLogradouro, rowData.codigoLogradouro);
        copiaValor(controles.codigoBairro, rowData.codigoBairro);
        copiaValor(controles.uf, rowData.uf);
        copiaValor(controles.nomeMunicipio, rowData.nomeMunicipio);

        if (typeof controles.codigoMunicipio != 'undefined') {
            var idCodigo, idNome;
            var $cod=$('#' + controles.codigoMunicipio);
            if ($cod.length > 0) {
                idCodigo = $cod[0].id;
                if ($cod[0].nextSibling != null) {
                    idNome = $cod[0].nextSibling.id;
                }
                
            }
            copiaValor(idCodigo, rowData.codigoMunicipio);
            copiaValor(idNome, rowData.nomeMunicipio);
        }
        if(typeof controles.codigos != 'undefined') {
            var comp = rowData.cep + "-" + rowData.codigoLogradouro + "-" + rowData.codigoBairro + "-" + rowData.codigoMunicipio;
            copiaValor(controles.codigos,comp);
        }
    });
}

        var Base64 = {

            // private property
            _keyStr: "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=",

            // public method for encoding
            encode: function(input) {
                var output = "";
                var chr1, chr2, chr3, enc1, enc2, enc3, enc4;
                var i = 0;

                input = Base64._utf8_encode(input);

                while (i < input.length) {

                    chr1 = input.charCodeAt(i++);
                    chr2 = input.charCodeAt(i++);
                    chr3 = input.charCodeAt(i++);

                    enc1 = chr1 >> 2;
                    enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
                    enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
                    enc4 = chr3 & 63;

                    if (isNaN(chr2)) {
                        enc3 = enc4 = 64;
                    } else if (isNaN(chr3)) {
                        enc4 = 64;
                    }

                    output = output +
			this._keyStr.charAt(enc1) + this._keyStr.charAt(enc2) +
			this._keyStr.charAt(enc3) + this._keyStr.charAt(enc4);

                }

                return output;
            },

            // public method for decoding
            decode: function(input) {
                var output = "";
                var chr1, chr2, chr3;
                var enc1, enc2, enc3, enc4;
                var i = 0;

                input = input.replace(/[^A-Za-z0-9\+\/\=]/g, "");

                while (i < input.length) {

                    enc1 = this._keyStr.indexOf(input.charAt(i++));
                    enc2 = this._keyStr.indexOf(input.charAt(i++));
                    enc3 = this._keyStr.indexOf(input.charAt(i++));
                    enc4 = this._keyStr.indexOf(input.charAt(i++));

                    chr1 = (enc1 << 2) | (enc2 >> 4);
                    chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
                    chr3 = ((enc3 & 3) << 6) | enc4;

                    output = output + String.fromCharCode(chr1);

                    if (enc3 != 64) {
                        output = output + String.fromCharCode(chr2);
                    }
                    if (enc4 != 64) {
                        output = output + String.fromCharCode(chr3);
                    }

                }

                output = Base64._utf8_decode(output);

                return output;

            },

            // private method for UTF-8 encoding
            _utf8_encode: function(string) {
                string = string.replace(/\r\n/g, "\n");
                var utftext = "";

                for (var n = 0; n < string.length; n++) {

                    var c = string.charCodeAt(n);

                    if (c < 128) {
                        utftext += String.fromCharCode(c);
                    }
                    else if ((c > 127) && (c < 2048)) {
                        utftext += String.fromCharCode((c >> 6) | 192);
                        utftext += String.fromCharCode((c & 63) | 128);
                    }
                    else {
                        utftext += String.fromCharCode((c >> 12) | 224);
                        utftext += String.fromCharCode(((c >> 6) & 63) | 128);
                        utftext += String.fromCharCode((c & 63) | 128);
                    }

                }

                return utftext;
            },

            // private method for UTF-8 decoding
            _utf8_decode: function(utftext) {
                var string = "";
                var i = 0;
                var c = c1 = c2 = 0;

                while (i < utftext.length) {

                    c = utftext.charCodeAt(i);

                    if (c < 128) {
                        string += String.fromCharCode(c);
                        i++;
                    }
                    else if ((c > 191) && (c < 224)) {
                        c2 = utftext.charCodeAt(i + 1);
                        string += String.fromCharCode(((c & 31) << 6) | (c2 & 63));
                        i += 2;
                    }
                    else {
                        c2 = utftext.charCodeAt(i + 1);
                        c3 = utftext.charCodeAt(i + 2);
                        string += String.fromCharCode(((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63));
                        i += 3;
                    }

                }

                return string;
            }

        }



function disableNavigationKeys(disabledkeys){
    var keyTypes={Enter:1, Backspace:2};
    $(document).keydown(function (event) {
        var cancelKey=false;
        var key = event.keyCode || event.which;
        if(key==13){
            if((disabledkeys & keyTypes.Enter)>0){
                cancelKey = true;
            }
        }
         if(key==8){
            if((disabledkeys & keyTypes.Backspace)>0){
                cancelKey = true;
            }
        }
        if(cancelKey == true && event.currentTarget==$(document)[0])
        {
            var t=event.srcElement.type;
            var readonly = (event.srcElement.readOnly == true);
            var $parentSkip=$(event.srcElement).parents('[skipDisableNavigationKey=true]');
            if($parentSkip.length>0)
                return true;
            if( (t == 'text' || t == 'textarea' || t == 'submit'  || t == 'password'))
            {
                if(key==8 || (key==13 && t == 'textarea'))
                    return !readonly;
                else
                    return false;
            }
        }
        return true;

    });          
}

function getTextSelection(textComponent)
{  
    var selectedText;
    // IE version
    if (document.selection != undefined) {
        textComponent.focus();
        var sel = document.selection.createRange();
        selectedText = sel.text;
    }
    // Mozilla version
    else if (textComponent.selectionStart != undefined) {
        var startPos = textComponent.selectionStart;
        var endPos = textComponent.selectionEnd;
        selectedText = textComponent.value.substring(startPos, endPos)
    }
    return selectedText;
}

function copyToClipboard(s) {
	if(window.clipboardData && clipboardData.setData )	
		clipboardData.setData("Text", s);	
}

function CopyASPxGridViewToClipboardAsTSV(grid) {
    var content = "";
    var headers = grid.GetHeadersRow().cells;
    for (icell = 0; icell < headers.length; icell++)
        content += headers[icell].innerText + "\t";
    content += "\n";
    for (irow = grid.visibleStartIndex; irow < grid.pageRowCount; irow++) {
        var row = grid.GetRow(irow).cells;
        for (icell = 0; icell < row.length; icell++) {
            var children = row[icell].childNodes;
            for(ichild = 0; ichild < children.length; ichild++)
                if(children[ichild].nodeValue)
                    content += children[ichild].nodeValue + " ";        
            content += "\t";    
        }
        content += "\n";                               
    }
    copyToClipboard(content);
}

function isIE() {
     return window.clipboardData && clipboardData.setData;
}

function preencherDadosPorCEP(controles) {
    var copiaValor = function(id, val) {
        if (typeof id != 'undefined'
            && id != null
            && typeof val != 'undefined') {
            $('#' + id).val(val);
        }
    };

    var $tscep = $('#' + controles.tscep);
    var $cep = $('#' + controles.cep);

    $tscep.bind('onButtonClick', function() {
        if ($cep.val() != '') {
            $tscep[0].setFilter([{ name: 'cep', controlid: controles.cep}]);
            $tscep[0].setAutoExecute(true);
        }
        else {
            $tscep[0].setAutoExecute(false);
        }
    });

    $tscep.bind('onValueChanged', function() {
        var rowData = $tscep[0].getRow();

        copiaValor(controles.cep, rowData.cep);
        copiaValor(controles.nomeLogradouro, rowData.logradouro);
        copiaValor(controles.uf, rowData.uf);
        copiaValor(controles.nomeMunicipio, rowData.municipio);

        if (typeof controles.codigoMunicipio != 'undefined') {
            var idCodigo, idNome;
            var $cod = $('#' + controles.codigoMunicipio);

            if ($cod.length > 0) {
                idCodigo = $cod[0].id;

                if ($cod[0].nextSibling != null) {
                    idNome = $cod[0].nextSibling.id;
                }

            }

            copiaValor(idCodigo, rowData.id_municipio);
            copiaValor(idNome, rowData.municipio);
        }
    });
}
