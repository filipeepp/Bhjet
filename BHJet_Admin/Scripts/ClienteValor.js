$(document).ready(function () {
	//MASCARAS
	$(".mask-valor").maskMoney({
		prefix: "R$:",
		decimal: ",",
		thousands: "."
	});

	$('.ctmErrorFranquia').maskMoney({
		decimal: '.',
		precision: 2
	});
	$('.ctmErrorFranquiaAdicional').maskMoney({
		decimal: '.',
		precision: 2
	});
	//OBS: RETIRAR MASCARA PARA ENVIO SERVIÇO: $('##Valor_ValorUnitario').maskMoney('unmasked')[0];

	//Valor ativado
	$("input[id='Valor_0__ValorAtivado']").prop("checked", true);
	$('.ctmErrorValorAtivado').on('change', function () {
		$('.ctmErrorValorAtivado').not(this).prop('checked', false);
	});

	var aberturaSpan = '<span class="text-danger field-validation-error" data-valmsg-replace="true"><span>';
	var fechamentoSpan = '</span></span></span>';

	//Limpa erro do campo Vigencia início quando alterado
	$("input[name$='VigenciaInicio']").change(function () {

		$("div[id$='VigenciaInicio']").each(function () {
			var aux = $(this).children('input');

			if (aux.length > 0) {
				$(this).find("span[id^='spanError_']").remove();
				if (aux[0].value) {

					if (aux[0].value.length > 10) {
						aux[0].value = "";
						aux[0].blur();
						$(this).append('<span id="spanError_" >' + aberturaSpan + "Data inválida" + fechamentoSpan);
					}
				}
			}
		});

	});

	//Confere campo vigência final
	$("input[name$='VigenciaFim']").change(function () {

		//Limpa erro do campo Vigencia final
		$("div[id$='VigenciaFim']").each(function () {

			var aux = $(this).children('input');

			if (aux.length > 0) {
				$(this).find("span[id^='spanError_']").remove();
				if (aux[0].value) {

					if (aux[0].value.length > 10) {
						aux[0].value = "";
						aux[0].blur();
						$(this).append('<span id="spanError_" >' + aberturaSpan + "Data inválida" + fechamentoSpan);
					}
				}
					
			}
		});

	});


	//Confere se vigencia inicio é menor que a vigência final
	$('input[id$="VigenciaFim"]').on("change keyup", function () {
		$("div[id^='date_range']").each(function () {

			if ($(this).find("span[id^='spanError_']").length > 0)
					$(this).find("span[id^='spanError_']").remove();

			var vigenciaInicio = $(this).find('input[id$="VigenciaInicio"]');
			var vigenciaFim = $(this).find('input[id$="VigenciaFim"]');
			var dateVigenciaInicio = new Date(vigenciaInicio[0].value);
			var dateVigenciaFim = new Date(vigenciaFim[0].value);

			dateVigenciaInicio > dateVigenciaFim ? $(this).append('<span id="spanError_" >' + aberturaSpan + "A data final dever ser maior que a data inicial" + fechamentoSpan) : "";
		});
	});

	//Habilita botão avançar
	$(document).on("change keyup mouseup mousemove", function () {

		if ($(document).find('span[id^="spanError_"]').length === 0)
			$("#btnSubmitFormCliente").removeAttr("disabled");
	});


	//Limpa Erro quando digitado
	$("input[name^='Valor']").on("change keyup mouseup mousemove", function () {

		$("div[id^='Valor']").each(function () {
			var aux = $(this).children('input');

			if (aux.length > 0) {

				if (aux.hasClass("ctmErrorTipoTarifa")) {
					aux.is(":checked") ? $(this).children('span[id="spanError_' + aux[0].id + '"]').remove() : "";

				} else if (aux[0].value) {
					$(this).children('span[id="spanError_' + aux[0].id + '"]').remove();
				}
			}
			

		});
	});

});

window.ValidarValor = function () {

	var aberturaSpan = '<span class="text-danger field-validation-error" data-valmsg-replace="true"><span>';
	var fechamentoSpan = '</span></span></span>';

	$("div[id^='Valor']").each(function () {
		var aux = $(this).children('input');

		if (aux.hasClass("ctmErrorTipoTarifa") && $(this).closest('.div-contato-removido').length <= 0) {
			//Tipo de Tarifa
			aux.is(":checked") ? "" : $(this).append('<span id="spanError_' + aux[0].id + '" >' + aberturaSpan + 'Tipo de tarifa é obrigatório.' + fechamentoSpan);
		} else if (aux.length > 0 && !aux[0].value && $(this).closest('.div-contato-removido').length <= 0) {
			//Valor Unitário
			aux.hasClass("ctmErrorValorUnitario") ? $(this).append('<span id="spanError_' + aux[0].id + '" >' + aberturaSpan + "Valor unitario é obrigatório." + fechamentoSpan) : "";
			//Vigência Início
			aux.hasClass("ctmErrorVigenciaInicio") ? $(this).append('<span id="spanError_' + aux[0].id + '" >' + aberturaSpan + "Obrigatório." + fechamentoSpan) : "";
			//Vigência Fim
			aux.hasClass("ctmErrorVigenciaFim") ? $(this).append('<span id="spanError_' + aux[0].id + '" >' + aberturaSpan + "Obrigatório." + fechamentoSpan) : "";
			//Franquia
			aux.hasClass("ctmErrorFranquia") ? $(this).append('<span id="spanError_' + aux[0].id + '" >' + aberturaSpan + "Franquia é obrigatório." + fechamentoSpan) : "";
			//Franquia Adicional
			aux.hasClass("ctmErrorFranquiaAdicional") ? $(this).append('<span id="spanError_' + aux[0].id + '" >' + aberturaSpan + "Franquia adicional é orbigatória." + fechamentoSpan) : "";
			//Observação
			aux.hasClass("ctmErrorObservacao") ? $(this).append('<span id="spanError_' + aux[0].id + '" >' + aberturaSpan + "Observação é obrigatório." + fechamentoSpan) : "";

		}
	});

	//Desabilita botão de avançar caso encontre erro
	if ($(document).find('span[id^="spanError_"]').length > 0) {
		$("#btnSubmitFormCliente").attr("disabled", "true");
		return false;
	}

	return true;

};

window.RemoverBlocoValor = function (divBlocoValor) {
	var id = divBlocoValor.id;

	//Adicionta classe para identificar que o valor foi removido
	$("#" + id).addClass("div-contato-removido");

	//Remove span de erro
	$('span', '#' + id).each(function () {
		$(this).remove();
	});

	//Adiciona true para model ValorRemovido
	$("#" + id).find("input[id$='ValorRemovido']").attr("value", true);

	//Exclui o bloco
	$("#" + id).hide();

	return true;

}

window.ComparaVigencias = function (vigenciaInicio, vigenciaFim) {

}