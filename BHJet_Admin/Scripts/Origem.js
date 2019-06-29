
function BuscaProfissionais() {
    var jqVariavel = $("#pesquisaProfissional");
    $("#ProfissionalSelecionado").find('option').remove().end();
    var tipoVeiculo = $("input[id='TipoProfissional']:checked").val();;
    var urlInicio = "HomeExterno/BuscaProfissionais?trechoPesquisa=";
    if (window.location.href.indexOf("HomeExterno") > -1) {
        urlInicio = "../" + urlInicio;
    }

    $.ajax({
        dataType: "json",
        type: "GET",
        url: urlInicio + jqVariavel.val() + "&tipoProfissional=" + tipoVeiculo,
        success: function (data) {
            if (data !== "" && data !== undefined) {
                $("#ProfissionalSelecionado").find('option').remove().end();
                var div_data = "<option value=></option>";
                $(div_data).appendTo('#ProfissionalSelecionado');
                $.each(data, function (i, obj) {
                    var div_data = "<option value=" + obj.value + ">" + obj.label + "</option>";
                    $(div_data).appendTo('#ProfissionalSelecionado');
                });
                $("#ProfissionalSelecionado").mouseup();
            }
            else {
                AdicionarErroCampo('ProfissionalSelecionado', 'Não foi possível encontrar o profissional desejado.', 4000);
            }
        }
    });
}

var poligonos = new Array();

function BuscaAreasCadastradas() {
    var urlInicio = "HomeExterno/BuscaAreas";
    if (window.location.href.indexOf("HomeExterno") > -1) {
        urlInicio = "../" + urlInicio;
    }

    $.ajax({
        url: urlInicio,
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

    $("#loading").hide();

    $(".btnSolicitacao").click(function () {
        if ($('#fmOrgPed').valid()) {
            $("#loading").show()
        }
    })

    $("#pesquisaProfissional").keyup(delay(function (e) {
        BuscaProfissionais();
    }, 500));

    $('input[type=radio][name=TipoProfissional]').change(function () {
        $("#ProfissionalSelecionado").find('option').remove().end();
        BuscaProfissionais();
    });

    var input = document.getElementById('txtEnderecoPartida');

    BuscaAreasCadastradas();

    var options = {
        'address': ', Brasil',
        'region': 'PT',
        componentRestrictions: { country: "BR" }
    };
    var autocomplete = new google.maps.places.Autocomplete(input, options);
    autocomplete.setComponentRestrictions({ 'country': 'br' });
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
            $("input[id*='txtEnderecoPartida']").parent().find("input[id*='Latitude']").val(location.lat());
            $("input[id*='txtEnderecoPartida']").parent().find("input[id*='Longitude']").val(location.lng());
            marker.setPosition(locationGM);
            map.setCenter(locationGM);
            map.setZoom(16);
        }
        else {
            $("#txtEnderecoPartida").val("");
            AdicionarErroCampo('txtEnderecoPartida', 'Endereço selecionado não está dentro da área de atuação da BHJet.', 10000);
        }
    });

    carregaMapaDir();
    BuscaProfissionais();
});