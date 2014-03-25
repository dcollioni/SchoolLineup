SL.mask = function (loading) {
    $('#mask').fadeIn('fast');

    if (!!loading) {
        $('#loading').fadeIn('fast');
    }
};
SL.unmask = function () {
    $('#mask').fadeOut('fast');
    SL.hideModals();
};
SL.hideModals = function () {
    $('.modal').fadeOut('fast');
};
SL.closeModals = function () {
    $('.closable .close').click();
};

SL.setModalPosition = function () {
    var $modal = $('.modal.active');

    var width = $modal.width();
    var height = $modal.height();

    $modal.css('margin-left', -(width/2));
    $modal.css('margin-top', -(height/2));
};

$(function () {
    $(document).keyup(function (e) {
        if (e.keyCode == 27) {
            SL.closeModals();
        }
    });
});