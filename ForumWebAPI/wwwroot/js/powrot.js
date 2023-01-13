if (document.getElementById("skrimisha") != null) {
    document.getElementById("skrimisha").addEventListener("click", function () {
        localStorage.removeItem('PostId');
        window.location.href = "/pages/genDiscussionPosts.html";
    });
}

if (document.getElementById("skrimishaHome") != null) {
    document.getElementById("skrimishaHome").addEventListener("click", function () {
        window.location.href = "/pages/homeLoggedIn.html";
    });
}

if (document.getElementById("skrimishaadminowski") != null) {
    document.getElementById("skrimishaadminowski").addEventListener("click", function () {
        window.location.href = "/pages/staffDiscussionPosts.html";
    });
}