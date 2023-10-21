var ActionPlanillaUrl = {};

; (function (app) {
    //===========================================================================================
    var current = app.planilla = {};
    //===========================================================================================

    jQuery.extend(app.planilla,
        {
            Initialize: function (actionUrls) {
                jQuery.extend(ActionPlanillaUrl, actionUrls);
                fnConfigurarGrillaPagoHabilitado();
                //fnConfigurarGrillaPagoExcluido();
                fnInicializarPlanilla();
            }
        })
})(project);

function fnInicializarPlanilla() {   
    
    $('.content').combobox_sigees({
        id: '#cmb_tipo_planilla',
        url: ActionPlanillaUrl._GetTipoPlanillaJson
    });

    $('.content').combobox_sigees({
        id: '#cmb_canal_venta',
        url: ActionPlanillaUrl._GetCanalJson
    });

    $('#dtp_fecha_inicio').formatteDate();
    $('#dtp_fecha_fin').formatteDate();

    $('.content').ResizeModal({
        widthMax: '90%',
        widthMin: '80%',
        div: 'div_registrar_planilla'
    });
   
}

function fnConfigurarGrillaPagoHabilitado() {
    $('#dgv_planilla_pago_habilitado').datagrid({
       // fitColumns: true,
        data: null,
        rownumbers: true,               
        url: ActionPlanillaUrl._GetPagoHabilitadoJson,
        queryParams: {
            codigo_planilla: ActionPlanillaUrl._codigo_planilla            
        },
        pagination: true,
        singleSelect: true,
        //toolbar:'#toolbar_pago_habilitado',
        pageList: [20, 50, 100, 200, 400, 500],
        pageSize: 20,

        columns:
        [[
            { field: 'nombre_canal', title: 'Canal', width: 150, align: 'left' },
            { field: 'nombre_grupo_canal', title: 'Grupo', width: 150, align: 'left' },
            { field: 'apellidos_nombres', title: 'Vendedor', width: 430, align: 'left' },
            { field: 'nombre_empresa', title: 'Empresa', width: 80, halign: "center", align: "left" },
            { field: 'nro_contrato', title: 'Contrato', width: 95, halign: "center", align: "center" },
            //{ field: 'nombre_articulo', title: 'Artículo', width: 350, halign: "center", align: "left" },
            { field: 'nombre_moneda', title: 'Moneda', width: 80, halign: "center", align: "left" },
            {
                field: 'monto_neto', title: 'Monto<br>Bono', width: 100, halign: "center", align: "right", formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                }
            },
        ]]
    });
    $('#dgv_planilla_pago_habilitado').datagrid('enableFilter');

    if (ActionPlanillaUrl._es_consulta == '1') {
        return false;
    }

    let botonExcel = true;

    if (ActionPlanillaUrl._codigo_estado_planilla == '2' && ActionPlanillaUrl._codigo_tipo_planilla == "2") {
        botonExcel = false;
    }

    var pager = $('#dgv_planilla_pago_habilitado').datagrid('getPager');
    var buttonDisabled = (ActionPlanillaUrl._codigo_planilla > 0 ? false : true);
    //pager.pagination({
    //    showPageList: true,
    //    buttons: [{
    //        iconCls: 'icon-print',
    //        text: 'Resúmen',
    //        disabled: buttonDisabled,
    //        handler: function () {
    //            fnReporteIndividual(1);
    //        },
    //    },
    //    {
    //        iconCls: 'icon-print',
    //        text: 'Detalle',
    //        disabled: buttonDisabled,
    //        handler: function () {
    //            fnReporteIndividual(2);
    //        }
    //    },
    //    {
    //        iconCls: 'icon-print',
    //        text: 'Liquidación',
    //        disabled: buttonDisabled,
    //        handler: function () {
    //            fnReporteIndividual(3);
    //        }
    //    },
    //    {
    //        iconCls: 'icon-excel',
    //        text: 'Exp. Reporte Liq.',
    //        disabled: botonExcel,
    //        handler: function () {
    //            GenerarExcel(ActionPlanillaUrl._codigo_planilla);
    //        }
    //    }]
    //});
    $(window).resize(function () {
        $('#dgv_planilla_pago_habilitado').datagrid('resize');
    });
}

