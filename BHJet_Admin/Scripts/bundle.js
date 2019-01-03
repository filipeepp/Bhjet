// Vr
var map = null;
var areas = new Array();;

function carregaMapa() {
    // Centro do mapa
    var myLatLng = new google.maps.LatLng(-19.878951, -43.933833);
    // Options
    var mapOptions = {
        zoom: 6,
        center: myLatLng,
        mapTypeId: google.maps.MapTypeId.RoadMap
    };
    map = new google.maps.Map(document.getElementById('map_canvas'), mapOptions);
    // LDArea
    BuscaAreasCadastradas();
}

function adicionarArea(triangleCoords) {
    // Area
    var myPolygon2 = new google.maps.Polygon({
        paths: triangleCoords,
        draggable: true,
        editable: true,
        strokeColor: '#ffeb3b',
        strokeOpacity: 0.8,
        strokeWeight: 2,
        fillColor: '#ffeb3b',
        fillOpacity: 0.35
    });

    areas.push(myPolygon2);

    myPolygon2.setMap(map);
    google.maps.event.addListener(myPolygon2, 'rightclick', function (event) {
        if (confirm('Deseja remover esta Área de Atuação ?')) {

            var index = areas.indexOf(this);
            areas.splice(index, 1);
            this.setMap(null);
            getPolygonCoords();
        }
    });

    google.maps.event.addListener(myPolygon2.getPath(), "insert_at", getPolygonCoords);
    google.maps.event.addListener(myPolygon2.getPath(), "set_at", getPolygonCoords);

    getPolygonCoords();
}


function getPolygonCoords() {
    var htmlStr = "[\n ";
    htmlStr += " {\n ";

    for (var i = 0; i < areas.length; i++) {
        htmlStr += "\"Area\": [ \n";
        var polygon = areas[i];
        var len = polygon.getPath().getLength();
        for (var j = 0; j < len; j++) {
            htmlStr += "\n {";
            htmlStr += "\n   \"Latitude\": \"" + polygon.getPath().getAt(j).lat() + "\",";
            htmlStr += "\n   \"Longitude\": \"" + polygon.getPath().getAt(j).lng() + "\"";
            htmlStr += "\n }";

            if (j < (len - 1))
                htmlStr += ",";

        }
        htmlStr += "\n]\n";
    }

    htmlStr += " }\n ";
    htmlStr += "]";

    //document.getElementById('info').value = htmlStr;
    $("#info").val(htmlStr);
}

function initAutocomplete() {
    var input = document.getElementById('pac-input');
    var options = {

    };
    autocomplete = new google.maps.places.Autocomplete(input, options);

    autocomplete.addListener('place_changed', function () {
        var place = autocomplete.getPlace();
        var location = place.geometry.location;

        var cords = [
            new google.maps.LatLng(place.geometry.location.lat() + 1, place.geometry.location.lng() + 1),
            new google.maps.LatLng(place.geometry.location.lat() - 0.5, place.geometry.location.lng() + 1),
            new google.maps.LatLng(place.geometry.location.lat() - 0.5, place.geometry.location.lng() - 0.5),
            new google.maps.LatLng(place.geometry.location.lat() + 1, place.geometry.location.lng() - 0.5),
        ];

        adicionarArea(cords)
        $("#pac-input").val("")
    });
}

function BuscaAreasCadastradas(idCom) {
    $.ajax({
        url: '/Atuacao/BuscaAreas',
        dataType: "json",
        type: "GET",
        success: function (dados) {

            var obj = JSON.parse(dados);
            $.each(obj, function (index, value) {
                var cords = [];
                for (var i = 0; i < value.GeoVertices.length; i++) {
                    var lat = value.GeoVertices[i].Latitude;
                    var long = value.GeoVertices[i].Longitude;
                    cords.push(new google.maps.LatLng(lat, long));
                }

                adicionarArea(cords);
            });
            if (dados == "" || dados == undefined) {
                $("#msgModal").text("Não foram encontrados Áreas de atuação para ser exibida no mapa.")
                $("#imgMensagem").attr("src", "..\\Images\\warming.png");
                $('#myModal').modal('show')
            }
        },
        error: function (e) {
            var teste = e;
        }
    });
}

function AtualizaAreas() {
    $.ajax({
        url: '/Atuacao/CadastraAreas',
        type: "POST",
        dataType: "json",
        contentType: 'application/json',
        data: $("#info").val(),
        success: function (data) {
            MensagemSucesso("Áreas de atuação atualizadas com sucesso.");
        },
        error: function () {
            $("html, body").animate({
                scrollTop: $(document).height()
            }, 1000);
        }
    });
}

