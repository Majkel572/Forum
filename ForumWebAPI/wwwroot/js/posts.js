function userIsAuthorized() {
    const jwt = localStorage.getItem('jwt');
    if (jwt === null) {
        alert("You must be logged in to create a post.");
    } else {
        const headers = new Headers({
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${jwt}`
        });
        fetch('https://localhost:7025/api/User/checkAuth', {
            method: 'POST',
            headers: headers,
            body: JSON.stringify({ some: 'data' })
        })
            .then(response => response.text())
            .then(data => {
                if (data === "admin") {
                    window.location.replace("https://localhost:7025/pages/addPost.html");
                } else if (data === "mod") {
                    window.location.replace("https://localhost:7025/pages/addPost.html");
                } else if (data === "owner") {
                    window.location.replace("https://localhost:7025/pages/addPost.html");
                } else if (data === "default") {
                    window.location.replace("https://localhost:7025/pages/addPost.html");
                } else {
                    alert("You must be logged in to create a post.");
                }
            })                    
    }

}
function createPost() {
    // Redirect to createPost.html for authorized user
    userIsAuthorized();
}