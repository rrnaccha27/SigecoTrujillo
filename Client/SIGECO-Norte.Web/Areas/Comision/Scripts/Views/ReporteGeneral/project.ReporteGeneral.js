;
(function (app) {
    //===========================================================================================
    var current = app.ReporteGeneral = {};
    //===========================================================================================

    jQuery.extend(app.ReporteGeneral,
        {

            ActionUrls: {},
            Identificador: '',
            
            Initialize: function (actionUrls, identificador) {
                jQuery.extend(project.ActionUrls, actionUrls);
                current.Identificador = identificador;

                var patronFecha = /^([0-9]{2})\/([0-9]{2})\/([0-9]{4})$/;

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


                $('#btnBuscar').click(function () {

                    if (current.Identificador == 'rango_fecha') {

                        var fechaInicio = $.trim($('#fechaInicio').textbox('getText'));
                        var fechaFin = $.trim($('#fechaFin').textbox('getText'));

                        var message = '';
                        if (fechaInicio.length == 0) {
                            message = 'Ingrese fecha inicio<br>';
                        }
                        if (fechaFin.length == 0) {
                            message += 'Ingrese fecha fin';
                        }

                        if (message.length == 0) {
                            if (!patronFecha.test(fechaInicio)) {
                                message += 'Formato incorrecto en fecha inicio<br>';
                            }
                            if (!patronFecha.test(fechaFin)) {
                                message += 'Formato incorrecto en fecha fin<br>';
                            }
                        }

                        if (message.length > 0) {
                            $.messager.alert('Alerta', message, 'info');
                        } else {
                            var url = project.ActionUrls.Ver + '?fechaInicio=' + fechaInicio + '&fechaFin=' + fechaFin;
                            document.getElementById('iframeVisualizar').src = url;
                        }
                    }

                });

                $('#btnPDF').click(function () {

                    if (current.Identificador == 'rango_fecha') {

                        var fechaInicio = $.trim($('#fechaInicio').textbox('getText'));
                        var fechaFin = $.trim($('#fechaFin').textbox('getText'));

                        var message = '';
                        if (fechaInicio.length == 0) {
                            message = 'Ingrese fecha inicio<br>';
                        }
                        if (fechaFin.length == 0) {
                            message += 'Ingrese fecha fin';
                        }

                        if (message.length == 0) {
                            if (!patronFecha.test(fechaInicio)) {
                                message += 'Formato incorrecto en fecha inicio<br>';
                            }
                            if (!patronFecha.test(fechaFin)) {
                                message += 'Formato incorrecto en fecha fin<br>';
                            }
                        }

                        if (message.length > 0) {
                            $.messager.alert('Alerta', message, 'info');
                        } else {
                            var url = project.ActionUrls.ExportarPDF
                            window.location.href = url + '?fechaInicio=' + fechaInicio + '&fechaFin=' + fechaFin;
                        }

                    }

                });

                $('#btnEXCEL').click(function () {

                    if (current.Identificador == 'rango_fecha') {

                        var fechaInicio = $.trim($('#fechaInicio').textbox('getText'));
                        var fechaFin = $.trim($('#fechaFin').textbox('getText'));

                        var message = '';
                        if (fechaInicio.length == 0) {
                            message = 'Ingrese fecha inicio<br>';
                        }
                        if (fechaFin.length == 0) {
                            message += 'Ingrese fecha fin';
                        }
                        if (message.length == 0) {
                            if (!patronFecha.test(fechaInicio)) {
                                message += 'Formato incorrecto en fecha inicio<br>';
                            }
                            if (!patronFecha.test(fechaFin)) {
                                message += 'Formato incorrecto en fecha fin<br>';
                            }
                        }

                        if (message.length > 0) {
                            $.messager.alert('Alerta', message, 'info');
                        } else {
                            var url = project.ActionUrls.ExportarEXCEL
                            window.location.href = url + '?fechaInicio=' + fechaInicio + '&fechaFin=' + fechaFin;
                        }

                    }

                });

            },
        
        });

})
(project);