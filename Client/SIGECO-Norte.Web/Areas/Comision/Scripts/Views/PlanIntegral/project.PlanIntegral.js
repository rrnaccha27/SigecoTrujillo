var JsonTipoArticulo = {};
var JsonCamposanto = {};
var EstadoRegistro = false;

function camposantoFormatter(value) {

    for (var i = 0; i < JsonCamposanto.length; i++) {
        if (JsonCamposanto[i].id == value) return JsonCamposanto[i].text;
    }
    return value;
}

function tipoarticuloFormatter(value) {

    for (var i = 0; i < JsonTipoArticulo.length; i++) {
        if (JsonTipoArticulo[i].id == value) return JsonTipoArticulo[i].text;
    }
    return value;
}

;(function (app) {
    //===========================================================================================
    var current = app.PlanIntegral = {};
    //===========================================================================================

    jQuery.extend(app.PlanIntegral,
        {

            ActionUrls: {},
            EditType: '',
            HasRootNode: 'False',

            Initialize: function (actionUrls) {
                //$(window).resize(function () {
                //    current.Redimensionar();
                //});

                jQuery.extend(project.ActionUrls, actionUrls);

                InicializarDetalle();
                IncializarGrillaDetalle();

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


            },

        })
})(project);

function InicializarDetalle() {

    JsonTipoArticulo = $('.content').get_json_combobox({
        url: project.ActionUrls.GetTipoArticuloJson
    });

    JsonCamposanto = $('.content').get_json_combobox({
        url: project.ActionUrls.GetCamposantoJson
    });

}

function IncializarGrillaDetalle() {
    $('#dgv_lista_detalle').datagrid({
        fitColumns: false,
        url: project.ActionUrls.GetDetalleJson,
        queryParams: {
            codigo_plan_integral: project.ActionUrls.Codigo_Plan_Integral
        },
        idField: 'codigo_plan_integral_detalle',
        pagination: false,
        height: 200,
        singleSelect: true,
        rownumbers: true,
        columns: [[
            { field: 'codigo_plan_integral_detalle', hidden: true },
            {
                field: 'codigo_campo_santo', title: 'Camposanto', width: 150, align: 'left',
                editor: {
                    type: 'combobox',
                    options: {
                        valueField: 'id',
                        textField: 'text',
                        data: JsonCamposanto,
                        editable: false,
                        required: true
                    }
                },
                formatter: camposantoFormatter
            },
            {
                field: 'codigo_tipo_articulo', title: 'Tipo 1', width: 250, align: 'left', editor: {
                    type: 'combobox',
                    options: {
                        valueField: 'id',
                        textField: 'text',
                        data: JsonTipoArticulo,
                        editable: false,
                        required: true
                    }
                },
                formatter: tipoarticuloFormatter
            },
            {
                field: 'codigo_tipo_articulo_2', title: 'Tipo 2', width: 200, align: 'left',
                editor: {
                    type: 'combobox',
                    options: {
                        valueField: 'id',
                        textField: 'text',
                        data: JsonTipoArticulo,
                        editable: false,
                        required: true
                    }
                },
                formatter: tipoarticuloFormatter
            },
            { field: 'estado_registro_nombre', title: 'Estado', width: 120, align: 'left' }
        ]],

        onAfterEdit: function (index, row) {
            row.editing = false;
        },

        onClickRow: function (rowIndex, rowdata) {
            if (!project.ActionUrls.Estado_Registro)
            { return false; }

            if (!existeRegistroEnEdicion("dgv_lista_detalle")) {
                $("#btn_detalle_crear").linkbutton('enable');
                $("#btn_detalle_quitar").linkbutton('enable');
                $("#btn_detalle_cancelar").linkbutton('disable');
                $("#btn_detalle_aceptar").linkbutton('disable');
            }
        },

        onDblClickRow: function (rowIndex, rowdata) {
            if (!project.ActionUrls.Estado_Registro)
            { return false; }

            if (rowdata.codigo_plan_integral_detalle > 0) {
                $.messager.alert("Plan Integral", "El registro seleccionado no se puede editar.", "warning");
                return;
            }
            if (!$.existeRegistroEnEdicion("dgv_lista_detalle")) {
                $('#dgv_lista_detalle').datagrid('beginEdit', rowIndex);
                rowdata.editing = true;
                rowdata.confirmado = true;

                $("#btn_detalle_crear").linkbutton('disable');
                $("#btn_detalle_quitar").linkbutton('disable');
                $("#btn_detalle_cancelar").linkbutton('enable');
                $("#btn_detalle_aceptar").linkbutton('enable');

            }
        }
    });
    $('#dgv_lista_detalle').datagrid('enableFilter');
    $(window).resize(function () {
        $('#dgv_lista_detalle').datagrid('resize');
    });
}

