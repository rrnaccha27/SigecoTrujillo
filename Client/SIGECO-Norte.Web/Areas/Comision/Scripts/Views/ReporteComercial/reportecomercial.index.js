var ActionIndexUrl = {};

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

    var lstTipoReporte = [{ id: '1', text: '1era Quincena' }, { id: '2', text: '2da Quincena' }];




    $('#codigo_tipo_reporte').combobox({
        valueField: 'id',
        textField: 'text',
        data: lstTipoReporte
    });  
     
    
    $('.content').combobox_sigees({
        id: '#codigo_anio',
        url: ActionIndexUrl.GetAnioJson
    });

    $('.content').combobox_sigees({
        id: '#codigo_mes',
        url: ActionIndexUrl.GetMesJson
    });

     $('.content').combobox_sigees({
        id: '#codigo_canal',
        url: ActionIndexUrl.GetCanalJson
    });

    $('.content').combobox_sigees({
        id: '#codigo_tipo_planilla',
        url: ActionIndexUrl.GetTipoPlanillaJson
    });
    $('#codigo_tipo_planilla').combobox('setValue', 1);
    

    //$('#dtp_fecha_inicio').formatteDate();
    //$('#dtp_fecha_fin').formatteDate();
   

    var fecha_inicio = null;// $.trim($("#dtp_fecha_inicio").datebox('getValue'));
    var fecha_fin = null;//$.trim($("#dtp_fecha_fin").datebox('getValue'));

    var queryParams = {
        codigo_canal: "-1", codigo_tipo_planilla: 0, codigo_tipo_reporte: 0, fecha_inicio: fecha_inicio, fecha_fin: fecha_fin
    };

    $('#DataGrid').datagrid({
        url: ActionIndexUrl.GetAllJson,
        /*  idField: 'nombre_grupo',*/
        fitColumns: true,
        queryParams: queryParams,
        singleSelect: true,
        rownumbers: true,
        pageSize: 100,
        pageList: [100, 200, 1000],
        pagination: true,
        remoteFilter: false,
        columns:
            [[
                { field: 'codigo_personal', hidden: true },
                
                //{ field: 'tipo', title: 'TIPO', width: 80, align: 'left', halign: 'center' },
                { field: 'nombre', title: 'APELLIDOS Y NOMBRES', width: 150, align: 'left', halign: 'center' },
                { field: 'monto_bruto', title: 'TOTAL BRUTO', width: 100, align: 'right', halign: 'center', formatter: function (value, row) { return $.NumberFormat(value, 2); } },
                { field: 'monto_igv', title: 'TOTAL IGV', width: 100, align: 'right', halign: 'center', formatter: function (value, row) { return $.NumberFormat(value, 2); } },
                { field: 'monto_neto', title: 'TOTAL NETO', width: 100, align: 'right', halign: 'center', formatter: function (value, row) { return $.NumberFormat(value, 2); } },

                { field: 'monto_neto_espacio', title: 'TOTAL CAMPO', width: 100, align: 'right', halign: 'center', formatter: function (value, row) { return $.NumberFormat(value, 2); } },
                { field: 'monto_neto_cremacion', title: 'TOTAL CREMACIÓN', width: 100, align: 'right', halign: 'center', formatter: function (value, row) { return $.NumberFormat(value, 2); } },
                { field: 'monto_neto_servicio', title: 'TOTAL FUNERARIA', width: 100, align: 'right', halign: 'center', formatter: function (value, row) { return $.NumberFormat(value, 2); } },

                { field: 'bono_espacio', title: 'BONO CAMPO', width: 100, align: 'right', halign: 'center', formatter: function (value, row) { return $.NumberFormat(value, 2); } },
                { field: 'bono_cremacion', title: 'BONO CREMACIÓN', width: 100, align: 'right', halign: 'center', formatter: function (value, row) { return $.NumberFormat(value, 2); } },
                { field: 'bono_servicio', title: 'BONO FUNERARIA', width: 100, align: 'right', halign: 'center', formatter: function (value, row) { return $.NumberFormat(value, 2); } },

                //{ field: 'monto_neto_otros', title: 'TOTAL OTROS', width: 100, align: 'right', halign: 'center', formatter: function (value, row) { return $.NumberFormat(value, 2); } },
                { field: 'monto_ir', title: 'TOTAL IR', width: 100, align: 'right', halign: 'center', formatter: function (value, row) { return $.NumberFormat(value, 2); } }

            ]]
    });

    //if ($("#codigo_canal").data('combobox')) {
    //    fnFormatComboboxCheck($("#codigo_canal"));
    //}

    //if ($("#periodo").data('combobox')) {
    //    fnFormatComboboxCheck($("#periodo"));
    //}

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

    //$('#tipo').switchbutton({
    //    onChange: function (checked) {
    //        $('#codigo_tipo_reporte').combobox(checked ? 'enable' : 'disable');
    //    }
    //});

    $("#btnResumen").on("click", function () {
        fnExportarExcel();
    });

    $("#btnBuscar").on("click", function () {
        GetAllJson();
    });
    
    $("#btnDetalle").on("click", function () {
        fnExportarDetalleExcel(null,0);
    });
    $("#btnOrdenPago").on("click", function () {
        fnGenerarOrdenPago();
    });
}

