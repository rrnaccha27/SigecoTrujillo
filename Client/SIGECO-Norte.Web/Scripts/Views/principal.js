jQuery(document).ready(function () {
    
    $.ajax({
        type: 'post',
        url: $("#GetMenuByPerfilJSON").val(),
        async: false,
        cache: false,
        dataType: 'json',
        success: function (data) {
            $('#MenuPrincipal').tree({
                data: data,
                onClick: function (node) {
                    
                    if (node.ruta.length > 0) {
                        ActualizarSesion(node.id);

                        var splitRuta = node.ruta.split("/");
                        var controlador = splitRuta[0];
                        var metodo = splitRuta[1];
                        var url = '@Url.Action(' + metodo + ', ' + controlador + ')';

                        alert("url: " + url);

                        //window.location.href = "/" + node.ruta;
                        window.location.href = url;
                        /*
                        $.get("Index").done(function (result) {
                            $("#cuerpo").html(result);
                        });*/
                    }
 
                }
            });

        },
        error: function () {
            project.AlertErrorMessage('Error', 'Error');
        }
    });

});

function ActualizarSesion(codigoMenu) {
    
    $.ajax({
        url: $("#ActualizarSesion_CodigoMenu").val(),
        data: { codigoMenu: codigoMenu },
        async: false,
        cache: false,
        type: "POST",
        dataType: "json",
        success: function (data) {
            if (data.Msg != "Success") {
                project.AlertErrorMessage('Alerta', data.Msg);
            }
        }
    });


}