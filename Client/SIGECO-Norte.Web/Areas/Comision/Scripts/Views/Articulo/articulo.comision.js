var ActionComisionUrl = {};
var JsonCanalVenta = {};
var JsonTipoPago = {};
var JsonComisionadoPor = {};
var JsonTipoComisionSupervisor = {};
var manejarClonComisiones = 0;
var tipoComisionVendedor = 1;
var tipoComisionSupervisor = 2;

function comisionadoPorFormatter(value) {
    for (var i = 0; i < JsonComisionadoPor.length; i++) {
        if (JsonComisionadoPor[i].id == value) return JsonComisionadoPor[i].text;
    }
    return value;
}

function tipoPagoFormatter(value) {
    for (var i = 0; i < JsonTipoPago.length; i++) {
        if (JsonTipoPago[i].id == value) return JsonTipoPago[i].text;
    }
    return value;
}

function canalVentaFormatter(value) {
    for (var i = 0; i < JsonCanalVenta.length; i++) {
        if (JsonCanalVenta[i].id == value) return JsonCanalVenta[i].text;
    }
    return value;
}

function tipoComisionSupervisorFormatter(value) {
    for (var i = 0; i < JsonTipoComisionSupervisor.length; i++) {
        if (JsonTipoComisionSupervisor[i].id == value) return JsonTipoComisionSupervisor[i].text;
    }
    return value;
}

; (function (app) {
    //===========================================================================================
    var current = app.comision = {};
    //===========================================================================================
    jQuery.extend(app.comision,
        {
            Initialize: function (actionUrls) {
                jQuery.extend(ActionComisionUrl, actionUrls);
                fnInicializarComision();
                fnConfigurarGrillaComision();
                fnListarComisionByPrecio();
                fnConfigurarGrillaComisionSupervisor();
                fnListarComisionByPrecioSupervisor();
            }
        })
})(project);

function fnInicializarComision() {

    JsonCanalVenta = $('.content').get_json_combobox({
        url: ActionComisionUrl._GetCanalVentaJson
    });

    JsonTipoPago = $('.content').get_json_combobox({
        url: ActionComisionUrl._GetTipoPagoJson
    });

    JsonComisionadoPor = $('.content').get_json_combobox({
        url: ActionComisionUrl._GetComisionadoPorJson
    });

    JsonTipoComisionSupervisor = $('.content').get_json_combobox({
        url: ActionComisionUrl._GetTipoComisionSupervisorJson
    });

    $('.content').ResizeModal({
        widthMax: '90%',
        widthMin: '80%',
        div: 'div_registrar_comision'
    });

    var txt_nombre_articulo = $("#div_registrar_articulo #txt_articulo_nombre").val();
    $("#div_registrar_comision #txt_articulo_nombre").val(txt_nombre_articulo);

    var v_codigo_precio = ActionComisionUrl._codigo_precio;

    var rws = $("#dgv_precio_articulo").datagrid("getRows");
    var rowIndex = 0;
    $.each(rws, function (i, objeto) {
        if (objeto.codigo_precio == v_codigo_precio) {
            rowIndex = i;
            return false;
        }
    });
    var rowdata = $('#dgv_precio_articulo').datagrid("getRows")[rowIndex];

    var txt_nombre_empresa = empresaFormatter(rowdata.codigo_empresa);
    $("#div_registrar_comision #txt_empresa_nombre").val(txt_nombre_empresa);

    
    var txt_nombre_tipo_venta = tipoVentaFormatter(rowdata.codigo_tipo_venta);
    $("#div_registrar_comision #txt_tipo_venta_nombre").val(txt_nombre_tipo_venta);
    ActionComisionUrl.nombre_tipo_venta = txt_nombre_tipo_venta;

    var txt_fecha_vigencia_inicio = rowdata.str_vigencia_inicio;
    $("#div_registrar_comision #dtp_fecha_vigencia_inicio").val(txt_fecha_vigencia_inicio);

    var txt_fecha_vigencia_fin = rowdata.str_vigencia_fin;
    $("#div_registrar_comision #dtp_fecha_vigencia_fin").val(txt_fecha_vigencia_fin);

    var txt_nombre_moneda = monedaFormatter(rowdata.codigo_moneda);
    $("#div_registrar_comision #txt_moneda_nombre").val(txt_nombre_moneda);

    var precio_total = $.NumberFormat(rowdata.precio_total, 2);
    $("#div_registrar_comision #txt_articulo_precio").val(precio_total);
    
    var precio = $('#dgv_precio_articulo').datagrid('getSelected');
    manejarClonComisiones = precio.clonarcomisiones;
}

