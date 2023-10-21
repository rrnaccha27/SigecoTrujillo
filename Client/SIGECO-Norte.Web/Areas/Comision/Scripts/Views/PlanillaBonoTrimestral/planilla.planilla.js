var ActionPlanillaUrl = {};

; (function (app) {
    //===========================================================================================
    var current = app.planilla = {};
    //===========================================================================================

    jQuery.extend(app.planilla,
        {
            Initialize: function (actionUrls) {
                jQuery.extend(ActionPlanillaUrl, actionUrls);
                fnInicializarPlanilla();
                fnConfigurarGrillaPagoHabilitado();
            }
        })
})(project);

function fnInicializarPlanilla() {   
    
    $('.content').combobox_sigees({
        id: '#cmb_tipo_planilla',
        url: ActionPlanillaUrl._GetTipoPlanillaJson
    });

    $('.content').combobox_sigees({
        id: '#cmb_regla',
        url: ActionPlanillaUrl._GetReglaJson
    });

    $('.content').combobox_sigees({
        id: '#cmb_periodo',
        url: ActionPlanillaUrl._GetPeriodoJson
    });


    $('.content').ResizeModal({
        widthMax: '90%',
        widthMin: '80%',
        div: 'div_registrar_planilla'
    });
   
}

function fnConfigurarGrillaPagoHabilitado() {
    $('#dgv_planilla_pago_habilitado').datagrid({
        fitColumns: true,
        data: null,
        rownumbers: true,               
        url: ActionPlanillaUrl._GetPagoHabilitadoJson,
        queryParams: {
            codigo_planilla: ActionPlanillaUrl._codigo_planilla            
        },
        pagination: true,
        singleSelect: true,
        toolbar:'#toolbar_pago_habilitado',
        pageList: [20, 50, 100, 200, 400, 500],
        pageSize: 20,

        columns:
        [[
            { field: 'nombre_canal', title: 'Canal', width: 160, align: 'left' },
            { field: 'nombre_grupo', title: 'Grupo', width: 220, align: 'left' },
            { field: 'nombre_supervisor', title: 'Supervisor', width: 300, align: 'left' },
            { field: 'nombre_personal', title: 'Vendedor', width: 300, halign: "center", align: "left" },
            {
                field: 'monto_contratado', title: 'Monto<br>Contratado', width: 120, halign: "center", align: "right", formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                }
            },
            { field: 'rango', title: 'Puesto', width: 70, halign: "center", align: "right" },
            {
                field: 'monto_bono', title: 'Monto<br>Bono', width: 100, halign: "center", align: "right", formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                }
            },
        ]]
    });
    $('#dgv_planilla_pago_habilitado').datagrid('enableFilter');

    $(window).resize(function () {
        $('#dgv_planilla_pago_habilitado').datagrid('resize');
    });
}

function RegistrarPlanilla() {
    if (!$("#fmr_registrar_planilla").form('enableValidation').form('validate'))
        return;

    var pEntidad = {
        codigo_regla: $.trim($("#cmb_regla").combobox('getValue')),
        codigo_periodo: $.trim($("#cmb_periodo").combobox('getValue')),
    };

    $.messager.confirm('Confirmaci&oacute;n', '¿Est&aacute; seguro que desea aperturar la planilla?', function (result) {
        if (result) {
            $.ajax({
                type: 'post',
                url: ActionPlanillaUrl._Registrar,
                data: JSON.stringify({ v_planilla: pEntidad}),
                async: true,
                cache: false,
                dataType: 'json',
                contentType: 'application/json',
                success: function (data) {
                    switch (data.v_resultado){
                        case 1:
                            $.messager.alert('Operación exitosa', data.v_mensaje, 'info', function () {
                                fnConsultarPlanilla();
                                $("#div_registrar_planilla").dialog("close");
                                fnModificarPlanillaById(data.codigo_planilla);
                            });
                            break;
                        case -1:
                            $.messager.alert('Apertura Planilla', 'No se generó planilla debido a que no se cumplió la meta.', 'warning');
                            break;
                        case -2:
                            $.messager.alert('Apertura Planilla', data.v_mensaje, 'warning');
                            break;
                    }
                },
                error: function () {
                    $.messager.alert('Error', "Error en el servidor", 'error');
                }
            });
        }
    });
}

