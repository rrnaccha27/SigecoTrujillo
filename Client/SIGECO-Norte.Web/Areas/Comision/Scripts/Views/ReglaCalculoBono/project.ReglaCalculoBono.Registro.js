var RegistrarUrls = { };
var JsonCanal = {};
var JsonGrupo = {};
var Registro = {};
var LongitudGrupo = 0;
var MontoGlobal = 0;
var PorcentajePagoGlobal = 0;
var CodigoArticuloBusqueda = '';

;(function (app) {
    //===========================================================================================
    var current = app.ReglaCalculoBonoRegistro = {};
    //===========================================================================================

    jQuery.extend(app.ReglaCalculoBonoRegistro,
        {
            EditType: '',
            HasRootNode: 'False',

            Initialize: function (registrarUrls) {
                //$(window).resize(function () {
                //    current.Redimensionar();
                //});

                jQuery.extend(RegistrarUrls, registrarUrls);

                //current.Redimensionar();
                InicializarControles();
                InicializarListadoMontos();
                InicializarListadoArticulos();
                GetArticulosJson();
                GetMatrizJson();

                $('#dlgArticulo #valor').textbox({
                    onClickButton: function () {
                        var valor = $.trim($('#dlgArticulo #valor').textbox('getText'));
                        current.GetArticulosBusquedaJson(valor);
                    }
                });

                $('#dlgArticulo #btnCancelar').click(function () {
                    $('#dlgArticulo').dialog('close');
                    $('#dlgArticulo #dataArticulo').datagrid('clearSelections');
                });

                $('#dlgArticulo #btnSeleccionar').click(function () {
                    current.SeleccionarArticulo();
                });

                $('#dlgArticulo #dataArticulo').datagrid({
                    //shrinkToFit: false,
                    data: null,
                    fitColumns: true,
                    idField: 'codigo_articulo',
                    singleSelect: true,
                    columns:
                    [[
                        { field: 'nombre', title: 'Nombre', width: 100, align: 'left' },
                        { field: 'abreviatura', title: 'Abreviatura', width: 50, align: 'left' }
                    ]]
                    , onDblClickRow: function (index, row) {
                        SeleccionarArticulo();
                    }
                });

                $('#fechaInicio').datebox({

                    formatter: function (date) {
                        var y = date.getFullYear();
                        var m = date.getMonth() + 1;
                        var d = date.getDate();
                        return (d < 10 ? ('0' + d) : d) + '/' + (m < 10 ? ('0' + m) : m) + '/' + y;
                    },
                    parser: function (s) {

                        if (!s) return new Date();
                        var ss = s.split('/');
                        var y = parseInt(ss[2], 10);
                        var m = parseInt(ss[1], 10);
                        var d = parseInt(ss[0], 10);
                        if (!isNaN(y) && !isNaN(m) && !isNaN(d)) {
                            return new Date(y, m - 1, d)
                        } else {
                            return new Date();
                        }
                    }

                });

                $('#fechaFin').datebox({

                    formatter: function (date) {
                        var y = date.getFullYear();
                        var m = date.getMonth() + 1;
                        var d = date.getDate();
                        return (d < 10 ? ('0' + d) : d) + '/' + (m < 10 ? ('0' + m) : m) + '/' + y;
                    },
                    parser: function (s) {

                        if (!s) return new Date();
                        var ss = s.split('/');
                        var y = parseInt(ss[2], 10);
                        var m = parseInt(ss[1], 10);
                        var d = parseInt(ss[0], 10);
                        if (!isNaN(y) && !isNaN(m) && !isNaN(d)) {
                            return new Date(y, m - 1, d)
                        } else {
                            return new Date();
                        }
                    }

                });


            },

            GetArticulosBusquedaJson: function (valor) {
                $.ajax({
                    type: 'post',
                    url: RegistrarUrls.GetArticulosBusquedaJson,
                    data: { valor: valor },
                    async: false,
                    cache: false,
                    dataType: 'json',
                    success: function (data) {
                        $('#dlgArticulo #dataArticulo').datagrid({
                            data: data
                        });
                    },
                    error: function () {
                        project.AlertErrorMessage('Error', 'Error');
                    }
                });

            },



        })
})(project);


