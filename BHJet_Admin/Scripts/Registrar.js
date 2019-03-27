document.addEventListener("DOMContentLoaded", function (event) {

    $('body').removeClass('modal-open');
    $("#mdSu").modal({ backdrop: 'static', keyboard: false });
    setTimeout(function () {
        $('.modal-backdrop').remove();
    }, 1000);

    var endereco = {
        Cep: $("#Cep"),
        Rua: $("#Rua"),
        Bairro: $("#Bairro"),
        Cidade: $("#Cidade"),
        Estado: $("#Pais"),
        NumeroEndereco: $("#Numero")
    }
    $('#Cep').mask('00000-000', GetOptionsViaCep(endereco));


});

