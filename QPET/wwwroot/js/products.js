/*

This script provides client side filtering for products.

*/



document.addEventListener("DOMContentLoaded", function () {

    const categoryFilter =
        document.getElementById("categoryFilter");

    const subCategoryFilter =
        document.getElementById("subCategoryFilter");

    const productList =
        document.getElementById("productList");

    const productCount =
        document.getElementById("productCount");

    const noProductsMessage =
        document.getElementById("noProductsMessage");

    const clearButton =
        document.getElementById("clearProductFilters");

    const emptyStateClearButton =
        document.getElementById("emptyStateClearFilters");

    const activeFilterLabel =
        document.getElementById("activeFilterLabel");


    if (
        !categoryFilter ||
        !subCategoryFilter ||
        !productList
    ) {
        return;
    }


    const productCards =
        Array.from(
            productList.querySelectorAll(
                ".catalogue-product-card"
            )
        );


    const subCategories =
        Array.from(
            subCategoryFilter.querySelectorAll(
                "option[data-category-id]"
            )
        ).map(function (option) {

            return {
                value: option.value,
                text: option.textContent.trim(),
                categoryId: option.dataset.categoryId
            };

        });


    function loadSubCategories() {

        const categoryId =
            categoryFilter.value;

        subCategoryFilter.innerHTML =
            '<option value="">All subcategories</option>';


        if (!categoryId) {

            subCategoryFilter.disabled = true;

            return;
        }


        subCategories
            .filter(function (subCategory) {

                return (
                    subCategory.categoryId ===
                    categoryId
                );

            })
            .forEach(function (subCategory) {

                const option =
                    document.createElement("option");

                option.value =
                    subCategory.value;

                option.textContent =
                    subCategory.text;

                subCategoryFilter.appendChild(
                    option
                );

            });


        subCategoryFilter.disabled = false;
    }


    function filterProducts() {

        const categoryId =
            categoryFilter.value;

        const subCategoryId =
            subCategoryFilter.value;

        let visibleCount = 0;


        productCards.forEach(function (card) {

            const matchesCategory =
                !categoryId ||
                card.dataset.categoryId === categoryId;

            const matchesSubCategory =
                !subCategoryId ||
                card.dataset.subcategoryId ===
                subCategoryId;


            const visible =
                matchesCategory &&
                matchesSubCategory;


            card.hidden =
                !visible;


            if (visible) {
                visibleCount++;
            }

        });


        productCount.textContent =
            visibleCount +
            (
                visibleCount === 1
                    ? " product found"
                    : " products found"
            );


        productList.hidden =
            visibleCount === 0;

        noProductsMessage.hidden =
            visibleCount !== 0;


        updateActiveFilter();
    }


    function updateActiveFilter() {

        if (!categoryFilter.value) {

            activeFilterLabel.hidden =
                true;

            return;
        }


        activeFilterLabel.textContent =
            categoryFilter.options[
                categoryFilter.selectedIndex
            ].textContent.trim();

        activeFilterLabel.hidden =
            false;
    }


    function clearFilters() {

        categoryFilter.value = "";

        subCategoryFilter.innerHTML =
            '<option value="">All subcategories</option>';

        subCategoryFilter.disabled =
            true;

        filterProducts();
    }


    categoryFilter.addEventListener(
        "change",
        function () {

            loadSubCategories();
            filterProducts();

        }
    );


    subCategoryFilter.addEventListener(
        "change",
        filterProducts
    );


    clearButton.addEventListener(
        "click",
        clearFilters
    );


    emptyStateClearButton.addEventListener(
        "click",
        clearFilters
    );


    filterProducts();

});