
function bsoc() {
    $.ajax({
        type: "POST",
        url: "../Resumo/CallCB", // the URL of the controller action method
        data: null, // optional data
        success: function (result) {
            
            var f = result;
        },
        error: function (status) {
            
            var f = status;

            //$('#mdSu').modal('hide');
            $('body').removeClass('modal-open');
            $("#mdSu").modal('show');
            setTimeout(function () {
                $('.modal-backdrop').remove();
            }, 1000);
        }
    });
}

document.addEventListener("DOMContentLoaded", function (event) {

    $("#draggable").draggable();

    $("#btnEnviar").unbind("click");
    $("#btnEnviar").click(function () {
        bsoc();
        event.stopPropagation();
    });

    carregaMapa();
    CalculaRota();
});