var __submitted;

function switchPostBack() {
  __submitted = false;
  __oldPostBack = __doPostBack;
  __doPostBack = __newPostBack;
}

function checkSubmit(bypass) {
  SaveFocus();
  
  if(bypass) return true;
  
  if(__submitted) {
    if(window.event != null) window.event.returnValue = false;
    waitSign();
    return false;
  }
  else {
    __submitted = true;
    disableHref(document);
    if(window.event != null) window.event.returnValue = true;
    
    return true;
  }
}

function __oldPostBack() {
}

function __newPostBack(eventTarget, eventArgument) {
  var targetID = (new String(eventTarget)).replace(/\$/g, "_").replace(/\:/g, "_");
  var target = document.getElementById(targetID);
  var bypassCheck = target != null && target.getAttribute != null && target.getAttribute("bypassCheck") == "true";

  if(checkSubmit(bypassCheck))
    __oldPostBack(eventTarget, eventArgument);
}

function disableHref(element) {
  var links = [];
  for(var i = 0; i < document.links.length; i++)
    links.push(document.links[i]);
    
  for(var i = 0; i < links.length; i++) {
    var element = links[i];
    if(!element) continue;
    
    var href = element.attributes.getNamedItem('href');
    if(href != null) element.attributes.removeNamedItem('href');
  }
}

function createFocusEvent(elem) {
  if(elem.id == null || elem.id == "") return;
  
  var functionBody = 'document.FocusElement = document.getElementById("' + elem.id + '");';
  var func = new Function('eventfocus', functionBody);
  if(elem.addEventListener) elem.addEventListener('focus', func, true);
  if(elem.attachEvent) elem.attachEvent('onfocus', func);
}

//Criando eventos que preenchem a propriedade FocusElement 
function initFocusElement(e) {
  var elems = document.body.getElementsByTagName("input");
  for(var i = 0; i < elems.length; i++)
    if(elems[i].type.toLowerCase() != "hidden")
      createFocusEvent(elems[i]);
      
  elems = document.body.getElementsByTagName("select");
  for(var i = 0; i < elems.length; i++) 
    createFocusEvent(elems[i]);
    
  elems = document.body.getElementsByTagName("textarea");
  for(var i = 0; i < elems.length; i++) 
    createFocusEvent(elems[i]);
      
  elems = document.body.getElementsByTagName("a");
  for(var i = 0; i < elems.length; i++) 
    createFocusEvent(elems[i]);
}

if(window.addEventListener)
  window.addEventListener('load', RestoreFocus, true);
else if(window.attachEvent)
  window.attachEvent('onload', RestoreFocus);
  
function RestoreFocus(e) {
  initFocusElement(e);
  
  if(document.forms[0].elements['__lastfocus']) {
    var a = document.forms[0].elements['__lastfocus'].value.split(',');
    var c = null;
    if(a.length > 1) {
      if(a[0] != "" && a[0] != null)
        c = document.getElementById(a[0]);
      if(a[1] != "" && a[1] != null && a[2] != "" && a[2] != null)
        if(window.scrollTo) window.scrollTo(a[1], a[2]);
      if (c != null && c.focus) {
          if (c.style == null || c.style.visibility == null || c.style.visibility != 'hidden') {
              try
              {
              c.focus();
              }
              catch(focusError)
              {}
          }
        }
    }
    if(c != null) return;
    
    //primeiro controle
    var x = document.all ? document.all : document.getElementsByTagName('*');
    
    for(var i = 0; i < x.length; i++) {
      if("a:input:select:textarea:button:".indexOf(x[i].tagName.toLowerCase() + ":") == -1)
        continue;
      if(x[i].getAttribute('type') != null && x[i].getAttribute('type').toLowerCase() == "hidden")
        continue;
      if(x[i].style != null && x[i].style.visibility != null && x[i].style.visibility.toLowerCase() == 'hidden')
        continue;
      if(x[i].style != null && x[i].style.display != null && x[i].style.display.toLowerCase() == 'none')
        continue;
      if(x[i].tagName.toLowerCase() == "a" && x[i].getAttribute('disabled') != null)
        continue;
        
      //Se chegou até aqui é porque o elemento pode ser ativado
      x[i].focus();
      break;
    }
  }
}

function SaveFocus() {
  if(document.forms[0].elements['__lastfocus']) {
    var vid = document.FocusElement ? document.FocusElement.id : "";
    var vtag = document.FocusElement ? document.FocusElement.tagName : "";
    
    var sX = 0, sY = 0;
    if(window.pageXOffset != null) {
      sX = window.pageXOffset;
      sY = window.pageYOffset;
    }
    else if(document.body.scrollLeft != null) {
      sX = document.body.scrollLeft;
      sY = document.body.scrollTop;
    }
    
    document.forms[0].elements['__lastfocus'].value = vid + "," + sX + "," + sY + "," + vtag;
  }
}
