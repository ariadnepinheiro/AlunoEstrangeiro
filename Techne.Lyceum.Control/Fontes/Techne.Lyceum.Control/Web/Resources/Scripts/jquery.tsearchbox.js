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
                var jsondata = $.TSearchBoxPlugin.parseJSON(xmldata);
                $grid[0].addJSONData(jsondata);
            }
            var failure = function(xmldata) {
            };
            var context = null;
            callbackdata.operation = 'gridSearch';
            options.callbackSuccess = success;
            options.callbackFailure = failure;
            var postdata = $.TSearchBoxPlugin.toJSON(callbackdata);
            var callbackMethod = $.TSearchBoxPlugin.evalFunction(options.callbackMethod);
            callbackMethod(postdata, success, context, failure);
        };

        //método de busca de dados para os campos


        //adiciona plugin de jqGrid
        //cria grid
        $grid.jqGrid({
            datatype: getGridData,
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
            if (options.modal == true) {
                $popup.css({ visibility: 'visible' });
                $.TSearchBoxPlugin.center($popup);
                $popup.jqmShow();
            }
            else {
                var reference = $textfield[0];
                var offset = $(reference).offset();
                $popup.css({
                    top: offset.top + reference.offsetHeight,
                    left: offset.left,
                    visibility: 'visible'
                }).show();
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
                    var pb = $.TSearchBoxPlugin.evalFunction(options.postback);
                    pb();
                    options.postback = null; //remove função de postback para que ela seja chamada somente uma vez
                }
            }
        }

        var setValues = function(textValue, descriptionValue, rowData) {
            if ($textfield != null) {
                $textfield.val(textValue);
            }
            if ($descriptionfield != null) {
                $descriptionfield.val(descriptionValue);
            }
            if ($rowdata != null) {
                $rowdata.val(rowData);
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
                    doPostBack();
                }
                else {
                    //limpa campo de descrição antes do callback
                    if ($descriptionfield != null) {
                        $descriptionfield.val('');
                    }
                    var success = function(strdata) {
                        var jsondata = $.TSearchBoxPlugin.parseJSON(strdata);
                        var erro = '';
                        if (jsondata == null || typeof jsondata == 'undefined') {
                            erro = 'Código inválido';
                        }
                        else if (typeof jsondata.message != 'undefined') {
                            erro = jsondata.message;
                        }
                        if (erro != '' && erro != null) {
                            setValues('', '', '');
                            alert(erro);
                            if ($textfield[0].disabled != true)
                                $textfield[0].focus();
                        }
                        else {
                            setValues(jsondata.keyvalue, jsondata.descriptionvalue, jsondata.rowdata);
                            doPostBack();
                        }
                        $ts[0].doingCallback = 0;
                    };
                    var failure = function(strdata) {
                        $ts[0].doingCallback = 0;
                        setValues('', '', '');
                        alert('Erro na requisição. Tente novamente.');
                        if ($textfield[0].disabled != true)
                            $textfield[0].focus();
                    };
                    var context = null;
                    var callbackdata = new Object();
                    callbackdata.operation = "description";
                    callbackdata.key = $textfield.val();
                    var postdata = $.TSearchBoxPlugin.toJSON(callbackdata);
                    options.callbackSuccess = success;
                    options.callbackFailure = failure;
                    var callbackMethod = $.TSearchBoxPlugin.evalFunction(options.callbackMethod);
                    callbackMethod(postdata, success, context, failure);
                }
            });
        }

    };

    $.TSearchBoxPlugin.evalFunction = function(txt) {
        if (typeof txt == 'function')
            return txt;
        var zz;
        var txtCorr = "zz=" + txt;
        eval(txtCorr);
        return zz;
    };

    $.TSearchBoxPlugin.center = function(obj, absolute) {
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

    var m = {
        '\b': '\\b',
        '\t': '\\t',
        '\n': '\\n',
        '\f': '\\f',
        '\r': '\\r',
        '"': '\\"',
        '\\': '\\\\'
    },
        s = {
            'array': function(x) {
                var a = ['['], b, f, i, l = x.length, v;
                for (i = 0; i < l; i += 1) {
                    v = x[i];
                    f = s[typeof v];
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
                        return s.array(x);
                    }
                    var a = ['{'], b, f, i, v;
                    for (i in x) {
                        v = x[i];
                        f = s[typeof v];
                        if (f) {
                            v = f(v);
                            if (typeof v == 'string') {
                                if (b) {
                                    a[a.length] = ',';
                                }
                                a.push(s.string(i), ':', v);
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
                        var c = m[b];
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

    $.TSearchBoxPlugin.toJSON = function(v) {
        var f = isNaN(v) ? s[typeof v] : s['number'];
        if (f) return f(v);
    };

    $.TSearchBoxPlugin.parseJSON = function(v, safe) {
        if (safe === undefined) safe = $.TSearchBoxPlugin.parseJSON.safe;
        if (safe && !/^("(\\.|[^"\\\n\r])*?"|[,:{}\[\]0-9.\-+Eaeflnr-u \n\r\t])+?$/.test(v))
            return undefined;
        if (v === undefined || v == null || v == '')
            return new Object();
        else
            return eval('(' + v + ')');
    };

    $.TSearchBoxPlugin.parseJSON.safe = false;




})(jQuery);



function tsearchboxFind(cfg) {
    if (typeof cfg == 'undefined' || cfg == null)
        return null;
    var $cfg = $('#' + cfg);
    if ($cfg == null || $cfg.length == 0)
        return null;
    var options = $.TSearchBoxPlugin.parseJSON($cfg.val());
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
