function disableSelection() {
    if (typeof document.activeElement == "object") {
        if (document.activeElement != undefined) {
            if (document.activeElement.tagName == 'INPUT') {
                if (document.activeElement.type.toUpperCase() == 'TEXT') {
                    return true;
                } else {
                    if (document.activeElement.type.toUpperCase() == 'PASSWORD') {
                        return true;
                    }
                }
            } else {
                if (document.activeElement.tagName == 'TEXTAREA') {
                    return true;
                }
            }
        }
    }

    event.cancelBubble = true;
    event.returnValue = false;
    return false;
};

String.prototype.format = function () {
    var formatted = this;
    for (var i = 0; i < arguments.length; i++) {
        var regexp = new RegExp('\\{' + i + '\\}', 'gi');
        formatted = formatted.replace(regexp, arguments[i]);
    }
    return formatted;
};

function deleteItem(element, url, id, seq) {

    if (confirm("Confirma exclusão do registro?")) {

        if (seq != null) {

            $.ajax({ url: url,
                type: 'post',
                data: { id: id, seq: seq },
                success: function (data) {
                    alert(data.Result);
                    $(element).closest("form").submit();
                },
                error: function (xhr, status, error) {
                    console.log(xhr.status);
                    if (xhr.status != 403) {
                        $(".boxInfo").remove();
                        $(".validation-summary-errors").remove();
                        $(".boxContent").prepend("<div class='validation-summary-errors'><ul><li>" + xhr.responseText + "</li></ul></div>");
                    }
                }
            });

        } else {

        $.ajax({ url: url,
            type: 'post',
            data: "id=" + id,
            success: function (data) {
                alert(data.Result);
                $(element).closest("form").submit();
            },
            error: function (xhr, status, error) {
                if (xhr.status != 403) {
                    $(".boxInfo").remove();
                    $(".validation-summary-errors").remove();
                    $(".boxContent").prepend("<div class='validation-summary-errors'><ul><li>" + xhr.responseText + "</li></ul></div>");
                }
            }
        });
        }
    }
};

function deleteItemAndExecuteFunction(element, url, functionName, boxFeedback, id, seq) {

    if (confirm("Confirma exclusão do registro?")) {

        if (seq != null) {
            $.ajax({ url: url,
                type: 'post',
                data: { id: id, seq: seq },
                success: function (data) {
                    alert(data.Result);
                    eval(functionName);
                },
                error: function (xhr, status, error) {
                    if (xhr.status != 403) {
                        $(".boxInfo").remove();
                        $(".validation-summary-errors").remove();
                        $("#" + boxFeedback).prepend("<div class='validation-summary-errors'><ul><li>" + xhr.responseText + "</li></ul></div>");
                    }
                }
            });

        } else {
        $.ajax({ url: url,
            type: 'post',
            data: "id=" + id,
            success: function (data) {
                alert(data.Result);
                eval(functionName);
            },
            error: function (xhr, status, error) {
                if (xhr.status != 403) {
                    $(".boxInfo").remove();
                    $(".validation-summary-errors").remove();
                    $("#" + boxFeedback).prepend("<div class='validation-summary-errors'><ul><li>" + xhr.responseText + "</li></ul></div>");
                }
            }
        });
        }
    }
};

function deleteItemAndRedirect(element, url, urlRedirect, id, seq) {

    if (confirm("Confirma exclusão do registro?")) {

        if (seq != null) {

            $.ajax({ url: url,
                type: 'post',
                data: { id: id, seq: seq },
                success: function (data) {
                    alert(data.Result);
                    window.location.href = urlRedirect;
                },
                error: function (xhr, status, error) {
                    if (xhr.status != 403) {
                        $(".boxInfo").remove();
                        $(".validation-summary-errors").remove();
                        $(".boxContent").prepend("<div class='validation-summary-errors'><ul><li>" + xhr.responseText + "</li></ul></div>");
                    }
                }
            });
        } else {

        $.ajax({ url: url,
            type: 'post',
            data: "id=" + id,
            success: function (data) {
                alert(data.Result);
                window.location.href = urlRedirect;
            },
            error: function (xhr, status, error) {
                if (xhr.status != 403) {
                    $(".boxInfo").remove();
                    $(".validation-summary-errors").remove();
                    $(".boxContent").prepend("<div class='validation-summary-errors'><ul><li>" + xhr.responseText + "</li></ul></div>");
                }
            }
        });
        }
    }
};

function htmlEncode(value) {
    return $('<div/>').text(value).html();
}

function htmlDecode(value) {
    //mantem codificação para aspas
    return $('<div/>').html(value).text().replace(/'/g, "\\'").replace(/"/g, "\\'");
}