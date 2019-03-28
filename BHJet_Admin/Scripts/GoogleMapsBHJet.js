var map = null;
var geocoder;
var directionsDisplay;
var directionsService = new google.maps.DirectionsService();
var marker;
var markers = [];

function carregaMapa() {
    var latlng = new google.maps.LatLng(-19.8157, -43.9542); 
    var myOptions = {
        zoom: 15,
        center: latlng,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);

    map.setCenter(new google.maps.LatLng(-19.878946, -43.933877));
}

function crmp() {
    var latlng = new google.maps.LatLng(-19.8157, -43.9542);
    var myOptions = {
        zoom: 13,
        center: latlng,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    if (map == undefined)
        map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);

    return map;
}

function carregaMapaDir() {
    directionsDisplay = new google.maps.DirectionsRenderer();
    geocoder = new google.maps.Geocoder();

    map = crmp();

    if (navigator.geolocation) { 
        navigator.geolocation.getCurrentPosition(function (position) {

            pontoPadrao = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);
            map.setCenter(pontoPadrao);

            geocoder.geocode({ 
                "location": new google.maps.LatLng(position.coords.latitude, position.coords.longitude)
            },
                function (results, status) {
                    if (status == google.maps.GeocoderStatus.OK) {
                        $("#txtEnderecoPartida").val(results[0].formatted_address);
                    }
                });
        });
    }

    directionsDisplay.setMap(map);

    //directionsDisplay.setPanel(document.getElementById("trajeto-texto"));

    geocoder = new google.maps.Geocoder();

    marker = new google.maps.Marker({
        map: map,
        draggable: true,
    });

    markers.push(marker);

    map.setCenter(new google.maps.LatLng(-19.878946, -43.933877));
}

function clearAllMarker() {
    if (markers != undefined) {
        for (var i = 0; i < markers.length; i++) {
            markers[i].setMap(null);
        }
    }
}


function FazMarcacao(lat, long, bcarro, bdesc) {

    lat = lat.replace(",", ".");
    long = long.replace(",", ".");
    var latlong = lat + "," + long;
    var myLatLgn = new google.maps.LatLng(lat, long);

    map.setZoom(15);
    var marker = new google.maps.Marker({
        position: myLatLgn,
        map: map,
        icon: bcarro == true ? '../Images/mapCorrida.png' : '../Images/mapCar.png',
        animation: google.maps.Animation.DROP
    });

    marker.addListener('click', function () {
        var infowindow = new google.maps.InfoWindow();
        infowindow.setContent("-----------------------</br><b>BH Jet Express</b></br>-----------------------</br>" + bdesc);
        infowindow.open(map, marker);
    });

    markers.push(marker);
    map.setCenter(myLatLgn);
}