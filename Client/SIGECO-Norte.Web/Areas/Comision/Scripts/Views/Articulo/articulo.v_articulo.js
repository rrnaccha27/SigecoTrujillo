var ActionVArticuloUrl = {};
var JsonMoneda = {};
var JsonEmpresa = {};
var JsonTipoVenta = {};

var lst_comision_v = [];
var lst_comision_eliminar_v = [];
var lista_precio_articulo_eliminado = [];
var lst_comision_supervisor_v = [];
var lst_comision_supervisor_eliminar_v = [];

function monedaFormatter(value) {

    for (var i = 0; i < JsonMoneda.length; i++) {
        if (JsonMoneda[i].id == value) return JsonMoneda[i].text;
    }
    return value;
}
function tipoVentaFormatter(value) {

    for (var i = 0; i < JsonTipoVenta.length; i++) {
        if (JsonTipoVenta[i].id == value) return JsonTipoVenta[i].text;
    }
    return value;
}
function empresaFormatter(value) {

    for (var i = 0; i < JsonEmpresa.length; i++) {
        if (JsonEmpresa[i].id == value) return JsonEmpresa[i].text;
    }
    return value;
}

function comisionFormatter(value, row) {
    var link = '<a href="javascript:void(0)" onclick="fnNuevaComision(' + row.codigo_precio + ',' + row.actualizado + ')">Comisión</a>';

    return link;
}

; (function (app) {
    //===========================================================================================
    var current = app.v_articulo = {};
    //===========================================================================================
    jQuery.extend(app.v_articulo,
        {
            Initialize: function (actionUrls) {
                jQuery.extend(ActionVArticuloUrl, actionUrls);
                fnInicializarArticulo();
                fnConfigurarGrillaPrecioArticulo();
                fnListarPrecioByArticulo();
            }
        })
})(project);

function fnInicializarArticulo() {

    JsonEmpresa = $('.content').get_json_combobox({
        url: ActionVArticuloUrl._GetEmpresaJson
    });

    JsonMoneda = $('.content').get_json_combobox({
        url: ActionVArticuloUrl._GetMonedaJson
    });

    JsonTipoVenta = $('.content').get_json_combobox({
        url: ActionVArticuloUrl._GetTipoVentaJson
    });

    $('.content').combobox_sigees({
        id: "#cmb_unidad_negocio_v",
        url: ActionVArticuloUrl._GetUnidadNegocioJson
    });

    $('.content').combobox_sigees({
        id: "#cmb_tipo_articulo_v",
        url: ActionVArticuloUrl._GetTipoArticuloJson
    });

    $('.content').combobox_sigees({
        id: "#cmb_categoria_v",
        url: ActionVArticuloUrl._GetCategoriaJson
    });

    //$('.content').ResizeModal({
    //    widthMax: '90%',
    //    widthMin: '80%',
    //    div: 'div_registrar_articulo'
    //});
}

function fnNuevaComision(p_codigo_precio, p_actualizado) {
    $(this).AbrirVentanaEmergente({
        parametro: "?p_codigo_precio=" + p_codigo_precio + "&&p_estado_registro=" + ActionVArticuloUrl._estado_registro,
        div: 'div_visualizar_comision',
        title: "Datos de Comisión",
        url: ActionVArticuloUrl._UrlComision
    });
}

function fnListarPrecioByArticulo() {
    $.ajax({
        type: 'post',
        url: ActionVArticuloUrl._GetPreciobyArticuloJson,
        data: { codigoArticulo: ActionVArticuloUrl._codigo_articulo },
        async: false,
        cache: false,
        dataType: 'json',
        success: function (data) {
            $('#dgv_precio_articulo_v').datagrid('loadData', data);
        },
        error: function () {
            alert("Error en recuperar precio articulo.");
        }
    });
}

