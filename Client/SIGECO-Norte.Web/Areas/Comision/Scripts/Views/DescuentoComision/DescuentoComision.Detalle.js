var ActionDetalleUrl = {};

; (function (app) {
    //===========================================================================================
    var current = app.descuentocomision_detalle = {};
    //===========================================================================================
    jQuery.extend(app.descuentocomision_detalle,
        {
            Initialize: function (actionDetalleUrls) {
                jQuery.extend(ActionDetalleUrl, actionDetalleUrls);
                fnConfigurarGrilla();
                
            }
        })
})(project);

function fnConfigurarGrilla() {
	var queryParams = {
        codigo_descuento_comision: ActionDetalleUrl._codigo_descuento_comision
	};

	$('#dgDetalle').datagrid({
        url: ActionDetalleUrl.GetPlanillaJson,
		fitColumns: true,
		queryParams: queryParams,
		pagination: true,
		singleSelect: true,
		rownumbers: true,
		pageList: [20, 60, 80, 100, 150],
		pageSize: 20,
		columns:
		[[
			{ field: 'nro_planilla', title: 'Nro.<br>Planilla', width: 100, align: 'center', halign: 'center' },
			{ field: 'nombre_planilla', title: 'Planilla', width: 100, align: 'left', halign: 'center' },
			{ field: 'nombre_tipo_planilla', title: 'Tipo<br>Planilla', width: 100, align: 'left', halign: 'center' },
            { field: 'fecha_inicio', title: 'Fecha Inicio', width: 80, align: 'center', halign: 'center' },
            { field: 'fecha_fin', title: 'Fecha Fin', width: 80, align: 'center', halign: 'center' },
            { field: 'estado_planilla', title: 'Estado<br>Planilla', width: 100, align: 'center', halign: 'center' },
            {
                field: 'monto', title: 'Monto', width: 100, align: 'right', halign: 'center', formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                }
            },
            { field: 'fecha_registra', title: 'Fecha<br>Registro', width: 80, align: 'center', halign: 'center' },
            { field: 'usuario_registra', title: 'Usuario<br>Registro', width: 120, align: 'left', halign: 'center' },
            { field: 'codigo_estado', hidden: true },
		]]
	});
}

function Cerrar() {
    $('#dlgDetalle').dialog('close');
}

