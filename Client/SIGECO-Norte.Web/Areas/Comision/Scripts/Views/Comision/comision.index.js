var ActionIndexUrl = {};
var EstadoRegistro;

; (function (app) {
    //===========================================================================================
    var current = app.index = {};
    //===========================================================================================

    jQuery.extend(app.index,
        {
            Initialize: function (actionUrls) {

                jQuery.extend(ActionIndexUrl, actionUrls);
                fnInicializarCombo();
                fnConfigurarGrillaComision();
                
            }
        })
})(project);


function fnNuevoRegistro() {
    
    $(this).AbrirVentanaEmergente({
        parametro: "?p_codigo_comision=" + -1,
        div: 'div_registrar_comision',
        title: "Registro Comisión",
        url: ActionIndexUrl._Registrar
    });
}

function fnModificar() {
    var nombreOpcion = "Modificar";
    var row = $('#dgv_comision').datagrid('getSelected');
    if (!row) {
        $.messager.alert(nombreOpcion, "Para continuar con el proceso debe seleccionar un registro.", 'warning');
        return;
    }

    if (!EstadoRegistro) {
        $.messager.alert(nombreOpcion, 'Este registro est&aacute; desactivado.', 'warning');
        return false;
    }

    $.ajax({
        type: 'post',
        url: ActionIndexUrl.ValidarReferencia,
        data: {
            codigo_comision_manual: row.codigo_comision_manual
        },
        async: false,
        cache: false,
        dataType: 'json',
        success: function (data) {
            if (data.mensaje == '') {
                $(this).AbrirVentanaEmergente({
                    parametro: "?p_codigo_comision=" + row.codigo_comision_manual,
                    div: 'div_registrar_comision',
                    title: "Modificar Comisión",
                    url: ActionIndexUrl._Registrar
                });
            }
            else {
                //$.messager.alert(nombreOpcion, data.mensaje, 'error');
                $(this).AbrirVentanaEmergente({
                    parametro: "?p_codigo_comision=" + row.codigo_comision_manual,
                    div: 'div_modificar_comision',
                    title: "Modificar Comisión",
                    url: ActionIndexUrl._Modificar
                });
            }
        },
        error: function () {
            $.messager.alert('Error', "Error en el servidor", 'error');
        }
    });
}

function fnDetalle() {
    var nombreOpcion = "Detalle";
    var row = $('#dgv_comision').datagrid('getSelected');
    if (!row) {
        $.messager.alert(nombreOpcion, "Para continuar con el proceso debe seleccionar un registro.", 'warning');
        return;
    }

    $(this).AbrirVentanaEmergente({
        parametro: "?p_codigo_comision=" + row.codigo_comision_manual,
        div: 'div_detalle_comision',
        title: "Detalle Comisión",
        url: ActionIndexUrl._Detalle
    });
}

function fnDesactivar() {
    var nombreOpcion = "Desactivar";
    var row = $('#dgv_comision').datagrid('getSelected');
    if (!row) {
        $.messager.alert(nombreOpcion, "Para continuar con el proceso debe seleccionar un registro.", 'warning');
        return;
    }

    if (!EstadoRegistro) {
        $.messager.alert(nombreOpcion, 'Este registro ya est&aacute; desactivado.', 'warning');
        return false;
    }

    $.messager.confirm('Confirmaci&oacute;n', '¿Est&aacute; seguro que desea desactivar esta comisión?', function (result) {
        if (result) {
            $.ajax({
                type: 'post',
                url: ActionIndexUrl.Desactivar,
                data: {
                    codigo_comision_manual: row.codigo_comision_manual
                },
                async: false,
                cache: false,
                dataType: 'json',
                success: function (data) {
                    if (data.v_resultado == 1) {
                        $.messager.alert('Operación exitosa', data.v_mensaje, 'info', function () {
                            $('#dgv_comision').datagrid('clearSelections');
                            fnReloadGrillaComision();                            
                        });
                    }
                    else {
                        $.messager.alert(nombreOpcion, data.v_mensaje, 'error');
                    }
                },
                error: function () {
                    $.messager.alert('Error', "Error en el servidor", 'error');
                }
            });
        }
    });
}

