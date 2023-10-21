var ActionComisionUrl = {};

; (function (app) {
    //===========================================================================================
    var current = app.comision = {};
    //===========================================================================================

    jQuery.extend(app.comision,
        {
            Initialize: function (actionUrls) {
                jQuery.extend(ActionComisionUrl, actionUrls);
                fnInicializarComision();
                testValidar();
            }
        })
})(project);


function fnInicializarComision() {

    $('#cmb_empresa, #cmb_tipo_pago').combobox({
        onChange: function () {
            LimpiarSeleccionArticulo();
        }
    });

    $('.content').combobox_sigees({
        id: '#cmb_tipo_pago',
        url: ActionComisionUrl.GetTipoPagoJson
    });

    $('.content').combobox_sigees({
        id: '#cmb_tipo_documento',
        url: ActionComisionUrl._GetTipoDocumentoJson
    });
    

    $('.content').combobox_sigees({
        id: '#cmb_empresa',
        url: ActionComisionUrl._GetEmpresaJson
    });

    if (parseInt(ActionComisionUrl._codigo_comision_manual, 10) > 0)//Para casos de Modificar
    {
        
        $('#hdCodigoPersonal').val(ActionComisionUrl._codigo_personal);
        $('#hdCodigoCanal').val(ActionComisionUrl._codigo_canal);
        $('#hdCodigoArticulo').val(ActionComisionUrl._codigo_articulo);
    }
    else {
        $('#hdCodigoPersonal').val('');
        $('#hdCodigoCanal').val('');
        $('#hdCodigoArticulo').val('');
    }
   
    $('#hdIGV').val(ActionComisionUrl._igv);
    $('#hdCodigoTipoVenta').val(ActionComisionUrl._codigo_tipo_venta);
    $('#hdCodigoTipoPago').val(ActionComisionUrl._codigo_tipo_pago);

    /*$('#txt_numero_contrato').bind('keydown', function (e) {
        if (e.keyCode == 13) {
            fnValidarContratoEmpresa();
        }
    });*/

    $('#txt_numero_documento').bind('keydown', function (e) {
        var key = e.charCode || e.keyCode || 0;
        return (
                key == 8 ||
                key == 9 ||
                key == 13 ||
                key == 46 ||
              //  key == 110 || punto en teclado derecjo
               // key == 190 || punto en teclado izquierdo
                (key >= 35 && key <= 40) ||
                (key >= 48 && key <= 57) ||
                (key >= 96 && key <= 105)
            );
    });

    /*
    var v_re = $('#txt_monto_total_comision').numberbox('textbox');
    console.log(v_re);*/
    /*
    $('#txt_monto_total_comision').numberbox('textbox').bind('keydown', function (e) {
        if (e.keyCode == 13) {
            fnValidarContratoEmpresa();
        }
    });*/

    /*
    var t = $('#txt_numero_contrato');
    t.validatebox('textbox').bind('keydown', function (e) {
        if (e.keyCode == 13) {   // when press ENTER key, accept the inputed value.
            alert();
        }
    });
    */
    //$("#txt_monto_total_comision").next("span").children().first().blur(function () {
    //    alert('algo');
    //});

    //('#txt_monto_total_comision').bind('blur', function () {
    //    alert();
    //});
    // $('#txt_monto_total_comision').numberbox('textbox').focus();
    //$('#txt_monto_total_comision').numberbox('clear').focus()

    //$('#txt_monto_total_comision').numberbox().numberbox('textbox').bind('blur', function (e) {
    //    console.log(this);
    //});

    //$('#txt_monto_total_comision').numberbox().numberbox('textbox').bind('blur', function (e) {
    //   alert();
    //});

    //$("#txt_monto_total_comision").bind("keydown", function () {
    //        alert();

    //});

    //$('#txt_monto_total_comision').numberbox('textbox').bind('blur', function (e) {
    //    alert(this);
    //});


    //var a = $('#txt_monto_total_comision').numberbox();
    //$("#txt_monto_total_comision").numberbox('options').events.blur = function () { alert("事件来了") };

    //$('#txt_monto_total_comision').numberbox({
    //    inputEvents: $.extend({}, $.fn.numberbox.defaults.inputEvents, {
    //        blur: function (e) {
    //            fnCalcularIgvComision();
    //            //var result = $.fn.numberbox.defaults.inputEvents.keypress.call(this, e);
    //            //alert(e.keyCode);
    //        }
    //    })
    //})
    //$(".ajax-loading-tooltip").removeClass("show");
    //$(".overlay-loading").removeClass("show");

    setTimeout(function () {
        $('#txt_monto_total_comision').numberbox('textbox').bind('keydown', function (e) {
            if (e.keyCode == 13) {
                fnCalcularIgvComision();
            }
        });
        $('#txt_monto_total_comision').numberbox('textbox').bind('blur', function (e) {
            fnCalcularIgvComision();
        });
    }, 1000);

}


