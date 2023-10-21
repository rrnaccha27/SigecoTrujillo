var ActionIndexUrl = {};

; (function (app) {
    //===========================================================================================
    var current = app.comision_index = {};
    //===========================================================================================
    jQuery.extend(app.comision_index,
        {
            Initialize: function (actionUrls) {
                jQuery.extend(ActionIndexUrl, actionUrls);
                fnInicializarCombox();
                fnConfigurarGrillaComisionPago();
            }
        })
})(project);

function fnInicializarCombox() {
    $('.content').combobox_sigees({
        id: '#cmb_estado_cuota',
        url: ActionIndexUrl.GetEstadoCuotaJson
    });


    $('.content').combobox_sigees({
        id: '#cmb_canal',
        url: ActionIndexUrl.GetCanalJson
    });

    $('#cmb_canal').combobox({
        onSelect: function (rec) {
            fnLoadGrupo(rec.id);
        }
    });
    $('#cmb_grupo').combobox({
        valueField: 'id',
        textField: 'text',
        panelWidth: '300px',
        separator: '-'
    })


    $('#dtp_fecha_habilitado_inicio').formatteDate();
    $('#dtp_fecha_habilitado_fin').formatteDate();

    $('#dtp_fecha_contrato_inicio').formatteDate();
    $('#dtp_fecha_contrato_fin').formatteDate();

    $('.content').combobox_sigees({
        id: '#cmb_tipo_planilla',
        url: ActionIndexUrl.GetTipoPlanillaJson
    });
}
function fnLoadGrupo(id) {
    //$('.content').combobox_sigees({
    //    id: '#cmb_grupo',
    //    url: ActionIndexUrl.GetGrupoJson
    //});

    $("#cmb_grupo").combobox('clear');
    $('#cmb_grupo').combobox('reload', ActionIndexUrl.GetGrupoJson + "/" + id)

}
function fnConfigurarGrillaComisionPago() {

    $('#dgv_comision_pago').datagrid({
        fitColumns: false,
        //url: ActionIndexUrl.GetRegistrosAllJson,
        pagination: true,
        singleSelect: true,
        //toolbar: "#toolbar",
        // view: bufferview,// no aplicar buffer
        rownumbers: true,
        //pageNumber: 1,
        queryParams: {
            codigo_estado_cuota: $("#cmb_estado_cuota").combobox("getValue"),
            codigo_grupo: $("#cmb_grupo").combobox("getValue"),
            codigo_canal: $("#cmb_canal").combobox("getValue"),
            codigo_tipo_planilla: $("#cmb_tipo_planilla").combobox("getValue"),
            codigo_personal: ActionIndexUrl.codigo_personal
        },
        pageList: [20, 60, 80, 100, 150],
        pageSize: 20,
        url: ActionIndexUrl.GetRegistrosAllJson,
        //loader: loader,
        columns: [[
            { field: 'numero_planilla', title: 'N° Planilla', width: 80, align: 'left' },
            { field: 'nombre_regla', title: 'Planilla', width: 120, align: 'left' },
            { field: 'nombre_articulo', title: 'Artículo', width: 200, align: 'left' },
            { field: 'nombre_canal', title: 'Canal', width: 120, align: 'left' },
            { field: 'nombre_grupo', title: 'Grupo', width: 160, align: 'left' },
            { field: 'datos_vendedor', title: 'Personal de Venta', width: 250, align: 'left' },
            { field: 'nombre_empresa', title: 'Empresa', width: 90, align: 'center' },
            { field: 'nombre_tipo_venta', title: 'Tipo Venta', width: 90, align: 'center' },
            { field: 'nombre_tipo_pago', title: 'Tipo Pago', width: 90, align: 'center' },
            { field: 'nro_contrato', title: 'N° Contrato', width: 90, align: 'left' },
            { field: 'fecha_programada', title: 'Fecha <br>Habilitada', width: 90, align: 'center' },

            { field: 'nro_cuota', title: 'N° Cuota', width: 80, align: 'center' },
            { field: 'str_monto_bruto', title: 'Comisión<br>sin IGV', width: 80, align: 'right' },
            { field: 'str_igv', title: 'I.G.V', width: 80, align: 'right' },
            { field: 'str_monto_neto', title: 'Comisión<br>con IGV', width: 80, align: 'right' },
            { field: 'nombre_estado_cuota', title: 'Estado', width: 150, align: 'center' }
        ]]
    });

    $('#dgv_comision_pago').datagrid('enableFilter');
    $(window).resize(function () {
        $('#dgv_comision_pago').datagrid('resize');
    });

    var pager = $('#dgv_comision_pago').datagrid('getPager');    // get the pager of datagrid
    //$("#cmb_paginar_server").combobox("loadata", [100,200,3000]);
    pager.pagination({
        showPageList: true,
        buttons: [{
            iconCls: 'icon-excel',
            handler: function () {
                fnExportarExcel();
            }
        }, {
            iconCls: 'icon-pdf',
            handler: function () {
                fnExportarPdf();
            }
        }]
    });
}

