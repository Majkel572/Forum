window.onload = function () {
    const code = localStorage.getItem('code');
    const codeID = document.getElementById("codeID");
    codeID.innerHTML = code;
};
