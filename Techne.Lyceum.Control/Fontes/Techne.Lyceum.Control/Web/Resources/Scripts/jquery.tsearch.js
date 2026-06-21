

(function($) {
	$.fn.tsearch = function(options) {
		return this.each(function() {
			new $.TSearchPlugin(this, options);
		});
	};

	$.TSearchPlugin = function(ts, options) {
		var $ts = $(ts);
		var $namefield = null;
		var $keyfield = null;
		var $button = null;
		var $popup = null;
		var $rowdata = null;
		var $anchor = null;
		var $searchButton = null;
		var $grid = null;
		var $msg = null;
		if (typeof options.keyFieldID != 'undefined' && $('#' + options.keyFieldID) != null)
			$keyfield = $('#' + options.keyFieldID);
		if (typeof options.nameFieldID != 'undefined' && $('#' + options.nameFieldID) != null)
			$namefield = $('#' + options.nameFieldID);
		if (typeof options.buttonID != 'undefined' && $('#' + options.buttonID) != null)
			$button = $('#' + options.buttonID);
		if (typeof options.popupID != 'undefined' && $('#' + options.popupID) != null)
			$popup = $('#' + options.popupID);
		if (typeof options.rowDataID != 'undefined' && $('#' + options.rowDataID) != null)
			$rowdata = $('#' + options.rowDataID);
		if (options.anchorId != null)
			$anchor = $('#' + options.anchorId);
		if (options.searchButtonId != null)
			$searchButton = $('#' + options.searchButtonId);
		if (options.gridId != null)
			$grid = $('#' + options.gridId);
		if (options.messageId != null)
			$msg = $('#' + options.messageId);

		//adiciona plugin à tabela
		$grid.jqGrid(
        {
        	datatype: function(callbackdata) { },
        	width: options.width,
        	colNames: options.colNames,
        	colModel: options.colModel,
        	rowNum: options.rowNum,
        	rowList: options.rowList,
        	imgpath: options.imgpath,
        	//pager: '#' + options.pagerId,
        	sortname: options.sortname,
        	sortorder: options.sortorder,
        	viewrecords: true,
        	rownumbers: true,
        	gridview: true,
        	onSelectRow: function(rowid) {
        		if (typeof rowid != 'undefined' && rowid != null) {
        			var rowdata = $grid.getRowData(rowid);
        			$popup[0].rowValues = rowdata;
        			if (typeof $popup[0].onSelectRow != 'undefined' && $popup[0].onSelectRow != null) {
        				$popup[0].onSelectRow();
        			}
        			$popup[0].hidePopup();
        		}
        	}
        });

		//cria popup modal
		if (options.modal == true) {
			$popup.jqm();
		}

		//atribui máscaras/filtros de caracteres
		if (typeof options.filterFields != 'undefined' && options.filterFields != null) {
			var i;
			for (i = 0; i < options.filterFields.length; i++) {
				if (typeof options.filterFields[i].mask != 'undefined') {
					var $field = $('#' + options.filterFields[i].id);
					$field.mask(options.filterFields[i].mask, { placeholder: " " });
				}
				else if (typeof options.filterFields[i].charFilter != 'undefined') {
					var $field = $('#' + options.filterFields[i].id);
					if (options.charFilter == 'integer')
						$field.tsearchNumeric();
					else if (options.charFilter == 'decimal')
						$field.tsearchNumeric({ allow: '.,' });
				}
			}
		}
		if (typeof options.mask != 'undefined' && $keyfield != null)
			$keyfield.mask(options.mask, { placeholder: " " });
		else if (typeof options.charFilter != 'undefined' && $keyfield != null) {
			if (options.charFilter == 'integer')
				$keyfield.tsearchNumeric();
			else if (options.charFilter == 'decimal')
				$keyfield.tsearchNumeric({ allow: '.,' });
		}

		var disableFilter = function(disable) {
			$searchButton[0].disabled = disable || options.disabled;
			if (typeof options.filterFields != 'undefined' && options.filterFields != null) {
				for (i = 0; i < options.filterFields.length; i++) {
					var $field = $('#' + options.filterFields[i].id);
					if ($field.length > 0) {
						$field[0].disabled = disable;
					}
				}
			}
		};

		var getData = function(callbackdata) {
			var success = function(xmldata) {
				//hack para contornar bug da jqGrid
				$.TSearchPlugin.endReq($grid);
				var jsondata = $.TSearchPlugin.parseJSON(xmldata);
				$grid[0].addJSONData(jsondata);
				if (typeof jsondata.message != 'undefined') {
					disableFilter(false);
					if ($msg != null) {
						$msg[0].innerHTML = jsondata.message;
						$msg.show();
					}
				}
			}
			var failure = function(xmldata) {
				//hack para contornar bug da jqGrid
				$.TSearchPlugin.endReq($grid);
				disableFilter(false);
			};
			var context = null;
			var filter = new Array();
			var i;
			if (typeof options.filterFields != 'undefined' && options.filterFields != null) {
				for (i = 0; i < options.filterFields.length; i++) {
					var $field = $('#' + options.filterFields[i].id);
					if ($field.is(':checkbox')) {
						filter[filter.length] = ({ name: options.filterFields[i].name, value: $field.get(0).checked ? 'true' : 'false' });
					}
					else if ($field.length > 0) {
						filter[filter.length] = ({ name: options.filterFields[i].name, value: $field.val() });
					}
				}
			}
			if (options.extraFilter != null) {
				for (i = 0; i < options.extraFilter.length; i++) {
					filter[filter.length] = ({ name: options.extraFilter[i].name, value: options.extraFilter[i].value });
				}
			}
			if (typeof $grid.clearGridData == 'function')
				$grid.clearGridData();
			$msg[0].innerHTML = 'Aguarde...';

			//hack para contornar bug da jqGrid
			$.TSearchPlugin.beginReq($grid);

			disableFilter(true);
			callbackdata.filter = filter;
			callbackdata.operation = 'gridSearch';
			doCallback(callbackdata, success, context, failure);
		};


		//evento de click para o botão de busca
		if (typeof $searchButton != undefined) {
			$searchButton.click(function(event) {
				$grid.setGridParam({ datatype: getData, page: 1 }).trigger("reloadGrid");
				event.preventDefault();
			});
		};
		//evento de click do body
		//esconde o popup quando se clica fora dele
		$(document).click(function(event) {
			if (typeof $popup[0].openingPopup != 'undefined')
				$popup[0].openingPopup = undefined;
			else {
				if ($anchor == null || typeof $anchor[0] == 'undefined' || $anchor[0].id != event.target.id) {
					$popup[0].hidePopup();
				}
			}
		});

		//não esconde o popup quando se clica dentro dele
		//impede a propagação do evento de click do popup para o body
		$popup.click(function(event) {
			event.stopPropagation();
		});

		//método para limpar o filtro
		$popup[0].clearFilter = function() {
			if (typeof options.filterFields != 'undefined' && options.filterFields != null) {
				for (i = 0; i < options.filterFields.length; i++) {
					var $field = $('#' + options.filterFields[i].id);
					$field.val('');
				}
			}
			options.extraFilter = new Array();
		}

		//método para popular os campos de filtro
		$popup[0].setFilter = function(filterValues, keepOldValues) {
			var i, j, k, fieldFound;
			var filterName, filterVal;

			//limpa filtro anterior
			if (keepOldValues != true)
				$popup[0].clearFilter();
			if (typeof options.extraFilter == 'undefined')
				options.extraFilter = new Array();

			//preenche filtros
			if (filterValues != null) {
				for (j = 0; j < filterValues.length; j++) {
					fieldFound = false;
					if (typeof filterValues[j].name == 'undefined')
						continue;
					filterName = filterValues[j].name.toLowerCase();
					if (typeof filterValues[j].value != 'undefined')
						filterVal = filterValues[j].value;
					else if (typeof filterValues[j].controlid != 'undefined')
						filterVal = $('#' + filterValues[j].controlid).val();
					//filtro visível
					if (typeof options.filterFields != 'undefined' && options.filterFields != null) {
						for (i = 0; i < options.filterFields.length; i++) {
							if (options.filterFields[i].name == filterName) {
								var $field = $('#' + options.filterFields[i].id);
								if ($field.length > 0) {
									fieldFound = true;
									$field.val(filterVal);
								}
							}
						}
					}
					//filtro invisível
					if (!fieldFound) {
						fieldFound = false;
						for (k = 0; k < options.extraFilter.length; k++) {
							if (options.extraFilter[k].name == filterName) {
								options.extraFilter[k].value = filterVal;
								fieldFound = true;
							}
						}
						if (!fieldFound) {
							options.extraFilter[options.extraFilter.length] = { name: filterName, value: filterVal };
						}
					}
				}
			}
		}
		//método para mostrar o popup
		$popup[0].showPopup = function(filterValues, autoexecute) {
			var auto = (autoexecute == true ? true : false);
			if (filterValues != null && typeof filterValues != 'undefined') {
				$popup[0].setFilter(filterValues);
				auto = true;
			}
			if (options.modal == true) {
				$popup.css({ visibility: 'visible' });
				$.TSearchPlugin.center($popup);
				$popup.jqmShow();
			}
			else {
				$popup.css({ visibility: 'visible' });
				$.TSearchPlugin.setPosition($popup, $anchor);
				$popup.show();
			}
			if (auto)
				$searchButton.click();
			$popup[0].openingPopup = true;
		};

		//método para esconder o popup
		$popup[0].hidePopup = function() {
			if (typeof $popup[0].onHide != 'undefined' && $popup[0].onHide != null)
				$popup[0].onHide();
			if (options.modal == true) {
				$popup.jqmHide();
			}
			else
				$popup.hide();
		}

		//adiciona atributos de referência
		$ts[0].options = options;

		//guarda valores iniciais para auxiliar validações
		if ($namefield != null) {
			$namefield.attr('validatedName', $namefield.val());
		}

		//Adiciona funções getText, getDescription, getRow e setFilter
		$ts[0].getDescription = function() { return $namefield == null ? '' : $namefield.val(); };
		$ts[0].getText = function() { return $keyfield == null ? '' : $keyfield.val(); }
		$ts[0].setDescription = function(text) { if ($namefield != null) $namefield.val(text); }
		$ts[0].setText = function(text) { if ($keyfield != null) $keyfield.val(text); }
		$ts[0].getRow = function() { return $rowdata.val() == '' ? null : $.TSearchPlugin.parseJSON($rowdata.val()); }
		$ts[0].forceValid = function() {
			var rowdata = new Object();
			if ($keyfield != null && typeof options.textColumn != 'undefined') {
				rowdata[options.textColumn.toLowerCase()] = $keyfield.val();
			}
			if ($namefield != null && typeof options.descriptionColumn != 'undefined') {
				rowdata[options.descriptionColumn.toLowerCase()] = $namefield.val();
			}
			$rowdata.val($.TSearchPlugin.toJSON(rowdata));
		};

		$ts[0].setFilterValue = function(name, value) {
			if ($popup != null && $popup.length > 0) {
				$popup[0].setFilter([{ name: name, value: value}], true);
			}
		}
		$ts[0].setFilter = function(filterValues) {
			if ($popup != null && $popup.length > 0)
				$popup[0].setFilter(filterValues, false);
		}
		$ts[0].clearFilter = function() {
			if ($popup != null && $popup.length > 0)
				$popup[0].clearFilter();
		}
		$ts[0].setAutoExecute = function(autoexecute) {
			$ts[0].options.autoExecute = (autoexecute == true ? true : false);
		};
		$ts[0].clearGrid = function(keepFilter) {
			if ($grid != null) {
				$grid.clearGridData();
				$.TSearchPlugin.endReq($grid);
			}
			if (keepFilter != true)
				$ts[0].clearFilter();
			$msg[0].innerHTML = '';
		}

		//adiciona evento onValueChanged
		$ts.bind("onValueChanged", function(event) {
			if (typeof $ts[0].onValueChanged == 'function') {
				$ts[0].onValueChanged($ts[0]);
			}
			if (typeof options.onValueChanged != 'undefined') {
				var onValueChanged = $.TSearchPlugin.evalFunction(options.onValueChanged);
				if (typeof onValueChanged == 'function') {
					onValueChanged($ts[0], event);
				}
			}
		});

		//adiciona evento onButtonClick
		$ts.bind("onButtonClick", function() {
			if (typeof $ts[0].onButtonClick == 'function') {
				$ts[0].onButtonClick($ts[0]);
			}
		});

		//função de postback
		var tsearchDoPostBack = function() {
			var pb = $.TSearchPlugin.evalFunction(options.postback);
			if (typeof pb == 'function') {
				var keyVal = $keyfield == null ? null : $keyfield.val();
				var origKeyVal = $keyfield == null ? null : $keyfield.attr('originalValue');
				if (typeof origKeyVal != 'undefined' && keyVal != origKeyVal) {
					pb();
					options.postback = null; //remove função de postback para que ela seja chamada somente uma vez
				}
			}
		}

		$ts[0].onTextFieldKeyPress = function(event) {
			if (event.keyCode == 13) {
				$.TSearchPlugin.preventDefault(event);
				$keyfield.change();
			}
		};

		var doCallback = function(callbackdata, success, context, failure) {
			options.callbackSuccess = success;
			options.callbackFailure = failure;
			var postdata = $.TSearchPlugin.toJSON(callbackdata);
			var callbackMethod = $.TSearchPlugin.evalFunction(options.callbackMethod);
			callbackMethod(postdata, success, context, failure);
		};

		//valida campo chave
		$ts[0].onTextFieldChange = function() {
			var keyValue = $keyfield.val();
			while (keyValue != null && keyValue.length > 0 && keyValue[0] == ' ')
				keyValue = keyValue.substr(1);
			if (keyValue == null || keyValue.length == 0) {
				$keyfield.val('');
				if ($namefield != null) {
					$namefield.val('');
					$namefield.attr('validatedName', '');
				}
				$rowdata.val('');
				$ts.trigger('onValueChanged');
				tsearchDoPostBack();
			}
			else {
				if ($namefield != null) {
					$namefield.val('');
					$namefield.attr('validatedName', '');
				}
				var success = function(strdata) {
					var jsondata = $.TSearchPlugin.parseJSON(strdata);
					var vals = new Array();
					var erro = '';
					if (jsondata == null || typeof jsondata == 'undefined') {
						erro = 'Código inválido';
					}
					else {
						if (typeof jsondata.message != 'undefined')
							erro = jsondata.message;
						else
							erro = '';
					}
					if (erro != '' && erro != null) {
						$rowdata.val('');
						if ($namefield != null) {
							$namefield.val('');
							$namefield.attr('validatedName', '');
						}
						$ts.trigger('onValueChanged');
						var textValidation = (typeof options.textValidation != 'undefined' && options.textValidation == true);
						if (!textValidation) {
							tsearchDoPostBack();
						}
						else {
							$keyfield.val('');
							alert(erro);
							if ($keyfield[0].disabled != true)
								$keyfield[0].focus();
						}
					}
					else {
						$keyfield.val(jsondata.keyvalue);
						if ($namefield != null) {
							$namefield.val(jsondata.namevalue);
							$namefield.attr('validatedName', jsondata.namevalue);
						}
						if ($rowdata != null)
							$rowdata.val($.TSearchPlugin.toJSON(jsondata.rowdata));
						$ts.trigger('onValueChanged');
						tsearchDoPostBack();
					}
					$ts[0].doingCallback = 0;
				};
				var failure = function(strdata) {
					$ts[0].doingCallback = 0;
					$keyfield.val('');
					if ($namefield != null) {
						$namefield.val('');
						$namefield.attr('validatedName', '');
					}
					$rowdata.val('');
					$ts.trigger('onValueChanged');
					alert('Erro na requisição. Tente novamente.');
					if ($keyfield[0].disabled != true)
						$keyfield[0].focus();
				};
				var context = null;
				var callbackdata = new Object();
				callbackdata.operation = "description";
				callbackdata.key = $keyfield.val();
				$ts[0].doingCallback = 5;
				doCallback(callbackdata, success, context, failure);
			}
		};


		//cancela tecla ENTER para o campo descrição
		$ts[0].onDescriptionFieldKeyPress = function(event) {
			if (event.keyCode == 13) {
				$.TSearchPlugin.preventDefault(event);
			}
		};

		//limpa campo chave se o campo descrição for alterado manualmente
		$ts[0].onDescriptionFieldKeyUp = function(event) {
			if (event.keyCode == 13 || event.keyCode == 10) {
				if ($button != null)
					tsearchOnButtonClick(true);
				return;
			}
			var nameValue = $namefield.val();
			var validatedName = $namefield.attr('validatedName');
			if (typeof validatedName != 'undefined' && nameValue != validatedName)
				$keyfield.val('');

		};

		$ts[0].onDescriptionFieldBlur = function(event) {
			var keyVal = $keyfield.val();
			var origKeyVal = $keyfield.attr('originalValue');
			if (keyVal == '' || keyVal == null)
				$namefield.val('');
			if (typeof origKeyVal != 'undefined' && keyVal != origKeyVal)
				tsearchDoPostBack();
		};

		$ts[0].onSearchButtonClick = function(event) {
			tsearchOnButtonClick();
			$.TSearchPlugin.preventDefault(event);
		};


		$popup[0].onSelectRow = function() {
			var popup = this;
			if ($keyfield != null) {
				$keyfield.val(popup.rowValues[options.keyColumn]);
			}
			if ($namefield != null) {
				$namefield.val(popup.rowValues[options.nameColumn]);
				$namefield.attr('validatedName', popup.rowValues[options.nameColumn]);
			}
			if ($rowdata != null)
				$rowdata.val($.TSearchPlugin.toJSON(popup.rowValues));
			$ts.trigger('onValueChanged');
			if ($keyfield != null) {
				tsearchDoPostBack();
			}
		}

		$popup[0].onHide = function() {
			if ($keyfield != null)
				$keyfield[0].disabled = false || options.disabled;
			if ($namefield != null)
				$namefield[0].disabled = false || options.disabled;
		}

		var tsearchOnButtonClick = function(filterByDescription) {
			$ts.trigger('onButtonClick');
			if ($ts[0] != null && $ts[0].doingPostBack == true)
				$ts[0].doingPostBack = false;
			else if (typeof $ts[0].doingCallback != 'undefined' && $ts[0].doingCallback > 0) {
				$ts[0].doingCallback = $ts[0].doingCallback - 1;
				if ($ts[0].doingCallback < 0)
					$ts[0].doingCallback = 0;
			}
			else {
				if (filterByDescription == true && $namefield != null) {
					var nvalue = $namefield.val();
					var validatedName = $namefield.attr('validatedName');
					if (validatedName != nvalue) {
						if ($keyfield != null)
							$keyfield.val('');
						$namefield.val('');
					}
					$popup[0].showPopup([{ name: options.nameColumn, value: nvalue}]);
				}
				else
					$popup[0].showPopup(null, $ts[0].options.autoExecute);

				if ($keyfield != null)
					$keyfield[0].disabled = true || options.disabled;
				if ($namefield != null)
					$namefield[0].disabled = true || options.disabled;
			}
			return false;
		}

	};

	$.fn.extend({
		tsearchPopup: function(options) {
			options = $.extend({}, $.TSearchPopupControl.defaults, {
				anchorId: null,
				filterFields: null,
				searchButtonId: null
			}, options);

			return this.each(function() {
				new $.TSearchPopupControl(this, options);

			});
		}
	});

	$.TSearchPopupControl = function(popup, options) {


	};

	$.TSearchPlugin.beginReq = function($ts) {
		var ts = $ts[0];
		ts.grid.hDiv.loading = true;
		var grid = ts.grid;
		switch (ts.p.loadui) {
			case "disable":
				break;
			case "enable":
				$("#load_" + ts.p.id).fadeIn("fast").css({ visibility: 'visible' });
				break;
			case "block":
				$("#lui_" + ts.id).width($(grid.bDiv).width()).height(IntNum($(grid.bDiv).height()) + IntNum(ts.p._height)).fadeIn("fast").css({ visibility: 'visible' });
				break;
		}
		ts.temploadui = ts.p.loadui;
		ts.p.loadui = '';
	};
	$.TSearchPlugin.endReq = function($ts) {
		var ts = $ts[0];
		ts.grid.hDiv.loading = false;
		var grid = ts.grid;
		if (typeof ts.p.loadui != 'undefined' && typeof ts.temploadui != 'undefined' && ts.temploadui != null) {
			ts.p.loadui = ts.temploadui;
			ts.temploadui = null;
		}

		switch (ts.p.loadui) {
			case "disable":
				break;
			case "enable":
				$("#load_" + ts.p.id).fadeOut("fast").css({ visibility: 'hidden' });
				break;
			case "block":
				$("#lui_" + ts.id).fadeOut("fast").css({ visibility: 'hidden' });
				break;
		}
	};
	$.TSearchPlugin.preventDefault = function(event) {
		if (typeof event.preventDefault != 'undefined')
			event.preventDefault();
		else {
			event.cancelBubble = true;
			event.returnValue = false;
		}
	};

	$.TSearchPlugin.evalFunction = function(txt) {
		if (typeof txt == 'function')
			return txt;
		var zz;
		var txtCorr = "zz=" + txt;
		eval(txtCorr);
		return zz;
	};

	$.TSearchPlugin.center = function(obj, absolute) {
		var t = jQuery(obj);

		t.css({
			position: absolute ? 'absolute' : 'fixed',
			left: '50%',
			top: '50%',
			zIndex: '99'
		}).css({
			marginLeft: '-' + (t.outerWidth() / 2) + 'px',
			marginTop: '-' + (t.outerHeight() / 2) + 'px'
		});

		if (absolute) {
			t.css({
				marginTop: parseInt(t.css('marginTop'), 10) + jQuery(window).scrollTop(),
				marginLeft: parseInt(t.css('marginLeft'), 10) + jQuery(window).scrollLeft()
			});
		}

	};

	$.TSearchPlugin.setPosition = function($popup, $reference) {
		var reference = $reference[0];
		var offset = $(reference).offset();
		var offsetDisp = { top: 0, left: 0 };
		if ($.browser.mozilla == false) {
			var offsetParent = ($(reference).css('position') == 'absolute') ? $(reference).parents().eq(0).offsetParent() : $(reference).offsetParent();
			offsetDisp = offsetParent.offset();
		}

		var anchortop = offset.top - offsetDisp.top;
		var anchorleft = offset.left - offsetDisp.left;

		var ptop = anchortop + reference.offsetHeight;
		var pleft = anchorleft;
		var pheight = $popup.outerHeight();
		var pwidth = $popup.outerWidth();

		var scrolledX = $(document).scrollLeft();
		var scrolledY = $(document).scrollTop();
		var screenWidth = 0;
		var screenHeight = 0;

		if (typeof (window.innerWidth) == 'number') {
			//Non-IE
			screenWidth = window.innerWidth;
			screenHeight = window.innerHeight;
		} else if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight)) {
			//IE 6+ in 'standards compliant mode'
			screenWidth = document.documentElement.clientWidth;
			screenHeight = document.documentElement.clientHeight;
		} else if (document.body && (document.body.clientWidth || document.body.clientHeight)) {
			//IE 4 compatible
			screenWidth = document.body.clientWidth;
			screenHeight = document.body.clientHeight;
		}

		if (ptop + pheight > screenHeight - scrolledY)
			ptop = anchortop - pheight;
		if (ptop < scrolledY)
			ptop = anchortop + reference.offsetHeight;


		if (pleft + pwidth > screenWidth - scrolledX)
			pleft = anchorleft + $(reference).width() - pwidth;
		if (pleft < scrolledX)
			pleft = anchorleft;

		$popup.css({
			top: ptop,
			left: pleft
		});
	}

	var TSearchPlugin_m = {
		'\b': '\\b',
		'\t': '\\t',
		'\n': '\\n',
		'\f': '\\f',
		'\r': '\\r',
		'"': '\\"',
		'\\': '\\\\'
	},
        TSearchPlugin_s = {
        	'array': function(x) {
        		var a = ['['], b, f, i, l = x.length, v;
        		for (i = 0; i < l; i += 1) {
        			v = x[i];
        			f = TSearchPlugin_s[typeof v];
        			if (f) {
        				v = f(v);
        				if (typeof v == 'string') {
        					if (b) {
        						a[a.length] = ',';
        					}
        					a[a.length] = v;
        					b = true;
        				}
        			}
        		}
        		a[a.length] = ']';
        		return a.join('');
        	},
        	'boolean': function(x) {
        		return String(x);
        	},
        	'null': function(x) {
        		return "null";
        	},
        	'number': function(x) {
        		return isFinite(x) ? String(x) : 'null';
        	},
        	'object': function(x) {
        		if (x) {
        			if (x instanceof Array) {
        				return TSearchPlugin_s.array(x);
        			}
        			var a = ['{'], b, f, i, v;
        			for (i in x) {
        				v = x[i];
        				f = TSearchPlugin_s[typeof v];
        				if (f) {
        					v = f(v);
        					if (typeof v == 'string') {
        						if (b) {
        							a[a.length] = ',';
        						}
        						a.push(TSearchPlugin_s.string(i), ':', v);
        						b = true;
        					}
        				}
        			}
        			a[a.length] = '}';
        			return a.join('');
        		}
        		return 'null';
        	},
        	'string': function(x) {
        		if (/["\\\x00-\x1f]/.test(x)) {
        			x = x.replace(/([\x00-\x1f\\"])/g, function(a, b) {
        				var c = TSearchPlugin_m[b];
        				if (c) {
        					return c;
        				}
        				c = b.charCodeAt();
        				return '\\u00' +
                            Math.floor(c / 16).toString(16) +
                            (c % 16).toString(16);
        			});
        		}
        		return '"' + x + '"';
        	}
        };

	$.TSearchPlugin.toJSON = function(v) {
		var f = isNaN(v) ? TSearchPlugin_s[typeof v] : TSearchPlugin_s['number'];
		if (f) return f(v);
	};

	$.TSearchPlugin.parseJSON = function(v, safe) {
		if (safe === undefined) safe = $.TSearchPlugin.parseJSON.safe;
		if (safe && !/^("(\\.|[^"\\\n\r])*?"|[,:{}\[\]0-9.\-+Eaeflnr-u \n\r\t])+?$/.test(v))
			return undefined;
		return eval('(' + v + ')');
	};

	$.TSearchPlugin.parseJSON.safe = false;

	$.fn.tsearchAlphanumeric = function(p) {
		p = $.extend({
			ichars: "!@#$%^&*()+=[]\\\';,/{}|\":<>?~`.- ",
			nchars: "",
			allow: ""
		}, p);
		return this.each
			(
				function() {
					if (p.nocaps) p.nchars += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
					if (p.allcaps) p.nchars += "abcdefghijklmnopqrstuvwxyz";
					var s = p.allow.split('');
					for (i = 0; i < s.length; i++) if (p.ichars.indexOf(s[i]) != -1) s[i] = "\\" + s[i];
					p.allow = s.join('|');
					var reg = new RegExp(p.allow, 'gi');
					var ch = p.ichars + p.nchars;
					ch = ch.replace(reg, '');
					$(this).keypress
						(
							function(e) {

								if (!e.charCode) k = String.fromCharCode(e.which);
								else k = String.fromCharCode(e.charCode);

								if (ch.indexOf(k) != -1) e.preventDefault();
								if (e.ctrlKey && k == 'v') e.preventDefault();

							}

						);
					$(this).bind('contextmenu', function() { return false });
				}
			);

	};

	$.fn.tsearchNumeric = function(p) {
		var az = "abcdefghijklmnopqrstuvwxyz";
		az += az.toUpperCase();
		p = $.extend({
			nchars: az
		}, p);
		return this.each(function() {
			$(this).tsearchAlphanumeric(p);
		});

	};

	$.fn.tsearchAlpha = function(p) {
		var nm = "1234567890";
		p = $.extend({
			nchars: nm
		}, p);
		return this.each(function() { $(this).tsearchAlphanumeric(p); });
	};


})(jQuery);

