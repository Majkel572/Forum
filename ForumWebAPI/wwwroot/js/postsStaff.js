function userIsAuthorized() {
    const jwt = localStorage.getItem('jwt');
    if (jwt === null) {
        window.location.href = "/pages/home.html";
    } else {
        const headers = new Headers({
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${jwt}`
        });
        fetch('/api/User/checkAuth', {
            method: 'POST',
            headers: headers,
            body: JSON.stringify({ some: 'data' })
        })
            .then(response => response.text())
            .then(data => {
                if (data === "admin") {
                    window.location.href = "/pages/addAdminPost.html";
                } else if (data === "mod") {
                    window.location.href = "/pages/addAdminPost.html";
                } else if (data === "owner") {
                    window.location.href = "/pages/addAdminPost.html";
                } else if (data === "default") {
                    window.location.href = "/pages/homeLoggedIn.html";
                } else {
                    window.location.href = "/pages/home.html";
                }
            })
    }

}

async function whoiam() {
    const jwt = localStorage.getItem('jwt');
    var who = "";
    if (jwt === null) {
        who = "notloggedin";
    } else {
        const headers = new Headers({
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${jwt}`
        });
        const response = await fetch('/api/User/checkAuth', {
            method: 'POST',
            headers: headers,
            body: JSON.stringify({ some: 'data' })
        })
            .then(response => response.text())
            .then(data => {
                if (data == "owner") {
                    who = "admin";
                } else {
                    who = "notAuthorized";
                }
            })
        return who;
    }

}

function createPost() {
    // Redirect to createPost.html for authorized user
    userIsAuthorized();
}

async function addPost(title, id, author) {
    var postList = document.getElementById("post-list");
    var newRow = document.createElement("tr");
    var titleCell = document.createElement("a");
    titleCell.href = "/pages/poscikAdmin.html";
    titleCell.innerText = "Visit this post";
    titleCell.target = "_self";
    titleCell.addEventListener("click", function () {
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
    if (await whoiam() == "admin") {
        var button1 = document.createElement("button");
        button1.innerText = "Edit";
        var button2 = document.createElement("button");
        button2.innerText = "Delete ";// + id;
        button2.name = id;
        button1.addEventListener("click", function () {
            location.href = "/pages/editposcik.html";
            localStorage.setItem("PostId", id);
        });
        button2.addEventListener("click", async function (event) {
            const jwt = localStorage.getItem('jwt');
            const id = event.target.name;
            const headers = new Headers({
                'Authorization': `Bearer ${jwt}`
            });
            const url = new URL("https://localhost:7025/api/Post/deletepost");
            url.searchParams.append("id", id);
            await fetch(url, {
                method: 'DELETE',
                headers: headers
            })
                .then(response => response.json())
                .then(data => {
                    location.reload(true);
                });
        });
        newRow.appendChild(button1);
        newRow.appendChild(button2);
    }
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
    const jwt = localStorage.getItem('jwt');
    if (jwt === null) {
        alert("Sign in to continue.");
        window.location.href = "/pages/home.html";
    } else {
        const headers = new Headers({
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${jwt}`
        });
        fetch('/api/User/rolegetter', {
            method: 'POST',
            headers: headers,
            body: JSON.stringify({ some: 'data' })
        })
            .then(response => response.text())
            .then(data => {
                if (data == "administrator" || data == "moderator" || data == "owner") {
                    fetch('/api/Post/getstaffposts', {
                        method: 'GET',
                        headers: {
                            'Authorization': `Bearer ${jwt}`
                        }
                    }).then(response => response.json())
                        .then(data => {
                            for (const item of data) {
                                console.log(item.topic, item.postId, item.username);
                                handleNewPost(item.topic, item.postId, item.username);
                            }
                        })
                        .catch(error => {
                            console.error("Error:", error);
                        });
                } else if (data == "default") {
                    window.location.href = "/pages/homeLoggedIn.html";
                } else {
                    window.location.href = "/pages/home.html";
                }
            })
    }
};