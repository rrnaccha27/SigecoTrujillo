$(document).ready(function () {

    $('#ribbon').ribbon();
    $("#ribbon .sel").removeClass("sel");
    $(".ribbon-tab").attr('style', 'display:none');

    var tab = $('#ribbon').data("tab");
    tab = (tab == '' || tab == null) ? 'Home' : tab;
    
    var id = $("div .ribbon-button").filter('[data-action="' + tab + '"]').parent().parent().attr("id");

    $("#" + id).attr('style', 'display: block');
    if (id != null) id = id.replace("format-tab", "ribbon-tab-header-");
    $("#" + id).addClass('sel');    
    $('div .ribbon-button').click(function () {
        if (this.isEnabled()) {
            var url = $(this).attr('data-action');
            if (url !== undefined)
                window.location.href = url;
        }
    });
});