function fnListarComisionByPrecio() {
    $.each(lst_comision, function (index, rows) {
        rows.valor = parseFloat(rows.valor);
        rows.vigencia_inicio = rows.str_vigencia_inicio;
        rows.vigencia_fin = rows.str_vigencia_fin;
    });

    $.ajax({
        type: 'post',
        url: ActionComisionUrl._GetReglasbyPrecioJson,
        data: JSON.stringify({ codigoPrecio: ActionComisionUrl._codigo_precio, lst_regla_calculo_comision: lst_comision, lst_eliminados: lst_comision_eliminar }),
        async: false,
        cache: false,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $('#dgv_comision').datagrid('loadData', data);
        },
        error: function () {
            alert("Error en recuperar comisión.");
        }
    });
}

function fnListarComisionByPrecioSupervisor() {

    $.each(lst_comision_supervisor, function (index, rows) {
        rows.valor = parseFloat(rows.valor);
        rows.vigencia_inicio = rows.str_vigencia_inicio;
        rows.vigencia_fin = rows.str_vigencia_fin;
    });

    $.ajax({
        type: 'post',
        url: ActionComisionUrl._GetComisionPrecioSupervisorJson,
        data: JSON.stringify({ codigoPrecio: ActionComisionUrl._codigo_precio, lst_comision_precio_supervisor: lst_comision_supervisor, lst_eliminados: lst_comision_supervisor_eliminar }),
        async: false,
        cache: false,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $('#dgv_comision_supervisor').datagrid('loadData', data);
        },
        error: function () {
            alert("Error en recuperar comisión.");
        }
    });
}


function fnConfigurarGrillaComision() {

    $('#dgv_comision').datagrid({
        fitColumns: true,
        idField: 'codigo_regla',
        toolbar:"#tlb_regla_calculo_precio",
        rownumbers: true,
        pageList: [30, 50, 100, 200, 400, 500],
        pageSize: 30,
        pagination: true,
        singleSelect: true,
        columns:
        [[
            { field: 'codigo_regla', title: 'Codigo Regla<br> Comisión', width: 120, align: 'left',hidden:true },
            { field: 'codigo_precio', title: 'Codigo Precio', width: 180, align: 'left', hidden: true },
            { field: 'nombre_tipo_venta', title: 'Tipo Venta', width: 150, halign: 'center',align: 'left' },
            {
                field: 'codigo_canal', title: 'Canal de Venta', width: 180,halign: 'center', align: 'left',

                editor: {
                    type: 'combobox',
                    options: {
                        valueField: 'id',
                        textField: 'text',
                        data: JsonCanalVenta,
                        editable: false,
                        required: true
                    }
                },
                formatter: canalVentaFormatter
            },
            {
                field: 'codigo_tipo_pago', title: 'Tipo Pago', width: 180, halign: 'center',align: 'left', editor: {
                    type: 'combobox',
                    options: {
                        valueField: 'id',
                        textField: 'text',
                        data: JsonTipoPago,
                        editable: false,
                        required: true
                    }
                },
                formatter: tipoPagoFormatter
            },

            {
                field: 'codigo_tipo_comision', title: 'Comisiona por', width: 180,halign: 'center', align: 'left', editor: {
                    type: 'combobox',
                    options: {
                        valueField: 'id',
                        textField: 'text',
                        editable: false,
                        data: JsonComisionadoPor,
                        required: true
                    }
                },
                formatter: comisionadoPorFormatter
            },
            {
                field: 'valor', title: 'Valor', width: 130, halign: 'center', align: 'right',
                formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                },
                editor: {
                    type: 'numberbox',
                    options: {
                        precision: 2,
                        required: true,
                        min: 0,
                        max: 99999999
                    }
                }
            },
              {
                  field: 'str_vigencia_inicio', title: 'Vigencia Inicio', width: 150,halign: 'center', align: 'center',
                  editor: {
                      type: 'datebox',
                      options: {
                          required: true,
                          validType: "fechaMenorIgualA['td[field=\"str_vigencia_fin\"] input.datebox-f',\'Fecha Vigencia Fin\',\'dd/MM/yyyy\']",
                          formatter: myformatter,
                          parser: myparser
                      }
                  }
              },
            {
                field: 'str_vigencia_fin', title: 'Vigencia Fin', width: 150, halign: 'center',align: 'center',
                editor: {
                    type: 'datebox',
                    options: {
                        required: true,
                        formatter: myformatter,
                        parser: myparser

                    }
                }
            }
            , { field: 'actualizado', hidden: true }
        ]],
        onBeforeEdit: function (index, row) {
            row.editing = true;
        },
        onAfterEdit: function (index, row) {
            row.editing = false;
        },
        onCancelEdit: function (index, row) {
            row.editing = false;
        },
        
        onClickRow: function (rowIndex, rowdata)
            {
                if (!existeRegistroEnEdicion("dgv_comision"))
                {
                    $("#btn_comision_quitar").linkbutton('enable');
                }
            },
        onDblClickRow: function (rowIndex, rowdata) {
            if (ActionComisionUrl._estado_registro == "0") { return false; }

            if (!existeRegistroEnEdicion("dgv_comision")) {
                $('#dgv_comision').datagrid('beginEdit', rowIndex);

                var Editor = $("#dgv_comision").datagrid("getEditor", { index: rowIndex, field: 'valor' });
                if ($(Editor.target).data('numberbox')) {
                    //$(Editor.target).numberbox({ validType: 'mayorACero[0]' });
                    $(Editor.target).numberbox('textbox').css('text-align', 'right');                    
                }


                rowdata.editing = true;
                rowdata.confirmado = true;
                $("#btn_comision_crear").linkbutton('disable');
                $("#btn_comision_aceptar").linkbutton('enable');
                $("#btn_comision_cancelar").linkbutton('enable');
                $("#btn_comision_quitar").linkbutton('disable');                
            }
        }
    });
    //$('#dgv_comision').datagrid('enableFilter');
}

