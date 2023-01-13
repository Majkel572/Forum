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
            document.getElementById("usernameDisplay").innerHTML = data.split(" ")[0] + " " + data.split(" ")[1] + "<br>" + data.split(" ")[2] + " " + data.split(" ")[3];
        })
}

const loginForm = document.forms['wyloguj'];

loginForm.addEventListener('submit', function (e) {
    e.preventDefault();
    localStorage.clear();
    window.location.href = "/pages/home.html";
});