function InicializarControles()
{
    $('#cmb_tipo_planilla').combobox({
        valueField: 'id',
        textField: 'text',
        url: RegistrarUrls.GetTipoPlanillaJson
    });

	JsonCanal = $('.content').get_json_combobox({
	    url: RegistrarUrls.GetCanalGrupoJson,
        parametro: "?es_canal_grupo=1"
	});

	JsonGrupo = $('.content').get_json_combobox({
	    url: RegistrarUrls.GetCanalGrupoJson,
	    parametro: "?es_canal_grupo=0"
	});
    
	$('#codigo_grupo').combobox({
	    valueField: 'codigo_canal_grupo',
	    textField: 'nombre',
	});

	$('#codigo_canal').combobox({
	    valueField: 'codigo_canal_grupo',
	    textField: 'nombre',
	    data: JsonCanal,
	    required: true,
	    onSelect: function (row) {
	        setTimeout(function () {
	            //console.log('1');
	            var dataGrupo = grupoReload(row.codigo_canal_grupo);
	            $('#codigo_grupo').combobox('clear').combobox('loadData', dataGrupo);
	            LongitudGrupo = dataGrupo.length;

	            if (RegistrarUrls._codigo_canal == row.codigo_canal_grupo) {
	                if (RegistrarUrls._codigo_grupo) {
	                    //console.log('2');
	                    //$('#codigo_grupo').combobox('reset');
	                    $('#codigo_grupo').combobox('setValue', RegistrarUrls._codigo_grupo);

	                    //$('#codigo_grupo').combobox('validate');
	                }
	            }
	        }, 0);
	    }
	});
	


}

function grupoReload(value) {
    grupoFiltrado = JsonGrupo.filter(function (el) {
        return el.codigo_padre == value;
    });

    return grupoFiltrado;
}

var codigo_grupo_original = '';
var codigo_canal_original = '';
var longitud_grupo = 0;

function InicializarListadoMontos () {

    $('#dgMontos').datagrid({
        idField: 'codigo_registro',
        toolbar: '#tbMonto',
        fitColumns: true,
        data: null,
        pagination: false,
        singleSelect: true,
        pageNumber: 0,
        pageList: [5, 10, 15, 20, 25, 30],
        pageSize: 10,
        columns:
        [[
            { field: 'codigo_registro', hidden: 'true' },
            {
                field: 'monto_meta', title: 'Monto Meta', width: 80, align: 'right', halign: 'center',
                formatter: function (value, row) {
                    return $.NumberFormat(value, 2);
                }
            },
            {
                field: 'porcentaje_meta', title: 'Porcentaje Meta', width: 80, align: 'right',halign: 'center',
                editor: {
                    type: 'numberbox',
                    options: {
                        precision: 2,
                        required: true,
                        max: 99
                    }
                }
            },
            {
                field: 'porcentaje_pago', title: 'Porcentaje a Pagar', width: 100, align: 'right',halign: 'center',
                editor: {
                    type: 'numberbox',
                    options: {
                        precision: 2,
                        required: true,
                        max: 99
                    }
                }
            }
        ]],

        onBeforeEdit: function (index, row) {
            row.editing = true;
        },
        onAfterEdit: function (index, row) {
            row.editing = false;
        },
        onCancelEdit: function (index, row) {
            row.editing = false;
        },
        //onDblClickRow: function (rowIndex, rowdata) {
        //    if (!existeRegistroEnEdicion("dgMontos")) {
        //        $('#dgMontos').datagrid('beginEdit', rowIndex);
        //        rowdata.editing = true;
        //    }
        //}
    });
}

function InicializarListadoArticulos() {

    $('#dgArticulos').datagrid({
        idField: 'codigo_articulo',
        toolbar: '#tbArticulos',
        fitColumns: true,
        data: null,
        pagination: false,
        singleSelect: true,
        pageNumber: 0,
        pageList: [5, 10, 15, 20, 25, 30],
        pageSize: 10,
        columns:
        [[
            { field: 'codigo_articulo', hidden: 'true' },
            {
                field: 'nombre_articulo', title: 'Artículo', width: 80, align: 'left', halign: 'center',
            },
            {
                field: 'cantidad', title: 'Cantidad', width: 80, align: 'right', halign: 'center',
            }
        ]],

        onBeforeEdit: function (index, row) {
            row.editing = true;
        },
        onAfterEdit: function (index, row) {
            row.editing = false;
        },
        onCancelEdit: function (index, row) {
            row.editing = false;
        },
        onDblClickRow: function (rowIndex, rowdata) {
            rowdata.editing = false;
        }
    });
}