function fnConfigurarGrillaPagoExcluido() {
    $('#dgv_planilla_pago_excluido').datagrid({
        data: null,
        rownumbers: true,
        url: ActionPlanillaUrl._GetPagoExcluidoJson,
        queryParams: {
            codigo_planilla: ActionPlanillaUrl._codigo_planilla
        },
        pagination: true,
        singleSelect: true,
        pageList: [20, 50, 100, 200, 400, 500],
        pageSize: 20,

        columns:
            [[
                { field: 'nombre_canal', title: 'Canal', width: 150, align: 'left' },
                { field: 'nombre_grupo_canal', title: 'Grupo', width: 150, align: 'left' },
                { field: 'apellidos_nombres', title: 'Vendedor', width: 330, align: 'left' },
                { field: 'nombre_empresa', title: 'Empresa', width: 80, halign: "center", align: "left" },
                { field: 'nro_contrato', title: 'Contrato', width: 95, halign: "center", align: "center" },
                { field: 'nombre_articulo', title: 'Artículo', width: 350, halign: "center", align: "left" },
                { field: 'nombre_moneda', title: 'Moneda', width: 80, halign: "center", align: "left" },
                {
                    field: 'monto_neto', title: 'Monto<br>Bono', width: 100, halign: "center", align: "right", formatter: function (value, row) {
                        return $.NumberFormat(value, 2);
                    }
                },
                { field: 'fecha_exclusion', title: 'Fecha', width: 100, align: 'center' },
                { field: 'usuario_exclusion', title: 'Usuario', width: 200, align: 'left' },
                { field: 'motivo_exclusion', title: 'Motivo', width: 500, align: 'left' },
            ]]
    });
    $(window).resize(function () {
        $('#dgv_planilla_pago_excluido').datagrid('resize');
    });
}


