jQuery(document).ready(function ($) {
    //open the lateral panel
    $('.cd-btn').on('click', function (event) {
        event.preventDefault();
        $('#cd-panel').addClass('is-visible');
        $('.cd-mask').show();
    });
    //clode the lateral panel
    $('.cd-mask').on('click', function (event) {
        $('.cd-panel').removeClass('is-visible');
        $('.cd-mask').hide();
        event.preventDefault();
    });
    $('.cd-panel-close').on('click', function (event) {
        $('.cd-panel').removeClass('is-visible');
        $('.cd-mask').hide();
        event.preventDefault();
    });
});