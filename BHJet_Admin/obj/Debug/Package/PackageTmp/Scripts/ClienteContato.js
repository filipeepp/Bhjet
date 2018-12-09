$(document).ready(function () {
	//MASCARAS
	$(".mask-telefone").mask("(00) 0000-0000");
	$(".mask-celular").mask("(00) 00009-0000");

	//VARIÁVEIS INICIAIS
	var aberturaSpan = '<span class="text-danger field-validation-error" data-valmsg-replace="true"><span>';
	var fechamentoSpan = '</span></span></span>';

	var reEmail = /^[\w._-]+[+]?[\w._-]+@[\w.-]+\.[a-zA-Z]{2,6}$/;
	var reTelefoneComercial = /^(\([0-9]{2}\))\s([9]{1})?([0-9]{4})-([0-9]{4})$/;
	var reTelefoneCelular = /^(\([0-9]{2}\))\s([9]{1})?([0-9]{4})-([0-9]{4})$/;


	//Limpa erro do campo data quando alterado
	$("input[name$='DataNascimento']").change(function () {

		$("div[id$='DataNascimento']").each(function () {
			var aux = $(this).children('input');
			if (aux[0].value)
				$(this).children('span[id="spanError_' + aux[0].id + '"]').remove();
		});
	
	});

	//Habilita botão avançar
	$(document).on("change keyup mouseup mousemove", function () {

		if ($(document).find('span[id^="spanError_"]').length === 0) {
			$("#lnkValidarContato").removeClass("isDisabled");
		}

	});


	//Limpa Erro quando digitado e trata expressões regulares
	$("input[name^='Contato']").keyup(function () {

		$("div[id^='Contato']").each(function () {
			var aux = $(this).children('input');
		
			if (aux[0].value) {
				$(this).children('span[id="spanError_' + aux[0].id + '"]').remove();

				if (aux.hasClass("ctmErrorEmail")) {

					$(this).append('<span id="spanError_' + aux[0].id + '" >');

					if (!reEmail.test(aux[0].value))
						$(this).children('span[id$="' + aux[0].id + '"]').html(aberturaSpan + 'E-mail inválido.' + fechamentoSpan);
					else
						$(this).children('span[id="spanError_' + aux[0].id + '"]').remove();

				} else if (aux.hasClass("ctmErrorTelefoneComercial")) {

					$(this).append('<span id="spanError_' + aux[0].id + '" >');

					if (!reTelefoneComercial.test(aux[0].value))
						$(this).children('span[id$="' + aux[0].id + '"]').html(aberturaSpan + 'Telefone Comercial inválido. Deve ser fixo.' + fechamentoSpan);
					else
						$(this).children('span[id="spanError_' + aux[0].id + '"]').remove();

				} else if (aux.hasClass("ctmErrorTelefoneCelular")) {

					$(this).append('<span id="spanError_' + aux[0].id + '" >');

					if (!reTelefoneCelular.test(aux[0].value))
						$(this).children('span[id$="' + aux[0].id + '"]').html(aberturaSpan + 'Telefone Celular inválido. Deve conter 9 adicional.' + fechamentoSpan);
					else
						$(this).children('span[id="spanError_' + aux[0].id + '"]').remove();

				}

			}

		});
	});

});

window.ValidarContato = function () {

	var aberturaSpan = '<span class="text-danger field-validation-error" data-valmsg-replace="true"><span>';
	var fechamentoSpan = '</span></span></span>';

	$("div[id^='Contato']").each(function () {
		var aux = $(this).children('input');
		if (!aux[0].value && $(this).closest('.div-contato-removido').length <= 0) {
			//Contato
			aux.hasClass("ctmErrorContato") ? $(this).append('<span id="spanError_' + aux[0].id + '" >' + aberturaSpan + 'Nome do Contato obrigatório.' + fechamentoSpan) : "";
			//Email
			aux.hasClass("ctmErrorEmail") ? $(this).append('<span id="spanError_' + aux[0].id + '" >' + aberturaSpan + "E-mail obrigatório." + fechamentoSpan) : "";
			//Telefone Comercial
			aux.hasClass("ctmErrorTelefoneComercial") ? $(this).append('<span id="spanError_' + aux[0].id + '" >' + aberturaSpan + "Telefone Comercial obrigatório." + fechamentoSpan) : "";
			//Telefone Celular
			aux.hasClass("ctmErrorTelefoneCelular") ? $(this).append('<span id="spanError_' + aux[0].id + '" >' + aberturaSpan + "Telefone Celular obrigatório." + fechamentoSpan) : "";
			//Setor
			aux.hasClass("ctmErrorSetor") ? $(this).append('<span id="spanError_' + aux[0].id + '" >' + aberturaSpan + "Setor é obrigatório." + fechamentoSpan) : "";
			//Data Nascimento
			aux.hasClass("ctmErrorDataNascimento") ? $(this).append('<span id="spanError_' + aux[0].id + '" >' + aberturaSpan + "A Data de Nascimento é obrigatória" + fechamentoSpan) : ""

		}
	});

	//Desabilita botão de avançar caso encontre erro
	if ($(document).find('span[id^="spanError_"]').length > 0) {
		$("#lnkValidarContato").addClass("isDisabled");
		return false;
	}

	return true;


};

window.RemoverBlocoContato = function (divBlocoContato) {
	var id = divBlocoContato.id;

	$("#" + id).addClass("div-contato-removido");

	$('span', '#'+id).each(function () {
		$(this).remove();
	});

	$("#" + id).hide();
	return true;

}