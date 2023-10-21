var ActionNuevoUrl = {};

var JsonCanal = {};
var JsonEmpresa = {};
var JsonCampoSanto = {};
var JsonTipoVenta = {};

function cananFormatter(value) {

    for (var i = 0; i < JsonCanal.length; i++) {
        if (JsonCanal[i].id == value) return JsonCanal[i].text;
    }
    return value;
}
function empresaFormatter(values) {
    if (values == null)
        return values;

    var nombres = null;
    var existe = false;
    var codigos = values.split(',');
    $.each(codigos, function (index, value) {
        for (var i = 0; i < JsonEmpresa.length; i++) {
            if (JsonEmpresa[i].id == value) {
                existe = true;
                if (nombres == null) {
                    nombres = JsonEmpresa[i].text;
                }
                else {
                    nombres = nombres + "," + JsonEmpresa[i].text;
                }

            }
        }
    });
    if (existe) {
        return nombres;
    }
    return values;
}
function tipoVentaFormatter(values) {

    if (values == null)
        return values;

    var nombres = null;
    var existe = false;
    var codigos = values.split(',');
    $.each(codigos, function (index, value) {
        for (var i = 0; i < JsonTipoVenta.length; i++) {
            if (JsonTipoVenta[i].id == value) {
                existe = true;
                if (nombres == null) {
                    nombres = JsonTipoVenta[i].text;
                }
                else {
                    nombres = nombres + "," + JsonTipoVenta[i].text;
                }

            }
        }
    });
    if (existe) {
        return nombres;
    }
    return values;

}
function campoSantoFormatter(values) {

    if (values == null)
        return values;

    var nombres = null;
    var existe = false;
    var codigos = values.split(',');
    $.each(codigos, function (index, value) {
        for (var i = 0; i < JsonCampoSanto.length; i++) {
            if (JsonCampoSanto[i].id == value) {
                existe = true;
                if (nombres == null) {
                    nombres = JsonCampoSanto[i].text;
                }
                else {
                    nombres = nombres + "," + JsonCampoSanto[i].text;
                }
            }
        }
    });
    if (existe) {
        return nombres;
    }
    return values;


}


; (function (app) {
    //===========================================================================================
    var current = app.regla_tipo_planilla_nuevo = {};
    //===========================================================================================
    jQuery.extend(app.regla_tipo_planilla_nuevo,
        {
            Initialize: function (actionUrls) {
                jQuery.extend(ActionNuevoUrl, actionUrls);
                fnInicializarDetalleRegla();
                fnConfigurarGrillaDetalleRegla();
            }
        })
})(project);


function fnInicializarDetalleRegla() {
    $('.content').combobox_sigees({
        id: '#cmb_tipo_planilla',
        url: ActionNuevoUrl._GetTipoPlanillaJson
    });

    /*$('.content').combobox_sigees({
        id: '#cmb_tipo_reporte',
        url: ActionNuevoUrl._GetTipoReporteJson
    });*/

    JsonCampoSanto = $('.content').get_json_combobox({
        url: ActionNuevoUrl._GetCampoSantoJson
    });


    JsonEmpresa = $('.content').get_json_combobox({
        url: ActionNuevoUrl._GetEmpresaJson
    });

    JsonCanal = $('.content').get_json_combobox({
        url: ActionNuevoUrl._GetCanalJson
    });

    JsonTipoVenta = $('.content').get_json_combobox({
        url: ActionNuevoUrl._GetTipoVentaJson
    });


}

