;
(function (app) {
    //===========================================================================================
    var current = app.Persona = {};
    //===========================================================================================

    jQuery.extend(app.Persona,
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

                    current.CargarCombos(true);

                    $('#dlgRegistrar').dialog('open').dialog('setTitle', 'NUEVO REGISTRO');
                    $('#frmRegistrar').form('clear');
                    $('#frmRegistrar #cboCorporacion')[0].selectedIndex = 0;
                    $('#frmRegistrar #cboTipoDocumento')[0].selectedIndex = 0;
                    $('#frmRegistrar #cboEstadoCivil')[0].selectedIndex = 0;
                    $('#frmRegistrar #cboSexo')[0].selectedIndex = 0;

                    $('#frmRegistrar #fechaNacimiento').datebox({

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

                        current.CargarCombos(false);

                        $('#dlgModificar').dialog('open').dialog('setTitle', 'MODIFICAR REGISTRO');

                        $('#dlgModificar #codigo').textbox('setText', Registro.codigo);
                        $('#dlgModificar #nombre').textbox('setText', Registro.nombre);
                        $('#dlgModificar #apellidoPaterno').textbox('setText', Registro.apellido_paterno);
                        $('#dlgModificar #apellidoMaterno').textbox('setText', Registro.apellido_materno);
                        $('#dlgModificar #numeroDocumento').textbox('setText', Registro.numero_documento);
                        $('#dlgModificar #fechaNacimiento').textbox('setText', Registro.fecha_nacimiento);
                        $('#dlgModificar #corporacion').textbox('setText', Registro.nombre_corporacion);
                        $('#dlgModificar #cboTipoDocumento').combobox('setValue', Registro.codigo_tipo_documento);
                        $('#dlgModificar #cboEstadoCivil').combobox('setValue', Registro.codigo_estado_civil);
                        $('#dlgModificar #cboSexo').combobox('setValue', Registro.codigo_sexo);
                        $('#dlgModificar #esVendedor').switchbutton({ checked: Registro.es_vendedor });
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

                        $('#dlgVisualizar #codigo').textbox('setText', Registro.codigo);
                        $('#dlgVisualizar #nombre').textbox('setText', Registro.nombre);
                        $('#dlgVisualizar #apellidoPaterno').textbox('setText', Registro.apellido_paterno);
                        $('#dlgVisualizar #apellidoMaterno').textbox('setText', Registro.apellido_materno);
                        $('#dlgVisualizar #numeroDocumento').textbox('setText', Registro.numero_documento);
                        $('#dlgVisualizar #fechaNacimiento').textbox('setText', Registro.fecha_nacimiento);
                        $('#dlgVisualizar #corporacion').textbox('setText', Registro.nombre_corporacion);
                        $('#dlgVisualizar #estadoCivil').textbox('setText', Registro.nombre_estado_civil);
                        $('#dlgVisualizar #tipoDocumento').textbox('setText', Registro.nombre_tipo_documento);
                        $('#dlgVisualizar #sexo').textbox('setText', Registro.nombre_sexo);
                        $('#dlgVisualizar #esVendedor').switchbutton({ checked: Registro.es_vendedor });
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
                                    data: { codigoPersona: codigo },
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

            
            CargarCombos: function (registrar) {

                if (registrar) {
                    $('#dlgRegistrar #cboCorporacion').combobox({
                        valueField: 'id',
                        textField: 'text',
                        url: project.ActionUrls.GetCorporacionJSON
                    });

                    $('#dlgRegistrar #cboTipoDocumento').combobox({
                        valueField: 'id',
                        textField: 'text',
                        url: project.ActionUrls.GetTipoDocumentoJSON
                    });

                    $('#dlgRegistrar #cboEstadoCivil').combobox({
                        valueField: 'id',
                        textField: 'text',
                        url: project.ActionUrls.GetEstadoCivilJSON
                    });

                    $('#dlgRegistrar #cboSexo').combobox({
                        valueField: 'id',
                        textField: 'text',
                        url: project.ActionUrls.GetSexoJSON
                    });

                } else {

                    $('#dlgModificar #cboTipoDocumento').combobox({
                        valueField: 'id',
                        textField: 'text',
                        url: project.ActionUrls.GetTipoDocumentoJSON
                    });

                    $('#dlgModificar #cboEstadoCivil').combobox({
                        valueField: 'id',
                        textField: 'text',
                        url: project.ActionUrls.GetEstadoCivilJSON
                    });

                    $('#dlgModificar #cboSexo').combobox({
                        valueField: 'id',
                        textField: 'text',
                        url: project.ActionUrls.GetSexoJSON
                    });

                }
                
            },

            Initialize_DataGrid: function() {

                $('#DataGrid').datagrid({
                    url: project.ActionUrls.GetRegistrosJSON,
                    //shrinkToFit: false,
                    fitColumns: true,
                    idField: 'codigo_persona',
                    data: null,
                    pagination: true,
                    singleSelect: true,
                    pageNumber: 0,
                    pageList: [5,10,15,20,25,30],
                    pageSize: 15,
                    columns:
                    [[
                        { field: 'codigo_persona', title: 'Codigo', width: 30, align: 'left' },
                        { field: 'nombre_persona', title: 'Nombre', width: 50, align: 'left' },
                        { field: 'apellido_paterno', title: 'Apellido Paterno', width: 50, align: 'left' },
                        { field: 'apellido_materno', title: 'Apellido Materno', width: 50, align: 'left' },
                        { field: 'numero_documento', title: 'Nro. Doc', width: 50, align: 'left' }
                    ]],
                    onClickRow: function (index, row) {
                        var rowColumn = row['codigo_persona'];
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
                                codigo: data.codigo_persona,
                                nombre: data.nombre_persona,
                                apellido_paterno: data.apellido_paterno,
                                apellido_materno: data.apellido_materno,
                                numero_documento: data.numero_documento,
                                fecha_nacimiento: data.fecha_nacimiento,
                                codigo_sexo: data.codigo_sexo,
                                nombre_sexo: data.nombre_sexo,
                                nombre_corporacion: data.nombre_corporacion,
                                codigo_modalidad_persona: data.codigo_modalidad_persona,
                                nombre_modalidad_persona: data.nombre_modalidad_persona,
                                codigo_estado_civil: data.codigo_estado_civil,
                                nombre_estado_civil: data.nombre_estado_civil,
                                codigo_tipo_documento: data.codigo_tipo_documento,
                                nombre_tipo_documento: data.nombre_tipo_documento,
                                es_vendedor: data.es_vendedor == 'True' ? true : false,
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
                var apellidoPaterno = $.trim($('#dlgModificar #apellidoPaterno').textbox('getText'));
                var apellidoMaterno = $.trim($('#dlgModificar #apellidoMaterno').textbox('getText'));
                var numeroDocumento = $.trim($('#dlgModificar #numeroDocumento').textbox('getText'));
                var fechaNacimiento = $.trim($('#dlgModificar #fechaNacimiento').textbox('getText'));

                var codigoTipoDocumento = $.trim($('#dlgModificar #cboTipoDocumento').combobox('getValue'));
                var codigoEstadoCivil = $.trim($('#dlgModificar #cboEstadoCivil').combobox('getValue'));
                var codigoSexo = $.trim($('#dlgModificar #cboSexo').combobox('getValue'));
                var esVendedor = $('#dlgModificar #esVendedor').switchbutton('options').checked;
                
                if (message.length > 0) {
                    $.messager.alert('Error', message, 'error');
                }
                else {
                    $.messager.confirm('Confirm', 'Seguro que desea modificar?', function (result) {
                        if (result) {
                            var mapData = { codigoPersona: codigo, codigoTipoDocumento: codigoTipoDocumento, codigoEstadoCivil: codigoEstadoCivil, nombrePersona: nombre, apellidoPaterno: apellidoPaterno, apellidoMaterno: apellidoMaterno, numeroDocumento: numeroDocumento, fechaNacimiento: fechaNacimiento, codigoSexo: codigoSexo, esVendedor: esVendedor };

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

                                            $('#dlgModificar #nombre').val('');
                                            $('#dlgRegistrar #apellidoPaterno').val('');
                                            $('#dlgModificar #apellidoMaterno').val('');
                                            $('#dlgModificar #fechaNacimiento').val('');
                                            $('#dlgModificar #numeroDocumento').val('');
                                            $('#dlgModificar #corporacion').val('');

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

                var codigoCorporacion = $.trim($('#dlgRegistrar #cboCorporacion').combobox('getValue'));
                var codigoTipoDocumento = $.trim($('#dlgRegistrar #cboTipoDocumento').combobox('getValue'));
                var codigoEstadoCivil = $.trim($('#dlgRegistrar #cboEstadoCivil').combobox('getValue'));
                var codigoSexo = $.trim($('#dlgRegistrar #cboSexo').combobox('getValue'));

                var nombre = $.trim($('#dlgRegistrar #nombre').textbox('getText'));
                var apellidoPaterno = $.trim($('#dlgRegistrar #apellidoPaterno').textbox('getText'));
                var apellidoMaterno = $.trim($('#dlgRegistrar #apellidoMaterno').textbox('getText'));
                var numeroDocumento = $.trim($('#dlgRegistrar #numeroDocumento').textbox('getText'));
                var fechaNacimiento = $.trim($('#dlgRegistrar #fechaNacimiento').textbox('getText'));
                var esVendedor = $('#dlgRegistrar #esVendedor').switchbutton('options').checked;

                if (message.length > 0) {
                    $.messager.alert('Error', message, 'error');
                }
                else {
                    $.messager.confirm('Confirm', 'Seguro que desea registrar?', function (result) {
                        if (result) {
                            var mapData = { codigoCorporacion: codigoCorporacion, codigoTipoDocumento: codigoTipoDocumento, codigoEstadoCivil: codigoEstadoCivil, nombrePersona: nombre, apellidoPaterno: apellidoPaterno, apellidoMaterno: apellidoMaterno, numeroDocumento: numeroDocumento, fechaNacimiento: fechaNacimiento, codigoSexo: codigoSexo, esVendedor: esVendedor };

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
                                            $('#dlgRegistrar #apellidoPaterno').val('');
                                            $('#dlgRegistrar #apellidoMaterno').val('');
                                            $('#dlgRegistrar #fechaNacimiento').val('');
                                            $('#dlgRegistrar #numeroDocumento').val('');

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