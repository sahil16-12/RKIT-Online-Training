$(document).ready(function(){
    $(".box").on("click", function(){
        $(this).fadeOut(2000, function(){
            alert("The box has disappeared!");
            $(this).fadeIn();
        });
    }) 
})