function fnConfigurarGrillaComisionSupervisor() {

    $('#dgv_comision_supervisor').datagrid({
        fitColumns: true,
        idField: 'codigo_comision',
        toolbar: "#tlb_regla_calculo_precio_s",
        rownumbers: true,
        pageList: [30, 50, 100, 200, 400, 500],
        pageSize: 30,
        pagination: true,
        singleSelect: true,
        columns:
        [[
            { field: 'codigo_comision', hidden: true },
            { field: 'codigo_precio', hidden: true },
            { field: 'nombre_tipo_venta', title: 'Tipo Venta', width: 150, halign: 'center', align: 'left' },
            {
                field: 'codigo_canal_grupo', title: 'Canal de Venta', width: 180, halign: 'center', align: 'left',

                editor: {
                    type: 'combobox',
                    options: {
                        valueField: 'id',
                        textField: 'text',
                        data: JsonCanalVenta,
                        editable: false,
                        required: true
                    }
                },
                formatter: canalVentaFormatter
            },
            {
                field: 'codigo_tipo_pago', title: 'Tipo Pago', width: 180, halign: 'center', align: 'left', editor: {
                    type: 'combobox',
                    options: {
                        valueField: 'id',
                        textField: 'text',
                        data: JsonTipoPago,
                        editable: false,
                        required: true
                    }
                },
                formatter: tipoPagoFormatter
            },
            {
                field: 'codigo_tipo_comision_supervisor', title: 'Comisiona por', width: 180, halign: 'center', align: 'left', editor: {
                    type: 'combobox',
                    options: {
                        valueField: 'id',
                        textField: 'text',
                        data: JsonTipoComisionSupervisor,
                        editable: false,
                        required: true
                    }
                },
                formatter: tipoComisionSupervisorFormatter
            },
            {
                field: 'valor', title: 'Valor', width: 130, halign: 'center', align: 'right',
                formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                },
                editor: {
                    type: 'numberbox',
                    options: {
                        precision: 2,
                        required: true,
                        min: 0,
                        max: 99999999
                    }
                }
            },
              {
                  field: 'str_vigencia_inicio', title: 'Vigencia Inicio', width: 150, halign: 'center', align: 'center',
                  editor: {
                      type: 'datebox',
                      options: {
                          required: true,
                          validType: "fechaMenorIgualA['td[field=\"str_vigencia_fin\"] input.datebox-f',\'Fecha Vigencia Fin\',\'dd/MM/yyyy\']",
                          formatter: myformatter,
                          parser: myparser
                      }
                  }
              },
            {
                field: 'str_vigencia_fin', title: 'Vigencia Fin', width: 150, halign: 'center', align: 'center',
                editor: {
                    type: 'datebox',
                    options: {
                        required: true,
                        formatter: myformatter,
                        parser: myparser

                    }
                }
            }
            , { field: 'actualizado', hidden: true }
        ]],
        onBeforeEdit: function (index, row) {
            row.editing = true;
        },
        onAfterEdit: function (index, row) {
            row.editing = false;
        },
        onCancelEdit: function (index, row) {
            row.editing = false;
        },

        onClickRow: function (rowIndex, rowdata) {
            if (!existeRegistroEnEdicion("dgv_comision_supervisor")) {
                $("#btn_comision_quitar_s").linkbutton('enable');
            }
        },
        onDblClickRow: function (rowIndex, rowdata) {
            if (ActionComisionUrl._estado_registro == "0") { return false; }

            if (!existeRegistroEnEdicion("dgv_comision_supervisor")) {
                $('#dgv_comision_supervisor').datagrid('beginEdit', rowIndex);

                var Editor = $("#dgv_comision_supervisor").datagrid("getEditor", { index: rowIndex, field: 'valor' });
                if ($(Editor.target).data('numberbox')) {
                    //$(Editor.target).numberbox({ validType: 'mayorACero[0]' });
                    $(Editor.target).numberbox('textbox').css('text-align', 'right');
                }


                rowdata.editing = true;
                rowdata.confirmado = true;
                $("#btn_comision_crear_s").linkbutton('disable');
                $("#btn_comision_aceptar_s").linkbutton('enable');
                $("#btn_comision_cancelar_s").linkbutton('enable');
                $("#btn_comision_quitar_s").linkbutton('disable');
            }
        }
    });
    //$('#dgv_comision_supervisor').datagrid('enableFilter');
}

