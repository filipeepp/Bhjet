//variavel cria para que seja criado o mapa Google Maps
var map = null;
var areas = new Array(); ;

//Essa e a funcao que criara o mapa GoogleMaps
function carregaMapa() {
    // Centro do mapa
    var myLatLng = new google.maps.LatLng(-19.878951, -43.933833);
    //  Options
    var mapOptions = {
        zoom: 7,
        center: myLatLng,
        mapTypeId: google.maps.MapTypeId.RoadMap
    };
    map = new google.maps.Map(document.getElementById('map_canvas'), mapOptions);

    // new google.maps.LatLng(-16.81286,-46.54554), new google.maps.LatLng(-16.4904,-41.18104), new google.maps.LatLng(-21.20714,-42.74728), new google.maps.LatLng(-19.50577,-48.9168), 
    // Cordenadas
    var triangleCoords = [
        new google.maps.LatLng(-17.002071, -47.072885),
        new google.maps.LatLng(-15.719882, -44.367077),
        new google.maps.LatLng(-21.512188, -43.654345)
    ];

    // SArea
    myPolygon = new google.maps.Polygon({
        paths: triangleCoords,
        draggable: true, 
        editable: true,
        strokeColor: '#ffeb3b',
        strokeOpacity: 0.8,
        strokeWeight: 2,
        fillColor: '#ffeb3b',
        fillOpacity: 0.35,
       
    });

    areas.push(myPolygon);

    myPolygon.setMap(map);
    google.maps.event.addListener(myPolygon, 'rightclick', function (event) {
        if (confirm('Deseja remover esta Área de Atuação ?')) {
            myPolygon.setMap(null);
        }
    });
    google.maps.event.addListener(myPolygon.getPath(), "insert_at", getPolygonCoords);
    google.maps.event.addListener(myPolygon.getPath(), "set_at", getPolygonCoords);
}


function adicionarArea(triangleCoords) {

    // Area
    myPolygon2 = new google.maps.Polygon({
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
            myPolygon.setMap(null);
        }
    });
    google.maps.event.addListener(myPolygon2.getPath(), "insert_at", getPolygonCoords);
    google.maps.event.addListener(myPolygon2.getPath(), "set_at", getPolygonCoords);
}


function getPolygonCoords() {

    var htmlStr = "";

    for (var i = 0; i < areas.length; i++) {
        htmlStr += "\n Area " + i + " \n";
        var polygon = areas[i];
        var len = polygon.getPath().getLength();
        for (var j = 0; j < len; j++) {
            htmlStr += "new google.maps.LatLng(" + polygon.getPath().getAt(j).toUrlValue(5) + "), ";
        }
    }

    document.getElementById('info').innerHTML = htmlStr;
}

function copyToClipboard(text) {
    window.prompt("Copy to clipboard: Ctrl+C, Enter", text);
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


    });
}


$(document).ready(function () {
    carregaMapa();

    initAutocomplete();

    //$("#pac-input").keyup(delay(function (e) {
    //    if ($(this).val() != "") {
    //        initAutocomplete();
    //    }
    //}, 500));

});
