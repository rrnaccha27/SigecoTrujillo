(function ($) {
    function makeTable(target) {
        var state = $(target).data('datagrid');
        var opts = state.options;
        var fields1 = $(target).datagrid('getColumnFields', true);
        var fields2 = $(target).datagrid('getColumnFields', false);
        var rows = $(target).datagrid('getRows');
        if (opts.view.type == 'scrollview') {
            rows = state.data.firstRows;
        }
        var table = ['<table class="datagrid-htable" border=0 cellspacing=0 >'];
        table.push('<thead><tr class="datagrid-header-row" >');
        if (opts.rownumbers) {
            table.push('<th>&nbsp;</th>');
        }
        $.map(fields1.concat(fields2), function (field) {
            var col = $(target).datagrid('getColumnOption', field);
            table.push('<th field="' + field + '">' + col.title + '</th>');
        });
        table.push('</tr></thead>');
        table.push('<tbody>');
        $.map(rows, function (row, index) {
            table.push('<tr>');
            table.push(opts.view.renderRow(target, fields1, true, index, row));
            table.push(opts.view.renderRow(target, fields2, false, index, row));
            table.push('</tr>');
        });
        table.push('</tbody>');
        table.push('</table>');

        var data = $(table.join(''));
        $.map(fields1.concat(fields2), function (field) {
            var col = $(target).datagrid('getColumnOption', field);
            if (col.hidden) {
                data.find('td[field="' + field + '"],th[field="' + field + '"]').hide();
            }
        });
        var w = window.open('');
        w.document.write(data.wrap('<div></div>').parent().html());
    }

 
    /*
    $.extend($.fn.datagrid.methods, {
        toHtml: function (jq) {
            return jq.each(function () {
                makeTable(this);
            });
        }
    })
    */

    $.extend($.fn.datagrid.methods, {

        toHtml: function (jq, style) {
            return jq.each(function () {
                var format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) }
                var template = '<html ><head><style>{style}</style></head><body><table  >{table}</table></body></html>'
                var alink = $('<a style="display:none"></a>').appendTo('body');
                var view = $(this).datagrid('getPanel').find('div.datagrid-view');
                var table = view.find('div.datagrid-view2 table.datagrid-btable').clone();
                var theader = view.find('.datagrid-header-row').clone();
                var tbody = table.find('>tbody');
                table.prepend(theader);
                view.find('div.datagrid-view1 table.datagrid-btable>tbody>tr').each(function (index) {
                    $(this).clone().children().prependTo(tbody.children('tr:eq(' + index + ')'));
                });

                var style = style || " table {border:0px solid #aaa;  }  td {white-space: nowrap;border:1px solid #ddd; font-family:Tahoma;}  .datagrid-header-row { background-color:#eee; } "
                var lista = { style: style, table: table.html() }
                var dd = format(template, lista)
                var w = window.open('');
                w.document.write(dd);

            })
        }
    });



})(jQuery);




