document.addEventListener("DOMContentLoaded", function (event) {
    $("#loading").hide()

    $("#TelefoneResidencial").mask("(00) 0000-0000");
    $("#TelefoneCelular").mask("(00) 0000-00009");

    $("#PeriodoInicial").mask("00/00/0000", {
        onComplete: function (a) {
            
        },
        placeholder: "__/__/____"
    });

    $("#PeriodoFinal").mask("00/00/0000", {
        onComplete: function (a) {
            
        },
        placeholder: "__/__/____"
    });

    $(".mask-valor").maskMoney({
        prefix: "R$:",
        decimal: ",",
        thousands: "."
    });

    var cpfMascara = function (val) {
        return val.replace(/\D/g, '').length > 11 ? '00.000.000/0000-00' : '000.000.000-009';
    },
        cpfOptions = {
            onKeyPress: function (val, e, field, options) {
                field.mask(cpfMascara.apply({}, arguments), options);
            }
        };
    $('#CpfCnpj').mask(cpfMascara, cpfOptions);

    $('#confirmaMotorista').click(function (event) {
        //$("#loading").show()
    });

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