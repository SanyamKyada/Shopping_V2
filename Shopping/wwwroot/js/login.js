$(document).ready(function () {
    debugger;
    var modal = document.getElementById("login-modal");

    // Get the login button
    var loginBtn = document.getElementById("login-btn");

    // Get the close button
    var closeBtn = modal.getElementsByClassName("close")[0];

    // When the user clicks on the login button, show the modal
    loginBtn.onclick = function () {
        alert('me');
        modal.style.display = "block";
    }

    // When the user clicks on the close button, hide the modal
    closeBtn.onclick = function () {
        modal.style.display = "none";
    }

});
    // Get the modal
    
// When the user clicks anywhere outside of the modal, hide the modal
window.onclick = function (event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
}