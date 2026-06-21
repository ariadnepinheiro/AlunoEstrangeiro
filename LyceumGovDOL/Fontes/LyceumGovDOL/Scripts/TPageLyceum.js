function waitSign() { var c = document.createElement("table"); var d = c.insertRow(); var a = d.insertCell(); c.width = "100%"; a.valign = "middle"; var b = document.createElement("<spam style='background-color:white;z-index:500;border-style:solid;border-width:1px;border-color:black;position:absolute;text-align:center;vertical-align:middle;top:10;left:10;height:250px;width:90%'>"); b.innerHTML = "Aguarde..."; a.insertBefore(b); document.body.insertBefore(c) };

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