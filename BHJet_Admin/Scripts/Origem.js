
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
        url: urlInicio  + jqVariavel.val() + "&tipoProfissional=" + tipoVeiculo,
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

    var options = {
        'address': ', Brasil',
        'region': 'PT',
        componentRestrictions: { country: "BR" }
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
    BuscaProfissionais();
});