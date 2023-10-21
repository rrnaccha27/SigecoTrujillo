;
(function (app) {
    //===========================================================================================
    var current = app.TreeNode = {};
    //===========================================================================================

    jQuery.extend(app.TreeNode,
        {

            ActionUrls: {},
            EditTreeNode: {},
            EditType: '',
            HasRootNode: 'False',

            Initialize: function (actionUrls) {
                /// <summary>
                /// Initialize
                /// </summary>
                /// <param name="actionUrls"></param>

                jQuery.extend(project.ActionUrls, actionUrls);
    
                current.CheckHasRootNode();
                current.Initilaize_TreeNodeDLL();

                if (current.HasRootNode == 'True') {
                    current.Initialize_TreeGrid();
                }

                $('#btnNuevo').click(function () {
                    current.MoveDownEditType = 'Create';
                    current.Initilaize_TreeNodeDLL();
                    $('#dlgModificar').dialog('open').dialog('setTitle', 'Registrar Menu');
                    $('#frmModificar').form('clear');
                    $('#ParentNode')[0].selectedIndex = 0;
                });

                $('#btnModificar').click(function () {
                    current.Initilaize_TreeNodeDLL();
                    var codigo = $("#hdCodigo").val();

                    current.GetTreeNodeData(codigo);

                    if (!EditTreeNode.ID) {
                        $.messager.alert('錯誤', 'Error al cargar los datos del menu', 'error', function () {
                            $('#TreeGrid').treegrid('reload');  //重新載入TreeGrid
                        });
                    }
                    else {
                        current.EditType = 'Update';
                        $('#dlgModificar').dialog('open').dialog('setTitle', 'Modificar Menu');

                        $('#dlgModificar #NodeID').val(EditTreeNode.ID);
                        $('#dlgModificar #NodeName').textbox('setText', EditTreeNode.Name);
                        $('#dlgModificar #IsRootNode').val(EditTreeNode.IsRootNode);
                        $('#dlgModificar #ParentNode').combotree('setValue', EditTreeNode.ParentID);
                        $('#dlgModificar #NodeEnable').prop('checked', EditTreeNode.IsEnable);
                    }
                });

                $('#btnEliminar').click(function () {
                    var codigo = $("#hdCodigo").val();

                    if (codigo != null) {
                        $.messager.confirm('Confirm', 'Seguro que desea eliminar este nodo?', function (result) {
                            if (result) {
                                $.ajax({
                                    type: 'post',
                                    url: project.ActionUrls.Delete,
                                    data: { id: codigo },
                                    async: false,
                                    cache: false,
                                    dataType: 'json',
                                    success: function (data) {
                                        if (data.Msg) {
                                            if (data.Msg != 'Success') {
                                                $.messager.alert('錯誤', data.Msg, 'error');
                                            }
                                            else {
                                                project.ShowMessage('訊息', 'Eliminacion de nodo satisfactorio.');
                                                $('#TreeGrid').treegrid('reload');  //重新載入TreeGrid
                                            }
                                        }
                                        else {
                                            project.AlertErrorMessage('錯誤', 'Error de procesamiento');
                                        }
                                    },
                                    error: function () {
                                        project.AlertErrorMessage('錯誤', 'Error');
                                    }
                                });
                            }
                        });
                    }
                });

                $('#btnVisualizar').click(function () {
                    current.Initilaize_TreeNodeDLL();
                    var codigo = $("#hdCodigo").val();
                    
                    current.GetTreeNodeData(codigo);

                    if (!EditTreeNode.ID) {
                        $.messager.alert('錯誤', 'Error al cargar los datos del menu', 'error', function () {
                            $('#TreeGrid').treegrid('reload');  //重新載入TreeGrid
                        });
                    }
                    else {
                        current.EditType = 'Visualizar';
                        $('#dlgVisualizar').dialog('open').dialog('setTitle', 'Detalle Registro');

                        $('#dlgVisualizar #NodeID').val(EditTreeNode.ID);
                        //$('#dlgVisualizar #NodeName').val(EditTreeNode.Name);
                        $('#dlgVisualizar #NodeName').textbox('setText', EditTreeNode.Name);
                        $('#dlgVisualizar #IsRootNode').val(EditTreeNode.IsRootNode);
                        $('#dlgVisualizar #ParentNode').combotree('setValue', EditTreeNode.ParentID);
                        $('#dlgVisualizar #NodeEnable').prop('checked', EditTreeNode.IsEnable);
                    }
                });

                $('#btnRefrescar').click(function () {
                    $('#TreeGrid').treegrid('reload');  //重新載入TreeGrid
                    current.Initilaize_TreeNodeDLL(); //重新載入TreeNode下拉選單
                });

                $('#dlgModificar #ButtonCancel').click(function () {
                    $('#frmModificar').form('clear');
                    $('#dlgModificar').dialog('close');
                    
                });

                $('#dlgModificar #ButtonSave').click(function () {
                    current.ButtonSaveEventHandler();
                });

            },

            CheckHasRootNode: function() {
                /// <summary>
                /// 檢查是否有建立根節點
                /// </summary>

                $.ajax({
                    type: 'Get',
                    url: project.ActionUrls.HasRootNode,
                    dataType: 'json',
                    cache: false,
                    async: false,
                    success: function (data) {
                        if (data.Msg) {
                            current.HasRootNode = data.Msg;
                            if (current.HasRootNode == 'False') {
                                $.messager.confirm('Alerta', 'No se encontro Registros, Desea registrar?', function (result) {
                                    if (result) {
                                        $('#btnNuevo').trigger('click');
                                    }
                                });
                            }
                        }
                    }
                });
            },

            Initilaize_TreeNodeDLL: function() {
                //cargar el combo de nodos
                var cboTreeNodo = project.ActionUrls.LoadTreeNodeDDL;

                $('#dlgModificar #ParentNode').combotree({
                    url: cboTreeNodo
                });

                $('#dlgVisualizar #ParentNode').combotree({
                    url: cboTreeNodo
                });
            },

            Initialize_TreeGrid: function() {

                $('#TreeGrid').treegrid({
                    url: project.ActionUrls.GetTreeNodeJSON,
                    shrinkToFit: false,
                    nowrap: false,
                    fitColumns: true,
                    rownumbers: true,
                    idField: "ID",
                    treeField: 'Name',
                    animate: true,
                    collapsible: true,
                    columns:
                    [[
                        {
                            field: 'Optional',
                            title: 'Funciones',
                            width: 100,
                            align: 'center',
                            formatter: function (value, row) {
                                var content = '';

                                if (row.MoveUp) {
                                    content += String.format('<a class="MoveUp" nodeID="{0}" style="cursor:pointer; color:#0000ff;">Subir</a>　', row.ID);
                                } else {
                                    content += 'Subir　';
                                }

                                if (row.MoveDown) {
                                    content += String.format('<a class="MoveDown" nodeID="{0}" style="cursor:pointer; color:#0000ff;">Bajar</a>　', row.ID);
                                } else {
                                    content += 'Bajar　';
                                }

                                return content;
                            }
                        },
                        { field: 'Name', title: 'Nombre', width: 200, align: 'left' },
                        {
                            field: 'IsEnable',
                            title: 'EsHabilitado',
                            width: 80,
                            align: 'center',
                            formatter: function (value, row) {
                                if (value) {
                                    return 'SI';
                                } else {
                                    return 'NO';
                                }
                            }
                        },
                    ]],
                    onLoadSuccess: function (row) {
                        current.MoveUp();
                        current.MoveDown();
                        
                    },
                    onClickRow: function (row) {
                        $("#hdCodigo").val(row.ID);
                    },
                });
            },

            ButtonSaveEventHandler: function() {
                /// <summary>
                /// 儲存按鍵的事件處理
                /// </summary>

                var saveType = current.EditType.length == 0
                    ? 'Registrar'
                    : current.EditType == 'Create' ? 'Registrar' : 'Modificar';

                if (saveType == 'Registrar') {
                    current.CreateNode();
                }
                if (saveType == 'Modificar') {
                    current.UpdateNode();
                    current.EditType = '';
                }
            },

            CreateNode: function() {
                /// <summary>
                /// 新增節點
                /// </summary>

                var message = '';

                var parentId = $.trim($('#dlgModificar #ParentNode').combotree('getValue'));
                parentId = parentId.length == 0 ? 'root' : parentId;

                //var nodeName = $.trim($('#NodeName').val());
                var nodeName = $.trim($('#dlgModificar #NodeName').textbox('getText'));
                var nodeEnable = $('#NodeEnable').is(':checked');

                if (current.HasRootNode == 'True' && parentId.length == 0) {
                    message += "Por favor seleccione un nodo principal.<br/>";
                }
                if (nodeName.length == 0) {
                    message += "Por favor introduzca un nombre de nodo";
                }
                if (message.length > 0) {
                    $.messager.alert('Error', message, 'error');
                } else {
                    $.messager.confirm('Confirmar', 'Seguro que desea guardar?', function (result) {
                        if (result) {
                            var mapData = { parentId: parentId, name: nodeName, enable: nodeEnable };

                            $.ajax({
                                type: 'post',
                                url: project.ActionUrls.Create,
                                data: mapData,
                                dataType: 'json',
                                cache: false,
                                async: false,
                                success: function (data) {
                                    if (data.Msg) {
                                        if (data.Msg != 'Success') {
                                            AlertErrorMessage('建立錯誤', data.Msg);
                                        } else {
                                            $('#dlgModificar').dialog('close');
                                            project.ShowMessage('Aviso', 'Registro exitoso');

                                            $('#dlgModificar #NodeName').val('');
                                            $('#dlgModificar #NodeEnable').attr('checked', false);
                                            $('#dlgModificar #ParentNode')[0].selectedIndex = 0;
                                            /*
                                            alert('111');
                                            var data = $('#TreeGrid').treegrid('getData');
                                            alert(data);
                                            */
                                            //$('#TreeGrid').treegrid('reload');
                                            current.Initialize_TreeGrid();
                                            //alert($('#TreeGrid').treegrid('getData').total);

                                        }
                                    } else {
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
            },

            GetTreeNodeData: function(nodeId) {
                //<summary>取得單一節點的Json資料</summary>

                $.ajax({
                    type: 'post',
                    url: project.ActionUrls.GetTreeNode,
                    data: { id: nodeId },
                    async: false,
                    cache: false,
                    dataType: 'json',
                    success: function (data) {
                        if (data.Msg) {
                            project.AlertErrorMessage('建立錯誤', data.Msg);
                        }
                        else {
                            EditTreeNode =
                            {
                                ID: data.ID,
                                ParentID: data.ParentID,
                                Name: data.Name,
                                Sort: data.Sort,
                                IsEnable: data.IsEnable == 'True',
                                IsRootNode: data.IsRootNode == 'True'
                            };
                        }
                    },
                    error: function () {
                        project.AlertErrorMessage('Error', 'Error');
                    }
                });
            },

            UpdateNode: function() {

                var message = '';

                var nodeId = $.trim($('#NodeID').val());

                var parentId = $.trim($('#dlgModificar #ParentNode').combotree('getValue'));
                var nodeName = $.trim($('#dlgModificar #NodeName').textbox('getText'));
                //var nodeName = $.trim($('#dlgModificar #NodeName').val());
                var nodeEnable = $('#NodeEnable').is(':checked');
                var isRootNode = $('#IsRootNode').val() == 'true';

                if (nodeId.length == 0) {
                    message += "Sin informacion el id del nodo.<br/>";
                }
                if (!isRootNode && parentId.length == 0) {
                    message += "Por favor seleccione el nodo superior.<br/>";
                }
                if (parentId == nodeId) {
                    message += "No se puede elegir el mismo nodo como nodo superior.<br/>";
                }
                if (nodeName.length == 0) {
                    message += "Por favor introduzca un nombre de nodo";
                }
                if (message.length > 0) {
                    $.messager.alert('Error', message, 'error');
                }
                else {
                    $.messager.confirm('Confirm', 'Seguro que desea modificar?', function (result) {
                        if (result) {
                            var mapData = { id: nodeId, parentId: parentId, name: nodeName, enable: nodeEnable };

                            $.ajax({
                                type: 'post',
                                url: project.ActionUrls.Update,
                                data: mapData,
                                dataType: 'json',
                                cache: false,
                                async: false,
                                success: function (data) {
                                    if (data.Msg) {
                                        if (data.Msg != 'Success') {
                                            project.AlertErrorMessage('更新錯誤', data.Msg);
                                        }
                                        else {
                                            $('#dlgModificar').dialog('close');

                                            project.ShowMessage('Alerta', 'Modificacion Exitosa');

                                            //清空編輯Dialog的內容
                                            $('#dlgModificar #NodeName').val('');
                                            $('#dlgModificar #NodeEnable').attr('checked', false);
                                            $('#dlgModificar #ParentNode')[0].selectedIndex = 0;

                                            $('#TreeGrid').treegrid('reload');  //重新載入TreeGrid
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
            },

            MoveUp: function() {
                //<summary>移動節點：上移</summary>

                $('.MoveUp').each(function (i, item) {
                    $(item).click(function () {

                        var nodeID = $(item).attr('nodeid');
                        var nodeName = $.trim($('[id^=datagrid-row][id$=' + nodeID + '] .tree-title').text());

                        if (nodeID.length > 0) {
                            $.ajax({
                                type: 'post',
                                url: project.ActionUrls.MoveUp,
                                data: { id: nodeID },
                                async: false,
                                cache: false,
                                dataType: 'json',
                                success: function (data) {
                                    if (data.Msg) {
                                        if (data.Msg != 'Success') {
                                            project.AlertErrorMessage('錯誤', data.Msg);
                                        }
                                        else {
                                            project.ShowMessage('訊息', String.format('Orden de menu modificado.', nodeName));
                                            $('#TreeGrid').treegrid('reload'); //重新載入TreeGrid
                                        }
                                    }
                                    else {
                                        project.AlertErrorMessage('錯誤', 'Error de procesamiento');
                                    }
                                },
                                error: function () {
                                    project.AlertErrorMessage('錯誤', 'Error');
                                }
                            });
                        }
                    });
                });
            },

            MoveDown: function() {
                //<summary>移動節點：下移</summary>

                $('.MoveDown').each(function (i, item) {
                    $(item).click(function () {

                        var nodeID = $(item).attr('nodeId');
                        var nodeName = $.trim($('[id^=datagrid-row][id$=' + nodeID + '] .tree-title').text());

                        if (nodeID.length > 0) {
                            $.ajax({
                                type: 'post',
                                url: project.ActionUrls.MoveDown,
                                data: { id: nodeID },
                                async: false,
                                cache: false,
                                dataType: 'json',
                                success: function (data) {
                                    if (data.Msg) {
                                        if (data.Msg != 'Success') {
                                            project.AlertErrorMessage('錯誤', data.Msg);
                                        }
                                        else {
                                            project.ShowMessage('訊息', String.format('Orden de menu modificado.', nodeName));
                                            $('#TreeGrid').treegrid('reload');  //重新載入TreeGrid
                                        }
                                    }
                                    else {
                                        project.AlertErrorMessage('錯誤', 'Error de procesamiento.');
                                    }
                                },
                                error: function () {
                                    project.AlertErrorMessage('錯誤', 'Error');
                                }
                            });
                        }
                    });
                });
            }
        
        });
})
(project);