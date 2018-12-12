function BuscaClientes() {
	var jqVariavel = $("#pesquisaCliente");
	$.ajax({
		dataType: "json",
		type: "GET",
		url: "/Faturamento/BuscaClientes" + jqVariavel.val(),
		success: function (data) {
			if (data !== "" && data !== undefined && data.length > 0) {
				$("#ClienteSelecionado").find('option').remove().end();
				var div_data = "<option value=></option>";
				$(div_data).appendTo('#ClienteSelecionado');
				$.each(data, function (i, obj) {
					var div_data = "<option value=" + obj.value + ">" + obj.label + "</option>";
					$(div_data).appendTo('#ClienteSelecionado');
				});
				$("#ClienteSelecionado").mouseup();
			}
			else {
				AdicionarErroCampo('ClienteSelecionado', 'Não foi possível encotrar o cliente desejado.', 4000);
			}
		},
	});
}

document.addEventListener("DOMContentLoaded", function (event) {
	$("#pesquisaCliente").keyup(delay(function (e) {
		BuscaClientes();
	}, 500));

	BuscaClientes();

	$('#confirmaFaturamento').click(function (event) {
		//$("#loading").show()
	});

	$("#ClienteSelecionado").dblclick(function () {

	});

});