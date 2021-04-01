var modal3 = document.getElementById("download-review");
var modal4 = document.getElementById("Report-popup");
var span5 = document.getElementsByClassName("close")[0];
var span6 = document.getElementsByClassName("close")[1];
var span7 = document.getElementsByClassName("close-btn")[0];

hide_btn(span5, modal3);
hide_outside(modal3);
hide_btn(span6, modal4);
hide_btn(span7, modal4);
hide_outside(modal4);

function addReview(id) {
 
    modal3.style.display = "block";
}


function hide_outside(modal) {
    window.addEventListener("click", function (event) {

        if (event.target == modal) {
            modal.style.display = "none";
            $("#msg").html("");
        }
    });
}

function hide_btn(span, modal) {
    span.onclick = function () {
        modal.style.display = "none";
        $("#msg").html("");
    }
}

$('.spam-btn').unbind('click').click(function () {

  
    var $title = $(this).parents("tr").find('td').eq(2).text();
    var $id = $(this).parents("tr").find('td').eq(0).text();
    $(".popup-heading").html($title);

   $("input[name='noteId']").val($id);

    modal4.style.display = "block";





});

$(document).ready(function () {
    $("#confirm-spam").click(function () {
        var issues = $("input[name=noteIssues]").val() || "";
        if (issues.trim().length == 0)
            $("#msg").html("filed is required");
        else {
            
        //Serialize the form datas.  
        var valdata = $("input[name=noteId]").val();
        
        //to get alert popup  


            $.ajax({
                type: "POST",
                url: '/User/reportSpam/',
                data: JSON.stringify({ id: valdata }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                beforeSend: function () {
                    $('#over').show();
                    modal4.style.display = "none";
                },
                success: function (response) {
                    $('#over').hide();
                    if (response.success) {
                        if (response.alertMsg != null)
                            alert(response.alertMsg)
                        else {


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
        }
    });

});