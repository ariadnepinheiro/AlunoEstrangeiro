

function TDateBox_AttachEvents(id)
{
	if(typeof(window.captureEvents)!='undefined')
		window.captureEvents(Event.CLICK);
	if(typeof(document.attachEvent)!='undefined')
	{
		document.getElementById(id).attachEvent('onkeypress',TDateBox_OnKeyPress);
		document.getElementById(id).attachEvent('onkeyup',TDateBox_OnKeyUp);
		document.getElementById(id).attachEvent('onblur',TDateBox_OnBlur);
		document.getElementById(id+"_btn").attachEvent('onclick',TDateBox_OnButtonClick);
		document.getElementById(id+"_cld").attachEvent('onclick',new Function("TDateBox_OnCalendarClick('"+id+"',window.event);"));
		document.attachEvent('onclick',new Function("TDateBox_OnDocumentClick('"+id+"',window.event);"));
	}
	else if(typeof(document.addEventListener)!='undefined')
	{
		document.addEventListener('click',new Function("e","TDateBox_OnDocumentClick('"+id+"',e);"),true);
		document.getElementById(id+"_cld").addEventListener('click',new Function("e","TDateBox_OnCalendarClick('"+id+"',e);"),true);
		document.getElementById(id+"_btn").addEventListener('click',TDateBox_OnButtonClick,true);
		document.getElementById(id).addEventListener('blur',TDateBox_OnBlur,true);
		document.getElementById(id).addEventListener('keyup',TDateBox_OnKeyUp,true);
		document.getElementById(id).addEventListener('keypress',TDateBox_OnKeyPress,true);
	}

}

function TDateBox_OnButtonClick(e)
{
	var textbox=null,button=null,data=null,inc=null,dateformat;
	if(window.event)
	{
		e=window.event;
		button=window.event.srcElement;
	}
	else
	{
		button=e.currentTarget;
	}
	textbox=document.getElementById(button.id.replace("_btn",""));
	if(textbox!=null)
	{
		TDateBox_ShowCalendar(textbox);
	}
	//Pendura variável no evento, para checagem no TDateBox_OnDocumentClick
	//e.TDateBox_OnButtonClicked=textbox.id;

}

function TDateBox_OnCalendarClick(textboxid,e)
{
	var cld=null;
	// se TDateBox_OnDocumentClick ainda não ocorreu (EI,Opera),
	// então cancela propagação
	cld=document.getElementById(textboxid+"_cld");

	if(cld!=null)
	{
		if(typeof(e.TDateBox_OnDocumentClicked)=='undefined')
			e.cancelBubble=true;
	}
	
}

function TDateBox_OnDocumentClick(textboxid,e)
{
  var textbox,cld,pont;
		
	//Pendura variável no evento, para checagem no TDateBox_OnCalendarClick (NS6)
	e.TDateBox_OnDocumentClicked=textboxid;
	
	//Checa se o botão foi clicado. Neste caso, não esconde o calendário
	if(typeof(e.srcElement)!='undefined' && e.srcElement.id==textboxid+"_btn")
			return;

	//Checa se o objeto clicado é o calendário (NS6)
	if(typeof(e.originalTarget)!='undefined')
	{
		cld=document.getElementById(textboxid+"_cld");
		pont=e.originalTarget;
		while(pont!=null && pont!=cld)
			pont=pont.parentNode;
		if(pont==cld)
			return;
	}
	
	//Esconde o calendário quando outro elemento da página for clicado
  textbox=document.getElementById(textboxid);
  if(textbox!=null)
		TDateBox_HideCalendar(textbox);
}

function TDateBox_OnBlur(e)
{
	var textbox=null,texto=null,data=null,inc=null,dateformat;
	if(window.event)
	{
		e=window.event;
		textbox=window.event.srcElement;
	}
	else
	{
		textbox=e.currentTarget;
	}
	if(textbox!=null)
	{
		dateformat=textbox.getAttribute("dateformat");
		texto=textbox.value;
		for(;texto.length>0 && texto.charAt(0)==" "; texto=(texto==" "?"":texto.substr(1)) );
		if(texto.length>0 && (texto.charAt(0)=="+" ||  texto.charAt(0)=="-"))
		{
			if(texto.length==1)
				inc=0;
			else
				inc=parseInt("0"+texto.substr(1),10);
			if(texto.charAt(0)=="-")
				inc=-inc;
			if(Math.abs(inc)>100000)
			{
				textbox.value="";
			}
			else
			{
				data=new Date();
				if(dateformat=="M/yyyy")
					data=new Date(data.getFullYear(),data.getMonth()+inc,1);
				else
					data=new Date(data.getFullYear(),data.getMonth(),data.getDate()+inc);
				textbox.value=TDateBox_sDate(data,dateformat);
			}
		}
		else
			TDateBox_CompletaData(textbox);
		
		TControl_OnChangeValidate(textbox.id);
	}
}

