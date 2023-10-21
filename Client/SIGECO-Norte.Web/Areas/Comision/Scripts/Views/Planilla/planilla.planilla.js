var ActionPlanillaUrl = {};
var JsonTipoVenta = {};
var CodigoPersonalFiltro = 0;

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
                fnConfigurarGrillaPagoExclusiones();
                fnConfigurarGrillaPagoDescuento();
                fnConfigurarGrillaPagoInclusion();
                ValidarDescuentos();
                fnConfigurarGrillaComisionManual();
            }
        })
})(project);


function fnInicializarPlanilla() {
    $('#cmbg_personal_planilla').combogrid({
        panelWidth: 500,
        idField: 'codigo_personal',
        textField: 'apellidos_nombres_vendedor',
        //mode: 'remote',
        fitColumns: true,
        rownumbers: true,
        url: ActionPlanillaUrl._GetPersonalPlanillaAllJson,
        queryParams: {
            codigo_planilla: ActionPlanillaUrl._codigo_planilla
        },
        columns: [[
            { field: 'codigo_persona', hidden: true },
            { field: 'codigo_equivalencia', title: 'Código', width: 100, align:'center' },
            { field: 'apellidos_nombres_vendedor', title: 'Vendedor', align: 'left', width: 300 }
        ]]
    });
    
    JsonTipoVenta = $('.content').get_json_combobox({
        url: ActionPlanillaUrl._GetFilterTipoVentaJson
    });    

    $('.content').combobox_sigees({
        id: '#cmb_tipo_planilla',
        url: ActionPlanillaUrl._GetTipoPlanillaJson
    });

    $('.content').combobox_sigees({
        id: "#cmb_regla_tipo_planilla",
        url: ActionPlanillaUrl._GetReglaTipoPlanillaJson
    });

    $('#dtp_fecha_inicio').DeshabilitarFechaMayorHoy();
    $('#dtp_fecha_fin').DeshabilitarFechaMayorHoy();

    $('#dtp_fecha_inicio').formatteDate();
    $('#dtp_fecha_fin').formatteDate();

    $('.content').ResizeModal({
        widthMax: '90%',
        widthMin: '80%',
        div: 'div_registrar_planilla'
    });
    
    $(window).resize(function () {

      
        $('#dgv_planilla_pago_habilitado').datagrid('resize', {
            height: '100%'
        });
        $('#dgv_planilla_pago_exclusiones').datagrid('resize', {
            height: '100%'
        });
        
    });
}

function fnReloadGrillaPagoDescuento() {
    $('#dgv_planilla_descuento').datagrid("reload");
}


function fnConfigurarGrillaPagoDescuento() {
    $('#dgv_planilla_descuento').datagrid({
        fitColumns: false,
        idField: 'codigo_descuento',
        //height: '300',
        url: ActionPlanillaUrl._GetPagosDescuentoJson,
        queryParams: {
            p_codigo_planilla: ActionPlanillaUrl._codigo_planilla
        },
        toolbar: "#toolbar_pago_descuento",
        pagination: true,
        singleSelect: true,
        rownumbers: true,
        pageList: [20, 50, 100, 200, 400, 500],
        pageSize: 20,
        nowrap: false,
        columns:
        [[
            { field: 'codigo_descuento', title: 'Codigo', hidden: 'true' },
            { field: 'indica_estado_registro', title: 'Codigo', hidden: 'true' },            
            { field: 'nombre_empresa', title: 'Empresa', width: 100, align: 'left', halign: 'center' },
            { field: 'nombre_grupo', title: 'Grupo', width: 140, align: 'left', halign: 'center' },
            { field: 'apellidos_nombres', title: 'Vendedor', width: 250, align: 'left', halign: 'center' },
            {
                field: 'monto', title: 'Importe<br>Descuento', width: 120, align: 'right', halign: 'center', formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                }
            },
            { field: 'motivo', title: 'Motivo', width: 500, align: 'left', halign: 'center' },
            { field: 'nombre_estado_registro', title: 'Estado', width: 110, align: 'center', halign: 'center' }
        ]]
    });
    //{ field: 'motivo', title: 'Motivo', width: 500, align: 'left', halign: 'center', formatter: function (value, row) { return '<span title=\"' + value + '\" class=\"easyui-tooltip\">' + value + '</span>' } },
    $('#dgv_planilla_descuento').datagrid('enableFilter');

    $(window).resize(function () {
        $('#dgv_planilla_descuento').datagrid('resize');
    });
}

