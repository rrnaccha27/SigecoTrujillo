var ActionExclusionUrl = {};

; (function (app) {
    //===========================================================================================
    var current = app.exclusion = {};
    //===========================================================================================

    jQuery.extend(app.exclusion,
    {
        Initialize: function (actionUrls) {
            jQuery.extend(ActionExclusionUrl, actionUrls);
            //$('#txt_motivo_exclusion').textbox('clear').textbox('textbox').focus();
        }
    })
})(project);


function Excluir() {
    
    if (!$("#frm_exclusion").form('enableValidation').form('validate'))
        return;

    $.messager.confirm('Confirmaci&oacute;n', '¿Est&aacute; seguro que desea excluir el pago seleccionado??', function (result) {
        if (result) {
            var pEntidad = {
                codigo_planilla: ActionExclusionUrl._codigo_planilla,
                codigo_detalle_planilla: ActionExclusionUrl._codigo_detalle_planilla,
                observacion: $.trim($("#txt_motivo_exclusion").val()),
                excluido: $('#ckb_permanente').switchbutton('options').checked
            };
            $.ajax({
                type: 'post',
                url: ActionExclusionUrl.Excluir,
                data: pEntidad,
                async: true,
                cache: false,
                dataType: 'json',
                success: function (data) {
                    if (data.v_resultado == 1) {
                        $.messager.alert('Operación exitosa', data.v_mensaje, 'info', function () {
                            $('#dgv_planilla_pago_habilitado').datagrid('reload');
                            $('#dgv_planilla_pago_exclusiones').datagrid('reload');
                            Cerrar();
                        });
                    }
                    else {
                        $.messager.alert('Error en la exclusión de pago', data.v_mensaje, 'error');
                    }
                },
                error: function () {
                    $.messager.alert('Error', "Error en el server", 'error');
                }
            });
        }
    });
    
}

function Cerrar() {
    $('#div_exclusion_planilla').dialog('close');
}

