
function mascaraComissoes() {
    $("input[id*='ValorComissao']").each(function () {
        $(this).maskMoney({
            prefix: "R$ ",
            decimal: ",",
            thousands: "."
        });
    })

    $("input[id*='VigenciaInicio'], input[id*='VigenciaFim']").each(function () {
        $(this).mask("00/00/0000", {
            onComplete: function (a) {

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
        //$("#loading").show()
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
                //var utfstring = unescape(encodeURIComponent(data));
                //var url = '@Html.Raw(Url.Action("Index", new { model = utfstring, adicionarComissao: true }))';
                window.location.href = "/Motorista/Novo?alteraComissao=TRUE";
                $("#confirmaMotorista").mousemove();
                mascaraComissoes();
                //$.get("/Motorista/Novo", { adicionarComissao: true });
            },
            error: function () { $("#confirmaMotorista").focus();}
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
            $("#confirmaMotorista").mousemove();
            mascaraComissoes();
        },
        error: function () { $("#confirmaMotorista").focus(); }
    });
}