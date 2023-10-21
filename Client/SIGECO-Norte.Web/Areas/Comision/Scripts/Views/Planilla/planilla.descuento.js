var ActionDescuentoUrl = {};

; (function (app) {
    //===========================================================================================
    var current = app.descuento = {};
    //===========================================================================================

    jQuery.extend(app.descuento,
        {
            Initialize: function (actionUrls) {
                jQuery.extend(ActionDescuentoUrl, actionUrls);

                fnInicializarDescuento();
            }
        })
})(project);


function fnInicializarDescuento() {
    fnConfigurarGrillaSaldoPlanilla();
    $('.content').ResizeModal({
        widthMax: '90%',
        widthMin: '80%',
        div: 'div_registrar_descuento'
    });

}

function cerrar_descuento() {
    $("#div_registrar_descuento").dialog("close");

}

function fnRegistrarDescuento() {


    if (!$.existeRegistroEnEdicion('dgv_planilla_saldo'))
    {
        var v_lista_descuento_planilla = [];
        var rows = $("#dgv_planilla_saldo").datagrid("getChanges");
        var saldo_insuficiente = false;
        $.each(rows, function (index, objecto) {
            
            if (!objecto.tiene_descuento && parseFloat(objecto.monto_descuento)>0)
            {
                if (parseFloat(objecto.monto_descuento) > parseFloat(objecto.monto_afecto_descuento))
                {
                    saldo_insuficiente = true;
                    return;
                }

                var v_descuento = {                   
                    codigo_planilla: ActionDescuentoUrl._codigo_planilla,
                    codigo_personal: ActionDescuentoUrl._codigo_personal,
                    codigo_empresa:objecto.codigo_empresa,
                    monto: parseFloat(objecto.monto_descuento),
                    motivo: objecto.motivo
                    ///motivo: $("#txt_motivo_descuento").textbox('getText')
                };
                v_lista_descuento_planilla.push(v_descuento);
            }
        });

        if (saldo_insuficiente) {
            $.messager.alert('Saldo insuficiente', "Verifique que el descuento sea menor o igual que comisión afecto a descuento.", 'warning');
            return;
        }

        if (v_lista_descuento_planilla.length==0)
        {
            $.messager.alert('Información del registro', "No existe descuento ingresado en la grilla.", 'warning');
            return;
        }
            /***********************************************************/
            $.messager.confirm('Confirmaci&oacute;n', '¿Est&aacute; seguro que desea registrar el descuento?', function (result) {
                if (result) {
                    $.ajax({
                        type: 'post',
                        url: ActionDescuentoUrl._Registrar_Descuento,
                        data: JSON.stringify(v_lista_descuento_planilla),
                        async: false,
                        cache: false,
                        dataType: 'json',
                        contentType: 'application/json; charset=utf-8',
                        success: function (data) {
                            if (data.v_resultado == 1) {

                                $.messager.alert('Operación exitosa', data.v_mensaje, 'info', function () {
                                    fnReloadGrillaPagoDescuento();
                                    $("#div_registrar_descuento").dialog("close");
                                });
                            }
                            else {
                                $.messager.alert('Error en registrar descuento', data.v_mensaje, 'error');
                            }
                        },
                        error: function (error) {
                            console.log(error);
                            $.messager.alert('Error', error, 'error');
                        }
                    });
                }
            });
            /***********************************************************/
        
    }
    
    
    


}

function fnBuscarPersonal() {

    $(this).AbrirVentanaEmergente({
        div: 'div_listado_personal',
        title: "Buscar Vendedor",
        url: ActionDescuentoUrl._Personal
    });
}
/**********************************************************/


function fnCalcularComision(p_codigo_persona) {

    $('#dgv_planilla_saldo').datagrid("reload", { p_codigo_planilla: ActionPlanillaUrl._codigo_planilla, p_codigo_persona: p_codigo_persona });
    
}



