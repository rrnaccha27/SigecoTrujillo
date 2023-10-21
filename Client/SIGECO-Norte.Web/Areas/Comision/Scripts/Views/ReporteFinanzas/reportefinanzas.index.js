var ActionIndexUrl = {};
var Filtros = {};

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
    $('.content').combobox_sigees({
        id: '#codigo_canal',
        url: ActionIndexUrl.GetCanalJson
    });

    $('.content').combobox_sigees({
        id: '#codigo_tipo_planilla',
        url: ActionIndexUrl.GetTipoPlanillaJson
    });
    $('#codigo_tipo_planilla').combobox('setValue', 2);
    

    //$('#dtp_fecha_inicio').formatteDate();
    //$('#dtp_fecha_fin').formatteDate();

    $('.content').combobox_sigees({
        id: '#codigo_anio',
        url: ActionIndexUrl.GetAnioJson
    });

    $('.content').combobox_sigees({
        id: '#codigo_mes',
        url: ActionIndexUrl.GetMesJson
    });

    
    
    var queryParams = {
        tipo: -1, codigo_canal: "-1", codigo_tipo_planilla: 0, codigo_tipo_reporte: 0, fecha_inicio: null, fecha_fin: null
    };

    $('#DataGrid').datagrid({
        url: ActionIndexUrl.GetAllJson,
        idField: 'id',
        fitColumns: true,
        queryParams: queryParams,
        singleSelect: true,
        rownumbers: true,
        pageSize: 20,
        pageList: [20, 50, 100],
        pagination: true,
        remoteFilter: false,
        
            columns:
        [[
            /*{ field: 'tipo', hidden: true },*/
            { field: 'nombres', title: 'APELLIDOS Y NOMBRES', width: 150, align: 'left', halign: 'center' },
            { field: 'monto_bruto', title: 'TOTAL BRUTO', width: 100, align: 'right', halign: 'center', formatter: function (value, row) { return $.NumberFormat(value, 2); } },
            { field: 'monto_igv', title: 'TOTAL IGV', width: 100, align: 'right', halign: 'center', formatter: function (value, row) { return $.NumberFormat(value, 2); } },
            { field: 'monto_neto', title: 'TOTAL NETO', width: 100, align: 'right', halign: 'center', formatter: function (value, row) { return $.NumberFormat(value, 2); } },

            { field: 'monto_neto_espacio', title: 'TOTAL CAMPO', width: 100, align: 'right', halign: 'center', formatter: function (value, row) { return $.NumberFormat(value, 2); } },
            { field: 'monto_neto_cremacion', title: 'TOTAL CREMACIÓN', width: 100, align: 'right', halign: 'center', formatter: function (value, row) { return $.NumberFormat(value, 2); } },
            { field: 'monto_neto_servicio', title: 'TOTAL FUNERARIA', width: 100, align: 'right', halign: 'center', formatter: function (value, row) { return $.NumberFormat(value, 2); } },
            { field: 'monto_neto_otros', title: 'TOTAL OTROS', width: 100, align: 'right', halign: 'center', formatter: function (value, row) { return $.NumberFormat(value, 2); } }
            

        ]]
           
    });

    $('#DataGrid').datagrid('enableFilter');

    //if ($("#codigo_canal").data('combobox')) {
    //    fnFormatComboboxCheck($("#codigo_canal"));
    //}

    //if ($("#codigo_tipo_planilla").data('combobox')) {
    //    fnFormatComboboxCheck($("#codigo_tipo_planilla"));
    //}

    $("#btnResumen").on("click", function () {
        if (!$("#btnResumen").linkbutton("options").disabled) {
            fnExportarDetalleExcel();
        }
    });

    $("#btnDetalle").on("click", function () {
        if (!$("#btnDetalle").linkbutton("options").disabled) {            
            fnExportarContratoComisionExcel();
        }
    });

}

function ClearSelection() {
    $('#DataGrid').datagrid('clearSelections');
}

function fnFormatComboboxCheck(cmb_target) {
    $(cmb_target).combobox({
        formatter: function (row) {
            var opts = $(this).combobox('options');
            return '<input type="checkbox" class="combobox-checkbox">' + row[opts.textField]
        },
        onLoadSuccess: function () {
            var opts = $(this).combobox('options');
            var target = this;
            var values = $(target).combobox('getValues');

            $.map(values, function (value) {
                var el = opts.finder.getEl(target, value);
                el.find('input.combobox-checkbox')._propAttr('checked', true);
            })
        },
        onSelect: function (row) {
            var opts = $(this).combobox('options');
            var el = opts.finder.getEl(this, row[opts.valueField]);
            el.find('input.combobox-checkbox')._propAttr('checked', true);
        },
        onUnselect: function (row) {
            var opts = $(this).combobox('options');
            var el = opts.finder.getEl(this, row[opts.valueField]);
            el.find('input.combobox-checkbox')._propAttr('checked', false);
        }
    });
}

