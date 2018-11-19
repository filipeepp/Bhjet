function GetOptionsViaCep(arrayEndereco) {
    var options = "";
    return options = {
        onComplete: function (cep) {
            //Nova variável "cep" somente com dígitos.
            var cep = arrayEndereco.Cep.val().replace(/\D/g, '');
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
                            arrayEndereco.Rua.val(dados.logradouro);
                            arrayEndereco.Bairro.val(dados.bairro);
                            arrayEndereco.Cidade.val(dados.localidade);
                            arrayEndereco.Estado.val(dados.uf);
                            arrayEndereco.NumeroEndereco.focus();

                        } //end if.
                        else {
                            limpaEndereco();
                        }
                    });
                } //end if.
            } //end if.
        },
    };
}

function limpaEndereco() {
    // Limpa valores do formulário de cep.
    $("#Rua").val("");
    $("#Bairro").val("");
    $("#Cidade").val("");
    $("#UF").val("");
}

