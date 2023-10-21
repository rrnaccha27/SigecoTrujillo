
var JsonCanal = {};
var JsonGrupo = {};
var Registro = {};

;(function (app) {
    //===========================================================================================
    var current = app.Personal = {};
    //===========================================================================================

    jQuery.extend(app.Personal,
        {

            ActionUrls: {},
            EditType: '',
            HasRootNode: 'False',

            Initialize: function (actionUrls) {
                //$(window).resize(function () {
                //    current.Redimensionar();
                //});

                jQuery.extend(project.ActionUrls, actionUrls);

                //current.Redimensionar();
                InicializarControles();
                InicializarListadoCanales();
                InicializarListadoHistoricoValidacion();
                InicializarListadoHistoricoBloqueo();
                GetPersonalCanalesJson();
            }

        })
})(project);


function InicializarControles()
{
    $('#cmb_moneda').combobox({
		valueField: 'id',
		textField: 'text',
		url: project.ActionUrls.GetTipoMonedaJson
	});

    $('#cmb_tipo_cuenta').combobox({
        valueField: 'id',
        textField: 'text',
        url: project.ActionUrls.GetTipoCuentaJson
    });
    

	$('#cboTipo_Documento').combobox({
	    valueField: 'id',
	    textField: 'text',
	    url: project.ActionUrls.GetTipoDocumentoJson
	});

	$('#cboCodigo_Banco').combobox({
		valueField: 'id',
		textField: 'text',
		url: project.ActionUrls.GetBancoJson
	});

	$('#correo_electronico').validatebox({
		required: true,
		validType: 'email'
	});

	JsonCanal = $('.content').get_json_combobox({
	    url: project.ActionUrls.GetCanalGrupoJson,
        parametro: "?es_canal_grupo=1"
	});

	JsonGrupo = $('.content').get_json_combobox({
	    url: project.ActionUrls.GetCanalGrupoJson,
	    parametro: "?es_canal_grupo=0"
	});


}

function canalFormatter(value) {
    for (var i = 0; i < JsonCanal.length; i++) {
        if (JsonCanal[i].codigo_canal_grupo == value) return JsonCanal[i].nombre;
    }
    return value;
}

function grupoFormatter(value) {
    for (var i = 0; i < JsonGrupo.length; i++) {
        if (JsonGrupo[i].codigo_canal_grupo == value) return JsonGrupo[i].nombre;
    }
    return value;
}

function grupoReload(value) {
    grupoFiltrado = JsonGrupo.filter(function (el) {
        return el.codigo_padre == value;
    });

    return grupoFiltrado;
}

function canalFilter(value) {
    canalFiltrado = JsonCanal.filter(function (el) {
        return el.codigo_canal_grupo == value;
    });

    return canalFiltrado;
}

function grupoFilter(value) {
    grupoFiltrado = JsonGrupo.filter(function (el) {
        return el.codigo_canal_grupo == value;
    });

    return grupoFiltrado;
}

var codigo_grupo_original = '';
var codigo_canal_original = '';
var longitud_grupo = 0;
var no_seleccion = true;

