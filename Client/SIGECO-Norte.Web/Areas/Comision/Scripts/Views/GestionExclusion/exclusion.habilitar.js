var ActionHabilitarUrl = {};
var JsondataPlanillaAbierta = [];
var array_id_exclusion ="";
var JsonTipoVenta = {};
var JsonTipoPago = {};
var JsonTipoPlanilla = {};

; (function (app) {
    //===========================================================================================
    var current = app.habilitar = {};
    //===========================================================================================

    jQuery.extend(app.habilitar,
        {
            Initialize: function (actionUrls) {

                jQuery.extend(ActionHabilitarUrl, actionUrls);
                fnInicializarHabilitarExclusion();
                fnCargarExclusion();
                fnListarPlanilla();
                fnConfigurarGrillaExclusionPagoHabilitado();
                

            }
        })
})(project);

function fnInicializarHabilitarExclusion(){

    JsonTipoVenta = $('.content').get_json_combobox({
        url: ActionHabilitarUrl.GetTipoVentaJson
    });

    JsonTipoPago = $('.content').get_json_combobox({
        url:ActionHabilitarUrl.GetTipoPagoJson
    });
    JsonTipoPlanilla = $('.content').get_json_combobox({
        url: ActionHabilitarUrl.GetTipoPlanillaJson
    });     
    $('.content').ResizeModal({
        widthMax: '90%',
        widthMin: '60%',
        div: 'div_habilitar_exclusion'
    });
}