function CerrarPlanilla(p_codigo_planilla) {
    var pEntidad = {
        codigo_planilla: p_codigo_planilla
    };

    console.log(pEntidad);
    $.messager.confirm('Confirmaci&oacute;n', '¿Est&aacute; seguro que desea cerrar la planilla?', function (result) {
        if (result) {
            $.ajax({
                type: 'post',
                url: ActionPlanillaUrl._Cerrar,
                data: pEntidad,
                async: true,
                cache: false,
                dataType: 'json',
                success: function (data) {
                    if (data.v_resultado == 1) {

                        $.messager.alert('Operación exitosa', data.v_mensaje, 'info', function () {
                            fnConsultarPlanilla();
                            $("#div_registrar_planilla").dialog("close");
                            fnModificarPlanillaById(p_codigo_planilla);
                        });
                    }
                    else {
                        $.messager.alert('Cierre Planilla', data.v_mensaje, 'error');
                    }
                },
                error: function () {
                    $.messager.alert('Error', "Error en el server", 'error');
                }
            });
        }
    });
}

function AnularPlanilla(p_codigo_planilla) {
    $.messager.confirm('Confirmaci&oacute;n', '¿Est&aacute; seguro que desea anular la planilla?', function (result) {
        if (result) {
            $.ajax({
                type: 'post',
                url: ActionPlanillaUrl._Anular,
                data: { p_codigo_planilla: p_codigo_planilla },
                async: false,
                cache: false,
                dataType: 'json',
                success: function (data) {
                    if (data.v_resultado == 1) {

                        $.messager.alert('Operación exitosa', data.v_mensaje, 'info', function () {
                            fnConsultarPlanilla();
                            $("#div_registrar_planilla").dialog("close");
                            fnModificarPlanillaById(p_codigo_planilla);
                        });
                    }
                    else {
                        $.messager.alert('Anular Planilla', data.v_mensaje, 'error');
                    }
                },
                error: function () {
                    $.messager.alert('Error', "Error en el servidor", 'error');
                }
            });
        }
    });
}

function cerrar_planilla() {
    $("#div_registrar_planilla").dialog("close");

}

function fnReporte(tipo_reporte) {
    var titulo = "";
    var url = "";
    debugger;
    switch (tipo_reporte)
    {
        case 1:
            titulo = "Reporte Planilla";
            //url = ActionPlanillaUrl._Reporte_Planilla;
            break;
        case 2:
            titulo = "Reporte Liquidación";
            //url = ActionPlanillaUrl._Reporte_Liquidacion;
            break;
        case 3:
            titulo = "Reporte Resúmen Liquidación";
            //url = ActionPlanillaUrl._Reporte_Resumen_Liquidacion;
            break;
    }

    $(this).AbrirVentanaEmergente({
        parametro: "?p_codigo_planilla=" + ActionPlanillaUrl._codigo_planilla + "&tipo_reporte=" + tipo_reporte,
        div: 'div_reporte_general',
        title: titulo,
        url: ActionPlanillaUrl._Reporte
    });
}

function fnEnviarLiquidacion() {
    $.messager.confirm('Confirmaci&oacute;n', '¿Est&aacute; seguro que desea enviar liquidaciones?', function (result) {
        if (result) {
            $.ajax({
                type: 'post',
                url: ActionPlanillaUrl._Enviar_Correo_Liquidacion,
                data: { p_codigo_planilla: ActionPlanillaUrl._codigo_planilla },
                async: true,
                cache: false,
                dataType: 'json',
                success: function (data) {
                    if (data.v_resultado == 1) {
                        $.messager.alert('Operación exitosa', data.v_mensaje, 'info');
                    }
                    else {
                        $.messager.alert('Error en enviar correo', data.v_mensaje, 'error');
                    }
                },
                error: function () {
                    $.messager.alert('Error', "Error en el servidor", 'error');
                }
            });
        }
    });
}
