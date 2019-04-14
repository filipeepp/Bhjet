﻿

$(document).ready(function () {

    $(".inputDesc").prop("disabled", true); //Disable

    $("input[id*='FranquiaKM'], input[id*='FranquiaMinutosParados'], input[id*='FranquiaHoras']").each(function () {
        $(this).mask("#######0");
    })
    $("input[id*='ValorPontoColeta'], input[id*='ValorContrato'], input[id*='ValorKMAdicional'], input[id*='ValorMinutoParado'],  input[id*='ValorHoraAdicional'],  input[id*='ValorPontoExcedente']").each(function () {
        $(this).mask('000.000.000.000.000,00', { reverse: true });
    })


})