function fnConfigurarGrillaComision() {
    $('#dgv_comision').datagrid({
        fitColumns: true,
        url: ActionIndexUrl.ListarAllJson,
        idField: 'codigo_comision_manual',
        pagination: true,
        singleSelect: true,
        rownumbers: true,
        //height: '500',
        queryParams: {
            codigo_empresa: $("#cmb_empresa_f").combobox("getValue"),
            codigo_canal: $("#cmb_canal").combobox("getValue"),
            fecha_inicio: FormatoFecha($("#fecha_inicio").datebox("getText")),
            fecha_fin: FormatoFecha($("#fecha_fin").datebox("getText")),
            estado_registro: $("#cmb_estado").combobox("getValue")
        },
        pageList: [20, 60, 80, 100, 150],
        pageSize: 20,
        columns:
        [[
            { field: 'codigo_comision_manual', hidden: 'true' },
            { field: 'estado_registro', hidden: 'true' },
            { field: 'codigo_estado_cuota', hidden: 'true' },
            { field: 'usuario_registra', title: 'Registrado', width: 100, align: 'center', halign: 'center' },
            { field: 'nombre_personal', title: 'Vendedor', width: 220, align: 'left', halign: 'center' },
            { field: 'nombre_fallecido', title: 'Fallecido', width: 220, align: 'left', halign: 'center' },
            { field: 'nro_contrato', title: 'Nro. Contrato', width: 120, align: 'center', halign: 'center' },
            { field: 'nombre_empresa', title: 'Empresa', width: 100, align: 'left', halign: 'center' },
            { field: 'nombre_estado_cuota', title: 'Estado<br>Cuota', width: 90, align: 'left', halign: 'center' },
            { field: 'fecha_registra', title: 'Fecha', width: 160, align: 'center', halign: 'center' },
            { field: 'nombre_estado_proceso', title: 'Proceso', width: 130, align: 'center', halign: 'center' },
            { field: 'nombre_estado_registro', title: 'Estado', width: 90, align: 'center', halign: 'center' },
        ]],
        onClickRow: function (index, row) {
            $("#hdCodigo").val(row.codigo_comision_manual);
            EstadoRegistro = row.estado_registro;
        }
    });
    $('#dgv_comision').datagrid('enableFilter');  
}

function fnReloadGrillaComision() {
    $('#dgv_comision').datagrid('reload');
}

function fnBuscar() {
    if (!$("#frm_consultar").form('enableValidation').form('validate'))
        return;

    var queryParams = {
        codigo_empresa: $("#cmb_empresa_f").combobox("getValue"),
        codigo_canal: $("#cmb_canal").combobox("getValue"),
        fecha_inicio: FormatoFecha($("#fecha_inicio").datebox("getText")),
        fecha_fin: FormatoFecha($("#fecha_fin").datebox("getText")),
        estado_registro: $("#cmb_estado").combobox("getValue"),
    };

    $('#dgv_comision').datagrid("reload", queryParams);
}


function fnVerReporte() {
    if (!$("#frm_consultar").form('enableValidation').form('validate'))
        return;
    var queryParams = {
        codigo_empresa: $("#cmb_empresa_f").combobox("getValue"),
        codigo_canal: $("#cmb_canal").combobox("getValue"),
        fecha_inicio: FormatoFecha($("#fecha_inicio").datebox("getText")),
        fecha_fin: FormatoFecha($("#fecha_fin").datebox("getText")),
        estado_registro: $("#cmb_estado").combobox("getValue"),
    };
    $.ajax({
        type: 'post',
        url: ActionIndexUrl.SetFiltroGrilla,
        data: JSON.stringify({ v_entidad: queryParams }),
        async: true,
        cache: false,
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            window.open(data.v_url, '_blank');
        },
        error: function (data) {

            $.messager.alert('Error', "Error en el servidor", 'error');
        }
    });

}

function fnInicializarCombo() {
    var empresa = {};
    var canal = {};
    var elementoTodos = { "id": "0", "text": "Todos" };

    $.ajax({
        type: 'post',
        url: ActionIndexUrl.GetEmpresaJson,
        data: null,
        async: false,
        cache: false,
        dataType: 'json',
        success: function (data) {
            data.push(elementoTodos);
            empresa = data;
        },
        error: function () {
            empresa = {};
        }
    });

    $.ajax({
        type: 'post',
        url: ActionIndexUrl.GetCanalJson,
        data: null,
        async: false,
        cache: false,
        dataType: 'json',
        success: function (data) {
            data.push(elementoTodos);
            canal = data;
        },
        error: function () {
            canal = {};
        }
    });

    $('#cmb_empresa_f').combobox({
        valueField: 'id',
        textField: 'text',
        data: empresa
    });

    $('#cmb_canal').combobox({
        valueField: 'id',
        textField: 'text',
        data: canal
    });

    $('#fecha_inicio').formatteDate();
    $('#fecha_fin').formatteDate();

    $('#cmb_estado').combobox({
        valueField: 'id',
        textField: 'text',
        data: [{ id: '-1', text: 'Todos' }, { id: '1', text: 'Activo' }, { id: '0', text: 'Inactivo' }],
    });
    $('#cmb_estado').combobox('setValue', '-1');
}

function FormatoFecha(fecha) {
    return (fecha.length > 1 ? fecha.substring(6, 10) + fecha.substring(3, 5) + fecha.substring(0, 2) : "");
}
