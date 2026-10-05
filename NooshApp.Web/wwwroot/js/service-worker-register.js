// Registers the service worker so the browser knows this site is a PWA
// and can cache assets / work offline.

if ('serviceWorker' in navigator) {
    window.addEventListener('load', function () {
        navigator.serviceWorker.register('/service-worker.js')
            .then(function (registration) {
                console.log('Service Worker registered with scope:', registration.scope);
            })
            .catch(function (error) {
                console.error('Service Worker registration failed:', error);
            });
    });
}