function fnConfigurarGrillaPagoExclusiones() {

    $('#dgv_planilla_pago_exclusiones').datagrid({
        //fitColumns: true,
        idField: 'codigo_detalle_cronograma',
        url: ActionPlanillaUrl._GetPagosExcluidoJson,
        queryParams: {
            codigo_planilla: ActionPlanillaUrl._codigo_planilla,
            codigo_tipo_busqueda: 2
        },
        //height: '300',
        pagination: true,
        singleSelect: true,
        rownumbers: true,
        autoRowHeight: false,
        pageList: [20, 50, 100, 200, 400, 500],
        pageSize: 20,
        columns:
        [[
            { field: 'codigo_detalle_cronograma', title: 'Codigo', hidden: 'true' },
            
        { field: 'nombre_estado_exclusion', title: 'Estado <br> Exclusión', width: 120, align: 'left' },
            { field: 'nombre_grupo_canal', title: 'Grupo', width: 120, align: 'left' },
            { field: 'apellidos_nombres', title: 'Vendedor', width: 180, align: 'left' },
            { field: 'nombre_empresa', title: 'Empresa', width: 100, align: 'left' },
            { field: 'nro_contrato', title: 'Nro. <br>Contrato', width: 100, align: 'center' },
            { field: 'nombre_articulo', title: 'Articulo', width: 180, align: 'left' },
            { field: 'nombre_tipo_venta', title: 'Tipo Venta', width: 180, align: 'left' },
            { field: 'nombre_tipo_pago', title: 'Tipo Pago', width: 180, align: 'left' },
            { field: 'str_fecha_pago', title: 'Fecha <br> Habilitada', width: 120, align: 'center' },
            { field: 'nro_cuota', title: 'Nro.<br> Cuota', width: 80, align: 'left' },
            {
                field: 'monto_bruto', title: 'Importe <br> Comisión', width: 120, halign: "center", align: "right", formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                }
            },
            {
                field: 'igv', title: 'IGV', width: 100, halign: "center", align: "right", formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                }
            },
            {
                field: 'monto_neto', title: 'Comisión<br> Total', width: 100, halign: "center", align: "right", formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                }
            },
            { field: 'usuario_exclusion', title: 'Usuario Exclusión', width: 150, align: 'left' },
            { field: 'motivo_exclusion', title: 'Motivo Exclusión', width: 150, align: 'left' },
            { field: 'usuario_habilita_exclusion', title: 'Usuario Habilitación', width: 150, align: 'left' },
            { field: 'motivo_habilitacion_exclusion', title: 'Motivo Habilitación', width: 150, align: 'left' },
            { field: 'observacion', title: 'Observación', width: 250, align: 'left', hidden: 'true' }
                                    
                        
        ]],
        onClickRow: function (index, row) {

        }
    });
    $('#dgv_planilla_pago_exclusiones').datagrid('enableFilter');
}

