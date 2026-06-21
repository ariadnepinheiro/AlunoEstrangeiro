//*************************************************************************//

function _aspxIsExists(obj) {
    return (typeof (obj) != "undefined") && (obj != null);
}

//*************************************************************************//
ASPxClientEventArgs = _aspxCreateClass(null, {
    constructor: function() {
    }
});

ASPxClientEventArgs.Empty = new ASPxClientEventArgs();
ASPxClientProcessingModeEventArgs = _aspxCreateClass(ASPxClientEventArgs, {
    constructor: function(processOnServer) {
        this.constructor.prototype.constructor.call(this);
        this.processOnServer = processOnServer;
    }
});

//*************************************************************************//

function _aspxCreateClass(parentClass, properties) {
    var ret = function() {
        if (ret.preparing)
            return delete (ret.preparing);
        if (ret.constr) {
            this.constructor = ret;
            ret.constr.apply(this, arguments);
        }
    }
    ret.prototype = {};
    if (_aspxIsExists(parentClass)) {
        parentClass.preparing = true;
        ret.prototype = new parentClass;
        ret.prototype.constructor = parentClass;
        ret.constr = parentClass;
    }
    if (_aspxIsExists(properties)) {
        var constructorName = "constructor";
        for (var name in properties) {
            if (name != constructorName)
                ret.prototype[name] = properties[name];
        }
        if (properties[constructorName] && properties[constructorName] != Object)
            ret.constr = properties[constructorName];
    }
    return ret;
}


//*************************************************************************//

ASPxClientControl = _aspxCreateClass(null, {
    constructor: function(name) {
        this.isASPxClientControl = true;
        this.name = name;
        this.uniqueID = name;
        this.enabled = true;
        this.clientEnabled = true;
        this.clientVisible = true;
        this.autoPostBack = false;
        this.allowMultipleCallbacks = true;
        this.callBack = null;
        this.savedCallbacks = null;
        this.isNative = false;
        this.requestCount = 0;
        this.isInitialized = false;
        this.initialFocused = false;
        this.leadingAfterInitCall = false;
        this.sizeCorrectedOnce = false;
        this.serverEvents = [];
        this.dialogContentHashTable = {};
        this.sizeCorrectedOnce = false;
        this.loadingPanelElement = null;
        this.loadingDivElement = null;
        this.mainElement = null;
        this.renderIFrameForPopupElements = false;
        this.Init = new ASPxClientEvent();
        this.BeginCallback = new ASPxClientEvent();
        this.EndCallback = new ASPxClientEvent();
        this.CallbackError = new ASPxClientEvent();
        this.CustomDataCallback = new ASPxClientEvent();
        aspxGetControlCollection().Add(this);
    },
    Initialize: function() {
        if (this.callBack != null)
            this.InitializeCallBackData();
    },
    InlineInitialize: function() {
    },
    InitailizeFocus: function() {
        if (this.initialFocused && this.IsVisible())
            this.Focus();
    },
    AfterInitialize: function() {
        this.AdjustControl(__aspxCheckSizeCorrectedFlag);
        this.InitailizeFocus();
        this.isInitialized = true;
        this.RaiseInit();
        if (_aspxIsExists(this.savedCallbacks)) {
            for (var i = 0; i < this.savedCallbacks.length; i++)
                this.CreateCallbackInternal(this.savedCallbacks[i].arg, this.savedCallbacks[i].command,
     false, this.savedCallbacks[i].callbackInfo);
            this.savedCallbacks = null;
        }
    },
    InitializeCallBackData: function() {
    },
    CollapseControl: function(checkSizeCorrectedFlag) {
    },
    AdjustControl: function(checkSizeCorrectedFlag, nestedCall) {
        if (checkSizeCorrectedFlag && this.sizeCorrectedOnce || ASPxClientControl.adjustControlLocked && !nestedCall)
            return;
        ASPxClientControl.adjustControlLocked = true;
        try {
            var mainElement = this.GetMainElement();
            if (!_aspxIsExists(mainElement) || !this.IsDisplayed())
                return;
            this.AdjustControlCore();
            this.sizeCorrectedOnce = true;
        } finally {
            delete ASPxClientControl.adjustControlLocked;
        }
    },
    AdjustControlCore: function() {
    },
    OnBrowserWindowResize: function(evt) {
    },
    RegisterServerEventAssigned: function(eventNames) {
        for (var i = 0; i < eventNames.length; i++)
            this.serverEvents[eventNames[i]] = true;
    },
    IsServerEventAssigned: function(eventName) {
        return _aspxIsExists(this.serverEvents[eventName]);
    },
    GetChild: function(idPostfix) {
        var mainElement = this.GetMainElement();
        return _aspxIsExists(mainElement) ? _aspxGetChildById(mainElement, this.name + idPostfix) : null;
    },
    GetItemElementName: function(element) {
        var name = "";
        if (_aspxIsExists(element.id))
            name = element.id.substring(this.name.length + 1);
        return name;
    },
    GetLinkElement: function(element) {
        if (element == null) return null;
        return (element.tagName == "A") ? element : _aspxGetChildByTagName(element, "A", 0);
    },
    GetInternalHyperlinkElement: function(parentElement, index) {
        var element = _aspxGetChildByTagName(parentElement, "A", index);
        if (element == null)
            element = _aspxGetChildByTagName(parentElement, "SPAN", index);
        return element;
    },
    GetMainElement: function() {
        if (!_aspxIsExistsElement(this.mainElement))
            this.mainElement = _aspxGetElementById(this.name);
        return this.mainElement;
    },
    IsRightToLeft: function() {
        return _aspxIsElementRigthToLeft(this.GetMainElement());
    },
    OnControlClick: function(clickedElement, htmlEvent) {
    },
    GetLoadingPanelElement: function() {
        return _aspxGetElementById(this.name + "_LP");
    },
    CloneLoadingPanel: function(element, parent) {
        var clone = element.cloneNode(true);
        clone.id = element.id + "V";
        parent.appendChild(clone);
        return clone;
    },
    CreateLoadingPanelInsideContainer: function(parentElement) {
        if (parentElement == null) return null;
        this.HideLoadingPanel();
        var element = this.GetLoadingPanelElement();
        if (element != null) {
            var width = 0;
            var height = 0;
            var itemsTable = _aspxGetChildByTagName(parentElement, "TABLE", 0);
            if (itemsTable != null) {
                width = itemsTable.offsetWidth;
                height = itemsTable.offsetHeight;
            } else if (parentElement.childNodes.length == 0) {
                var dummyDiv = document.createElement("DIV");
                parentElement.appendChild(dummyDiv);
                width = dummyDiv.offsetWidth;
            } else {
                width = parentElement.clientWidth;
                height = parentElement.clientHeight;
            }
            parentElement.innerHTML = "";
            var table = document.createElement("TABLE");
            parentElement.appendChild(table);
            table.border = 0;
            table.cellPadding = 0;
            table.cellSpacing = 0;
            table.style.height = (height > 0) ? height + "px" : "100%";
            table.style.width = (width > 0) ? width + "px" : "100%";
            var tbody = document.createElement("TBODY");
            table.appendChild(tbody);
            var tr = document.createElement("TR");
            tbody.appendChild(tr);
            var td = document.createElement("TD");
            tr.appendChild(td);
            td.align = "center";
            td.vAlign = "middle";
            element = this.CloneLoadingPanel(element, td);
            _aspxSetElementDisplay(element, true);
            this.loadingPanelElement = element;
            return element;
        } else
            parentElement.innerHTML = "&nbsp;";
        return null;
    },
    CreateLoadingPanelWithAbsolutePosition: function(parentElement, offsetElement) {
        if (parentElement == null) return null;
        this.HideLoadingPanel();
        if (!_aspxIsExists(offsetElement))
            offsetElement = parentElement;
        var element = this.GetLoadingPanelElement();
        if (element != null) {
            element = this.CloneLoadingPanel(element, parentElement);
            element.style.position = "absolute";
            _aspxSetElementDisplay(element, true);
            this.SetLoadingPanelLocation(offsetElement, element);
            this.loadingPanelElement = element;
            return element;
        }
        return null;
    },
    CreateLoadingPanelInline: function(parentElement) {
        if (parentElement == null) return null;
        this.HideLoadingPanel();
        var element = this.GetLoadingPanelElement();
        if (element != null) {
            element = this.CloneLoadingPanel(element, parentElement);
            _aspxSetElementDisplay(element, true);
            this.loadingPanelElement = element;
            return element;
        }
        return null;
    },
    HideLoadingPanel: function() {
        if (_aspxIsExistsElement(this.loadingPanelElement)) {
            _aspxRemoveElement(this.loadingPanelElement);
            this.loadingPanelElement = null;
        }
    },
    SetLoadingPanelLocation: function(offsetElement, loadingPanel, x, y, offsetX, offsetY) {
        if (!_aspxIsExists(x) || !_aspxIsExists(y)) {
            var x1 = _aspxGetAbsoluteX(offsetElement) - _aspxGetIEDocumentClientOffset(true);
            var y1 = _aspxGetAbsoluteY(offsetElement) - _aspxGetIEDocumentClientOffset(false);
            var x2 = x1;
            var y2 = y1;
            if (offsetElement == document.body) {
                x2 += _aspxGetDocumentMaxClientWidth();
                y2 += _aspxGetDocumentMaxClientHeight();
            }
            else {
                x2 += offsetElement.offsetWidth;
                y2 += offsetElement.offsetHeight;
            }
            if (x1 < _aspxGetDocumentScrollLeft())
                x1 = _aspxGetDocumentScrollLeft();
            if (y1 < _aspxGetDocumentScrollTop())
                y1 = _aspxGetDocumentScrollTop();
            if (x2 > _aspxGetDocumentScrollLeft() + _aspxGetDocumentClientWidth())
                x2 = _aspxGetDocumentScrollLeft() + _aspxGetDocumentClientWidth();
            if (y2 > _aspxGetDocumentScrollTop() + _aspxGetDocumentClientHeight())
                y2 = _aspxGetDocumentScrollTop() + _aspxGetDocumentClientHeight();
            x = x1 + ((x2 - x1 - loadingPanel.offsetWidth) / 2);
            y = y1 + ((y2 - y1 - loadingPanel.offsetHeight) / 2);
        }
        if (_aspxIsExists(offsetX) && _aspxIsExists(offsetY)) {
            x += offsetX;
            y += offsetY;
        }
        loadingPanel.style.left = _aspxPrepareClientPosForElement(x, loadingPanel, true) + "px";
        loadingPanel.style.top = _aspxPrepareClientPosForElement(y, loadingPanel, false) + "px";
    },
    GetLoadingDiv: function() {
        return _aspxGetElementById(this.name + "_LD");
    },
    CreateLoadingDiv: function(parentElement, offsetElement) {
        if (parentElement == null) return null;
        this.HideLoadingDiv();
        if (!_aspxIsExists(offsetElement))
            offsetElement = parentElement;
        var div = this.GetLoadingDiv();
        if (div != null) {
            div = div.cloneNode(true);
            parentElement.appendChild(div);
            _aspxSetElementDisplay(div, true);
            this.SetLoadingDivBounds(offsetElement, div);
            this.loadingDivElement = div;
            return div;
        }
        return null;
    },
    SetLoadingDivBounds: function(offsetElement, loadingDiv) {
        var absX = (offsetElement == document.body) ? 0 : _aspxGetAbsoluteX(offsetElement);
        var absY = (offsetElement == document.body) ? 0 : _aspxGetAbsoluteY(offsetElement);
        loadingDiv.style.left = _aspxPrepareClientPosForElement(absX, loadingDiv, true) + "px";
        loadingDiv.style.top = _aspxPrepareClientPosForElement(absY, loadingDiv, false) + "px";
        var width = (offsetElement == document.body) ? _aspxGetDocumentWidth() : offsetElement.offsetWidth;
        var height = (offsetElement == document.body) ? _aspxGetDocumentHeight() : offsetElement.offsetHeight;
        _aspxSetStyleSize(loadingDiv, width, height);
        var correctedWidth = 2 * width - loadingDiv.offsetWidth;
        if (correctedWidth <= 0) correctedWidth = width;
        var correctedHeight = 2 * height - loadingDiv.offsetHeight;
        if (correctedHeight <= 0) correctedHeight = height;
        _aspxSetStyleSize(loadingDiv, correctedWidth, correctedHeight);
    },
    HideLoadingDiv: function() {
        if (_aspxIsExistsElement(this.loadingDivElement)) {
            _aspxRemoveElement(this.loadingDivElement);
            this.loadingDivElement = null;
        }
    },
    RaiseInit: function() {
        if (!this.Init.IsEmpty()) {
            var args = new ASPxClientEventArgs();
            this.Init.FireEvent(this, args);
        }
    },
    RaiseBeginCallback: function(command) {
        if (!this.BeginCallback.IsEmpty()) {
            var args = new ASPxClientBeginCallbackEventArgs(command);
            this.BeginCallback.FireEvent(this, args);
        }
        if (_aspxIsExistsType(typeof (aspxGetGlobalEvents)))
            aspxGetGlobalEvents().OnBeginCallback(this, command);
    },
    RaiseEndCallback: function() {
        if (!this.EndCallback.IsEmpty()) {
            var args = new ASPxClientEndCallbackEventArgs();
            this.EndCallback.FireEvent(this, args);
        }
        if (_aspxIsExistsType(typeof (aspxGetGlobalEvents)))
            aspxGetGlobalEvents().OnEndCallback(this);
    },
    RaiseCallbackError: function(message) {
        if (!this.CallbackError.IsEmpty()) {
            var args = new ASPxClientCallbackErrorEventArgs(message);
            this.CallbackError.FireEvent(this, args);
            if (args.handled)
                return { isHandled: true, errorMessage: args.message };
        }
        if (_aspxIsExistsType(typeof (aspxGetGlobalEvents))) {
            var args = new ASPxClientCallbackErrorEventArgs(message);
            aspxGetGlobalEvents().OnCallbackError(this, args);
            if (args.handled)
                return { isHandled: true, errorMessage: args.message };
        }
        return { isHandled: false, errorMessage: message };
    },
    IsVisible: function() {
        var element = this.GetMainElement();
        while (_aspxIsExists(element) && element.tagName != "BODY") {
            if (!_aspxGetElementVisibility(element) || !_aspxGetElementDisplay(element))
                return false;
            element = element.parentNode;
        }
        return true;
    },
    IsDisplayed: function() {
        var element = this.GetMainElement();
        while (_aspxIsExists(element) && element.tagName != "BODY") {
            if (!_aspxGetElementDisplay(element))
                return false;
            element = element.parentNode;
        }
        return true;
    },
    Focus: function() {
    },
    GetClientVisible: function() {
        return this.GetVisible();
    },
    SetClientVisible: function(visible) {
        this.SetVisible(visible);
    },
    GetVisible: function() {
        return this.clientVisible;
    },
    SetVisible: function(visible) {
        if (this.clientVisible != visible) {
            this.clientVisible = visible;
            _aspxSetElementDisplay(this.GetMainElement(), visible);
            if (visible) {
                this.AdjustControl(__aspxCheckSizeCorrectedFlag);
                var mainElement = this.GetMainElement();
                if (_aspxIsExists(mainElement))
                    aspxGetControlCollection().AdjustControls(mainElement, __aspxCheckSizeCorrectedFlag);
            }
        }
    },
    InCallback: function() {
        return this.requestCount > 0;
    },
    DoBeginCallback: function(command) {
        if (!_aspxIsExists(command)) command = "";
        this.RaiseBeginCallback(command);
        aspxGetControlCollection().Before_WebForm_InitCallback();
        if (_aspxIsExistsType(typeof (WebForm_InitCallback)) && _aspxIsExists(WebForm_InitCallback)) {
            __theFormPostData = "";
            __theFormPostCollection = new Array();
            this.ClearPostBackEventInput("__EVENTTARGET");
            this.ClearPostBackEventInput("__EVENTARGUMENT");
            WebForm_InitCallback();
            this.savedFormPostData = __theFormPostData;
            this.savedFormPostCollection = __theFormPostCollection;
        }
    },
    ClearPostBackEventInput: function(id) {
        var element = _aspxGetElementById(id);
        if (element != null) element.value = "";
    },
    PerformDataCallback: function(arg, handler) {
        this.CreateCustomDataCallback(arg, "", handler);
    },
    CreateCallback: function(arg, command) {
        var callbackInfo = this.CreateCallbackInfo(ASPxCallbackType.Common, null);
        this.CreateCallbackByInfo(arg, command, callbackInfo);
    },
    CreateCustomDataCallback: function(arg, command, handler) {
        var callbackInfo = this.CreateCallbackInfo(ASPxCallbackType.Data, handler);
        this.CreateCallbackByInfo(arg, command, callbackInfo);
    },
    CreateCallbackByInfo: function(arg, command, callbackInfo) {
        if (_aspxIsExistsType(typeof (WebForm_DoCallback)) && _aspxIsExists(WebForm_DoCallback))
            this.CreateCallbackInternal(arg, command, true, callbackInfo);
        else {
            if (!_aspxIsExists(this.savedCallbacks))
                this.savedCallbacks = [];
            this.savedCallbacks.push({ arg: arg, command: command, callbackInfo: callbackInfo });
        }
    },
    CreateCallbackInternal: function(arg, command, viaTimer, callbackInfo) {
        if (!this.CanCreateCallback())
            return;
        this.requestCount++;
        this.DoBeginCallback(command);
        if (typeof (arg) == "undefined")
            arg = "";
        if (typeof (command) == "undefined")
            command = "";
        var callbackID = this.SaveCallbackInfo(callbackInfo);
        if (viaTimer)
            window.setTimeout("aspxCreateCallback('" + this.name + "', '" + escape(arg) + "', '" + escape(command) + "', " + callbackID + ");", 0);
        else
            this.CreateCallbackCore(arg, command, callbackID);
    },
    CreateCallbackCore: function(arg, command, callbackID) {
        __theFormPostData = this.savedFormPostData;
        __theFormPostCollection = this.savedFormPostCollection;
        this.callBack(this.GetSerializedCallbackInfoByID(callbackID) + arg);
    },
    CanCreateCallback: function() {
        return this.allowMultipleCallbacks || !this.InCallback();
    },
    DoLoadCallbackScripts: function() {
        _aspxProcessScriptsAndLinks(this.name);
    },
    DoEndCallback: function() {
        this.RaiseEndCallback();
    },
    DoFinalizeCallback: function() {
    },
    HideLoadingPanelOnCallback: function() {
        return true;
    },
    DoCallback: function(result) {
        this.requestCount--;
        if (this.HideLoadingPanelOnCallback()) {
            this.HideLoadingDiv();
            this.HideLoadingPanel();
        }
        if (result.indexOf(__aspxCallbackResultPrefix) != 0)
            this.ProcessCallbackGeneralError(result);
        else {
            var resultObj = null;
            try {
                resultObj = eval(result);
            }
            catch (e) {
            }
            if (_aspxIsExists(resultObj)) {
                if (_aspxIsExists(resultObj.redirect))
                    window.location.href = resultObj.redirect;
                else if (_aspxIsExists(resultObj.generalError))
                    this.ProcessCallbackGeneralError(resultObj.generalError);
                else {
                    var errorObj = resultObj.error;
                    if (_aspxIsExists(errorObj))
                        this.ProcessCallbackError(errorObj);
                    else {
                        if (resultObj.cp) {
                            for (var name in resultObj.cp)
                                this[name] = resultObj.cp[name];
                        }
                        var callbackInfo = this.DequeueCallbackInfo(resultObj.id);
                        if (callbackInfo.type == ASPxCallbackType.Data)
                            this.ProcessCustomDataCallback(resultObj.result, callbackInfo);
                        else
                            this.ProcessCallback(resultObj.result);
                    }
                    this.DoLoadCallbackScripts();
                }
            }
        }
    },
    DoCallbackError: function(result) {
        this.HideLoadingDiv();
        this.HideLoadingPanel();
        this.OnCallbackGeneralError(result);
    },
    DoControlClick: function(evt) {
        this.OnControlClick(_aspxGetEventSource(evt), evt);
    },
    ProcessCallback: function(result) {
        this.OnCallback(result);
    },
    ProcessCustomDataCallback: function(result, callbackInfo) {
        if (callbackInfo.handler != null)
            callbackInfo.handler(this, result);
        this.RaiseCustomDataCallback(result);
    },
    RaiseCustomDataCallback: function(result) {
        if (!this.CustomDataCallback.IsEmpty()) {
            var arg = new ASPxClientCustomDataCallbackEventArgs(result);
            this.CustomDataCallback.FireEvent(this, arg);
        }
    },
    OnCallback: function(result) {
    },
    CreateCallbackInfo: function(type, handler) {
        return { type: type, handler: handler };
    },
    GetSerializedCallbackInfoByID: function(callbackID) {
        return this.GetCallbackInfoByID(callbackID).type + callbackID + __aspxCallbackSeparator;
    },
    SaveCallbackInfo: function(callbackInfo) {
        var activeCallbacksInfo = this.GetActiveCallbacksInfo();
        for (var i = 0; i < activeCallbacksInfo.length; i++) {
            if (activeCallbacksInfo[i] == null) {
                activeCallbacksInfo[i] = callbackInfo;
                return i;
            }
        }
        activeCallbacksInfo.push(callbackInfo);
        return activeCallbacksInfo.length - 1;
    },
    GetActiveCallbacksInfo: function() {
        var persistentProperties = this.GetPersistentProperties();
        if (!persistentProperties.activeCallbacks)
            persistentProperties.activeCallbacks = [];
        return persistentProperties.activeCallbacks;
    },
    GetPersistentProperties: function() {
        var storage = _aspxGetPersistentControlPropertiesStorage();
        var persistentProperties = storage[this.name];
        if (!persistentProperties) {
            persistentProperties = {};
            storage[this.name] = persistentProperties;
        }
        return persistentProperties;
    },
    GetCallbackInfoByID: function(callbackID) {
        return this.GetActiveCallbacksInfo()[callbackID];
    },
    DequeueCallbackInfo: function(index) {
        var activeCallbacksInfo = this.GetActiveCallbacksInfo();
        if (index < 0 || index >= activeCallbacksInfo.length)
            return null;
        var result = activeCallbacksInfo[index];
        activeCallbacksInfo[index] = null;
        return result;
    },
    ProcessCallbackError: function(errorObj) {
        var data = _aspxIsExists(errorObj.data) ? errorObj.data : null;
        var result = this.RaiseCallbackError(errorObj.message);
        if (!result.isHandled)
            this.OnCallbackError(result.errorMessage, data);
    },
    OnCallbackError: function(errorMessage, data) {
        if (errorMessage)
            alert(errorMessage);
    },
    ProcessCallbackGeneralError: function(errorMessage) {
        var result = this.RaiseCallbackError(errorMessage);
        if (!result.isHandled)
            this.OnCallbackGeneralError(result.errorMessage);
    },
    OnCallbackGeneralError: function(errorMessage) {
        this.OnCallbackError(errorMessage, null);
    },
    SendPostBack: function(params) {
        __doPostBack(this.uniqueID, params);
    }
});





