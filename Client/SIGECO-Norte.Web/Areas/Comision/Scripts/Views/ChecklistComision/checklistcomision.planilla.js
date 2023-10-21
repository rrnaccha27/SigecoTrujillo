var ActionPlanillaUrl = {};

; (function (app) {
    //===========================================================================================
    var current = app.checklist = {};
    //===========================================================================================

    jQuery.extend(app.checklist,
        {
            Initialize: function (actionUrls) {
                jQuery.extend(ActionPlanillaUrl, actionUrls);
                fnInicializarDetalle();
                fnConfigurarGrillaDetallePendiente();
                fnConfigurarGrillaDetalleValidado();
            }
        })
})(project);

function fnInicializarDetalle() {
}

function fnConfigurarGrillaDetallePendiente() {

    $('#dg_detalle_pendiente').datagrid({
        nowrap: false,
        fitColumns: true,
        //height: '300',
        idField: 'codigo_checklist_detalle',
        data: null,
        rownumbers: false,
        url: ActionPlanillaUrl._GetDetalleJson,
        queryParams: {
            codigo_checklist: ActionPlanillaUrl.codigo_checklist, validado : 0
        },
        pagination: true,
        singleSelect: false,
        pageList: [300, 400, 500],
        pageSize: 300,
        columns:
        [[
            { field: 'codigo_checklist_detalle', title: '', hidden: 'false' },
            { field: 'codigo_grupo', title: '', hidden: 'false' },
            { field: 'nombre_grupo', title: 'Grupo', hidden: 'false' },
            { field: 'nombre_empresa', title: 'Empresa', width: 80, halign: "center", align: "left" },
            { field: 'ruc_personal', title: 'Ruc', hidden: 'false' },
            { field: 'nombre_personal', title: 'Vendedor', width: 350, align: 'left' },
            { field: 'numero_planilla', title: 'Planilla', width: 80, align: 'center' },
            {
                field: 'importe_abono_personal', title: 'Importe', width: 100, align: 'right', formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                }
            },
            { field: 'ck', title: '', checkbox: true },
        ]],
        collapsible: true,
        groupField: 'codigo_grupo',
        view: groupview,
        groupFormatter: function (value, rows) {
            return rows.length > 0 ? rows[0].nombre_grupo + ',  ' + rows.length + ' vendedores' : "Sin datos";
        },
        onLoadSuccess: function () {
            setTabTitle("#tabChecklist", 0, "Pendientes", "#dg_detalle_pendiente");
        },
    });

    $('#dg_detalle_pendiente').datagrid('enableFilter');

    $(window).resize(function () {
        $('#dg_detalle_pendiente').datagrid('resize');
    });

    if (ActionPlanillaUrl.ocultar != ''){
        $('#dg_detalle_pendiente').datagrid('hideColumn', 'ck');
    }

    var pager = $('#dg_detalle_pendiente').datagrid('getPager');
    pager.pagination({
        showPageList: true,
        buttons: [{
            iconCls: 'icon-excel',
            text: 'Expotar',
            disabled: false,
            handler: function () {
                Exportar('dg_detalle_pendiente', 'Pendientes');
            },
        }]
    });

}

function fnConfigurarGrillaDetalleValidado() {

    $('#dg_detalle_validado').datagrid({
        fitColumns: true,
        idField: 'codigo_checklist_detalle',
        data: null,
        rownumbers: false,
        url: ActionPlanillaUrl._GetDetalleJson,
        queryParams: {
            codigo_checklist: ActionPlanillaUrl.codigo_checklist, validado: 1
        },
        pagination: true,
        singleSelect: true,
        pageList: [300, 400, 500],
        pageSize: 300,
        view: groupview,
        groupField: 'codigo_grupo',
        groupFormatter: function (value, rows) {
            return rows.length > 0 ? rows[0].nombre_grupo + ',  ' + rows.length + ' vendedores' : "Sin datos";
        },
        columns:
        [[
            { field: 'codigo_checklist_detalle', title:'', hidden: 'false' },
            { field: 'codigo_grupo', title: '', hidden: 'false' },
            { field: 'nombre_grupo', title: 'Grupo', hidden: 'false' },
            { field: 'nombre_empresa', title: 'Empresa', width: 80, halign: "center", align: "left" },
            { field: 'ruc_personal', title: 'Ruc', hidden: 'false' },
            { field: 'nombre_personal', title: 'Vendedor', width: 350, align: 'left' },
            { field: 'numero_planilla', title: 'Planilla', width: 80, align: 'center' },
            {
                field: 'importe_abono_personal', title: 'Importe', width: 100, align: 'right', formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                }
            },
            { field: 'usuario_modifica', title: 'Usuario', width: 150, align: 'left' },
            { field: 'fecha_modifica', title: 'Fecha', width: 100, align: 'center' },
        ]],
        onLoadSuccess: function () {
            setTabTitle("#tabChecklist", 1, "Revisados", "#dg_detalle_validado");
        },
    });

    $('#dg_detalle_validado').datagrid('enableFilter');

    $(window).resize(function () {
        $('#dg_detalle_validado').datagrid('resize');
    });

    var pager = $('#dg_detalle_validado').datagrid('getPager');
    pager.pagination({
        showPageList: true,
        buttons: [{
            iconCls: 'icon-excel',
            text: 'Expotar',
            disabled: false,
            handler: function () {
                Exportar('dg_detalle_validado', 'Revisado');
            },
        }]
    });

}

