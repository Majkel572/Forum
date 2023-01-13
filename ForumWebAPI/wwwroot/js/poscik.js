window.onload = function () {
    const id = localStorage.getItem('PostId');
    console.log(id);
    if (id == null) {
        alert("Error, please try again.");
        const jwt = localStorage.getItem('jwt');
        if (jwt == null) {
            window.location.href = "/pages/home.html";
        } else {
            window.location.href = "/pages/homeLoggedIn.html";
        }
    }
    const url = new URL("https://localhost:7025/api/Post/getpostbyid");
    url.searchParams.append("id", id);
    fetch(url, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
        },
    }).then(response => response.json())
        .then(data => {
            console.log(data);
            var base64 = "data:image/jpg;base64," + data.imageData;
            var byteCharacters = atob(base64.split(',')[1]);
            var byteNumbers = new Array(byteCharacters.length);
            for (var i = 0; i < byteCharacters.length; i++) {
                byteNumbers[i] = byteCharacters.charCodeAt(i);
            }
            var byteArray = new Uint8Array(byteNumbers);
            var blob = new Blob([byteArray], { type: "image/jpg" });
            var img = document.createElement("img");
            img.src = URL.createObjectURL(blob);
            document.getElementById("image-container").appendChild(img);
            document.getElementById("topica").innerHTML = data.username;
            document.getElementById("contica").innerHTML = data.content;
            document.getElementById("titlepost").innerHTML = data.topic;
        })
};
