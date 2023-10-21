
;(function (app) {
    //===========================================================================================
    var current = app.ReglaRecupero = {};
    //===========================================================================================

    jQuery.extend(app.ReglaRecupero,
        {

            ActionUrls: {},
            EditType: '',
            HasRootNode: 'False',

            Initialize: function (actionUrls) {
                //$(window).resize(function () {
                //    current.Redimensionar();
                //});

                jQuery.extend(project.ActionUrls, actionUrls);

                $('#fechaInicio').datebox({

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

                $('#fechaFin').datebox({

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


            },

        })
})(project);


function GuardarRegla() {
    var esNuevo = project.ActionUrls.codigo_regla_recupero == -1? true:false;
    var message = '';
    var codigo_regla_recupero = project.ActionUrls.codigo_regla_recupero;
    var nombre = $.trim($('#nombre').val());
    var nro_cuota = $('#nro_cuota').val();
    var vigencia_inicio = $.trim($('#fechaInicio').textbox('getText'));
    var vigencia_fin = $.trim($('#fechaFin').textbox('getText'));

    if (!$("#frmRegistro").form('enableValidation').form('validate')) {
        return false;
    }

    if (parseInt(nro_cuota, 10) == 0)
    {
        $.messager.alert("Regla Recupero", "El nro de cuota debe ser mayor a cero.", "warning");
        return false;
    }


    if (!ValidarFecha(vigencia_inicio))
    {
        $.messager.alert("Regla Recupero", "Inicio de Vigencia en formato incorrecto.", "warning");
        return false;
    }

    if (!ValidarFecha(vigencia_fin)) {
        $.messager.alert("Regla Recupero", "Fin de Vigencia en formato incorrecto.", "warning");
        return false;
    }

    var v_vigencia_inicio = FormatoFecha(vigencia_inicio);
    var v_vigencia_fin = FormatoFecha(vigencia_fin);

    if (parseInt(v_vigencia_inicio) > parseInt(v_vigencia_fin))
    {
        $.messager.alert("Regla Recupero", "Inicio de Vigencia debe ser menor a Fin Vigencia.", "warning");
        return false;
    }

    var datosReglaBono = {
        codigo_regla_recupero: codigo_regla_recupero,
        nombre: nombre,
        nro_cuota: nro_cuota,
        vigencia_inicio: vigencia_inicio,
        vigencia_fin: vigencia_fin,
    };

    if (message.length > 0) {
        $.messager.alert((esNuevo ? 'Registro' : 'Modificar'), message, 'warning');
        return false;
    }

    $.messager.confirm('Confirm', '&iquest;Seguro que desea guardar?', function (result) {
        if (result) {
            $.ajax({
                type: 'post',
                url: project.ActionUrls.Guardar,
                data: JSON.stringify({ regla: datosReglaBono }),
                dataType: 'json',
                cache: false,
                async: false,
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    if (data.Msg) {
                        if (data.Msg != 'Success') {
                            $.messager.alert("Error", data.Msg, "error");
                            //project.AlertErrorMessage('Error', data.Msg);
                        }
                        else {
                            $('#dlgRegistro').dialog('close');                          

                            project.ShowMessage('Alerta', (esNuevo?'Registro':'Modificado') + ' Exitoso');
                            GetAllJson();
                        }
                    }
                    else {
                        project.AlertErrorMessage('Error', 'Error de procesamiento');
                    }
                },
                error: function () {
                    project.AlertErrorMessage('Error', 'Error');
                }
            });
        }
    });

}

function Cancelar() {
    $('#dlgRegistro').dialog('close');
}

function ValidarFecha(fecha)
{
    try {
        testdate = $.datepicker.parseDate('dd/mm/yy', fecha);
        return true;
    } catch (e) {
        
        return false;
    }
}

function FormatoFecha(fecha) {
    return fecha.substring(6, 10) + fecha.substring(3, 5) + fecha.substring(0, 2);
}
