var ActionContabilidadUrl = {};

; (function (app) {
    //===========================================================================================
    var current = app.contabilidad = {};
    //===========================================================================================

    jQuery.extend(app.contabilidad,
        {
            Initialize: function (actionUrls) {
                jQuery.extend(ActionContabilidadUrl, actionUrls);
                fnConfigurarGrillaEmpresas();
            }
        })
})(project);

function fnConfigurarGrillaEmpresas() {
    $('#dgv_resumen_planilla_txt').datagrid({
        fitColumns: true,
        idField: 'codigo_planilla',
        url: ActionContabilidadUrl.GetResumenPlanillaTxtJson,
        queryParams: { codigo_planilla: ActionContabilidadUrl._codigo_planilla },
        pagination: false,
        singleSelect: true,
        remoteFilter: false,
        rownumbers: true,        
        pageList: [ 10 ],
        pageSize: 10,
        showPageList: false,
        showPageInfo: false,
        columns:
        [[
            { field: 'codigo_planilla', hidden: true },
            { field: 'codigo_empresa', hidden: true },
            { field: 'nombre_empresa', title: 'Empresa', width: 100, align: 'left' },
            { field: 'comisiones', title: 'Comisiones', width: 100, align: 'right' },
        ]],
        onClickRow: function (index, row) {

        }
    });

    //var pager = $('#dgv_resumen_planilla_txt').datagrid('getPager');
    //pager.pagination({
    //    showPageList: false,
    //    showPageInfo: false
    //});
}

function fnGenerarTxt() {
    var row = $('#dgv_resumen_planilla_txt').datagrid('getSelected');
    if (!row) {
        $.messager.alert('Seleccione registro', "Para continuar con el proceso debe seleccionar un registro.", 'warning');
        return;
    }

    var url = ActionContabilidadUrl.GenerarTxtContabilidad + "?codigo_planilla=" + ActionContabilidadUrl._codigo_planilla + "&codigo_empresa=" + row.codigo_empresa;
    window.open(url, '_blank');
    $('#div_txt_contabilidad').dialog('close');
}

function fnCancelar() {
    $('#div_txt_contabilidad').dialog('close');
}