function NuevaComision() {
    if (!existeRegistroEnEdicion('dgv_comision')) {
        var new_row = {
            codigo_regla: -new Date().toString("ddHHmmss")
            , codigo_precio: ActionComisionUrl._codigo_precio
            , codigo_canal: null
            , codigo_tipo_pago: null
            , codigo_tipo_comision: null
            , valor: 0.00
            , str_vigencia_inicio: $("#div_registrar_comision #dtp_fecha_vigencia_inicio").val()
            , str_vigencia_fin: $("#div_registrar_comision #dtp_fecha_vigencia_fin").val()
            , nombre_tipo_venta: ActionComisionUrl.nombre_tipo_venta
            , confirmado: false
            , actualizado : 0
        }

        $("#dgv_comision").datagrid("appendRow", new_row);
        var editIndex = $("#dgv_comision").datagrid("getRowIndex", new_row.codigo_regla);
        $("#dgv_comision").datagrid("selectRow", editIndex);
        $("#dgv_comision").datagrid("beginEdit", editIndex);

        var Editor = $("#dgv_comision").datagrid("getEditor", { index: editIndex, field: 'valor' });
        if ($(Editor.target).data('numberbox')) {
            //$(Editor.target).numberbox({ validType: 'mayorACero[0]' });
            $(Editor.target).numberbox('textbox').css('text-align', 'right');
        }


        $("#btn_comision_crear").linkbutton('disable'); 
        $("#btn_comision_aceptar").linkbutton('enable');
        $("#btn_comision_cancelar").linkbutton('enable');
        $("#btn_comision_quitar").linkbutton('disable');
        //$('#dgv_comision').datagrid('clearSelections');

        
    }
}

