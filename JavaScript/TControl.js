function TControl_BrowserInfo()
{
  var browser=new Object();
  browser.browser  = '';
  browser.version  = 0;
  var i;
  var uagent = window.navigator.userAgent.toLowerCase();
  
  if (uagent .indexOf('opera') != -1)
  {
    i = uagent .indexOf('opera');
    browser.browser  = 'opera';
    browser.version = Math.floor(parseFloat('0' + uagent.substr(i+6), 10));
  }
  else if (uagent.indexOf('msie') != -1)
  {
    i = uagent.indexOf('msie');
    browser.browser  = 'IE';
    browser.version = Math.floor(parseFloat('0' + uagent.substr(i+5), 10));
  }
  else if(uagent.indexOf('gecko') !=-1)
  {
  browser.browser="NS";
    browser.version  = 6;
  }
  else if(document.layers)
  {
  browser.browser="NS";
    browser.version  = 4;
  }
  else
  {
  browser.browser="other";
    browser.version  = 0;
  }
  
  return browser;  
}

function TControl_getElementById(el)
{
  var bi=TControl_BrowserInfo();
  if (bi.browser=="IE")
    return document.all[el];
  else if ((bi.browser=="NS" && bi.version>4) || bi.browser=="opera")
    return document.getElementById(el);
  else if (bi.browser=="NS" && bi.version==4)
    return document.layers[el];
  else
    return new object;
}

function TControl_elementTop(eSrc)
{
  var iTop = 0;
  var eParent;
  var bi=TControl_BrowserInfo();
  if(bi.browser!="other" && !(bi.browser=="NS" && bi.version<5))
    {
    eParent = eSrc;
    while (eParent.tagName.toUpperCase() != "BODY")
    {
      iTop += eParent.offsetTop;
      eParent = eParent.offsetParent;
      if (eParent == null)
        break;
    }
  }
  else if(bi.browser=="NS" && bi.version<5)
  {
    if(eSrc.pageY)
    {
      iTop=eSrc.pageY;
    }
    else if(eSrc.y)
    {
      iTop=eSrc.y;
    }
  }
  return iTop;

}
function TControl_elementLeft(eSrc)
{  
  var iLeft = 0;
  var eParent;
  var bi=TControl_BrowserInfo();
  if(bi.browser!="other" && !(bi.browser=="NS" && bi.version<5))
    {
    eParent = eSrc;
    while (eParent.tagName.toUpperCase() != "BODY")
    {
      iLeft += eParent.offsetLeft;
      eParent = eParent.offsetParent;
      if (eParent == null)
        break;
    }
  }
  else if(bi.browser=="NS" && bi.version<5)
  {
    if(eSrc.pageX)
    {
      iLeft=eSrc.pageX;
    }
    else if(eSrc.x)
    {
      iLeft=eSrc.x;
    }
  }
  return iLeft;
}

function TControl_elementBottom(obj)
{
  var top,bottom=0,obj;
  top=TControl_elementTop(obj);
  var bi=TControl_BrowserInfo();
  if(bi.browser!="other" && !(bi.browser=="NS" && bi.version<5))
    {
    if(obj.offsetHeight)
      bottom=top+obj.offsetHeight;
    else if(obj.style.pixelHeight)
      bottom=top+obj.style.pixelHeight;
  }
  else if(document.layers)
  {
    if(obj.clip)
    {
      bottom=top+obj.clip.height;
    }
    else
    {
      bottom=top+20;
    }
  }
  
  return(bottom);
  
}

function TControl_elementRight(obj)
{
  var left,right=0,obj;
  left=TControl_elementLeft(obj);
  var bi=TControl_BrowserInfo();
  if(bi.browser!="other" && !(bi.browser=="NS" && bi.version<5))
    {
    if(obj.offsetWidth)
      right=left+obj.offsetWidth;
    else if(obj.style.pixelWidth)
      right=left+obj.style.pixelWidth;
  }
  else if(document.layers)
  {
    if(objID.clip)
    {
      right=left+obj.clip.width;
    }
    else
    {
      right=left+20;
    }
  }
  return(right);
}