function fnConfigurarGrillaPagoHabilitado() {
    $('#dgv_planilla_pago_habilitado').datagrid({
        //fitColumns: true,
        //height: '300',
        idField: 'codigo_detalle_cronograma',
        data: null,
        rownumbers: true,        
        toolbar: '#toolbar_pago_habilitado',
        url: ActionPlanillaUrl._GetPagoHabilitadoJson,
        queryParams: {
            codigo_planilla: ActionPlanillaUrl._codigo_planilla,
            codigo_tipo_busqueda: 1
        },
        pagination: true,
        singleSelect: true,
        
        pageList: [20, 50, 100, 200, 400, 500],
        pageSize: 20,
        columns:
        [[
            { field: 'codigo_detalle_planilla', title: 'Codigo', hidden: 'false' },
            { field: 'indica_registro_manual_comision', title: 'Es Registro Manual de Comision', hidden: 'false' },
            
            { field: 'codigo_empresa', title: 'Codigo Empresa', hidden: 'false' },
            { field: 'codigo_cronograma', title: 'Codigo', hidden: 'false' },
            { field: 'codigo_estado_cuota', hidden: 'false' },

            { field: "mostrar_es_transferencia", title: "", width: "10", align: "center", halign: "center", styler: cellStylerEsTransferencia },
            { field: 'nombre_grupo_canal', title: 'Grupo', width: 150, align: 'left' },
            { field: 'apellidos_nombres', title: 'Vendedor', width: 200, align: 'left' },
            { field: 'nombre_empresa', title: 'Empresa', width: 70, halign: "center", align: "left" },
            { field: 'nro_contrato', title: 'Nro. <br>Contrato', width: 100, align: 'center' },
            { field: 'nombre_articulo', title: 'Articulo', width: 200, align: 'left' },
            { field: 'nombre_tipo_venta', title: 'Tipo Venta', width: 170, align: 'left' },
            { field: 'codigo_tipo_venta', hidden: 'false' },
            { field: 'nombre_tipo_pago', title: 'Tipo Pago', width: 90, align: 'left' },
            { field: 'str_fecha_pago', title: 'Fecha <br> Habilitada', width: 120, align: 'center' },
            { field: 'nro_cuota', title: 'Nro.<br> Cuota', width: 70, align: 'center' },

            {
                field: 'monto_bruto', title: 'Importe <br> Comisión', halign: "center", align: "right", width: 120, formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                }
            },
            {
                field: 'igv', title: 'IGV', width: 120, halign: "center", align: "right", formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                }
            },
            {
                field: 'monto_neto', title: 'Comisión<br> Total', width: 120, halign: "center", align: "right", formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                }
            },
            { field: 'nombre_estado_cuota', title: 'Estado<br> Cuota', width: 115, align: 'left' },
            { field: 'nombre_estado_registro', title: 'Estado<br>Registro', width: 90, align: 'center', halign: 'center' }
        ]],
        onClickRow: function (index, row) {

        }
    });
    
    $('#dgv_planilla_pago_habilitado').datagrid('enableFilter', [{
        field: 'nombre_tipo_venta',
        type: 'combobox',
        options: {
            panelHeight: 'auto',
            data: JsonTipoVenta,
            onChange: function (value) {                
                if (value == '') {
                    $('#dgv_planilla_pago_habilitado').datagrid('removeFilterRule', 'codigo_tipo_venta');
                } else {
                    $('#dgv_planilla_pago_habilitado').datagrid('addFilterRule', {
                        field: 'codigo_tipo_venta',
                        op: 'equal',
                        value: value
                    });
                }
                $('#dgv_planilla_pago_habilitado').datagrid('doFilter');
            }
        }
    }]);

    let botonExcel = true;

    if (ActionPlanillaUrl._codigo_estado_planilla == '2' && $("#cmb_tipo_planilla").val() == "2") {
        botonExcel = false;
    }
    /*
    var pager = $('#dgv_planilla_pago_habilitado').datagrid('getPager');    // get the pager of datagrid
    pager.pagination({
        showPageList: true,
        buttons: [{
            iconCls: 'icon-print',
            text: 'Planilla',
            disabled: true,
            handler: function () {
                fnReportePlanillaCustom();
            },
        },
        {
            iconCls: 'icon-print',
            text: 'Liquidación',
            disabled: true,
            handler: function () {
                fnReporteLiquidacionCustom();
            }
        },
        {
            iconCls: 'icon-excel',
            text: 'Exp. Reporte Liq.',
            disabled: botonExcel,
            handler: function () {
                GenerarExcel(ActionPlanillaUrl._codigo_planilla);
            }
        }]
    });*/

    $(window).resize(function () {
        $('#dgv_planilla_pago_habilitado').datagrid('resize');
    });
}

