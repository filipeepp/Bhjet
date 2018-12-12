﻿function ExcluirUsuario(idUser) {

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


document.addEventListener("DOMContentLoaded", function (event) {

    setTimeout(function afterTwoSeconds() {
        $('#novo_Email').val("")
        $('#novo_Senha').val("")
    }, 80)



});