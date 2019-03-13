document.addEventListener("DOMContentLoaded", function (event) {
    $("#loading").hide();

    $("#closeMsgGeral").click(function () {
        $("#msgModal").text('')
    })

    $('#menuLateral a').click(function (event) {
        $("#loading").show()
    });

});