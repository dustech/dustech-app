const brandBarCollapsible = document.getElementById('brand-bar-collapsible');

function getNavbar(){
    return document.querySelector('.brand-bar');
}

function getMaxScrollForNavbarTransparency(){
    return 50;
}

function isBrandBarCollapsibleShown(){
    return brandBarCollapsible.classList.contains('show');
}
function userScroll() {
    
    const navbar = getNavbar();

    window.addEventListener('scroll', () => {
        if (window.scrollY > getMaxScrollForNavbarTransparency()) {
            makeNavbarVisible(navbar);
        } else if(!isBrandBarCollapsibleShown()){
            makeNavbarTransparent(navbar);
        }
    });
}

function makeNavbarVisible(navbar){
    navbar.classList.remove('bg-transparent');
    navbar.classList.add('brand-bar-sticky');
}

function makeNavbarTransparent(navbar) {
    navbar.classList.add('bg-transparent');
    navbar.classList.remove('brand-bar-sticky');
}


brandBarCollapsible.addEventListener('show.bs.collapse', event => {
    const navbar = getNavbar();
    makeNavbarVisible(navbar);
})

brandBarCollapsible.addEventListener('hide.bs.collapse', event => {
    const navbar = getNavbar();
    if(window.scrollY <= getMaxScrollForNavbarTransparency())
    {
        makeNavbarTransparent(navbar);
    }
})


document.addEventListener('DOMContentLoaded', userScroll);