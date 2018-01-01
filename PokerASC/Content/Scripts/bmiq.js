function OpenPopupConfirmation(mensagem, href, callback) {
    var modal = $('#modal-confirmacao')
    if (modal.length === 0) $('body').append('<div id="modal-confirmacao" class="modal fade" role="dialog"><div class="modal-dialog"><div class="modal-content"><div class="modal-header"><h2>Confirma sua ação?</h2></div><div class="modal-body">' + mensagem + '</div><div class="modal-footer"><a href="#" class="btn btn-primary"><span class="glyphicon glyphicon-ok"></span> Sim</a><a href="#" class="btn btn-danger" data-dismiss="modal"><span class="glyphicon glyphicon-remove"></span> Não</a></div></div></div></div>');
    $('#modal-confirmacao').modal("show");
    $('#modal-confirmacao').on('hidden.bs.modal', function () {
        $(this).remove();
    })

    $('#modal-confirmacao .btn-primary').on('click', function () {
        $.ajax({
            type: "POST",
            url: href,
            success: function (data) {
                if (data.status === "OK") {
                    callback();
                    $('#modal-confirmacao').modal("hide");
                }
                else {
                    console.log(data.status);
                }
            }
        })
    })

}

function OpenPopup(pageUrl, callback) {
    var modalId = "modal_auto_" + makeid(5);
    if ($(modalId).length === 0) $('body').append('<div id="' + modalId + '" class="modal fade" role="dialog"></div>');
    modalId = '#' + modalId;
    $(modalId).load(pageUrl, function () {
        $(modalId).modal("show");

        $(modalId).on('hidden.bs.modal', function () {
            $(this).remove();
        })

        $(modalId + ' form').on('submit', function (e) {
            e.preventDefault();
            var url = $(modalId + ' form')[0].action;
            console.log(url);
            $(this).find('input[type=submit]').attr("disabled", "");
            $.ajax({
                type: "POST",
                url: url,
                data: $('' + modalId + ' form').serialize(),
                success: function (data) {
                    if (data.status === "OK") {
                        callback();
                        $(modalId).modal("hide");
                    }
                    else {
                        console.log(data.status);
                    }
                }
            })
        })
    });
}

$(document).on('click', 'div.img-selector', function () {
    imgselector = $(this);
    $("#form-img-selector input").click();
});

$(document).on('show.bs.modal', '.modal', function () {
    var zIndex = 1040 + (10 * $('.modal:visible').length);
    $(this).css('z-index', zIndex);
    setTimeout(function () {
        $('.modal-backdrop').not('.modal-stack').css('z-index', zIndex - 1).addClass('modal-stack');
    }, 0);
});

$(document).on('click', 'a.popup', function (e) {
    e.preventDefault();
    var callback = $(this).attr("popup-callback");
    var confirm = $(this).attr("popup-confirm");
    var el = $(this);

    if (confirm) {
        OpenPopupConfirmation("Tem certeza que deseja efetuar a ação?", el.attr('href'), function () {
            if (callback !== undefined) eval(callback);
        })
    }
    else {
        OpenPopup(el.attr('href'), function () {
            if (callback !== undefined) eval(callback);
        });
    }
})

$(document).on('hidden.bs.modal', '.modal', function () {
    $('.modal:visible').length && $(document.body).addClass('modal-open');
});


function makeid(qtd = 5) {
    var text = "";
    var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

    for (var i = 0; i < qtd; i++)
        text += possible.charAt(Math.floor(Math.random() * possible.length));

    return text;
}
