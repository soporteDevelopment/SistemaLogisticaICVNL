$(function () {
    $('.my-multiselectBP').attr('data-live-search', true);
    $('.my-multiselectWP').attr('data-live-search', true);
    $('.my-multiselectBS').attr('data-live-search', true);
    $('.my-multiselectWS').attr('data-live-search', true);

    //// Enable multiple select.
    $('.my-multiselectBP').attr('multiple', true);
    $('.my-multiselectWP').attr('multiple', true);
    $('.my-multiselectBS').attr('multiple', true);
    $('.my-multiselectWS').attr('multiple', true);
    $(".my-multiselectBP option")[0].remove();
    $(".my-multiselectWP option")[0].remove();
    $(".my-multiselectBS option")[0].remove();
    $(".my-multiselectWS option")[0].remove();
    $('.my-multiselectBP').attr('data-selected-text-format', 'count');
    $('.my-multiselectWP').attr('data-selected-text-format', 'count');
    $('.my-multiselectBS').attr('data-selected-text-format', 'count');
    $('.my-multiselectWS').attr('data-selected-text-format', 'count');

    $('.my-multiselectBP').selectpicker(
        {
            width: '100%',
            title: '- Todos -',
            style: 'btn-light',
            size: 6,
            iconBase: 'fa',
            tickIcon: 'fa-check'
        });
    $('.my-multiselectWP').selectpicker(
        {
            width: '100%',
            title: '- Todos -',
            style: 'btn-light',
            size: 6,
            iconBase: 'fa',
            tickIcon: 'fa-check'
        });
    $('.my-multiselectBS').selectpicker(
        {
            width: '100%',
            title: '- Todos -',
            style: 'btn-light',
            size: 6,
            iconBase: 'fa',
            tickIcon: 'fa-check'
        });
    $('.my-multiselectWS').selectpicker(
        {
            width: '100%',
            title: '- Todos -',
            style: 'btn-light',
            size: 6,
            iconBase: 'fa',
            tickIcon: 'fa-check'
        });
});