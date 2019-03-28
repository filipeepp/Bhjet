
function bsoc() {
    $.ajax({
        type: "POST",
        url: "../Resumo/CallCB",
        data: null,
        success: function (result) {

            var f = result;
        },
        error: function (status) {

            var f = status;
            $('body').removeClass('modal-open');
            $("#mdSu").modal('show');
            setTimeout(function () {
                $('.modal-backdrop').remove();
            }, 1000);
        }
    });
}

document.addEventListener("DOMContentLoaded", function (event) {

    $("#draggable").draggable();

    //$("#btnEnviar").unbind("click");
    //$("#btnEnviar").click(function () {
    //    bsoc();
    //    event.stopPropagation();
    //});

    carregaMapaDir();
    CalculaRota();
});


function CalculaRota() {
    var log = [];
    var lat = [];
    $("input[id*='_Longitude']").each(function () {
        log.push($(this).val());
    });
    $("input[id*='_Latitude']").each(function () {
        //lat[$(this).attr("name")] = $(this).val();
        lat.push($(this).val());
    });

    var localizacoes = [];
    function locAgp(value, index, array) {
        var latitude = lat[index];
        if (latitude != "" && value != "") {
            localizacao = {
                _lat: latitude,
                _log: value
            }
            localizacoes.push(localizacao);
        }
    }
    log.forEach(locAgp);

    if (localizacoes !== undefined && localizacoes.length > 1) {
        // Calcula pontos seguintes
        var waypts = [];
        var loc = localizacoes.slice(1, (localizacoes.length - 1));
        for (i = 0; i < loc.length; i++) {
            waypts.push({
                location: loc[i]._lat + "," + loc[i]._log,
                stopover: true
            });
        }
        // DS Route
        var origem = localizacoes[0];
        var destino = localizacoes[localizacoes.length - 1];
        if (destino !== null && destino !== undefined) {

            clearAllMarker();

            var request = {
                origin: origem._lat + "," + origem._log, // origem
                destination: destino._lat + "," + destino._log, // destino
                waypoints: waypts,
                optimizeWaypoints: true,
                travelMode: google.maps.TravelMode.DRIVING // meio de transporte, nesse caso, de carro
            };

            directionsService.route(request, function (result, status) {
                if (status == google.maps.DirectionsStatus.OK) {
                    directionsDisplay.setDirections(result);
                }
            });
        }
    } else {
        FazMarcacao(localizacoes[0]._lat, localizacoes[0]._log, true, "Endereço de origem");
    }
}