function TControl_setPosition(objid,x,y)
{
  var obj;
  obj=TControl_getElementById(objid);
  if (obj) 
  {
    var bi = TControl_BrowserInfo();
    if (bi.browser == "IE") {
        if (obj.style) {
            obj.style.left = x;
            obj.style.top = y;
        }
        else if (obj.left) {
            obj.left = x;
            obj.top = y;
        }
    }
    else {
        if (document.getBoxObjectFor) {
            obj.style.left = x+"px";
            obj.style.top = y + "px";
        }
        else 
        {
            obj.style.pixelLeft = x + document.documentElement.scrollTop;
            obj.style.pixelTop = y + document.documentElement.scrollLeft;
        } 
    }
  }
}

var TControl_hiddenElements=new Array();

function TControl_ShowElement(id, visible)
{
  var i,j,k,listobj,obj,listobjstyle,tags,tagid,added,newhidden,auxarray;
  var bi;
  bi=TControl_BrowserInfo();

  
  if(bi.browser=="NS" && bi.version<5)
  {
    listobj=document.layers[id];
    listobjstyle=listobj;
  }
  else if(bi.browser=="IE" && bi.version==4)
  {
    listobj=document.all[id];
    listobjstyle=((listobj && listobj.style)?listobj.style:null);
  }
  else if(bi.browser!="other")
  {
    listobj=document.getElementById(id);
    listobjstyle=((listobj && listobj.style)?listobj.style:null);
  }
    
  if(listobj){} else return;
    
  if(visible==true && listobjstyle.visibility!="hidden")
    return;
  else if(visible==false && listobjstyle.visibility=="hidden")
    return;
  
  if(visible==true)
    listobjstyle.visibility="visible";
  else
    listobjstyle.visibility="hidden";
    
  if(bi.browser!="IE" && bi.browser!="opera")
    return;
  if(bi.browser=="IE")
    tags=new Array("SELECT","OBJECT");
  else
    tags=new Array("SELECT","INPUT");
  for(j=0;j<tags.length;j++)
  {
    tagid=tags[j];
    for (i = 0; i < document.getElementsByTagName(tagid).length; i++)
    {
      obj = document.getElementsByTagName(tagid)[i];
      if (TControl_elementTop(obj) > parseInt(listobj.style.top) + listobj.offsetHeight)
      {}
      else if (TControl_elementLeft(obj) > parseInt(listobj.style.left) + listobj.offsetWidth)
      {}
      else if (TControl_elementLeft(obj) + obj.offsetWidth < parseInt(listobj.style.left))
      {}
      else if (TControl_elementTop(obj) + obj.offsetHeight < parseInt(listobj.style.top))
      {}
      else
      {
        added=false;
        if(visible==true && listobj.style.visibility!="hidden")
        {
          for(k=0;k<TControl_hiddenElements.length;k++)
          {
            if(TControl_hiddenElements[k].obj==obj)
            {
                TControl_hiddenElements[k].count++;
                added=true;
              break;
            }
          }
          if(added==false)
          {
             obj.style.visibility = "hidden";
            newhidden=new Object();
            newhidden.obj=obj;
            newhidden.count=1;
            if(typeof(TControl_hiddenElements.push)!='undefined')
              TControl_hiddenElements.push(newhidden);
            else
              TControl_hiddenElements[TControl_hiddenElements.length]=newhidden;
          }
        }
        else if(visible==false && listobj.style.visibility=="hidden")
        {
          for(k=0;k<TControl_hiddenElements.length;k++)
          {
            if(TControl_hiddenElements[k].obj==obj)
            {
                TControl_hiddenElements[k].count--;
                if(TControl_hiddenElements[k].count==0)
                {
                  if(typeof(TControl_hiddenElements.splice)!='undefined')
                    auxarray=TControl_hiddenElements.splice(k,1);
                  else
                  {
                    var ll;
                    auxarray=new Array();
                    for(ll=0;ll<TControl_hiddenElements.length;ll++)
                      if(ll!=k)
                        auxarray[auxarray.length]=TControl_hiddenElements[k];
                    TControl_hiddenElements=auxarray;
                  }
                  obj.style.visibility = "visible";
                }
              break;
            }
          }
        }
      }
    }
  }
}

function ShowTSearch(id, visible) {
  var listid = id + '_list';
  var argobj = TControl_getElementById(id);
  var top = TControl_elementBottom(argobj); if(TControl_BrowserInfo().browser == "opera") top++;
  var left = TControl_elementLeft(argobj) + 1;

  TControl_setPosition(listid, left, top);
  TControl_ShowElement(listid, visible);
}

