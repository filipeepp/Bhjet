document.addEventListener("DOMContentLoaded", function (event) {

    $('body').removeClass('modal-open');
    $("#mdSu").modal({ backdrop: 'static', keyboard: false });
    setTimeout(function () {
        $('.modal-backdrop').remove();
    }, 1000);

    $('#CPF').mask('000.000.000-00', { reverse: true });
    $('#DataNascimento').mask('00/00/0000');
    $("#Comercial").mask("(00) 0000-0000");
    $("#Celular").mask("(00) 0000-00009");
    $('#ValidadeCartaoCredito').mask('00/0000');
    $('#NumeroCartaoCredito').mask('0000.0000.0000.0000');

    var endereco = {
        Cep: $("#CEP"),
        Rua: $("#Rua"),
        Bairro: $("#Bairro"),
        Cidade: $("#Cidade"),
        Estado: $("#Estado"),
        NumeroEndereco: $("#Numero")
    }
    $('#CEP').mask('00000-000', GetOptionsViaCep(endereco));


});

