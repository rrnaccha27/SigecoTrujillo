var ActionPlanillaUrl = {};
var JsonTipoVenta = {};


; (function (app) {
    //===========================================================================================
    var current = app.planilla = {};
    //===========================================================================================

    jQuery.extend(app.planilla,
        {
            Initialize: function (actionUrls) {
                jQuery.extend(ActionPlanillaUrl, actionUrls);
                fnInicializarPlanilla();

                fnConfigurarGrillaPagoHabilitado();
                fnConfigurarGrillaPagoExclusiones();
                fnConfigurarGrillaPagoDescuento();
                
            }
        })
})(project);


function fnInicializarPlanilla() {


    $('#cmbg_personal_planilla').combogrid({
        panelWidth: 500,
        idField: 'codigo_personal',
        textField: 'nombre',
        //mode: 'remote',
        fitColumns: true,
        rownumbers: true,
        url: ActionPlanillaUrl._GetPersonalPlanillaAllJson,
        queryParams: {
            codigo_planilla: ActionPlanillaUrl._codigo_planilla
        },
        columns: [[
            { field: 'codigo_persona', title: 'Codigo', width: 60, hidden: true },
            { field: 'nro_documento', title: 'Nro. Documento', width: 60 },
            { field: 'apellidos_nombres_vendedor', title: 'Vendedor', align: 'left', width: 120 }
        ]]
    });
    

     JsonTipoVenta = $('.content').get_json_combobox({
        url: ActionPlanillaUrl._GetFilterTipoVentaJson
    });    

    $('.content').combobox_sigees({
        id: '#cmb_tipo_planilla',
        url: ActionPlanillaUrl._GetTipoPlanillaJson
    });
    /*
    $('.content').combobox_sigees({
        id: '#cmb_canal_venta',
        url: ActionPlanillaUrl._GetCanalJson
    });
    */
    $('.content').combobox_sigees({
        id: "#cmb_regla_tipo_planilla",
        url: ActionPlanillaUrl._GetReglaTipoPlanillaJson
    });

    /*
    $('#cmb_empresa_planilla').combobox({
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
    */
    /*
    $('#cmb_canal_venta').combobox({
        validType: "restricted[\'#cmb_canal_venta\']"
    });
  */
    //$('#cmb_canal_venta').filter();
    /*
    $('#cmb_canal_venta').combobox({
        filter: function (q, row) {
            var opts = $(this).combobox('options');
            return row[opts.textField].toLowerCase().indexOf(q.toLowerCase()) == 0;

            //return row[opts.textField].indexOf(q) == 0;
        }
    });*/

    $('#dtp_fecha_inicio').DeshabilitarFechaMayorHoy();
    $('#dtp_fecha_fin').DeshabilitarFechaMayorHoy();

    $('#dtp_fecha_inicio').formatteDate();
    $('#dtp_fecha_fin').formatteDate();



    $('.content').ResizeModal({
        widthMax: '90%',
        widthMin: '80%',
        div: 'div_registrar_planilla'
    });
    
    $(window).resize(function () {

      
        $('#dgv_planilla_pago_habilitado').datagrid('resize', {
            height: '100%'
        });
        $('#dgv_planilla_pago_exclusiones').datagrid('resize', {
            height: '100%'
        });
        
        var v_altura_ventana = $(window).height();
        if (v_altura_ventana < 800) {
            $("#div_planilla_pago_exclusiones").height(300);
            $("#div_planilla_pago_habilitado").height(300);

        }
        else {
            $("#div_planilla_pago_exclusiones").height(500);
            $("#div_planilla_pago_habilitado").height(500);
        }
    });
}

function fnReloadGrillaPagoDescuento() {
    $('#dgv_planilla_descuento').datagrid("reload");

}