function fnCalcularIgvComision() {
    /*
    $('#txt_monto_total_comision').numberbox('textbox').bind('keydown', function (e) {
        if (e.keyCode == 13) {
            alert();
        }
    });*/

    $("#txt_monto_total_comision").validatebox({ required: true, novalidate: false });
    $('#txt_monto_total_comision').numberbox('enableValidation').numberbox('validate');
    if (!$('#txt_monto_total_comision').numberbox("isValid")) {

        //$.messager.alert('Campo obligatorio', 'Ingrese el total de la comisión', 'error');
        $('#txt_monto_total_comision').numberbox('textbox').focus();
        return;
    }



    var v_monto_total_comision = parseFloat($("#txt_monto_total_comision").numberbox("getValue"));

    var v_monto_igv = parseFloat(v_monto_total_comision * (ActionComisionUrl._igv_calculo / 100));

    var v_monto_comision = v_monto_total_comision - v_monto_igv;

    $("#txt_monto_igv").numberbox("setValue", v_monto_igv);
    $("#txt_monto_comision").numberbox("setValue", v_monto_comision);

    /*
    var v_monto_total_comision = $("#txt_monto_comision").numberbox("getValue");
    var v_monto_total_comision = $("#txt_monto_igv").numberbox("getValue");*/


}

function fnVentanaBuscarArticulo() {

    fnValidarContratoEmpresa();
}

function fnVentanaBuscarPersonal() {
    $(this).AbrirVentanaEmergente({
        div: 'div_busqueda_personal',
        title: "Buscar Vendedor",
        url: ActionComisionUrl._Busqueda_Personal
    });
}
function fnValidarContratoEmpresa() {

    var _v_codigo_empresa = $("#cmb_empresa").combobox("getValue");
    var _v_numero_contrato = $.trim($("#txt_numero_contrato").val());
    var codigo_personal = $("#hdCodigoPersonal").val();

    if (!codigo_personal) {
        $.messager.alert('Campo obligatorio', "Seleccione Vendedor para continuar con la selecci&oacute;n de art&iacute;culo.", 'warning');
        return;
    }

    if (!_v_codigo_empresa)
    {
        $.messager.alert('Campo obligatorio', "Seleccione Empresa para continuar con la selecci&oacute;n de art&iacute;culo.", 'warning');
        return;
    }

    $('#hdNroContrato').val(_v_numero_contrato);
    $('#hdCodigoEmpresa').val(_v_codigo_empresa);

    $(this).AbrirVentanaEmergente({
        div: 'div_busqueda_articulo',
        title: "Buscar Art&iacute;culo",
        url: ActionComisionUrl._Busqueda_Articulo
    });

    /*$.ajax({
        type: 'post',
        url: ActionComisionUrl._ValidarExisteContratoJson,
        data: { empresa: _v_codigo_empresa, contrato: _v_numero_contrato },
        async: false,
        cache: false,
        dataType: 'json',
        success: function (data) {
            if (data.Msg) {
                if (data.Msg == 'Success') {


                }
                else {
                    //project.AlertErrorMessage('Contrato no existe', data.Msg);      //ico error
                    $.messager.alert('Registro Manual de Comisión', data.Msg, 'warning');
                }
            } else {

            }
        },
        error: function () {
            project.AlertErrorMessage('Error', 'Error');
        }
    });*/
}

