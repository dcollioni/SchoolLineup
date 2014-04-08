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
    $('.active.closable .close').click();
};

SL.setModalPosition = function () {
    var $modal = $('.modal.active');

    var width = $modal.width();
    var height = $modal.height();

    $modal.css('margin-left', -(width/2));
    $modal.css('margin-top', -(height / 2));

    $modal.find('.close').focus();
    $modal.find('input[type=button][name=confirm]').focus();
};

SL.formatters = {
    phone: function (value) {
        if (!!value) {
            value = String.format('({0}) {1}-{2}', value.substr(0, 2), value.substr(2, 4), value.substr(6));
        }

        return value;
    }
};

SL.validation = {
    isEmailValid: function (email) {
        var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(email);
    }
}

$(function () {
    $(document).keyup(function (e) {
        if (e.keyCode == 27) {
            SL.closeModals();
        }
    });
});