function guardarComision() {
    if (existeRegistroEnEdicionParaGrabar("dgv_comision")) {
        var rowIndex = IndexRowEditing("dgv_comision");
        var rowdata = $('#dgv_comision').datagrid("getRows")[rowIndex];
        var v_count = $('#dgv_comision').datagrid("getRows").length;
        var precioData = $('#dgv_precio_articulo').datagrid("getSelected");

        if (validarRowGrilla(rowIndex, 'dgv_comision')) {

            /****************************************************************************************************************************/

            var dg = $('#dgv_precio_articulo');
            var rowIndex_row = $('#dgv_precio_articulo').datagrid("getRowIndex", rowdata.codigo_precio);
            var row_precio = $('#dgv_precio_articulo').datagrid("getRows")[rowIndex_row];


            var cmb_canal_venta = $("#dgv_comision").datagrid("getEditor", { index: rowIndex, field: 'codigo_canal' });
            var cmb_codigo_tipo_pago = $("#dgv_comision").datagrid("getEditor", { index: rowIndex, field: 'codigo_tipo_pago' });
            var cmb_codigo_tipo_comision = $("#dgv_comision").datagrid("getEditor", { index: rowIndex, field: 'codigo_tipo_comision' });

            var dtb_comision_vigencia_inicio = $("#dgv_comision").datagrid("getEditor", { index: rowIndex, field: 'str_vigencia_inicio' });
            var dtb_comision_vigencia_fin = $("#dgv_comision").datagrid("getEditor", { index: rowIndex, field: 'str_vigencia_fin' });


            var dtb_comision_valor = $("#dgv_comision").datagrid("getEditor", { index: rowIndex, field: 'valor' });

            var v_codigo_canal = cmb_canal_venta.target.combobox("getValue");
            var v_codigo_tipo_pago = cmb_codigo_tipo_pago.target.combobox("getValue");
            var v_codigo_tipo_comision = cmb_codigo_tipo_comision.target.combobox("getValue");

            var v_comision_vigencia_inicio = dtb_comision_vigencia_inicio.target.datebox("getValue");
            var v_comision_vigencia_fin = dtb_comision_vigencia_fin.target.datebox("getValue");

            var v_valor = parseFloat(dtb_comision_valor.target.numberbox("getValue"));

            var v_precio = {
                precio_total: parseFloat(row_precio.precio_total),
                vigencia_inicio: row_precio.str_vigencia_inicio,
                vigencia_fin: row_precio.str_vigencia_fin
            };

            var v_comision = {
                codigo_regla: rowdata.codigo_regla,
                codigo_canal: v_codigo_canal,
                codigo_tipo_pago: v_codigo_tipo_pago,
                codigo_tipo_comision: v_codigo_tipo_comision,
                valor: v_valor,
                vigencia_inicio: v_comision_vigencia_inicio,
                vigencia_fin: v_comision_vigencia_fin
            };

            var rows = $("#dgv_comision").datagrid("getRows");
            var lst_regla_calculo_comision = [];
            var v_comision_original;

            $.each(rows, function (index, data) {
                if (rowIndex != index) {
                    var v_detalle = {
                        codigo_regla: data.codigo_regla,
                        codigo_canal: data.codigo_canal,
                        codigo_tipo_pago: data.codigo_tipo_pago,
                        codigo_tipo_comision: data.codigo_tipo_comision,
                        valor: 0.0,
                        vigencia_inicio: data.str_vigencia_inicio,
                        vigencia_fin: data.str_vigencia_fin
                    };
                    lst_regla_calculo_comision.push(v_detalle);
                }
                else {
                    v_comision_original = {
                        codigo_regla: data.codigo_regla,
                        codigo_canal: data.codigo_canal,
                        codigo_tipo_pago: data.codigo_tipo_pago,
                        codigo_tipo_comision: data.codigo_tipo_comision,
                        valor: data.valor,
                        vigencia_inicio: data.str_vigencia_inicio,
                        vigencia_fin: data.str_vigencia_fin
                    };
                }
            });

            var v_validacion = {
                codigo_empresa: precioData.codigo_empresa,
                codigo_canal: v_codigo_canal,
                codigo_tipo_venta: precioData.codigo_tipo_venta,
                codigo_articulo: ActionArticuloUrl._codigo_articulo,
                codigo_tipo_planilla: tipoComisionVendedor
            };

            $.messager.confirm('Confirmaci&oacute;n', '&iquest;Est&aacute; seguro que desea grabar?', function (result) {
                if (result) {
                    $.ajax({
                        type: 'post',
                        url: ActionComisionUrl._ValidarRangoFechaComision,
                        data: JSON.stringify({ v_precio: v_precio, v_comision: v_comision, lst_regla_calculo_comision: lst_regla_calculo_comision, v_comision_original: v_comision_original, v_validacion: v_validacion }),
                        async: false,
                        cache: false,
                        dataType: 'json',
                        contentType: 'application/json; charset=utf-8',
                        success: function (data_respueta) {
                            if (data_respueta.v_resultado == 1) {
                                //alert(row_index+'/'+rowIndex);
                                /******************************************************************************/
                                rowdata.confirmado = true;
                                rowdata.actualizado = 1;
                                $('#dgv_comision').datagrid('endEdit', rowIndex);
                                var v_existe = false;
                                $.each(lst_comision, function (i, data) {
                                    if (data.codigo_regla == rowdata.codigo_regla) {
                                        v_existe = true;
                                        $.extend(data, rowdata);
                                        rowdata.valor = parseFloat(rowdata.valor);
                                        return false;
                                    }
                                });
                                if (!v_existe)
                                    lst_comision.push(rowdata);
                                /**************************************************/                               

                                //var dg = $('#dgv_precio_articulo');
                                //var rowIndex_row = $('#dgv_precio_articulo').datagrid("getRowIndex", rowdata.codigo_precio);

                                dg.datagrid('updateRow', {
                                    index: rowIndex_row,
                                    row: { tiene_comision: v_count>0 }
                                });
                                /******************************************************************************/
                                $("#btn_comision_crear").linkbutton('enable');
                                $("#btn_comision_aceptar").linkbutton('disable');
                                $("#btn_comision_cancelar").linkbutton('disable');
                                $("#btn_comision_quitar").linkbutton('disable');
                                $('#dgv_comision').datagrid('clearSelections');                                
                            }
                            else {

                                $.messager.alert('Error en la operación.', data_respueta.v_mensaje, 'error');
                            }
                        },
                        error: function () {
                            $.messager.alert('Error', "Error en la operación", 'error');
                        }
                    });
                }
            });
            /*****************************************************************************************************************************/




            //console.log('**********************************');
            //console.log(lst_comision);



        }//fin validar grilla
    }
}

