$(document).ready(function () {
	//MASCARAS
	$('.ctmErrorDataNascimento').on("keyup mouseup", function (event) {
		var id = event.target.id;
		$('input[id="' + id + '"]').prop("type", "date");
	});

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

		//if ($(document).find('span[id^="spanError_"]').length === 0) {
			$("#lnkValidarContato").removeClass("isDisabled");
		//}

	});


	//Limpa Erro quando digitado e trata expressões regulares
	$("input[name^='Contato']").keyup(function () {

		$("div[id^='Contato']").each(function () {
			var aux = $(this).children('input');
		
			if (aux[0].value) {
				$(this).find("span[id^='spanError_']").remove();

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

				} else if (aux.hasClass("ctmErrorDataNascimento")) {
					if (aux[0].value.length > 10) {
						aux[0].value = "";
						aux[0].blur();
						$(this).append('<span id="spanError_" >' + aberturaSpan + "Data inválida" + fechamentoSpan);
					}
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

            $("#spanError_" + aux[0].id).remove();
			//Contato
            aux.hasClass("ctmErrorContato") ? $(this).append('<span id="spanError_' + aux[0].id + '" >' + aberturaSpan + 'Nome do Contato obrigatório.' + fechamentoSpan) : "";
			//Email
			aux.hasClass("ctmErrorEmail") ? $(this).append('<span id="spanError_' + aux[0].id + '" >' + aberturaSpan + "E-mail obrigatório." + fechamentoSpan) : "";
			//Telefone Comercial
			aux.hasClass("ctmErrorTelefoneComercial") ? $(this).append('<span id="spanError_' + aux[0].id + '" >' + aberturaSpan + "Telefone Comercial obrigatório." + fechamentoSpan) : "";
			//Telefone Celular
			//aux.hasClass("ctmErrorTelefoneCelular") ? $(this).append('<span id="spanError_' + aux[0].id + '" >' + aberturaSpan + "Telefone Celular obrigatório." + fechamentoSpan) : "";
			//Setor
			aux.hasClass("ctmErrorSetor") ? $(this).append('<span id="spanError_' + aux[0].id + '" >' + aberturaSpan + "Setor é obrigatório." + fechamentoSpan) : "";
			//Data Nascimento
			//aux.hasClass("ctmErrorDataNascimento") ? $(this).append('<span id="spanError_' + aux[0].id + '" >' + aberturaSpan + "A Data de Nascimento é obrigatória" + fechamentoSpan) : ""

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

	//Adiciona classe para identificar que o contato foi removido
	$("#" + id).addClass("div-contato-removido");

	//Remove span de erro
	$('span', '#'+id).each(function () {
		$(this).remove();
	});

	//Adiciona true para model ContatoRemovido
	$("#" + id).find("input[id$='ContatoRemovido']").attr("value", true);

	//Exclui o bloco
	$("#" + id).hide();

	return true;

}

window.ExcluirContato = function (divBlocoContato) {

	var idBloco = divBlocoContato.id;
	var idContato = $("#" + idBloco).find("input[id$='ID']").val();

	var alertConfirmacao = window.confirm("Tem certeza que deseja excluir esse contato da base?");

	if (alertConfirmacao) {
		$.ajax({
			url: '../Clientes/ExcluirContato?idContato=' + idContato,
			type: "POST",
			success: function () {

				//var idCliente = $("input[id='ID']").val();
				RemoverBlocoContato(divBlocoContato);
				$("html, body").animate({ scrollTop: $(document).height() }, 1000);
				//window.location.href = "/Clientes/NovoCliente?edicao=true&clienteID=" + idCliente;
			},
			error: function () {
				var idCliente = $("input[id='ID']").val();
				window.location.href = "/Clientes/NovoCliente?edicao=true&clienteID=" + idCliente;
				$("html, body").animate({ scrollTop: $(document).height() }, 1000);

			}
		});

	} else {
		event.stopPropagation();
	}

}