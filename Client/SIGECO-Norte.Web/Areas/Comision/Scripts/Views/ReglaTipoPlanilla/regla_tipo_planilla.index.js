var ActionIndexUrl = {};

; (function (app) {
    //===========================================================================================
    var current = app.regla_tipo_planilla_index = {};
    //===========================================================================================
    jQuery.extend(app.regla_tipo_planilla_index,
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
        idField: 'codigo_regla_tipo_planilla',
        pagination: true,
        singleSelect: true,               
        rownumbers: true,        
        pageList: [20, 60, 80,100, 150],
        pageSize: 20,        
        columns: [[
            { field: 'codigo_regla_tipo_planilla', hidden: true },            
            { field: 'nombre', title: 'Nombre', width: 250, align: 'left' },
            { field: 'nombre_tipo_planilla', title: 'Tipo Planilla', width: 250, align: 'left' },
            { field: 'afecto_doc_completa', title: 'Doc.<br>Completa', width: 80, align: 'center' },
            { field: 'usuario_registra', title: 'Usuario Registra', width: 250, align: 'left' },
            { field: 'str_fecha_registra', title: 'Fecha Registro', width: 100, align: 'center' },
            { field: 'str_estado_registro', title: 'Estado', width: 90, align: 'center' },
            { field: 'indica_estado', hidden: true, formatter: function (value, row, index) { if (row.str_estado_registro == 'Activo') { return '1'; } else { return '0'; } } }
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
        field: 'str_estado_registro',
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



function fnNuevoReglaTipoPlanilla() {    
    $(this).AbrirVentanaEmergente({
        parametro: "?p_codigo_regla_tipo_planilla=" + 0,
        div: 'div_registrar_regla',
        title: "Configuración de Planilla",
        url: ActionIndexUrl._NuevaRegla
    });
}


function fnModificarReglaTipoPlanilla() {

    var row = $('#dgv_lista_regla').datagrid('getSelected');    
    if (!row)
    {
        $.messager.alert('Seleccione registro', "Para continuar con el proceso debe seleccionar un registro.", 'warning');
        return;
    };
    

    $(this).AbrirVentanaEmergente({
        parametro: "?p_codigo_regla_tipo_planilla=" + row.codigo_regla_tipo_planilla,
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
                data: { p_codigo_regla_tipo_planilla: row.codigo_regla_tipo_planilla },
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