function tipoPlanillaFormatter(value) {

    for (var i = 0; i < JsonTipoPlanilla.length; i++) {
        if (JsonTipoPlanilla[i].id == value) return JsonTipoPlanilla[i].text;
    }
    return value;
}

    function tipoPagoFormatter(value) {

        for (var i = 0; i < JsonTipoPago.length; i++) {
            if (JsonTipoPago[i].id == value) return JsonTipoPago[i].text;
        }
        return value;
    }
    function tipoVentaFormatter(value) {

        for (var i = 0; i < JsonTipoVenta.length; i++) {
            if (JsonTipoVenta[i].id == value) return JsonTipoVenta[i].text;
        }
        return value;
    }
    function planillaFormatter(value) {

        for (var i = 0; i < JsondataPlanillaAbierta.length; i++) {
            if (JsondataPlanillaAbierta[i].codigo_planilla == value) return JsondataPlanillaAbierta[i].numero_planilla;
        }
        return value;
    }


    function fnCargarExclusion() {
        var _rows = $("#dgv_exclusion").datagrid("getSelections");
        $.each(_rows, function (index, row) {
            if (index == 0) {
                array_id_exclusion = row.codigo_exclusion;
            }
            else {
                array_id_exclusion = array_id_exclusion + ',' + row.codigo_exclusion;
            }

        });
        //console.log(array_id_exclusion);
        // $('#dgv_exclusion_habilitar').datagrid("loadData", _rows);
    }
    function cellStylerMach(value, row, index) {
        if (value == 1) {
            return 'background-color:#4cff00;';
        }
        else if (value == 0) {
            return 'background-color:#ffd800;';
        }
    }


    function fnConfigurarGrillaExclusionPagoHabilitado() {

        $('#dgv_exclusion_habilitar').datagrid({
            fitColumns: false,
            height: "100%",
            url: ActionHabilitarUrl.GetDetallePagoComisionVsPlanillaAbierta,
            queryParams: {
                p_lst_id_exclusion: array_id_exclusion
            },
            idField: 'codigo_detalle_cronograma',
            pagination: true,
            singleSelect: true,
            remoteSort: false,
            rownumbers: true,
            pageList: [10, 20, 50, 100, 200, 400, 500],
            pageSize: 50,            
            columns:
            [[

                { field: 'codigo_detalle_cronograma', title: 'Codigo', hidden: 'true' },
                { field: 'codigo_exclusion', title: 'id exclusion', hidden: 'true' },
                { field: 'codigo_detalle_planilla', title: 'Codigo', hidden: 'true' },
                { field: 'codigo_regla_tipo_planilla', hidden: 'true' },

                {
                    field: 'indica_modificar', title: 'En<br>Planilla', width: 60, align: 'center', styler: cellStylerMach, formatter: function (val, row) {
                        if (val == 0) {
                            return '<input type="checkbox" disabled  readonly>';
                        }
                        else if (val == 1) {
                            return '<input type="checkbox" checked="checked" disabled  readonly>';
                        }

                        //<span style="color:red;">(' + val + ')</span>
                    }
                },
                   {
                       field: 'codigo_planilla', title: 'Nro. Planilla', width: 120, halign: 'center', align: 'left', editor: {
                           type: 'combobox',
                           options: {
                               valueField: 'codigo_planilla',
                               textField: 'numero_planilla',
                               data: JsondataPlanillaAbierta,
                               editable: false,
                               // novalidate: true,
                               // required: true,
                               onBeforeLoad: function (index, rows) {

                               }
                           }
                       },
                       formatter: planillaFormatter
                   },
                     {
                         field: 'nombre_regla_tipo_planilla', title: 'Planilla', width: 120, halign: 'center', align: 'left'
                     },
                   {
                       field: 'str_fecha_inicio', title: 'Fecha Inicio', width: 100, halign: 'center', align: 'center',
                       editor: {
                           type: 'textbox',
                           options: {
                               editable: false,
                               disabled: true
                           }
                       }
                   },
                    {
                        field: 'str_fecha_fin', title: 'Fecha Fin', width: 100, halign: 'center', align: 'center',
                        editor: {
                            type: 'textbox',
                            options: {
                                editable: false,
                                disabled: true
                            }
                        }
                    },

                { field: 'nombre_empresa', title: 'Empresa', width: 80, align: 'left' },
                { field: 'nro_contrato', title: 'Nro. <br>Contrato', width: 110, align: 'center' },
                 {
                     field: 'codigo_tipo_venta', title: 'Tipo Venta', width: 120, halign: 'center', align: 'left', formatter: tipoVentaFormatter
                 },
                {
                    field: 'codigo_tipo_pago', title: 'Tipo Pago', width: 120, halign: 'center', align: 'left', formatter: tipoPagoFormatter
                },
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
                { field: 'apellidos_nombres', title: 'Vendedor', width: 250, align: 'left' }
            ]],
            onBeforeEdit: function (index, row) {

                shortcut.add("Enter", function () {
                    var row_index_edicion = $.IndexRowEditing('dgv_exclusion_habilitar');

                    if (!fnValidarSeleccionPlanilla(index, row))
                    {
                        return false;
                    }

                    if ($.validarRowGrilla(row_index_edicion, 'dgv_exclusion_habilitar')) {
                        $("#dgv_exclusion_habilitar").datagrid("endEdit", row_index_edicion);
                    }

                });
                shortcut.add("Esc", function () {
                    var row_index_edicion = $.IndexRowEditing('dgv_exclusion_habilitar');
                    $('#dgv_exclusion_habilitar').datagrid('cancelEdit', row_index_edicion);
                });
            },
            onAfterEdit: function (index, row) {
                shortcut.remove("Enter");
                shortcut.remove("Esc");
                row.editing = false;
            },
            onCancelEdit: function (index, row) {
                row.editing = false;
            },
            onDblClickRow: function (rowIndex, row) {
                if (!$.existeRegistroEnEdicion('dgv_exclusion_habilitar', false)) {
                    RowEdicion(rowIndex, row);
                }
                else {

                    var row_index_edicion = $.IndexRowEditing('dgv_exclusion_habilitar');
                    if ($.validarRowGrilla(row_index_edicion, 'dgv_exclusion_habilitar')) {
                        $("#dgv_exclusion_habilitar").datagrid("endEdit", row_index_edicion);
                    };
                    RowEdicion(rowIndex, row);
                }
            }
        });

    }

    function RowEdicion(rowIndex, row) {
        $('#dgv_exclusion_habilitar').datagrid('beginEdit', rowIndex);
        row.editing = true;
        var _cmb_planilla = $('#dgv_exclusion_habilitar').datagrid('getEditor', { index: rowIndex, field: 'codigo_planilla' }).target;

        var _txb_fecha_inicio = $('#dgv_exclusion_habilitar').datagrid('getEditor', { index: rowIndex, field: 'str_fecha_inicio' }).target;
        var _txb_fecha_fin = $('#dgv_exclusion_habilitar').datagrid('getEditor', { index: rowIndex, field: 'str_fecha_fin' }).target;

        _txb_fecha_inicio.textbox("setValue", row.str_fecha_inicio);
        _txb_fecha_fin.textbox("setValue", row.str_fecha_fin);


        _cmb_planilla.combobox({
            onChange: function (newValue, oldValue) {
                if (newValue == 0) {
                    _txb_fecha_inicio.textbox("setValue", null);
                    _txb_fecha_fin.textbox("setValue", null);
                }
                else {
                    $.each(JsondataPlanillaAbierta, function (j, planilla) {

                        if (planilla.codigo_planilla == newValue) {
                            _txb_fecha_inicio.textbox("setValue", planilla.str_fecha_inicio);
                            _txb_fecha_fin.textbox("setValue", planilla.str_fecha_fin);
                        }

                    });
                }
            }
        });
        _cmb_planilla.combobox("setValue", row.codigo_planilla);
    }
    function cerrar_exclusion() {
        $("#div_habilitar_exclusion").dialog("close");
    }


    function fnListarPlanilla() {
        $.ajax({
            type: 'post',
            url: ActionHabilitarUrl.GetPlanillaAbierta,
            dataType: 'json',
            cache: false,
            async: false,
            success: function (data) {
                JsondataPlanillaAbierta = data;           

            },
            error: function (error) {
                alert(Error);
            }
        });
    }


    function fnRegistrarPagoComision() {
        if (!$("#frm_habilitar_exclusion").form('enableValidation').form('validate'))
            return;
        if ($.existeRegistroEnEdicion('dgv_exclusion_habilitar'))
            return;
        var rows = $("#dgv_exclusion_habilitar").datagrid("getRows");
        if (rows.length < 1) {
            $.messager.alert("Habilitar Pago Comisión", "No existe pagos de comisión para habilitar, intente nuevamente.", "warning");
            return;
        }

        var lst_cuota_pago_comision = [];

        $.each(rows, function (i, row) {
            var _cuota = {
                codigo_planilla: row.codigo_planilla,
                codigo_exclusion:row.codigo_exclusion,/*
                codigo_detalle_cronograma: row.codigo_detalle_cronograma,
                codigo_detalle_planilla: row.codigo_detalle_planilla,*/
                motivo_registro: $("#txt_motivo_registro").textbox("getText")
            };
            lst_cuota_pago_comision.push(_cuota);
        });

        $.messager.confirm('Confirmaci&oacute;n', '&iquest;Est&aacute; seguro que desea habilitar?', function (result) {
            if (result) {
                $.ajax({
                    type: 'post',
                    url: ActionHabilitarUrl.HabilitarCuotaPagoComision,
                    data: JSON.stringify({ lst_cuota_pago_comision: lst_cuota_pago_comision }),
                    async: false,
                    cache: false,
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8',
                    success: function (data) {
                        if (data.v_resultado == 1)
                        {
                            fnConsultarExclusion();
                            $.messager.alert('Habilitar Pago Comisión.', data.v_mensaje, 'info', function () {                                
                                $("#div_habilitar_exclusion").dialog("close");
                            });                      
                        }
                        else {
                            $.messager.alert('Error en Habilitar Pago Comisión', data.v_mensaje, 'error');
                        }
                    },
                    error: function (erro) {
                        $.messager.alert('Error', "Error en el servidor", 'error');
                    }
                });
            }
        });

    }

    function fnValidarSeleccionPlanilla(index, row) {
        var _cmb_planilla = $('#dgv_exclusion_habilitar').datagrid('getEditor', { index: index, field: 'codigo_planilla' }).target;
        var codigocomboplanilla = _cmb_planilla.combobox("getValue");
        var codigoreglatipoplanilla = 0;

        $.each(JsondataPlanillaAbierta, function (j, planilla) {
            if (planilla.codigo_planilla == codigocomboplanilla) {
                codigoreglatipoplanilla = planilla.codigo_regla_tipo_planilla;
            }
        });

        if ((codigoreglatipoplanilla != 0) && (row.codigo_regla_tipo_planilla != codigoreglatipoplanilla)) {
            $.messager.alert('Habilitar Pago Comisión', 'No puede seleccionar una planilla que no corresponde al pago.', 'info');
            return false;
        }
        return true;
    }