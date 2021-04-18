
/* =========================================
                Preloader
============================================ */
$(window).on('load', function () { // makes sure that whole site is loaded
    $('#status').fadeOut();
    $('#preloader').delay(350).fadeOut('slow');


});



/* =============== ======================
              Navigation
=============== ======================= */

/* Show & Hide White Navigation 
$(function () {

    // show/hide nav on page load
    showHideNav();

    $(window).scroll(function () {

        // show/hide nav on window's scroll
        showHideNav();
    });

    function showHideNav() {

        if ($(window).scrollTop() > -1) {

            // Show white nav
            $("nav").addClass("purple-nav-top");

            // Show dark logo
            $(".navbar-brand img").attr("src", "img/home/logo.png");

            // Show back to top button
            $("#back-to-top").fadeIn();

        } else {

            // Hide white nav
            $("nav").removeClass("purple-nav-top");

            // Show logo
            $(".navbar-brand img").attr("src", "img/home/top-logo.png");

            // Hide back to top button
            $("#back-to-top").fadeOut();
        }
    }
});*/

// Smooth Scrolling
$(function () {

    $("a.smooth-scroll").click(function (event) {

        event.preventDefault();

        // get section id like #about, #servcies, #work, #team and etc.
        var section_id = $(this).attr("href");

        $("html, body").animate({
            scrollTop: $(section_id).offset().top - 64
        }, 1250, "easeInOutExpo");

    });

});

/* =============== =====================
              Mobile Menu
================= ====================== */
$(function () {

    // Show mobile nav
    $("#mobile-nav-open-btn").click(function () {
        $("#mobile-nav").css("height", "100%");
    });

    // Hide mobile nav
    $("#mobile-nav-close-btn, #mobile-nav a").click(function () {
        $("#mobile-nav").css("height", "0%");
    });

});





var userName;
function UnderReviewFilter(obj) {
    var data = {};
    data.userName = obj.value;
    data.Search = $("#search-underReview-input").val();
 
    $.ajax
        ({
            method: 'GET',
            url: 'noteUnderReview',
            data: data,
            success: function (data) {
                document.body.innerHTML = data;
                $('#status').fadeOut();
                $('#preloader').delay(350).fadeOut('slow');



            },
            error: function () {
                alert("Something Wrong");
            }
        });
}




var Note, Seller, Buyer, Search;
function adminDownloadFilter(obj) {
    
    var data = {}
    if (obj.id == "Note")
        Note = obj.value;
    data.Note = Note;
    if (obj.id == "Seller")
        Seller = obj.value;
    data.Seller = Seller;
    if (obj.id == "Buyer")
        Buyer = obj.value;
    data.Buyer = Buyer;
    data.Search = $("#search-downloaded-input").val();

   
    $.ajax
        ({
            method: 'GET',
            url: 'downloadedNote',
            data: data,
            success: function (data) {
                document.body.innerHTML = data;
                $('#status').fadeOut();
                $('#preloader').delay(350).fadeOut('slow');



            },
            error: function () {
                alert("Something Wrong. Please try again");
            }
        });

}




function rejectedNoteFilter(obj) {
    var data = {}
    data.Seller = obj.value;
    data.Search = $("#search-rejected-input").val();


    $.ajax
        ({
            method: 'GET',
            url: 'rejectedNotes',
            data: data,
            success: function (data) {
                document.body.innerHTML = data;
                $('#status').fadeOut();
                $('#preloader').delay(350).fadeOut('slow');



            },
            error: function () {
                alert("Something Wrong. Please try again");
            }
        });


}