//*************************************************************************//

ASPxClientMenuBase = _aspxCreateClass(ASPxClientControl, {
    constructor: function(name) {
        this.constructor.prototype.constructor.call(this, name);
        this.allowSelectItem = false;
        this.allowCheckItems = false;
        this.appearAfter = 300;
        this.animationDelay = 30;
        this.animationMaxDelay = 400;
        this.disappearAfter = 500;
        this.enableAnimation = true;
        this.checkedItems = new Array();
        this.itemCheckedGroups = new Array();
        this.lockHoverEvents = false;
        this.popupToLeft = false;
        this.popupCount = 0;
        this.rootItem = null;
        this.showSubMenus = false;
        this.savedCallbackHoverItem = null;
        this.savedCallbackHoverElement = null;
        this.rootSubMenuFIXOffset = 0;
        this.rootSubMenuFIYOffset = 0;
        this.rootSubMenuLIXOffset = 0;
        this.rootSubMenuLIYOffset = 0;
        this.rootSubMenuXOffset = 0;
        this.rootSubMenuYOffset = 0;
        this.subMenuFIXOffset = 0;
        this.subMenuFIYOffset = 0;
        this.subMenuLIXOffset = 0;
        this.subMenuLIYOffset = 0;
        this.subMenuXOffset = 0;
        this.subMenuYOffset = 0;
        this.ItemClick = new ASPxClientEvent();
        this.ItemMouseOver = new ASPxClientEvent();
        this.ItemMouseOut = new ASPxClientEvent();
        this.PopUp = new ASPxClientEvent();
        this.CloseUp = new ASPxClientEvent();
        aspxGetMenuCollection().Add(this);
    },
    InlineInitialize: function() {
        this.InitializeInternal(true);
        if (this.IsCallbacksEnabled()) {
            this.showSubMenus = this.GetLoadingPanelElement() != null;
            this.CreateCallback("DXMENUCONTENT");
        }
        else
            this.showSubMenus = true;
    },
    InitializeInternal: function(inline) {
        this.InitializeCheckedItems();
        this.InitializeSelectedItem();
        this.InitializeEnabledAndVisible(!inline || !this.IsCallbacksEnabled());
    },
    InitializeEnabledAndVisible: function(recursive) {
        if (this.rootItem == null) return;
        for (var i = 0; i < this.rootItem.items.length; i++)
            this.rootItem.items[i].InitializeEnabledAndVisible(recursive);
    },
    IsCallbacksEnabled: function() {
        return _aspxIsFunction(this.callBack);
    },
    GetMenuElement: function(indexPath) {
        return _aspxGetElementById(this.name + __aspxMMIdSuffix + indexPath + "_");
    },
    GetMenuIFrameElement: function(indexPath) {
        var level = this.GetMenuLevel(indexPath);
        return _aspxGetElementById(this.name + "_DXMIF" + level);
    },
    GetMenuBorderCorrectorElement: function(indexPath) {
        return _aspxGetElementById(this.name + "_DXMBC" + indexPath + "_");
    },
    GetMenuMainCell: function(element) {
        return this.GetMenuMainTable(element).rows[0].cells[0];
    },
    GetMenuMainTable: function(element) {
        var indexPath = this.GetIndexPathById(element.id, true);
        var shadowTable = _aspxGetElementById(this.name + "_DXMST" + indexPath + "_");
        return shadowTable != null ? shadowTable : element;
    },
    GetItemElement: function(indexPath) {
        return _aspxGetElementById(this.name + __aspxMIIdSuffix + indexPath + "_");
    },
    GetItemTemplateCell: function(indexPath) {
        return _aspxGetElementById(this.name + __aspxMIIdSuffix + indexPath + "_ITC");
    },
    GetItemImageCell: function(indexPath) {
        return _aspxGetElementById(this.name + __aspxMIIdSuffix + indexPath + "_I");
    },
    GetItemIndentCell: function(indexPath) {
        return _aspxGetElementById(this.name + __aspxMIIdSuffix + indexPath + "_N");
    },
    GetItemTextCell: function(indexPath) {
        return _aspxGetElementById(this.name + __aspxMIIdSuffix + indexPath + "_T");
    },
    GetItemPopOutImageCell: function(indexPath) {
        return _aspxGetElementById(this.name + __aspxMIIdSuffix + indexPath + "_P");
    },
    GetItemTextOrImageCell: function(indexPath) {
        var element = this.GetItemTextCell(indexPath);
        if (element == null)
            element = this.GetItemImageCell(indexPath);
        return element;
    },
    GetSelectedItemInputElement: function() {
        return _aspxGetElementById(this.name + "SI");
    },
    GetCheckedItemsInputElement: function() {
        return _aspxGetElementById(this.name + "CI");
    },
    GetSubMenuXPosition: function(indexPath, menuElement) {
        var position = 0;
        var element = null;
        var imageElement = this.GetItemImageCell(indexPath);
        var textElement = this.GetItemTextCell(indexPath);
        var popOutImageElement = this.GetItemPopOutImageCell(indexPath);
        var imagePos = _aspxGetAbsoluteX(imageElement);
        var textPos = _aspxGetAbsoluteX(textElement);
        var popOutImagePos = _aspxGetAbsoluteX(popOutImageElement);
        if (imageElement != null || textElement != null || popOutImageElement != null) {
            if (this.IsVertical(indexPath)) {
                position = __aspxAbsoluteLeftPosition;
                if (imageElement != null && position < imagePos) {
                    position = imagePos;
                    element = imageElement;
                }
                if (textElement != null && position < textPos) {
                    position = textPos;
                    element = textElement;
                }
                if (popOutImageElement != null && position < popOutImagePos) {
                    position = popOutImagePos;
                    element = popOutImageElement;
                }
                position = _aspxGetAbsoluteX(element) + element.clientWidth;
            }
            else {
                position = __aspxAbsoluteRightPosition;
                if (imageElement != null && position > imagePos) {
                    position = imagePos;
                    element = imageElement;
                }
                if (textElement != null && position > textPos) {
                    position = textPos;
                    element = textElement;
                }
                if (popOutImageElement != null && position > popOutImagePos) {
                    position = popOutImagePos;
                    element = popOutImageElement;
                }
            }
            if (element != null && _aspxIsExistsAbsolutePosParent(element))
                position -= _aspxGetIEDocumentClientOffset(true);
        }
        return position;
    },
    GetSubMenuYPosition: function(indexPath, menuElement) {
        var position = 0;
        var element = this.GetItemTextOrImageCell(indexPath);
        if (element != null) {
            if (this.IsVertical(indexPath)) {
                position = _aspxGetAbsoluteY(element);
            }
            else {
                if (__aspxNetscapeFamily || __aspxOpera && __aspxBrowserVersion >= 9 || __aspxSafari && __aspxBrowserVersion >= 3 || __aspxChrome)
                    position = _aspxGetAbsoluteY(element) + element.offsetHeight - _aspxGetClientTop(element);
                else if (__aspxWebKitFamily)
                    position = _aspxGetAbsoluteY(element) + element.offsetHeight + element.offsetTop - _aspxGetClientTop(element);
                else
                    position = _aspxGetAbsoluteY(element) + element.clientHeight + _aspxGetClientTop(element);
            }
            if (_aspxIsExistsAbsolutePosParent(element))
                position -= _aspxGetIEDocumentClientOffset(false);
        }
        return position;
    },
    GetClientSubMenuXPosition: function(element, x, indexPath) {
        var itemInfo = new ASPxClientMenuItemInfo(this, indexPath);
        var itemWidth = itemInfo.clientWidth;
        var itemOffsetWidth = itemInfo.offsetWidth;
        var menuWidth = this.GetMenuMainCell(element).offsetWidth;
        var bodyWidth = _aspxGetDocumentClientWidth();
        if (this.IsVertical(indexPath)) {
            var left = x - _aspxGetDocumentScrollLeft();
            var right = left + menuWidth;
            var toLeftX = x - menuWidth - itemWidth;
            var toLeftLeft = left - menuWidth - itemWidth;
            var toLeftRight = right - menuWidth - itemWidth;
            if (this.popupToLeft) {
                if (toLeftLeft < 0 && toLeftLeft < bodyWidth - right) {
                    this.popupToLeft = false;
                    return x;
                }
                else
                    return toLeftX;
            }
            else {
                if (bodyWidth - right < 0 && bodyWidth - right < toLeftLeft) {
                    this.popupToLeft = true;
                    return toLeftX;
                }
                else
                    return x;
            }
        }
        else {
            var left = x - _aspxGetDocumentScrollLeft();
            var right = left + menuWidth;
            var toLeftX = x - menuWidth + itemOffsetWidth;
            var toLeftLeft = left - menuWidth + itemOffsetWidth;
            var toLeftRight = right - menuWidth + itemOffsetWidth;
            if (this.popupToLeft) {
                if (toLeftLeft < 0 && toLeftLeft < bodyWidth - right) {
                    this.popupToLeft = false;
                    return x;
                }
                else
                    return toLeftX;
            }
            else {
                if (bodyWidth - right < 0 && bodyWidth - right < toLeftLeft) {
                    this.popupToLeft = true;
                    return toLeftX;
                }
                else
                    return x;
            }
        }
    },
    GetClientSubMenuYPosition: function(element, y, indexPath) {
        var itemInfo = new ASPxClientMenuItemInfo(this, indexPath);
        var itemHeight = itemInfo.clientHeight;
        var itemOffsetHeight = itemInfo.offsetHeight;
        var menuHeight = this.GetMenuMainCell(element).offsetHeight;
        var top = y - _aspxGetDocumentScrollTop();
        var bottom = top + menuHeight;
        var bodyHeight = _aspxGetDocumentClientHeight();
        if (this.IsVertical(indexPath)) {
            menuHeight -= itemOffsetHeight - itemHeight;
            if (bottom > bodyHeight && top - menuHeight + itemHeight > bodyHeight - bottom)
                return y - menuHeight + itemHeight;
        }
        else {
            if (bottom > bodyHeight && top - menuHeight - itemHeight > bodyHeight - bottom)
                return y - menuHeight - itemHeight;
        }
        return y;
    },
    HasChildren: function(indexPath) {
        return (this.GetMenuElement(indexPath) != null);
    },
    IsVertical: function(indexPath) {
        return true;
    },
    IsRootItem: function(indexPath) {
        return this.GetMenuLevel(indexPath) <= 1;
    },
    IsParentElementPositionStatic: function(indexPath) {
        return this.IsRootItem(indexPath);
    },
    GetItemIndexPath: function(indexes) {
        return aspxGetMenuCollection().GetItemIndexPath(indexes);
    },
    GetItemIndexes: function(indexPath) {
        return aspxGetMenuCollection().GetItemIndexes(indexPath);
    },
    GetItemIndexPathById: function(id) {
        return aspxGetMenuCollection().GetIndexPathById(id, false);
    },
    GetMenuIndexPathById: function(id) {
        return aspxGetMenuCollection().GetIndexPathById(id, true);
    },
    GetIndexPathById: function(id, checkMenu) {
        var indexPath = this.GetItemIndexPathById(id);
        if (indexPath == "" && checkMenu)
            indexPath = this.GetMenuIndexPathById(id);
        return indexPath;
    },
    GetMenuLevel: function(indexPath) {
        return aspxGetMenuCollection().GetMenuLevel(indexPath);
    },
    GetParentIndexPath: function(indexPath) {
        var indexes = this.GetItemIndexes(indexPath);
        indexes.length--;
        return (indexes.length > 0) ? this.GetItemIndexPath(indexes) : "";
    },
    GetFirstChildIndexPath: function(indexPath) {
        var indexes = this.GetItemIndexes(indexPath);
        indexes[indexes.length] = 0;
        var newIndexPath = this.GetItemIndexPath(indexes);
        return this.GetFirstSiblingIndexPath(newIndexPath);
    },
    GetFirstSiblingIndexPath: function(indexPath) {
        var indexes = this.GetItemIndexes(indexPath);
        var i = 0;
        while (true) {
            indexes[indexes.length - 1] = i;
            var newIndexPath = this.GetItemIndexPath(indexes);
            if (!this.IsItemExist(newIndexPath))
                return null;
            if (this.IsItemExistAndEnabled(newIndexPath))
                return newIndexPath;
            i++;
        }
        return null;
    },
    GetLastSiblingIndexPath: function(indexPath) {
        var indexes = this.GetItemIndexes(indexPath);
        var newIndexPath = null;
        var i = indexes[indexes.length - 1] + 1;
        while (true) {
            indexes[indexes.length - 1] = i;
            var nextIndexPath = this.GetItemIndexPath(indexes);
            if (!this.IsItemExist(nextIndexPath))
                return newIndexPath;
            if (this.IsItemExistAndEnabled(nextIndexPath))
                newIndexPath = nextIndexPath;
            i++;
        }
        return null;
    },
    GetNextSiblingIndexPath: function(indexPath) {
        if (this.IsLastItem(indexPath)) return null;
        var indexes = this.GetItemIndexes(indexPath);
        var i = indexes[indexes.length - 1] + 1;
        while (true) {
            indexes[indexes.length - 1] = i;
            var newIndexPath = this.GetItemIndexPath(indexes);
            if (!this.IsItemExist(newIndexPath))
                return null;
            if (this.IsItemExistAndEnabled(newIndexPath))
                return newIndexPath;
            i++;
        }
        return null;
    },
    GetPrevSiblingIndexPath: function(indexPath) {
        if (this.IsFirstItem(indexPath)) return null;
        var indexes = this.GetItemIndexes(indexPath);
        var i = indexes[indexes.length - 1] - 1;
        while (true) {
            indexes[indexes.length - 1] = i;
            var newIndexPath = this.GetItemIndexPath(indexes);
            if (!this.IsItemExist(newIndexPath))
                return null;
            if (this.IsItemExistAndEnabled(newIndexPath))
                return newIndexPath;
            i--;
        }
        return null;
    },
    IsLastElement: function(element) {
        return _aspxIsExists(element) && (!_aspxIsExists(element.nextSibling) || !_aspxIsExists(element.nextSibling.tagName));
    },
    IsLastItem: function(indexPath) {
        if (this.IsVertical(indexPath)) {
            var itemElement = this.GetItemElement(indexPath);
            return this.IsLastElement(itemElement);
        }
        else {
            var imageCell = this.GetItemImageCell(indexPath);
            if (this.IsLastElement(imageCell))
                return true;
            var textCell = this.GetItemTextCell(indexPath);
            if (this.IsLastElement(textCell))
                return true;
            var popOutImageCell = this.GetItemPopOutImageCell(indexPath);
            if (this.IsLastElement(popOutImageCell))
                return true;
            return false;
        }
    },
    IsFirstElement: function(element) {
        return _aspxIsExists(element) && (!_aspxIsExists(element.previousSibling) || !_aspxIsExists(element.previousSibling.tagName));
    },
    IsFirstItem: function(indexPath) {
        if (this.IsVertical(indexPath)) {
            var itemElement = this.GetItemElement(indexPath);
            return this.IsFirstElement(itemElement);
        }
        else {
            var imageCell = this.GetItemImageCell(indexPath);
            if (this.IsFirstElement(imageCell))
                return true;
            var textCell = this.GetItemTextCell(indexPath);
            if (this.IsFirstElement(textCell))
                return true;
            var popOutImageCell = this.GetItemPopOutImageCell(indexPath);
            if (this.IsFirstElement(popOutImageCell))
                return true;
            return false;
        }
    },
    IsItemExist: function(indexPath) {
        return _aspxIsExists(this.GetItemTextOrImageCell(indexPath));
    },
    IsItemExistAndEnabled: function(indexPath) {
        var cell = this.GetItemTextOrImageCell(indexPath);
        if (_aspxIsExists(cell)) {
            var link = _aspxGetChildByTagName(cell, "A", 0);
            if (link != null)
                return !_aspxIsExists(cell.enabled) || cell.enabled;
        }
        return false;
    },
    GetClientSubMenuPos: function(element, indexPath, pos, isXPos) {
        if (!_aspxGetIsValidPosition(pos)) {
            pos = isXPos ? this.GetSubMenuXPosition(indexPath, element) :
    this.GetSubMenuYPosition(indexPath, element);
            if (__aspxWebKitFamily) {
                if (!this.IsParentElementPositionStatic(indexPath))
                    pos -= isXPos ? document.body.offsetLeft : document.body.offsetTop;
            }
        }
        var clientPos = isXPos ? this.GetClientSubMenuXPosition(element, pos, indexPath) :
   this.GetClientSubMenuYPosition(element, pos, indexPath);
        var isInverted = pos != clientPos;
        var offset = isXPos ? this.GetSubMenuXOffset(indexPath) : this.GetSubMenuYOffset(indexPath);
        clientPos += isInverted ? -offset : offset;
        clientPos -= _aspxGetPositionElementOffset(this.GetMenuElement(indexPath), isXPos);
        return new _aspxPopupPosition(clientPos, isInverted);
    },
    GetSubMenuXOffset: function(indexPath) {
        if (indexPath == "")
            return 0;
        else if (this.IsRootItem(indexPath)) {
            if (this.IsFirstItem(indexPath))
                return this.rootSubMenuFIXOffset;
            else if (this.IsLastItem(indexPath))
                return this.rootSubMenuLIXOffset;
            else
                return this.rootSubMenuXOffset;
        }
        else {
            if (this.IsFirstItem(indexPath))
                return this.subMenuFIXOffset;
            else if (this.IsLastItem(indexPath))
                return this.subMenuLIXOffset;
            else
                return this.subMenuXOffset;
        }
    },
    GetSubMenuYOffset: function(indexPath) {
        if (indexPath == "")
            return 0;
        else if (this.IsRootItem(indexPath)) {
            if (this.IsFirstItem(indexPath))
                return this.rootSubMenuFIYOffset;
            else if (this.IsLastItem(indexPath))
                return this.rootSubMenuLIYOffset;
            else
                return this.rootSubMenuYOffset;
        }
        else {
            if (this.IsFirstItem(indexPath))
                return this.subMenuFIYOffset;
            else if (this.IsLastItem(indexPath))
                return this.subMenuLIYOffset;
            else
                return this.subMenuYOffset;
        }
    },
    ClearAppearTimer: function() {
        aspxGetMenuCollection().ClearAppearTimer();
    },
    ClearDisappearTimer: function() {
        aspxGetMenuCollection().ClearDisappearTimer();
    },
    IsAppearTimerActive: function() {
        return aspxGetMenuCollection().IsAppearTimerActive();
    },
    IsDisappearTimerActive: function() {
        return aspxGetMenuCollection().IsDisappearTimerActive();
    },
    SetAppearTimer: function(indexPath) {
        aspxGetMenuCollection().SetAppearTimer(this.name, indexPath, this.appearAfter);
    },
    SetDisappearTimer: function() {
        aspxGetMenuCollection().SetDisappearTimer(this.name, this.disappearAfter);
    },
    IsDropDownItem: function(indexPath) {
        var element = this.GetItemPopOutImageCell(indexPath);
        if (_aspxIsExists(element) && _aspxIsExists(element.onclick))
            return element.onclick.toString().indexOf("aspxMIDDClick") > -1;
        return false;
    },
    DoItemClick: function(indexPath, hasItemLink, htmlEvent) {
        var processOnServer = this.RaiseItemClick(indexPath, htmlEvent);
        if (processOnServer && !hasItemLink)
            this.SendPostBack("CLICK:" + indexPath);
        else {
            this.ClearDisappearTimer();
            this.ClearAppearTimer();
            if (!this.HasChildren(indexPath) || this.IsDropDownItem(indexPath))
                aspxGetMenuCollection().DoHidePopupMenus(null, -1, this.name, false, "");
            else if (this.IsItemEnabled(indexPath) && !this.IsDropDownItem(indexPath))
                this.ShowSubMenu(indexPath);
        }
    },
    DoShowPopupMenu: function(element, x, y, indexPath) {
        if (element != null && this.IsCallbacksEnabled()) {
            var mainCell = this.GetMenuMainCell(element);
            if (mainCell != null && mainCell.innerHTML == "")
                this.CreateLoadingPanelInsideContainer(mainCell);
        }
        if (__aspxNetscapeFamily)
            _aspxSetStylePosition(element, -1000, -1000);
        _aspxSetElementDisplay(element, true);
        if (this.popupCount == 0) this.popupToLeft = false;
        var horizontalPopupPosition = this.GetClientSubMenuPos(element, indexPath, x, true);
        var verticalPopupPosition = this.GetClientSubMenuPos(element, indexPath, y, false);
        var clientX = horizontalPopupPosition.position;
        var clientY = verticalPopupPosition.position;
        var toTheLeft = horizontalPopupPosition.isInverted;
        var toTheTop = verticalPopupPosition.isInverted;
        if (this.enableAnimation) {
            this.StartAnimation(element, indexPath, horizontalPopupPosition, verticalPopupPosition);
        }
        else {
            _aspxSetStylePosition(element, clientX, clientY);
            _aspxSetElementVisibility(element, true);
            this.DoShowPopupMenuIFrame(element, clientX, clientY, __aspxInvalidDimension, __aspxInvalidDimension, indexPath);
            this.DoShowPopupMenuBorderCorrector(element, clientX, clientY, indexPath, toTheLeft, toTheTop);
        }
        aspxGetMenuCollection().RegisterVisiblePopupMenu(this.name, element.id);
        this.popupCount++;
        aspxGetControlCollection().AdjustControls(element);
        this.RaisePopUp(indexPath);
    },
    DoShowPopupMenuIFrame: function(element, x, y, width, height, indexPath) {
        if (!this.renderIFrameForPopupElements) return;
        var iFrame = element.overflowElement;
        if (!_aspxIsExists(iFrame)) {
            iFrame = this.GetMenuIFrameElement(indexPath);
            element.overflowElement = iFrame;
        }
        if (_aspxIsExists(iFrame)) {
            var cell = this.GetMenuMainCell(element);
            if (width < 0)
                width = cell.offsetWidth;
            if (height < 0)
                height = cell.offsetHeight;
            _aspxSetStyleSize(iFrame, width, height);
            _aspxSetStylePosition(iFrame, x, y);
            _aspxSetElementDisplay(iFrame, true);
        }
    },
    DoShowPopupMenuBorderCorrector: function(element, x, y, indexPath, toTheLeft, toTheTop) {
        var borderCorrectorElement = this.GetMenuBorderCorrectorElement(indexPath);
        if (_aspxIsExists(borderCorrectorElement)) {
            var itemInfo = new ASPxClientMenuItemInfo(this, indexPath);
            var menuXOffset = _aspxGetClientLeft(this.GetMenuMainCell(element));
            var menuYOffset = _aspxGetClientTop(this.GetMenuMainCell(element));
            var menuClientWidth = this.GetMenuMainCell(element).clientWidth;
            var menuClientHeight = this.GetMenuMainCell(element).clientHeight;
            var width = 0, height = 0, left = 0, top = 0;
            if (this.IsVertical(indexPath)) {
                var commonClientHeight = itemInfo.clientHeight < menuClientHeight ? itemInfo.clientHeight : menuClientHeight;
                width = menuXOffset;
                height = commonClientHeight + itemInfo.clientTop - menuYOffset;
                left = toTheLeft ? x + menuClientWidth + menuXOffset : x;
                top = toTheTop ? y + menuClientHeight - height + menuYOffset : y + menuYOffset;
            }
            else {
                var itemWidth = itemInfo.clientWidth;
                if (this.IsDropDownItem(indexPath)) {
                    var popOutImageElement = this.GetItemPopOutImageCell(indexPath);
                    if (popOutImageElement != null)
                        itemWidth -= popOutImageElement.clientWidth;
                }
                var commonClientWidth = itemWidth < menuClientWidth ? itemWidth : menuClientWidth;
                width = commonClientWidth + itemInfo.clientLeft - menuXOffset;
                height = menuYOffset;
                left = toTheLeft ? x + menuClientWidth - width + menuXOffset : x + menuXOffset;
                top = toTheTop ? y + menuClientHeight + menuYOffset : y;
                if (__aspxWebKitFamily && itemInfo.offsetLeft > 0)
                    width += itemInfo.clientLeft;
            }
            _aspxSetStyleSize(borderCorrectorElement, width, height);
            _aspxSetStylePosition(borderCorrectorElement, left, top);
            _aspxSetElementVisibility(borderCorrectorElement, true);
            _aspxSetElementDisplay(borderCorrectorElement, true);
            element.borderCorrectorElement = borderCorrectorElement;
        }
    },
    DoHidePopupMenu: function(evt, element) {
        this.DoHidePopupMenuBorderCorrector(element);
        this.DoHidePopupMenuIFrame(element);
        _aspxStopAnimation(element);
        _aspxSetElementVisibility(element, false);
        _aspxSetElementDisplay(element, false);
        this.CancelSubMenuItemHoverItem(element);
        aspxGetMenuCollection().UnregisterVisiblePopupMenu(this.name, element.id);
        this.popupCount--;
        var indexPath = this.GetIndexPathById(element.id, true);
        this.RaiseCloseUp(indexPath);
    },
    DoHidePopupMenuIFrame: function(element) {
        if (!this.renderIFrameForPopupElements) return;
        var iFrame = element.overflowElement;
        if (_aspxIsExists(iFrame))
            _aspxSetElementDisplay(iFrame, false);
    },
    DoHidePopupMenuBorderCorrector: function(element) {
        var borderCorrectorElement = element.borderCorrectorElement;
        if (_aspxIsExists(borderCorrectorElement)) {
            _aspxSetElementVisibility(borderCorrectorElement, false);
            _aspxSetElementDisplay(borderCorrectorElement, false);
            element.borderCorrectorElement = null;
        }
    },
    SetHoverElement: function(element) {
        this.lockHoverEvents = true;
        aspxGetStateController().SetCurrentHoverElementBySrcElement(element);
        this.lockHoverEvents = false;
    },
    ApplySubMenuItemHoverItem: function(element, hoverItem, hoverElement) {
        if (_aspxGetElementDisplay(element) && !_aspxIsExists(element.hoverItem)) {
            var newHoverItem = hoverItem.Clone();
            element.hoverItem = newHoverItem;
            element.hoverElement = hoverElement;
            newHoverItem.Apply(hoverElement);
        }
    },
    CancelSubMenuItemHoverItem: function(element) {
        if (_aspxIsExists(element.hoverItem)) {
            element.hoverItem.Cancel(element.hoverElement);
            element.hoverItem = null;
            element.hoverElement = null;
        }
    },
    ShowSubMenu: function(indexPath) {
        var element = this.GetMenuElement(indexPath);
        if (element != null) {
            var level = this.GetMenuLevel(indexPath);
            aspxGetMenuCollection().DoHidePopupMenus(null, level - 1, this.name, false, element.id);
            if (!_aspxGetElementDisplay(element))
                this.DoShowPopupMenu(element, __aspxInvalidPosition, __aspxInvalidPosition, indexPath);
        }
        this.ClearAppearTimer();
    },
    SelectItem: function(indexPath) {
        var element = this.GetItemTextOrImageCell(indexPath);
        if (element != null)
            aspxGetStateController().SelectElementBySrcElement(element);
    },
    DeselectItem: function(indexPath) {
        var element = this.GetItemTextOrImageCell(indexPath);
        if (element != null) {
            var hoverItem = null;
            var hoverElement = null;
            var menuElement = this.GetMenuElement(indexPath);
            if (menuElement != null && _aspxIsExists(menuElement.hoverItem)) {
                hoverItem = menuElement.hoverItem;
                hoverElement = menuElement.hoverElement;
                this.CancelSubMenuItemHoverItem(menuElement);
            }
            aspxGetStateController().DeselectElementBySrcElement(element);
            if (menuElement != null && hoverItem != null)
                this.ApplySubMenuItemHoverItem(menuElement, hoverItem, hoverElement);
        }
    },
    InitializeSelectedItem: function() {
        if (!this.allowSelectItem) return;
        this.SelectItem(this.GetSelectedItemIndexPath());
    },
    GetSelectedItemIndexPath: function() {
        var inputElement = this.GetSelectedItemInputElement();
        if (inputElement != null)
            return inputElement.value;
        return "";
    },
    SetSelectedItemInternal: function(indexPath, modifyHotTrackSelection) {
        if (modifyHotTrackSelection)
            this.SetHoverElement(null);
        var inputElement = this.GetSelectedItemInputElement();
        if (inputElement != null) {
            this.DeselectItem(inputElement.value);
            inputElement.value = indexPath;
            var item = this.GetItemByIndexPath(indexPath);
            if (item == null || item.GetEnabled())
                this.SelectItem(inputElement.value);
        }
        if (modifyHotTrackSelection) {
            var element = this.GetItemTextOrImageCell(indexPath);
            if (element != null)
                this.SetHoverElement(element);
        }
    },
    InitializeCheckedItems: function() {
        if (!this.allowCheckItems) return;
        var inputElement = this.GetCheckedItemsInputElement();
        if (inputElement != null) {
            var indexPathes = inputElement.value.split(";");
            for (var i = 0; i < indexPathes.length; i++) {
                if (indexPathes[i] != "") {
                    _aspxArrayPush(this.checkedItems, indexPathes[i]);
                    this.SelectItem(indexPathes[i]);
                }
            }
        }
    },
    ChangeCheckedItem: function(indexPath) {
        this.SetHoverElement(null);
        var inputElement = this.GetCheckedItemsInputElement();
        if (inputElement != null) {
            var itemsGroup = this.GetItemsGroup(indexPath);
            if (itemsGroup != null) {
                if (itemsGroup.length > 1) {
                    if (!this.IsCheckedItem(indexPath)) {
                        for (var i = 0; i < itemsGroup.length; i++) {
                            if (itemsGroup[i] == indexPath) continue;
                            if (this.IsCheckedItem(itemsGroup[i])) {
                                _aspxArrayRemove(this.checkedItems, itemsGroup[i]);
                                this.DeselectItem(itemsGroup[i]);
                            }
                        }
                        this.SelectItem(indexPath);
                        _aspxArrayPush(this.checkedItems, indexPath);
                    }
                }
                else {
                    if (this.IsCheckedItem(indexPath)) {
                        _aspxArrayRemove(this.checkedItems, indexPath);
                        this.DeselectItem(indexPath);
                    }
                    else {
                        this.SelectItem(indexPath);
                        _aspxArrayPush(this.checkedItems, indexPath);
                    }
                }
                this.UpdateCheckedInputElement(inputElement);
            }
        }
        var element = this.GetItemTextOrImageCell(indexPath);
        if (element != null)
            this.SetHoverElement(element);
    },
    GetItemsGroup: function(indexPath) {
        for (var i = 0; i < this.itemCheckedGroups.length; i++) {
            if (_aspxArrayIndexOf(this.itemCheckedGroups[i], indexPath) > -1)
                return this.itemCheckedGroups[i];
        }
        return null;
    },
    IsCheckedItem: function(indexPath) {
        return _aspxArrayIndexOf(this.checkedItems, indexPath) > -1;
    },
    UpdateCheckedInputElement: function(inputElement) {
        var state = "";
        for (var i = 0; i < this.checkedItems.length; i++) {
            state += this.checkedItems[i];
            if (i < this.checkedItems.length - 1)
                state += ";";
        }
        inputElement.value = state;
    },
    GetAnimationVerticalDirection: function(indexPath, popupPosition) {
        var verticalDirection = (this.IsRootItem(indexPath) && !this.IsVertical(indexPath)) ? -1 : 0;
        if (popupPosition.isInverted) verticalDirection *= -1;
        return verticalDirection;
    },
    GetAnimationHorizontalDirection: function(indexPath, popupPosition) {
        var horizontalDirection = (this.IsRootItem(indexPath) && !this.IsVertical(indexPath)) ? 0 : -1;
        if (popupPosition.isInverted) horizontalDirection *= -1;
        return horizontalDirection;
    },
    StartAnimation: function(animationDivElement, indexPath, horizontalPopupPosition, verticalPopupPosition) {
        var element = this.GetMenuMainTable(animationDivElement);
        var clientX = horizontalPopupPosition.position;
        var clientY = verticalPopupPosition.position;
        _aspxInitAnimationDiv(animationDivElement, clientX, clientY, "aspxMATimer(\"" + this.name + "\", " + "\"" + indexPath + "\")", "");
        var verticalDirection = this.GetAnimationVerticalDirection(indexPath, verticalPopupPosition);
        var horizontalDirection = this.GetAnimationHorizontalDirection(indexPath, horizontalPopupPosition);
        var yPos = verticalDirection * element.offsetWidth;
        var xPos = horizontalDirection * element.offsetHeight;
        _aspxSetStylePosition(element, xPos, yPos);
        _aspxSetElementVisibility(animationDivElement, true);
        this.DoShowPopupMenuIFrame(animationDivElement, clientX, clientY, 0, 0, indexPath);
        this.DoShowPopupMenuBorderCorrector(animationDivElement, clientX, clientY, indexPath,
   horizontalPopupPosition.isInverted, verticalPopupPosition.isInverted);
        animationDivElement.timerID = window.setTimeout(animationDivElement.onTimerString, this.animationDelay);
    },
    OnAnimationTimer: function(indexPath) {
        var animationDivElement = this.GetMenuElement(indexPath);
        if (_aspxIsExists(animationDivElement)) {
            var element = this.GetMenuMainTable(animationDivElement);
            var mainCell = this.GetMenuMainCell(element);
            var iframeElement = this.GetMenuIFrameElement(indexPath);
            _aspxOnAnimationTimer(animationDivElement, element, mainCell, iframeElement, this.animationDelay, this.animationMaxDelay, __aspxMenuAnimationAccelerator);
        }
    },
    OnItemClick: function(indexPath, evt) {
        var sourceElement = _aspxGetEventSource(evt);
        var clickedLinkElement = _aspxGetParentByTagName(sourceElement, "A");
        var isLinkClicked = (clickedLinkElement != null && clickedLinkElement.href != __aspxAccessibilityEmptyUrl);
        var element = this.GetItemTextOrImageCell(indexPath);
        var linkElement = (element != null) ? _aspxGetChildByTagName(element, "A", 0) : null;
        if (linkElement != null && linkElement.href == __aspxAccessibilityEmptyUrl)
            linkElement = null;
        if (this.allowSelectItem)
            this.SetSelectedItemInternal(indexPath, true);
        if (this.allowCheckItems)
            this.ChangeCheckedItem(indexPath);
        this.DoItemClick(indexPath, isLinkClicked || (linkElement != null), evt);
        if (!isLinkClicked && linkElement != null)
            _aspxNavigateUrl(linkElement.href, linkElement.target);
    },
    OnItemDropDownClick: function(indexPath, evt) {
        if (this.IsItemEnabled(indexPath))
            this.ShowSubMenu(indexPath);
    },
    OnAfterItemOver: function(hoverItem, hoverElement) {
        if (hoverItem.name == "" || this.lockHoverEvents) return;
        if (!this.showSubMenus) {
            this.savedCallbackHoverItem = hoverItem;
            this.savedCallbackHoverElement = hoverElement;
            return;
        }
        this.ClearDisappearTimer();
        this.ClearAppearTimer();
        var indexPath = this.GetMenuIndexPathById(hoverItem.name, false);
        if (indexPath == "") {
            indexPath = this.GetIndexPathById(hoverItem.name, true);
            var canShowSubMenu = true;
            var level = this.GetMenuLevel(indexPath);
            var menuElement = this.GetMenuElement(indexPath);
            if (this.IsDropDownItem(indexPath)) {
                var popOutImageElement = this.GetItemPopOutImageCell(indexPath);
                if (popOutImageElement != null && popOutImageElement != hoverElement) {
                    hoverItem.needRefreshBetweenElements = true;
                    canShowSubMenu = false;
                }
            }
            var id = (menuElement != null) ? menuElement.id : "";
            aspxGetMenuCollection().DoHidePopupMenus(null, level - 1, this.name, false, id);
            if (canShowSubMenu) {
                if (hoverItem.enabled && hoverItem.kind == __aspxHoverItemKind) {
                    this.SetAppearTimer(indexPath);
                    this.RaiseItemMouseOver(indexPath);
                }
            }
        }
    },
    OnBeforeItemOver: function(hoverItem, hoverElement) {
        if (__aspxNetscapeFamily && _aspxIsExists(hoverElement.offsetParent) &&
    hoverElement.offsetParent.style.borderCollapse == "collapse") {
            hoverElement.offsetParent.style.borderCollapse = "separate";
            hoverElement.offsetParent.style.borderCollapse = "collapse";
        }
        var indexPath = this.GetItemIndexPathById(hoverItem.name);
        var element = this.GetMenuElement(indexPath);
        if (_aspxIsExists(element)) this.CancelSubMenuItemHoverItem(element);
    },
    OnItemOverTimer: function(indexPath) {
        if (this.IsAppearTimerActive()) {
            this.ClearAppearTimer();
            if (this.GetItemImageCell(indexPath) != null || this.GetItemIndentCell(indexPath) != null ||
    this.GetItemTextCell(indexPath) != null || this.GetItemPopOutImageCell(indexPath) != null) {
                this.ShowSubMenu(indexPath);
            }
        }
    },
    OnBeforeItemDisabled: function(disabledItem, disabledElement) {
        this.ClearAppearTimer();
        var indexPath = this.GetIndexPathById(disabledElement.id, false);
        if (indexPath != "") {
            var element = this.GetMenuElement(indexPath);
            if (element != null) this.DoHidePopupMenu(null, element);
        }
    },
    OnAfterItemOut: function(hoverItem, hoverElement, newHoverElement) {
        if (!this.showSubMenus) {
            this.savedCallbackHoverItem = null;
            this.savedCallbackHoverElement = null;
        }
        if (hoverItem.name == "" || this.lockHoverEvents) return;
        if (hoverItem.IsChildElement(newHoverElement)) return;
        var indexPath = this.GetItemIndexPathById(hoverItem.name);
        var element = this.GetMenuElement(indexPath);
        this.ClearDisappearTimer();
        this.ClearAppearTimer();
        if (element == null || !_aspxGetIsParent(element, newHoverElement))
            this.SetDisappearTimer();
        if (element != null)
            this.ApplySubMenuItemHoverItem(element, hoverItem, hoverElement);
        if (indexPath != "")
            this.RaiseItemMouseOut(indexPath);
    },
    OnItemOutTimer: function() {
        if (this.IsDisappearTimerActive()) {
            this.ClearDisappearTimer();
            if (aspxGetMenuCollection().CheckFocusedElement())
                this.SetDisappearTimer();
            else
                aspxGetMenuCollection().DoHidePopupMenus(null, 0, this.name, true, "");
        }
    },
    OnFocusedItemKeyDown: function(evt, focusedItem, focusedElement) {
        var handled = false;
        var indexPath = this.GetItemIndexPathById(focusedItem.name);
        switch (evt.keyCode) {
            case ASPxKey.Tab:
                {
                    handled = this.FocusNextTabItem(indexPath, evt.shiftKey);
                    break;
                }
            case ASPxKey.Down:
                {
                    if (this.IsVertical(indexPath)) {
                        this.FocusNextItem(indexPath);
                    }
                    else {
                        this.ShowSubMenu(indexPath);
                        this.FocusItemByIndexPath(this.GetFirstChildIndexPath(indexPath));
                    }
                    handled = true;
                    break;
                }
            case ASPxKey.Up:
                {
                    if (this.IsVertical(indexPath)) {
                        this.FocusPrevItem(indexPath);
                    }
                    else {
                        this.ShowSubMenu(indexPath);
                        this.FocusItemByIndexPath(this.GetFirstChildIndexPath(indexPath));
                    }
                    handled = true;
                    break;
                }
            case ASPxKey.Left:
                {
                    if (this.IsVertical(indexPath)) {
                        var parentIndexPath = this.GetParentIndexPath(indexPath);
                        if (this.IsVertical(parentIndexPath)) {
                            this.FocusItemByIndexPath(parentIndexPath);
                        }
                        else {
                            this.FocusPrevItem(parentIndexPath);
                        }
                    }
                    else {
                        this.FocusPrevItem(indexPath);
                    }
                    handled = true;
                    break;
                }
            case ASPxKey.Right:
                {
                    if (this.IsVertical(indexPath)) {
                        if (this.HasChildren(indexPath)) {
                            this.ShowSubMenu(indexPath);
                            this.FocusItemByIndexPath(this.GetFirstChildIndexPath(indexPath));
                        }
                        else {
                            while (!this.IsRootItem(indexPath))
                                indexPath = this.GetParentIndexPath(indexPath);
                            this.FocusNextItem(indexPath);
                        }
                    }
                    else {
                        this.FocusNextItem(indexPath);
                    }
                    handled = true;
                    break;
                }
            case ASPxKey.Esc:
                {
                    var parentIndexPath = this.GetParentIndexPath(indexPath);
                    this.FocusItemByIndexPath(parentIndexPath);
                    var element = this.GetMenuElement(parentIndexPath);
                    if (element != null) {
                        this.DoHidePopupMenu(null, element);
                        handled = true;
                    }
                }
        }
        if (handled)
            _aspxPreventEventAndBubble(evt);
    },
    FocusItemByIndexPath: function(indexPath) {
        var element = this.GetItemTextOrImageCell(indexPath);
        var link = _aspxGetChildByTagName(element, "A", 0);
        if (link != null) _aspxSetFocus(link);
    },
    FocusNextTabItem: function(indexPath, shiftKey) {
        if (this.IsRootItem(indexPath)) return false;
        while (true) {
            if (this.IsRootItem(indexPath)) {
                if (!shiftKey) {
                    if (this.GetNextSiblingIndexPath(indexPath) != null) {
                        this.FocusNextItem(indexPath);
                        return true;
                    }
                }
                else {
                    if (this.GetPrevSiblingIndexPath(indexPath) != null) {
                        this.FocusPrevItem(indexPath);
                        return true;
                    }
                }
                break;
            }
            else {
                if (!shiftKey) {
                    if (this.GetNextSiblingIndexPath(indexPath) == null)
                        indexPath = this.GetParentIndexPath(indexPath);
                    else {
                        this.FocusNextItem(indexPath);
                        return true;
                    }
                }
                else {
                    if (this.GetPrevSiblingIndexPath(indexPath) == null)
                        indexPath = this.GetParentIndexPath(indexPath);
                    else {
                        this.FocusPrevItem(indexPath);
                        return true;
                    }
                }
            }
        }
        return false;
    },
    FocusNextItem: function(indexPath) {
        var newIndexPath = this.GetNextSiblingIndexPath(indexPath);
        if (newIndexPath == null)
            newIndexPath = this.GetFirstSiblingIndexPath(indexPath);
        if (indexPath != newIndexPath)
            this.FocusItemByIndexPath(newIndexPath);
    },
    FocusPrevItem: function(indexPath) {
        var newIndexPath = this.GetPrevSiblingIndexPath(indexPath);
        if (newIndexPath == null)
            newIndexPath = this.GetLastSiblingIndexPath(indexPath);
        if (indexPath != newIndexPath)
            this.FocusItemByIndexPath(newIndexPath);
    },
    Focus: function() {
        this.FocusNextItem("-1");
    },
    FocusLastItem: function() {
        this.FocusPrevItem(this.GetItemCount() - 1);
    },
    OnCallback: function(result) {
        _aspxInitializeScripts();
        for (var indexPath in result) {
            var menuElement = this.GetMenuElement(indexPath);
            if (_aspxIsExists(menuElement)) {
                var mainCell = this.GetMenuMainCell(menuElement);
                _aspxSetInnerHtml(mainCell, result[indexPath]);
            }
        }
        this.InitializeInternal(false);
        if (!this.showSubMenus) {
            this.showSubMenus = true;
            if (this.savedCallbackHoverItem != null && this.savedCallbackHoverElement != null)
                this.OnAfterItemOver(this.savedCallbackHoverItem, this.savedCallbackHoverElement);
            this.savedCallbackHoverItem = null;
            this.savedCallbackHoverElement = null;
        }
    },
    CreateItems: function(itemsProperties) {
        var itemType = this.GetClientItemType();
        this.rootItem = new itemType(this, null, 0, "");
        this.rootItem.CreateItems(itemsProperties);
    },
    GetClientItemType: function() {
        return ASPxClientMenuItem;
    },
    GetItemByIndexPath: function(indexPath) {
        var item = this.rootItem;
        if (indexPath != "" && item != null) {
            var indexes = this.GetItemIndexes(indexPath);
            for (var i = 0; i < indexes.length; i++)
                item = item.GetItem(indexes[i]);
        }
        return item;
    },
    SetItemChecked: function(indexPath, checked) {
        var inputElement = this.GetCheckedItemsInputElement();
        if (inputElement != null) {
            var itemsGroup = this.GetItemsGroup(indexPath);
            if (itemsGroup != null) {
                if (!checked && this.IsCheckedItem(indexPath)) {
                    _aspxArrayRemove(this.checkedItems, indexPath);
                    this.DeselectItem(indexPath);
                }
                else if (checked && !this.IsCheckedItem(indexPath)) {
                    if (itemsGroup.length > 1) {
                        for (var i = 0; i < itemsGroup.length; i++) {
                            if (itemsGroup[i] == indexPath) continue;
                            if (this.IsCheckedItem(itemsGroup[i])) {
                                _aspxArrayRemove(this.checkedItems, itemsGroup[i]);
                                this.DeselectItem(itemsGroup[i]);
                            }
                        }
                    }
                    this.SelectItem(indexPath);
                    _aspxArrayPush(this.checkedItems, indexPath);
                }
                this.UpdateCheckedInputElement(inputElement);
            }
        }
    },
    ChangeEnabledAttributes: function(indexPath, method, styleMethod) {
        var itemElement = this.IsVertical(indexPath) ? this.GetItemElement(indexPath) : null;
        if (_aspxIsExists(itemElement))
            method(itemElement, "onclick");
        var templateElement = this.GetItemTemplateCell(indexPath);
        if (_aspxIsExists(templateElement))
            method(templateElement, "onclick");
        var imageElement = this.GetItemImageCell(indexPath);
        if (_aspxIsExists(imageElement)) {
            method(imageElement, "onclick");
            styleMethod(imageElement, "cursor");
            var link = this.GetInternalHyperlinkElement(imageElement, 0);
            if (link != null) {
                method(link, "onclick");
                method(link, "href");
                styleMethod(link, "cursor");
            }
        }
        var textElement = this.GetItemTextCell(indexPath);
        if (_aspxIsExists(textElement)) {
            method(textElement, "onclick");
            styleMethod(textElement, "cursor");
            var link = this.GetInternalHyperlinkElement(textElement, 0);
            if (link != null) {
                method(link, "onclick");
                method(link, "href");
                styleMethod(link, "cursor");
                link = this.GetInternalHyperlinkElement(textElement, 1);
                if (link != null) {
                    method(link, "onclick");
                    method(link, "href");
                    styleMethod(link, "cursor");
                }
            }
        }
        var popOutImageElement = this.GetItemPopOutImageCell(indexPath);
        if (_aspxIsExists(popOutImageElement)) {
            method(popOutImageElement, "onclick");
            styleMethod(popOutImageElement, "cursor");
        }
        var indentElement = this.GetItemIndentCell(indexPath);
        if (_aspxIsExists(indentElement)) {
            method(indentElement, "onclick");
            styleMethod(indentElement, "cursor");
        }
    },
    IsItemEnabled: function(indexPath) {
        var item = this.GetItemByIndexPath(indexPath);
        return (item != null) ? item.GetEnabled() : true;
    },
    SetItemEnabled: function(indexPath, enabled, initialization) {
        if (indexPath == "" || !this.GetItemByIndexPath(indexPath).enabled) return;
        if (!enabled) {
            if (this.GetSelectedItemIndexPath() == indexPath)
                this.DeselectItem(indexPath);
        }
        if (!initialization || !enabled)
            this.ChangeItemEnabledStateItems(indexPath, enabled);
        this.ChangeItemEnabledAttributes(indexPath, enabled);
        if (enabled) {
            if (this.GetSelectedItemIndexPath() == indexPath)
                this.SelectItem(indexPath);
        }
    },
    ChangeItemEnabledStateItems: function(indexPath, enabled) {
        var element = this.GetItemTextOrImageCell(indexPath);
        if (element != null)
            aspxGetStateController().SetElementEnabled(element, enabled);
    },
    ChangeItemEnabledAttributes: function(indexPath, enabled) {
        var element = this.GetItemTextOrImageCell(indexPath);
        if (element != null) {
            element.enabled = enabled;
            this.ChangeEnabledAttributes(indexPath, _aspxChangeAttributesMethod(enabled),
    _aspxChangeStyleAttributesMethod(enabled));
        }
    },
    GetItemImageUrl: function(indexPath) {
        var element = this.GetItemImageCell(indexPath);
        if (element != null) {
            var img = _aspxGetChildByTagName(element, "IMG", 0);
            if (img != null)
                return img.src;
        }
        element = this.GetItemTextCell(indexPath);
        if (element != null) {
            var img = _aspxGetChildByTagName(element, "IMG", 0);
            if (img != null)
                return img.src;
        }
        return "";
    },
    SetItemImageUrl: function(indexPath, url) {
        var element = this.GetItemImageCell(indexPath);
        if (element != null) {
            var img = _aspxGetChildByTagName(element, "IMG", 0);
            if (img != null)
                img.src = url;
        }
        element = this.GetItemTextCell(indexPath);
        if (element != null) {
            var img = _aspxGetChildByTagName(element, "IMG", 0);
            if (img != null)
                img.src = url;
        }
    },
    GetItemNavigateUrl: function(indexPath) {
        var element = this.GetItemTextCell(indexPath);
        if (element != null) {
            var link = _aspxGetChildByTagName(element, "A", 0);
            if (link != null)
                return link.href;
        }
        element = this.GetItemImageCell(indexPath);
        if (element != null) {
            var link = _aspxGetChildByTagName(element, "A", 0);
            if (link != null)
                return link.href;
        }
        return "";
    },
    SetItemNavigateUrl: function(indexPath, url) {
        var element = this.GetItemTextCell(indexPath);
        if (element != null) {
            var link = _aspxGetChildByTagName(element, "A", 0);
            if (link != null)
                link.href = url;
            link = _aspxGetChildByTagName(element, "A", 1);
            if (link != null)
                link.href = url;
        }
        element = this.GetItemImageCell(indexPath);
        if (element != null) {
            var link = _aspxGetChildByTagName(element, "A", 0);
            if (link != null)
                link.href = url;
        }
    },
    GetItemText: function(indexPath) {
        var element = this.GetItemTextCell(indexPath);
        if (element != null) {
            var textNode = _aspxGetChildTextNode(element, 0);
            if (textNode != null)
                return textNode.nodeValue;
        }
        return "";
    },
    SetItemText: function(indexPath, text) {
        var element = this.GetItemTextCell(indexPath);
        if (element != null) {
            var textNode = _aspxGetChildTextNode(element, 0);
            if (textNode != null)
                textNode.nodeValue = text;
        }
    },
    SetItemVisible: function(indexPath, visible, initialization) {
        if (indexPath == "" || !this.GetItemByIndexPath(indexPath).visible) return;
        if (visible && initialization) return;
        var element = null;
        if (this.IsVertical(indexPath)) {
            element = this.GetItemElement(indexPath);
            if (element != null)
                _aspxSetElementDisplay(element, visible);
        }
        else {
            element = this.GetItemTemplateCell(indexPath);
            if (element != null)
                _aspxSetElementDisplay(element, visible);
            element = this.GetItemImageCell(indexPath);
            if (element != null)
                _aspxSetElementDisplay(element, visible);
            element = this.GetItemTextCell(indexPath);
            if (element != null)
                _aspxSetElementDisplay(element, visible);
            element = this.GetItemPopOutImageCell(indexPath);
            if (element != null)
                _aspxSetElementDisplay(element, visible);
        }
        this.SetIndentsVisiblility(indexPath);
        this.SetSeparatorsVisiblility(indexPath);
    },
    SetIndentsVisiblility: function(indexPath) {
        var parent = this.GetItemByIndexPath(indexPath).parent;
        for (var i = 0; i < parent.GetItemCount(); i++) {
            var item = parent.GetItem(i);
            var separatorVisible = item.GetVisible() && this.HasNextVisibleItems(parent, i);
            var element = this.GetItemIndentElement(item.GetIndexPath());
            if (element != null) _aspxSetElementDisplay(element, separatorVisible);
        }
    },
    SetSeparatorsVisiblility: function(indexPath) {
        var parent = this.GetItemByIndexPath(indexPath).parent;
        for (var i = 0; i < parent.GetItemCount(); i++) {
            var item = parent.GetItem(i);
            var separatorVisible = item.GetVisible() && this.HasPrevVisibleItems(parent, i);
            var element = this.GetItemSeparatorElement(item.GetIndexPath());
            if (element != null) _aspxSetElementDisplay(element, separatorVisible);
            element = this.GetItemSeparatorIndentElement(item.GetIndexPath());
            if (element != null) _aspxSetElementDisplay(element, separatorVisible);
        }
    },
    HasNextVisibleItems: function(parent, index) {
        for (var i = index + 1; i < parent.GetItemCount(); i++) {
            if (parent.GetItem(i).GetVisible())
                return true;
        }
        return false;
    },
    HasPrevVisibleItems: function(parent, index) {
        for (var i = index - 1; i >= 0; i--) {
            if (parent.GetItem(i).GetVisible())
                return true;
        }
        return false;
    },
    GetItemIndentElement: function(indexPath) {
        return _aspxGetElementById(this.name + __aspxMIIdSuffix + indexPath + "_II");
    },
    GetItemSeparatorElement: function(indexPath) {
        return _aspxGetElementById(this.name + __aspxMIIdSuffix + indexPath + "_IS");
    },
    GetItemSeparatorIndentElement: function(indexPath) {
        return _aspxGetElementById(this.name + __aspxMIIdSuffix + indexPath + "_ISI");
    },
    RaiseItemClick: function(indexPath, htmlEvent) {
        var processOnServer = this.autoPostBack || this.IsServerEventAssigned("ItemClick");
        if (!this.ItemClick.IsEmpty()) {
            var item = this.GetItemByIndexPath(indexPath);
            var htmlElement = this.GetItemTextOrImageCell(indexPath);
            var args = new ASPxClientMenuItemClickEventArgs(processOnServer, item, htmlElement, htmlEvent);
            this.ItemClick.FireEvent(this, args);
            processOnServer = args.processOnServer;
        }
        return processOnServer;
    },
    RaiseItemMouseOver: function(indexPath) {
        if (!this.ItemMouseOver.IsEmpty()) {
            var item = this.GetItemByIndexPath(indexPath);
            var htmlElement = this.GetItemTextOrImageCell(indexPath);
            var args = new ASPxClientMenuItemMouseEventArgs(item, htmlElement);
            this.ItemMouseOver.FireEvent(this, args);
        }
    },
    RaiseItemMouseOut: function(indexPath) {
        if (!this.ItemMouseOut.IsEmpty()) {
            var item = this.GetItemByIndexPath(indexPath);
            var htmlElement = this.GetItemTextOrImageCell(indexPath);
            var args = new ASPxClientMenuItemMouseEventArgs(item, htmlElement);
            this.ItemMouseOut.FireEvent(this, args);
        }
    },
    RaisePopUp: function(indexPath) {
        var item = this.GetItemByIndexPath(indexPath);
        if (!this.PopUp.IsEmpty()) {
            var args = new ASPxClientMenuItemEventArgs(false, item);
            this.PopUp.FireEvent(this, args);
        }
    },
    RaiseCloseUp: function(indexPath) {
        var item = this.GetItemByIndexPath(indexPath);
        if (!this.CloseUp.IsEmpty()) {
            var args = new ASPxClientMenuItemEventArgs(false, item);
            this.CloseUp.FireEvent(this, args);
        }
    },
    GetItemCount: function() {
        return (this.rootItem != null) ? this.rootItem.GetItemCount() : 0;
    },
    GetItem: function(index) {
        return (this.rootItem != null) ? this.rootItem.GetItem(index) : null;
    },
    GetItemByName: function(name) {
        return (this.rootItem != null) ? this.rootItem.GetItemByName(name) : null;
    },
    GetSelectedItem: function() {
        var indexPath = this.GetSelectedItemIndexPath();
        if (indexPath != "")
            return this.GetItemByIndexPath(indexPath);
        return null;
    },
    SetSelectedItem: function(item) {
        var indexPath = (item != null) ? item.GetIndexPath() : "";
        this.SetSelectedItemInternal(indexPath, false);
    },
    GetRootItem: function() {
        return this.rootItem;
    }
});