function fnConfigurarGrillaPagoDescuento() {

    $('#dgv_planilla_descuento').datagrid({
       // fitColumns: true,
        idField: 'codigo_descuento',
        height: '300',
        url: ActionPlanillaUrl._GetPagosDescuentoJson,
        queryParams: {
            p_codigo_planilla: ActionPlanillaUrl._codigo_planilla
        },
        toolbar: "#toolbar_pago_descuento",
        pagination: true,
        singleSelect: true,
        rownumbers: true,
        pageList: [10, 20, 50, 100, 200, 400, 500],
        pageSize: 10,
        columns:
        [[
            { field: 'codigo_descuento', title: 'Codigo', hidden: 'true' },
            { field: 'indica_estado_registro', title: 'Codigo', hidden: 'true' },            
            { field: 'nombre_empresa', title: 'Empresa', width: 120, align: 'left', halign: 'center' },
            { field: 'nombre_grupo', title: 'Grupo', width: 120, align: 'left', halign: 'center' },
            { field: 'apellidos_nombres', title: 'Vendedor', width: 250, align: 'left', halign: 'center' },
            {
                field: 'monto', title: 'Importe <br> Comisión', width: 120, align: 'right', halign: 'center', formatter: function (value, row) {
                    return value.toFixed(2);
                }
            },
            { field: 'motivo', title: 'Motivo', width: 350, align: 'left', halign: 'center' },
            { field: 'nombre_estado_registro', title: 'Estado', width: 150, align: 'center', halign: 'center' }
        ]]
    });

    $('#dgv_planilla_descuento').datagrid('enableFilter');

    $(window).resize(function () {
        $('#dgv_planilla_descuento').datagrid('resize');
    });
}

function fnConfigurarGrillaPagoExclusiones() {

    $('#dgv_planilla_pago_exclusiones').datagrid({
        //fitColumns: true,
        idField: 'codigo_detalle_cronograma',
        url: ActionPlanillaUrl._GetPagosExcluidoJson,
        queryParams: {
            codigo_planilla: ActionPlanillaUrl._codigo_planilla,
            codigo_tipo_busqueda: 2
        },
        height: '300',
        pagination: true,
        singleSelect: true,
        rownumbers: true,
        autoRowHeight: false,
        pageList: [10, 20, 50, 100, 200, 400, 500],
        pageSize: 10,
        columns:
        [[
            { field: 'codigo_detalle_cronograma', title: 'Codigo', hidden: 'true' },
            
        { field: 'nombre_estado_exclusion', title: 'Estado <br> Exclusión', width: 120, align: 'left' },
            { field: 'nombre_grupo_canal', title: 'Grupo', width: 120, align: 'left' },
            { field: 'apellidos_nombres', title: 'Vendedor', width: 180, align: 'left' },
            { field: 'nombre_empresa', title: 'Empresa', width: 100, align: 'left' },
            { field: 'nro_contrato', title: 'Nro. <br>Contrato', width: 100, align: 'center' },
            { field: 'nombre_articulo', title: 'Articulo', width: 180, align: 'left' },
            { field: 'nombre_tipo_venta', title: 'Tipo Venta', width: 180, align: 'left' },
            { field: 'nombre_tipo_pago', title: 'Tipo Pago', width: 180, align: 'left' },
            { field: 'str_fecha_pago', title: 'Fecha <br> Habilitada', width: 120, align: 'center' },
            { field: 'nro_cuota', title: 'Nro.<br> Cuota', width: 80, align: 'left' },
            {
                field: 'monto_bruto', title: 'Importe <br> Comisión', width: 120, halign: "center", align: "right", formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                }
            },
            {
                field: 'igv', title: 'IGV', width: 100, halign: "center", align: "right", formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                }
            },
            {
                field: 'monto_neto', title: 'Comisión<br> Total', width: 100, halign: "center", align: "right", formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                }
            },
            { field: 'usuario_exclusion', title: 'Usuario Exclusión', width: 150, align: 'left' },
            { field: 'motivo_exclusion', title: 'Motivo Exclusión', width: 150, align: 'left' },
            { field: 'usuario_habilita_exclusion', title: 'Usuario Habilitación', width: 150, align: 'left' },
            { field: 'motivo_habilitacion_exclusion', title: 'Motivo Habilitación', width: 150, align: 'left' },
            { field: 'observacion', title: 'Observación', width: 250, align: 'left', hidden: 'true' }
                                    
                        
        ]],
        //onResize: function (width, height) {

        //    var v_altura_ventana = $(window).height();
        //    console.log(v_altura_ventana);
        //    if (v_altura_ventana < 600) {
        //        $('#dgv_planilla_pago_exclusiones').datagrid('resize', {
        //            height: 300
        //        });
        //    }
        //    else {
        //        $('#dgv_planilla_pago_exclusiones').datagrid('resize', {
        //            height: 'auto'
        //        });
        //    }


        //},
        onClickRow: function (index, row) {

        }
    });
    $('#dgv_planilla_pago_exclusiones').datagrid('enableFilter');



}

