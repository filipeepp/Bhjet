document.addEventListener("DOMContentLoaded", function (event) {

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
		}
	});



	carregaMapa();
});