function InicializarListadoCanales () {

    $('#dgCanales').datagrid({
        height:'100%',
        idField: 'codigo_registro',
        fitColumns: true,
        data: null,
        toolbar: '#tb',
        pagination: true,
        singleSelect: true,
        pageNumber: 0,
        pageList: [5, 10, 15, 20, 25, 30],
        pageSize: 10,
        columns:
        [[
            { field: 'codigo_registro', hidden: 'true' },
            { field: 'confirmado', hidden: 'true' },
            {
                field: 'codigo_canal', title: 'Canal', width: 80, align: 'left',
                editor: {
                    type: 'combobox',
                    options: {
                        valueField: 'codigo_canal_grupo',
                        textField: 'nombre',
                        data: JsonCanal,
                        //novalidate: true,
                        required: true,
                        editable: false,
                        onChange: function (value) {
                            setTimeout(function () {
                                var dataGrupo = grupoReload(value);
                                var dataCanal = canalFilter(value);
                                var indice = IndexRowEditing('dgCanales');
                                var ed = $('#dgCanales').datagrid('getEditor', { index: indice, field: 'codigo_grupo' });

                                longitud_grupo = dataGrupo.length;
                                $(ed.target).combobox('clear');
                                $(ed.target).combobox('loadData', dataGrupo);

                                no_seleccion = true;
                                if (codigo_grupo_original != '')
                                {
                                    no_seleccion = false;
                                    $(ed.target).combobox('setValue', codigo_grupo_original);
                                }

                                if (codigo_canal_original == value) {
                                    codigo_canal_original = '';
                                    return false;
                                }

                                if (longitud_grupo == 0) {
                                    //console.log('Setea por Canal')
                                    ed = $('#dgCanales').datagrid('getEditor', { index: indice, field: 'percibe_comision' });
                                    $(ed.target).prop('checked', dataCanal[0].personal_percibe_comision);

                                    ed = $('#dgCanales').datagrid('getEditor', { index: indice, field: 'percibe_bono' });
                                    $(ed.target).prop('checked', dataCanal[0].personal_percibe_bono);
                                }
                                else {
                                    ed = $('#dgCanales').datagrid('getEditor', { index: indice, field: 'percibe_comision' });
                                    $(ed.target).prop('checked', false);

                                    ed = $('#dgCanales').datagrid('getEditor', { index: indice, field: 'percibe_bono' });
                                    $(ed.target).prop('checked', false);
                                }
                            }, 0);
                        }
                    }
                },
                formatter: canalFormatter
            },
            {
                field: 'codigo_grupo', title: 'Grupo', width: 100, align: 'left',
                editor: {
                    type: 'combobox',
                    options: {
                        valueField: 'codigo_canal_grupo',
                        textField: 'nombre',
                        data: JsonGrupo,
                        //novalidate: true,
                        required: false,
                        editable: false,
                        onChange: function (value) {
                            setTimeout(function () {

                                if (longitud_grupo == 0)
                                {
                                    return false;
                                }

                                if (codigo_grupo_original == value) {
                                    codigo_grupo_original = '';
                                    //console.log('val 1');
                                    return false;
                                }
                                else if (!no_seleccion) {
                                    //console.log('val 2');
                                    no_seleccion = true;
                                    return false;
                                }
                                else {
                                    //alert('setea chk x grupo');
                                    var dataGrupo = grupoFilter(value);
                                    var indice = IndexRowEditing('dgCanales');

                                    if (dataGrupo.length > 0)
                                    { 
                                        //console.log('setea por grupo');
                                        ed = $('#dgCanales').datagrid('getEditor', { index: indice, field: 'percibe_comision' });
                                        $(ed.target).prop('checked', dataGrupo[0].personal_percibe_comision);

                                        ed = $('#dgCanales').datagrid('getEditor', { index: indice, field: 'percibe_bono' });
                                        $(ed.target).prop('checked', dataGrupo[0].personal_percibe_bono);
                                    }
                                }
                            }, 0);
                        }
                    }
                },
                formatter: grupoFormatter
            }
            , {
                field: 'es_supervisor_canal', title: 'Supervisor<br>Canal', width: 50, align: 'center', halign: 'center', 
                formatter: function (value, row, index) {
                    if (row.es_supervisor_canal == 'true') { return 'Si'; } else { return ''; }
                },
                editor: {
                    type: 'checkbox',
                    options: { on: true, off: false }
                }
            }
            , {
                field: 'es_supervisor_grupo', title: 'Supervisor<br>Grupo', width: 50, align: 'center', halign: 'center',
                formatter: function (value, row, index) {
                    if (row.es_supervisor_grupo == 'true') { return 'Si'; } else { return ''; }
                },
                editor: {
                    type: 'checkbox',
                    options: { on: true, off: false }
                }
            }
            , {
                field: 'percibe_comision', title: 'Percibe<br>Comision', width: 50, align: 'center', halign: 'center',
                formatter: function (value, row, index) {
                    if (row.percibe_comision == 'true') { return 'Si'; } else { return ''; }
                },
                editor: {
                    type: 'checkbox',
                    options: { on: true, off: false }
                }
            }
            , {
                field: 'percibe_bono', title: 'Percibe<br>Bono', width: 50, align: 'center', halign: 'center',
                formatter: function (value, row, index) {
                    if (row.percibe_bono == 'true') { return 'Si'; } else { return ''; }
                },
                editor: {
                    type: 'checkbox',
                    options: { on: true, off: false }
                }
            }
            , {
                field: 'estado_registro', title: 'Activo', width: 50, align: 'center', halign: 'center',
                formatter: function (value, row, index) {
                    if (row.estado_registro == 'true') { return 'Si'; } else { return ''; }
                },
                editor: {
                    type: 'checkbox',
                    options: { on: true, off: false }
                }
            }
        ]],

        onBeforeEdit: function (index, row) {
            row.editing = true;
        },
        onAfterEdit: function (index, row) {
            row.editing = false;
        },
        onCancelEdit: function (index, row) {
            row.editing = false;
        },
        onDblClickRow: function (rowIndex, rowdata) {
            if (!existeRegistroEnEdicion("dgCanales")) {
                if (rowdata.estado_registro == 'true'){
                    codigo_grupo_original = rowdata.codigo_grupo;
                    codigo_canal_original = rowdata.codigo_canal;
                    $('#dgCanales').datagrid('beginEdit', rowIndex);
                    rowdata.editing = true;
                    no_seleccion = true;
                    HabilitarEdicion(false);
                    HabilitarGuardarEdicion(true);
                    HabilitarGuardarPersonal(false);
                }
            }
        }
    });
}

