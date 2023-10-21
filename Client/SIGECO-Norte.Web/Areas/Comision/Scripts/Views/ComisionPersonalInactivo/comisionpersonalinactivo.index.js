var ActionIndexUrl = {};

; (function (app) {
    //===========================================================================================
    var current = app.index = {};
    //===========================================================================================

    jQuery.extend(app.index,
        {
            Initialize: function (actionUrls) {
                jQuery.extend(ActionIndexUrl, actionUrls);
				InicializarControles();
				InicializarGrillas();
            }
        })
})(project);

function InicializarControles()
{
    var fecha_inicio = '';
    var fecha_fin = '';

    $.ajax({
        type: 'post',
        url: ActionIndexUrl.GetFechasJson,
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

	$('.content').combobox_sigees({
		id: '#codigo_canal',
        url: ActionIndexUrl.GetCanalJson
	});

	$('.content').combobox_sigees({
	    id: '#cmb_liquidado',
	    url: ActionIndexUrl.GetLiquidadoJson
	});

	$('#fechaInicio').datebox({
		formatter: function (date) {
			var y = date.getFullYear();
			var m = date.getMonth() + 1;
			var d = date.getDate();
			return (d < 10 ? ('0' + d) : d) + '/' + (m < 10 ? ('0' + m) : m) + '/' + y;
		},
		parser: function (s) {

			if (!s) return new Date();
			var ss = s.split('/');
			var y = parseInt(ss[2], 10);
			var m = parseInt(ss[1], 10);
			var d = parseInt(ss[0], 10);
			if (!isNaN(y) && !isNaN(m) && !isNaN(d)) {
				return new Date(y, m - 1, d)
			} else {
				return new Date();
			}
		}
	});

	$('#fechaFin').datebox({
		formatter: function (date) {
			var y = date.getFullYear();
			var m = date.getMonth() + 1;
			var d = date.getDate();
			return (d < 10 ? ('0' + d) : d) + '/' + (m < 10 ? ('0' + m) : m) + '/' + y;
		},
		parser: function (s) {

			if (!s) return new Date();
			var ss = s.split('/');
			var y = parseInt(ss[2], 10);
			var m = parseInt(ss[1], 10);
			var d = parseInt(ss[0], 10);
			if (!isNaN(y) && !isNaN(m) && !isNaN(d)) {
				return new Date(y, m - 1, d)
			} else {
				return new Date();
			}
		}
	})

	$('#fechaInicio').textbox('setText', fecha_inicio);
	$('#fechaFin').textbox('setText', fecha_fin);

	if ($("#codigo_canal").data('combobox')) {
		fnFormatComboboxCheck($("#codigo_canal"));
	}

}

function InicializarGrillas()
{
    var fecha_inicio = $.trim($('#fechaInicio').textbox('getText'));
    var fecha_fin = $.trim($('#fechaFin').textbox('getText'));

	var queryParams = {
		fecha_inicio: FormatoFecha(fecha_inicio), fecha_fin: FormatoFecha(fecha_fin), codigo_canal : '', liquidado : '99'
	};

	$('#DataGrid').datagrid({
        height:'550px',
		url: ActionIndexUrl.GetAllJson,
		fitColumns: false,
		idField: 'codigo_detalle_cronograma',
		queryParams: queryParams,
		pagination: true,
		singleSelect: false,
		rownumbers: true,
		pageList: [100, 200, 500],
		pageSize: 100,
		columns:
		[[
            { field: 'ck', title: '', checkbox: true },
            {
                field: 'resultado_n1', title: 'Resultado<br>Comercial', width: 100, align: 'center', halign: 'center', styler: function (value, row, index) {
                    return row.estilo_n1;
                }
            },
            {
                field: 'resultado_n2', title: 'Resultado<br>Adminitracion', width: 100, align: 'center', halign: 'center', styler: function (value, row, index) {
                    return row.estilo_n2;
                }
            },
			{ field: 'nombre_canal', title: 'Canal', width: 140, align: 'left', halign: 'center' },
			{ field: 'nombre_grupo', title: 'Grupo', width: 140, align: 'left', halign: 'center' },
			{ field: 'nombre_tipo_planilla', title: 'Tipo', width: 80, align: 'center', halign: 'center' },
			{ field: 'nombre_personal', title: 'Vendedor', width: 250, align: 'left', halign: 'center' },
			{ field: 'nombre_empresa', title: 'Empresa', width: 80, align: 'center', halign: 'center' },
			{ field: 'nro_contrato', title: 'Nro.<br>Contrato', width: 100, align: 'center', halign: 'center' },
            { field: 'nombre_tipo_venta', title: 'Tipo<br>Venta', width: 80, align: 'center', halign: 'center' },
            { field: 'nombre_tipo_pago', title: 'Tipo<brPago', width: 80, align: 'center', halign: 'center' },
            { field: 'nombre_articulo', title: 'Artículo', width: 350, align: 'left', halign: 'center' },
            { field: 'nro_cuota', title: 'Nro.<br>Cuota', width: 85, align: 'center', halign: 'center' },
            {
                field: 'monto_sin_igv', title: 'Importe', width: 85, align: 'right', halign: 'center', formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                }
            },
            {
                field: 'monto_igv', title: 'IGV', width: 85, align: 'right', halign: 'center', formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                }
            },
            {
                field: 'monto_con_igv', title: 'Importe<br>Total', width: 85, align: 'right', halign: 'center', formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                }
            },
            { field: 'observacion', title: 'Observacion', width: 350, align: 'left', halign: 'center' },
            { field: 'fecha_registra', title: 'Fecha', width: 150, align: 'right', halign: 'center' },
            { field: 'usuario_registra', title: 'Usuario', width: 160, align: 'left', halign: 'center' },
		]],
		onClickRow: function (index, row) {
		    if (row.liquidado == 1) {
		        //$('#DataGrid').datagrid('unselectRow', index);
		    }
		}
	});


    $('#DataGrid').datagrid('enableFilter');
}

