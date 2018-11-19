//variavel cria para que seja criado o mapa Google Maps
var map = null;

//Essa e a funcao que criara o mapa GoogleMaps
function carregaMapa() {
    //aqui vamos definir as coordenadas de latitude e longitude no qual
    //sera exibido o nosso mapa
    var latlng = new google.maps.LatLng(-19.8157, -43.9542); //DEFINE A LOCALIZAÇÃO EXATA DO MAPA
    //aqui vamos configurar o mapa, como o zoom, o centro do mapa, etc
    var myOptions = {
        zoom: 15,//utilizaremos o zoom 15
        center: latlng,//aqui a nossa variavel constando latitude e
        //longitude ja declarada acima
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    //criando o mapa dentro da div com o id="map_canvas" que ja criamos
    map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);

    //DEFINE AS COORDENADAS do ponto exato - CENTRALIZAÇÃO DO MAPA
    map.setCenter(new google.maps.LatLng(-19.878946, -43.933877));
}

function FazMarcacao(lat, long, bcarro) {

    var latlong = lat + "," + long;//colocando na conficuracao necessaria (lat,long)
    var myLatLgn = new google.maps.LatLng(lat, long);
    //criando variavel tipo google.maps.LatLng e
    //passando como parametro a latitude e longitude
    //na configuracao: latitude,longitude.

    //aproximando o mapa, aumentando o zoom
    map.setZoom(15);

    //fazendo  a marcacao, usando o latitude e longitude da variavel criada acima
    var marker = new google.maps.Marker({
        position: myLatLgn,
        map: map,
        icon: bcarro == true ? '/./Images/mapCorrida.png' : '/./Images/mapCar.png',
        animation: google.maps.Animation.DROP
    });

    marker.addListener('click', function () {
        var infowindow = new google.maps.InfoWindow();
        infowindow.setContent("<b>BH Jet Express.</b>");
        infowindow.open(map, marker);
    });

    //aqui a variavel e o comando que faz a marcação
    map.setCenter(myLatLgn);//leva o mapa para a posicao da marcacao
}