function RegistrarComision() {

    var monto_total_comision = parseFloat($("#txt_monto_total_comision").numberbox("getValue"));

    $("#cmb_tipo_documento").combobox('enableValidation').combobox('validate');
    if (!$("#cmb_tipo_documento").combobox('isValid'))
        return;

    $("#txt_numero_documento").validatebox('enableValidation').validatebox('validate');
    if (!$("#txt_numero_documento").validatebox('isValid'))
        return;

    $("#txt_nombres_fallecido").validatebox('enableValidation').validatebox('validate');
    if (!$("#txt_nombres_fallecido").validatebox('isValid'))
        return;

    $("#txt_apellido_paterno_fallecido").validatebox('enableValidation').validatebox('validate');
    if (!$("#txt_apellido_paterno_fallecido").validatebox('isValid'))
        return;

    $("#txt_apellido_materno_fallecido").validatebox('enableValidation').validatebox('validate');
    if (!$("#txt_apellido_materno_fallecido").validatebox('isValid'))
        return;

    var codigo_personal = $('#hdCodigoPersonal').val();
    var codigo_canal = $('#hdCodigoCanal').val();
    var codigo_articulo = $('#hdCodigoArticulo').val();
    var tipo_venta = $('#hdCodigoTipoVenta').val();
    var tipo_pago = $('#hdCodigoTipoPago').val();

    if (!codigo_personal) {
        //fnVentanaBuscarPersonal();
        $.messager.alert('Campo obligatorio', "Seleccione Vendedor, para continuar con el proceso de registro.", 'warning');
        return;
    }

    $("#cmb_empresa").combobox('enableValidation').combobox('validate');
    if (!$("#cmb_empresa").combobox('isValid'))
        return;

    $("#cmb_tipo_pago").combobox('enableValidation').combobox('validate');
    if (!$("#cmb_tipo_pago").combobox('isValid'))
        return;

    var nro_contrato = $("#txt_numero_contrato").val();
    if (nro_contrato) {
        if (nro_contrato.length <= 9) {
            $.messager.alert('Campo incorrecto', "El nro. de contrato debe tener 10 dígitos como mínimo.", 'warning');
            return;
        }
    }

    if (!codigo_articulo) {
        $.messager.alert('Campo obligatorio', "Seleccione Artículo, para continuar con el proceso de registro.", 'warning');
        return;
    }

    if (!(monto_total_comision > 0)) {
        $.messager.alert('Campo obligatorio', "El monto comisión del vendedor  deber ser mayor que cero, para continuar con el proceso de registro.", 'warning');
        return;
    }

    if (!$("#frm_registrar_comision").form('enableValidation').form('validate'))
        return;

    var codigo_comision_manual = ActionComisionUrl._codigo_comision_manual;
    //codigo_comision_manual = codigo_comision_manual.length == 0 ? '0' : codigo_comision_manual;

    var pEntidad = {
        codigo_comision_manual: codigo_comision_manual,
        apellido_paterno_fallecido: $("#txt_apellido_paterno_fallecido").val(),
        apellido_materno_fallecido: $("#txt_apellido_materno_fallecido").val(),
        nombre_fallecido: $("#txt_nombres_fallecido").val(),
        nro_documento:$("#txt_numero_documento").val(),
        codigo_tipo_documento: $("#cmb_tipo_documento").combobox("getValue"),
        codigo_personal: codigo_personal,
        codigo_canal: codigo_canal,
        codigo_empresa: $("#cmb_empresa").combobox("getValue"),
        nro_contrato: nro_contrato,
        codigo_articulo: codigo_articulo,
        comentario: $("#txt_comentario").val(),
        codigo_tipo_venta: tipo_venta,
        codigo_tipo_pago: $("#cmb_tipo_pago").combobox("getValue"),
        monto_bruto: $("#txt_monto_comision").numberbox("getValue"),
        monto_igv:$("#txt_monto_igv").numberbox("getValue"),
        monto_neto: $("#txt_monto_total_comision").numberbox("getValue"),
        nro_factura_vendedor: $("#txt_nro_factura_vendedor").val(),
    };

    $.messager.confirm('Confirmaci&oacute;n', '¿Est&aacute; seguro que desea guardar esta comisión?', function (result) {
        if (result) {
            $.ajax({
                type: 'post',
                url: ActionComisionUrl._Registrar_Comision,
                data: pEntidad,
                async: false,
                cache: false,
                dataType: 'json',
                success: function (data) {
                    if (data.v_resultado == 1) {
                        $.messager.alert('Operación exitosa', data.v_mensaje, 'info', function () {
                            $('#hdCodigo').val('');
                            $('#dgv_comision').datagrid('clearSelections');
                            fnReloadGrillaComision();
                            $("#div_registrar_comision").dialog("close");                          
                        });
                    }
                    else {
                        $.messager.alert('Error', data.v_mensaje, 'error');
                    }
                },
                error: function () {
                    $.messager.alert('Error', "Error en el servidor", 'error');
                }
            });
        }
    });
}