function RegistrarPlanilla() {

    if (!$("#fmr_registrar_planilla").form('enableValidation').form('validate'))
        return;

    var pEntidad = {
        codigo_tipo_planilla: $.trim($("#cmb_tipo_planilla").combobox('getValue')),
        codigo_canal: $.trim($("#cmb_canal_venta").combobox('getValue')),
        fecha_inicio: $.trim($("#dtp_fecha_inicio").datebox('getValue')),
        fecha_fin: $.trim($("#dtp_fecha_fin").datebox('getValue'))
    };
    /****************************************************************************/
 


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
                    if (data.v_resultado == 1) {

                        $.messager.alert('Operación exitosa', data.v_mensaje, 'info', function () {
                            fnConsultarPlanilla();
                            $("#div_registrar_planilla").dialog("close");
                            fnModificarPlanillaById(data.codigo_planilla);
                        });
                    }
                    else {
                        $.messager.alert('Apertura Planilla', data.v_mensaje, 'error');
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

function cerrar_planilla() {
    $("#div_registrar_planilla").dialog("close");

}

function fnReporteResumen(p_codigo_planilla, p_codigo_personal = null) {
    $(this).AbrirVentanaEmergente({
        parametro: "?p_codigo_planilla=" + p_codigo_planilla + "&p_codigo_tipo_planilla=" + ActionPlanillaUrl._codigo_tipo_planilla + "&p_codigo_personal=" + p_codigo_personal,
        div: 'div_reporte_general',
        title: "Reporte Planilla Bono - Resúmen",
        url: ActionPlanillaUrl._Reporte_Planilla
    });
}

function fnReporteDetalle(p_codigo_planilla, p_codigo_personal = null) {
    $(this).AbrirVentanaEmergente({
        parametro: "?p_codigo_planilla=" + p_codigo_planilla + "&p_codigo_tipo_planilla=" + ActionPlanillaUrl._codigo_tipo_planilla + "&p_codigo_personal=" + p_codigo_personal,
        div: 'div_reporte_general',
        title: "Reporte Planilla Bono - Detalle",
        url: ActionPlanillaUrl._Reporte_Liquidacion
    });
}

function fnReporteLiquidacion(p_codigo_planilla, p_codigo_personal = null) {
    $(this).AbrirVentanaEmergente({
        parametro: "?p_codigo_planilla=" + p_codigo_planilla + "&p_codigo_tipo_planilla=" + ActionPlanillaUrl._codigo_tipo_planilla + "&p_codigo_personal=" + p_codigo_personal,
        div: 'div_reporte_general',
        title: "Reporte Planilla Bono - Liquidación",
        url: ActionPlanillaUrl._Reporte_Liquidacion_individual
    });
}

function fnReporteLiquidacionResumen(p_codigo_planilla) {
    $(this).AbrirVentanaEmergente({
        parametro: "?p_codigo_planilla=" + p_codigo_planilla + "&p_codigo_tipo_planilla=" + ActionPlanillaUrl._codigo_tipo_planilla,
        div: 'div_reporte_general',
        title: "Reporte Planilla Bono - Resúmen Liquidación",
        url: ActionPlanillaUrl._Reporte_Liquidacion_Resumen
    });
}

function fnReporteIndividual(tipo) {
    var nombreOpcion = "Planilla Bono";
    var row = $('#dgv_planilla_pago_habilitado').datagrid('getSelected');
    var p_codigo_personal = null;

    if (tipo == 1) { nombreOpcion = "Reporte Resúmen"; }
    else if (tipo == 2) { nombreOpcion = "Reporte Detalle"; }
    else if (tipo == 3) { nombreOpcion = "Reporte Liquidación"; }

    if (!row) {
        $.messager.alert(nombreOpcion, "Para continuar con el proceso debe seleccionar un registro.", 'warning');
        return false;
    }

    p_codigo_personal = row.codigo_personal;
    p_codigo_planilla = ActionPlanillaUrl._codigo_planilla;

    if (tipo == 1) { fnReporteResumen(p_codigo_planilla, p_codigo_personal); }
    else if (tipo == 2) { fnReporteDetalle(p_codigo_planilla, p_codigo_personal); }
    else if (tipo == 3) { fnReporteLiquidacion(p_codigo_planilla, p_codigo_personal); }
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


function fnEnviarLiquidacion(p_codigo_planilla)
{
    $.messager.confirm('Confirmaci&oacute;n', '¿Est&aacute; seguro que desea enviar liquidación?', function (result) {
        if (result) {
            $.ajax({
                type: 'post',
                url: ActionPlanillaUrl._Enviar_Correo_Liquidacion,
                data: { p_codigo_planilla: p_codigo_planilla, p_nro_pllanilla: ActionPlanillaUrl._numero_planilla },
                async: false,
                cache: false,
                dataType: 'json',
                success: function (data) {
                    if (data.v_resultado == 1)
                    {
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

function ExcluirPago(codigoPlanillaBono) {
    var nombreOpcion = "Excluir Pago";
    var row = $('#dgv_planilla_pago_habilitado').datagrid('getSelected');

    if (!row) {
        $.messager.alert(nombreOpcion, "Para continuar con el proceso debe seleccionar un registro.", 'warning');
        return;
    }

    $.messager.observacion(nombreOpcion, 'Ingrese el motivo de exclusión:', function (win, data) {
        if (data) {

            var pEntidad = {
                codigo_planilla_bono: codigoPlanillaBono,
                codigo_empresa: row.codigo_empresa,
                codigo_personal: row.codigo_personal,
                codigo_articulo: row.codigo_articulo,
                nro_contrato: row.nro_contrato,
                excluido_motivo: data
            };

            $.ajax({
                type: 'post',
                url: ActionPlanillaUrl._Excluir_Pago,
                data: pEntidad,
                async: false,
                cache: false,
                dataType: 'json',
                success: function (data) {
                    if (data.v_resultado == 1) {
                        $.messager.alert('Operación exitosa', data.v_mensaje, 'info', function () {
                            ReloadGrillas();
                        });
                    }
                    else {
                        $.messager.alert('Error en registrar', data.v_mensaje, 'error');
                    }
                },
                error: function () {
                    $.messager.alert('Error', "Error en el servidor", 'error');
                }
            });
        }
    });
}

function ReloadGrillas() {
    $('#dgv_planilla_pago_habilitado').datagrid("clearSelections");
    $('#dgv_planilla_pago_habilitado').datagrid("reload");
    $('#dgv_planilla_pago_excluido').datagrid("clearSelections");
    $('#dgv_planilla_pago_excluido').datagrid("reload");
}

    /**/
function GenerarExcel(codigo_planilla) {

    //var rows = $("#dgv_planilla_pago_habilitado").datagrid("getData");
    //if (rows.total < 1) {
    //    $.messager.alert("Exportar", "No existen registros para exportar.", "warning");
    //    return;
    //}

    if (ActionPlanillaUrl._codigo_estado_planilla != '2') {
        $.messager.alert("Exportar", "Sólo para planillas cerradas.", "warning");
        return false;
    }

    if (ActionPlanillaUrl._codigo_tipo_planilla == "1") {
        $.messager.alert("Exportar", "Sólo para planillas de supervisores.", "warning");
        return;
    }

    $.ajax({
        type: 'post',
        url: ActionPlanillaUrl.SetDataExcel,
        data: JSON.stringify({ p_codigo_planilla: codigo_planilla }),
        async: true,
        cache: false,
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            var url = ActionPlanillaUrl.GenerarExcel + "?p_codigo_planilla=" + data.p_codigo_planilla;
            window.location.href = url;
        },
        error: function (data) {
            $.messager.alert('Error', "Error en el servidor", 'error');
        }
    });
}
