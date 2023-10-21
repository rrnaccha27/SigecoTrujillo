var ActionArticuloUrl = {};
var JsonMoneda = {};
var JsonEmpresa = {};
var JsonTipoVenta = {};

var lst_comision = [];
var lst_comision_eliminar = [];
var lista_precio_articulo_eliminado = [];
var lst_comision_supervisor = [];
var lst_comision_supervisor_eliminar = [];
var lista_precio_original = [];

function monedaFormatter(value) {

    for (var i = 0; i < JsonMoneda.length; i++) {
        if (JsonMoneda[i].id == value) return JsonMoneda[i].text;
    }
    return value;
}
function tipoVentaFormatter(value) {

    for (var i = 0; i < JsonTipoVenta.length; i++) {
        if (JsonTipoVenta[i].id == value) return JsonTipoVenta[i].text;
    }
    return value;
}
function empresaFormatter(value) {

    for (var i = 0; i < JsonEmpresa.length; i++) {
        if (JsonEmpresa[i].id == value) return JsonEmpresa[i].text;
    }
    return value;
}

function comisionFormatter(value, row) {
    var link = '<a href="javascript:void(0)" onclick="fnNuevaComision(' + row.codigo_precio + ',' + row.actualizado + ')">Comisión</a>';

    return link;
}

; (function (app) {
    //===========================================================================================
    var current = app.articulo = {};
    //===========================================================================================
    jQuery.extend(app.articulo,
        {
            Initialize: function (actionUrls) {
                jQuery.extend(ActionArticuloUrl, actionUrls);
                fnInicializarArticulo();
                fnConfigurarGrillaPrecioArticulo();
                fnListarPrecioByArticulo();
            }
        })
})(project);

function fnInicializarArticulo() {

    JsonEmpresa = $('.content').get_json_combobox({
        url: ActionArticuloUrl._GetEmpresaJson
    });

    JsonMoneda = $('.content').get_json_combobox({
        url: ActionArticuloUrl._GetMonedaJson
    });

    JsonTipoVenta = $('.content').get_json_combobox({
        url: ActionArticuloUrl._GetTipoVentaJson
    });

    $('.content').combobox_sigees({
        id: "#cmb_unidad_negocio",
        url: ActionArticuloUrl._GetUnidadNegocioJson
    });

    $('.content').combobox_sigees({
        id: "#cmb_tipo_articulo",
        url: ActionArticuloUrl._GetTipoArticuloJson
    });

    $('.content').combobox_sigees({
        id: "#cmb_categoria",
        url: ActionArticuloUrl._GetCategoriaJson
    });

    //$('.content').ResizeModal({
    //    widthMax: '90%',
    //    widthMin: '80%',
    //    div: 'div_registrar_articulo'
    //});
}

function fnNuevaComision(p_codigo_precio, p_actualizado) {
    if (p_codigo_precio > 0 && p_actualizado == 1) {
        $.messager.alert("Mantenimiento de Comisión", "Deberá Guardar las modificaciones para continuar con este proceso.", "warning");
        return false;
    }

    if (!existeRegistroEnEdicionSinMensaje('dgv_precio_articulo')) {
        $(this).AbrirVentanaEmergente({
            parametro: "?p_codigo_precio=" + p_codigo_precio + "&&p_estado_registro=" + ActionArticuloUrl._estado_registro,
            div: 'div_registrar_comision',
            title: "Mantenimiento de Comisión",
            url: ActionArticuloUrl._UrlComision
        });
    }
}

function fnListarPrecioByArticulo() {
    $.ajax({
        type: 'post',
        url: ActionArticuloUrl._GetPreciobyArticuloJson,
        data: { codigoArticulo: ActionArticuloUrl._codigo_articulo },
        async: false,
        cache: false,
        dataType: 'json',
        success: function (data) {
            $('#dgv_precio_articulo').datagrid('loadData', data);
        },
        error: function () {
            alert("Error en recuperar precio articulo.");
        }
    });
}