function TDateBox_OnKeyPress(e)
{
	var textbox=null,tecla=0,data=null,texto="",dateformat=null;
	if(window.event)
	{
		e=window.event;
		textbox=window.event.srcElement;
	}
	else
	{
		textbox=e.currentTarget;
	}
	if(typeof(e.which)!='undefined')
		tecla=e.which;
	else
		tecla=e.keyCode;
	if('0123456789/+-Hh'.indexOf(String.fromCharCode(tecla))==-1 && tecla!=38 && tecla!=40 && tecla!=13 && tecla!=0)
	{
			e.returnValue=false; //IE
			if(e.cancelable) //NS6
				e.preventDefault();
			return;
	}
	if(textbox!=null)
	{
		dateformat=textbox.getAttribute("dateformat");
		if(tecla=="H".charCodeAt(0) || tecla=="h".charCodeAt(0))
		{
			textbox.value=TDateBox_sDate(TDateBox_Hoje(),dateformat);
			e.returnValue=false; //IE
			if(e.cancelable) //NS6
				e.preventDefault();
			return;
		}

		texto=textbox.value;
		for(;texto.length>0 && texto.charAt(0)==" "; texto=(texto==" "?"":texto.substr(1)) );
		if(texto.charAt(0)=="+" || texto.charAt(0)=="-")
		{
			if('0123456789'.indexOf(String.fromCharCode(tecla))==-1 && tecla!=13 && tecla!=0)
			{
					e.returnValue=false; //IE
					if(e.cancelable) //NS6
						e.preventDefault();
					return;
			}
		}

	}
}

function TDateBox_OnKeyUp(e)
{
	var textbox=null,tecla=0,data=null,texto="";
	var cancela=false;

	if(window.event)
	{
		e=window.event;
		textbox=window.event.srcElement;
	}
	else
	{
		textbox=e.currentTarget;
	}

	if(typeof(e.which)!='undefined')
		tecla=e.which;
	else
		tecla=e.keyCode;
		
	
	if(textbox==null || e.shiftKey || e.altKey || e.ctrlKey) 
		cancela=true;
	else if('0123456789'.indexOf(String.fromCharCode(tecla))!=-1)
		cancela=false;
	else if(tecla!=38 && tecla!=40 && tecla!=13 && tecla!=0 && tecla!=193)
			cancela=true;

	if(cancela==false)
		TDateBox_CompletaDataAoDigitar(textbox,tecla);

}

function TDateBox_Hoje()
{
	var hoje=new Date();
	hoje=new Date(hoje.getFullYear(),hoje.getMonth(),hoje.getDate());
	return hoje;
}

function TDateBox_HideCalendar(textbox)
{
	var cld;
	cld=document.getElementById(textbox.id+"_cld");
	if(cld!=null)
	{
			TControl_ShowElement(cld.id,false);
	}
	
}

function TDateBox_ShowCalendar(textbox)
{
	var cld=null,left,top,html,data,dateformat;
	cld=document.getElementById(textbox.id+"_cld");
	if(cld!=null)
	{
		left=TControl_elementLeft(textbox);
		top=TControl_elementBottom(textbox)+2;
		
		dateformat=textbox.getAttribute("dateformat");
		if(dateformat==null) 
			dateformat="d";
	
		sdata=textbox.value;
		data=TDateBox_cDate(sdata,dateformat)
		if(isNaN(data)) 
			data=new Date()

		TDateBox_FillCalendar(textbox.id,data,dateformat);
		
		TControl_setPosition(cld.id,left,top);
		TControl_ShowElement(cld.id,true);
	}

}

function TDateBox_FillCalendar(textboxid,data,dateformat)
{
	var cld=document.getElementById(textboxid+"_cld");
		cld.innerHTML=TDateBox_getHtmlCalendario(textboxid,data,dateformat);
}



