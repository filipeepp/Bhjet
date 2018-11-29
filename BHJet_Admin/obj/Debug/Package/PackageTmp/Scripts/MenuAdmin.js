

document.addEventListener("DOMContentLoaded", function (event) {
    $("#loading").hide();
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

    $("#closeMsgGeral").click(function () {
        $("#msgModal").text('')
    })

    $('a').click(function (event) {
        $("#loading").show()
    });

});

function alteraMenu() {

    $(".sidebar-menu").find("li").each(function () {
        $(this).removeClass("active")
        if ($(this)[0].textContent.indexOf(window.location.href.split('/')[3]) !== -1) {
            $(this).addClass("active")
            return false;
        }
        else if ($(this)[0].textContent.indexOf("Início") !== -1 && window.location.href.split('/')[3] == "Home" && window.location.href.split('/')[4] == "Index") {
            $(this).addClass("active")
            return false;
        }
    });

}


function AdicionarErroCampo(idField, message, tempoAtivo) {
    var para = document.createElement("p");
    var node = document.createTextNode(message);
    para.appendChild(node);
    para.id = "msgDigitacao";
    para.style.position = 'relative';
    para.style.fontSize = '12px';
    para.style.color = '#c33939';
    para.style.display = 'inline';
    if ($("#" + idField) != undefined) {
        $("#" + idField).after(para);
    }
    var removeAfter = tempoAtivo;
    (function (removeAfter) {
        setTimeout(function () {
            $("#" + idField).next().remove();
        }, removeAfter);
    })(removeAfter)
}

function delay(callback, ms) {
    var timer = 0;
    return function () {
        var context = this, args = arguments;
        clearTimeout(timer);
        timer = setTimeout(function () {
            callback.apply(context, args);
        }, ms || 0);
    };
}
