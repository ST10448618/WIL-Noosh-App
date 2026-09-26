// Powers the "Customize Your Shawarma" live order summary.
// Ingredients start all-included; clicking toggles them off/on.
// Summary panel re-renders on every change — protein, or ingredient toggle.

document.addEventListener('DOMContentLoaded', function () {
    const proteinInputs = document.querySelectorAll('input[name="protein"]');
    const ingredientToggles = document.querySelectorAll('.ingredient-toggle');

    const summaryProtein = document.getElementById('summaryProtein');
    const summaryIngredientList = document.getElementById('summaryIngredientList');
    const summaryTotal = document.getElementById('summaryTotal');

    function getSelectedProtein() {
        const checked = document.querySelector('input[name="protein"]:checked');
        return {
            name: checked.value,
            price: parseFloat(checked.getAttribute('data-price'))
        };
    }

    function getActiveIngredients() {
        const active = [];
        ingredientToggles.forEach(function (toggle) {
            if (toggle.classList.contains('active')) {
                active.push(toggle.getAttribute('data-name'));
            }
        });
        return active;
    }

    function renderSummary() {
        const protein = getSelectedProtein();
        const activeIngredients = getActiveIngredients();

        // Update protein name + total price
        summaryProtein.textContent = protein.name;
        summaryTotal.textContent = 'R' + protein.price.toFixed(2);

        // Rebuild the included-ingredients list
        summaryIngredientList.innerHTML = '';

        if (activeIngredients.length === 0) {
            const li = document.createElement('li');
            li.textContent = 'No extras — just the protein.';
            li.classList.add('summary-empty');
            summaryIngredientList.appendChild(li);
        } else {
            activeIngredients.forEach(function (name) {
                const li = document.createElement('li');
                li.textContent = name;
                summaryIngredientList.appendChild(li);
            });
        }
    }

     // Protein selection changes the summary.
    proteinInputs.forEach(function (input) {
        input.addEventListener('change', renderSummary);
    });

    // Clicking an ingredient toggles it on/off and updates styling + summary.
    ingredientToggles.forEach(function (toggle) {
        toggle.addEventListener('click', function () {
            toggle.classList.toggle('active');
            renderSummary();
        });
    });

    // "Add To Order" — for now, confirms the selection.
    // Real cart/order persistence is out of scope for the Master Prompt's
    // feature list, so this stays a lightweight confirmation for now.
    document.getElementById('addToOrderBtn').addEventListener('click', function () {
        const protein = getSelectedProtein();
        const activeIngredients = getActiveIngredients();
        const summaryText = protein.name + ' Shawarma with: ' +
            (activeIngredients.length ? activeIngredients.join(', ') : 'no extras');

        alert('Added to order!\n\n' + summaryText + '\nTotal: R' + protein.price.toFixed(2));
    });

    // Render the initial state on page load.
    renderSummary();
});