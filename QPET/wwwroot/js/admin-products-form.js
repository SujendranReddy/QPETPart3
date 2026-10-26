
/*
This file updates the available product subcategories when an 
admin selects a category
 */



document.addEventListener("DOMContentLoaded", function () {
    const categorySelect =
        document.getElementById("productCategory");

    const subCategorySelect =
        document.getElementById("productSubCategory");

    if (categorySelect && subCategorySelect) {
        const placeholder =
            subCategorySelect.querySelector('option[value=""]')
                ?.textContent.trim() || "Select Subcategory";

        const subCategories = Array.from(
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
            const categoryId = categorySelect.value;

            subCategorySelect.replaceChildren();

            const defaultOption =
                document.createElement("option");

            defaultOption.value = "";
            defaultOption.textContent = placeholder;

            subCategorySelect.appendChild(defaultOption);

            if (!categoryId || categoryId === "0") {
                subCategorySelect.disabled = true;
                return;
            }

            subCategories
                .filter(function (subCategory) {
                    return subCategory.categoryId === categoryId;
                })
                .forEach(function (subCategory) {
                    const option =
                        document.createElement("option");

                    option.value = subCategory.value;
                    option.textContent = subCategory.text;

                    subCategorySelect.appendChild(option);
                });

            subCategorySelect.disabled = false;

            if (selectedValue) {
                subCategorySelect.value = selectedValue;
            }
        }

        categorySelect.addEventListener("change", function () {
            loadSubCategories();
        });

        loadSubCategories(selectedSubCategory);
    }

    const imageInput =
        document.getElementById("productImageInput");

    const imagePreview =
        document.getElementById("productImagePreview");

    const imagePlaceholder =
        document.getElementById("productImagePlaceholder");

    if (imageInput && imagePreview) {
        let previewUrl = null;

        const originalSrc =
            imagePreview.dataset.originalSrc || "";

        imageInput.addEventListener("change", function () {
            if (previewUrl) {
                URL.revokeObjectURL(previewUrl);
                previewUrl = null;
            }

            const file = imageInput.files?.[0];

            if (file && file.type.startsWith("image/")) {
                previewUrl = URL.createObjectURL(file);
                imagePreview.src = previewUrl;
                imagePreview.hidden = false;

                if (imagePlaceholder) {
                    imagePlaceholder.hidden = true;
                }
            }
            else if (originalSrc) {
                imagePreview.src = originalSrc;
                imagePreview.hidden = false;

                if (imagePlaceholder) {
                    imagePlaceholder.hidden = true;
                }
            }
            else {
                imagePreview.removeAttribute("src");
                imagePreview.hidden = true;

                if (imagePlaceholder) {
                    imagePlaceholder.hidden = false;
                }
            }
        });

        window.addEventListener("pagehide", function () {
            if (previewUrl) {
                URL.revokeObjectURL(previewUrl);
            }
        });
    }
});