function tsearchFind(cfg) {
    if (typeof cfg == 'undefined' || cfg == null)
        return null;
    var $cfg = $('#' + cfg);
    if ($cfg == null || $cfg.length == 0)
        return null;
    var options = $.TSearchPlugin.parseJSON($cfg.val());
    var $ts = $('#' + options.tsID);
    if ($ts.length == 0)
        return null;
    var initialized = $ts.attr('initialized');
    if (typeof initialized != 'undefined')
        return $ts;
    var $popup = $('#' + options.popupID);
    $popup.tsearchPopup(options);
    $ts.tsearch(options);
    $ts.attr('initialized', 'true');
    return $ts;
}

function tsearchControl(id) {
    var $ts=null,cfg,$ctl;
    $ctl = $('#' + id);
    if (typeof $ctl.attr('cfg') != 'undefined') {
        cfg = $ctl.attr('cfg');
        $ts=tsearchFind(cfg);
    }
    if ($ts != null && $ts.length > 0)
        return $ts[0];
    else
        return null;
}

function tsearchTextFieldKeyPress(cfg,event) {
    var $ts=tsearchFind(cfg);
    if($ts.length>0)
        $ts[0].onTextFieldKeyPress(event);
}

function tsearchTextFieldChange(cfg) {
    var $ts=tsearchFind(cfg);
    if($ts.length>0)
        $ts[0].onTextFieldChange();
}

