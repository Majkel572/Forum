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
            const abc = document.getElementById("username-display").innerHTML = data.split(" ")[0] + " " + data.split(" ")[1] + "<br>" + data.split(" ")[2] + " " + data.split(" ")[3];
        })
}

const changeForm = document.forms['chpswd'];

changeForm.addEventListener('submit', function (e) {
    e.preventDefault();
    const oldp = document.getElementById('oldpasswordo').children['oldpassword'].value;
    const newp = document.getElementById('password').children['password'].value;
    const headers = new Headers({
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${jwt}`
    });
    fetch('/api/User/chpswd', {
        method: 'PUT',
        body: JSON.stringify([oldp, newp]),
        headers: headers
    })
        .then(response => {
            if(response.ok){
                window.location.href = "/pages/homeLoggedIn.html";
            } else {
                return;
            }
        })
        .then(data => console.log(data))
        .catch(error => console.error(error))
});