function TDateBox_CompletaDataAoDigitar(textbox,tecla) {
  var valor = textbox.value;
  var tamanho = valor.length;
  var dateformat=textbox.getAttribute("dateformat");
  if(dateformat==null) 
		dateformat="d";
  if (tamanho > 0) 
  {
		var ultima_letra = valor.charAt(tamanho - 1);

		if ( "/0123456789".indexOf(ultima_letra)!=-1 ) 
		{
			if (tamanho == 1) 
			{
				if (valor == "/") textbox.value = "";
				return;
			}
			if (tamanho == 2) 
			{
				if (tecla != 8) 
				{
					if (valor.charAt(1) == "/") 
						textbox.value = "0" + valor;
					else 
						textbox.value = valor + "/";
				}
				else 
					textbox.value = valor.substring(0,1);
				return;
			}
			if (tamanho == 4 && (dateformat=="d" || dateformat=="G") ) 
			{
				if (valor.charAt(3) == "/") textbox.value = valor.substring(0,3);
				return;
			}
			if (tamanho == 5 && (dateformat=="d" || dateformat=="G") )
			{
				if (tecla != 8) 
				{
					if (valor.charAt(4) == "/") 
						textbox.value = valor.substring(0,3) + "0" + valor.substring(3,5);
					else
						textbox.value = valor + "/";
				}
				else textbox.value = valor.substring(0,4);
				return;
			}
			if (tamanho > 6 && (dateformat=="d" || dateformat=="G") )
			{
				if (valor.charAt(tamanho - 1) == "/") textbox.value = valor.substring(0,tamanho - 1);
				return;
			}
			if (tamanho > 4 && dateformat=="M/yyyy" )
			{
				if (valor.charAt(tamanho - 1) == "/") textbox.value = valor.substring(0,tamanho - 1);
				return;
			}
		}
		else textbox.value = valor.substring(0,tamanho - 1);
	}
}

// TDateBox_CompletaData:
// Funcao que completa dia, mes e ano de um campo onde esta sendo preenchida uma data.
// Deve ser associada a propriedade ONBLUR do input tipo "text". No caso de nao  sido entrado
// um mes ou ano a data sera completada com o mes e ano atuais tambem completa o dia, mes e 
// ano "sem zeros" (1/1/1 -> 01/01/2001 e 1/1/99 -> 01/01/1999) So completa se tiver algum 
// conteudo no campo (permite data vazia). Esta funcao foi criada com base no formato de 
// data DD/MM/YYYY     
// 					                                                  
//  Parametros:							                              
//   textbox: campo da data									  
//  Autor: Henrico Scaranello						           24/10/2001 

function TDateBox_CompletaData(textbox) 
{
	var valor = new String(textbox.value);
  var tamanho = valor.length;
  var dateformat;
  
  dateformat=textbox.getAttribute("dateformat");
  if(dateformat==null) dateformat="d";

	if (tamanho > 0) 
	{

	  var hoje = new Date();
	  var dia = hoje.getDate();
	  var mes = hoje.getMonth() + 1;
	  var ano = hoje.getFullYear();
	  
	  var sdia,smes,sano;

		data = valor.split("/");
		if((dateformat=="d" || dateformat=="G") && TControl_CultureInfo.dateorder=="dmy")
		{
			sdia=data[0];
			smes=data[1];
			sano=data[2];
		}
		else if((dateformat=="d" || dateformat=="G") && TControl_CultureInfo.dateorder=="mdy")
		{
			if(data[0]!=null && data[1]==null && data[2]==null)
			{
				sdia=data[0];
				smes=null;
				sano=null;
			}
			else
			{
				smes=data[0];
				sdia=data[1];
				sano=data[2];
			}
		}
		else if(dateformat=="M/yyyy")
		{
			sdia=null;
			smes=data[0];
			sano=data[1];
		}
		// Arrumando o dia
		if ((sdia != null) && (sdia.length > 0)) 
		{}
		else
			sdia=dia
		if ( (sdia.length == 1) && (!isNaN(sdia)) ) 
				sdia = "0" + sdia;
			
		
		// Arrumando o mes
		if (!((smes != null) && (smes.length > 0))) 
			smes = mes.toString();

		if (smes.length == 1) 
			smes = "0" + smes;
		
		// Arrumando o ano
		if ((sano != null) && (sano.length > 0)) {
			if ( (sano.length < 4) && (!isNaN(sano)) ) {
				if (sano.length == 3) {
					if (sano > 500) sano = "1" + sano;
					else sano = "2" + sano;
				}
				if (sano.length == 2) {
					if (sano > 50) sano = "19" + sano;
					else sano = "20" + sano;
				}
				if (sano.length == 1) sano = "200" + sano;
			}
		}
		else sano = ano;
		
		if((dateformat=="d" || dateformat=="G") && TControl_CultureInfo.dateorder=="dmy")
			textbox.value = sdia + "/" + smes + "/" + sano;
		else if((dateformat=="d" || dateformat=="G") && TControl_CultureInfo.dateorder=="mdy")
			textbox.value = smes + "/" + sdia + "/" + sano;
		else if(dateformat=="M/yyyy")
			textbox.value = smes + "/" + sano;
	}
}