function tsearchDescriptionFieldKeyPress(cfg,event) {
    var $ts=tsearchFind(cfg);
    if($ts.length>0)
        $ts[0].onDescriptionFieldKeyPress(event);
}

function tsearchDescriptionFieldKeyUp(cfg,event) {
    var $ts=tsearchFind(cfg);
    if($ts.length>0)
        $ts[0].onDescriptionFieldKeyUp(event);
}

function tsearchDescriptionFieldBlur(cfg,event) {
    var $ts=tsearchFind(cfg);
    if($ts.length>0)
        $ts[0].onDescriptionFieldBlur(event);
}

function tsearchButtonClick(cfg,event) {
    var $ts=tsearchFind(cfg);
    if($ts.length>0)
        $ts[0].onSearchButtonClick(event);
}

function tsearchCallbackSuccess(s, e, id) {
    var $ctl = $('#' + id);
    if ($ctl.length > 0 && typeof $ctl[0].options != 'undefined' && typeof $ctl[0].options.callbackSuccess != 'undefined')
        $ctl[0].options.callbackSuccess(e.result);
}

function tsearchCallbackFailure(s, e, id) {
    var $ctl = $('#' + id);
    if ($ctl.length > 0 && typeof $ctl[0].options != 'undefined' && typeof $ctl[0].options.callbackFailure != 'undefined')
        $ctl[0].options.callbackFailure(e.result);
}