$(document).ready(function () {
    // ---
    carregaMapa();
    // ---
    initAutocomplete();
    // ---
    $("#bntAtualizaAreas").click(function () {

        var test = $("#info").val().replace(/[^\w\s]/gi, '');
        test = test.replace(/\s/g, '');
        if (test == "" || $("#info").val() == "" || $("#info").val() == undefined) {
            MensagemAlerta("Não existem Áreas de atuação selecionadas no mapa.");
            return false;
        }
        else {
            AtualizaAreas();
        }
    })
});


function BuscaProfissionais() {
    var jqVariavel = $("#pesquisaProfissional");
    $("#ProfissionalSelecionado").find('option').remove().end();
    $.ajax({
        dataType: "json",
        type: "GET",
        url: "/Dashboard/BuscaProfissionais?trechoPesquisa=" + jqVariavel.val(),
        success: function (data) {
            if (data !== "" && data !== undefined) {
                $("#ProfissionalSelecionado").find('option').remove().end();
                var div_data = "<option value=></option>";
                $(div_data).appendTo('#ProfissionalSelecionado');
                $.each(data, function (i, obj) {
                    var div_data = "<option value=" + obj.value + ">" + obj.label + "</option>";
                    $(div_data).appendTo('#ProfissionalSelecionado');
                });
                $("#ProfissionalSelecionado").mouseup();
            }
            else {
                AdicionarErroCampo('ProfissionalSelecionado', 'Não foi possível encontrar o profissional desejado.', 4000);
            }
        }
    });
}

function BuscaTarifas(idCliente) {
    $.ajax({
        dataType: "json",
        type: "GET",
        url: "/Dashboard/BuscaTarifas?idCliente=" + idCliente,
        success: function (data) {
            if (data !== "" && data !== undefined) {
                //$("#TarifaCliente").find('option').remove().end();
                //var div_data = "<option value=></option>";
                //$(div_data).appendTo('#TarifaCliente');
                //$.each(data, function (i, obj) {
                //    var div_data = "<option value=" + obj.value + ">" + obj.label + "</option>";
                //    $(div_data).appendTo('#TarifaCliente');
                //});
                $("#ValorDiaria").val(data.DecValorDiaria);
                $("#ValorKMAdicional").val(data.decValorKMAdicionalDiaria);
                $("#FranquiaKMDiaria").val(data.decFranquiaKMDiaria);
            }
        }
    });
}

function BuscaComissaoProfissional(idProfissional) {
    $.ajax({
        dataType: "json",
        type: "GET",
        url: "/Dashboard/BuscaComissao?idProfissional=" + idProfissional,
        success: function (data) {
            if (data !== "" && data !== undefined) {
                $("#ValorComissao").val(data.decPercentualComissao);
                $("#ValorComissao").mask('##0,00%', { reverse: true });
            }
        }
    });
}

