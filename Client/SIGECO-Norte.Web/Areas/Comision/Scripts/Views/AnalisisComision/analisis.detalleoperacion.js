var ActionDetalleUrl = {};

; (function (app) {
    //===========================================================================================
    var current = app.detalle_operacion = {};
    //===========================================================================================
    jQuery.extend(app.detalle_operacion,
        {
            Initialize: function (actionDetalleUrls) {
                jQuery.extend(ActionDetalleUrl, actionDetalleUrls);
                fnConfigurarGrilla();
                
            }
        })
})(project);

function fnConfigurarGrilla() {
	var queryParams = {
        codigo_detalle_cronograma: ActionDetalleUrl._codigo_detalle_cronograma
	};

	$('#dgDetalle').datagrid({
        url: ActionDetalleUrl.GetListadoOperacionJson,
		fitColumns: true,
		queryParams: queryParams,
		pagination: true,
		singleSelect: true,
		rownumbers: true,
		pageList: [20, 60, 80, 100, 150],
		pageSize: 20,
		columns:
		[[
            { field: 'fecha_operacion', title: 'Fecha', width: 180, align: 'center', halign: 'center' },
            { field: 'nombre_operacion', title: "Operaci&oacute;n", width: 150, align: 'left', halign: 'center' },
			{ field: 'observacion', title: 'Motivo', width: 550, align: 'left', halign: 'center' },
			{ field: 'valor_original', title: 'Valor<br>Original', width: 100, align: 'right', halign: 'center' },
            { field: 'usuario', title: 'Usuario', width: 200, align: 'left', halign: 'center' },
            { field: 'nombre_estado', title: 'Estado', width: 70, align:'left', halign: 'center' },

		]]
        , onRowContextMenu: function (e, index, row) {
            if (index >= 0) {
                $(this).datagrid('selectRow', index);
                e.preventDefault();
                $('#mnuDetalle').menu('show', {
                    left: e.pageX,
                    top: e.pageY
                });
            }
        }
	});
}

function fnCopiarDetalle(atributo) {
    var row = $('#dgDetalle').datagrid("getSelected");
    CopyPaste(row, atributo);
}

function CerrarDetalle() {
    $('#div_detalle_operacion').dialog('close');
}

