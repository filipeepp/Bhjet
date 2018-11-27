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