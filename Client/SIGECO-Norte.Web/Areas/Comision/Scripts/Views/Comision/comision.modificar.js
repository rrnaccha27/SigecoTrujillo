var ActionModificarComisionUrl = {};

; (function (app) {
    //===========================================================================================
    var current = app.modificarcomision = {};
    //===========================================================================================

    jQuery.extend(app.modificarcomision,
        {
            Initialize: function (actionUrls) {
                jQuery.extend(ActionModificarComisionUrl, actionUrls);
                testValidar();
            }
        })
})(project);


function GuardarModificar() {
    var codigo_comision_manual = ActionModificarComisionUrl._codigo_comision_manual;

    var pEntidad = {
        codigo_comision_manual: codigo_comision_manual,
        nro_factura_vendedor: $("#txt_nro_factura_vendedor_m").val()
    };

    debugger;

    $.messager.confirm('Confirmaci&oacute;n', '¿Est&aacute; seguro que desea guardar esta comisión?', function (result) {
        if (result) {
            $.ajax({
                type: 'post',
                url: ActionModificarComisionUrl.ActualizarLimitado,
                data: pEntidad,
                async: false,
                cache: false,
                dataType: 'json',
                success: function (data) {
                    if (data.v_resultado == 1) {
                        $.messager.alert('Operación exitosa', data.v_mensaje, 'info', function () {
                            $('#hdCodigo').val('');
                            $('#dgv_comision').datagrid('clearSelections');
                            fnReloadGrillaComision();
                            $("#div_modificar_comision").dialog("close");
                        });
                    }
                    else {
                        $.messager.alert('Error', data.v_mensaje, 'error');
                    }
                },
                error: function () {
                    $.messager.alert('Error', "Error en el servidor", 'error');
                }
            });
        }
    });
}

function testValidar() {
    if (parseInt(ActionModificarComisionUrl._codigo_comision_manual, 10) < 1) {
        return false;
    }
    var mensaje = '';
    $.ajax({
        type: 'post',
        url: ActionModificarComisionUrl.ValidarReferencia,
        data: { codigo_comision_manual: ActionModificarComisionUrl._codigo_comision_manual },
        async: false,
        cache: false,
        dataType: 'json',
        success: function (data) {
            if (data.mensaje.length > 0) {
                $.messager.alert('Modificar', "Solo podrá modificar algunos datos: " + data.mensaje, 'warning');
            }
        },
        error: function () {
            $.messager.alert('Registro Manual de Comisión', "Error en el servidor", 'error');
        }
    });
}

function CerrarModificar() {
    $('#div_modificar_comision').dialog("close");
}
