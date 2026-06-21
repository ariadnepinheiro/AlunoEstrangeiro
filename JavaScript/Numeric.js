//------------------------------------------------------------------------------
// Numeric.js
// ----------
//
//  <INPUT TYPE="text" ID="<id>" MIN="<float>" MAX="<float>" LEN="<int>" DEC="<int>" />
//  numericPrepare("txt");
//
//------------------------------------------------------------------------------

var decSep = ',';
var dash1 = 109;

function numericPrepare(elementName) {
    var element = document.getElementById(elementName);
    if (element == null) return;

    if (element.addEventListener) { // FF
        element.addEventListener("keydown", filterNumber, false);
        element.addEventListener("focus", selectToEnd, false);
        element.addEventListener("mousedown", selectToEnd, false);
        element.addEventListener("contextmenu", filterContextMenu, false);
    }
    else if (element.attachEvent) { // IE
        element.attachEvent("onkeydown", filterNumber);
        element.attachEvent("onfocus", selectToEnd);
        element.attachEvent("oncontextmenu", filterContextMenu);
    }
}

function selectToEnd(evt) {
    if (evt.target) { // FF
        var c = evt.target;
        c.focus();
        c.setSelectionRange(c.value.length, c.value.length);
        evt.preventDefault();
    }

    else { // IE
        var r = document.selection.createRange();
        r.collapse(false);
        r.select();
    }
}

function filterContextMenu(evt) {
    if (evt.target) // FF
        evt.preventDefault();

    else // IE
        evt.returnValue = false;
}

function getAttributeFloat(evt, attributeName) {
    var att = null;
    if (evt.target) // FF
        att = evt.target.attributes[attributeName];
    else if (evt.srcElement) // IE
        att = evt.srcElement.attributes.getNamedItem(attributeName);

    if (att != null)
        return parseFloat(att.value)
    else
        return null;
}

function getAttributeInt(evt, attributeName) {
    var att = null;
    if (evt.target) // FF
        att = evt.target.attributes[attributeName];
    else if (evt.srcElement) // IE
        att = evt.srcElement.attributes.getNamedItem(attributeName);

    if (att != null)
        return parseInt(att.value)
    else
        return null;
}

function setAttribute(element, attributeName, value) {
    if (element.attributes.getNamedItem)
        element.attributes.getNamedItem(attributeName).value = value;
    else
        element.attributes[attributeName].value = value;
}

var Keys = { "Tab": 9, "F2": 113, "Backspace": 8, "Delete": 46, "Dot": 190, "NumPadDot": 194, "Comma": 188, "NumPadComma": 110 };

