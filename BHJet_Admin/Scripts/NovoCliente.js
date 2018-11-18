document.addEventListener("DOMContentLoaded", function (event) {


    /*DADOS CADASTRAIS*/
        //CPF e CNPJ
    var cpfMascara = function (val) {
        return val.replace(/\D/g, '').length > 11 ? '00.000.000/0000-00' : '000.000.000-009';
    },
        cpfOptions = {
            onKeyPress: function (val, e, field, options) {
                field.mask(cpfMascara.apply({}, arguments), options);
            }
        };
    $('#DadosCadastrais_CPFCNPJ').mask(cpfMascara, cpfOptions);

        //CEP
    var options = {
        onComplete: function (cep) {
            //Nova variável "cep" somente com dígitos.
            var cep = $("#DadosCadastrais_CEP").val().replace(/\D/g, '');
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
                            $("#DadosCadastrais_Endereco").val(dados.logradouro);
                            $("#DadosCadastrais_Bairro").val(dados.bairro);
                            $("#DadosCadastrais_Cidade").val(dados.localidade);
                            $("#DadosCadastrais_Estado").val(dados.uf);
                            $("#DadosCadastrais_NumeroEndereco").focus();

                        } //end if.
                        else {
                            limpaEndereco();
                        }
                    });
                } //end if.
            } //end if.
        },
    };
    $('#DadosCadastrais_CEP').mask('00000-000', options);

    /*CONTATO*/
        //TELEFONE COMERCIAL
    $("#TelefoneComercial").mask("(00) 0000-00009");    
        //TELEFONE CELULAR
    $("#TelefoneCelular").mask("(00) 0000-00009");

    /*VALOR*/
    $("#Valor_ValorUnitario").maskMoney({
        prefix: "R$:",
        decimal: ",",
        thousands: "."
    });
    //OBS: RETIRAR MASCARA PARA ENVIO SERVIÇO: $('##Valor_ValorUnitario').maskMoney('unmasked')[0];


    function limpaEndereco() {
        // Limpa valores do formulário de cep.
        $("#Rua").val("");
        $("#Bairro").val("");
        $("#Cidade").val("");
        $("#UF").val("");
    }

    
});