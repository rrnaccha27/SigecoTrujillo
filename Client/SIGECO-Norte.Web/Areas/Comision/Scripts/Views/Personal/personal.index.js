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
		singleSelect: true,
		rownumbers: true,
		pageList: [20, 60, 80, 100, 150],
		pageSize: 20,
		queryParams: {
			codigoCanal: '-1', codigoGrupo: '-1', estadoPersonal: '-1', nombre: ''
		},
		columns:
		[[
			{ field: 'codigo_personal', hidden: 'true' },
			{ field: 'codigo_equivalencia', title: 'Codigo', width: 50, align: 'center', halign: 'center' },
			{ field: 'nombre_completo', title: 'Nombres y Apellidos', width: 170, align: 'left', halign: 'center' },
            { field: 'validado', title: 'Validado', width: 30, align: 'center', halign: 'center' },
            { field: 'nombre_tipo_documento', title: 'Tipo<br>Doc', width: 30, align: 'left', halign: 'center' },
			{ field: 'nro_documento', title: 'Nro Doc', width: 50, align: 'right', halign: 'center' },
			{ field: 'nombre_canal', title: 'Canal de Venta', width: 80, align: 'left', halign: 'center' },
			{ field: 'es_supervisor_canal', title: 'Supervisor<br>Canal', width: 50, align: 'center', halign:'center', formatter: function (value, row, index) { if (row.es_supervisor_canal) { return 'Si'; } else { return ''; } } },
			{ field: 'nombre_grupo', title: 'Grupo', width: 80, align: 'left', halign: 'center' },
			{ field: 'es_supervisor_grupo', title: 'Supervisor<br>Grupo', width: 50, align: 'center', halign: 'center', formatter: function (value, row, index) { if (row.es_supervisor_grupo) { return 'Si'; } else { return ''; } } },
			{ field: 'estado_registro', title: 'Estado', width: 35, align: 'center', halign: 'center', formatter: function (value, row, index) { if (row.estado_registro) { return 'Activo'; } else { return 'Inactivo'; } } },
			{ field: 'indica_estado', hidden: 'true', title: 'Estado', width: 35, align: 'center', halign: 'center', formatter: function (value, row, index) { if (row.estado_registro) { return '1'; } else { return '0'; } } }
		]],
		onClickRow: function (index, row) {
			var rowColumn = row['codigo_personal'];
			$("#hdCodigo").val(rowColumn);
			rowColumn = row['nombre'];
			$("#hdNombre").val(rowColumn);
			rowColumn = row['estado_registro'];
			$("#hdEstado").val(rowColumn);
			rowColumn = row['codigo_equivalencia'];
			$("#hdCodigoEquivalencia").val(rowColumn);

			if (!row['estado_registro']) {
				$("#btnDesactivar").linkbutton('disable');
				$("#btnModificar").linkbutton('disable');
				$("#btnActivar").linkbutton('enable');
			}
			else {
				$("#btnDesactivar").linkbutton('enable');
				$("#btnModificar").linkbutton('enable');
				$("#btnActivar").linkbutton('disable');
			}

		},
		loadFilter: function (data) {
			ClearSelection();
			return data;
		}
	});
	/******************************************************************************************************/
	$('#DataGrid').datagrid('enableFilter', [{
		field: 'estado_registro',
		type: 'combobox',
		options: {
			editable:false,
			panelHeight: 'auto',
			data: [{ value: '', text: 'Todos' }, { value: '1', text: 'Activo' }, { value: '0', text: 'Inactivo' }],
			onChange: function (value) {

				if (value == '') {
					$('#DataGrid').datagrid('removeFilterRule', 'indica_estado');
				} else {
					$('#DataGrid').datagrid('addFilterRule', {
						field: 'indica_estado',
						op: 'equal',
						value: value
					});
				}
				$('#DataGrid').datagrid('doFilter');
			}
		}
	}]);
	/*********************************************************************************************************/

	$(window).resize(function () {
		$('#DataGrid').datagrid('resize');
	});

	var GrupoFiltro = $('#cboGrupoFiltro').combobox({
		valueField: 'id',
		textField: 'text'
	});

	$('#cboCanalFiltro').combobox({
		valueField: 'id',
		textField: 'text',
		url: ActionIndexUrls.GetListarCanalJson,
		onSelect: function (rec) {
			$.get(ActionIndexUrls.GetListarGrupoJson, { 'codigo_canal': rec.id }, function (data) {
				GrupoFiltro.combobox("clear").combobox('loadData', data);
			});
		}
	});

	$("#btnModificar").on("click", function () {
		if (!$("#btnModificar").linkbutton("options").disabled) {
			Modificar();
		}
	});

	$("#btnDesactivar").on("click", function () {
		if (!$("#btnDesactivar").linkbutton("options").disabled) {
			Desactivar();
		}
	});

	$("#btnActivar").on("click", function () {
		if (!$("#btnActivar").linkbutton("options").disabled) {
			Activar();
		}
	});

    $("#btnDetalle").on("click", function () {
        if (!$("#btnDetalle").linkbutton("options").disabled) {
            Detalle();
        }
    });

}

function ObtenerFiltro(nombreCombo) {
	var codigo = $.trim($('#' + nombreCombo).combobox('getValue'));
	if (!codigo) {
		codigo = '-1';
	}
	return codigo;
}