function loader(param, success, error) {

    console.log(param);
    var opts = $(this).datagrid('options');
    if (!opts.url) return false;
    $.ajax({
        type: opts.method,
        url: opts.url,
        data: param,
        dataType: 'json',
        success: function (data) {
            //debugger;
            console.log(data);
            if (data.sucess) {
                success(data);
            } else {
                //error(data);
                success(data);
                $.messager.alert('Error', data.message, 'error');

            }
        },
        error: function () {
            error.apply(this, arguments);
        }
    });
}
function fnBuscar() {
    if (!$("#frm_consultar").form('enableValidation').form('validate'))
        return;

    var queryParams = {
        codigo_estado_cuota: $("#cmb_estado_cuota").combobox("getValue"),
        codigo_grupo: $("#cmb_grupo").combobox("getValue"),
        codigo_canal: $("#cmb_canal").combobox("getValue"),
        codigo_tipo_planilla: $("#cmb_tipo_planilla").combobox("getValue"),
        codigo_personal: ActionIndexUrl.codigo_personal,
        str_fecha_habilitado_inicio: $("#dtp_fecha_habilitado_inicio").datebox("getValue"),
        str_fecha_habilitado_fin: $("#dtp_fecha_habilitado_fin").datebox("getValue"),
        str_fecha_contrato_inicio: $("#dtp_fecha_contrato_inicio").datebox("getValue"),
        str_fecha_contrato_fin: $("#dtp_fecha_contrato_fin").datebox("getValue")
    };

    $('#dgv_comision_pago').datagrid("reload", queryParams);
}

function fnLimpiarVendedor() {
    $("#txt_nombre_personal").textbox('clear');
    ActionIndexUrl.codigo_personal = null;
}

function fnLimpiarBuscar() {
    ActionIndexUrl.codigo_personal = null;

    //$("#cmb_canal").combobox('setValue', null);
    //$("#cmb_grupo").combobox('setValue', null);
    //$("#cmb_estado_cuota").combobox('setValue', null);
    //$("#cmb_tipo_planilla").combobox('setValue', null);

    $("#cmb_estado_cuota").combobox('clear');
    $("#cmb_canal").combobox('clear');
    $("#cmb_grupo").combobox('clear');
    $("#cmb_tipo_planilla").combobox('clear');
    $("#txt_nombre_personal").textbox('clear');

    $("#frm_consultar").form("clear");
}

