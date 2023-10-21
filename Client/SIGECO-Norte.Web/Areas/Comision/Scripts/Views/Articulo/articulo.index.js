var ActionIndexUrl = {};

; (function (app) {
    //===========================================================================================
    var current = app.articulo_index = {};
    //===========================================================================================
    jQuery.extend(app.articulo_index,
        {
            Initialize: function (actionUrls) {
                jQuery.extend(ActionIndexUrl, actionUrls);
                fnConfigurarGrillaArticulo();
                fnConfigurarBotones();
            }
        })
})(project);


function fnConfigurarBotones() {
    $("#btnActivar").on("click", function () {
        if (!$("#btnActivar").linkbutton("options").disabled) {
            Activar();
        }
    });
}

function fnConfigurarGrillaArticulo() {

    $('#dgv_lista_articulo').datagrid({
        fitColumns: true,
        url: ActionIndexUrl._GetAllJson,
        idField: 'codigo_articulo',
        pagination: true,
        singleSelect: true,
        //toolbar: "#toolbar",
       // view: bufferview, no aplicar buffer
        rownumbers: true,
        //pageNumber: 1,
        pageList: [20, 60, 80,100, 150],
        pageSize: 20,
        queryParams: {
            nombre: ""
            //iCurrentPage: 1,
            //iPageSize: -1
        },
        columns: [[
            { field: 'codigo_articulo', hidden: true },
            { field: 'codigo_sku', title: 'Código SKU', width: 120, align: 'left' },
            { field: 'nombre', title: 'Nombre', width: 330, align: 'left' },
            { field: 'str_genera_comision', title: 'Comisiona', width: 70, align: 'center' },
            { field: 'str_genera_bono', title: 'Dinero<br>Ingresado', width: 70, align: 'center' },
            { field: 'str_bolsa_bono', title: 'Monto<br>Contratado', width: 70, align: 'center' },
            { field: 'str_tiene_precio', title: 'Tiene<br>Precio', width: 70, align: 'center' },
            { field: 'str_tiene_comision', title: 'Tiene<br>Comision', width: 70, align: 'center' },
            { field: 'str_estado_registro', title: 'Estado', width: 80, align: 'center' }
        ]],
        onClickRow: function (index, row) {
            var rowColumn = row['codigo_articulo'];
            if (row['str_estado_registro'] == 'Inactivo')
            {
                $("#btnEliminar").linkbutton('disable');
                $("#btnModificar").linkbutton('disable');
                $("#btnActivar").linkbutton('enable');
            }
            else
            {
                $("#btnEliminar").linkbutton('enable');
                $("#btnModificar").linkbutton('enable');
                $("#btnActivar").linkbutton('disable');
            }
            $("#hdCodigo").val(rowColumn);
        },
        onRowContextMenu: function (e, index, row) {
            if (index >= 0) {
                $(this).datagrid('selectRow', index);
                e.preventDefault();
                $('#mm').menu('show', {
                    left: e.pageX,
                    top: e.pageY
                });
            }
        },
        loadFilter: function (data) {
            ClearSelection();
            return data;
        }
    });

    $('#dgv_lista_articulo').datagrid('enableFilter', [{
        field: 'str_estado_registro',
        type: 'combobox',
        options: {
            editable:false,
            panelHeight: 'auto',
            data: [{ value: '', text: 'Todos' }, { value: 'Activo', text: 'Activo' }, { value: 'Inactivo', text: 'Inactivo' }],
            onChange: function (value) {
                if (value == '') {
                    $('#dgv_lista_articulo').datagrid('removeFilterRule', 'str_estado_registro');
                } else {
                    $('#dgv_lista_articulo').datagrid('addFilterRule', {
                        field: 'str_estado_registro',
                        op: 'equal',
                        value: value
                    });
                }
                $('#dgv_lista_articulo').datagrid('doFilter');
            }
        }
    }]);
    
    $(window).resize(function () {
        $('#dgv_lista_articulo').datagrid('resize');
    });

}

function fnBuscarArticulo() {
    var queryParams= {
        nombre: $("#txt_nombre_articulo").textbox("getText")
    };
    $('#dgv_lista_articulo').datagrid("reload", queryParams);
}

