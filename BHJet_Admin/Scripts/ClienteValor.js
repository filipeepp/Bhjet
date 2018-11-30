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

	//$(".mask-data").mask("99/99/9999");

	//Limpa erro do campo Vigencia início quando alterado
	$("input[name$='VigenciaInicio']").change(function () {

		$("div[id$='VigenciaInicio']").each(function () {
			var aux = $(this).children('input');
			if (aux[0].value)
				$(this).children('span[id="spanError_' + aux[0].id + '"]').remove();
		});

	});

	//Limpa erro do campo Vigencia final quando alterado
	$("input[name$='VigenciaFim']").change(function () {

		$("div[id$='VigenciaFim']").each(function () {
			var aux = $(this).children('input');
			if (aux[0].value)
				$(this).children('span[id="spanError_' + aux[0].id + '"]').remove();
		});

	});

	//Habilita botão avançar
	$(document).change(function () {

		if ($(document).find('span[id^="spanError_"]').length === 0)
			$("#btnSubmitFormCliente").removeAttr("disabled");
	});


	//Limpa Erro quando digitado
	$("input[name^='Valor']").keyup(function () {

		$("div[id^='Valor']").each(function () {
			var aux = $(this).children('input');

			if (aux[0].value) {
				$(this).children('span[id="spanError_' + aux[0].id + '"]').remove();
			}

		});
	});

});

window.ValidarValor = function () {

	var aberturaSpan = '<span class="text-danger field-validation-error" data-valmsg-replace="true"><span>';
	var fechamentoSpan = '</span></span></span>';

	$("div[id^='Valor']").each(function () {
		var aux = $(this).children('input');
		if (!aux[0].value) {
			//Tipo de Tarifa
			aux.hasClass("ctmErrorTipoTarifa") ? $(this).append('<span id="spanError_' + aux[0].id + '" >' + aberturaSpan + 'Tipo de tarifa é obrigatório.' + fechamentoSpan) : "";
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

		//Desabilita botão de avançar caso encontre erro
		if ($(document).find('span[id^="spanError_"]').length > 0)
			$("#btnSubmitFormCliente").attr("disabled", "true");

		//return false;
	});

};

window.RemoverBlocoValor = function (divBlocoValor) {
	var id = divBlocoValor.id;
	$("#" + id).hide();
	return true;

}