var listTSearch = new Array();
function ClearTSearch(id, initvalue, initarg) {
  var tsearch = null;
  
  var i;
  for(i = 0; i < listTSearch.length; i++) {
    if(listTSearch[i].id == id) {
      tsearch = listTSearch[i];
      break;
    }
  }
  
  if(tsearch == null) {
    tsearch = new Object();
    tsearch.id = id;
    tsearch.cleared = false;
    listTSearch.push(tsearch);
  }
  
  if(!tsearch.cleared) {
    if(document.forms[0].elements[tsearch.id].value != '' && document.forms[0].elements[tsearch.id].value != initvalue)
      document.forms[0].elements[tsearch.id + '___Argument__'].value = '';
    else if(document.forms[0].elements[tsearch.id + '___Argument__'].value != initarg)
      document.forms[0].elements[tsearch.id].value = '';
    tsearch.cleared = true;
  }
}

function TControl_OnSubmitValidate(rmid) {
  var rm = document.getElementById(rmid);
  var vc;
  if(rm != null) {
    vc = rm.getAttribute('validatedcontrols');
    if(vc != null) vc = vc.split(',');
  }
  
  var haserror, hasmessage;
  if(rm != null && vc != null && vc.length) {
    for(var i = 0; i < vc.length; i++) {
      haserror = haserror || TControl_CheckError(vc[i]);
      TControl_DisplayError(vc[i]);
    }
    hasmessage = TControl_DisplaySummary(rmid);
  }
  
  return !(haserror && hasmessage);
}

function TControl_DisplaySummary(rmid)
{
  var c,rm,i,haserror,errlist,vc,showmessagebox,hasmessage;
  var cerror=null;
  
  hasmessage=false;
  rm=document.getElementById(rmid);
  if(rm!=null)
  {
    showmessagebox=rm.getAttribute('showmessagebox');
    vc=rm.getAttribute('validatedcontrols');
    if(vc!=null) vc=vc.split(',');
    
    if(vc!=null && vc.length)
    {
      if(showmessagebox=="true")
      {
        errlist="";
        for(i=0;i<vc.length;i++)
        {
          c=document.getElementById(vc[i]);
          if(c && c.error)
          {
            errlist+="- "+(c.fieldname?c.fieldname+": ":"")+c.error+"\n";
            if(cerror==null) cerror=c;
          }
        }
        if(errlist!="")
        {
          alert(errlist);
          if(cerror!=null && typeof(cerror.focus)!="undefined")
            cerror.focus();
          if(cerror!=null && typeof(cerror.select)!="undefined")
            cerror.select();
          hasmessage=true;
        }
      }
    }
  }
  return hasmessage;
}

function TControl_OnChangeValidate(id) {
  TControl_CheckError(id);
  TControl_DisplayError(id);
}

function TControl_DisplayError(id) {
  var ctl = document.getElementById(id);
  if(ctl == null) return;
  
  showMsg(ctl, ctl.error);
}

function TControl_CheckError(id) {
  var ctl = document.getElementById(id);
  if(ctl == null) return false;
  
  ctl.error = checkForErrors(ctl);
  return ctl.error != null;
}

