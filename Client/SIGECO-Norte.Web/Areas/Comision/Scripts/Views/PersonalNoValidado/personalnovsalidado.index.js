var ActionIndexUrls = {};
			
;(function (app) {
    //===========================================================================================
    var current = app.PersonalIndex = {};
    //===========================================================================================

    jQuery.extend(app.PersonalIndex,
        {
            Initialize: function (actionurls) {
                jQuery.extend(ActionIndexUrls, actionurls);
                fnConfigurarGrilla();
            }
        })
})(project);

function fnConfigurarGrilla()
{
	$('#DataGrid').datagrid({
		fitColumns: true,
		idField: 'codigo_personal',
		url: ActionIndexUrls.GetAllJson,
		pagination: true,
		singleSelect: false,
		rownumbers: true,
		pageList: [300, 400, 500],
		pageSize: 300,
    		columns:
		[[
			{ field: 'codigo_personal', hidden: 'true' },
            { field: 'codigo_equivalencia', title: 'Código', width: 50, align: 'center', halign: 'center' },
			{ field: 'nombre_personal', title: 'Nombre', width: 150, align: 'left', halign: 'center' },
			{ field: 'documento', title: 'Doc.', width: 50, align: 'left    ', halign: 'center' },
            { field: 'banco', title: 'Banco', width: 40, align: 'left', halign: 'center' },
            { field: 'tipo_cuenta', title: 'Cuenta', width: 50, align: 'left', halign: 'center' },
            { field: 'nro_cuenta', title: 'Nro Cuenta', width: 80, align: 'center', halign: 'center' },
			{ field: 'canal_grupo', title: 'Canal\Grupo', width: 80, align: 'left', halign: 'center' },
            { field: 'fecha_modifica', title: 'Fecha<br>Modifica', width: 60, align: 'center', halign: 'center' },
            { field: 'usuario_modifica', title: 'Usuario<br>Modifica', width: 35, align: 'left', halign: 'center' },
            { field: 'tipo_validacion', title: 'Tipo', width: 50, align: 'left', halign: 'center' },
            { field: 'ck', title: '', checkbox: true }
		]],
		loadFilter: function (data) {
			ClearSelection();
			return data;
		}
	});

	$('#DataGrid').datagrid('enableFilter');

	var pager = $('#DataGrid').datagrid('getPager');
	pager.pagination({
	    showPageList: true,
	    buttons: [{
	        iconCls: 'icon-excel',
	        text: 'Expotar',
	        disabled: false,
	        handler: function () {
	            Exportar('DataGrid', 'NoValidados');
	        },
	    }]
	});


	$(window).resize(function () {
		$('#DataGrid').datagrid('resize');
	});

	$("#btnGuardar").on("click", function () {
		if (!$("#btnGuardar").linkbutton("options").disabled) {
		    GuardarValidacion();
		}
	});

	$("#btnDetalle").on("click", function () {
	    if (!$("#btnDetalle").linkbutton("options").disabled) {
	        Detalle();
	    }
	});

}

function GetAllJson() {
	$('#DataGrid').datagrid('reload');
	ClearSelection();
}

function Detalle() {
    var nombreOpcion = 'Detalle'
    var row = $('#DataGrid').datagrid('getSelected');


    if (!row) {
        $.messager.alert(nombreOpcion, 'Por favor seleccione un registro.', 'warning');
        return false;
    }

    AbrirVentanaPersonal(row.codigo_personal, ' : ' + row.codigo_equivalencia, false);
}

function ClearSelection()
{
	$('#DataGrid').datagrid('clearSelections');
	$("#hdCodigo").val('');
}

function AbrirVentanaPersonal(codigoPersonal, mensaje, escritura)
{
	$(this).AbrirVentanaEmergente({
		parametro: "?codigo_personal=" + codigoPersonal,
        div: (escritura ? 'dlgRegistro' : 'dlgDetalle'),
		title: (escritura ? "Mantenimiento de Vendedor " : "Detalle de Vendedor") + mensaje,
        url: (escritura ? ActionIndexUrls.Registro : ActionIndexUrls.Detalle)
	});
}

function GuardarValidacion() {
    var rows = $('#DataGrid').datagrid('getSelections');

    if (rows.length == 0) {
        $.messager.alert('Guardar', "Para continuar con el proceso debe seleccionar uno o mas registros.", 'warning');
        return;
    };

    var lst_entidad = [];
    var validacion = "";
    $.each(rows, function (index, data) {
        var v_entidad = {
            codigo_personal: data.codigo_personal
        }
        lst_entidad.push(v_entidad);
    });

    $.messager.confirm('Confirmaci&oacute;n', '¿Est&aacute; seguro que desea guardar esta validaci&oacute;n?', function (result) {
        if (result) {
            $.ajax({
                type: 'post',
                url: ActionIndexUrls.Validar,
                data: JSON.stringify({ lst_personal: lst_entidad }),
                async: true,
                cache: false,
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    if (data.v_resultado == 1) {
                        GetAllJson();
                        $.messager.alert('Operación exitosa', 'Se guard&oacute; correctamente la validaci&oacute;n.', 'info');
                    }
                    else {
                        $.messager.alert('Error', data.vMensaje, 'error');
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
        filename: 'Listado_' + titulo + '.xls',
        worksheet: 'Worksheet'
    });
}