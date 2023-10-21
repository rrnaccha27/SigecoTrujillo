$.fn.hasAttr = function (name) {
    return this.attr(name) !== undefined;
};

$.extend($.fn.validatebox.defaults.rules, {
    date: {
        validator: function (value, param) {
            var format = param[0];
            if (Date.parseExact(value, format)) return true;
            return false;
        },
        message: $.fn.validatebox.defaults.rules.date.message
    }
});

$.extend($.fn.validatebox.defaults.rules, {
    group: {
        validator: function (value, param) {
            var nomgrupo = param[0];

            var valido = false;

            $("input[name='" + nomgrupo + "']").each(function (i) {
                if ($(this).is(":checked")) {
                    valido = true;
                    return false;
                }
            });

            return valido;
        },
        message: $.fn.validatebox.defaults.rules.group.message
    }
});

$.extend($.fn.validatebox.defaults.rules, {
    minLength: {
        validator: function (value, param) {
            return value.length >= param[0];
        },
        message: $.fn.validatebox.defaults.rules.minLength.message
    }
});

$.extend($.fn.validatebox.defaults.rules, {
    equalTo: {
        validator: function (value, param) {
            var to = $(param[0])
            return (value == $(to).val());
        },
        message: $.fn.validatebox.defaults.rules.equalTo.message
    }
});

jQuery.centerDialog = function (selector) {
    var panelDialog = $(".panel.window:has(" + selector + ")");
    var top = ($(window).height() - parseInt(panelDialog.outerHeight())) / 2;
    top = (top > 0) ? top : 50;
    var left = ($(window).width() - parseInt(panelDialog.outerWidth())) / 2;
    left = (left > 0) ? left : 50;
    $(selector).dialog("move", {
        top: top + $(window).scrollTop(),
        left: left
    });
};

$.fn.validarformulario = function () {
    var form = this;
    var res = true;
    var validables = ["combo", "combobox", "datebox", "validatebox", "numberbox", "datetimebox", "combogrid"];

    $.each(validables, function (j, type) {
        var inputs = $(form).find(".easyui-" + type);
        $.each(inputs, function (i, input) {
            var _dis = $(input).attr("disabled");
            if (!(_dis == true || _dis == "true" || _dis == "disabled")) {
                eval("if(res) res = $(input)." + type + "('isValid')");
                eval("$(input)." + type + "('validate')");
            }
        });
    });

    $(form).find(".validatebox-text:disabled").removeClass("validatebox-invalid");

    return res;
}