var ActionModificarCuotaUrl = {};

; (function (app) {
    //===========================================================================================
    var current = app.modificarcuota = {};
    //===========================================================================================

    jQuery.extend(app.modificarcuota,
    {
        Initialize: function (actionUrls) {
            jQuery.extend(ActionModificarCuotaUrl, actionUrls);
        }
    })
})(project);


function AceptarModificar() {
    var nombreOpcion = 'Modificar Cuota';
    if (!$("#frm_modificarcuota").form('enableValidation').form('validate'))
        return;

    var importe_comision = parseFloat($('#importe_comision').numberbox('getValue'));
    var importe_comision_original = parseFloat($('#importe_comision_original').numberbox('getValue'));

    if (importe_comision == importe_comision_original) {
        $.messager.alert(nombreOpcion, 'El nuevo monto de comisión es igual al actual.', 'warning');
        return false;
    }
    
    $.messager.confirm('Confirmaci&oacute;n', '¿Est&aacute; seguro que desea modificar la cuota?', function (result) {
        if (result) {
            var pEntidad = {
                codigo_detalle_cronograma: ActionModificarCuotaUrl._codigo_detalle_cronograma,
                observacion: $.trim($("#txt_motivo").val()),
                importe_comision : importe_comision            
            };
            $.ajax({
                type: 'post',
                url: ActionModificarCuotaUrl.ModificarCuota,
                data: pEntidad,
                async: true,
                cache: false,
                dataType: 'json',
                success: function (data) {
                    if (data.v_resultado == 1) {
                        $.messager.alert('Operación exitosa', data.v_mensaje, 'info', function () {
                            ActualizarGrillaArticulos();
                            CerrarModificar();
                        });
                    }
                    else {
                        $.messager.alert('Error', data.v_mensaje, 'error');
                    }
                },
                error: function () {
                    $.messager.alert('Error', "Error en el server", 'error');
                }
            });
        }
    });
    
}

function CerrarModificar() {
    $('#div_modificar_cuota').dialog('close');
}
