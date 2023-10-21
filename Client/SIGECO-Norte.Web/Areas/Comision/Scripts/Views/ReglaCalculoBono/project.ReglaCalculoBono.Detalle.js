var DetalleUrls = {};
var JsonCanal = {};
var JsonGrupo = {};
var Registro = {};
var LongitudGrupo = 0;
var MontoGlobal = 0;
var PorcentajePagoGlobal = 0;
var CodigoArticuloBusqueda = '';

;(function (app) {
    //===========================================================================================
    var current = app.ReglaCalculoBonoDetalle = {};
    //===========================================================================================

    jQuery.extend(app.ReglaCalculoBonoDetalle,
        {
            EditType: '',
            HasRootNode: 'False',

            Initialize: function (detalleUrls) {
                jQuery.extend(DetalleUrls, detalleUrls);
                InicializarListadoMontos();
                InicializarListadoArticulos();
                GetArticulosJson();
                GetMatrizJson();
            }
        })
})(project);


function InicializarListadoMontos () {

    $('#dgMontosDetalle').datagrid({
        idField: 'codigo_registro',
        fitColumns: true,
        data: null,
        pagination: false,
        singleSelect: true,
        pageNumber: 0,
        pageList: [5, 10, 15, 20, 25, 30],
        pageSize: 10,
        columns:
        [[
            { field: 'codigo_registro', hidden: 'true' },
            {
                field: 'monto_meta', title: 'Monto Meta', width: 80, align: 'right', halign: 'center',
                formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                }
            },
            {
                field: 'porcentaje_meta', title: 'Porcentaje Meta', width: 80, align: 'right',halign: 'center',
                editor: {
                    type: 'numberbox',
                    options: {
                        precision: 2,
                        required: true,
                        max: 99
                    }
                }
            },
            {
                field: 'porcentaje_pago', title: 'Porcentaje a Pagar', width: 100, align: 'right',halign: 'center',
                editor: {
                    type: 'numberbox',
                    options: {
                        precision: 2,
                        required: true,
                        max: 99
                    }
                }
            }
        ]],
        onSelect: function (index, row) {
            $(this).datagrid('unselectRow', index);
        }
    });
}

function InicializarListadoArticulos() {

    $('#dgArticulosDetalle').datagrid({
        idField: 'codigo_articulo',
        fitColumns: true,
        data: null,
        pagination: false,
        singleSelect: true,
        pageNumber: 0,
        pageList: [5, 10, 15, 20, 25, 30],
        pageSize: 10,
        columns:
        [[
            { field: 'codigo_articulo', hidden: 'true' },
            {
                field: 'nombre_articulo', title: 'Artículo', width: 80, align: 'left', halign: 'center',
            },
            {
                field: 'cantidad', title: 'Cantidad', width: 80, align: 'right', halign: 'center',
            }
        ]],
        onSelect: function (index, row) {
            $(this).datagrid('unselectRow', index);
        }
    });
}


function GetArticulosJson() {
    var codigo_regla_calculo_bono = DetalleUrls.codigo_regla_calculo_bono;

    if (parseInt(codigo_regla_calculo_bono) == -1)
    {
        return false;
    }

    $.ajax({
        type: 'post',
        url: DetalleUrls.GetArticulosJson,
        data: { codigo_regla_calculo_bono: codigo_regla_calculo_bono },
        async: false,
        cache: false,
        dataType: 'json',
        success: function (data) {
            if (data.Msg) {
                project.AlertErrorMessage('Error', data.Msg);
            }
            else {
                $('#dgArticulosDetalle').datagrid('loadData', data);
            }
        },
        error: function () {
            project.AlertErrorMessage('Error', 'Error');
        }
    });
}

function GetMatrizJson() {
    debugger;
    var codigo_regla_calculo_bono = DetalleUrls.codigo_regla_calculo_bono;

    if (parseInt(codigo_regla_calculo_bono) == -1) {
        return false;
    }

    $.ajax({
        type: 'post',
        url: DetalleUrls.GetMatrizJson,
        data: { codigo_regla_calculo_bono: codigo_regla_calculo_bono },
        async: false,
        cache: false,
        dataType: 'json',
        success: function (data) {
            if (data.Msg) {
                project.AlertErrorMessage('Error', data.Msg);
            }
            else {
                $('#dgMontosDetalle').datagrid('loadData', data);
            }
        },
        error: function () {
            project.AlertErrorMessage('Error', 'Error');
        }
    });
}

function Cancelar() {
    $('#dlgDetalle').dialog('close');
}
