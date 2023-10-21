;
(function (app) {
    //===========================================================================================
    var current = app.ReglaPagoComision = {};

    var elementoTodos = { "id": "0", "text": "Ninguno" };
    //===========================================================================================

    jQuery.extend(app.ReglaPagoComision,
        {

            ActionUrls: {},
            ComboCanalVenta: {},
            comboTipoVenta: {},
            comboTipoPlanilla: {},
            comboTipoArticulo: {},
            Initialize: function (actionUrls) {
                jQuery.extend(project.ActionUrls, actionUrls);
                current.Initialize_DataGrid();
                current.InicializarCombo();             


                

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
                    $('#dlgRegistrar').dialog('resize', { width: '70%' });
                    $('#dlgRegistrar').window('center');

                    $('#dlgModificar').dialog('resize', { width: '70%' });
                    $('#dlgModificar').window('center');

                    $('#dlgVisualizar').dialog('resize', { width: '70%' });
                    $('#dlgVisualizar').window('center');
                }

                $(window).resize(function () {

                    var mediaquery = window.matchMedia("(max-width: 600px)");
                    if (mediaquery.matches) {
                        $('#dlgRegistrar').dialog('resize', { width: '95%' });
                        $('#dlgModificar').dialog('resize', { width: '95%' });
                        $('#dlgVisualizar').dialog('resize', { width: '95%' });

                    } else {
                        $('#dlgRegistrar').dialog('resize', { width: '70%' });
                        $('#dlgModificar').dialog('resize', { width: '70%' });
                        $('#dlgVisualizar').dialog('resize', { width: '70%' });
                    }

                    $('#dlgRegistrar').window('center');
                    $('#dlgModificar').window('center');
                    $('#dlgVisualizar').window('center');
                });
                //FIN REDIMENSIONAMIENTO



               
                $('#btnNuevo').click(function () {              

                    $(this).AbrirVentanaEmergente({
                        parametro: "?p_codigo_regla_comision=" + 0 + "&tipo=1", 
                        div: 'div_registrar_regla',
                        title: "REGLA PAGO COMISION - NUEVO REGISTRO",
                        url: project.ActionUrls._ViewRegistrarRegla
                    });
                });




                $('#btnRefrescar').click(function () {
                    $('#DataGrid').datagrid('clearSelections');
                    current.Buscar();
                    $("#hdCodigo").val("");
                    $("#hdEstado").val("");
                });
                $('#btnModificar').click(function () {

                    var codigo = $("#hdCodigo").val();
                    current.ViewRegistroRegla(codigo);
              
                });
                $('#btnDetalle').click(function () {

                    var codigo = $("#hdCodigo").val();
                    if (codigo.length == 0) {
                        project.AlertErrorMessage('Alerta', 'Necesita seleccionar un registro', 'info');
                        return;
                    }

                    $(this).AbrirVentanaEmergente({
                        parametro: "?p_codigo_regla_comision=" + codigo+"&tipo=0",
                        div: 'div_registrar_regla',
                        title: "REGLA PAGO COMISION - DETALLE REGISTRO",
                        url: project.ActionUrls._ViewRegistrarRegla
                    });

                 

                });
                $("#btnEliminar").on('click', function () { current.Eliminar(); });                        

            },

            ViewRegistroRegla: function (codigo) {
                if (codigo.length == 0) {
                    project.AlertErrorMessage('Alerta', 'Necesita seleccionar un registro', 'info');
                    return;
                }

                $(this).AbrirVentanaEmergente({
                    parametro: "?p_codigo_regla_comision=" + codigo + "&tipo=1",
                    div: 'div_registrar_regla',
                    title: "REGLA PAGO COMISION - ACTUALIZAR REGISTRO",
                    url: project.ActionUrls._ViewRegistrarRegla
                });

            },
            InicializarCombo: function () {


                $.ajax({
                    type: 'post',
                    url: project.ActionUrls.GetCanalGrupoJSON,
                    data: null,
                    async: false,
                    cache: false,
                    dataType: 'json',
                    success: function (data) {
                        data.push(elementoTodos);
                        current.ComboCanalVenta = data;
                    },
                    error: function () {
                        current.ComboCanalVenta = {};
                    }
                });

                $.ajax({
                    type: 'post',
                    url: project.ActionUrls.GetTipoVentaJSON,
                    data: null,
                    async: false,
                    cache: false,
                    dataType: 'json',
                    success: function (data) {
                        data.push(elementoTodos);
                        current.comboTipoVenta = data;
                    },
                    error: function () {
                        current.comboTipoVenta = {};
                    }
                });



                $.ajax({
                    type: 'post',
                    url: project.ActionUrls.GetTipoArticuloJson,
                    data: null,
                    async: false,
                    cache: false,
                    dataType: 'json',
                    success: function (data) {
                        data.push(elementoTodos);
                        current.comboTipoArticulo = data;
                    },
                    error: function () {
                        current.comboTipoArticulo = {};
                    }
                });

                $.ajax({
                    type: 'post',
                    url: project.ActionUrls.GetTipoPlanillaJSON,
                    data: null,
                    async: false,
                    cache: false,
                    dataType: 'json',
                    success: function (data) {
                        data.push(elementoTodos);
                        current.comboTipoPlanilla = data;
                    },
                    error: function () {
                        current.comboTipoPlanilla = {};
                    }
                });


                $('#cmbCanalVenta').combobox({
                    valueField: 'id',
                    textField: 'text',
                    data: current.ComboCanalVenta
                });

                $('#cmbTipoVenta').combobox({
                    valueField: 'id',
                    textField: 'text',
                    data: current.comboTipoVenta
                });


                $('#cmbTipoArticulo').combobox({
                    valueField: 'id',
                    textField: 'text',
                    data: current.comboTipoArticulo
                });

                $('#cmbTipoPlanilla').combobox({
                    valueField: 'id',
                    textField: 'text',
                    data: current.comboTipoPlanilla
                });


                $("#cmbTipoPlanilla").combobox('setValue', "0");
                $("#cmbCanalVenta").combobox('setValue', "0");
                $("#cmbTipoVenta").combobox('setValue', "0");
                $("#cmbTipoArticulo").combobox('setValue', "0");

            },

       
      

            Initialize_DataGrid: function () {

                var v_entidad = {
                    codigo_canal_grupo: '0',
                    codigo_tipo_articulo: '0',
                    codigo_tipo_venta: '0'
                };
                var things = JSON.stringify(v_entidad);

                $('#DataGrid').datagrid({
                    url: project.ActionUrls.Listar,
                    queryParams: things,
                    fitColumns: true,
                    idField: 'codigo_regla_comision',
                    data: null,
                    pagination: true,
                    singleSelect: true,
                    rownumbers: true,
                    pageList: [20, 60, 80, 100, 150],
                    pageSize: 20,
                    columns:
                        [[
                            { field: 'nombre_regla_comision', title: 'Nombre', width: 100, align: 'left', halign: 'center' },
                            { field: 'nombre_canal_grupo', title: 'Canal Venta', width: 50, align: 'left', halign: 'center' },
                            { field: 'nombre_tipo_venta', title: 'Tipo Venta', width: 50, align: 'left', halign: 'center' },
                            { field: 'nombre_tipo_planilla', title: 'Tipo Planilla', width: 50, align: 'left', halign: 'center' },
                            { field: 'nombre_tipo_articulo', title: 'Tipo Articulo', width: 50, align: 'left', halign: 'center' },

                            { field: 'nombre_articulo', title: 'Artículo', width: 50, align: 'center', halign: 'center' },
                            { field: 'estado_registro_nombre', title: 'Estado', width: 50, align: 'center', halign: 'center' },
                            { field: 'estado_registro', hidden: 'true' }
                            //{ field: 'indica_estado', hidden: 'true' }

                        ]],
                    onClickRow: function (index, row) {
                        $("#hdCodigo").val(row['codigo_regla_comision']);
                        $("#hdEstado").val(row['estado_registro']);
                        current.EvalEliminar(row['estado_registro_nombre']);
                    }
                });

                /******************************************************************************************************/
                $('#DataGrid').datagrid('enableFilter', [{
                    field: 'estado_registro_nombre',
                    type: 'combobox',
                    options: {
                        panelHeight: 'auto',
                        data: [{ value: '', text: 'Todos' }, { value: '1', text: 'Activo' }, { value: '0', text: 'Inactivo' }],
                        editable: false,
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

            },


            Buscar: function () {
                var queryParams = {
                    codigo_canal_grupo: $.trim($("#cmbCanalVenta").combobox('getValue')),
                    codigo_tipo_articulo: $.trim($("#cmbTipoArticulo").combobox('getValue')),
                    codigo_tipo_venta: $.trim($("#cmbTipoVenta").combobox('getValue')),
                    codigo_tipo_planilla: $.trim($("#cmbTipoPlanilla").combobox('getValue'))

                };


                $('#DataGrid').datagrid('reload', queryParams);
                current.EvalEliminar();
            },

                   
                     

            Eliminar: function () {
                var codigo = $("#hdCodigo").val();

                if ($('#btnEliminar').linkbutton('options').disabled) { return false };

                if (codigo.length == 0) {
                    project.AlertErrorMessage('Alerta', 'Necesita seleccionar un registro.', 'warning');
                    return;
                }

                if ($("#hdEstado").val() == 'false') {
                    project.AlertErrorMessage('Alerta', 'No puede desactivar un registro inactivo.', 'warning');
                    return;
                }

                if (codigo != null) {
                    $.messager.confirm('Confirm', '&iquest;Seguro que desea desactivar este registro?', function (result) {
                        if (result) {

                            var v_entidad = {
                                codigo_regla_comision: codigo
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
                                        /*  $('#dlgRegistrar').dialog('close');*/

                                        $('#DataGrid').datagrid('clearSelections');
                                        $("#hdCodigo").val("");
                                        $("#hdEstado").val("");
                                        current.Buscar();
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

            EvalEliminar: function (valor) {
                $("#btnEliminar").linkbutton((valor == 'Activo' ? 'enable' : 'disable'));
            },

        });

})
    (project);