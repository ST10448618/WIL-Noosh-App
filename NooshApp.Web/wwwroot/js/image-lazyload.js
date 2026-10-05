(function () {

    document.addEventListener('DOMContentLoaded', function () {

        document.querySelectorAll('img').forEach(function (img) {

            if (
                img.hasAttribute('loading') ||
                img.hasAttribute('data-eager')
            ) {
                return;
            }

            img.setAttribute('loading', 'lazy');
        });

    });

})();