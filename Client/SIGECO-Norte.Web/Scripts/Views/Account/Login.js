$(document).ready(function () {
    $('#btnIngresar').bind("click", ValidarUsuario);
    $("#txtvUsuario").focus();

});



function ValidarUsuario() {
    
    var usuario = $("#txtvUsuario").val();
    var clave = $("#txtvPassword").val();

    $.ajax({
        url: $("#UrlLogin").val(),
        type: "POST",
        dataType: "json",
        data: { usuario: usuario, clave: clave },
        success: function (data) {

            if (data.Msg != "Success") {
                project.AlertErrorMessage('Alerta', data.Msg);
            } else {

                alert(data.url_pagina);
                var url = '@Url.Action("Home", "Index")';
                var asd = '@Url.Action("GetRegistrosJSON", "Agencia")'
                alert("url: " + url);
                window.location.href = url;

            }            
        }
    });
    

}