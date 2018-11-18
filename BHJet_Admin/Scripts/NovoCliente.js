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
    var endereco = {
        Cep : $("#DadosCadastrais_CEP"),
        Rua : $("#DadosCadastrais_Endereco"),
        Bairro : $("#DadosCadastrais_Bairro"),
        Cidade : $("#DadosCadastrais_Cidade"),
        Estado : $("#DadosCadastrais_Estado"),
        NumeroEndereco : $("#DadosCadastrais_NumeroEndereco")
    }
    $('#DadosCadastrais_CEP').mask('00000-000', GetOptionsViaCep(endereco));

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