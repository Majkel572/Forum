window.onload = function () {
    const id = localStorage.getItem('PostId');
    console.log(id);
    if (id == null) {
        alert("Error, please try again.");
        window.location.replace("https://localhost:7025/pages/home.html");
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
            document.getElementById("image-container").src = "data:image/png;base64," + data.imageData;
        })
};

const postTitle = document.getElementById("post-title");