//MYJ - 20171124
function fnConfigurarGrillaPagoInclusion() {

    $("#btnIncluirProcesar").on('click', function () { fnIncluirProcesar(); });

    $('#dgv_planilla_pago_inclusion').datagrid({
        //fitColumns: true,
        //height: '300',
        idField: 'codigo_detalle_cronograma',
        data: null,
        rownumbers: true,
        toolbar: '#toolbar_pago_inclusion',
        url: ActionPlanillaUrl._Incluir_Listar,
        queryParams: {
            nro_contrato: '',
            codigo_planilla: -1
        },
        pagination: true,
        singleSelect: false,

        pageList: [10, 20, 50, 100, 200, 400, 500],
        pageSize: 10,
        columns:
        [[
            { field: 'codigo_detalle_planilla', title: 'Codigo', hidden: 'false' },
            { field: 'codigo_detalle_cronograma', title: 'Codigo', hidden: 'false' },
            { field: 'indica_registro_manual_comision', title: 'Es Registro Manual de Comision', hidden: 'false' },

            { field: 'codigo_empresa', title: 'Codigo Empresa', hidden: 'false' },
            { field: 'codigo_cronograma', title: 'Codigo', hidden: 'false' },
            { field: 'codigo_estado_cuota', hidden: 'false' },

            { field: 'nombre_grupo_canal', title: 'Grupo', width: 120, align: 'left' },
            { field: 'apellidos_nombres', title: 'Vendedor', width: 180, align: 'left' },
            { field: 'nombre_empresa', title: 'Empresa', width: 120, halign: "center", align: "left" },
            { field: 'nro_contrato', title: 'Nro. <br>Contrato', width: 100, align: 'center' },
            { field: 'nombre_articulo', title: 'Articulo', width: 180, align: 'left' },
            { field: 'nombre_tipo_venta', title: 'Tipo Venta', width: 180, align: 'left' },
            { field: 'codigo_tipo_venta', title: ' codigo_tipo_venta Tipo Venta', width: 180, align: 'left', hidden: 'false' },
            { field: 'nombre_tipo_pago', title: 'Tipo Pago', width: 180, align: 'left' },
            { field: 'str_fecha_pago', title: 'Fecha <br> Habilitada', width: 120, align: 'center' },
            { field: 'nro_cuota', title: 'Nro.<br> Cuota', width: 80, align: 'center' },

            {
                field: 'monto_bruto', title: 'Importe <br> Comisión', halign: "center", align: "right", width: 120, formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                }
            },
            {
                field: 'igv', title: 'IGV', width: 120, halign: "center", align: "right", formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                }
            },
            {
                field: 'monto_neto', title: 'Comisión<br> Total', width: 120, halign: "center", align: "right", formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                }
            },
            { field: 'nombre_estado_cuota', title: 'Estado<br> Cuota', width: 100, align: 'left' },
            { field: 'nombre_estado_registro', title: 'Estado<br>Registro', width: 100, align: 'center', halign: 'center' }
        ]],
        onClickRow: function (index, row) {

        },
        onLoadSuccess: function () {
            if ($("#dgv_planilla_pago_inclusion").datagrid('options').queryParams.codigo_planilla != '-1') {
                if ($("#dgv_planilla_pago_inclusion").datagrid("getData").total == 0) {
                    $("#nro_contrato_incluir").textbox({ editable: true });
                    $('#btnIncluirProcesar').linkbutton('disable');
                    $('#btnIncluirListar').linkbutton('enable');
                    $.messager.alert("Inclusión", "No se encontraron comisiones habilitadas.", "warning");
                }
                else {
                    $('#nro_contrato_incluir').textbox({ editable: false });
                    $('#btnIncluirProcesar').linkbutton('enable');
                    $('#btnIncluirListar').linkbutton('disable');
                }
            }
            else {
                $('#btnIncluirProcesar').linkbutton('disable');
                $('#btnIncluirListar').linkbutton('enable');
            }
        }
    });

    $(window).resize(function () {
        $('#dgv_planilla_pago_inclusion').datagrid('resize');
    });
}