function CrearDetalle() {
    if (!$.existeRegistroEnEdicion('dgv_lista_detalle')) {
        var new_row = {
            codigo_plan_integral_detalle: -new Date().toString("ddHHmmss")
            , codigo_campo_santo: null
            , codigo_tipo_articulo: null
            , codigo_tipo_articulo_2: null
            , estado_registro: true
            , estado_registro_nombre: 'Activo'
            , confirmado: false
            , editing: true
        }

        $("#dgv_lista_detalle").datagrid("appendRow", new_row);
        var editIndex = $("#dgv_lista_detalle").datagrid("getRowIndex", new_row.codigo_plan_integral_detalle);
        $("#dgv_lista_detalle").datagrid("selectRow", editIndex);
        $("#dgv_lista_detalle").datagrid("beginEdit", editIndex);

        $("#btn_detalle_crear").linkbutton('disable');
        $("#btn_detalle_quitar").linkbutton('disable');
        $("#btn_detalle_cancelar").linkbutton('enable');
        $("#btn_detalle_aceptar").linkbutton('enable');
    }
}

function CancelarDetalle() {
    var rowIndex = IndexRowEditing("dgv_lista_detalle");
    if (rowIndex != null) {
        var row_precio = $('#dgv_lista_detalle').datagrid("getRows")[rowIndex];
        if (row_precio.confirmado) {
            $('#dgv_lista_detalle').datagrid('cancelEdit', rowIndex);
        }
        else {
            $('#dgv_lista_detalle').datagrid('deleteRow', rowIndex);
        }

        $("#btn_detalle_crear").linkbutton('enable');
        $("#btn_detalle_quitar").linkbutton('enable');
        $("#btn_detalle_cancelar").linkbutton('disable');
        $("#btn_detalle_aceptar").linkbutton('disable');
        $('#dgv_lista_detalle').datagrid('clearSelections');
    }
}

function EliminarDetalle() {
    if (!existeRegistroEnEdicion("dgv_lista_detalle")) {
        var rows = $("#dgv_lista_detalle").datagrid("getSelections");
        if (rows.length <= 0) {
            $.messager.alert("Eliminar", "Seleccione un registro.", "warning");
        }
        else {
            var row = $("#dgv_lista_detalle").datagrid("getSelected");

            if (row.estado_registro_nombre == 'Inactivo') {
                $.messager.alert("Eliminar", "No se puede eliminar este registro.", "warning");
                return false;
            }

            $(".messager-window,.window-shadow,.window-mask").remove();

            if (row.codigo_plan_integral_detalle > 0) {
                $.messager.confirm('Confirmar', 'Este registro solo puede ser desactivado, seguro de continuar?', function (r) {
                    if (r) {
                        var index = $("#dgv_lista_detalle").datagrid("getRowIndex", row.codigo_plan_integral_detalle);
                        $('#dgv_lista_detalle').datagrid('beginEdit', index);
                        var rowdata = $('#dgv_lista_detalle').datagrid("getRows")[index];
                        rowdata.estado_registro = false;
                        rowdata.estado_registro_nombre = 'Inactivo';
                        $('#dgv_lista_detalle').datagrid('endEdit', index);
                    }
                });
            }

            if (row && row.codigo_plan_integral_detalle < 0) {
                $.messager.confirm('Confirmar', '&iquest;Est&aacute; seguro de eliminar este registro?', function (r) {
                    if (r) {
                        var index = $("#dgv_lista_detalle").datagrid("getRowIndex", row.codigo_plan_integral_detalle);
                        var rowdata = $('#dgv_lista_detalle').datagrid("getRows")[index];
                        $('#dgv_lista_detalle').datagrid('deleteRow', index);

                        $("#btn_detalle_crear").linkbutton('enable');
                        $("#btn_detalle_quitar").linkbutton('enable');
                        $("#btn_detalle_cancelar").linkbutton('disable');
                        $("#btn_detalle_aceptar").linkbutton('disable');
                        $('#dgv_lista_detalle').datagrid('clearSelections');
                    }
                });
            }
        }
    }
}

