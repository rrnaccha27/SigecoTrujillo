var IndexUrls = {};
var listaTipoRegla = [];

;(function (app) {
    //===========================================================================================
    var current = app.ReglaCalculoBonoIndex = {};
    //===========================================================================================

    jQuery.extend(app.ReglaCalculoBonoIndex,
        {
            EditType: '',
            HasRootNode: 'False',

            Initialize: function (indexUrls) {
                jQuery.extend(IndexUrls, indexUrls);
                LlenarTipoRegla();
                ConfigurarGrillaListado();
            }
        })
})(project);


function ConfigurarGrillaListado() {
    $('#DataGrid').datagrid({
        url: IndexUrls.GetAllJson,
        queryParams: { codigo_tipo_planilla: '-1' },
        fitColumns: true,
        idField: 'codigo_regla_calculo_bono',
        data: null,
        pagination: true,
        singleSelect: true,
        rownumbers: true,
        pageList: [20, 60, 80, 100, 150],
        pageSize: 20,
        columns:
        [[
            { field: 'codigo_regla_calculo_bono', title: 'Codigo', hidden: 'true' },
            { field: 'nombre_tipo_planilla', title: 'Tipo Regla', width: 80, align: 'left', halign: 'center' },
            { field: 'calcular_igv', title: 'Calcular<br>IGV', width: 40, align: 'center', halign: 'center' },
            { field: 'canal_nombre', title: 'Canal', width: 100, align: 'left', halign: 'center' },
            { field: 'grupo_nombre', title: 'Grupo', width: 100, align: 'left', halign: 'center' },
            { field: 'vigencia_inicio', title: 'Vigencia Inicio', width: 80, align: 'center', halign: 'center' },
            { field: 'vigencia_fin', title: 'Vigencia Fin', width: 80, align: 'center', halign: 'center' },
            { field: 'estado_registro', title: 'Estado', width: 80, align: 'center', halign: 'center' },
            { field: 'indica_estado', hidden: true, formatter: function (value, row, index) { if (row.estado_registro == 'Activo') { return '1'; } else { return '0'; } } }
        ]],
        onClickRow: function (index, row) {
            var rowColumn = row['codigo_regla_calculo_bono'];
            $("#hdCodigo").val(rowColumn);
            EvalBotones(row['estado_registro'], true);
        }
    });
    /******************************************************************************************************/
    $('#DataGrid').datagrid('enableFilter', [{
        field: 'estado_registro',
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

    $('#codigo_tipo_planilla').combobox({
        valueField: 'id',
        textField: 'text',
        data: listaTipoRegla
    });

    $('#codigo_tipo_planilla').combobox({ editable: false });
    $('#codigo_tipo_planilla').combobox('setValue', listaTipoRegla[0].id);
    $("#btnDesactivar").on('click', function () { Desactivar(); });
    $("#btnDetalle").on('click', function () { Detalle(); });
}

function ObtenerFiltro(nombreCombo) {
    var codigo = $.trim($('#' + nombreCombo).combobox('getValue'));
    if (!codigo) {
        codigo = '-1';
    }
    return codigo;
}

function GetAllJson() {
    var queryParams = { codigo_tipo_planilla: ObtenerFiltro('codigo_tipo_planilla') };
    $('#DataGrid').datagrid('reload', queryParams);
    ClearSelection();
    EvalBotones('', false);
}

function Crear() {
    AbrirVentanaPersonal(-1, true);
}

function Modificar() {
    var codigo = $.trim($("#hdCodigo").val());
    if (!codigo) {
        $.messager.alert('Modificar', 'Por favor seleccione un registro', 'warning');
        return false;
    }

    if ($("#hdEstado").val() == 'false') {
        $.messager.alert('Modificar', 'No se puede modificar registro inactivo.', 'warning');
        return false;
    }

    AbrirVentanaPersonal(codigo, false);
}

function Detalle() {
    var codigo = $.trim($("#hdCodigo").val());

    if ($('#btnDetalle').linkbutton('options').disabled) { return false };

    if (!codigo) {
        $.messager.alert('Detalle', 'Por favor seleccione un registro', 'warning');
        return false;
    }

    //if ($("#hdEstado").val() == 'false') {
    //    $.messager.alert('Modificar', 'No se puede modificar personal inactivo.', 'warning');
    //    return false;
    //}

    AbrirVentanaPersonal(codigo, false);
}

function Desactivar() {
    var codigo = $("#hdCodigo").val();

    if ($('#btnDesactivar').linkbutton('options').disabled) { return false };

    if (!codigo) {
        $.messager.alert('Desactivar', 'Por favor seleccione un registro', 'warning');
        return false;
    }
    $.messager.confirm('Confirm', '¿Seguro que desea desactivar este registro?', function (result) {
        if (result) {
            $.ajax({
                type: 'post',
                url: IndexUrls.Desactivar,
                data: { codigo_regla_calculo_bono: codigo },
                async: false,
                cache: false,
                dataType: 'json',
                success: function (data) {
                    if (data.Msg) {
                        if (data.Msg != 'Success') {
                            $.messager.alert('Error', data.Msg, 'error');
                        }
                        else {
                            project.ShowMessage('Alerta', 'Se Desactivó con éxito');
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

function ClearSelection() {
    $('#DataGrid').datagrid('clearSelections');
    $("#hdCodigo").val('');
}

function LlenarTipoRegla() {
    listaTipoRegla.push({ "id": "-1", "text": "Todos" });
    listaTipoRegla.push({ "id": "1", "text": "Personal" });
    listaTipoRegla.push({ "id": "0", "text": "Supervisor" });
}

function AbrirVentanaPersonal(codigo_regla_calculo_bono, tipo) {
    $(this).AbrirVentanaEmergente({
        parametro: "?codigo_regla_calculo_bono=" + codigo_regla_calculo_bono,
        div: (tipo == true ? 'dlgRegistro' : 'dlgDetalle'),
        title: (tipo == true ? "Mantenimiento" : "Detalle") + " de Regla Cálculo Bono",
        url: (tipo == true ? IndexUrls.Registro : IndexUrls.Detalle)
    });
}

function EvalBotones(valor, habilitar) {
    $("#btnDesactivar").linkbutton((valor == 'Activo' ? 'enable' : 'disable'));
    $("#btnDetalle").linkbutton((habilitar ? 'enable' : 'disable'));
}
