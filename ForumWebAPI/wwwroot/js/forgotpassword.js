const loginForm = document.forms['login'];
const buttoneczek = document.getElementById("submitterek");
let failedLoginAttempts = 0;
loginForm.addEventListener('submit', function (e) {
    e.preventDefault();
    buttoneczek.disabled = true;
    setTimeout(() => {
        buttoneczek.disabled = false;
    }, 2000);
    var email = document.getElementById('email').children['email'].value;
    var code = document.getElementById('code').children['code'].value;
    setTimeout(() => {
        if (loginForm.checkValidity()) {
            fetch('/api/AuthAuth/forgot', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    username: email,
                    password: code
                })
            })
                .then(response => {
                    let alerts = document.querySelectorAll('.alert');
                    alerts.forEach(alert => {
                        alert.remove();
                    });
                    if (response.ok) {
                        failedLoginAttempts = 0;
                        window.location.href = "/pages/homeLoggedIn.html";
                        return response.text();
                    }
                    failedLoginAttempts++;
                    if (failedLoginAttempts >= 3) {
                        buttoneczek.disabled = true;
                        failedLoginAttempts = 0;
                        const alert = document.createElement('div');
                        alert.classList.add('alert', 'alert-danger');
                        alert.textContent = "Too many login fails. Wait until u can do more.";
                        const alertContainer = document.getElementById('alert-container');
                        alertContainer.appendChild(alert);
                        setTimeout(() => {
                            alertContainer.removeChild(alert);
                        }, 5000);
                        setTimeout(() => {
                            buttoneczek.disabled = false;
                        }, 5000);
                    } else {
                        const alert = document.createElement('div');
                        alert.classList.add('alert', 'alert-danger');
                        alert.textContent = 'Wrong email or code.';
                        const alertContainer = document.getElementById('alert-container');
                        alertContainer.appendChild(alert);
                        setTimeout(() => {
                            alertContainer.removeChild(alert);
                        }, 5000);
                    }
                    throw new Error('Request failed: ' + response.status);
                })
                .then(jwt => {
                    localStorage.setItem('jwt', jwt);
                })
                .catch(error => console.error(error));
        }
    }, 2000);
    
});