function fnConfigurarGrillaPrecioArticulo() {
    $('#dgv_precio_articulo').datagrid({
        fitColumns: false,
        idField: 'codigo_precio',
        toolbar: "#tlb_articulo",
        rownumbers: true,
        pageList: [30, 50, 100, 200, 400, 500],
        pageSize: 30,
        pagination: true,
        singleSelect: true,
        columns:
        [[
            { field: 'codigo_precio', title: 'Codigo Precio', width: 150, hidden: true },
            {
                field: 'codigo_empresa', title: 'Empresa', width: 120,halign: 'center', align: 'left',
                editor: {
                    type: 'combobox',
                    options: {
                        valueField: 'id',
                        textField: 'text',
                        data: JsonEmpresa,
                        //novalidate: true,
                        editable: false,
                        required: true
                    }
                },
                formatter: empresaFormatter
            },
            {
                field: 'codigo_tipo_venta', title: 'Tipo Venta', width: 180,halign: 'center', align: 'left', editor: {
                    type: 'combobox',
                    options: {
                        valueField: 'id',
                        textField: 'text',
                        data: JsonTipoVenta,
                        editable: false,
                        // novalidate: true,
                        required: true
                    }
                },
                formatter: tipoVentaFormatter
            },
            {
                field: 'codigo_moneda', title: 'Moneda', width: 150, halign: 'center', align: 'left',
                editor: {
                    type: 'combobox',
                    options: {
                        valueField: 'id',
                        textField: 'text',
                        data: JsonMoneda,
                        editable: false,
                        //novalidate: true,
                        required: true
                       // validType: "restricted['#dgv_precio_articulo td[field=codigo_moneda] input']",
                    }
                },
                formatter: monedaFormatter
            },
            {
                field: 'precio', title: 'Precio', width: 130, halign: 'center', align: 'right', formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                }
            },
             {
                 field: 'igv', title: 'IGV', width: 130, halign: 'center', align: 'right', formatter: function (value, row) {
                     return $.NumberFormat(value, 2);
                 }
             },
            {
                field: 'precio_total', title: 'Precio Total', width: 130, halign: 'center', align: 'right',
                formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                },
                editor: {
                    type: 'numberbox',
                    options: {
                        precision: 2,
                        required: true,
                        max: 9999999
                    }
                }
            },
              /*{
                  field: 'cuota_inicial', title: 'Cuota Inicial', width: 130, halign: 'center', align: 'right',
                  editor: {
                      type: 'numberbox',
                      options: {
                          precision: 2,
                          //validType: "MayorACeroMenorIgualANumberBox['td[field=\"precio_total\"] input.numberbox-f']",
                          required: true
                      }
                  }
              },*/
            {
                field: 'str_vigencia_inicio', title: 'Fecha Inicio', width: 130, halign: 'center', align: 'center',
                editor: {
                    type: 'datebox',
                    options: {
                        required: true,
                        validType: "fechaMenorIgualA['td[field=\"str_vigencia_fin\"] input.datebox-f',\'Fecha Fin\',\'dd/MM/yyyy\']",
                        formatter: myformatter,
                        parser: myparser
                    }
                }
            },
            {
                field: 'str_vigencia_fin', title: 'Fecha Fin', width: 130, halign: 'center',align: 'center',
                editor: {
                    type: 'datebox',
                    options: {
                        required: true,
                        formatter: myformatter,
                        parser: myparser

                    }
                }
            },
             { field: 'tiene_comision', title: 'Tiene</br>Comisión', width: 120, align: 'center', formatter: function (value, row, index) { if (row.tiene_comision) { return 'Si'; } else { return 'No'; } } },
              
            {
                field: 'codigo_comision', width: 120, title: 'Comisiones', halign: 'center',align: 'center',
                formatter: comisionFormatter
            }
            ,
            {
                field: 'actualizado', hidden: true
            },
            {
                field: 'clonarcomisiones', hidden: true
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

            if (!existeRegistroEnEdicion("dgv_precio_articulo")) {
                //$("#btn_articulo_crear").linkbutton('enable');
                //$("#btn_articulo_quitar").linkbutton('enable');
                //$("#btn_articulo_cancelar").linkbutton('disable');
                //$("#btn_articulo_aceptar").linkbutton('disable');

            }
        },
        onDblClickRow: function (rowIndex, rowdata) {
        if (!existeRegistroEnEdicion("dgv_precio_articulo")) {
            $('#dgv_precio_articulo').datagrid('beginEdit', rowIndex);
            rowdata.editing = true;
            rowdata.confirmado = true;

            copiaPrecio(rowIndex);

            var Editor = $("#dgv_precio_articulo").datagrid("getEditor", { index: rowIndex, field: 'precio_total' });
                
            if ($(Editor.target).data('numberbox')) {
                $(Editor.target).numberbox({ validType: 'mayorACero[0]' });
                $(Editor.target).numberbox('textbox').css('text-align', 'right');
            }

            $("#btn_articulo_crear").linkbutton('disable');
            $("#btn_articulo_quitar").linkbutton('disable');
            $("#btn_articulo_cancelar").linkbutton('enable');
            $("#btn_articulo_aceptar").linkbutton('enable');

        }
    }    });
    $('#dgv_precio_articulo').datagrid('enableFilter');
}



