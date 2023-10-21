var ActionIndexUrl = {};
var v_estado_cuota_excluido = 4;
; (function (app) {
    //===========================================================================================
    var current = app.index = {};
    //===========================================================================================

    jQuery.extend(app.index,
        {
            Initialize: function (actionUrls) {

                jQuery.extend(ActionIndexUrl, actionUrls);
                fnConfigurarGrillaPagoExclusion();                
                

            }
        })
})(project);


function fnConfigurarGrillaPagoExclusion() {

    $('#dgv_exclusion').datagrid({
        fitColumns: false,
        height: "500",
        idField: 'codigo_exclusion',
        url: ActionIndexUrl._GetPagosExcluidoAllJson,         
        pagination: true,
        singleSelect: false,
        remoteSort: false,
        rownumbers: true,
        pageList: [10, 20, 50, 100, 200, 400, 500],
        pageSize: 50,
        columns:
        [[
            //{ field: 'ck', title: '', checkbox: true, styler: cellStyler },
            { field: 'codigo_estado_cuota', title: 'estado actual de la cuota', hidden: 'true' },
            { field: 'codigo_planilla', title: 'id planilla', hidden: 'true' },
            { field: 'codigo_exclusion', title: 'Id', sortable: true, width: 60, align: 'center', styler: cellStyler },
            { field: 'codigo_detalle_planilla', title: 'id detalle planilla', hidden: 'true' },            
            { field: 'codigo_detalle_cronograma', title: 'ID',  hidden: 'true' },
            { field: 'nombre_estado_exclusion', title: 'Estado', sortable: true, width: 90, align: 'left' },
            { field: 'str_fecha_exclusion', title: 'Fecha<br>Exclusión',  width: 100, align: 'center' },
            { field: 'usuario_exclusion', title: 'Usuario<br>Exclusión',  width: 120, align: 'left' },

            { field: 'numero_planilla', title: 'Nro. Planilla<br>Excluido', sortable: true, width: 100, align: 'center' },
            { field: 'numero_planilla_incluido', title: 'Nro. Planilla<br>Incluido', width: 105, align: 'center' },
            { field: 'str_fecha_registra', title: 'Fecha<br>Planilla E.', width: 105, align: 'center' },
            { field: 'str_fecha_inicio', title: 'Fecha Inicio E.', width: 100, align: 'center' },
            { field: 'str_fecha_fin', title: 'Fecha Fin E.', width: 100, align: 'center' },
            { field: 'nombre_empresa', title: 'Empresa', width: 80, align: 'left' },
            { field: 'nro_contrato', title: 'Nro.<br>Contrato', width: 110, align: 'center' },
            { field: 'nro_cuota', title: 'Nro.<br>Cuota', width: 50, align: 'center' },
            { field: 'apellidos_nombres', title: 'Vendedor', sortable: true, width: 300, align: 'left' },

            { field: 'nombre_estado_cuota', title: 'Estado<br>de la cuota', sortable: true, width: 150, align: 'left' },
            {
                field: 'opcion', width: 120, title: 'Ver Exclusión',align: 'center',
                formatter: exclusionFormatter
            }
            
        ]],     
        /*
        onLoadSuccess: function (data) {
            $('#dgv_exclusion').datagrid('getPanel').find('div.datagrid-header input[type=checkbox]').attr('disabled', 'disabled');
            var opts = $(this).datagrid('options');
            for(var i=0; i<data.rows.length; i++){
                var row = data.rows[i];
                if (row.codigo_estado_cuota != v_estado_cuota_excluido) {
                    var tr = opts.finder.getTr(this,i);
                    tr.find('input[type=checkbox]').attr('disabled','disabled');   
                }
            }
        },*//*
        onCheck: function (index, row) {
            if (!(row.codigo_estado_cuota == v_estado_cuota_excluido && row.codigo_estado_exclusion==1))
            {
                $(this).datagrid('unselectRow', index);              
                
            }
            
        },*/
        onClickRow: function (index, row) {            
            if (!(row.codigo_estado_cuota == v_estado_cuota_excluido && row.codigo_estado_exclusion == 1)) {
                $(this).datagrid('unselectRow', index);
            }
        }
      
    });
    

    /*
   var dg = $('#dgv_exclusion').datagrid();
   dg.datagrid('enableFilter', [{
       field: 'nombre_estado_cuota',
       type:'combobox',
       options:{
           panelHeight:'auto',
           data: [{ value: '', text: 'Todos' }, { value: 'Pendiente', text: 'Pendiente' }, { value: 'Pagado', text: 'Pagado' }, { value: 'En proceso pago', text: 'En proceso pago' }],
           onChange:function(value){
               if (value == ''){
                   dg.datagrid('removeFilterRule', 'nombre_estado_cuota');
               } else {
                   dg.datagrid('addFilterRule', {
                       field: 'nombre_estado_cuota',
                       op: 'equal',
                       value: value
                   });
               }
               dg.datagrid('doFilter');
           }
       }
   }]);
    */
    $('#dgv_exclusion').datagrid('enableFilter');
    $(window).resize(function () {
        $('#dgv_exclusion').datagrid('resize');
    });
}

function cellStyler(value, row, index) {
    if (row.codigo_estado_cuota == v_estado_cuota_excluido && row.codigo_estado_exclusion == 1) {
        return 'background-color:#4cff00;';
    }
    else {
        return 'background-color:#ff0000;';
    }
}

function exclusionFormatter(value, row) {

    var link = '<a href="javascript:void(0)" onclick="fnDetalleExclusion(' + row.codigo_exclusion + ')">Detalle</a>';
    return link;
}

function fnDetalleExclusion(p_codigo_exclusion)
{
    $(this).AbrirVentanaEmergente({
        parametro: "?p_codigo_exclusion=" + p_codigo_exclusion,
        div: 'div_detalle_exclusion',
        title: "Detalle Exclusión",
        url: ActionIndexUrl._Detalle_Exclusion
    });
}

function fnHabilitarExclusion() {
    var row = $('#dgv_exclusion').datagrid('getSelected');    
    if (!row) {
        $.messager.alert('Seleccione registro', "Para continuar con el proceso debe seleccionar un registro.", 'warning');
        return;
    }

    $(this).AbrirVentanaEmergente({        
        div: 'div_habilitar_exclusion',
        title: "Habilitar Exclusión",
        url: ActionIndexUrl._Registrar_Exclusion
    });
}

function fnConsultarExclusion()
{
    $('#dgv_exclusion').datagrid("reload");
    $('#dgv_exclusion').datagrid("clearSelections");

    
}