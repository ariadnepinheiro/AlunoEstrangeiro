$(document).ready(function () {
    $('.dropdown-submenu a.test').on("click", function (e) {
        $(this).next('ul').toggle();
        e.stopPropagation();
        e.preventDefault();
    });

    $('.top_inf').hover(
        () => { $("#top_menu").css("display", "block"); },
        () => { $("#top_menu").css("display", "none"); }
    );
});