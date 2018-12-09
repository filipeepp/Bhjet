function ExcluirUsuario(idUser) {

    $.ajax({
        dataType: "json",
        type: "GET",
        url: "/Usuario/DeletaUsuario?id=" + idUser,
        success: function (dados) {
            $("#loading").hide();
            $("#msgModal").text(dados)
            $("#imgMensagem").attr("src", "\\.\\Images\\sucesso.png");
            $('#myModal button').click(function () {
                window.location.href = '/Usuario/Index/';
            })
            $('#myModal').modal('show')
        },
        error: function (dadosEr) {
            $("#loading").hide();
            $("#msgModal").text('Não foi possível excluir o usuário selecionado, tente novamente mais tarde.')
            $("#imgMensagem").attr("src", "\\.\\Images\\warming.png");
            $('#myModal').modal('show')
        }
    });
}

function AlterarSituacao(idUser, situacao) {
    var queryString = "situacao=" + situacao + "&" + "id=" + idUser;
    $.ajax({
        dataType: "json",
        type: "GET",
        url: "/Usuario/AlteraSituacao?" + queryString,
        success: function (dados) {
            $("#loading").hide();
            $("#msgModal").text(dados)
            $("#imgMensagem").attr("src", "\\.\\Images\\sucesso.png");
            $('#myModal button').click(function () {
                window.location.href = '/Usuario/Index/';
            })
            $('#myModal').modal('show')
        },
        error: function (dadosEr) {
            $("#loading").hide();
            $("#msgModal").text('Não foi possível alterar a situação do usuário selecionado, tente novamente mais tarde.')
            $("#imgMensagem").attr("src", "\\.\\Images\\warming.png");
            $('#myModal').modal('show')
        }
    });
}


function BuscaClientes() {
    var jqVariavel = $("#pesquisaCliente");
    $.ajax({
        dataType: "json",
        type: "GET",
        url: "/Usuario/BuscaClientes?trechoPesquisa=" + jqVariavel.val(),
        success: function (data) {
            if (data !== "" && data !== undefined && data.length > 0) {
                $("#novo_ClienteSelecionado").find('option').remove().end();
                var div_data = "<option value=></option>";
                $(div_data).appendTo('#novo_ClienteSelecionado');
                $.each(data, function (i, obj) {
                    var div_data = "<option value=" + obj.value + ">" + obj.label + "</option>";
                    $(div_data).appendTo('#novo_ClienteSelecionado');
                });
                $("#novo_ClienteSelecionado").mouseup();
            }
            else {
                AdicionarErroCampo('ClienteSelecionado', 'Não foi possível encotrar o cliente desejado.', 4000);
            }
        },
    });

}

function necessarioCliente() {
    var $option = $("#novo_TipoUser").prop('selectedIndex');
    if ($option != undefined && ($option === 2 || $option === 3)) {
        $("#cliRefer").css("display", "unset")
        return true;
    }
    else {
        $("#cliRefer").css("display", "none")
        return false;
    }
    return false;
}

$("#pesquisaCliente").keyup(delay(function (e) {
    BuscaClientes();
}, 500));

document.addEventListener("DOMContentLoaded", function (event) {

    $("#novo_TipoUser").val(0);
    BuscaClientes();
    setTimeout(function afterTwoSeconds() {
        $('#novo_Email').val("")
        $('#novo_Senha').val("")
    }, 80)

    $("#novo_TipoUser").change(function () {
        necessarioCliente();
    });

    $("#novo_ClienteSelecionado").change(function () {
        $("#novo_ClienteSelecionado").css('border-color', '#ccc');
    });

    $("#confirmaMotorista").click(function () {
        if (necessarioCliente()) {
            var index = $("#novo_ClienteSelecionado").prop('selectedIndex');
            if (index === undefined || index === "undefined" || index == 0) {
                $("#novo_ClienteSelecionado").css('border-color', 'red');
                MensagemAlerta("Selecione um cliente para prosseguir.");
                event.preventDefault();
                event.stopPropagation();
                return false;
            }
            else {
                $("#novo_ClienteSelecionado").css('border-color', '#ccc');
            }
        }
    })
});