//*************************************************************************//

//var __aspxClientValidationStateNameSuffix = "$CVS";
ASPxClientEditBase = _aspxCreateClass(ASPxClientControl, {
    constructor: function(name) {
        this.constructor.prototype.constructor.call(this, name);
    },
    InlineInitialize: function() {
        this.InitializeEnabled();
    },
    InitializeEnabled: function() {
        this.SetEnabledInternal(this.clientEnabled, true);
    },
    GetValue: function() {
        var element = this.GetMainElement();
        if (_aspxIsExistsElement(element))
            return element.innerHTML;
        return "";
    },
    GetValueString: function() {
        var value = this.GetValue();
        return (value == null) ? null : value.toString();
    },
    SetValue: function(value) {
        if (value == null)
            value = "";
        var element = this.GetMainElement();
        if (_aspxIsExistsElement(element))
            element.innerHTML = value;
    },
    GetEnabled: function() {
        return this.enabled && this.clientEnabled;
    },
    SetEnabled: function(enabled) {
        if (this.clientEnabled != enabled) {
            var errorFrameRequiresUpdate = this.GetIsValid && !this.GetIsValid();
            if (errorFrameRequiresUpdate && !enabled)
                this.UpdateErrorFrameAndFocus(false, null, true);
            this.clientEnabled = enabled;
            this.SetEnabledInternal(enabled, false);
            if (errorFrameRequiresUpdate && enabled)
                this.UpdateErrorFrameAndFocus(false);
        }
    },
    SetEnabledInternal: function(enabled, initialization) {
        if (!this.enabled) return;
        if (!initialization || !enabled)
            this.ChangeEnabledStateItems(enabled);
        this.ChangeEnabledAttributes(enabled);
    },
    ChangeEnabledAttributes: function(enabled) {
    },
    ChangeEnabledStateItems: function(enabled) {
    }
});