function fnConsultarDetalle() {
    $('#dg_detalle_pendiente').datagrid('reload');
    $('#dg_detalle_validado').datagrid('reload');
}

function CerrarRevision() {
    $("#dlgRevisar").dialog("close");
}

function AnularChecklist(codigo_checklist) {

    var pEntidad = {
        codigo_checklist: codigo_checklist
    };

    $.messager.confirm('Confirmaci&oacute;n', '¿Est&aacute; seguro que desea anular el CheckList?', function (result) {
        if (result) {
            $.ajax({
                type: 'post',
                url: ActionPlanillaUrl._Anular,
                data: JSON.stringify({ checklist: pEntidad }),
                async: true,
                cache: false,
                dataType: 'json',
                contentType: 'application/json',
                success: function (data) {
                    if (data.v_resultado == 1) {
                        $.messager.alert('Operación exitosa', 'Se anul&oacute; correctamente.', 'info', function () {
                            fnConsultarIndex();
                            CerrarRevision();
                            Revisar(codigo_checklist);
                        });
                    }
                    else {
                        $.messager.alert('Anular', data.v_mensaje, 'error');
                    }
                },
                error: function () {
                    $.messager.alert('Error', "Error en el servidor", 'error');
                }
            });
        }
    });
}

function CerrarChecklist(codigo_checklist) {
    var nombreOpcion = 'Cerrar';
    var pEntidad = {
        codigo_checklist: codigo_checklist
    };
    var rows = $('#dg_detalle_validado').datagrid('getData').total;

    if (!rows) {
        $.messager.alert(nombreOpcion, "Debe de existir al menos un registro revisado.", 'warning');
        return;
    }

    if (rows == 0) {
        $.messager.alert(nombreOpciontitulo, "Debe de existir al menos un registro revisado.", 'warning');
        return;
    }

    $.messager.confirm('Confirmaci&oacute;n', '¿Est&aacute; seguro que desea cerrar el CheckList?', function (result) {
        if (result) {
            $.ajax({
                type: 'post',
                url: ActionPlanillaUrl._Cerrar,
                data: JSON.stringify({ checklist: pEntidad }),
                async: true,
                cache: false,
                dataType: 'json',
                contentType: 'application/json',
                success: function (data) {
                    if (data.v_resultado == 1) {
                        $.messager.alert('Operación exitosa', 'Se cerr&oacute; correctamente.', 'info', function () {
                            fnConsultarIndex();
                            CerrarRevision();
                            Revisar(codigo_checklist);
                        });
                    }
                    else {
                        $.messager.alert('Cerrar', data.v_mensaje, 'error');
                    }
                },
                error: function () {
                    $.messager.alert('Error', "Error en el servidor", 'error');
                }
            });
        }
    });
}

function GuardarChecklist()
{
    var rows = $('#dg_detalle_pendiente').datagrid('getSelections');

    if (rows.length == 0) {
        $.messager.alert('Guardar', "Para continuar con el proceso debe seleccionar uno o mas registros.", 'warning');
        return;
    };

    var lst_entidad = [];
    var validacion = "";
    $.each(rows, function (index, data) {
        var v_entidad = {
            codigo_checklist_detalle: data.codigo_checklist_detalle
        }
        lst_entidad.push(v_entidad);
    });

    $.messager.confirm('Confirmaci&oacute;n', '¿Est&aacute; seguro que desea guardar esta revisi&oacute;n?', function (result) {
        if (result) {
            $.ajax({
                type: 'post',
                url: ActionPlanillaUrl._Validar,
                data: JSON.stringify({ checklist_detalle: lst_entidad }),
                async: true,
                cache: false,
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    if (data.v_resultado == 1) {
                        fnConsultarDetalle();
                        $.messager.alert('Operación exitosa', 'Se guard&oacute; correctamente la revisi&oacute;n.', 'info');
                    }
                    else {
                        $.messager.alert('Error al asignar', data.vMensaje, 'error');
                    }
                },
                error: function () {
                    $.messager.alert('Error', "Error en el servidor", 'error');
                }
            });
        }
    });
}

function Exportar(nombre_dg, titulo) {
    var rows = $('#' + nombre_dg).datagrid('getData').total;

    if (!rows) {
        $.messager.alert('Exportar ' + titulo, "No se han detectado registros.", 'warning');
        return;
    }

    if (rows == 0) {
        $.messager.alert('Exportar ' + titulo, "No se han detectado registros.", 'warning');
        return;
    }

    $('#' + nombre_dg).datagrid('toExcel', {
        filename: 'CheckList_' + titulo + '_' + ActionPlanillaUrl.numero_checklist + '.xls',
        worksheet: 'Worksheet'
    });
}

function setTabTitle(selector, index, newTitle, nombre_dg) {
    try{
        var tab = $(selector).tabs('getTab', index);

        $(selector).tabs('update', {
            tab: tab,
            options: {
                title: newTitle + ' - ' + $(nombre_dg).datagrid('getData').total
            }
        });
    }
    catch(e){
    }
}