function openTab(tabName) {
    var i;
    var x = document.getElementsByClassName("tab-content");
    for (i = 0; i < x.length; i++) {
        x[i].style.display = "none";
    }
    document.getElementById(tabName).style.display = "block";
    
    var tabs = document.getElementsByClassName("tab-button");
    for (i = 0; i < tabs.length; i++) {
        tabs[i].className = tabs[i].className.replace(" active", "");
    }
    event.currentTarget.className += " active";
}

function toggleMobileMenu() {
    const desktopMenu = document.querySelector('.desktop-menu');
    desktopMenu.style.display = desktopMenu.style.display === 'block' ? 'none' : 'block';
}

window.onclick = function(event) {
    if (!event.target.matches('.mobile-menu-btn') && !event.target.closest('.mobile-menu-btn')) {
        var mobileMenu = document.getElementById("mobileMenu");
        if (mobileMenu && mobileMenu.style.display === "block") {
            mobileMenu.style.display = "none";
        }
    }
}

