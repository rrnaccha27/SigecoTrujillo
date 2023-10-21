;
(function (app) {
    //===========================================================================================
    var current = app.Articulo = {};
    //===========================================================================================

    jQuery.extend(app.Articulo,
        {

            ActionUrls: {},
            EditTypes: { Registrar: 'Registrar', Modificar: 'Modificar', Ninguno: '' },
            EditType: '',
            
            Initialize: function (actionUrls) {
                jQuery.extend(project.ActionUrls, actionUrls);
                current.Initialize_DataGrid();
                current.CargarCombos();

                current.Redimensionar();

                $(window).resize(function () {
                    current.Redimensionar();
                });

                $('#btnNuevo').click(function () {
                    current.EditType = current.EditTypes.Registrar;
                    $('#dlgRegistrar').dialog('open').dialog('setTitle', 'NUEVO REGISTRO');
                    $('#frmRegistrar').form('clear');
                    $('#dlgRegistrar #vigente').switchbutton({ checked: true });
                });

                $('#btnRefrescar').click(function () {
                    current.LimpiarGrilla();
                    current.CargarCombos();
                });

                $('#dlgRegistrar #btnGuardar').click(function () {
                    current.Guardar();
                });

                $('#dlgRegistrar #btnCancelar').click(function () {
                    $('#frmRegistrar').form('clear');
                    current.LimpiarGrillaEditable($('#dlgRegistrar #dg'));
                    $('#dlgRegistrar').dialog('close');
                });

                $('#btnModificar').click(function () {

                    var codigo = $.trim($("#hdCodigo").val());
                    if (current.ValidarSeleccion(codigo) && current.ObtenerRegistro(codigo)) {
                        current.EditType = current.EditTypes.Modificar;
                        $('#dlgModificar').dialog('open').dialog('setTitle', 'MODIFICAR REGISTRO');

                        //$('#dlgModificar #codigo').textbox('setText', Registro.codigo_articulo);
                        $('#dlgModificar #nombre').textbox('setText', Registro.nombre);
                        $('#dlgModificar #abreviatura').textbox('setText', Registro.abreviatura);
                        $('#dlgModificar #genera_comision').switchbutton({ checked: Registro.genera_comision });
                        $('#dlgModificar #genera_bono').switchbutton({ checked: Registro.genera_bono });

                        current.GetPrecios(codigo);
                        $('#hdPreciosEliminar').val('');
                    }
                });

                $('#dlgModificar #btnGuardar').click(function () {
                    current.Guardar();
                });

                $('#dlgModificar #btnCancelar').click(function () {
                    $('#frmModificar').form('clear');
                    $('#hdPreciosEliminar').val('');
                    current.LimpiarGrillaEditable($('#dlgModificar #dgPrecioModificar'));
                    $('#dlgModificar').dialog('close');
                });

                $('#btnEliminar').click(function () {
                    var codigo = $.trim($("#hdCodigo").val());
                    if (current.ValidarSeleccion(codigo)) {
                        current.Eliminar(codigo);
                    }
                });
				
				$('#btnBuscar').click(function () {
                    var nombre = $.trim($('#nombre').textbox('getText'));

                    current.GetAll(nombre);
                    current.LimpiarGrilla();
                });

				$('#dlgComision #btnGuardar').click(function () {
				    current.GuardarReglasComision();
				});

				$('#dlgComision #btnCancelar').click(function () {
				    $('#frmComision').form('clear');
				    $('#hdComisionEliminar').val('');
				    $('#hdCodigoPrecio').val('');
				    current.LimpiarGrillaEditable($('#dlgComision #dgComision'));
				    $('#dlgComision').dialog('close');
				});

				
            },

            Initialize_DataGrid: function () {
                $('#DataGrid').datagrid({
                    fitColumns: true,
                    idField: 'codigo_articulo',
                    data: null,
                    pagination: true,
                    singleSelect: true,
                    pageNumber: 0,
                    pageList: [5, 10, 15, 20, 25, 30],
                    pageSize: 10,
                    columns:
                    [[
                        { field: 'codigo_articulo', title: 'Codigo', hidden:'true' },
                        { field: 'nombre', title: 'Nombre', width: 50, align: 'left'},
                        { field: 'abreviatura', title: 'Abreviatura', width: 50, align: 'left' },
                        { field: 'genera_comision', title: 'Comisiona', width: 50, align: 'center', formatter: function (value, row, index) { if (row.genera_comision) { return 'Si'; } else { return 'No'; } } },
						{ field: 'genera_bono', title: 'Bono', width: 50, align: 'center', formatter: function (value, row, index) { if (row.genera_bono) { return 'Si'; } else { return 'No'; } } },
						{ field: 'tiene_precio', title: 'Tiene Precio', width: 50, align: 'center', formatter: function (value, row, index) { if (row.tiene_precio) { return 'Si'; } else { return 'No'; } } },
						{ field: 'tiene_comision', title: 'Tiene Comision', width: 50, align: 'center', formatter: function (value, row, index) { if (row.tiene_comision) { return 'Si'; } else { return 'No'; } } },
                    ]],
                    onClickRow: function (index, row) {
                        var rowColumn = row['codigo_articulo'];
                        $("#hdCodigo").val(rowColumn);
						rowColumn = row['nombre'];
						$("#hdNombre").val(rowColumn);
                    }
                });

                $('#DataGrid').datagrid('enableFilter');
                $('#DataGrid').datagrid('loadData', {});
            },

            Guardar: function () {
                if (current.EditType == current.EditTypes.Registrar) {
                    current.Registrar();
                } else if (current.EditType == current.EditTypes.Modificar) {
                    current.Modificar();
                }
            },

            Registrar: function () {
                var message = '';
                var nombre = $.trim($('#dlgRegistrar #nombre').textbox('getText'));
                var abreviatura = $.trim($('#dlgRegistrar #abreviatura').textbox('getText'));
                var genera_comision = $('#dlgRegistrar #genera_comision').switchbutton('options').checked;
                var genera_bono = $('#dlgRegistrar #genera_bono').switchbutton('options').checked;
                var dataGrilla = '';

                var longitud = $('#dlgRegistrar #dg').datagrid('getRows').length;
                if (longitud > 0){
                    for (var indice = 0 ; indice < longitud; indice++) {
                        $('#dlgRegistrar #dg').datagrid('selectRow', indice);
                        var registro = $('#dlgRegistrar #dg').datagrid('getSelected');

                        dataGrilla += registro.codigo_empresa + ',' + registro.codigo_tipo_venta + ',' + registro.codigo_moneda + ',' + registro.precio + '|';
                    }
                    dataGrilla = dataGrilla.substring(0, dataGrilla.length - 1);
                }

                if (!nombre) {
                    message = 'Por favor ingrese un nombre';
                }
                else if (nombre.length > 250) {
                    message = 'Nombre, numero maximo de caracteres 250';
                }
                else if (!abreviatura) {
                    message = 'Por favor ingrese un abreviatura';
                }
                else if (abreviatura.length > 10) {
                    message = 'Abreviatura, numero maximo de caracteres 10';
                }

                if (message.length > 0) {
                    $.messager.alert('Error', message, 'error');
                }
                else {
                    $.messager.confirm('Confirm', 'Seguro que desea registrar?', function (result) {
                        if (result) {
                            var mapData = { nombre: nombre, abreviatura: abreviatura, genera_comision: genera_comision, genera_bono: genera_bono, dataGrilla: dataGrilla };

                            $.ajax({
                                type: 'post',
                                url: project.ActionUrls.Registrar,
                                data: mapData,
                                dataType: 'json',
                                cache: false,
                                async: false,
                                success: function (data) {
                                    if (data.Msg) {
                                        if (data.Msg != 'Success') {
                                            project.AlertErrorMessage('Error', data.Msg);
                                        }
                                        else {
                                            $('#frmRegistrar').form('clear');
                                            $('#dlgRegistrar').dialog('close');
                                            current.LimpiarGrillaEditable($('#dlgRegistrar #dg'));
                                            project.ShowMessage('Alerta', 'Registro Exitoso');
                                            current.LimpiarGrilla();
                                        }
                                    }
                                    else {
                                        project.AlertErrorMessage('Error', 'Error de procesamiento');
                                    }
                                },
                                error: function () {
                                    project.AlertErrorMessage('Error', 'Error');
                                }
                            });
                        }
                    });
                }
            },

            Modificar: function () {
                var message = '';
                //var codigo = $.trim($('#dlgModificar #codigo').textbox('getText'));
                var codigo = $.trim($("#hdCodigo").val());
                var nombre = $.trim($('#dlgModificar #nombre').textbox('getText'));
                var abreviatura = $.trim($('#dlgModificar #abreviatura').textbox('getText'));
                var genera_comision = $('#dlgModificar #genera_comision').switchbutton('options').checked;
                var genera_bono = $('#dlgModificar #genera_bono').switchbutton('options').checked;
                var dataGrilla = '';
                var preciosEliminados = '';

                if (!codigo) {
                    message = 'Codigo registro nulo';
                }
                else if (!nombre) {
                    message = 'Por favor ingrese un nombre';
                }
                else if (nombre.length > 250) {
                    message = 'Nombre, numero maximo de caracteres 250';
                }
                else if (!abreviatura) {
                    message = 'Por favor ingrese un abreviatura';
                }
                else if (abreviatura.length > 10) {
                    message = 'Abreviatura, numero maximo de caracteres 10';
                }

                var longitud = $('#dlgModificar #dgPrecioModificar').datagrid('getRows').length;
                if (longitud > 0) {
                    for (var indice = 0 ; indice < longitud; indice++) {
                        $('#dlgModificar #dgPrecioModificar').datagrid('selectRow', indice);
                        var registro = $('#dlgModificar #dgPrecioModificar').datagrid('getSelected');

                        dataGrilla += (registro.codigo_precio ? registro.codigo_precio : '') + ',' + registro.codigo_empresa + ',' + registro.codigo_tipo_venta + ',' + registro.codigo_moneda + ',' + registro.precio + '|';
                    }
                    dataGrilla = dataGrilla.substring(0, dataGrilla.length - 1);
                }

                if ($('#hdPreciosEliminar').val().length > 0) {
                    preciosEliminados = $('#hdPreciosEliminar').val();
                    preciosEliminados = preciosEliminados.substring(0, preciosEliminados.length - 1);
                }

                if (message.length > 0) {
                    $.messager.alert('Error', message, 'error');
                }
                else {
                    $.messager.confirm('Confirm', 'Seguro que desea modificar?', function (result) {
                        if (result) {
                            var mapData = { codigo: codigo, nombre: nombre, abreviatura: abreviatura, genera_comision: genera_comision, genera_bono: genera_bono, dataGrilla: dataGrilla, preciosEliminados: preciosEliminados };

                            $.ajax({
                                type: 'post',
                                url: project.ActionUrls.Modificar,
                                data: mapData,
                                dataType: 'json',
                                cache: false,
                                async: false,
                                success: function (data) {
                                    if (data.Msg) {
                                        if (data.Msg != 'Success') {
                                            project.AlertErrorMessage('Error', data.Msg);
                                        }
                                        else {
                                            $('#hdPreciosEliminar').val('');
                                            project.ShowMessage('Alerta', 'Modificacion Exitoso');
                                            current.GetPrecios(codigo);
                                        }
                                    }
                                    else {
                                        project.AlertErrorMessage('Error', 'Error de procesamiento');
                                    }
                                },
                                error: function () {
                                    project.AlertErrorMessage('Error', 'Error');
                                }
                            });
                        }
                    });
                }
            },

            Eliminar: function (codigo) {
                $.messager.confirm('Confirm', 'Seguro que desea desactivar este registro?', function (result) {
                    if (result) {
                        $.ajax({
                            type: 'post',
                            url: project.ActionUrls.Eliminar,
                            data: { codigo: codigo },
                            async: false,
                            cache: false,
                            dataType: 'json',
                            success: function (data) {
                                if (data.Msg) {
                                    if (data.Msg != 'Success') {
                                        $.messager.alert('Error', data.Msg, 'error');
                                    }
                                    else {
                                        project.ShowMessage('Alerta', 'Desactivacion exitosa');
                                        $('#btnBuscar').click();
                                    }
                                }
                                else {
                                    project.AlertErrorMessage('Error', 'Error de procesamiento');
                                }
                            },
                            error: function () {
                                project.AlertErrorMessage('Error', 'Error');
                            }
                        });
                    }
                });
            },

            GuardarReglasComision: function () {

                var dataGrilla = '';
                var comisionesEliminadas = '';
                var codigoPrecio = $.trim($("#hdCodigoPrecio").val());
                var codigo = $.trim($("#hdCodigo").val());

                var longitud = $('#dlgComision #dgComision').datagrid('getRows').length;
                if (longitud > 0) {
                    for (var indice = 0 ; indice < longitud; indice++) {
                        $('#dlgComision #dgComision').datagrid('selectRow', indice);
                        var registro = $('#dlgComision #dgComision').datagrid('getSelected');

                        dataGrilla += (registro.codigo_regla ? registro.codigo_regla : '') + ',' + registro.codigo_canal + ',' + registro.codigo_tipo_pago + ',' + registro.codigo_tipo_comision + ',' + registro.valor + '|';
                    }
                    dataGrilla = dataGrilla.substring(0, dataGrilla.length - 1);
                }

                if ($('#hdComisionEliminar').val().length > 0) {
                    comisionesEliminadas = $('#hdComisionEliminar').val();
                    comisionesEliminadas = comisionesEliminadas.substring(0, comisionesEliminadas.length - 1);
                }

                $.messager.confirm('Confirm', 'Seguro que desea registrar?', function (result) {
                    if (result) {
                        var mapData = { codigoPrecio: codigoPrecio, dataGrilla: dataGrilla, comisionesEliminadas: comisionesEliminadas };

                        $.ajax({
                            type: 'post',
                            url: project.ActionUrls.RegistrarReglasComision,
                            data: mapData,
                            dataType: 'json',
                            cache: false,
                            async: false,
                            success: function (data) {
                                if (data.Msg) {
                                    if (data.Msg != 'Success') {
                                        project.AlertErrorMessage('Error', data.Msg);
                                    }
                                    else {
                                        $('#frmComision').form('clear');
                                        current.LimpiarGrillaEditable($('#dlgComision #dgComision'));
                                        $('#hdComisionEliminar').val('');
                                        $('#dlgComision').dialog('close');
                                        project.ShowMessage('Alerta', 'Registro Exitoso');
                                        current.GetPrecios(codigo);
                                    }
                                }
                                else {
                                    project.AlertErrorMessage('Error', 'Error de procesamiento');
                                }
                            },
                            error: function () {
                                project.AlertErrorMessage('Error', 'Error');
                            }
                        });
                    }
                });


            },

            GetSingle: function (codigo) {
                $.ajax({
                    type: 'post',
                    url: project.ActionUrls.GetSingleJson,
                    data: { codigo: codigo },
                    async: false,
                    cache: false,
                    dataType: 'json',
                    success: function (data) {
                        if (data.Msg) {
                            project.AlertErrorMessage('Error', data.Msg);
                        }
                        else {
                            Registro =
                            {
                                codigo_articulo: data.codigo_articulo,
                                nombre: data.nombre,
                                abreviatura: data.abreviatura,
                                genera_comision: data.genera_comision == 'True' ? true : false,
                                genera_bono: data.genera_bono == 'True' ? true : false,
                                estado: data.estado == 'True',
                                fecha_registro: data.fecha_registro,
                                fecha_modifica: data.fecha_modifica,
                                usuario_registro: data.usuario_registro,
                                usuario_modifica: data.usuario_modifica,
                            };
                        }
                    },
                    error: function () {
                        project.AlertErrorMessage('Error', 'Error');
                    }
                });
            },

			GetAll: function (nombre) {
                $.ajax({
                    type: 'post',
                    url: project.ActionUrls.GetAllJson,
                    data: { nombre: nombre },
                    async: false,
                    cache: false,
                    dataType: 'json',
                    success: function (data) {
                        if (data.Msg) {
                            project.AlertErrorMessage('Error', data.Msg);
                        }
                        else {
                            $('#DataGrid').datagrid('loadData', data);
                        }
                    },
                    error: function () {
                        project.AlertErrorMessage('Error', 'Error');
                    }
                });
            },
			
			GetPrecios: function (codigoArticulo){
			    $.ajax({
			        type: 'post',
			        url: project.ActionUrls.GetPreciobyArticuloJson,
			        data: { codigoArticulo: codigoArticulo },
			        async: false,
			        cache: false,
			        dataType: 'json',
			        success: function (data) {
			            if (data.Msg) {
			                project.AlertErrorMessage('Error', data.Msg);
			            }
			            else {
			                $('#dlgModificar #dgPrecioModificar').datagrid('loadData', data);
			            }
			        },
			        error: function () {
			            project.AlertErrorMessage('Error', 'Error');
			        }
			    });

			},

            CargarCombos: function () {

			},

            ValidarSeleccion: function (codigo) {
                if (!codigo) {
                    $.messager.alert('Error', 'Por favor seleccione un registro', 'error');
                    return false;
                }
                return true;
            },

            ObtenerRegistro: function (codigo) {
                current.GetSingle(codigo);

                if (!Registro.codigo_articulo) {
                    $.messager.alert('Error', 'Error al cargar los datos del registro', 'error');
                    $('#DataGrid').datagrid('reload');
                    return false;
                }
                return true;
            },

            LimpiarGrilla: function () {
                $('#DataGrid').datagrid('clearSelections');
                $('#DataGrid').datagrid('reload');
                $("#hdCodigo").val("");
                $("#hdNombre").val("");
                current.EditType = current.EditTypes.Ninguno;
            },

            LimpiarGrillaEditable: function (grillaEditable) {
            
                var longitud = grillaEditable.datagrid('getRows').length;
                if (longitud > 0){
                    for (var indice = longitud -1 ; indice >= 0; indice--) {
                        grillaEditable.datagrid('deleteRow', indice);
                    }
                }
            },

            Redimensionar: function () {
                var mediaquery = window.matchMedia("(max-width: 600px)");
                var ancho = '60%';

                if (mediaquery.matches) {
                    ancho = '95%';
                }

                $('#dlgRegistrar').dialog('resize', { width: ancho });
                $('#dlgModificar').dialog('resize', { width: ancho });
                $('#dlgVisualizar').dialog('resize', { width: ancho });

                $('#dlgRegistrar').window('center');
                $('#dlgModificar').window('center');
                $('#dlgVisualizar').window('center');
            },

        });

})
(project);