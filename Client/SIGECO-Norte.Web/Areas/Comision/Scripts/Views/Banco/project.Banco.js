;
(function (app) {
    //===========================================================================================
    var current = app.Banco = {};
    //===========================================================================================

    jQuery.extend(app.Banco,
        {

            ActionUrls: {},
            EditTreeNode: {},
            EditType: '',
            HasRootNode: 'False',
            
            Initialize: function (actionUrls) {
                jQuery.extend(project.ActionUrls, actionUrls);
                current.Initialize_DataGrid();

                /*PARA REDIMENSIONAR LAS INTERFACES*/
                var mediaquery = window.matchMedia("(max-width: 600px)");
                if (mediaquery.matches) {
                    $('#dlgRegistrar').dialog('resize', { width: '95%' });
                    $('#dlgRegistrar').window('center');

                    $('#dlgModificar').dialog('resize', { width: '95%' });
                    $('#dlgModificar').window('center');

                    $('#dlgVisualizar').dialog('resize', { width: '95%' });
                    $('#dlgVisualizar').window('center');
                } else {
                    $('#dlgRegistrar').dialog('resize', { width: '60%' });
                    $('#dlgRegistrar').window('center');

                    $('#dlgModificar').dialog('resize', { width: '60%' });
                    $('#dlgModificar').window('center');

                    $('#dlgVisualizar').dialog('resize', { width: '60%' });
                    $('#dlgVisualizar').window('center');
                }

                $(window).resize(function () {

                    var mediaquery = window.matchMedia("(max-width: 600px)");
                    if (mediaquery.matches) {
                        $('#dlgRegistrar').dialog('resize', { width: '95%' });
                        $('#dlgModificar').dialog('resize', { width: '95%' });
                        $('#dlgVisualizar').dialog('resize', { width: '95%' });

                    } else {
                        $('#dlgRegistrar').dialog('resize', { width: '60%' });
                        $('#dlgModificar').dialog('resize', { width: '60%' });
                        $('#dlgVisualizar').dialog('resize', { width: '60%' });
                    }

                    $('#dlgRegistrar').window('center');
                    $('#dlgModificar').window('center');
                    $('#dlgVisualizar').window('center');
                });
                //FIN REDIMENSIONAMIENTO

                $('#btnNuevo').click(function () {
                    current.EditType = 'Registrar';
                    $('#dlgRegistrar').dialog('open').dialog('setTitle', 'NUEVO REGISTRO');
                    $('#frmRegistrar').form('clear');
                });

                $('#btnModificar').click(function () {

                    var codigo = $("#hdCodigo").val();
                    current.GetRegistro(codigo);

                    if (!Registro.codigo) {
                        $.messager.alert('Error', 'Error al cargar los datos del registro', 'error', function () {
                            $('#DataGrid').treegrid('reload');
                        });
                    }
                    else {
                        current.EditType = 'Modificar';
                        $('#dlgModificar').dialog('open').dialog('setTitle', 'MODIFICAR REGISTRO');
                        $('#frmModificar').form('clear');

                        //$('#dlgModificar #codigo').textbox('setText', Registro.codigo);
                        $('#dlgModificar #nombre').textbox('setText', Registro.nombre);
                    }
                });

                $('#btnVisualizar').click(function () {

                    var codigo = $("#hdCodigo").val();
                    current.GetRegistro(codigo);

                    if (!Registro.codigo) {
                        $.messager.alert('Error', 'Error al cargar los datos del registro', 'error', function () {
                            $('#DataGrid').treegrid('reload');
                        });
                    }
                    else {
                        current.EditType = 'Visualizar';
                        $('#dlgVisualizar').dialog('open').dialog('setTitle', 'DETALLE REGISTRO');

                        //$('#dlgVisualizar #codigo').textbox('setText', Registro.codigo);
                        $('#dlgVisualizar #nombre').textbox('setText', Registro.nombre);
                        $('#dlgVisualizar #fechaRegistra').textbox('setText', Registro.fecha_registra);
                        $('#dlgVisualizar #fechaModifica').textbox('setText', Registro.fecha_modifica);
                        $('#dlgVisualizar #usuarioRegistra').textbox('setText', Registro.usuario_registra);
                        $('#dlgVisualizar #usuarioModifica').textbox('setText', Registro.usuario_modifica);
                    }
                });


                $('#btnEliminar').click(function () {
                    var codigo = $("#hdCodigo").val();
                    if (codigo != null) {
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
                                                project.ShowMessage('Alerta', 'Eliminacion exitosa');
                                                $('#DataGrid').datagrid('reload');
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
                });

                $('#btnRefrescar').click(function () {
                    $('#DataGrid').datagrid('clearSelections');
                    $('#DataGrid').datagrid('reload');
                    $("#hdCodigo").val("");
                });

                $('#dlgModificar #btnGuardar').click(function () {
                    current.Guardar();
                });

                $('#dlgModificar #btnCancelar').click(function () {
                    $('#frmModificar').form('clear');
                    $('#dlgModificar').dialog('close');
                });

                $('#dlgRegistrar #btnGuardar').click(function () {
                    current.Guardar();
                });

                $('#dlgRegistrar #btnCancelar').click(function () {
                    $('#frmRegistrar').form('clear');
                    $('#dlgRegistrar').dialog('close');
                });

            },

            Initialize_DataGrid: function() {

                $('#DataGrid').datagrid({
                    url: project.ActionUrls.GetRegistrosJSON,
                    //shrinkToFit: false,
                    fitColumns: true,
                    idField: 'codigo_banco',
                    data: null,
                    pagination: true,
                    singleSelect: true,
                    pageNumber: 0,
                    pageList: [5,10,15,20,25,30],
                    pageSize: 10,
                    columns:
                    [[
                        { field: 'codigo_banco', title: 'Codigo', width: 30, align: 'left' },
                        { field: 'nombre', title: 'Nombre', width: 100, align: 'left' },
                    ]],
                    onClickRow: function (index, row) {
                        var rowColumn = row['codigo_banco'];
                        $("#hdCodigo").val(rowColumn);
                    }
                });

                $('#DataGrid').datagrid('enableFilter');
                $('#DataGrid').datagrid('loadData', {});
            },


            Guardar: function () {

                if (current.EditType == 'Modificar') {
                    current.Modificar();
                }else if (current.EditType == 'Registrar') {
                    current.Registrar();
                }
            },


            GetRegistro: function (nodeId) {

                $.ajax({
                    type: 'post',
                    url: project.ActionUrls.GetRegistro,
                    data: { id: nodeId },
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
                                codigo: data.codigo_banco,
                                nombre: data.nombre,
                                estado_registro: data.estado_registro == 'True',
                                fecha_registra: data.fecha_registra,
                                fecha_modifica: data.fecha_modifica,
                                usuario_registra: data.usuario_registra,
                                usuario_modifica: data.usuario_modifica,
                            };
                        }
                    },
                    error: function () {
                        project.AlertErrorMessage('Error', 'Error');
                    }
                });
            },


            Modificar: function () {

                var message = '';

                var codigo = $("#hdCodigo").val();
                var nombre = $.trim($('#dlgModificar #nombre').textbox('getText'));

                if (message.length > 0) {
                    $.messager.alert('Error', message, 'error');
                }
                else {
                    $.messager.confirm('Confirm', 'Seguro que desea modificar?', function (result) {
                        if (result) {
                            var mapData = { codigo: codigo, nombre: nombre };

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
                                            $('#dlgModificar').dialog('close');

                                            project.ShowMessage('Alerta', 'Modificacion Exitosa');

                                            $('#dlgModificar #codigo').val('');
                                            $('#dlgModificar #nombre').val('');

                                            $('#DataGrid').datagrid('clearSelections');
                                            $('#DataGrid').datagrid('reload');

                                            current.EditType = '';
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


            Registrar: function () {

                var message = '';

                var nombre = $.trim($('#dlgRegistrar #nombre').textbox('getText'));

                if (message.length > 0) {
                    $.messager.alert('Error', message, 'error');
                }
                else {
                    $.messager.confirm('Confirm', 'Seguro que desea registrar?', function (result) {
                        if (result) {
                            var mapData = { nombre: nombre };

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
                                            $('#dlgRegistrar').dialog('close');

                                            project.ShowMessage('Alerta', 'Registro Exitoso');

                                            $('#dlgRegistrar #nombre').val('');

                                            $('#DataGrid').datagrid('reload');

                                            current.EditType = '';
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


        
        });

})
(project);