
document.addEventListener("DOMContentLoaded", function (event) {

    $("#ctnLg").show("slow");
    $("#ctnReg").hide("slow");
    $(".modal-title").text("Acessar minha conta BHJet:");

    $("#btnCrCC").click(function () {
        $("#ctnLg").hide("slow");
        $("#ctnReg").show("slow");
        $("#mdSu").css('overflow-y', 'initial');
        $("#mdSu").css('margin-top', 'unset');
        $(".modal-title").text("Criar conta BHJet");
    });

    $("#btnJaPossui").click(function () {
        $("#ctnReg").hide("slow");
        $("#ctnLg").show("slow");
        $(".modal-title").text("Acessar minha conta BHJet:");
    });

});