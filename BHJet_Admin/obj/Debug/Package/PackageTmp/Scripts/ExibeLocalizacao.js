﻿document.addEventListener("DOMContentLoaded", function (event) {
    function atualizarMarcacoes() {
        $.ajax({
            dataType: "json",
            type: "GET",
            url: "/Dashboard/BuscaLocalizacao",
            success: function (dados) {
                $(dados).each(function (i) {
                    var lat = dados[i].geoPosicao.split(';')[0]
                    var long = dados[i].geoPosicao.split(';')[1]
                    FazMarcacao(lat, long, dados[i].psCorrida)
                });
            }
        });
    }
    carregaMapa();
    atualizarMarcacoes();
});