function fnConfigurarGrillaDetalleRegla() {

    $('#dgv_lista_detalle_regla').datagrid({
        fitColumns: true,
        url: ActionNuevoUrl._GetDetalleAllJson,
        queryParams: {
            p_codigo_regla_tipo_planilla: ActionNuevoUrl._codigo_regla_tipo_planilla
        },
        idField: 'codigo_detalle_regla_tipo_planilla',
        pagination: false,
        singleSelect: true,
        rownumbers: true,
        toolbar:'#tlb_detalle_regla',
        columns: [[
            { field: 'codigo_regla_tipo_planilla', hidden: true },
            { field: 'codigo_detalle_regla_tipo_planilla', hidden: true },
            {
                field: 'codigo_canal', title: 'Canal', width: 150, align: 'left',
                editor: {
                    type: 'combobox',
                    options: {
                        valueField: 'id',
                        textField: 'text',
                        data: JsonCanal,
                        editable: false,
                        required: true
                    }
                },
                formatter: cananFormatter
            },
            {
                field: 'codigo_empresa', title: 'Empresa', width: 150, align: 'left', editor: {
                    type: 'combobox',
                    options: {
                        valueField: 'id',
                        textField: 'text',
                        data: JsonEmpresa,
                        multiple: true,
                        editable: false,
                        required: true
                    }
                },
                formatter: empresaFormatter
            },
            {
                field: 'codigo_tipo_venta', title: 'Tipo de Venta', width: 200, align: 'left',
                editor: {
                    type: 'combobox',
                    options: {
                        valueField: 'id',
                        textField: 'text',
                        data: JsonTipoVenta,
                        editable: false,
                        multiple: true,
                        required: true
                    }
                },
                formatter: tipoVentaFormatter
            },
            {
                field: 'codigo_campo_santo', title: 'Campo Santo', width: 300, align: 'left', editor: {
                    type: 'combobox',
                    options: {
                        valueField: 'id',
                        textField: 'text',
                        data: JsonCampoSanto,
                        //novalidate: true,
                        editable: false,
                        multiple: true,
                        required: true
                    }
                },
                formatter: campoSantoFormatter
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
        onClickRow: function (rowIndex, rowdata) {
            if (!$.existeRegistroEnEdicion("dgv_lista_detalle_regla")) {
                if (rowdata.codigo_detalle_regla_tipo_planilla<=0) {
                    $("#btn_detalle_regla_quitar").linkbutton('enable');
                } else {
                    $("#btn_detalle_regla_quitar").linkbutton('disable');
                }

            }
        },
        onDblClickRow: function (rowIndex, rowdata) {

            if (rowdata.codigo_detalle_regla_tipo_planilla > 0) {
                $.messager.alert("Configurar planilla", "El registro seleccionado no se puede editar, intente nuevamente.", "warning");
                return;
            }
            if (!$.existeRegistroEnEdicion("dgv_lista_detalle_regla")) {
                $('#dgv_lista_detalle_regla').datagrid('beginEdit', rowIndex);
                rowdata.editing = true;
                rowdata.confirmado = true;

                var cmb_empresa = $("#dgv_lista_detalle_regla").datagrid("getEditor", { index: rowIndex, field: 'codigo_empresa' });
                var cmb_tipo_venta = $("#dgv_lista_detalle_regla").datagrid("getEditor", { index: rowIndex, field: 'codigo_tipo_venta' });
                var cmb_campo_santo = $("#dgv_lista_detalle_regla").datagrid("getEditor", { index: rowIndex, field: 'codigo_campo_santo' });

                if ($(cmb_empresa.target).data('combobox')) {
                    var cod_em = $(cmb_empresa.target).combobox('getValues')
                    fnFormatComboboxCheck(cmb_empresa.target);
                    $(cmb_empresa.target).combobox('setValue', cod_em);
                }
                if ($(cmb_tipo_venta.target).data('combobox')) {
                    var cod_tv = $(cmb_tipo_venta.target).combobox('getValues')
                    fnFormatComboboxCheck(cmb_tipo_venta.target);
                    $(cmb_tipo_venta.target).combobox('setValue', cod_tv);
                }
                if ($(cmb_campo_santo.target).data('combobox')) {
                    var cod_cs = $(cmb_campo_santo.target).combobox('getValues')
                    fnFormatComboboxCheck(cmb_campo_santo.target);
                    $(cmb_campo_santo.target).combobox('setValue', cod_cs);
                }

                /*******************************************/
                rowdata.editing = true;
                rowdata.confirmado = true;
                $("#btn_detalle_regla_crear").linkbutton('disable');
                $("#btn_detalle_regla_aceptar").linkbutton('enable');
                $("#btn_detalle_regla_cancelar").linkbutton('enable');
                $("#btn_detalle_regla_quitar").linkbutton('disable');
                $("#btnGuardar").linkbutton('disable');
            }
        }
    });
    $('#dgv_lista_detalle_regla').datagrid('enableFilter');
    $(window).resize(function () {
        $('#dgv_lista_detalle_regla').datagrid('resize');
    });
}



function NuevoDetalleRegla() {
    if (!$.existeRegistroEnEdicion('dgv_lista_detalle_regla')) {
        var new_row = {
            codigo_detalle_regla_tipo_planilla: -new Date().toString("ddHHmmss")
            , codigo_canal: null
            , codigo_empresa: null
            , codigo_Campo_santo: null
            , codigo_tipo_venta: null
            , confirmado: false
            , editing: true
        }

        $("#dgv_lista_detalle_regla").datagrid("appendRow", new_row);
        var editIndex = $("#dgv_lista_detalle_regla").datagrid("getRowIndex", new_row.codigo_detalle_regla_tipo_planilla);
        fnEditarDetalleRegla(editIndex);
        /************************************/
        $("#btn_detalle_regla_crear").linkbutton('disable');
        $("#btn_detalle_regla_aceptar").linkbutton('enable');
        $("#btn_detalle_regla_cancelar").linkbutton('enable');
        $("#btn_detalle_regla_quitar").linkbutton('disable');
        $("#btnGuardar").linkbutton('disable');
    }
}

function fnEditarDetalleRegla(editIndex) {
    $("#dgv_lista_detalle_regla").datagrid("selectRow", editIndex);
    $("#dgv_lista_detalle_regla").datagrid("beginEdit", editIndex);

    var cmb_empresa = $("#dgv_lista_detalle_regla").datagrid("getEditor", { index: editIndex, field: 'codigo_empresa' });
    var cmb_tipo_venta = $("#dgv_lista_detalle_regla").datagrid("getEditor", { index: editIndex, field: 'codigo_tipo_venta' });
    var cmb_campo_santo = $("#dgv_lista_detalle_regla").datagrid("getEditor", { index: editIndex, field: 'codigo_campo_santo' });

    if ($(cmb_empresa.target).data('combobox')) {
        fnFormatComboboxCheck(cmb_empresa.target);
    }
    if ($(cmb_tipo_venta.target).data('combobox')) {
        fnFormatComboboxCheck(cmb_tipo_venta.target);
    }
    if ($(cmb_campo_santo.target).data('combobox')) {
        fnFormatComboboxCheck(cmb_campo_santo.target);
    }

}


function fnFormatComboboxCheck(cmb_empresa_target) {

    $(cmb_empresa_target).combobox({
        formatter: function (row) {
            var opts = $(this).combobox('options');
            return '<input type="checkbox" class="combobox-checkbox">' + row[opts.textField]
        },
        onLoadSuccess: function () {
            var opts = $(this).combobox('options');
            var target = this;
            var values = $(target).combobox('getValues');

            $.map(values, function (value) {
                var el = opts.finder.getEl(target, value);
                el.find('input.combobox-checkbox')._propAttr('checked', true);
            })
        },
        onSelect: function (row) {
            console.log(row)
            var opts = $(this).combobox('options');
            var el = opts.finder.getEl(this, row[opts.valueField]);
            el.find('input.combobox-checkbox')._propAttr('checked', true);
        },
        onUnselect: function (row) {
            var opts = $(this).combobox('options');
            var el = opts.finder.getEl(this, row[opts.valueField]);
            el.find('input.combobox-checkbox')._propAttr('checked', false);
        }
    });
    //$(cmb_empresa_target).combobox("setValue", valores);
}


function fnGuardarDetalleRegla() {
    if ($.existeRegistroEnEdicionParaGrabar("dgv_lista_detalle_regla")) {
        var rowIndex = $.IndexRowEditing("dgv_lista_detalle_regla");
        var rowdata = $('#dgv_lista_detalle_regla').datagrid("getRows")[rowIndex];
        if ($.validarRowGrilla(rowIndex, 'dgv_lista_detalle_regla')) {

            var cmb_campo_santo = $("#dgv_lista_detalle_regla").datagrid("getEditor", { index: rowIndex, field: 'codigo_campo_santo' });
            var cmb_empresa = $("#dgv_lista_detalle_regla").datagrid("getEditor", { index: rowIndex, field: 'codigo_empresa' });
            var cmb_tipo_venta = $("#dgv_lista_detalle_regla").datagrid("getEditor", { index: rowIndex, field: 'codigo_tipo_venta' });
            var cmb_canal = $("#dgv_lista_detalle_regla").datagrid("getEditor", { index: rowIndex, field: 'codigo_canal' });


            debugger;
            var v_codigo_campo_santo = $.trim(cmb_campo_santo.target.combobox("getValues"));
            var v_codigo_empresa = $.trim(cmb_empresa.target.combobox("getValues"));
            var v_codigo_tipo_venta = $.trim(cmb_tipo_venta.target.combobox("getValues"));
            var v_codigo_canal = cmb_canal.target.combobox("getValue");

            /******************************************************************/

            var v_detalle_regla = {
                codigo_regla_tipo_planilla: ActionNuevoUrl._codigo_regla_tipo_planilla,
                codigo_campo_santo: v_codigo_campo_santo.toString(),
                codigo_empresa: v_codigo_empresa.toString(),
                codigo_tipo_venta: v_codigo_tipo_venta.toString(),
                codigo_canal: v_codigo_canal
            };

            var rows = $("#dgv_lista_detalle_regla").datagrid("getRows");
            var v_lista_detalle_regla = [];

            $.each(rows, function (index, data) {
                if (rowIndex != index) {

                    var v_regla = {
                        codigo_regla_tipo_planilla: ActionNuevoUrl._codigo_regla_tipo_planilla,
                        codigo_detalle_regla_tipo_planilla: data.codigo_detalle_regla_tipo_planilla,
                        codigo_campo_santo: data.codigo_campo_santo,
                        codigo_empresa: data.codigo_empresa,
                        codigo_tipo_venta: data.codigo_tipo_venta,
                        codigo_canal: data.codigo_canal
                    };
                    v_lista_detalle_regla.push(v_regla);
                }

            });
            $.messager.confirm('Confirmaci&oacute;n', '&iquest;Est&aacute; seguro que desea grabar?', function (result) {
                if (result) {
                    $.ajax({
                        type: 'post',
                        url: ActionNuevoUrl._ValidarDuplicidadRegistro,
                        data: JSON.stringify({ detalle_regla: v_detalle_regla, lista_detalle_regla: v_lista_detalle_regla }),
                        async: true,
                        cache: false,
                        dataType: 'json',
                        contentType: 'application/json; charset=utf-8',
                        success: function (data) {
                            if (data.v_resultado == 1) {
                                rowdata.confirmado = true;
                                $('#dgv_lista_detalle_regla').datagrid('endEdit', rowIndex);
                                $('#dgv_lista_detalle_regla').datagrid('clearSelections');
                                $.messager.alert('Operación exitosa', data.v_mensaje, 'info');

                                $("#btn_detalle_regla_crear").linkbutton('enable');
                                $("#btn_detalle_regla_aceptar").linkbutton('disable');
                                $("#btn_detalle_regla_cancelar").linkbutton('disable');
                                $("#btn_detalle_regla_quitar").linkbutton('disable');
                                $('#dgv_lista_detalle_regla').datagrid('clearSelections');
                                $("#btnGuardar").linkbutton('enable');
                            }
                            else {
                                $.messager.alert('Error en la operación.', data.v_mensaje, 'warning');
                            }
                        },
                        error: function () {
                            $.messager.alert('Error', data.v_mensaje, 'error');
                        }
                    });
                }
            });
            /******************************************************************/
        }
    }
}



function fnRegistrarReglaTipoPlanilla() {

    if (!$("#frmregistrar").form('enableValidation').form('validate'))
        return;
    if ($.existeRegistroEnEdicion('dgv_lista_detalle_regla'))
        return;
    var rows = $("#dgv_lista_detalle_regla").datagrid("getRows");
    if (rows.length < 1) {
        $.messager.alert("Configurar planilla", "Registre configuración de planilla, intente nuevamente.", "warning");
        return;
    }
    /*****************************************************************************/
    var v_regla_planilla = {
        codigo_regla_tipo_planilla: ActionNuevoUrl._codigo_regla_tipo_planilla,
        nombre: $.trim($("#txt_nombre_regla").val()),
        codigo_tipo_planilla: $("#frmregistrar #cmb_tipo_planilla").combobox('getValue'),
        afecto_doc_completa: $('#ckb_doc_completa').switchbutton('options').checked,
        codigo_tipo_reporte: '',//$("#frmregistrar #cmb_tipo_reporte").combobox('getValue'),
        detraccion_contrato: $('#ckb_detraccion_contrato').switchbutton('options').checked,
        envio_liquidacion: $('#ckb_envio_liquidacion').switchbutton('options').checked
    };
    /*****************************************************************************/
    var v_lista_detalle_regla = [];
    $.each(rows, function (index, data) {

        var v_detalle_regla = {
            codigo_regla_tipo_planilla: ActionNuevoUrl._codigo_regla_tipo_planilla,
            codigo_detalle_regla_tipo_planilla: data.codigo_detalle_regla_tipo_planilla,
            codigo_campo_santo: data.codigo_campo_santo,
            codigo_empresa: data.codigo_empresa,
            codigo_tipo_venta: data.codigo_tipo_venta,
            codigo_canal: data.codigo_canal
        };
        v_lista_detalle_regla.push(v_detalle_regla);

    });
    /*****************************************************************************/

    $.messager.confirm('Confirmaci&oacute;n', '&iquest;Est&aacute; seguro que desea registrar?', function (result) {
        if (result) {
            $.ajax({
                type: 'post',
                url: ActionNuevoUrl._Registrar,
                data: JSON.stringify({ v_entidad: v_regla_planilla, lista_detalle_regla: v_lista_detalle_regla }),
                async: true,
                cache: false,
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    if (data.v_resultado == 1) {
                        $.messager.alert('Operación exitosa', data.v_mensaje, 'info', function () {
                            $("#div_registrar_regla").dialog("close");
                            fnBuscarRegla();
                        });
                    }
                    else {
                        $.messager.alert('Error en la operación.', data.v_mensaje, 'error');
                    }
                },
                error: function () {
                    $.messager.alert('Error', "Error en el servidor", 'error');
                }
            });
        }
    });
}




function fnCancelarDetalleRegla() {

    var rowIndex = $.IndexRowEditing("dgv_lista_detalle_regla");
    if (rowIndex != null) {

        var row_regla = $('#dgv_lista_detalle_regla').datagrid("getRows")[rowIndex];
        if (row_regla.confirmado) {
            $('#dgv_lista_detalle_regla').datagrid('cancelEdit', rowIndex);
        }
        else {
            $('#dgv_lista_detalle_regla').datagrid('deleteRow', rowIndex);
        }



        $("#btn_detalle_regla_crear").linkbutton('enable');
        $("#btn_detalle_regla_quitar").linkbutton('disable');
        $("#btn_detalle_regla_cancelar").linkbutton('disable');
        $("#btn_detalle_regla_aceptar").linkbutton('disable');
        $('#dgv_lista_detalle_regla').datagrid('clearSelections');
        $("#btnGuardar").linkbutton('enable');
    }
}


function fnEliminarDetalleRegla() {
    if (!$.existeRegistroEnEdicion("dgv_lista_detalle_regla")) {
        var rows = $("#dgv_lista_detalle_regla").datagrid("getSelections");
        if (rows.length <= 0) {
            $.messager.alert("Eliminar Detalle Planilla", "Seleccione un registro.", "warning");
        }
        else {
            var row = $("#dgv_lista_detalle_regla").datagrid("getSelected");
            $(".messager-window,.window-shadow,.window-mask").remove();

            if (row) {
                $.messager.confirm('Confirmar', '&iquest;Est&aacute; seguro de eliminar este registro?', function (r) {
                    if (r) {
                       
                        var index = $("#dgv_lista_detalle_regla").datagrid("getRowIndex", row.codigo_detalle_regla_tipo_planilla);
                        $('#dgv_lista_detalle_regla').datagrid('deleteRow', index);

                        $("#btn_detalle_regla_crear").linkbutton('enable');
                        $("#btn_detalle_regla_aceptar").linkbutton('disable');
                        $("#btn_detalle_regla_cancelar").linkbutton('disable');
                        $("#btn_detalle_regla_quitar").linkbutton('disable');
                        $('#dgv_lista_detalle_regla').datagrid('clearSelections');
                        $("#btnGuardar").linkbutton('enable');
                    }
                });
            }
        }
    }
}