function filterNumber(e) {
    var dash2, txt;
    if (e.target) { // FF
        dash2 = dash1;
        txt = e.target;
    }
    else if (e.srcElement) { // IE
        dash2 = 189;
        txt = e.srcElement;
    }

    var min = getAttributeFloat(e, "minimumvalue");
    var max = getAttributeFloat(e, "maximumvalue");
    var pre = getAttributeInt(e, "Prec");
    var sca = getAttributeInt(e, "Scal");
    var len = getAttributeInt(e, "maxlength");

    var code = e.keyCode ? e.keyCode : e.charCode;
    var newChar = String.fromCharCode(code);
    var oldValue = txt.value;
    var newValue = null;

    if (code == Keys.Tab || code == Keys.F2) {
        newValue = oldValue;
    } else if (code == Keys.Backspace) {
        newValue = oldValue.substring(0, oldValue.length - 1);
    } else if (code == Keys.Delete) {
        var caretPosition = GetCaretPosition(txt);
        newValue = oldValue.substring(0, caretPosition) + oldValue.substring(caretPosition + 1, oldValue.length);
    } else if ((len == null || oldValue.length < len) && (code >= 48 && code <= 57)) {  // 0, 9
        newValue = oldValue + newChar;
    } else if (code >= 96 && code <= 105) {// numpad 0, numpad 9
        newValue = oldValue + String.fromCharCode(code - 48);
    } else if ((code == dash1 || code == dash2) && oldValue.length == 0) { // -
        newValue = '-';
    } else if (code == Keys.Comma || code == Keys.NumPadComma) {
        if (sca > 0 && oldValue.indexOf(decSep) < 0) {
            newValue = oldValue + decSep;
        }
    }

    if (newValue == null) {
        if (e.preventDefault) e.preventDefault();
        return false;
    } else {
        hideMsg(txt);
        var error = null;
        if (newValue != '-') {
            if (pre != null) {
                var x = newValue;
                if (x.charAt(0) == '-') x = x.substr(1, x.length - 1); //remove eventual sinal negativo do início
                while (x.charAt(0) == '0') x = x.substr(1, x.length - 1); //remove zeros à esquerda
                if (x.indexOf(decSep) >= 0) x = x.substr(0, x.indexOf(decSep)); //obtém apenas a parte inteira
                if ((sca == null || sca == 0) && x.length > pre) {
                    error = true;
                    showMsg(txt, "O n\u00famero pode conter no m\u00e1ximo " + pre.toString() + " d\u00edgito" + (pre > 1 ? "s" : ""));
                } else if (sca != null && sca > 0 && x.length > pre - sca) {
                    error = true;
                    if (pre > sca)
                        showMsg(txt, "O n\u00famero pode conter no m\u00e1ximo " + (pre - sca).toString() + " d\u00edgito" + (pre - sca > 1 ? "s" : "") + " na parte inteira");
                    else
                        showMsg(txt, "O n\u00famero não pode conter d\u00edgitos na parte inteira");
                }
            }

            if (!error && sca != null) {
                var p = newValue.indexOf(decSep);
                if (p >= 0 && p < newValue.length - 1) {
                    var d = newValue.substr(p + 1); // parte decimal
                    while (d.charAt(d.length - 1) == '0') d = d.substr(0, d.length - 1); // remove zeros do fim
                    if (d.length > sca) {
                        error = true;
                        showMsg(txt, "O n\u00famero pode conter no m\u00e1ximo " + sca.toString() + " casa" + (sca > 1 ? "s" : "") + " decima" + (sca > 1 ? "is" : "l"));
                    }
                }
            }

            if (!error) {
                var iNewValue = newValue.replace(decSep, '.');
                var v;
                if (iNewValue.indexOf('.') < iNewValue.length - 1)
                    v = parseFloat(iNewValue);
                else
                    v = parseFloat(iNewValue.substr(0, iNewValue.length-1));

                if(min != null && v < min) {
                    error = true;
                    var smin = min.toString().replace('.', decSep);
                    if(max == null)
                        showMsg(txt, "Valor deve ser maior que " + smin);
                    else {
                        var smax = max.toString().replace('.', decSep);
                        showMsg(txt, "Valor deve estar entre " + smin + " e " + smax);
                    }
                }
                
                if(!error && max != null && v > max) {
                    error = true;
                    var smax = max.toString().replace('.', decSep);
                    if(min == null)
                        showMsg(txt, "Valor deve ser menor que " + smax);
                    else {
                        var smin = min.toString().replace('.', decSep);
                        showMsg(txt, "Valor deve estar entre " + smin + " e " + smax);                        
                    }
                }
            }                        
        }
    }    
    txt.style.backgroundColor = (error != null && error == true) ? "mistyrose" : "transparent";    
}

function GetCaretPosition(ctrl) {
    var CaretPos = 0; // IE Support
    if (document.selection) {
        ctrl.focus();
        var Sel = document.selection.createRange();
        Sel.moveStart('character', -ctrl.value.length);
        CaretPos = Sel.text.length;
    }
    // Firefox support
    else if (ctrl.selectionStart || ctrl.selectionStart == '0')
        CaretPos = ctrl.selectionStart;
    return (CaretPos);
}

function SetCaretPosition(ctrl, pos) {
    if (ctrl.setSelectionRange) {
        ctrl.focus();
        ctrl.setSelectionRange(pos, pos);
    } else if (ctrl.createTextRange) {
        var range = ctrl.createTextRange();
        range.collapse(true);
        range.moveEnd('character', pos);
        range.moveStart('character', pos);
        range.select();
    }
}