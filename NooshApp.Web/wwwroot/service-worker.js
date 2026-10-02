// Service Worker for NOOSH PWA
// Handles install-time caching, activation cleanup,
// offline navigation, and fast static asset loading.

const CACHE_VERSION = 'noosh-cache-v10';

const CORE_ASSETS = [
    '/',
    '/offline.html',
    '/css/site.css',
    '/js/site.js',
    '/lib/bootstrap/dist/css/bootstrap.min.css',
    '/lib/bootstrap/dist/js/bootstrap.bundle.min.js',
    '/images/icons/icon-192.png',
    '/images/icons/icon-512.png'
];

// INSTALL
self.addEventListener('install', function (event) {
    event.waitUntil(
        caches.open(CACHE_VERSION)
            .then(function (cache) {
                return cache.addAll(CORE_ASSETS);
            })
    );

    self.skipWaiting();
});

// ACTIVATE
self.addEventListener('activate', function (event) {
    event.waitUntil(
        caches.keys().then(function (cacheNames) {
            return Promise.all(
                cacheNames
                    .filter(function (name) {
                        return name !== CACHE_VERSION;
                    })
                    .map(function (name) {
                        return caches.delete(name);
                    })
            );
        })
    );

    self.clients.claim();
});

// FETCH
self.addEventListener('fetch', function (event) {
    const request = event.request;

    if (request.method !== 'GET') {
        return;
    }

    // Page navigation
    if (request.mode === 'navigate') {
        event.respondWith(
            fetch(request)
                .catch(function () {
                    return caches.match('/offline.html');
                })
        );

        return;
    }

    const url = new URL(request.url);

    // Never cache API requests
    if (url.pathname.startsWith('/api/')) {
        event.respondWith(
            fetch(request)
                .catch(function () {
                    return Response.error();
                })
        );

        return;
    }

    // Cache same-origin static assets
    if (url.origin === self.location.origin) {
        event.respondWith(
            caches.match(request).then(function (cachedResponse) {

                const networkFetch = fetch(request)
                    .then(function (networkResponse) {

                        if (networkResponse.ok) {
                            caches.open(CACHE_VERSION)
                                .then(function (cache) {
                                    cache.put(
                                        request,
                                        networkResponse.clone()
                                    );
                                });
                        }

                        return networkResponse;
                    })
                    .catch(function () {
                        if (cachedResponse) {
                            return cachedResponse;
                        }

                        return Response.error();
                    });

                if (cachedResponse) {
                    return cachedResponse;
                }

                return networkFetch;
            })
        );

        return;
    }

    // Third-party requests
    event.respondWith(
        fetch(request)
    );
});