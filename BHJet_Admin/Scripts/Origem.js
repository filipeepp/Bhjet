document.addEventListener("DOMContentLoaded", function (event) {

    //$("#trajeto-texto").change(function () {
    //    $(".adp-directions").hide();// Esconde trajeto e mostra apenas ponteiro
    //});

    $("#loading").hide();

    $(".btnSolicitacao").click(function () {
        if ($('#fmOrgPed').valid()) {
            $("#loading").show()
        }
    })

    //Auto completa os campos de endereço de partida e de chegada
    var input = document.getElementById('txtEnderecoPartida');
    var options = {
        'address': ', Brasil',
        'region': 'BR'
    };
    var autocomplete = new google.maps.places.Autocomplete(input, options);
    autocomplete.addListener('place_changed', function () {
        var place = autocomplete.getPlace();
        var location = place.geometry.location;

        $("input[id*='txtEnderecoPartida']").parent().find("input[id*='Latitude']").val(location.lat());
        $("input[id*='txtEnderecoPartida']").parent().find("input[id*='Longitude']").val(location.lng());
        var locationGM = new google.maps.LatLng(location.lat(), location.lng());
        marker.setPosition(locationGM);
        map.setCenter(locationGM);
        map.setZoom(16);
    });

    carregaMapaDir();
});