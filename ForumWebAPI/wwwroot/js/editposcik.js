(() => {
    'use strict'

    // Fetch all the forms we want to apply custom Bootstrap validation styles to
    const forms = document.querySelectorAll('.needs-validation')

    // Loop over them and prevent submission
    Array.from(forms).forEach(form => {
        form.addEventListener('submit', event => {
            if (!form.checkValidity()) {
                event.preventDefault()
                event.stopPropagation()
            }

            form.classList.add('was-validated')
        }, false)
    })
})();
var posterekId;
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
            posterekId = data.postId;
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
            document.getElementById("topica").value = data.topic;
            document.getElementById("contica").value = data.content;
        })
};

var myVariable = "User!";
const jwt = localStorage.getItem('jwt');
if (jwt === null) {
    myVariable = "User Error";
} else {
    const headers = new Headers({
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${jwt}`
    });
    fetch('/api/User/whoiam', {
        method: 'POST',
        headers: headers,
        body: JSON.stringify({ some: 'data' })
    })
        .then(response => response.text())
        .then(data => {
            document.getElementById("usernameDisplay").innerHTML = data;
        })
}

const addPosta = document.forms['addPosta'];

addPosta.addEventListener('submit', function (e) {
    document.getElementById("submitterek").disabled = true;
    e.preventDefault();
    if (addPosta.checkValidity()) {
        var topic = document.getElementById('topica').value;
        var content = document.getElementById('contica').value;
        var image = document.getElementById('image').files[0];

        const formData = new FormData();
        formData.append('topic', topic);
        formData.append('content', content);
        formData.append('image', image, {
            contentType: 'image/jpeg',
            filename: 'image.jpg'
        });

        const jwt = localStorage.getItem('jwt');
        if (jwt === null) {
            alert("Sign in to proceed.");
            window.location.href = "/pages/home.html";
            return false;
        } else {
            const headers = new Headers({
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${jwt}`
            });
            fetch('/api/User/signPost', {
                method: 'POST',
                headers: headers,
                body: JSON.stringify({ some: 'data' })
            })
                .then(response => response.text())
                .then(data => {
                    var role = data.split(" ")[1];
                    var section = "default";
                    var email = data.split(" ")[0];
                    var username = data.split(" ")[2];
                    console.log(data.split(" "));
                    formData.append('role', role);
                    formData.append('section', section);
                    formData.append('email', email);
                    formData.append('username', username);
                    formData.append('postId', posterekId);
                    fetch('/api/Post/editpost', {
                        method: 'PUT',
                        body: formData,
                        headers: {
                            // 'Content-Type': 'multipart/form-data',
                            'Authorization': `Bearer ${jwt}`
                        }
                    }).then(response => response.text())
                        .then(data => {
                            if (data == "Successfully created a new post.") {
                                setTimeout(function () {
                                    window.location.replace("https://localhost:7025/pages/homeLoggedIn.html");
                                }, 3000);
                            }
                        })
                        .catch(error => {
                            document.getElementById("submitterek").disabled = true;
                            console.error("Error:", error);
                        });
                })
        }
    }
});

const loginForm = document.forms['wyloguj'];

loginForm.addEventListener('submit', function (e) {
    e.preventDefault();
    localStorage.clear();
    window.location.href = "/pages/home.html";
});