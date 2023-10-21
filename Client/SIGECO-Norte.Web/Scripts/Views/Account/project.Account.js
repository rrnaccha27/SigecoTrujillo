;
(function (app) {
    //===========================================================================================
    var current = app.Account = {};
    //===========================================================================================

    jQuery.extend(app.Account,
        {
            
            ActionUrls: {},

            Initialize: function (actionUrls) {
                jQuery.extend(project.ActionUrls, actionUrls);

                alert('entro1`');

                $('#btnIngresar').click(function () {

                    alert('asd');

                    /*
                    $.messager.confirm('Confirm', 'Seguro que desea eliminar este registro?', function (result) {
                        if (result) {
                            $.ajax({
                                type: 'post',
                                url: project.ActionUrls.Eliminar,
                                data: { codigo: codigo },
                                async: false,
                                cache: false,
                                dataType: 'json',
                                success: function (data) {
                                    if (data.Msg) {
                                        if (data.Msg != 'Success') {
                                            $.messager.alert('Error', data.Msg, 'error');
                                        }
                                        else {
                                            project.ShowMessage('Error', 'Eliminacion exitosa');
                                            $('#DataGrid').datagrid('reload');
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
                    */

                });



            },
        
        });

})
(project);