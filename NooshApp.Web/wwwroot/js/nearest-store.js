// Finds the nearest Noosh store to the visitor and points delivery
// buttons (navbar dropdown, if present, and the homepage "Get It
// Delivered" section, if present) at that store's real delivery links.

function haversineDistanceKm(lat1, lng1, lat2, lng2) {
    const R = 6371;
    const dLat = (lat2 - lat1) * Math.PI / 180;
    const dLng = (lng2 - lng1) * Math.PI / 180;
    const a = Math.sin(dLat / 2) ** 2 +
        Math.cos(lat1 * Math.PI / 180) * Math.cos(lat2 * Math.PI / 180) *
        Math.sin(dLng / 2) ** 2;
    return R * 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
}

function setOrderLinks(store) {
    if (!store) {
        return;
    }

    const navLabel = document.getElementById('orderNowStoreLabel');
    const navUber = document.getElementById('orderNowUberLink');
    const navMrD = document.getElementById('orderNowMrDLink');

    if (navLabel && navUber && navMrD) {
        navLabel.textContent = 'Nearest: ' + store.name;
        navUber.href = store.uber;
        navMrD.href = store.mrd;
    }

    const deliveryLabel = document.getElementById('deliveryStoreLabel');
    const deliveryUber = document.getElementById('deliveryUberLink');
    const deliveryMrD = document.getElementById('deliveryMrDLink');

    if (deliveryUber && deliveryMrD) {
        deliveryUber.href = store.uber;
        deliveryMrD.href = store.mrd;
    }

    if (deliveryLabel) {
        deliveryLabel.textContent = 'Get It Delivered — Nearest: ' + store.name;
    }
}

document.addEventListener('DOMContentLoaded', function () {
    const stores = window.NOOSH_STORES || [];

    if (stores.length === 0) {
        return;
    }

    setOrderLinks(stores[0]);

    if (!navigator.geolocation) {
        return;
    }

    navigator.geolocation.getCurrentPosition(function (position) {
        const userLat = position.coords.latitude;
        const userLng = position.coords.longitude;

        let nearest = stores[0];
        let nearestDistance = Infinity;

        stores.forEach(function (store) {
            const distance = haversineDistanceKm(userLat, userLng, store.lat, store.lng);
            if (distance < nearestDistance) {
                nearestDistance = distance;
                nearest = store;
            }
        });

        setOrderLinks(nearest);
    }, function () {
        // Permission denied or unavailable — keep the default (first store).
    }, { timeout: 5000 });
});