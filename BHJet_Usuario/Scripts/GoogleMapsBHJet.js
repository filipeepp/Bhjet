//variavel cria para que seja criado o mapa Google Maps
var map = null;
var geocoder;
var directionsDisplay;
var directionsService = new google.maps.DirectionsService();
var marker;

//Essa e a funcao que criara o mapa GoogleMaps
function carregaMapa() {
    //aqui vamos definir as coordenadas de latitude e longitude no qual
    //sera exibido o nosso mapa
	directionsDisplay = new google.maps.DirectionsRenderer();
	geocoder = new google.maps.Geocoder();

    var latlng = new google.maps.LatLng(-19.8157, -43.9542); //DEFINE A LOCALIZAÇÃO EXATA DO MAPA
    //aqui vamos configurar o mapa, como o zoom, o centro do mapa, etc
    var myOptions = {
        zoom: 15,//utilizaremos o zoom 15
        center: latlng,//aqui a nossa variavel constando latitude e
        //longitude ja declarada acima
        mapTypeId: google.maps.MapTypeId.ROADMAP
	};

	if (navigator.geolocation) { // Se o navegador do usuário tem suporte ao Geolocation
		navigator.geolocation.getCurrentPosition(function (position) {

			pontoPadrao = new google.maps.LatLng(position.coords.latitude, position.coords.longitude); // Com a latitude e longitude que retornam do Geolocation, criamos um LatLng
			map.setCenter(pontoPadrao);

			geocoder.geocode({ // Usando nosso velho amigo geocoder, passamos a latitude e longitude do geolocation, para pegarmos o endereço em formato de string
				"location": new google.maps.LatLng(position.coords.latitude, position.coords.longitude)
			},
				function (results, status) {
					if (status == google.maps.GeocoderStatus.OK) {
						$("#txtEnderecoPartida").val(results[0].formatted_address);
					}
				});
		});
	}

    //criando o mapa dentro da div com o id="map_canvas" que ja criamos
	map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);
	directionsDisplay.setMap(map);

	//Mostra trajeto
	directionsDisplay.setPanel(document.getElementById("trajeto-texto")); 

	geocoder = new google.maps.Geocoder();

	marker = new google.maps.Marker({
		map: map,
		draggable: true,
	});

    //DEFINE AS COORDENADAS do ponto exato - CENTRALIZAÇÃO DO MAPA
	map.setCenter(new google.maps.LatLng(-19.878946, -43.933877));

	//initialize();
}

function FazMarcacao(lat, long, bcarro, bdesc) {
	
    lat = lat.replace(",", ".");
    long = long.replace(",", ".");
    var latlong = lat + "," + long;//colocando na conficuracao necessaria (lat,long)
    var myLatLgn = new google.maps.LatLng(lat, long);
    //criando variavel tipo google.maps.LatLng e
    //passando como parametro a latitude e longitude
    //na configuracao: latitude,longitude.


    //aproximando o mapa, aumentando o zoom
    map.setZoom(15);

    //fazendo  a marcacao, usando o latitude e longitude da variavel criada acima
    marker = new google.maps.Marker({
        position: myLatLgn,
        map: map,
        icon: bcarro == true ? '/./Images/mapCorrida.png' : '/./Images/mapCar.png',
        animation: google.maps.Animation.DROP
    });

    marker.addListener('click', function () {
        var infowindow = new google.maps.InfoWindow();
        infowindow.setContent("-----------------------</br><b>BH Jet Express</b></br>-----------------------</br>" + bdesc);
        infowindow.open(map, marker);
    });


    //aqui a variavel e o comando que faz a marcação
    map.setCenter(myLatLgn);//leva o mapa para a posicao da marcacao
}