//*************************************************************************//
ASPxClientEdit = _aspxCreateClass(ASPxClientEditBase, {
    constructor: function(name) {
        this.constructor.prototype.constructor.call(this, name);
        this.isASPxClientEdit = true;
        this.inputElement = null;
        this.elementCache = {};
        this.convertEmptyStringToNull = true;
        this.readOnly = false;
        this.focused = false;
        this.focusEventsLocked = false;
        this.receiveGlobalMouseWheel = true;
        this.styleDecoration = null;
        this.widthCorrectionRequired = false;
        this.heightCorrectionRequired = false;
        this.customValidationEnabled = false;
        this.display = ASPxErrorFrameDisplay.Static;
        this.initialErrorText = "";
        this.causesValidation = false;
        this.validateOnLeave = true;
        this.validationGroup = "";
        this.sendPostBackWithValidation = null;
        this.validationPatterns = [];
        this.setFocusOnError = false;
        this.errorDisplayMode = "it";
        this.errorText = "";
        this.isValid = true;
        this.errorImageIsAssigned = false;
        this.clientValidationStateElement = null;
        this.enterProcessed = false;
        this.keyDownHandlers = {};
        this.keyPressHandlers = {};
        this.keyUpHandlers = {};
        this.specialKeyboardHandlingUsed = false;
        this.onKeyDownHandler = null;
        this.onKeyPressHandler = null;
        this.onKeyUpHandler = null;
        this.onGotFocusHandler = null;
        this.onLostFocusHandler = null;
        this.GotFocus = new ASPxClientEvent();
        this.LostFocus = new ASPxClientEvent();
        this.Validation = new ASPxClientEvent();
        this.ValueChanged = new ASPxClientEvent();
        this.KeyDown = new ASPxClientEvent();
        this.KeyPress = new ASPxClientEvent();
        this.KeyUp = new ASPxClientEvent();
        ASPxClientEdit.controls.push(this);
    },
    Initialize: function() {
        this.initialErrorText = this.errorText;
        ASPxClientEditBase.prototype.Initialize.call(this);
        this.InitializeKeyHandlers();
        this.UpdateClientValidationState();
    },
    InlineInitialize: function() {
        ASPxClientEditBase.prototype.InlineInitialize.call(this);
        if (this.styleDecoration != null)
            this.styleDecoration.Update();
    },
    InitSpecialKeyboardHandling: function() {
        this.onKeyDownHandler = _aspxCreateEventHandlerFunction("aspxKBSIKeyDown", this.name, true);
        this.onKeyPressHandler = _aspxCreateEventHandlerFunction("aspxKBSIKeyPress", this.name, true);
        this.onKeyUpHandler = _aspxCreateEventHandlerFunction("aspxKBSIKeyUp", this.name, true);
        this.onGotFocusHandler = _aspxCreateEventHandlerFunction("aspxESGotFocus", this.name, false);
        this.onLostFocusHandler = _aspxCreateEventHandlerFunction("aspxESLostFocus", this.name, false);
        this.specialKeyboardHandlingUsed = true;
        this.InitializeDelayedSpecialFocus();
    },
    InitializeKeyHandlers: function() {
    },
    AddKeyDownHandler: function(key, handler) {
        this.keyDownHandlers[key] = handler;
    },
    ChangeSpecialInputEnabledAttributes: function(element, method) {
        element.autocomplete = "off";
        if (this.onKeyDownHandler != null)
            method(element, "keydown", this.onKeyDownHandler);
        if (this.onKeyPressHandler != null)
            method(element, "keypress", this.onKeyPressHandler);
        if (this.onKeyUpHandler != null)
            method(element, "keyup", this.onKeyUpHandler);
        if (this.onGotFocusHandler != null)
            method(element, "focus", this.onGotFocusHandler);
        if (this.onLostFocusHandler != null)
            method(element, "blur", this.onLostFocusHandler);
    },
    UpdateClientValidationState: function() {
        if (!this.customValidationEnabled)
            return;
        var mainElement = this.GetMainElement();
        if (_aspxIsExists(mainElement)) {
            var hiddenField = this.GetClientValidationStateHiddenField();
            if (_aspxIsExists(hiddenField))
                hiddenField.value = _aspxEncodeHtml(!this.GetIsValid() ? ("-" + this.GetErrorText()) : "");
        }
    },
    GetCachedElementByIdSuffix: function(idSuffix) {
        var element = this.elementCache[idSuffix];
        if (!_aspxIsExistsElement(element)) {
            element = _aspxGetElementById(this.name + idSuffix);
            this.elementCache[idSuffix] = element;
        }
        return element;
    },
    FindInputElement: function() {
        return null;
    },
    GetInputElement: function() {
        if (!_aspxIsExistsElement(this.inputElement))
            this.inputElement = this.FindInputElement();
        return this.inputElement;
    },
    GetErrorImage: function() {
        return this.GetCachedElementByIdSuffix(ASPxEditElementSuffix.ErrorImage);
    },
    GetExternalTable: function() {
        return this.GetCachedElementByIdSuffix(ASPxEditElementSuffix.ExternalTable);
    },
    GetControlCell: function() {
        return this.GetCachedElementByIdSuffix(ASPxEditElementSuffix.ControlCell);
    },
    GetErrorCell: function() {
        return this.GetCachedElementByIdSuffix(ASPxEditElementSuffix.ErrorCell);
    },
    GetErrorTextCell: function() {
        return this.GetCachedElementByIdSuffix(this.errorImageIsAssigned ?
   ASPxEditElementSuffix.ErrorTextCell : ASPxEditElementSuffix.ErrorCell);
    },
    GetClientValidationStateHiddenField: function() {
        if (!_aspxIsExists(this.clientValidationStateElement))
            this.clientValidationStateElement = this.CreateClientValidationStateHiddenField();
        return this.clientValidationStateElement;
    },
    CreateClientValidationStateHiddenField: function() {
        var mainElement = this.GetMainElement();
        var hiddenField = _aspxCreateHiddenField(this.uniqueID + __aspxClientValidationStateNameSuffix);
        mainElement.parentNode.appendChild(hiddenField);
        return hiddenField;
    },
    SetVisible: function(isVisible) {
        if (this.clientVisible == isVisible)
            return;
        if (this.customValidationEnabled) {
            var errorFrame = this.GetExternalTable();
            if (_aspxIsExists(errorFrame)) {
                _aspxSetElementDisplay(errorFrame, isVisible);
                var isValid = !isVisible ? true : void (0);
                this.UpdateErrorFrameAndFocus(false, true, isValid);
            }
        }
        ASPxClientControl.prototype.SetVisible.call(this, isVisible);
    },
    GetValueInputToValidate: function() {
        return this.GetInputElement();
    },
    IsVisible: function() {
        if (!this.clientVisible)
            return false;
        var element = this.GetMainElement();
        while (_aspxIsExists(element) && element.tagName != "BODY") {
            if (element.getAttribute("errorFrame") != "errorFrame" && (!_aspxGetElementVisibility(element) || !_aspxGetElementDisplay(element)))
                return false;
            element = element.parentNode;
        }
        return true;
    },
    AdjustControlCore: function() {
        this.CollapseControl();
        if (this.WidthCorrectionRequired())
            this.CorrectEditorWidth();
        else
            this.UnstretchInputElement();
        if (this.heightCorrectionRequired)
            this.CorrectEditorHeight();
    },
    WidthCorrectionRequired: function() {
        var mainElement = this.GetMainElement();
        if (_aspxIsExistsElement(mainElement)) {
            var mainElementCurStyle = _aspxGetCurrentStyle(mainElement);
            return this.widthCorrectionRequired && mainElementCurStyle.width != "" && mainElementCurStyle.width != "auto";
        }
        return false;
    },
    CorrectEditorWidth: function() {
    },
    CorrectEditorHeight: function() {
    },
    UnstretchInputElement: function() {
    },
    UseDelayedSpecialFocus: function() {
        return false;
    },
    GetDelayedSpecialFocusTriggers: function() {
        return [this.GetMainElement()];
    },
    InitializeDelayedSpecialFocus: function() {
        if (!this.UseDelayedSpecialFocus())
            return;
        this.specialFocusTimer = -1;
        var instance = this;
        var handler = function() {
            window.setTimeout(function() { instance.SetFocus(); }, 0);
        };
        var triggers = this.GetDelayedSpecialFocusTriggers();
        for (var i = 0; i < triggers.length; i++)
            _aspxAttachEventToElement(triggers[i], "mousedown", handler);
    },
    IsFocusEventsLocked: function() {
        return this.focusEventsLocked;
    },
    LockFocusEvents: function() {
        if (!this.focused) return;
        this.focusEventsLocked = true;
    },
    UnlockFocusEvents: function() {
        this.focusEventsLocked = false;
    },
    ForceRefocusEditor: function() {
        this.LockFocusEvents();
        this.GetInputElement().blur();
        window.setTimeout("aspxGetControlCollection().Get('" + this.name + "').SetFocus();", 0);
    },
    IsEditorElement: function(element) {
        return this.GetMainElement() == element || _aspxGetIsParent(this.GetMainElement(), element);
    },
    OnFocusCore: function() {
        if (this.UseDelayedSpecialFocus())
            window.clearTimeout(this.specialFocusTimer);
        if (!this.IsFocusEventsLocked()) {
            this.focused = true;
            ASPxClientEdit.SetFocusedEditor(this);
            if (this.styleDecoration != null && !this.readOnly)
                this.styleDecoration.Update();
            if (this.isInitialized)
                this.RaiseFocus();
        }
        else
            this.UnlockFocusEvents();
    },
    OnLostFocusCore: function() {
        if (!this.IsFocusEventsLocked()) {
            this.focused = false;
            ASPxClientEdit.SetFocusedEditor(null);
            if (this.styleDecoration != null && !this.readOnly)
                this.styleDecoration.Update();
            this.RaiseLostFocus();
            if (this.validateOnLeave)
                this.SetFocusOnError();
        }
    },
    OnFocus: function() {
        if (!this.specialKeyboardHandlingUsed)
            this.OnFocusCore();
    },
    OnLostFocus: function() {
        if (this.isInitialized && !this.specialKeyboardHandlingUsed)
            this.OnLostFocusCore();
    },
    OnSpecialFocus: function() {
        if (this.isInitialized)
            this.OnFocusCore();
    },
    OnSpecialLostFocus: function() {
        if (this.isInitialized)
            this.OnLostFocusCore();
    },
    OnMouseWheel: function(evt) {
    },
    OnValidation: function(validationType) {
        if (this.customValidationEnabled && this.isInitialized && _aspxIsExistsElement(this.GetExternalTable())) {
            this.BeginErrorFrameUpdate();
            try {
                this.SetIsValid(true);
                this.SetErrorText(this.initialErrorText);
                if (this.validateOnLeave || validationType != ASPxValidationType.PersonalOnValueChanged) {
                    this.ValidateWithPatterns();
                    this.RaiseValidation();
                }
                this.UpdateErrorFrameAndFocus(validationType == ASPxValidationType.PersonalOnValueChanged && this.validateOnLeave && !this.GetIsValid());
            } finally {
                this.EndErrorFrameUpdate();
            }
        }
    },
    OnValueChanged: function() {
        var processOnServer = this.RaiseValidationInternal();
        processOnServer = this.RaiseValueChangedEvent() && processOnServer;
        if (processOnServer)
            this.SendPostBackInternal("");
    },
    ParseValue: function() {
    },
    RaisePersonalStandardValidation: function() {
        if (_aspxIsFunction(window.ValidatorOnChange)) {
            var inputElement = this.GetValueInputToValidate();
            if (_aspxIsExists(inputElement.Validators))
                window.ValidatorOnChange({ srcElement: inputElement });
        }
    },
    RaiseValidationInternal: function() {
        if (this.autoPostBack && this.causesValidation && this.validateOnLeave)
            return ASPxClientEdit.ValidateGroup(this.validationGroup);
        else {
            this.OnValidation(ASPxValidationType.PersonalOnValueChanged);
            return this.GetIsValid();
        }
    },
    RaiseValueChangedEvent: function() {
        return this.RaiseValueChanged();
    },
    SendPostBackInternal: function(postBackArg) {
        if (_aspxIsFunction(this.sendPostBackWithValidation))
            this.sendPostBackWithValidation(postBackArg);
        else
            this.SendPostBack(postBackArg);
    },
    SetElementToBeFocused: function() {
        if (this.IsVisible())
            __aspxInvalidEditorToBeFocused = this;
    },
    SetFocus: function() {
        var inputElement = this.GetInputElement();
        if (_aspxGetActiveElement() != inputElement && _aspxIsEditorFocusable(inputElement))
            _aspxSetFocus(inputElement);
    },
    SetFocusOnError: function() {
        if (__aspxInvalidEditorToBeFocused == this) {
            this.SetFocus();
            __aspxInvalidEditorToBeFocused = null;
        }
    },
    BeginErrorFrameUpdate: function() {
        if (!this.errorFrameUpdateLocked)
            this.errorFrameUpdateLocked = true;
    },
    EndErrorFrameUpdate: function() {
        this.errorFrameUpdateLocked = false;
        var args = this.updateErrorFrameAndFocusLastCallArgs;
        if (args) {
            this.UpdateErrorFrameAndFocus(args[0], args[1]);
            delete this.updateErrorFrameAndFocusLastCallArgs;
        }
    },
    UpdateErrorFrameAndFocus: function(setFocusOnError, ignoreVisibilityCheck, isValid) {
        if (!this.GetEnabled() || !ignoreVisibilityCheck && !this.GetVisible())
            return;
        if (this.errorFrameUpdateLocked) {
            this.updateErrorFrameAndFocusLastCallArgs = [setFocusOnError, ignoreVisibilityCheck];
            return;
        }
        if (typeof (isValid) == "undefined")
            isValid = this.GetIsValid();
        var externalTable = this.GetExternalTable();
        var isStaticDisplay = this.display == ASPxErrorFrameDisplay.Static;
        if (isValid) {
            if (isStaticDisplay) {
                externalTable.style.visibility = "hidden";
            } else {
                this.HideErrorCell();
                this.SaveErrorFrameStyles();
                this.ClearErrorFrameElementsStyles();
            }
        } else {
            var editorLocatedWithinVisibleContainer = this.IsVisible();
            if (this.widthCorrectionRequired) {
                if (editorLocatedWithinVisibleContainer)
                    this.CollapseControl();
                else
                    this.sizeCorrectedOnce = false;
            }
            this.UpdateErrorCellContent();
            if (isStaticDisplay) {
                externalTable.style.visibility = "visible";
            } else {
                this.EnsureErrorFrameStylesLoaded();
                this.RestoreErrorFrameElementsStyles();
                this.ShowErrorCell();
            }
            if (editorLocatedWithinVisibleContainer) {
                if (this.widthCorrectionRequired)
                    this.AdjustControl();
                if (setFocusOnError && this.setFocusOnError && __aspxInvalidEditorToBeFocused == null)
                    this.SetElementToBeFocused();
            }
        }
    },
    ShowErrorCell: function() {
        var errorCell = this.GetErrorCell();
        if (_aspxIsExists(errorCell))
            _aspxSetElementDisplay(errorCell, true);
    },
    HideErrorCell: function() {
        var errorCell = this.GetErrorCell();
        if (_aspxIsExists(errorCell))
            _aspxSetElementDisplay(errorCell, false);
    },
    SaveErrorFrameStyles: function() {
        this.EnsureErrorFrameStylesLoaded();
    },
    EnsureErrorFrameStylesLoaded: function() {
        if (typeof (this.errorFrameStyles) == "undefined") {
            var externalTable = this.GetExternalTable();
            var controlCell = this.GetControlCell();
            this.errorFrameStyles = {
                errorFrame: {
                    cssClass: externalTable.className,
                    style: this.ExtractElementStyleStringIgnoringVisibilityProps(externalTable)
                },
                controlCell: {
                    cssClass: controlCell.className,
                    style: this.ExtractElementStyleStringIgnoringVisibilityProps(controlCell)
                }
            };
        }
    },
    ClearErrorFrameElementsStyles: function() {
        this.ClearElementStyle(this.GetExternalTable());
        this.ClearElementStyle(this.GetControlCell());
    },
    RestoreErrorFrameElementsStyles: function() {
        var externalTable = this.GetExternalTable();
        externalTable.className = this.errorFrameStyles.errorFrame.cssClass;
        externalTable.style.cssText = this.errorFrameStyles.errorFrame.style;
        var controlCell = this.GetControlCell();
        controlCell.className = this.errorFrameStyles.controlCell.cssClass;
        controlCell.style.cssText = this.errorFrameStyles.controlCell.style;
    },
    ExtractElementStyleStringIgnoringVisibilityProps: function(element) {
        var savedVisibility = element.style.visibility;
        var savedDisplay = element.style.display;
        element.style.visibility = "";
        element.style.display = "";
        var styleStr = element.style.cssText;
        element.style.visibility = savedVisibility;
        element.style.display = savedDisplay;
        return styleStr;
    },
    ClearElementStyle: function(element) {
        if (!_aspxIsExists(element))
            return;
        element.className = "";
        var excludedAttrNames = [
   "width", "display", "visibility",
   "position", "left", "top", "z-index",
   "margin", "margin-top", "margin-right", "margin-bottom", "margin-left",
   "float", "clear"
  ];
        var savedAttrValues = {};
        for (var i = 0; i < excludedAttrNames.length; i++) {
            var attrName = excludedAttrNames[i];
            var attrValue = element.style[attrName];
            if (attrValue)
                savedAttrValues[attrName] = attrValue;
        }
        element.style.cssText = "";
        for (var styleAttrName in savedAttrValues)
            element.style[styleAttrName] = savedAttrValues[styleAttrName];
    },
    UpdateErrorCellContent: function() {
        if (this.errorDisplayMode.indexOf("t") > -1)
            this.UpdateErrorText();
        if (this.errorDisplayMode == "i")
            this.UpdateErrorImage();
    },
    UpdateErrorImage: function() {
        var image = this.GetErrorImage();
        if (_aspxIsExistsElement(image)) {
            image.alt = this.errorText;
            image.title = this.errorText;
        } else {
            this.UpdateErrorText();
        }
    },
    UpdateErrorText: function() {
        var errorTextCell = this.GetErrorTextCell();
        if (_aspxIsExistsElement(errorTextCell))
            errorTextCell.innerHTML = _aspxEncodeHtml(this.errorText);
    },
    ValidateWithPatterns: function() {
        if (this.validationPatterns.length > 0) {
            var value = this.GetValue();
            for (var i = 0; i < this.validationPatterns.length; i++) {
                var validator = this.validationPatterns[i];
                if (!validator.EvaluateIsValid(value)) {
                    this.SetIsValid(false);
                    this.SetErrorText(validator.errorText);
                    return;
                }
            }
        }
    },
    OnSpecialKeyDown: function(evt) {
        this.RaiseKeyDown(evt);
        var handler = this.keyDownHandlers[evt.keyCode];
        if (_aspxIsExists(handler))
            return this[handler](evt);
        return false;
    },
    OnSpecialKeyPress: function(evt) {
        this.RaiseKeyPress(evt);
        var handler = this.keyPressHandlers[evt.keyCode];
        if (_aspxIsExists(handler))
            return this[handler](evt);
        if (__aspxNetscapeFamily || __aspxOpera) {
            if (evt.keyCode == ASPxKey.Enter)
                return this.enterProcessed;
        }
        return false;
    },
    OnSpecialKeyUp: function(evt) {
        this.RaiseKeyUp(evt);
        var handler = this.keyUpHandlers[evt.keyCode];
        if (_aspxIsExists(handler))
            return this[handler](evt);
        return false;
    },
    OnKeyDown: function(evt) {
        if (!this.specialKeyboardHandlingUsed)
            this.RaiseKeyDown(evt);
    },
    OnKeyPress: function(evt) {
        if (!this.specialKeyboardHandlingUsed)
            this.RaiseKeyPress(evt);
    },
    OnKeyUp: function(evt) {
        if (!this.specialKeyboardHandlingUsed)
            this.RaiseKeyUp(evt);
    },
    RaiseKeyDown: function(evt) {
        if (!this.KeyDown.IsEmpty()) {
            var args = new ASPxClientEditKeyEventArgs(evt);
            this.KeyDown.FireEvent(this, args);
        }
    },
    RaiseKeyPress: function(evt) {
        if (!this.KeyPress.IsEmpty()) {
            var args = new ASPxClientEditKeyEventArgs(evt);
            this.KeyPress.FireEvent(this, args);
        }
    },
    RaiseKeyUp: function(evt) {
        if (!this.KeyUp.IsEmpty()) {
            var args = new ASPxClientEditKeyEventArgs(evt);
            this.KeyUp.FireEvent(this, args);
        }
    },
    RaiseFocus: function() {
        if (!this.GotFocus.IsEmpty()) {
            var args = new ASPxClientEventArgs();
            this.GotFocus.FireEvent(this, args);
        }
    },
    RaiseLostFocus: function() {
        if (!this.LostFocus.IsEmpty()) {
            var args = new ASPxClientEventArgs();
            this.LostFocus.FireEvent(this, args);
        }
    },
    RaiseValidation: function() {
        if (this.customValidationEnabled && !this.Validation.IsEmpty()) {
            var currentValue = this.GetValue();
            var args = new ASPxClientEditValidationEventArgs(currentValue, this.errorText, this.GetIsValid());
            this.Validation.FireEvent(this, args);
            this.SetErrorText(args.errorText);
            this.SetIsValid(args.isValid);
            if (args.value != currentValue)
                this.SetValue(args.value);
        }
    },
    RaiseValueChanged: function() {
        var processOnServer = this.autoPostBack;
        if (!this.ValueChanged.IsEmpty()) {
            var args = new ASPxClientProcessingModeEventArgs(processOnServer);
            this.ValueChanged.FireEvent(this, args);
            processOnServer = args.processOnServer;
        }
        return processOnServer;
    },
    RequireStyleDecoration: function() {
        this.styleDecoration = new ASPxClientEditStyleDecoration(this);
        this.PopulateStyleDecorationPostfixes();
    },
    PopulateStyleDecorationPostfixes: function() {
        this.styleDecoration.AddPostfix("");
    },
    Focus: function() {
        this.SetFocus();
    },
    GetIsValid: function() {
        var externalTable = this.GetExternalTable();
        return _aspxIsExistsElement(externalTable) ? this.isValid : true;
    },
    GetErrorText: function() {
        return this.errorText;
    },
    SetIsValid: function(isValid) {
        if (this.customValidationEnabled) {
            this.isValid = isValid;
            this.UpdateErrorFrameAndFocus(false);
            this.UpdateClientValidationState();
        }
    },
    SetErrorText: function(errorText) {
        if (this.customValidationEnabled) {
            this.errorText = errorText;
            this.UpdateErrorFrameAndFocus(false);
            this.UpdateClientValidationState();
        }
    },
    Validate: function() {
        this.ParseValue();
        this.OnValidation(ASPxValidationType.PersonalViaScript);
    }
});