function TDateBox_setDate(textboxid,ano,mes,dia)
{
	var textbox,data;
	textbox=document.getElementById(textboxid);
	if(textbox!=null)
	{
		data=new Date(ano,mes,dia);
		textbox.value=TDateBox_sDate(data,textbox.getAttribute('dateformat'));
	}
	TControl_ShowElement(textboxid+"_cld",false);

	TControl_OnChangeValidate(textboxid); //revalida
}

function TDateBox_getHtmlCalendario(textboxid,data,dateformat)
{
	var textbox,html,dias,i,j,dayclass="",titleclass="",dayheaderclass="",othermonthdayclass="";
	var fillnext,fillprev;
	
	textbox=document.getElementById(textboxid);
	if(textbox.getAttribute('dayclass')!=null)
		dayclass=" class='"+textbox.getAttribute('dayclass')+"' ";
	if(textbox.getAttribute('titleclass')!=null)
		titleclass=" class='"+textbox.getAttribute('titleclass')+"' ";
	if(textbox.getAttribute('dayheaderclass')!=null)
		dayheaderclass=" class='"+textbox.getAttribute('dayheaderclass')+"' ";
	if(textbox.getAttribute('othermonthdayclass')!=null)
		othermonthdayclass=" class='"+textbox.getAttribute('othermonthdayclass')+"' ";
		
	if(dateformat=="d" || dateformat=="G")
	{
		dias=TDateBox_getMonthDays(data);
		fillprev='TDateBox_FillCalendar("'+textboxid+'",new Date('+data.getFullYear()+','+(data.getMonth()-1)+',1),"'+dateformat+'");';
		fillnext='TDateBox_FillCalendar("'+textboxid+'",new Date('+data.getFullYear()+','+(data.getMonth()+1)+',1),"'+dateformat+'");';
		html="<table border=0 cellpadding=2 style='border-collapse:collapse;cursor:default'>";
		html+="<tr><td colspan=9><table border=0 cellpadding=2 cellspacing=0 width=100%><tr><td "+titleclass+"><span onclick='"+fillprev+"' style='cursor:pointer;'>&lt;&lt;</span></td><td align='center' "+titleclass+">"+TControl_CultureInfo.monthnames[data.getMonth()]+" "+data.getFullYear()+"</td><td align='right' "+titleclass+"><span onclick='"+fillnext+"' style='cursor:pointer;'>&gt;&gt;</span></td></tr></table></td>";
		html+="<tr><td rowspan='"+(1+dias.length/7)+"'>&nbsp;</td>";
		for(j=0;j<7;j++)
			html+="<td"+dayheaderclass+">"+TControl_CultureInfo.abbreviateddaynames[j]+"</td>";
		html+="<td rowspan='"+(1+dias.length/7)+"'>&nbsp;</td></tr>";
		for(i=0;i<dias.length/7;i++)
		{
			html+="<tr>";
				for(j=0;j<7;j++)
					html+="<td align='right' "+(dias[i*7+j].getMonth()!=data.getMonth()?othermonthdayclass:dayclass)+"><span style='cursor:pointer;"+(dias[i*7+j].getMonth()!=data.getMonth()?"color:silver;":"")+"' onclick='TDateBox_setDate(\""+textboxid+"\","+dias[i*7+j].getFullYear()+","+dias[i*7+j].getMonth()+","+dias[i*7+j].getDate()+")'>"+dias[i*7+j].getDate()+"</span></td>";
			html+="</tr>";
		}
		html+="<tr><td colspan=9 align='center' "+dayclass+"><span style='cursor:pointer;' onclick='TDateBox_setDate(\""+textboxid+"\","+(new Date()).getFullYear()+","+(new Date()).getMonth()+","+(new Date()).getDate()+")'>"+TControl_CultureInfo.todayname+"</td></tr>";
		html+="</table>"
	}
	else if(dateformat=="M/yyyy")
	{
		fillprev='TDateBox_FillCalendar("'+textboxid+'",new Date('+(data.getFullYear()-1)+',0,1),"'+dateformat+'");';
		fillnext='TDateBox_FillCalendar("'+textboxid+'",new Date('+(data.getFullYear()+1)+',0,1),"'+dateformat+'");';
		html="<table border=0 cellpadding=2 cellspacing=0 style='border-collapse:collapse;cursor:default'>";
		html+="<tr><td colspan=6><table border=0 cellpadding=2 cellspacing=0 width=100% style='cursor:default'><tr><td "+titleclass+"><span onclick='"+fillprev+"' style='cursor:pointer;'>&lt;&lt;</span></td><td align='center' "+titleclass+">"+data.getFullYear()+"</td><td align='right' "+titleclass+"><span onclick='"+fillnext+"' style='cursor:pointer;'>&gt;&gt;</span></td></tr></table></td></tr>";
		for(i=0;i<4;i++)
		{
			html+="<tr><td></td>";
				for(j=0;j<3;j++)
					html+="<td align='center' "+dayclass+"><span style='cursor:pointer;' onclick='TDateBox_setDate(\""+textboxid+"\","+data.getFullYear()+","+(i*3+j)+",1)'>"+TControl_CultureInfo.monthnames[i*3+j]+"</span></td>";
			html+="<td></td></tr>";
		}
		html+="</table>"
	}
	return html;
}