function fnExportarPdf() {

    var rows = $("#dgv_comision_pago").datagrid("getRows");
    if (rows.length < 1) {
        $.messager.alert("Exportar", "No existe registros para exportar, intente nuevamente.", "warning");
        return;
    }
    var state = $("#dgv_comision_pago").data('datagrid');
    console.log(state);
   // var filterSource = state.filterSource.rows;

    $.ajax({
        type: 'post',
        url: ActionIndexUrl.SetDataGrilla,
        data: JSON.stringify({ v_entidad: rows }),
        async: true,
        cache: false,
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            var url = ActionIndexUrl.ExportarComisionPdf + "?id=" + data.v_guid;
            window.open(url, '_blank');
        },
        error: function (data) {

            $.messager.alert('Error', "Error en el servidor", 'error');
        }
    });

    

}
function fnVerReporte()
{
    if (!$("#frm_consultar").form('enableValidation').form('validate'))
        return;
    var queryParams = {
        codigo_estado_cuota: $("#cmb_estado_cuota").combobox("getValue"),
        codigo_grupo: $("#cmb_grupo").combobox("getValue"),
        codigo_canal: $("#cmb_canal").combobox("getValue"),
        codigo_tipo_planilla: $("#cmb_tipo_planilla").combobox("getValue"),
        codigo_personal: ActionIndexUrl.codigo_personal,
        str_fecha_habilitado_inicio: $("#dtp_fecha_habilitado_inicio").datebox("getValue"),
        str_fecha_habilitado_fin: $("#dtp_fecha_habilitado_fin").datebox("getValue"),
        str_fecha_contrato_inicio: $("#dtp_fecha_contrato_inicio").datebox("getValue"),
        str_fecha_contrato_fin: $("#dtp_fecha_contrato_fin").datebox("getValue")

    };
    $.ajax({
        type: 'post',
        url: ActionIndexUrl.SetFiltroGrilla,
        data: JSON.stringify({ v_entidad: queryParams }),
        async: true,
        cache: false,
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {            
            window.open(data.v_url, '_blank');
        },
        error: function (data) {

            $.messager.alert('Error', "Error en el servidor", 'error');
        }
    });

}
function fnExportarExcel() {

    

    var rows = $("#dgv_comision_pago").datagrid("getRows");
    if (rows.length < 1) {
        $.messager.alert("Exportar", "No existe registros para exportar, intente nuevamente.", "warning");
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
            var url = ActionIndexUrl.ExportarComisionExcel + "?id=" + data.v_guid;
            window.open(url, '_blank');
        },
        error: function (data) {

            $.messager.alert('Error', "Error en el servidor", 'error');
        }
    });

    //if (!$("#frm_consultar").form('enableValidation').form('validate'))
    //    return;
    //var queryParams = {
    //    codigo_estado_cuota: $("#cmb_estado_cuota").combobox("getValue"),
    //    codigo_grupo: $("#cmb_grupo").combobox("getValue"),
    //    codigo_canal: $("#cmb_canal").combobox("getValue"),
    //    codigo_tipo_planilla: $("#cmb_tipo_planilla").combobox("getValue"),
    //    codigo_personal: ActionIndexUrl.codigo_personal,
    //    str_fecha_habilitado_inicio: $("#dtp_fecha_habilitado_inicio").datebox("getValue"),
    //    str_fecha_habilitado_fin: $("#dtp_fecha_habilitado_fin").datebox("getValue"),
    //    str_fecha_contrato_inicio: $("#dtp_fecha_contrato_inicio").datebox("getValue"),
    //    str_fecha_contrato_fin: $("#dtp_fecha_contrato_fin").datebox("getValue")

    //};
    //$.ajax({
    //    type: 'post',
    //    url: ActionIndexUrl.SetFiltroGrilla,
    //    data: JSON.stringify({ v_entidad: queryParams }),
    //    async: true,
    //    cache: false,
    //    dataType: 'json',
    //    contentType: 'application/json',
    //    success: function (data) {
    //        var url = ActionIndexUrl.ExportarComisionExcel + "?id=" + data.v_guid;
    //        window.open(data.v_url, '_blank');
    //    },
    //    error: function (data) {

    //        $.messager.alert('Error', "Error en el servidor", 'error');
    //    }
    //});
}


function fnVentanaPersona() {
    $(this).AbrirVentanaEmergente({
        parametro: null,
        div: 'div_listado_personal',
        title: "Listado de Personal",
        url: ActionIndexUrl._Persona_Busqueda
    });

}

