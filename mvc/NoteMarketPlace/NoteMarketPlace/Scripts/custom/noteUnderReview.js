
/* ============= ==========
           Pop up Action
  ============= ========== */
// Get the modal
var modal1 = document.getElementById("reject-popup");
var modal2 = document.getElementById("Approve-popup");
var modal3 = document.getElementById("InReview-popup");
// Get the button that opens the modal
//var btn = document.getElementsByClassName("reject-btn");

// Get the <span> element that closes the modal
var span = document.getElementsByClassName("close")[0];

var span2 = document.getElementsByClassName("close-btn")[0];
// When the user clicks the button, open the modal

var span3 = document.getElementsByClassName("close")[1];

var span4 = document.getElementsByClassName("close-btn")[1];

var span5 = document.getElementsByClassName("close")[2];

var span6 = document.getElementsByClassName("close-btn")[2];



$('.reject-btn').unbind('click').click(function () {

    var $title = $(this).parents("tr").find('td').eq(2).text();
    $title += " - "
    $title += $(this).parents("tr").find('td').eq(3).text();
    var $id = $(this).parents("tr").find('td').eq(0).text();

    $(".popup-heading").html($title);

    $("input[name='noteId']").val($id);

    modal1.style.display = "block";




});
$('.approve-btn').unbind('click').click(function () {

    var $title = $(this).parents("tr").find('td').eq(2).text();
    $title += " - "
    $title += $(this).parents("tr").find('td').eq(3).text();
    var $id = $(this).parents("tr").find('td').eq(0).text();

    $(".popup-heading").html($title);

    $("input[name='noteId']").val($id);

    modal2.style.display = "block";





});

$('.InReview-btn').unbind('click').click(function () {

    var $title = $(this).parents("tr").find('td').eq(2).text();
    $title += " - "
    $title += $(this).parents("tr").find('td').eq(3).text();
    var $id = $(this).parents("tr").find('td').eq(0).text();

    $(".popup-heading").html($title);

    $("input[name='noteId']").val($id);

    modal3.style.display = "block";





});


hide_btn(span, modal1);
hide_btn(span2, modal1);
hide_btn(span3, modal2);
hide_btn(span4, modal2);
hide_btn(span5, modal3);
hide_btn(span6, modal3);
hide_outside(modal1);
hide_outside(modal2);
hide_outside(modal3);


//when user click outside popup
function hide_outside(modal) {
    window.addEventListener("click", function (event) {

        if (event.target == modal) {
            modal.style.display = "none";
        }
    });
}



// When the user clicks on <span> (x), close the modal

function hide_btn(span, modal) {
    span.onclick = function () {
        modal.style.display = "none";
    }
}
