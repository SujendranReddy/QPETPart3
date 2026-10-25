/*

This script provides client side filtering for products.

*/




document.addEventListener("DOMContentLoaded", function () {
    const categoryFilter = document.getElementById("categoryFilter");
    const subCategoryFilter = document.getElementById("subCategoryFilter");
    const capacityFilter = document.getElementById("capacityFilter");
    const productList = document.getElementById("productList");
    const productCount = document.getElementById("productCount");
    const noProductsMessage = document.getElementById("noProductsMessage");
    const clearButton = document.getElementById("clearProductFilters");
    const emptyStateClearButton = document.getElementById("emptyStateClearFilters");
    const activeFilterLabel = document.getElementById("activeFilterLabel");

    if (!categoryFilter || !subCategoryFilter || !capacityFilter ||
        !productList || !productCount || !noProductsMessage ||
        !clearButton || !emptyStateClearButton || !activeFilterLabel) {
        return;
    }

    const productCards = Array.from(
        productList.querySelectorAll(".catalogue-product-card")
    );

    const subCategories = Array.from(
        subCategoryFilter.querySelectorAll("option[data-category-id]")
    ).map(function (option) {
        return {
            value: option.value,
            name: option.textContent.trim(),
            categoryId: option.dataset.categoryId
        };
    });

    function normalizeCapacity(value) {
        return (value || "").replace(/\s+/g, "").toLowerCase();
    }

    function loadSubCategories() {
        const categoryId = categoryFilter.value;

        const matchingSubCategories = subCategories.filter(function (item) {
            return item.categoryId === categoryId;
        });

        subCategoryFilter.replaceChildren(
            new Option("All subcategories", "")
        );

        matchingSubCategories.forEach(function (item) {
            subCategoryFilter.add(new Option(item.name, item.value));
        });

        subCategoryFilter.disabled =
            !categoryId || matchingSubCategories.length === 0;
    }

    function updateActiveFilter() {
        const selected = [categoryFilter, subCategoryFilter, capacityFilter]
            .filter(function (filter) {
                return filter.value !== "";
            })
            .map(function (filter) {
                return filter.options[filter.selectedIndex].textContent.trim();
            });

        activeFilterLabel.textContent = selected.join(" · ");
        activeFilterLabel.hidden = selected.length === 0;
    }

    function filterProducts() {
        const categoryId = categoryFilter.value;
        const subCategoryId = subCategoryFilter.value;
        const capacity = normalizeCapacity(capacityFilter.value);

        let visibleCount = 0;

        productCards.forEach(function (card) {
            const matchesCategory =
                !categoryId || card.dataset.categoryId === categoryId;

            const matchesSubCategory =
                !subCategoryId || card.dataset.subcategoryId === subCategoryId;

            const matchesCapacity =
                !capacity ||
                normalizeCapacity(card.dataset.capacity) === capacity;

            const visible =
                matchesCategory &&
                matchesSubCategory &&
                matchesCapacity;

            card.hidden = !visible;

            if (visible) {
                visibleCount++;
            }
        });

        productCount.textContent = visibleCount +
            (visibleCount === 1 ? " product found" : " products found");

        productList.hidden = visibleCount === 0;
        noProductsMessage.hidden = visibleCount !== 0;

        updateActiveFilter();
    }

    function clearFilters() {
        categoryFilter.value = "";
        capacityFilter.value = "";

        loadSubCategories();
        filterProducts();
    }

    categoryFilter.addEventListener("change", function () {
        loadSubCategories();
        filterProducts();
    });

    subCategoryFilter.addEventListener("change", filterProducts);
    capacityFilter.addEventListener("change", filterProducts);

    clearButton.addEventListener("click", clearFilters);
    emptyStateClearButton.addEventListener("click", clearFilters);

    loadSubCategories();
    filterProducts();
});