function myformatter(date) {
    var y = date.getFullYear();
    var m = date.getMonth() + 1;
    var d = date.getDate();
    return (d < 10 ? ('0' + d) : d) + '/' + (m < 10 ? ('0' + m) : m) + '/' + y;
}
function myparser(s) {
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


/************************************************************************/
function existeRegistroEnEdicionSinMensaje(lista) {
    var editando = false;
    var rows = $("#" + lista).datagrid("getRows");

    $.each(rows, function (i, objeto) {
        if (objeto.editing) {
            editando = true;
            return;
        }
    });

    return editando;
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

function validarRowGrilla(index, lista) {
    var _Valid = $('#' + lista).datagrid('validateRow', index);
    if (!_Valid) {
        $.messager.alert("Campos Obligatorios", "Falta ingresar los campos requeridos de la grilla en edici&oacute;n.", "warning");
    }
    return _Valid;

}
/*********************************************************************************/
function cancelarEdicion(dgv) {
    //alert(dgv);
    var rowIndex = IndexRowEditing(dgv);
    if (rowIndex != null) {
        $('#' + dgv).datagrid('cancelEdit', rowIndex);
    }
}

function cancelarPrecio() {
    //alert(dgv);
    var rowIndex = IndexRowEditing("dgv_precio_articulo");
    if (rowIndex != null) {

        var row_precio = $('#dgv_precio_articulo').datagrid("getRows")[rowIndex];
        if (row_precio.confirmado) {
            $('#dgv_precio_articulo').datagrid('cancelEdit', rowIndex);
        }
        else {
            $('#dgv_precio_articulo').datagrid('deleteRow', rowIndex);
        }

        $("#btn_articulo_crear").linkbutton('enable');
        $("#btn_articulo_quitar").linkbutton('disable');
        $("#btn_articulo_cancelar").linkbutton('disable');
        $("#btn_articulo_aceptar").linkbutton('disable');
        $('#dgv_precio_articulo').datagrid('clearSelections');
        lista_precio_original = [];
    }
}
function EliminarPrecio() {
    if (!existeRegistroEnEdicion("dgv_precio_articulo")) {
        var rows = $("#dgv_precio_articulo").datagrid("getSelections");
        if (rows.length <= 0) {
            $.messager.alert("Eliminar precio articulo", "Seleccione un registro.", "warning");
        }
        else {
            var row = $("#dgv_precio_articulo").datagrid("getSelected");
            $(".messager-window,.window-shadow,.window-mask").remove();

            if (row) {
                $.messager.confirm('Confirmar', '&iquest;Est&aacute; seguro de eliminar este registro?', function (r) {
                    if (r) {
                        var index = $("#dgv_precio_articulo").datagrid("getRowIndex", row.codigo_precio);
                        var rowdata = $('#dgv_precio_articulo').datagrid("getRows")[index];
                        if (parseInt(rowdata.codigo_precio) > 0) {
                            lista_precio_articulo_eliminado.push(rowdata);
                        }
                        $('#dgv_precio_articulo').datagrid('deleteRow', index);

                        $("#btn_articulo_crear").linkbutton('enable');
                        $("#btn_articulo_quitar").linkbutton('disable');
                        $("#btn_articulo_cancelar").linkbutton('disable');
                        $("#btn_articulo_aceptar").linkbutton('disable');
                        $('#dgv_precio_articulo').datagrid('clearSelections');
                    }
                });
            }
        }
    }
}

function guardarPrecio() {
    if (existeRegistroEnEdicionParaGrabar("dgv_precio_articulo")) {
        var rowIndex = IndexRowEditing("dgv_precio_articulo");
        var rowdata = $('#dgv_precio_articulo').datagrid("getRows")[rowIndex];
        if (validarRowGrilla(rowIndex, 'dgv_precio_articulo')) {

            var cmb_codigo_empresa = $("#dgv_precio_articulo").datagrid("getEditor", { index: rowIndex, field: 'codigo_empresa' });
            var cmb_codigo_tipo_venta = $("#dgv_precio_articulo").datagrid("getEditor", { index: rowIndex, field: 'codigo_tipo_venta' });
            var cmb_codigo_moneda = $("#dgv_precio_articulo").datagrid("getEditor", { index: rowIndex, field: 'codigo_moneda' });

            var dtb_vigencia_inicio = $("#dgv_precio_articulo").datagrid("getEditor", { index: rowIndex, field: 'str_vigencia_inicio' });
            var dtb_vigencia_fin = $("#dgv_precio_articulo").datagrid("getEditor", { index: rowIndex, field: 'str_vigencia_fin' });

            var v_codigo_empresa = cmb_codigo_empresa.target.combobox("getValue");
            var v_codigo_tipo_venta = cmb_codigo_tipo_venta.target.combobox("getValue");
            var v_codigo_moneda = cmb_codigo_moneda.target.combobox("getValue");

            var v_vigencia_inicio = dtb_vigencia_inicio.target.datebox("getValue");
            var v_vigencia_fin = dtb_vigencia_fin.target.datebox("getValue");

            var dtb_precio_articulo = $("#dgv_precio_articulo").datagrid("getEditor", { index: rowIndex, field: 'precio_total' });

            var m_precio_articulo = parseFloat(dtb_precio_articulo.target.numberbox("getValue"));


            //var nbx_cuota_inicial = $("#dgv_precio_articulo").datagrid("getEditor", { index: rowIndex, field: 'cuota_inicial' });
            var v_cuota_inicial = 0;//parseFloat(nbx_cuota_inicial.target.numberbox("getValue"));
            /******************************************************************/

            var v_precio_articulo = {
                codigo_precio : rowdata.codigo_precio,
                codigo_empresa: v_codigo_empresa,
                codigo_tipo_venta: v_codigo_tipo_venta,
                codigo_moneda: v_codigo_moneda,
                vigencia_inicio: v_vigencia_inicio,
                vigencia_fin: v_vigencia_fin,
                precio_total: m_precio_articulo,
                cuota_inicial: v_cuota_inicial
            };

            var rows = $("#dgv_precio_articulo").datagrid("getRows");
            var v_lista_precio_articulo = [];

            $.each(rows, function (index, data) {
                if (rowIndex != index) {
                    
                    var v_precio = {
                        codigo_articulo: ActionArticuloUrl._codigo_articulo,
                        codigo_precio: data.codigo_precio,
                        codigo_empresa: data.codigo_empresa,
                        codigo_tipo_venta: data.codigo_tipo_venta,
                        codigo_moneda: data.codigo_moneda,
                        precio_total: parseFloat(data.precio_total),
                        vigencia_inicio: data.str_vigencia_inicio,
                        vigencia_fin: data.str_vigencia_fin
                    };
                    v_lista_precio_articulo.push(v_precio);
                }

            });
            $.messager.confirm('Confirmaci&oacute;n', '&iquest;Est&aacute; seguro que desea grabar?', function (result)
            {
                if (result) {
                    $.ajax({
                        type: 'post',
                        url: ActionArticuloUrl._ValidarRangoFechaPrecio,
                        data: JSON.stringify({ v_precio: v_precio_articulo, lista_precio_articulo: v_lista_precio_articulo, v_precio_original: lista_precio_original }),
                        async: true,
                        cache: false,
                        dataType: 'json',
                        contentType: 'application/json; charset=utf-8',
                        success: function (data) {
                            if (data.v_resultado == 1) {

                                rowdata.confirmado = true;
                                $('#dgv_precio_articulo').datagrid('endEdit', rowIndex);
                                $('#dgv_precio_articulo').datagrid('updateRow', {
                                    index: rowIndex,
                                    row: {
                                        igv: parseFloat(data.v_impuesto),
                                        precio: parseFloat(data.v_precio_articulo),
                                        actualizado: 1
                                    }
                                });

                                $("#btn_articulo_crear").linkbutton('enable');
                                $("#btn_articulo_quitar").linkbutton('disable');
                                $("#btn_articulo_cancelar").linkbutton('disable');
                                $("#btn_articulo_aceptar").linkbutton('disable');

                                $('#dgv_precio_articulo').datagrid('clearSelections');
                                lista_precio_original = [];

                                if (rowdata.tiene_comision && rowdata.codigo_precio > 0) {
                                    $.messager.confirm('Confirmaci&oacute;n', '¿Desea que se autogeneren las comisiones?<br><br>Deberá presionar Guardar para que los cambios se hagan efectivos.', function (result) {
                                        var clonarcomisiones = 0;
                                        if (result) {
                                            clonarcomisiones = 1;
                                        }
                                        $('#dgv_precio_articulo').datagrid('updateRow', {
                                            index: rowIndex,
                                            row: {
                                                tiene_comision: false,
                                                codigo_comision: '',
                                                clonarcomisiones: clonarcomisiones
                                            }
                                        });
                                    });
                                }
                                else {
                                    $.messager.alert('Operación exitosa', data.v_mensaje, 'info');
                                }
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



            //if (!ValidarDuplicidadProfesion(rowIndex)) {
            //    rowdata.confirmado = true;
            //    $('#dgv_precio_articulo').datagrid('endEdit', rowIndex);                
            //}
            //else {
            //    $.messager.alert("Duplicidad de Registro", "La Profesi&oacute;n/Especialidad ya se encuentra registrada, intente nuevamente.", "error");
            //    return;
            //}
        }
    }
}

function NuevoPrecio() {

    if (!$("#frmregistrar").form('enableValidation').form('validate'))
        return;
    if (!existeRegistroEnEdicion('dgv_precio_articulo')) {
        var new_row = {
            codigo_precio: -new Date().toString("ddHHmmss"),
            codigo_empresa: "",
            codigo_tipo_venta: "",
            codigo_moneda: "",
            precio: 0.0,
            precio_total: 0.0,
            cuota_inicial:0.0,
            igv: 0.0,
            str_vigencia_inicio: "",
            str_vigencia_fin: "",
            tiene_comision: false,
            nuevo: true,
            confirmado: false,
            actualizado: 0,
            clonarcomisiones:0
        }

        $("#dgv_precio_articulo").datagrid("appendRow", new_row);
        var editIndex = $("#dgv_precio_articulo").datagrid("getRowIndex", new_row.codigo_precio);
        $("#dgv_precio_articulo").datagrid("selectRow", editIndex);
        $("#dgv_precio_articulo").datagrid("beginEdit", editIndex);

        /*******************************************************************************************/
       

        var Editor = $("#dgv_precio_articulo").datagrid("getEditor", { index: editIndex, field: 'precio_total' });
        if ($(Editor.target).data('numberbox')) {

            
            $(Editor.target).numberbox({ validType: 'mayorACero[0]' });
            $(Editor.target).numberbox('textbox').css('text-align', 'right');            
        }
        /*
        var ed_cuota_inicial = $("#dgv_precio_articulo").datagrid("getEditor", { index: editIndex, field: 'cuota_inicial' });
        if ($(ed_cuota_inicial.target).data('numberbox')) {

            console.log($(ed_cuota_inicial.target));
            $(ed_cuota_inicial.target).numberbox({
                validType: "MayorACeroMenorIgualANumberBox['td[field=\"precio_total\"] input.numberbox-f']"               
           });
            $(ed_cuota_inicial.target).numberbox('textbox').css('text-align', 'right');            
        }*/

        /*******************************************************************************************/
        $("#btn_articulo_crear").linkbutton('disable');
        $("#btn_articulo_quitar").linkbutton('disable');
        $("#btn_articulo_cancelar").linkbutton('enable');
        $("#btn_articulo_aceptar").linkbutton('enable');
    }
}


function RegistrarArticulo() {
    if (!$("#frmregistrar").form('enableValidation').form('validate'))
        return;
    if (existeRegistroEnEdicion('dgv_precio_articulo'))
        return;
    var rows = $("#dgv_precio_articulo").datagrid("getRows");
    if (rows.length < 1) {
        $.messager.alert("Configurar precio articulo", "Registre configuración  precio articulo, intente nuevamente.", "warning");
        return;
    }

    var v_lista_precio_articulo = [];

    $.each(rows, function (index, data) {
        var v_precio = {
            codigo_articulo: ActionArticuloUrl._codigo_articulo,
            codigo_precio: data.codigo_precio,
            codigo_empresa: data.codigo_empresa,
            codigo_tipo_venta: data.codigo_tipo_venta,
            cuota_inicial:0,// parseFloat(data.cuota_inicial),
            codigo_moneda: data.codigo_moneda,
            precio: parseFloat(data.precio),
            igv: parseFloat(data.igv),
            precio_total: parseFloat(data.precio_total),
            str_vigencia_inicio: data.str_vigencia_inicio,
            str_vigencia_fin: data.str_vigencia_fin,
            /*
            vigencia_inicio: data.str_vigencia_inicio,
            vigencia_fin: data.str_vigencia_fin,
            */
            estado_registro: true,
            actualizado: data.actualizado,
            clonarcomisiones: data.clonarcomisiones
        };
        v_lista_precio_articulo.push(v_precio);
    });

    $.each(lista_precio_articulo_eliminado, function (index, data) {
        var v_precio = {
            codigo_articulo: ActionArticuloUrl._codigo_articulo,
            codigo_precio: data.codigo_precio,
            codigo_empresa: data.codigo_empresa,
            codigo_tipo_venta: data.codigo_tipo_venta,
            codigo_moneda: data.codigo_moneda,
            precio: parseFloat(data.precio),
            igv: parseFloat(data.igv),
            precio_total: parseFloat(data.precio_total),
            str_vigencia_inicio: data.str_vigencia_inicio,
            str_vigencia_fin: data.str_vigencia_fin,
            /*vigencia_inicio: data.str_vigencia_inicio,
            vigencia_fin: data.str_vigencia_fin,*/
            estado_registro: false
        };
        v_lista_precio_articulo.push(v_precio);

    });

    var v_anho_excepcion = $("#frmregistrar #nbb_anio_contrato_vinculante").numberbox('getValue');
    
    if (v_anho_excepcion.length) {
        
        if (v_anho_excepcion.length!=4)
        {
            $.messager.alert('Campo obligatorio', "El campo año excepción es de longitud 4", 'warning');
            return;
        }

    }
    else {
        v_anho_excepcion = null;
    }
    
    
        //console.log(v_lista_precio_articulo);
    var v_articulo = {
        codigo_articulo: ActionArticuloUrl._codigo_articulo,
        codigo_unidad_negocio: $("#frmregistrar #cmb_unidad_negocio").combobox('getValue'),
        codigo_categoria: $("#frmregistrar #cmb_categoria").combobox('getValue'),

        codigo_tipo_articulo: $("#frmregistrar #cmb_tipo_articulo").combobox('getValue'),

        codigo_sku: $.trim($("#frmregistrar #txt_articulo_codigo_sku").val()),
        nombre: $.trim($("#frmregistrar #txt_articulo_nombre").val()),
        abreviatura: $.trim($("#frmregistrar #txt_articulo_abreviatura").val()),
        genera_comision: $('#frmregistrar #ckb_genera_comision').switchbutton('options').checked,
        genera_bono: $('#frmregistrar #ckb_genera_bono').switchbutton('options').checked,
        genera_bolsa_bono: $('#frmregistrar #ckb_genera_bolsa_bono').switchbutton('options').checked,

        tiene_contrato_vinculante:v_anho_excepcion==null?false:true,
        anio_contrato_vinculante: $("#frmregistrar #nbb_anio_contrato_vinculante").numberbox('getValue'),
        cantidad_unica: $('#frmregistrar #ckb_cantidad_unica').switchbutton('options').checked,
        lista_precio_articulo: v_lista_precio_articulo
    };

    $.each(lst_comision, function (index, rows) {
        rows.valor = parseFloat(rows.valor);
        rows.estado_registro = true;
    });
    $.each(lst_comision_supervisor, function (index, rows) {
        rows.valor = parseFloat(rows.valor);
        rows.estado_registro = true;
    });
    
    $.each(lst_comision_eliminar, function (index, rows) {
        rows.valor = parseFloat(rows.valor);
        rows.estado_registro = false;
        lst_comision.push(rows);
    });

    $.each(lst_comision_supervisor_eliminar, function (index, rows) {
        rows.valor = parseFloat(rows.valor);
        rows.estado_registro = false;
        lst_comision_supervisor.push(rows);
    });

    $.messager.confirm('Confirmaci&oacute;n', '&iquest;Est&aacute; seguro que desea registrar?', function (result) {
        if (result) {
            $.ajax({
                type: 'post',
                url: ActionArticuloUrl._Registrar,
                data: JSON.stringify({ v_entidad: v_articulo, lst_regla_calcula_comision: lst_comision, lst_comision_precio_supervisor: lst_comision_supervisor }),
                async: true,
                cache: false,
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    if (data.v_resultado == 1) {

                        ActionArticuloUrl._codigo_articulo = data.codigo_articulo;

                        // $('#txt_codigo_articulo').textbox('setText', data.codigo_articulo);

                        $.messager.alert('Operación exitosa', data.v_mensaje, 'info', function () {
                            lst_comision = [];
                            lst_comision_eliminar = [];
                            lst_comision_supervisor = [];
                            lst_comision_supervisor_eliminar = [];
                            lista_precio_articulo_eliminado = [];
                            fnListarPrecioByArticulo();
                            fnBuscarArticulo(1, 50);
                            $("#div_registrar_articulo").dialog("close");
                            fnModificarArticuloByCodigo(data.codigo_articulo);
                        });
                    }
                    else {
                        $.messager.alert('Error en la operación.', data.v_mensaje, 'error');
                    }
                },
                error: function () {
                    $.messager.alert('Error', data.v_mensaje, 'error');
                }
            });
        }
    });
}


function fnMantenimientoAnhoExcepcion()
{
    var tiene_contrato_vinculante=$('#frmregistrar #ckb_contrato_vinculante').switchbutton('options').checked;
    var anio_contrato_vinculante = $("#frmregistrar #nbb_anio_contrato_vinculante").numberbox('getValue');

    $.messager.anho_excepcion("Registro de excepción de contrato vinculante", 'A&ntilde;o vinculante contrato', anio_contrato_vinculante, tiene_contrato_vinculante, function (win, data, ckb_tiene_contrato_vinculante)
    {
        if (ckb_tiene_contrato_vinculante!=undefined)
        {
            if (ckb_tiene_contrato_vinculante) {
                $('#frmregistrar #ckb_contrato_vinculante').switchbutton('check');
            }
            else {
                $('#frmregistrar #ckb_contrato_vinculante').switchbutton('uncheck');
            }
           
            $("#frmregistrar #nbb_anio_contrato_vinculante").numberbox('setValue', data);
        }     
    });
}

function fnReporteArticulo(p_codigo_articulo)
{
    $(this).AbrirVentanaEmergente({
        parametro: "?p_codigo_articulo=" + p_codigo_articulo,
        div: 'div_reporte_articulo',
        title: "Reporte de Artículo",
        url: ActionArticuloUrl._Reporte
    });
}

function copiaPrecio(index) {
    var rowdata = $('#dgv_precio_articulo').datagrid("getRows")[index];

    lista_precio_original = {
        codigo_precio: rowdata.codigo_precio,
        codigo_empresa: rowdata.codigo_empresa,
        codigo_tipo_venta: rowdata.codigo_tipo_venta,
        codigo_moneda: rowdata.codigo_moneda,
        vigencia_inicio: rowdata.vigencia_inicio,
        vigencia_fin: rowdata.vigencia_fin,
        precio_total: parseFloat(rowdata.precio_total),
        cuota_inicial: rowdata.cuota_inicial
    };
}