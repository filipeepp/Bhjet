

function AlterarSituacao(idUser, situacao) {
    var queryString = "situacao=" + situacao + "&" + "id=" + idUser;
    $.ajax({
        dataType: "json",
        type: "GET",
        url: "AlteraSituacao?" + queryString,
        success: function (dados) {
            $("#loading").hide();
            $("#msgModal").text(dados)
            $("#imgMensagem").attr("src", "..\\Images\\sucesso.png");
            $('#myModal button').click(function () {
                window.location.href = 'Listar';
            })
            $('#myModal').modal('show')
        },
        error: function (dadosEr) {
            $("#loading").hide();
            $("#msgModal").text('Não foi possível alterar a situação do usuário selecionado, tente novamente mais tarde.')
            $("#imgMensagem").attr("src", "..\\Images\\warming.png");
            $('#myModal').modal('show')
        }
    });
}

function AlterarSituacao(idUser, situacao) {
    var queryString = "situacao=" + situacao + "&" + "id=" + idUser;
    $.ajax({
        dataType: "json",
        type: "GET",
        url: "AlteraSituacao?" + queryString,
        success: function (dados) {
            $("#loading").hide();
            $("#msgModal").text(dados)
            $("#imgMensagem").attr("src", "..\\Images\\sucesso.png");
            $('#myModal button').click(function () {
                window.location.href = 'Listar';
            })
            $('#myModal').modal('show')
        },
        error: function (dadosEr) {
            $("#loading").hide();
            $("#msgModal").text('Não foi possível alterar a situação do usuário selecionado, tente novamente mais tarde.')
            $("#imgMensagem").attr("src", "..\\Images\\warming.png");
            $('#myModal').modal('show')
        }
    });
}

function EditarUsuario(idUser) {
    editarUsuarioNavegacao(idUser);
}

document.addEventListener("DOMContentLoaded", function (event) {
    $(".cst-tbl-linhaHover").click(function (id) {
        editarUsuarioNavegacao(id);
    });
});

function editarUsuarioNavegacao(idUser) {
    window.location = 'Novo?Edicao=true&ID=' + idUser;
}