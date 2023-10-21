var ActionIndexUrl = {};

; (function (app) {
    //===========================================================================================
    var current = app.index = {};
    //===========================================================================================

    jQuery.extend(app.index,
        {
            Initialize: function (actionUrls) {
                jQuery.extend(ActionIndexUrl, actionUrls);
                fnInicializarIndex();
                fnConfigurarGrilla();
            }
        })
})(project);

function fnInicializarIndex() {
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

function fnConsultarIndex() {
    $('#dg_principal').datagrid('reload');
}

function fnConfigurarGrilla() {
    $('#dg_principal').datagrid({
        fitColumns: true,
        idField: 'codigo_checklist',
        url: ActionIndexUrl._Buscar,
        data: null,
        height: '550',        
        pagination: true,
        singleSelect: true,
        remoteFilter: false,
        rownumbers: true,
        pageList: [100, 200, 500 , 1000],
        pageSize: 100,
        columns:
        [[
            { field: 'codigo_checklist', title: 'Codigo', hidden: 'true' },
            { field: 'numero_checklist', title: 'Nro. CheckList', width: 120, align: 'center' },
            { field: 'nombre_regla_tipo_checklist', title: 'Planilla', width: 120, align: 'center' },
            { field: 'nombre_estado_checklist', title: 'Estado<br>CheckList', width: 120, align: 'center', styler: cellStylerEstadoPlanilla },
            { field: 'fecha_apertura', title: 'Fecha<br>Apertura', width: 120, align: 'center' },
            { field: 'usuario_apertura', title: 'Usuario<br>Apertura', width: 100, align: 'left' },
            { field: 'fecha_cierre', title: 'Fecha<br>Cierre', width: 120, align: 'center' },
            { field: 'usuario_cierre', title: 'Usuario<br>Cierre', width: 100, align: 'left' },
            { field: 'fecha_anulacion', title: 'Fecha<br>Anulación', width: 120, align: 'center' },
            { field: 'usuario_anulacion', title: 'Usuario<br>Anulación', width: 100, align: 'left' },
            { field: 'estilo', hidden: 'true' }
        ]],
        onClickRow: function (index, row) {
            if (row.codigo_estado_checklist == 2) {
                $("#btnGenerarTxtRRHH").linkbutton('enable');
                $("#btnGenerarTxtCont").linkbutton('enable');
            }
            else {
                $("#btnGenerarTxtRRHH").linkbutton('disable');
                $("#btnGenerarTxtCont").linkbutton('disable');
            }
        }
    });
    $('#dg_principal').datagrid('enableFilter');
    $(window).resize(function () {
        $('#dg_principal').datagrid('resize');
    });
}

function cellStylerEstadoPlanilla(value, row, index) {
    return row.estilo;
}

function ListarPlanilla() {
    $(this).AbrirVentanaEmergente({
        div: 'dlgListadoPlanilla',
        title: "Listado de Planilla",
        url: ActionIndexUrl._Listado
    });
}

function ClearSelection() {
    $('#dg_principal').datagrid('clearSelections');
}

function Revisar(codigo_checklist) {
    var row = $('#dg_principal').datagrid('getSelected');

    if (!codigo_checklist){
        if (!row) {
            $.messager.alert('Seleccione registro', "Para continuar con el proceso debe seleccionar un registro.", 'warning');
            return;
        }
        codigo_checklist = row.codigo_checklist
    }

    $(this).AbrirVentanaEmergente({
        parametro: "?codigo_checklist=" + codigo_checklist,
        div: 'dlgRevisar',
        title: "Revisión CheckList",
        url: ActionIndexUrl._Planilla
    });
}

function fnGenerarTxtPlanilla() {
    var row = $('#dg_principal').datagrid('getSelected');
    if (!row) {
        $.messager.alert('Seleccione registro', "Para continuar con el proceso debe seleccionar un registro.", 'warning');
        return;
    }
    if (row.codigo_estado_checklist == 2) {
        $.messager.confirm('Confirmaci&oacute;n', '¿Est&aacute; seguro que desea generar el archivo txt?', function (result) {
            if (result) {
                var url = ActionIndexUrl.GenerarTxt + "?id=" + row.codigo_checklist;
                window.open(url, '_blank');
            }
        });

    }
    else {
        $.messager.alert('Error', "El txt solo esta permitido con planillas cerradas.", 'warning');
    }
}

function fnGenerarTxtPlanillaCont() {
    var row = $('#dg_principal').datagrid('getSelected');
    if (!row) {
        $.messager.alert('Seleccione registro', "Para continuar con el proceso debe seleccionar un registro.", 'warning');
        return;
    }
    if (row.codigo_estado_checklist == 2) {
        $.messager.confirm('Confirmaci&oacute;n', '¿Est&aacute; seguro que desea generar el archivo txt?', function (result) {
            if (result) {
                var url = ActionIndexUrl.GenerarTxt + "?id=" + row.codigo_checklist;

                $(this).AbrirVentanaEmergente({
                    parametro: "?codigo_planilla=" + row.codigo_checklist,
                    div: 'dlgTxtContabilidad',
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
