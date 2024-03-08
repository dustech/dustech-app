(
    function () {
        function hideMask(mask) {
            setTimeout(() => {
                mask.classList.add("hidden");
            }, 200);
        }

        document.addEventListener('DOMContentLoaded', (event) => {
            const video = document.getElementById('background-video');
            const siteMask = document.getElementById("site-mask");


            if (video) {

                

                if (!video.hasAttribute("src")) {
                    video.src = "/media/background.mp4";
                }
                
                video.addEventListener('loadeddata', function () {
                    video.playbackRate = 0.5;
                    if (siteMask) {
                        hideMask(siteMask);
                    }

                });


            }

            if (!video && siteMask) {
                hideMask(siteMask);
            } else if (siteMask) { //fallback in case of errors
                setTimeout(() => {
                    hideMask(siteMask);
                }, 5000);
            }

        });
    }
)
();