function TDateBox_getMonthDays(mesano) {
  // A hora "1" para cada um dos new Date()'s é para contornar o bug em meses
  // de início de horário de verão.
  
  var dias = new Array();
  var prim = new Date(mesano.getFullYear(), mesano.getMonth(), 1);
  while(prim.getDay() > 0)
    prim = new Date(prim.getFullYear(), prim.getMonth(), prim.getDate() - 1, 1);
    
  var mesini = prim.getMonth();
  var mes = mesano.getMonth();
  while(prim.getMonth() == mes || prim.getMonth() == mesini) {
    dias[dias.length] = prim;
    prim = new Date(prim.getFullYear(), prim.getMonth(), prim.getDate() + 1, 1);
  }
  
  while(prim.getDay() > 0) {
    dias[dias.length] = prim;
    prim = new Date(prim.getFullYear(), prim.getMonth(), prim.getDate() + 1, 1);
  }
  
  return dias;
}

function TDateBox_cDate(sData,dateformat)
{
	var sAux=new String(sData);
	var iDia,iMes,iAno,dUltDia,iUltDia;
	var sDia,sMes,sAno
	var dHoje
	var pos1,pos2,i,nums
	dHoje=new Date();
	// Procura as barras
	nums=sAux.split('/');
	if(dateformat==null || ((dateformat=='d' || dateformat=='G') && TControl_CultureInfo.dateorder=="dmy"))
	{
		sDia=(nums.length>0?nums[0]:'');
		sMes=(nums.length>1?nums[1]:'');
		sAno=(nums.length>2?nums[2]:'');
	}
	else if(dateformat=='M/yyyy')
	{
		sDia='01';
		sMes=(nums.length>0?nums[0]:'');
		sAno=(nums.length>1?nums[1]:'');
	}
	else if((dateformat=='d' || dateformat=='G') && TControl_CultureInfo.dateorder=="mdy")
	{
		sMes=(nums.length>0?nums[0]:'');
		sDia=(nums.length>1?nums[1]:'');
		sAno=(nums.length>2?nums[2]:'');
	}
	else
	{
	//Se não achou as barras, assume formato DDMMYYYY
		sDia=sData.substring(0,2)
		sMes=sData.substring(2,4)
		sAno=sData.substring(4)
	}
	iDia=new Number(sDia);
	iMes=new Number(sMes);
	iAno=new Number(sAno);
	if(isNaN(iMes) || sMes=='') iMes=dHoje.getMonth()+1;
	if(isNaN(iAno) || sAno=='') iAno=dHoje.getFullYear();
	if(isNaN(iDia) || isNaN(iMes) || isNaN(iAno))
	{
	 return NaN;
	}
	if(iMes<1 || iMes>12)
	{
	 return NaN;
	}
	if(iAno<0) return NaN;
	if(iAno<50) iAno=iAno+2000;
	dUltDia=new Date(iAno,iMes,1);
	dUltDia.setDate(0);
	iUltDia=new Number(dUltDia.getDate());
	if(iDia<1 || iDia>iUltDia)
	{
	 return NaN;
	}
	var dData=new Date(iAno,iMes-1,iDia);
	return dData;
};


function TDateBox_sDate(dData,dateformat)
{
	var iDia,iMes,iAno,sDia,sMes,sAno;
	sDia=new String(dData.getDate());
	if(sDia.length==1) sDia="0"+sDia;
	sMes=new String(dData.getMonth()+1);
	if(sMes.length==1) sMes="0"+sMes;
	sAno=new String(dData.getFullYear());
	if(dateformat=="M/yyyy")
		return sMes+"/"+sAno;
	else if(TControl_CultureInfo.dateorder=="mdy")
		return sMes+"/"+sDia+"/"+sAno;
	else 
		return sDia+"/"+sMes+"/"+sAno;
};

