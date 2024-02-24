(function() {

    const brandBarCollapsible = document.getElementById('brand-bar-collapsible');
    //const bsCollapse = new bootstrap.Collapse('#brand-bar-collapsible', {});
    const brandBar = document.querySelector('.brand-bar');
    const dsSmBreakpoint = 576;
    const maxScrollForNavbarTransparency = 50;  
    const navbarToggler = document.querySelector('.navbar-toggler');
    
    let lastWidth = window.innerWidth;

    function isBrandBarCollapsibleShown(){
        return brandBarCollapsible.classList.contains('show');
    }
    function userScroll() {

        window.addEventListener('scroll', () => {
            if (window.scrollY > maxScrollForNavbarTransparency) {
                makeNavbarVisible(brandBar);
            } else if(!isBrandBarCollapsibleShown()){
                makeNavbarTransparent(brandBar);
            }
        });
    }

    function userResizeWindowX(){
        window.addEventListener('resize', event =>
        {
            const actualWidth = event.target.innerWidth;
            if(actualWidth !== lastWidth)
            {
                if(actualWidth > dsSmBreakpoint && isBrandBarCollapsibleShown()){
                    navbarToggler.click();
                }
                lastWidth = actualWidth;
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


    brandBarCollapsible.addEventListener('show.bs.collapse', () => {
        
        makeNavbarVisible(brandBar);
    })

    brandBarCollapsible.addEventListener('hide.bs.collapse', () => {
        
        if(window.scrollY <= maxScrollForNavbarTransparency)
        {
            makeNavbarTransparent(brandBar);
        }
    })

    

    document.addEventListener('DOMContentLoaded', userScroll);
    document.addEventListener('DOMContentLoaded', userResizeWindowX);
    

})();
