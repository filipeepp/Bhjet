

document.addEventListener("DOMContentLoaded", function (event) {

    $("#ctnDados").hide();
    $("#ctnEndereco").hide();

    $("#icnDados").click(function () {
        var ctn = $("#ctnDados");
        if (ctn.is(":hidden")) {
            $("#icnExpDados").attr('class', 'fa fa-minus');
            ctn.show("slow");
        }
        else {
            $("#icnExpDados").attr('class', 'fa fa-plus');
            ctn.hide("slow");
        }
    });

    $("#icnEnd").click(function () {
        var ctn = $("#ctnEndereco");
        if (ctn.is(":hidden")) {
            $("#icnExpEND").attr('class', 'fa fa-minus');
            ctn.show("slow");
        }
        else {
            $("#icnExpEND").attr('class', 'fa fa-plus');
            ctn.hide("slow");
        }
    });

});