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

const inputs = document.querySelectorAll("input");

Object.keys(inputs).map((key, input) => onBlurOnFocus(inputs[key]));

function onBlurOnFocus(input) {
    input.onfocus = (e) => {
        e.target.previousElementSibling.classList.toggle("text-primary");
    };
    input.onblur = (e) => {
        e.target.previousElementSibling.classList.toggle("text-primary");
    };
}


let failedLoginAttempts = 0;

// register form
const loginForm = document.forms['login'];
const buttoneczek = document.getElementById("submitterek");

loginForm.addEventListener('submit', function (e) {
    e.preventDefault();
    buttoneczek.disabled = true;
    setTimeout(() => {
        buttoneczek.disabled = false;
    }, 2000);
    var username = document.getElementById('username').children['username'].value;
    var password = document.getElementById('password').children['password'].value;
    setTimeout(() => {
        if (loginForm.checkValidity()) {
            fetch('/api/AuthAuth/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    username: username,
                    password: password
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
                        alert.textContent = 'Wrong username or password.';
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