function BuscaClientes() {
    var jqVariavel = $("#pesquisaCliente");
    $("#TarifaCliente").find('option').remove().end();

    $.ajax({
        dataType: "json",
        type: "GET",
        url: "/Dashboard/BuscaClientes?trechoPesquisa=" + jqVariavel.val(),
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

function validaDatePickerManual(id, valor) {
    var dgtDatesp = valor.split("/");
    var dgtDate = new Date(dgtDatesp[2].split(' ')[0], dgtDatesp[1] - 1, dgtDatesp[0]);
    var todayDate = new Date();
    if (dgtDate < todayDate) {
        $("#" + id).val("")
        $('#' + id).datepicker("hide");
        AdicionarErroCampo(id, 'A data digitada não pode ser menor que a data atual.', 4000);
    };
}

document.addEventListener("DOMContentLoaded", function (event) {

    $("#ValorDiaria").mask('000.000.000.000.000,00', { reverse: true });
    $("#FranquiaKMDiaria").mask('000.000.000.000.000,00', { reverse: true });
    $("#ValorComissao").mask('##0,00%', { reverse: true });
    $("#HorarioInicial").mask('00:00', { reverse: true, placeholder: "00:00" });
    $("#HorarioFim").mask('00:00', { reverse: true, placeholder: "00:00" });

    $("#PeriodoInicial").mask("00/00/0000", {
        onComplete: function (a) {
            validaDatePickerManual("PeriodoInicial", a)
        },
        placeholder: "__/__/____"
    });
    $("#PeriodoFinal").mask("00/00/0000", {
        onComplete: function (a) {
            validaDatePickerManual("PeriodoFinal", a)
        },
        placeholder: "__/__/____"
    });

    $("#PeriodoInicial").datepicker({
        dateFormat: "dd/mm/yy",
        dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'],
        dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S', 'D'],
        dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'],
        monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
        monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
        nextText: 'Próximo',
        prevText: 'Anterior',
        minDate: 0
    });

    $("#PeriodoFinal").datepicker({
        dateFormat: "dd/mm/yy",
        dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'],
        dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S', 'D'],
        dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'],
        monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
        monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
        nextText: 'Próximo',
        prevText: 'Anterior',
        minDate: 0
    });

    $('#pesquisaCliente').click(function (event) {
        $(this).val("");
        $("#TarifaCliente").find('option').remove().end();
        event.preventDefault();
        event.stopPropagation();
        return false;
    });

    $("#pesquisaCliente").keyup(delay(function (e) {
        BuscaClientes();
    }, 500));

    $("#pesquisaProfissional").keyup(delay(function (e) {
        BuscaProfissionais();
    }, 500));

    $("#ClienteSelecionado").change(function () {
        var $option = $(this).find('option:selected');
        if ($option != undefined) {
            var value = $option.val();
            var text = $option.text();
            if (value != undefined && value != "") {
                BuscaTarifas(value)
            }
        }
    });

    $("#ProfissionalSelecionado").change(function () {
        var $option = $(this).find('option:selected');
        if ($option != undefined) {
            var value = $option.val();
            var text = $option.text();
            if (value != undefined && value != "") {
                BuscaComissaoProfissional(value)
            }
        }
    });

    BuscaClientes();
    BuscaProfissionais();

});


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

		if ($(document).find('span[id^="spanError_"]').length === 0) {
			$("#lnkValidarContato").removeClass("isDisabled");
		}

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
			url: '/Clientes/ExcluirContato?idContato=' + idContato,
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

$(document).ready(function () {
	//MASCARAS
	$('.ctmErrorVigenciaInicio').on("keyup mouseup", function (event) {
		var id = event.target.id;
		$('input[id="' + id + '"]').prop("type", "date");
	});

	$('.ctmErrorVigenciaFim').on("keyup mouseup", function (event) {
		var id = event.target.id;
		$('input[id="' + id + '"]').prop("type", "date");
	});

	$('.ctmErrorValorUnitario').keyup(function (event) {
		var id = event.target.id;
		$('input[id="' + id + '"]').maskMoney({ prefix: "R$:", decimal: ',', thousands: "." });
	});

	$('.ctmErrorFranquia').keyup(function (event) {
		var id = event.target.id;
		$('input[id="' + id + '"]').maskMoney({ decimal: ',', thousands: "." });
	});

	$('.ctmErrorFranquiaAdicional').keyup(function (event) {
		var id = event.target.id;
		$('input[id="' + id + '"]').maskMoney({ decimal: ',', thousands: "." });
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
			aux.hasClass("ctmErrorFranquiaAdicional") ? $(this).append('<span id="spanError_' + aux[0].id + '" >' + aberturaSpan + "Franquia adicional é obrigatória." + fechamentoSpan) : "";
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

window.ExcluirValor = function (divBlocoValor) {

	var idBloco = divBlocoValor.id;
	var idValor = $("#" + idBloco).find("input[id$='ID']").val();

	var alertConfirmacao = window.confirm("Tem certeza que deseja excluir esse valor da base?");

	if (alertConfirmacao) {
		$.ajax({
			url: '/Clientes/ExcluirValor?idValor=' + idValor,
			type: "POST",
			success: function () {
				RemoverBlocoValor(divBlocoValor);
				$("html, body").animate({ scrollTop: $(document).height() }, 1000);
			},
			error: function () {
				var idCliente = $("input[id='ID']").val();
				$("html, body").animate({ scrollTop: $(document).height() }, 1000);
				window.location.href = "/Clientes/NovoCliente?edicao=true&clienteID=" + idCliente;
			}
		});

	} else {
		event.stopPropagation();
	}

}

window.PesquisaTarifarioPadrao = function (divBlocoValor) {

	var idBloco = divBlocoValor.id;
	var inputValorPadrao = $("#" + idBloco).find("input[id^='ValorPadrao']");

	if (inputValorPadrao.prop("checked")) {

		$.ajax({
			url: '/Tarifario/BuscarTarifarioPadraoAtivo',
			type: "GET",
			dataType: "json",
			success: function (data) {

				var tarifarioPadrao = JSON.parse(data);

				$("#" + idBloco).find("input[id$='ValorUnitario']").val(tarifarioPadrao.ValorDiaria);
				$("#" + idBloco).find("input[id$='VigenciaInicio']").val(tarifarioPadrao.VigenciaInicio);
				$("#" + idBloco).find("input[id$='VigenciaFim']").val(tarifarioPadrao.VigenciaFim);
				$("#" + idBloco).find("input[id$='Franquia']").val(tarifarioPadrao.FranquiaKMDiaria);
				$("#" + idBloco).find("input[id$='FranquiaAdicional']").val(tarifarioPadrao.ValorKMAdicionalMensalidade);
				$("#" + idBloco).find("input[id$='Observacao']").val(tarifarioPadrao.Descricao);
			},
			error: function () { alert("Ops! Não foi possível buscar essa informação. Tente novamente mais tarde.") }
		});
	}
}

function mascaraComissoes() {
    $("input[id*='ValorComissao']").each(function () {
        $(this).maskMoney({
            prefix: "R$ ",
            decimal: ",",
            thousands: "."
        });
    })

    $("input[id*='VigenciaInicio'], input[id*='VigenciaFim']").each(function () {
        $(this).mask("00/00/0000", {
            onComplete: function (a) {
                //if (!isValidDate(a)) {
                //    var teste = $('input[text*=' + a + ']');
                //    teste.val();
                //    AdicionarErroCampo(teste.attr('id'), 'Favor preencher uma data válida.', 10000);
                //}
            },
            placeholder: "__/__/____"
        });

        $(this).datepicker({
            dateFormat: "dd/mm/yy",
            dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'],
            dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S', 'D'],
            dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'],
            monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
            monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
            nextText: 'Próximo',
            prevText: 'Anterior',
            minDate: 0,
            onSelect: function () {
                $("#PeriodoInicial").val($("#PeriodoInicial").val() + " 09:00")
            }
        });
    })
}

document.addEventListener("DOMContentLoaded", function (event) {
    $("#loading").hide()
    mascaraComissoes();
    $("#TelefoneResidencial").mask("(00) 0000-0000");
    $("#TelefoneCelular").mask("(00) 0000-00009");

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
        //ms
        var erro;
        var t = "";
        //vld
        $(".rowList").each(function () {
            var comissao = $(this).find("input[id*='ValorComissao']")
            var vgInicio = $(this).find("input[id*='VigenciaInicio']")
            var vgFim = $(this).find("input[id*='VigenciaFim']")
            if ((comissao.val() != "" || vgInicio.val() != "" || vgFim.val() != "") || $(".rowList").length > 1) {
                //com
                if (comissao.val() == "") {
                    comissao.css('border-color', 'red');
                    AdicionarErroCampo(comissao.attr('id'), 'Favor preencher um valor para a comissão.', 10000);
                    erro = true;
                }
                else {
                    comissao.css('border-color', '#ccc');
                }
                //vgIni
                if (vgInicio.val() == "") {
                    vgInicio.css('border-color', 'red');
                    AdicionarErroCampo(vgInicio.attr('id'), 'Favor preencher um valor para a data de vigência inicial.', 10000);
                    erro = true;
                }
                else {
                    if (!isValidDate(vgInicio.val())) {
                        vgInicio.css('border-color', 'red');
                        AdicionarErroCampo(vgInicio.attr('id'), 'Favor preencher uma data válida.', 10000);
                        erro = true;
                    } else {
                        vgInicio.css('border-color', '#ccc');
                    }
                }
                //vgFim
                if (vgFim.val() == "") {
                    vgFim.css('border-color', 'red');
                    AdicionarErroCampo(vgFim.attr('id'), 'Favor preencher um valor para a data de vigência final.', 10000);
                    erro = true;
                }
                else {
                    if (!isValidDate(vgFim.val())) {
                        vgFim.css('border-color', 'red');
                        AdicionarErroCampo(vgFim.attr('id'), 'Favor preencher uma data válida.', 10000);
                        erro = true;
                    } else {
                        vgFim.css('border-color', '#ccc');
                    }
                }
            }
            else {
                comissao.css('border-color', '#ccc');
                vgInicio.css('border-color', '#ccc');
                vgFim.css('border-color', '#ccc');
            }
        })
        // err
        if (erro) {
            MensagemAlerta("Os Campos destacados são obrigatórios.");
            event.preventDefault();
            event.stopPropagation();
            return false;
        }
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

    $("#btnNovoContatos").click(function () {
        $.ajax({
            url: '/Motorista/AddComissao',
            type: "POST",
            dataType: "html",
            data: $("#DetalheMotorista").serialize(),
            success: function (data) {
                window.location.href = "/Motorista/Novo?alteraComissao=TRUE";
                $("html, body").animate({ scrollTop: $(document).height() }, 1000);
                mascaraComissoes();
            },
            error: function () { $("html, body").animate({ scrollTop: $(document).height() }, 1000); }
        });
    })
});

function removeComissao(idCom) {
    $.ajax({
        url: '/Motorista/ExcluirComissao?numeroComissao=' + idCom,
        type: "POST",
        dataType: "html",
        data: $("#DetalheMotorista").serialize(),
        success: function (data) {
            window.location.href = "/Motorista/Novo?alteraComissao=TRUE";
            mascaraComissoes();
            $("html, body").animate({ scrollTop: $(document).height() }, 1000);
        },
        error: function () { $("html, body").animate({ scrollTop: $(document).height() }, 1000); }
    });
}
document.addEventListener("DOMContentLoaded", function (event) {
    function atualizarMarcacoes() {
        $.ajax({
            dataType: "json",
            type: "GET",
            url: "/Dashboard/BuscaLocalizacao",
            success: function (dados) {
                $(dados).each(function (i) {
                    var lat = dados[i].geoPosicao.split(';')[0]
                    var long = dados[i].geoPosicao.split(';')[1]
                    var desc = dados[i].desc
                    FazMarcacao(lat, long, dados[i].psCorrida, desc)
                });

                if (dados == "" || dados == undefined) {
                    $("#msgModal").text("Não foram encontrados localizações para marcação no mapa.")
                    $("#imgMensagem").attr("src", "..\\Images\\warming.png");
                    $('#myModal').modal('show')
                }
            }
        });
    }
    carregaMapa();
    atualizarMarcacoes();
});
function BuscaClientes() {
    var jqVariavel = $("#pesquisaCliente");
        $.ajax({
            dataType: "json",
            type: "GET",
            url: "/Faturamento/BuscaClientes?trechoPesquisa=" + jqVariavel.val(),
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

});
//variavel cria para que seja criado o mapa Google Maps
var map = null;

//Essa e a funcao que criara o mapa GoogleMaps
function carregaMapa() {
    //aqui vamos definir as coordenadas de latitude e longitude no qual
    //sera exibido o nosso mapa
    var latlng = new google.maps.LatLng(-19.8157, -43.9542); //DEFINE A LOCALIZAÇÃO EXATA DO MAPA
    //aqui vamos configurar o mapa, como o zoom, o centro do mapa, etc
    var myOptions = {
        zoom: 15,//utilizaremos o zoom 15
        center: latlng,//aqui a nossa variavel constando latitude e
        //longitude ja declarada acima
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    //criando o mapa dentro da div com o id="map_canvas" que ja criamos
    map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);

    //DEFINE AS COORDENADAS do ponto exato - CENTRALIZAÇÃO DO MAPA
    map.setCenter(new google.maps.LatLng(-19.878946, -43.933877));
}

function FazMarcacao(lat, long, bcarro, bdesc) {

    lat = lat.replace(",", ".");
    long = long.replace(",", ".");
    var latlong = lat + "," + long;//colocando na conficuracao necessaria (lat,long)
    var myLatLgn = new google.maps.LatLng(lat, long);
    //criando variavel tipo google.maps.LatLng e
    //passando como parametro a latitude e longitude
    //na configuracao: latitude,longitude.

    //aproximando o mapa, aumentando o zoom
    map.setZoom(15);

    //fazendo  a marcacao, usando o latitude e longitude da variavel criada acima
    var marker = new google.maps.Marker({
        position: myLatLgn,
        map: map,
        icon: bcarro == true ? '/./Images/mapCorrida.png' : '/./Images/mapCar.png',
        animation: google.maps.Animation.DROP
    });

    marker.addListener('click', function () {
        var infowindow = new google.maps.InfoWindow();
        infowindow.setContent("-----------------------</br><b>BH Jet Express</b></br>-----------------------</br>" + bdesc);
        infowindow.open(map, marker);
    });

    //aqui a variavel e o comando que faz a marcação
    map.setCenter(myLatLgn);//leva o mapa para a posicao da marcacao
}


function buscaGraficoResumoSituacaoChamados() {
    $.ajax({
        dataType: "json",
        type: "GET",
        url: "/Dashboard/BuscaResumoSituacaoChamados",
        success: function (dados) {

            var lbls = [];
            var DataCh1 = [];
            var DataCh2 = [];

            $(dados).each(function (i) {
                lbls.push(month[dados[i].Mes]);
                DataCh1.push(dados[i].ChamadosConcluidos);
                DataCh2.push(dados[i].ChamadosAdvertentes);
            });

            var ctx = document.getElementById('canvasz').getContext('2d');
            var myChart = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: lbls,
                    datasets: [{
                        label: 'Pendetes',
                        data: DataCh2,
                        backgroundColor: [
                            'rgb(255, 255, 102, 0.2)'
                        ],
                        borderColor: [
                            'rgb(255, 204, 0, 1)'
                        ],
                        borderWidth: 1
                    },
                    {
                        label: 'Concluídos',
                        data: DataCh1,
                        backgroundColor: [
                            '	rgb(0, 153, 77, 0.2)',
                        ],
                        borderColor: [
                            'rgb(0, 153, 0, 1)',
                        ],
                        borderWidth: 1
                    },]
                },
                options: {
                    title: {
                        display: true,
                        text: 'Chamados por Situação',
                        fontColor: "#000"
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });
            // ******
        }
    });
}

function buscaGraficoResumoSituacaoAtendimentos() {
    $.ajax({
        dataType: "json",
        type: "GET",
        url: "/Dashboard/BuscaResumoAtendimentos",
        success: function (dados) {

            var lbls = [];
            var DataMoto = [];
            var DataCarro = [];

            $(dados).each(function (i) {
                lbls.push(month[dados[i].Mes]);
                DataMoto.push(dados[i].QtdAtendimentoMotociclista);
                DataCarro.push(dados[i].QtdAtendimentoMotorista);
            });

            var ctx = document.getElementById('canvas').getContext('2d');
            var myChart = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: lbls,
                    datasets: [{
                        label: 'Motoqueiro',
                        data: DataMoto,
                        backgroundColor: [
                            'rgba(255, 99, 132, 0.2)',
                            'rgba(54, 162, 235, 0.2)',
                            'rgba(255, 206, 86, 0.2)',
                            'rgba(75, 192, 192, 0.2)',
                            'rgba(153, 102, 255, 0.2)',
                            'rgba(255, 159, 64, 0.2)'
                        ],
                        borderColor: [
                            'rgba(255,99,132,1)',
                            'rgba(54, 162, 235, 1)',
                            'rgba(255, 206, 86, 1)',
                            'rgba(75, 192, 192, 1)',
                            'rgba(153, 102, 255, 1)',
                            'rgba(255, 159, 64, 1)'
                        ],
                        borderWidth: 1
                    },
                    {
                        label: 'Motorista',
                        data: DataCarro,
                        backgroundColor: [
                            'rgb(54, 162, 235, 0.2)',
                        ],
                        borderColor: [
                            'rgb(0, 128, 255, 1)',
                        ],
                        borderWidth: 1
                    },]
                },
                options: {
                    title: {
                        display: true,
                        text: 'Atendimentos por profissional',
                        fontColor: "#000"
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });
            // ******
        }
    });
}

$(document).ready(function ($) {
    $('.counter').counterUp({
        delay: 10,
        time: 1000
    });
    $('#PesquisaOSCliente').mask('0#');
    $('#PesquisaMotociclista').mask('0#');
    buscaGraficoResumoSituacaoChamados();
    buscaGraficoResumoSituacaoAtendimentos();

});




document.addEventListener("DOMContentLoaded", function (event) {
    $("#loading").hide()
    $('#btnLogar').click(function (event) {
        $("#loading").show()
    });
});




document.addEventListener("DOMContentLoaded", function (event) {
    $("#loading").hide();
    alteraMenu();
  
    $("#menuLogisticaMobile").click(function () {
        var menu = $(".main-sidebar");
        if (menu.css("display") === "none" || menu.css("display") == undefined) {

            menu.animate({
                opacity: 0.25,
                left: "+=0",
                height: "toggle"
            }, 400, function () {
                menu.css('display', 'inline-block');
                menu.css('opacity', '1');
                if ($(window).width() > 1112) {
                    $(".content-wrapper").first().css('padding-left', "230px")
                }
                else {
                    $(".content-wrapper").first().css('padding-left', "0px")
                }
            });
        }
        else {
            menu.animate({
                opacity: 0.25,
                right: "+=50",
                height: "toggle"
            }, 400, function () {
                menu.css('display', 'none');
                menu.css('opacity', '1');
                if ($(window).width() > 1112) {
                    $(".content-wrapper").first().css('padding-left', "0px")
                }
                else {
                    $(".content-wrapper").first().css('padding-left', "0px")
                }
            });
        }
    })

    $("#closeMsgGeral").click(function () {
        $("#msgModal").text('')
    })

    $('#menuLateral a').click(function (event) {
        $("#loading").show()
    });

});

function parseDMY(value) {
    var date = value.split("/");
    var d = parseInt(date[0], 10),
        m = parseInt(date[1], 10),
        y = parseInt(date[2], 10);
    return new Date(y, m - 1, d);
}

function isValidDate(txtDate) {
    var currVal = txtDate;
    if (currVal == '')
        return false;
    //Declare Regex 
    var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;
    var dtArray = currVal.match(rxDatePattern); // is format OK?
    if (dtArray == null)
        return false;
    //Checks for mm/dd/yyyy format.
    dtDay = dtArray[1];
    dtMonth = dtArray[3];
    dtYear = dtArray[5]; 
    if (dtMonth < 1 || dtMonth > 12)
        return false;
    else if (dtDay < 1 || dtDay > 31)
        return false;
    else if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay == 31)
        return false;
    else if (dtMonth == 2) {
        var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
        if (dtDay > 29 || (dtDay == 29 && !isleap))
            return false;
    }
    return true;

}

function alteraMenu() {

    $(".sidebar-menu").find("li").each(function () {
        $(this).removeClass("active")
        if ($(this)[0].textContent.indexOf(window.location.href.split('/')[3]) !== -1) {
            $(this).addClass("active")
            return false;
        }
        else if ($(this)[0].textContent.indexOf("Início") !== -1 && window.location.href.split('/')[3] == "Home" && window.location.href.split('/')[4] == "Index") {
            $(this).addClass("active")
            return false;
        }
    });

}

function MensagemAlerta(msg) {
    $("#msgModal").text(msg)
    $("#imgMensagem").attr("src", "..\\Images\\warming.png");
    $('#myModal').modal('show')
}

function MensagemSucesso(msg) {
    $("#msgModal").text(msg)
    $("#imgMensagem").attr("src", "..\\Images\\sucesso.png");
    $('#myModal').modal('show')
}

function Carregamento() {
   // $("#loading").show();
}


function AdicionarErroCampo(idField, message, tempoAtivo) {
    var para = document.createElement("p");
    var node = document.createTextNode(message);
    para.appendChild(node);
    para.className = "msgDigitacao";
    para.style.position = 'relative';
    para.style.fontSize = '12px';
    para.style.color = '#c33939';
    para.style.display = 'inline';
    if ($("#" + idField) != undefined) {
        if ($("#" + idField).next().hasClass("msgDigitacao")) {
            $("#" + idField).next().remove();
        }
        $("#" + idField).after(para);
    }
    var removeAfter = tempoAtivo;
    (function (removeAfter) {
        setTimeout(function () {
            $("#" + idField).next().remove();
        }, removeAfter);
    })(removeAfter)
}

function delay(callback, ms) {
    var timer = 0;
    return function () {
        var context = this, args = arguments;
        clearTimeout(timer);
        timer = setTimeout(function () {
            callback.apply(context, args);
        }, ms || 0);
    };
}

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



document.addEventListener("DOMContentLoaded", function (event) {

    $('label[rel=popover]').popover({
        html: true,
        trigger: 'click',
        placement: 'auto',
        title: '<button type="button" id="close" class="close" onclick="closePopover()">&times;</button></br>',
        content: function () {
            return '<img class="img-responsive" src=' + $(this).data('img') + ' />';
        }
    });


});


function closePopover() {
    $('label[rel=popover]').popover('hide');
}



function BuscaClientes(idPreCliente) {
    var jqVariavel = $("#pesquisaCliente");
    $.ajax({
        dataType: "json",
        type: "GET",
        url: "/Usuario/BuscaClientes?trechoPesquisa=" + jqVariavel.val(),
        success: function (data) {
            if (data !== "" && data !== undefined && data.length > 0) {
                $("#ClienteSelecionado").find('option').remove().end();
                var div_data = "<option value=></option>";
                $(div_data).appendTo('#ClienteSelecionado');
                $.each(data, function (i, obj) {
                    var div_data = "<option value=" + obj.value + ">" + obj.label + "</option>";
                    $(div_data).appendTo('#ClienteSelecionado');
                });

                if (idPreCliente !== undefined) {
                    $("#ClienteSelecionado").val(idPreCliente);
                }

                $("#ClienteSelecionado").mouseup();
            }
            else {
                AdicionarErroCampo('ClienteSelecionado', 'Não foi possível encotrar o cliente desejado.', 4000);
            }
        },
    });

}

function necessarioCliente() {
    var $option = $("#TipoUser").prop('selectedIndex');
    if ($option != undefined && ($option === 2 || $option === 3)) {
        $("#cliRefer").css("display", "unset")
        return true;
    }
    else {
        $("#cliRefer").css("display", "none")
        return false;
    }
    return false;
}

$("#pesquisaCliente").keyup(delay(function (e) {
    BuscaClientes();
}, 500));

window.onload = function () {
    $("#loading").show();
};

document.addEventListener("DOMContentLoaded", function (event) {

    var x = document.getElementById("TipoUser");
    x.remove(2);

    $("#TipoUser").change(function () {
        necessarioCliente();
    });

    $("#ClienteSelecionado").change(function () {
        $("#ClienteSelecionado").css('border-color', '#ccc');
    });

    $("#confirmaMotorista").click(function () {
        if (necessarioCliente()) {
            var index = $("#ClienteSelecionado").prop('selectedIndex');
            if (index === undefined || index === "undefined" || index == 0) {
                $("#ClienteSelecionado").css('border-color', 'red');
                MensagemAlerta("Selecione um cliente para prosseguir.");
                event.preventDefault();
                event.stopPropagation();
                return false;
            }
            else {
                $("#ClienteSelecionado").css('border-color', '#ccc');
            }
        }
    })

    if ($("#EdicaoCadastro").val() === "True") {
        necessarioCliente();
        setTimeout(function afterTwoSeconds() {
            $('#Senha').val("")
            $("#loading").hide()
        }, 100)

        BuscaClientes($("#ClienteSelecionadoBKP").val());
    }
    else {
        setTimeout(function afterTwoSeconds() {
            $('#Senha').val("")
            $('#Email').val("")
            $("#loading").hide()
        }, 700)
        BuscaClientes();
    }

});
function ExcluirUsuario(idUser) {

    $.ajax({
        dataType: "json",
        type: "GET",
        url: "/Usuario/DeletaUsuario?id=" + idUser,
        success: function (dados) {
            $("#loading").hide();
            $("#msgModal").text(dados)
            $("#imgMensagem").attr("src", "\\.\\Images\\sucesso.png");
            $('#myModal button').click(function () {
                window.location.href = '/Usuario/Listar/';
            })
            $('#myModal').modal('show')
        },
        error: function (dadosEr) {
            $("#loading").hide();
            $("#msgModal").text('Não foi possível excluir o usuário selecionado, tente novamente mais tarde.')
            $("#imgMensagem").attr("src", "\\.\\Images\\warming.png");
            $('#myModal').modal('show')
        }
    });
}

function AlterarSituacao(idUser, situacao) {
    var queryString = "situacao=" + situacao + "&" + "id=" + idUser;
    $.ajax({
        dataType: "json",
        type: "GET",
        url: "/Usuario/AlteraSituacao?" + queryString,
        success: function (dados) {
            $("#loading").hide();
            $("#msgModal").text(dados)
            $("#imgMensagem").attr("src", "\\.\\Images\\sucesso.png");
            $('#myModal button').click(function () {
                window.location.href = '/Usuario/Listar/';
            })
            $('#myModal').modal('show')
        },
        error: function (dadosEr) {
            $("#loading").hide();
            $("#msgModal").text('Não foi possível alterar a situação do usuário selecionado, tente novamente mais tarde.')
            $("#imgMensagem").attr("src", "\\.\\Images\\warming.png");
            $('#myModal').modal('show')
        }
    });
}

function AlterarSituacao(idUser, situacao) {
    var queryString = "situacao=" + situacao + "&" + "id=" + idUser;
    $.ajax({
        dataType: "json",
        type: "GET",
        url: "/Usuario/AlteraSituacao?" + queryString,
        success: function (dados) {
            $("#loading").hide();
            $("#msgModal").text(dados)
            $("#imgMensagem").attr("src", "\\.\\Images\\sucesso.png");
            $('#myModal button').click(function () {
                window.location.href = '/Usuario/Listar/';
            })
            $('#myModal').modal('show')
        },
        error: function (dadosEr) {
            $("#loading").hide();
            $("#msgModal").text('Não foi possível alterar a situação do usuário selecionado, tente novamente mais tarde.')
            $("#imgMensagem").attr("src", "\\.\\Images\\warming.png");
            $('#myModal').modal('show')
        }
    });
}

function EditarUsuario(idUser) {
    editarUsuarioNavegacao(idUser);
}

document.addEventListener("DOMContentLoaded", function (event) {
    $(".cst-tbl-linhaHover").click(function (id) {
        editarUsuarioNavegacao(id);
    });
});

function editarUsuarioNavegacao(idUser) {
    window.location = '/Usuario/Novo?Edicao=true&ID=' + idUser;
}


window.month = new Array();
month[1] = "Janeiro";
month[2] = "Fevereiro";
month[3] = "Março";
month[4] = "Abril";
month[5] = "Maio";
month[6] = "Junho";
month[7] = "Julho";
month[8] = "Agosto";
month[9] = "Setembro";
month[10] = "Outubro";
month[11] = "Novembro";
month[12] = "Dezembro";
function GetOptionsViaCep(arrayEndereco) {
    var options = "";
    return options = {
        onComplete: function (cep) {
            //Nova variável "cep" somente com dígitos.
            var cep = arrayEndereco.Cep.val().replace(/\D/g, '');
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
                            arrayEndereco.Rua.val(dados.logradouro);
                            arrayEndereco.Bairro.val(dados.bairro);
                            arrayEndereco.Cidade.val(dados.localidade);
                            arrayEndereco.Estado.val(dados.uf);
                            arrayEndereco.NumeroEndereco.focus();

                        } //end if.
                        else {
                            limpaEndereco();
                        }
                    });
                } //end if.
            } //end if.
        },
    };
}

function limpaEndereco() {
    // Limpa valores do formulário de cep.
    $("#Rua").val("");
    $("#Bairro").val("");
    $("#Cidade").val("");
    $("#UF").val("");
}