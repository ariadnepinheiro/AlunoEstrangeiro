
var TEditor__ns6=(document.getElementById&&!document.all);
var TEditor__opera=(navigator.appName.indexOf("Opera")>-1);
var TEditor__ie=(document.all&&!TEditor__opera);

function TEditor__move(e)
{
  var evento,editor,control,direc,h,w,i;
  
  editor=TEditor__getEditor(e);
  if(!editor || editor.Action==null)
    return;
    
  evento=TEditor__getEvento(e);
  if(!editor)
    return true;
  control=editor.SelectedControl;
  if(editor.Action=="move" && control)
  {
    if(TEditor__ns6)
    { 
    	control.style.left=editor.snapX*(Math.floor((control.offX+e.clientX-control.mousedownX)/editor.snapX+0.5));
    	control.style.top=editor.snapY*(Math.floor((control.offY+e.clientY-control.mousedownY)/editor.snapY+0.5));
    	if(parseInt(control.style.left)<0) control.style.left=0;
    	if(parseInt(control.style.top)<0)  control.style.top=0;
      editor.ShowSelectedBorder(control);
      if(control.anchoredControls && control.anchoredControls.length)
      {
        for(i=0;i<control.anchoredControls.length;i++)
        {
          control.anchoredControls[i].style.left=control.anchoredControls[i].offX+parseInt(control.style.left)-control.offX;
    	    control.anchoredControls[i].style.top =control.anchoredControls[i].offY+parseInt(control.style.top) -control.offY;
        }
      }
      window.status='('+control.style.left+','+control.style.lop+')';
    }
    else if(TEditor__ie)
    {
      document.selection.clear();
   	  control.style.posLeft=editor.snapX*(Math.floor((control.offX+window.event.clientX-control.mousedownX)/editor.snapX+0.5));
    	control.style.posTop=editor.snapY*(Math.floor((control.offY+window.event.clientY-control.mousedownY)/editor.snapY+0.5));
    	if(control.style.posLeft<0)control.style.posLeft=0;
    	if(control.style.posTop<0)control.style.posTop=0;
      editor.ShowSelectedBorder(control);
      if(control.anchoredControls && control.anchoredControls.length)
      {
        for(i=0;i<control.anchoredControls.length;i++)
        {
          control.anchoredControls[i].style.posLeft=control.anchoredControls[i].offX+control.style.posLeft-control.offX;
    	    control.anchoredControls[i].style.posTop =control.anchoredControls[i].offY+control.style.posTop -control.offY;
        }
      }
      window.status='('+control.style.posLeft+','+control.style.posTop+')';
    }
  }
  else if(editor.Action && editor.Action.indexOf("resize-")==0)
  {
    direc=editor.Action.substr(7);
    if(TEditor__ns6)
    {
      if(direc.indexOf("s")>=0 && (!control.getAttribute('resizeHeight') || control.getAttribute('resizeHeight')=="yes"))
      {
        h=editor.snapY*(Math.floor((control.offHeight+e.clientY-control.mousedownY)/editor.snapY+0.5));
        if(h<0) h=0;
        if(control.getAttribute('minHeight') && h<parseInt(control.getAttribute('minHeight')))
          h=parseInt(control.getAttribute('minHeight'));
        control.style.height=h;
      }
      if(direc.indexOf("e")>=0 && (!control.getAttribute('resizeWidth') || control.getAttribute('resizeWidth')=="yes"))
      {
        w=editor.snapX*(Math.floor((control.offWidth+e.clientX-control.mousedownX)/editor.snapX+0.5));
        if(w<0)w=0;
        if(control.getAttribute('minWidth') && w<parseInt(control.getAttribute('minWidth')))
          w=parseInt(control.getAttribute('minWidth'));
        control.style.width=w;
      }
      if(direc.indexOf("n")>=0 && (!control.getAttribute('resizeHeight') || control.getAttribute('resizeHeight')=="yes"))
      {
        h=editor.snapY*(Math.floor((control.offHeight-e.clientY+control.mousedownY)/editor.snapY+0.5));
        if(h<0) h=0;
        if(control.getAttribute('minHeight') && h<parseInt(control.getAttribute('minHeight')))
          h=parseInt(control.getAttribute('minHeight'));
    	  control.style.top=control.offY+control.offHeight-h;
        control.style.height=h;
      }
      if(direc.indexOf("w")>=0 && (!control.getAttribute('resizeWidth') || control.getAttribute('resizeWidth')=="yes"))
      {
        w=editor.snapX*(Math.floor((control.offWidth-e.clientX+control.mousedownX)/editor.snapX+0.5));
        if(w<0)w=0;
        if(control.getAttribute('minWidth') && w<parseInt(control.getAttribute('minWidth')))
          w=parseInt(control.getAttribute('minWidth'));
    	  control.style.left=control.offX+control.offWidth-w;
        control.style.width=w;
      }
      editor.ShowSelectedBorder(control);
      window.status='('+control.style.width+','+control.style.height+')';
    }
    else if(TEditor__ie)
    {
      document.selection.clear();
      if(direc.indexOf("s")>=0 && (!control.getAttribute('resizeHeight') || control.getAttribute('resizeHeight')=="yes"))
      {
        h=editor.snapY*(Math.floor((control.offHeight+window.event.clientY-control.mousedownY)/editor.snapY+0.5));
        if(h<0) h=0;
        if(control.getAttribute('minHeight') && h<parseInt(control.getAttribute('minHeight')))
          h=parseInt(control.getAttribute('minHeight'));
        control.style.height=h;
      }
      if(direc.indexOf("e")>=0 && (!control.getAttribute('resizeWidth') || control.getAttribute('resizeWidth')=="yes"))
      {
        w=editor.snapX*(Math.floor((control.offWidth+window.event.clientX-control.mousedownX)/editor.snapX+0.5));
        if(w<0)w=0;
        if(control.getAttribute('minWidth') && w<parseInt(control.getAttribute('minWidth')))
          w=parseInt(control.getAttribute('minWidth'));
        control.style.width=w;
      }
      if(direc.indexOf("n")>=0 && (!control.getAttribute('resizeHeight') || control.getAttribute('resizeHeight')=="yes"))
      {
        h=editor.snapY*(Math.floor((control.offHeight-window.event.clientY+control.mousedownY)/editor.snapY+0.5));
        if(h<0) h=0;
        if(control.getAttribute('minHeight') && h<parseInt(control.getAttribute('minHeight')))
          h=parseInt(control.getAttribute('minHeight'));
    	  control.style.top=control.offY+control.offHeight-h;
        control.style.height=h;
      }
      if(direc.indexOf("w")>=0 && (!control.getAttribute('resizeWidth') || control.getAttribute('resizeWidth')=="yes"))
      {
        w=editor.snapX*(Math.floor((control.offWidth-window.event.clientX+control.mousedownX)/editor.snapX+0.5));
        if(w<0)w=0;
        if(control.getAttribute('minWidth') && w<parseInt(control.getAttribute('minWidth')))
          w=parseInt(control.getAttribute('minWidth'));
    	  control.style.left=control.offX+control.offWidth-w;
        control.style.width=w;
      }
      editor.ShowSelectedBorder(control);
      window.status='('+control.style.width+','+control.style.height+')';
    }
  }
  return true;
}
function TEditor__up(e)
{
  var evento,editor,control;
  
  evento=TEditor__getEvento(e);;
  editor=TEditor__getEditor(e);
  control=TEditor__getControl(e);
  if(!editor)
    return;

  window.status='';
  editor.Action=null;
  editor.SetLayout();
  return true;
}