function GetArticulosJson() {
    var codigo_regla_calculo_bono = RegistrarUrls.codigo_regla_calculo_bono;

    if (parseInt(codigo_regla_calculo_bono) == -1)
    {
        return false;
    }

    $.ajax({
        type: 'post',
        url: RegistrarUrls.GetArticulosJson,
        data: { codigo_regla_calculo_bono: codigo_regla_calculo_bono },
        async: false,
        cache: false,
        dataType: 'json',
        success: function (data) {
            if (data.Msg) {
                project.AlertErrorMessage('Error', data.Msg);
            }
            else {
                $('#dgArticulos').datagrid('loadData', data);
            }
        },
        error: function () {
            project.AlertErrorMessage('Error', 'Error');
        }
    });
}

function GetMatrizJson() {
    var codigo_regla_calculo_bono = RegistrarUrls.codigo_regla_calculo_bono;

    if (parseInt(codigo_regla_calculo_bono) == -1) {
        return false;
    }

    $.ajax({
        type: 'post',
        url: RegistrarUrls.GetMatrizJson,
        data: { codigo_regla_calculo_bono: codigo_regla_calculo_bono },
        async: false,
        cache: false,
        dataType: 'json',
        success: function (data) {
            if (data.Msg) {
                project.AlertErrorMessage('Error', data.Msg);
            }
            else {
                $('#dgMontos').datagrid('loadData', data);
            }
        },
        error: function () {
            project.AlertErrorMessage('Error', 'Error');
        }
    });
}

