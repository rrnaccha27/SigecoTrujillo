var ActionPersonalUrl = {};

; (function (app) {
    //===========================================================================================
    var current = app.personal = {};
    //===========================================================================================

    jQuery.extend(app.personal,
        {
            Initialize: function (actionUrls) {
                jQuery.extend(ActionPersonalUrl, actionUrls);
                fnInicializarPersonal();
                fnConfigurarGrillaPersonal();
            }
        })
})(project);




function fnInicializarPersonal() {

    //$('.content').ResizeModal({
    //    widthMax: '90%',
    //    widthMin: '50%',
    //    div: 'div_busqueda_personal'
    //});
        
    $('#txt_nombre_personal').textbox({
        onClickButton: function () {
            fnBuscarPersonal();
        }
    });

}

function fnConfigurarGrillaPersonal(){    
        
    $('#dgv_listado_personal').datagrid({
        fitColumns: true,
        url: ActionPersonalUrl._GetListarPersonalJson,
        idField: 'codigo_personal',
        pagination: true,
        singleSelect: true,
        rownumbers: true,
        queryParams: {
            nombre: ""
        },
        columns: [[
            { field: 'codigo_personal', hidden: true },
            { field: 'codigo_canal', hidden: true },
            { field: 'codigo_equivalencia', title: 'Código', width: '16%', align: 'left' },
            { field: 'nombres', title: 'Nombre', width: '60%', align: 'left' },
            { field: 'nombre_canal', title: 'Canal', sortable: true, width: '24%', align: 'left' },
        ]],
        pageList: [20, 50, 100, 200, 400, 500],
        pageSize: 20,
        onDblClickRow: function (index, row) {
            //ActionComisionUrl._codigo_personal = row.codigo_personal;
            //ActionComisionUrl._codigo_canal = row['codigo_canal'];

            $('#hdCodigoPersonal').val(row.codigo_personal);
            $('#hdCodigoCanal').val(row['codigo_canal']);

            $("#txt_apellidos_nombres_personal").textbox('setText', row['nombres']);
            $("#txt_nombre_canal").textbox('setText', row['nombre_canal']);
            $("#div_busqueda_personal").dialog("close");
        }
    });
    $('#dgv_listado_personal').datagrid('enableFilter');    
    $(window).resize(function () {
        $('#dgv_listado_personal').datagrid('resize');
    });

   
}


function fnBuscarPersonal() {
    var queryParams= {
        nombre: $("#txt_nombre_personal").textbox("getText")
    };

    $('#dgv_listado_personal').datagrid('reload', queryParams);
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
