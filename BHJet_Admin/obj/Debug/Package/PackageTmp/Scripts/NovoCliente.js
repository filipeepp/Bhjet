document.addEventListener("DOMContentLoaded", function (event) {

	/*VALORES INICIAIS */
	$("#tabs").tabs({ disabled: [1, 2] });

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
    $("#ValorUnitario").maskMoney({
        prefix: "R$:",
        decimal: ",",
        thousands: "."
	});
	//OBS: RETIRAR MASCARA PARA ENVIO SERVIÇO: $('##Valor_ValorUnitario').maskMoney('unmasked')[0];

    $("#VigenciaInicio").mask("99/99/9999");
    $("#VigenciaFim").mask("99/99/9999");

    
});

window.Validar = function (tab1 = null, tab2 = null, tab3 = null) {

	if ($("form").valid())
		ProximaTab(tab1, tab2, tab3);
}

window.ProximaTab = function (tab1, tab2, tab3) {

	if (tab1 && tab2)
		$("#tabs").tabs({ disabled: [0, 1] });
	else if (tab1 && tab3)
		$("#tabs").tabs({ disabled: [0, 2] });
	else if (tab2 && tab3)
		$("#tabs").tabs({ disabled: [1, 2] });

	var active = $("#tabs").tabs("option", "active");
	$("#tabs").tabs("option", "active", active + 1);
}

window.AnteriorTab = function (tab1, tab2, tab3) {

	if (tab1 && tab2)
		$("#tabs").tabs({ disabled: [0, 1] });
	else if (tab1 && tab3)
		$("#tabs").tabs({ disabled: [0, 2] });
	else if (tab2 && tab3)
		$("#tabs").tabs({ disabled: [1, 2] });

	var active = $("#tabs").tabs("option", "active");
	$("#tabs").tabs("option", "active", active - 1);
}