function fnConfigurarGrillaComisionManual() {
    $('#dgv_planilla_comision_manual').datagrid({
        //fitColumns: true,
        //height: '300',
        idField: 'codigo_detalle_cronograma',
        data: null,
        rownumbers: true,
        url: ActionPlanillaUrl._GetPagosComisionManualJson,
        queryParams: {
            codigo_planilla: ActionPlanillaUrl._codigo_planilla,
            codigo_tipo_busqueda: 1
        },
        pagination: true,
        singleSelect: true,

        pageList: [20, 50, 100, 200, 400, 500],
        pageSize: 20,
        columns:
            [[
                { field: 'codigo_detalle_planilla', title: 'Codigo', hidden: 'false' },
                { field: 'indica_registro_manual_comision', title: 'Es Registro Manual de Comision', hidden: 'false' },

                { field: 'codigo_empresa', title: 'Codigo Empresa', hidden: 'false' },
                { field: 'codigo_cronograma', title: 'Codigo', hidden: 'false' },
                { field: 'codigo_estado_cuota', hidden: 'false' },

                { field: 'usuario_comision_manual', title: 'Usuario<br>Registro CM', width: 90, align: 'left' },
                { field: 'nombre_grupo_canal', title: 'Grupo', width: 200, align: 'left' },
                { field: 'apellidos_nombres', title: 'Vendedor Contrato', width: 230, align: 'left' },
                { field: 'personal_comision_manual', title: 'Vendedor Registro CM', width: 230, align: 'left' },
                { field: 'nombre_empresa', title: 'Empresa', width: 70, halign: "center", align: "left" },
                { field: 'nro_contrato', title: 'Nro. <br>Contrato', width: 100, align: 'center' },
                { field: 'nombre_articulo', title: 'Articulo', width: 200, align: 'left' },
                { field: 'nombre_tipo_venta', title: 'Tipo Venta', width: 170, align: 'left' },
                { field: 'codigo_tipo_venta', hidden: 'false' },
                { field: 'nombre_tipo_pago', title: 'Tipo Pago', width: 90, align: 'left' },
                { field: 'str_fecha_pago', title: 'Fecha <br> Habilitada', width: 120, align: 'center' },
                { field: 'nro_cuota', title: 'Nro.<br> Cuota', width: 70, align: 'center' },

                {
                    field: 'monto_bruto', title: 'Importe <br> Comisión', halign: "center", align: "right", width: 120, formatter: function (value, row) {
                        return $.NumberFormat(value, 2);
                    }
                },
                {
                    field: 'igv', title: 'IGV', width: 120, halign: "center", align: "right", formatter: function (value, row) {
                        return $.NumberFormat(value, 2);
                    }
                },
                {
                    field: 'monto_neto', title: 'Comisión<br> Total', width: 120, halign: "center", align: "right", formatter: function (value, row) {
                        return $.NumberFormat(value, 2);
                    }
                },
                { field: 'nombre_estado_cuota', title: 'Estado<br> Cuota', width: 115, align: 'left' },
                { field: 'nombre_estado_registro', title: 'Estado<br>Registro', width: 90, align: 'center', halign: 'center' }
            ]],
        onClickRow: function (index, row) {

        }
    });

    $(window).resize(function () {
        $('#dgv_planilla_comision_manual').datagrid('resize');
    });
}


