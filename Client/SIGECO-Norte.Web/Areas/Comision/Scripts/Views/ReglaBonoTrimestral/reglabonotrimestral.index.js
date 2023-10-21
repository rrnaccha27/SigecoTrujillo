var ActionIndexUrl = {};

; (function (app) {
    //===========================================================================================
    var current = app.reglabonotrimestral_index = {};
    //===========================================================================================
    jQuery.extend(app.reglabonotrimestral_index,
        {
            Initialize: function (actionUrls) {
                jQuery.extend(ActionIndexUrl, actionUrls);
                fnConfigurarGrillaRegla();
            }
        })
})(project);


function fnConfigurarGrillaRegla() {

    $('#dgv_lista_regla').datagrid({
        fitColumns: true,
        url: ActionIndexUrl._GetAllJson,
        idField: 'codigo_regla',
        pagination: true,
        singleSelect: true,               
        rownumbers: true,        
        pageList: [20, 60, 80,100, 150],
        pageSize: 20,        
        columns: [[
            { field: 'codigo_regla', hidden: true },            
            { field: 'descripcion', title: 'Descripción', width: 150, align: 'left' },
            { field: 'nombre_tipo_bono', title: 'Tipo Bono', width: 150, align: 'left' },
            { field: 'vigencia', title: 'Vigencia', width: 150, align: 'center' },
            { field: 'usuario_registra', title: 'Usuario Registra', width: 200, align: 'left' },
            { field: 'fecha_registra', title: 'Fecha Registro', width: 150, align: 'center' },
            { field: 'nombre_estado_registro', title: 'Estado', width: 90, align: 'center' },
            { field: 'indica_estado', hidden: true, formatter: function (value, row, index) { if (row.nombre_estado_registro == 'Activo') { return '1'; } else { return '0'; } } }
        ]],
        onClickRow: function (index, row) {
            if (row['str_estado_registro'] == 'Inactivo')
            {
                $("#btnEliminar").linkbutton('disable');               
            }
            else
            {
                $("#btnEliminar").linkbutton('enable');
            }
        }
    });
    
    /******************************************************************************************************/
    $('#dgv_lista_regla').datagrid('enableFilter', [{
        field: 'nombre_estado_registro',
        type: 'combobox',
        options: {
            panelHeight: 'auto',
            data: [{ value: '', text: 'Todos' }, { value: '1', text: 'Activo' }, { value: '0', text: 'Inactivo' }],
            editable:false,
            onChange: function (value) {

                if (value == '') {
                    $('#dgv_lista_regla').datagrid('removeFilterRule', 'indica_estado');
                } else {
                    $('#dgv_lista_regla').datagrid('addFilterRule', {
                        field: 'indica_estado',
                        op: 'equal',
                        value: value
                    });
                }
                $('#dgv_lista_regla').datagrid('doFilter');
            }
        }
    }]);
    /*********************************************************************************************************/
    $(window).resize(function () {
        $('#dgv_lista_regla').datagrid('resize');
    });   
}

function fnBuscarRegla()
{
    $('#dgv_lista_regla').datagrid("reload");
}

function fnNuevoReglaBonoTrimestral() {    
    $(this).AbrirVentanaEmergente({
        parametro: "?p_codigo_regla=" + 0,
        div: 'div_registrar_regla',
        title: "Configuración de Planilla",
        url: ActionIndexUrl._NuevaRegla
    });
}

function fnModificarReglaBonoTrimestral() {
    var row = $('#dgv_lista_regla').datagrid('getSelected');    

    if (!row)
    {
        $.messager.alert('Seleccione registro', "Para continuar con el proceso debe seleccionar un registro.", 'warning');
        return;
    };

    $(this).AbrirVentanaEmergente({
        parametro: "?p_codigo_regla=" + row.codigo_regla,
        div: 'div_registrar_regla',
        title: "Mantenimiento de Regla Tipo Planilla",
        url: ActionIndexUrl._NuevaRegla
    });
}

function Eliminar() {
    var row = $('#dgv_lista_regla').datagrid('getSelected');

    if (!row) {
        $.messager.alert('Seleccione registro', "Para continuar con el proceso debe seleccionar un registro.", 'warning');
        return;
    }

    $.messager.confirm('Confirmación', 'Seguro que desea desactivar este registro?', function (result) {
        if (result) {
            $.ajax({
                type: 'post',
                url: ActionIndexUrl._Eliminar,
                data: { p_codigo_regla: row.codigo_regla },
                async: false,
                cache: false,
                dataType: 'json',
                success: function (data) {
                    if (data.v_resultado == 1) {
                        $.messager.alert('Operación exitosa', data.v_mensaje, 'info', function () {
                            fnBuscarRegla();
                        });
                    }
                    else {
                        $.messager.alert('Error en la operación', data.v_mensaje, 'error');
                    }
                },
                error: function () {
                    $.messager.alert('Error', "Error en el servidor", 'error');
                }
            });
        }
    });
}