
function BuscaProfissionais() {
    var jqVariavel = $("#ProfissionalSelecionado");

    if (jqVariavel.val() != "" && jqVariavel.val() !== undefined) {
        $.ajax({
            dataType: "json",
            type: "GET",
            url: "/Dashboard/BuscaProfissionais?trechoPesquisa=" + jqVariavel.val(),
            success: function (data) {
                if (data !== "" && data !== undefined) {
                    jqVariavel.autocomplete({
                        source: data,
                        select: function (el, ui) {
                            $("#IDProfissionalSelecionado").val(ui.item.value);
                            jqVariavel.val(ui.item.label);
                            el.preventDefault();
                        },
                        focus: function (event, ui) {
                            event.preventDefault();
                            el.val(ui.item.label);
                        }
                    });
                }
                else {
                    jqVariavel.val("");
                    AdicionarErroCampo('ProfissionalSelecionado', 'Não foi possível encotrar o profissional desejado.', 4000);
                }
            }
        });
    }
}

function BuscaTarifas(idProfissional) {
    $.ajax({
        dataType: "json",
        type: "GET",
        url: "/Dashboard/BuscaTarifas?idCliente=" + idProfissional,
        success: function (data) {
            if (data !== "" && data !== undefined) {
                $("#TarifaCliente").find('option').remove().end();
                var div_data = "<option value=></option>";
                $(div_data).appendTo('#TarifaCliente');
                $.each(data, function (i, obj) {
                    var div_data = "<option value=" + obj.value + ">" + obj.label + "</option>";
                    $(div_data).appendTo('#TarifaCliente');
                });
            }
        }
    });
}

function BuscaClientes() {
    var jqVariavel = $("#ClienteSelecionado");
    if (jqVariavel.val() != "" && jqVariavel.val() !== undefined) {
        $.ajax({
            dataType: "json",
            type: "GET",
            url: "/Dashboard/BuscaClientes?trechoPesquisa=" + jqVariavel.val(),
            success: function (data) {
                if (data !== "" && data !== undefined && data.length > 0) {
                    jqVariavel.autocomplete({
                        source: data,
                        select: function (el, ui) {
                            jqVariavel.val(ui.item.label);
                            $("#IDClienteSelecionado").val(ui.item.value);
                            BuscaTarifas(ui.item.value);
                            el.preventDefault();
                        },
                        focus: function (event, ui) {
                            event.preventDefault();
                            el.val(ui.item.label);
                        }
                    });
                }
                else {
                    jqVariavel.val("");
                    AdicionarErroCampo('ClienteSelecionado', 'Não foi possível encotrar o cliente desejado.', 4000);
                }
            },
        });
    }
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

    $("#ValorDiaria").mask('000.000.000.000.000,00', { reverse: true });
    $("#ValorComissao").mask('000.000.000.000.000,00', { reverse: true });
    $("#PeriodoInicial").mask("00/00/0000", { placeholder: "__/__/____" });

    $("#PeriodoFinal").mask("00/00/0000", { placeholder: "__/__/____" });
    if ($('#IDClienteSelecionado').val() == "") {
        $("#TarifaCliente").find('option').remove().end();
    }
    else {
        BuscaTarifas($('#IDClienteSelecionado').val());
    }
    if ($('#IDProfissionalSelecionado').val() == "") {
        $("#ProfissionalSelecionado").val("");
    }

    $('#ProfissionalSelecionado').click(function (event) {
        $(this).val("");
        event.preventDefault();
        event.stopPropagation();
        return false;
    });

    $('#ClienteSelecionado').click(function (event) {
        $(this).val("");
        $("#TarifaCliente").find('option').remove().end();
        event.preventDefault();
        event.stopPropagation();
        return false;
    });

    $("#ProfissionalSelecionado").keyup(delay(function (e) {
        if (/^\d+$/.test($(this).val())) {
            BuscaProfissionais();
        }
        else {
            if ($(this).val().length >= 3) {
                $(this).next().remove();
                BuscaProfissionais();
            }
            else {
                if ($(this).next().attr("id") != "msgDigitacao") {
                    AdicionarErroCampo('ClienteSelecionado', 'Preencha no minímo 3 digitos para pesquisa.', 4000);
                }
            }
        }
    }, 500));

    $("#ClienteSelecionado").keyup(delay(function (e) {
        if (/^\d+$/.test($(this).val())) {
            BuscaClientes();
        }
        else {
            if ($(this).val().length >= 3) {
                BuscaClientes();
                $(this).next().remove();
            }
            else {
                if ($(this).next().attr("id") != "msgDigitacao") {
                    AdicionarErroCampo('ClienteSelecionado', 'Preencha no minímo 3 digitos para pesquisa.', 4000);
                }
            }
        }
    }, 500));
});

