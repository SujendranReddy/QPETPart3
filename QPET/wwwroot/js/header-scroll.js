document.addEventListener("DOMContentLoaded", () => {
    const header =
        document.getElementById("siteHeader");

    const navigation =
        document.getElementById("mainNavigation");

    const menuToggle =
        document.getElementById("menuToggle");

    if (!header) {
        return;
    }

    let previousScrollPosition =
        window.scrollY;

    let ticking = false;

    const navigationIsOpen = () => {
        const classOpen =
            navigation?.classList.contains("open") ?? false;

        const ariaOpen =
            menuToggle?.getAttribute("aria-expanded") === "true";

        return classOpen || ariaOpen;
    };

    const updateHeader = () => {
        const currentScrollPosition =
            window.scrollY;

        if (
            currentScrollPosition <= 20 ||
            navigationIsOpen()
        ) {
            header.classList.remove(
                "site-header-hidden"
            );

            previousScrollPosition =
                currentScrollPosition;

            ticking = false;

            return;
        }

        if (
            currentScrollPosition >
            previousScrollPosition &&
            currentScrollPosition >
            header.offsetHeight
        ) {
            header.classList.add(
                "site-header-hidden"
            );
        }
        else {
            header.classList.remove(
                "site-header-hidden"
            );
        }

        previousScrollPosition =
            currentScrollPosition;

        ticking = false;
    };

    window.addEventListener(
        "scroll",
        () => {
            if (ticking) {
                return;
            }

            window.requestAnimationFrame(
                updateHeader
            );

            ticking = true;
        },
        {
            passive: true
        }
    );

    menuToggle?.addEventListener(
        "click",
        () => {
            header.classList.remove(
                "site-header-hidden"
            );
        }
    );

    window.addEventListener(
        "resize",
        () => {
            header.classList.remove(
                "site-header-hidden"
            );
        }
    );
});