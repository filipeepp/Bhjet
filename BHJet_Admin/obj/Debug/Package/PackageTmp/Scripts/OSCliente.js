document.addEventListener("DOMContentLoaded", function (event) {

    $('label[rel=popover]').popover({
        html: true,
        trigger: 'click',
        placement: 'auto',
        title: '<button type="button" id="close" class="close" onclick="closePopover()">&times;</button></br>',
        content: function () {
            return '<img class="img-responsive" src=' + $(this).data('img') + ' />';
        }
    });


});


function closePopover() {
    $('label[rel=popover]').popover('hide');
}
