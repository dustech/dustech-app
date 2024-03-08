(
    function() {
        function hideMask(mask) {
            setTimeout(() => {
                mask.classList.add("hidden");
            }, 300);
        }

        document.addEventListener('DOMContentLoaded', (event) => {
            const video = document.getElementById('background-video');
            const siteMask = document.getElementById("site-mask");


            if (video) {
                video.addEventListener('loadeddata', function () {
                    video.playbackRate = 0.5;
                    if (siteMask) {
                        hideMask(siteMask);
                    }
                });
            }

            if (!video && siteMask) {
                hideMask(siteMask);
            }

        });
    }
)
();