function EliminarComision() {
    if (!existeRegistroEnEdicion("dgv_comision")) {
        var rows = $("#dgv_comision").datagrid("getSelections");
        if (rows.length <= 0) {
            $.messager.alert("Eliminar comisión", "Seleccione un registro.", "warning");
        }
        else {
            var row = $("#dgv_comision").datagrid("getSelected");
            $(".messager-window,.window-shadow,.window-mask").remove();

            if (row) {
                $.messager.confirm('Confirmar', '&iquest;Est&aacute; seguro de eliminar este registro?', function (r) {
                    if (r) {
                        var index = $("#dgv_comision").datagrid("getRowIndex", row.codigo_regla);

                        /**************************************************/
                        var index_remove = -1;
                        $.each(lst_comision, function (i, data) {
                            if (data.codigo_regla == row.codigo_regla) {
                                index_remove = i;
                                return false;
                            }

                        });
                        if (index_remove>=0)
                        {
                            lst_comision.splice(index_remove, 1);
                        }
                        
                        if (parseInt(row.codigo_regla) > 0)
                        {
                            lst_comision_eliminar.push(row);
                        }
                        
                        /**************************************************/
                        $('#dgv_comision').datagrid('deleteRow', index);


                        var rowIndex_row = $('#dgv_precio_articulo').datagrid("getRowIndex", row.codigo_precio);
                        var row_precio = $('#dgv_precio_articulo').datagrid("getRows")[rowIndex_row];
                        var v_count = $('#dgv_comision').datagrid("getRows").length;
                        $('#dgv_precio_articulo').datagrid('updateRow', {
                            index: rowIndex_row,
                            row: { tiene_comision: v_count > 0 }
                        });

                        $("#btn_comision_crear").linkbutton('enable');
                        $("#btn_comision_aceptar").linkbutton('disable');
                        $("#btn_comision_cancelar").linkbutton('disable');
                        $("#btn_comision_quitar").linkbutton('disable');
                        $('#dgv_comision').datagrid('clearSelections');
                    }
                });
            }
        }
    }
}

function ImportarConfiguracionComision() {
    alert("importar");
}

function cancelarComision() {
    //alert(dgv);
    var rowIndex = IndexRowEditing("dgv_comision");
    if (rowIndex != null) {

        var row_comision = $('#dgv_comision').datagrid("getRows")[rowIndex];
        if (row_comision.confirmado) {
            $('#dgv_comision').datagrid('cancelEdit', rowIndex);
        }
        else {
            $('#dgv_comision').datagrid('deleteRow', rowIndex);
        }      
        


        $("#btn_comision_crear").linkbutton('enable');
        $("#btn_comision_quitar").linkbutton('disable');
        $("#btn_comision_cancelar").linkbutton('disable');
        $("#btn_comision_aceptar").linkbutton('disable');
        $('#dgv_comision').datagrid('clearSelections');
    }
}

function NuevaComisionSupervisor() {
    if (!existeRegistroEnEdicion('dgv_comision_supervisor')) {
        var new_row = {
            codigo_comision: -new Date().toString("ddHHmmss")
            , codigo_precio: ActionComisionUrl._codigo_precio
            , codigo_canal_grupo: null
            , codigo_tipo_pago: null
            , codigo_tipo_comision_supervisor: null
            , valor: 0.00
            , str_vigencia_inicio: $("#div_registrar_comision #dtp_fecha_vigencia_inicio").val()
            , str_vigencia_fin: $("#div_registrar_comision #dtp_fecha_vigencia_fin").val()
            , nombre_tipo_venta: ActionComisionUrl.nombre_tipo_venta
            , confirmado: false
            , actualizado: 0
        }

        $("#dgv_comision_supervisor").datagrid("appendRow", new_row);
        var editIndex = $("#dgv_comision_supervisor").datagrid("getRowIndex", new_row.codigo_comision);
        $("#dgv_comision_supervisor").datagrid("selectRow", editIndex);
        $("#dgv_comision_supervisor").datagrid("beginEdit", editIndex);

        var Editor = $("#dgv_comision_supervisor").datagrid("getEditor", { index: editIndex, field: 'valor' });
        if ($(Editor.target).data('numberbox')) {
            //$(Editor.target).numberbox({ validType: 'mayorACero[0]' });
            $(Editor.target).numberbox('textbox').css('text-align', 'right');
        }


        $("#btn_comision_crear_s").linkbutton('disable');
        $("#btn_comision_aceptar_s").linkbutton('enable');
        $("#btn_comision_cancelar_s").linkbutton('enable');
        $("#btn_comision_quitar_s").linkbutton('disable');
    }
}

function cancelarComisionSupervisor() {
    var rowIndex = IndexRowEditing("dgv_comision_supervisor");
    if (rowIndex != null) {

        var row_comision = $('#dgv_comision_supervisor').datagrid("getRows")[rowIndex];
        if (row_comision.confirmado) {
            $('#dgv_comision_supervisor').datagrid('cancelEdit', rowIndex);
        }
        else {
            $('#dgv_comision_supervisor').datagrid('deleteRow', rowIndex);
        }

        $("#btn_comision_crear_s").linkbutton('enable');
        $("#btn_comision_quitar_s").linkbutton('disable');
        $("#btn_comision_cancelar_s").linkbutton('disable');
        $("#btn_comision_aceptar_s").linkbutton('disable');
        $('#dgv_comision_supervisor').datagrid('clearSelections');
    }
}

