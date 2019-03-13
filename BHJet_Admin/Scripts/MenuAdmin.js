,0

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

    $('#menuLateral a').click(function (event) {
        $("#loading").show()
    });

});

function parseDMY(value) {
    var date = value.split("/");
    var d = parseInt(date[0], 10),
        m = parseInt(date[1], 10),
        y = parseInt(date[2], 10);
    return new Date(y, m - 1, d);
}

function isValidDate(txtDate) {
    var currVal = txtDate;
    if (currVal == '')
        return false;
    //Declare Regex 
    var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;
    var dtArray = currVal.match(rxDatePattern); // is format OK?
    if (dtArray == null)
        return false;
    //Checks for mm/dd/yyyy format.
    dtDay = dtArray[1];
    dtMonth = dtArray[3];
    dtYear = dtArray[5]; 
    if (dtMonth < 1 || dtMonth > 12)
        return false;
    else if (dtDay < 1 || dtDay > 31)
        return false;
    else if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay == 31)
        return false;
    else if (dtMonth == 2) {
        var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
        if (dtDay > 29 || (dtDay == 29 && !isleap))
            return false;
    }
    return true;

}

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

function MensagemAlerta(msg) {
    $("#msgModal").text(msg)
    $("#imgMensagem").attr("src", "..\\Images\\warming.png");
    $('#myModal').modal('show')
}

function MensagemSucesso(msg) {
    $("#msgModal").text(msg)
    $("#imgMensagem").attr("src", "..\\Images\\sucesso.png");
    $('#myModal').modal('show')
}

function Carregamento() {
   // $("#loading").show();
}


function AdicionarErroCampo(idField, message, tempoAtivo) {
    var para = document.createElement("p");
    var node = document.createTextNode(message);
    para.appendChild(node);
    para.className = "msgDigitacao";
    para.style.position = 'relative';
    para.style.fontSize = '12px';
    para.style.color = '#c33939';
    para.style.display = 'inline';
    if ($("#" + idField) != undefined) {
        if ($("#" + idField).next().hasClass("msgDigitacao")) {
            $("#" + idField).next().remove();
        }
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
