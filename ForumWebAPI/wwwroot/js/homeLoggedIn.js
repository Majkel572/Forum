window.onload = function () {
    const jwt = localStorage.getItem('jwt');
    if (jwt === null) {
        window.location.replace("https://localhost:7025/pages/home.html");
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
                console.log(data.split(" "));
                document.getElementById("usernameDisplay").innerHTML = data.split(" ")[0] + " " + data.split(" ")[1] + "<br>" + data.split(" ")[2] + " " + data.split(" ")[3] + "<br>" + data.split(" ")[4] + " " + data.split(" ")[5] + " " + data.split(" ")[6] + " " + data.split(" ")[7];
            })
    }
};

const loginForm = document.forms['wyloguj'];

loginForm.addEventListener('submit', function (e) {
    e.preventDefault();
    localStorage.clear();
    window.location.href = "/pages/home.html";
});

const changePasswordForm = document.forms['haslo'];
changePasswordForm.addEventListener('submit', function (e) {
    e.preventDefault();
    window.location.href = "/pages/changePassword.html";
});