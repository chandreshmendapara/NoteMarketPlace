var modal1 = document.getElementById("Buying-popup");
var modal2 = document.getElementById("confirm-popup");

var span1 = document.getElementsByClassName("close")[0];
var span2 = document.getElementsByClassName("close-btn")[0];



var span3 = document.getElementsByClassName("close")[1];
var span4 = document.getElementsByClassName("close-btn")[1];






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
                    if (response.alertMsg != null)
                        alert(response.alertMsg)
                    else {
                        modal1.style.display = "none";

                        modal2.style.display = "block";
                        $('#seller-name').text(response.responseText);
                    }
                }
                else {
                    // DoSomethingElse()
                    alert(response.responseText);
                }
            }


        });
    });

});






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

function Selling(id) {
    var price = $('#book-price').text();

    var title = $('#note-name').text();
    if (price == 0) {
        window.location = '/User/FreeDownload/' + id;

    }
    else {


        $(".popup-heading").html(title);

        $("input[name='Id']").val(id);
        modal1.style.display = "block";





    }
}

hide_btn(span1, modal1);
hide_btn(span2, modal1);
hide_outside(modal1);


hide_btn(span3, modal2);
hide_btn(span4, modal2);
hide_outside(modal2);