function GetAllJson() {
	if (!$("#frm_consultar").form('enableValidation').form('validate'))
		return;

	var queryParams = {
		codigoCanal : ObtenerFiltro('cboCanalFiltro'),
		codigoGrupo : ObtenerFiltro('cboGrupoFiltro'),
		estadoPersonal : -1,
		nombre : $.trim($('#nombre').textbox('getText'))
	};

	$('#DataGrid').datagrid('reload', queryParams);
	ClearSelection();
}

function Crear() {
	AbrirVentanaPersonal(-1, '', true);
}

function Modificar() {
	var codigo = $.trim($("#hdCodigo").val());
	if (!codigo) {
		$.messager.alert('Modificar', 'Por favor seleccione un registro.', 'warning');
		return false;
	}

	if ($("#hdEstado").val() == 'false') {
		$.messager.alert('Modificar', 'No se puede modificar vendedor inactivo.', 'warning');
		return false;
	}

	$.ajax({
	    type: 'post',
	    url: ActionIndexUrls.GetCantidadBloqueo,
	    data: { codigo_personal: codigo },
	    async: false,
	    cache: false,
	    dataType: 'json',
	    success: function (data) {
	        if (data.Msg) {
	            if (data.Msg != 'Success') {
	                $.messager.alert('Modificar', 'No se puede efectuar esta acción. Verifique los bloqueos por planilla con la opcion [Detalle].', 'warning');
	            }
	            else {
	                AbrirVentanaPersonal(codigo, ' : ' + $('#hdCodigoEquivalencia').val(), true);
	            }
	        }
	        else {
	            project.AlertErrorMessage('Error', 'Error de procesamiento.');
	        }
	    },
	    error: function () {
	        project.AlertErrorMessage('Error', 'Error');
	    }
	});


	
}

function Detalle() {
    var nombreOpcion = 'Detalle'
    var codigo = $.trim($("#hdCodigo").val());
    if (!codigo) {
        $.messager.alert(nombreOpcion, 'Por favor seleccione un registro.', 'warning');
        return false;
    }

    //if ($("#hdEstado").val() == 'false') {
    //    $.messager.alert('Modificar', 'No se puede modificar vendedor inactivo.', 'warning');
    //    return false;
    //}

    AbrirVentanaPersonal(codigo, ' : ' + $('#hdCodigoEquivalencia').val(), false);
}

function Desactivar() {
	var codigo = $.trim($("#hdCodigo").val());
	if (!codigo) {
		$.messager.alert('Desactivar', 'Por favor seleccione un registro.', 'warning');
		return false;
    }

    var row = $('#DataGrid').datagrid("getSelected");
    var esSupervisor = 0;
    if (row.es_supervisor_canal || row.es_supervisor_grupo) {
        esSupervisor = 1;
    }
    var codigo_equivalencia = row.codigo_equivalencia;

	$.messager.confirm('Confirm', '¿Seguro que desea desactivar este vendedor?', function (result) {
		if (result) {
			$.ajax({
				type: 'post',
				url: ActionIndexUrls.Desactivar,
				data: { codigo: codigo, esSupervisor:esSupervisor, codigo_equivalencia: codigo_equivalencia },
				async: false,
				cache: false,
				dataType: 'json',
				success: function (data) {
					if (data.Msg) {
						if (data.Msg != 'Success') {
							$.messager.alert('Error', data.Msg, 'error');
						}
						else {
							project.ShowMessage('Alerta', 'Se desactivó con éxito.');
							GetAllJson();
						}
					}
					else {
						project.AlertErrorMessage('Error', 'Error de procesamiento.');
					}
				},
				error: function () {
					project.AlertErrorMessage('Error', 'Error');
				}
			});
		}
	});
}

function Activar()
{
    var row = $('#DataGrid').datagrid('getSelected');

    if (!row) {
        $.messager.alert('Activar', 'Por favor seleccione un registro.', 'warning');
        return false;
    }

    var esSupervisor = 0;
    if (row.es_supervisor_canal || row.es_supervisor_grupo) {
        esSupervisor = 1;
    }
    var codigo_equivalencia = row.codigo_equivalencia;

    $.messager.confirm('Confirm', '¿Seguro que desea activar este vendedor?', function (result) {
        if (result) {
            $.ajax({
                type: 'post',
                url: ActionIndexUrls.Activar,
                data: { codigo: row.codigo_personal, esSupervisor: esSupervisor, codigo_equivalencia: codigo_equivalencia },
                async: false,
                cache: false,
                dataType: 'json',
                success: function (data) {
                    if (data.Msg) {
                        if (data.Msg != 'Success') {
                            $.messager.alert('Error', data.Msg, 'error');
                        }
                        else {
                            project.ShowMessage('Alerta', 'Se activó con éxito.');
                            GetAllJson();
                        }
                    }
                    else {
                        project.AlertErrorMessage('Error', 'Error de procesamiento.');
                    }
                },
                error: function () {
                    project.AlertErrorMessage('Error', 'Error');
                }
            });
        }
    });
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

function fnReporte() {

	var p_codigo_canal = ObtenerFiltro('cboCanalFiltro');
	var p_codigo_grupo = ObtenerFiltro('cboGrupoFiltro');
	var p_estado_registro = '1';

	$(this).AbrirVentanaEmergente({
		parametro: "?p_codigo_canal=" + p_codigo_canal + "&&p_codigo_grupo=" + p_codigo_grupo + "&&p_estado_registro=" + p_estado_registro,
		div: 'div_reporte_personal',
		title: "Reporte de Vendedor",
		url: ActionIndexUrls.Reporte
	});
}