function fnCopiar(atributo) {
    row = $('#dgv_lista_articulo').datagrid("getSelected")
    if (!row)
    { return false; }
    var $temp = $("<input>");
    $("body").append($temp);
    $temp.val(row[atributo]).select();
    document.execCommand("copy");
    $temp.remove();
}

function fnNuevoArticulo() {

    $(this).AbrirVentanaEmergente({
        parametro: "?p_codigo_articulo=" + 0,
        div: 'div_registrar_articulo',
        title: "Mantenimiento de Articulo",
        url: ActionIndexUrl._Registrar
    });
}

function fnModificarArticulo() {

    var codigo = $.trim($("#hdCodigo").val());
    if (!codigo) {
        $.messager.alert('Modificar', 'Por favor seleccione un registro.', 'warning');
        return false;
    }
    $(this).AbrirVentanaEmergente({
        parametro: "?p_codigo_articulo=" + codigo,
        div: 'div_registrar_articulo',
        title: "Mantenimiento de Articulo",
        url: ActionIndexUrl._Registrar
    });

}
function fnModificarArticuloByCodigo(p_codigo_articulo) {

    $(this).AbrirVentanaEmergente({
        parametro: "?p_codigo_articulo=" + p_codigo_articulo,
        div: 'div_registrar_articulo',
        title: "Mantenimiento de Articulo",
        url: ActionIndexUrl._Registrar
    });

}

function Eliminar() {

    var row = $('#dgv_lista_articulo').datagrid('getSelected');

    if (!row) {
        $.messager.alert('Desactivar', "Para continuar con el proceso debe seleccionar un registro.", 'warning');
        return;
    }

    $.messager.confirm('Confirmación', 'Seguro que desea desactivar este registro?', function (result) {
        if (result) {
            $.ajax({
                type: 'post',
                url: ActionIndexUrl._Eliminar,
                data: { p_codigo_articulo: row.codigo_articulo },
                async: false,
                cache: false,
                dataType: 'json',
                success: function (data) {

                    if (data.v_resultado == 1) {
                        $.messager.alert('Operación exitosa', data.v_mensaje, 'info', function () {
                            fnBuscarArticulo();
                        });
                    }
                    else {
                        $.messager.alert('Error en el proceso', data.v_mensaje, 'error');
                    }
                },
                error: function () {
                    project.AlertErrorMessage('Error', 'Error');
                }
            });
        }
    });
}

function Activar() {
    var row = $('#dgv_lista_articulo').datagrid('getSelected');

    if (!row) {
        $.messager.alert('Activar', "Para continuar con el proceso debe seleccionar un registro.", 'warning');
        return;
    }

    $.messager.confirm('Confirmación', 'Seguro que desea activar este registro?', function (result) {
        if (result) {
            $.ajax({
                type: 'post',
                url: ActionIndexUrl._Activar,
                data: { p_codigo_articulo: row.codigo_articulo },
                async: false,
                cache: false,
                dataType: 'json',
                success: function (data) {
                    if (data.v_resultado == 1) {
                        $.messager.alert('Operación exitosa', data.v_mensaje, 'info', function () {
                            fnBuscarArticulo();
                        });
                    }
                    else {
                        $.messager.alert('Error en el proceso', data.v_mensaje, 'error');
                    }
                },
                error: function () {
                    project.AlertErrorMessage('Error', 'Error');
                }
            });
        }
    });
}

function fnVisualizarArticulo() {
    var codigo = $.trim($("#hdCodigo").val());
    if (!codigo) {
        $.messager.alert('Detalle', 'Por favor seleccione un registro.', 'warning');
        return false;
    }

    var row = $('#dgv_lista_articulo').datagrid('getSelected');

    if (!row) {
        $.messager.alert('Detalle', "Por favor seleccione un registro.", 'warning');
        return;
    }

    $(this).AbrirVentanaEmergente({
        parametro: "?p_codigo_articulo=" + codigo,
        div: 'div_visualizar_articulo',
        title: "Datos de Articulo",
        url: ActionIndexUrl._Visualizar
    });
}

function ClearSelection() {
    $('#dgv_lista_articulo').datagrid('clearSelections');
    $("#hdCodigo").val('');
}
