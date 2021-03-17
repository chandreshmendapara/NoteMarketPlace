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

/* Show & Hide White Navigation */
$(function () {

    // show/hide nav on page load
    showHideNav();

    $(window).scroll(function () {

        // show/hide nav on window's scroll
        showHideNav();
    });

    function showHideNav() {

        if ($(window).scrollTop() >= 0) {

            // Show white nav
            $("nav").addClass("purple-nav-top");

            // Show dark logo
            $(".navbar-brand img").attr("src", "../../img/home/logo.png");

            // Show back to top button
            $("#back-to-top").fadeIn();

        } else {

            // Hide white nav
           // $("nav").removeClass("purple-nav-top");

            // Show logo
           // $(".navbar-brand img").attr("src", "../img/home/top-logo.png");

            // Hide back to top button
            $("#back-to-top").fadeOut();
        }
    }
});

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


/* ================= ======================
         Book Discription In details
=================== =======================*/
function openNav() {
  document.getElementById("myNav").style.display = "block";
   $('body').css({"overflow": "hidden"});


}

function closeNav() {
  document.getElementById("myNav").style.display = "none";
  $('body').css({"overflow-y": "scroll"});
}

/* close popup when click outside*/
$(document).mouseup(function(e) 
{
    var container = $("#myNav");

    // if the target of the click isn't the container nor a descendant of the container
    if (!container.is(e.target) && container.has(e.target).length === 0) 
    {
        container.hide();
        document.getElementById("thank-you").style.display = "none";
        $('body').css({"overflow-y": "scroll"});
    }
});
var fixed = document.getElementById('myNav');

fixed.addEventListener('touchmove', function(e) {

        e.preventDefault();

}, false);


/* Thank you */
function openThanku() {
  document.getElementById("thank-you").style.display = "block";
   $('body').css({"overflow": "hidden"});
   

}

function closeThanku() {
  document.getElementById("thank-you").style.display = "none";
   $('body').css({"overflow": "hidden"});
   

}



/* ============== =============
                FAQ
 ============= ===============*/
 var acc = document.getElementsByClassName("accordion");
var i;

for (i = 0; i < acc.length; i++) {
  acc[i].addEventListener("click", function() {
    this.classList.toggle("active");
    var panel = this.nextElementSibling;
    if (panel.style.maxHeight) {
      panel.style.maxHeight = null;
      

    } else {
      panel.style.maxHeight = panel.scrollHeight + "px";
      panel.style.border ="1px solid #d1d1d1";

    } 
  });
}


/* ============= ==========
            My Downloads
  ============= ========== 

function openReview() {
  document.getElementById("download-review").style.display = "block";
   $('body').css({"overflow": "hidden"});
    
}

function closeReview() {
  document.getElementById("download-review").style.display = "none";
  $('body').css({"overflow-y": "scroll"});
    
}*/

/* close popup when click outside
$(document).mouseup(function(e) 
{
    var container = $("#download-review");

    // if the target of the click isn't the container nor a descendant of the container
    if (!container.is(e.target) && container.has(e.target).length === 0) 
    {
        container.hide();
        $('body').css({"overflow-y": "scroll"});
    }
});
var fixed = document.getElementById('download-review');

fixed.addEventListener('touchmove', function(e) {

        e.preventDefault();

}, false);

/* #CodebyChandreshMendapara
var scrollHandler = function(){
    myScroll = $(window).scrollTop();
}

$("body").click(function(){
    $(window).scroll(scrollHandler);
}).click(); // .click() will execute this handler immediately

$("body").click(function(){
    $(window).off("scroll", scrollHandler);
});

*/

/* ================= Selling Book / Download ====================*/


var modal1 = document.getElementById("Buying-popup");
var modal2 = document.getElementById("confirm-popup");

var span1 = document.getElementsByClassName("close")[0];
var span2 = document.getElementsByClassName("close-btn")[0];

var span3 = document.getElementsByClassName("close")[1];
var span4 = document.getElementsByClassName("close-btn")[1];



function Selling(id) {
    var price = $('#book-price').text();
     
    var title = $('#note-name').text();
    if (price == 0) {
        window.location = '/User/FreeDownload/'+id;
       
    }
    else {


            $(".popup-heading").html(title);

        $("input[name='Id']").val(id);
        modal1.style.display = "block";

        



    }
}




$(document).ready(function () {
    $("#confirm-download").click(function () {

        //Serialize the form datas.  
        var valdata = $("input[name=Id]").val();

        //to get alert popup  


        $.ajax({
            type: "POST",
            url: '/User/AskforDownload/',
            data: JSON.stringify({ id: valdata }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",

            success: function (response) {
                if (response.success) {

                    modal1.style.display = "none";

                    modal2.style.display = "block";
                    $('#seller-name').text(response.responseText);

                }
                else {
                    // DoSomethingElse()
                    alert(response.responseText);
                }
            }


        });
    });

});

 

//for popup #CodeBy...

//when user click outside popup



// When the user clicks on <span> (x), close the modal
function hide_outside(modal) {
    window.addEventListener("click", function (event) {

        if (event.target == modal) {
            modal.style.display = "none";
        }
    });
}

function hide_btn(span, modal) {
    span.onclick = function () {
        modal.style.display = "none";
    }
}




function hide_btn(span, modal) {
    span.onclick = function () {
        modal.style.display = "none";
    }
}

hide_btn(span1, modal1);
hide_btn(span2, modal1);
hide_outside(modal1);


hide_btn(span3, modal2);
hide_btn(span4, modal2);
hide_outside(modal1);