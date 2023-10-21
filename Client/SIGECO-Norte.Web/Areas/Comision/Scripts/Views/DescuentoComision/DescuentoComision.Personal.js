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
       
    $('#txt_nombre_personal').textbox({
        onClickButton: function () {
            fnBuscarPersonal();
        }
    });

}

function fnConfigurarGrillaPersonal(){    
        
    $('#dgv_listado_personal').datagrid({
        fitColumns: true,
        url: ActionPersonalUrl.GetPersonalJson,
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
            { field: 'nombres', title: 'Nombre', width: '50%', align: 'left' },
            { field: 'nombre_canal', title: 'Canal', sortable: true, width: '50%', align: 'left' },
        ]],
        pageList: [20, 50, 100, 200, 400, 500],
        pageSize: 20,
        onDblClickRow: function (index, row) {
            $('#hdCodigoPersonal').val(row.codigo_personal);
            $("#nombre_personal").textbox('setText', row['nombres']);
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
