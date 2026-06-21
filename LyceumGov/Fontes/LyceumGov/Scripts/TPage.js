var __submitted; function switchPostBack() { __submitted = false; __oldPostBack = __doPostBack; __doPostBack = __newPostBack } function checkSubmit(a) { SaveFocus(); if (a) { return true } if (__submitted) { if (window.event != null) { window.event.returnValue = false } waitSign(); return false } else { __submitted = true; disableHref(document); if (window.event != null) { window.event.returnValue = true } return true } } function __oldPostBack() { } function __newPostBack(e, a) { var b = (new String(e)).replace(/\$/g, "_").replace(/\:/g, "_"); var d = document.getElementById(b); var c = d != null && d.getAttribute != null && d.getAttribute("bypassCheck") == "true"; if (checkSubmit(c)) { __oldPostBack(e, a) } } function disableHref(d) { var b = []; for (var c = 0; c < document.links.length; c++) { b.push(document.links[c]) } for (var c = 0; c < b.length; c++) { var d = b[c]; if (!d) { continue } var a = d.attributes.getNamedItem("href"); if (a != null) { d.attributes.removeNamedItem("href") } } } function createFocusEvent(c) { if (c.id == null || c.id == "") { return } var a = 'document.FocusElement = document.getElementById("' + c.id + '");'; var b = new Function("eventfocus", a); if (c.addEventListener) { c.addEventListener("focus", b, true) } if (c.attachEvent) { c.attachEvent("onfocus", b) } } function initFocusElement(c) { var a = document.body.getElementsByTagName("input"); for (var b = 0; b < a.length; b++) { if (a[b].type.toLowerCase() != "hidden") { createFocusEvent(a[b]) } } a = document.body.getElementsByTagName("select"); for (var b = 0; b < a.length; b++) { createFocusEvent(a[b]) } a = document.body.getElementsByTagName("textarea"); for (var b = 0; b < a.length; b++) { createFocusEvent(a[b]) } a = document.body.getElementsByTagName("a"); for (var b = 0; b < a.length; b++) { createFocusEvent(a[b]) } } if (window.addEventListener) { window.addEventListener("load", RestoreFocus, true) } else { if (window.attachEvent) { window.attachEvent("onload", RestoreFocus) } } function RestoreFocus(g) { initFocusElement(g); if (document.forms[0].elements.__lastfocus) { var d = document.forms[0].elements.__lastfocus.value.split(","); var j = null; if (d.length > 1) { if (d[0] != "" && d[0] != null) { j = document.getElementById(d[0]) } if (d[1] != "" && d[1] != null && d[2] != "" && d[2] != null) { if (window.scrollTo) { window.scrollTo(d[1], d[2]) } } if (j != null && j.focus) { if (j.style == null || j.style.visibility == null || j.style.visibility != "hidden") { try { j.focus() } catch (h) { } } } } if (j != null) { return } var b = document.all ? document.all : document.getElementsByTagName("*"); for (var f = 0; f < b.length; f++) { if ("a:input:select:textarea:button:".indexOf(b[f].tagName.toLowerCase() + ":") == -1) { continue } if (b[f].getAttribute("type") != null && b[f].getAttribute("type").toLowerCase() == "hidden") { continue } if (b[f].style != null && b[f].style.visibility != null && b[f].style.visibility.toLowerCase() == "hidden") { continue } if (b[f].style != null && b[f].style.display != null && b[f].style.display.toLowerCase() == "none") { continue } if (b[f].tagName.toLowerCase() == "a" && b[f].getAttribute("disabled") != null) { continue } b[f].focus(); break } } } function SaveFocus() { if (document.forms[0].elements.__lastfocus) { var a = document.FocusElement ? document.FocusElement.id : ""; var d = document.FocusElement ? document.FocusElement.tagName : ""; var c = 0, b = 0; if (window.pageXOffset != null) { c = window.pageXOffset; b = window.pageYOffset } else { if (document.body.scrollLeft != null) { c = document.body.scrollLeft; b = document.body.scrollTop } } document.forms[0].elements.__lastfocus.value = a + "," + c + "," + b + "," + d } };

function Bloqueio() {
    var divBloqueio = document.getElementById("dvbloqueio");
    divBloqueio.className = "Bloqueado";
}

function OnInitASPxPopupControl(s, e) {
    //Verificar se o navegador utilizado é o Firefox
    var isFirefox = typeof InstallTrigger !== 'undefined';
    if (typeof s.name == 'undefined' || !isFirefox) {
        return;
    }
    var lightboxDiv = document.getElementById(s.name + "_TCFix-1");
    lightboxDiv.style.zIndex = 12000;
}

function OnInitASPxPopupControlSize(s, e, tamanho) {

    var isFirefox = typeof InstallTrigger !== 'undefined';
    if (typeof s.name == 'undefined' || !isFirefox) {
        return;
    }
    var lightboxDiv = document.getElementById(s.name + "_TCFix-1");
    lightboxDiv.style.zIndex = tamanho;
}