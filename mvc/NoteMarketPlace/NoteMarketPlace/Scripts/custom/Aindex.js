

var modal1 = document.getElementById("Unpublish-popup");

var span = document.getElementsByClassName("close")[0];

var span2 = document.getElementsByClassName("close-btn")[0];



$('.unpublish-btn').unbind('click').click(function () {

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



function publishedFilterMonth(obj) {
    var data = {}

    data.search = $("#published_search").val();
    data.month = obj.value;

    $.ajax
        ({
            method: 'GET',
            url: 'Admin/Index',
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