// Recebe um controle e devolve mensagem de erro caso haja algum erro no valor contido nele.
// Devolve null se o valor está ok.
function checkForErrors(ctl) {
  var required = ctl.getAttribute("required");
  var datatype = ctl.getAttribute("datatype");
  if(datatype == null) datatype = 'string';
  
  var result = getDBValue(ctl.id);
  if(result[0] != null) return result[0];
  var cval = result[1];
  
  if(cval == null || typeof cval == "string" && cval == "") {
    if(required == 'true')
      return TControl_ValidationMessages.RequiredFieldMsg;
  }
  
  else if(typeof cval != "string" && isNaN(cval)) {
    if(datatype == 'date')
      return "Data inv\u00e1lida"
    else
      return "Valor inv\u00e1lido";
  }
    
  else if(datatype != null) {
    var dateformat = ctl.getAttribute("dateformat");
    var minimumvalue = ctl.getAttribute("minimumvalue");
    var maximumvalue = ctl.getAttribute("maximumvalue");
    
    if(minimumvalue != null || maximumvalue != null) {
      var fieldname = ctl.getAttribute("fieldname");
    
      var msgformat;
      if(minimumvalue != null && maximumvalue != null)
        msgformat = TControl_ValidationMessages.RangeValueMsg;
      else if(minimumvalue != null)
        msgformat = TControl_ValidationMessages.MinValueMsg;
      else
        msgformat = TControl_ValidationMessages.MaxValueMsg;
      msgformat = msgformat.replace('{0}', fieldname == null ? 'Valor' : fieldname).replace('{1}', minimumvalue).replace('{2}', maximumvalue);
      
      var cmin = tconvert(minimumvalue, datatype, TControl_CultureInfo.dateorder, TControl_CultureInfo.decimalchar, TControl_CultureInfo.groupchar, null);
      var cmax = tconvert(maximumvalue, datatype, TControl_CultureInfo.dateorder, TControl_CultureInfo.decimalchar, TControl_CultureInfo.groupchar, null);
      if((cmin != null && cval < cmin) || (cmax != null && cval > cmax))
        return msgformat;
    }
    
    if(datatype == 'double' || datatype == 'integer' || datatype == 'currency') {
      var prec = parseInt(ctl.getAttribute("Prec"));
      var scal = parseInt(ctl.getAttribute("Scal"));
      var min = parseInt(ctl.getAttribute("MinLen"));
      return verifNum(cval, isNaN(min) ? 0 : min, isNaN(prec) ? -1 : prec, isNaN(scal) ? -1 : scal);
    }
    else if(datatype == 'string') {
      var max = parseInt(ctl.getAttribute("maxlength"));
      var min = parseInt(ctl.getAttribute("MinLen"));
      return verifStr(cval, isNaN(min) ? 0 : min, isNaN(max) ? 0 : max);
    }
    else if(datatype == 'date') {
      if(cval.getYear() < 1 || cval.getYear() > 9999)
        return "Ano inv\u00e1lido";
    }
  }
    
  return null;
}

// Parseia strValue devolvendo um array com três posições:
// [0]: sinal
// [1]: parte inteira
// [2]: parte fracionária
function parseDoubleStr(strValue, decimalChar, groupChar) {
  var exp = new RegExp("^\\s*" +
  // 1      1
    "([-\\+])?" +
  //     2
    "\\s*(" +
  //   3        3 4                   5         54
      "(\\d{1,3})?(\\" + groupChar + "(\\d\\d\\d))+" + // Parte inteira com separadores de milhar
    "|" +
      "\\d*" + // Parte inteira sem separadores de milhar
  // 2
    ")" +
  // 6                     7    76
    "(\\" + decimalChar + "(\\d+))?" + // Parte fracionária
  "\\s*$");
  
  var match = strValue.match(exp);
  if(match == null) return null;
  
  return new Array(
    match[1] == null ? "" : match[1],
    match[2].replace(new RegExp("\\" + groupChar, "g"), ""),
    match[7] == null ? "" : match[7]
  );
}

