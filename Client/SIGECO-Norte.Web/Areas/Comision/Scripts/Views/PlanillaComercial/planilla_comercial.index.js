var ActionIndexUrl = {};

; (function (app) {
    //===========================================================================================
    var current = app.index = {};
    //===========================================================================================

    jQuery.extend(app.index,
        {
            Initialize: function (actionUrls) {
                jQuery.extend(ActionIndexUrl, actionUrls);
                fnInicializarPlanillaComercial();
                fnConfigurarGrillaPlanillaComercial();
            }
        })
})(project);

function fnInicializarPlanillaComercial() {
}

function fnConsultarPlanilla() {
    $('#dgv_planilla_comercial').datagrid('reload');
}

function fnConfigurarGrillaPlanillaComercial() {
    $('#dgv_planilla_comercial').datagrid({
        fitColumns: true,
        idField: 'codigo_planilla',
        url: ActionIndexUrl._Buscar,
        data: null,
        height: '550',        
        pagination: true,
        singleSelect: true,
        remoteFilter: false,
        rownumbers: true,
        pageList: [60, 100, 150,500],
        pageSize: 60,
        columns:
        [[
            { field: 'codigo_planilla', title: 'Codigo', hidden: 'true' },
            { field: 'numero_planilla', title: 'Nro. Planilla', width: 120, align: 'center' },
            { field: 'nombre_regla_tipo_planilla', title: 'Planilla', width: 200, align: 'left' },
            { field: 'nombre_tipo_planilla', title: 'Tipo<br>Planilla', width: 130, align: 'left' },
            { field: 'fecha_inicio', title: 'Fecha<br>Inicio', width: 120, align: 'center' },
            { field: 'fecha_fin', title: 'Fecha<br>Fin', width: 120, align: 'center' },
            { field: 'nombre_estado_planilla', title: 'Estado', width: 120, align: 'center', styler: cellStylerEstadoPlanilla },
            { field: 'fecha_apertura', title: 'Fecha<br>Apertura', width: 120, align: 'center' },
            { field: 'fecha_cierre', title: 'Fecha<br>Cierre', width: 120, align: 'center' },
            { field: 'fecha_anulacion', title: 'Fecha<br>Anulación', width: 120, align: 'center' },
            { field: 'estilo', hidden: 'true' }
        ]],
        onClickRow: function (index, row) {

        }
    });
    $('#dgv_planilla_comercial').datagrid('enableFilter');
    $(window).resize(function () {
        $('#dgv_planilla_comercial').datagrid('resize');
    });
}

function fnPlanillaComercial() {
    var row = $('#dgv_planilla_comercial').datagrid('getSelected');
    console.log(row);
    if (!row) {
        $.messager.alert('Consulta Planilla', "Para continuar con el proceso seleccione un registro.", 'warning');
    }

    $(this).AbrirVentanaEmergente({
        parametro: "?p_codigo_planilla=" + row.codigo_planilla,
        div: 'div_planilla_comercial',
        title: "Consulta Planilla",
        url: ActionIndexUrl._Planilla
    });
}

function cellStylerEstadoPlanilla(value, row, index) {
    return row.estilo;
}