function EliminarComisionSupervisor() {
    if (!existeRegistroEnEdicion("dgv_comision_supervisor")) {
        var rows = $("#dgv_comision_supervisor").datagrid("getSelections");
        if (rows.length <= 0) {
            $.messager.alert("Eliminar comisión", "Seleccione un registro.", "warning");
        }
        else {
            var row = $("#dgv_comision_supervisor").datagrid("getSelected");
            $(".messager-window,.window-shadow,.window-mask").remove();

            if (row) {
                $.messager.confirm('Confirmar', '&iquest;Est&aacute; seguro de eliminar este registro?', function (r) {
                    if (r) {
                        var index = $("#dgv_comision_supervisor").datagrid("getRowIndex", row.codigo_comision);

                        /**************************************************/
                        var index_remove = -1;
                        $.each(lst_comision_supervisor, function (i, data) {
                            if (data.codigo_comision == row.codigo_comision) {
                                index_remove = i;
                                return false;
                            }

                        });
                        if (index_remove >= 0) {
                            lst_comision_supervisor.splice(index_remove, 1);
                        }

                        if (parseInt(row.codigo_comision) > 0) {
                            lst_comision_supervisor_eliminar.push(row);
                        }

                        /**************************************************/
                        $('#dgv_comision_supervisor').datagrid('deleteRow', index);


                        var rowIndex_row = $('#dgv_precio_articulo').datagrid("getRowIndex", row.codigo_precio);
                        var row_precio = $('#dgv_precio_articulo').datagrid("getRows")[rowIndex_row];
                        var v_count = $('#dgv_comision_supervisor').datagrid("getRows").length;

                        //$('#dgv_precio_articulo').datagrid('updateRow', {
                        //    index: rowIndex_row,
                        //    row: { tiene_comision: v_count > 0 }
                        //});

                        $("#btn_comision_crear_s").linkbutton('enable');
                        $("#btn_comision_aceptar_s").linkbutton('disable');
                        $("#btn_comision_cancelar_s").linkbutton('disable');
                        $("#btn_comision_quitar_s").linkbutton('disable');
                        $('#dgv_comision_supervisor_s').datagrid('clearSelections');
                    }
                });
            }
        }
    }
}

