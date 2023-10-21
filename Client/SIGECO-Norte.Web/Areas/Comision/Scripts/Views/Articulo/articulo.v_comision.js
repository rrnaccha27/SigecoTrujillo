var ActionVComisionUrl = {};
var JsonCanalVenta = {};
var JsonTipoPago = {};
var JsonComisionadoPor = {};
var JsonTipoComisionSupervisor = {};
var manejarClonComisiones = 0;

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
    var current = app.v_comision = {};
    //===========================================================================================
    jQuery.extend(app.v_comision,
        {
            Initialize: function (actionUrls) {
                jQuery.extend(ActionVComisionUrl, actionUrls);
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
        url: ActionVComisionUrl._GetCanalVentaJson
    });

    JsonTipoPago = $('.content').get_json_combobox({
        url: ActionVComisionUrl._GetTipoPagoJson
    });

    JsonComisionadoPor = $('.content').get_json_combobox({
        url: ActionVComisionUrl._GetComisionadoPorJson
    });

    JsonTipoComisionSupervisor = $('.content').get_json_combobox({
        url: ActionVComisionUrl._GetTipoComisionSupervisorJson
    });

    $('.content').ResizeModal({
        widthMax: '90%',
        widthMin: '80%',
        div: 'div_visualizar_comision'
    });

    var txt_nombre_articulo = $("#div_visualizar_articulo #txt_articulo_nombre").val();
    $("#div_visualizar_comision #txt_articulo_nombre").val(txt_nombre_articulo);

    var v_codigo_precio = ActionVComisionUrl._codigo_precio;

    var rws = $("#dgv_precio_articulo_v").datagrid("getRows");
    var rowIndex = 0;
    $.each(rws, function (i, objeto) {
        if (objeto.codigo_precio == v_codigo_precio) {
            rowIndex = i;
            return false;
        }
    });
    var rowdata = $('#dgv_precio_articulo_v').datagrid("getRows")[rowIndex];

    var txt_nombre_empresa = empresaFormatter(rowdata.codigo_empresa);
    $("#div_visualizar_comision #txt_empresa_nombre").val(txt_nombre_empresa);

    
    var txt_nombre_tipo_venta = tipoVentaFormatter(rowdata.codigo_tipo_venta);
    $("#div_visualizar_comision #txt_tipo_venta_nombre").val(txt_nombre_tipo_venta);
    ActionVComisionUrl.nombre_tipo_venta = txt_nombre_tipo_venta;

    var txt_fecha_vigencia_inicio = rowdata.str_vigencia_inicio;
    $("#div_visualizar_comision #dtp_fecha_vigencia_inicio").val(txt_fecha_vigencia_inicio);

    var txt_fecha_vigencia_fin = rowdata.str_vigencia_fin;
    $("#div_visualizar_comision #dtp_fecha_vigencia_fin").val(txt_fecha_vigencia_fin);

    var txt_nombre_moneda = monedaFormatter(rowdata.codigo_moneda);
    $("#div_visualizar_comision #txt_moneda_nombre").val(txt_nombre_moneda);

    var precio_total = $.NumberFormat(rowdata.precio_total, 2);
    $("#div_visualizar_comision #txt_articulo_precio").val(precio_total);
    
    var precio = $('#dgv_precio_articulo_v').datagrid('getSelected');
    manejarClonComisiones = precio.clonarcomisiones;
}

function fnListarComisionByPrecio() {
    $.each(lst_comision_v, function (index, rows) {
        rows.valor = parseFloat(rows.valor);
        rows.vigencia_inicio = rows.str_vigencia_inicio;
        rows.vigencia_fin = rows.str_vigencia_fin;
    });

    $.ajax({
        type: 'post',
        url: ActionVComisionUrl._GetReglasbyPrecioJson,
        data: JSON.stringify({ codigoPrecio: ActionVComisionUrl._codigo_precio, lst_regla_calculo_comision: lst_comision_v, lst_eliminados: lst_comision_eliminar_v }),
        async: false,
        cache: false,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $('#dgv_comision_v').datagrid('loadData', data);
        },
        error: function () {
            alert("Error en recuperar comisión.");
        }
    });
}

function fnListarComisionByPrecioSupervisor() {

    $.each(lst_comision_supervisor_v, function (index, rows) {
        rows.valor = parseFloat(rows.valor);
        rows.vigencia_inicio = rows.str_vigencia_inicio;
        rows.vigencia_fin = rows.str_vigencia_fin;
    });

    $.ajax({
        type: 'post',
        url: ActionVComisionUrl._GetComisionPrecioSupervisorJson,
        data: JSON.stringify({ codigoPrecio: ActionVComisionUrl._codigo_precio, lst_comision_precio_supervisor: lst_comision_supervisor_v, lst_eliminados: lst_comision_supervisor_eliminar_v }),
        async: false,
        cache: false,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $('#dgv_comision_supervisor_v').datagrid('loadData', data);
        },
        error: function () {
            alert("Error en recuperar comisión.");
        }
    });
}


function fnConfigurarGrillaComision() {

    $('#dgv_comision_v').datagrid({
        fitColumns: true,
        idField: 'codigo_regla',
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
        ]]
    });
    //$('#dgv_comision_v').datagrid('enableFilter');
}

function fnConfigurarGrillaComisionSupervisor() {

    $('#dgv_comision_supervisor_v').datagrid({
        fitColumns: true,
        idField: 'codigo_comision',
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
        ]]
    });
    //$('#dgv_comision_supervisor_v').datagrid('enableFilter');
}

function ImportarConfiguracionComision() {
    alert("importar");
}