function fnConfigurarGrillaSaldoPlanilla() {

    $('#dgv_planilla_saldo').datagrid({
        fitColumns: true,
        //height: '200',
        data: null,
        rownumbers: true,
        toolbar: "#tlb_descuento",        
        url: ActionDescuentoUrl._Calcular_Comision_Persona,
        queryParams: {
            p_codigo_planilla: ActionPlanillaUrl._codigo_planilla,
            p_codigo_persona: 0
        },
        pagination: false,
        singleSelect: true,
        pageSize: 10,
        columns:
        [[

            { field: 'codigo_empresa', title: 'Codigo Empresa', hidden: 'false' },
            { field: 'tiene_descuento', title: 'tiene descuento', hidden: 'false' },
            { field: 'nombre_empresa', title: 'Empresa', width: 90, halign: "center", align: "left" },

            {
                field: 'monto_bruto', title: 'Importe<br>Comisión', halign: "center", align: "right", width: 120, formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                }
            },
            {
                field: 'igv', title: 'IGV', width: 120, halign: "center", align: "right", formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                }
            },
             {
                 field: 'monto_descuento', title: 'Descuento', width: 120, halign: 'center', align: 'right', formatter: function (value, row) {
                     return $.NumberFormat(value, 2);
                 },
                 editor: {
                     type: 'numberbox',
                     options: {
                         precision: 2,
                         required: true
                     }
                 }
             },
            {
                field: 'monto_afecto_descuento', title: 'Comisión<br>afecto a Descuento', width: 130, halign: "center", align: "right", formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                }
            },
              {
                  field: 'motivo', title: 'Motivo', width: 350, halign: 'center', align: 'left',
                  editor: {
                      type: 'textbox',
                      options: {                         
                          required: true,
                          multiline: true,
                          maxlength: 4
                      }
                  }
              },

        ]],
        onBeforeEdit: function (index, row) {
            var ed = $("#dgv_planilla_saldo").datagrid('getEditor', { index: index, field: 'monto_descuento' });
            /*
            shortcut.add("Enter", function () {
                var row_index_edicion = $.IndexRowEditing('dgv_planilla_saldo');
                if ($.validarRowGrilla(row_index_edicion, 'dgv_planilla_saldo',false))
                {
                    $("#dgv_planilla_saldo").datagrid("endEdit", row_index_edicion);
                }
                
            });
            shortcut.add("Esc", function () {
                var row_index_edicion = $.IndexRowEditing('dgv_planilla_saldo');
                $('#dgv_planilla_saldo').datagrid('cancelEdit', row_index_edicion);
            });*/
        },
        onAfterEdit: function (index, row) {
            /*
            shortcut.remove("Enter");
            shortcut.remove("Esc");*/
            row.editing = false;
        },
        onCancelEdit: function (index, row) {
            row.editing = false;
        }        ,
        onDblClickRow: function (index, row) {

            if (!$.existeRegistroEnEdicion('dgv_planilla_saldo')) {
                if (!row.tiene_descuento) {
                    $("#dgv_planilla_saldo").datagrid("beginEdit", index);
                    var Editor = $("#dgv_planilla_saldo").datagrid("getEditor", { index: index, field: 'monto_descuento' });
                    var v_monto_afecto = parseFloat(row.monto_afecto_descuento);
                    
                    if ($(Editor.target).data('numberbox')) {
                        $(Editor.target).numberbox({ validType: 'MayorACeroMenorIgualA[' + v_monto_afecto + ']' });
                        $(Editor.target).numberbox('textbox').focus();
                        //$(Editor.target).numberbox('clear').numberbox('textbox').focus();
                    }
                    else {
                        ($(Editor.target).data('textbox') ? $(Editor.target).textbox('textbox') : $(Editor.target)).focus();
                    }



                    row.editing = true;
                }
                else {
                    $.messager.alert('Registro no editable', "El registro seleccionado tiene descuento activo.", 'error');
                }
            }
        }
    });
    $('#dgv_planilla_saldo').datagrid('enableFilter');
    $(window).resize(function () {
        $('#dgv_planilla_saldo').datagrid('resize');
    });
}

function fnAceptarDescuento()
{
    var row_index_edicion = $.IndexRowEditing('dgv_planilla_saldo');
    if ($.validarRowGrilla(row_index_edicion, 'dgv_planilla_saldo', false))
    {
        $("#dgv_planilla_saldo").datagrid("endEdit", row_index_edicion);
    }

}

function fncancelarDescuento()
{

    var row_index_edicion = $.IndexRowEditing('dgv_planilla_saldo');
    $('#dgv_planilla_saldo').datagrid('cancelEdit', row_index_edicion);
}
function addCommas(nStr) {
    nStr += '';
    x = nStr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}