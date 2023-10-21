var ActionDetalleUrl = {};

; (function (app) {
    //===========================================================================================
    var current = app.logprocesobono_detalle = {};
    //===========================================================================================
    jQuery.extend(app.logprocesobono_detalle,
        {
            Initialize: function (actionDetalleUrls) {
                jQuery.extend(ActionDetalleUrl, actionDetalleUrls);
                fnConfigurarGrilla();
                
            }
        })
})(project);

function fnConfigurarGrilla() {
	var queryParams = {
	    codigo_planilla: ActionDetalleUrl._codigo_planilla
	};

	$('#dgDetalle').datagrid({
		url: ActionDetalleUrl.GetDetalleJson,
		fitColumns: true,
		queryParams: queryParams,
		pagination: true,
		singleSelect: true,
		rownumbers: true,
		pageList: [20, 60, 80, 100, 150],
		pageSize: 20,
		columns:
		[[
			{ field: 'nro_contrato', title: 'Nro. Contrato', width: 35, align: 'center', halign: 'center' },
			{ field: 'empresa', title: 'Empresa', width: 30, align: 'left', halign: 'center' },
			{ field: 'canal_grupo', title: 'Canal/Grupo', width: 100, align: 'left', halign: 'center' },
            { field: 'estado_nombre', title: 'Estado', width: 35, align: 'center', halign: 'center' },
			{ field: 'observacion', title: 'Observacion', width: 200, align: 'left', halign: 'center' },
            { field: 'codigo_estado', title: 'Codigo Estado', hidden: true },
            { field: 'monto_ingresado', title: 'Monto Ingresado', hidden: true },
		]]
	});

	$('#dgDetalle').datagrid('enableFilter', [{
	    field: 'estado_nombre',
		type: 'combobox',
		options: {
			editable:false,
			panelHeight: 'auto',
			data: [{ value: '', text: 'Todos' }, { value: '1', text: 'No Procesado' }, { value: '2', text: 'Error' }, { value: '3', text: 'Procesado' }],
			onChange: function (value) {
				if (value == '') {
					$('#dgDetalle').datagrid('removeFilterRule', 'codigo_estado');
				} else {
					$('#dgDetalle').datagrid('addFilterRule', {
						field: 'codigo_estado',
						op: 'equal',
						value: value
					});
				}
				$('#dgDetalle').datagrid('doFilter');
			}
		}
	}]);
}

function Cerrar() {
    $('#dlgDetalle').dialog('close');
}

function ToExcel() {
    $('#dgDetalle').datagrid('toExcel', {
        filename: 'LogProcesoBono.xls',
        worksheet: 'Worksheet'
    });
}
