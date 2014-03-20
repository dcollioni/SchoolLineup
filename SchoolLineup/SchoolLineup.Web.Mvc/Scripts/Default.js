SL.mask = function (loading) {
    $('#mask').fadeIn('fast');

    if (!!loading) {
        $('#loading').fadeIn('fast');
    }
};
SL.unmask = function () {
    $('#mask').fadeOut('fast');
    $('#loading').fadeOut('fast');
};