function fnConfigurarGrillaPagoHabilitado() {

    $('#dgv_planilla_pago_habilitado').datagrid({
        //fitColumns: true,
        height: '300',
        idField: 'codigo_detalle_cronograma',
        data: null,
        rownumbers: true,        
        toolbar: '#toolbar_pago_habilitado',
        url: ActionPlanillaUrl._GetPagoHabilitadoJson,
        queryParams: {
            codigo_planilla: ActionPlanillaUrl._codigo_planilla,
            codigo_tipo_busqueda: 1
        },
        pagination: true,
        singleSelect: true,
        
        pageList: [10, 20, 50, 100, 200, 400, 500],
        pageSize: 10,
        columns:
        [[
            { field: 'codigo_detalle_planilla', title: 'Codigo', hidden: 'false' },
            { field: 'indica_registro_manual_comision', title: 'Es Registro Manual de Comision', hidden: 'false' },
            
            { field: 'codigo_empresa', title: 'Codigo Empresa', hidden: 'false' },
            { field: 'codigo_cronograma', title: 'Codigo', hidden: 'false' },
            { field: 'codigo_estado_cuota', hidden: 'false' },
            
            { field: 'nombre_grupo_canal', title: 'Grupo', width: 120, align: 'left' },
            { field: 'apellidos_nombres', title: 'Vendedor', width: 180, align: 'left' },
            { field: 'nombre_empresa', title: 'Empresa', width: 120, halign: "center", align: "left" },
            { field: 'nro_contrato', title: 'Nro. <br>Contrato', width: 100, align: 'center' },
            { field: 'nombre_articulo', title: 'Articulo', width: 180, align: 'left' },
            { field: 'nombre_tipo_venta', title: 'Tipo Venta', width: 180, align: 'left' },
            { field: 'codigo_tipo_venta', title: ' codigo_tipo_venta Tipo Venta', width: 180, align: 'left', hidden: 'false' },
            { field: 'nombre_tipo_pago', title: 'Tipo Pago', width: 180, align: 'left' },
            { field: 'str_fecha_pago', title: 'Fecha <br> Habilitada', width: 120, align: 'center' },
            { field: 'nro_cuota', title: 'Nro.<br> Cuota', width: 80, align: 'center' },

            {
                field: 'monto_bruto', title: 'Importe <br> Comisión', halign: "center", align: "right", width: 120, formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                }
            },
            {
                field: 'igv', title: 'IGV', width: 120, halign: "center", align: "right", formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                }
            },
            {
                field: 'monto_neto', title: 'Comisión<br> Total', width: 120, halign: "center", align: "right", formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                }
            },
            { field: 'nombre_estado_cuota', title: 'Estado<br> Cuota', width: 100, align: 'left' },
            { field: 'nombre_estado_registro', title: 'Estado<br>Registro', width: 100, align: 'center', halign: 'center' }
        ]],
        onClickRow: function (index, row) {

        }
    });
    
    $('#dgv_planilla_pago_habilitado').datagrid('enableFilter', [{
        field: 'nombre_tipo_venta',
        type: 'combobox',
        options: {
            panelHeight: 'auto',
            data: JsonTipoVenta,
            onChange: function (value) {                
                if (value == '') {
                    $('#dgv_planilla_pago_habilitado').datagrid('removeFilterRule', 'codigo_tipo_venta');
                } else {
                    $('#dgv_planilla_pago_habilitado').datagrid('addFilterRule', {
                        field: 'codigo_tipo_venta',
                        op: 'equal',
                        value: value
                    });
                }
                $('#dgv_planilla_pago_habilitado').datagrid('doFilter');
            }
        }
    }]);


    $(window).resize(function () {
        $('#dgv_planilla_pago_habilitado').datagrid('resize');
    });
}


