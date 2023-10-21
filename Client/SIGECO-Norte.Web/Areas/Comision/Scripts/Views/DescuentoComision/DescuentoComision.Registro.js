var RegistroUrls = { };

;(function (app) {
    //===========================================================================================
    var current = app.DescuentoComisionRegistro = {};
    //===========================================================================================

    jQuery.extend(app.DescuentoComisionRegistro,
        {
            EditType: '',
            HasRootNode: 'False',

            Initialize: function (urls) {
                jQuery.extend(RegistroUrls, urls);
                InicializarControles();
            }
        })
})(project);


function InicializarControles()
{
    $('#codigo_empresa').combobox({
        valueField: 'id',
        textField: 'text',
        url: RegistroUrls.GetEmpresaJson
    });

}

function Guardar() {
    var esNuevo = RegistroUrls.codigo_descuento_comision == -1? true:false;
    var codigo_descuento_comision = RegistroUrls.codigo_descuento_comision;
    var codigo_personal = $('#hdCodigoPersonal').val();

    if (!codigo_personal) {
        $.messager.alert("Descuento Comisión", "No ha seleccionado Vendedor.", "warning");
        return false;
    }

    if (!$("#frmRegistro").form('enableValidation').form('validate')) {
        return false;
    }

    var monto = $('#monto').numberbox('getValue');
    if (!monto || parseFloat(monto) <= 0) {
        $.messager.alert("Descuento Comisión", "No ha ingresado el Monto.", "warning");
        return false;
    }
    monto = parseFloat(monto);

    var datosDescuento = {
        codigo_descuento_comision: codigo_descuento_comision,
        codigo_empresa: $('#codigo_empresa').combobox('getValue'),
        codigo_personal: codigo_personal,
        monto: monto,
        motivo: $.trim($("#motivo").val()),
    };

    $.messager.confirm('Confirm', '&iquest;Seguro que desea guardar?', function (result) {
        if (result) {
            $.ajax({
                type: 'post',
                url: RegistroUrls.Guardar,
                data: JSON.stringify({ descuento_comision: datosDescuento }),
                dataType: 'json',
                cache: false,
                async: false,
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    if (data.Msg) {
                        if (data.Msg != 'Success') {
                            $.messager.alert("Registro", data.Msg, "error");
                        }
                        else {
                            $('#dlgRegistro').dialog('close');
                            project.ShowMessage('Alerta', (esNuevo?'Registro':'Modificado') + ' Exitoso');
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

function Cancelar() {
    $('#dlgRegistro').dialog('close');
}

function FormatoFecha(fecha) {
    return fecha.substring(6, 10) + fecha.substring(3, 5) + fecha.substring(0, 2);
}

function BuscarPersonal() {
    $(this).AbrirVentanaEmergente({
        div: 'div_busqueda_personal',
        title: "Buscar Vendedor",
        url: RegistroUrls._Busqueda_Personal
    });
}

