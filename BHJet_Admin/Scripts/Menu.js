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