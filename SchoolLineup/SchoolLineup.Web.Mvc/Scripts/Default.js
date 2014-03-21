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

$(function () {
    $(document).keyup(function (e) {
        if (e.keyCode == 27) {
            SL.closeModals();
        }
    });
});