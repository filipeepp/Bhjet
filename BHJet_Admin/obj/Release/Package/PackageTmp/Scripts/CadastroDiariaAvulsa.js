
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
    $("#ValorComissao").mask('000.000.000.000.000,00', { reverse: true });

    $("#PeriodoInicial").mask("00/00/0000 00:00", {
        onComplete: function (a) {
            validaDatePickerManual("PeriodoInicial", a)
        },
        placeholder: "__/__/____ 00:00"
    });
    $("#PeriodoFinal").mask("00/00/0000 00:00", {
        onComplete: function (a) {
            validaDatePickerManual("PeriodoFinal", a)
        },
        placeholder: "__/__/____ 00:00"
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
        minDate: 0,
        onSelect: function () {
            $("#PeriodoInicial").val($("#PeriodoInicial").val() + " 09:00")
        }
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
        minDate: 0,
        onSelect: function () {
            $("#PeriodoFinal").val($("#PeriodoFinal").val() + " 18:00")
        }
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

    BuscaClientes();
    BuscaProfissionais();

});

