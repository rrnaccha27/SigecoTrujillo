$(document).ready(function () {
    /*
    var options = {
        url: '@Url.Content("~/Content/images/img-ocultar")',
        title: 'Informaci&oacute;n de espacio'
    };
    SIGEES_SHOW_HIDE(options);*/
});

function MM_swapImgRestore() { //v3.0
    var i, x, a = document.MM_sr; for (i = 0; a && i < a.length && (x = a[i]) && x.oSrc; i++) x.src = x.oSrc;
}
function MM_preloadImages() { //v3.0
    var d = document; if (d.images) {
        if (!d.MM_p) d.MM_p = new Array();
        var i, j = d.MM_p.length, a = MM_preloadImages.arguments; for (i = 0; i < a.length; i++)
            if (a[i].indexOf("#") != 0) { d.MM_p[j] = new Image; d.MM_p[j++].src = a[i]; }
    }
}

function MM_findObj(n, d) { //v4.01
    var p, i, x; if (!d) d = document; if ((p = n.indexOf("?")) > 0 && parent.frames.length) {
        d = parent.frames[n.substring(p + 1)].document; n = n.substring(0, p);
    }
    if (!(x = d[n]) && d.all) x = d.all[n]; for (i = 0; !x && i < d.forms.length; i++) x = d.forms[i][n];
    for (i = 0; !x && d.layers && i < d.layers.length; i++) x = MM_findObj(n, d.layers[i].document);
    if (!x && d.getElementById) x = d.getElementById(n); return x;
}

function MM_swapImage() { //v3.0
    var i, j = 0, x, a = MM_swapImage.arguments; document.MM_sr = new Array; for (i = 0; i < (a.length - 2) ; i += 3)
        if ((x = MM_findObj(a[i])) != null) { document.MM_sr[j++] = x; if (!x.oSrc) x.oSrc = x.src; x.src = a[i + 2]; }
}
function mostrarbuscador(div) {
    document.getElementById(div).style.display = 'none';
    document.getElementById(div + '_div_ocultar').style.display = 'none';
    document.getElementById(div + '_div_mostrar').style.display = '';
}

function ocultarbuscador(div) {
    document.getElementById(div).style.display = '';
    document.getElementById(div + '_div_ocultar').style.display = '';
    document.getElementById(div + '_div_mostrar').style.display = 'none';
}

function SIGEES_SHOW_HIDE(options) {

    var number = 1 + Math.floor(Math.random() * 6);
    var img4 = 'Image' + (4 + number);
    var img7 = 'Image' + (7 + number);

    
    var div = document.createElement("div");
    div.className = "bl_ayuda";

    var div_ocultar = document.createElement("div");
    div_ocultar.id =options.div+"_div_ocultar";
    div_ocultar.style.display = "block";
    div_ocultar.style.float = "left";

    var a_o = document.createElement('a');
    a_o.setAttribute('href', "javascript:mostrarbuscador('" + options.div + "');");
    a_o.setAttribute('onmouseout', "MM_swapImgRestore();");
    a_o.setAttribute('onmouseover', "MM_swapImage('" + img4 + "','\'\,'" + options.url + '/icn_ocultar_on.gif' + "',1);");

    var img_o = document.createElement('img');
    img_o.src = options.url + '/icn_ocultar_off.gif';
    img_o.width = '21';
    img_o.height = '10';
    img_o.border = '0';
    img_o.id = img4;
    img_o.alt = 'Ocultar';
    a_o.innerHTML = "Ocultar";
    a_o.appendChild(img_o);
    div_ocultar.appendChild(a_o);
    div.appendChild(div_ocultar);

    /******************************/
    var div_mostrar = document.createElement("div");
    div_mostrar.id = options.div + "_div_mostrar";
    div_mostrar.style.display = "none";
    div_mostrar.style.float = "left";

    var a_m = document.createElement('a');
    a_m.setAttribute('href', "javascript:ocultarbuscador('" + options.div + "');");
    a_m.setAttribute('onmouseout', "MM_swapImgRestore();");
    a_m.setAttribute('onmouseover', "MM_swapImage('" + img7 + "','\'\,'" + options.url + '/icn_mostrar_on.gif' + "',1);");

    var img_m = document.createElement('img');
    img_m.src = options.url + '/icn_mostrar_off.gif';
    img_m.width = '21';
    img_m.height = '10';
    img_m.border = '0';
    img_m.id = img7;
    img_m.alt = 'Mostrar';


    a_m.innerHTML = "Mostrar";
    a_m.appendChild(img_m);
    div_mostrar.appendChild(a_m);

    div.appendChild(div_mostrar);


    $("#"+options.div_padre).html(div);
    //console.log(div);
}