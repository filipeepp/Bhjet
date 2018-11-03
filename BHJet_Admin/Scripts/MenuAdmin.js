
document.addEventListener("DOMContentLoaded", function (event) {

    $("#menuLogisticaMobile").click(function () {

        var menu = $(".main-sidebar");
        if (menu.css("display") === "none" || menu.css("display") == undefined) {

            menu.animate({
                opacity: 0.25,
                left: "+=0",
                height: "toggle"
            }, 400, function () {
                menu.css('display', 'inline-block');
                menu.css('opacity', '1');
                $("#menuLogisticaMobile").find('i').removeClass();
                $("#menuLogisticaMobile").find('i').addClass("fa fa-bars");
                $("#menuLogisticaMobile").find('i').text('')
                });
        }
        else {
            menu.animate({
                opacity: 0.25,
                right: "+=50",
                height: "toggle"
            }, 400, function () {
                menu.css('display', 'none');
                menu.css('opacity', '1');

                $("#menuLogisticaMobile").find('i').removeClass();
                $("#menuLogisticaMobile").find('i').addClass("fa fa-angle-double-down");
                $("#menuLogisticaMobile").find('i').text(' menu')
            });
        }
    })

});