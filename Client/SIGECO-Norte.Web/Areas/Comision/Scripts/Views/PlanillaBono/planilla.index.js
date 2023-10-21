var ActionIndexUrl = {};

; (function (app) {
    //===========================================================================================
    var current = app.index = {};
    //===========================================================================================

    jQuery.extend(app.index,
        {
            Initialize: function (actionUrls) {

                jQuery.extend(ActionIndexUrl, actionUrls);
                fnConfigurarGrillaPlanilla();
                fnConfigurarBotones();
            }
        })
})(project);

function fnNuevaPlanilla()
{
    
    $(this).AbrirVentanaEmergente({
        parametro: "?p_codigo_planilla=" + 0,
         div: 'div_registrar_planilla',
        title: "Generación de Planilla Bono",
        url: ActionIndexUrl._Planilla
    });
}

function fnModificarPlanilla() {

    var row = $('#dgv_planilla').datagrid('getSelected');
    //console.log(row);
    if (!row)
    {
        $.messager.alert('Seleccione registro', "Para continuar con el proceso debe seleccionar un registro.", 'warning');
        return;
    }

    $(this).AbrirVentanaEmergente({
        parametro: "?p_codigo_planilla=" + row.codigo_planilla,
        div: 'div_registrar_planilla',
        title: "Planilla Bono",
        url: ActionIndexUrl._Planilla
    });
}

function fnModificarPlanillaById(pCodigoPlanilla) {
 
    $(this).AbrirVentanaEmergente({
        parametro: "?p_codigo_planilla=" + pCodigoPlanilla,
        div: 'div_registrar_planilla',
        title: "Planilla Bono",
        url: ActionIndexUrl._Planilla
    });
}

function fnConfigurarGrillaPlanilla() {
    $('#dgv_planilla').datagrid({
        fitColumns: true,
        idField: 'codigo_planilla',
        url: ActionIndexUrl._Buscar,
        height: '600',
        rownumbers: true,
        pagination: true,
        pageList: [60, 80, 100, 150],
        pageSize: 60,
        singleSelect: true,
        remoteFilter: false,       
        columns:
        [[
            { field: 'codigo_planilla', title: 'Codigo', hidden: 'true' },
            { field: 'numero_planilla', title: 'Nro. Planilla', width: 100, align: 'center' },
            { field: 'nombre_canal', title: 'Canal', width: 200, align: 'left' },
            { field: 'nombre_tipo_planilla', title: 'Tipo', width: 130, align: 'left' },
            { field: 'fecha_inicio', title: 'Fecha<br>Inicio', width: 120, align: 'center' },
            { field: 'fecha_fin', title: 'Fecha<br>Fin', width: 120, align: 'center' },
            { field: 'nombre_estado_planilla', title: 'Estado', width: 120, align: 'center', styler: cellStylerEstadoPlanilla },
            { field: 'fecha_apertura', title: 'Fecha<br>Apertura', width: 120, align: 'center' },
            { field: 'fecha_cierre', title: 'Fecha<br>Cierre', width: 120, align: 'center' },
            { field: 'fecha_anulacion', title: 'Fecha<br>Anulación', width: 120, align: 'center' },
            { field: 'codigo_estado_planilla', title: 'Estado', hidden: true },
            { field: 'estilo', title: 'Estado', hidden: true },
        ]],
        onClickRow: function (index, row) {
            if (row.codigo_estado_planilla == 2 && row.envio_liquidacion == 0) {
                $("#btnGenerarTxtRRHH").linkbutton('enable');
                $("#btnGenerarTxtCont").linkbutton('enable');
            }
            else {
                $("#btnGenerarTxtRRHH").linkbutton('disable');
                $("#btnGenerarTxtCont").linkbutton('disable');
            }
        },
        loadFilter: function (data) {
            ClearSelection();
            return data;
        }
    });
    $('#dgv_planilla').datagrid('enableFilter');

    $(window).resize(function () {
        $('#dgv_planilla').datagrid('resize');
    }); 
}

function fnConfigurarBotones() {
    $("#btnGenerarTxtRRHH").on("click", function () {
        if (!$("#btnGenerarTxtRRHH").linkbutton("options").disabled) {
            fnGenerarTxtPlanilla();
        }
    });

    $("#btnGenerarTxtCont").on("click", function () {
        if (!$("#btnGenerarTxtCont").linkbutton("options").disabled) {
            fnGenerarTxtPlanillaCont();
        }
    });
}

function ClearSelection() {
    $('#dgv_planilla').datagrid('clearSelections');
}

function fnConsultarPlanilla() {

    var queryParams = {
        numero_planilla: null,
        iCurrentPage: 1,
        iPageSize: -1
    };
    $('#dgv_planilla').datagrid("reload", queryParams);  
}

function cellStylerEstadoPlanilla(value, row, index) {
    return row.estilo;
}

function fnGenerarTxtPlanilla() {
    var row = $('#dgv_planilla').datagrid('getSelected');
    if (!row) {
        $.messager.alert('Seleccione registro', "Para continuar con el proceso debe seleccionar un registro.", 'warning');
        return;
    }
    if (row.codigo_estado_planilla == 2) {
        $.messager.confirm('Confirmaci&oacute;n', '¿Est&aacute; seguro que desea generar el archivo txt?', function (result) {
            if (result) {
                var url = ActionIndexUrl.GenerarTxt + "?id=" + row.codigo_planilla;
                window.open(url, '_blank');
            }
        });

    }
    else {
        $.messager.alert('Error', "El txt solo esta permitido con planillas cerradas.", 'warning');
    }
}

function fnGenerarTxtPlanillaCont() {
    var row = $('#dgv_planilla').datagrid('getSelected');
    if (!row) {
        $.messager.alert('Seleccione registro', "Para continuar con el proceso debe seleccionar un registro.", 'warning');
        return;
    }
    if (row.codigo_estado_planilla == 2) {
        $.messager.confirm('Confirmaci&oacute;n', '¿Est&aacute; seguro que desea generar el archivo txt?', function (result) {
            if (result) {
                var url = ActionIndexUrl.GenerarTxt + "?id=" + row.codigo_planilla;

                $(this).AbrirVentanaEmergente({
                    parametro: "?codigo_planilla=" + row.codigo_planilla,
                    div: 'div_txt_contabilidad',
                    title: "Selección Empresa",
                    url: ActionIndexUrl._TXTContabilidad
                });
            }
        });
    }
    else {
        $.messager.alert('Error', "El txt solo esta permitido con planillas cerradas.", 'warning');
    }
}