function RegistrarPlanilla() {
        
    
    if (!$("#fmr_registrar_planilla").form('enableValidation').form('validate'))
        return;

    var pEntidad = {
        codigo_regla_tipo_planilla: $.trim($("#cmb_regla_tipo_planilla").combobox('getValue')),
        fecha_inicio: $.trim($("#dtp_fecha_inicio").datebox('getValue')),
        fecha_fin: $.trim($("#dtp_fecha_fin").datebox('getValue'))
    };




    $.messager.confirm('Confirmaci&oacute;n', '¿Est&aacute; seguro que desea generar la planilla?', function (result) {
        if (result) {
            $.ajax({
                type: 'post',
                url: ActionPlanillaUrl._Registrar,
                data: JSON.stringify({v_planilla:pEntidad}),
                async: true,
                cache: false,
                dataType: 'json',
                contentType: 'application/json',
                success: function (data) {
                    console.log(data);
                    if (data.v_resultado == 1) {
                        $.messager.alert('Operación exitosa', data.v_mensaje, 'info', function () {
                            fnConsultarPlanilla();
                            $("#div_registrar_planilla").dialog("close");
                            fnModificarPlanillaById(data.codigo_planilla);
                        });
                    }
                    else {
                        $.messager.alert('Error en la generación de planilla', data.v_mensaje, 'error');
                    }
                },
                error: function (data) {
                    
                    $.messager.alert('Error', "Error en el servidor", 'error');
                }
            });
        }
    });
}

function CerrarPlanilla(p_codigo_planilla) {


    var pEntidad = {
        codigo_planilla: p_codigo_planilla
    };

    console.log(pEntidad);
    $.messager.confirm('Confirmaci&oacute;n', '¿Est&aacute; seguro que desea cerrar la planilla?', function (result) {
        if (result) {
            $.ajax({
                type: 'post',
                url: ActionPlanillaUrl._Cerrar,
                data: pEntidad,
                async: true,
                cache: false,
                dataType: 'json',
                success: function (data) {
                    if (data.v_resultado == 1) {

                        $.messager.alert('Operación exitosa', data.v_mensaje, 'info', function () {
                            fnConsultarPlanilla();
                            $("#div_registrar_planilla").dialog("close");
                            fnModificarPlanillaById(p_codigo_planilla);
                        });
                    }
                    else {
                        $.messager.alert('Error en la generación de planilla', data.v_mensaje, 'error');
                    }
                },
                error: function () {
                    $.messager.alert('Error', "Error en el server", 'error');
                }
            });
        }
    });
}

function cerrar_planilla() {
    $("#div_registrar_planilla").dialog("close");

}

