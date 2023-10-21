;
(function (app) {
    //===========================================================================================
    var current = app.PerfilUsuario = {};
    //===========================================================================================

    jQuery.extend(app.PerfilUsuario,
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
                    current.GetMenuJSON();
                    current.GetListaTipoAccesoItemJSON();
                });

                $('#btnModificar').click(function () {
                    var codigo = $("#hdCodigo").val();
                    current.GetRegistro(codigo);

                    if (!Registro.codigo) {
                        $.messager.alert('錯誤', 'Error al cargar los datos del Perfil', 'error', function () {
                            $('#DataGrid').treegrid('reload');
                        });
                    }
                    else {
                        current.EditType = 'Modificar';
                        $('#dlgModificar').dialog('open').dialog('setTitle', 'MODIFICAR REGISTRO');

                        $('#dlgModificar #codigo').textbox('setText', Registro.codigo);
                        $('#dlgModificar #nombre').textbox('setText', Registro.nombre);

                        current.GetMenuByPerfilJSON(Registro.codigo, false);
                        current.GetListaTipoAccesoItemBYPerfilJSON(Registro.codigo, false);
                    }
                });

                $('#btnVisualizar').click(function () {

                    var codigo = $("#hdCodigo").val();
                    current.GetRegistro(codigo);

                    if (!Registro.codigo) {
                        $.messager.alert('Error', 'Error al cargar los datos del Perfil', 'error', function () {
                            $('#DataGrid').treegrid('reload');
                        });
                    }
                    else {
                        current.EditType = 'Visualizar';
                        $('#dlgVisualizar').dialog('open').dialog('setTitle', 'Detalle Registro');

                        $('#dlgVisualizar #codigo').textbox('setText', Registro.codigo);
                        $('#dlgVisualizar #nombre').textbox('setText', Registro.nombre);
                        $('#dlgVisualizar #fechaRegistra').textbox('setText', Registro.fecha_registra);
                        $('#dlgVisualizar #fechaModifica').textbox('setText', Registro.fecha_modifica);
                        $('#dlgVisualizar #usuarioRegistra').textbox('setText', Registro.usuario_registra);
                        $('#dlgVisualizar #usuarioModifica').textbox('setText', Registro.usuario_modifica);
                        current.GetMenuByPerfilJSON(Registro.codigo, true);
                        current.GetListaTipoAccesoItemBYPerfilJSON(Registro.codigo, true);
                    }
                });


                $('#btnEliminar').click(function () {
                    var codigo = $("#hdCodigo").val();

                    if (codigo != null) {
                        $.messager.confirm('Confirm', 'Seguro que desea eliminar este registro?', function (result) {
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
                    idField: 'codigo_perfil_usuario',
                    data: null,
                    pagination: true,
                    singleSelect: true,
                    pageNumber: 0,
                    pageList: [5,10,15,20,25,30],
                    pageSize: 20,
                    columns:
                    [[
                        { field: 'codigo_perfil_usuario', title: 'Codigo', width: 30, align: 'left' },
                        { field: 'nombre_perfil_usuario', title: 'Nombre', width: 100, align: 'left' }
                    ]],
                    onClickRow: function (index, row) {
                        var rowColumn = row['codigo_perfil_usuario'];
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
                                codigo: data.codigo_perfil_usuario,
                                nombre: data.nombre_perfil_usuario,
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
                var codigo = $.trim($('#dlgModificar #codigo').textbox('getText'));
                var nombre = $.trim($('#dlgModificar #nombre').textbox('getText'));

                var nodeTree = $('#dlgModificar #treeMenu').tree('getChecked');
                var chilenodesTree = '';
                for (var i = 0; i < nodeTree.length; i++) {
                    if ($('#dlgModificar #treeMenu').tree('isLeaf', nodeTree[i].target)) {
                        chilenodesTree += nodeTree[i].id + ',';
                    }
                }
                chilenodesTree = chilenodesTree.substring(0, chilenodesTree.length - 1);

                var nodeGrid = $('#dlgModificar #dataGridTipoAccesoItem').datagrid('getChecked');
                var chilenodesGrid = '';
                for (var i = 0; i < nodeGrid.length; i++) {
                    chilenodesGrid += nodeGrid[i].codigo_tipo_acceso_item + ',';
                }
                chilenodesGrid = chilenodesGrid.substring(0, chilenodesGrid.length - 1);


                if (message.length > 0) {
                    $.messager.alert('Error', message, 'error');
                }
                else {
                    $.messager.confirm('Confirm', 'Seguro que desea modificar?', function (result) {
                        if (result) {
                            var mapData = { codigo: codigo, nombre: nombre, menu: chilenodesTree, tipoAcceso: chilenodesGrid };

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
                
                var nodeTree = $('#dlgRegistrar #treeMenu').tree('getChecked');
                var chilenodesTree = '';
                for (var i = 0; i < nodeTree.length; i++) {
                    if ($('#dlgRegistrar #treeMenu').tree('isLeaf', nodeTree[i].target)) {
                        chilenodesTree += nodeTree[i].id + ',';
                    }
                }
                chilenodesTree = chilenodesTree.substring(0, chilenodesTree.length - 1);

                var nodeGrid = $('#dlgRegistrar #dataGridTipoAccesoItem').datagrid('getChecked');
                var chilenodesGrid = '';
                for (var i = 0; i < nodeGrid.length; i++) {
                    chilenodesGrid += nodeGrid[i].codigo_tipo_acceso_item + ',';
                }
                chilenodesGrid = chilenodesGrid.substring(0, chilenodesGrid.length - 1);

                var nombre = $.trim($('#dlgRegistrar #nombre').textbox('getText'));

                if (message.length > 0) {
                    $.messager.alert('Error', message, 'error');
                }
                else {
                    $.messager.confirm('Confirm', 'Seguro que desea registrar?', function (result) {
                        if (result) {
                            var mapData = { nombre: nombre, menu: chilenodesTree, tipoAcceso: chilenodesGrid };

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



            GetMenuJSON: function () {

                $('#dlgRegistrar #treeMenu').tree({
                    url: project.ActionUrls.GetMenuJSON,
                    checkbox: true
                });

            },



            GetListaTipoAccesoItemJSON: function () {

                $('#dlgRegistrar #dataGridTipoAccesoItem').datagrid({
                    url: project.ActionUrls.GetListaTipoAccesoItemJSON,
                    //shrinkToFit: false,
                    fitColumns: true,
                    idField: 'codigo_tipo_acceso_item',
                    data: null,
                    singleSelect: false,
                    columns:
                    [[
                        { field: 'id', checkbox: true },
                        { field: 'codigo_tipo_acceso_item', title: 'Codigo', width: 30, align: 'left' },
                        { field: 'nombre_tipo_acceso_item', title: 'Nombre', width: 100, align: 'left' }
                    ]],
                    onLoadSuccess: function (data) {
                        $('#dlgRegistrar #dataGridTipoAccesoItem').datagrid('clearSelections');
                    }
                });
            },



            GetListaTipoAccesoItemBYPerfilJSON: function (codigoPerfil, visualizar) {

                $.ajax({
                    type: 'post',
                    url: project.ActionUrls.GetListaTipoAccesoItemBYPerfilJSON,
                    data: { codigoPerfil: codigoPerfil },
                    async: false,
                    cache: false,
                    dataType: 'json',
                    success: function (data) {
                        if (visualizar) {
                            $('#dlgVisualizar #dataGridTipoAccesoItem').datagrid({
                                data: data,
                                //shrinkToFit: false,
                                fitColumns: true,
                                idField: 'codigo_tipo_acceso_item',
                                singleSelect: false,
                                columns:
                                [[
                                    {
                                        field: 'checked', formatter: function (value, row, index) {
                                            if (row.registrado == 'true') {
                                                return '<input type="checkbox" name="chkrow" checked="checked" disabled >';
                                            }
                                            else {
                                                return '<input type="checkbox" name="chkrow" disabled >';
                                            }
                                        }
                                    },
                                    { field: 'codigo_tipo_acceso_item', title: 'Codigo', width: 30, align: 'left' },
                                    { field: 'nombre_tipo_acceso_item', title: 'Nombre', width: 100, align: 'left' },
                                ]],
                                rowStyler: function (index, row) {
                                    return 'background-color: transparent;';
                                }
                            });
                        } else {

                            $('#dlgModificar #dataGridTipoAccesoItem').datagrid({
                                data: data,
                                //shrinkToFit: false,
                                fitColumns: true,
                                idField: 'codigo_tipo_acceso_item',
                                singleSelect: false,
                                columns:
                                [[
                                    { field: 'ck', checkbox: true },
                                    { field: 'codigo_tipo_acceso_item', title: 'Codigo', width: 30, align: 'left' },
                                    { field: 'nombre_tipo_acceso_item', title: 'Nombre', width: 100, align: 'left' }
                                ]],
                                onLoadSuccess: function (data) {
                                    $('#dlgModificar #dataGridTipoAccesoItem').datagrid('clearSelections');
                                    for (i = 0; i < data.rows.length; ++i) {
                                        if (data.rows[i]['registrado'] == 'true') $(this).datagrid('checkRow', i);
                                    }
                                }
                            });
                        }
                    },
                    error: function () {
                        project.AlertErrorMessage('Error', 'Error');
                    }
                });



                $('#dlgRegistrar #dataGridTipoAccesoItem').datagrid({
                    url: project.ActionUrls.GetListaTipoAccesoItemBYPerfilJSON,
                    //shrinkToFit: false,
                    fitColumns: true,
                    idField: 'codigo_tipo_acceso_item',
                    data: null,
                    singleSelect: false,
                    columns:
                    [[
                        { field: 'id', checkbox: true },
                        { field: 'codigo_tipo_acceso_item', title: 'Codigo', width: 30, align: 'left' },
                        { field: 'nombre_tipo_acceso_item', title: 'Nombre', width: 100, align: 'left' }
                    ]]
                });
            },



            GetMenuByPerfilJSON: function (codigoPerfil, visualizar) {

                $.ajax({
                    type: 'post',
                    url: project.ActionUrls.GetMenuByPerfilJSON,
                    data: { codigoPerfil: codigoPerfil },
                    async: false,
                    cache: false,
                    dataType: 'json',
                    success: function (data) {
                        if (visualizar) {
                            $('#dlgVisualizar #treeMenu').tree({
                                data: data,
                                checkbox: true,
                                onBeforeCheck: function (node, checked) {
                                    return false;
                                }
                            });
                        } else {
                            $('#dlgModificar #treeMenu').tree({
                                data: data,
                                checkbox: true
                            });
                        }
                    },
                    error: function () {
                        project.AlertErrorMessage('Error', 'Error');
                    }
                });
            },


        });

})
(project);