function ClearSelection()
{
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
    var nombreOpcion = 'Reporte Comercial';
    //var sede = $.trim($('#sede').combobox('getValues'));
    var canal = $.trim($('#codigo_canal').combobox('getValue'));
    var codigo_tipo_planilla = $.trim($('#codigo_tipo_planilla').combobox('getValue'));
    var codigo_tipo_reporte = $.trim($('#codigo_tipo_reporte').combobox('getValue'));
    var periodo = $.trim($('#codigo_mes').combobox('getValue'));
    var anio = $.trim($('#codigo_anio').combobox('getValue'));
/*    var tipo = $('#tipo').switchbutton('options').checked;*/
    //var fecha_inicio = $.trim($("#dtp_fecha_inicio").datebox('getValue'));
    //var fecha_fin = $.trim($("#dtp_fecha_fin").datebox('getValue'));

     
    if (!canal) {
        $.messager.alert(nombreOpcion, "Debe seleccionar un Canal de Venta.", "warning");
        return false;
    }

    if (!codigo_tipo_planilla) {
        $.messager.alert(nombreOpcion, "Debe seleccionar un Tipo de Planilla.", "warning");
        return false;
    }
    if (!codigo_tipo_reporte) {
        $.messager.alert(nombreOpcion, "Debe seleccionar un Tipo reporte.", "warning");
        return false;
    }
   
    if (!anio) {
        $.messager.alert(nombreOpcion, "Debe seleccionar año.", "warning");
        return false;
    }

    if (!periodo) {
        $.messager.alert(nombreOpcion, "Debe seleccionar mes.", "warning");
        return false;
    }

    var queryParams = {
        //sede:sede,
         codigo_canal: canal, codigo_tipo_planilla: codigo_tipo_planilla, codigo_mes: periodo, codigo_anio: anio, codigo_tipo_reporte: codigo_tipo_reporte
    };
    console.log(queryParams);
    $('#DataGrid').datagrid('reload', queryParams);
    ClearSelection();
}
function DownloadExcel() {
    window.location.href = "data:application/vnd.ms-excel;base64, bindata"
}