function CreateFormPage(strPrintName, printDatagrid) {
    var tableString = '<table cellspacing="0" class="pb">';
    var frozenColumns = printDatagrid.datagrid("options").frozenColumns;  // Get the frozenColumns object
    var columns = printDatagrid.datagrid("options").columns;    // Get the columns object
    var nameList = '';

    // Load title
    if (typeof columns != 'undefined' && columns != '') {
        $(columns).each(function (index) {
            tableString += '\n<tr>';
            if (typeof frozenColumns != 'undefined' && typeof frozenColumns[index] != 'undefined') {
                for (var i = 0; i < frozenColumns[index].length; ++i) {
                    if (!frozenColumns[index][i].hidden) {
                        tableString += '\n<th width="' + frozenColumns[index][i].width + '"';
                        if (typeof frozenColumns[index][i].rowspan != 'undefined' && frozenColumns[index][i].rowspan > 1) {
                            tableString += ' rowspan="' + frozenColumns[index][i].rowspan + '"';
                        }
                        if (typeof frozenColumns[index][i].colspan != 'undefined' && frozenColumns[index][i].colspan > 1) {
                            tableString += ' colspan="' + frozenColumns[index][i].colspan + '"';
                        }
                        if (typeof frozenColumns[index][i].field != 'undefined' && frozenColumns[index][i].field != '') {
                            nameList += ',{"f":"' + frozenColumns[index][i].field + '", "a":"' + frozenColumns[index][i].align + '"}';
                        }
                        tableString += '>' + frozenColumns[0][i].title + '</th>';
                    }
                }
            }
            for (var i = 0; i < columns[index].length; ++i) {
                if (!columns[index][i].hidden) {
                    tableString += '\n<th width="' + columns[index][i].width + '"';
                    if (typeof columns[index][i].rowspan != 'undefined' && columns[index][i].rowspan > 1) {
                        tableString += ' rowspan="' + columns[index][i].rowspan + '"';
                    }
                    if (typeof columns[index][i].colspan != 'undefined' && columns[index][i].colspan > 1) {
                        tableString += ' colspan="' + columns[index][i].colspan + '"';
                    }
                    if (typeof columns[index][i].field != 'undefined' && columns[index][i].field != '') {
                        nameList += ',{"f":"' + columns[index][i].field + '", "a":"' + columns[index][i].align + '"}';
                    }
                    tableString += '>' + columns[index][i].title + '</th>';
                }
            }
            tableString += '\n</tr>';
        });
    }
    // Load content
    var rows = printDatagrid.datagrid("getRows"); // This code is all for access to the current page
    var nl = eval('([' + nameList.substring(1) + '])');
    for (var i = 0; i < rows.length; ++i) {
        tableString += '\n<tr>';
        $(nl).each(function (j) {
            var e = nl[j].f.lastIndexOf('_0');

            tableString += '\n<td';
            if (nl[j].a != 'undefined' && nl[j].a != '') {
                tableString += ' style="text-align:' + nl[j].a + ';"';
            }
            tableString += '>';
            if (e + 2 == nl[j].f.length) {
                tableString += rows[i][nl[j].f.substring(0, e)];
            }
            else
                tableString += rows[i][nl[j].f];
            tableString += '</td>';
        });
        tableString += '\n</tr>';
    }
    tableString += '\n</table>';


    win = document.open(",",'height=1000,width=1200,scrollbars=yes,status =yes');

    win.document.write(tableString);


    //var w = window.open('');
    //w.document.write(tableString.wrap('<div></div>').parent().html());
     //window.open(tableString,"location:No;status:No;help:No;dialogWidth:800px;dialogHeight:600px;scroll:auto;");
    //w.document.write(data.wrap('<div></div>').parent().html());

    //window.showModalDialog("print.htm", tableString,
    //"location:No;status:No;help:No;dialogWidth:800px;dialogHeight:600px;scroll:auto;");
}