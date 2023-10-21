﻿;
(function (app) {
    //===========================================================================================
    var current = app.ReglaCalculoComisionSupervisor = {};
    //===========================================================================================

    jQuery.extend(app.ReglaCalculoComisionSupervisor,
        {

            ActionUrls: {},
            EditTreeNode: {},
            EditType: '',
            HasRootNode: 'False',
            ComboEmpresa: {},
            ComboCampoSanto: {},
            ComboCanalVenta: {},

            ComboEmpresaFiltro: {},
            ComboCampoSantoFiltro: {},
            ComboCanalVentaFiltro: {},
            
            Initialize: function (actionUrls) {
                jQuery.extend(project.ActionUrls, actionUrls);
                current.Initialize_DataGrid();
                current.ConsultarCombo();
                current.CargarCombosPrincipal();
                current.CargarCombosDialog();

                /*PARA REDIMENSIONAR LAS INTERFACES*/
                var mediaquery = window.matchMedia("(max-width: 600px)");
                if (mediaquery.matches) {
                    $('#dlgRegistrar').dialog('resize', { width: '95%' });
                    $('#dlgRegistrar').window('center');

                    //$('#dlgModificar').dialog('resize', { width: '95%' });
                    //$('#dlgModificar').window('center');

                } else {
                    $('#dlgRegistrar').dialog('resize', { width: '70%' });
                    $('#dlgRegistrar').window('center');

                    //$('#dlgModificar').dialog('resize', { width: '70%' });
                    //$('#dlgModificar').window('center');

                }

                $(window).resize(function () {

                    var mediaquery = window.matchMedia("(max-width: 600px)");
                    if (mediaquery.matches) {
                        $('#dlgRegistrar').dialog('resize', { width: '95%' });
                        //$('#dlgModificar').dialog('resize', { width: '95%' });

                    } else {
                        $('#dlgRegistrar').dialog('resize', { width: '70%' });
                        //$('#dlgModificar').dialog('resize', { width: '70%' });
                    }

                    $('#dlgRegistrar').window('center');
                    //$('#dlgModificar').window('center');
                });
                //FIN REDIMENSIONAMIENTO

                var patronFecha = /^([0-9]{2})\/([0-9]{2})\/([0-9]{4})$/;

                $('#dlgRegistrar #fechaInicio').datebox({

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

                //$('#dlgModificar #fechaInicio').datebox({

                //    formatter: function (date) {
                //        var y = date.getFullYear();
                //        var m = date.getMonth() + 1;
                //        var d = date.getDate();
                //        return (d < 10 ? ('0' + d) : d) + '/' + (m < 10 ? ('0' + m) : m) + '/' + y;
                //    },
                //    parser: function (s) {

                //        if (!s) return new Date();
                //        var ss = s.split('/');
                //        var y = parseInt(ss[2], 10);
                //        var m = parseInt(ss[1], 10);
                //        var d = parseInt(ss[0], 10);
                //        if (!isNaN(y) && !isNaN(m) && !isNaN(d)) {
                //            return new Date(y, m - 1, d)
                //        } else {
                //            return new Date();
                //        }
                //    }

                //});

                $('#dlgRegistrar #fechaFin').datebox({

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

                //$('#dlgModificar #fechaFin').datebox({

                //    formatter: function (date) {
                //        var y = date.getFullYear();
                //        var m = date.getMonth() + 1;
                //        var d = date.getDate();
                //        return (d < 10 ? ('0' + d) : d) + '/' + (m < 10 ? ('0' + m) : m) + '/' + y;
                //    },
                //    parser: function (s) {

                //        if (!s) return new Date();
                //        var ss = s.split('/');
                //        var y = parseInt(ss[2], 10);
                //        var m = parseInt(ss[1], 10);
                //        var d = parseInt(ss[0], 10);
                //        if (!isNaN(y) && !isNaN(m) && !isNaN(d)) {
                //            return new Date(y, m - 1, d)
                //        } else {
                //            return new Date();
                //        }
                //    }

                //});


                $('#btnNuevo').click(function () {
                    current.EditType = 'Registrar';
                    $('#dlgRegistrar').dialog('open').dialog('setTitle', 'REGLA CALCULO COMISION SUPERVISOR - NUEVO REGISTRO');
                    $('#frmRegistrar').form('clear');

                    current.SetearCombos();

                });


                //$('#btnModificar').click(function () {

                //    var codigo = $("#hdCodigo").val();
                //    if (codigo.length == 0) {
                //        project.AlertErrorMessage('Alerta', 'Necesita seleccionar un registro','info');
                //        return;
                //    }

                //    $('#frmModificar').form('clear');

                //    current.GetRegistro(codigo);

                //    if (!Registro.codigo) {
                //        $.messager.alert('Error', 'Error al cargar los datos del registro', 'error', function () {
                //            $('#DataGrid').datagrid('reload');
                //        });
                //    }
                //    else {

                //        current.SetearCombos();

                //        current.EditType = 'Modificar';
                //        $('#dlgModificar').dialog('open').dialog('setTitle', 'REGLA CALCULO COMISION SUPERVISOR - MODIFICAR REGISTRO');

                //        $('#dlgModificar #nombre').textbox('setText', Registro.nombre);
                //        $("#dlgModificar #cmbEmpresa").combobox('setValue', Registro.codigo_empresa);
                //        $("#dlgModificar #cmbCampoSanto").combobox('setValue', Registro.codigo_campo_santo);
                //        $("#dlgModificar #cmbCanalVenta").combobox('setValue', Registro.codigo_canal_grupo);
                //        $('#dlgModificar #valorPago').numberbox('setValue', Registro.valor_pago);
                //        $('#dlgModificar #sbIncluyeIGV').switchbutton({ checked: Registro.incluye_igv });
                //        $('#dlgModificar #fechaInicio').textbox('setText', Registro.vigencia_inicio_str);
                //        $('#dlgModificar #fechaFin').textbox('setText', Registro.vigencia_fin_str);

                //        $('input[name=tipoSupervisorM][value="' + Registro.tipo_supervisor + '"]').prop('checked', true);
                        
                //    }
                //});

                $('#btnVisualizar').click(function () {

                    var codigo = $("#hdCodigo").val();

                    if (codigo.length == 0) {
                        project.AlertErrorMessage('Alerta', 'Necesita seleccionar un registro');
                        return;
                    }

                });


                $('#btnRefrescar').click(function () {
                    $('#DataGrid').datagrid('clearSelections');
                    current.Buscar();
                    $("#hdCodigo").val("");
                });

                //$('#dlgModificar #btnGuardar').click(function () {
                //    current.Guardar();
                //});

                //$('#dlgModificar #btnCancelar').click(function () {
                //    $('#frmModificar').form('clear');
                //    $('#dlgModificar').dialog('close');
                //});

                $('#dlgRegistrar #btnGuardar').click(function () {
                    current.Guardar();
                });

                $('#dlgRegistrar #btnCancelar').click(function () {
                    $('#frmRegistrar').form('clear');
                    $('#dlgRegistrar').dialog('close');
                });

                $("#btnEliminar").on('click', function () { current.Eliminar(); });
            },

            ConsultarCombo: function () {
                var elementoTodos = { "id": "0", "text": "Ninguno" };
                $.ajax({
                    type: 'post',
                    url: project.ActionUrls.GetEmpresaJSON,
                    data: null,
                    async: false,
                    cache: false,
                    dataType: 'json',
                    success: function (data) {
                        current.ComboEmpresa = data;
                        data.push(elementoTodos);
                        current.ComboEmpresaFiltro = data;
                    },
                    error: function () {
                        current.ComboEmpresa = {};
                    }
                });

                $.ajax({
                    type: 'post',
                    url: project.ActionUrls.GetCampoSantoJSON,
                    data: null,
                    async: false,
                    cache: false,
                    dataType: 'json',
                    success: function (data) {
                        data.push(elementoTodos);
                        current.ComboCampoSanto = data;
                        current.ComboCampoSantoFiltro = data;
                    },
                    error: function () {
                        current.ComboCampoSanto = {};
                    }
                });

                $.ajax({
                    type: 'post',
                    url: project.ActionUrls.GetCanalGrupoJSON,
                    data: null,
                    async: false,
                    cache: false,
                    dataType: 'json',
                    success: function (data) {
                        current.ComboCanalVenta = data;
                        data.push(elementoTodos);
                        current.ComboCanalVentaFiltro = data;
                    },
                    error: function () {
                        current.ComboCanalVenta = {};
                    }
                });

            },

            CargarCombosPrincipal: function () {

                $('#cmbEmpresa').combobox({
                    valueField: 'id',
                    textField: 'text',
                    data: current.ComboEmpresaFiltro
                });

                $("#cmbEmpresa").combobox('clear').combobox('setValue', "0");

                $('#cmbCanalVenta').combobox({
                    valueField: 'id',
                    textField: 'text',
                    data: current.ComboCanalVentaFiltro
                });

                $("#cmbCanalVenta").combobox('clear').combobox('setValue', "0");

                $('#cmbCampoSanto').combobox({
                    valueField: 'id',
                    textField: 'text',
                    data: current.ComboCampoSantoFiltro
                });

                $("#cmbCampoSanto").combobox('clear').combobox('setValue', "0");
            },

            CargarCombosDialog: function () {

                $('#dlgRegistrar #cmbEmpresa').combobox({
                    valueField: 'id',
                    textField: 'text',
                    data: current.ComboEmpresa
                });

                $('#dlgRegistrar #cmbCampoSanto').combobox({
                    valueField: 'id',
                    textField: 'text',
                    data: current.ComboCampoSanto
                });

                $('#dlgRegistrar #cmbCanalVenta').combobox({
                    valueField: 'id',
                    textField: 'text',
                    data: current.ComboCanalVenta
                });

                //$('#dlgModificar #cmbEmpresa').combobox({
                //    valueField: 'id',
                //    textField: 'text',
                //    data: current.ComboEmpresa
                //});

                //$('#dlgModificar #cmbCampoSanto').combobox({
                //    valueField: 'id',
                //    textField: 'text',
                //    data: current.ComboCampoSanto
                //});

                //$('#dlgModificar #cmbCanalVenta').combobox({
                //    valueField: 'id',
                //    textField: 'text',
                //    data: current.ComboCanalVenta
                //});

            },

            Initialize_DataGrid: function () {

                var v_entidad = {

                    codigo_campo_santo: $.trim($("#cmbCampoSanto").combobox('getValue')),
                    codigo_empresa: $.trim($("#cmbEmpresa").combobox('getValue')),
                    codigo_canal_grupo: $.trim($("#cmbCanalVenta").combobox('getValue')),

                };

                var things = JSON.stringify(v_entidad);

                $('#DataGrid').datagrid({
                    url: project.ActionUrls.Listar,
                    queryParams: things,
                    fitColumns: true,
                    idField: 'codigo',
                    data:null,
                    pagination: true,
                    singleSelect: true,
                    rownumbers: true,
                    pageList: [20, 60, 80, 100, 150],
                    pageSize: 20,
                    columns:
                    [[
                        { field: 'nombre', title: 'Nombre', width: 100, align: 'left', halign: 'center' },
                        { field: 'nombre_campo_santo', title: 'Camposanto', width: 50, align: 'left', halign: 'center' },
                        { field: 'nombre_empresa', title: 'Empresa', width: 40, align: 'left', halign: 'center' },
                        { field: 'nombre_canal_grupo', title: 'Canal Venta', width: 50, align: 'left', halign: 'center' },
                        { field: 'tipo_supervisor_nombre', title: 'Tipo<br>Supervisor', width: 50, align: 'left', halign: 'center' },
                        {
                            field: 'valor_pago', title: '%<br>Comisión', width: 40, align: 'right', halign: 'center', formatter: function (value, row) {
                                return $.NumberFormat(value, 2);
                            }
                        },
                        { field: 'incluye_igv_str', title: 'Incluye<br>IGV', width: 35, align: 'center', halign: 'center' },
                        { field: 'vigencia_inicio_str', title: 'Inicio<br>Vigencia', width: 45, align: 'center', halign: 'center' },
                        { field: 'vigencia_fin_str', title: 'Fin<br>Vigencia', width: 45, align: 'center', halign: 'center' },
                        { field: 'estado_registro_str', title: 'Estado', width: 40, align: 'center', halign: 'center' },
                         { field: 'indica_estado', hidden:true }
                    ]],
                    onClickRow: function (index, row) {
                        $("#hdCodigo").val(row['codigo']);
                        current.EvalEliminar(row['estado_registro_str']);
                    }
                });

                /******************************************************************************************************/
                $('#DataGrid').datagrid('enableFilter', [{
                    field: 'estado_registro_str',
                    type: 'combobox',
                    options: {
                        panelHeight: 'auto',
                        data: [{ value: '', text: 'Todos' }, { value: '1', text: 'Activo' }, { value: '0', text: 'Inactivo' }],
                        editable:false,
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
                //$('#DataGrid').datagrid('loadData', {});

            },


            Buscar: function () {

                var v_entidad = {

                    codigo_campo_santo: $.trim($("#cmbCampoSanto").combobox('getValue')),
                    codigo_empresa: $.trim($("#cmbEmpresa").combobox('getValue')),
                    codigo_canal_grupo: $.trim($("#cmbCanalVenta").combobox('getValue')),

                };

                $('#DataGrid').datagrid('reload', v_entidad);
                current.EvalEliminar();
            },


            Guardar: function () {

                if (current.EditType == 'Modificar') {
                    current.Modificar();
                }else if (current.EditType == 'Registrar') {
                    current.Registrar();
                }
            },


            GetRegistro: function (id) {

                $.ajax({
                    type: 'post',
                    url: project.ActionUrls.GetRegistro,
                    data: { id: id },
                    async: false,
                    cache: false,
                    dataType: 'json',
                    success: function (data) {
                        var existe = data.existe;

                        if (!existe) {
                            project.AlertErrorMessage('Error', 'Error al consultar el registro seleccionado');
                        }
                        else {
                            var row = data.registro;
                            Registro =
                            {
                                codigo: row.codigo_regla,
                                nombre: row.nombre,
                                codigo_empresa: row.codigo_empresa,
                                codigo_campo_santo: row.codigo_campo_santo,
                                codigo_canal_grupo: row.codigo_canal_grupo,
                                tipo_supervisor: row.tipo_supervisor,
                                valor_pago: row.valor_pago,
                                incluye_igv: row.incluye_igv,
                                vigencia_inicio: row.vigencia_inicio,
                                vigencia_fin: row.vigencia_fin,
                                vigencia_inicio_str: row.vigencia_inicio_str,
                                vigencia_fin_str: row.vigencia_fin_str
                            };
                        }
                    },
                    error: function () {
                        project.AlertErrorMessage('Error', 'Error');
                    }
                });
            },


            //Modificar: function () {

            //    var message = '';

            //    if (message.length > 0) {
            //        $.messager.alert('Error', message, 'error');
            //    }
            //    else {

            //        var mensajeError = "";

            //        var nombre = $.trim($('#dlgModificar #nombre').textbox('getText'));

            //        if (nombre.length == 0) {
            //            mensajeError = "Debe ingresar un nombre.</br>";
            //        }

            //        var codigoCampoSanto = $.trim($("#dlgModificar #cmbCampoSanto").combobox('getValue'));

            //        if (codigoCampoSanto.length == 0) {
            //            mensajeError += "Debe seleccionar un camposanto.</br>";
            //        }

            //        var codigoEmpresa = $.trim($("#dlgModificar #cmbEmpresa").combobox('getValue'));

            //        if (codigoEmpresa.length == 0) {
            //            mensajeError += "Debe seleccionar una empresa.</br>";
            //        }

            //        var codigoCanalGrupo = $.trim($("#dlgModificar #cmbCanalVenta").combobox('getValue'));

            //        if (codigoCanalGrupo.length == 0) {
            //            mensajeError += "Debe seleccionar un canal de venta.</br>";
            //        }

            //        var tipoSupervisor = $('input[name=tipoSupervisorM]:checked').val();

            //        if (tipoSupervisor == null) {
            //            mensajeError += "Debe seleccionar un tipo de supervisor.</br>";
            //        }

            //        var valorPago = $.trim($('#dlgModificar #valorPago').numberbox('getValue'));
                    
            //        if (valorPago.length == 0) {
            //            mensajeError += "Debe ingresar un % del total de Comisiones.</br>";
            //        }
            //        valorPago = parseFloat(valorPago);

            //        var incluyeIGV = $('#dlgRegistrar #sbIncluyeIGV').switchbutton('options').checked;

            //        var fechaInicio = $.trim($('#dlgModificar #fechaInicio').textbox('getText'));

            //        if (fechaInicio.length == 0) {
            //            mensajeError += "Debe ingresar una fecha inicio de vigencia.</br>";
            //        }

            //        var fechaFin = $.trim($('#dlgModificar #fechaFin').textbox('getText'));

            //        if (fechaFin.length == 0) {
            //            mensajeError += "Debe ingresar una fecha fin de vigencia.</br>";
            //        }

            //        if (mensajeError.length > 0) {
            //            project.AlertErrorMessage('Alerta', mensajeError,'info');
            //            return;
            //        }


            //        $.messager.confirm('Confirm', '&iquest;Seguro que desea modificar?', function (result) {
            //            if (result) {

            //                var v_entidad = {
            //                    codigo_regla: $("#hdCodigo").val(),
            //                    nombre: nombre,
            //                    codigo_empresa: codigoEmpresa,
            //                    codigo_campo_santo: codigoCampoSanto,
            //                    codigo_canal_grupo: codigoCanalGrupo,
            //                    tipo_supervisor: tipoSupervisor,
            //                    valor_pago: valorPago,
            //                    incluye_igv: incluyeIGV,
            //                    vigencia_inicio_str: fechaInicio,
            //                    vigencia_fin_str: fechaFin
            //                };

            //                var things = JSON.stringify(v_entidad);

            //                $.ajax({
            //                    type: 'post',
            //                    url: project.ActionUrls.Modificar,
            //                    data: things,
            //                    async: false,
            //                    cache: false,
            //                    dataType: 'json',
            //                    contentType: 'application/json; charset=utf-8',
            //                    success: function (data) {
            //                        if (data.v_resultado == 1) {
            //                            //OK
            //                            project.ShowMessage('Alerta', data.v_mensaje);

            //                            $('#dlgModificar').dialog('close');

            //                            $('#DataGrid').datagrid('clearSelections');
            //                            $("#hdCodigo").val("");

            //                            current.Buscar();

            //                        } else {
            //                            //ERROR
            //                            project.AlertErrorMessage('Alerta', data.v_mensaje, 'info');

            //                        }
            //                    },
            //                    error: function () {
            //                        project.AlertErrorMessage('Error', 'Error');
            //                    }
            //                });
            //            }
            //        });
            //    }
            //},


            Registrar: function () {

                var message = '';

                if (message.length > 0) {
                    $.messager.alert('Error', message, 'error');
                }
                else {

                    var mensajeError = "";

                    var nombre = $.trim($('#dlgRegistrar #nombre').textbox('getText'));

                    if (nombre.length == 0) {
                        mensajeError = "Debe ingresar un nombre.</br>";
                    }

                    var codigoCampoSanto = $.trim($("#dlgRegistrar #cmbCampoSanto").combobox('getValue'));
                    if (codigoCampoSanto.length == 0) {
                        mensajeError += "Debe seleccionar un camposanto.</br>";
                    }

                    var codigoEmpresa = $.trim($("#dlgRegistrar #cmbEmpresa").combobox('getValue'));

                    if (codigoEmpresa.length == 0) {
                        mensajeError += "Debe seleccionar una empresa.</br>";
                    }

                    var codigoCanalGrupo = $.trim($("#dlgRegistrar #cmbCanalVenta").combobox('getValue'));

                    if (codigoCanalGrupo.length == 0) {
                        mensajeError += "Debe seleccionar un canal de venta.</br>";
                    }

                    var tipoSupervisor = $('input[name=tipoSupervisorR]:checked').val();

                    if (tipoSupervisor == null) {
                        mensajeError += "Debe seleccionar un tipo de supervisor.</br>";
                    }

                    var valorPago = $.trim($('#dlgRegistrar #valorPago').numberbox('getValue'));

                    if (valorPago.length == 0) {
                        mensajeError += "Debe ingresar un % del total de Comisiones.</br>";
                    }
                    valorPago = parseFloat(valorPago);

                    var incluyeIGV = $('#dlgRegistrar #sbIncluyeIGV').switchbutton('options').checked;

                    var fechaInicio = $.trim($('#dlgRegistrar #fechaInicio').textbox('getText'));

                    if (fechaInicio.length == 0) {
                        mensajeError += "Debe ingresar una fecha inicio de vigencia.</br>";
                    }

                    var fechaFin = $.trim($('#dlgRegistrar #fechaFin').textbox('getText'));

                    if (fechaFin.length == 0) {
                        mensajeError += "Debe ingresar una fecha fin de vigencia.</br>";
                    }

                    if (mensajeError.length > 0) {
                        project.AlertErrorMessage('Alerta', mensajeError,'info');
                        return;
                    }

                    $.messager.confirm('Confirm', '&iquest;Seguro que desea registrar?', function (result) {
                        if (result) {

                            var v_entidad = {
                                nombre: nombre,
                                codigo_empresa: codigoEmpresa,
                                codigo_campo_santo: codigoCampoSanto,
                                codigo_canal_grupo: codigoCanalGrupo,
                                tipo_supervisor: tipoSupervisor,
                                valor_pago: valorPago,
                                incluye_igv: incluyeIGV,
                                vigencia_inicio_str: fechaInicio,
                                vigencia_fin_str: fechaFin
                            };

                            var things = JSON.stringify(v_entidad);

                            $.ajax({
                                type: 'post',
                                url: project.ActionUrls.Registrar,
                                data: things,
                                async: false,
                                cache: false,
                                dataType: 'json',
                                contentType: 'application/json; charset=utf-8',
                                success: function (data) {
                                    if (data.v_resultado == 1) {
                                        //OK
                                        project.ShowMessage('Alerta', data.v_mensaje);

                                        $('#dlgRegistrar').dialog('close');

                                        current.Buscar();

                                    } else {
                                        //ERROR
                                        project.AlertErrorMessage('Alerta', data.v_mensaje, 'info');

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

            SetearCombos: function () {
                $("#dlgRegistrar #cmbCampoSanto").combobox('clear').combobox('setValue', "0");

                //$("#dlgModificar #cmbCampoSanto").combobox('clear').combobox('setValue', "0");
            },

            EvalEliminar: function (valor) {
                $("#btnEliminar").linkbutton((valor == 'Activo' ? 'enable' : 'disable'));
            },
        
            Eliminar: function () {
                var codigo = $("#hdCodigo").val();

                if ($('#btnEliminar').linkbutton('options').disabled) { return false };

                if (codigo.length == 0) {
                    project.AlertErrorMessage('Alerta', 'Necesita seleccionar un registro', 'info');
                    return;
                }

                if (codigo != null) {
                    $.messager.confirm('Confirm', '&iquest;Seguro que desea desactivar este registro?', function (result) {
                        if (result) {

                            var v_entidad = {
                                codigo_regla: codigo
                            };

                            var things = JSON.stringify(v_entidad);

                            $.ajax({
                                type: 'post',
                                url: project.ActionUrls.Eliminar,
                                data: things,
                                async: false,
                                cache: false,
                                dataType: 'json',
                                contentType: 'application/json; charset=utf-8',
                                success: function (data) {
                                    if (data.v_resultado == 1) {
                                        //OK
                                        project.ShowMessage('Alerta', data.v_mensaje);
                                        $('#dlgRegistrar').dialog('close');

                                        $('#DataGrid').datagrid('clearSelections');
                                        $("#hdCodigo").val("");
                                    } else {
                                        //ERROR
                                        project.AlertErrorMessage('Error', data.v_mensaje);
                                    }
                                    current.Buscar();
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