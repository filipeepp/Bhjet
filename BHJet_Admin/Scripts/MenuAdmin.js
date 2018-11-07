
document.addEventListener("DOMContentLoaded", function (event) {

    alteraMenu();

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
                if ($(window).width() > 1112) {
                    $(".content-wrapper").first().css('padding-left', "230px")
                }
                else {
                    $(".content-wrapper").first().css('padding-left', "0px")
                }
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
                if ($(window).width() > 1112) {
                    $(".content-wrapper").first().css('padding-left', "0px")
                }
                else {
                    $(".content-wrapper").first().css('padding-left', "0px")
                }
            });
        }
    })

});

function alteraMenu() {

    $(".sidebar-menu").find("li").each(function () {
        $(this).removeClass("active")
        if ($(this)[0].textContent.indexOf(window.location.href.split('/')[3]) !== -1) {
            $(this).addClass("active")
            return false;
        }
    });

}