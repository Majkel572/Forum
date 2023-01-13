window.onload = function () {
    const jwt = localStorage.getItem('jwt');
    if (jwt == null) {
        
    } else {
        window.location.href = "/pages/homeLoggedIn.html";
    }
};
