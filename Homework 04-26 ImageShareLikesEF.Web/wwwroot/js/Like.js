$(() => {
    const imageId = $("#image-id").val()
    $("#like-button").on("click", function () {



        $.post("/Home/Like", { imageId }, function (likes) {
            $("#likes-count").text(likes)
        })

        $("#like-button").prop('disabled', true)
    })

    setInterval(() => {
        $.get('/Home/GetLikes', { imageId }, likes => {
            $("#likes-count").text(likes)
        })
    }, 1000);
})