(function ($) {


    $.urlParam = function (name) {
        alert();
        var results = new RegExp('[\\?&]' + name + '=([^&#]*)').exec(window.location.href);
        if (results == null) {
            return null;
        } else {
            return results[1] || 0;
        }
    }

    $.extend($.fn.combobox.defaults.rules, {
        restricted:
        {
            validator: function (value, param) {
                var cbo = param[0];
                var texto = $(cbo).combobox("getValue");
                var rows = $(cbo).combobox("getData");
                var opts = $(cbo).combobox("options");

                var valid = false;

                if (!opts.multiple) {
                    $.each(rows, function (i, row) {
                        if (row[opts.valueField] == texto) {
                            valid = true;
                            return false;
                        }
                    });
                } else {
                    var values = $(cbo).combobox("getValues");

                    $.each(values, function (i, value) {
                        if (value.trim() != "") {
                            $.each(rows, function (i, row) {
                                if (row[opts.valueField] == value) {
                                    valid = true;
                                    return false;
                                }
                            });
                        }
                        if (valid) { return false; }
                    });
                }

                return valid;
            },
            message: "Debe seleccionar una opci&oacute;n de la lista."
        }
    });

    $.fn.combobox.defaults.filter = function (q, row) {
        var opts = $(this).combobox("options");
        if (!opts.multiple) {
            return row[opts.textField].toLowerCase().indexOf(q.toLowerCase()) == 0;
        }
    }

    $.extend($.fn.datebox.defaults.rules, {
        minLength:
        {
            validator:function(value,param){
                return value.length >=param[0]
            },
            message:'Por favor, ingrese un n&uacute;mero mayor a {0}'  
        }
    });


    $.extend($.fn.datebox.defaults.rules, {
        maxLength:
        {
            validator: function (value, param) {                
                return value.length <= param[0]
            },
            message: 'El campo  permite ingreso de {0}  caracteres.'
        }
    });

  



    $.extend($.fn.datebox.defaults.rules, {
    fechaMenorAHoyYMenorA:
    {
        validator: function (value, param) {
            var format = param[2];
            
            
            var fechaMin = Date.parse("1900-01-01");
            var fechaMax = Date.parse("2100-01-01");

            var dFecha1 = null;
            var valido = true;
            try {
                if (!Date.parseExact(value, format))
                {
                    param[3] = 'Por favor, ingrese una fecha v&aacute;lida. Formato v&aacute;lido de Fecha: dd/mm/aaaa';
                    return false;
                }
                else {
                    dFecha1 = Date.parseExact(value, format);
                }
            } catch (e) {
                param[3] = 'Por favor, ingrese una fecha v&aacute;lida. Formato v&aacute;lido de Fecha: dd/mm/aaaa';
                valido = false;
                return false;
            }

            var dFecha2;

            if ($(param[0]).datebox("getValue") == null || $(param[0]).datebox("getValue") == "") {
                dFecha2 = Date.today();
            } else {
                dFecha2 = Date.parseExact($(param[0]).datebox("getValue"), format);
            }



            //return (dFecha1 < dFecha2 && dFecha1 < Date.today());

            param[3] = 'Por favor, ingrese una fecha v&aacute;lida. Formato v&aacute;lido de Fecha: dd/mm/aaaa';

            if (dFecha1 > dFecha2) {
                param[3] = "La fecha no es menor que " + param[1];
                return false;
            }

            if (dFecha1 > Date.today()) {
                param[3] = "La fecha no es menor que hoy";
                return false;
            }

            if (dFecha1 < fechaMin) {
                param[3] = "La fecha no es mayor que " + fechaMin.toString("dd/MM/yyyy");
                return false;
            }

            if (dFecha1 > fechaMax) {
                param[3] = "La fecha no es menor que " + fechaMax.toString("dd/MM/yyyy");
                return false;
            }

            return true;
        },
        message: "{3}"
        // $.fn.validatebox.defaults.rules.pattern.message
    }
});

    $.extend($.fn.datebox.defaults.rules, {
        MenorIgualAHoy:
        {
            validator: function (value, param) {
                param[3] = 'Por favor, ingrese una fecha v&aacute;lida. Formato v&aacute;lido de Fecha: dd/mm/aaaa';

                var format = param[0];
                //var fecha1 = Date.parseExact(value, format);
                var dFecha2 = Date.today();
                var dFecha1; // = new Date(fecha1);
                var fechaMin = Date.parse("1900-01-01");
                var valido = true;
                try {
                    if (!Date.parseExact(value, format)) return false;
                    else {
                        dFecha1 = Date.parseExact(value, format);
                    }
                } catch (e) {
                    return false;
                }

                if (dFecha1 < fechaMin) {
                    param[3] = "La fecha no es mayor que " + fechaMin.toString("dd/MM/yyyy");
                    return false;
                }

                if (dFecha1 > dFecha2) {
                    param[3] = "La fecha debe ser igual o anterior al d&iacute;a de hoy";
                    return false;
                }

                return true
            },
            message: "{3}"
        }
    });


    $.extend($.fn.validatebox.defaults.rules,
{
    fechaMayorA:
    {
        validator: function (value, param) {
            var format = param[2];
            param[3] = 'Por favor, ingrese una fecha v&aacute;lida. Formato v&aacute;lido de Fecha: dd/mm/aaaa';
           
            var dFecha1 = null;
            var valido = true;
            try {
                if (!Date.parseExact(value, format)) return false;
                else {
                    dFecha1 = Date.parseExact(value, format);
                }
            } catch (e) {
                valido = false;
                return false;
            }
            var fechaMin = Date.parse("1900-01-01");
            var fechaMax = Date.parse("2100-01-01");


            var dFecha2 = Date.parseExact($(param[0]).datebox("getValue"), format);

            if ($(param[0]).datebox("getValue") == "") {
                param[3] = 'Por favor, ingrese la fechas desde. Formato v&aacute;lido de Fecha: dd/mm/aaaa en el campo' + param[1];
                return false;
            }


            if (dFecha1 <= dFecha2) {
                param[3] = "La fecha no es mayor que " + param[1];
                return false;
            }


            if (dFecha1 < fechaMin) {
                param[3] = "La fecha no es mayor que " + fechaMin.toString("dd/MM/yyyy");
                return false;
            }

            if (dFecha1 > fechaMax) {
                param[3] = "La fecha no es menor que " + fechaMax.toString("dd/MM/yyyy");
                return false;
            }

            return true;
        },
        message: "La fecha no es mayor que {1}"
        // $.fn.validatebox.defaults.rules.pattern.message
    }
});


    $.extend($.fn.validatebox.defaults.rules,
    {
        fechaMenorIgualA:
        {
            validator: function (value, param) {
                var format = param[2];

                //return true;
                //return fecha1.getTime() < fecha2.getTime()
                //return (fecha1.isBefore(fecha2))

                var valido = true;
                var dFecha1 = null;

                try {
                    if (!Date.parseExact(value, format)) return false;
                    else {
                        dFecha1 = Date.parseExact(value, format);
                    }
                } catch (e) {
                    valido = false;
                }
                var fechaMin = Date.parse("1900-01-01");
                var fechaMax = Date.parse("2100-01-01");
                
                var dFecha2 = Date.parseExact($(param[0]).datebox("getValue"), format);
                param[3] = 'Por favor, ingrese una fecha v&aacute;lida. Formato v&aacute;lido de Fecha: dd/mm/aaaa';

                if ($(param[0]).datebox("getValue") == "") {
                    param[3] = 'Por favor, ingrese la Fecha, Formato v&aacute;lido de Fecha: dd/mm/aaaa';
                    valido = false;
                }



                if (dFecha1 > dFecha2) {
                    param[3] = "La fecha no es menor que " + param[1];
                }


                if (dFecha1 < fechaMin) {
                    param[3] = "La fecha no es mayor que " + fechaMin.toString("dd/MM/yyyy");
                }

                if (dFecha1 > fechaMax) {
                    param[3] = "La fecha no es menor que " + fechaMax.toString("dd/MM/yyyy");
                }


                return (dFecha1 <= dFecha2 && dFecha1 >= fechaMin && dFecha1 <= fechaMax && valido);
            },
            message: "{3}"
            // $.fn.validatebox.defaults.rules.pattern.message
        }
    });
    $.extend($.fn.validatebox.defaults.rules,
    {
        fechaMenorA:
        {
            validator: function (value, param) {
                var format = param[2];

                //return true;
                //return fecha1.getTime() < fecha2.getTime()
                //return (fecha1.isBefore(fecha2))

                var valido = true;
                var dFecha1 = null;

                try {
                    if (!Date.parseExact(value, format)) return false;
                    else {
                        dFecha1 = Date.parseExact(value, format);
                    }
                } catch (e) {
                    valido = false;
                }
                var fechaMin = Date.parse("1900-01-01");
                var fechaMax = Date.parse("2100-01-01");
                var dFecha2 = Date.parseExact($(param[0]).datebox("getValue"), format);
                param[3] = 'Por favor, ingrese una fecha v&aacute;lida. Formato v&aacute;lido de Fecha: dd/mm/aaaa';

                if ($(param[0]).datebox("getValue") == "") {
                    param[3] = 'Por favor, ingrese la Fecha, Formato v&aacute;lido de Fecha: dd/mm/aaaa';
                    valido = false;
                }



                if (dFecha1 >= dFecha2) {
                    param[3] = "La fecha no es menor que " + param[1];
                }


                if (dFecha1 < fechaMin) {
                    param[3] = "La fecha no es mayor que " + fechaMin.toString("dd/MM/yyyy");
                }

                if (dFecha1 > fechaMax) {
                    param[3] = "La fecha no es menor que " + fechaMax.toString("dd/MM/yyyy");
                }


                return (dFecha1 < dFecha2 && dFecha1 >= fechaMin && dFecha1 <= fechaMax && valido);
            },
            message: "{3}"
            // $.fn.validatebox.defaults.rules.pattern.message
        }
    });
 

    $.fn.DeshabilitarFechaMayorHoy = function () {
        $(this).datebox().datebox('calendar').calendar({
            validator: function (endDate) {
                return endDate <= new Date();
            }
        });
    }
    
    $.fn.formatteDate = function () {
       
            $(this).datebox({
                formatter: function (date) {
                    var y = date.getFullYear();
                    var m = date.getMonth() + 1;
                    var d = date.getDate();
                    return (d < 10 ? ('0' + d) : d) + '/' + (m < 10 ? ('0' + m) : m) + '/' + y;
                },
                parser: function (s) {

                    if (!s) return new Date();
                    var ss = s.split('/');
                    var y = parseInt(ss[2], 10);
                    var m = parseInt(ss[1], 10);
                    var d = parseInt(ss[0], 10);
                    if (!isNaN(y) && !isNaN(m) && !isNaN(d)) {
                        return new Date(y, m - 1, d)
                    } else {
                        return new Date();
                    }
                }

            });

      

    }

    $.fn.combobox_sigees = function (options)
    {
        var defaults = {
            valueField: 'id',
            textField: 'text',
            url: "",
            id:"",
            parametro:null
        };
        var options = jQuery.extend({}, defaults, options);
        var url = options.url;
        if (options.parametro != null)
            url = url + '/' + options.parametro;

  
        $.ajax({
            url: url,            
            async: false,
            cache: false,
            dataType: 'json',
            success: function (resultado)
            {   $(options.id).combobox({ 
                    valueField:options.valueField,
                    textField:options.textField,
                    data: resultado 
                
                });
            },
            error: function (xhr, status, error) {
                $("#Message").Message({ Message: error, Icon: 'Error' });
            }
        });
      
    }


    $.fn.get_json_combobox = function (options) {
        var defaults = {        
            parametro: null
        };
        var resultado = "";

        var options = jQuery.extend({}, defaults, options);
        $.ajax({
            url: options.url + '/' + options.parametro,
            async: false,
            cache: false,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                
                resultado = data;
             
            },
            error: function (xhr, status, error) {
                $("#Message").Message({ Message: error, Icon: 'Error' });
            }
        });
        return resultado;
    }


    /*
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
    };*/
 

    $.fn.AbrirVentanaEmergente = function (options) {
        var defaults = {
            parametro: null,
            div: '',
            title: '',
            url: '',
            type: 'Post',
            success: null
        };
        var options = $.extend(defaults, options);
        this.each(function () {
            
            var url = options.url;
            if (options.parametro != null)
            {
                url = url + '/' + options.parametro;
            }
            
            $("#" + options.div).dialog("open").dialog('refresh', url);
            $("#" + options.div).dialog('setTitle', options.title);
            
        });
    };

    $.fn.checkText = function (options) {
        var defaults = {
            Value: '', // Compara con el valor del control.
            Section: '',
            Message: '', // Mensaje a mostrar.
            Controls: [], // Controles a incluir en la validación
            Operator: 'And', // Operador de validación de controles
            Control: null, // Control que indica la obligatoriedad del principal, sólo cuando este tenga un valor.
            CssClass: true,
            Focus: true,
            mostrarMessaje: true
        };
        var options = jQuery.extend({}, defaults, options);
        var blnText = true;

        var strValue = jQuery.trim($(this).val()).replace($(this).attr('title'), '');

        this.each(function () {
            blnText = strValue == options.Value ? false : true;
            for (i = 0; i < options.Controls.length; i++) {
                if (options.Operator == 'And') {
                    blnText = (blnText) && (jQuery.trim(options.Controls[i].val().replace(options.Controls[i].attr('title'), '')) == options.Value ? false : true);
                } else {
                    blnText = (blnText) || (jQuery.trim(options.Controls[i].val().replace(options.Controls[i].attr('title'), '')) == options.Value ? false : true);
                };
            };
            if (!blnText) {
                if (options.mostrarMessaje) {
                    $.messager.alert('Datos de validación.', options.Message, 'error');
                }

            };
        });
        return blnText;
    };

    $.NumberFormat = function (nStr,decimal) {
        nStr = parseFloat(nStr).toFixed(decimal);
        nStr += '';
        x = nStr.split('.');
        x1 = x[0];
        x2 = x.length > 1 ? '.' + x[1] : '';
        var rgx = /(\d+)(\d{3})/;
        while (rgx.test(x1)) {
            x1 = x1.replace(rgx, '$1' + ',' + '$2');
        }
        return x1 + x2;
    }

    $.validarFecha = function (fecha) {
        try {
            testdate = $.datepicker.parseDate('dd/mm/yy', fecha);
            return true;
        } catch (e) {

            return false;
        }
    }

    $.formatoFecha = function (fecha) {
        return fecha.substring(6, 10) + fecha.substring(3, 5) + fecha.substring(0, 2);
    }

    $.existeRegistroEnEdicionParaGrabar=  function (lista,mostrar_mensaje) {
        var editando = false;
        mostrar_mensaje = mostrar_mensaje == undefined || mostrar_mensaje==null ? true : mostrar_mensaje;
        var rows = $("#" + lista).datagrid("getRows");
        $.each(rows, function (i, objeto) {
            if (objeto.editing) {
                editando = true;
                return;
            }
        });
        if (!editando && mostrar_mensaje) {
            $.messager.alert("Registro en edici&oacute;n", "No existe registro en edici&oacute;n para  guardar.", "warning");
        }

        return editando;
    }

    $.validarRowGrilla=function (index, lista,mostrar_mensaje) {
        var _Valid = $('#' + lista).datagrid('validateRow', index);
        mostrar_mensaje = mostrar_mensaje == undefined || mostrar_mensaje == null ? true : mostrar_mensaje;
        if (!_Valid && mostrar_mensaje) {
            $.messager.alert("Campos Obligatorios", "Falta ingresar los campos requeridos de la grilla en edici&oacute;n.", "warning");
        }
        return _Valid;
    }

    $.existeRegistroEnEdicion = function (lista, mostrar_mensaje) {
        var editando = false;
        mostrar_mensaje = mostrar_mensaje == undefined || mostrar_mensaje == null ? true : mostrar_mensaje;
        
        var rows = $("#" + lista).datagrid("getRows");
        $.each(rows, function (i, objeto) {
            if (objeto.editing) {
                editando = true;
                return;
            }
        });
        if (editando && mostrar_mensaje) {
            $.messager.alert("Registro en edici&oacute;n", "Existe un registro en edici&oacute;n, para continuar guarde o cancele la edici&oacuten.", "warning");
        }
        return editando;
    }

    $.IndexRowEditing=  function(lista) {
        var index = null;
        var rows = $("#" + lista).datagrid("getRows");
        $.each(rows, function (i, objeto) {
            if (objeto.editing) {
                index = i;
                return false;
            }
        });
        return index;
    }


    $.fn.checkTextSM = function (options) {
        var defaults = {
            Value: '', // Compara con el valor del control.
            Section: '',
            Message: '', // Mensaje a mostrar.
            Controls: [], // Controles a incluir en la validación
            Operator: 'And', // Operador de validación de controles
            Control: null, // Control que indica la obligatoriedad del principal, sólo cuando este tenga un valor.
            CssClass: true,
            Focus: true

        };
        var options = jQuery.extend({}, defaults, options);
        var blnText = true;

        var strValue = jQuery.trim($(this).val()).replace($(this).attr('title'), '');

        this.each(function () {
            blnText = strValue == options.Value ? false : true;
            for (i = 0; i < options.Controls.length; i++) {
                if (options.Operator == 'And') {
                    blnText = (blnText) && (jQuery.trim(options.Controls[i].val().replace(options.Controls[i].attr('title'), '')) == options.Value ? false : true);
                } else {
                    blnText = (blnText) || (jQuery.trim(options.Controls[i].val().replace(options.Controls[i].attr('title'), '')) == options.Value ? false : true);
                };
            };

        });
        return blnText;
    };


    $.fn.ResizeModal = function (options) {
        var defaults = {
            widthMax: '95%',
            widthMin: '60%',
            div:''
        };
        var options = jQuery.extend({}, defaults, options);

        var mediaquery = window.matchMedia("(max-width: 600px)");
        if (mediaquery.matches) {
            $('#' + options.div).dialog('resize', { width: options.widthMax });
            $('#' + options.div).window('center');
        } else {
            $('#' + options.div).dialog('resize', { width: options.widthMin });
            $('#' + options.div).window('center');
        }

        $(window).resize(function () {
            var mediaquery = window.matchMedia("(max-width: 600px)");
            if (mediaquery.matches) {
                $('#' + options.div).dialog('resize', { width: '95%' });
            } else {
                $('#' + options.div).dialog('resize', { width: options.widthMin });
            }
            $('#' + options.div).window('center');

        });

    }

    $.extend($.fn.validatebox.defaults.rules, {
        menorA: {
            validator: function (value, param) {
                param[3] = "No es un n&uacute;mero v&aacute;lido.";
                var num_val = parseFloat(value);
                var num = parseFloat(value);

                if (isNaN(value)) {
                    return false;
                }                
                param[3] = "El valor que intenta ingresar es superior a (" + param[0] + ").";
                num = parseFloat(num.toFixed(param[1]));
                if (num > param[0]) {
                    return false;
                }
                return true;
            },
            message: "{3}"
        }
    });

    $.extend($.fn.validatebox.defaults.rules, {
        MayorACeroMenorA: {
            validator: function (value, param) {
                param[3] = "No es un n&uacute;mero v&aacute;lido.";
                //var num_val = parseFloat(value);
                var num = parseFloat(value);

                if (isNaN(value)) {
                    return false;
                }

                param[3] = "El monto ingresado debe ser mayor a cero.";
                if (num<=0) {
                    return false;
                }
                param[3] = "El valor que intenta ingresar es superior a (" + param[0] + ").";
                num = parseFloat(num.toFixed(param[1]));
                if (num > param[0]) {
                    return false;
                }
                

                return true;
            },
            message: "{3}"
        }
    });


    $.extend($.fn.validatebox.defaults.rules, {
        MayorACeroMenorIgualA: {
            validator: function (value, param) {
                param[3] = "No es un n&uacute;mero v&aacute;lido.";
                //var num_val = parseFloat(value);
                var num = parseFloat(value);
                //console.log("num " + num);
                if (isNaN(value)) {
                    return false;
                }

                param[3] = "El monto ingresado debe ser mayor a cero.";
                if (num <= 0) {
                    return false;
                }
                //console.log("param[1]: " +parseFloat(param[0]));
                param[3] = "El valor que intenta ingresar es superior a (" + param[0] + ").";
                //num = parseFloat(param[0]);
                //console.log(num);
                if (num > parseFloat(param[0])) {
                    return false;
                }
              

                return true;
            },
            message: "{3}"
        }
    });


    $.extend($.fn.validatebox.defaults.rules, {
        MayorACeroMenorIgualANumberBox: {
            validator: function (value, param) {
                param[3] = "No es un n&uacute;mero v&aacute;lido.";
                
                var num = parseFloat(value);
                
                if (isNaN(value)) {
                    return false;
                }

                param[3] = "El monto ingresado debe ser mayor a cero.";
                if (num <= 0) {
                    return false;
                }
                                
                var monto_limite = $(param[0]).numberbox("getValue");
                
                param[3] = "El valor que intenta ingresar es superior a (" + monto_limite + ").";
                
                if (num > parseFloat(monto_limite)) {
                    return false;
                }


                return true;
            },
            message: "{3}"
        }
    });

    $.extend($.fn.validatebox.defaults.rules, {
        validarMontoEntre: {
            validator: function (value, param) {
                param[3] = "No es un n&uacute;mero v&aacute;lido.";
                //var num_val = parseFloat(value);
                var num = parseFloat(value);

                if (isNaN(value)) {
                    return false;
                }

                param[3] = "El monto ingresado debe mayor o igual que cero.";
                if (num < 0) {
                    return false;
                }
                param[3] = "El valor que intenta ingresar es superior a (" + param[0] + ").";
                num = parseFloat(num.toFixed(param[1]));
                if (num > param[0]) {
                    return false;
                }


                return true;
            },
            message: "{3}"
        }
    });
    $.extend($.fn.validatebox.defaults.rules, {
        mayorACero: {
            validator: function (value, param) {
                param[3] = "No es un n&uacute;mero v&aacute;lido.";
                //var num_val = parseFloat(value);
                var num = parseFloat(value);

                if (isNaN(value)) {
                    return false;
                }
                param[3] = "El valor que intenta ingresar es menor  a (" + param[0] + ").";
                //num = parseFloat(param[1]);
                if (num < parseFloat(param[0]) || num == 0) {
                    
                    return false;
                }
                return true;
            },
            message: "{3}"
        }
    });
  
    $.extend($.fn.validatebox.defaults.rules, {
        ValidarSoloTexto: {
            validator: function (value, param) {
                var characterReg = /[`~!@#$^¬|\=?;\{\}\[\]\\\/]/gi;
                return !characterReg.test(value);                
            },
            message: 'Este campo solo permite ingreso de caracteres.'
        }
    });

    $.extend($.fn.validatebox.defaults.rules, {
        ValidarNumeros: {
            validator: function (value, param) {
                var characterReg = /[a-zA-ZñÑ_`~!@#$%^&*()°¬|+\=?;:'"<>\{\}\[\]\\\/]/gi;
                return !characterReg.test(value);
            },
            message: 'Este campo solo permite ingreso de numeros.'
        }
    });

    $.extend($.fn.validatebox.defaults.rules, {
        ValidarTelefono: {
            validator: function (value, param) {
                var characterReg = /[a-zA-ZñÑ_`~!@#$%^&*°¬|+\=?;:'"<>\{\}\[\]\\\/]/gi;
                return !characterReg.test(value);
            },
            message: 'Este campo solo permite ingreso de caracteres telefonicos.'
        }
    });

    $.extend($.fn.validatebox.defaults.rules, {
        ValidarCuentaBancaria: {
            validator: function (value, param) {
                var characterReg = /[a-zA-ZñÑ_()`~!@#$%^&*°¬|+\=?;:'"<>\{\}\[\]\\\/]/gi;
                return !characterReg.test(value);
            },
            message: 'Este campo solo permite ingreso de numeros.'
        }
    });

    $.fnLoaderDatagrid = function (param, success, error) {
        $.ajax({
            url: param.url,
            data: param,
            type: 'post',
            dataType: 'json',
            success: function (data, textStatus, jqXHR) {
                if (data.sucess == 1)
                    success(data);
                else {
                    success(data);
                    error(data);
                }
                return;
            }
        });
    }


    $.fn.datagrid_configurable = function (options) {
        var defaults = {
            id: '',
            url: '',
            name: '',
            columns: [],
            colSearch: [],
            colModel: [],
            height: 270,
            width: 360,
            rowNum: 0,
            page: 0,
            sortName: '',
            sortOrder: 'asc',
            /*-----------------------*/
            emptyMsg:"",
            fitColumns: false,
            idField: '',
            pagination: false,
            singleSelect: true,
            rownumbers: true,
            onDblClickRow: null,
            onClickRow:null,
            remoteSort: false,
            onLoadError: null,
            loader: null,
            cargar_data: true,
            view:null,
            pageSize: 20,
            paginar_server:false,
            pageList: [20, 40, 60, 80,100,150,200],
            selectOnCheck: false,
        };

        var options = $.extend(defaults, options);
        var strID = '';
        this.each(function () {
            strID = (options.name == '') ? $(this).attr("id") : options.name;
            strID = '#' + strID;
            $(strID).datagrid({
                columns: options.columns,
                fitColumns: options.fitColumns,
                idField: options.idField,
                pagination: options.pagination,
                singleSelect: options.singleSelect,
                rownumbers: options.rownumbers,
                remoteSort: options.remoteSort,
                view: options.view,
                //selectOnCheck:options.selectOnCheck,
                //emptyMsg: options.emptyMsg,
                 pageList: options.pageList,
                 pageSize:options.pageSize,
                 loader: function (param, success, error) {
                    var item = {};
                    for (i = 0; i < options.colSearch.length; i++) {
                        if (options.colSearch[i].type == 'int') {
                            if (options.colSearch[i].value == null) {
                                item[options.colSearch[i].col] = $('#' + options.colSearch[i].name).valInt();
                            } else {
                                item[options.colSearch[i].col] = $('#' + options.colSearch[i].name).valInt({ value: options.colSearch[i].value });
                            };
                        } else if (options.colSearch[i].type == 'string') {
                            //item[options.colSearch[i].col] = $('#' + options.colSearch[i].name).valStr();
                        } /*else if (options.colSearch[i].type == 'bool') {
                            if (options.colSearch[i].control != null) {
                                item[options.colSearch[i].col] = $('#' + options.colSearch[i].name).is(':checked') ? 1 : $('#' + options.colSearch[i].control).is(':checked') ? 0 : '';
                            } else {
                                item[options.colSearch[i].col] = $('#' + options.colSearch[i].name).valStr();
                            };
                        } */else if (options.colSearch[i].type == 'val') {
                            item[options.colSearch[i].col] = options.colSearch[i].value;
                        } else if (options.colSearch[i].type == 'easyui_textbox') {
                            item[options.colSearch[i].col] =$.trim($("#" + options.colSearch[i].name).val());//$.trim($('#' + options.colSearch[i].name).textbox('getText'));
                        }
                    };
                    var opts = $(strID).datagrid('options');
                     /*
                    debugger;
                    var opts = $(strID).datagrid('options');                    
                    var _view = opts.view;                   
                    var iCurrentPage = 0;
                    if (typeof (_view) === 'undefined' || _view == null) {
                        iCurrentPage = opts.pageNumber;
                    }
                    else {
                        var _renderedCount = _view.renderedCount;
                        if (typeof (_renderedCount) === 'undefined' || _renderedCount == null) {
                            iCurrentPage = opts.pageNumber;
                        }
                        else {
                            iCurrentPage= Math.floor(_view.renderedCount / opts.pageSize) + 1;
                        }

                    }
                 */
                    item['iPageSize'] =  opts.pageSize;
                    item['iCurrentPage'] = opts.pageNumber;
                    item['vSortColumn'] = opts.sortName;
                    item['vSortOrder'] = opts.sortOrder;
                    
                        $.ajax({
                            url: options.url,
                            data: item,
                            type: 'post',
                            dataType: 'json',
                            //cache: false,
                            success: function (data, textStatus, jqXHR) {
                                if (data.sucess == 1) {
                                    success(data);
                                }
                                else {
                                    success(data);
                                    //error(data);
                                }
                                return;
                            }
                        });
                },

                onDblClickRow: options.onDblClickRow,
                onClickRow: options.onClickRow,
                onLoadError: function (_error, textStatus, errorThrown) {
                    var opts = $(strID).datagrid('options');
                    var vc = $(strID).datagrid('getPanel').children('div.datagrid-view');
                    vc.children('div.datagrid-empty').remove();
                    var _v_mensaje = _error.message ? _error.message : opts.emptyMsg;
                    //if (!$(strID).datagrid('getRows').length)
                    //{
                    //    _v_mensaje = "Registro no encontrado";
                    //}
                    var d = $('<div class="datagrid-empty"></div>').html(_v_mensaje).appendTo(vc);
                    d.css({
                        position: 'absolute',
                        color: 'red',
                        left: 0,
                        top: 50,
                        width: '100%',
                        textAlign: 'center'
                    });
                }
            });

            //debugger;
            //if(options.pagination&&options.paginar_server)
            //{                
            //    var p = $(strID).datagrid('getPager');
            //    $(p).pagination({                    
            //        onSelectPage: function (pageNumber, pageSize) {
            //            alert(pageNumber);
            //            $(strID).datagrid('reload')

            //        }
            //    });
            //    //var obj = $(p).pagination('options');
            //    //fnBuscarArticulo(obj.pageNumber, obj.pageSize);
            
            //}




        });
    }


    
   

})(jQuery);