//************************************************************************//


ASPxValidationPattern = _aspxCreateClass(null, {
    constructor: function(errorText) {
        this.errorText = errorText;
    }
});

//************************************************************************//

ASPxClientTextEdit = _aspxCreateClass(ASPxClientEdit, {
    constructor: function(name) {
        this.constructor.prototype.constructor.call(this, name);
        this.isASPxClientTextEdit = true;
        this.nullText = "";
        this.raiseValueChangedOnEnter = true;
        this.maskInfo = null;
        this.maskValueBeforeUserInput = "";
        this.maskPasteTimerID = -1;
        this.maskPasteLock = false;
        this.maskPasteCounter = 0;
        this.maskTextBeforePaste = "";
        this.maskHintHtml = "";
        this.maskHintTimerID = -1;
        this.displayFormat = null;
        this.TextChanged = new ASPxClientEvent();
    },
    InlineInitialize: function() {
        ASPxClientEdit.prototype.InlineInitialize.call(this);
        if (this.maskInfo != null)
            this.InitMask();
    },
    FindInputElement: function() {
        return this.isNative ? this.GetMainElement() : _aspxGetElementById(this.name + __aspxTEInputSuffix);
    },
    GetRawInputElement: function() {
        return _aspxGetElementById(this.name + __aspxTERawInputSuffix);
    },
    DecodeRawInputValue: function(value) {
        return value;
    },
    SetRawInputValue: function(value) {
        this.GetRawInputElement().value = value;
    },
    SyncRawInputValue: function() {
        if (this.maskInfo != null)
            this.SetRawInputValue(this.maskInfo.GetValue());
        else
            this.SetRawInputValue(this.GetInputElement().value);
    },
    HasTextDecorators: function() {
        return this.nullText != "" || this.displayFormat != null;
    },
    CanApplyTextDecorators: function() {
        return !this.focused;
    },
    GetDecoratedText: function(value) {
        var isNull = value == null || (value === "" && this.convertEmptyStringToNull);
        if (isNull && this.nullText != "")
            return this.nullText;
        if (this.displayFormat != null)
            return ASPxFormatter.Format(this.displayFormat, value);
        if (this.maskInfo != null)
            return this.maskInfo.GetText();
        if (value == null)
            return "";
        return value;
    },
    ToggleTextDecoration: function() {
        if (this.readOnly) return;
        if (!this.HasTextDecorators()) return;
        if (this.focused) {
            var input = this.GetInputElement();
            var oldValue = input.value;
            var sel = _aspxGetSelectionInfo(input);
            this.ToggleTextDecorationCore();
            if (oldValue != input.value) {
                if (sel.startPos == 0 && sel.endPos == oldValue.length)
                    sel.endPos = input.value.length;
                else
                    sel.endPos = sel.startPos;
                _aspxSetInputSelection(input, sel.startPos, sel.endPos);
            }
        } else {
            this.ToggleTextDecorationCore();
        }
    },
    ToggleTextDecorationCore: function() {
        if (this.maskInfo != null) {
            this.ApplyMaskInfo(false);
        } else {
            var input = this.GetInputElement();
            var rawValue = this.GetRawInputElement().value;
            var value = this.CanApplyTextDecorators() ? this.GetDecoratedText(rawValue) : rawValue;
            if (input.value != value)
                input.value = value;
        }
    },
    PopulateStyleDecorationPostfixes: function() {
        ASPxClientEdit.prototype.PopulateStyleDecorationPostfixes.call(this);
        this.styleDecoration.AddPostfix(__aspxTEInputSuffix);
    },
    GetValue: function() {
        var value = null;
        if (this.maskInfo != null)
            value = this.maskInfo.GetValue();
        else if (this.HasTextDecorators())
            value = this.GetRawInputElement().value;
        else
            value = this.GetInputElement().value;
        return (value == "" && this.convertEmptyStringToNull) ? null : value;
    },
    SetValue: function(value) {
        if (value == null) value = "";
        if (this.maskInfo != null) {
            this.maskInfo.SetValue(value);
            this.ApplyMaskInfo(false);
            this.SavePrevMaskValue();
        }
        else if (this.HasTextDecorators()) {
            this.SetRawInputValue(value);
            this.GetInputElement().value = this.CanApplyTextDecorators() ? this.GetDecoratedText(value) : value;
        }
        else
            this.GetInputElement().value = value;
        if (this.styleDecoration != null)
            this.styleDecoration.Update();
    },
    CollapseControl: function(checkSizeCorrectedFlag) {
        if (checkSizeCorrectedFlag && this.sizeCorrectedOnce)
            return;
        var mainElement = this.GetMainElement();
        if (!_aspxIsExistsElement(mainElement))
            return;
        if (this.WidthCorrectionRequired())
            this.GetInputElement().style.width = "0";
    },
    CorrectEditorWidth: function() {
        var inputElement = this.GetInputElement();
        var stretchedInputsManager = _aspxGetEditorStretchedInputElementsManager();
        try {
            stretchedInputsManager.HideInputElementsExceptOf(this);
            _aspxSetOffsetWidth(inputElement, _aspxGetClearClientWidth(_aspxFindOffsetParent(inputElement)));
        } finally {
            stretchedInputsManager.ShowInputElements();
        }
    },
    UnstretchInputElement: function() {
        var inputElement = this.GetInputElement();
        var mainElement = this.GetMainElement();
        var mainElementCurStyle = _aspxGetCurrentStyle(mainElement);
        if (_aspxIsExistsElement(mainElement) && _aspxIsExistsElement(inputElement) && _aspxIsExistsElement(mainElementCurStyle) &&
   inputElement.style.width == "100%" &&
   (mainElementCurStyle.width == "" || mainElementCurStyle.width == "auto"))
            inputElement.style.width = "";
    },
    RaiseValueChangedEvent: function() {
        var processOnServer = ASPxClientEdit.prototype.RaiseValueChangedEvent.call(this);
        processOnServer = this.RaiseTextChanged(processOnServer);
        return processOnServer;
    },
    InitMask: function() {
        this.SetValue(this.DecodeRawInputValue(this.GetRawInputElement().value));
        this.validationPatterns.unshift(new ASPxMaskValidationPattern(this.maskInfo.errorText, this.maskInfo));
        this.maskPasteTimerID = _aspxSetInterval("aspxMaskPasteTimerProc('" + this.name + "')", __aspxPasteCheckInterval);
    },
    SavePrevMaskValue: function() {
        this.maskValueBeforeUserInput = this.maskInfo.GetValue();
    },
    FillMaskInfo: function() {
        var input = this.GetInputElement();
        if (!input) return;
        var sel = _aspxGetSelectionInfo(input);
        this.maskInfo.SetCaret(sel.startPos, sel.endPos - sel.startPos);
    },
    ApplyMaskInfo: function(applyCaret) {
        this.SyncRawInputValue();
        var input = this.GetInputElement();
        var text = this.GetMaskDisplayText();
        this.maskTextBeforePaste = text;
        if (input.value != text)
            input.value = text;
        if (applyCaret)
            _aspxSetInputSelection(input, this.maskInfo.caretPos, this.maskInfo.caretPos + this.maskInfo.selectionLength);
    },
    GetMaskDisplayText: function() {
        if (!this.focused && this.HasTextDecorators())
            return this.GetDecoratedText(this.maskInfo.GetValue());
        return this.maskInfo.GetText();
    },
    ShouldCancelMaskKeyProcessing: function(htmlEvent, keyDownInfo) {
        return htmlEvent.returnValue === false;
    },
    HandleMaskKeyDown: function(evt) {
        var keyInfo = _aspxMaskManager.CreateKeyInfoByEvent(evt);
        _aspxMaskManager.keyCancelled = this.ShouldCancelMaskKeyProcessing(evt, keyInfo);
        if (_aspxMaskManager.keyCancelled) {
            _aspxPreventEvent(evt);
            return;
        }
        this.maskPasteLock = true;
        this.FillMaskInfo();
        var canHandle = _aspxMaskManager.CanHandleControlKey(keyInfo);
        _aspxMaskManager.savedKeyDownKeyInfo = keyInfo;
        if (canHandle) {
            _aspxMaskManager.OnKeyDown(this.maskInfo, keyInfo);
            this.ApplyMaskInfo(true);
            _aspxPreventEvent(evt);
        }
        _aspxMaskManager.keyDownHandled = canHandle;
        this.maskPasteLock = false;
        this.UpdateMaskHintHtml();
    },
    HandleMaskKeyPress: function(evt) {
        var keyInfo = _aspxMaskManager.CreateKeyInfoByEvent(evt);
        _aspxMaskManager.keyCancelled = _aspxMaskManager.keyCancelled || this.ShouldCancelMaskKeyProcessing(evt, _aspxMaskManager.savedKeyDownKeyInfo);
        if (_aspxMaskManager.keyCancelled) {
            _aspxPreventEvent(evt);
            return;
        }
        this.maskPasteLock = true;
        var printable = _aspxMaskManager.savedKeyDownKeyInfo != null && _aspxMaskManager.IsPrintableKeyCode(_aspxMaskManager.savedKeyDownKeyInfo);
        if (printable) {
            _aspxMaskManager.OnKeyPress(this.maskInfo, keyInfo);
            this.ApplyMaskInfo(true);
        }
        if (printable || _aspxMaskManager.keyDownHandled)
            _aspxPreventEvent(evt);
        this.maskPasteLock = false;
        this.UpdateMaskHintHtml();
    },
    MaskPasteTimerProc: function() {
        if (this.maskPasteLock) return;
        this.maskPasteCounter++;
        var inputElement = this.inputElement;
        if (!inputElement || this.maskPasteCounter > 40) {
            this.maskPasteCounter = 0;
            inputElement = this.GetInputElement();
            if (!_aspxIsExistsElement(inputElement)) {
                this.maskPasteTimerID = _aspxClearInterval(this.maskPasteTimerID);
                return;
            }
        }
        if (this.maskTextBeforePaste != inputElement.value) {
            this.maskInfo.ProcessPaste(inputElement.value, _aspxGetSelectionInfo(inputElement).endPos);
            this.ApplyMaskInfo(true);
        }
    },
    BeginShowMaskHint: function() {
        if (!this.readOnly && this.maskHintTimerID == -1)
            this.maskHintTimerID = window.setInterval(aspxMaskHintTimerProc, 500);
    },
    EndShowMaskHint: function() {
        window.clearInterval(this.maskHintTimerID);
        this.maskHintTimerID = -1;
    },
    MaskHintTimerProc: function() {
        if (this.maskInfo) {
            this.FillMaskInfo();
            this.UpdateMaskHintHtml();
        } else {
            this.EndShowMaskHint();
        }
    },
    UpdateMaskHintHtml: function() {
        var hint = this.GetMaskHintElement();
        if (!_aspxIsExistsElement(hint))
            return;
        var html = _aspxMaskManager.GetHintHtml(this.maskInfo);
        if (html == this.maskHintHtml)
            return;
        if (html != "") {
            var mainElement = this.GetMainElement();
            if (_aspxIsExistsElement(mainElement)) {
                hint.innerHTML = html;
                hint.style.position = "absolute";
                hint.style.left = _aspxGetAbsoluteX(mainElement) + "px";
                hint.style.top = (_aspxGetAbsoluteY(mainElement) + mainElement.offsetHeight + 2) + "px";
                hint.style.display = "block";
            }
        } else {
            hint.style.display = "none";
        }
        this.maskHintHtml = html;
    },
    HideMaskHint: function() {
        var hint = this.GetMaskHintElement();
        if (_aspxIsExistsElement(hint))
            hint.style.display = "none";
        this.maskHintHtml = "";
    },
    GetMaskHintElement: function() {
        return _aspxGetElementById(this.name + "_MaskHint");
    },
    OnMouseWheel: function(evt) {
        if (this.readOnly || this.maskInfo == null) return;
        this.FillMaskInfo();
        _aspxMaskManager.OnMouseWheel(this.maskInfo, _aspxGetWheelDelta(evt) < 0 ? -1 : 1);
        this.ApplyMaskInfo(true);
        _aspxPreventEvent(evt);
        this.UpdateMaskHintHtml();
    },
    OnKeyDown: function(evt) {
        ASPxClientEdit.prototype.OnKeyDown.call(this, evt);
        if (!this.specialKeyboardHandlingUsed && this.raiseValueChangedOnEnter && evt.keyCode == ASPxKey.Enter) {
            this.RaiseStandardOnChange();
            return;
        }
        if (!this.readOnly && this.maskInfo != null)
            this.HandleMaskKeyDown(evt);
    },
    OnKeyPress: function(evt) {
        ASPxClientEdit.prototype.OnKeyPress.call(this, evt);
        if (!this.readOnly && this.maskInfo != null)
            this.HandleMaskKeyPress(evt);
    },
    OnKeyUp: function(evt) {
        if (this.HasTextDecorators())
            this.SyncRawInputValue();
        ASPxClientEdit.prototype.OnKeyUp.call(this, evt);
    },
    OnFocusCore: function() {
        if (!this.GetEnabled()) {
            var inputElement = this.GetInputElement();
            if (_aspxIsExists(inputElement)) inputElement.blur();
            return;
        }
        ASPxClientEdit.prototype.OnFocusCore.call(this);
        if (this.maskInfo != null) {
            this.SavePrevMaskValue();
            this.BeginShowMaskHint();
        }
        this.ToggleTextDecoration();
    },
    OnLostFocusCore: function() {
        ASPxClientEdit.prototype.OnLostFocusCore.call(this);
        if (this.maskInfo != null) {
            this.EndShowMaskHint();
            this.HideMaskHint();
            if (this.maskInfo.ApplyFixes(null))
                this.ApplyMaskInfo(false);
            this.RaiseStandardOnChange();
        }
        this.ToggleTextDecoration();
    },
    OnValueChanged: function() {
        if (this.maskInfo != null) {
            if (this.maskInfo.GetValue() == this.maskValueBeforeUserInput)
                return;
            this.SavePrevMaskValue();
        }
        if (this.HasTextDecorators())
            this.SyncRawInputValue();
        ASPxClientEdit.prototype.OnValueChanged.call(this);
    },
    RaiseStandardOnChange: function() {
        var element = this.GetInputElement();
        if (_aspxIsExists(element) && _aspxIsExists(element.onchange)) {
            var eventMock = {
                target: this.GetInputElement()
            };
            element.onchange(eventMock);
        }
    },
    RaiseTextChanged: function(processOnServer) {
        if (!this.TextChanged.IsEmpty()) {
            var args = new ASPxClientProcessingModeEventArgs(processOnServer);
            this.TextChanged.FireEvent(this, args);
            processOnServer = args.processOnServer;
        }
        return processOnServer;
    },
    GetText: function() {
        if (this.maskInfo != null) {
            return this.maskInfo.GetText();
        } else {
            var value = this.GetValue();
            return value != null ? value : "";
        }
    },
    SetText: function(value) {
        if (this.maskInfo != null) {
            this.maskInfo.SetText(value);
            this.ApplyMaskInfo(false);
            this.SavePrevMaskValue();
        } else {
            this.SetValue(value);
        }
    },
    SelectAll: function() {
        this.SetSelection(0, -1, false);
    },
    SetCaretPosition: function(pos) {
        var inputElement = this.GetInputElement();
        _aspxSetCaretPosition(inputElement, pos);
    },
    SetSelection: function(startPos, endPos, scrollToSelection) {
        var inputElement = this.GetInputElement();
        _aspxSetSelection(inputElement, startPos, endPos, scrollToSelection);
    },
    ChangeEnabledAttributes: function(enabled) {
        var inputElement = this.GetInputElement();
        if (_aspxIsExists(inputElement)) {
            this.ChangeInputEnabledAttributes(inputElement, _aspxChangeAttributesMethod(enabled), enabled);
            if (this.specialKeyboardHandlingUsed)
                this.ChangeSpecialInputEnabledAttributes(inputElement, _aspxChangeEventsMethod(enabled));
            this.ChangeInputEnabled(inputElement, enabled, this.readOnly);
        }
    },
    ChangeEnabledStateItems: function(enabled) {
        if (!this.isNative) {
            var sc = aspxGetStateController();
            sc.SetElementEnabled(this.GetMainElement(), enabled);
            sc.SetElementEnabled(this.GetInputElement(), enabled);
        }
    },
    ChangeInputEnabled: function(element, enabled, readOnly) {
        if (this.UseReadOnlyForDisabled())
            element.readOnly = !enabled || readOnly;
        else
            element.disabled = !enabled;
    },
    ChangeInputEnabledAttributes: function(element, method, enabled) {
        if (enabled && __aspxWebKitFamily && element.tabIndex == -1)
            element.tabIndex = null;
        method(element, "tabIndex");
        if (!enabled) element.tabIndex = -1;
        method(element, "onclick");
        if (!this.NeedFocusCorrectionWhenDisabled())
            method(element, "onfocus");
        method(element, "onblur");
        method(element, "onkeydown");
        method(element, "onkeypress");
        method(element, "onkeyup");
    },
    UseReadOnlyForDisabled: function() {
        return (__aspxIE || __aspxOpera) && !this.isNative;
    },
    NeedFocusCorrectionWhenDisabled: function() {
        return __aspxIE && !this.isNative;
    }
});

