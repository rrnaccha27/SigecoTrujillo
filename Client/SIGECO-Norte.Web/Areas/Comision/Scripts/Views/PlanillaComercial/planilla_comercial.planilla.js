var ActionPlanillaComercialUrl = {};

; (function (app) {
    //===========================================================================================
    var current = app.planilla = {};
    //===========================================================================================

    jQuery.extend(app.planilla,
        {
            Initialize: function (actionUrls) {                            
                jQuery.extend(ActionPlanillaComercialUrl, actionUrls);

                fnInicializarPlanillaComercial();

                fnConfigurarGrillaPagoHabilitado();
                
            }
        })
})(project);


function fnInicializarPlanillaComercial()
{

    

    $('.content').combobox_sigees({
        id: '#cmb_tipo_planilla',
        url: ActionPlanillaComercialUrl._GetTipoPlanillaJson
    });
    
 
    $('.content').combobox_sigees({
        id: '#cmb_regla_tipo_planilla',
        url: ActionPlanillaComercialUrl._GetReglaTipoPlanillaJson
    });
    

    $('#dtp_fecha_inicio').DeshabilitarFechaMayorHoy();
    $('#dtp_fecha_fin').DeshabilitarFechaMayorHoy();

    $('#dtp_fecha_inicio').formatteDate();
    $('#dtp_fecha_fin').formatteDate();



    $('.content').ResizeModal({
        widthMax: '90%',
        widthMin: '80%',
        div: 'div_planilla_comercial'
    });

    $(window).resize(function () {


        $('#dgv_planilla_comercial_pago_habilitado').datagrid('resize', {
            height: '100%'
        });
        $('#dgv_planilla_comercial_pago_habilitado').datagrid('resize', {
            height: '100%'
        });

        var v_altura_ventana = $(window).height();
        if (v_altura_ventana < 800) {
            $("#div_planilla_comercial_pago_habilitado").height(300);
        }
        else {
            $("#div_planilla_comercial_pago_habilitado").height(500);
            
        }
    });
}





function fnConfigurarGrillaPagoHabilitado() {
    
    $('#dgv_planilla_comercial_pago_habilitado').datagrid({
        fitColumns: true,        
        data: null,
        //height: '300',
        rownumbers: true,
        toolbar: '#toolbar_pago_habilitado',
        url: ActionPlanillaComercialUrl._GetPagoHabilitadoJson,
        queryParams: {
            codigo_planilla: ActionPlanillaComercialUrl._codigo_planilla
        },
        pagination: true,
        singleSelect: true,
        pageList: [20, 50, 100, 200, 400, 500],
        pageSize: 20,
        columns:
        [[
            { field: 'codigo_personal', title: 'Cód. Personal', width: 120, hidden:true },
            { field: 'nombre_grupo_canal', title: 'Grupo', width: 180, align: 'left', halign: "center" },
            { field: 'apellidos_nombres', title: 'Vendedor', width: 200, align: 'left', halign: "center" },
            
            {
                field: 'monto_bruto', title: 'Importe <br> Comisión', width: 100, halign: "center", align: "right", formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                }
            },
            {
                field: 'igv', title: 'IGV', width: 100, halign: "center", align: "right", formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                }
            },
             {
                 field: 'monto_descuento', title: 'Descuento', width: 100, halign: "center", align: "right", formatter: function (value, row) {
                     return $.NumberFormat(value, 2);
                 }
             },
             {
                 field: 'comision_total', title: 'Comisión<br> Total', width: 100, halign: "center", align: "right", formatter: function (value, row) {
                     return $.NumberFormat(value, 2);
                 }
             },
            {
                field: 'monto_neto', title: 'Monto<br> Total', width: 100, halign: "center", align: "right", formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                }
            }

            
        ]],
        onClickRow: function (index, row) {
           
        }
    });
    $('#dgv_planilla_comercial_pago_habilitado').datagrid('enableFilter');
    $(window).resize(function () {
        $('#dgv_planilla_comercial_pago_habilitado').datagrid('resize');
    });

    var pager = $('#dgv_planilla_comercial_pago_habilitado').datagrid('getPager');    // get the pager of datagrid
    pager.pagination({
        showPageList: true,
        buttons: [{
            iconCls: 'icon-print',
            text: 'Liquidación',
            handler: function () {
                fnReporteLiquidacionCustom();
            }
        }]
    });

}



function cerrar_planilla()
{
    $("#div_planilla_comercial").dialog("close");

}


function fnReporteLiquidacion(p_codigo_planilla) {
    
    //var row = $('#dgv_planilla_comercial_pago_habilitado').datagrid('getSelected');
    //console.log(row);
    //if (!row) {
    //    $.messager.alert('Seleccione registro', "Para continuar con el proceso, seleccione un registro.", 'warning');
    //}
    var v_parametro = "?p_codigo_planilla=" + p_codigo_planilla;// + "&p_codigo_personal=" + row.codigo_personal;
    
    $(this).AbrirVentanaEmergente({
        parametro:v_parametro,
        div: 'div_reporte_general',
        title: "Reporte Liquidación",
        url: ActionPlanillaComercialUrl._Reporte_Liquidacion
    });
}

function fnReporteLiquidacionCustom() {
    var row = $('#dgv_planilla_comercial_pago_habilitado').datagrid('getSelected');
    if (!row) {
        $.messager.alert('Consulta Planilla', "Para continuar con el proceso seleccione un vendedor.", 'warning');
        return;
    }

    var v_parametro = "?p_codigo_planilla=" + ActionPlanillaComercialUrl._codigo_planilla + "&p_codigo_personal=" + row.codigo_personal;

    $(this).AbrirVentanaEmergente({
        parametro: v_parametro,
        div: 'div_reporte_general',
        title: "Reporte Liquidación",
        url: ActionPlanillaComercialUrl._Reporte_Liquidacion
    });
}

function fnReportePlanilla(p_codigo_planilla) {

    $(this).AbrirVentanaEmergente({
        parametro: "?p_codigo_planilla=" + p_codigo_planilla,
        div: 'div_reporte_general',
        title: "Reporte Planilla",
        url: ActionPlanillaUrl._Reporte
    });
}



function fnExportarPlanilla(p_codigo_planilla)
{
    
    $(this).AbrirVentanaEmergente({
        parametro: "?p_codigo_planilla=" + p_codigo_planilla,
        div: 'div_reporte_general',
        title: "Reporte Planilla",
        url: ActionPlanillaComercialUrl._Exportar
    });
}
