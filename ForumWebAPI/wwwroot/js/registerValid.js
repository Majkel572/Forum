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
  
  function calculateEntropy(password) {
    var characterSetSize = 0;//26 + 26 + 10;// + 28; // 26 letters in lowercase, 26 letters in uppercase, 10 digits, 28 special characters that are enabled
    let entropy = 0;
    if(password.toUpperCase() != password){
      characterSetSize += 26;
    }
    if(password.toLowerCase() != password){
      characterSetSize += 26;
    }
    if(/[0-9]/.test(password)){
      characterSetSize += 10;
    }
    var specialChars = /^[!@#$%&()_+\-=\[\]{};':"\\|,.\/?~]*$/g;
    if(specialChars.test(password)){
      characterSetSize += 22;
    }
    entropy = Math.log2(characterSetSize) * password.length;
    return entropy;
  }
  
  const passwordField = document.getElementById('password').children['passwordE'];
  passwordField.addEventListener('input', () => {
    const password = passwordField.value;
    const entropy = calculateEntropy(password);
    var entropyStrength = "Weak";
    if (entropy < 40) {
      entropyStrength = "Weak";
    } else if (entropy >= 40 && entropy < 60) {
      entropyStrength = "Normal";
    } else if (entropy >= 60 && entropy < 80) {
      entropyStrength = "Strong";
    } else if (entropy >= 80 && entropy < 100) {
      entropyStrength = "Super-strong";
    } else {
      entropyStrength = "Godlike";
    }
    const alert = document.createElement('div');
    alert.classList.add('alert', 'alert-info');
    alert.textContent = 'Password strengthmeter: ' + entropyStrength;
    const alertContainer = document.getElementById('entropy-alert-container');
    while (alertContainer.hasChildNodes()) {
      alertContainer.removeChild(alertContainer.firstChild);
    }
    alertContainer.appendChild(alert);
    // setTimeout(() => {
    //   alertContainer.removeChild(alert);
    // }, 5000);
  });
  
  // register form
  const registerForm = document.forms['register'];
  
  registerForm.addEventListener('submit', function (e) {
    e.preventDefault();
  
    var name = document.getElementById('name').children['name'].value;
    var surname = document.getElementById('surname').children['surname'].value;
    var country = document.getElementById('country').children['country'].value;
    var birthdate = document.getElementById('birthdate').children['birthdate'].value;
    var email = document.getElementById('email').children['email'].value;
    var username = document.getElementById('username').children['username'].value;
    var password = document.getElementById('password').children['passwordE'].value;
    console.log(name + " " + surname + " " + country + " " + birthdate + " " + email + " " + username + " " + password);
  
    if (registerForm.checkValidity()) {
      fetch('https://localhost:7025/api/User/register', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          name: name,
          surname: surname,
          country: country,
          birthDate: birthdate,
          email: email,
          username: username,
          password: password
        })
      });
      setTimeout(function() {
        window.location.replace("https://localhost:7025/pages/confirmRegister.html");
      }, 3000);
    }
  });