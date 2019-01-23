
function mascaraComissoes() {
    $("input[id*='ValorComissao']").each(function () {
        $(this).maskMoney({
            prefix: "% ",
            decimal: ",",
            thousands: "."
        });
    })

    $("input[id*='VigenciaInicio'], input[id*='VigenciaFim']").each(function () {
        $(this).mask("00/00/0000", {
            onComplete: function (a) {
                //if (!isValidDate(a)) {
                //    var teste = $('input[text*=' + a + ']');
                //    teste.val();
                //    AdicionarErroCampo(teste.attr('id'), 'Favor preencher uma data válida.', 10000);
                //}
            },
            placeholder: "__/__/____"
        });

        $(this).datepicker({
            dateFormat: "dd/mm/yy",
            dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'],
            dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S', 'D'],
            dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'],
            monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
            monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
            nextText: 'Próximo',
            prevText: 'Anterior',
            minDate: 0,
            onSelect: function () {
                $("#PeriodoInicial").val($("#PeriodoInicial").val() + " 09:00")
            }
        });
    })
}

document.addEventListener("DOMContentLoaded", function (event) {
    $("#loading").hide()
    mascaraComissoes();
    $("#TelefoneResidencial").mask("(00) 0000-0000");
    $("#TelefoneCelular").mask("(00) 0000-00009");

    var cpfMascara = function (val) {
        return val.replace(/\D/g, '').length > 11 ? '00.000.000/0000-00' : '000.000.000-009';
    },
        cpfOptions = {
            onKeyPress: function (val, e, field, options) {
                field.mask(cpfMascara.apply({}, arguments), options);
            }
        };
    $('#CpfCnpj').mask(cpfMascara, cpfOptions);

    $('#confirmaMotorista').click(function (event) {
        //ms
        var erro;
        var t = "";
        //vld
        $(".rowList").each(function () {
            var comissao = $(this).find("input[id*='ValorComissao']")
            var vgInicio = $(this).find("input[id*='VigenciaInicio']")
            var vgFim = $(this).find("input[id*='VigenciaFim']")
            if ((comissao.val() != "" || vgInicio.val() != "" || vgFim.val() != "") || $(".rowList").length > 1) {
                //com
                if (comissao.val() == "") {
                    comissao.css('border-color', 'red');
                    AdicionarErroCampo(comissao.attr('id'), 'Favor preencher um valor para a comissão.', 10000);
                    erro = true;
                }
                else {
                    comissao.css('border-color', '#ccc');
                }
                //vgIni
                if (vgInicio.val() == "") {
                    vgInicio.css('border-color', 'red');
                    AdicionarErroCampo(vgInicio.attr('id'), 'Favor preencher um valor para a data de vigência inicial.', 10000);
                    erro = true;
                }
                else {
                    if (!isValidDate(vgInicio.val())) {
                        vgInicio.css('border-color', 'red');
                        AdicionarErroCampo(vgInicio.attr('id'), 'Favor preencher uma data válida.', 10000);
                        erro = true;
                    } else {
                        vgInicio.css('border-color', '#ccc');
                    }
                }
                //vgFim
                if (vgFim.val() == "") {
                    vgFim.css('border-color', 'red');
                    AdicionarErroCampo(vgFim.attr('id'), 'Favor preencher um valor para a data de vigência final.', 10000);
                    erro = true;
                }
                else {
                    if (!isValidDate(vgFim.val())) {
                        vgFim.css('border-color', 'red');
                        AdicionarErroCampo(vgFim.attr('id'), 'Favor preencher uma data válida.', 10000);
                        erro = true;
                    } else {
                        vgFim.css('border-color', '#ccc');
                    }
                }
            }
            else {
                comissao.css('border-color', '#ccc');
                vgInicio.css('border-color', '#ccc');
                vgFim.css('border-color', '#ccc');
            }
        })
        // err
        if (erro) {
            MensagemAlerta("Os Campos destacados são obrigatórios.");
            event.preventDefault();
            event.stopPropagation();
            return false;
        }
    });

    function limpaEndereco() {
        // Limpa valores do formulário de cep.
        $("#Rua").val("");
        $("#Bairro").val("");
        $("#Cidade").val("");
        $("#UF").val("");
    }

    //CEP
    var endereco = {
        Cep: $("#Cep"),
        Rua: $("#Rua"),
        Bairro: $("#Bairro"),
        Cidade: $("#Cidade"),
        Estado: $("#UF"),
        NumeroEndereco: $("#RuaNumero")
    }
    $('#Cep').mask('00000-000', GetOptionsViaCep(endereco));

    $("#btnNovoContatos").click(function () {
        $.ajax({
            url: '/Motorista/AddComissao',
            type: "POST",
            dataType: "html",
            data: $("#DetalheMotorista").serialize(),
            success: function (data) {
                window.location.href = "/Motorista/Novo?alteraComissao=TRUE";
                $("html, body").animate({ scrollTop: $(document).height() }, 1000);
                mascaraComissoes();
            },
            error: function () { $("html, body").animate({ scrollTop: $(document).height() }, 1000); }
        });
    })
});

function removeComissao(idCom) {
    $.ajax({
        url: '/Motorista/ExcluirComissao?numeroComissao=' + idCom,
        type: "POST",
        dataType: "html",
        data: $("#DetalheMotorista").serialize(),
        success: function (data) {
            window.location.href = "/Motorista/Novo?alteraComissao=TRUE";
            mascaraComissoes();
            $("html, body").animate({ scrollTop: $(document).height() }, 1000);
        },
        error: function () { $("html, body").animate({ scrollTop: $(document).height() }, 1000); }
    });
}