var ActionListadoPlanilla = {};

; (function (app) {
    //===========================================================================================
    var current = app.checklist_listado_planilla = {};
    //===========================================================================================
    jQuery.extend(app.checklist_listado_planilla,
        {
            Initialize: function (actionListadoPlanillas) {
                jQuery.extend(ActionListadoPlanilla, actionListadoPlanillas);
                fnConfigurarListado();
                
            }
        })
})(project);

function fnConfigurarListado() {

    $('#dgPlanilla').datagrid({
        url: ActionListadoPlanilla.GetPlanillaJson,
        idField: 'codigo_planilla',
        fitColumns: true,
        pagination: true,
        singleSelect: true,
        rownumbers: true,
        pageList: [20, 60, 80, 100, 150],
        pageSize: 20,
        columns:
		[[
            { field: 'numero_planilla', title: 'Nro. Planilla', width: 100, align: 'center' },
            { field: 'nombre_regla_tipo_planilla', title: 'Planilla', width: 200, align: 'left' },
            { field: 'fecha_inicio', title: 'Fecha<br>Inicio', width: 120, align: 'center' },
            { field: 'fecha_fin', title: 'Fecha<br>Fin', width: 120, align: 'center' },
            { field: 'nombre_estado_planilla', title: 'Estado', width: 120, align: 'center'},
            { field: 'fecha_apertura', title: 'Fecha<br>Apertura', width: 120, align: 'center' },
            { field: 'fecha_cierre', title: 'Fecha<br>Cierre', width: 120, align: 'center' }
		]],
        onDblClickRow: function (index, row)
        {
            GenerarCheckList(row['codigo_planilla']);
        }
	});

    $('#dgPlanilla').datagrid('enableFilter');
}

function CerrarSeleccion() {
    $('#dlgListadoPlanilla').dialog('close');
}

function AceptarSeleccion() {
    var row = $('#dgPlanilla').datagrid('getSelected');

    if (!row)
    {
        $.messager.alert("Listado Planilla", "Debe seleccionar un registro.", "warning");
        return;
    }

    GenerarCheckList(row.codigo_planilla)
}

function GenerarCheckList(codigo_planilla)
{
    var pEntidad = {
        codigo_planilla: codigo_planilla
    };

    $.messager.confirm('Confirmaci&oacute;n', '&#191;Est&aacute; seguro que desea aperturar el CheckList?', function (result) {
        if (result) {
            $.ajax({
                type: 'post',
                url: ActionListadoPlanilla.AperturarChecklist,
                data: JSON.stringify({ planilla: pEntidad }),
                async: true,
                cache: false,
                dataType: 'json',
                contentType: 'application/json',
                success: function (data) {
                    if (data.v_resultado == 1) {
                        $.messager.alert('Operaci&oacute;n exitosa', "Se apertur&oacute; de forma exitosa.", 'info', function () {
                            CerrarSeleccion();
                            fnConsultarIndex();
                        });
                    }
                    else {
                        $.messager.alert('Error en la apertura.', data.v_mensaje, 'error');
                    }
                },
                error: function () {
                    $.messager.alert('Error', "Error en el server", 'error');
                }
            });
        }
    });

}
