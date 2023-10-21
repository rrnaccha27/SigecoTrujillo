var ActionNuevoUrl = {};

var JsonCanal = {};
var JsonEmpresa = {};
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

; (function (app) {
    //===========================================================================================
    var current = app.reglabonotrimestral_nuevo = {};
    //===========================================================================================
    jQuery.extend(app.reglabonotrimestral_nuevo,
        {
            Initialize: function (actionUrls) {
                jQuery.extend(ActionNuevoUrl, actionUrls);
                fnInicializarDetalleRegla();
                fnConfigurarGrillaDetalleRegla();
                InicializarListadoMeta();
            }
        })
})(project);


function fnInicializarDetalleRegla() {
    $('.content').combobox_sigees({
        id: '#cmb_tipo_bono',
        url: ActionNuevoUrl._GetTipoBonoTrimestralJson
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

    $('#fechaInicio').datebox({
        formatter: function (date) {
            var y = date.getFullYear();
            var m = date.getMonth() + 1;
            var d = date.getDate();
            return (d < 10 ? ('0' + d) : d) + '/' + (m < 10 ? ('0' + m) : m) + '/' + y;
        },
        parser: function (s) {
            if (!s) return new Date();
            var ss = s.split('/');
            var y = parseInt(ss[2], 10);
            var m = parseInt(ss[1], 10);
            var d = parseInt(ss[0], 10);
            if (!isNaN(y) && !isNaN(m) && !isNaN(d)) {
                return new Date(y, m - 1, d)
            } else {
                return new Date();
            }
        }
    });

    $('#fechaFin').datebox({
        formatter: function (date) {
            var y = date.getFullYear();
            var m = date.getMonth() + 1;
            var d = date.getDate();
            return (d < 10 ? ('0' + d) : d) + '/' + (m < 10 ? ('0' + m) : m) + '/' + y;
        },
        parser: function (s) {
            if (!s) return new Date();
            var ss = s.split('/');
            var y = parseInt(ss[2], 10);
            var m = parseInt(ss[1], 10);
            var d = parseInt(ss[0], 10);
            if (!isNaN(y) && !isNaN(m) && !isNaN(d)) {
                return new Date(y, m - 1, d)
            } else {
                return new Date();
            }
        }
    });

}

function fnConfigurarGrillaDetalleRegla() {
    $('#dgv_lista_detalle_regla').datagrid({
        fitColumns: true,
        url: ActionNuevoUrl._GetDetalleAllJson,
        queryParams: {
            p_codigo_regla: ActionNuevoUrl._codigo_regla
        },
        idField: 'codigo_regla_detalle',
        pagination: false,
        singleSelect: true,
        rownumbers: true,
        toolbar:'#tlb_detalle_regla',
        columns: [[
            { field: 'codigo_regla', hidden: true },
            { field: 'codigo_regla_detalle', hidden: true },
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
                if (rowdata.codigo_regla_detalle<=0) {
                    $("#btn_detalle_regla_quitar").linkbutton('enable');
                } else {
                    $("#btn_detalle_regla_quitar").linkbutton('disable');
                }

            }
        },
        //onDblClickRow: function (rowIndex, rowdata) {

        //    if (rowdata.codigo_regla_detalle > 0) {
        //        $.messager.alert("Configurar planilla", "El registro seleccionado no se puede editar, intente nuevamente.", "warning");
        //        return;
        //    }
        //    if (!$.existeRegistroEnEdicion("dgv_lista_detalle_regla")) {
        //        $('#dgv_lista_detalle_regla').datagrid('beginEdit', rowIndex);
        //        rowdata.editing = true;
        //        rowdata.confirmado = true;

        //        var cmb_empresa = $("#dgv_lista_detalle_regla").datagrid("getEditor", { index: rowIndex, field: 'codigo_empresa' });
        //        var cmb_tipo_venta = $("#dgv_lista_detalle_regla").datagrid("getEditor", { index: rowIndex, field: 'codigo_tipo_venta' });

        //        if ($(cmb_empresa.target).data('combobox')) {
        //            var cod_em = $(cmb_empresa.target).combobox('getValues')
        //            fnFormatComboboxCheck(cmb_empresa.target);
        //            $(cmb_empresa.target).combobox('setValue', cod_em);
        //        }
        //        if ($(cmb_tipo_venta.target).data('combobox')) {
        //            var cod_tv = $(cmb_tipo_venta.target).combobox('getValues')
        //            fnFormatComboboxCheck(cmb_tipo_venta.target);
        //            $(cmb_tipo_venta.target).combobox('setValue', cod_tv);
        //        }

        //        /*******************************************/
        //        rowdata.editing = true;
        //        rowdata.confirmado = true;
        //        $("#btn_detalle_regla_crear").linkbutton('disable');
        //        $("#btn_detalle_regla_aceptar").linkbutton('enable');
        //        $("#btn_detalle_regla_cancelar").linkbutton('enable');
        //        $("#btn_detalle_regla_quitar").linkbutton('disable');
        //    }
        //}
    });
    //$('#dgv_lista_detalle_regla').datagrid('enableFilter');
    $(window).resize(function () {
        $('#dgv_lista_detalle_regla').datagrid('resize');
    });
}

function NuevoDetalleRegla() {
    if (!$.existeRegistroEnEdicion('dgv_lista_detalle_regla')) {
        var new_row = {
            codigo_regla_detalle: -new Date().toString("ddHHmmss")
            , codigo_canal: null
            , codigo_empresa: null
            , codigo_tipo_venta: null
            , confirmado: false
            , editing: true
        }

        $("#dgv_lista_detalle_regla").datagrid("appendRow", new_row);
        var editIndex = $("#dgv_lista_detalle_regla").datagrid("getRowIndex", new_row.codigo_regla_detalle);
        fnEditarDetalleRegla(editIndex);
        /************************************/
        $("#btn_detalle_regla_crear").linkbutton('disable');
        $("#btn_detalle_regla_aceptar").linkbutton('enable');
        $("#btn_detalle_regla_cancelar").linkbutton('enable');
        $("#btn_detalle_regla_quitar").linkbutton('disable');
    }
}

function fnEditarDetalleRegla(editIndex) {
    $("#dgv_lista_detalle_regla").datagrid("selectRow", editIndex);
    $("#dgv_lista_detalle_regla").datagrid("beginEdit", editIndex);

    var cmb_empresa = $("#dgv_lista_detalle_regla").datagrid("getEditor", { index: editIndex, field: 'codigo_empresa' });
    var cmb_tipo_venta = $("#dgv_lista_detalle_regla").datagrid("getEditor", { index: editIndex, field: 'codigo_tipo_venta' });

    if ($(cmb_empresa.target).data('combobox')) {
        fnFormatComboboxCheck(cmb_empresa.target);
    }
    if ($(cmb_tipo_venta.target).data('combobox')) {
        fnFormatComboboxCheck(cmb_tipo_venta.target);
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

            var cmb_empresa = $("#dgv_lista_detalle_regla").datagrid("getEditor", { index: rowIndex, field: 'codigo_empresa' });
            var cmb_tipo_venta = $("#dgv_lista_detalle_regla").datagrid("getEditor", { index: rowIndex, field: 'codigo_tipo_venta' });
            var cmb_canal = $("#dgv_lista_detalle_regla").datagrid("getEditor", { index: rowIndex, field: 'codigo_canal' });

            var v_codigo_empresa = $.trim(cmb_empresa.target.combobox("getValues"));
            var v_codigo_tipo_venta = $.trim(cmb_tipo_venta.target.combobox("getValues"));
            var v_codigo_canal = cmb_canal.target.combobox("getValue");

            /******************************************************************/

            var v_detalle_regla = {
                codigo_regla: ActionNuevoUrl._codigo_regla,
                codigo_empresa: v_codigo_empresa.toString(),
                codigo_tipo_venta: v_codigo_tipo_venta.toString(),
                codigo_canal: v_codigo_canal
            };

            var rows = $("#dgv_lista_detalle_regla").datagrid("getRows");
            var v_lista_detalle_regla = [];

            $.each(rows, function (index, data) {
                if (rowIndex != index) {

                    var v_regla = {
                        codigo_regla: ActionNuevoUrl._codigo_regla,
                        codigo_regla_detalle: data.codigo_regla_detalle,
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
                                //$.messager.alert('Operación exitosa', data.v_mensaje, 'info');

                                $("#btn_detalle_regla_crear").linkbutton('enable');
                                $("#btn_detalle_regla_aceptar").linkbutton('disable');
                                $("#btn_detalle_regla_cancelar").linkbutton('disable');
                                $("#btn_detalle_regla_quitar").linkbutton('disable');
                                $('#dgv_lista_detalle_regla').datagrid('clearSelections');
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

function fnRegistrarReglaBonoTrimestral() {
    if (!$("#frmregistrar").form('enableValidation').form('validate'))
        return;

    var vigencia_inicio = $.trim($('#fechaInicio').textbox('getText'));
    var vigencia_fin = $.trim($('#fechaFin').textbox('getText'));

    if (!$.validarFecha(vigencia_inicio)) {
        $.messager.alert("Configurar Planilla", "Inicio de Vigencia en formato incorrecto.", "warning");
        return;
    }

    if (!$.validarFecha(vigencia_fin)) {
        $.messager.alert("Configurar Planilla", "Fin de Vigencia en formato incorrecto.", "warning");
        return;
    }

    vigencia_inicio = $.formatoFecha(vigencia_inicio);
    vigencia_fin = $.formatoFecha(vigencia_fin);

    if (parseInt(vigencia_inicio) > parseInt(vigencia_fin)) {
        $.messager.alert("Configurar Planilla", "Inicio de Vigencia debe ser menor a Fin Vigencia.", "warning");
        return;
    }

    if ($.existeRegistroEnEdicion('dgv_lista_detalle_regla'))
        return;

    if ($.existeRegistroEnEdicion('dgMeta'))
        return;

    var rowsDetalle = $("#dgv_lista_detalle_regla").datagrid("getRows");
    if (rowsDetalle.length < 1) {
        $.messager.alert("Configurar planilla", "Registre configuración de planilla, intente nuevamente.", "warning");
        return;
    }

    var rowsMeta = $("#dgMeta").datagrid("getRows");
    if (rowsMeta.length < 1) {
        $.messager.alert("Configurar planilla", "Registre meta(s), intente nuevamente.", "warning");
        return;
    }

    /*****************************************************************************/
    var v_regla_planilla = {
        codigo_regla: ActionNuevoUrl._codigo_regla,
        descripcion: $.trim($("#descripcion").val()),
        codigo_tipo_bono: $("#frmregistrar #cmb_tipo_bono").combobox('getValue'),
        vigencia_inicio: vigencia_inicio,
        vigencia_fin: vigencia_fin
    };
    /*****************************************************************************/
    var v_lista_detalle_regla = [];
    $.each(rowsDetalle, function (index, data) {
        var v_detalle_regla = {
            codigo_regla: ActionNuevoUrl._codigo_regla,
            codigo_regla_detalle: data.codigo_regla_detalle,
            codigo_empresa: data.codigo_empresa,
            codigo_tipo_venta: data.codigo_tipo_venta,
            codigo_canal: data.codigo_canal
        };
        v_lista_detalle_regla.push(v_detalle_regla);
    });
    /*****************************************************************************/
    var lst_meta = [];
    $.each(rowsMeta, function (index, data) {
        var monto = {
            codigo_meta: data.codigo_meta,
            rango_inicio: parseInt(data.rango_inicio, 10),
            rango_fin: parseInt(data.rango_fin, 10),
            monto: parseFloat(data.monto)
        };
        lst_meta.push(monto);
    });
    /*****************************************************************************/

    $.messager.confirm('Confirmaci&oacute;n', '&iquest;Est&aacute; seguro que desea registrar?', function (result) {
        if (result) {
            $.ajax({
                type: 'post',
                url: ActionNuevoUrl._Registrar,
                data: JSON.stringify({ v_entidad: v_regla_planilla, lista_detalle_regla: v_lista_detalle_regla, lst_meta: lst_meta }),
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
                        var index = $("#dgv_lista_detalle_regla").datagrid("getRowIndex", row.codigo_regla_detalle);
                        $('#dgv_lista_detalle_regla').datagrid('deleteRow', index);

                        $("#btn_detalle_regla_crear").linkbutton('enable');
                        $("#btn_detalle_regla_aceptar").linkbutton('disable');
                        $("#btn_detalle_regla_cancelar").linkbutton('disable');
                        $("#btn_detalle_regla_quitar").linkbutton('disable');
                        $('#dgv_lista_detalle_regla').datagrid('clearSelections');
                    }
                });
            }
        }
    }
}

function InicializarListadoMeta() {

    $('#dgMeta').datagrid({
        idField: 'codigo_meta',
        url: ActionNuevoUrl._GetMetaAllJson,
        queryParams: {
            p_codigo_regla: ActionNuevoUrl._codigo_regla
        },
        toolbar: '#tbMeta',
        fitColumns: true,
        data: null,
        pagination: false,
        singleSelect: true,
        pageNumber: 0,
        pageList: [5, 10, 15, 20, 25, 30],
        pageSize: 10,
        columns:
        [[
            { field: 'codigo_meta', hidden: 'true' },
            {
                field: 'rango_inicio', title: 'Rango Inicio', width: 80, align: 'right', halign: 'center',
                editor: {
                    type: 'numberbox',
                    options: {
                        precision: 0,
                        required: true,
                        max: 99
                    }
                }
            },
            {
                field: 'rango_fin', title: 'Rango Fin', width: 80, align: 'right', halign: 'center',
                editor: {
                    type: 'numberbox',
                    options: {
                        precision: 0,
                        required: true,
                        max: 99
                    }
                }
            },
            {
                field: 'monto', title: 'Monto', width: 100, align: 'right', halign: 'center',
                editor: {
                    type: 'numberbox',
                    options: {
                        precision: 2,
                        required: true,
                        max: 999
                    }
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
    });
}

function NuevoRegistro() {
    if (!$("#frmRegistro").form('enableValidation').form('validate'))
        return;

    if (!$.existeRegistroEnEdicion('dgMeta')) {
        var new_row = {
            "codigo_meta": -new Date().toString("ddHHmmss"),
            "rango_inicio": 0,
            "rango_fin": 0,
            "monto": 0.00,
        };

        $("#dgMeta").datagrid("appendRow", new_row);
        var editIndex = $("#dgMeta").datagrid("getRowIndex", new_row.codigo_meta);
        $("#dgMeta").datagrid("selectRow", editIndex);
        $("#dgMeta").datagrid("beginEdit", editIndex);

        $("#btn_meta_crear").linkbutton('disable');
        $("#btn_meta_quitar").linkbutton('disable');
        $("#btn_meta_aceptar").linkbutton('enable');
        $("#btn_meta_cancelar").linkbutton('enable');
    }
}

function AceptarRegistro() {
    //MontoGlobal = $('#rango_inicio_100').numberbox('getValue');

    //if (!MontoGlobal || parseFloat(MontoGlobal) <= 0) {
    //    $.messager.alert("Montos", "No ha ingresado el monto de la Meta al 100%.", "warning");
    //    return false;
    //}

    //var monto = $('#monto_100').numberbox('getValue');
    //if (!monto || parseFloat(monto) <= 0) {
    //    $.messager.alert("Montos", "No ha ingresado el porcentaje a pagar (al 100%).", "warning");
    //    return false;
    //}
    //PorcentajePagoGlobal = parseFloat(monto);

    if ($.existeRegistroEnEdicionParaGrabar("dgMeta")) {
        var rowIndex = $.IndexRowEditing("dgMeta");
        var rowdata = $('#dgMeta').datagrid("getRows")[rowIndex];
        if ($.validarRowGrilla(rowIndex, 'dgMeta')) {
            if (ValidarDataConsistente('dgMeta', rowIndex)) {
                rowdata.confirmado = true;
                $('#dgMeta').datagrid('endEdit', rowIndex);

                $("#btn_meta_crear").linkbutton('enable');
                $("#btn_meta_quitar").linkbutton('disable');
                $("#btn_meta_aceptar").linkbutton('disable');
                $("#btn_meta_cancelar").linkbutton('disable');
                $('#dgMeta').datagrid('clearSelections');
            }
        }
    }
}

function CancelarRegistro() {
    var rowIndex = $.IndexRowEditing('dgMeta');
    if (rowIndex != null) {
        $('#dgMeta').datagrid('cancelEdit', rowIndex);
        var rowdata = $('#dgMeta').datagrid("getRows")[rowIndex];
        if (!rowdata.confirmado && rowdata.codigo_meta < 0) {
            $('#dgMeta').datagrid('deleteRow', rowIndex);
            $("#btn_meta_crear").linkbutton('enable');
            $("#btn_meta_quitar").linkbutton('disable');
            $("#btn_meta_aceptar").linkbutton('disable');
            $("#btn_meta_cancelar").linkbutton('disable');
            $('#dgMeta').datagrid('clearSelections');
        }
    }
}

function ValidarDataConsistente(lista, indiceEditado) {
    var resultado = true;
    var rows = $("#" + lista).datagrid("getRows");

    debugger;

    var ed_rango_fin = $("#dgMeta").datagrid("getEditor", { index: indiceEditado, field: 'rango_fin' });
    var ed_rango_inicio = $("#dgMeta").datagrid("getEditor", { index: indiceEditado, field: 'rango_inicio' });
    var ed_monto = $("#dgMeta").datagrid("getEditor", { index: indiceEditado, field: 'monto' });
    var rango_fin = ed_rango_fin.target.numberbox("getValue");
    var rango_inicio = ed_rango_inicio.target.numberbox("getValue");
    var monto = ed_monto.target.numberbox("getValue");

    $.each(rows, function (i, objeto) {
        if (i != indiceEditado) {
            if (parseFloat(objeto.rango_inicio) >= parseFloat(rango_inicio) && parseFloat(objeto.rango_fin) <= parseFloat(rango_inicio)) {
                $.messager.alert("Meta", "Ya existe un rango conteniendo el valor de inicio.", "warning");
                resultado = false;
                return false;
            }
            if (parseFloat(rango_inicio) >= parseFloat(objeto.rango_inicio) && parseFloat(rango_inicio) <= parseFloat(objeto.rango_fin)) {
                $.messager.alert("Meta", "Ya existe un rango conteniendo el valor de inicio.", "warning");
                resultado = false;
                return false;
            }
            if (parseFloat(objeto.rango_inicio) >= parseFloat(rango_fin) && parseFloat(objeto.rango_fin) <= parseFloat(rango_fin)) {
                $.messager.alert("Meta", "Ya existe un rango conteniendo el valor de fin.", "warning");
                resultado = false;
                return false;
            }
            if (parseFloat(rango_fin) >= parseFloat(objeto.rango_inicio) && parseFloat(rango_fin) <= parseFloat(objeto.rango_fin)) {
                $.messager.alert("Meta", "Ya existe un rango conteniendo el valor de fin.", "warning");
                resultado = false;
                return false;
            }
        }
    });

    if (resultado) {
        if (parseFloat(rango_inicio) == 0 || parseFloat(rango_fin) == 0) {
            $.messager.alert("Meta", "El inicio/fin del rango no puede ser 0.", "warning");
            resultado = false;
        }
    }

    if (resultado) {
        if (parseFloat(rango_inicio) == parseFloat(rango_fin)) {
            $.messager.alert("Meta", "El inicio/fin del rango no pueden ser iguales.", "warning");
            resultado = false;
        }
    }

    if (resultado) {
        if (parseFloat(rango_inicio) > parseFloat(rango_fin)) {
            $.messager.alert("Meta", "El inicio del rango no pueden ser mayor al fin del mismo.", "warning");
            resultado = false;
        }
    }

    if (resultado) {
        if (parseFloat(monto) <= 0) {
            $.messager.alert("Meta", "El monto no puede ser 0.", "warning");
            resultado = false;
        }
    }

    return resultado;
}

//function existeRegistroEnEdicion(lista) {
//    var editando = false;
//    var rows = $("#" + lista).datagrid("getRows");

//    $.each(rows, function (i, objeto) {
//        if (objeto.editing) {
//            editando = true;
//            return;
//        }
//    });
//    if (editando) {
//        $.messager.alert("Registro en edici&oacute;n", "Existe un registro en edici&oacute;n, para continuar guarde o cancele la edici&oacuten.", "warning");
//    }
//    return editando;
//}
