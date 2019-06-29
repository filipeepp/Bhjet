
function bsoc() {
    $.ajax({
        dataType: "json",
        type: "GET",
        url: "Entregas/BcTpOc",
        success: function (data) {
            if (data !== "" && data !== undefined && data.length > 0) {

                var fields = $("select[id*='__TipoOcorrencia']");

                fields.each(function () {
                    var fld = $(this);
                    fld.find('option').remove().end();
                    $.each(data, function (i, obj) {
                        fld.append($("<option />").val(obj.value).text(obj.label));
                    });
                    fld.mouseup();
                });
            }
        },
    });
}



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

    $("#loading").hide();
    if ($('.owl-item').length <= 1) {
        $('.owl-stage').removeAttr("style");
        $('.owl-item').removeAttr("style");
    }

    $('input[type="submit"]').click(function () {
        if ($('#frmPc').valid()) {
            $("#loading").show()
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


    $("input[id*='__Descricao']").each(function () {
        var input = document.getElementById($(this).attr('id'));
        var options = {
            'address': ', Brasil',
            'region': 'PT',
            componentRestrictions: { country: "BR" }
        };
        var autocomplete = new google.maps.places.Autocomplete(input, options);
        autocomplete.setComponentRestrictions({ 'country': 'br' });
        autocomplete.parm = $(this);
        autocomplete.addListener('place_changed', function () {
            var place = autocomplete.getPlace();
            var location = place.geometry.location;

            autocomplete.parm.parent().find("input[id*='Latitude']").val(location.lat());
            autocomplete.parm.parent().find("input[id*='Longitude']").val(location.lng());
            var locationGM = new google.maps.LatLng(location.lat(), location.lng());
            if (marker != undefined)
                marker.setPosition(locationGM);
            map.setCenter(locationGM);
            map.setZoom(16);
            CalculaRota();
        });
    })


    function addChange(autocomplete, control) {

    }

    bsoc();
    carregaMapaDir();
    CalculaRota();
});

function changeButtonClicked() {
    var url = '@Url.Action("Finaliza", "Entregas")';
    $.post("Entregas/Finaliza", $('#frmPc').serialize(), function (view) {
        location.reload();
    });
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
                }
            });
        }
    } else {
        FazMarcacao(localizacoes[0]._lat, localizacoes[0]._log, true, "Endereço de origem");
    }
}