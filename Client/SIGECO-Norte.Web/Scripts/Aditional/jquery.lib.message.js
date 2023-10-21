(function ($) {
    function createWindow(title, content, buttons, valor, valor_2) {
        var win = $("<div class=\"messager-body\"></div>").appendTo("body");
        win.append(content);
        if (buttons) {
            var tb = $("<div class=\"messager-button\"></div>").appendTo(win);
            for (var btn in buttons) {
                //debugger;
                $("<a></a>").attr("href", "javascript:void(0)").text(btn).css("margin-left", 10).bind("click", eval(buttons[btn])).appendTo(tb).linkbutton();
            }
        }
        win.window({
            title: title,
            noheader: (title ? false : true),
            width: 400,
            height: "auto",
            modal: true,
            collapsible: false,
            minimizable: false,
            maximizable: false,
            resizable: false,
            onClose: function () {
                setTimeout(function () {
                    win.window("destroy");
                }, 100);
            }
        });
        win.window("window").addClass("messager-window");
        win.children("div.messager-button").children("a:first").focus();
        if ($('#txt_codigo_autorizacion_dialago').length > 0) {

            $('#txt_codigo_autorizacion_dialago').passwordbox({
                prompt: 'Password',
                showEye: true,
                required: true,
                novalidate: true
            });
        }


        if ($('#txt_observacion_dialago').length > 0) {
            $('#txt_observacion_dialago').textbox({
                multiline: true,
                showEye: true,
                required: true,
                novalidate: true
            });
        }
        if ($('#nbb_anio_contrato_vinculante_dialago').length > 0) {
            $('#nbb_anio_contrato_vinculante_dialago').numberbox({
                max: 2060,
                min: 1700,
                disabled: !valor_2,
                required: !valor_2,
                novalidate: true
            });
            $("#ckb_contrato_vinculante_dialog").switchbutton({
                onText: 'Si', offText: 'No',
                checked: valor_2,
                onChange: function (checked) {
                    if (checked) {
                        $("#nbb_anio_contrato_vinculante_dialago").numberbox({ required: true, disabled: false });
                    }
                    else {
                        $("#nbb_anio_contrato_vinculante_dialago").numberbox({ required: false, disabled: true });
                        $("#nbb_anio_contrato_vinculante_dialago").numberbox('clear');

                    }
                }
            });

            $('#nbb_anio_contrato_vinculante_dialago').numberbox("setValue", valor);
        }



        return win;
    };



    $.extend($.messager, {
        password: function (title, msg, fn) {

            var content = "<div class=\"messager-icon messager-question\"></div>" + "<div>" + msg + "</div>" + "<br/>" + "<div style=\"clear:both;\"/>" + "<div><input style='width:100%;' id=\"txt_codigo_autorizacion_dialago\" class=\"messager-input\" type=\"password\"/></div>";
            var buttons = {};
            buttons[$.messager.defaults.ok] = function () {
                var resultado = $('#txt_codigo_autorizacion_dialago').passwordbox('enableValidation').passwordbox("validate").passwordbox("isValid");

                if (resultado) {
                    win.window("close");
                    if (fn) {
                        fn(win, $(".messager-input", win).val());
                        return false;
                    }
                }

            };
            buttons[$.messager.defaults.cancel] = function () {
                win.window("close");
                if (fn) {
                    fn();
                    return false;
                }
            };
            var win = createWindow(title, content, buttons);
            win.find("input.messager-input").focus();
            return win;
        },
        anho_excepcion: function (title, msg, anho, tiene_anho_vinculante, fn) {

            var tabla = "<div class=\"rowGridModalDouble\" style=\"width:50%;\">" +
                         "   <input  id=\"ckb_contrato_vinculante_dialog\" />" +
                        "</div>" +
                        "<div style=\"clear:both;\"/>" +

                "<div class=\"rowGridModalDouble\" style=\"width:50%;\">" +
                        "   <input id=\"nbb_anio_contrato_vinculante_dialago\" class=\"easyui-numberbox textbox\" style=\"width:100%;\" >" +
                       "</div>";


            //var tabla =
            //    " <table style=\"width:20px;\"> <tr> " +
            //      "<td><label class=\"colGridModal\">Año Excepción</label></td>" +
            //      " <td><input type=\"radio\"  id=\"ckb_contrato_vinculante_dialago\">mmmm</td>" +
            //      " <td><input type=\"text\" class=\"messager-input\" id=\"nbb_anio_contrato_vinculante_dialago\" ></td> </tr> </table>";
            var content = "<div>" + msg + "</div>" + "<br/>" + "<div style=\"clear:both;\"/>" + "<div>" + tabla + "</div>";


            var buttons = {};
            buttons[$.messager.defaults.ok] = function () {
                var resultado = $('#nbb_anio_contrato_vinculante_dialago').numberbox('enableValidation').numberbox("validate").numberbox("isValid");
                var v_anho_excepcion = $('#nbb_anio_contrato_vinculante_dialago').numberbox("getValue");
                var chk_vinculante = $("#ckb_contrato_vinculante_dialog").switchbutton('options').checked;
                if (resultado) {
                    $.messager.confirm('Confirmaci&oacute;n', '&iquest;Est&aacute; seguro que desea grabar?', function (result) {

                        if (result) {
                            win.window("close");
                            if (fn) {
                                fn(win, v_anho_excepcion, chk_vinculante);
                                return false;
                            }
                        }


                    });
                }

            };
            buttons[$.messager.defaults.cancel] = function () {
                win.window("close");
                if (fn) {
                    fn();
                    return false;
                }
            };
            var win = createWindow(title, content, buttons, anho, tiene_anho_vinculante);
            win.find("input.messager-input").focus();
            return win;
        },


        observacion: function (title, msg, fn) {

            var content = "<div class=\"messager-icon messager-question\"></div>" + "<div>" + msg + "</div>" + "<br/>" + "<div style=\"clear:both;\"/>" + "<div><input style='width:100%; height:80px;' id=\"txt_observacion_dialago\" class=\"messager-input\" type=\"text\"/></div>";
            var buttons = {};
            buttons[$.messager.defaults.ok] = function () {
                var resultado = $('#txt_observacion_dialago').passwordbox('enableValidation').passwordbox("validate").passwordbox("isValid");

                if (resultado) {
                    win.window("close");
                    if (fn) {
                        fn(win, $(".messager-input", win).val());
                        return false;
                    }
                }

            };
            buttons[$.messager.defaults.cancel] = function () {
                win.window("close");
                if (fn) {
                    fn();
                    return false;
                }
            };
            var win = createWindow(title, content, buttons);
            win.find("input.messager-input").focus();
            return win;
        },


        leyenda_x: function (title, msg, fn) {

            var tabla =
                " <table style=\"width:100px;\"> <tr>  <td></td> <td><input type=\"radio\" name=\"rb_x\" id=\"ckb_x_superior\" ></td>" +
	" <td></td> </tr> <tr>   <td><input type=\"radio\" name=\"rb_x\" id=\"ckb_x_izquierda\" ></td>	<td></td>" +
	" <td><input type=\"radio\" name=\"rb_x\" id=\"ckb_x_derecha\" ></td>  </tr>  <tr><td></td>" +
	" <td><input type=\"radio\"  name=\"rb_x\" id=\"ckb_x_inferior\" ></td>	<td></td>  </tr> </table>";

            var content = "<div class=\"messager-icon messager-question\"></div>" + "<div>" + msg + "</div>" + "<br/>" + "<div style=\"clear:both;\"/>" + "<div>" + tabla + "</div>";
            var buttons = {};
            buttons[$.messager.defaults.cancel] = function () {
                win.window("close");
                if (fn) {
                    fn();
                    return false;
                }
            };
            var win = createWindow(title, content, buttons);
            $("#ckb_x_superior,#ckb_x_inferior,#ckb_x_izquierda,#ckb_x_derecha").change(function () {
                fnSeleccionarEjeX(this);
            });

            return win;

        },
        leyenda_y: function (title, msg, fn) {
            var tabla =
              " <table style=\"width:100px;\"> <tr>  <td></td> <td>" +
              " <input type=\"radio\" name=\"rb_y\" id=\"ckb_y_superior\" >" +
  " </td> <td></td> </tr> <tr>   <td>" +
  " <input type=\"radio\" name=\"rb_y\" id=\"ckb_y_izquierda\" >" +
  " </td>	<td></td> <td>" +
  " <input type=\"radio\" name=\"rb_y\" id=\"ckb_y_derecha\" ></td>  </tr>  <tr><td></td>" +
  " <td><input type=\"radio\"  name=\"rb_y\" id=\"ckb_y_inferior\" ></td>	<td></td>  </tr> </table>";

            var content = "<div class=\"messager-icon messager-question\"></div>" + "<div>" + msg + "</div>" + "<br/>" + "<div style=\"clear:both;\"/>" + "<div>" + tabla + "</div>";
            var buttons = {};
            buttons[$.messager.defaults.cancel] = function () {
                win.window("close");
                if (fn) {
                    fn();
                    return false;
                }
            };
            var win = createWindow(title, content, buttons);

            $("#ckb_y_superior,#ckb_y_inferior,#ckb_y_izquierda,#ckb_y_derecha").change(function () {
                fnSeleccionarEjeY(this);
            });

            return win;
        },

        login: function (title, msg, fn) {
            var content = "<div class=\"messager-icon messager-question\"></div>" + "<div>" + msg + "</div>" + "<br/>" + "<div style=\"clear:both;\"/>"
            + "<div class=\"righttext\">Login ID: <input class=\"messager-id \" type=\"text\"/></div>"
            + "<div class=\"righttext\">Password: <input class=\"messager-pass\" type=\"password\"/></div>";
            var buttons = {};
            buttons[$.messager.defaults.ok] = function () {
                win.window("close");
                if (fn) {
                    fn($(".messager-id", win).val(), $(".messager-pass", win).val());
                    return false;
                }
            };
            buttons[$.messager.defaults.cancel] = function () {
                win.window("close");
                if (fn) {
                    fn();
                    return false;
                }
            };
            var win = createWindow(title, content, buttons);
            win.find("input.messager-id").focus();
            return win;
        }
    });
})(jQuery);
