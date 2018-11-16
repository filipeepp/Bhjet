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

    var options = {
        onComplete: function (cep) {
            //Nova variável "cep" somente com dígitos.
            var cep = $("#Cep").val().replace(/\D/g, '');
            //Verifica se campo cep possui valor informado.
            if (cep != "") {
                //Expressão regular para validar o CEP.
                var validacep = /^[0-9]{8}$/;
                //Valida o formato do CEP.
                if (validacep.test(cep)) {
                    //Consulta o webservice viacep.com.br/
                    $.getJSON("https://viacep.com.br/ws/" + cep + "/json/?callback=?", function (dados) {
                        if (!("erro" in dados)) {
                            //Atualiza os campos com os valores da consulta.
                            $("#Rua").val(dados.logradouro);
                            $("#Bairro").val(dados.bairro);
                            $("#Cidade").val(dados.localidade);
                            $("#UF").val(dados.uf);
                            $("#RuaNumero").focus();
                        } //end if.
                        else {
                            limpaEndereco();
                        }
                    });
                } //end if.
            } //end if.
        },
    };
    $('#Cep').mask('00000-000', options);
});