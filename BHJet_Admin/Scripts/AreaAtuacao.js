// Vr
var map = null;
var areas = new Array();;

function carregaMapa() {
    // Centro do mapa
    var myLatLng = new google.maps.LatLng(-19.878951, -43.933833);
    // Options
    var mapOptions = {
        zoom: 6,
        center: myLatLng,
        mapTypeId: google.maps.MapTypeId.RoadMap
    };
    map = new google.maps.Map(document.getElementById('map_canvas'), mapOptions);
    // LDArea
    BuscaAreasCadastradas();
}

function adicionarArea(triangleCoords) {
    // Area
    var myPolygon2 = new google.maps.Polygon({
        paths: triangleCoords,
        draggable: true,
        editable: true,
        strokeColor: '#ffeb3b',
        strokeOpacity: 0.8,
        strokeWeight: 2,
        fillColor: '#ffeb3b',
        fillOpacity: 0.35
    });

    areas.push(myPolygon2);

    myPolygon2.setMap(map);
    google.maps.event.addListener(myPolygon2, 'rightclick', function (event) {
        if (confirm('Deseja remover esta Área de Atuação ?')) {

            var index = areas.indexOf(this);
            areas.splice(index, 1);
            this.setMap(null);
            getPolygonCoords();
        }
    });

    google.maps.event.addListener(myPolygon2.getPath(), "insert_at", getPolygonCoords);
    google.maps.event.addListener(myPolygon2.getPath(), "set_at", getPolygonCoords);

    getPolygonCoords();
}


function getPolygonCoords() {
    var htmlStr = "[\n ";
    htmlStr += " {\n ";

    for (var i = 0; i < areas.length; i++) {
        htmlStr += "\"Area\": [ \n";
        var polygon = areas[i];
        var len = polygon.getPath().getLength();
        for (var j = 0; j < len; j++) {
            htmlStr += "\n {";
            htmlStr += "\n   \"Latitude\": \"" + polygon.getPath().getAt(j).lat() + "\",";
            htmlStr += "\n   \"Longitude\": \"" + polygon.getPath().getAt(j).lng() + "\"";
            htmlStr += "\n }";

            if (j < (len - 1))
                htmlStr += ",";

        }
        htmlStr += "\n]\n";
    }

    htmlStr += " }\n ";
    htmlStr += "]";

    //document.getElementById('info').value = htmlStr;
    $("#info").val(htmlStr);
}

function initAutocomplete() {
    var input = document.getElementById('pac-input');
    var options = {

    };
    autocomplete = new google.maps.places.Autocomplete(input, options);

    autocomplete.addListener('place_changed', function () {
        var place = autocomplete.getPlace();
        var location = place.geometry.location;

        var cords = [
            new google.maps.LatLng(place.geometry.location.lat() + 1, place.geometry.location.lng() + 1),
            new google.maps.LatLng(place.geometry.location.lat() - 0.5, place.geometry.location.lng() + 1),
            new google.maps.LatLng(place.geometry.location.lat() - 0.5, place.geometry.location.lng() - 0.5),
            new google.maps.LatLng(place.geometry.location.lat() + 1, place.geometry.location.lng() - 0.5),
        ];

        adicionarArea(cords)
        $("#pac-input").val("")
    });
}

function BuscaAreasCadastradas(idCom) {
    $.ajax({
        url: '@Url.Content("~/Atuacao/BuscaAreas")',
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

                adicionarArea(cords);
            });
            if (dados == "" || dados == undefined) {
                $("#msgModal").text("Não foram encontrados Áreas de atuação para ser exibida no mapa.")
                $("#imgMensagem").attr("src", "..\\Images\\warming.png");
                $('#myModal').modal('show')
            }
        },
        error: function (e) {
            var teste = e;
        }
    });
}

function AtualizaAreas() {
    $.ajax({
        url: '../Atuacao/CadastraAreas',
        type: "POST",
        dataType: "json",
        contentType: 'application/json',
        data: $("#info").val(),
        success: function (data) {
            MensagemSucesso("Áreas de atuação atualizadas com sucesso.");
        },
        error: function () {
            $("html, body").animate({
                scrollTop: $(document).height()
            }, 1000);
        }
    });
}

$(document).ready(function () {
    // ---
    carregaMapa();
    // ---
    initAutocomplete();
    // ---
    $("#bntAtualizaAreas").click(function () {

        var test = $("#info").val().replace(/[^\w\s]/gi, '');
        test = test.replace(/\s/g, '');
        if (test == "" || $("#info").val() == "" || $("#info").val() == undefined) {
            MensagemAlerta("Não existem Áreas de atuação selecionadas no mapa.");
            return false;
        }
        else {
            AtualizaAreas();
        }
    })
});
