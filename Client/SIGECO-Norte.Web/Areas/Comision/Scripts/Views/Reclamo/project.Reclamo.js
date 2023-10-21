
var JsonBanco = {};
var Registro = {};
var dataEmpresa = {};
var dataResultado = {};

;(function (app) {
    //===========================================================================================
    var current = app.Reclamo = {};
    //===========================================================================================

    jQuery.extend(app.Reclamo,
        {

            ActionUrls: {},
            EditTreeNode: {},
            EditType: '',
            codigoPersonal: '',
            nombrePersonal: '',
            codigoArticulo: '',
            nombreArticulo: '',
            numeroCuota: '',
            montoCuota: '',
            estadoCuota: '',
            HasRootNode: 'False',

            Initialize: function (actionUrls) {
                $(window).resize(function () {
                    //current.Redimensionar();
                });

                jQuery.extend(project.ActionUrls, actionUrls);

                //current.Redimensionar();
                current.InicializarControles();
                current.InicializarListado();
                current.InicializarGrillaPersonal();
                current.InicializarGrillaContrato();
                current.LimpiarEstadoContrato();

                $('#btnBuscar').click(function () {
                    current.GetAllJson();
                    $('#DataGrid').datagrid('clearSelections');
                    $("#hdCodigo").val("");
                });

                $('#btnNuevo').click(function () {
                    current.EditType = 'Registrar';
                    current.LimpiarGrillaContrato();
                    current.LimpiarEstadoContrato();
                    
                    current.LimpiarRegistro();
                    $('#dlgRegistrar').dialog('open').dialog('setTitle', 'NUEVO RECLAMO');
                });

                $('#btnAtender').click(function () {
                    
                    var codigo = $("#hdCodigo").val();
                    var Estado = $("#hdEstado").val();
                    var row = $('#DataGrid').datagrid('getSelected');


                    if (!row)
                    {
                        $.messager.alert('Reclamo', 'Debe seleccionar un registro.', 'warning');
                        return false;
                    }

                    if (row.codigo_estado_resultado_n1 == '0') {
                        $.messager.alert('Reclamo', 'El reclamo debe ser atendido primero por el área comercial.', 'warning');
                        return false;
                    }

                    if (row.codigo_estado_resultado_n1 == '2') {
                        $.messager.alert('Reclamo', 'El reclamo fue rechazado por el área comercial.', 'warning');
                        return false;
                    }

                    if (row.codigo_estado_reclamo == '2') {
                        $.messager.alert('Reclamo', 'El reclamo ya fue atendido.', 'warning');
                        return false;
                    }

                    current.GetRegistro(codigo);

                    if (!Registro.codigo_reclamo) {
                        $.messager.alert('Error', 'Error al cargar los datos del registro', 'error', function () {
                            $('#DataGrid').treegrid('reload');
                        });
                        return false;
                    }
                    
                    current.EditType = 'Modificar';
                    current.LimpiarAtender();

                    $('#dlgAtender #hdCodigoPersonal').val(Registro.codigo_personal);
                    $('#dlgAtender #PersonalVentas').textbox('setText', Registro.PersonalVentas);
                    $('#dlgAtender #NroContrato').textbox('setText', Registro.NroContrato);
                    $('#dlgAtender #cboEmpresa_1').combobox('setValue', Registro.codigo_empresa);
                    $('#dlgAtender #hdCodigoArticulo_1').val(Registro.codigo_articulo);
                    $('#dlgAtender #Articulo_1').textbox('setText', Registro.Articulo);
                    $('#dlgAtender #Cuota_1').textbox('setText', Registro.Cuota);
                    $('#dlgAtender #Importe_1').textbox('setText', Registro.Importe);
                    $('#dlgAtender #Observacion_1').textbox('setText', Registro.Observacion);

                    //$('#dlgAtender #cboEmpresa_2').combobox('setValue', Registro.atencion_codigo_empresa);
                    //$('#dlgAtender #hdCodigoArticulo_2').val(Registro.atencion_codigo_articulo);
                    //$('#dlgAtender #Articulo_2').textbox('setText', Registro.atencion_Articulo);
                    if (Registro.atencion_Cuota == -1)
                        $('#dlgAtender #Cuota_2').textbox('setText', "");
                    else
                        $('#dlgAtender #Cuota_2').textbox('setText', Registro.atencion_Cuota);
                    $('#dlgAtender #Importe_2').textbox('setText', Registro.atencion_Importe);
                    $('#dlgAtender #cboEstado_2').combobox('setValue', Registro.codigo_estado_reclamo);
                    if (Registro.codigo_estado_resultado == 0 || Registro.codigo_estado_resultado == -1)
                        $('#dlgAtender #cboResultado_2').combobox('setValue', "");
                    else
                        $('#dlgAtender #cboResultado_2').combobox('setValue', Registro.codigo_estado_resultado);
                    $('#dlgAtender #Respuesta_2').textbox('setText', Registro.Respuesta);
                    
                    $('#dlgAtender #hdAtenderEsContratoMigrado').val(Registro.es_contrato_migrado);
                    var texto_es_contrato_migrado = (Registro.es_contrato_migrado == 1 ? 'Si' : 'No');
                    $('#dlgAtender #es_contrato_migrado').textbox('setText', texto_es_contrato_migrado);
                    $('#dlgAtender #Importe_2').textbox('readonly', (Registro.es_contrato_migrado == 1 ? true : false));

                    $('#dlgAtender').dialog('open').dialog('setTitle', 'ATENCIÓN RECLAMO');
                });

                $('#btnAtenderN1').click(function () {

                    var codigo = $("#hdCodigo").val();
                    var Estado = $("#hdEstado").val();
                    var row = $('#DataGrid').datagrid('getSelected');


                    if (!row) {
                        $.messager.alert('Reclamo', 'Debe seleccionar un registro.', 'warning');
                        return false;
                    }

                    if (row.codigo_estado_reclamo == '2') {
                        $.messager.alert('Reclamo', 'El reclamo ya fue atendido.', 'warning');
                        return false;
                    }

                    if (row.codigo_estado_resultado_n1 != '0') {
                        $.messager.alert('Reclamo', 'El reclamo fue ' + (row.codigo_estado_resultado_n1 == '1'?'aceptado':'rechazado') + ' por el área comercial.', 'warning');
                        return false;
                    }

                    current.GetRegistro(codigo);

                    if (!Registro.codigo_reclamo) {
                        $.messager.alert('Error', 'Error al cargar los datos del registro', 'error', function () {
                            $('#DataGrid').treegrid('reload');
                        });
                        return false;
                    }

                    current.EditType = 'Modificar';
                    current.LimpiarAtenderN1();

                    $('#dlgAtenderN1 #hdCodigoPersonal').val(Registro.codigo_personal);
                    $('#dlgAtenderN1 #PersonalVentas').textbox('setText', Registro.PersonalVentas);
                    $('#dlgAtenderN1 #NroContrato').textbox('setText', Registro.NroContrato);
                    $('#dlgAtenderN1 #nombre_empresa_a').textbox('setText', Registro.nombre_empresa);
                    $('#dlgAtenderN1 #hdCodigoArticulo_1').val(Registro.codigo_articulo);
                    $('#dlgAtenderN1 #Articulo_1').textbox('setText', Registro.Articulo);
                    $('#dlgAtenderN1 #Cuota_1').textbox('setText', Registro.Cuota);
                    $('#dlgAtenderN1 #Importe_1').textbox('setText', Registro.Importe);
                    $('#dlgAtenderN1 #Observacion_1').textbox('setText', Registro.Observacion);
                    $('#dlgAtenderN1 #nombre_estado_reclamo_a').textbox('setText', Registro.nombre_estado_reclamo);
                    if (Registro.codigo_estado_resultado == 0 || Registro.codigo_estado_resultado == -1)
                        $('#dlgAtenderN1 #cboResultado_N1').combobox('setValue', "");
                    else
                        $('#dlgAtenderN1 #cboResultado_N1').combobox('setValue', Registro.codigo_estado_resultado);
                    $('#dlgAtenderN1 #Respuesta_2').textbox('setText', Registro.Respuesta);

                    $('#dlgAtenderN1 #hdAtenderEsContratoMigrado').val(Registro.es_contrato_migrado);
                    var texto_es_contrato_migrado = (Registro.es_contrato_migrado == 1 ? 'Si' : 'No');
                    $('#dlgAtenderN1 #es_contrato_migrado').textbox('setText', texto_es_contrato_migrado);

                    $('#dlgAtenderN1').dialog('open').dialog('setTitle', 'ATENCIÓN RECLAMO');
                });

                $('#btnVisualizar').click(function () {

                    var codigo = $("#hdCodigo").val();
                    if (!codigo) {
                        $.messager.alert('Reclamo', '<br>Debe seleccionar un registro.', 'warning');
                        return false;
                    }

                    current.GetRegistro(codigo);

                    if (!Registro.codigo_reclamo) {
                        $.messager.alert('Error', 'Error al cargar los datos del registro', 'error', function () {
                            $('#DataGrid').treegrid('reload');
                        });
                        return false;
                    }

                    current.EditType = 'Visualizar';
                    //current.LimpiarAtender();

                    $('#dlgDetalle #PersonalVentas').textbox('setText', Registro.PersonalVentas);
                    $('#dlgDetalle #NroContrato').textbox('setText', Registro.NroContrato);
                    $('#dlgDetalle #nombre_empresa_v').textbox('setText', Registro.nombre_empresa);
                    $('#dlgDetalle #Articulo_1').textbox('setText', Registro.Articulo);
                    $('#dlgDetalle #Cuota_1').textbox('setText', Registro.Cuota);
                    $('#dlgDetalle #Importe_1').textbox('setText', Registro.Importe);
                    $('#dlgDetalle #Observacion_1').textbox('setText', Registro.Observacion);

                    if (Registro.atencion_Cuota == -1)
                        $('#dlgDetalle #Cuota_2').textbox('setText', "");
                    else
                        $('#dlgDetalle #Cuota_2').textbox('setText', Registro.atencion_Cuota);
                    $('#dlgDetalle #Importe_2').textbox('setText', Registro.atencion_Importe);
                    $('#dlgDetalle #nombre_estado_reclamo_v').textbox('setText', Registro.nombre_estado_reclamo);

                    $('#dlgDetalle #nombre_estado_resultado_n2_v').textbox('setText', Registro.nombre_estado_resultado_n2);

                    $('#dlgDetalle #observacion_n2_v').textbox('setText', Registro.observacion_n2);

                    $('#dlgDetalle #NroPlanilla').textbox('setText', Registro.numero_planilla);
                    
                    $('#dlgDetalle #usuario_n2_v').textbox('setText', Registro.usuario_n2);
                    $('#dlgDetalle #fecha_n2_v').textbox('setText', Registro.fecha_n2);
                    
                    var texto_es_contrato_migrado = (Registro.es_contrato_migrado == 1?'Si':'No');
                    $('#dlgDetalle #es_contrato_migrado').textbox('setText', texto_es_contrato_migrado);

                    $('#dlgDetalle #UsuarioReg').textbox('setText', Registro.UsuarioRegistra);
                    $('#dlgDetalle #FechaReg').textbox('setText', Registro.FechaRegistra);

                    $('#dlgDetalle #nombre_estado_resultado_n1_v').textbox('setText', Registro.nombre_estado_resultado_n1);
                    $('#dlgDetalle #usuario_n1_v').textbox('setText', Registro.usuario_n1);
                    $('#dlgDetalle #fecha_n1_v').textbox('setText', Registro.fecha_n1);
                    $('#dlgDetalle #observacion_n1_v').textbox('setText', Registro.observacion_n1);

                    $('#dlgDetalle #nombre_estado_resultado_n2_v').textbox('setText', Registro.nombre_estado_resultado_n2);
                    $('#dlgDetalle #usuario_n2_v').textbox('setText', Registro.usuario_n2);
                    $('#dlgDetalle #fecha_n2_v').textbox('setText', Registro.fecha_n2);
                    $('#dlgDetalle #observacion_n2_v').textbox('setText', Registro.observacion_n2);

                    $('#dlgDetalle').dialog('open').dialog('setTitle', 'DETALLE RECLAMO: ' + Registro.codigo_reclamo);
                    $('#dlgDetalle #tabAtencion').tabs('select', 0);
                });
                
                $('#btnSeleccionarCuotaComision').click(function () {
                    current.SeleccionarCuota();
                });


                //---------Carga Personal
                $('#dlgRegistrar #btnBuscarPersona').click(function () {
                    current.LimpiarGrillaPersonal();
                    console.log("Limpiando Grilla de personal");

                    $('#dlgBusquedaPersona').dialog('open').dialog('setTitle', 'BUSCAR VENDEDOR');
                    $('#dlgBusquedaPersona').form('clear');
                });

                //-------------CONTROLS ONCHANGE
                $('#dlgRegistrar #NroContrato').change(function () {
                        current.LimpiarSeleccionArticulo();
                });

                $('#dlgRegistrar #cboEmpresa').combobox({
                    onChange: function () {
                        current.LimpiarSeleccionArticulo();
                    }
                });

                //---------Carga ARTICULO
                $('#dlgRegistrar #btnBuscarArticulo').click(function () {
                    var message = '';

                    var NroContrato = $.trim($('#dlgRegistrar #NroContrato').val());
                    var codigo_empresa = $.trim($('#dlgRegistrar #cboEmpresa').combobox('getValue'));
                    var codigo_personal = $('#dlgRegistrar #hdCodigoPersonal').val();

                    if (NroContrato == '') {
                        message += "Contrato, campo requerido. \n"
                    }
                    if (codigo_empresa == '') {
                        message += "Empresa, campo requerido. \n"
                    }
                    if (codigo_personal == '') {
                        message += "Vendedor, campo requerido. \n"
                    }

                    if (message.length > 0) {
                        $.messager.alert('Reclamo', message, 'warning');
                    }
                    else {
                        current.ExisteContratoJson(codigo_empresa, NroContrato, codigo_personal);
                    }
                });

                $('#dlgSeleccionCuotaBotones #btnCancelar').click(function () {
                    $('#frmSeleccionCuota').form('clear');
                    $('#dlgSeleccionCuota').dialog('close');
                    current.LimpiarGrillaContrato();
                    current.LimpiarEstadoContrato();
                });

                //$('#dlgSeleccionCuotaBotones #btnSeleccionar').click(function () {
                //    current.SeleccionarCuota();
                //});

                //----------------------

                $('#dlgRegistrar #btnGuardar').click(function () {
                    current.Guardar();
                });
                $('#dlgRegistrar #btnCancelar').click(function () {
                    $('#frmRegistro').form('clear');
                    $('#dlgRegistrar').dialog('close');
                });

                $('#dlgAtender #btnGuardar').click(function () {
                    current.Guardar();
                });

                $('#dlgAtender #btnCancelar').click(function () {
                    $('#frmAtender').form('clear');
                    $('#dlgAtender').dialog('close');
                });

                $('#dlgAtenderN1 #btnGuardar').click(function () {
                    current.AtenderN1();
                });

                $('#dlgAtenderN1 #btnCancelar').click(function () {
                    $('#frmAtenderN1').form('clear');
                    $('#dlgAtenderN1').dialog('close');
                });

                $("#dlgRegistrar #NroContrato").blur(function () {
                    var texto = $.trim($("#dlgRegistrar #NroContrato").val());
                    var ceros = '0';
                    if (texto.length > 0 && texto.length < 10) {
                        texto = ceros.repeat(10 - texto.length) + texto;
                    }
                    $("#dlgRegistrar #NroContrato").val(texto);
                });

                $('#dlgDetalle #btnCerrar').click(function () {
                    $('#dlgDetalle').dialog('close');
                });

            },

            InicializarControles: function () {
                var dataEstadoReclamo = current.CargarDataTemporal(project.ActionUrls.GetAllEstadoReclamoJson);
                dataEmpresa = current.CargarDataTemporal(project.ActionUrls.GetAllEmpresasJson);
                dataResultado = current.CargarDataTemporal(project.ActionUrls.GetAllEstadoResultadoJson);
                
                $('#cboEstado').combobox({
                    valueField: 'id',
                    textField: 'text',
                    data: dataEstadoReclamo
                });

                $('#dlgRegistrar #cboEmpresa').combobox({
                    valueField: 'id',
                    textField: 'text',
                    data: dataEmpresa
                });

                $('#dlgAtender #cboEmpresa_1').combobox({
                    valueField: 'id',
                    textField: 'text',
                    data: dataEmpresa
                });
                $('#dlgAtender #cboEmpresa_2').combobox({
                    valueField: 'id',
                    textField: 'text',
                    data: dataEmpresa
                });

                $('#dlgAtender #cboEstado_2').combobox({
                    valueField: 'id',
                    textField: 'text',
                    data: dataEstadoReclamo
                });
                $('#dlgAtender #cboResultado_2').combobox({
                    valueField: 'id',
                    textField: 'text',
                    data: dataResultado
                });

                //$('#dlgDetalle #cboEmpresa_1').combobox({
                //    valueField: 'id',
                //    textField: 'text',
                //    data: dataEmpresa
                //});

                //$('#dlgDetalle #cboEmpresa_2').combobox({
                //    valueField: 'id',
                //    textField: 'text',
                //    data: dataEmpresa
                //});

                //$('#dlgDetalle #cboEstado_2').combobox({
                //    valueField: 'id',
                //    textField: 'text',
                //    data: dataEstadoReclamo
                //});

                //$('#dlgDetalle #cboResultado_2').combobox({
                //    valueField: 'id',
                //    textField: 'text',
                //    data: dataResultado
                //});

                $('#dlgAtenderN1 #cboResultado_N1').combobox({
                    valueField: 'id',
                    textField: 'text',
                    data: dataResultado
                });

                //$('#dlgAtenderN1 #cboEmpresa_N1').combobox({
                //    valueField: 'id',
                //    textField: 'text',
                //    data: dataEmpresa
                //});

                //$('#dlgAtenderN1 #cboEstado_N1').combobox({
                //    valueField: 'id',
                //    textField: 'text',
                //    data: dataEstadoReclamo
                //});

            },

            LimpiarRegistro: function () {
                
                codigoPersonal = '';
                nombrePersonal = '';

                codigoArticulo = '';
                nombreArticulo = '';

                numeroCuota = '';
                montoCuota = '';
                estadoCuota = '';

                $('#frmRegistro').form('clear');
                $('#dlgRegistrar #cboEmpresa').combobox('clear').combobox('loadData', dataEmpresa);

                $('#dlgRegistrar #hdCodigoPersonal').val('');
                $('#dlgRegistrar #PersonalVentas').val('');
                $('#dlgRegistrar #NroContrato').val('');
                //$('#dlgRegistrar #cboEmpresa')[0].selectedIndex = 0;
                $('#dlgRegistrar #hdCodigoArticulo').val('');
                $('#dlgRegistrar #Articulo').val('');
                $('#dlgRegistrar #Cuota').val('');
                $('#dlgRegistrar #Importe').val('');
                $('#dlgRegistrar #ImporteOriginal').val('');
                $('#dlgRegistrar #Observacion').val('');
                $('#dlgRegistrar #error_contrato_migrado').textbox('setText', '');
                
            },
            LimpiarAtender: function(){
                
                codigoPersonal = '';
                nombrePersonal = '';

                codigoArticulo = '';
                nombreArticulo = '';

                numeroCuota = '';
                montoCuota = '';
                estadoCuota = '';

                $('#frmAtender').form('clear');
                $('#dlgAtender #cboEmpresa_2').combobox('clear');
                $('#dlgAtender #cboResultado_2').combobox('clear').combobox('loadData', dataResultado);

                $('#dlgAtender #hdCodigoPersonal').val('');
                $('#dlgAtender #PersonalVentas').val('');
                $('#dlgAtender #NroContrato').val('');
                $('#dlgAtender #cboEmpresa_1')[0].selectedIndex = 0;
                $('#dlgAtender #Articulo_1').val('');
                $('#dlgAtender #Cuota_1').val('');
                $('#dlgAtender #Importe_1').val('');
                $('#dlgAtender #Observacion_1').val('');
                
                //$('#dlgAtender #cboEmpresa_2')[0].selectedIndex = 0;
                //$('#dlgAtender #Articulo_2').val('');
                $('#dlgAtender #Cuota_2').val('');
                $('#dlgAtender #Importe_2').val('');
                $('#dlgAtender #cboEstado_2')[0].selectedIndex = 0;
                $('#dlgAtender #cboResultado_2')[0].selectedIndex = 0;
                $('#dlgAtender #Respuesta_2').val('');

            },

            LimpiarAtenderN1: function () {
                $('#frmAtenderN1').form('clear');
                $('#dlgAtenderN1 #cboResultado_N1').combobox('clear').combobox('loadData', dataResultado);
                $('#dlgAtenderN1 #Respuesta_N1').val('');
            },


            InicializarListado: function () {

                var NroContrato = $('#NroContrato').val();
                var PersonalVentas = $('#PersonalVentas').val();
                var Estado = $.trim($('#cboEstado').combobox('getValue'));

                $('#DataGrid').datagrid({
                    url: project.ActionUrls.GetAllJson,
                    fitColumns: true,
                    idField: 'codigo_reclamo',
                    data: null,
                    queryParams: { nro_contrato: NroContrato, personal_ventas: PersonalVentas, codigo_estado: (!Estado ? '0' : Estado), codigo_perfil: $('#hdPerfil').val() },
                    pagination: true,
                    singleSelect: true,
                    rownumbers: true,
                    pageList: [20, 60, 80, 100, 150],
                    pageSize: 20,
                    columns:
                    [[
                        { field: 'codigo_reclamo', title: 'Código', align: 'center', haling: 'center' },
                        { field: 'PersonalVentas', title: 'Vendedor', width: 140, align: 'left', haling: 'center' },
                        { field: 'atencion_Empresa', title: 'Empresa', width: 45, align: 'left', haling: 'center' },
                        { field: 'NroContrato', title: 'Nro<br>Contrato', width: 55, align: 'center', haling: 'center' },
                        { field: 'atencion_Articulo', title: 'Art&iacute;culo', width: 130, align: 'left', haling: 'center' },
                        { field: 'atencion_Cuota', title: 'Cuota', width: 40, align: 'center', haling: 'center' },
                        {
                            field: 'atencion_Importe', title: 'Importe', width: 60, align: 'right', haling: 'center', formatter: function (value, row) {
                                return $.NumberFormat(value, 2);
                            }
                        },
                        //{ field: 'numero_planilla', title: 'Nro<br>Planilla', width: 60, align: 'center', haling: 'center' },
                        //{ field: 'error_contrato_migrado', title: 'Error<br>Contrato', width: 40, align: 'center', haling: 'center' },
                        { field: 'Estado', title: 'Estado', width: 60, align: 'center', haling: 'center' },
                        //{ field: 'nombre_estado_resultado_n1', title: 'Atencion<br>Comercial', width: 60, align: 'center', haling: 'center' },
                        {
                            field: 'nombre_estado_resultado_n2', title: 'Atención<br>Administrativo', width: 60, align: 'center', haling: 'center', styler: function (value, row, index) {
                                return row.estilo;
                            }
                        },
                        { field: 'estilo', hidden: true },
                    ]],
                    onClickRow: function (index, row) {
                        var rowColumn = row['codigo_reclamo'];
                        var rowEstado = row['Estado'];
                        $("#hdCodigo").val(rowColumn);
                        $("#hdEstado").val(rowEstado);
                    },
                    onDblClickRow: function (index, row) {
                        $('#btnVisualizar').click();
                    }
                });

                $('#DataGrid').datagrid('enableFilter');
                //$('#DataGrid').datagrid('loadData', {});
            },
            
            GetAllJson: function () {
                var NroContrato = $('#NroContrato').val();
                var PersonalVentas = $('#PersonalVentas').val();
                var Estado = $.trim($('#cboEstado').combobox('getValue'));

                var queryParams = {
                    nro_contrato: NroContrato,
                    personal_ventas: PersonalVentas,
                    codigo_estado: (!Estado ? '0' : Estado),
                    codigo_perfil: $('#hdPerfil').val()
                };

                $('#DataGrid').datagrid('reload', queryParams);
                $('#DataGrid').datagrid('clearSelections');
                $("#hdCodigo").val();
                $("#hdEstado").val();
            },
            
            ExisteContratoJson: function (empresa, contrato, personal) {
                
                $.ajax({
                    type: 'post',
                    url: project.ActionUrls.ValidarExisteContratoJson,
                    data: { empresa: empresa, contrato: contrato, personal: personal },
                    async: false,
                    cache: false,
                    dataType: 'json',
                    success: function (data) {
                        if (data.Msg) {
                            if (data.Msg == 'Success') {
                                $('#dlgSeleccionCuota').dialog('open').dialog('setTitle', 'BUSCAR ARTICULO');
                                $('#dlgSeleccionCuota').form('clear');

                                var parametros = { codigo_empresa: empresa, numero_contrato: contrato };

                                current.LimpiarGrillaContrato();
                                current.LimpiarSeleccionContrato();
                                current.GetEstadoContrato(parametros);

                                $('#dgv_analisis_contrato_articulo').datagrid("reload", parametros);
                            }
                            else {
                                //project.AlertErrorMessage('Contrato no existe', data.Msg);      //ico error
                                $.messager.alert('Reclamo', data.Msg, 'warning');
                            }
                        } else {
                            $.messager.alert('Reclamo', 'Error en proceso.', 'error');
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
                    data: { codigo: nodeId },
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
                                codigo_reclamo: data.codigo_reclamo,
                                codigo_personal: data.codigo_personal,
                                PersonalVentas: data.PersonalVentas,
                                NroContrato: data.NroContrato,
                                codigo_articulo: data.codigo_articulo,
                                Articulo: data.Articulo,
                                codigo_empresa: data.codigo_empresa,
                                Cuota: data.Cuota,
                                Importe: data.Importe,

                                atencion_codigo_articulo: data.atencion_codigo_articulo,
                                atencion_Articulo: data.atencion_Articulo,
                                atencion_codigo_empresa: data.atencion_codigo_empresa,
                                atencion_Cuota: data.atencion_Cuota,
                                atencion_Importe: data.atencion_Importe,

                                codigo_estado_reclamo: data.codigo_estado_reclamo,
                                codigo_estado_resultado: data.codigo_estado_resultado,
                                Observacion: data.Observacion,
                                Respuesta: data.Respuesta,
                                codigo_planilla: data.codigo_planilla,
                                numero_planilla: data.numero_planilla,
                                usuario_registra: data.usuario_registra,
                                fecha_registra: data.fecha_registra,
                                usuario_modifica: data.usuario_modifica,
                                fecha_modifica: data.fecha_modifica,
                                UsuarioAtencion: data.UsuarioAtencion,
                                UsuarioRegistra: data.UsuarioRegistra,
                                FechaAtencion: data.FechaAtencion,
                                es_contrato_migrado: data.es_contrato_migrado,
                                FechaRegistra: data.FechaRegistra,
                                nombre_empresa: data.nombre_empresa,
                                nombre_estado_reclamo: data.nombre_estado_reclamo,
                                nombre_estado_resultado_n1: data.nombre_estado_resultado_n1,
                                usuario_n1: data.usuario_n1,
                                observacion_n1: data.observacion_n1,
                                fecha_n1: data.fecha_n1,
                                nombre_estado_resultado_n2: data.nombre_estado_resultado_n2,
                                usuario_n2: data.usuario_n2,
                                observacion_n2: data.observacion_n2,
                                fecha_n2: data.fecha_n2,
                            };
                        }
                    },
                    error: function () {
                        project.AlertErrorMessage('Error', 'Error');
                    }
                });
            },
            
            Guardar: function () {
                if (current.EditType == 'Modificar') {
                    current.Modificar();
                } else if (current.EditType == 'Registrar') {
                    current.Registrar();
                }
            },

            Registrar: function () {

                var message = '';
                
                var codigo_personal = $.trim($('#dlgRegistrar #hdCodigoPersonal').val());
                var NroContrato = $.trim($('#dlgRegistrar #NroContrato').val());
                var codigo_empresa = $.trim($('#dlgRegistrar #cboEmpresa').combobox('getValue'));
                var codigo_articulo = $.trim($('#dlgRegistrar #hdCodigoArticulo').val());                
                var Cuota = $.trim($('#dlgRegistrar #Cuota').textbox('getText'));
                var Importe = $('#dlgRegistrar #Importe').numberbox('getValue');
                var Observacion = $.trim($('#dlgRegistrar #Observacion').textbox('getText'));
                var estadoContrato = $('#dlgSeleccionCuota #codigo_estado_proceso').val();
                var mensajeGuardar = "¿Seguro que desea registrar?";

                if (!estadoContrato) {
                    estadoContrato = 2;
                    mensajeGuardar += " No se ha validado el contrato / artículo ¿Desea registrar de reclamo?";
                }

                if (codigo_personal == '') {
                    message += "Vendedor, campo requerido.\n";
                }

                if (codigo_empresa == '') {
                    message += "Empresa, campo requerido.\n";
                }

                if (NroContrato == '') {
                    message += "Nro Contrato, campo requerido.\n";
                }

                if (estadoContrato == '3') //Contrato Procesado
                {
                    if (codigo_articulo == '') {
                        message += "Artículo, campo requerido. \n";
                    }
                    if (Cuota == '') {
                        message += "Cuota, campo requerido. \n";
                    }
                    else {
                        if (Cuota <= 0) {
                            message += "Cuota debe ser mayor a cero. \n";
                        }
                    }
                    if (Importe == '') {
                        message += "Importe, campo requerido. \n";
                    }
                    else {
                        if (Importe <= 0) {
                            message += "Importe debe ser mayor a cero. \n";
                        }
                    }
                }

                if (Observacion == '') {
                    message += "Observación, campo requerido. \n";
                }

                if (message.length > 0) {
                    $.messager.alert('Reclamo', message, 'warning');
                    return false;
                }

                if (estadoContrato == '3') //Contrato Procesado
                {
                    var ImporteOriginal = parseFloat($('#dlgRegistrar #ImporteOriginal').numberbox('getValue'));
                    Importe = parseFloat(Importe);

                    if (Importe < ImporteOriginal) {
                        $.messager.alert('Reclamo', 'El importe de reclamo debe ser mayor/igual a la cuota de pago comisión.', 'warning');
                        return false;
                    }
                }

                var mapData = {
                    codigo_personal: codigo_personal,
                    NroContrato: NroContrato,
                    codigo_empresa: codigo_empresa,
                    codigo_articulo: codigo_articulo,
                    Cuota: Cuota,
                    Importe: parseFloat(Importe),
                    Observacion: Observacion,
                    es_contrato_migrado: (estadoContrato == '2' ? 1 : 0)
                };

                $.messager.confirm('Confirm', mensajeGuardar, function (result) {
                    if (result) {
                        current.InsertarReclamo(mapData);
                    }
                });

            },

            InsertarReclamo: function (mapData) {
                $.ajax({
                    type: 'post',
                    url: project.ActionUrls.Registrar,
                    data: JSON.stringify({ v_entidad: mapData }),
                    dataType: 'json',
                    cache: false,
                    async: false,
                    contentType: 'application/json; charset=utf-8',
                    success: function (data) {
                        if (data.Tipo) {
                            if (data.Tipo == 'ERROR') {
                                project.AlertErrorMessage('Error', data.Msg);
                            }
                            else if (data.Tipo == 'ALERT') {
                                $.messager.alert('Reclamo', data.Msg, 'warning');
                            }
                            else if (data.Tipo == 'SUCCESS') {
                                $('#dlgRegistrar').dialog('close');

                                project.ShowMessage('Alerta', 'Registro Exitoso');

                                current.LimpiarRegistro();

                                $('#DataGrid').datagrid('reload');

                                current.EditType = '';
                            }
                            else {
                                $.messager.alert('Reclamo', 'Proceso no identificado', 'warning');
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
            },
            Modificar: function () {

                var message = '';

                var codigo_reclamo = $.trim($("#hdCodigo").val());
                var codigo_personal = $.trim($('#dlgAtender #hdCodigoPersonal').val());
                var NroContrato = $.trim($('#dlgAtender #NroContrato').textbox('getText'));
                var codigo_empresa = $.trim($('#dlgAtender #cboEmpresa_1').combobox('getValue'));
                var codigo_articulo = $.trim($('#dlgAtender #hdCodigoArticulo_1').val());
                var Cuota = $.trim($('#dlgAtender #Cuota_1').textbox('getText'));
                var Importe = $.trim($('#dlgAtender #Importe_1').textbox('getText'));

                var atencion_codigo_empresa = $.trim($('#dlgAtender #cboEmpresa_1').combobox('getValue'));//$.trim($('#dlgAtender #cboEmpresa_2').combobox('getValue'));
                var atencion_codigo_articulo = $.trim($('#dlgAtender #hdCodigoArticulo_1').val());//$.trim($('#dlgAtender #hdCodigoArticulo_2').val());
                var atencion_Cuota = $.trim($('#dlgAtender #Cuota_2').textbox('getText'));
                var atencion_Importe = $.trim($('#dlgAtender #Importe_2').textbox('getText'));

                var codigo_estado_reclamo = $.trim($('#dlgAtender #cboEstado_2').combobox('getValue'));
                var codigo_estado_resultado = $.trim($('#dlgAtender #cboResultado_2').combobox('getValue'));
                var Observacion = $.trim($('#dlgAtender #Observacion_1').textbox('getText'));
                var Respuesta = $.trim($('#dlgAtender #Respuesta_2').textbox('getText'));
                
                if (Cuota == '') {
                    message += "Cuota, campo requerido. \n"
                }
                if (atencion_Cuota == '') {
                    message += "Cuota atención, campo requerido. \n"
                }
                if (atencion_Importe == '') {
                    message += "Importe atención, campo requerido. \n"
                }
                if (atencion_codigo_articulo == '') {
                    message += "Articulo, campo requerido. \n"
                }
                if (codigo_estado_resultado == '') {
                    message += "Resultado, campo requerido. \n"
                }
                if (Respuesta == '') {
                    message += "Respuesta, campo requerido. \n"
                }

                if (message.length > 0) {
                    $.messager.alert('Reclamo', message, 'warning');
                }
                else {
                    if (codigo_estado_resultado == 2) {
                        //CASO 2: RECHAZADO
                        current.AtenderReclamo('RECHAZADO');
                    }
                    else {

                        //if ($('#dlgAtender #hdAtenderEsContratoMigrado').val() == '1') {
                        //    current.AtenderReclamoEsContratoMigrado();
                        //    return false;
                        //}

                        current.AtenderReclamo('NO_AFECTA_PLANILLA');
                    }
                    
                }
            },
            AtenderReclamo: function (TipoAfectaPlanilla) {
                var nombreOpcion = "Atenci&oacute;n Reclamo";
                var codigo_reclamo = $.trim($("#hdCodigo").val());
                var codigo_personal = $.trim($('#dlgAtender #hdCodigoPersonal').val());
                var NroContrato = $.trim($('#dlgAtender #NroContrato').textbox('getText'));
                var codigo_empresa = $.trim($('#dlgAtender #cboEmpresa_1').combobox('getValue'));
                var codigo_articulo = $.trim($('#dlgAtender #hdCodigoArticulo_1').val());
                var Cuota = $.trim($('#dlgAtender #Cuota_1').textbox('getText'));
                var Importe = $.trim($('#dlgAtender #Importe_1').textbox('getText'));

                var atencion_codigo_empresa = $.trim($('#dlgAtender #cboEmpresa_1').combobox('getValue'));//$.trim($('#dlgAtender #cboEmpresa_2').combobox('getValue'));
                var atencion_codigo_articulo = $.trim($('#dlgAtender #hdCodigoArticulo_1').val());//$.trim($('#dlgAtender #hdCodigoArticulo_2').val());
                var atencion_Cuota = $.trim($('#dlgAtender #Cuota_2').textbox('getText'));
                var atencion_Importe = $.trim($('#dlgAtender #Importe_2').textbox('getText'));

                var codigo_estado_reclamo = $.trim($('#dlgAtender #cboEstado_2').combobox('getValue'));
                var codigo_estado_resultado = $.trim($('#dlgAtender #cboResultado_2').combobox('getValue'));
                var Observacion = $.trim($('#dlgAtender #Observacion_1').textbox('getText'));
                var Respuesta = $.trim($('#dlgAtender #Respuesta_2').textbox('getText'));

                var mapData = {
                    codigo_reclamo: codigo_reclamo,
                    codigo_personal: codigo_personal,
                    NroContrato: NroContrato,
                    codigo_articulo: codigo_articulo,
                    codigo_empresa: codigo_empresa,
                    Cuota: Cuota,
                    Importe: Importe,

                    atencion_codigo_articulo: atencion_codigo_articulo,
                    atencion_codigo_empresa: atencion_codigo_empresa,
                    atencion_Cuota: atencion_Cuota,
                    atencion_Importe: parseFloat(atencion_Importe),

                    codigo_estado_reclamo: codigo_estado_reclamo,
                    codigo_estado_resultado: codigo_estado_resultado,
                    Observacion: Observacion,
                    Respuesta: Respuesta,
                    TipoAfectaPlanilla: TipoAfectaPlanilla,
                };
                $.ajax({
                    type: 'post',
                    url: project.ActionUrls.Modificar,
                    data: JSON.stringify({ v_entidad: mapData }),
                    dataType: 'json',
                    cache: false,
                    async: false,
                    contentType: 'application/json; charset=utf-8',
                    success: function (data) {
                        if (data.Msg) {
                            if (data.Msg != 'Success') {
                                project.AlertErrorMessage('Error', data.Msg);
                            }
                            else {
                                $('#dlgAtender').dialog('close');
                                current.CerrarAtencion();
                                current.ModificarNotificacionPendientes();
                                $.messager.alert(nombreOpcion, 'Se actualizó la atención.', 'warning');
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
            },

            AtenderN1: function () {
                var nombreOpcion = "Atenci&oacute;n Reclamo";
                var message = '';

                var codigo_estado_resultado = $.trim($('#dlgAtenderN1 #cboResultado_N1').combobox('getValue'));
                var respuesta = $.trim($('#dlgAtenderN1 #Respuesta_N1').textbox('getText'));

                if (codigo_estado_resultado == '') {
                    message += "Resultado, campo requerido. \n";
                }

                if (respuesta == '') {
                    message += "Respuesta, campo requerido. \n";
                }

                if (message.length > 0) {
                    $.messager.alert(nombreOpcion, message, 'warning');
                }
                else {
                    var mapData = {
                        codigo_reclamo: $.trim($("#hdCodigo").val()),
                        codigo_estado_resultado: codigo_estado_resultado,
                        observacion: respuesta
                    };

                    $.ajax({
                        type: 'post',
                        url: project.ActionUrls.AtenderN1,
                        data: JSON.stringify({ v_entidad: mapData }),
                        async: false,
                        cache: false,
                        dataType: 'json',
                        contentType: 'application/json; charset=utf-8',
                        success: function (data) {
                            if (data.Msg) {
                                if (data.Msg == 'Success') {
                                    $('#frmAtenderN1').form('clear');
                                    $('#dlgAtenderN1').dialog('close');
                                    current.CerrarAtencionN1();
                                    current.ModificarNotificacionPendientes();
                                    $.messager.alert(nombreOpcion, 'Se actualizó la atención.', 'warning');
                                }
                                else {
                                    $.messager.alert(nombreOpcion, data.Msg, 'warning');
                                }
                            }
                            else {
                                $.messager.alert(nombreOpcion, 'Error Crítico.', 'warning');
                            }
                        },
                        error: function () {
                            project.AlertErrorMessage('Error', 'Error');
                        }
                    });
                }

            },

            Redimensionar: function () {
                var mediaquery = window.matchMedia("(max-width: 600px)");
                var ancho = mediaquery.matches?'85%':'65%';

                $('#dlgRegistrar').dialog('resize', { width: ancho });
                $('#dlgRegistrar').window('center');
            },

            InicializarGrillaPersonal: function () {

                $('#dlgBusquedaPersona #dgPersonas').datagrid({
                    //shrinkToFit: false,
                    url: project.ActionUrls.GetPersonalByNombreJson,
                    queryParams: { texto: '-1' },
                    fitColumns: true,
                    idField: 'codigo_personal',
                    singleSelect: true,
                    rownumbers: true,
                    pageList: [20, 60, 80, 100, 150],
                    pageSize: 20,
                    pagination: true,
                    columns:
                    [[
                        { field: 'codigo_personal', hidden: true },
                        { field: 'codigo_equivalencia', title: 'Código', width: 90, align: 'center' },
                        { field: 'nombre_personal', title: 'Nombres', width: 250, align: 'left' },
                        { field: 'nombre_canal', title: 'Canal', width: 150, align: 'left' },
                        { field: 'nombre_grupo', title: 'Grupo', width: 150, align: 'left' },
                    ]],
                    onClickRow: function (index, row) {
                        nombrePersonal = row['nombre_personal'];//row['nombre'] + ' ' + row['apellido_paterno'] + ' ' + row['apellido_materno'];
                        codigoPersonal = row['codigo_personal'];
                    }
                    ,
                    onDblClickRow: function (index, row) {
                        current.SeleccionarPersona();
                    }
                });
                $('#dlgBusquedaPersona #dgPersonas').datagrid('enableFilter');
            },

            LimpiarGrillaPersonal: function () {
                $('#dgPersonas').datagrid('removeFilterRule', 'codigo_equivalencia');
                $('#dgPersonas').datagrid('removeFilterRule', 'nombre_personal');
                $('#dgPersonas').datagrid('removeFilterRule', 'nombre_canal');
                $('#dgPersonas').datagrid('removeFilterRule', 'nombre_grupo');
                //$('#dgPersonas').datagrid('doFilter');

                $('#dgPersonas').datagrid('clearSelections');

                var queryParams = { texto: ' ' };
                $('#dgPersonas').datagrid('reload', queryParams);

            },

            InicializarGrillaContrato: function () {
                $('#dgv_analisis_contrato_articulo').datagrid({
                    //fitColumns: true,
                    url: project.ActionUrls._GetArticulosByContratoEmpresaJson,
                    pagination: false,
                    singleSelect: true,
                    remoteFilter: false,
                    //height: 200,
                    rownumbers: true,
                    //queryParams: {
                    //    codigo_empresa: ActionContratoUrl._codigo_empresa,
                    //    numero_contrato: ActionContratoUrl._numero_contrato
                    //},
                    columns:
                   [[

                         { field: "codigo_empresa", title: "codigo_empresa", hidden: true, rowspan: "2" },
                         { field: "codigo_moneda", title: "codigo_moneda", hidden: true, rowspan: "2" },
                         { field: "codigo_articulo", title: "codigo_articulo", hidden: true, rowspan: "2" },
                         { field: "numero_contrato", title: "numero_contrato", hidden: true, rowspan: "2" },


                        { field: "nombre_articulo", title: "Artículo", width: "250", rowspan: "2", align: "left", halign: "center" },
                        { field: "cantidad_articulo", title: "Cantidad", width: "80", rowspan: "2", align: "center", halign: "center" },
                        { field: "nombre_moneda", title: "Moneda", width: "80", rowspan: "2", align: "left", halign: "center" },
                         {
                             field: "monto_comision_inicial_total", title: "Comisión <br>Total", width: "90", rowspan: "2", align: "right", halign: "center", formatter: function (value, row, index) {
                                 return $.NumberFormat(value, 2);
                             }
                         },
                        { title: "Supervisor", colspan: 5 },
                        { title: "Vendedor", colspan: 5 }
                   ], [
                        {
                            field: "monto_comision_inicial_supervisor", title: "Comisión <br>Inicial", halign: "center", align: "right", width: "80", formatter: function (value, row, index) {
                                return $.NumberFormat(value, 2);
                            }
                        },
                       {
                           field: "monto_total_comision_supervisor", title: "Comisión", halign: "center", align: "right", width: "80", formatter: function (value, row, index) {
                               return $.NumberFormat(value, 2);
                           }
                       },
                        {
                            field: "monto_total_pagado_supervisor", title: "Pagado", halign: "center", align: "right", width: "80", formatter: function (value, row, index) {
                                return $.NumberFormat(value, 2);
                            }
                        },
                        {
                            field: "monto_total_saldo_supervisor", halign: "center", align: "right", title: "Saldo", width: "80", formatter: function (value, row, index) {
                                return $.NumberFormat(value, 2);
                            }
                        },
                        {
                            //field: "monto_total_excluido_supervisor", title: "Exclusión", halign: "center", align: "right", width: "80", formatter: function (value, row, index) {
                            //    return $.NumberFormat(value, 2);
                            //}
                            field: "anulacion_supervisor", title: "Anulación", halign: "center", align: "left", width: "120"
                        },

                          {
                              field: "monto_comision_inicial_personal", title: "Comisión<br> Inicial", halign: "center", align: "right", width: "80", formatter: function (value, row, index) {
                                  return $.NumberFormat(value, 2);
                              }

                          },
                        {
                            field: "monto_total_comision_vendedor", title: "Comisión", halign: "center", align: "right", width: "80", formatter: function (value, row, index) {
                                return $.NumberFormat(value, 2);
                            }

                        },
                        {
                            field: "monto_total_pagado_vendedor", title: "Pagado", halign: "center", align: "right", width: "80", formatter: function (value, row, index) {
                                return $.NumberFormat(value, 2);
                            }
                        },
                        {
                            field: "monto_total_saldo_vendedor", title: "Saldo", width: "80", halign: "center", align: "right", formatter: function (value, row, index) {
                                return $.NumberFormat(value, 2);
                            }
                        },
                        {
                            field: "anulacion_vendedor", title: "Anulación", halign: "center", align: "left", width: "120"
                            //field: "monto_total_excluido_vendedor", title: "Exclusión", halign: "center", align: "right", width: "80", formatter: function (value, row, index) {
                            //    return $.NumberFormat(value, 2);
                            //}
                        }
                   ]],
                    onClickRow: function (index, row) {
                        var v_entidad = {
                            numero_contrato: row.numero_contrato,
                            codigo_empresa: row.codigo_empresa,
                            codigo_articulo: row.codigo_articulo,
                            codigo_moneda: row.codigo_moneda
                        };
                        codigoArticulo = row.codigo_articulo;
                        nombreArticulo = row.nombre_articulo;
                        numeroCuota = '';
                        montoCuota = '';
                        estadoCuota = '';
                        $('#dgv_contrato_detalle_pago').datagrid("reload", v_entidad);
                    }
                });

                var dg = $('#dgv_analisis_contrato_articulo');
                dg.datagrid();

                var columnas = "monto_comision_inicial_supervisor,monto_total_comision_supervisor,monto_total_pagado_supervisor,monto_total_saldo_supervisor,anulacion_supervisor, \
                    |monto_comision_inicial_personal,monto_total_comision_vendedor,monto_total_pagado_vendedor,monto_total_saldo_vendedor,anulacion_vendedor".split('|');
                var color = "#94DE82,#E49450".split(',');

                for (indice = 0; indice <= columnas.length - 1; indice++) {
                    var cols = columnas[indice].split(',');
                    for (indice2 = 0; indice2 <= cols.length - 1; indice2++) {
                        dg.datagrid('getPanel').find('div.datagrid-header td[field="' + cols[indice2] + '"]').css('background-color', color[indice]);
                    }
                }

                //$('#dgv_analisis_contrato_articulo').datagrid('enableFilter');

                $('#dgv_contrato_detalle_pago').datagrid({
                    //fitColumns: true,
                    rownumbers: true,
                    data: null,
                    //height: 300,
                    url: project.ActionUrls._GetDetalleCronogramaPagoByArticuloJson,
                    pagination: false,
                    singleSelect: true,
                    remoteFilter: false,
                    columns:
                    [[
                        { field: 'codigo_estado_cuota', width: "0", hidden: true },
                        { field: 'nombre_tipo_planilla', title: 'Tipo Planilla', width: "150", halign: 'center', align: 'left' },
                        { field: 'numero_planilla', title: 'Nro</br>Planilla', width: "100", halign: 'center', align: 'center' },
                        { field: 'str_fecha_cierre', title: 'Fecha Cierre</br>Planilla', width: "100", halign: 'center', align: 'center' },
                        { field: 'nro_cuota', title: 'Nro.</br> Cuota', width: "80", halign: 'center', align: 'left' },
                        { field: 'str_fecha_programada', title: 'Fecha </br>Habilitado', width: "100", halign: 'center', align: 'center' },
                        {
                            field: 'importe_sing_igv', title: 'Imp. </br>Sin IGV', width: "120", halign: 'center', align: 'right', formatter: function (value, row, index) {
                                return $.NumberFormat(value, 2);
                            }
                        },
                        {
                            field: 'igv', title: 'IGV', width: "120", halign: 'center', align: 'right', formatter: function (value, row, index) {
                                return $.NumberFormat(value, 2);
                            }
                        },
                        {
                            field: 'importe_comision', title: 'Monto a</br> Pagar', width: "120", halign: 'center', align: 'right', formatter: function (value, row, index) {
                                return $.NumberFormat(value, 2);
                            }
                        },
                        { field: 'str_fecha_exclusion', title: 'Fecha </br> Exclusión', width: "100", halign: 'center', align: 'center' },
                        { field: 'str_fecha_anulado', title: 'Fecha </br> Anulación', width: "100", halign: 'center', align: 'center' },
                        { field: 'nombre_estado_cuota', title: 'Estado', width: "200", halign: 'center', align: 'left' },
                        { field: 'observacion', title: 'Observación', width: "250", halign: 'center', align: 'left' }
                    ]]
                    ,onClickRow: function (index, row) {
                        numeroCuota = row.nro_cuota;
                        montoCuota = row.importe_comision;
                        estadoCuota = row.codigo_estado_cuota;
                    }
                    , onDblClickRow: function (index, row) {
                        current.SeleccionarCuota();
                    }
                });
                //$('#dgv_contrato_detalle_pago').datagrid('enableFilter');
            },

            LimpiarGrillaContrato: function () {
                var registros = $('#dgv_contrato_detalle_pago').datagrid("getRows");
                if (registros.length > 0) {
                    $('#dgv_contrato_detalle_pago').datagrid('clearSelections');
                    $('#dgv_contrato_detalle_pago').datagrid('loadData', { "total": 0, "rows": [] });
                }

                registros = $('#dgv_analisis_contrato_articulo').datagrid("getRows");
                if (registros.length > 0) {
                    $('#dgv_analisis_contrato_articulo').datagrid('clearSelections');
                    $('#dgv_analisis_contrato_articulo').datagrid('loadData', { "total": 0, "rows": [] });
                }
            },

            LimpiarSeleccionContrato: function () {
                codigoArticulo = '';
                nombreArticulo = '';

                numeroCuota = '';
                montoCuota = '';
                estadoCuota = '';
            },

            GetEstadoContrato: function (parametros) {
                current.LimpiarEstadoContrato();

                $.ajax({
                    type: 'post',
                    url: project.ActionUrls.GetEstadoContratoJson,
                    data: { codigo_empresa: parametros.codigo_empresa, nro_contrato: parametros.numero_contrato },
                    async: false,
                    cache: false,
                    dataType: 'json',
                    success: function (data) {
                        if (data.Msg) {
                            project.AlertErrorMessage('Error', data.Msg);
                        }
                        else {
                            $('#dlgSeleccionCuota #codigo_estado_proceso').val(data.codigo_estado_proceso);
                            $('#dlgSeleccionCuota #nombre_estado_proceso').textbox('setText', data.nombre_estado_proceso);
                            $('#dlgSeleccionCuota #observacion').textbox('setText', data.observacion);

                            if (data.codigo_estado_proceso == 2) {
                                $('#divSeleccionCuota').hide();
                                $('#divSeleccionContrato').show();
                            }
                            else {
                                $('#divSeleccionCuota').show();
                                $('#divSeleccionContrato').hide();
                            }
                        }
                    },
                    error: function () {
                        project.AlertErrorMessage('Error', 'Error');
                    }
                });
            },

            LimpiarEstadoContrato: function () {
                $('#dlgSeleccionCuota #codigo_estado_proceso').val('');
                $('#dlgSeleccionCuota #nombre_estado_proceso').textbox('setText', '');
                $('#dlgSeleccionCuota #observacion').textbox('setText', '');
            },

            LimpiarSeleccionArticulo: function () {
                $('#dlgRegistrar #hdCodigoArticulo').val('');
                $('#dlgRegistrar #Articulo').textbox('setText', '');
                $('#dlgRegistrar #Cuota').textbox('setText', '');
                $('#dlgRegistrar #Importe').numberbox('setValue', '');
                $('#dlgRegistrar #ImporteOriginal').numberbox('setValue', '');
                $('#dlgRegistrar #error_contrato_migrado').textbox('setText', '');
            },
 
            AtenderReclamoEsContratoMigrado: function (TipoAfectaPlanilla) {
                var codigo_reclamo = $.trim($("#hdCodigo").val());
                var respuesta = $.trim($('#dlgAtender #Respuesta_2').textbox('getText'));
                //var codigo_estado_resultado = $.trim($('#dlgAtender #cboResultado_2').combobox('getValue'));

                $.ajax({
                    type: 'post',
                    url: project.ActionUrls.AtenderContratoMigrado,
                    data: { codigo_reclamo: codigo_reclamo, respuesta_atencion: respuesta },
                    async: false,
                    cache: false,
                    dataType: 'json',
                    success: function (data) {
                        if (data.Msg) {
                            if (data.Msg != 'Success') {
                                project.AlertErrorMessage('Error', data.Msg);
                            }
                            else {
                                $('#dlgAtender').dialog('close');
                                project.ShowMessage('Reclamo', 'Se volverá a procesar el contrato.');

                                current.CerrarAtencion();
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

            },

            CerrarAtencion: function () {
                current.LimpiarAtender();
                $('#DataGrid').datagrid('clearSelections');
                $('#DataGrid').datagrid('reload');
                current.EditType = '';
            },

            CerrarAtencionN1: function () {
                //current.LimpiarAtender();
                $('#frmAtenderN1').form('clear');
                $('#dlgAtenderN1').dialog('close');
                $('#DataGrid').datagrid('clearSelections');
                $('#DataGrid').datagrid('reload');
                //current.EditType = '';
            },

            SeleccionarPersona: function () {
                if (codigoPersonal == '') {
                    $.messager.alert('Reclamo', 'No ha seleccionado un Vendedor.', 'warning');
                }
                else {
                    $('#dlgRegistrar #PersonalVentas').textbox('setText', nombrePersonal);
                    $('#dlgRegistrar #hdCodigoPersonal').val(codigoPersonal);

                    $('#dlgBusquedaPersona').dialog('close');

                    codigoPersonal = '';
                    nombrePersonal = '';

                    current.LimpiarGrillaPersonal();
                    current.LimpiarSeleccionArticulo();
                }
            },

            CargarDataTemporal: function (url) {
                var temporal = {};
                $.ajax({
                    type: 'post',
                    url: url,
                    data: null,
                    async: false,
                    cache: false,
                    dataType: 'json',
                    success: function (data) {
                        temporal = data;
                    },
                    error: function () {
                        temporal = {};
                    }
                });
                return temporal;
            },

            SeleccionarCuota: function () {
                var estadoContrato = $('#dlgSeleccionCuota #codigo_estado_proceso').val();

                if (estadoContrato == '1')//Contrato No Procesado
                {
                    $.messager.alert('Reclamo', 'El contrato indicado no ha sido procesado para comisión.', 'warning');
                    return false;
                }

                if (estadoContrato == '3')//Contrato Procesado
                {
                    if (codigoArticulo == '') {
                        $.messager.alert('Reclamo', 'No ha seleccionado un Articulo.', 'warning');
                        return false;
                    }

                    if (numeroCuota == '') {
                        $.messager.alert('Reclamo', 'No ha seleccionado una cuota.', 'warning');
                        return false;
                    }

                    if (estadoCuota == '4' || estadoCuota == '5') {
                        $.messager.alert('Reclamo', 'La cuota seleccionada no puede ser reclamada.', 'warning');
                        return false;
                    }
                }

                if (current.EditType == 'Modificar') {
                    $('#dlgAtender #Articulo_2').textbox('setText', nombreArticulo);
                    $('#dlgAtender #hdCodigoArticulo_2').val(codigoArticulo);
                } else if (current.EditType == 'Registrar') {

                    if (estadoContrato == '2') { //Contrato con error
                        $('#dlgRegistrar #Articulo').textbox('setText', 'TODO EL CONTRATO POR ERROR');
                        $('#dlgRegistrar #hdCodigoArticulo').val(0);
                        $('#dlgRegistrar #Cuota').textbox('setText', 0);
                        $('#dlgRegistrar #ImporteOriginal').numberbox('setValue', 0.00);
                        $('#dlgRegistrar #Importe').numberbox('setValue', 0.00);
                        $('#dlgRegistrar #Importe').textbox('readonly', true);
                        $('#dlgRegistrar #error_contrato_migrado').textbox('setText', 'Si');
                    }
                    else {
                        $('#dlgRegistrar #Articulo').textbox('setText', nombreArticulo);
                        $('#dlgRegistrar #hdCodigoArticulo').val(codigoArticulo);
                        $('#dlgRegistrar #Cuota').textbox('setText', numeroCuota);
                        $('#dlgRegistrar #ImporteOriginal').numberbox('setValue', montoCuota);
                        $('#dlgRegistrar #Importe').numberbox('setValue', montoCuota);
                        $('#dlgRegistrar #Importe').textbox('readonly', false);
                        $('#dlgRegistrar #error_contrato_migrado').textbox('setText', 'No');
                    }
                }

                $('#frmSeleccionCuota').form('clear');
                $('#dlgSeleccionCuota').dialog('close');
                current.LimpiarGrillaContrato();

            },

            ModificarNotificacionPendientes: function () {
                $.ajax({
                    type: 'post',
                    url: project.ActionUrls.ModificarNotificacionPendientes,
                    data: null,
                    async: false,
                    cache: false,
                    dataType: 'json',
                    success: function (data) {
                        if (parseInt(data.cantidad, 10) > 0) {
                            $("#div_cantidad_pendientes").show();
                            $("#enlace_cantidad_pendientes").attr('title', data.cantidad);
                            $("#texto_cantidad_pendientes").text(String.fromCharCode(160) + String.fromCharCode(160) + String.fromCharCode(160) + data.cantidad);
                        }
                        else {
                            $("#div_cantidad_pendientes").hide();
                        }
                    },
                    error: function () {
                        temporal = {};
                    }
                });
            },

        });

})
(project);
