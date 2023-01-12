function userIsAuthorized() {
    const jwt = localStorage.getItem('jwt');
    if (jwt === null) {
        alert("You must be logged in to create a post.");
    } else {
        const headers = new Headers({
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${jwt}`
        });
        fetch('https://localhost:7025/api/User/checkAuth', {
            method: 'POST',
            headers: headers,
            body: JSON.stringify({ some: 'data' })
        })
            .then(response => response.text())
            .then(data => {
                if (data === "admin") {
                    window.location.replace("https://localhost:7025/pages/addPost.html");
                } else if (data === "mod") {
                    window.location.replace("https://localhost:7025/pages/addPost.html");
                } else if (data === "owner") {
                    window.location.replace("https://localhost:7025/pages/addPost.html");
                } else if (data === "default") {
                    window.location.replace("https://localhost:7025/pages/addPost.html");
                } else {
                    alert("You must be logged in to create a post.");
                }
            })
    }

}

function createPost() {
    // Redirect to createPost.html for authorized user
    userIsAuthorized();
}

function addPost(title, id, author) {
    var postList = document.getElementById("post-list");
    var newRow = document.createElement("tr");
    var titleCell = document.createElement("a");
    titleCell.href = "https://localhost:7025/pages/poscik.html";
    titleCell.innerText = "Visit this post";
    titleCell.target = "_self";
    titleCell.addEventListener("click", function() {
        localStorage.setItem("PostId", id);
    });

    var idCell = document.createElement("td");
    var authorCell = document.createElement("td");
    titleCell.innerHTML = title;
    idCell.innerHTML = id;
    authorCell.innerHTML = author;
    newRow.appendChild(titleCell);
    newRow.appendChild(authorCell);
    newRow.appendChild(idCell);
    postList.appendChild(newRow);
}

var postCount = 0;
var postPerPage = 10;
function handleNewPost(title, id, author) {
    if (postCount >= postPerPage) {
        document.getElementById("next-page-link").style.visibility = "visible";
        return;
    }
    postCount++;
    addPost(title, id, author);
}

window.onload = function () {
    fetch('https://localhost:7025/api/Post/getdefaultposts', {
        method: 'GET'
    }).then(response => response.json())
        .then(data => {
            for (const item of data) {
                handleNewPost(item.topic, item.postId, item.username);
              }
        })
        .catch(error => {
            console.error("Error:", error);
        });
};