
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

var poligonos = new Array();

function BuscaAreasCadastradas() {
    $.ajax({
        url: "Entregas/BuscaAreas",
        dataType: "json",
        type: "GET",
        success: function (dados) {
            var obj = JSON.parse(dados);
            $.each(obj, function (index, value) {
                var cords = [];
                for (var i = 0; i < value.GeoVertices.length; i++) {
                    var lat = value.GeoVertices[i].Latitude;
                    var long = value.GeoVertices[i].Longitude;
                    cords.push(new google.maps.LatLng(lat, long));
                }
                var myPolygon2 = new google.maps.Polygon({
                    paths: cords,
                    draggable: true,
                    editable: true,
                    strokeColor: '#ffeb3b',
                    strokeOpacity: 0.8,
                    strokeWeight: 2,
                    fillColor: '#ffeb3b',
                    fillOpacity: 0.35
                });
                poligonos.push(myPolygon2);
            });
        },
        error: function (e) { }
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

    BuscaAreasCadastradas();

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
            var locationGM = new google.maps.LatLng(location.lat(), location.lng());

            // VACT
            var ectAt = false;
            $.each(poligonos, function (index, value) {
                var existe = google.maps.geometry.poly.containsLocation(locationGM, value);
                if (existe) {
                    ectAt = true;
                }
            });
            if (ectAt) {
                autocomplete.parm.parent().find("input[id*='Latitude']").val(location.lat());
                autocomplete.parm.parent().find("input[id*='Longitude']").val(location.lng());
                if (marker != undefined)
                    marker.setPosition(locationGM);
                map.setCenter(locationGM);
                map.setZoom(16);
                CalculaRota();
            }
            else {
                autocomplete.parm.val("");
                AdicionarErroCampo(autocomplete.parm[0].id, 'Endereço selecionado não está dentro da área de atuação da BHJet.', 10000);
            }
        });
    })


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