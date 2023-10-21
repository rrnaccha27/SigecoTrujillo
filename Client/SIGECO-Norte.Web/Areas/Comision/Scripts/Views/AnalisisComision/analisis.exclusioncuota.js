var ActionExclusionUrl = {};

; (function (app) {
    //===========================================================================================
    var current = app.exclusioncuota = {};
    //===========================================================================================

    jQuery.extend(app.exclusioncuota,
    {
        Initialize: function (actionUrls) {
            jQuery.extend(ActionExclusionUrl, actionUrls);
        }
    })
})(project);


function AceptarExcluir() {
    
    if (!$("#frm_exclusion").form('enableValidation').form('validate'))
        return;

    var rows = $('#dgv_contrato_detalle_pago').datagrid('getSelections');

    lst_detalle_cronograma = [];
    $.each(rows, function (index, data) {
        var detalle = {
            codigo_detalle_cronograma: data.codigo_detalle_cronograma
        };
        lst_detalle_cronograma.push(detalle);
    });

    $.messager.confirm('Confirmaci&oacute;n', '¿Est&aacute; seguro que desea excluir la cuota?', function (result) {
        if (result) {
            var pEntidad = {
                lst_detalle_cronograma: lst_detalle_cronograma,
                motivo: $.trim($("#txt_motivo_exclusion").val()),
                permanente: $('#ckb_permanente').switchbutton('options').checked
            };
            $.ajax({
                type: 'post',
                url: ActionExclusionUrl.Excluir,
                data: JSON.stringify({ v_exclusion: pEntidad}),
                async: true,
                cache: false,
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    if (data.v_resultado == 1) {
                        $.messager.alert('Operación exitosa', data.v_mensaje, 'info', function () {
                            ActualizarGrillaCuotas();
                            CerrarExcluir();
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

function CerrarExcluir() {
    $('#div_exclusion_planilla').dialog('close');
}

