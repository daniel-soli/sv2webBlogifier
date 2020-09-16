//$("#blog-search").on("show.bs.modal", function () {
//    $(".blog-header").css({
//        right: scrollbarWidth
//    })
//});
//$("#blog-search").on("hidden.bs.modal", function () {
//    $(".blog-header").css({
//        right: "0"
//    })
//});
$("#blog-search").on("shown.bs.modal", function () {
    $("#blog-search .form-control").trigger("focus")
});