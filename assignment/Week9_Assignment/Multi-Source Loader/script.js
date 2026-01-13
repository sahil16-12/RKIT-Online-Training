$(document).ready(function () {

  const USER_API  = "https://jsonplaceholder.typicode.com/users/1";
  const POSTS_API = "https://jsonplaceholder.typicode.com/posts?userId=1";

  $("#loadBtn").click(function () {

    // Reset UI
    $("#error").hide();
    $("#dashboard").hide();
    $("#loading").show();

    // Request A: User Profile
    let userRequest = $.ajax({
      url: USER_API,
      method: "GET"
    });

    // Request B: User Posts
    let postsRequest = $.ajax({
      url: POSTS_API,
      method: "GET"
    });

    // 🔥 Wait for BOTH requests to finish
    $.when(userRequest, postsRequest)

      .done(function (userResponse, postsResponse) {
        console.log(userRequest)

        /*
          userResponse[0]  → actual user data
          postsResponse[0] → actual posts data
        */

        let user = userResponse[0];
        let posts = postsResponse[0];

        // Render Profile
        $("#profile").text(`${user.name} (${user.email})`);

        // Render Posts
        $("#posts").empty();
        posts.slice(0, 5).forEach(post => {
          $("#posts").append(`<li>${post.title}</li>`);
        });

        $("#loading").hide();
        $("#dashboard").show();
      })

      .fail(function () {
        $("#loading").hide();
        $("#error").show();
      });

  });

});