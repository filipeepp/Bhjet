

$(document).ready(function ($) {
    $('.counter').counterUp({
        delay: 10,
        time: 1000
    });
    $('#PesquisaOSCliente').mask('0#');
    $('#PesquisaMotociclista').mask('0#');

    var ctx = document.getElementById('canvas').getContext('2d');
    var myChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: ["Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro"],
            datasets: [{
                label: 'Pendetes',
                data: [12, 19, 3, 5, 2, 3],
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
                data: [6, 5, 1, 5, 2, 20],
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
                fontColor: "#ffffff"
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

    var ctz = document.getElementById('canvasz').getContext('2d');
    var myChart = new Chart(ctz, {
        type: 'line',
        data: {
            labels: ["Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro"],
            datasets: [{
                label: 'Motoqueiro',
                data: [12, 19, 3, 5, 2, 3],
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
                data: [6, 5, 1, 5, 2, 20],
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
                fontColor: "#ffffff"
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

});