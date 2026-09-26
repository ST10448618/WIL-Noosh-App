// Handles live search + category filtering on the Menu page.
// Pure vanilla JS — no dependencies, works offline once cached by the service worker (Phase: PWA).

document.addEventListener('DOMContentLoaded', function () {
    const searchInput = document.getElementById('menuSearch');
    const filterPills = document.querySelectorAll('.filter-pill');
    const gridItems = document.querySelectorAll('.menu-grid-item');
    const noResultsMessage = document.getElementById('menuNoResults');

    let activeCategory = 'all';
    let searchTerm = '';

    function applyFilters() {
        let visibleCount = 0;

        gridItems.forEach(function (item) {
            const itemCategory = item.getAttribute('data-category');
            const itemName = item.getAttribute('data-name');

            const matchesCategory = activeCategory === 'all' || itemCategory === activeCategory;
            const matchesSearch = itemName.includes(searchTerm);

            if (matchesCategory && matchesSearch) {
                item.classList.remove('d-none');
                visibleCount++;
            } else {
                item.classList.add('d-none');
            }
        });

        // Toggle the "no results" message based on whether anything is visible.
        if (visibleCount === 0) {
            noResultsMessage.classList.remove('d-none');
        } else {
            noResultsMessage.classList.add('d-none');
        }
    }