function TEditor__leave(e)
{
  return TEditor__up(e);
}


function TEditor__down(e)
{
  var evento,editor,control,resizedir,anchoredids,i;
  
  evento=TEditor__getEvento(e);;
  editor=TEditor__getEditor(e);
  control=TEditor__getControl(e);
  resizedir=TEditor__getResizeDirection(e);
  
  if(!editor)
    return true;
    
  if(control && !(control.getAttribute("canSelect")=="no"))
  {
    editor.Select(control);
    editor.Action="move";
    if(TEditor__ns6)
    {
      control.offX=control.offsetLeft;
      control.offY=control.offsetTop;
      control.mousedownX=e.clientX;
      control.mousedownY=e.clientY;
      if(control.getAttribute('anchored'))
      {
				anchoredids=control.getAttribute('anchored').split(';');
				control.anchoredControls=new Array();
				for(i=0;i<anchoredids.length;i++)
				{
				 if(document.getElementById(anchoredids[i]))
				 {
					document.getElementById(anchoredids[i]).offX=document.getElementById(anchoredids[i]).offsetLeft;
					document.getElementById(anchoredids[i]).offY=document.getElementById(anchoredids[i]).offsetTop;
					control.anchoredControls[i]=document.getElementById(anchoredids[i]);
				 }
				}
      }
    }
    else if(TEditor__ie)
    {
      control.offX=control.offsetLeft;
      control.offY=control.offsetTop;
      control.mousedownX=window.event.clientX;
      control.mousedownY=window.event.clientY;
      if(control.getAttribute('anchored'))
      {
				anchoredids=control.getAttribute('anchored').split(';');
				control.anchoredControls=new Array();
				for(i=0;i<anchoredids.length;i++)
				{
				 if(document.getElementById(anchoredids[i]))
				 {
					document.getElementById(anchoredids[i]).offX=document.getElementById(anchoredids[i]).offsetLeft;
					document.getElementById(anchoredids[i]).offY=document.getElementById(anchoredids[i]).offsetTop;
					control.anchoredControls[i]=document.getElementById(anchoredids[i]);
				 }
				}
      }
    }
  }
  else if(resizedir)
  { 
    if(TEditor__ns6)
    {
      editor.SelectedControl.offX=editor.SelectedControl.offsetLeft;
      editor.SelectedControl.offY=editor.SelectedControl.offsetTop;
      editor.SelectedControl.offWidth =parseInt(editor.SelectedControl.style.width);
      editor.SelectedControl.offHeight=parseInt(editor.SelectedControl.style.height);
      editor.SelectedControl.mousedownX=e.clientX;
      editor.SelectedControl.mousedownY=e.clientY;
    }
    else if(TEditor__ie)
    {
      editor.SelectedControl.offX=editor.SelectedControl.offsetLeft;
      editor.SelectedControl.offY=editor.SelectedControl.offsetTop;
      editor.SelectedControl.offWidth =editor.SelectedControl.offsetWidth;
      editor.SelectedControl.offHeight=editor.SelectedControl.offsetHeight;
      editor.SelectedControl.mousedownX=window.event.clientX;
      editor.SelectedControl.mousedownY=window.event.clientY;
    }
    editor.Action="resize-"+resizedir;
  }
  else
  {
    editor.Select(null);
    editor.Action=null;
  }

  window.status='';
}


