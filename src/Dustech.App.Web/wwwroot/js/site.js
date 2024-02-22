
function userScroll() {
    const navbar = document.querySelector('.brand-bar');

    window.addEventListener('scroll', () => {
        if (window.scrollY > 50) {
            navbar.classList.remove('bg-transparent');
            navbar.classList.add('brand-bar-sticky');
        } else {
            navbar.classList.add('bg-transparent');
            navbar.classList.remove('brand-bar-sticky');
        }
    });
}

document.addEventListener('DOMContentLoaded', userScroll);