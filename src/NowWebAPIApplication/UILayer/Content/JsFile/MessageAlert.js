
$.msg = function (text, style) {
    style = style || 'notice';           //<== default style if it's not set

    //create message and show it
    $('<div>')
        .attr('class', style)
        .css({ "text-align": "center", "font-size": "20px", "color": "#ffff", "padding": "10px" })
        .html(text)
        .fadeIn('fast')
        .insertBefore($('#container-main'))
        .animate({ opacity: 1.0 }, 4000)
        .fadeOut('slow', function () {
            $(this).remove();
        });
};