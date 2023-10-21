var ActionIndexUrl = {};

; (function (app) {
    //===========================================================================================
    var current = app.logprocesobono_index = {};
    //===========================================================================================

    jQuery.extend(app.logprocesobono_index,
        {
            Initialize: function (actionUrls) {
                jQuery.extend(ActionIndexUrl, actionUrls);
                fnInicializarFechas();
                fnConfigurarGrilla();
                
            }
        })
})(project);

function fnInicializarFechas() {
	var fecha_inicio = '';
	var fecha_fin = '';

	$.ajax({
		type: 'post',
		url: ActionIndexUrl.GetFechas,
		async: false,
		cache: false,
		dataType: 'json',
		success: function (data) {
			if (data.Msg) {
				project.AlertErrorMessage('Error', data.Msg);
			} else {
				fecha_inicio = data.fecha_inicio;
				fecha_fin = data.fecha_fin;
			}
		},
		error: function () {
			project.AlertErrorMessage('Error', 'Error');
		}
	});

	$('#fechaInicio').textbox('setText', fecha_inicio);
	$('#fechaFin').textbox('setText', fecha_fin);
	
}

function fnConfigurarGrilla(){
	var fecha_inicio = $.trim($('#fechaInicio').textbox('getText'));
	var fecha_fin = $.trim($('#fechaFin').textbox('getText'));

	var queryParams = {
		fecha_inicio: FormatoFecha(fecha_inicio), fecha_fin: FormatoFecha(fecha_fin)
	};

	$('#DataGrid').datagrid({
		url: ActionIndexUrl.GetAllJson,
		fitColumns: true,
		idField: 'codigo_empresa, nro_contrato',
		queryParams: queryParams,
		pagination: true,
		singleSelect: true,
		rownumbers: true,
		pageList: [20, 60, 80, 100, 150],
		pageSize: 20,
		columns:
		[[
			{ field: 'codigo_planilla', hidden: true },
			{ field: 'nro_planilla', title: 'Nro. Planilla', width: 60, align: 'center', halign: 'center' },
			{ field: 'canal', title: 'Canal', width: 60, align: 'left', halign: 'center' },
			{ field: 'tipo_planilla', title: 'Tipo<br>Planilla', width: 60, align: 'left', halign: 'center' },
			{ field: 'fecha_inicio', title: 'Fecha<br>Inicio', width: 80, align: 'center', halign: 'center' },
			{ field: 'fecha_fin', title: 'Fecha<br>Fin', width: 80, align: 'center', halign: 'center' },
			{ field: 'usuario', title: 'Usuario', width: 100, align: 'left', halign: 'center' },
			{ field: 'fecha_registra', title: 'Fecha<br>Proceso', width: 100, align: 'center', halign: 'center' },
		]]
	});
}

function GetAllJson() {
	var fecha_inicio = $.trim($('#fechaInicio').textbox('getText'));
	var fecha_fin = $.trim($('#fechaFin').textbox('getText'));

	if (!ValidarFecha(fecha_inicio)) {
		$.messager.alert("Log", "Fecha Inicio en formato incorrecto.", "warning");
		return false;
	}

	if (!ValidarFecha(fecha_fin)) {
		$.messager.alert("Log", "Fecha Fin en formato incorrecto.", "warning");
		return false;
	}

	fecha_inicio = FormatoFecha(fecha_inicio);
	fecha_fin = FormatoFecha(fecha_fin);

	if (parseInt(fecha_inicio) > parseInt(fecha_fin)) {
		$.messager.alert("Log", "Inicio debe ser menor a Fin.", "warning");
		return false;
	}

	var queryParams = {
		fecha_inicio: fecha_inicio, fecha_fin: fecha_fin
	};
	
	$('#DataGrid').datagrid('reload', queryParams);
	ClearSelection();
}

function Detalle() {
    var row = $('#DataGrid').datagrid('getSelected');

	if (!row) {
		$.messager.alert('Detalle', 'Por favor seleccione un registro', 'warning');
		return false;
	}

	AbrirVentanaLocal(row.codigo_planilla);
}

function ClearSelection()
{
	$('#DataGrid').datagrid('clearSelections');
}

function AbrirVentanaLocal(codigo_planilla)
{
	$(this).AbrirVentanaEmergente({
		parametro: "?codigo_planilla=" + codigo_planilla,
		div: 'dlgDetalle',
		title: "Detalle de Proceso",
		url: ActionIndexUrl._Detalle
	});
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

