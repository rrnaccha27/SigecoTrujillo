var ActionContratoUrl = {};

; (function (app) {
    //===========================================================================================
    var current = app.contrato = {};
    //===========================================================================================

    jQuery.extend(app.contrato,
        {
            Initialize: function (actionUrls) {
                jQuery.extend(ActionContratoUrl, actionUrls);
                fnConfigurarGrillaContratoArticulo();
                fnConfigurarGrillaContratoDetallePago();
                fnConfigurarGrillaCronogramaPagos();
                fnInicializarContrato();
            }
        })
})(project);


function fnInicializarContrato() {


    $('.content').combobox_sigees({
        parametro: "?p_numero_contrato=" + ActionContratoUrl._numero_contrato,
        id: '#cmb_empresa_sigeco',
        url: ActionContratoUrl._GetEmpresaByContratoJson
    });

    $("#cmb_empresa_sigeco").combobox({
        onChange: function (newValue, oldValue) {
            fnBuscarContrato(newValue);

        }
    });

    $('#dgv_contrato_articulo').datagrid('resize', {
        height: '100%'
    });
    $('#dgv_contrato_detalle_pago').datagrid('resize', {
        height: '100%'
    });

    var p = $('#pnlDatosGenerales').panel({
        onCollapse: function () {
            ModificarTituloPanel('pnlDatosGenerales', 'Datos Generales', false);
        },
        onExpand: function () {
            ModificarTituloPanel('pnlDatosGenerales', 'Datos Generales', true);
        },
    });

    var p = $('#pnlDatosVenta').panel({
        onCollapse: function () {
            ModificarTituloPanel('pnlDatosVenta', 'Datos Venta', false);
        },
        onExpand: function () {
            ModificarTituloPanel('pnlDatosVenta', 'Datos Venta', true);
        },
    });
}

function ModificarTituloPanel(panel, titulo, defecto) {
    var p = $('#' + panel);
    var tituloModificado = titulo;
    if (defecto) {
        p.panel('setTitle', titulo);
    }
    else {
        var empresa = $("#cmb_empresa_sigeco").combobox("getText");
        var contrato = $("#txt_nro_contrato").val();

        if (panel == 'pnlDatosGenerales') {
            if (!(!empresa && !contrato)) {
                tituloModificado = tituloModificado + ': ' + empresa + ' - ' + contrato;
            }
        }
        else if (panel == 'pnlDatosVenta') {
            if (!(!empresa && !contrato)) {
                tituloModificado = tituloModificado + ': ' + $("#txt_apellidos_nombres_vendedor").val() + ' - ' + $("#txt_nombre_tipo_venta").val() + ' - ' + $("#txt_nombre_tipo_pago").val();
            }
        }
        p.panel('setTitle', tituloModificado);
    }
}

function cerrar_contrato() {
    $("#div_registrar_contrato").dialog("close");
}

