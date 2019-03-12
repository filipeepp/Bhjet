
document.addEventListener("DOMContentLoaded", function (event) {

    $("#draggable").draggable();
    $("#ctnOrigem").hide();
    $('.owl-carousel').owlCarousel({
        loop: true,
        margin: 10,
        responsiveClass: true,
        responsive: {
            0: {
                items: 1,
                nav: true
            },
            600: {
                items: 1,
                nav: false
            },
            1000: {
                items: 1,
                nav: true,
                loop: false,
                margin: 20
            }
        }
    })

    $("#ctnOrigemTitulo").click(function () {
        var ctn = $("#ctnOrigem");
        if (ctn.is(":hidden")) {
            $("#iconeExpandeOrigem").attr('class', 'fa fa-minus');
            ctn.show("slow");
        }
        else {
            $("#iconeExpandeOrigem").attr('class', 'fa fa-plus');
            ctn.hide("slow");
        }
    });

    $("#trajeto-texto").change(function () {
        $(".adp-directions").hide();// Esconde trajeto e mostra apenas ponteiro
    });

    //Auto completa os campos de endereço de partida e de chegada
    $("input[id*='txtEnderecoPartida']").autocomplete({
        source: function (request, response) {
            geocoder.geocode({
                'address': request.term + ', Brasil',
                'region': 'BR'
            }, function (results, status) {
                response($.map(results, function (item) {
                    return {
                        label: item.formatted_address,
                        value: item.formatted_address,
                        latitude: item.geometry.location.lat(),
                        longitude: item.geometry.location.lng()
                    }
                }));
            })
        },
        select: function (event, ui) {
            $(this).parent().find("input[id*='Latitude']").val(ui.item.latitude);
            $(this).parent().find("input[id*='Longitude']").val(ui.item.longitude);
            var location = new google.maps.LatLng(ui.item.latitude, ui.item.longitude);
            marker.setPosition(location);
            map.setCenter(location);
            map.setZoom(16);

            CalculaRota();
        }
    });

    carregaMapa();
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
        localizacao = {
            _lat: latitude,
            _log: value
        }
        localizacoes.push(localizacao);
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
        FazMarcacao(localizacoes[0]._lat, localizacoes[0]._log, true, "");
    }
}