function cerrar_comision() {
    $("#div_registrar_comision").dialog("close");

}

function fnObtenerComisionArticuloPersonal() {
    
    $('#txt_numero_contrato').validatebox('enableValidation').validatebox('validate');
    $('#cmb_empresa').combobox('enableValidation').combobox('validate');

    if (!(ActionComisionUrl._codigo_personal > 0)) {
        //$.messager.alert('Campo obligatorio', "Seleccione Personal, para obtener comisión del persoonal.", 'warning');
        return;
    }

    if (!$('#cmb_empresa').combobox("isValid")) {
        //$.messager.alert('Campo obligatorio', "Seleccione empresa, para obtener comisión del personal por artículo", 'warning');
        return;
    }

    if (!$('#txt_numero_contrato').validatebox("isValid")) {
        //$.messager.alert('Campo obligatorio', "Ingrese nro. contrato, para obtener comisión del personal.", 'warning');
        return;
    }

    if (!(ActionComisionUrl._codigo_articulo > 0)) {
       // $.messager.alert('Campo obligatorio', "Seleccione Artículo, para obtener comisión del personal", 'warning');
        return;
    }
    
    var _v_codigo_empresa = $("#cmb_empresa").combobox("getValue");
    var _v_numero_contrato = $.trim($("#txt_numero_contrato").val());

    $.ajax({
        type: 'post',
        url: ActionComisionUrl._GetComisionPersonalArticulo,
        data: {
            codigo_empresa: _v_codigo_empresa,
            nro_contrato: _v_numero_contrato,
            codigo_articulo: ActionComisionUrl._codigo_articulo,
            codigo_personal: ActionComisionUrl._codigo_personal
        },
        async: false,
        cache: false,
        dataType: 'json',
        success: function (data) {
            $("#lbl_mensaje_comision").text(data.mensaje_operacion);
            ActionComisionUrl._codigo_cronograma = data.codigo_cronograma;
            if (data.codigo_tipo_operacion == 1)
            {
                
                $("#txt_monto_comision").numberbox("setValue", data.monto_bruto);
                $("#txt_monto_igv").numberbox("setValue", data.monto_igv);
                $("#txt_monto_total_comision").numberbox("setValue", data.monto_neto);
            }
            else {
                
                $("#txt_monto_comision").numberbox("setValue", 0);
                $("#txt_monto_igv").numberbox("setValue", 0);
                $("#txt_monto_total_comision").numberbox("setValue", 0);
               // $.messager.alert('Registro Manual de Comisión', data.mensaje_operacion, 'error');
            }
        },
        error: function () {
            $.messager.alert('Registro Manual de Comisión', "Error en el servidor", 'error');
        }
    });
}

function fnSetearComision(monto_neto)
{
    var monto_bruto = 0;
    var monto_igv = 0;
    var igv = parseFloat($('#hdIGV').val());

    monto_neto = parseFloat(monto_neto);
    monto_igv = (monto_neto * igv) / (100 + igv);
    monto_bruto = monto_neto - monto_igv;

    $("#txt_monto_comision").numberbox("setValue",monto_bruto);
    $("#txt_monto_igv").numberbox("setValue", monto_igv);
    $("#txt_monto_total_comision").numberbox("setValue", monto_neto);
}

function LimpiarSeleccionArticulo() {
    $('#hdCodigoArticulo').val('');
    $("#txt_nombre_articulo").textbox("setValue", '');
    $("#txt_monto_igv, #txt_monto_comision, #txt_monto_total_comision").numberbox("setValue", 0);
}

function testValidar()
{
    if (parseInt(ActionComisionUrl._codigo_comision_manual, 10) < 1) {
        return false;
    }
    var mensaje = '';
    $.ajax({
        type: 'post',
        url: ActionComisionUrl.ValidarReferencia,
        data: { codigo_comision_manual: ActionComisionUrl._codigo_comision_manual },
        async: false,
        cache: false,
        dataType: 'json',
        success: function (data) {
            if (data.mensaje.length > 0)
            {
                $.messager.alert('Modificar', "No podrá modificar: " + data.mensaje, 'error');
                $("#btnGuardar").attr("disabled", true);
                //$("#btnGuardar").hide();
            }
        },
        error: function () {
            $.messager.alert('Registro Manual de Comisión', "Error en el servidor", 'error');
        }
    });
}