function fnBuscarContrato(p_codigo_empresa) {

    if (!$("#frm_buscar_contrato").form('enableValidation').form('validate'))
        return;
    var v_entidad = { codigo_empresa: p_codigo_empresa, numero_contrato: ActionContratoUrl._numero_contrato };

    $.ajax({
        type: 'post',
        url: ActionContratoUrl._GetContratoJson,
        data: v_entidad,
        dataType: 'json',
        cache: false,
        async: false,
        success: function (data) {

            if (data.existe_registro == 1) {

                $("#txt_apellidos_nombres_cliente").textbox("setValue", data.apellidos_nombres_cliente);
                $("#txt_nombre_canal_venta").textbox("setValue", data.nombre_canal_venta + ' - ' + data.nombre_grupo);
                $("#txt_apellidos_nombres_vendedor").textbox("setValue", data.apellidos_nombres_vendedor);
                $("#txt_apellidos_nombres_supervisor").textbox("setValue", data.apellidos_nombres_supervisor);
                $("#txt_nombre_tipo_venta").textbox("setValue", data.nombre_tipo_venta);
                $("#txt_nombre_tipo_pago").textbox("setValue", data.nombre_tipo_pago);

                $("#nro_contrato_ref").textbox("setValue", data.nro_contrato_ref);
                $("#nombre_empresa_ref").textbox("setValue", data.nombre_empresa_ref);
                $("#fecha_contrato").textbox("setValue", data.fecha_contrato);
                $("#fecha_proceso").textbox("setValue", data.fecha_proceso);
                $("#fecha_migracion").textbox("setValue", data.fecha_migracion);
                $("#usuario_proceso").textbox("setValue", data.usuario_proceso);

                $("#tiene_transferencia").textbox("setValue", data.tiene_transferencia);
                $("#nombre_empresa_transferencia").textbox("setValue", data.nombre_empresa_transferencia);
                $("#nro_contrato_transferencia").textbox("setValue", data.nro_contrato_transferencia);
                $("#monto_transferencia").numberbox("setValue", data.monto_transferencia);

                $('#dgv_analisis_contrato_articulo').datagrid("reload", v_entidad);
                $('#dgv_contrato_detalle_pago').datagrid('loadData', []);
                $('#dgv_analisis_cronograma').datagrid("reload", v_entidad);
            }
            else {

                $("#txt_apellidos_nombres_cliente").textbox("setValue", "");
                $("#txt_nombre_canal_venta").textbox("setValue", "");
                $("#txt_apellidos_nombres_vendedor").textbox("setValue", "");
                $("#txt_apellidos_nombres_supervisor").textbox("setValue", "");
                $("#txt_nombre_tipo_venta").textbox("setValue", "");
                $("#txt_nombre_tipo_pago").textbox("setValue", "");

                $("#nro_contrato_ref").textbox("setValue", "");
                $("#nombre_empresa_ref").textbox("setValue", "");
                $("#fecha_contrato").textbox("setValue", "");
                $("#fecha_proceso").textbox("setValue", "");
                $("#fecha_migracion").textbox("setValue", "");
                $("#usuario_proceso").textbox("setValue", "");

                $("#tiene_transferencia").textbox("setValue", "");
                $("#nombre_empresa_transferencia").textbox("setValue", "");
                $("#nro_contrato_transferencia").textbox("setValue", "");
                $("#monto_transferencia").textbox("setValue", "");

                $('#dgv_analisis_contrato_articulo').datagrid('loadData', []);
                $('#dgv_contrato_detalle_pago').datagrid('loadData', []);
                $('#dgv_analisis_cronograma').datagrid('loadData', []);

                $.messager.alert('Contrato no encontrado', "No se encontro contrato con los filtros establecidos.", 'warning');
            }

        },
        error: function (error) {
            alert(Error);
        }
    });
}


