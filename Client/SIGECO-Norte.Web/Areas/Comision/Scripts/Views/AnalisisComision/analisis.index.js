var ActionIndexUrl = {};
var indexContexMenu = -1;
var _NroContrato = "";
var _CodigoEmpresa = "";
var _EstadoBloqueo = "0";
; (function (app) {
    //===========================================================================================
    var current = app.index = {};
    //===========================================================================================

    jQuery.extend(app.index,
        {
            Initialize: function (actionUrls) {

                jQuery.extend(ActionIndexUrl, actionUrls);
                fnConfigurarGrillaContratoArticulo();
                fnConfigurarGrillaContratoDetallePago();
                fnConfigurarGrillaCronogramaPagos();
                fnInicializarContrato();

            }
        })
})(project);

function fnInicializarContrato() {

    $('.content').combobox_sigees({
        id: '#cmb_empresa_sigeco',
        url: ActionIndexUrl._GetEmpresaJson
    });
    
    $("#txt_nro_contrato").keypress(function (e) {
        if (e.keyCode == 13) {
            fnBuscarContrato();
        }
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
    //HabilitarCollapse('pnlDatosGenerales');
    //HabilitarCollapse('pnlDatosVenta');
}

function HabilitarCollapse(panel) {
    var p = $('#' + panel)
    p.panel('header').click(function () {
        if (p.panel('options').collapsed) {
            p.panel('expand', true);
        } else {
            p.panel('collapse', true);
        }
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
            if (!(!empresa && !contrato))
            {
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

function fnFormatearContrato(nombreControl) {
    var texto = $.trim($(nombreControl).val());
    var ceros = '0';
    if (texto.length > 0 && texto.length < 10)
    {
        texto = ceros.repeat(10 - texto.length) + texto;
    }
    return texto;
}

function fnBuscarContrato() {

    $("#txt_nro_contrato").val(fnFormatearContrato('#txt_nro_contrato'));

    if (!$("#frm_buscar_contrato").form('enableValidation').form('validate'))
        return;

    $('#dgv_analisis_contrato_articulo').datagrid("clearSelections");
    $('#dgv_contrato_detalle_pago').datagrid("clearSelections");
    $('#dgv_analisis_cronograma').datagrid("clearSelections");

    var v_entidad = { codigo_empresa: $("#cmb_empresa_sigeco").combobox("getValue"), numero_contrato: $.trim($("#txt_nro_contrato").val()) };
    indexContexMenu = -1;

    $.ajax({
        type: 'post',
        url: ActionIndexUrl._GetContratoJson,
        data: v_entidad,
        dataType: 'json',
        cache: false,
        async: false,
        success: function (data) {
            //console.log(data);
            if (data.existe_registro == 1) {
                $("#txt_apellidos_nombres_cliente").textbox("setValue", data.apellidos_nombres_cliente);
                $("#txt_nombre_canal_venta").textbox("setValue", data.nombre_canal_venta + ' - ' + data.nombre_grupo);
                $("#txt_apellidos_nombres_vendedor").textbox("setValue", data.apellidos_nombres_vendedor);
                $("#txt_apellidos_nombres_supervisor").textbox("setValue", data.apellidos_nombres_supervisor);
                $("#txt_nombre_tipo_venta").textbox("setValue", data.nombre_tipo_venta);
                $("#txt_nombre_tipo_pago").textbox("setValue", data.nombre_tipo_pago);
                $("#txt_doc_completa").textbox("setValue", data.doc_completa);

                $("#txt_nombre_estado_contrato_migrado").textbox("setValue", data.nombre_estado_proceso_migrado);
                $("#txt_observacion_contrato_migrado").textbox("setValue", data.observacion_contrato_migrado);

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

                $("#usuario_bloqueo").textbox("setValue", data.usuario_bloqueo);
                $("#fecha_bloqueo").textbox("setValue", data.fecha_bloqueo);
                $("#motivo_bloqueo").textbox("setValue", data.motivo_bloqueo);
                $("#bloqueo").switchbutton({ "checked": data.bloqueo == "1" ? true : false });
                $("#btnBloqueo").linkbutton({ text: data.bloqueo == "1" ? "Desbloquear" : "Bloquear"});

                $('#dgv_analisis_contrato_articulo').datagrid("reload", v_entidad);
                $('#dgv_contrato_detalle_pago').datagrid('loadData', []);
                $('#dgv_analisis_cronograma').datagrid("reload", v_entidad);
                _NroContrato = $.trim($("#txt_nro_contrato").val());
                _CodigoEmpresa = $("#cmb_empresa_sigeco").combobox("getValue");
                _EstadoBloqueo = data.bloqueo;
            }
            else {
                $("#txt_apellidos_nombres_cliente").textbox("setValue", "");
                $("#txt_nombre_canal_venta").textbox("setValue", "");
                $("#txt_apellidos_nombres_vendedor").textbox("setValue", "");
                $("#txt_apellidos_nombres_supervisor").textbox("setValue", "");
                $("#txt_nombre_tipo_venta").textbox("setValue", "");
                $("#txt_nombre_tipo_pago").textbox("setValue", "");
                $("#txt_doc_completa").textbox("setValue", "");

                $("#txt_nombre_estado_contrato_migrado").textbox("setValue", "");
                $("#txt_observacion_contrato_migrado").textbox("setValue", "");

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

                $("#usuario_bloqueo").textbox("setValue", "");
                $("#fecha_bloqueo").textbox("setValue", "");
                $("#motivo_bloqueo").textbox("setValue", "");

                $("#bloqueo").switchbutton({ checked: false });
                $("#btnBloqueo").linkbutton({ text: 'Bloquear' });

                $('#dgv_analisis_contrato_articulo').datagrid('loadData', []);
                $('#dgv_contrato_detalle_pago').datagrid('loadData', []);
                $('#dgv_analisis_cronograma').datagrid('loadData', []);
                _NroContrato = "";
                _CodigoEmpresa = "";
                _EstadoBloqueo = "0";

                $.messager.alert('Análisis Comisión', "No se encontró contrato con los filtros establecidos.", 'warning');
            }

        },
        error: function (error) {
            alert(Error);
        }
    });
}


function fnConfigurarGrillaContratoArticulo() {
    $('#dgv_analisis_contrato_articulo').datagrid({
        //fitColumns: true,
        url: ActionIndexUrl._GetArticulosByContratoEmpresaJson,
        pagination: false,
        singleSelect: true,
        remoteFilter: false,
        height:240,
        rownumbers: true,
        idField: 'codigo_sku',
        //queryParams: {
        //    codigo_empresa: ActionContratoUrl._codigo_empresa,
        //    numero_contrato: ActionContratoUrl._numero_contrato
        //},
        columns:
       [[

            { field: "codigo_empresa", title: "codigo_empresa", hidden: true, rowspan: "2" },
            { field: "codigo_moneda", title: "codigo_moneda", hidden: true, rowspan: "2" },
            { field: "codigo_articulo", title: "codigo_articulo", hidden: true, rowspan: "2" },
            { field: "codigo_sku", title: "codigo_sku", hidden: true, rowspan: "2" },
            { field: "es_hr", hidden: true, rowspan: "2", },
            { field: "genera_comision", hidden: true, rowspan: "2", },

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
            var v_entidad = {
                numero_contrato: row.numero_contrato,
                codigo_empresa: row.codigo_empresa,
                codigo_articulo: row.codigo_articulo,
                codigo_moneda: row.codigo_moneda
            };
            $('#dgv_contrato_detalle_pago').datagrid("reload", v_entidad);
        },
        onRowContextMenu: function (e, index, row) {
            if (index>=0){
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

    //$('#dgv_analisis_contrato_articulo').datagrid('enableFilter');
}

function fnCopiar(atributo)
{
    var row = $('#dgv_analisis_contrato_articulo').datagrid("getSelected");
    CopyPaste(row, atributo);
}

function fnCopiarComision(atributo) {
    var row = $('#dgv_contrato_detalle_pago').datagrid("getSelected");
    CopyPaste(row, atributo);
    $("#dgv_contrato_detalle_pago").datagrid('unselectRow', indexContexMenu);
}

function CopyPaste(row, atributo) {
    if (!row) { return false; }
    var $temp = $("<input>");
    $("body").append($temp);
    $temp.val(row[atributo]).select();
    document.execCommand("copy");
    $temp.remove();
}

function VerDetalleArticulo() {
    var nombreOpcion = "Detalle Artículo"
    var row = $('#dgv_analisis_contrato_articulo').datagrid("getSelected")
    if (!row) {
        return false;
    }

    if (row.codigo_articulo == 0) {
        $.messager.alert(nombreOpcion, "Artículo inactivo o no existe.", 'warning');
        return false;
    }

    $(this).AbrirVentanaEmergente({
        parametro: "?p_codigo_articulo=" + row.codigo_articulo,
        div: 'div_visualizar_articulo',
        title: "Datos de Articulo",
        url: ActionIndexUrl._VisualizarArticulo
    });
}

function fnConfigurarGrillaContratoDetallePago() {
    $('#dgv_contrato_detalle_pago').datagrid({
        //fitColumns: true,
        rownumbers: true,
        data: null,
        height:400,
        url: ActionIndexUrl._GetDetalleCronogramaPagoByArticuloJson,
        toolbar: "#toolbar_analisis",
        pagination: false,
        singleSelect: true,
        remoteFilter: false,
        columns:
        [[
            { field: 'nombre_tipo_planilla', title: 'Tipo Planilla', width: "150", halign: 'center', align: 'left' },
            { field: 'numero_planilla', title: 'Nro</br>Planilla', width: "100", halign: 'center', align: 'center' },
            { field: 'str_fecha_cierre', title: 'Fecha</br>Planilla', width: "100", halign: 'center', align: 'center' },
            { field: 'nro_cuota', title: 'Nro.</br> Cuota', width: "80", halign: 'center', align: 'left' },
            { field: 'str_fecha_programada', title: 'Fecha </br>Habilitado', width: "100", halign: 'center', align: 'center' },
            {
                field: 'importe_sing_igv', title: 'Imp. </br>Sin IGV', width: "120", halign: 'center', align: 'right', formatter: function (value, row, index) {
                    return $.NumberFormat(value, 2);
                }
            },
            {
                field: 'igv', title: 'IGV', width: "120", halign: 'center', align: 'right', formatter: function (value, row, index) {
                    return $.NumberFormat(value, 2);
                }
            },
            {
                field: 'importe_comision', title: 'Monto a</br> Pagar', width: "120", halign: 'center', align: 'right', formatter: function (value, row, index) {
                    return $.NumberFormat(value, 2);
                }
            },
            { field: 'str_fecha_exclusion', title: 'Fecha </br> Exclusión', width: "100", halign: 'center', align: 'center' },
            { field: 'str_fecha_anulado', title: 'Fecha </br> Anulación', width: "100", halign: 'center', align: 'center' },

            
        
        { field: 'nombre_estado_cuota', title: 'Estado', width: "200", halign: 'center', align: 'left' },
        { field: 'observacion', title: 'Observación', width: "350", halign: 'center', align: 'left' }
            
            
        ]]
        ,onRowContextMenu: function (e, index, row) {
            if (indexContexMenu != -1)
            {
                $(this).datagrid('unselectRow', indexContexMenu);
            }
            if (index >= 0) {
                $(this).datagrid('selectRow', index);
                e.preventDefault();
                $('#mnuObservacionOperacion').menu('show', {
                    left: e.pageX,
                    top: e.pageY
                });
                indexContexMenu = index;
            }
        }
    });
    //$('#dgv_contrato_detalle_pago').datagrid('enableFilter');
}

function fnConfigurarGrillaCronogramaPagos() {
    $('#dgv_analisis_cronograma').datagrid({
        fitColumns: true,
        rownumbers: true,
        url: ActionIndexUrl._GetCronogramaCuotasByContratoEmpresaJson,
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

function ModificarCuota() {
    var nombreOpcion = 'Modificar Cuota';
    var row = $("#dgv_contrato_detalle_pago").datagrid('getSelected');

    if (ValidarSeleccionMultiple()) {
        $.messager.alert(nombreOpcion, "No se puede ejecutar esta opción por haber seleccionado más de una cuota.", 'warning');
        return false;
    }

    if (!row) {
        $.messager.alert(nombreOpcion, "Seleccione un registro.", 'warning');
        return false;
    }

    if (row.indica_registro_manual_comision == '1') {
        $.messager.alert(nombreOpcion, "Una cuota de tipo registro manual de comisión no se puede modificar.", 'warning');
        return false;
    }

    if (row.codigo_estado_cuota == 2 || row.codigo_estado_cuota == 3 || row.codigo_estado_cuota == 4 || row.codigo_estado_cuota == 5) {
        $.messager.alert(nombreOpcion, 'No se puede modificar la cuota por estar ' + row.nombre_estado_cuota + '.', 'warning');
        return false;
    }

    $(this).AbrirVentanaEmergente({
        parametro: "?codigo_detalle_cronograma=" + row.codigo_detalle_cronograma + "&monto=" + row.importe_sing_igv,
        div: 'div_modificar_cuota',
        title: "Modificar Cuota",
        url: ActionIndexUrl._ModificarCuota
    });
}

function AnularCuota() {
    var nombreOpcion = 'Anular Cuota';
    var row = $('#dgv_contrato_detalle_pago').datagrid('getSelected');

    if (ValidarSeleccionMultiple()) {
        $.messager.alert(nombreOpcion, "No se puede ejecutar esta opción por haber seleccionado más de una cuota.", 'warning');
        return false;
    }

    if (!row) {
        $.messager.alert(nombreOpcion, "Seleccione un registro.", 'warning');
        return false;
    }

    if (row.indica_registro_manual_comision == '1') {
        $.messager.alert(nombreOpcion, "Una cuota de tipo registro manual de comisión no se puede anular.", 'warning');
        return false;
    }

    if (row.codigo_estado_cuota == 3 || row.codigo_estado_cuota == 5) {
        $.messager.alert(nombreOpcion, 'No se puede anular la cuota por estar ' + row.nombre_estado_cuota + '.', 'warning');
        return false;
    }

    $.messager.observacion(nombreOpcion, 'Ingrese el motivo de anulación de la cuota.', function (win, data) {
        if (data) {

            var pEntidad = {
                codigo_detalle_cronograma: row.codigo_detalle_cronograma,
                motivo_anulacion: data
            };

            $.ajax({
                type: 'post',
                url: ActionIndexUrl._Anular_Cuota,
                data: pEntidad,
                async: false,
                cache: false,
                dataType: 'json',
                success: function (data) {
                    if (data.v_resultado == 1) {
                        $.messager.alert('Operación exitosa', data.v_mensaje, 'info', function () {
                            ActualizarGrillaArticulos();
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

function DeshabilitarCuota() {
    var nombreOpcion = 'Deshabilitar Cuota';
    var estados = new Set([1]);
    var rows = $('#dgv_contrato_detalle_pago').datagrid('getSelections');
    var mensajeError = "";

    if (rows.length == 0) {
        $.messager.alert(nombreOpcion, "Seleccione un registro.", 'warning');
        return;
    }

    $.each(rows, function (index, data) {

        if (data.indica_registro_manual_comision == '1') {
            mensajeError = "Una cuota de tipo registro manual de comisión no se puede deshabilitar.";
            return false;
        }

        if (!estados.has(data.codigo_estado_cuota)) {
            mensajeError = 'No se puede deshabilitar la cuota por estar ' + data.nombre_estado_cuota + '.';
            return false;
        }
    
        if (!data.fecha_programada) {
            mensajeError = 'No se puede deshabilitar la cuota.';
            return false;
        }

    });

    if (mensajeError.length > 0) {
        $.messager.alert(nombreOpcion, mensajeError, 'warning');
        return false;
    }

    lst_detalle_cronograma = [];
    $.each(rows, function (index, data) {
        var detalle = {
            codigo_detalle_cronograma: data.codigo_detalle_cronograma
        };
        lst_detalle_cronograma.push(detalle);
    });


    $.messager.observacion(nombreOpcion, 'Ingrese el motivo de deshabilitar cuota(s).', function (win, result) {
        if (result) {

            var pEntidad = {
                lst_detalle_cronograma_elemento: lst_detalle_cronograma,
                motivo: result,
            };

            $.ajax({
                type: 'post',
                url: ActionIndexUrl.Deshabilitar_Cuota,
                data: JSON.stringify({ v_deshabilitar: pEntidad }),
                async: false,
                cache: false,
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    if (data.v_resultado == 1) {
                        $.messager.alert('Operación exitosa', data.v_mensaje, 'info', function () {
                            ActualizarGrillaArticulos();
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

function ExcluirCuota() {
    var nombreOpcion = "Exclusión Cuota";
    var row = $('#dgv_contrato_detalle_pago').datagrid('getSelected');
    var rows = $('#dgv_contrato_detalle_pago').datagrid('getSelections');
    var mensajeError = "";

    if (rows.length == 0) {
        $.messager.alert(nombreOpcion, "Seleccione un registro.", 'warning');
        return;
    }

    $.each(rows, function (index, data) {
        if (data.indica_registro_manual_comision == "1") {
            mensajeError = "La cuota " + data.nro_cuota + " es de tipo registro manual de comisión y no se puede excluir.";
            return false;
        }

        if (!data.numero_planilla) {
            mensajeError = "Solo se puede excluir cuotas que esten incluidas en una planilla.";
            return false;
        }

        if (data.codigo_estado_cuota != 2) {
            mensajeError = "No se puede excluir la cuota " + data.nro_cuota + " por estar " + row.nombre_estado_cuota + ".";
            return false;
        }
    });

    if (mensajeError.length > 0) {
        $.messager.alert(nombreOpcion, mensajeError, 'warning');
        return false;
    }

    $(this).AbrirVentanaEmergente({
        parametro: "?codigo_detalle_cronograma=-1",
        div: 'div_exclusion_planilla',
        title: "Exclusión Cuota",
        url: ActionIndexUrl._ExclusionCuota
    });
}

function DetalleCuota() {
    var nombreOpcion = 'Detalle Cuota';
    var row = $("#dgv_contrato_detalle_pago").datagrid('getSelected');

    if (!row) {
        $.messager.alert(nombreOpcion, "Seleccione un registro.", 'warning');
        return false;
    }

    if (ValidarSeleccionMultiple()) {
        $.messager.alert(nombreOpcion, "No se puede ejecutar esta opción por haber seleccionado más de una cuota.", 'warning');
        return false;
    }

    $(this).AbrirVentanaEmergente({
        parametro: "?codigo_detalle_cronograma=" + row.codigo_detalle_cronograma,
        div: 'div_detalle_operacion',
        title: "Detalle Operación de Cuota",
        url: ActionIndexUrl._DetalleCuota
    });
}

function AdicionarCuota() {
    var nombreOpcion = 'Adicionar Cuota';
    var row = $("#dgv_analisis_contrato_articulo").datagrid('getSelected');
    if (!row) {
        $.messager.alert(nombreOpcion, "Seleccione el artículo al cual adicionará una cuota.", 'warning');
        return false;
    }

    if (row.genera_comision == 0) {
        $.messager.alert(nombreOpcion, "No se puede adicionar una cuota a este artículo.", 'warning');
        return false;
    }

    $(this).AbrirVentanaEmergente({
        parametro: "?codigo_empresa=" + row.codigo_empresa + "&nro_contrato=" + row.numero_contrato + "&codigo_articulo=" + row.codigo_articulo,
        div: 'div_adicionar_cuota',
        title: "Adicionar Cuota",
        url: ActionIndexUrl._AdicionarCuota
    });
}

function ActualizarGrillaArticulos() {
    var selectedrow = $('#dgv_analisis_contrato_articulo').datagrid("getSelected");
    var rowIndex = 0;

    if (selectedrow) {
        rowIndex = $("#dgv_analisis_contrato_articulo").datagrid("getRowIndex", selectedrow);

        var v_entidad = { codigo_empresa: $("#cmb_empresa_sigeco").combobox("getValue"), numero_contrato: $.trim($("#txt_nro_contrato").val()) };

        $('#dgv_analisis_contrato_articulo').datagrid("reload", v_entidad);
        ActualizarGrillaCuotas();
    }
    $("#dgv_analisis_contrato_articulo").datagrid("selectRow", rowIndex);
}

function ActualizarGrillaCuotas() {
    var row = $('#dgv_analisis_contrato_articulo').datagrid('getSelected');

    if (!row) {
        $.messager.alert('Análisis Comisión', "No se ha selecciondo un registro.", 'warning');
        return;
    }
    var v_entidad = {
        numero_contrato: row.numero_contrato,
        codigo_empresa: row.codigo_empresa,
        codigo_articulo: row.codigo_articulo,
        codigo_moneda: row.codigo_moneda
    };
    $('#dgv_contrato_detalle_pago').datagrid("reload", v_entidad);
}

function ValidarSeleccionMultiple() {
    var rows = $("#dgv_contrato_detalle_pago").datagrid('getSelections');
    return (rows.length > 1);
}

function fnBloquearReproceso() {
    var nombreOpcion = '(Des)Bloquear Reproceso';
    //var bloqueo = $("#bloqueo").switchbutton('options').checked;

    if (!_NroContrato || !_CodigoEmpresa) {
        $.messager.alert(nombreOpcion, "Debe buscar un contrato.", 'warning');
        return false;
    }

    $.messager.observacion(nombreOpcion, 'Ingrese el motivo del (des)bloqueo de reproceso.', function (win, data) {
        if (data) {

            var pEntidad = {
                nro_contrato: _NroContrato,
                codigo_empresa: _CodigoEmpresa,
                bloqueo: (_EstadoBloqueo == "1"?0:1),
                motivo: data
            };

            $.ajax({
                type: 'post',
                url: ActionIndexUrl._Bloquear_Reproceso,
                data: pEntidad,
                async: false,
                cache: false,
                dataType: 'json',
                success: function (data) {
                    if (data.v_resultado == 1) {
                        $.messager.alert('Operación exitosa', data.v_mensaje, 'info', function () {
                            fnBuscarContrato();
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
