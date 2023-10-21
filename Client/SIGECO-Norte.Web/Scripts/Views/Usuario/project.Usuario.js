;
(function (app) {
    //===========================================================================================
    var current = app.Usuario = {};
    //===========================================================================================

    jQuery.extend(app.Usuario,
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

                    $('#dlgModalPersona').dialog('resize', { width: '95%' });
                    $('#dlgModalPersona').window('center');
                } else {
                    $('#dlgRegistrar').dialog('resize', { width: '60%' });
                    $('#dlgRegistrar').window('center');

                    $('#dlgModificar').dialog('resize', { width: '60%' });
                    $('#dlgModificar').window('center');

                    $('#dlgVisualizar').dialog('resize', { width: '60%' });
                    $('#dlgVisualizar').window('center');

                    $('#dlgModalPersona').dialog('resize', { width: '60%' });
                    $('#dlgModalPersona').window('center');
                }

                $(window).resize(function () {

                    var mediaquery = window.matchMedia("(max-width: 600px)");
                    if (mediaquery.matches) {
                        $('#dlgRegistrar').dialog('resize', { width: '95%' });
                        $('#dlgModificar').dialog('resize', { width: '95%' });
                        $('#dlgVisualizar').dialog('resize', { width: '95%' });
                        $('#dlgModalPersona').dialog('resize', { width: '95%' });
                    } else {
                        $('#dlgRegistrar').dialog('resize', { width: '60%' });
                        $('#dlgModificar').dialog('resize', { width: '60%' });
                        $('#dlgVisualizar').dialog('resize', { width: '60%' });
                        $('#dlgModalPersona').dialog('resize', { width: '60%' });
                    }

                    $('#dlgRegistrar').window('center');
                    $('#dlgModificar').window('center');
                    $('#dlgVisualizar').window('center');
                    $('#dlgModalPersona').window('center');
                });
                //FIN REDIMENSIONAMIENTO

                $('#btnNuevo').click(function () {
                    current.EditType = 'Registrar';
                    $('#dlgRegistrar').dialog('open').dialog('setTitle', 'NUEVO REGISTRO');
                    $('#frmRegistrar').form('clear');

                    current.CargarCombos(true);
                    current.GetDataEmpresa('','1');

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

                        current.GetDataEmpresa(codigo, '2');
                        current.CargarCombos(false);
                        
                        $('#dlgModificar #codigo').textbox('setText', Registro.codigo);
                        
                        $('#dlgModificar #clave').textbox('setValue', Registro.clave);

                        $('#dlgModificar #cboPerfilUsuario').combobox('setValue', Registro.codigo_perfil_usuario);
                        $('#dlgModificar #persona').textbox('setText', Registro.persona);
                        $('#dlgModificar #estadoRegistro').textbox('setText', Registro.estado_registro);
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

                        current.GetDataEmpresa(codigo, '3');

                        $('#dlgVisualizar #codigo').textbox('setText', Registro.codigo);
                        $('#dlgVisualizar #perfilUsuario').textbox('setText', Registro.nombre_perfil_usuario);
                        $('#dlgVisualizar #persona').textbox('setText', Registro.persona);
                        $('#dlgVisualizar #estadoRegistro').textbox('setText', Registro.estado_registro);
                        $('#dlgVisualizar #fechaRegistra').textbox('setText', Registro.fecha_registra);
                        $('#dlgVisualizar #fechaModifica').textbox('setText', Registro.fecha_modifica);
                        $('#dlgVisualizar #usuarioRegistra').textbox('setText', Registro.usuario_registra);
                        $('#dlgVisualizar #usuarioModifica').textbox('setText', Registro.usuario_modifica);
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

                $('#btnResetearClave').click(function () {
                    var codigo = $("#hdCodigo").val();
                    if (codigo != null) {
                        $.messager.confirm('Confirm', 'Seguro que desea restaurar la clave del usuario?', function (result) {
                            if (result) {
                                $.ajax({
                                    type: 'post',
                                    url: project.ActionUrls.ResetearClave,
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
                                                project.ShowMessage('Error', 'Clave restaurada satisfactoriamente');
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

                $('#dlgModalPersonaBotones #btnCancelar').click(function () {
                    $('#dlgRegistrar #persona').textbox('setText', '');
                    $("#hdCodigoPersona").val('');

                    $('#frmModalPersona').form('clear');
                    $('#dlgModalPersona').dialog('close');
                    $('#dlgModalPersona #dataGridPersona').datagrid('clearSelections');
                });

                $('#dlgModalPersonaBotones #btnGuardar').click(function () {
                    $('#frmModalPersona').form('clear');
                    $('#dlgModalPersona').dialog('close');
                    $('#dlgModalPersona #dataGridPersona').datagrid('clearSelections');
                });

                $('#dlgRegistrar #btnBuscarPersona').click(function () {
                    $('#dlgModalPersona').dialog('open').dialog('setTitle', 'BUSCAR PERSONA');
                    $('#frmModalPersona').form('clear');
                });

                $('#dlgModalPersona #valor').textbox({
                    onClickButton: function () {
                        var tipo = $.trim($('#dlgModalPersona #cboFiltro').combobox('getValue'));
                        var valor = $.trim($('#dlgModalPersona #valor').textbox('getText'));

                        current.GetAllPersonaByFiltroJson(tipo, valor);
                        
                    }
                });
            },

            Initialize_DataGrid: function() {

                $('#DataGrid').datagrid({
                    url: project.ActionUrls.GetRegistrosJSON,
                    //shrinkToFit: false,
                    fitColumns: true,
                    idField: 'codigo_usuario',
                    data: null,
                    pagination: true,
                    singleSelect: true,
                    pageNumber: 0,
                    pageList: [5,10,15,20,25,30],
                    pageSize: 20,
                    columns:
                    [[
                        { field: 'codigo_usuario', title: 'Codigo', width: 50, align: 'left' },
                        { field: 'persona', title: 'Nombre Persona', width: 100, align: 'left' },
                        { field: 'nombre_perfil_usuario', title: 'Perfil Usuario', width: 70, align: 'left' },
                        {
                            field: 'estado_registro', title: 'Estado', width: 50, align: 'left',
                            formatter: function (value, row, index) {
                                if (row.estado_registro == 'I') { return 'Inactivo'; } else { return 'Activo'; }
                            },
                        }
                    ]],
                    onClickRow: function (index, row) {
                        var rowColumn = row['codigo_usuario'];
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


            CargarCombos: function (registrar) {
                if (registrar) {
                    $('#dlgRegistrar #cboPerfilUsuario').combobox({
                        valueField: 'id',
                        textField: 'text',
                        url: project.ActionUrls.GetPerfilUsuarioJSON
                    });
                } else {
                    $('#dlgModificar #cboPerfilUsuario').combobox({
                        valueField: 'id',
                        textField: 'text',
                        url: project.ActionUrls.GetPerfilUsuarioJSON
                    });
                }
                
            },

            
            GetDataEmpresa: function (codigoUsuario, tipo) {

                $.ajax({
                    type: 'post',
                    url: project.ActionUrls.GetEmpresaJSON,
                    data: { codigoUsuario: codigoUsuario },
                    async: false,
                    cache: false,
                    dataType: 'json',
                    success: function (data) {
                        if (data.Msg) {
                            project.AlertErrorMessage('Error', data.Msg);
                        }
                        else {

                            if (tipo == '1') {
                                var dg = $('#dlgRegistrar #DataGridEmpresa');
                                dg.datagrid({
                                    data: data,
                                    shrinkToFit: false,
                                    fitColumns: true,
                                    idField: 'codigo_empresa',
                                    singleSelect: false,
                                    columns:
                                    [[
                                        { field: 'id', checkbox: true },
                                        { field: 'codigo_empresa', title: 'Codigo', width: 30, align: 'left' },
                                        { field: 'nombre_empresa', title: 'Empresa', width: 50, align: 'left' }
                                    ]],
                                    onLoadSuccess: function (data) {
                                        $(this).datagrid('clearSelections');
                                    }
                                });
                            } else if (tipo == '2') {
                                var dg = $('#dlgModificar #DataGridEmpresa');
                                dg.datagrid({
                                    data: data,
                                    shrinkToFit: false,
                                    fitColumns: true,
                                    idField: 'codigo_empresa',
                                    singleSelect: false,
                                    columns:
                                    [[
                                        { field: 'id', checkbox: true },
                                        { field: 'codigo_empresa', title: 'Codigo', width: 30, align: 'left' },
                                        { field: 'nombre_empresa', title: 'Empresa', width: 50, align: 'left' }
                                    ]],
                                    onLoadSuccess: function (data) {
                                        $(this).datagrid('clearSelections');

                                        for (i = 0; i < data.rows.length; ++i) {
                                            if (data.rows[i]['registrado'] == 'true') $(this).datagrid('checkRow', i);
                                        }
                                    }
                                });
                            } else {

                                var dg = $('#dlgVisualizar #DataGridEmpresa');
                                dg.datagrid({
                                    data: data,
                                    shrinkToFit: false,
                                    fitColumns: true,
                                    idField: 'codigo_empresa',
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
                                        { field: 'codigo_empresa', title: 'Codigo', width: 30, align: 'left' },
                                        { field: 'nombre_empresa', title: 'Empresa', width: 50, align: 'left' }
                                    ]]
                                });
                            }

                        }
                    },
                    error: function () {
                        project.AlertErrorMessage('Error', 'Error');
                    }
                });
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
                        console.log(data);
                        if (data.Msg) {
                            project.AlertErrorMessage('Error', data.Msg);
                        }
                        else {
                            Registro =
                            {
                                codigo: data.codigo_usuario,
                                codigo_perfil_usuario: data.codigo_perfil_usuario,
                                nombre_perfil_usuario: data.nombre_perfil_usuario,
                                persona: data.persona,
                                clave: data.clave_usuario,
                                estado_registro: data.estado_registro,
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

                if (!$("#frmModificar").form('enableValidation').form('validate'))
                    return;

                var message = '';

                var codigo = $.trim($('#dlgModificar #codigo').textbox('getText'));
                var codigoPerfilUsuario = $.trim($('#dlgModificar #cboPerfilUsuario').combobox('getValue'));
                var clave = $.trim($('#dlgModificar #clave').val());
                var empresaChecked = $('#dlgModificar #DataGridEmpresa').datagrid('getChecked');
                var listaCodigoEmpresa = '';

                for (var i = 0; i < empresaChecked.length; i++) {
                    listaCodigoEmpresa += empresaChecked[i].codigo_empresa + ',';
                }
                listaCodigoEmpresa = listaCodigoEmpresa.substring(0, listaCodigoEmpresa.length - 1);

                if (message.length > 0) {
                    $.messager.alert('Error', message, 'error');
                }
                else {
                    $.messager.confirm('Confirm', 'Seguro que desea modificar?', function (result) {
                        if (result) {
                            var mapData = { codigo: codigo,clave:clave, codigoPerfilUsuario: codigoPerfilUsuario, listaCodigoEmpresa: listaCodigoEmpresa };

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
                                            $('#dlgModificar #persona').val('');
                                            $('#dlgModificar #clave').val('');
                                            $('#dlgModificar #estadoRegistro').val('');
                                            $('#dlgModificar #estadoRegistro').attr('checked', false);

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
                if (!$("#frmRegistrar").form('enableValidation').form('validate'))
                    return;
                var message = '';
                
                var codigo = $.trim($('#dlgRegistrar #codigo').textbox('getText'));
                var codigoPerfilUsuario = $.trim($('#dlgRegistrar #cboPerfilUsuario').combobox('getValue'));
                var codigoPersona = $("#hdCodigoPersona").val();
                var clave = $.trim($('#dlgRegistrar #clave').val());
                
                
                var empresaChecked = $('#dlgRegistrar #DataGridEmpresa').datagrid('getChecked');
                var listaCodigoEmpresa = '';

                for (var i = 0; i < empresaChecked.length; i++) {
                    listaCodigoEmpresa += empresaChecked[i].codigo_empresa + ',';
                }
                listaCodigoEmpresa = listaCodigoEmpresa.substring(0, listaCodigoEmpresa.length - 1);
                

                if (message.length > 0) {
                    $.messager.alert('Error', message, 'error');
                }
                else {
                    $.messager.confirm('Confirm', 'Seguro que desea registrar?', function (result) {
                        if (result) {
                            var mapData = { codigo: codigo,clave:clave, codigoPerfilUsuario: codigoPerfilUsuario, codigoPersona: codigoPersona, listaCodigoEmpresa: listaCodigoEmpresa };

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

                                            $('#dlgRegistrar #codigo').val('');
                                            $('#dlgRegistrar #cboPerfilUsuario')[0].selectedIndex = 0;

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



            GetAllPersonaByFiltroJson: function (tipo, valor) {

                $.ajax({
                    type: 'post',
                    url: project.ActionUrls.GetAllPersonaByFiltroJson,
                    data: { tipo: tipo, valor: valor },
                    async: false,
                    cache: false,
                    dataType: 'json',
                    success: function (data) {
                        
                        if (data.Msg) {
                            project.AlertErrorMessage('Error', data.Msg);
                        } else {
                            $('#dlgModalPersona #dataGridPersona').datagrid({
                                url: project.ActionUrls.GetAllPersonaByFiltroJson,
                                queryParams: {
                                    tipo: tipo,
                                    valor: valor
                                },
                                //shrinkToFit: false,
                                fitColumns: true,
                                idField: 'codigo_persona',
                                data: null,
                                singleSelect: true,
                                columns:
                                [[
                                    { field: 'codigo_persona', title: 'Codigo', width: 30, align: 'left' },
                                    { field: 'persona', title: 'Nombre Persona', width: 100, align: 'left' },
                                    { field: 'numero_documento', title: 'Nro. Doc', width: 50, align: 'left' }
                                ]],
                                onClickRow: function (index, row) {
                                    var rowColumnCodigo = row['codigo_persona'];
                                    var rowColumnNombre = row['persona'];
                                    $("#hdCodigoPersona").val(rowColumnCodigo);
                                    $('#dlgRegistrar #persona').textbox('setText', rowColumnNombre);

                                }
                            });

                        }
                    },
                    error: function () {
                        project.AlertErrorMessage('Error', 'Error');
                    }
                });

            }

        
        });

})
(project);