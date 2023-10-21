var ActionIndexUrl = {};

; (function (app) {
    //===========================================================================================
    var current = app.index = {};
    //===========================================================================================

    jQuery.extend(app.index,
        {
            Initialize: function (actionUrls) {
                jQuery.extend(ActionIndexUrl, actionUrls);
                fnConfigurarGrillaEmpresa();
                
                

            }
        })
})(project);




function fnNuevaEmpresa()
{
    
    $(this).AbrirVentanaEmergente({
        parametro: "?codigo_empresa=" + 0,
        div: 'div_registrar_empresa',
         title: "Mantenimiento Empresa",
         url: ActionIndexUrl.Registro
    });
}
function fnModificarEmpresa() {

    var row = $('#dgv_empresa').datagrid('getSelected');
    console.log(row);
    if (!row)
    {
        $.messager.alert('Seleccione registro', "Para continuar con el proceso debe seleccionar un registro.", 'warning');
        return;
    }

    $(this).AbrirVentanaEmergente({
        parametro: "?codigo_empresa=" + row.codigo_empresa,
        div: 'div_registrar_empresa',
        title: "Mantenimiento Empresa",
        url: ActionIndexUrl.Registro
    });
}



function fnConfigurarGrillaEmpresa() {
    $('#dgv_empresa').datagrid({
        fitColumns: true,
        idField: 'codigo_empresa',
        url: ActionIndexUrl.GetRegistrosJSON,
        height: '600',       
        pagination: true,
        singleSelect: true,
        remoteFilter: false,
        rownumbers: true,        
        pageList: [ 20, 40, 60],
        pageSize: 20,
        columns:
        [[
            
            { field: 'codigo_empresa', title: 'codigo_empresa',hidden:true },
            { field: 'estado', title: 'estado',hidden:true },
            { field: 'nombre', title: 'Abreviatura', width: 150, align: 'left' },
            { field: 'nombre_largo', title: 'Empresa', width: 200, align: 'left' },
            { field: 'ruc', title: 'R.U.C', width: 130, align: 'left' },
            { field: 'nro_cuenta', title: 'N° de Cuenta', width: 130, align: 'left' },                        
            { field: 'nombre_estado', title: 'Estado', width: 100, align: 'left' }
            
            
        ]],
        onClickRow: function (index, row) {
            if (row.codigo_estado_planilla == 2) {
                $("#btnGenerarTxt").linkbutton('enable');
            }
            else {
                $("#btnGenerarTxt").linkbutton('disable');

            }
        }
    });
    $('#dgv_empresa').datagrid('enableFilter', [{
        field: 'nombre_estado',
        type: 'combobox',
        options: {
            panelHeight: 'auto',
            data: [{ value: '', text: 'Todos' }, { value: '1', text: 'Activo' }, { value: '0', text: 'Inactivo' }],
            onChange: function (value) {

                if (value == '') {
                    $('#dgv_empresa').datagrid('removeFilterRule', 'estado');
                } else {
                    $('#dgv_empresa').datagrid('addFilterRule', {
                        field: 'estado',
                        op: 'equal',
                        value: value
                    });
                }
                $('#dgv_empresa').datagrid('doFilter');
            }
        }
    }]);
    
  
}

function GetAllJson() {
    $('#dgv_empresa').datagrid("reload");    
}