function GetAllJson() {
	var nombreOpcion = 'Comision Personal Inactivo';
	var fecha_inicio = $.trim($('#fechaInicio').textbox('getText'));
	var fecha_fin = $.trim($('#fechaFin').textbox('getText'));
	var canal = $.trim($('#codigo_canal').combobox('getValues'));
	var liquidado = $.trim($('#cmb_liquidado').combobox('getValue'));

	if (!fecha_inicio) {
		$.messager.alert(nombreOpcion, "Debe seleccionar una Fecha Inicio.", "warning");
		return false;
	}

	if (!ValidarFecha(fecha_inicio)) {
		$.messager.alert(nombreOpcion, "Fecha Inicio en formato incorrecto.", "warning");
		return false;
	}

	if (!fecha_fin) {
		$.messager.alert(nombreOpcion, "Debe seleccionar una Fecha Fin.", "warning");
		return false;
	}

	if (!ValidarFecha(fecha_fin)) {
		$.messager.alert(nombreOpcion, "Fecha Fin en formato incorrecto.", "warning");
		return false;
	}

	fecha_inicio = FormatoFecha(fecha_inicio);
	fecha_fin = FormatoFecha(fecha_fin);

	if (parseInt(fecha_inicio) > parseInt(fecha_fin)) {
		$.messager.alert(nombreOpcion, "Inicio debe ser menor a Fin.", "warning");
		return false;
	}

	if (!canal) {
		$.messager.alert(nombreOpcion, "Debe seleccionar un Canal de Venta.", "warning");
		return false;
	}

	if (!liquidado) {
	    $.messager.alert(nombreOpcion, "Debe seleccionar el estado de la comisión.", "warning");
	    return false;
	}

	var queryParams = {
		fecha_inicio: fecha_inicio, fecha_fin: fecha_fin, codigo_canal : canal, liquidado : liquidado
	};

	$('#DataGrid').datagrid('reload', queryParams);
	ClearSelection();
}

