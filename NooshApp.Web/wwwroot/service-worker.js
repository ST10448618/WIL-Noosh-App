const CACHE_VERSION = 'noosh-cache-v15';

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

self.addEventListener('install', function (event) {
    event.waitUntil(
        caches.open(CACHE_VERSION)
            .then(function (cache) {
                return Promise.all(
                    CORE_ASSETS.map(function (url) {
                        return cache.add(url)
                            .catch(function (error) {
                                console.warn(
                                    'Could not cache:',
                                    url,
                                    error
                                );
                            });
                    })
                );
            })
            .then(function () {
                return self.skipWaiting();
            })
    );
});

self.addEventListener('activate', function (event) {
    event.waitUntil(
        caches.keys()
            .then(function (cacheNames) {
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
            .then(function () {
                return self.clients.claim();
            })
    );
});

self.addEventListener('fetch', function (event) {
    const request = event.request;

    if (request.method !== 'GET') {
        return;
    }

    const url = new URL(request.url);

    if (url.pathname.startsWith('/api/')) {
        return;
    }

    if (url.origin !== self.location.origin) {
        return;
    }

    if (request.mode === 'navigate') {
        event.respondWith(
            fetch(request)
                .catch(function () {
                    return caches.match('/offline.html');
                })
        );

        return;
    }

    event.respondWith(
        caches.match(request)
            .then(function (cachedResponse) {

                if (cachedResponse) {
                    return cachedResponse;
                }

                return fetch(request)
                    .then(function (networkResponse) {

                        if (!networkResponse || !networkResponse.ok) {
                            return networkResponse;
                        }

                        const responseToCache =
                            networkResponse.clone();

                        caches.open(CACHE_VERSION)
                            .then(function (cache) {
                                cache.put(
                                    request,
                                    responseToCache
                                );
                            });

                        return networkResponse;
                    });
            })
    );
});