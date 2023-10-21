var ActionAdicionarCuotaUrl = {};

; (function (app) {
    //===========================================================================================
    var current = app.adicionarcuota = {};
    //===========================================================================================

    jQuery.extend(app.adicionarcuota,
    {
        Initialize: function (actionUrls) {
            jQuery.extend(ActionAdicionarCuotaUrl, actionUrls);
            InicializarControles();
        }
    })
})(project);

function InicializarControles() {
    $('.content').combobox_sigees({
        parametro: "?codigo_empresa=" + ActionAdicionarCuotaUrl._codigo_empresa + "&nro_contrato=" + ActionAdicionarCuotaUrl._nro_contrato,
        id: '#tipo_planilla',
        url: ActionAdicionarCuotaUrl.GetTipoPlanillaJson
    });

}

function AceptarAdicionar() {
    var nombreOpcion = 'Adicionar Cuota';
    if (!$("#frm_adicionarcuota").form('enableValidation').form('validate'))
        return;

    var monto_neto = parseFloat($('#monto_neto').numberbox('getValue'));

    if (monto_neto == 0) {
        $.messager.alert(nombreOpcion, 'El monto de comisión no puede ser cero.', 'warning');
        return false;
    }
    
    $.messager.confirm('Confirmaci&oacute;n', '¿Est&aacute; seguro que desea adicionar esta cuota?', function (result) {
        if (result) {
            var pEntidad = {
                codigo_empresa: ActionAdicionarCuotaUrl._codigo_empresa,
                nro_contrato: ActionAdicionarCuotaUrl._nro_contrato,
                codigo_articulo: ActionAdicionarCuotaUrl._codigo_articulo,
                codigo_tipo_planilla: $.trim($('#tipo_planilla').combobox('getValue')),
                motivo: $.trim($("#txt_motivo").val()),
                monto_neto : monto_neto            
            };
            $.ajax({
                type: 'post',
                url: ActionAdicionarCuotaUrl.AdicionarCuota,
                data: pEntidad,
                async: true,
                cache: false,
                dataType: 'json',
                success: function (data) {
                    if (data.v_resultado == 1) {
                        $.messager.alert('Operación exitosa', data.v_mensaje, 'info', function () {
                            ActualizarGrillaArticulos();
                            CerrarAdicionar();
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

function CerrarAdicionar() {
    $('#div_adicionar_cuota').dialog('close');
}
