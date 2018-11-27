document.addEventListener("DOMContentLoaded", function (event) {

	/*VALORES INICIAIS */
	$("#tabs").tabs({ disabled: [1, 2] });

    /*MASCARAS*/
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