ASPxIdent = {};
ASPxIdent.IsDate = function(obj) {
    return _aspxIsExists(obj) && obj.constructor == Date;
};
ASPxIdent.IsRegExp = function(obj) {
    return _aspxIsExists(obj) && obj.constructor === RegExp;
};
ASPxIdent.IsArray = function(obj) {
    return _aspxIsExists(obj) && obj.constructor == Array;
};
ASPxIdent.IsASPxClientControl = function(obj) {
    return obj != null && _aspxIsExists(obj.isASPxClientControl) && obj.isASPxClientControl;
};
ASPxIdent.IsASPxClientEdit = function(obj) {
    return _aspxIsExists(obj.isASPxClientEdit) && obj.isASPxClientEdit;
};

ASPxIdent.IsASPxClientTextEdit = function(obj) {
    return _aspxIsExists(obj.isASPxClientTextEdit) && obj.isASPxClientTextEdit;
};

//************************************************************************//

ASPxClientTextBoxBase = _aspxCreateClass(ASPxClientTextEdit, {
});

//************************************************************************//

ASPxClientButtonEditBase = _aspxCreateClass(ASPxClientTextBoxBase, {
    constructor: function(name) {
        this.constructor.prototype.constructor.call(this, name);
        this.allowUserInput = true;
        this.buttonCount = 0;
        this.ButtonClick = new ASPxClientEvent();
    },
    GetButton: function(number) {
        return this.GetChild("_B" + number);
    },
    ProcessInternalButtonClick: function(number) {
        return false;
    },
    OnButtonClick: function(number) {
        var processOnServer = this.RaiseButtonClick(number);
        if (!this.ProcessInternalButtonClick(number) && processOnServer)
            this.SendPostBack('BC:' + number);
    },
    SelectInputElement: function() {
        var element = this.GetInputElement();
        if (_aspxIsExistsElement(element)) {
            _aspxSetFocus(element);
            element.select();
        }
    },
    RaiseButtonClick: function(number) {
        var processOnServer = this.autoPostBack || this.IsServerEventAssigned("ButtonClick");
        if (!this.ButtonClick.IsEmpty()) {
            var args = new ASPxClientButtonEditClickEventArgs(processOnServer, number);
            this.ButtonClick.FireEvent(this, args);
            processOnServer = args.processOnServer;
        }
        return processOnServer;
    },
    ChangeEnabledAttributes: function(enabled) {
        ASPxClientTextEdit.prototype.ChangeEnabledAttributes.call(this, enabled);
        for (var i = 0; i < this.buttonCount; i++) {
            var element = this.GetButton(i);
            if (_aspxIsExists(element))
                this.ChangeButtonEnabledAttributes(element, _aspxChangeAttributesMethod(enabled));
        }
    },
    ChangeEnabledStateItems: function(enabled) {
        ASPxClientTextEdit.prototype.ChangeEnabledStateItems.call(this, enabled);
        for (var i = 0; i < this.buttonCount; i++) {
            var element = this.GetButton(i);
            if (_aspxIsExists(element))
                aspxGetStateController().SetElementEnabled(element, enabled);
        }
    },
    ChangeButtonEnabledAttributes: function(element, method) {
        method(element, "onclick");
        method(element, "ondblclick");
        method(element, "onmousedown");
        method(element, "onmouseup");
    },
    ChangeInputEnabled: function(element, enabled, readOnly) {
        ASPxClientTextEdit.prototype.ChangeInputEnabled.call(this, element, enabled, readOnly || !this.allowUserInput);
    }
});
ASPxClientButtonEdit = _aspxCreateClass(ASPxClientButtonEditBase, {
});
ASPxClientButtonEditClickEventArgs = _aspxCreateClass(ASPxClientProcessingModeEventArgs, {
    constructor: function(processOnServer, buttonIndex) {
        this.constructor.prototype.constructor.call(this, processOnServer);
        this.buttonIndex = buttonIndex;
    }
});
function aspxETextChanged(name) {
    var edit = aspxGetControlCollection().Get(name);
    if (edit != null) edit.OnTextChanged();
}
function aspxBEClick(name, number) {
    var edit = aspxGetControlCollection().Get(name);
    if (edit != null) edit.OnButtonClick(number);
}
function aspxMaskPasteTimerProc(name) {
    var edit = aspxGetControlCollection().Get(name);
    if (edit != null) edit.MaskPasteTimerProc();
}
function aspxMaskHintTimerProc() {
    var focusedEditor = ASPxClientEdit.GetFocusedEditor();
    if (focusedEditor != null && _aspxIsFunction(focusedEditor.MaskHintTimerProc))
        focusedEditor.MaskHintTimerProc();
}
function _aspxSetFocusToTextEditWithDelay(name) {
    _aspxSetTimeout("var edit = aspxGetControlCollection().Get('" + name + "'); __aspxIE ? edit.SetCaretPosition(0) : edit.SetFocus();", 500);
}

