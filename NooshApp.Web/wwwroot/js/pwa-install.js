// Handles the custom "Install App" button, hooking into the browser's
// native install capability rather than replacing it.

let deferredInstallPrompt = null;

window.addEventListener('beforeinstallprompt', function (event) {
    // Prevent the browser's default mini-infobar from showing immediately.
    event.preventDefault();

    // Save the event so we can trigger it later, from our own button click.
    deferredInstallPrompt = event;

    const installBtn = document.getElementById('installAppBtn');
    if (installBtn) {
        installBtn.classList.remove('d-none');
    }
});

document.addEventListener('DOMContentLoaded', function () {
    const installBtn = document.getElementById('installAppBtn');

    if (installBtn) {
        installBtn.addEventListener('click', async function () {
            if (!deferredInstallPrompt) {
                return;
            }

            // Show the browser's real install dialog.
            deferredInstallPrompt.prompt();

            const choice = await deferredInstallPrompt.userChoice;

            if (choice.outcome === 'accepted') {
                installBtn.classList.add('d-none');
            }

            deferredInstallPrompt = null;

            console.log('PWA install choice:', choice.outcome);
        });
    }
});

// Once installed, hide the button permanently for that session (the app
// is now running standalone, so there's nothing left to install).
window.addEventListener('appinstalled', function () {
    const installBtn = document.getElementById('installAppBtn');
    if (installBtn) {
        installBtn.classList.add('d-none');
    }
    deferredInstallPrompt = null;
});