// Converte uma string (strValue) para o tipo (dataType) informado.
// O parâmetro dateOrder deve ser "ymd", "dmy" ou "mdy".
// O parâmetro decimalChar normalmente é "." ou ",".
// Devolve null se a conversão não for possível ou se o parâmetro de entrada (strValue) for null.
// Devolve Array(msgerro, valor)
function tconvert(strValue, dataType, dateOrder, decimalChar, groupChar, dateformat) {
  function GetFullYear(year) {
    return year + parseInt(TControl_CultureInfo.century) - (year < TControl_CultureInfo.cutoffyear ? 0 : 100);
  }
  
  if(strValue == null || strValue == "") return new Array(null, strValue);
    
  if(dataType == "integer") {
    var expr = new RegExp("^\\s*[-\\+]?\\d+\\s*$");
    if(strValue.match(expr) != null) {
      var num = parseInt(strValue);
      if(!isNaN(num))
        return new Array(null, num);
    }
    return new Array("Formato incorreto para n\u00famero inteiro", null);
  }
  
  else if(dataType == "double") {
    var match = parseDoubleStr(strValue, decimalChar, groupChar);
    if(match != null) {
      var cleanInput = match[0] + ((match[1] != null && match[1].length > 0) ? match[1] : "0") + "." + match[2];
      var num = parseFloat(cleanInput);
      if(!isNaN(num))
        return new Array(null, num);
    }
    return new Array("Formato incorreto para n\u00famero", null);
  } 

  else if(dataType == "date" && dateformat!="M/yyyy") {
    var day, month, year;
    var hour, min, sec;
    
    //              8     9  0     1   2
    var timeExpr = "(\\d+)(:)(\\d+)(\\9(\\d+))?";
    
    if(dateOrder == "ymd") {
      //              12        3      4      5        6
      var dateExpr = "((\\d{2})|(\\d+))([-./])(\\d+)\\4(\\d+)";
      var expr = new RegExp("^\\s*" + dateExpr + "\\s*(" + timeExpr + ")?\\s*$");
      var match = strValue.match(expr);

      if(match == null) return new Array("Formato incorreto para data (yyyy/mm/dd)", null);
      
      day = match[6];
      month = match[5];
      year = (match[2] != null && match[2].length == 2)? GetFullYear(parseInt(match[2])) : match[3];
    }
    else {
      //              1     2      3        45        6
      var dateExpr = "(\\d+)([-./])(\\d+)\\2((\\d{2})|(\\d+))";
      var expr = new RegExp("^\\s*" + dateExpr + "\\s*(" + timeExpr + ")?\\s*$");
      match = strValue.match(expr);
      if(match == null) return new Array("Formato incorreto para data (" + (dateOrder == "mdy" ? "mm/dd/yyyy" : "dd/mm/yyyy") + ")", null);
      
      if(dateOrder == "mdy") {
        day = match[3];
        month = match[1] - 1;
      }
      else { // dmy
        day = match[1];
        month = match[3] - 1;
      }
      year = (match[5] != null && match[5].length == 2) ? GetFullYear(parseInt(match[5])) : match[6];
    }
    
    // No Mozilla, new Date() não aceita nulos em hour, min e sec
    hour = match[8]; if(hour == null) hour = 0;
    min = match[10]; if(min == null) min = 0;
    sec = match[12]; if(sec == null) sec = 0;

    return new Array(null, new Date(year, month, day, hour, min, sec));
  }
  else if(dataType == "date" && dateformat=="M/yyyy") {
    var day, month, year;
    //              1         2      3
    var dateExpr = "(\\d{1,2})([-./])((\\d{4})|(\\d{2}))";
    var expr = new RegExp("^\\s*" + dateExpr + "\\s*$");
    var match = strValue.match(expr);

    if(match == null) return new Array("Formato incorreto para data (mm/yyyy)", null);
      
    day = 1;
    month = match[1] - 1;
    year = (match[3]!=null && match[3].length == 4) ? match[3] : GetFullYear(parseInt(match[3]))

    return new Array(null, new Date(year, month, day));
  }  
  else
    return new Array(null, strValue);
}

// Converte um valor (dbValue) do tipo (dataType) para string.
// Se (dbValue) não for do tipo (dataType), retorna string vazia
// O parâmetro dateOrder deve ser "ymd", "dmy" ou "mdy".
// O parâmetro decimalChar normalmente é "." ou ",".
function ttostring(dbValue, dataType, dateOrder, decimalChar) {
  var str;  
  if(dbValue == null) return '';
    
  if(dataType == "integer") 
  {
    if(typeof(dbValue)!='number') return '';
    str=String(Math.ceil(dbValue));
    return str;
  }
  
  else if(dataType == "double") 
  {
    if(typeof(dbValue)!='number') return '';
    str=String(dbValue).replace(/\./,decimalChar);
    return str;
  } 

  else if(dataType == "date") 
  {
    if(!(dbValue instanceof Date)) return '';
    if(dateOrder == "ymd") 
    {
      str=dbValue.getFullYear()+"/"+(dbValue.getMonth()+1)+"/"+dbValue.getDate();
      return str;
    }
    else if(dateOrder == "mdy") 
    {
      str=(dbValue.getMonth()+1)+"/"+dbValue.getDate()+"/"+dbValue.getFullYear();
      return str;
    }
    else 
    { // dmy
      str=dbValue.getDate()+"/"+(dbValue.getMonth()+1)+"/"+dbValue.getFullYear();
      return str;
    }
  }
  else //string
    return dbValue;
}

function TControl_Trim(text)
{
  var i,j,r;
  if(typeof(text)!="string" || text=='') return '';
  for(i=0;i<text.length && text[i]==' ';i++);
  for(j=text.length-1;j>-1 && text[j]==' ';j--);
  if(i>j)
    return ''; 
  else 
    return text.substring(i,j+1);
}

