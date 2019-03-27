

function buscaGraficoResumoSituacaoChamados() {
    $.ajax({
        dataType: "json",
        type: "GET",
        url: "Dashboard/BuscaResumoSituacaoChamados",
        success: function (dados) {

            var lbls = [];
            var DataCh1 = [];
            var DataCh2 = [];

            $(dados).each(function (i) {
                lbls.push(month[dados[i].Mes]);
                DataCh1.push(dados[i].ChamadosConcluidos);
                DataCh2.push(dados[i].ChamadosAdvertentes);
            });

            var ctx = document.getElementById('canvasz').getContext('2d');
            var myChart = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: lbls,
                    datasets: [{
                        label: 'Pendetes',
                        data: DataCh2,
                        backgroundColor: [
                            'rgb(255, 255, 102, 0.2)'
                        ],
                        borderColor: [
                            'rgb(255, 204, 0, 1)'
                        ],
                        borderWidth: 1
                    },
                    {
                        label: 'Concluídos',
                        data: DataCh1,
                        backgroundColor: [
                            '	rgb(0, 153, 77, 0.2)',
                        ],
                        borderColor: [
                            'rgb(0, 153, 0, 1)',
                        ],
                        borderWidth: 1
                    },]
                },
                options: {
                    title: {
                        display: true,
                        text: 'Chamados por Situação',
                        fontColor: "#000"
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });
            // ******
        }
    });
}

function buscaGraficoResumoSituacaoAtendimentos() {
    $.ajax({
        dataType: "json",
        type: "GET",
        url: "Dashboard/BuscaResumoAtendimentos",
        success: function (dados) {

            var lbls = [];
            var DataMoto = [];
            var DataCarro = [];

            $(dados).each(function (i) {
                lbls.push(month[dados[i].Mes]);
                DataMoto.push(dados[i].QtdAtendimentoMotociclista);
                DataCarro.push(dados[i].QtdAtendimentoMotorista);
            });

            var ctx = document.getElementById('canvas').getContext('2d');
            var myChart = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: lbls,
                    datasets: [{
                        label: 'Motoqueiro',
                        data: DataMoto,
                        backgroundColor: [
                            'rgba(255, 99, 132, 0.2)',
                            'rgba(54, 162, 235, 0.2)',
                            'rgba(255, 206, 86, 0.2)',
                            'rgba(75, 192, 192, 0.2)',
                            'rgba(153, 102, 255, 0.2)',
                            'rgba(255, 159, 64, 0.2)'
                        ],
                        borderColor: [
                            'rgba(255,99,132,1)',
                            'rgba(54, 162, 235, 1)',
                            'rgba(255, 206, 86, 1)',
                            'rgba(75, 192, 192, 1)',
                            'rgba(153, 102, 255, 1)',
                            'rgba(255, 159, 64, 1)'
                        ],
                        borderWidth: 1
                    },
                    {
                        label: 'Motorista',
                        data: DataCarro,
                        backgroundColor: [
                            'rgb(54, 162, 235, 0.2)',
                        ],
                        borderColor: [
                            'rgb(0, 128, 255, 1)',
                        ],
                        borderWidth: 1
                    },]
                },
                options: {
                    title: {
                        display: true,
                        text: 'Atendimentos por profissional',
                        fontColor: "#000"
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });
            // ******
        }
    });
}

function bsccli() {
    var jqVariavel = $("#pesquisaCliente");
    $.ajax({
        dataType: "json",
        type: "GET",
        url: "Dashboard/BuscaClientes?trechoPesquisa=" + jqVariavel.val(),
        success: function (data) {
            if (data !== "" && data !== undefined && data.length > 0) {
                $("#ClienteSelecionado").find('option').remove().end();
                var div_data = "<option value=></option>";
                $(div_data).appendTo('#ClienteSelecionado');
                $.each(data, function (i, obj) {
                    var div_data = "<option value=" + obj.value + ">" + obj.label + "</option>";
                    $(div_data).appendTo('#ClienteSelecionado');
                });
                $("#ClienteSelecionado").mouseup();
            }
            else {
                AdicionarErroCampo('ClienteSelecionado', 'Não foi possível encotrar o cliente desejado.', 4000);
            }
        },
    });
}

$(document).ready(function ($) {
    $('.counter').counterUp({
        delay: 10,
        time: 1000
    });
    $('#PesquisaOSCliente').mask('0#');
    $('#PesquisaMotociclista').mask('0#');
    buscaGraficoResumoSituacaoChamados();
    buscaGraficoResumoSituacaoAtendimentos();

    $("#btnOSAV").click(function () {
        $("#mdCliente").modal("show");
        setTimeout(function () {
            $('.modal-backdrop').remove();
        }, 1000);
    })

    $("#pesquisaCliente").keyup(delay(function (e) {
        bsccli();
    }, 500));

    $("#ClienteSelecionado").change(function () {
        var $option = $(this).find('option:selected');
        //location.href = '@Url.Action("Index", "HomeExterno")?idCliente=' + $option.val();
        location.href = '/HomeExterno/Index?idCliente=' + $option.val();
    });

    bsccli();


});