function GuardarRegla() {
    var esNuevo = RegistrarUrls.codigo_regla_calculo_bono == -1? true:false;
    var message = '';
    var codigo_regla_calculo_bono = RegistrarUrls.codigo_regla_calculo_bono;

    if (!$("#frmRegistro").form('enableValidation').form('validate')) {
        return false;
    }

    var calcular_igv = $('#calcular_igv').switchbutton('options').checked;

    var codigo_grupo = $.trim($('#codigo_grupo').combobox('getValue'));

    //if (!codigo_grupo && LongitudGrupo > 0)
    //{
    //    $.messager.alert("Regla Calculo Bono", "Debe seleccionar un Grupo.", "warning");
    //    return false;
    //}

    if (!codigo_grupo)
    {
        codigo_grupo = 0;
    }

    var vigencia_inicio = $.trim($('#fechaInicio').textbox('getText'));
    var vigencia_fin = $.trim($('#fechaFin').textbox('getText'));

    if (!ValidarFecha(vigencia_inicio))
    {
        $.messager.alert("Regla Calculo Bono", "Inicio de Vigencia en formato incorrecto.", "warning");
        return false;
    }

    if (!ValidarFecha(vigencia_fin)) {
        $.messager.alert("Regla Calculo Bono", "Fin de Vigencia en formato incorrecto.", "warning");
        return false;
    }

    vigencia_inicio = FormatoFecha(vigencia_inicio);
    vigencia_fin = FormatoFecha(vigencia_fin);

    if (parseInt(vigencia_inicio) > parseInt(vigencia_fin))
    {
        $.messager.alert("Regla Calculo Bono", "Inicio de Vigencia debe ser menor a Fin Vigencia.", "warning");
        return false;
    }

    var monto_meta = $('#monto_meta_100').numberbox('getValue');
    if (!monto_meta || parseFloat(monto_meta) <= 0) {
        $.messager.alert("Regla Calculo Bono", "No ha ingresado el monto de la Meta (al 100%).", "warning");
        return false;
    }
    monto_meta = parseFloat(monto_meta);

    var porcentaje_pago = $('#porcentaje_pago_100').numberbox('getValue');
    if (!porcentaje_pago || parseFloat(porcentaje_pago) <= 0) {
        $.messager.alert("Regla Calculo Bono", "No ha ingresado el porcentaje a pagar (al 100%).", "warning");
        return false;
    }
    porcentaje_pago = parseFloat(porcentaje_pago);

    var monto_tope = $('#monto_tope_100').numberbox('getValue');
    if (!monto_tope || parseFloat(monto_tope) <= 0) {
        $.messager.alert("Regla Calculo Bono", "No ha ingresado el tope a pagar (al 100%).", "warning");
        return false;
    }
    monto_tope = parseFloat(monto_tope);

    if (monto_tope >= monto_meta) {
        $.messager.alert("Regla Calculo Bono", "El tope no puede ser mayor o igual al monto de la Meta.", "warning");
        return false;
    }

    var rows = $("#dgMontos").datagrid("getRows");
    var lst_montos = [];

    $.each(rows, function (index, data) {
        var monto = {
            codigo_regla_calculo_bono: codigo_regla_calculo_bono,
            monto_meta: parseFloat(data.monto_meta),
            porcentaje_meta: parseFloat(data.porcentaje_meta),
            porcentaje_pago: parseFloat(data.porcentaje_pago)
        };
        lst_montos.push(monto);
    });

    var rows = $("#dgArticulos").datagrid("getRows");
    var lst_articulos = [];

    $.each(rows, function (index, data) {
        var articulo = {
            codigo_regla_calculo_bono: codigo_regla_calculo_bono,
            codigo_articulo: data.codigo_articulo,
            cantidad: data.cantidad
        };
        lst_articulos.push(articulo);
    });

    var datosReglaBono = {
        codigo_regla_calculo_bono: codigo_regla_calculo_bono,
        codigo_tipo_planilla: $('#cmb_tipo_planilla').combobox('getValue'),
        codigo_canal: $.trim($('#codigo_canal').combobox('getValue')),
        codigo_grupo: codigo_grupo,
        vigencia_inicio: vigencia_inicio,
        vigencia_fin: vigencia_fin,
        monto_meta: monto_meta,
        porcentaje_pago: porcentaje_pago,
        monto_tope: monto_tope,
        cantidad_ventas: $.trim($('#cantidad_ventas').numberbox('getValue')),
        calcular_igv: calcular_igv,
        lista_matriz: lst_montos,
        lista_articulo: lst_articulos
    };

    if (message.length > 0) {
        $.messager.alert((esNuevo ? 'Registro' : 'Modificar'), message, 'warning');
        return false;
    }

    $.messager.confirm('Confirm', '&iquest;Seguro que desea guardar?', function (result) {
        if (result) {
            $.ajax({
                type: 'post',
                url: RegistrarUrls.Guardar,
                data: JSON.stringify({ regla: datosReglaBono }),
                dataType: 'json',
                cache: false,
                async: false,
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    if (data.Msg) {
                        if (data.Msg != 'Success') {
                            //project.AlertErrorMessage('Error', data.Msg);
                            $.messager.alert("Registro", data.Msg, "error");
                        }
                        else {
                            $('#dlgRegistro').dialog('close');
                            project.ShowMessage('Alerta', (esNuevo?'Registro':'Modificado') + ' Exitoso');
                            //$('#DataGrid').datagrid('clearSelections');
                            GetAllJson();
                            //current.EditType = '';
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

function Cancelar() {
    $('#dlgRegistro').dialog('close');
}

function NuevoRegistro() {
    if (!$("#frmRegistro").form('enableValidation').form('validate'))
        return;

    MontoGlobal = $('#monto_meta_100').numberbox('getValue');

    if (!MontoGlobal || parseFloat(MontoGlobal) <= 0) {
        $.messager.alert("Montos", "No ha ingresado el monto de la Meta al 100%.", "warning");
        return false;
    }

    var porcentaje_pago = $('#porcentaje_pago_100').numberbox('getValue');
    if (!porcentaje_pago || parseFloat(porcentaje_pago) <= 0) {
        $.messager.alert("Montos", "No ha ingresado el porcentaje a pagar (al 100%).", "warning");
        return false;
    }

    if (!existeRegistroEnEdicion('dgMontos')) {
        var new_row = {
            "codigo_registro": -new Date().toString("ddHHmmss"),
            "monto_meta": 0.00,
            "porcentaje_meta": 0.00,
            "porcentaje_pago": 0.00,
        };

        $("#dgMontos").datagrid("appendRow", new_row);
        var editIndex = $("#dgMontos").datagrid("getRowIndex", new_row.codigo_registro);
        $("#dgMontos").datagrid("selectRow", editIndex);
        $("#dgMontos").datagrid("beginEdit", editIndex);
    }
}

function AceptarRegistro() {
    MontoGlobal = $('#monto_meta_100').numberbox('getValue');

    if (!MontoGlobal || parseFloat(MontoGlobal) <= 0) {
        $.messager.alert("Montos", "No ha ingresado el monto de la Meta al 100%.", "warning");
        return false;
    }

    var porcentaje_pago = $('#porcentaje_pago_100').numberbox('getValue');
    if (!porcentaje_pago || parseFloat(porcentaje_pago) <= 0) {
        $.messager.alert("Montos", "No ha ingresado el porcentaje a pagar (al 100%).", "warning");
        return false;
    }
    PorcentajePagoGlobal = parseFloat(porcentaje_pago);

    if (existeRegistroEnEdicionParaGrabar("dgMontos")) {
        var rowIndex = IndexRowEditing("dgMontos");
        var rowdata = $('#dgMontos').datagrid("getRows")[rowIndex];
        if (validarRowGrilla(rowIndex, 'dgMontos')) {
            if (ValidarDataConsistente('dgMontos', rowIndex)) {
                rowdata.confirmado = true;
                $('#dgMontos').datagrid('endEdit', rowIndex);
                $('#dgMontos').datagrid('updateRow', {
                    index: rowIndex,
                    row: {
                        monto_meta: parseFloat(MontoGlobal * (rowdata.porcentaje_meta / 100))
                    }
                });
            }
        } 
    }
}

function CancelarRegistro() {
    var rowIndex = IndexRowEditing('dgMontos');
    if (rowIndex != null) {
        $('#dgMontos').datagrid('cancelEdit', rowIndex);
        var rowdata = $('#dgMontos').datagrid("getRows")[rowIndex];
        if (!rowdata.confirmado && rowdata.codigo_registro < 0){
            $('#dgMontos').datagrid('deleteRow', rowIndex);
        }
    }
}

var canalesEliminados = '';
function EliminarRegistro() {
    if (!existeRegistroEnEdicion("dgMontos")) {
        var rows = $("#dgMontos").datagrid("getSelections");
        if (rows.length <= 0) {
            $.messager.alert("Eliminar", "Seleccione un registro.", "warning");
        }
        else {
            var row = $("#dgMontos").datagrid("getSelected");
            $(".messager-window,.window-shadow,.window-mask").remove();

            if (row) {
                $.messager.confirm('Confirmar', '&iquest;Est&aacute; seguro de eliminar este registro?', function (r) {
                    if (r) {
                        var index = $("#dgMontos").datagrid("getRowIndex", row.codigo_registro);
                        $('#dgMontos').datagrid('deleteRow', index);
                        //if (row.codigo_registro > 0)
                        //{ canalesEliminados += row.codigo_registro + '|';}
                    }
                });
            }
        }
    }
}

function BuscarArticulo()
{
    $('#dlgArticulo').dialog('open').dialog('setTitle', 'BUSCAR ARTICULO');
    $('#frmArticulo').form('clear');
    $('#dlgArticulo #dataArticulo').datagrid('loadData', { "total": 0, "rows": [] });
    $('#dlgArticulo #dataArticulo').datagrid('clearSelections');
}

function AgregarArticulo()
{
    cantidad_articulo = $('#cantidad_articulo').numberbox('getValue'); 

    if (!CodigoArticuloBusqueda)
    {
        $.messager.alert("Articulo", "No ha seleccionado un Articulo.", "warning");
        return false;
    }

    if (!cantidad_articulo || parseInt(cantidad_articulo) <= 0)
    {
        $.messager.alert("Articulo", "No ha ingresado una cantidad correcta.", "warning");
        return false;
    }

    $('#dgArticulos').datagrid('appendRow', {
        codigo_articulo: CodigoArticuloBusqueda,
        nombre_articulo: $('#nombre_articulo').textbox('getText'),
        cantidad: cantidad_articulo
    });

    CodigoArticuloBusqueda = '';
    $('#cantidad_articulo').numberbox('setValue', '0');
    $('#nombre_articulo').textbox('setText', '');
}

function QuitarArticulo() {
    if (!existeRegistroEnEdicion("dgArticulos")) {
        var rows = $("#dgArticulos").datagrid("getSelections");
        if (rows.length <= 0) {
            $.messager.alert("Eliminar", "Seleccione un registro.", "warning");
        }
        else {
            var row = $("#dgArticulos").datagrid("getSelected");
            $(".messager-window,.window-shadow,.window-mask").remove();

            if (row) {
                $.messager.confirm('Confirmar', '&iquest;Est&aacute; seguro de eliminar este registro?', function (r) {
                    if (r) {
                        var index = $("#dgArticulos").datagrid("getRowIndex", row.codigo_articulo);
                        $('#dgArticulos').datagrid('deleteRow', index);
                        //if (row.codigo_registro > 0)
                        //{ canalesEliminados += row.codigo_registro + '|';}
                    }
                });
            }
        }
    }
}


function existeRegistroEnEdicion(lista) {
    var editando = false;
    var rows = $("#" + lista).datagrid("getRows");

    $.each(rows, function (i, objeto) {
        if (objeto.editing) {
            editando = true;
            return;
        }
    });
    if (editando) {
        $.messager.alert("Registro en edici&oacute;n", "Existe un registro en edici&oacute;n, para continuar guarde o cancele la edici&oacuten.", "warning");
    }
    return editando;
}

function existeRegistroEnEdicionParaGrabar(lista) {
    var editando = false;
    var rows = $("#" + lista).datagrid("getRows");
    $.each(rows, function (i, objeto) {
        if (objeto.editing) {
            editando = true;
            return;
        }
    });
    if (!editando) {
        $.messager.alert("Registro en edici&oacute;n", "No existe registro en edici&oacute;n para  guardar.", "warning");
    }

    return editando;
}

function IndexRowEditing(lista) {
    var index = null;
    var rows = $("#" + lista).datagrid("getRows");
    $.each(rows, function (i, objeto) {
        if (objeto.editing) {
            index = i;
            return false;
        }
    });
    return index;
}

function ValidarDataConsistente(lista, indiceEditado) {
    var resultado = true;
    var rows = $("#" + lista).datagrid("getRows");

    debugger;

    var ed_porcentaje_meta = $("#dgMontos").datagrid("getEditor", { index: indiceEditado, field: 'porcentaje_meta' });
    var ed_porcentaje_pago = $("#dgMontos").datagrid("getEditor", { index: indiceEditado, field: 'porcentaje_pago' });
    var porcentaje_meta = ed_porcentaje_meta.target.numberbox("getValue");
    var porcentaje_pago = ed_porcentaje_pago.target.numberbox("getValue");

    $.each(rows, function (i, objeto) {
        if (i != indiceEditado) {
            if (parseFloat(objeto.porcentaje_meta) == parseFloat(porcentaje_meta) || parseFloat(objeto.porcentaje_pago) == parseFloat(porcentaje_pago)) {
                $.messager.alert("Meta", "Ya existe un porcentaje de meta o porcentaje de pago del mismo valor.", "warning");
                resultado = false;
                return false;
            }
        }
    });

    if (resultado) {
        if (parseFloat(porcentaje_meta) == 0 || parseFloat(porcentaje_pago) == 0) {
            $.messager.alert("Meta", "El porcentaje de meta o porcentaje de pago no pude ser 0.", "warning");
            resultado = false;
        }
    }

    if (resultado) {
        if (parseFloat(porcentaje_pago) >= parseFloat(PorcentajePagoGlobal)) {
            $.messager.alert("Meta", "El porcentaje de pago debe ser menor que el porcentaje al 100%.", "warning");
            resultado = false;
        }
    }

    return resultado;
}

function validarRowGrilla(index, lista) {
    var _Valid = $('#' + lista).datagrid('validateRow', index);
    if (!_Valid) {
        $.messager.alert("Campos Obligatorios", "Falta ingresar los campos requeridos de la grilla en edici&oacute;n.", "warning");
    }
    return _Valid;
}

function ValidarFecha(fecha)
{
    try {
        testdate = $.datepicker.parseDate('dd/mm/yy', fecha);
        return true;
    } catch (e) {
        
        return false;
    }
}

function FormatoFecha(fecha) {
    return fecha.substring(6, 10) + fecha.substring(3, 5) + fecha.substring(0, 2);
}

function SeleccionarArticulo()
{
    var row = $('#dlgArticulo #dataArticulo').datagrid('getSelected');

    if (row == null) {
        project.AlertErrorMessage('Alerta', 'Necesita seleccionar un artículo', 'info');
        return;
    }

    CodigoArticuloBusqueda = row['codigo_articulo'];
    $('#nombre_articulo').textbox('setText', row['nombre']);
    $('#dlgArticulo').dialog('close');
}