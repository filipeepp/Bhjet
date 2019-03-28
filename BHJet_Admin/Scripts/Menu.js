document.addEventListener("DOMContentLoaded", function (event) {
    $("#loading").hide();
    $("#map_canvas").css('height', $(window).height() - $(".navExt").height())
    $(".drop").mouseover(function () {
        $(this).find("ul").first().show(300);
        });
    $(".drop").mouseleave(function () {
        $(this).find("ul").first().hide(300);
        });
    $("#closeMsgGeral").click(function () {
        $("#msgModal").text('')
    })

    $('#menuLateral a').click(function (event) {
        $("#loading").show()
    });
});