function AceptarDetalle() {
    if (existeRegistroEnEdicionParaGrabar("dgv_lista_detalle")) {
        var rowIndex = IndexRowEditing("dgv_lista_detalle");
        var rowdata = $('#dgv_lista_detalle').datagrid("getRows")[rowIndex];
        if (validarRowGrilla(rowIndex, 'dgv_lista_detalle')) {
            if (ValidarDataConsistente('dgv_lista_detalle', rowIndex, rowdata.codigo_plan_integral_detalle)) {
                rowdata.confirmado = true;
                $('#dgv_lista_detalle').datagrid('endEdit', rowIndex);
                $("#btn_detalle_crear").linkbutton('enable');
                $("#btn_detalle_quitar").linkbutton('enable');
                $("#btn_detalle_cancelar").linkbutton('disable');
                $("#btn_detalle_aceptar").linkbutton('disable');
            }
        }
    }
}

function GuardarPlan() {
    var esNuevo = project.ActionUrls.Codigo_Plan_Integral == -1 ? true : false;
    var message = '';
    var codigo_plan_integral = project.ActionUrls.Codigo_Plan_Integral;
    var nombre = $.trim($('#nombre').val());
    var vigencia_inicio = $.trim($('#fechaInicio').textbox('getText'));
    var vigencia_fin = $.trim($('#fechaFin').textbox('getText'));

    if (!$("#frmRegistro").form('enableValidation').form('validate')) {
        return false;
    }

    if (!ValidarFecha(vigencia_inicio)) {
        $.messager.alert("Plan Integral", "Inicio de Vigencia en formato incorrecto.", "warning");
        return false;
    }

    if (!ValidarFecha(vigencia_fin)) {
        $.messager.alert("Plan Integral", "Fin de Vigencia en formato incorrecto.", "warning");
        return false;
    }

    vigencia_inicio = FormatoFecha(vigencia_inicio);
    vigencia_fin = FormatoFecha(vigencia_fin);

    if (parseInt(vigencia_inicio) > parseInt(vigencia_fin)) {
        $.messager.alert("Plan Integral", "Inicio de Vigencia debe ser menor a Fin Vigencia.", "warning");
        return false;
    }

    if (existeRegistroEnEdicion('dgv_lista_detalle'))
        return;

    var rows = $("#dgv_lista_detalle").datagrid("getRows");
    if (rows.length < 1) {
        $.messager.alert("Plan Integral", "Registre un detalle, intente nuevamente.", "warning");
        return;
    }

    var listaDetalle = [];

    $.each(rows, function (index, data) {
        var canal_grupo = {
            codigo_plan_integral_detalle: data.codigo_plan_integral_detalle,
            codigo_campo_santo: data.codigo_campo_santo,
            codigo_tipo_articulo: data.codigo_tipo_articulo,
            codigo_tipo_articulo_2: data.codigo_tipo_articulo_2,
            estado_registro: data.estado_registro
        };
        listaDetalle.push(canal_grupo);
    });

    var datosPlan = {
        codigo_plan_integral: codigo_plan_integral,
        nombre: nombre,
        vigencia_inicio: vigencia_inicio,
        vigencia_fin: vigencia_fin,
        plan_integral_detalle: listaDetalle,
    };

    if (message.length > 0) {
        $.messager.alert((esNuevo ? 'Registro' : 'Modificar'), message, 'warning');
        return false;
    }

    $.messager.confirm('Confirm', '&iquest;Seguro que desea guardar?', function (result) {
        if (result) {
            $.ajax({
                type: 'post',
                url: project.ActionUrls.Guardar,
                data: JSON.stringify({ plan: datosPlan }),
                dataType: 'json',
                cache: false,
                async: false,
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    if (data.Msg) {
                        if (data.Msg != 'Success') {
                            project.AlertErrorMessage('Error', data.Msg);
                        }
                        else {
                            $('#dlgRegistro').dialog('close');
                            project.ShowMessage('Alerta', (esNuevo ? 'Registro' : 'Modificado') + ' Exitoso');
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

function CancelarPlan() {
    $('#dlgRegistro').dialog('close');
}

function ValidarFecha(fecha) {
    try {
        testdate = $.datepicker.parseDate('dd/mm/yy', fecha);
        return true;
    } catch (e) {

        return false;
    }
}

function FormatoFecha(fecha) {
    return fecha.substring(6, 10) + fecha.substring(3, 5) + fecha.substring(0, 2);
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

function ValidarDataConsistente(lista, indiceEditado, codigo_registro) {
    lista = "#" + lista;
    var resultado = true;
    var rows = $(lista).datagrid("getRows");

    var cmb_campo_santo = $(lista).datagrid("getEditor", { index: indiceEditado, field: 'codigo_campo_santo' });
    var cmb_tipo_articulo = $(lista).datagrid("getEditor", { index: indiceEditado, field: 'codigo_tipo_articulo' });
    var cmb_tipo_articulo_2 = $(lista).datagrid("getEditor", { index: indiceEditado, field: 'codigo_tipo_articulo_2' });
    var codigo_campo_santo = cmb_campo_santo.target.combobox("getValue");
    var codigo_tipo_articulo = cmb_tipo_articulo.target.combobox("getValue");
    var codigo_tipo_articulo_2 = cmb_tipo_articulo_2.target.combobox("getValue");

    //var chk_es_supervisor_canal = $(lista).datagrid("getEditor", { index: indiceEditado, field: 'es_supervisor_canal' });
    //var chk_es_supervisor_grupo = $(lista).datagrid("getEditor", { index: indiceEditado, field: 'es_supervisor_grupo' });
    //var chk_estado_registro = $(lista).datagrid("getEditor", { index: indiceEditado, field: 'estado_registro' });

    //var es_supervisor_canal = chk_es_supervisor_canal.target.prop('checked');
    //var es_supervisor_grupo = chk_es_supervisor_grupo.target.prop('checked');
    //var estado_registro = chk_estado_registro.target.prop('checked');

    $.each(rows, function (i, objeto) {
        if (i != indiceEditado) {
            if (objeto.codigo_campo_santo == codigo_campo_santo &&
                (
                    (objeto.codigo_tipo_articulo == codigo_tipo_articulo && objeto.codigo_tipo_articulo_2 == codigo_tipo_articulo_2)
                    ||
                    (objeto.codigo_tipo_articulo == codigo_tipo_articulo_2 && objeto.codigo_tipo_articulo_2 == codigo_tipo_articulo))
                ) {
                $.messager.alert("Plan Integral", "Ya hay una misma configuración ingresada.", "warning");
                resultado = false;
                return false;
            }
        }
    });

    if (resultado) {
        if (codigo_tipo_articulo == codigo_tipo_articulo_2) {
            $.messager.alert("Plan Integral", "El tipo de artículo no debe ser el mismo en ambos criterios.", "warning");
            resultado = false;
        }
    }

    //if (resultado) {
    //    if (es_supervisor_canal && es_supervisor_grupo) {
    //        $.messager.alert("Canal", "No puede ser supervisor de Canal y Grupo a la vez.", "warning");
    //        resultado = false;
    //    }
    //}

    //if (resultado) {
    //    if (es_supervisor_canal && codigo_grupo != '') {
    //        $.messager.alert("Canal", "No puede ser supervisor de Canal si selecciono un Grupo.", "warning");
    //        resultado = false;
    //    }
    //}

    //if (resultado) {
    //    if (!estado_registro && codigo_registro < 1) {
    //        $.messager.alert("Canal", "No puede guardar un registro nuevo como no activo.", "warning");
    //        resultado = false;
    //    }
    //}

    return resultado;
}
