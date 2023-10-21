var IndexUrls = {};

;(function (app) {
    //===========================================================================================
    var current = app.DescuentoComisionIndex = {};
    //===========================================================================================

    jQuery.extend(app.DescuentoComisionIndex,
        {
            EditType: '',
            HasRootNode: 'False',

            Initialize: function (indexUrls) {
                jQuery.extend(IndexUrls, indexUrls);
                ConfigurarGrillaListado();
            }
        })
})(project);

function ConfigurarGrillaListado() {
    $('#DataGrid').datagrid({
        url: IndexUrls.GetAllJson,
        fitColumns: true,
        idField: 'codigo_descuento_comision',
        data: null,
        pagination: true,
        singleSelect: true,
        rownumbers: true,
        pageList: [20, 60, 80, 100, 150],
        pageSize: 20,
        columns:
        [[
            { field: 'codigo_descuento_comision', title: 'Codigo', hidden: 'true' },
            { field: 'nombre_personal', title: 'Vendedor', width: 350, align: 'left', halign: 'center' },
            { field: 'nombre_empresa', title: 'Empresa', width: 100, align: 'left', halign: 'center' },
            {
                field: 'monto', title: 'Monto', width: 80, align: 'right', halign: 'center', formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                }
            },
            {
                field: 'saldo', title: 'Saldo', width: 80, align: 'right', halign: 'center', formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                }
            },
            { field: 'fecha_registra', title: 'Fecha<br>Registro', width: 100, align: 'center', halign: 'center' },
            { field: 'usuario_registra', title: 'Usuario<br>Registra', width: 200, align: 'left', halign: 'center' },
            { field: 'nombre_estado_registro', title: 'Estado', width: 100, align: 'center', halign: 'center' },
            { field: 'estado_registro', hidden: true },
        ]],
        onClickRow: function (index, row) {
            if (row.estado_registro == 1) {
                $("#btnDesactivar").linkbutton('enable');
            }
            else {
                $("#btnDesactivar").linkbutton('disable');
            }
        },
        loadFilter: function (data) {
            ClearSelection();
            return data;
        }
    });
    /******************************************************************************************************/
    $('#DataGrid').datagrid('enableFilter', [{
        field: 'nombre_estado_registro',
        type: 'combobox',
        options: {
            panelHeight: 'auto',
            data: [{ value: '', text: 'Todos' }, { value: '1', text: 'Activo' }, { value: '0', text: 'Inactivo' }],
            onChange: function (value) {

                if (value == '') {
                    $('#DataGrid').datagrid('removeFilterRule', 'estado_registro');
                } else {
                    $('#DataGrid').datagrid('addFilterRule', {
                        field: 'estado_registro',
                        op: 'equal',
                        value: value
                    });
                }
                $('#DataGrid').datagrid('doFilter');
            }
        }
    }]);
    /*********************************************************************************************************/
    
	$("#btnDesactivar").on("click", function () {
        if (!$("#btnDesactivar").linkbutton("options").disabled) {
            Desactivar();
        }
    });

}

function GetAllJson() {
    var queryParams = {};
    $('#DataGrid').datagrid('reload', queryParams);
    ClearSelection();
}

function Crear() {
    $(this).AbrirVentanaEmergente({
        parametro: "?codigo_descuento_comision=-1",
        div: 'dlgRegistro',
        title: "Mantenimiento de Descuento",
        url: IndexUrls._Registro
    });
}

function Detalle() {
    var row = $('#DataGrid').datagrid('getSelected');

    if (!row) {
        $.messager.alert('Detalle', 'Por favor seleccione un registro', 'warning');
        return false;
    }
    $(this).AbrirVentanaEmergente({
        parametro: "?codigo_descuento_comision=" + row.codigo_descuento_comision,
        div: 'dlgDetalle',
        title: "Detalle de Descuento",
        url: IndexUrls._Detalle
    });
}

function Desactivar() {
	var row = $('#DataGrid').datagrid('getSelected');

    if (!row) {
        $.messager.alert('Desactivar', 'Por favor seleccione un registro', 'warning');
        return false;
    }
    $.messager.confirm('Confirm', '¿Seguro que desea desactivar este registro?', function (result) {
        if (result) {
            $.ajax({
                type: 'post',
                url: IndexUrls.Desactivar,
                data: { codigo_descuento_comision: row.codigo_descuento_comision },
                async: false,
                cache: false,
                dataType: 'json',
                success: function (data) {
                    if (data.Msg) {
                        if (data.Msg != 'Success') {
                            $.messager.alert('Desactivar', data.Msg, 'warning');
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
}
