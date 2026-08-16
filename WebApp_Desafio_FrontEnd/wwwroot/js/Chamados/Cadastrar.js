$(document).ready(function () {

    let datepickerOptions = {
        todayBtn: "linked",
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: false,
        format: 'dd/mm/yyyy',
        autoclose: true,
        language: 'pt-BR'
    };

    if (parseInt($('#ID').val()) === 0) {
        datepickerOptions.startDate = '0d';
    }

    $('.glyphicon-calendar').closest("div.date").datepicker(datepickerOptions);

    let solicitantes = [];
    let $inputSolicitante = $('#Solicitante');
    let $listaSolicitantes = $('#listaSolicitantes');

    $.get(config.contextPath + 'Chamados/SolicitantesAutocomplete', function (result) {
        solicitantes = result || [];
    });

    function renderizarSugestoesSolicitante(termo) {
        $listaSolicitantes.empty();

        if (!termo) {
            $listaSolicitantes.hide();
            return;
        }

        let termoLower = termo.toLowerCase();
        let sugestoes = solicitantes.filter(function (nome) {
            return nome.toLowerCase().indexOf(termoLower) !== -1;
        }).slice(0, 8);

        if (sugestoes.length === 0) {
            $listaSolicitantes.hide();
            return;
        }

        sugestoes.forEach(function (nome) {
            $('<li class="list-group-item"></li>').text(nome).appendTo($listaSolicitantes);
        });

        $listaSolicitantes.show();
    }

    $inputSolicitante.on('input', function () {
        renderizarSugestoesSolicitante($(this).val());
    });

    $inputSolicitante.on('focus', function () {
        if ($(this).val()) {
            renderizarSugestoesSolicitante($(this).val());
        }
    });

    $inputSolicitante.on('keydown', function (e) {
        let $items = $listaSolicitantes.find('li');
        if ($items.length === 0) {
            return;
        }

        let $active = $items.filter('.active');
        let index = $items.index($active);

        if (e.key === 'ArrowDown') {
            e.preventDefault();
            index = (index + 1) % $items.length;
            $items.removeClass('active').eq(index).addClass('active');
        } else if (e.key === 'ArrowUp') {
            e.preventDefault();
            index = (index <= 0) ? $items.length - 1 : index - 1;
            $items.removeClass('active').eq(index).addClass('active');
        } else if (e.key === 'Enter') {
            if ($active.length) {
                e.preventDefault();
                $inputSolicitante.val($active.text());
                $listaSolicitantes.hide();
            }
        } else if (e.key === 'Escape') {
            $listaSolicitantes.hide();
        }
    });

    $listaSolicitantes.on('click', 'li', function () {
        $inputSolicitante.val($(this).text());
        $listaSolicitantes.hide();
    });

    $(document).on('click', function (e) {
        if (!$(e.target).closest('.autocomplete-wrapper').length) {
            $listaSolicitantes.hide();
        }
    });

    $('#btnCancelar').click(function () {
        Swal.fire({
            html: "Deseja cancelar essa operação? O registro não será salvo.",
            type: "warning",
            showCancelButton: true,
        }).then(function (result) {
            if (result.value) {
                history.back();
            } else {
                console.log("Cancelou a inclusão.");
            }
        });
    });

    $('#btnSalvar').click(function () {

        if ($('#form').valid() != true) {
            FormularioInvalidoAlert();
            return;
        }

        let chamado = SerielizeForm($('#form'));
        let url = $('#form').attr('action');
        //debugger;

        $.ajax({
            type: "POST",
            url: url,
            data: chamado,
            success: function (result) {

                Swal.fire({
                    type: result.Type,
                    title: result.Title,
                    text: result.Message,
                }).then(function () {
                    window.location.href = config.contextPath + result.Controller + '/' + result.Action;
                });

            },
            error: function (jqXHR) {

                let mensagem = (jqXHR.responseJSON && jqXHR.responseJSON.Message) || jqXHR.responseText || "Ocorreu um erro ao processar a solicitação.";

                Swal.fire({
                    text: mensagem,
                    confirmButtonText: 'OK',
                    icon: 'error'
                });

            },
        });
    });

});
