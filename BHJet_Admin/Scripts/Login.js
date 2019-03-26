

document.addEventListener("DOMContentLoaded", function (event) {
    $("#loading").hide()
    $('#btnLogar').click(function (event) {
        $("#loading").show()
    });

    $("#lnkRegister").click(function (event) {
        $('body').removeClass('modal-open');
        $("#mdSu").modal('show');
        setTimeout(function () {
            $('.modal-backdrop').remove();
        }, 1000);
    });


});