function RegistrarPlanilla() {
    if (!$("#fmr_registrar_planilla").form('enableValidation').form('validate'))
        return;

    var pEntidad = {
        codigo_regla_tipo_planilla: $.trim($("#cmb_regla_tipo_planilla").combobox('getValue')),
        fecha_inicio: $.trim($("#dtp_fecha_inicio").datebox('getValue')),
        fecha_fin: $.trim($("#dtp_fecha_fin").datebox('getValue'))
    };

    $.messager.confirm('Confirmaci&oacute;n', '¿Est&aacute; seguro que desea aperturar la planilla?', function (result) {
        if (result) {
            $.ajax({
                type: 'post',
                url: ActionPlanillaUrl._Registrar,
                data: JSON.stringify({v_planilla:pEntidad}),
                async: true,
                cache: false,
                dataType: 'json',
                contentType: 'application/json',
                success: function (data) {
                    //console.log(data);
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
                error: function (data) {
                    
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
                        $.messager.alert('Error en la generación de planilla', data.v_mensaje, 'error');
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


function generar_comision_excel() {


}
function fnNuevoAnalisisContrato(p_codigo_planilla) {
    var nombreOpcion = "Análisis Comisión";
    var row = $('#dgv_planilla_pago_habilitado').datagrid('getSelected');
    
    if (!row) {
        $.messager.alert(nombreOpcion, "Para continuar con el proceso, seleccionar un registro.", 'warning');
        return;
    }  

    $(this).AbrirVentanaEmergente({
        parametro: "?codigo_empresa=" + row.codigo_empresa + "&nro_contrato=" + row.nro_contrato,
        div: 'div_registrar_contrato',
        title: "Análisis de Comisión",
        url: ActionPlanillaUrl._Analisis_Contrato
    });
}

function excluir_planilla(p_codigo_planilla) {
    var nombreOpcion = "Exclusion de Pago";
    var row = $('#dgv_planilla_pago_habilitado').datagrid('getSelected');

    if (!row) {
        $.messager.alert(nombreOpcion, "Para continuar con el proceso debe seleccionar un registro.", 'warning');
        return;
    }

    if (row.indica_registro_manual_comision == "1") {
        $.messager.alert(nombreOpcion, "No se puede excluir un pago de tipo registro manual de comisión.", 'warning');
        return;
    }

    if (row.codigo_estado_cuota != "2") {
        $.messager.alert(nombreOpcion, "Solo se puede excluir cuotas en proceso de pago.", 'warning');
        return;
    }

    $(this).AbrirVentanaEmergente({
        parametro: "?codigo_detalle_planilla=" + row.codigo_detalle_planilla + "&codigo_planilla=" + row.codigo_planilla,
        div: 'div_exclusion_planilla',
        title: "Exclusión",
        url: ActionPlanillaUrl._Exclusion
    });
}

function fnNuevoDescuento(p_codigo_planilla) {
    $.messager.confirm('Confirmaci&oacute;n', '¿Est&aacute; seguro que desea generar descuentos?', function (result) {
        if (result) {
            GenerarDescuento(p_codigo_planilla);
        }
    });
}

function GenerarDescuento(p_codigo_planilla) {
    $.ajax({
        type: 'post',
        url: ActionPlanillaUrl.GenerarDescuento,
        data: { codigo_planilla: p_codigo_planilla },
        async: true,
        cache: false,
        dataType: 'json',
        success: function (data) {
            if (data.Msg) {
                if (data.Msg != 'Success') {
                    $.messager.alert('Error', data.Msg, 'error');
                }
                else {
                    if (parseInt(data.Cantidad, 10) > 0) {
                        $.messager.alert('Descuento', 'Se generaron ' + data.Cantidad + ' descuento(s).', 'warning');
                    }
                    else {
                        $.messager.alert('Descuento', 'No se generaron descuentos.', 'warning');
                    }
                    fnReloadGrillaPagoDescuento();
                }
            }
            else {
                project.AlertErrorMessage('Error', 'Error de procesamiento');
            }
        },
        error: function () {
            $.messager.alert('Error', "Error en el servidor", 'error');
        }
    });
}

function fnDesactivarDescuento() {
    var rows = $("#dgv_planilla_descuento").datagrid("getSelections");
    if (rows.length <= 0) {
        $.messager.alert("Desactivar descuento planilla", "Seleccione un registro para desactivar.", "warning");
        return;
    }

    var row = $("#dgv_planilla_descuento").datagrid("getSelected");
    if (row.indica_estado_registro!=1)
    {
        $.messager.alert("Desactivar descuento planilla", "El registro seleccionado se encuentra desactivado.", "warning");
        return;
    }

    $.messager.confirm('Confirmaci&oacute;n', '¿Est&aacute; seguro que desea desactivar el descuento?', function (result) {
        if (result) {
            $.ajax({
                type: 'post',
                url: ActionPlanillaUrl._Descuento_Desactivar,
                data: { p_codigo_descuento: row.codigo_descuento },
                async: true,
                cache: false,
                dataType: 'json',
                success: function (data) {
                    if (data.v_resultado == 1) {
                        $.messager.alert('Operación exitosa', data.v_mensaje, 'info', function () {
                            fnReloadGrillaPagoDescuento();
                        });
                    }
                    else {
                        $.messager.alert('Error en la operación', data.v_mensaje, 'error');
                    }
                },
                error: function () {
                    $.messager.alert('Error', "Error en el servidor", 'error');
                }
            });
        }
    });
}

function fnFiltrarPlanilla() {
    var codigo_personal = $("#cmbg_personal_planilla").combogrid("getValue");
    if (!codigo_personal) {
        $.messager.alert("Buscar", "No ha hecho una selección.", "warning");
        return false;
    }

    var item = {
        codigo_planilla: ActionPlanillaUrl._codigo_planilla,
        codigo_tipo_venta:null,// $("#cmbg_tipo_venta_planilla").combobox("getValue"),
        codigo_personal: codigo_personal
        //nro_documento: $.trim($("#txt_numero_documento").textbox('getText')),
        //apellido_paterno: $.trim($("#txt_apellido_paterno").textbox('getText')),
        //apellido_materno: $.trim($("#txt_apellido_materno").textbox('getText')),
        //nombre: $.trim($("#txt_nombre_persona").textbox('getText'))
    };
    $('#dgv_planilla_pago_habilitado').datagrid('reload', item);
    CodigoPersonalFiltro = $("#cmbg_personal_planilla").combogrid("getValue");
    //HabilitarBotonesCustomizados(false);
}

function fnLimpiarFiltrarPlanilla() {
    $("#cmbg_tipo_venta_planilla").combobox("setValue", null);
    $("#cmbg_personal_planilla").combogrid("setValue", null);
    var item = {
        codigo_planilla: ActionPlanillaUrl._codigo_planilla
    };
    $('#dgv_planilla_pago_habilitado').datagrid('reload', item);
    CodigoPersonalFiltro = 0;
    //HabilitarBotonesCustomizados(true);
}

function HabilitarBotonesCustomizados(valor) {
    $('#dgv_planilla_pago_habilitado').datagrid('getPager').pagination('options').buttons[1].disabled = valor;
    $('#dgv_planilla_pago_habilitado').datagrid('getPager').pagination('options').buttons[0].disabled = valor;
}

function fnReportePlanilla(p_codigo_planilla) {
    $(this).AbrirVentanaEmergente({
        parametro: "?p_codigo_planilla=" + p_codigo_planilla + "&p_codigo_personal=0",
        div: 'div_reporte_general',
        title: "Reporte Planilla",
        url: ActionPlanillaUrl._Reporte
    });
}

function fnReporteLiquidacion(p_codigo_planilla) {
    $(this).AbrirVentanaEmergente({
        parametro: "?p_codigo_planilla=" + p_codigo_planilla + "&p_codigo_personal=",
        div: 'div_reporte_general',
        title: "Reporte Liquidación",
        url: ActionPlanillaUrl._Reporte_Liquidacion
    });
}

function fnReporteResumenLiquidacion(p_codigo_planilla) {
    $(this).AbrirVentanaEmergente({
        parametro: "?p_codigo_planilla=" + p_codigo_planilla,
        div: 'div_reporte_general',
        title: "Reporte Resúmen Liquidación",
        url: ActionPlanillaUrl._Reporte_Resumen_Liquidacion
    });
}

function fnReportePlanillaCustom() {
    //var p_codigo_personal = $("#cmbg_personal_planilla").combogrid("getValue")

    if (CodigoPersonalFiltro == 0)
    {
        $.messager.alert('Planilla', "Para continuar debe buscar un vendedor.", 'warning');
        return false;
    }

    $(this).AbrirVentanaEmergente({
        parametro: "?p_codigo_planilla=" + ActionPlanillaUrl._codigo_planilla + "&p_codigo_personal=" + CodigoPersonalFiltro,
        div: 'div_reporte_general',
        title: "Reporte Planilla",
        url: ActionPlanillaUrl._Reporte
    });
}

function fnReporteLiquidacionCustom() {
    //var p_codigo_personal = $("#cmbg_personal_planilla").combogrid("getValue")

    if (CodigoPersonalFiltro == 0) {
        $.messager.alert('Planilla', "Para continuar debe buscar un vendedor.", 'warning');
        return false;
    }

    $(this).AbrirVentanaEmergente({
        parametro: "?p_codigo_planilla=" + ActionPlanillaUrl._codigo_planilla + "&p_codigo_personal=" + CodigoPersonalFiltro,
        div: 'div_reporte_general',
        title: "Reporte Liquidación",
        url: ActionPlanillaUrl._Reporte_Liquidacion
    });
}

function AnularPlanilla(p_codigo_planilla) {
    $.messager.confirm('Confirmaci&oacute;n', '¿Est&aacute; seguro que desea anular la planilla?', function (result) {
        if (result) {
            $.ajax({
                type: 'post',
                url: ActionPlanillaUrl._Anular,
                data: { p_codigo_planilla: p_codigo_planilla },
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
    $.messager.confirm('Confirmaci&oacute;n', '¿Est&aacute; seguro que desea enviar liquidaciones?', function (result) {
        if (result) {
            $.ajax({
                type: 'post',
                url: ActionPlanillaUrl._Enviar_Correo_Liquidacion,
                data: { p_codigo_planilla: p_codigo_planilla, p_nro_pllanilla: ActionPlanillaUrl._numero_planilla },
                async: true,
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

function fnGenerarTxtPlanillaCerrado(p_codigo_planilla) {  
    $.messager.confirm('Confirmaci&oacute;n', '¿Est&aacute; seguro que desea generar el archivo txt?', function (result) {
        if (result) {
            var url = ActionPlanillaUrl.GenerarTxt + "?id=" + p_codigo_planilla;
            window.open(url, '_blank');
        }
    });
 }

//MYJ - 20171124
function fnIncluirListar() {
    var nro_contrato = $.trim($("#nro_contrato_incluir").val());

    if (nro_contrato.length == 0)
    {
        $.messager.alert('Inclusión', 'No ha ingresado un nro. de contrato.', 'warning');
        return false;
    }

    var ceros = '0';
    if (nro_contrato.length > 0 && nro_contrato.length < 10) {
        nro_contrato = ceros.repeat(10 - nro_contrato.length) + nro_contrato;
    }

    $("#nro_contrato_incluir").textbox({ value: nro_contrato });
    $("#nro_contrato_incluir").val(nro_contrato);

    var query = {
        nro_contrato: nro_contrato,
        codigo_planilla: ActionPlanillaUrl._codigo_planilla
    };

    $('#dgv_planilla_pago_inclusion').datagrid('clearSelections');
    $('#dgv_planilla_pago_inclusion').datagrid('reload', query);
}

//MYJ - 20171124
function fnIncluirLimpiar() {
    $("#nro_contrato_incluir").textbox({ editable: true, value: '' });

    var query = {
        nro_contrato: '',
        codigo_planilla: -1
    };

    $('#dgv_planilla_pago_inclusion').datagrid('clearSelections');
    $('#dgv_planilla_pago_inclusion').datagrid('reload', query);
}

//MYJ - 20171124
function fnIncluirProcesar() {
    var nro_contrato = $.trim($("#nro_contrato_incluir").val());

    if ($('#btnIncluirProcesar').linkbutton('options').disabled) { return false };

    if (nro_contrato.length == 0) {
        $.messager.alert('Inclusión', 'No ha ingresado un nro. de contrato.', 'warning');
        return false;
    }

    var rows = $('#dgv_planilla_pago_inclusion').datagrid('getSelections');

    if (rows.length == 0)
    {
        $.messager.alert('Inclusión', 'No ha seleccionado alguna comisión a incluir.', 'warning');
        return false;
    }

    $.messager.confirm('Confirmaci&oacute;n', '¿Est&aacute; seguro que desea incluir ' + rows.length + ' comision(es)?', function (result) {
        if (result) {
            var lista = [];
            $.each(rows, function (index, data) {
                var record = {
                    codigo_detalle_cronograma: data.codigo_detalle_cronograma
                };
                lista.push(record);
            });

            $.ajax({
                type: 'post',
                url: ActionPlanillaUrl._Incluir_Procesar,
                data: JSON.stringify({ codigo_planilla: ActionPlanillaUrl._codigo_planilla, lst_inclusion: lista }),
                async: true,
                cache: false,
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    if (data.v_resultado == 1) {
                        fnLimpiarFiltrarPlanilla();
                        fnIncluirLimpiar();
                        $.messager.alert('Inclusión', data.v_mensaje, 'info');
                    }
                    else {
                        $.messager.alert('Inclusión', data.v_mensaje, 'error');
                    }
                    $('#dgv_planilla_pago_inclusion').datagrid('clearSelections');
                },
                error: function () {
                    $.messager.alert('Error', "Error en el servidor", 'error');
                }
            });
        }
    });
}

function ValidarDescuentos() {
    if (ActionPlanillaUrl._codigo_estado_planilla != '1'){
        return false;
    }

    $.ajax({
        type: 'post',
        url: ActionPlanillaUrl.ValidarDescuentos,
        data: JSON.stringify({ codigo_planilla: ActionPlanillaUrl._codigo_planilla }),
        async: true,
        cache: false,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data.Msg) {
                if (data.Msg != 'Success') {
                    $.messager.alert('Error', data.Msg, 'error');
                }
                else {
                    if (parseInt(data.Cantidad, 10) > 0)
                    {
                        $.messager.confirm('Confirmaci&oacute;n', 'Existen ' + data.Cantidad + ' descuento(s) por aplicar a esta planilla.<br>¿Desea generar los descuentos?', function (result) {
                            if (result) {
                                GenerarDescuento(ActionPlanillaUrl._codigo_planilla);
                            }
                        });

                    }
                }
            }
            else {
                project.AlertErrorMessage('Error', 'Error de procesamiento');
            }
        },
        error: function () {
            $.messager.alert('Error', "Error en el servidor", 'error');
        }
    });
}

function cellStylerEsTransferencia(value, row, index) {
    if (row.es_transferencia == 1) {
        return 'background-color:#25a01c;';
    }
    else {
        return 'background-color:#ffffff;';
    }
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

    if ($("#cmb_tipo_planilla").val() == "1") {
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