(function($) {
    $.fn.tsearchbox = function(options) {
        return this.each(function() {
            new $.TSearchBoxPlugin(this, options);
        });
    };

    $.TSearchBoxPlugin = function(popup, options) {
        var $ts = $(popup);
        var $textfield = null;
        var $descriptionfield = null;
        var $button = null;
        var $popup = null;
        var $grid = null;
        var $pager = null;
        var $rowdata = null;
        if (typeof options.text != 'undefined' && $('#' + options.text) != null)
            $textfield = $('#' + options.text);
        if (typeof options.description != 'undefined' && $('#' + options.description) != null)
            $descriptionfield = $('#' + options.description);
        if (typeof options.button != 'undefined' && $('#' + options.button) != null)
            $button = $('#' + options.button);
        if (typeof options.popup != 'undefined' && $('#' + options.popup) != null)
            $popup = $('#' + options.popup);
        if (typeof options.grid != 'undefined' && $('#' + options.grid) != null)
            $grid = $('#' + options.grid);
        if (typeof options.pager != 'undefined' && $('#' + options.pager) != null)
            $pager = $('#' + options.pager);
        if (typeof options.rowdata != 'undefined' && $('#' + options.rowdata) != null)
            $rowdata = $('#' + options.rowdata);

        $textfield[0].options = options;

        //método de busca de dados para a grid
        var getGridData = function(callbackdata) {
            var success = function(xmldata) {
                //hack para contornar bug da jqGrid
                $.TSearchPlugin.endReq($grid);
                var jsondata = $.TSearchPlugin.parseJSON(xmldata);
                $grid[0].addJSONData(jsondata);
            }
            var failure = function(xmldata) {
                //hack para contornar bug da jqGrid
                $.TSearchPlugin.endReq($grid);
            };
            //hack para contornar bug da jqGrid
            $.TSearchPlugin.beginReq($grid);
            var context = null;
            callbackdata.operation = 'gridSearch';
            options.callbackSuccess = success;
            options.callbackFailure = failure;
            var postdata = $.TSearchPlugin.toJSON(callbackdata);
            var callbackMethod = $.TSearchPlugin.evalFunction(options.callbackMethod);
            callbackMethod(postdata, success, context, failure);
        };

        //método de busca de dados para os campos


        //adiciona plugin de jqGrid
        //cria grid
        $grid.jqGrid({
            datatype: function(callbackdata) { },
            colNames: options.colNames,
            colModel: options.colModel,
            rowNum: options.rowNum,
            rowList: [10, 20, 50],
            imgpath: options.imgpath,
            pager: $pager,
            sortname: options.sortname,
            sortorder: options.sortorder,
            width: options.width,
            height: options.height,
            sidx: options.sidx,
            sord: options.sord,
            viewrecords: true,
            rownumbers: true,
            gridview: true,
            toolbar: [true, "top"],
            onSelectRow: function(rowid) {
                if (typeof rowid != 'undefined' && rowid != null) {
                    var rowdata = $grid.getRowData(rowid);
                    $popup[0].rowValues = rowdata;
                    if ($textfield != null && typeof options.textColumn != 'undefined') {
                        if (typeof rowdata[options.textColumn.toLowerCase()] != 'undefined')
                            $textfield.val(rowdata[options.textColumn.toLowerCase()]);
                    }
                    if ($descriptionfield != null && typeof options.descriptionColumn != 'undefined') {
                        if (typeof rowdata[options.descriptionColumn.toLowerCase()] != 'undefined')
                            $descriptionfield.val(rowdata[options.descriptionColumn.toLowerCase()]);
                    }
                    $popup[0].hidePopup();
                    doPostBack();
                }
            }
        });
        var $toolbar = $('#t_' + $grid[0].id);
        $toolbar.height(25).show().filterGrid('#' + $grid[0].id, { gridModel: true, gridToolbar: true });

        //guarda valor original do campo
        options.originalValue = $textfield.val();

        //evento de click do body
        //esconde o popup quando se clica fora dele
        $('body').click(function(event) {
            if (typeof $popup[0].openingPopup != 'undefined')
                $popup[0].openingPopup = undefined;
            else {
                if ($textfield == null || typeof $textfield[0] == 'undefined' || $textfield[0].id != event.target.id) {
                    $popup[0].hidePopup();
                }
            }
        });

        //não esconde o popup quando se clica dentro dele
        //impede a propagação do evento de click do popup para o body
        $popup.click(function(event) {
            event.stopPropagation();
        });

        //cria popup modal
        if (options.modal == true) {
            $popup.jqm();
        }

        //método para mostrar o popup
        $ts[0].showPopup = function() {
            $grid.setGridParam({ datatype: getGridData, page: 1 }).trigger("reloadGrid");
            if (options.modal == true) {
                $popup.css({ visibility: 'visible' });
                $.TSearchPlugin.center($popup);
                $popup.jqmShow();
            }
            else {
                $popup.css({ visibility: 'visible' });
                $.TSearchPlugin.setPosition($popup, $textfield);
                $popup.show();
            }
            $popup[0].openingPopup = true;
        };

        //método para esconder o popup
        $popup[0].hidePopup = function() {
            if (typeof $popup[0].onHide != 'undefined' && $popup[0].onHide != null)
                $popup[0].onHide();
            if (options.modal == true) {
                $popup.jqmHide();
            }
            else
                $popup.hide();
        }

        //função de postback
        var doPostBack = function() {
            if (typeof options.postback != 'undefined') {
                var textVal = $textfield == null ? null : $textfield.val();
                if (typeof options.originalValue != 'undefined' && textVal != options.originalValue) {
                    var pb = $.TSearchPlugin.evalFunction(options.postback);
                    pb();
                    options.postback = null; //remove função de postback para que ela seja chamada somente uma vez
                }
            }
        }

        var setValues = function(textValue, descriptionValue, rowData) {
            if ($textfield != null) {
                $textfield.val(typeof textValue == 'undefined' ? '' : textValue);
            }
            if ($descriptionfield != null) {
                $descriptionfield.val(typeof descriptionValue == 'undefined' ? '' : descriptionValue);
            }
            if ($rowdata != null) {
                $rowdata.val(typeof rowData == 'undefined' ? '' : rowData);
            }
        }
        var saveLastValidValues = function() {
            if ($textfield != null) {
                options.lastValidText = $textfield.val();
            }
            if ($descriptionfield != null) {
                options.lastValidDescr = $descriptionfield.val();
            }
            if ($rowdata != null) {
                options.lastValidRowData = $rowdata == null ? '' : $rowdata.val();
            }
        }
        var restoreLastValidValues = function() {
            setValues(options.lastValidText, options.lastValidDescr, options.lastValidRowData);
        }
        var setFocusOnTextField = function() {
            if ($textfield == null || $textfield.length == 0 || $textfield[0].disabled == true)
                return;

            $textfield[0].focus();

            var pos = $textfield.val().length;
            if ($textfield[0].setSelectionRange) {
                $textfield[0].setSelectionRange(pos, pos);
            }
            else if ($textfield[0].createTextRange) {
                var range = $textfield[0].createTextRange();
                range.collapse(true);
                range.moveEnd('character', pos);
                range.moveStart('character', pos);
                range.select();
            }

        }

        if ($textfield != null) {
            //cancela tecla ENTER para o campo chave
            $textfield.keypress(function(event) {
                if (event.keyCode == 13) {
                    event.preventDefault();
                }
            });

            //valida campo chave
            $textfield.change(function() {
                var keyValue = $textfield.val();
                while (keyValue != null && keyValue.length > 0 && keyValue[0] == ' ')
                    keyValue = keyValue.substr(1);
                if (keyValue == null || keyValue.length == 0) {
                    setValues('', '', '');
                    saveLastValidValues();
                    doPostBack();
                }
                else {
                    //limpa campo de descrição antes do callback
                    if ($descriptionfield != null) {
                        $descriptionfield.val('');
                    }
                    var success = function(strdata) {
                        var jsondata = $.TSearchPlugin.parseJSON(strdata);
                        var erro = '';
                        if (jsondata == null || typeof jsondata == 'undefined') {
                            erro = 'Código inválido';
                        }
                        else if (typeof jsondata.message != 'undefined') {
                            erro = jsondata.message;
                        }
                        if (erro != '' && erro != null) {
                            var textValidation = (typeof options.textValidation != 'undefined' && options.textValidation == true);
                            if (textValidation) {
                                restoreLastValidValues();
                                alert(erro);
                                setFocusOnTextField();
                            }
                            else {
                                doPostBack();
                            }

                        }
                        else {
                            setValues(jsondata.keyvalue, jsondata.descriptionvalue, jsondata.rowdata);
                            saveLastValidValues();
                            doPostBack();
                        }
                        $ts[0].doingCallback = 0;
                    };
                    var failure = function(strdata) {
                        $ts[0].doingCallback = 0;
                        restoreLastValidValues();
                        alert('Erro na requisição. Tente novamente.');
                        setFocusOnTextField();
                    };
                    var context = null;
                    var callbackdata = new Object();
                    callbackdata.operation = "description";
                    callbackdata.key = $textfield.val();
                    var postdata = $.TSearchPlugin.toJSON(callbackdata);
                    options.callbackSuccess = success;
                    options.callbackFailure = failure;
                    var callbackMethod = $.TSearchPlugin.evalFunction(options.callbackMethod);
                    callbackMethod(postdata, success, context, failure);
                }
            });
        }
        saveLastValidValues();
    };

})(jQuery);



