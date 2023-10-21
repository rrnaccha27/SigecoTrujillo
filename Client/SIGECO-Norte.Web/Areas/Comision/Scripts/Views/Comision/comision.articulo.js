var ActionArticuloUrl = {};

; (function (app) {
    //===========================================================================================
    var current = app.articulo = {};
    //===========================================================================================

    jQuery.extend(app.articulo,
        {
            Initialize: function (actionUrls) {
                jQuery.extend(ActionArticuloUrl, actionUrls);
                fnInicializarArticulo();
                fnConfigurarGrillaArticulo();
            }
        })
})(project);




function fnInicializarArticulo() {

    //$('.content').ResizeModal({
    //    widthMax: '90%',
    //    widthMin: '50%',
    //    div: 'div_busqueda_articulo'
    //});
        
    $('#txt_nombre_articulo_busqueda').textbox({
        onClickButton: function () {                    
            fnBuscar_Articulo_By_ContratoEmpresa();                
        }
    });

}

function fnConfigurarGrillaArticulo()
{
    //alert(numero_contrato + '-' + codigo_empresa);
        
    $('#dgv_listado_articulo').datagrid({
        idField: 'codigo_articulo',
        url:ActionArticuloUrl._GetListarArticuloJson,
        pagination: true,
        singleSelect: true,
        fitColumns: true,
        queryParams: {
            codigo_empresa: $('#hdCodigoEmpresa').val(),
            nro_contrato: $('#hdNroContrato').val(),
            nombre: '',
            codigo_personal: $('#hdCodigoPersonal').val(),
            codigo_canal: $('#hdCodigoCanal').val(),
            codigo_campo_santo: 0,
            codigo_tipo_venta: $('#hdCodigoTipoVenta').val(),
            codigo_tipo_pago: $('#hdCodigoTipoPago').val()
        },
        rownumbers: true,        
        emptyMsg: '',
        pageList: [20, 50, 100, 200, 400, 500],
        pageSize: 20,
        //pageList: [2, 4, 6, 100, 200, 400, 500],
        //pageSize: 2,
        columns:
        [[
            { field: 'codigo_articulo', hidden: true },
            { field: 'codigo_sku', title: 'Código SKU', width: 120, align: 'left', haling: 'center' },
            { field: 'nombre', title: 'Nombre', width: 300, align: 'left', haling: 'center' },
            {
                field: 'precio', title: 'Precio', width: 100, align: 'right', haling: 'center', formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                }
            },
            {
                field: 'monto_comision', title: 'Comisi&oacute;n', width: 100, align: 'right', haling: 'center', formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                }
            },
        ]],    
        onDblClickRow: function (index, row)
        {
            $("#txt_nombre_articulo").textbox("setValue", row.nombre);
            $("#hdCodigoArticulo").val(row.codigo_articulo);
            //$("#cmb_campo_santo").combobox("setValue", row.codigo_campo_santo);
            //ActionComisionUrl._codigo_articulo=row.codigo_articulo;
            $("#div_busqueda_articulo").dialog("close");
            fnSetearComision(row.monto_comision);
        }
    });

    $('#dgv_listado_articulo').datagrid('enableFilter');
    
    $(window).resize(function () {
        $('#dgv_listado_articulo').datagrid('resize');
    });
}


function fnBuscar_Articulo_By_ContratoEmpresa() {   
    var queryParams = {
        codigo_empresa: $('#hdCodigoEmpresa').val(),
        nro_contrato: $('#hdNroContrato').val(),
        nombre: $.trim($('#txt_nombre_articulo_busqueda').textbox("getText")),
        codigo_personal: $('#hdCodigoPersonal').val(),
        codigo_canal: $('#hdCodigoCanal').val(),
        codigo_campo_santo: $('#hdCodigoCamposanto').val(),
        codigo_tipo_venta: $('#hdCodigoTipoVenta').val(),
        codigo_tipo_pago: $('#hdCodigoTipoPago').val()
    };

    $('#dgv_listado_articulo').datagrid('reload', queryParams);
}

/*
   var dg = $('#dgv_listado_articulo').datagrid();
   dg.datagrid('enableFilter', [{
       field:'nombre_campo_santo',
       type:'combobox',
       options:{
           panelHeight:'auto',
           data:fnLoadCampoSanto,//[{value:'',text:'Todos'},{value:'Lurin',text:'Lurin'},{value:'N',text:'N'}],
           onChange:function(value){
               if (value == ''){
                   dg.datagrid('removeFilterRule', 'nombre_campo_santo');
               } else {
                   dg.datagrid('addFilterRule', {
                       field: 'nombre_campo_santo',
                       op: 'equal',
                       value: value
                   });
               }
               dg.datagrid('doFilter');
           }
       }
   }]);*/
