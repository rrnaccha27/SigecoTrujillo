var ActionDetalleUrls = {};
var JsonCanal = {};
var JsonGrupo = {};
var Registro = {};

;(function (app) {
    //===========================================================================================
    var current = app.PersonalDetalle = {};
    //===========================================================================================

    jQuery.extend(app.PersonalDetalle,
        {

            EditType: '',
            HasRootNode: 'False',

            Initialize: function (actionUrls) {
                //$(window).resize(function () {
                //    current.Redimensionar();
                //});

                jQuery.extend(ActionDetalleUrls, actionUrls);
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
    $('#cmb_moneda_detalle').combobox({
		valueField: 'id',
		textField: 'text',
		url: ActionDetalleUrls.GetTipoMonedaJson
	});

    $('#cmb_tipo_cuenta_detalle').combobox({
        valueField: 'id',
        textField: 'text',
        url: ActionDetalleUrls.GetTipoCuentaJson
    });
    

	$('#cboTipo_Documento_detalle').combobox({
	    valueField: 'id',
	    textField: 'text',
	    url: ActionDetalleUrls.GetTipoDocumentoJson
	});

	$('#cboCodigo_Banco_detalle').combobox({
		valueField: 'id',
		textField: 'text',
		url: ActionDetalleUrls.GetBancoJson
	});

	JsonCanal = $('.content').get_json_combobox({
	    url: ActionDetalleUrls.GetCanalGrupoJson,
        parametro: "?es_canal_grupo=1"
	});

	JsonGrupo = $('.content').get_json_combobox({
	    url: ActionDetalleUrls.GetCanalGrupoJson,
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

function InicializarListadoCanales () {

    $('#dgCanalesDetalle').datagrid({
        height:'100%',
        idField: 'codigo_registro',
        fitColumns: true,
        data: null,
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
                        editable: false
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
                        editable: false
                    }
                },
                formatter: grupoFormatter
            }
            , {
                field: 'es_supervisor_canal', title: 'Supervisor<br>Canal', width: 50, align: 'center', halign: 'center', 
                formatter: function (value, row, index) {
                    if (row.es_supervisor_canal == 'true') { return 'Si'; } else { return ''; }
                }
            }
            , {
                field: 'es_supervisor_grupo', title: 'Supervisor<br>Grupo', width: 50, align: 'center', halign: 'center',
                formatter: function (value, row, index) {
                    if (row.es_supervisor_grupo == 'true') { return 'Si'; } else { return ''; }
                }
            }
            , {
                field: 'percibe_comision', title: 'Percibe<br>Comision', width: 50, align: 'center', halign: 'center',
                formatter: function (value, row, index) {
                    if (row.percibe_comision == 'true') { return 'Si'; } else { return ''; }
                }
            }
            , {
                field: 'percibe_bono', title: 'Percibe<br>Bono', width: 50, align: 'center', halign: 'center',
                formatter: function (value, row, index) {
                    if (row.percibe_bono == 'true') { return 'Si'; } else { return ''; }
                }
            }
            , {
                field: 'estado_registro', title: 'Activo', width: 50, align: 'center', halign: 'center',
                formatter: function (value, row, index) {
                    if (row.estado_registro == 'true') { return 'Si'; } else { return ''; }
                }
            }
        ]]
    });
}

function InicializarListadoHistoricoValidacion() {
    var codigo_personal = ActionDetalleUrls.codigo_personal;

    $('#dgHistoricoValidacionesDetalle').datagrid({
        fitColumns: true,
        height: '207px',
        idField: 'id',
        url: ActionDetalleUrls.GetHistorialValidacionJson,
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
    var codigo_personal = ActionDetalleUrls.codigo_personal;

    $('#dgHistoricoBloqueoDetalle').datagrid({
        fitColumns: true,
        height: '100%',
        idField: 'codigo_persona',
        url: ActionDetalleUrls.GetHistorialBloqueoJson,
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
    var codigoPersonal = ActionDetalleUrls.codigo_personal;

    $.ajax({
        type: 'post',
        url: ActionDetalleUrls.GetPersonalCanalesJson,
        data: { codigoPersonal: codigoPersonal },
        async: false,
        cache: false,
        dataType: 'json',
        success: function (data) {
            if (data.Msg) {
                project.AlertErrorMessage('Error', data.Msg);
            }
            else {
                $('#dgCanalesDetalle').datagrid('loadData', data);
            }
        },
        error: function () {
            project.AlertErrorMessage('Error', 'Error');
        }
    });
}

function CerrarDetalle() {
    $('#dlgDetalle').dialog('close');
}

