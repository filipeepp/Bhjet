

$(document).ready(function () {

    $("#ctnContratoCliente").hide();

    $("input[id*='FranquiaKM'], input[id*='FranquiaMinutosParados'], input[id*='FranquiaHoras']").each(function () {
        $(this).mask("#######0");
    })
    $("input[id*='ValorContrato'], input[id*='ValorKMAdicional'], input[id*='ValorMinutoParado'],  input[id*='ValorHoraAdicional'],  input[id*='ValorPontoExcedente']").each(function () {
        $(this).mask('000.000.000.000.000,00', { reverse: true });
    })

    if ($("input[type=radio][id=Contrato]:checked").val() == "ContratoLocacao") {
        $("#ctnContratoCliente").show();
    } else {
        $("#ctnContratoCliente").hide();
    }

    $('input[type=radio][id=Contrato]').change(function () {
        if ($(this).val() == "ContratoLocacao") {
            $("#ctnContratoCliente").show();
        } else {
            $("#ctnContratoCliente").hide();
        }
    });

    $("input[data-cont='true']").blur(function () {
        window.ValidaCampos(false);
    })

});

window.ValidarValor = function (event) {

    if ($("input[type=radio][id=Contrato]:checked").val() == "ContratoLocacao") {
        var erro = false;
        $("input[data-cont='true']").each(function () {
            if ($(this).val() == "" || $(this).val() == undefined) {
                erro = true;
            }
        })
        if (erro == true) {
            window.ValidaCampos();
            MensagemAlerta("Os Campos destacados são obrigatórios.");
            return false;
            event.preventDefault();
            event.stopPropagation();
        }
        else {
            return true;
        }
    }
    return true;
};


window.ValidaCampos = function (validaErro) {
    $("input[data-cont='true']").each(function () {
        if ($(this).val() == "" || $(this).val() == undefined) {
            if (validaErro != false) {
                $(this).css({
                    "border-color": "red",
                    "border-width": "1px",
                    "border-style": "solid"
                });
            }
        }
        else {
            $(this).css({
                "border-color": "transparent",
                "border-width": "0px",
                "border-style": "solid"
            });
        }
    })
};