/******************************************/
function fnConfigurarGrillaContratoArticulo() {
    $('#dgv_analisis_contrato_articulo').datagrid({
        //fitColumns: true,
        url: ActionContratoUrl._GetArticulosByContratoEmpresaJson,
        pagination: false,
        singleSelect: true,
        remoteFilter: false,

        rownumbers: true,
        queryParams: {
            codigo_empresa: ActionContratoUrl._codigo_empresa,
            numero_contrato: ActionContratoUrl._numero_contrato
        },
        columns:
            [[

                { field: "codigo_empresa", title: "codigo_empresa", hidden: true, rowspan: "2" },
                { field: "codigo_moneda", title: "codigo_moneda", hidden: true, rowspan: "2" },
                { field: "codigo_articulo", title: "codigo_articulo", hidden: true, rowspan: "2" },

                { field: "es_hr", hidden: true, rowspan: "2", },

                { field: "mostrar_es_hr", title: "", width: "10", rowspan: "2", align: "left", halign: "center", styler: cellStylerEsHR  },

                { field: "nombre_articulo", title: "Artículo", width: "250", rowspan: "2", align: "left", halign: "center" },
                { field: "cantidad_articulo", title: "Cantidad", width: "80", rowspan: "2", align: "center", halign: "center" },
                { field: "nombre_moneda", title: "Moneda", width: "80", rowspan: "2", align: "left", halign: "center" },
                {
                    field: "monto_comision_inicial_total", title: "Comisión <br>Total", width: "90", rowspan: "2", align: "right", halign: "center", formatter: function (value, row, index) {
                        return $.NumberFormat(value, 2);
                    }
                },
                { title: "Supervisor", colspan: 5 },
                { title: "Vendedor", colspan: 5 }
            ], [
                {
                    field: "monto_comision_inicial_supervisor", title: "Comisión <br>Inicial", halign: "center", align: "right", width: "80", formatter: function (value, row, index) {
                        return $.NumberFormat(value, 2);
                    }
                },
                {
                    field: "monto_total_comision_supervisor", title: "Comisión", halign: "center", align: "right", width: "80", formatter: function (value, row, index) {
                        return $.NumberFormat(value, 2);
                    }
                },
                {
                    field: "monto_total_pagado_supervisor", title: "Pagado", halign: "center", align: "right", width: "80", formatter: function (value, row, index) {
                        return $.NumberFormat(value, 2);
                    }
                },
                {
                    field: "monto_total_saldo_supervisor", halign: "center", align: "right", title: "Saldo", width: "80", formatter: function (value, row, index) {
                        return $.NumberFormat(value, 2);
                    }
                },
                {
                    //field: "monto_total_excluido_supervisor", title: "Exclusión", halign: "center", align: "right", width: "80", formatter: function (value, row, index) {
                    //    return $.NumberFormat(value, 2);
                    //}
                    field: "anulacion_supervisor", title: "Anulación", halign: "center", align: "left", width: "120"
                },

                {
                    field: "monto_comision_inicial_personal", title: "Comisión<br> Inicial", halign: "center", align: "right", width: "80", formatter: function (value, row, index) {
                        return $.NumberFormat(value, 2);
                    }

                },

                {
                    field: "monto_total_comision_vendedor", title: "Comisión", halign: "center", align: "right", width: "80", formatter: function (value, row, index) {
                        return $.NumberFormat(value, 2);
                    }

                },
                {
                    field: "monto_total_pagado_vendedor", title: "Pagado", halign: "center", align: "right", width: "80", formatter: function (value, row, index) {
                        return $.NumberFormat(value, 2);
                    }
                },
                {
                    field: "monto_total_saldo_vendedor", title: "Saldo", width: "80", halign: "center", align: "right", formatter: function (value, row, index) {
                        return $.NumberFormat(value, 2);
                    }
                },
                {
                    field: "anulacion_vendedor", title: "Anulación", halign: "center", align: "left", width: "120"
                    //field: "monto_total_excluido_vendedor", title: "Exclusión", halign: "center", align: "right", width: "80", formatter: function (value, row, index) {
                    //    return $.NumberFormat(value, 2);
                    //}
                }
            ]],
        onClickRow: function (index, row) {
            fnReloadDetallePagoComision();
            //var v_entidad = {
            //    numero_contrato: ActionContratoUrl._numero_contrato,
            //    codigo_empresa: row.codigo_empresa,
            //    codigo_articulo: row.codigo_articulo,
            //    codigo_moneda: row.codigo_moneda
            //};
            //$('#dgv_contrato_detalle_pago').datagrid("reload", v_entidad);
        },
        onRowContextMenu: function (e, index, row) {
            if (index >= 0) {
                $(this).datagrid('selectRow', index);
                e.preventDefault();
                $('#mm').menu('show', {
                    left: e.pageX,
                    top: e.pageY
                });
            }
        }
    });

    var dg = $('#dgv_analisis_contrato_articulo');
    dg.datagrid();

    var columnas = "monto_comision_inicial_supervisor,monto_total_comision_supervisor,monto_total_pagado_supervisor,monto_total_saldo_supervisor,anulacion_supervisor, \
                    |monto_comision_inicial_personal,monto_total_comision_vendedor,monto_total_pagado_vendedor,monto_total_saldo_vendedor,anulacion_vendedor".split('|');
    var color = "#94DE82,#E49450".split(',');

    for (indice = 0; indice <= columnas.length - 1; indice++) {
        var cols = columnas[indice].split(',');
        for (indice2 = 0; indice2 <= cols.length - 1; indice2++) {
            dg.datagrid('getPanel').find('div.datagrid-header td[field="' + cols[indice2] + '"]').css('background-color', color[indice]);
        }
    }
    //  $('#dgv_analisis_contrato_articulo').datagrid('enableFilter');
}

function fnCopiar(atributo) {
    row = $('#dgv_analisis_contrato_articulo').datagrid("getSelected")
    if (!row) { return false; }
    var $temp = $("<input>");
    $("body").append($temp);
    $temp.val(row[atributo]).select();
    document.execCommand("copy");
    $temp.remove();
}

function fnReloadContratoArticulo() {

    var selectedrow = $('#dgv_analisis_contrato_articulo').datagrid("getSelected");
    var rowIndex = 0;

    if (selectedrow) {
        var rowIndex = $("#dgv_analisis_contrato_articulo").datagrid("getRowIndex", selectedrow);

        var _codigo_empresa = $("#cmb_empresa_sigeco").combobox("getValue");
        var v_entidad = { codigo_empresa: _codigo_empresa, numero_contrato: ActionContratoUrl._numero_contrato };
        $('#dgv_analisis_contrato_articulo').datagrid("reload", v_entidad);
        fnReloadDetallePagoComision();

    }

    $("#dgv_analisis_contrato_articulo").datagrid("selectRow", rowIndex);
}