function fnExportarDetalleExcel(codigo_persona,generar_orden_pago) {
    var rows = $("#DataGrid").datagrid("getRows");
    if (rows.length < 1) {
        $.messager.alert("Exportar", "No existen registros para exportar.", "warning");
        return;
    }

    var canal = $.trim($('#codigo_canal').combobox('getValue'));
    var codigo_tipo_planilla = $.trim($('#codigo_tipo_planilla').combobox('getValue'));
 /*   var tipo = $('#tipo').switchbutton('options').checked;*/
    var periodo = $.trim($('#codigo_mes').combobox('getValue'));
    var anio = $.trim($('#codigo_anio').combobox('getValue'));
    var codigo_tipo_reporte = $.trim($('#codigo_tipo_reporte').combobox('getValue'));

    if (!canal) {
        $.messager.alert(nombreOpcion, "Debe seleccionar un Canal de Venta.", "warning");
        return false;
    }

    if (!codigo_tipo_planilla) {
        $.messager.alert(nombreOpcion, "Debe seleccionar un Tipo de Planilla.", "warning");
        return false;
    }

    if (!codigo_tipo_reporte) {
        $.messager.alert(nombreOpcion, "Debe seleccionar un Tipo de reporte.", "warning");
        return false;
    }

    
    if (!anio) {
        $.messager.alert(nombreOpcion, "Debe seleccionar año.", "warning");
        return false;
    }

    if (!periodo) {
        $.messager.alert(nombreOpcion, "Debe seleccionar mes.", "warning");
        return false;
    }

    var v_entidad = {
        //sede:sede,
       // tipo: (tipo ? 1 : 2),
        codigo_canal: canal,
        codigo_tipo_planilla: codigo_tipo_planilla,
        codigo_mes: periodo,
        codigo_anio: anio,
        codigo_persona: codigo_persona,
        generar_orden_pago: generar_orden_pago,
        codigo_tipo_reporte: codigo_tipo_reporte
    };

    $.ajax({
        type: 'post',
        url: ActionIndexUrl.SetDataDetalleGrilla,
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

function fnExportarExcel() {
    var rows = $("#DataGrid").datagrid("getRows");
    if (rows.length < 1) {
        $.messager.alert("Exportar", "No existen registros para exportar.", "warning");
        return;
    }

    var canal = $.trim($('#codigo_canal').combobox('getValue'));
    var codigo_tipo_planilla = $.trim($('#codigo_tipo_planilla').combobox('getValue'));
   // var tipo = $('#tipo').switchbutton('options').checked;
    var periodo = $.trim($('#codigo_mes').combobox('getValue'));
    var anio = $.trim($('#codigo_anio').combobox('getValue'));
    var codigo_tipo_reporte = $.trim($('#codigo_tipo_reporte').combobox('getValue'));

    if (!canal) {
        $.messager.alert(nombreOpcion, "Debe seleccionar un Canal de Venta.", "warning");
        return false;
    }

    if (!codigo_tipo_planilla) {
        $.messager.alert(nombreOpcion, "Debe seleccionar un Tipo de Planilla.", "warning");
        return false;
    }
    if (!anio) {
        $.messager.alert(nombreOpcion, "Debe seleccionar año.", "warning");
        return false;
    }

    if (!periodo) {
        $.messager.alert(nombreOpcion, "Debe seleccionar mes.", "warning");
        return false;
    }
    if (!codigo_tipo_reporte) {
        $.messager.alert(nombreOpcion, "Debe seleccionar un Tipo de reporte.", "warning");
        return false;
    }
    var v_entidad = {
        //sede:sede,
       // tipo: (tipo ? 1 : 2),
        codigo_canal: canal,
        codigo_tipo_planilla: codigo_tipo_planilla,
        codigo_mes: periodo,
        codigo_anio: anio,
        codigo_tipo_reporte: codigo_tipo_reporte
    };   

    $.ajax({
        type: 'post',
        url: ActionIndexUrl.SetDataGrilla,
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


function fnGenerarOrdenPago() {

    //var row = $("#DataGrid").datagrid('getSelected');
    //if (!row) {
    //    $.messager.alert('Seleccione registro', "Para continuar con el proceso debe seleccionar un registro.", 'warning');
    //    return;
    //}

    //fnExportarDetalleExcel(row.codigo_personal, true);
    fnExportarDetalleExcel("0", true);
    
}

//(function ($) {
//    function getRows(target) {
//        var state = $(target).data('datagrid');
//        if (state.filterSource) {
//            return state.filterSource.rows;
//        } else {
//            return state.data.rows;
//        }
//    }
//    function toHtml(target, rows) {
//        rows = rows || getRows(target);
//        var dg = $(target);
//        var data = ['<table border="1" rull="all" style="border-collapse:collapse">'];
//        var fields = dg.datagrid('getColumnFields', true).concat(dg.datagrid('getColumnFields', false));
//        var trStyle = 'height:32px';
//        var tdStyle0 = 'vertical-align:middle;padding:0 4px';
//        data.push('<tr style="' + trStyle + '">');
//        for (var i = 0; i < fields.length; i++) {
//            var col = dg.datagrid('getColumnOption', fields[i]);
//            var tdStyle = tdStyle0 + ';width:' + col.boxWidth + 'px;';
//            data.push('<th style="' + tdStyle + '">' + col.title + '</th>');
//        }
//        data.push('</tr>');
//        $.map(rows, function (row) {
//            data.push('<tr style="' + trStyle + '">');
//            for (var i = 0; i < fields.length; i++) {
//                var field = fields[i];
//                data.push(
//                    '<td style="' + tdStyle0 + '">' + row[field] + '</td>'
//                );
//            }
//            data.push('</tr>');
//        });
//        data.push('</table>');
//        return data.join('');
//    }

//    function toArray(target, rows) {
//        rows = rows || getRows(target);
//        var dg = $(target);
//        var fields = dg.datagrid('getColumnFields', true).concat(dg.datagrid('getColumnFields', false));
//        var data = [];
//        var r = [];
//        for (var i = 0; i < fields.length; i++) {
//            var col = dg.datagrid('getColumnOption', fields[i]);
//            r.push(col.title);
//        }
//        data.push(r);
//        $.map(rows, function (row) {
//            var r = [];
//            for (var i = 0; i < fields.length; i++) {
//                r.push(row[fields[i]]);
//            }
//            data.push(r);
//        });
//        return data;
//    }

//    function print(target, param) {
//        var title = null;
//        var rows = null;
//        if (typeof param == 'string') {
//            title = param;
//        } else {
//            title = param['title'];
//            rows = param['rows'];
//        }
//        var newWindow = window.open('', '', 'width=800, height=500');
//        var document = newWindow.document.open();
//        var content =
//            '<!doctype html>' +
//            '<html>' +
//            '<head>' +
//            '<meta charset="utf-8">' +
//            '<title>' + title + '</title>' +
//            '</head>' +
//            '<body>' + toHtml(target, rows) + '</body>' +
//            '</html>';
//        document.write(content);
//        document.close();
//        newWindow.print();
//    }

//    function b64toBlob(data) {
//        var sliceSize = 512;
//        var chars = atob(data);
//        var byteArrays = [];
//        for (var offset = 0; offset < chars.length; offset += sliceSize) {
//            var slice = chars.slice(offset, offset + sliceSize);
//            var byteNumbers = new Array(slice.length);
//            for (var i = 0; i < slice.length; i++) {
//                byteNumbers[i] = slice.charCodeAt(i);
//            }
//            var byteArray = new Uint8Array(byteNumbers);
//            byteArrays.push(byteArray);
//        }
//        return new Blob(byteArrays, {
//            type: ''
//        });
//    }

//    function toExcel(target, param) {
//        var filename = null;
//        var rows = null;
//        var worksheet = 'Worksheet';
//        if (typeof param == 'string') {
//            filename = param;
//        } else {
//            filename = param['filename'];
//            rows = param['rows'];
//            worksheet = param['worksheet'] || 'Worksheet';
//        }
//        var dg = $(target);
//        var uri = 'data:application/vnd.ms-excel;base64,'
//        , template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><meta http-equiv="content-type" content="application/vnd.ms-excel; charset=UTF-8"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body>{table}</body></html>'
//        , base64 = function (s) { return window.btoa(unescape(encodeURIComponent(s))) }
//        , format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) }

//        var table = toHtml(target, rows);
//        var ctx = { worksheet: worksheet, table: table };
//        var data = base64(format(template, ctx));
//        if (window.navigator.msSaveBlob) {
//            var blob = b64toBlob(data);
//            window.navigator.msSaveBlob(blob, filename);
//        } else {
//            var alink = $('<a style="display:none"></a>').appendTo('body');
//            alink[0].href = uri + data;
//            alink[0].download = filename;
//            alink[0].click();
//            alink.remove();
//        }
//    }

//    $.extend($.fn.datagrid.methods, {
//        toHtml: function (jq, rows) {
//            return toHtml(jq[0], rows);
//        },
//        toArray: function (jq, rows) {
//            return toArray(jq[0], rows);
//        },
//        toExcel: function (jq, param) {
//            return jq.each(function () {
//                toExcel(this, param);
//            });
//        },
//        print: function (jq, param) {
//            return jq.each(function () {
//                print(this, param);
//            });
//        }
//    });
//})(jQuery);
