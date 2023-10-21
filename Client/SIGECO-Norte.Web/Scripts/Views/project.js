
(function (window) {
    //===========================================================================================
    if (typeof (jQuery) === 'undefined') { alert('jQuery Library NotFound.'); return; }

    var project = window.project =
    {
        AppName: 'project',
        ActionUrls: {}
    };
    //===========================================================================================
    jQuery.extend(project, {

        Initialize: function () {

        },
        
        ShowMessage: function (title, message) {

            $.messager.show({
                title: title,
                msg: message,
                timeout: 4000,
                showType: 'show',
                style:
                {
                    right: 0,
                    left: '',
                    top: document.body.scrollTop + document.documentElement.scrollTop,
                    bottom: ''
                }
            });
        },

        AlertErrorMessage: function (title, message) {

            $.messager.alert(title, message, 'error');
        },
        
        AlertErrorMessage: function (title, message, type) {

            $.messager.alert(title, message, type);
        }

    });
})
(window);