function tsearchboxFind(cfg) {
    if (typeof cfg == 'undefined' || cfg == null)
        return null;
    var $cfg = $('#' + cfg);
    if ($cfg == null || $cfg.length == 0)
        return null;
    var options = $.TSearchPlugin.parseJSON($cfg.val());
    var $popup = $('#' + options.popup);
    if ($popup.length == 0)
        return null;
    var initialized = $popup.attr('initialized');
    if (typeof initialized != 'undefined')
        return $popup;
    $popup.tsearchbox(options);
    $popup.attr('initialized', 'true');
    return $popup;
}

function tsearchboxTextFocus(cfg) {
    var $popup = tsearchboxFind(cfg);
    if ($popup == null)
        return false;
    return false;
}

function tsearchboxTextBlur(cfg) {
    var $popup = tsearchboxFind(cfg);
    if ($popup == null)
        return false;
    return false;
}

function tsearchboxButtonClick(cfg) {
    var $popup = tsearchboxFind(cfg);
    if ($popup == null)
        return false;
    $popup[0].showPopup();
    return false;
}

function tsearchboxCallbackSuccess(s, e, id) {
    var $ctl = $('#' + id);
    if ($ctl.length > 0 && typeof $ctl[0].options != 'undefined' && typeof $ctl[0].options.callbackSuccess != 'undefined')
        $ctl[0].options.callbackSuccess(e.result);
}

function tsearchboxCallbackFailure(s, e, id) {
    var $ctl = $('#' + id);
    if ($ctl.length > 0 && typeof $ctl[0].options != 'undefined' && typeof $ctl[0].options.callbackFailure != 'undefined')
        $ctl[0].options.callbackFailure(e.result);
}