function fnReloadDetallePagoComision() {
    var row = $('#dgv_analisis_contrato_articulo').datagrid('getSelected');

    if (!row) {
        $.messager.alert('Seleccione registro', "Para continuar con el proceso debe seleccionar un registro.", 'warning');
        return;
    }
    var v_entidad = {
        numero_contrato: ActionContratoUrl._numero_contrato,
        codigo_empresa: row.codigo_empresa,
        codigo_articulo: row.codigo_articulo,
        codigo_moneda: row.codigo_moneda
    };
    $('#dgv_contrato_detalle_pago').datagrid("reload", v_entidad);
}

function fnConfigurarGrillaContratoDetallePago() {
    $('#dgv_contrato_detalle_pago').datagrid({
        // fitColumns: true,
        rownumbers: true,
        data: null,
        toolbar: "#toolbar_cuota_cronograma",
        url: ActionContratoUrl._GetDetalleCronogramaPagoByArticuloJson,
        pagination: false,
        singleSelect: true,
        remoteFilter: false,
        columns:
            [[
                { field: 'nombre_tipo_planilla', title: 'Tipo Planilla', width: "150", halign: 'center', align: 'left' },
                { field: 'numero_planilla', title: 'Nro</br>Planilla', width: "100", halign: 'center', align: 'center' },
                { field: 'str_fecha_cierre', title: 'Fecha</br>Planilla', width: "100", halign: 'center', align: 'center' },
                { field: 'nro_cuota', title: 'Nro.</br> Cuota', width: "80", halign: 'center', align: 'center' },
                { field: 'str_fecha_programada', title: 'Fecha </br>Habilitado', width: "100", halign: 'center', align: 'center' },
                {
                    field: 'importe_sing_igv', title: 'Imp. </br>Sin IGV', width: "100", halign: 'center', align: 'right', formatter: function (value, row, index) {
                        return $.NumberFormat(value, 2);
                    }
                },
                {
                    field: 'igv', title: 'IGV', width: "100", halign: 'center', align: 'right', formatter: function (value, row, index) {
                        return $.NumberFormat(value, 2);
                    }
                },
                {
                    field: 'importe_comision', title: 'Monto a</br> Pagar', width: "100", halign: 'center', align: 'right', formatter: function (value, row, index) {
                        return $.NumberFormat(value, 2);
                    }
                },
                { field: 'str_fecha_exclusion', title: 'Fecha </br> Exclusión', width: "100", halign: 'center', align: 'center' },
                { field: 'str_fecha_anulado', title: 'Fecha </br> Anulación', width: "100", halign: 'center', align: 'center' },
                { field: 'nombre_estado_cuota', title: 'Estado', width: "200", halign: 'center', align: 'left' },
                { field: 'observacion', title: 'Observación', width: "350", halign: 'center', align: 'left' }

            ]]
    });
    // $('#dgv_contrato_detalle_pago').datagrid('enableFilter');
}

function fnConfigurarGrillaCronogramaPagos() {
    $('#dgv_analisis_cronograma').datagrid({
        fitColumns: true,
        rownumbers: true,
        url: ActionContratoUrl._GetCronogramaCuotasByContratoEmpresaJson,
        queryParams: {
            codigo_empresa: ActionContratoUrl._codigo_empresa,
            numero_contrato: ActionContratoUrl._numero_contrato
        },
        pagination: false,
        singleSelect: true,
        columns:
            [[
                { field: 'tipo_cuota', title: 'Tipo Cuota', width: 150, halign: 'center', align: 'left' },
                { field: 'cuota', title: 'Nro</br>Cuota', width: 100, halign: 'center', align: 'center' },
                {
                    field: 'importe_sin_igv', title: 'Importe<br>sin IGV', width: 120, halign: 'center', align: 'right', formatter: function (value, row, index) {
                        return $.NumberFormat(value, 2);
                    }
                },
                {
                    field: 'importe_igv', title: 'IGV', width: 120, halign: 'center', align: 'right', formatter: function (value, row, index) {
                        return $.NumberFormat(value, 2);
                    }
                },
                {
                    field: 'importe_total', title: 'Importe', width: 120, halign: 'center', align: 'right', formatter: function (value, row, index) {
                        return $.NumberFormat(value, 2);
                    }
                },
                { field: 'fec_vencimiento', title: 'Fecha </br> Vencimiento', width: 100, halign: 'center', align: 'center' },
                { field: 'fec_pago', title: 'Fecha </br> Pago', width: 100, halign: 'center', align: 'center' },
                { field: 'estado', title: 'Estado', width: 100, halign: 'center', align: 'center' }
            ]]
    });
    $('#dgv_analisis_cronograma').datagrid('enableFilter');
}

function cellStylerEsHR(value, row, index) {
    if (row.es_hr == 1) {
        return 'background-color:#2b6dd8;';
    }
    else {
        return 'background-color:#ffffff;';
    }
}
