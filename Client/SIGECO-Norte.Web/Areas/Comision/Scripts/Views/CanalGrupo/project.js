;
(function (app) {
    //===========================================================================================
    var current = app.CanalGrupo = {};
    //===========================================================================================

    var JsonCanal = {};
    var JsonGrupo = {};
    var JsonEmpresa = {};
    var LongitudGrupo = 0;
    var RegistroTransferencia = {};
    var CodigoPersonalOriginal = '';
    var NombrePersonalOriginal = '';
    var FechaRegistraOriginal = '';
    var UsuarioRegistraOriginal = '';
    var CantidadSupervisiones = 0;
    var CanalGrupoAccion = '0';
    var PercibeBono = '0';
    var PercibeComision = '0';
    var Activo = true;

    jQuery.extend(app.CanalGrupo,
        {

            ActionUrls: {},
            EditTypes: { Registrar: 'Registrar', Modificar: 'Modificar', Ninguno:'' },
            EditType: '',
            codigoPersonal: '',
            nombrePersonal: '',
            fechaRegistra: '',
            usuarioRegistra: '',
            personal_tmp: [],
            accionSupervisor: 0,
            cargoDisponible: '1',
            dataYaAsignado: {},
            personalRemovido: [],
            esNuevo: '',

            Initialize: function (actionUrls) {
                jQuery.extend(project.ActionUrls, actionUrls);
                current.Initialize_DataGrid();

                JsonCanal = $('.content').get_json_combobox({
                    url: project.ActionUrls.GetCanalGrupoJson,
                    parametro: "?es_canal_grupo=1"
                });

                JsonGrupo = $('.content').get_json_combobox({
                    url: project.ActionUrls.GetCanalGrupoJson,
                    parametro: "?es_canal_grupo=0"
                });
                
                JsonEmpresa = $('.content').get_json_combobox({
                    url: project.ActionUrls.GetEmpresaJson
                });

                current.Initialize_ComboBox();

                current.Redimensionar();

                $(window).resize(function () {
                    current.Redimensionar();
                });
                
                $('#btnNuevo').click(function () {
                    Activo = true;
                    current.EditType = current.EditTypes.Registrar;
                    current.InicializarGrillas();
                    cargoDisponible = '1';
                    $('#dlgRegistrar').dialog('open').dialog('setTitle', 'NUEVO REGISTRO');
                    current.InicializarFormulario();
                });

                $('#btnRefrescar').click(function () {
                    current.RecargarGrilla();
                });

                $('#dlgRegistrar #btnGuardar').click(function () {
                    if (!Activo)
                    {
                        $.messager.alert('Canal', 'No se guardar por ser un registro inactivo.', 'warning');
                        return false;
                    }
                    current.Guardar();
                });

                $('#dlgRegistrar #btnCancelar').click(function () {
                    current.InicializarGrillas();
                    $('#frmRegistrar').form('clear');
                    $('#dlgRegistrar').dialog('close');
                });

                $('#btnModificar').click(function () {
                    var codigo = $("#hdCodigo").val();

                    if (current.ValidarSeleccion(codigo) && current.ObtenerRegistro(codigo)) {
                        Activo = Registro.estado_registro;

                        if (!Activo)
                        {
                            $('#dlgRegistrar #btnGuardar').hide();
                        }

                        current.EditType = current.EditTypes.Modificar;
                        current.InicializarGrillas();
                        $('#dlgRegistrar').dialog('open').dialog('setTitle', 'MODIFICAR REGISTRO');
                        current.InicializarFormulario();

                        $('#dlgRegistrar #nombre').textbox('setText', Registro.nombre);
                        $('#dlgRegistrar #administraGrupos').switchbutton({ checked: Registro.administra_grupos });

                        var supervisorComisiona = (Registro.s_percibe_comision == '1');
                        $('#dlgRegistrar #comisionSupervisor ').switchbutton({ checked: supervisorComisiona });
                        if (supervisorComisiona)
                        {
                            if (Registro.s_c_empresa_planilla != '')
                            {
                                $('#dlgRegistrar #planillaComisionSupervisor ').switchbutton({ checked: true });
                                $('#dlgRegistrar #empresaPlanillaComisionSupervisor ').combobox('setValues', Registro.s_c_empresa_planilla.replace(/\./g, ','))
                            }
                            if (Registro.s_c_empresa_factura != '') {
                                $('#dlgRegistrar #facturaComisionSupervisor').switchbutton({ checked: true });
                                $('#dlgRegistrar #empresaFacturaComisionSupervisor').combobox('setValues', Registro.s_c_empresa_factura.replace(/\./g, ','))
                            }
                        }

                        var supervisorBono = (Registro.s_percibe_bono == '1');
                        $('#dlgRegistrar #bonoSupervisor').switchbutton({ checked: supervisorBono });
                        if (supervisorBono) {
                            if (Registro.s_b_empresa_planilla != '') {
                                $('#dlgRegistrar #planillaBonoSupervisor ').switchbutton({ checked: true });
                                $('#dlgRegistrar #empresaPlanillaBonoSupervisor ').combobox('setValues', Registro.s_b_empresa_planilla.replace(/\./g, ','))
                            }
                            if (Registro.s_b_empresa_factura != '') {
                                $('#dlgRegistrar #facturaBonoSupervisor').switchbutton({ checked: true });
                                $('#dlgRegistrar #empresaFacturaBonoSupervisor').combobox('setValues', Registro.s_b_empresa_factura.replace(/\./g, ','))
                            }
                        }

                        var personalComisiona = (Registro.p_percibe_comision == '1');
                        $('#dlgRegistrar #comisionPersonal').switchbutton({ checked: personalComisiona });
                        if (personalComisiona) {
                            if (Registro.p_c_empresa_planilla != '') {
                                $('#dlgRegistrar #planillaComisionPersonal ').switchbutton({ checked: true });
                                $('#dlgRegistrar #empresaPlanillaComisionPersonal ').combobox('setValues', Registro.p_c_empresa_planilla.replace(/\./g, ','))
                            }
                            if (Registro.p_c_empresa_factura != '') {
                                $('#dlgRegistrar #facturaComisionPersonal').switchbutton({ checked: true });
                                $('#dlgRegistrar #empresaFacturaComisionPersonal').combobox('setValues', Registro.p_c_empresa_factura.replace(/\./g, ','))
                            }
                        }

                        var personalBono = (Registro.p_percibe_bono == '1');
                        $('#dlgRegistrar #bonoPersonal').switchbutton({ checked: personalBono });
                        if (personalBono) {
                            if (Registro.p_b_empresa_planilla != '') {
                                $('#dlgRegistrar #planillaBonoPersonal ').switchbutton({ checked: true });
                                $('#dlgRegistrar #empresaPlanillaBonoPersonal ').combobox('setValues', Registro.p_b_empresa_planilla.replace(/\./g, ','))
                            }
                            if (Registro.p_b_empresa_factura != '') {
                                $('#dlgRegistrar #facturaBonoPersonal').switchbutton({ checked: true });
                                $('#dlgRegistrar #empresaFacturaBonoPersonal').combobox('setValues', Registro.p_b_empresa_factura.replace(/\./g, ','))
                            }
                        }

                        $('#dlgRegistrar #tieneSupervisor').switchbutton({ checked: false });
                        current.CargarGrillasPersonalAsignado(codigo, true);
                        if (CodigoPersonalOriginal != '')
                        {
                            current.GetOtrasSupervisiones(CodigoPersonalOriginal, codigo);
                        }
                    }
                });

                $('#btnEliminar').click(function () {
                    var codigo = $("#hdCodigo").val();
                    if (current.ValidarSeleccion(codigo)) {
                        current.Eliminar(codigo);
                    }
                });

                $('#dlgRegistrar #btnBuscarPersona').click(function () {
                    if (!Activo)
                    {
                        $.messager.alert('Canal', 'No se puede configurar por ser un registro inactivo.', 'warning');
                        return false;
                    }

                    var tieneSupervisor = $('#dlgRegistrar #tieneSupervisor').switchbutton('options').checked;
                    var administraGrupos = $('#dlgRegistrar #administraGrupos').switchbutton('options').checked;
                    var message = '';

                    if (!tieneSupervisor) {
                        $.messager.alert('Canal', 'No ha configurado que este Canal pueda tener Supervisor.', 'warning');
                        return false;
                    }

                    //if (administraGrupos) {
                    //    $.messager.alert('Canal', 'No debe configurar Supervisor si Administra Grupos.', 'warning');
                    //    return false;
                    //}

                    personal_tmp[0] = '';
                    personal_tmp[1] = '';
                    personal_tmp[2] = '';
                    personal_tmp[3] = '';   

                    $('#dlgBusquedaPersona').dialog('open').dialog('setTitle', 'BUSCAR VENDEDOR');
                    $('#dlgBusquedaPersona').form('clear');
                });

                $('#dlgBusquedaPersona #nombre').textbox({
                    onClickButton: function () {
                        var nombre = $.trim($('#dlgBusquedaPersona #nombre').textbox('getText'));
                        current.GetPersonaByNombreJson(nombre);
                    }
                });

                $('#dlgBusquedaPersonaBotones #btnCancelar').click(function () {
                    personal_tmp[0] = '';
                    personal_tmp[1] = '';
                    personal_tmp[2] = '';
                    personal_tmp[3] = '';
                    $('#frmBusquedaPersona').form('clear');
                    $('#dlgBusquedaPersona').dialog('close');
                    $('#dlgBusquedaPersona #dgPersonas').datagrid('clearSelections');
                });

                $('#dlgBusquedaPersonaBotones #btnSeleccionar').click(function () {
                    current.SeleccionarPersona();
                });

                $('#dlgRegistrar #btnTransferir').click(function () {
                    if (!Activo) {
                        $.messager.alert('Canal', 'No se puede configurar por ser un registro inactivo.', 'warning');
                        return false;
                    }

                    RegistroTransferencia = $('#dgPersonalYaAsignado').datagrid('getSelected');

                    if (!RegistroTransferencia) {
                        $.messager.alert('Transferencia', 'No ha seleccionado a vendedor.', 'warning');
                        return false;
                    }

                    if (RegistroTransferencia.es_supervisor) {
                        $.messager.alert('Transferencia', 'No puede usar esta opcion en el supervisor.', 'warning');
                        return false;
                    }

                    $('#dlgTransferir #codigo_canal').combobox('clear').combobox('loadData', JsonCanal);
                    $('#dlgTransferir').dialog('open').dialog('setTitle', 'TRANSFERENCIA: ' + RegistroTransferencia.nombre);
                    $('#dlgTransferir').form('clear');
                });

                $('#divTransferirBotones #btnAceptar').click(function () {
                    var codigo_canal_t = $.trim($('#dlgTransferir #codigo_canal').combobox('getValue'));
                    var codigo_grupo_t = $.trim($('#dlgTransferir #codigo_grupo').combobox('getValue'));
                    var codigo_old = $.trim($('#hdCodigo').val());
                    var codigo_canal_grupo = '';

                    var percibe_comision = $('#dlgTransferir #percibe_comision').switchbutton('options').checked;
                    var percibe_bono = $('#dlgTransferir  #percibe_bono').switchbutton('options').checked;

                    if (!codigo_canal_t)
                    {
                        $.messager.alert('Transferencia', 'Debe seleccionar un Canal.', 'warning');
                        return false;
                    }

                    if (!codigo_grupo_t && LongitudGrupo > 0)
                    {
                        $.messager.alert('Transferencia', 'Debe seleccionar un Grupo.', 'warning');
                        return false;
                    }

                    if (codigo_canal_t == codigo_old || codigo_grupo_t == codigo_old) {
                        $.messager.alert('Transferencia', 'No puede seleccionar el mismo Canal/Grupo para tranferir.', 'warning');
                        return false;
                    }

                    if (codigo_grupo_t != '')
                    {
                        codigo_canal_grupo = codigo_grupo_t
                    }
                    else
                    {
                        codigo_canal_grupo = codigo_canal_t
                    }

                    current.Transferir(codigo_old, 1, RegistroTransferencia.codigo_personal, codigo_canal_grupo, percibe_comision, percibe_bono);
                    current.CargarGrillasPersonalAsignado(codigo_old, false);
                    $('#dgPersonalYaAsignado').datagrid('clearSelections');
                    $('#frmTransferir').form('clear');
                    $('#dlgTransferir').dialog('close');
                });

                $('#divTransferirBotones #btnCancelar').click(function () {
                    $('#frmTransferir').form('clear');
                    $('#dlgTransferir').dialog('close');
                });

                $('#divTransferirSupervisorBotones #btnAceptar').click(function () {
                    var codigo_canal_t = $.trim($('#dlgTransferirSupervisor #codigo_canal').combobox('getValue'));
                    var codigo_grupo_t = $.trim($('#dlgTransferirSupervisor #codigo_grupo').combobox('getValue'));
                    var codigo_old = $.trim($('#hdCodigo').val());
                    var codigo_canal_grupo = '';

                    var percibe_comision = $('#dlgTransferirSupervisor #percibe_comision').switchbutton('options').checked;
                    var percibe_bono = $('#dlgTransferirSupervisor #percibe_bono').switchbutton('options').checked;

                    var anular = $('#dlgTransferirSupervisor #anular').switchbutton('options').checked;
                    var mantener = $('#dlgTransferirSupervisor #mantener').switchbutton('options').checked;

                    if (!codigo_grupo_t && LongitudGrupo > 0) {
                        $.messager.alert('Canal', 'Debe seleccionar un Grupo.', 'warning');
                        return false;
                    }

                    if (codigo_grupo_t != '') {
                        codigo_canal_grupo = codigo_grupo_t
                    }
                    else {
                        codigo_canal_grupo = codigo_canal_t
                    }

                    if ( (anular && mantener && codigo_canal_grupo != '') 
                        || (anular && mantener && codigo_canal_grupo == '')
                        || (!anular && mantener && codigo_canal_grupo != '')
                        || (anular && !mantener && codigo_canal_grupo != '')
                        ){
                        $.messager.alert('Canal', 'Debe elegir solo una acci&oacute;n a tomar.', 'warning');
                        return false;
                    }

                    if (!anular && !mantener && codigo_canal_grupo == '') {
                        $.messager.alert('Canal', 'Debe elegir una acci&oacute;n a tomar.', 'warning');
                        return false;
                    }

                    if (!anular && mantener && codigo_canal_grupo == '') {
                        if (CantidadSupervisiones == 0)
                        {
                            $.messager.alert('Canal', 'No puede elegir esta acci&oacute;n por no contar con otras supervisiones.', 'warning');
                            return false;
                        }
                        accionSupervisor = 3;
                    }

                    if (anular && !mantener && codigo_canal_grupo == '') {
                        accionSupervisor = 2;
                    }

                    if (!anular && !mantener && codigo_canal_grupo != '') {
                        if (CantidadSupervisiones != 0) {
                            $.messager.alert('Canal', 'No puede elegir esta acci&oacute;n por contar con otras supervisiones.', 'warning');
                            return false;
                        }
                        CanalGrupoAccion = codigo_canal_grupo;
                        PercibeComision = percibe_comision ? "1" : "0";
                        PercibeBono = percibe_bono ? "1" : "0";
                        accionSupervisor = 1;
                    }

                    $('#frmTransferirSupervisor').form('clear');
                    $('#dlgTransferirSupervisor').dialog('close');
                });

                $('#divTransferirSupervisorBotones #btnCancelar').click(function () {
                    $('#frmTransferirSupervisor').form('clear');
                    $('#dlgTransferirSupervisor').dialog('close');
                });

                $('#dlgRegistrar #tieneSupervisor').switchbutton({
                    onChange: function (checked) {
                        if (!checked)
                        {
                            if (codigoPersonal != '')
                            {
                                var result = $.messager.confirm('Confirm', '&iquest;Seguro de quitar el supervisor del Canal?', function (result) {
                                    if (result) {
                                        codigoPersonal = '';
                                        nombrePersonal = ''
                                        fechaRegistra = '';
                                        usuarioRegistra = '';
                                        accionSupervisor = 0;
                                        $('#dlgRegistrar #nombre_personal').textbox('setText', nombrePersonal);
                                        current.AsignarGrillaPersonal();
                                    }
                                    else {
                                        $('#dlgRegistrar #tieneSupervisor').switchbutton({ checked: true });
                                    }
                                });
                            }
                        }
                    }
                });

            },

            Initialize_DataGrid: function () {
                $('#DataGrid').datagrid({
                    url: project.ActionUrls.GetAllJson,
                    fitColumns: true,
                    idField: 'codigo_canal_grupo',
                    data: null,
                    pagination: true,
                    singleSelect: true,
                    rownumbers: true,
                    pageList: [20, 60, 80, 100, 150],
                    pageSize: 20,
                    columns:
                    [[
                        { field: 'codigo_canal_grupo', hidden: true },
                        { field: 'codigo_personal', hidden: true },
						{ field: 'nombre', title: 'Nombre', width: 150, align: 'left', halign: 'center' },
						{ field: 'nombre_personal', title: 'Supervisor', width: 250, align: 'left', halign: 'center' },
						{ field: 's_percibe_comision', title: 'Supervisor<br>Comisiona', width: 50, align: 'center', halign: 'center', formatter: function (value, row, index) { if (row.s_percibe_comision) { return 'Si'; } else { return 'No'; } } },
						{ field: 's_percibe_bono', title: 'Supervisor<br>Bono', width: 50, align: 'center', halign: 'center', formatter: function (value, row, index) { if (row.s_percibe_bono) { return 'Si'; } else { return 'No'; } } },
						{ field: 'p_percibe_comision', title: 'Vendedor<br>Comisiona', width: 50, align: 'center', halign: 'center', formatter: function (value, row, index) { if (row.p_percibe_comision) { return 'Si'; } else { return 'No'; } } },
						{ field: 'p_percibe_bono', title: 'Vendedor<br>Bono', width: 50, align: 'center', halign: 'center', formatter: function (value, row, index) { if (row.p_percibe_bono) { return 'Si'; } else { return 'No'; } } },
                        { field: 'administra_grupos', title: 'Administra<br>Grupos', width: 50, align: 'center', halign: 'center', formatter: function (value, row, index) { if (row.administra_grupos) { return '<a href=javascript:Redireccionar();>Si<a/>'; } else { return 'No'; } } },
                        { field: 'estado', title: 'Estado', width: 70, align: 'center', halign: 'center', formatter: function (value, row, index) { if (row.estado_registro) { return 'Activo'; } else { return 'Inactivo'; } } },
                        { field: 'indica_estado', hidden: true ,title: 'Estado', width: 70, align: 'center', halign: 'center', formatter: function (value, row, index) { if (row.estado_registro) { return '1'; } else { return '0'; } } }
                    ]],
                    onClickRow: function (index, row) {
                        var rowColumn = row['codigo_canal_grupo'];
                        $("#hdCodigo").val(rowColumn);
                    }
                });

                

                $('#DataGrid').datagrid('enableFilter', [{
                    field: 'estado',
                    type: 'combobox',
                    options: {
                        panelHeight: 'auto',
                        data: [{ value: '', text: 'Todos' }, { value: '1', text: 'Activo' }, { value: '0', text: 'Inactivo' }],
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

                //$('#DataGrid').datagrid('enableFilter');
                //$('#DataGrid').datagrid('loadData', {});
            },

            Guardar: function () {
                current.Registrar();
            },

            Registrar: function () {

                var codigo_canal_grupo = -1;
                var message = '';
                var noPuedeGuardar = true;
                var nombre = $.trim($('#dlgRegistrar #nombre').textbox('getText'));
                var tieneSupervisor = $('#dlgRegistrar #tieneSupervisor').switchbutton('options').checked;
                var administra_grupos = $('#dlgRegistrar #administraGrupos').switchbutton('options').checked;

                if (current.EditType == current.EditTypes.Modificar) {
                    codigo_canal_grupo = $("#hdCodigo").val();
                }

                if (!nombre) {
                    $.messager.alert('Canal', 'Por favor ingrese un nombre.', 'warning');
                    return false;
                }
                else if (nombre.length > 50) {
                    $.messager.alert('Canal', 'Nombre, numero maximo de caracteres 50', 'warning');
                    return false;
                }

                if ((tieneSupervisor) && (codigoPersonal == '')) {
                    $.messager.alert('Canal', 'Debe seleccionar un Supervisor.', 'warning');
                    return false;
                }

                if (accionSupervisor == 0)
                {
                    if (!current.TransferirSupervisorVentana())
                    {
                        return false;
                    }
                    
                }
                //if (tieneSupervisor && administra_grupos) {
                //    $.messager.alert('Canal', 'No puede configurar Supervisor y a la vez Administrar Grupos.', 'warning');
                //    return false;
                //}

                var trama1 = ''; var trama2 = ''; var trama3 = ''; var trama4 = '';

                var comisionSupervisor = $('#dlgRegistrar #comisionSupervisor ').switchbutton('options').checked;
                var planillaComisionSupervisor = $('#dlgRegistrar #planillaComisionSupervisor ').switchbutton('options').checked;
                var empresaPlanillaComisionSupervisor = $.trim($('#dlgRegistrar #empresaPlanillaComisionSupervisor ').combobox('getValues'));
                var facturaComisionSupervisor = $('#dlgRegistrar #facturaComisionSupervisor ').switchbutton('options').checked;
                var empresaFacturaComisionSupervisor = $.trim($('#dlgRegistrar #empresaFacturaComisionSupervisor ').combobox('getValues'));

                var bonoSupervisor = $('#dlgRegistrar #bonoSupervisor ').switchbutton('options').checked;
                var planillaBonoSupervisor = $('#dlgRegistrar #planillaBonoSupervisor ').switchbutton('options').checked;
                var empresaPlanillaBonoSupervisor = $.trim($('#dlgRegistrar #empresaPlanillaBonoSupervisor ').combobox('getValues'));
                var facturaBonoSupervisor = $('#dlgRegistrar #facturaBonoSupervisor ').switchbutton('options').checked;
                var empresaFacturaBonoSupervisor = $.trim($('#dlgRegistrar #empresaFacturaBonoSupervisor ').combobox('getValues'));

                var comisionPersonal = $('#dlgRegistrar #comisionPersonal ').switchbutton('options').checked;
                var planillaComisionPersonal = $('#dlgRegistrar #planillaComisionPersonal ').switchbutton('options').checked;
                var empresaPlanillaComisionPersonal = $.trim($('#dlgRegistrar #empresaPlanillaComisionPersonal ').combobox('getValues'));
                var facturaComisionPersonal = $('#dlgRegistrar #facturaComisionPersonal ').switchbutton('options').checked;
                var empresaFacturaComisionPersonal = $.trim($('#dlgRegistrar #empresaFacturaComisionPersonal ').combobox('getValues'));

                var bonoPersonal = $('#dlgRegistrar #bonoPersonal ').switchbutton('options').checked;
                var planillaBonoPersonal = $('#dlgRegistrar #planillaBonoPersonal ').switchbutton('options').checked;
                var empresaPlanillaBonoPersonal = $.trim($('#dlgRegistrar #empresaPlanillaBonoPersonal ').combobox('getValues'));
                var facturaBonoPersonal = $('#dlgRegistrar #facturaBonoPersonal ').switchbutton('options').checked;
                var empresaFacturaBonoPersonal = $.trim($('#dlgRegistrar #empresaFacturaBonoPersonal ').combobox('getValues'));

                if (comisionSupervisor) {
                    if (planillaComisionSupervisor && !empresaPlanillaComisionSupervisor) {
                        $.messager.alert('Comision', 'Debe seleccionar Empresa para planilla de Supervisor', 'warning');
                        return false;
                    }

                    if (facturaComisionSupervisor && !empresaFacturaComisionSupervisor) {
                        $.messager.alert('Comision', 'Debe seleccionar Empresa para facturacion de Supervisor', 'warning');
                        return false;
                    }

                    if ((planillaComisionSupervisor && facturaComisionSupervisor) && (current.ExisteCodigoRepetido(empresaPlanillaComisionSupervisor,empresaFacturaComisionSupervisor))) {
                        $.messager.alert('Comision', ' Supervisor no puede comisionar por planilla y factura a la misma empresa.', 'warning');
                        return false;
                    }

                    if (!facturaComisionSupervisor && !planillaComisionSupervisor) {
                        $.messager.alert('Comision', 'Debe seleccionar si cobra por planilla o por factura el Supervisor', 'warning');
                        return false;
                    }
                }
                else {
                    planillaComisionSupervisor = false;
                    empresaPlanillaComisionSupervisor = '';
                    facturaComisionSupervisor = false;
                    empresaFacturaComisionSupervisor = '';
                }
                trama1 = 'true,' + 'true,' + comisionSupervisor + ',' + planillaComisionSupervisor + ',' + empresaPlanillaComisionSupervisor.replace(/,/g, '.') + ',' + facturaComisionSupervisor + ',' + empresaFacturaComisionSupervisor.replace(/,/g, '.') + '|';

                if (bonoSupervisor) {
                    if (planillaBonoSupervisor && !empresaPlanillaBonoSupervisor) {
                        $.messager.alert('Bono', 'Debe seleccionar Empresa para planilla de Supervisor', 'warning');
                        return false;
                    }

                    if (facturaBonoSupervisor && !empresaFacturaBonoSupervisor) {
                        $.messager.alert('Bono', 'Debe seleccionar Empresa para facturacion de Supervisor', 'warning');
                        return false;
                    }

                    if ((planillaBonoSupervisor && facturaBonoSupervisor) && (current.ExisteCodigoRepetido(empresaPlanillaBonoSupervisor, empresaFacturaBonoSupervisor))) {
                        $.messager.alert('Bono', ' Supervisor no puede recibir bono por planilla y factura de la misma empresa.', 'warning');
                        return false;
                    }

                    if (!facturaBonoSupervisor && !planillaBonoSupervisor) {
                        $.messager.alert('Bono', 'Debe seleccionar si cobra por planilla o por factura el Supervisor', 'warning');
                        return false;
                    }
                }
                else {
                    planillaBonoSupervisor = false;
                    empresaPlanillaBonoSupervisor = '';
                    facturaBonoSupervisor = false;
                    empresaFacturaBonoSupervisor = '';
                }
                trama2 = 'true,' + 'false,' + bonoSupervisor + ',' + planillaBonoSupervisor + ',' + empresaPlanillaBonoSupervisor.replace(/,/g, '.') + ',' + facturaBonoSupervisor + ',' + empresaFacturaBonoSupervisor.replace(/,/g, '.') + '|';

                if (comisionPersonal) {
                    if (planillaComisionPersonal && !empresaPlanillaComisionPersonal) {
                        $.messager.alert('Comision', 'Debe seleccionar Empresa para planilla de Vendedor', 'warning');
                        return false;
                    }

                    if (facturaComisionPersonal && !empresaFacturaComisionPersonal) {
                        $.messager.alert('Comision', 'Debe seleccionar Empresa para facturacion de Vendedor', 'warning');
                        return false;
                    }

                    if ((planillaComisionPersonal && facturaComisionPersonal) && (current.ExisteCodigoRepetido(empresaPlanillaComisionPersonal, empresaFacturaComisionPersonal))) {
                        $.messager.alert('Comision', ' Vendedor no puede comisionar por planilla y factura a la misma empresa.', 'warning');
                        return false;
                    }

                    if (!facturaComisionPersonal && !planillaComisionPersonal) {
                        $.messager.alert('Comision', 'Debe seleccionar si cobra por planilla o por factura el Vendedor', 'warning');
                        return false;
                    }
                }
                else {
                    planillaComisionPersonal = false;
                    empresaPlanillaComisionPersonal = '';
                    facturaComisionPersonal = false;
                    empresaFacturaComisionPersonal = '';
                }
                trama3 = 'false,' + 'true,' + comisionPersonal + ',' + planillaComisionPersonal + ',' + empresaPlanillaComisionPersonal.replace(/,/g, '.') + ',' + facturaComisionPersonal + ',' + empresaFacturaComisionPersonal.replace(/,/g, '.') + '|';

                if (bonoPersonal) {
                    if (planillaBonoPersonal && !empresaPlanillaBonoPersonal) {
                        $.messager.alert('Bono', 'Debe seleccionar Empresa para planilla de Vendedor.', 'warning');
                        return false;
                    }

                    if (facturaBonoPersonal && !empresaFacturaBonoPersonal) {
                        $.messager.alert('Bono', 'Debe seleccionar Empresa para facturacion de Vendedor.', 'warning');
                        return false;
                    }

                    if ((planillaBonoPersonal && facturaBonoPersonal) && (current.ExisteCodigoRepetido(empresaPlanillaBonoPersonal, empresaFacturaBonoPersonal))) {
                        $.messager.alert('Bono', ' Vendedor no puede recibir bono por planilla y factura de la misma empresa.', 'warning');
                        return false;
                    }

                    if (!facturaBonoPersonal && !planillaBonoPersonal) {
                        $.messager.alert('Bono', 'Debe seleccionar si cobra por planilla o por factura el Vendedor.', 'warning');
                        return false;
                    }
                }
                else {
                    planillaBonoPersonal = false;
                    empresaPlanillaBonoPersonal = '';
                    facturaBonoPersonal = false;
                    empresaFacturaBonoPersonal = '';
                }
                trama4 = 'false,' + 'false,' + bonoPersonal + ',' + planillaBonoPersonal + ',' + empresaPlanillaBonoPersonal.replace(/,/g, '.') + ',' + facturaBonoPersonal + ',' + empresaFacturaBonoPersonal.replace(/,/g, '.');

                var accion = accionSupervisor + ',' + CanalGrupoAccion + ',' + PercibeComision + ',' + PercibeBono;

                var dataPersonal = ''

                noPuedeGuardar = false;

                if (noPuedeGuardar) {
                    $.messager.alert('Error', message, 'error');
                }
                else {
                    $.messager.confirm('Confirm', '&iquest;Seguro que desea guardar este registro?', function (result) {
                        if (result) {
                            var mapData = { codigo_cg: codigo_canal_grupo, nombre: nombre, codigoPersonalOriginal: CodigoPersonalOriginal, codigoPersonal: codigoPersonal, administra_grupos: administra_grupos, dataConfiguracion: trama1 + trama2 + trama3 + trama4, accion: accion };
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
                                            $.messager.alert('Canal', data.Msg, 'warning');
                                        }
                                        else {
                                            $('#frmRegistrar').form('clear');
                                            $('#dlgRegistrar').dialog('close');
                                            project.ShowMessage('Alerta', 'Registro Exitoso');
                                            current.RecargarGrilla();
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

            Eliminar: function(codigo) {
                $.messager.confirm('Confirm', '&iquest;Seguro que desea desactivar este registro?', function (result) {
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
                                        current.RecargarGrilla();
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
                                codigo_canal_grupo: data.codigo_canal_grupo,
                                nombre: data.nombre,
                                administra_grupos: data.administra_grupos,
                                s_percibe_comision: data.s_percibe_comision,
                                s_percibe_bono: data.s_percibe_bono,
                                p_percibe_comision: data.p_percibe_comision,
                                p_percibe_bono: data.p_percibe_bono,
                                s_c_empresa_planilla: data.s_c_empresa_planilla,
                                s_c_empresa_factura: data.s_c_empresa_factura,
                                s_b_empresa_planilla: data.s_b_empresa_planilla,
                                s_b_empresa_factura: data.s_b_empresa_factura,
                                p_c_empresa_planilla: data.p_c_empresa_planilla,
                                p_c_empresa_factura: data.p_c_empresa_factura,
                                p_b_empresa_planilla: data.p_b_empresa_planilla,
                                p_b_empresa_factura: data.p_b_empresa_factura,
                                estado_registro: data.estado_registro,
                            };
                        }
                    },
                    error: function () {
                        project.AlertErrorMessage('Error', 'Error');
                    }
                });
            },

            Initialize_ComboBox: function () {
                $('#dlgRegistrar #empresaPlanillaComisionSupervisor').combobox({
                    valueField: 'id',
                    textField: 'text'
                });
                $('#dlgRegistrar #empresaFacturaComisionSupervisor').combobox({
                    valueField: 'id',
                    textField: 'text'
                });
                $('#dlgRegistrar #empresaPlanillaBonoSupervisor').combobox({
                    valueField: 'id',
                    textField: 'text'
                });
                $('#dlgRegistrar #empresaFacturaBonoSupervisor').combobox({
                    valueField: 'id',
                    textField: 'text'
                });
                $('#dlgRegistrar #empresaPlanillaComisionPersonal').combobox({
                    valueField: 'id',
                    textField: 'text'
                });
                $('#dlgRegistrar #empresaFacturaComisionPersonal').combobox({
                    valueField: 'id',
                    textField: 'text'
                });
                $('#dlgRegistrar #empresaPlanillaBonoPersonal').combobox({
                    valueField: 'id',
                    textField: 'text'
                });
                $('#dlgRegistrar #empresaFacturaBonoPersonal').combobox({
                    valueField: 'id',
                    textField: 'text'
                });

                $('#dlgTransferir #codigo_grupo').combobox({
                    valueField: 'codigo_canal_grupo',
                    textField: 'nombre',
                    onSelect: function (row) {
                        var dataGrupo = current.grupoFilter(row.codigo_canal_grupo);
                        $('#dlgTransferir #percibe_comision').switchbutton({ checked: dataGrupo[0].personal_percibe_comision });
                        $('#dlgTransferir #percibe_bono').switchbutton({ checked: dataGrupo[0].personal_percibe_bono });
                    }
                });

                $('#dlgTransferir #codigo_canal').combobox({
                    valueField: 'codigo_canal_grupo',
                    textField: 'nombre',
                    data: JsonCanal,
                    required: true,
                    onSelect: function (row) {
                        var dataGrupo = current.grupoReload(row.codigo_canal_grupo);
                        var dataCanal = current.canalFilter(row.codigo_canal_grupo);
                        $('#dlgTransferir #codigo_grupo').combobox('clear').combobox('loadData', dataGrupo);
                        LongitudGrupo = dataGrupo.length;

                        $('#dlgTransferir #percibe_comision').switchbutton({ checked: dataCanal[0].personal_percibe_comision });
                        $('#dlgTransferir #percibe_bono').switchbutton({ checked: dataCanal[0].personal_percibe_bono });
                    }
                });

                $('#dlgTransferirSupervisor #codigo_grupo').combobox({
                    valueField: 'codigo_canal_grupo',
                    textField: 'nombre',
                    onSelect: function (row) {
                        var dataGrupo = current.grupoFilter(row.codigo_canal_grupo);
                        $('#dlgTransferirSupervisor #percibe_comision').switchbutton({ checked: dataGrupo[0].personal_percibe_comision });
                        $('#dlgTransferirSupervisor #percibe_bono').switchbutton({ checked: dataGrupo[0].personal_percibe_bono });
                    }
                });

                $('#dlgTransferirSupervisor #codigo_canal').combobox({
                    valueField: 'codigo_canal_grupo',
                    textField: 'nombre',
                    data: JsonCanal,
                    required: true,
                    onSelect: function (row) {
                        var dataGrupo = current.grupoReload(row.codigo_canal_grupo);
                        var dataCanal = current.canalFilter(row.codigo_canal_grupo);
                        $('#dlgTransferirSupervisor #codigo_grupo').combobox('clear').combobox('loadData', dataGrupo);
                        LongitudGrupo = dataGrupo.length;

                        $('#dlgTransferirSupervisor #percibe_comision').switchbutton({ checked: dataCanal[0].personal_percibe_comision });
                        $('#dlgTransferirSupervisor #percibe_bono').switchbutton({ checked: dataCanal[0].personal_percibe_bono });
                    }
                });

            },

            GetPersonaByNombreJson: function (texto) {
                $.ajax({
                    type: 'post',
                    url: project.ActionUrls.GetPersonalByNombreJson,
                    data: { texto: texto },
                    async: false,
                    cache: false,
                    dataType: 'json',
                    success: function (data) {

                        if (data.Msg) {
                            project.AlertErrorMessage('Error', data.Msg);
                        } else {
                            $('#dlgBusquedaPersona #dgPersonas').datagrid({
                                data: data,
                                //shrinkToFit: false,
                                fitColumns: true,
                                idField: 'codigo_usuario',
                                singleSelect: true,
                                columns:
                                [[
                                    { field: 'codigo_personal', hidden: true },
									{ field: 'nombre', title: 'Nombre', width: 50, align: 'left' },
                                    { field: 'apellido_paterno', title: 'Apellido Paterno', width: 50, align: 'left' },
                                    { field: 'apellido_materno', title: 'Apellido Materno', width: 50, align: 'left' },
                                    { field: 'fecha_registra', hidden: true },
                                    { field: 'usuario_registra', hidden: true },
                                ]],
                                onClickRow: function (index, row) {
                                    personal_tmp[0] = row['codigo_personal'];
                                    personal_tmp[1] = row['nombre'] + ' ' + row['apellido_paterno'] + ' ' + row['apellido_materno'];
                                    personal_tmp[2] = row['fecha_registra'];
                                    personal_tmp[3] = row['usuario_registra'];
                                }
                                , onDblClickRow: function (index, row) {
                                    current.SeleccionarPersona();
                                }
                            });

                        }
                    },
                    error: function () {
                        project.AlertErrorMessage('Error', 'Error');
                    }
                });
            },

            GetPersonalDisponible: function () {
                $.ajax({
                    type: 'post',
                    url: project.ActionUrls.GetPersonalDisponibleJson,
                    data: {  },
                    async: false,
                    cache: false,
                    dataType: 'json',
                    success: function (data) {
                        if (data.Msg) {
                            project.AlertErrorMessage('Error', data.Msg);
                        }
                        else {
                            $('#dlgGestionarPersonal #dgPersonalDisponible').datagrid('loadData', data);
                        }
                    },
                    error: function () {
                        project.AlertErrorMessage('Error', 'Error');
                    }
                });

            },

            GetPersonalYaAsignado: function (codigoCanalGrupo) {
                $.ajax({
                    type: 'post',
                    url: project.ActionUrls.GetPersonalYaAsignadoJson,
                    data: { codigoCanalGrupo: codigoCanalGrupo },
                    async: false,
                    cache: false,
                    dataType: 'json',
                    success: function (data) {
                        if (data.Msg) {
                            project.AlertErrorMessage('Error', data.Msg);
                        }
                        else {
                            dataYaAsignado = data;
                        }
                    },
                    error: function () {
                        project.AlertErrorMessage('Error', 'Error');
                    }
                });

            },

            AdicionarSupervisor: function () {
                if (codigoPersonal) {
                    $('#dlgRegistrar #dgPersonalYaAsignado').datagrid('insertRow', {
                        index: 0,
                        row: {
                            codigo_registro: '',
                            codigo_personal: codigoPersonal,
                            nombre: nombrePersonal,
                            fecha_registra: fechaRegistra,
                            usuario_registra: usuarioRegistra,
                            es_supervisor: 'true'
                        }
                    }
                    );
                }
            },

            ValidarSeleccion: function (codigo) {
                if (!codigo) {
                    $.messager.alert('Canal', 'Por favor seleccione un registro', 'warning');
                    return false;
                }
                return true;
            },

            ObtenerRegistro: function (codigo) {
                current.GetSingle(codigo);

                if (!Registro.codigo_canal_grupo) {
                    $.messager.alert('Error', 'Error al cargar los datos del registro', 'error');
                    $('#DataGrid').datagrid('reload');
                    return false;
                }
                return true;
            },

            RecargarGrilla: function ()
            {
                $('#DataGrid').datagrid('clearSelections');
                $('#DataGrid').datagrid('reload');
                $("#hdCodigo").val("");
                current.EditType = current.EditTypes.Ninguno;
            },

            Redimensionar: function() {
                var mediaquery = window.matchMedia("(max-width: 600px)");
                var ancho = '70%';
                    
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

            LimpiarGrilla: function (grillaALimpiar) {
                grillaALimpiar.datagrid('loadData', { "total": 0, "rows": [] });
                grillaALimpiar.datagrid('reload');
            },

            InicializarGrillas: function () {

                current.LimpiarGrilla($('#dlgRegistrar #dgPersonalYaAsignado'));
                //current.LimpiarGrilla($('#dlgGestionarPersonal #dgPersonalPorAsignar'));
                //current.LimpiarGrilla($('#dlgGestionarPersonal #dgPersonalDisponible'));

                //$('#dlgGestionarPersonal #dgPersonalPorAsignar').datagrid('enableFilter');
                //$('#dlgGestionarPersonal #dgPersonalDisponible').datagrid('enableFilter');
            },

            InicializarFormulario: function () {
                $('#frmRegistrar').form('clear');
                $('#dlgRegistrar #tieneSupervisor').switchbutton({ checked: false });
                $('#dlgRegistrar #administraGrupos').switchbutton({ checked: false });
                $('#dlgRegistrar #empresaPlanillaComisionSupervisor').combobox('clear').combobox('loadData', JsonEmpresa);
                $('#dlgRegistrar #empresaFacturaComisionSupervisor').combobox('clear').combobox('loadData', JsonEmpresa);
                $('#dlgRegistrar #empresaPlanillaBonoSupervisor').combobox('clear').combobox('loadData', JsonEmpresa);
                $('#dlgRegistrar #empresaFacturaBonoSupervisor').combobox('clear').combobox('loadData', JsonEmpresa);
                $('#dlgRegistrar #empresaPlanillaComisionPersonal').combobox('clear').combobox('loadData', JsonEmpresa);
                $('#dlgRegistrar #empresaFacturaComisionPersonal').combobox('clear').combobox('loadData', JsonEmpresa);
                $('#dlgRegistrar #empresaPlanillaBonoPersonal').combobox('clear').combobox('loadData', JsonEmpresa);
                $('#dlgRegistrar #empresaFacturaBonoPersonal').combobox('clear').combobox('loadData', JsonEmpresa);
                $('#dlgTransferir #codigo_canal').combobox('clear').combobox('loadData', JsonCanal);
                codigoPersonal = '';
                nombrePersonal = '';
                fechaRegistra = '';
                usuarioRegistra = '';
                personal_tmp = [];
                CodigoPersonalOriginal = '';
                NombrePersonalOriginal = '';
                FechaRegistraOriginal = '';
                UsuarioRegistraOriginal = '';
                accionSupervisor = 0;

                personalRemovido = [];
                $('#tabRegistrar').tabs('select', 0);
            },

            AsignarGrillaPersonal: function () {
                var esNuevo = (current.EditType == current.EditTypes.Registrar);
                var codigoCanalGrupo = esNuevo?'-1':$('#hdCodigo').val();

                //var codigoSupervisor = current.CodigoSupervisor('#dgPersonalYaAsignado');

                if (esNuevo) {
                    current.LimpiarGrilla($('#dlgRegistrar #dgPersonalYaAsignado'));
                    current.AdicionarSupervisor();
                }
                else {
                    current.CargarGrillasPersonalAsignado(codigoCanalGrupo, false);
                }

            },

            CargarGrillasPersonalAsignado: function (codigoCanalGrupo, incluirSupervisor)
            {
                current.LimpiarGrilla($('#dgPersonalYaAsignado'));
                current.GetPersonalYaAsignado(codigoCanalGrupo);
                $.each(dataYaAsignado, function (index, data) {
                    if (index == 0 && data.es_supervisor)
                    {
                        if (current.EditType == current.EditTypes.Modificar && incluirSupervisor)
                        {
                            if (CodigoPersonalOriginal == '') {
                                CodigoPersonalOriginal = data.codigo_personal;
                                NombrePersonalOriginal = data.nombre;
                                FechaRegistraOriginal = data.fecha_creacion;
                                UsuarioRegistraOriginal = data.usuario_creacion;
                            }

                            if (codigoPersonal == '') {
                                codigoPersonal = data.codigo_personal;
                                nombrePersonal = data.nombre;
                                fechaRegistra = data.fecha_creacion;
                                usuarioRegistra = data.usuario_creacion;

                                $('#dlgRegistrar #tieneSupervisor').switchbutton({ checked: true });
                                $('#nombre_personal').textbox('setText', nombrePersonal);
                            }
                        }
                    }
                    else {
                        if (codigoPersonal != data.codigo_personal){
                            $('#dgPersonalYaAsignado').datagrid('appendRow', {
                                codigo_registro: data.codigo_registro,
                                codigo_personal: data.codigo_personal,
                                nombre: data.nombre,
                                fecha_registra: data.fecha_creacion,
                                usuario_registra: data.usuario_creacion,
                                es_supervisor: data.es_supervisor
                            });
                        }
                    }
                });
                current.AdicionarSupervisor();
            },

            ExistePreviamente: function (nombreGrilla, codigoBusqueda) {
                var rows = $(nombreGrilla).datagrid('getRows');
                var encontrado = false;

                $.each(rows, function (index, data) {
                    if (data.codigo_personal == codigoBusqueda) {
                        encontrado = true;
                        return false;
                    }
                });

                return encontrado;
            },

            IndexRow: function (nombreGrilla, codigoBusqueda) {
                var rows = $(nombreGrilla).datagrid('getRows');
                var encontrado = null;

                $.each(rows, function (index, data) {
                    if (data.codigo_personal == codigoBusqueda) {
                        encontrado = index;
                        return false;
                    }
                });

                return encontrado;
            },

            //CodigoSupervisor: function (nombreGrilla) {
            //    var rows = $(nombreGrilla).datagrid('getRows');
            //    var encontrado = '';

            //    $.each(rows, function (index, data) {
            //        if (data.es_supervisor) {
            //            encontrado = data.codigo_personal;
            //            return false;
            //        }
            //    });

            //    return encontrado;
            //},

            //AsignarSupervisor: function (codigo_personal, codigoCanalGrupo, es_canal_grupo, percibe_comision, percibe_bono) {
            //    $.ajax({
            //        type: 'post',
            //        url: project.ActionUrls.AsignarSupervisor,
            //        data: { codigo_personal: codigo_personal, codigo_canal_grupo: codigoCanalGrupo, es_canal_grupo: es_canal_grupo, percibe_comision: percibe_comision, percibe_bono: percibe_bono },
            //        async: false,
            //        cache: false,
            //        dataType: 'json'
            //    });
            //},

            //DesasignarSupervisor: function (codigo_personal, codigoCanalGrupo, percibe_comision, percibe_bono) {
            //    $.ajax({
            //        type: 'post',
            //        url: project.ActionUrls.DesasignarSupervisor,
            //        data: { codigo_personal: codigo_personal, codigo_canal_grupo: codigoCanalGrupo, percibe_comision: percibe_comision, percibe_bono: percibe_bono },
            //        async: false,
            //        cache: false,
            //        dataType: 'json'
            //    });
            //},

            Transferir: function (codigo_canal_grupo_old, es_canal_grupo, codigo_personal, codigo_canal_grupo, percibe_comision, percibe_bono) {
                $.ajax({
                    type: 'post',
                    url: project.ActionUrls.Transferir,
                    data: { codigo_canal_grupo_old: codigo_canal_grupo_old, es_canal_grupo: es_canal_grupo, codigo_personal: codigo_personal, codigo_canal_grupo: codigo_canal_grupo, percibe_comision: percibe_comision, percibe_bono: percibe_bono },
                    async: false,
                    cache: false,
                    dataType: 'json',
                    success: function (data) {
                        if (data.Msg) {
                            project.AlertErrorMessage('Error', data.Msg);
                        }
                    },
                    error: function () {
                        project.AlertErrorMessage('Error', 'Error');
                    }
                });
            },

            grupoReload: function (value) {
                grupoFiltrado = JsonGrupo.filter(function (el) {
                    return el.codigo_padre == value;
                });

                return grupoFiltrado;
            },

            canalFilter: function (value) {
                canalFiltrado = JsonCanal.filter(function (el) {
                    return el.codigo_canal_grupo == value;
                });

                return canalFiltrado;
            },

            grupoFilter: function (value) {
                grupoFiltrado = JsonGrupo.filter(function (el) {
                    return el.codigo_canal_grupo == value;
                });

                return grupoFiltrado;
            },

            GetOtrasSupervisiones: function (codigo_personal, codigo_canal_grupo) {
                $.ajax({
                    type: 'post',
                    url: project.ActionUrls.GetOtrasSupervisioneJson,
                    data: { codigo_personal: codigo_personal, codigo_canal_grupo: codigo_canal_grupo },
                    async: false,
                    cache: false,
                    dataType: 'json',
                    success: function (data) {
                        if (data.Msg) {
                            project.AlertErrorMessage('Error', data.Msg);
                        }
                        else {
                            CantidadSupervisiones = data;
                        }
                    },
                    error: function () {
                        project.AlertErrorMessage('Error', 'Error');
                    }
                });

            },

            TransferirSupervisorVentana: function ()
            {
                if (accionSupervisor == 0)
                {
                    if ((CodigoPersonalOriginal != '') && (CodigoPersonalOriginal != codigoPersonal))
                    {
                        $('#dlgTransferirSupervisor #codigo_canal').combobox('clear').combobox('loadData', JsonCanal);
                        $('#dlgTransferirSupervisor').dialog('open').dialog('setTitle', 'Anterior Supervisor: ' + NombrePersonalOriginal);
                        $('#dlgTransferirSupervisor').form('clear');
                        return false;
                    }
                    return true;
                }
            },

            ExisteCodigoRepetido: function (origen, destino)
            {
                var retorno = false;
                var arrOrigen = origen.split(',');
                var arrDestino = destino.split(',');

                for (indice = 0; indice <= arrOrigen.length-1; indice++)
                {
                    var valorOrigen = arrOrigen[indice];
                    for (indiceDestino = 0; indiceDestino <= arrDestino.length-1; indiceDestino++)
                    {
                        if (valorOrigen == arrDestino[indiceDestino])
                        {
                            retorno = true;
                            break;
                        }
                    }
                    if (retorno)
                    {
                        break;
                    }
                }

                return retorno;
            },

            SeleccionarPersona: function () {
                if (personal_tmp[0] == '') {
                    $.messager.alert('Busqueda', 'No ha seleccionado un Vendedor.', 'warning');
                }
                else {
                    $('#dlgRegistrar #nombre_personal').textbox('setText', personal_tmp[1]);
                    //$('#hdCodigoPersonal').val(codigoPersonal);
                    codigoPersonal = personal_tmp[0];
                    nombrePersonal = personal_tmp[1];
                    fechaRegistra = personal_tmp[2];
                    usuarioRegistra = personal_tmp[3];

                    current.AsignarGrillaPersonal();
                    accionSupervisor = 0;

                    $('#frmBusquedaPersona').form('clear');
                    $('#dlgBusquedaPersona').dialog('close');
                    $('#dlgBusquedaPersona #dgPersonas').datagrid('clearSelections');
                }
            },

        });

})
(project);