document.addEventListener("DOMContentLoaded", function (event) {

    $("#TelefoneResidencial").mask("(00) 0000-0000");
    $("#TelefoneCelular").mask("(00) 0000-00009");

    function limpaEndereco() {
        // Limpa valores do formulário de cep.
        $("#Rua").val("");
        $("#Bairro").val("");
        $("#Cidade").val("");
        $("#UF").val("");
    }

    //CEP
    var endereco = {
        Cep: $("#Cep"),
        Rua: $("#Rua"),
        Bairro: $("#Bairro"),
        Cidade: $("#Cidade"),
        Estado: $("#UF"),
        NumeroEndereco: $("#RuaNumero")
    }
    $('#Cep').mask('00000-000', GetOptionsViaCep(endereco));

});