function TEditor__attachEditorEvents(id)
{
  if(window.addEventListener)
  {
    document.getElementById(id).addEventListener('mousedown',TEditor__down,true);
    document.getElementById(id).addEventListener('mouseup',TEditor__up,true);
    document.getElementById(id).addEventListener('mouseleave',TEditor__leave,true);
    document.getElementById(id).addEventListener('mousemove',TEditor__move,true);
    document.getElementById(id).Select=TEditor__select;
    document.getElementById(id).SelectedControl=null;
    document.getElementById(id).ShowSelectedBorder=TEditor__showselectedborder;
    document.getElementById(id).SetLayout=TEditor__setLayout;
  }
  else if(window.attachEvent)
  {
    document.getElementById(id).attachEvent('onmousedown',TEditor__down);
    document.getElementById(id).attachEvent('onmouseup',TEditor__up);
    document.getElementById(id).attachEvent('onmouseleave',TEditor__leave);
    document.getElementById(id).attachEvent('onmousemove',TEditor__move);
    document.getElementById(id).Select=TEditor__select;
    document.getElementById(id).SelectedControl=null;
    document.getElementById(id).ShowSelectedBorder=TEditor__showselectedborder;
    document.getElementById(id).SetLayout=TEditor__setLayout;
  }
  else
  {
    document.getElementById(id).onmousedown=TEditor__down;
    document.getElementById(id).onmouseup=TEditor__up;
    document.getElementById(id).onmouseleave=TEditor__leave;
    document.getElementById(id).onmousemove=TEditor__move;
    document.getElementById(id).Select=TEditor__select;
    document.getElementById(id).SelectedControl=null;
    document.getElementById(id).ShowSelectedBorder=TEditor__showselectedborder;
    document.getElementById(id).SetLayout=TEditor__setLayout;
  }
}
function TEditor__select(control)
{
  if(control)
  {
     this.SelectedControl=control;
     if(!(control.getAttribute && control.getAttribute('raiseWhenSelected') && control.getAttribute('raiseWhenSelected')=="no"))
     {
       if(!this.nextzIndex)
         this.nextzIndex=101;
       else
         this.nextzIndex++;
       this.SelectedControl.style.zIndex=this.nextzIndex;
     }
     this.ShowSelectedBorder(control);
  }
  else
  {
    if(this.SelectedControl)
    {
     this.SelectedControl=null;
     this.ShowSelectedBorder(null);
    }
  }
}
function TEditor__showselectedborder(control)
{
  var ne,n,nw,e,w,se,s,sw;
  var show=false;
	ne=document.getElementById(this.id+"_resizene");
	n =document.getElementById(this.id+"_resizen");
	nw=document.getElementById(this.id+"_resizenw");
	e =document.getElementById(this.id+"_resizee");
	w =document.getElementById(this.id+"_resizew");
	se=document.getElementById(this.id+"_resizese");
	s =document.getElementById(this.id+"_resizes");
	sw=document.getElementById(this.id+"_resizesw");
	if(control && TEditor__ie)
	  show=true;
	else if(control && TEditor__ns6 && control.style.height && control.style.width)
	  show=true;
	if(show)
   {
     if(TEditor__ns6)
     {
     		ne.style.top=parseInt(control.style.top) - parseInt(ne.style.height);
     		n.style.top=parseInt(control.style.top) - parseInt(n.style.height);
     		nw.style.top=parseInt(control.style.top) - parseInt(nw.style.height);
     		se.style.top=parseInt(control.style.top) + parseInt(control.style.height);
     		s.style.top=parseInt(control.style.top) + parseInt(control.style.height);
     		sw.style.top=parseInt(control.style.top) + parseInt(control.style.height);
     		w.style.top=parseInt(control.style.top) + (parseInt(control.style.height) - parseInt(w.style.height))/2;
     		e.style.top=parseInt(control.style.top) + (parseInt(control.style.height) - parseInt(e.style.height))/2;
     		
     		nw.style.left=parseInt(control.style.left) - parseInt(nw.style.width);
     		w.style.left=parseInt(control.style.left) - parseInt(w.style.width);
     		sw.style.left=parseInt(control.style.left) - parseInt(sw.style.width);
     		ne.style.left=parseInt(control.style.left) + parseInt(control.style.width);
     		e.style.left=parseInt(control.style.left) + parseInt(control.style.width);
     		se.style.left=parseInt(control.style.left) + parseInt(control.style.width);
     		s.style.left=parseInt(control.style.left) + (parseInt(control.style.width) - parseInt(s.style.width))/2;
     		n.style.left=parseInt(control.style.left) + (parseInt(control.style.width) - parseInt(n.style.width))/2;
     }
     else if (TEditor__ie)
     {
     		ne.style.posTop=control.style.posTop - ne.offsetHeight;
     		n.style.posTop=control.style.posTop - n.offsetHeight;
     		nw.style.posTop=control.style.posTop - nw.offsetHeight;
     		se.style.posTop=control.style.posTop + control.offsetHeight;
     		s.style.posTop=control.style.posTop + control.offsetHeight;
     		sw.style.posTop=control.style.posTop + control.offsetHeight;
     		w.style.posTop=control.style.posTop + (control.offsetHeight - w.offsetHeight)/2;
     		e.style.posTop=control.style.posTop + (control.offsetHeight - e.offsetHeight)/2;
     		
     		nw.style.posLeft=control.style.posLeft - nw.offsetWidth;
     		w.style.posLeft=control.style.posLeft - w.offsetWidth;
     		sw.style.posLeft=control.style.posLeft - sw.offsetWidth;
     		ne.style.posLeft=control.style.posLeft + control.offsetWidth;
     		e.style.posLeft=control.style.posLeft + control.offsetWidth;
     		se.style.posLeft=control.style.posLeft + control.offsetWidth;
     		s.style.posLeft=control.style.posLeft + (control.offsetWidth - s.offsetWidth)/2;
     		n.style.posLeft=control.style.posLeft + (control.offsetWidth - n.offsetWidth)/2;
     }
	  ne.style.visibility='visible';
	  n.style.visibility='visible';
	  nw.style.visibility='visible';
	  e.style.visibility='visible';
	  w.style.visibility='visible';
	  se.style.visibility='visible';
	  s.style.visibility='visible';
	  sw.style.visibility='visible';
	}
	else
	{
	  ne.style.visibility='hidden';
	  n.style.visibility='hidden';
	  nw.style.visibility='hidden';
	  e.style.visibility='hidden';
	  w.style.visibility='hidden';
	  se.style.visibility='hidden';
	  s.style.visibility='hidden';
	  sw.style.visibility='hidden';
	}
}
function TEditor__getEvento(e)
{
  if(TEditor__ie)
    return window.event;
  else if(TEditor__ns6)
    return e;
  else
    return null;
}

