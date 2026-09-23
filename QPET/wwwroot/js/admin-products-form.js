
/*
This file updates the available product subcategories when an 
admin selects a category
 */


document.addEventListener("DOMContentLoaded", function () {

    const categorySelect =
        document.getElementById("productCategory");

    const subCategorySelect =
        document.getElementById("productSubCategory");


    if (!categorySelect || !subCategorySelect) {
        return;
    }


    const subCategories =
        Array.from(
            subCategorySelect.querySelectorAll(
                "option[data-category-id]"
            )
        ).map(function (option) {

            return {
                value: option.value,
                text: option.textContent.trim(),
                categoryId: option.dataset.categoryId
            };

        });


    const selectedSubCategory =
        subCategorySelect.dataset.selectedSubcategory || "";


    function loadSubCategories(selectedValue = "") {

        const categoryId =
            categorySelect.value;

        subCategorySelect.innerHTML =
            '<option value="">Select Subcategory</option>';


        if (!categoryId || categoryId === "0") {

            subCategorySelect.disabled = true;

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

                subCategorySelect.appendChild(option);

            });


        subCategorySelect.disabled = false;


        if (selectedValue) {
            subCategorySelect.value = selectedValue;
        }
    }


    categorySelect.addEventListener(
        "change",
        function () {

            loadSubCategories();

        }
    );


    loadSubCategories(selectedSubCategory);

});