function InicializarListadoHistoricoValidacion()
{
    var codigo_personal = project.ActionUrls.codigo_personal;

    $('#dgHistoricoValidaciones').datagrid({
        fitColumns: true,
        height: '207px',
        idField: 'id',
        url: project.ActionUrls.GetHistorialValidacionJson,
        pagination: true,
        singleSelect: true,
        rownumbers: true,
        pageList: [20, 60, 80],
        pageSize: 20,
        queryParams: {
            codigo_personal: codigo_personal
        },
        columns:
		[[
			{ field: 'id', hidden: 'true' },
			{ field: 'texto_registra', title: '', width: 50, align: 'center', halign: 'center' },
			{ field: 'fecha_registra', title: 'Fecha<br>Validación', width: 50, align: 'center', halign: 'center' },
            { field: 'usuario_registra', title: 'Usuario<br>Validación', width: 50, align: 'left', halign: 'center' },
            { field: 'texto_modifica', title: '', width: 50, align: 'center', halign: 'center' },
			{ field: 'fecha_modifica', title: 'Fecha<br>Invalidacion', width: 50, align: 'center', halign: 'center' },
			{ field: 'usuario_modifica', title: 'Usuario<br>Invalidacion', width: 50, align: 'left', halign: 'center' }
		]],
        loadFilter: function (data) {
            return data;
        }
    });
}

function InicializarListadoHistoricoBloqueo() {
    var codigo_personal = project.ActionUrls.codigo_personal;

    $('#dgHistoricoBloqueo').datagrid({
        fitColumns: true,
        height: '100%',
        idField: 'codigo_persona',
        url: project.ActionUrls.GetHistorialBloqueoJson,
        pagination: true,
        singleSelect: true,
        rownumbers: true,
        pageList: [20, 60, 80],
        pageSize: 20,
        queryParams: {
            codigo_personal: codigo_personal
        },
        columns:
		[[
			{ field: 'codigo_persona', hidden: 'true' },
			{ field: 'descripcion', title: 'Bloqueo', width: 55, align: 'left', halign: 'center' },
			{ field: 'numero_planilla', title: 'Número<br>Planilla', width: 35, align: 'center', halign: 'center' },
            { field: 'descripcion_planilla', title: 'Descripción<br>Planilla', width: 85, align: 'left', halign: 'center' },
            { field: 'estado_planilla', title: 'Estado<br>Planilla', width: 35, align: 'center', halign: 'center' },
			{ field: 'fecha_registra', title: 'Fecha<br>Registro', width: 50, align: 'center', halign: 'center' },
			{ field: 'fecha_modifica', title: 'Fecha<br>Modificacion', width: 50, align: 'center', halign: 'center' },
		    { field: 'estado_registro_texto', title: 'Estado<br>Bloqueo', width: 35, align: 'center', halign: 'center' }
		]],
        loadFilter: function (data) {
            return data;
        }
    });
}