function fnNuevoAnalisisContrato(p_codigo_planilla) {

    var row = $('#dgv_planilla_pago_habilitado').datagrid('getSelected');
    
    if (!row) {
        $.messager.alert('Seleccione registro', "Para continuar con el proceso, seleccionar un registro.", 'warning');
        return;
    }  

    $(this).AbrirVentanaEmergente({
        parametro: "?codigo_empresa=" + row.codigo_empresa + "&nro_contrato=" + row.nro_contrato,
        div: 'div_registrar_contrato',
        title: "Análisis de Comisión",
        url: ActionPlanillaUrl._Analisis_Contrato
    });




}
function excluir_planilla(p_codigo_planilla) {
    var row = $('#dgv_planilla_pago_habilitado').datagrid('getSelected');
    console.log(row);
    if (!row) {
        $.messager.alert('Seleccione registro', "Para continuar con el proceso debe seleccionar un registro.", 'warning');
        return;
    }

    if (row.indica_registro_manual_comision == "1") {
        $.messager.alert('Exclusion de Pago', "No se puede excluir un pago de tipo registro manual de comisión.", 'warning');
        return;
    }

    if (row.codigo_estado_cuota != "2") {
        $.messager.alert('Exclusion de Pago', "Solo se puede excluir cuotas en proceso de pago.", 'warning');
        return;
    }

    $.messager.observacion("¿Est&aacute; seguro que desea excluir el pago seleccionado?", 'Motivo por el que se excluye el pago', function (win, data) {
        if (data) {

            var pEntidad = {
                codigo_planilla: row.codigo_planilla,
                codigo_detalle_planilla: row.codigo_detalle_planilla,
                observacion: $.trim(data)
            };
            $.ajax({
                type: 'post',
                url: ActionPlanillaUrl._Excluir,
                data: pEntidad,
                async: true,
                cache: false,
                dataType: 'json',
                success: function (data) {
                    if (data.v_resultado == 1) {
                        $.messager.alert('Operación exitosa', data.v_mensaje, 'info', function () {
                            fnFiltrarPlanilla();
                            $('#dgv_planilla_pago_exclusiones').datagrid('reload');
                            //$("#div_registrar_planilla").dialog("close");
                            //fnModificarPlanillaById(p_codigo_planilla);
                        });
                    }
                    else {
                        $.messager.alert('Error en la exclusión de pago', data.v_mensaje, 'error');
                    }
                },
                error: function () {
                    $.messager.alert('Error', "Error en el server", 'error');
                }
            });


        }
    });

}


function fnNuevoDescuento(p_codigo_planilla) {

    $(this).AbrirVentanaEmergente({
        parametro: "?p_codigo_planilla=" + p_codigo_planilla,
        div: 'div_registrar_descuento',
        title: "Registro Descuento",
        url: ActionPlanillaUrl._Descuento
    });
}


