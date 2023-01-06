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



// register form
const registerForm = document.forms['register'];

registerForm.addEventListener('submit', function(e){
  e.preventDefault();
  
  const value = registerForm.querySelector('input[type="text"]').value;

  var name = document.getElementById('name').children['name'].value;
  var surname = document.getElementById('surname').children['surname'].value;
  var country = document.getElementById('country').children['country'].value;
  var birthdate = document.getElementById('birthdate').children['birthdate'].value;
  var email = document.getElementById('email').children['email'].value;
  var username = document.getElementById('username').children['username'].value;
  var password = document.getElementById('password').children['password'].value;
  console.log(name + " " + surname + " " + country + " " + birthdate + " " + email + " " + username + " " + password);
});