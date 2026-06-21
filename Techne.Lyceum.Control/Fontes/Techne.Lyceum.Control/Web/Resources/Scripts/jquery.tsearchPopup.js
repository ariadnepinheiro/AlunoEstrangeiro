; (function($) {
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
        var $popup = $(popup);
        var $anchor = null;
        var $searchButton = null;
        var $grid = null;
        var $msg = null;
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

        //expõe método de callback
        $popup[0].callback = options.callbackMethod;

        var disableFilter = function(disable) {
            $searchButton[0].disabled = disable;
            for (i = 0; i < options.filterFields.length; i++) {
                var $field = $('#' + options.filterFields[i].id);
                if ($field.length > 0) {
                    $field[0].disabled = disable;
                }
            }

        };

        var getData = function(callbackdata) {
            var success = function(xmldata) {
                var jsondata = $.TSearchPopupControl.parseJSON(xmldata);
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
                disableFilter(false);
            };
            var context = null;
            var filter = new Array();
            var i;
            if (options.filterFields != null) {
                for (i = 0; i < options.filterFields.length; i++) {
                    var $field = $('#' + options.filterFields[i].id);
                    if ($field.length > 0) {
                        filter[filter.length] = ({ name: options.filterFields[i].name, value: $field.val() });
                    }
                }
            }
            if (typeof $grid.clearGridData == 'function')
                $grid.clearGridData();
            $msg[0].innerHTML = 'Aguarde...';
            disableFilter(true);
            callbackdata.filter = filter;
            callbackdata.operation = 'gridSearch';
            var postdata = $.TSearchPopupControl.toJSON(callbackdata);
            var callbackMethod = options.callbackMethod;
            callbackMethod(postdata, success, context, failure);
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
        $('body').click(function(event) {
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

        //método para popular os campos de filtro
        $popup[0].setFilter = function(filterValues) {
            if (options.filterFields != null) {
                var i, j;
                for (i = 0; i < options.filterFields.length; i++) {
                    var $field = $('#' + options.filterFields[i].id);
                    if ($field.length > 0) {
                        $field.val('');
                        for (j = 0; j < filterValues.length; j++) {
                            if (filterValues[j].name == options.filterFields[i].name) {
                                if(typeof filterValues[j].value != 'undefined')
                                    $field.val(filterValues[j].value);
                                else if(typeof filterValues[j].controlid !='undefined')
                                    $field.val($('#'+filterValues[j].controlid).val());
                            }
                        }
                    }
                }
            }
        }
        //método para mostrar o popup
        $popup[0].showPopup = function(filterValues, autoexecute) {
            var auto = (autoexecute ==true?true:false);
            if (filterValues != null && typeof filterValues != 'undefined') {
                $popup[0].setFilter(filterValues);
                auto = true;
            }
            if (options.modal == true) {
                $popup.css({ visibility: 'visible' });
                $.TSearchPopupControl.center($popup);
                $popup.jqmShow();
            }
            else {
                var reference = $anchor[0];
                var offset = $(reference).offset();
                $popup.css({
                    top: offset.top + reference.offsetHeight,
                    left: offset.left,
                    visibility: 'visible'
                }).show();
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


    };

    $.TSearchPopupControl.center = function(obj, absolute) {
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

    $.TSearchPopupControl.toJSON = function(v) {
        var f = isNaN(v) ? s[typeof v] : s['number'];
        if (f) return f(v);
    };

    $.TSearchPopupControl.parseJSON = function(v, safe) {
        if (safe === undefined) safe = $.TSearchPopupControl.parseJSON.safe;
        if (safe && !/^("(\\.|[^"\\\n\r])*?"|[,:{}\[\]0-9.\-+Eaeflnr-u \n\r\t])+?$/.test(v))
            return undefined;
        return eval('(' + v + ')');
    };

    $.TSearchPopupControl.parseJSON.safe = false;


})(jQuery);