function fnDesactivarDescuento() {


    var rows = $("#dgv_planilla_descuento").datagrid("getSelections");
    if (rows.length <= 0) {
        $.messager.alert("Desactivar descuento planilla", "Seleccione un registro para desactivar.", "warning");
        return;
    }
    var row = $("#dgv_planilla_descuento").datagrid("getSelected");
    if (row.indica_estado_registro!=1)
    {
        $.messager.alert("Desactivar descuento planilla", "El registro seleccionado se encuentra desactivado.", "warning");
        return;
    }
    

    $.messager.confirm('Confirmaci&oacute;n', '¿Est&aacute; seguro que desea desactivar el descuento?', function (result) {
        if (result) {
            $.ajax({
                type: 'post',
                url: ActionPlanillaUrl._Descuento_Desactivar,
                data: { p_codigo_descuento: row.codigo_descuento },
                async: true,
                cache: false,
                dataType: 'json',
                success: function (data) {
                    if (data.v_resultado == 1) {
                        $.messager.alert('Operación exitosa', data.v_mensaje, 'info', function () {
                            fnReloadGrillaPagoDescuento();
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


function fnFiltrarPlanilla() {

    var item = {
        codigo_planilla: ActionPlanillaUrl._codigo_planilla,
        codigo_tipo_venta:null,// $("#cmbg_tipo_venta_planilla").combobox("getValue"),
        codigo_personal: $("#cmbg_personal_planilla").combogrid("getValue")
        //nro_documento: $.trim($("#txt_numero_documento").textbox('getText')),
        //apellido_paterno: $.trim($("#txt_apellido_paterno").textbox('getText')),
        //apellido_materno: $.trim($("#txt_apellido_materno").textbox('getText')),
        //nombre: $.trim($("#txt_nombre_persona").textbox('getText'))
    };
    $('#dgv_planilla_pago_habilitado').datagrid('reload', item);

}

function fnLimpiarFiltrarPlanilla() {

    $("#cmbg_tipo_venta_planilla").combobox("setValue", null);
    $("#cmbg_personal_planilla").combogrid("setValue", null);
    var item = {
        codigo_planilla: ActionPlanillaUrl._codigo_planilla
    };
    $('#dgv_planilla_pago_habilitado').datagrid('reload', item);

}

function fnReportePlanilla(p_codigo_planilla) {

    $(this).AbrirVentanaEmergente({
        parametro: "?p_codigo_planilla=" + p_codigo_planilla,
        div: 'div_reporte_general',
        title: "Reporte Planilla",
        url: ActionPlanillaUrl._Reporte
    });
}

function fnReporteLiquidacion(p_codigo_planilla) {

    $(this).AbrirVentanaEmergente({
        parametro: "?p_codigo_planilla=" + p_codigo_planilla,
        div: 'div_reporte_general',
        title: "Reporte Liquidación",
        url: ActionPlanillaUrl._Reporte_Liquidacion
    });
}




function AnularPlanilla(p_codigo_planilla) {
    $.messager.confirm('Confirmaci&oacute;n', '¿Est&aacute; seguro que desea anular la planilla?', function (result) {
        if (result) {
            $.ajax({
                type: 'post',
                url: ActionPlanillaUrl._Anular,
                data: { p_codigo_planilla: p_codigo_planilla },
                async: true,
                cache: false,
                dataType: 'json',
                success: function (data) {
                    if (data.v_resultado == 1) {

                        $.messager.alert('Operación exitosa', data.v_mensaje, 'info', function () {
                            fnConsultarPlanilla();
                            $("#div_registrar_planilla").dialog("close");
                            fnModificarPlanillaById(p_codigo_planilla);
                        });
                    }
                    else {
                        $.messager.alert('Error en anular planilla', data.v_mensaje, 'error');
                    }
                },
                error: function () {
                    $.messager.alert('Error', "Error en el servidor", 'error');
                }
            });
        }
    });
}


function fnEnviarLiquidacion(p_codigo_planilla)
{
    $.messager.confirm('Confirmaci&oacute;n', '¿Est&aacute; seguro que desea enviar liquidación a vendedores?', function (result) {
        if (result) {
            $.ajax({
                type: 'post',
                url: ActionPlanillaUrl._Enviar_Correo_Liquidacion,
                data: { p_codigo_planilla: p_codigo_planilla, p_nro_pllanilla: ActionPlanillaUrl._numero_planilla },
                async: true,
                cache: false,
                dataType: 'json',
                success: function (data) {
                    if (data.v_resultado == 1)
                    {
                        $.messager.alert('Operación exitosa', data.v_mensaje, 'info');
                    }
                    else {
                        $.messager.alert('Error en enviar correo', data.v_mensaje, 'error');
                    }
                },
                error: function () {
                    $.messager.alert('Error', "Error en el servidor", 'error');
                }
            });
        }
    });
}



function fnGenerarTxtPlanillaCerrado(p_codigo_planilla) {  
    
    
        $.messager.confirm('Confirmaci&oacute;n', '¿Est&aacute; seguro que desea generar el archivo txt?', function (result) {
            if (result) {
                var url = ActionPlanillaUrl.GenerarTxt + "?id=" + p_codigo_planilla;
                window.open(url, '_blank');
            }
        });

    
        
    
}