function guardarComisionSupervisor() {
    if (existeRegistroEnEdicionParaGrabar("dgv_comision_supervisor")) {
        var rowIndex = IndexRowEditing("dgv_comision_supervisor");
        var rowdata = $('#dgv_comision_supervisor').datagrid("getRows")[rowIndex];
        var v_count = $('#dgv_comision_supervisor').datagrid("getRows").length;
        var precioData = $('#dgv_precio_articulo').datagrid("getSelected");

        if (validarRowGrilla(rowIndex, 'dgv_comision_supervisor')) {

            /****************************************************************************************************************************/

            var dg = $('#dgv_precio_articulo');
            var rowIndex_row = $('#dgv_precio_articulo').datagrid("getRowIndex", rowdata.codigo_precio);
            var row_precio = $('#dgv_precio_articulo').datagrid("getRows")[rowIndex_row];


            var cmb_canal_venta = $("#dgv_comision_supervisor").datagrid("getEditor", { index: rowIndex, field: 'codigo_canal_grupo' });
            var cmb_codigo_tipo_pago = $("#dgv_comision_supervisor").datagrid("getEditor", { index: rowIndex, field: 'codigo_tipo_pago' });
            var cmb_codigo_tipo_comision_supervisor = $("#dgv_comision_supervisor").datagrid("getEditor", { index: rowIndex, field: 'codigo_tipo_comision_supervisor' });

            var dtb_comision_vigencia_inicio = $("#dgv_comision_supervisor").datagrid("getEditor", { index: rowIndex, field: 'str_vigencia_inicio' });
            var dtb_comision_vigencia_fin = $("#dgv_comision_supervisor").datagrid("getEditor", { index: rowIndex, field: 'str_vigencia_fin' });


            var dtb_comision_valor = $("#dgv_comision_supervisor").datagrid("getEditor", { index: rowIndex, field: 'valor' });

            var v_codigo_canal = cmb_canal_venta.target.combobox("getValue");
            var v_codigo_tipo_pago = cmb_codigo_tipo_pago.target.combobox("getValue");
            var v_codigo_tipo_comision_supervisor = cmb_codigo_tipo_comision_supervisor.target.combobox("getValue");

            var v_comision_vigencia_inicio = dtb_comision_vigencia_inicio.target.datebox("getValue");
            var v_comision_vigencia_fin = dtb_comision_vigencia_fin.target.datebox("getValue");

            var v_valor = parseFloat(dtb_comision_valor.target.numberbox("getValue"));

            var v_precio = {
                precio_total: parseFloat(row_precio.precio_total),
                vigencia_inicio: row_precio.str_vigencia_inicio,
                vigencia_fin: row_precio.str_vigencia_fin
            };

            var v_comision = {
                codigo_comision: rowdata.codigo_comision,
                codigo_canal_grupo: v_codigo_canal,
                codigo_tipo_pago: v_codigo_tipo_pago,
                codigo_tipo_comision_supervisor: v_codigo_tipo_comision_supervisor,
                valor: v_valor,
                vigencia_inicio: v_comision_vigencia_inicio,
                vigencia_fin: v_comision_vigencia_fin
            };

            var rows = $("#dgv_comision_supervisor").datagrid("getRows");
            var lst_regla_calculo_comision = [];
            var v_comision_original;

            $.each(rows, function (index, data) {
                if (rowIndex != index) {
                    var v_detalle = {
                        codigo_comision: data.codigo_comision,
                        codigo_canal_grupo: data.codigo_canal_grupo,
                        codigo_tipo_pago: data.codigo_tipo_pago,
                        codigo_tipo_comision_supervisor: data.codigo_tipo_comision_supervisor,
                        valor: 0.0,
                        vigencia_inicio: data.str_vigencia_inicio,
                        vigencia_fin: data.str_vigencia_fin
                    };
                    lst_regla_calculo_comision.push(v_detalle);
                }
                else {
                    v_comision_original = {
                        codigo_comision: data.codigo_comision,
                        codigo_canal_grupo: data.codigo_canal_grupo,
                        codigo_tipo_pago: data.codigo_tipo_pago,
                        codigo_tipo_comision_supervisor: data.codigo_tipo_comision_supervisor,
                        valor: data.valor,
                        vigencia_inicio: data.str_vigencia_inicio,
                        vigencia_fin: data.str_vigencia_fin
                    };
                }
            });

            var v_validacion = {
                codigo_empresa: precioData.codigo_empresa,
                codigo_canal: v_codigo_canal,
                codigo_tipo_venta: precioData.codigo_tipo_venta,
                codigo_articulo: ActionArticuloUrl._codigo_articulo,
                codigo_tipo_planilla: tipoComisionSupervisor
            };

            $.messager.confirm('Confirmaci&oacute;n', '&iquest;Est&aacute; seguro que desea grabar?', function (result) {
                if (result) {
                    $.ajax({
                        type: 'post',
                        url: ActionComisionUrl._ValidarRangoFechaComisionSupervisor,
                        data: JSON.stringify({ v_precio: v_precio, v_comision: v_comision, lst_regla_calculo_comision: lst_regla_calculo_comision, v_comision_original: v_comision_original, v_validacion: v_validacion }),
                        async: false,
                        cache: false,
                        dataType: 'json',
                        contentType: 'application/json; charset=utf-8',
                        success: function (data_respueta) {
                            if (data_respueta.v_resultado == 1) {
                                //alert(row_index+'/'+rowIndex);
                                /******************************************************************************/
                                rowdata.confirmado = true;
                                $('#dgv_comision_supervisor').datagrid('endEdit', rowIndex);
                                var v_existe = false;
                                $.each(lst_comision_supervisor, function (i, data) {
                                    if (data.codigo_comision == rowdata.codigo_comision) {
                                        v_existe = true;
                                        $.extend(data, rowdata);
                                        rowdata.valor = parseFloat(rowdata.valor);
                                        rowdata.actualizado = 1;
                                        return false;
                                    }

                                });
                                if (!v_existe)
                                    lst_comision_supervisor.push(rowdata);
                                /**************************************************/

                                //var dg = $('#dgv_precio_articulo');
                                //var rowIndex_row = $('#dgv_precio_articulo').datagrid("getRowIndex", rowdata.codigo_precio);

                                //dg.datagrid('updateRow', {
                                //    index: rowIndex_row,
                                //    row: { tiene_comision: v_count > 0 }
                                //});
                                /******************************************************************************/
                                $("#btn_comision_crear_s").linkbutton('enable');
                                $("#btn_comision_aceptar_s").linkbutton('disable');
                                $("#btn_comision_cancelar_s").linkbutton('disable');
                                $("#btn_comision_quitar_s").linkbutton('disable');
                                $('#dgv_comision_supervisor').datagrid('clearSelections');
                            }
                            else {

                                $.messager.alert('Error en la operación.', data_respueta.v_mensaje, 'error');
                            }
                        },
                        error: function () {
                            $.messager.alert('Error', "Error en la operación", 'error');
                        }
                    });
                }
            });

        }//fin validar grilla
    }
}
