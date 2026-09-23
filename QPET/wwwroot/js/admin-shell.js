/*
This file controls the responsive admin sidebar. 
*/


document.addEventListener(
    "DOMContentLoaded",
    function () {

        const sidebar =
            document.getElementById(
                "adminSidebar"
            );


        if (!sidebar) {
            return;
        }


        const menuToggle =
            document.getElementById(
                "adminMenuToggle"
            );


        const overlay =
            document.getElementById(
                "adminSidebarOverlay"
            );


        function openSidebar() {

            sidebar.classList.add(
                "open"
            );


            overlay?.classList.add(
                "open"
            );


            menuToggle?.setAttribute(
                "aria-expanded",
                "true"
            );
        }


        function closeSidebar() {

            sidebar.classList.remove(
                "open"
            );


            overlay?.classList.remove(
                "open"
            );


            menuToggle?.setAttribute(
                "aria-expanded",
                "false"
            );
        }


        menuToggle?.addEventListener(
            "click",
            function () {

                if (
                    sidebar.classList.contains(
                        "open"
                    )
                ) {
                    closeSidebar();
                }
                else {
                    openSidebar();
                }
            }
        );


        overlay?.addEventListener(
            "click",
            closeSidebar
        );


        sidebar
            .querySelectorAll("a")
            .forEach(
                function (link) {

                    link.addEventListener(
                        "click",
                        function () {

                            if (
                                window.innerWidth <=
                                900
                            ) {
                                closeSidebar();
                            }
                        }
                    );
                }
            );

    }
);