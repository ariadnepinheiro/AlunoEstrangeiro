var __KIL = new Array(0);
 
function __Help() {
  var page = document.location.pathname + '?help';
  var width = screen.availWidth - 100;
  var height = screen.availHeight - 150;
  var left = screen.availLeft ? screen.availLeft + 50 : 50;
  var top = screen.availTop ? screen.availTop + 20 : 20;
  var hw = window.open(page, 'help',
                       'resizable=yes, location=no, scrollbars=yes, status=yes, toolbar=yes, ' +
                       'width=' + width + ', height=' + height + ', screenY=' + top + ', screenX=' + left + ', top=' + top + ', left=' + left);
  hw.focus();
}

function __ki(key,url) {
  var f=new Object();
  f.key=key;
  f.url=url;
  __KIL.push(f);
}

function __dockey(e) {
  if(e.keyCode==113 || e.keyCode==112)
    __Help();
    
  if(e.shiftKey & e.ctrlKey)
    for(var i=0;i<__KIL.length;i++)
      if(__KIL[i].key==e.keyCode) {
        window.location.href=__KIL[i].url;
        if(e.preventDefault) e.preventDefault();
        if(e.stopPropagation) e.stopPropagation();
        break;
      }
}

function __F1(e) {
  __Help();
  return false;
}

function navega_apl(url) {
  if(self.opener != null)
    self.opener.location.href = url;
}

function BrowserInfo() {
  var browser = new Object();
  browser.browser = '';
  browser.version = 0;
  
  var uagent = window.navigator.userAgent.toLowerCase();
  
  if(uagent.indexOf('opera') != -1) {
    var i = uagent.indexOf('opera');
    browser.browser = 'opera';
    browser.version = Math.floor(parseFloat('0' + uagent.substr(i + 6), 10));
  }
  else if(uagent.indexOf('msie') != -1) {
    var i = uagent.indexOf('msie');
    browser.browser = 'IE';
    browser.version = Math.floor(parseFloat('0' + uagent.substr(i + 5), 10));
  }
  else if(uagent.indexOf('gecko') != -1) {
    browser.browser = "NS";
    browser.version = 6;
  }
  else if(document.layers) {
    browser.browser = "NS";
    browser.version = 4;
  }
  else {
    browser.browser = "other";
    browser.version = 0;
  }
  
  return browser;  
}

function getElementById(el) {
  var bi = BrowserInfo();
  
  if(bi.browser == "IE")
    return document.all[el];
    
  else if(bi.browser == "NS" && bi.version > 4 ||
          bi.browser == "opera")
    return document.getElementById(el);
    
  else if(bi.browser == "NS" && bi.version == 4)
    return document.layers[el];
    
  else
    return new object;
}

function toggleVisibility(id) {
  var obj = getElementById(id);
  if(obj.style.visibility.toLowerCase() == "hidden")
    obj.style.visibility = "visible";
  else
    obj.style.visibility = "hidden";
}

function setVisibility(id, visible) {
  var obj = getElementById(id);
  if(visible)
    obj.style.visibility = "visible";
  else
    obj.style.visibility = "hidden";
}

if(document.addEventListener) {
  document.addEventListener('keyup',__dockey,true);
  document.addEventListener('help',__F1,true);
}

if(document.attachEvent) {
  document.attachEvent('onkeyup',__dockey);
  document.attachEvent('onhelp',__F1);
}