function GetPersonalCanalesJson() {
    var codigoPersonal = project.ActionUrls.codigo_personal;

    $.ajax({
        type: 'post',
        url: project.ActionUrls.GetPersonalCanalesJson,
        data: { codigoPersonal: codigoPersonal },
        async: false,
        cache: false,
        dataType: 'json',
        success: function (data) {
            if (data.Msg) {
                project.AlertErrorMessage('Error', data.Msg);
            }
            else {
                $('#dgCanales').datagrid('loadData', data);
            }
        },
        error: function () {
            project.AlertErrorMessage('Error', 'Error');
        }
    });
}

function GuardarPersonal() {
    var esNuevo = project.ActionUrls.codigo_personal == -1? true:false;
    var message = '';

    if (!$("#frmRegistro").form('enableValidation').form('validate')) {
        $('#tabPersonal').tabs('select', 0);
        return false;
    }

    var es_persona_juridica = $('#es_persona_juridica').switchbutton('options').checked;
    var codigo_tipo_documento = $.trim($('#cboTipo_Documento').combobox('getValue'));
    var codigo_moneda = $.trim($('#cmb_moneda').combobox('getValue'));

    
    var nro_documento = $.trim($('#nro_documento').val());
    var codigo_tipo_cuenta = $('#cmb_tipo_cuenta').combobox("getValue");
    var nro_ruc = $.trim($('#nro_ruc').val().length == 0 ? "" : $('#nro_ruc').val());
    var apellido_paterno = $.trim($('#apellido_paterno').textbox('getText'));
    var apellido_materno = $.trim($('#apellido_materno').textbox('getText'));
    var direccion = $.trim($('#direccion').textbox('getText'));
    var contacto = $.trim($('#contacto').textbox('getText'));

    if (es_persona_juridica && codigo_tipo_documento != '2')
    {
        $.messager.alert("Vendedor", "Si es Persona Juridica debe seleccionar RUC como tipo documento.", "warning");
        return false;
    }

    if (es_persona_juridica && codigo_tipo_documento == '2' && !nro_ruc) {
        $.messager.alert("Vendedor", "Ingrese el RUC.", "warning");
        return false;
    }

    if (nro_ruc.length > 0 && nro_ruc.length < 11) {
        $.messager.alert("Vendedor", "Ingrese el RUC con 11 d&iacute;gitos.", "warning");
        return false;
    }

    if (!es_persona_juridica && codigo_tipo_documento == '2') {
        $.messager.alert("Vendedor", "Si no es persona Jurídica no seleccione RUC.", "warning");
        return false;
    }

    if (!es_persona_juridica && codigo_tipo_documento != '2' && !nro_documento) {
        $.messager.alert("Vendedor", "Ingrese el nro de documento.", "warning");
        return false;
    }

    if (!es_persona_juridica && (!apellido_paterno || !apellido_materno)) {
        $.messager.alert("Vendedor", "Debe ingresar los apellidos.", "warning");
        return false;
    }

    if (existeRegistroEnEdicion('dgCanales'))
        return;

    var rows = $("#dgCanales").datagrid("getRows");
    if (rows.length < 1) {
        $.messager.alert("Canal/Grupo", "Registre un Canal o Grupo, intente nuevamente.", "warning");
        return;
    }
    
    var tieneActivo = false;
    $.each(rows, function (i, objeto) {
        if (objeto.estado_registro == 'true') {
            tieneActivo = true;
            return false;
        }
    });

    if (!tieneActivo){
        $.messager.alert("Canal/Grupo", "Registre un Canal o Grupo como Activo, intente nuevamente.", "warning");
        return;
    }

    var tieneSupervision = 0;
    $.each(rows, function (i, objeto) {
        if (objeto.es_supervisor_grupo == 'true' || objeto.es_supervisor_canal == 'true') {
            tieneSupervision ++;
        }
    });

    if (tieneSupervision > 0 && tieneSupervision != rows.length) {
        $.messager.alert("Canal/Grupo", "No puede registrar ser supervisor y vendedor a la vez.", "warning");
        return;
    }

    //activos = rows.filter(function (el) {
    //    return el.estado_registro == 'true';
    //});

    //if (tieneSupervision == 0 && activos.length > 1) {
    //    $.messager.alert("Canal/Grupo", "No puede registrar un vendedor en mas de un canal/grupo.", "warning");
    //    return;
    //}

    var lst_canal_grupo = [];

    $.each(rows, function (index, data) {
        var canal_grupo = {
            codigo_registro: data.codigo_registro,
            codigo_canal_grupo: data.codigo_grupo != '' ? data.codigo_grupo : data.codigo_canal,
            codigo_canal: data.codigo_canal,
            es_supervisor_canal: (data.es_supervisor_canal == 'true'),
            es_supervisor_grupo: (data.es_supervisor_grupo == 'true'),
            percibe_comision: (data.percibe_comision == 'true'),
            percibe_bono: (data.percibe_bono == 'true'),
            confirmado: data.confirmado,
            estado_registro: (data.estado_registro == 'true')
        };
        lst_canal_grupo.push(canal_grupo);
    });

    var datosPersonal = {
        codigo_personal: project.ActionUrls.estado_registro == 'True'? project.ActionUrls.codigo_personal : '-1',
        es_persona_juridica: $('#es_persona_juridica').switchbutton('options').checked,
        codigo_tipo_documento: $.trim($('#cboTipo_Documento').combobox('getValue')),
        nro_documento: nro_documento,
        nro_ruc: nro_ruc,
        codigo_tipo_cuenta: codigo_tipo_cuenta,
        codigo_moneda:codigo_moneda,
        nombre: $.trim($('#txt_nombre_personal').textbox('getText')),
        apellido_paterno: $.trim($('#apellido_paterno').textbox('getText')),
        apellido_materno: $.trim($('#apellido_materno').textbox('getText')),
        telefono_fijo: $.trim($('#telefono_fijo').val()),
        telefono_celular: $.trim($('#telefono_celular').val()),
        correo_electronico: $.trim($('#correo_electronico').textbox('getText')),
        codigo_banco: $.trim($('#cboCodigo_Banco').combobox('getValue')),
        nro_cuenta: $.trim($('#nro_cuenta').val()),
        codigo_interbancario: $.trim($('#codigo_interbancario').val()),
        contacto: contacto,
        direccion: direccion,
        lista_canal_grupo: lst_canal_grupo
    };

    if (canalesEliminados.length > 0)
    {
        canalesEliminados = canalesEliminados.substring(0, canalesEliminados.length - 1);
    }

    if (message.length > 0) {
        $.messager.alert((esNuevo ? 'Registro' : 'Modificar'), message, 'warning');
        return false;
    }

    $.messager.confirm('Confirm', '&iquest;Seguro que desea guardar?', function (result) {
        if (result) {
            $.ajax({
                type: 'post',
                url: project.ActionUrls.Guardar,
                data: JSON.stringify({ personal: datosPersonal, canalesEliminados: canalesEliminados }),
                dataType: 'json',
                cache: false,
                async: false,
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    if (data.Msg) {
                        if (data.Msg != 'Success') {
                            $.messager.alert('Vendedor', data.Msg, 'warning');
                        }
                        else {
                            $('#dlgRegistro').dialog('close');
                            //project.ShowMessage('Alerta', (esNuevo?'Registro':'Modificado') + ' Exitoso');
                            var mensajeGuardado = (esNuevo ? 'Registro' : 'Modificado') + ' Exitoso.' + (esNuevo ? "<br>C&oacute;digo: " + data.Equivalencia : '') + (!data.MsgWcf ? '' : "<br>" + data.MsgWcf);
                            $.messager.alert("Vendedor", mensajeGuardado, "info");
                            GetAllJson();
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

function Cancelar() {
    $('#dlgRegistro').dialog('close');
}

function NuevoRegistro() {
    if (!existeRegistroEnEdicion('dgCanales')) {
        var new_row = {
            "codigo_registro": -new Date().toString("ddHHmmss"),
            "codigo_canal": "",
            "codigo_grupo": "",
            "es_supervisor_canal": false,
            "es_supervisor_grupo": false,
            "percibe_comision": false,
            "percibe_bono": false,
            "confirmado": false,
            "estado_registro":"true"
        }

        $("#dgCanales").datagrid("appendRow", new_row);
        var editIndex = $("#dgCanales").datagrid("getRowIndex", new_row.codigo_registro);
        $("#dgCanales").datagrid("selectRow", editIndex);
        $("#dgCanales").datagrid("beginEdit", editIndex);

        HabilitarGuardarPersonal(false);
        HabilitarEdicion(false);
        HabilitarGuardarEdicion(true);
    }
}

function AceptarRegistro() {
    if (existeRegistroEnEdicionParaGrabar("dgCanales")) {
        var rowIndex = IndexRowEditing("dgCanales");
        var rowdata = $('#dgCanales').datagrid("getRows")[rowIndex];
        if (validarRowGrilla(rowIndex, 'dgCanales')) {
            if (ValidarDataConsistente('dgCanales', rowIndex, rowdata.codigo_registro)) {
                rowdata.confirmado = true;
                $('#dgCanales').datagrid('endEdit', rowIndex);
                HabilitarGuardarPersonal(true);
                HabilitarEdicion(true);
                HabilitarGuardarEdicion(false);
            }
        } 
    }
}

function CancelarRegistro() {
    var rowIndex = IndexRowEditing('dgCanales');
    if (rowIndex != null) {
        $('#dgCanales').datagrid('cancelEdit', rowIndex);
        var rowdata = $('#dgCanales').datagrid("getRows")[rowIndex];
        if (!rowdata.confirmado && rowdata.codigo_registro < 0){
            $('#dgCanales').datagrid('deleteRow', rowIndex);
        }
        HabilitarGuardarPersonal(true);
        HabilitarEdicion(true);
        HabilitarGuardarEdicion(false);
    }
}

var canalesEliminados = '';
function EliminarRegistro() {
    if (!existeRegistroEnEdicion("dgCanales")) {
        var rows = $("#dgCanales").datagrid("getSelections");
        if (rows.length <= 0) {
            $.messager.alert("Eliminar", "Seleccione un registro.", "warning");
        }
        else {
            var row = $("#dgCanales").datagrid("getSelected");
            $(".messager-window,.window-shadow,.window-mask").remove();

            if (row) {
                if (row.codigo_registro > 1)
                {
                    $.messager.alert("Eliminar", "Solo puede eliminar registros nuevos.", "warning");
                    return false;
                }
                $.messager.confirm('Confirmar', '&iquest;Est&aacute; seguro de eliminar este registro?', function (r) {
                    if (r) {
                        var index = $("#dgCanales").datagrid("getRowIndex", row.codigo_registro);
                        $('#dgCanales').datagrid('deleteRow', index);
                        if (row.codigo_registro > 0)
                        { canalesEliminados += row.codigo_registro + '|';}
                    }
                });
            }
        }
    }
}

function existeRegistroEnEdicion(lista) {
    var editando = false;
    var rows = $("#" + lista).datagrid("getRows");

    $.each(rows, function (i, objeto) {
        if (objeto.editing) {
            editando = true;
            return;
        }
    });
    if (editando) {
        $.messager.alert("Registro en edici&oacute;n", "Existe un registro en edici&oacute;n, para continuar guarde o cancele la edici&oacuten.", "warning");
    }
    return editando;
}

function existeRegistroEnEdicionParaGrabar(lista) {
    var editando = false;
    var rows = $("#" + lista).datagrid("getRows");
    $.each(rows, function (i, objeto) {
        if (objeto.editing) {
            editando = true;
            return;
        }
    });
    if (!editando) {
        $.messager.alert("Registro en edici&oacute;n", "No existe registro en edici&oacute;n para  guardar.", "warning");
    }

    return editando;
}

function IndexRowEditing(lista) {
    var index = null;
    var rows = $("#" + lista).datagrid("getRows");
    $.each(rows, function (i, objeto) {
        if (objeto.editing) {
            index = i;
            return false;
        }
    });
    return index;
}

function ValidarDataConsistente(lista, indiceEditado, codigo_registro) {
    var resultado = true;
    var rows = $("#" + lista).datagrid("getRows");

    var cmb_codigo_canal = $("#dgCanales").datagrid("getEditor", { index: indiceEditado, field: 'codigo_canal' });
    var cmb_codigo_grupo = $("#dgCanales").datagrid("getEditor", { index: indiceEditado, field: 'codigo_grupo' });
    var codigo_canal = cmb_codigo_canal.target.combobox("getValue");
    var codigo_grupo = cmb_codigo_grupo.target.combobox("getValue");

    var chk_es_supervisor_canal = $("#dgCanales").datagrid("getEditor", { index: indiceEditado, field: 'es_supervisor_canal' });
    var chk_es_supervisor_grupo = $("#dgCanales").datagrid("getEditor", { index: indiceEditado, field: 'es_supervisor_grupo' });
    var chk_estado_registro = $("#dgCanales").datagrid("getEditor", { index: indiceEditado, field: 'estado_registro' });

    var es_supervisor_canal = chk_es_supervisor_canal.target.prop('checked');
    var es_supervisor_grupo = chk_es_supervisor_grupo.target.prop('checked');
    var estado_registro = chk_estado_registro.target.prop('checked');

    $.each(rows, function (i, objeto) {
        if (i != indiceEditado) {
            if (objeto.codigo_canal == codigo_canal && objeto.codigo_grupo == codigo_grupo && objeto.estado_registro == 'true')
            {
                $.messager.alert("Canal", "No debe ingresar el mismo Canal y/o Grupo.", "warning");
                resultado = false;
                return false;
            }
        }
    });

    if (resultado) {
        if (longitud_grupo > 0 && codigo_grupo == '')
        {
            $.messager.alert("Canal", "Debe seleccionar un grupo.", "warning");
            resultado = false;
        }
    }

    if (resultado) {
        if (es_supervisor_canal && es_supervisor_grupo) {
            $.messager.alert("Canal", "No puede ser supervisor de Canal y Grupo a la vez.", "warning");
            resultado = false;
        }
    }

    if (resultado) {
        if (es_supervisor_canal && codigo_grupo != '') {
            $.messager.alert("Canal", "No puede ser supervisor de Canal si selecciono un Grupo.", "warning");
            resultado = false;
        }
    }

    if (resultado) {
        if (!estado_registro && codigo_registro < 1) {
            $.messager.alert("Canal", "No puede guardar un registro nuevo como no activo.", "warning");
            resultado = false;
        }
    }

    return resultado;
}


function validarRowGrilla(index, lista) {
    var _Valid = $('#' + lista).datagrid('validateRow', index);
    if (!_Valid) {
        $.messager.alert("Campos Obligatorios", "Falta ingresar los campos requeridos de la grilla en edici&oacute;n.", "warning");
    }
    return _Valid;
}

function HabilitarEdicion(valor) {
    $('#btnNuevoRegistro').linkbutton((valor?'enable':'disable'));
    $('#btnQuitarRegistro').linkbutton((valor ? 'enable' : 'disable'));
}

function HabilitarGuardarEdicion(valor) {
    $('#btnAceptarRegistro').linkbutton((valor ? 'enable' : 'disable'));
    $('#btnCancelarRegistro').linkbutton((valor ? 'enable' : 'disable'));
}

function HabilitarGuardarPersonal(valor) {
    $('#btnGuardar').linkbutton((valor ? 'enable' : 'disable'));
}