function GetAllJson() {
    var canal = $.trim($('#codigo_canal').combobox('getValue'));
    var codigo_tipo_planilla = $.trim($('#codigo_tipo_planilla').combobox('getValue')); 
   /* var tipo = $('#tipo').switchbutton('options').checked;*/
    var fecha_inicio = $.trim($("#codigo_anio").datebox('getValue'));
    var fecha_fin = $.trim($("#codigo_mes").datebox('getValue'));

    if (!canal) {
        $.messager.alert(nombreOpcion, "Debe seleccionar un Canal de Venta.", "warning");
        return false;
    }

    if (!codigo_tipo_planilla) {
        $.messager.alert(nombreOpcion, "Debe seleccionar un Tipo de Planilla.", "warning");
        return false;
    } 

    if (!fecha_inicio) {
        $.messager.alert(nombreOpcion, "Debe seleccionar fecha inicio.", "warning");
        return false;
    }

    if (!fecha_fin) {
        $.messager.alert(nombreOpcion, "Debe seleccionar fecha fin.", "warning");
        return false;
    }
    var queryParams = {codigo_canal: canal, codigo_tipo_planilla: codigo_tipo_planilla, codigo_anio: fecha_inicio, codigo_mes: fecha_fin
    };
 
    Filtros = queryParams;

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

function GenerarExcel(resumen_detalle) {
    if ($("#btnResumen").linkbutton('options').disabled) { return false; }

    var rows = $("#DataGrid").datagrid("getData");
    if (rows.total < 1) {
        $.messager.alert("Exportar", "No existen registros para exportar.", "warning");
        return;
    }

    Filtros.resumen_detalle = resumen_detalle;

    $.ajax({
        type: 'post',
        url: ActionIndexUrl.SetDataExcel,
        data: JSON.stringify(Filtros),
        async: true,
        cache: false,
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            var url = ActionIndexUrl.GenerarExcel + "?id=" + data.v_guid;
            window.location.href = url;
        },
        error: function (data) {
            $.messager.alert('Error', "Error en el servidor", 'error');
        }
    });
}

function fnExportarDetalleExcel(codigo_persona, generar_orden_pago) {
    var rows = $("#DataGrid").datagrid("getRows");
    if (rows.length < 1) {
        $.messager.alert("Exportar", "No existen registros para exportar.", "warning");
        return;
    }

    var canal = $.trim($('#codigo_canal').combobox('getValue'));
    var codigo_tipo_planilla = $.trim($('#codigo_tipo_planilla').combobox('getValue'));
/*    var tipo = $('#tipo').switchbutton('options').checked;*/
    var anio = $.trim($("#codigo_anio").datebox('getValue'));
    var mes = $.trim($("#codigo_mes").datebox('getValue'));
    if (!canal) {
        $.messager.alert(nombreOpcion, "Debe seleccionar un Canal de Venta.", "warning");
        return false;
    }

    if (!codigo_tipo_planilla) {
        $.messager.alert(nombreOpcion, "Debe seleccionar un Tipo de Planilla.", "warning");
        return false;
    }
    if (!anio) {
        $.messager.alert(nombreOpcion, "Debe seleccionar fecha inicio.", "warning");
        return false;
    }

    if (!mes) {
        $.messager.alert(nombreOpcion, "Debe seleccionar fecha fin.", "warning");
        return false;
    }

    var v_entidad = {
        //sede:sede,
        //tipo: (tipo ? 1 : 2),
        codigo_canal: canal,
        codigo_tipo_planilla: codigo_tipo_planilla,
        codigo_anio: anio,
        codigo_mes: mes,
        codigo_persona: codigo_persona,
        generar_orden_pago: generar_orden_pago
    };

    $.ajax({
        type: 'post',
        url: ActionIndexUrl.ReporteDetalleComSup,
        data: v_entidad,
        async: true,
        cache: false,
        dataType: 'json',
        success: function (data) {
            console.log(data);
            var fileName = data.fileName;
            var url = ActionIndexUrl.ExportarExcel + "?fileName=" + fileName;
            window.location.href = url;

        },
        error: function () {
            $.messager.alert('Error', "Error en el server", 'error');
        }
    });

}

function fnExportarContratoComisionExcel(codigo_persona, generar_orden_pago) {
    var rows = $("#DataGrid").datagrid("getRows");
    if (rows.length < 1) {
        $.messager.alert("Exportar", "No existen registros para exportar.", "warning");
        return;
    }

    var canal = $.trim($('#codigo_canal').combobox('getValue'));
    var codigo_tipo_planilla = $.trim($('#codigo_tipo_planilla').combobox('getValue'));
   /* var tipo = $('#tipo').switchbutton('options').checked;*/
    var anio = $.trim($("#codigo_anio").datebox('getValue'));
    var mes = $.trim($("#codigo_mes").datebox('getValue'));
    if (!canal) {
        $.messager.alert(nombreOpcion, "Debe seleccionar un Canal de Venta.", "warning");
        return false;
    }

    if (!codigo_tipo_planilla) {
        $.messager.alert(nombreOpcion, "Debe seleccionar un Tipo de Planilla.", "warning");
        return false;
    }
    if (!anio) {
        $.messager.alert(nombreOpcion, "Debe seleccionar fecha inicio.", "warning");
        return false;
    }

    if (!mes) {
        $.messager.alert(nombreOpcion, "Debe seleccionar fecha fin.", "warning");
        return false;
    }

    var v_entidad = {
        //sede:sede,
       // tipo: (tipo ? 1 : 2),
        codigo_canal: canal,
        codigo_tipo_planilla: codigo_tipo_planilla,
        codigo_anio: anio,
        codigo_mes: mes,
        codigo_persona: codigo_persona,
        generar_orden_pago: generar_orden_pago
    };

    $.ajax({
        type: 'post',
        url: ActionIndexUrl.ReporteContratoComision,
        data: v_entidad,
        async: true,
        cache: false,
        dataType: 'json',
        success: function (data) {
            console.log(data);
            var fileName = data.fileName;
            var url = ActionIndexUrl.ExportarExcel + "?fileName=" + fileName;
            window.location.href = url;

        },
        error: function () {
            $.messager.alert('Error', "Error en el server", 'error');
        }
    });

}