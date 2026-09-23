document.addEventListener(
    "DOMContentLoaded",
    function () {

        const menuToggle =
            document.getElementById(
                "menuToggle"
            );

        const mainNavigation =
            document.getElementById(
                "mainNavigation"
            );


        if (!menuToggle ||
            !mainNavigation) {

            return;
        }


        menuToggle.addEventListener(
            "click",
            function () {

                mainNavigation
                    .classList
                    .toggle("open");


                const isOpen =
                    mainNavigation
                        .classList
                        .contains("open");


                menuToggle.setAttribute(
                    "aria-expanded",
                    isOpen
                );

            }
        );


 
        mainNavigation
            .querySelectorAll("a")
            .forEach(
                function (link) {

                    link.addEventListener(
                        "click",
                        function () {

                            mainNavigation
                                .classList
                                .remove("open");


                            menuToggle.setAttribute(
                                "aria-expanded",
                                "false"
                            );

                        }
                    );

                }
            );

    }
);