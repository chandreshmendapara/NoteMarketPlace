
var modal1 = document.getElementById("Approve-popup");
// Get the button that opens the modal
//var btn = document.getElementsByClassName("reject-btn");

// Get the <span> element that closes the modal
var span = document.getElementsByClassName("close")[0];

var span2 = document.getElementsByClassName("close-btn")[0];



$('.approve-btn').unbind('click').click(function () {

    var $title = $(this).parents("tr").find('td').eq(2).text();
    $title += " - "
    $title += $(this).parents("tr").find('td').eq(3).text();
    var $id = $(this).parents("tr").find('td').eq(0).text();

    $(".popup-heading").html($title);

    $("input[name='noteId']").val($id);

    modal1.style.display = "block";





});



hide_btn(span, modal1);
hide_btn(span2, modal1);
hide_outside(modal1);


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
