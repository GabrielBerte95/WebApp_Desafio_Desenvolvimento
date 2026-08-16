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
