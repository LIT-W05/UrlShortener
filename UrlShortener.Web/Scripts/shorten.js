$(() => {
    $("#shorten-button").on('click', function() {
        const url = $("#url").val();
        $.post('/account/shortenurl', {originalUrl: url}, result => {
            $("#shortened-url").html(`<a href='${result.shortUrl}'>${result.shortUrl}</a>`);
            $("#shortened-url").slideDown();
        });
    });
});