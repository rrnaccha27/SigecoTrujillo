;
(function (app) {
    //===========================================================================================
    var current = app.ReglaPagoComision.Registro = {};

    var elementoTodos = { "id": "0", "text": "Ninguno" };
    //===========================================================================================

    jQuery.extend(app.ReglaPagoComision.Registro,
        {

            ActionUrls: {},



            comboArticulo: {},
            Initialize: function (actionUrls) {
                jQuery.extend(project.ActionUrls, actionUrls);

                current.LoadCombobox();



                ///cargar grilla popup
                if (project.ActionUrls.CodigoReglaComision > 0) {
                    current.Initialize_dgv_meta_comision();
                    current.Initialize_dgv_detalle_comision();

                }

                /*PARA REDIMENSIONAR LAS INTERFACES*/
                var mediaquery = window.matchMedia("(max-width: 600px)");
                if (mediaquery.matches) {
                    $('#div_registrar_regla').dialog('resize', { width: '95%' });
                    $('#div_registrar_regla').window('center');
                } else {
                    $('#div_registrar_regla').dialog('resize', { width: '70%' });
                    $('#div_registrar_regla').window('center');
                }

                $(window).resize(function () {

                    var mediaquery = window.matchMedia("(max-width: 600px)");
                    if (mediaquery.matches) {
                        $('#div_registrar_regla').dialog('resize', { width: '95%' });
                    } else {
                        $('#div_registrar_regla').dialog('resize', { width: '70%' });
                    }
                    $('#div_registrar_regla').window('center');
                });
                //FIN REDIMENSIONAMIENTO





                $('#div_registrar_regla #btnGuardar').click(function () {
                    current.Registrar();
                });

                $('#div_registrar_regla #btnCancelar').click(function () {
                    $('#div_registrar_regla').dialog('close');
                });



                ///DETALLE META
                $('#div_registrar_regla #href_nueva_meta').click(function () {
                    if ($(this).linkbutton('options').disabled)
                        return;

                    
                    current.NuevaMeta();

                    $("#href_nueva_meta").linkbutton('disable');
                    $("#href_cancelar_meta").linkbutton('enable');
                    $("#href_eliminar_meta").linkbutton('disable');
                    $("#href_grabar_meta").linkbutton('enable');

                });
                $('#div_registrar_regla #href_grabar_meta').click(function () {

                    if ($(this).linkbutton('options').disabled)
                        return;
                    current.GrabarMeta();
                });
                $('#div_registrar_regla #href_cancelar_meta').click(function () {
                    if ($(this).linkbutton('options').disabled)
                        return;
                    current.cancelarMeta();

                    $("#href_nueva_meta").linkbutton('enable');
                    $("#href_cancelar_meta").linkbutton('disable');
                    $("#href_eliminar_meta").linkbutton('disable');
                    $("#href_grabar_meta").linkbutton('disable');
                });
                $('#div_registrar_regla #href_eliminar_meta').click(function () {
                    if ($(this).linkbutton('options').disabled)
                        return;
                    current.EliminarMeta();

                    $("#href_nueva_meta").linkbutton('enable');
                    $("#href_cancelar_meta").linkbutton('disable');
                    $("#href_eliminar_meta").linkbutton('disable');
                    $("#href_grabar_meta").linkbutton('disable');
                });

                ///DETALLE REGLA
                $('#div_registrar_regla #href_nueva_detalle').click(function () {                    
                    current.ViewRegistroDetalleRegla(0);
                });
                $('#div_registrar_regla #href_ver_detalle').click(function () {

                    var row = $('#div_registrar_regla #dgv_detalle_comision').datagrid('getSelected');
                    if (!row) {
                        project.AlertErrorMessage('Alerta', "Debe seleccionar un registro.", 'warning');
                    }

                    current.ViewRegistroDetalleRegla(row.codigo_detalle_regla_comision);
                });

            },
            NuevaMeta: function () {

                if (current.existeRegistroEnEdicion("#div_registrar_regla #dgv_meta_comision"))
                    return;

                var new_row = {
                    codigo_meta_regla_comision: -new Date().toString("ddHHmmss"),
                    tope_unidad: "",
                    tope_unidad_comisionable: "",
                    tope_unidad_fin: "",
                    nuevo: true,
                    confirmado: false,
                    actualizado: 0
                }

                $('#div_registrar_regla #dgv_meta_comision').datagrid("appendRow", new_row);

                var editIndex = $("#div_registrar_regla #dgv_meta_comision").datagrid("getRowIndex", new_row.codigo_meta_regla_comision);

                $("#div_registrar_regla #dgv_meta_comision").datagrid("selectRow", editIndex);
                $("#div_registrar_regla #dgv_meta_comision").datagrid("beginEdit", editIndex);

                /*******************************************************************************************/


                var tope_unidad = $("#div_registrar_regla #dgv_meta_comision").datagrid("getEditor", { index: editIndex, field: 'tope_unidad' });
                if ($(tope_unidad.target).data('numberbox')) {
                    $(tope_unidad.target).numberbox({ validType: 'mayorACero[0]' });
                    $(tope_unidad.target).numberbox('textbox').css('text-align', 'right');
                }

                var tope_unidad_comisionable = $("#div_registrar_regla #dgv_meta_comision").datagrid("getEditor", { index: editIndex, field: 'tope_unidad_comisionable' });
                if ($(tope_unidad_comisionable.target).data('numberbox')) {
                    $(tope_unidad_comisionable.target).numberbox({ validType: 'mayorACero[0]' });
                    $(tope_unidad_comisionable.target).numberbox('textbox').css('text-align', 'right');
                }
                var tope_unidad_fin = $("#div_registrar_regla #dgv_meta_comision").datagrid("getEditor", { index: editIndex, field: 'tope_unidad_fin' });
                if ($(tope_unidad_fin.target).data('numberbox')) {
                    $(tope_unidad_fin.target).numberbox({ validType: 'mayorACero[0]' });
                    $(tope_unidad_fin.target).numberbox('textbox').css('text-align', 'right');
                }


            },
            GrabarMeta: function () {
                var grilla = "#div_registrar_regla #dgv_meta_comision";

                if (!current.existeRegistroEnEdicionParaGrabar(grilla)) return;

                var rowIndex = current.IndexRowEditing(grilla);
                var rowdata = $(grilla).datagrid("getRows")[rowIndex];
                if (!current.validarRowGrilla(rowIndex, grilla)) return;


                var txt_tope_unidad = $(grilla).datagrid("getEditor", { index: rowIndex, field: 'tope_unidad' });
                var txt_tope_unidad_comisionable = $(grilla).datagrid("getEditor", { index: rowIndex, field: 'tope_unidad_comisionable' });
                var txt_tope_unidad_fin = $(grilla).datagrid("getEditor", { index: rowIndex, field: 'tope_unidad_fin' });

                var data = {
                    codigo_regla_comision: project.ActionUrls.CodigoReglaComision,
                    codigo_meta_regla_comision: rowdata.codigo_meta_regla_comision,
                    tope_unidad: parseFloat(txt_tope_unidad.target.numberbox("getValue")),
                    tope_unidad_comisionable: parseFloat(txt_tope_unidad_comisionable.target.numberbox("getValue")),
                    tope_unidad_fin: parseFloat(txt_tope_unidad_fin.target.numberbox("getValue"))
                };


                $.messager.confirm('Confirmaci&oacute;n', '&iquest;Est&aacute; seguro que desea grabar?', function (result) {
                    if (result) {
                        $.ajax({
                            type: 'post',
                            url: project.ActionUrls.RegistrarMetaReglaComision,
                            data: JSON.stringify(data),
                            async: true,
                            cache: false,
                            dataType: 'json',
                            contentType: 'application/json; charset=utf-8',
                            success: function (response) {
                                if (response.v_resultado == 1) {
                                    //OK
                                    project.ShowMessage('Alerta', response.v_mensaje);
                                    $(grilla).datagrid('clearSelections');
                                    current.Reload_dgv_meta();
                                } else {
                                    //ERROR
                                    project.AlertErrorMessage('Alerta', response.v_mensaje, 'info');

                                }

                            },
                            error: function (response) {
                                $.messager.alert('Error', "Ocurrio un error inesperado en proceso de registro.", 'error');
                            }
                        });
                    }
                });



            },
            EliminarMeta: function () {
                var grilla = "#div_registrar_regla #dgv_meta_comision";
                if (current.existeRegistroEnEdicion(grilla)) return;

                var row = $(grilla).datagrid('getSelected');
                if (!row) {
                    project.AlertErrorMessage('Alerta', 'Necesita seleccionar un registro', 'info');
                    return;
                }

                var data = {
                    codigo_meta_regla_comision: row.codigo_meta_regla_comision
                };


                $.messager.confirm('Confirmaci&oacute;n', '&iquest;Est&aacute; seguro que desea eliminar?', function (result) {
                    if (result) {
                        $.ajax({
                            type: 'post',
                            url: project.ActionUrls.EliminarMetaReglaComision,
                            data: JSON.stringify(data),
                            async: true,
                            cache: false,
                            dataType: 'json',
                            contentType: 'application/json; charset=utf-8',
                            success: function (response) {
                                if (response.v_resultado == 1) {
                                    //OK
                                    project.ShowMessage('Alerta', response.v_mensaje);
                                    $(grilla).datagrid('clearSelections');
                                    current.Reload_dgv_meta();
                                } else {
                                    //ERROR
                                    project.AlertErrorMessage('Alerta', response.v_mensaje, 'info');

                                }

                            },
                            error: function (response) {
                                $.messager.alert('Error', "Ocurrio un error inesperado en proceso de eliminación.", 'error');
                            }
                        });
                    }
                });


            },
            cancelarMeta: function () {
                var rowIndex = current.IndexRowEditing("#div_registrar_regla #dgv_meta_comision");
                if (rowIndex != null) {

                    var row_precio = $("#div_registrar_regla #dgv_meta_comision").datagrid("getRows")[rowIndex];
                    if (row_precio.codigo_meta_regla_comision > 0) {
                        $("#div_registrar_regla #dgv_meta_comision").datagrid('cancelEdit', rowIndex);
                    }
                    else {
                        $("#div_registrar_regla #dgv_meta_comision").datagrid('deleteRow', rowIndex);
                    }

                    //$("#btn_articulo_crear").linkbutton('enable');
                    //$("#btn_articulo_quitar").linkbutton('disable');
                    //$("#btn_articulo_cancelar").linkbutton('disable');
                    //$("#btn_articulo_aceptar").linkbutton('disable');
                    //$('#dgv_precio_articulo').datagrid('clearSelections');

                }
            },
            cancelarEdicion: function (dgv) {
                //alert(dgv);
                var rowIndex = current.IndexRowEditing(dgv);
                if (rowIndex != null) {
                    $(dgv).datagrid('cancelEdit', rowIndex);
                }
            },
            IndexRowEditing: function (lista) {
                var index = null;
                var rows = $(lista).datagrid("getRows");
                $.each(rows, function (i, objeto) {
                    if (objeto.editing) {
                        index = i;
                        return false;
                    }
                });
                return index;
            },
            validarRowGrilla: function (index, lista) {
                var _Valid = $(lista).datagrid('validateRow', index);
                if (!_Valid) {
                    $.messager.alert("Campos Obligatorios", "Falta ingresar los campos requeridos de la grilla en edici&oacute;n.", "warning");
                }
                return _Valid;

            },
            existeRegistroEnEdicionParaGrabar: function (lista) {
                var editando = false;
                var rows = $(lista).datagrid("getRows");
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
            },
            existeRegistroEnEdicion: function (lista) {


                var editando = false;
                var rows = $(lista).datagrid("getRows");
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

            },

            Initialize_dgv_meta_comision: function () {


                $('#div_registrar_regla #dgv_meta_comision').datagrid({
                    url: project.ActionUrls.GetMetaReglaComision,
                    queryParams: { codigo_regla_comision: project.ActionUrls.CodigoReglaComision },
                    fitColumns: true,
                    idField: 'codigo_meta_regla_comision',
                    data: null,
                    pagination: true,
                    singleSelect: true,
                    rownumbers: true,
                    pageList: [20, 60, 80, 100, 150],
                    pageSize: 20,
                    toolbar: "#div_registrar_regla #toolbar_meta_comision",
                    columns:
                        [[
                            { field: 'codigo_meta_regla_comision', title: 'Código Meta Regla Comision', width: 150, hidden: true },
                            { field: 'codigo_regla_comision', title: 'Código  Regla Comision', width: 150, hidden: true },

                            {
                                field: 'tope_unidad', title: 'Tope Unidad', width: 100, halign: 'center', align: 'right', formatter: function (value, row) {
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
                            {
                                field: 'tope_unidad_comisionable', title: 'Tope Unidad Comisionable', width: 100, halign: 'center', align: 'right', formatter: function (value, row) {
                                    return $.NumberFormat(value, 2);
                                }, editor: {
                                    type: 'numberbox',
                                    options: {
                                        precision: 2,
                                        required: true,
                                        max: 9999999
                                    }
                                }
                            },

                            {
                                field: 'tope_unidad_fin', title: 'Tope Unidad Fin', width: 100, halign: 'center', align: 'right', formatter: function (value, row) {
                                    return $.NumberFormat(value, 2);
                                }, editor: {
                                    type: 'numberbox',
                                    options: {
                                        precision: 2,
                                        required: true,
                                        max: 9999999
                                    }
                                }
                            },
                            { field: 'estado_registro_nombre', title: 'Estado', width: 50, align: 'center', halign: 'center' },
                            { field: 'estado_registro', hidden: 'true' }

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
                    onClickRow: function (index, row) {
                        if (row.estado_registro) {
                            $("#href_nueva_meta").linkbutton('disable');
                            $("#href_cancelar_meta").linkbutton('disable');
                            $("#href_eliminar_meta").linkbutton('enable');
                            $("#href_grabar_meta").linkbutton('disable');
                        } else {
                            $("#href_nueva_meta").linkbutton('enable');
                            $("#href_cancelar_meta").linkbutton('disable');
                            $("#href_eliminar_meta").linkbutton('disable');
                            $("#href_grabar_meta").linkbutton('disable');
                        }

                    
                    },
                    onDblClickRow: function (rowIndex, rowdata) {


                        
                        if (!rowdata.estado_registro) {
                            project.AlertErrorMessage('Alerta', "El registro se encuentra inactivo.", 'warning');
                            return;
                        }

                        if (current.existeRegistroEnEdicion('#div_registrar_regla #dgv_meta_comision')) return;

                        $('#div_registrar_regla #dgv_meta_comision').datagrid('beginEdit', rowIndex);

                        var tope_unidad = $("#div_registrar_regla #dgv_meta_comision").datagrid("getEditor", { index: rowIndex, field: 'tope_unidad' });
                        if ($(tope_unidad.target).data('numberbox')) {
                            $(tope_unidad.target).numberbox({ validType: 'mayorACero[0]' });
                            $(tope_unidad.target).numberbox('textbox').css('text-align', 'right');
                        }

                        var tope_unidad_comisionable = $("#div_registrar_regla #dgv_meta_comision").datagrid("getEditor", { index: rowIndex, field: 'tope_unidad_comisionable' });
                        if ($(tope_unidad_comisionable.target).data('numberbox')) {
                            $(tope_unidad_comisionable.target).numberbox({ validType: 'mayorACero[0]' });
                            $(tope_unidad_comisionable.target).numberbox('textbox').css('text-align', 'right');
                        }
                        var tope_unidad_fin = $("#div_registrar_regla #dgv_meta_comision").datagrid("getEditor", { index: rowIndex, field: 'tope_unidad_fin' });
                        if ($(tope_unidad_fin.target).data('numberbox')) {
                            $(tope_unidad_fin.target).numberbox({ validType: 'mayorACero[0]' });
                            $(tope_unidad_fin.target).numberbox('textbox').css('text-align', 'right');
                        }

                        $("#href_nueva_meta").linkbutton('disable');
                        $("#href_cancelar_meta").linkbutton('enable');
                        $("#href_eliminar_meta").linkbutton('disable');
                        $("#href_grabar_meta").linkbutton('enable');

                    }
                });
                // 
                /******************************************************************************************************/
                /* $('#dlgRegistrar #dgv_meta_comision').datagrid('enableFilter', [{
                     field: 'estado_registro_nombre',
                     type: 'combobox',
                     options: {
                         panelHeight: 'auto',
                         data: [{ value: '', text: 'Todos' }, { value: '1', text: 'Activo' }, { value: '0', text: 'Inactivo' }],
                         editable: false,
                         onChange: function (value) {
 
                             if (value == '') {
                                 $('#dlgRegistrar #dgv_meta_comision').datagrid('removeFilterRule', 'indica_estado');
                             } else {
                                 $('#dlgRegistrar #dgv_meta_comision').datagrid('addFilterRule', {
                                     field: 'indica_estado',
                                     op: 'equal',
                                     value: value
                                 });
                             }
                             $('#dlgRegistrar #dgv_meta_comision').datagrid('doFilter');
                         }
                     }
                 }]);*/
                /*********************************************************************************************************/

            },
            Reload_dgv_meta: function () {
                $('#div_registrar_regla #dgv_meta_comision').datagrid('reload', { codigo_regla_comision: project.ActionUrls.CodigoReglaComision });
            },
            Initialize_dgv_detalle_comision: function () {


                $('#div_registrar_regla #dgv_detalle_comision').datagrid({
                    fitColumns: true,
                    idField: 'codigo_detalle_regla_comision',
                    toolbar: "#toolbar_detalle_comision",
                    url: project.ActionUrls.GetDetalleReglaComision,
                    queryParams: { codigo_regla_comision: project.ActionUrls.CodigoReglaComision },
                    data: null,
                    pagination: true,
                    singleSelect: true,
                    rownumbers: true,
                    pageList: [20, 60, 80, 100, 150],
                    pageSize: 20,
                    columns:
                        [[
                            { field: 'rango_inicio', title: 'Rango Inicio', width: 100, align: 'left', halign: 'center' },
                            { field: 'rango_fin', title: 'Rango Fin', width: 50, align: 'left', halign: 'center' },
                            { field: 'comision', title: 'Comisión', width: 50, align: 'left', halign: 'center' },
                            { field: 'nombre_tipo_comision', title: 'Tipo Comisión', width: 50, align: 'left', halign: 'center' },
                            { field: 'porcentaje_pago_comision', title: '% Pago Comisión', width: 50, align: 'left', halign: 'center' },
                            { field: 'orden_regla', title: 'Orden Regla', width: 50, align: 'center', halign: 'center' },
                            { field: 'estado_registro_nombre', title: 'Estado', width: 50, align: 'center', halign: 'center' },
                            { field: 'estado_registro', hidden: 'true' }
                            //{ field: 'indica_estado', hidden: 'true' }

                        ]],
                    onClickRow: function (index, row) {
                        if (row.estado_registro) {                       
                            $("#href_eliminar_detalle").linkbutton('enable');                            
                        } else {
                          
                            $("#href_eliminar_detalle").linkbutton('disable');                            
                        }
                    }
                });

                /******************************************************************************************************/
                /* $('#dlgRegistrar #dgv_detalle_comision').datagrid('enableFilter', [{
                     field: 'estado_registro_nombre',
                     type: 'combobox',
                     options: {
                         panelHeight: 'auto',
                         data: [{ value: '', text: 'Todos' }, { value: '1', text: 'Activo' }, { value: '0', text: 'Inactivo' }],
                         editable: false,
                         onChange: function (value) {
 
                             if (value == '') {
                                 $('#dlgRegistrar #dgv_detalle_comision').datagrid('removeFilterRule', 'indica_estado');
                             } else {
                                 $('#dlgRegistrar #dgv_detalle_comision').datagrid('addFilterRule', {
                                     field: 'indica_estado',
                                     op: 'equal',
                                     value: value
                                 });
                             }
                             $('#dlgRegistrar #dgv_detalle_comision').datagrid('doFilter');
                         }
                     }
                 }]);*/
                /*********************************************************************************************************/

            },
            Reload_dgv_detalle: function () {
                $('#div_registrar_regla #dgv_detalle_comision').datagrid('reload', { codigo_regla_comision: project.ActionUrls.CodigoReglaComision });
            },
            LoadCombobox: function () {


                $('.content').combobox_sigees({
                    id: "#div_registrar_regla #cmbCanalVenta",
                    url: project.ActionUrls.GetCanalGrupoJSON,
                });

                $('.content').combobox_sigees({
                    id: "#div_registrar_regla #cmbTipoVenta",
                    url: project.ActionUrls.GetTipoVentaJSON,
                });
                $('.content').combobox_sigees({
                    id: "#div_registrar_regla #cmbTipoPlanilla",
                    url: project.ActionUrls.GetTipoPlanillaJSON,
                });
                $('.content').combobox_sigees({
                    id: "#div_registrar_regla #cmbTipoArticulo",
                    url: project.ActionUrls.GetTipoArticuloJson,
                });


                $('#div_registrar_regla #cmbTipoArticulo').combobox({
                    onSelect: function (rec) {

                        current.GetArticulo(rec.id);
                        $('#div_registrar_regla #cmbArticulo').combobox({
                            valueField: 'id',
                            textField: 'text',
                            data: current.comboArticulo
                        });
                    }
                });
            },
            Registrar: function () {

                var message = '';

                if (message.length > 0) {
                    $.messager.alert('Error', message, 'error');
                }
                else {

                    var mensajeError = "";

                    var nombre_regla_comision = $.trim($('#div_registrar_regla #txt_nombre').textbox('getText'));

                    if (nombre_regla_comision.length == 0) {
                        mensajeError = "Debe ingresar un nombre.";
                        project.AlertErrorMessage('Alerta', mensajeError, 'warning');
                        return false;
                    }

                    //Validacion de seleccion
                    var codigo_tipo_planilla = $.trim($("#div_registrar_regla #cmbTipoPlanilla").combobox('getValue'));
                    var codigo_canal_grupo = $.trim($("#div_registrar_regla #cmbCanalVenta").combobox('getValue'));
                    var codigo_tipo_venta = $.trim($("#div_registrar_regla #cmbTipoVenta").combobox('getValue'));
                    var codigo_tipo_articulo = $.trim($("#div_registrar_regla #cmbTipoArticulo").combobox('getValue'));
                    var codigo_articulo = $.trim($("#div_registrar_regla #cmbArticulo").combobox('getValue'));

                    var tope_minimo_contrato = $.trim($('#div_registrar_regla #txt_tope_minimo_contrato').numberbox('getValue'));
                    var tope_unidad = $.trim($('#div_registrar_regla #txt_tope_unidad').numberbox('getValue'));
                    var meta_general = $.trim($('#div_registrar_regla #txt_meta_general').numberbox('getValue'));                   
                    

                    $.messager.confirm('Confirm', '&iquest;Seguro que desea registrar?', function (result) {
                        if (result) {

                            var v_entidad = {
                                codigo_regla_comision: project.ActionUrls.CodigoReglaComision,
                                nombre_regla_comision: nombre_regla_comision,
                                codigo_tipo_venta: codigo_tipo_venta,
                                codigo_tipo_articulo: codigo_tipo_articulo,
                                codigo_articulo: codigo_articulo,
                                codigo_canal_grupo: codigo_canal_grupo,
                                codigo_tipo_planilla: codigo_tipo_planilla,

                                tope_minimo_contrato: tope_minimo_contrato,
                                tope_unidad: tope_unidad,
                                meta_general: meta_general
                            };


                            var things = JSON.stringify(v_entidad);

                            console.log(project);

                            $.ajax({
                                type: 'post',
                                url: project.ActionUrls.Registrar,
                                data: things,
                                async: false,
                                cache: false,
                                dataType: 'json',
                                contentType: 'application/json; charset=utf-8',
                                success: function (data) {
                                    if (data.v_resultado == 1) {
                                        //OK
                                        project.ShowMessage('Alerta', data.v_mensaje);                                        
                                        if (project.ActionUrls.CodigoReglaComision == 0)
                                        {
                                            $('#div_registrar_regla').dialog('close');
                                            project.ReglaPagoComision.Buscar();
                                            project.ReglaPagoComision.ViewRegistroRegla(data.codigo_regla_comision.toString());                                            
                                        }

                                    } else {
                                        //ERROR
                                        project.AlertErrorMessage('Alerta', data.v_mensaje, 'info');

                                    }
                                },
                                error: function () {
                                    project.AlertErrorMessage('Error', 'Error');
                                }
                            });
                        }
                    });
                }
            },
            ViewRegistroDetalleRegla: function (codigo) {
                //if (codigo.length == 0) {
                //    project.AlertErrorMessage('Alerta', 'Necesita seleccionar un registro', 'info');
                //    return;
                //}

                $(this).AbrirVentanaEmergente({
                    parametro: "?p_codigo_detalle_regla_comision=" + codigo + "&tipo=1",
                    div: 'div_registrar_detalle',
                    title: "REGLA DETALLE COMISION - REGISTRO ddd",
                    url: project.ActionUrls._ViewRegistrarDetalleRegla
                });

            },
            GetArticulo: function (tipo) {
                if (tipo == 0) {
                    current.comboArticulo = {};
                    return;
                }
                $.ajax({
                    type: 'post',
                    url: project.ActionUrls.GetArticuloByTipoJson,
                    data: { tipoArticulo: tipo },
                    async: false,
                    cache: false,
                    dataType: 'json',
                    success: function (data) {
                        data.push(elementoTodos);
                        current.comboArticulo = data;
                    },
                    error: function () {
                        current.comboArticulo = {};
                    }

                });
            },



            EvalEliminar: function (valor) {
                $("#btnEliminar").linkbutton((valor == 'Activo' ? 'enable' : 'disable'));
            },

        });

})
    (project);