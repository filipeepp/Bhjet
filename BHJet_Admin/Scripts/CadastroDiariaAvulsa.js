
function BuscaProfissionais() {
    var jqVariavel = $("#ClienteSelecionado");

    if (jqVariavel.val() != "" && jqVariavel.val() !== undefined) {
        $.ajax({
            dataType: "json",
            type: "GET",
            url: "/Dashboard/BuscaProfissionais",
            success: function (data) {
                if (data !== "" && data !== undefined) {
                jqVariavel.autocomplete({
                    source: data,
                    select: function (el, ui) {


                    }
                });
                }
            }
        });
    }
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

document.addEventListener("DOMContentLoaded", function (event) {

    $('#ClienteSelecionado').click(function (event) {
        $(this).val("");
        event.preventDefault();
        event.stopPropagation();
        return false;
    });

    $("#ClienteSelecionado").keyup(delay(function (e) {
        if (/^\d+$/.test($(this).val())) {
            BuscaProfissionais();
        }
        else {
            if ($(this).val().length >= 3) {
                BuscaProfissionais();
                $(this).next().remove();
            }
            else {
                if ($(this).next().attr("id") != "msgDigitacao") {
                    var para = document.createElement("p");
                    var node = document.createTextNode("Preencha no minímo 3 digitos para pesquisa.");
                    para.appendChild(node);
                    para.id = "msgDigitacao";
                    para.style.position = 'relative';
                    para.style.fontSize = '12px';
                    para.style.color = '#c33939';
                    para.style.display = 'inline';
                    $(this).after(para);

                    var removeAfter = 3000;
                    (function (removeAfter) {
                        setTimeout(function () {
                            $("#ClienteSelecionado").next().remove();
                        }, removeAfter);
                    })(removeAfter)
                }
            }
        }
        $(this).focus();
    }, 500));

    $("#ProfissionalSelecionado").keyup(delay(function (e) {
        if (/^\d+$/.test($(this).val())) {
            BuscaProfissionais();
        }
        else {
            if ($(this).val().length >= 3) {
                BuscaProfissionais();
                $(this).next().remove();
            }
            else {
                if ($(this).next().attr("id") != "msgDigitacao") {
                    var para = document.createElement("p");
                    var node = document.createTextNode("Preencha no minímo 3 digitos para pesquisa.");
                    para.appendChild(node);
                    para.id = "msgDigitacao";
                    para.style.position = 'relative';
                    para.style.fontSize = '12px';
                    para.style.color = '#c33939';
                    para.style.display = 'inline';
                    $(this).after(para);

                    var removeAfter = 3000;
                    (function (removeAfter) {
                        setTimeout(function () {
                            $("#ProfissionalSelecionado").next().remove();
                        }, removeAfter);
                    })(removeAfter)
                }
            }
        }
        $(this).focus();
    }, 500));
});