function TEditor__getParentEditor(control)
{
  var o;
  if(!(o.getAttribute && o.getAttribute('editorClass')=='Control'))
    return null;
  o=control;
  while(o && !(o.getAttribute && o.getAttribute('editorClass')=='Editor'))
  {
  	if(TEditor__ie)
  		o=o.parentElement;
  	else if(TEditor__ns6)
  		o=o.parentNode;
  	else
  	   o=null;
  }
  if(o && o.getAttribute && o.getAttribute('editorClass')=='Editor')
    return o;
  else
    return null;

}
function TEditor__getEditor(e)
{
  var o;
  
  if(TEditor__ie)
    o=window.event.srcElement;
  else if(TEditor__ns6)
    o=e.target;
  else
    o=null;
  while(o && !(o.getAttribute && o.getAttribute('editorClass')=='Editor'))
  {
  	if(TEditor__ie)
  		o=o.parentElement;
  	else if(TEditor__ns6)
  		o=o.parentNode;
  	else
  	   o=null;
  }
  if(o && o.getAttribute && o.getAttribute('editorClass')=='Editor')
  {
    if(o.getAttribute('snapX') && parseInt(o.getAttribute('snapX'))>0)
      o.snapX=parseInt(o.getAttribute('snapX'));
    else
      o.snapX=1;
    if(o.getAttribute('snapY') && parseInt(o.getAttribute('snapY'))>0)
      o.snapY=parseInt(o.getAttribute('snapY'));
    else
      o.snapY=1;
    return o;
  }
  else
    return null;
}
function TEditor__getControl(e)
{
  var o,offx=0,offy=0,s;
  
  if(TEditor__ie)
  {
    o=window.event.srcElement;
  }
  else if(TEditor__ns6)
  {
    o=e.target;
  }
  else
    o=null;
  while(o && !(o.getAttribute && o.getAttribute('editorClass')=='Control'))
  {
  	if(TEditor__ie)
  	{
  		o=o.parentElement;
  	}
  	else if(TEditor__ns6)
  	{
  		o=o.parentNode;
  	}
  	else
  	   o=null;
  }
  if(o && o.getAttribute && o.getAttribute('editorClass')=='Control')
  {
    return o;
  }
  else
    return null;
}

