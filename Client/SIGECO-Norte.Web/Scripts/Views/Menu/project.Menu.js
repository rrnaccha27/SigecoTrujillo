;
(function (app) {
    //===========================================================================================
    var current = app.Menu = {};
    //===========================================================================================

    jQuery.extend(app.Menu,
        {

            ActionUrls: {},
            EditTreeNode: {},
            EditType: '',

            Initialize: function (actionUrls) {

                jQuery.extend(project.ActionUrls, actionUrls);
    
                current.Initialize_TreeGrid();

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
                    current.Initilaize_TreeNodeDLL();
                    $('#dlgRegistrar').dialog('open').dialog('setTitle', 'NUEVO REGISTRO');
                    $('#frmRegistrar').form('clear');
                    $('#dlgRegistrar #cboMenu')[0].selectedIndex = 0;
                    $('#dlgRegistrar #orden').textbox('setText', 1);
                });


                $('#btnModificar').click(function () {
                    current.Initilaize_TreeNodeDLL();
                    var codigo = $("#hdCodigo").val();

                    current.GetRegistro(codigo);

                    if (!Registro.codigo) {
                        $.messager.alert('Error', 'Error al cargar los datos', 'error', function () {
                            $('#TreeGrid').treegrid('reload');
                        });
                    }
                    else {
                        current.EditType = 'Modificar';
                        $('#dlgModificar').dialog('open').dialog('setTitle', 'MODIFICAR REGISTRO');

                        $('#dlgModificar #codigo').textbox('setText', Registro.codigo);
                        $('#dlgModificar #nombre').textbox('setText', Registro.nombre);
                        $('#dlgModificar #cboMenu').combotree('setValue', Registro.codigo_menu_padre);
                        $('#dlgModificar #estadoRegistro').prop('checked', Registro.estado_registro);
                        $('#dlgModificar #orden').textbox('setText', Registro.orden);
                        $('#dlgModificar #ruta').textbox('setText', Registro.ruta);
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
                                    data: { codigoMenu: codigo },
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
                                                $('#TreeGrid').treegrid('reload');
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


                $('#btnVisualizar').click(function () {

                    var codigo = $("#hdCodigo").val();
                    current.GetRegistro(codigo);

                    if (!Registro.codigo) {
                        $.messager.alert('Error', 'Error al cargar los datos', 'error', function () {
                            $('#DataGrid').treegrid('reload');
                        });
                    }
                    else {
                        current.EditType = 'Visualizar';
                        $('#dlgVisualizar').dialog('open').dialog('setTitle', 'Detalle Registro');

                        $('#dlgVisualizar #codigo').textbox('setText', Registro.codigo);
                        $('#dlgVisualizar #nombre').textbox('setText', Registro.nombre);
                        $('#dlgVisualizar #nombre').textbox('setText', Registro.nombre);
                        $('#dlgVisualizar #estadoRegistro').prop('checked', Registro.estado_registro);
                        $('#dlgVisualizar #orden').textbox('setText', Registro.orden);
                        $('#dlgVisualizar #ruta').textbox('setText', Registro.ruta);
                        $('#dlgVisualizar #fechaRegistra').textbox('setText', Registro.fecha_registra);
                        $('#dlgVisualizar #fechaModifica').textbox('setText', Registro.fecha_modifica);
                        $('#dlgVisualizar #usuarioRegistra').textbox('setText', Registro.usuario_registra);
                        $('#dlgVisualizar #usuarioModifica').textbox('setText', Registro.usuario_modifica);
                    }
                });


                $('#btnRefrescar').click(function () {
                    $('#TreeGrid').treegrid('clearSelections');
                    $('#TreeGrid').treegrid('reload');
                    $("#hdCodigo").val("");
                });

                $('#dlgRegistrar #btnGuardar').click(function () {
                    current.Guardar();
                });

                $('#dlgRegistrar #btnCancelar').click(function () {
                    $('#frmRegistrar').form('clear');
                    $('#dlgRegistrar').dialog('close');
                });

                $('#dlgModificar #btnGuardar').click(function () {
                    current.Guardar();
                });

                $('#dlgModificar #btnCancelar').click(function () {
                    $('#frmModificar').form('clear');
                    $('#dlgModificar').dialog('close');
                });
            },

            Initilaize_TreeNodeDLL: function () {
                var cboTreeNodo = project.ActionUrls.LoadTreeNodeDDL;

                $('#dlgRegistrar #cboMenu').combotree({
                    url: cboTreeNodo
                });

                $('#dlgModificar #cboMenu').combotree({
                    url: cboTreeNodo
                });
            },


            Initialize_TreeGrid: function() {

                $('#TreeGrid').treegrid({
                    url: project.ActionUrls.GetTreeNodeJSON,
                    //shrinkToFit: false,
                    nowrap: false,
                    fitColumns: true,
                    rownumbers: true,
                    idField: "codigo_menu",
                    treeField: 'nombre_menu',
                    animate: true,
                    collapsible: true,
                    columns:
                    [[
                        { field: 'codigo_menu', title: 'Codigo', width: 30, align: 'left' },
                        { field: 'nombre_menu', title: 'Nombre', width: 150, align: 'left' },
                        { field: 'estado_registro', title: 'Habilitado', width: 50, align: 'left' },
                        { field: 'ruta_menu', title: 'Ruta', width: 150, align: 'left' },
                        { field: 'orden', title: 'Orden', width: 30, align: 'left' }
                    ]],
                    onClickRow: function (row) {
                        $("#hdCodigo").val(row.codigo_menu);
                    },
                });
            },


            Guardar: function () {

                if (current.EditType == 'Modificar') {
                    current.Modificar();
                } else if (current.EditType == 'Registrar') {
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
                                codigo: data.codigo_menu,
                                nombre: data.nombre_menu,
                                codigo_menu_padre: data.codigo_menu_padre,
                                estado_registro: data.estado_registro == 'True',
                                orden: data.orden,
                                ruta: data.ruta_menu,
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


            Registrar: function () {

                var message = '';

                var codigoMenu = $.trim($('#dlgRegistrar #cboMenu').combotree('getValue'));
                var nombre = $.trim($('#dlgRegistrar #nombre').textbox('getText'));
                var orden = $.trim($('#dlgRegistrar #orden').textbox('getText'));
                var ruta = $.trim($('#dlgRegistrar #ruta').textbox('getText'));

                if (message.length > 0) {
                    $.messager.alert('Error', message, 'error');
                }
                else {
                    $.messager.confirm('Confirm', 'Seguro que desea registrar?', function (result) {
                        if (result) {
                            var mapData = { codigoMenu: codigoMenu, nombre: nombre, orden: orden, ruta: ruta };

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

                                            $('#TreeGrid').treegrid('reload');

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


            Modificar: function () {

                var message = '';

                var codigo = $.trim($('#dlgModificar #codigo').textbox('getText'));
                var codigoMenuCabecera = $.trim($('#dlgModificar #cboMenu').combotree('getValue'));
                var nombre = $.trim($('#dlgModificar #nombre').textbox('getText'));
                var orden = $.trim($('#dlgModificar #orden').textbox('getText'));
                var ruta = $.trim($('#dlgModificar #ruta').textbox('getText'));
                var estadoRegistro = $('#dlgModificar #estadoRegistro').is(':checked');

                if (message.length > 0) {
                    $.messager.alert('Error', message, 'error');
                }
                else {
                    $.messager.confirm('Confirm', 'Seguro que desea modificar?', function (result) {
                        if (result) {
                            var mapData = { codigoMenu: codigo, codigoMenuCabecera: codigoMenuCabecera, nombre: nombre, orden: orden, ruta: ruta, estadoRegistro: estadoRegistro };

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

                                            $('#TreeGrid').treegrid('reload');

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