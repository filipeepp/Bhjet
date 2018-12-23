

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

document.addEventListener("DOMContentLoaded", function (event) {

    var x = document.getElementById("TipoUser");
    x.remove(2);

    if ($("#EdicaoCadastro").val() === "True") {
        necessarioCliente();
        setTimeout(function afterTwoSeconds() {
            $('#Senha').val("")
        }, 80)

        BuscaClientes($("#ClienteSelecionadoBKP").val());
    }
    else {
        setTimeout(function afterTwoSeconds() {
            $('#Senha').val("")
            $('#Email').val("")
        }, 80)
        BuscaClientes();
    }

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


});