function TEditor__getResizeDirection(e)
{
  var o;
  
  if(TEditor__ie)
    o=window.event.srcElement;
  else if(TEditor__ns6)
    o=e.target;
  else
    o=null;
  while(o && !(o.getAttribute && o.getAttribute('editorClass')=='Resize'))
  {
  	if(TEditor__ie)
  		o=o.parentElement;
  	else if(TEditor__ns6)
  		o=o.parentNode;
  	else
  	   o=null;
  }
  if(o && o.getAttribute && o.getAttribute('editorClass')=='Resize')
  {
    if(o.style && o.style.cursor && o.style.cursor.indexOf('-resize')>0)
      return o.style.cursor.substring(0,o.style.cursor.indexOf('-resize'));
    else
      return null;
  }
  else
    return null;
}

function TEditor__setLayout()
{
  var i;
  var lay="";
  var ctrl;
  var divs;
  var no;
  
  if(this && document.getElementById(this.id+"_in") && document.getElementById(this.id+"_in").getElementsByTagName("div"))
  {
		divs=document.getElementById(this.id+"_in").getElementsByTagName("div");
    for(i=0;i<divs.length;i++)
      if(divs[i].getAttribute("editorClass")=="Control")
      {
        if(TEditor__ie)
        {
          lay+=divs[i].id.substr(this.id.length+1)+",";
	        lay+=divs[i].style.posLeft+","+divs[i].style.posTop+",";
	        lay+=divs[i].offsetWidth+","+divs[i].offsetHeight;
	        lay+=";";
	      }
	      if(TEditor__ns6)
	      {
          lay+=divs[i].id.substr(this.id.length+1)+",";
	        lay+=parseInt(divs[i].style.left)+","+parseInt(divs[i].style.top)+",";
	        lay+=(isNaN(parseInt(divs[i].style.width))?"":parseInt(divs[i].style.width))+","+(isNaN(parseInt(divs[i].style.height))?"":parseInt(divs[i].style.height));
	        lay+=";";
	      }
      }
    document.getElementById(this.id+"_layout").value=lay;
  }
}
