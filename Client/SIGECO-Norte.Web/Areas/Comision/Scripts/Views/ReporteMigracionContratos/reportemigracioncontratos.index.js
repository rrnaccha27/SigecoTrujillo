﻿var ActionIndexUrl = {};

; (function (app) {
    //===========================================================================================
    var current = app.index = {};
    //===========================================================================================

    jQuery.extend(app.index,
        {
            Initialize: function (actionUrls) {
                jQuery.extend(ActionIndexUrl, actionUrls);
                fnConfigurarIndex();
            }
        })
})(project);


function fnConfigurarIndex() {
    var queryParams = {
        fecha_inicial: '', fecha_final: ''
    };

    $('#DataGrid').datagrid({
        url: ActionIndexUrl.GetAllJson,
        idField: 'docentry',
        fitColumns: true,
        queryParams: queryParams,
        singleSelect: true,
        rownumbers: true,
        pagination: true,
        pageSize: 1000,
        pageList: [1000],
        remoteSort:false,
        columns:
            [[
                { field: 'docentry', hidden: true },
                { field: 'nombre_empresa', title: 'Empresa', width: 60, align: 'left', halign: 'center' },
                { field: 'nro_contrato', title: 'Contrato', width: 80, align: 'center', halign: 'center' },
                { field: 'fecha_contrato', title: 'Fecha<br>Contrato', width: 75, align: 'center', halign: 'center' },
                { field: 'estado', title: 'Estado', width: 65, align: 'left', halign: 'center' },
                { field: 'fecha_migracion', title: 'Fecha<br>Migración', width: 75, align: 'center', halign: 'center' },
                { field: 'nombre_canal', title: 'Canal', width: 100, align: 'left', halign: 'center' },
                { field: 'nombre_grupo', title: 'Grupo', width: 100, align: 'left', halign: 'center' },
                { field: 'nombre_personal', title: 'Vendedor', width: 100, align: 'left', halign: 'center' },
                { field: 'intentos', title: 'Intentos', width: 45, align: 'right', halign: 'center', sortable:true }
            ]]
    });

    $('#DataGrid').datagrid('enableFilter', [{
        field: 'estado',
        type: 'combobox',
        options: {
            editable: false,
            panelHeight: 'auto',
            data: [{ value: '', text: 'Todos' }, { value: 'Migrado', text: 'Migrado' }, { value: 'No Migrado', text: 'No Migrado' }],
            onChange: function (value) {

                if (value == '') {
                    $('#DataGrid').datagrid('removeFilterRule', 'estado');
                } else {
                    $('#DataGrid').datagrid('addFilterRule', {
                        field: 'estado',
                        op: 'equal',
                        value: value
                    });
                }
                $('#DataGrid').datagrid('doFilter');
            }
        }
    }]);

    var pager = $('#DataGrid').datagrid('getPager');
    pager.pagination({
        showPageList: true,
        buttons: [{
            iconCls: 'icon-excel',
            handler: function () {
                fnExportarExcel();
            }
        }]
    });

    $("#btnBuscar").on("click", function () {
        GetAllJson();
    });

}

function ClearSelection() {
    $('#DataGrid').datagrid('clearSelections');
}

function GetAllJson() {
    var nombreOpcion = 'Reporte Migracion Contratos';
    var fecha_inicio = $.trim($('#fechaInicio').textbox('getText'));
    var fecha_fin = $.trim($('#fechaFin').textbox('getText'));

    if (!fecha_inicio) {
        $.messager.alert(nombreOpcion, "Fecha Inicio debe ingresarse.", "warning");
        return false;
    }

    if (!fecha_fin) {
        $.messager.alert(nombreOpcion, "Fecha Fin debe ingresarse.", "warning");
        return false;
    }

    if (!ValidarFecha(fecha_inicio)) {
        $.messager.alert(nombreOpcion, "Fecha Inicio en formato incorrecto.", "warning");
        return false;
    }

    if (!ValidarFecha(fecha_fin)) {
        $.messager.alert(nombreOpcion, "Fecha Fin en formato incorrecto.", "warning");
        return false;
    }

    fecha_inicio = FormatoFecha(fecha_inicio);
    fecha_fin = FormatoFecha(fecha_fin);

    if (parseInt(fecha_inicio) > parseInt(fecha_fin)) {
        $.messager.alert(nombreOpcion, "Inicio debe ser menor a Fin.", "warning");
        return false;
    }
    var queryParams = {
        fecha_inicial: fecha_inicio, fecha_final: fecha_fin
    };

    $('#DataGrid').datagrid('reload', queryParams);
    ClearSelection();
}

function fnExportarExcel() {
    var rows = $("#DataGrid").datagrid("getRows");
    if (rows.length < 1) {
        $.messager.alert("Exportar", "No existen registros para exportar.", "warning");
        return;
    }

    $.ajax({
        type: 'post',
        url: ActionIndexUrl.SetDataGrilla,
        data: JSON.stringify({ v_entidad: rows }),
        async: true,
        cache: false,
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            var url = ActionIndexUrl.ExportarExcel + "?id=" + data.v_guid;
            //window.open(url, '_blank');
            window.location.href = url;
        },
        error: function (data) {

            $.messager.alert('Error', "Error en el servidor", 'error');
        }
    });
    //$('#DataGrid').datagrid('toExcel', 'dg.xls');
    //return false;
}

function ValidarFecha(fecha) {
    try {
        testdate = $.datepicker.parseDate('dd/mm/yy', fecha);
        return true;
    } catch (e) {

        return false;
    }
}

function FormatoFecha(fecha) {
    return fecha.substring(6, 10) + fecha.substring(3, 5) + fecha.substring(0, 2);
}
