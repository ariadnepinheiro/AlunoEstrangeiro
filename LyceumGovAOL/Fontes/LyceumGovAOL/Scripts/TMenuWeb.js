	function TMenuWeb_getAttribute(menuid,attrid)
	{
		var m,p;
		m=TControl_getElementById(menuid);
		if(m==null)
			return "";
		else
			p=m.getAttribute(attrid);
		return p==null?"":p;
	}
	
	function TMenuWeb_OpenMenus(menuid)
	{
		var menu=TControl_getElementById(menuid);
		if(menu.openmenus==null)
		{
			menu.openmenus=new Array();
		}
		return menu.openmenus;
	}
	

	function TMenuWeb_Browser()
	{
		var bi=TControl_BrowserInfo();
		if(bi.browser=="IE")
			return "IE";
		else if(bi.browser=="NS" && bi.version>4)
			return "NS5";
		else if(bi.browser=="opera")
			return "opera";
		else
			return bi.browser;
	}

	function TMenuWeb_mb_c(sID,menuid)
	{
		var objCell = TControl_getElementById(menuid+"td" + sID);
		var objMenu = TControl_getElementById(menuid+"tbl" + sID);
		if(TMenuWeb_Browser()=="IE" || TMenuWeb_Browser()=="NS5")
		{
			TMenuWeb_initMenu(menuid);
			if (objMenu != null)
			{
				if (objMenu.style.Visibility == "visible")
				{
					TMenuWeb_hideAllMenus(menuid);		
				}
				else
				{
					TMenuWeb_positionMenu(objMenu, objCell,menuid);
					if(TMenuWeb_Browser()=="IE")
						TMenuWeb_handleTransition(objMenu);
					TMenuWeb_OpenMenus(menuid)[0] = sID;
					TControl_ShowElement(objMenu.id,true);
				}
			}
		}
		if(TMenuWeb_Browser()=="IE")
			window.event.cancelBubble = true;
		else if(TMenuWeb_Browser()=="NS5")
		{
			objCell.bubbles = false;
			return false;
		}
	}
	
	function TMenuWeb_handleTransition(objMenu)
	{
	}
	
	function TMenuWeb_mb_md(sID,menuid)
	{
		if(TMenuWeb_Browser()=="IE" || TMenuWeb_Browser()=="NS5")
		{
			var objCell = TControl_getElementById(menuid+"td" + sID);
			TMenuWeb_applyBorder(objCell, 1, TMenuWeb_getAttribute(menuid,'MENUBARITEMSHADOWCOLOR'), TMenuWeb_getAttribute(menuid,'MENUBARITEMHIGHLIGHTCOLOR'));
		}
	}
	function TMenuWeb_mb_mu(sID,menuid)
	{
		if(TMenuWeb_Browser()=="IE" || TMenuWeb_Browser()=="NS5")
		{
			var objCell = TControl_getElementById(menuid+"td" + sID);
			TMenuWeb_applyBorder(objCell, 1, TMenuWeb_getAttribute(menuid,'MENUBARITEMHIGHLIGHTCOLOR'), TMenuWeb_getAttribute(menuid,'MENUBARITEMSHADOWCOLOR'));
		}
	}
	
	function TMenuWeb_mb_mo(sID,menuid)
	{
		if(TMenuWeb_Browser()=="IE" || TMenuWeb_Browser()=="NS5")
		{
			TMenuWeb_initMenu(menuid);
				
			var objCell = TControl_getElementById(menuid+"td" + sID);
			var objMenu = TControl_getElementById(menuid+"tbl" + sID);
			if (TMenuWeb_OpenMenus(menuid).length)
			{
				TMenuWeb_hideAllMenus(menuid);
				if (objMenu != null)
				{
					TMenuWeb_positionMenu(objMenu, objCell,menuid);
					if(TMenuWeb_Browser()=="IE")
						TMenuWeb_handleTransition(objMenu);
					TMenuWeb_OpenMenus(menuid)[0] = sID;
					TControl_ShowElement(objMenu.id,true);
				}
				TMenuWeb_applyBorder(objCell, 1, TMenuWeb_getAttribute(menuid,'MENUBARITEMSHADOWCOLOR'), TMenuWeb_getAttribute(menuid,'MENUBARITEMHIGHLIGHTCOLOR'));
			}
			else
			{
				TMenuWeb_applyBorder(objCell, 1, TMenuWeb_getAttribute(menuid,'MENUBARITEMHIGHLIGHTCOLOR'), TMenuWeb_getAttribute(menuid,'MENUBARITEMSHADOWCOLOR'));
			}
		}
	}
	function TMenuWeb_mb_mout(sID,menuid)
	{
	  var bgcolor=TMenuWeb_getAttribute(menuid,'BGCOLOR');
		if(TMenuWeb_Browser()=="IE" || TMenuWeb_Browser()=="NS5")
		{
			var objCell = TControl_getElementById(menuid+"td" + sID);
			TMenuWeb_applyBorder(objCell, 1, bgcolor, bgcolor,"solid");	
		}
	}
	function TMenuWeb_mbi_c(sID,menuid)
	{
		if(TMenuWeb_Browser()=="IE" || TMenuWeb_Browser()=="NS5")
		{
			TMenuWeb_initMenu(menuid);
				
			var objRow = TMenuWeb_getSourceTR(sID,menuid);
			if (TMenuWeb_itemHasChildren(sID) == false)
			{		
				TMenuWeb_hideAllMenus(menuid);
			}
			if(TMenuWeb_Browser()=="IE")
				window.event.cancelBubble = true;
			else if(TMenuWeb_Browser()=="NS5")
			return true;
		}
	}
	function TMenuWeb_mbi_mo(sID,menuid)
	{		
		if(TMenuWeb_Browser()=="IE" || TMenuWeb_Browser()=="NS5")
		{
			TMenuWeb_handlembi_mo(sID,menuid);
		}
	}
	function TMenuWeb_mbi_mout(sID,menuid)
	{	
		if(TMenuWeb_Browser()=="IE" || TMenuWeb_Browser()=="NS5")
		{
			TMenuWeb_handlembi_mout(sID,menuid);
		}
	}
	function TMenuWeb_menuhook_MouseMove(e,menuid) 
	{
		var iNewLeft=0, iNewTop = 0;
		var m_objMenu;
		
		m_objMenu=TControl_getElementById(menuid);
		if(m_objMenu==null || m_objMenu.Moving==null)
			return;
			
		if(TMenuWeb_Browser()=="IE")
		{
			if ((event.button==1)) 
			{
				TMenuWeb_hideAllMenus(menuid);
				if (m_objMenu.startLeft == null)
					m_objMenu.startLeft = m_objMenu.offsetLeft;
				iNewLeft=event.clientX - m_objMenu.startLeft - 3;
				m_objMenu.style.pixelLeft= iNewLeft;
				if (m_objMenu.startTop == null)
					m_objMenu.startTop = m_objMenu.offsetTop;
				iNewTop=event.clientY - m_objMenu.startTop;
				m_objMenu.style.pixelTop = iNewTop - 10;
				event.returnValue = false
				event.cancelBubble = true
			}
		}
		else if(TMenuWeb_Browser()=="NS5")
		{
			if (m_objMenu.Moving) 
			{
				TMenuWeb_hideAllMenus(menuid);
				if (m_objMenu.startLeft == null)
					m_objMenu.startLeft = m_objMenu.offsetLeft;
				iNewLeft=e.clientX - m_objMenu.startLeft - 3;
				m_objMenu.style.left = iNewLeft;
				if (m_objMenu.startTop == null)
					m_objMenu.startTop = m_objMenu.offsetTop;
				iNewTop=e.clientY - m_objMenu.startTop;
				m_objMenu.style.top = iNewTop - 10;

			}
		}
	}
	function TMenuWeb_menuhook_MouseDown(e,menuid)
	{
		if(TMenuWeb_Browser()=="IE" || TMenuWeb_Browser()=="NS5")
		{
			TMenuWeb_initMenu(menuid);
			if(TMenuWeb_getElement(e,menuid)!=null)
				TControl_getElementById(menuid).Moving = true;
			else
				TControl_getElementById(menuid).Moving = null;
		}
	}
	
	function TMenuWeb_menuhook_MouseUp(e,menuid)
	{
		if(TMenuWeb_Browser()=="IE" || TMenuWeb_Browser()=="NS5")
		{
				TControl_getElementById(menuid).Moving = null;
		}
	}
	
	function TMenuWeb_document_MouseMove(e,menuid)
	{
		if(TMenuWeb_Browser()=="IE" || TMenuWeb_Browser()=="NS5")
				TMenuWeb_menuhook_MouseMove(e,menuid);
	}
	
	
	function TMenuWeb_document_MouseUp(e,menuid)
	{
		if(TMenuWeb_Browser()=="IE" || TMenuWeb_Browser()=="NS5")
		{
			TControl_getElementById(menuid).Moving=null;
		}
	}
	function TMenuWeb_bodyclick(e,menuid)
	{
		TMenuWeb_hideAllMenus(menuid);
	}
	

	function TMenuWeb_applyBorder(objCell, iSize, sTopLeftColor, sBottomRightColor, sStyle)
	{
		if (sStyle == null)
			sStyle = "solid";
		if (sTopLeftColor.length && sBottomRightColor.length)
		{
			objCell.style.borderTop = sStyle + " " + iSize + "px " + sTopLeftColor;
			objCell.style.borderLeft = sStyle + " " + iSize + "px " + sTopLeftColor;
			objCell.style.borderRight = sStyle + " " + iSize + "px " +sBottomRightColor;
			objCell.style.borderBottom = sStyle + " " + iSize + "px " + sBottomRightColor;	
		}
	}
	function TMenuWeb_applyRowStyle(menuid,sID, bSelected)
	{
		var objRow=TMenuWeb_getSourceTR(sID,menuid);
		var iSize=1;
		var sBGColor,sForeColor,sBorderColor,sIconBGColor,sIconBorderColor;
		var sStyle="solid";
		
		if(bSelected==true)
		{
			sBGColor=TMenuWeb_getAttribute(menuid,'SELECTEDCOLOR');
			sForeColor=TMenuWeb_getAttribute(menuid,'SELECTEDFORECOLOR');
			sIconBGColor=TMenuWeb_getAttribute(menuid,'SELECTEDCOLOR');
			sBorderColor=TMenuWeb_getAttribute(menuid,'SELECTEDBORDERCOLOR');
			sIconBorderColor=TMenuWeb_getAttribute(menuid,'SELECTEDBORDERCOLOR');
		}
		else
		{
			sBGColor=TMenuWeb_getAttribute(menuid,'ITEMBACKCOLOR');
			sForeColor=TMenuWeb_getAttribute(menuid,'ITEMFORECOLOR');
			sIconBGColor=TMenuWeb_getAttribute(menuid,'ICONBACKGROUNDCOLOR');
			sBorderColor=TMenuWeb_getAttribute(menuid,'ITEMBACKCOLOR');		
			sIconBorderColor=TMenuWeb_getAttribute(menuid,'ICONBACKGROUNDCOLOR');
		}
		if(sBorderColor=="") 
			sBorderColor=sBGColor;
		TControl_getElementById(menuid+"td" + sID).style.backgroundColor = sBGColor;
		TControl_getElementById(menuid+"icon" + sID).style.backgroundColor = sIconBGColor
		TControl_getElementById(menuid+"arrow" + sID).style.backgroundColor = sBGColor;
		TControl_getElementById(menuid+"td" + sID).style.color = sForeColor;
		TControl_getElementById(menuid+"icon" + sID).style.color = sForeColor;
		TControl_getElementById(menuid+"arrow" + sID).style.color = sForeColor;

		objRow.cells[0].style.borderLeft = sStyle + " " + iSize + "px " + sIconBorderColor;
		objRow.cells[0].style.borderTop = sStyle + " " + iSize + "px " + sIconBorderColor;
		objRow.cells[0].style.borderBottom = sStyle + " " + iSize + "px " + sIconBorderColor;
		objRow.cells[1].style.borderTop = sStyle + " " + iSize + "px " + sBorderColor;
		objRow.cells[1].style.borderBottom = sStyle + " " + iSize + "px " + sBorderColor;
		objRow.cells[2].style.borderRight = sStyle + " " + iSize + "px " + sBorderColor;
		objRow.cells[2].style.borderTop = sStyle + " " + iSize + "px " + sBorderColor;
		objRow.cells[2].style.borderBottom = sStyle + " " + iSize + "px " + sBorderColor;
		objRow.cells[0].style.borderRight = objRow.cells[0].style.backgroundColor;
		objRow.cells[1].style.borderLeft = objRow.cells[1].style.backgroundColor;
		objRow.cells[1].style.borderRight = objRow.cells[1].style.backgroundColor;
		objRow.cells[2].style.borderLeft = objRow.cells[2].style.backgroundColor;

	}
	function TMenuWeb_getElement(e, sID) 
	{
		var o=e;
		var i=0;
		while (o.id != sID)
		{
			o=o.parentNode;
			i++;
		}
		return o;
	}
	
	function TMenuWeb_handleNewItemSelect(menuid,sID)
	{
		var i;
		var iNewLength=-1;
		var bDeleteRest=false; 
		for (i=0; i<TMenuWeb_OpenMenus(menuid).length; i++)
		{		
			if (bDeleteRest)
				TControl_ShowElement(menuid+"tbl" + TMenuWeb_OpenMenus(menuid)[i],false);
			if (TMenuWeb_OpenMenus(menuid)[i] == sID)
			{
				bDeleteRest=true;
				iNewLength = i;
			}				
		}
		if (iNewLength != -1)
			TMenuWeb_OpenMenus(menuid).length = iNewLength+1;
	}
	function TMenuWeb_hideAllMenus(menuid)
	{
		var i;
		for (i=0; i<TMenuWeb_OpenMenus(menuid).length; i++)
		{	
			TControl_ShowElement(menuid+"tbl" + TMenuWeb_OpenMenus(menuid)[i],false);
		}
		TMenuWeb_OpenMenus(menuid).length = 0;
	}		
	function TMenuWeb_itemHasChildren(sID,menuid)
	{
		objTable = TControl_getElementById(menuid+"tbl" + sID);
		if (objTable != null)
		{
			if (objTable.rows != null)
			{
				if (objTable.rows.length > 0)
					return true;
				else
					return false;
			}		
		}
	}
	function TMenuWeb_getSourceTR(sID,menuid)
	{
			return TControl_getElementById(menuid+"tr" + sID);
	}
	
	function TMenuWeb_handlembi_mo(sID,menuid)
	{
		var objRow=TMenuWeb_getSourceTR(sID,menuid);
		TMenuWeb_applyRowStyle(menuid,sID, true);
		if(TMenuWeb_Browser()=="IE")
		{

			if (TMenuWeb_OpenMenus(menuid).join(',').indexOf(objRow.id.replace('tr', '')) == -1)
			{
				TMenuWeb_handleNewItemSelect(menuid,objRow.parentID);
			
				if (TControl_getElementById(menuid+"tbl" + sID) != null)
				{
					TControl_setPosition(menuid+"tbl" + sID,
                                                     TControl_elementLeft(objRow) + objRow.offsetWidth,
                                                     TControl_elementTop(objRow));
					objMenu = TControl_getElementById(menuid+"tbl" + sID);
					TMenuWeb_handleTransition(objMenu);
					TMenuWeb_OpenMenus(menuid)[TMenuWeb_OpenMenus(menuid).length] = sID;
					TControl_ShowElement(menuid+"tbl" + sID,true);
				}	
			}
		}
		else if(TMenuWeb_Browser()=="NS5")
		{
			TMenuWeb_handleNewItemSelect(menuid,objRow.getAttribute("parentID"));
			if (TControl_getElementById(menuid+"tbl" + sID) != null)
			{
				TControl_setPosition(menuid+"tbl" + sID,
                                                     TControl_elementLeft(objRow) + objRow.offsetWidth,
                                                     TControl_elementTop(objRow));
				TMenuWeb_OpenMenus(menuid)[TMenuWeb_OpenMenus(menuid).length] = sID;
				TControl_ShowElement(menuid+"tbl" + sID,true);
			}	
		}
	}
	
	function TMenuWeb_handlembi_mout(sID,menuid)
	{
			TMenuWeb_applyRowStyle(menuid,sID, false);
	}
		
	function TMenuWeb_positionMenu(objMenu, objCell,menuid)
	{
		if (TMenuWeb_getAttribute(menuid,'DISPLAYVERTICAL')=="true")
		{
			TControl_setPosition(objMenu.id, 
                           TControl_elementLeft(objCell) + objCell.offsetWidth,
			                     TControl_elementTop(objCell));
		}
		else
		{
			TControl_setPosition(objMenu.id, 
                           TControl_elementLeft(objCell), 
                           TControl_elementTop(objCell) + objCell.offsetHeight);
		}
	}
		


	function TMenuWeb_initMenu(menuid)
	{
		var menu=TControl_getElementById(menuid);
		var init=menu.getAttribute('initialized');
		if (init!='true')
			if(TMenuWeb_Browser()=="IE")
			{	
				menu.setAttribute('initialized','true');
				document.attachEvent("onmousemove", new Function("TMenuWeb_document_MouseMove(window.event,'"+menuid+"');"));
				document.attachEvent("onmouseup", new Function("TMenuWeb_document_MouseUp(window.event,'"+menuid+"');"));
				document.all.tags("BODY")[0].attachEvent("onclick", new Function("TMenuWeb_bodyclick(window.event,'"+menuid+"');"));
			}	
			else if(TMenuWeb_Browser()=="NS5")
			{
				menu.setAttribute('initialized','true');
			  window.addEventListener("click",new Function("e","TMenuWeb_bodyclick(e,'"+menuid+"');"),true);
				window.addEventListener("mousemove", new Function("e","TMenuWeb_document_MouseMove(e,'"+menuid+"');"), true);
				window.addEventListener("mouseup",   new Function("e","TMenuWeb_document_MouseUp(e,'"+menuid+"');"),   true);
			}
	}
		


