var myKey = config.MY_KEY;

function apiSearch() {
  var params = {
    q: $("#query").val(),
    count: 50,
    offset: 0,
    mkt: "en-us",
  };

  $.ajax({
    url: "https://api.bing.microsoft.com/v7.0/search?" + $.param(params),
    type: "GET",
    headers: {
      "Ocp-Apim-Subscription-Key": myKey,
    },
  })
    .done(function (data) {
      var len = data.webPages.value.length;
      var results = "";
      for (i = 0; i < len; i++) {
        results += `<p><a href="${data.webPages.value[i].url}">${data.webPages.value[i].name}</a>: ${data.webPages.value[i].snippet}</p>`;
      }

      $("#searchResults").html(results);
      $("#searchResults").css("visibility", "visible");
      $("#searchResults").dialog();
    })
    .fail(function () {
      alert("error");
    });
}

$("#search").click(apiSearch);

images = [
  "https://images.unsplash.com/photo-1726768616684-0ee7af06a685?q=80&w=1374&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
  "https://images.unsplash.com/photo-1727801168127-ed715a9b5300?q=80&w=1437&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
  "https://images.unsplash.com/photo-1724681880409-7da6be4c90b9?q=80&w=1470&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
];

var imageVal = 0;

$("#title").click(function () {
  imageVal = (imageVal + 1) % images.length;
  $("body").css("background-image", "url(" + images[imageVal] + ")");
});

$("#time").click(function () {
  $("#timeResults").text(new Date().toLocaleTimeString());
  $("#timeResults").css("visibility", "visible");
  $("#timeResults").dialog();
});

$("#lucky").click(function () {
  var params = {
    q: $("#query").val(),
    count: 1,
    offset: 0,
    mkt: "en-us",
  };

  $.ajax({
    url: "https://api.bing.microsoft.com/v7.0/search?" + $.param(params),
    type: "GET",
    headers: {
      "Ocp-Apim-Subscription-Key": myKey,
    },
  })
    .done(function (data) {
      var page = `<p><a href="${data.webPages.value[0].url}">${data.webPages.value[0].name}</a>: ${data.webPages.value[0].snippet}</p>`;
      $("#luckyResults").html(page);
      $("#luckyResults").css("visibility", "visible");
      $("#luckyResults").dialog();
    })
    .fail(function () {
      alert("error");
  });
});