function fnConfigurarGrillaPrecioArticulo() {
    $('#dgv_precio_articulo_v').datagrid({
        fitColumns: false,
        idField: 'codigo_precio',
        rownumbers: true,
        pageList: [30, 50, 100, 200, 400, 500],
        pageSize: 30,
        pagination: true,
        singleSelect: true,
        columns:
        [[
            { field: 'codigo_precio', title: 'Codigo Precio', width: 150, hidden: true },
            {
                field: 'codigo_empresa', title: 'Empresa', width: 120,halign: 'center', align: 'left',
                editor: {
                    type: 'combobox',
                    options: {
                        valueField: 'id',
                        textField: 'text',
                        data: JsonEmpresa,
                        //novalidate: true,
                        editable: false,
                        required: true
                    }
                },
                formatter: empresaFormatter
            },
            {
                field: 'codigo_tipo_venta', title: 'Tipo Venta', width: 180,halign: 'center', align: 'left', editor: {
                    type: 'combobox',
                    options: {
                        valueField: 'id',
                        textField: 'text',
                        data: JsonTipoVenta,
                        editable: false,
                        // novalidate: true,
                        required: true
                    }
                },
                formatter: tipoVentaFormatter
            },
            {
                field: 'codigo_moneda', title: 'Moneda', width: 150, halign: 'center', align: 'left',
                editor: {
                    type: 'combobox',
                    options: {
                        valueField: 'id',
                        textField: 'text',
                        data: JsonMoneda,
                        editable: false,
                        //novalidate: true,
                        required: true
                       // validType: "restricted['#dgv_precio_articulo_v td[field=codigo_moneda] input']",
                    }
                },
                formatter: monedaFormatter
            },
            {
                field: 'precio', title: 'Precio', width: 130, halign: 'center', align: 'right', formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                }
            },
             {
                 field: 'igv', title: 'IGV', width: 130, halign: 'center', align: 'right', formatter: function (value, row) {
                     return $.NumberFormat(value, 2);
                 }
             },
            {
                field: 'precio_total', title: 'Precio Total', width: 130, halign: 'center', align: 'right',
                formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                },
                editor: {
                    type: 'numberbox',
                    options: {
                        precision: 2,
                        required: true,
                        max: 9999999
                    }
                }
            },
            {
                field: 'str_vigencia_inicio', title: 'Fecha Inicio', width: 130, halign: 'center', align: 'center',
                editor: {
                    type: 'datebox',
                    options: {
                        required: true,
                        validType: "fechaMenorIgualA['td[field=\"str_vigencia_fin\"] input.datebox-f',\'Fecha Fin\',\'dd/MM/yyyy\']",
                        formatter: myformatter,
                        parser: myparser
                    }
                }
            },
            {
                field: 'str_vigencia_fin', title: 'Fecha Fin', width: 130, halign: 'center',align: 'center',
                editor: {
                    type: 'datebox',
                    options: {
                        required: true,
                        formatter: myformatter,
                        parser: myparser

                    }
                }
            },
             { field: 'tiene_comision', title: 'Tiene</br>Comisión', width: 120, align: 'center', formatter: function (value, row, index) { if (row.tiene_comision) { return 'Si'; } else { return 'No'; } } },
              
            {
                field: 'codigo_comision', width: 120, title: 'Comisiones', halign: 'center',align: 'center',
                formatter: comisionFormatter
            }
            ,
            {
                field: 'actualizado', hidden: true
            },
            {
                field: 'clonarcomisiones', hidden: true
            }
            ]],

        onBeforeEdit: function (index, row) {
            row.editing = true;
        },
        onAfterEdit: function (index, row) {
            row.editing = false;
        },
        onCancelEdit: function (index, row) {
            row.editing = false;
        }
    });
    $('#dgv_precio_articulo_v').datagrid('enableFilter');
}



function myformatter(date) {
    var y = date.getFullYear();
    var m = date.getMonth() + 1;
    var d = date.getDate();
    return (d < 10 ? ('0' + d) : d) + '/' + (m < 10 ? ('0' + m) : m) + '/' + y;
}
function myparser(s) {
    if (!s) return new Date();
    var ss = s.split('/');
    var y = parseInt(ss[2], 10);
    var m = parseInt(ss[1], 10);
    var d = parseInt(ss[0], 10);
    if (!isNaN(y) && !isNaN(m) && !isNaN(d)) {
        return new Date(y, m - 1, d)
    } else {
        return new Date();
    }
}


function fnReporteArticulo(p_codigo_articulo)
{
    $(this).AbrirVentanaEmergente({
        parametro: "?p_codigo_articulo=" + p_codigo_articulo,
        div: 'div_reporte_articulo',
        title: "Reporte de Artículo",
        url: ActionVArticuloUrl._Reporte
    });
}