function ClearSelection()
{
	$('#DataGrid').datagrid('clearSelections');
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
			console.log(row)
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

function ExportarExcel() {
    var rows = $("#DataGrid").datagrid("getRows");
    if (rows.length < 1) {
        $.messager.alert("Exportar a Excel", "No existen registros para exportar, intente nuevamente.", "warning");
        return;
    }


    
    $.ajax({
        type: 'post',
        url: ActionIndexUrl.SetExportData,
        data: JSON.stringify({ listado: rows }),
        async: true,
        cache: false,
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            var url = ActionIndexUrl.ExportarExcel + "?id=" + data.v_guid;
            //window.open(url, '_blank');
            window.location.href = url;

            //$(this).AbrirVentanaEmergente({
            //    parametro: "?id=" + data.v_guid,
            //    div: 'div_exportar',
            //    title: "Exportacion",
            //    url: ActionIndexUrl.ExportarExcel
            //});
        },
        error: function (data) {

            $.messager.alert('Error', "Error en el servidor", 'error');
        }
    });

}

function GuardarResultado(nivel_aprobacion, codigo_resultado) {
    var rows = $('#DataGrid').datagrid('getSelections');
    var texto_operacion = (codigo_resultado == 1 ? 'aprobar' : 'rechazar');
    var titulo = (codigo_resultado == 1 ? 'Aprobar' : 'Rechazar');

    if (rows.length == 0) {
        $.messager.alert('Aprobar', "Para continuar con el proceso debe seleccionar uno o mas registros.", 'warning');
        return;
    };

    var lst_entidad = [];
    var validacion = "";
    $.each(rows, function (index, data) {
        if (data.liquidado == 1)
        {
            validacion = 'Se ha seleccionado un registro liquidado, no se efecutará la operación.';
            return false;
        }

        if (nivel_aprobacion == 1)
        {
            if (data.codigo_resultado_n1 != 0 || data.codigo_resultado_n2 != 0)
            {
                validacion = 'Se ha seleccionado un registro que ya fue evaluado.';
                return false;
            }
        }

        if (nivel_aprobacion == 2) {
            if (data.codigo_resultado_n1 != 1) {
                validacion = 'Se ha seleccionado un registro que ya fue rechazado o falta aprobar.';
                return false;
            }
            if (data.codigo_resultado_n2 != 0) {
                validacion = 'Se ha seleccionado un registro que ya fue evaluado.';
                return false;
            }
        }

        var v_entidad = {
            codigo_detalle_cronograma: data.codigo_detalle_cronograma
        }
        lst_entidad.push(v_entidad);
    });

    if (validacion.length > 0) {
        $.messager.alert(titulo, validacion, 'warning');
        return;
    };

    $.messager.observacion(titulo, 'Para ' + texto_operacion + ' ingrese una observacion:', function (win, result) {
        if (result) {
            $.ajax({
                type: 'post',
                url: ActionIndexUrl.Aprobar,
                data: JSON.stringify({ lst_detalle: lst_entidad, nivel: nivel_aprobacion, codigo_resultado: codigo_resultado, observacion: result }),
                async: true,
                cache: false,
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    if (data.v_resultado == 1) {
                        GetAllJson();
                        $.messager.alert(titulo, 'Se efectu&oacute; la operaci&oacute;n.', 'info');
                    }
                    else {
                        $.messager.alert(titulo, data.vMensaje, 'error');
                    }
                },
                error: function () {
                    $.messager.alert('Error', "Error en el servidor", 'error');
                }
            });
        }
    });
}


function Detalle() {
    var rows = $('#DataGrid').datagrid('getSelections');

    if (rows.length > 1) {
        $.messager.alert('Detalle', "Para continuar con el proceso debe seleccionar solo un registro.", 'warning');
        return;
    };

    var row = $('#DataGrid').datagrid('getSelected');
    if (!row) {
        $.messager.alert(nombreOpcion, 'Por favor seleccione un registro.', 'warning');
        return false;
    }

    $(this).AbrirVentanaEmergente({
        parametro: "?codigo_detalle=" + row.codigo_detalle_cronograma,
        div: 'dlgDetalle',
        title: 'Detalle de Comisión',
        url: ActionIndexUrl._Detalle
    });

}
