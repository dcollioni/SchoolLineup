jQuery(function ($) {
    $('.mask-phone').mask('(99) 9999-9999');
    $('.mask-phonesp').mask('(99) 9999-9999?9');
    $('.mask-zipcode').mask('99999-999');
    $('.mask-date').mask('99/99/9999');
    $('.mask-cpf').mask('999.999.999-99');
    $('.number').bind('keypress', function (e) {
        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
            return false;
        }
        return true;
    });
});
