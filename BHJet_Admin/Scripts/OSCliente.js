
var lines = [];
var rotas = [];

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

    desenhaLinhas();
    carregaMapaDir();

    $("#map_canvas").css("position", "absolute");
    $("#map_canvas").height($("#prContent").height() + 150);

    CalculaRota();
});

function desenhaLinhas() {
    lines = [];
    $(".boxRotas").each(function () {
        var id = $(this).prop('id');
        window.rotas.push(document.getElementById(id))
    })

    for (var i = 0; i < rotas.length; i++) {
        var origem = rotas[i];
        var next = i + 1;
        var destino = rotas[next];

        if (destino == undefined) {
            continue;
        }

        var myLine = new LeaderLine(origem, destino,
            {
                dash: {
                    animation: true
                }
            }
        );
        myLine.setOptions({ startSocket: 'bottom', endSocket: 'top', positionByWindowResize: true, color: '#cecccc' });
        lines.push(myLine);
    }
}

function closePopover() {
    $('label[rel=popover]').popover('hide');
}

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
                    var totalDistance = 0;
                    var totalDuration = 0;
                    var legs = result.routes[0].legs;
                    for (var i = 0; i < legs.length; ++i) {
                        totalDistance += legs[i].distance.value;
                        totalDuration += legs[i].duration.value;
                    }
                    var dkmc = Math.round(totalDistance / 100) / 10;
                    $('#QtdKM').text(dkmc + " km"); $('#QsusasnstsisdsasdsesKsM').val(dkmc);
                }
            });
        }
    } else {
        FazMarcacao(localizacoes[0]._lat, localizacoes[0]._log, true, "Endereço de origem");
    }
}