var TControl_CultureInfo = new Object();
TControl_CultureInfo.decimalchar = ',';
TControl_CultureInfo.groupchar = '.';
TControl_CultureInfo.dateorder = 'dmy';
TControl_CultureInfo.century = 2000;
TControl_CultureInfo.cutoffyear = 50;
TControl_CultureInfo.currencydigits = 2;
TControl_CultureInfo.daynames = new Array("Domingo", "Segunda", "Ter\u00e7a", "Quarta", "Quinta", "Sexta", "S\u00e1bado");
TControl_CultureInfo.abbreviateddaynames = [ 'Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'S\u00e1b' ];
TControl_CultureInfo.monthnames = [ 'Janeiro', 'Fevereiro', 'Mar\u00e7o', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro' ];
TControl_CultureInfo.abbreviatedmonthnames = [ 'Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez' ];
TControl_CultureInfo.todayname = 'Hoje'
  
var TControl_ValidationMessages = new Object();
TControl_ValidationMessages.RequiredFieldMsg = 'Preenchimento obrigat\u00f3rio.';
TControl_ValidationMessages.InvalidDateMsg = 'Data inv\u00e1lida.';
TControl_ValidationMessages.InvalidDoubleMsg = 'N\u00famero inv\u00e1lido.';
TControl_ValidationMessages.InvalidIntegerMsg = 'Inteiro inv\u00e1lido.';
TControl_ValidationMessages.InvalidCurrencyMsg = 'Quantia inv\u00e1lida.';
TControl_ValidationMessages.MinValueMsg = '{0} deve ser igual ou superior a {1}.';
TControl_ValidationMessages.MaxValueMsg = '{0} deve ser igual ou inferior a {2}.';
TControl_ValidationMessages.RangeValueMsg = '{0} deve estar entre {1} e {2}.';
TControl_ValidationMessages.MaxPrec = 'O n\u00famero pode conter no m\u00e1ximo {0} d\u00edgito{1}.';
TControl_ValidationMessages.NoScale = 'O n\u00famero n\u00e3o pode conter casas decimais.';
TControl_ValidationMessages.MaxScale = 'O n\u00famero pode conter no m\u00e1ximo {0} casa{1} decima{2}.';
TControl_ValidationMessages.NumMinLen = 'O n\u00famero deve conter no m\u00ednimo {0} d\u00edgito{1}.';
TControl_ValidationMessages.StrMinLen = 'O campo deve conter no m\u00ednimo {0} caractere{1}.';
TControl_ValidationMessages.StrMaxLen = 'O campo pode conter no m\u00e1ximo {0} caractere{1}.';

function TDBItem_OnMouseOver(button)
{
  var pop,top,left,urlico,urlhico,ico;
  
  urlico=button.getAttribute("icon");
  urlhico=button.getAttribute("hovericon");
  ico=button.id;
  if(ico!=null) ico=document.getElementById(ico)
  if(ico!=null && urlhico!=null && urlico!=null )
  {
    ico.src=urlhico;
  }

  pop=document.getElementById(button.id+"_popupwindow");
  if(pop!=null && pop.style)
  {
    top=TControl_elementBottom(button)+10;
    left=TControl_elementLeft(button);
    TControl_setPosition(pop.id,left,top);
    TControl_ShowElement(pop.id,true);
  }
}

function TDBItem_OnMouseOut(button)
{
  var i,pop,top,left;
  var urlico,urlhico,ico;
  
  button.style.cursor="hand";
  urlico=button.getAttribute("icon");
  urlhico=button.getAttribute("hovericon");
  ico=button.id;
  if(ico!=null) ico=document.getElementById(ico)
  if(ico!=null && urlhico!=null && urlico!=null )
  {
    ico.src=urlico;
  }
  if(typeof(event)!="undefined")
  {
    if(event.toElement==button)
      return;
    for(i=0;i<button.childNodes.length;i++)
      if(event.toElement==button.childNodes[i])
        return;
  }
  pop=document.getElementById(button.id+"_popupwindow");
  if(pop!=null && pop.style)
  {
    TControl_ShowElement(pop.id,false);
  }
}

// Altera o href de um anchor <a> gerado a partir de THyperLink, colocando
// o valor dos parâmetros adequadamente. Chamado no OnClick() de <a>.
function substTLinkBaseHref(id) {
  var lnk = document.getElementById(id);
  var params = lnk.getAttribute("params");
  var values = lnk.getAttribute("values");
  var fixParams = lnk.getAttribute("fixParams");
  var returnEnabled = lnk.getAttribute("return");
  
  var queryString = "";
  
  if(params.length > 0) {
    var arrayParams = params.split(",");
    var arrayValues = values.split(",");
    
    for(i = 0; i < arrayParams.length; i++) {
      var value;
      if(arrayValues[i].charAt(0) == '#' && arrayValues[i].charAt(arrayValues[i].length - 1) == '#') {
        var ctlName = arrayValues[i].substr(1, arrayValues[i].length - 2);
        var result = getDBValue(ctlName);
        var dbValue = result[0] == null ? result[1] : null;
        value = toStringInvariant(dbValue);
      }
      else
        value = arrayValues[i];
        
      if(queryString.length > 0) queryString = queryString.concat("&");
      queryString = queryString.concat(arrayParams[i] + "=" + value);
    }
  
    var method = lnk.getAttribute("method");
    if(method != null) {
      if(queryString.length > 0) queryString = queryString.concat("&");
      queryString = queryString.concat("method=" + method);
    }

    var types = lnk.getAttribute("types");
    if(types != null) {
      if(queryString.length > 0) queryString = queryString.concat("&");
      queryString = queryString.concat("types=" + types);
    }
  }
  
  if(fixParams != null) {
    if(queryString.length > 0) queryString = queryString.concat("&");
    queryString = queryString.concat(fixParams);
  }
  
  if(returnEnabled == "True") {
    var returnUrl = document.forms[0].elements["_myUrl"];
    if(returnUrl.value == undefined) // Deve ter encontrado mais de um _myUrl.
      returnUrl = returnUrl[0];
    
    if(returnUrl != undefined) {
      if(queryString.length > 0) queryString = queryString.concat("&");
      queryString = queryString.concat("returnUrl=" + returnUrl.value);
    }
  }
  
  lnk.search = queryString.length == 0 ? '' : '?' + queryString;
}

// Obtém o valor tipado de um TControl.
// A propriedade DataTypeValidation deve estar setada para True.
// Devolve Array(msgerro, valor)
function getDBValue(id) {
  var ctl = document.getElementById(id);
  var dataType = ctl.getAttribute("datatype");
  var attr = ctl.getAttribute("DBValue");
  var dateformat = ctl.getAttribute("dateformat");
  
  if(attr == null) {
    // Caso o controle seja checkbox, usa ctl.checked ao invés de ctl.value.
    var checkBoxValue = ctl.type.toLowerCase() == "checkbox" ? ctl.checked : null;
    var controlValue = checkBoxValue == null ? ctl.value : (checkBoxValue ? "S" : "N");
    if(ctl.type.toLowerCase() == "select-one")
      // Valores especiais de dropdown's são tratados à parte.
      if(controlValue == "__ALL__")
        return new Array(null, controlValue);
      else if(controlValue == "__NULL__")
        return new Array(null, "");

    return tconvert(controlValue, dataType, TControl_CultureInfo.dateorder, TControl_CultureInfo.decimalchar, TControl_CultureInfo.groupchar, dateformat);
  }
  else
    // O atributo DBValue sempre estará no InvariantCulture
    return tconvert(attr, dataType, "mdy", ".", ",", null);
}

function setDBValue(id,value) {
  var ctl = document.getElementById(id);
  var dataType = ctl.getAttribute("datatype");
  var attr = ctl.getAttribute("DBValue");
  
  if(attr != null)
    ctl.setAttribute("DBValue",ttostring(value, dataType, "mdy", "."));
  if(ctl.value != null)
    ctl.value=ttostring(value, dataType, TControl_CultureInfo.dateorder, TControl_CultureInfo.decimalchar);
}

// Converte um valor para string no formato que Convert.ChangeType(InvariantCulture) entenda.
function toStringInvariant(value) {
  if(value == null)
    return "";
  else if(value instanceof Date)
    return (value.getMonth() + 1) + "/" + value.getDate() + "/" + value.getFullYear() + " " +
           value.getHours() + ":" + value.getMinutes() + ":" + value.getSeconds()
  else
    return value.toString();
}

function verifStr(value, lenMin, lenMax) {
  if(lenMin > 0 && value.length < lenMin)
    return TControl_ValidationMessages.StrMinLen.replace('{0}', lenMin.toString()).replace('{1}', lenMin > 1 ? 's' : '');

  if(lenMax > 0 && value.length > lenMax)
    return TControl_ValidationMessages.StrMaxLen.replace('{0}', lenMax.toString()).replace('{1}', lenMax > 1 ? 's' : '');
}

function verifNum(value, lenMin, precision, scale) {
  // toString() usa formato de cultura invariante
  var match = parseDoubleStr(value.toString(), ".", ",");
  if(match == null)
    return TControl_ValidationMessages.InvalidDoubleMsg;
  
  else {
    var intLen = parseInt(match[1]) == 0 ? 0 : match[1].toString().length;
    var fracLen = parseInt(match[2]) == 0 ? 0 : match[2].toString().length;

    if(precision >= 0 && intLen + fracLen > precision)
      return TControl_ValidationMessages.MaxPrec.replace('{0}', precision.toString()).replace('{1}', precision > 1 ? 's' : '');
    else if(scale >= 0 && fracLen > scale)
      if(scale == 0)
        return TControl_ValidationMessages.NoScale;
      else
        return TControl_ValidationMessages.MaxScale.replace('{0}', scale.toString()).replace('{1}', scale > 1 ? 's' : '').replace('{2}', scale > 1 ? 'is' : 'l');

    if(lenMin > 0 && value.toString().length < lenMin)
      return TControl_ValidationMessages.NumMinLen.replace('{0}', lenMin.toString()).replace('{1}', lenMin > 1 ? 's' : '');
  }
}

function setImage(button, imageurl) {
	if(button.getAttribute('src') != null)
		button.setAttribute('src', imageurl);
		
	else {
	  var filhos = null;
		if(button.children)
			filhos = button.children;
		else if(button.childNodes)
			filhos = button.childNodes;
			
		if(filhos != null && filhos.length > 0) {
		  var i = 0;
		  while(i < filhos.length && filhos[i].getAttribute('src') == null) i++;
      if(i < filhos.length) filhos[i].setAttribute('src', imageurl);
		}
	}
}

function TC_tooltip(id,show)
{
  var text,ttimg,ttelem,x,y;
  ttelem=TControl_getElementById("__tooltip__");
  if(ttelem==null)
  {
    ttelem = document.createElement("div");
    ttelem.id="__tooltip__";
    ttelem.style.position="absolute";
		ttelem.style.border="solid 1px black";
		ttelem.style.padding="1px 1px 1px 1px";
		ttelem.style.background="#FFFFEE";
		ttelem.style.color="black";
		ttelem.style.width="auto";
		ttelem.style.zIndex="800";
		ttelem.style.font="normal normal normal 8pt normal Arial";
		ttelem.style.zIndex="800";
		document.body.appendChild(ttelem);
  };
  
  ttimg=TControl_getElementById(id);
  if(ttimg==null) return;
  text=ttimg.getAttribute("tooltip");
  if(text==null || text=="") return;
 
  x=TControl_elementLeft(ttimg);
  y=4+TControl_elementBottom(ttimg);
  
	ttelem.innerHTML=text;

  ttelem.style.position="absolute";
  TControl_setPosition("__tooltip__",x,y);
	TControl_ShowElement("__tooltip__", show);
	
}

function showMsg(element, message) {
  if(element == null) return;
  
  if(message == null || message.length == 0) {
    hideMsg(element);
    return;
  }
  
  var msg = document.getElementById(element.id + "_msg");
  if(msg == null) {
    var controlMessageType = getAttributeString(element, "ControlMessageType");
    if(controlMessageType == "Icon") {
      msg = document.createElement("IMG");
      var imageUrl = getAttributeString(element, "MessageImageUrl");
      msg.src = imageUrl != null ? imageUrl : MessageImageUrl_Def;
    }
    else
      msg = document.createElement("SPAN");
    msg.id = element.id + "_msg";
    
    element.parentNode.insertBefore(msg, element.nextSibling);
  }
  
  msg.style.visibility = "visible";
  msg.style.display = "inline";
  
  if(msg.tagName == "IMG")
    msg.title = message;
  else
    msg.innerHTML = message;
}

function hideMsg(element) {
  if(element == null) return;
  
  var msg = document.getElementById(element.id + "_msg");
  if(msg == null) return;
  
  msg.style.visibility = "hidden";
  msg.style.display = "none";
  
  if(msg.tagName == "IMG")
    msg.title = "";
  else
    msg.innerHTML = "";
}

function getAttributeString(element, attributeName) {
  var att = null;
  if(element.attributes.getNamedItem == null) // FF
    att = element.attributes[attributeName];
  else // IE
    att = element.attributes.getNamedItem(attributeName);
    
  return att != null ? att.value : null;
}
