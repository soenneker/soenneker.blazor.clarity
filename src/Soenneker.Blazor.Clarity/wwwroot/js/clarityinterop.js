export function init(key) {
    if (document.querySelector('script[src^="https://www.clarity.ms/tag/"]')) {
        return;
    }

    (function (c, l, a, r, i, t, y) {
        c[a] = c[a] || function () { (c[a].q = c[a].q || []).push(arguments) };
        t = l.createElement(r);
        t.async = 1;
        t.src = "https://www.clarity.ms/tag/" + encodeURIComponent(i);
        y = l.getElementsByTagName(r)[0];
        y.parentNode.insertBefore(t, y);
    })(window, document, "clarity", "script", key);
}

export function consent(adStorage, analyticsStorage) {
    window.clarity("consentv2", {
        ad_Storage: adStorage ? "granted" : "denied",
        analytics_Storage: analyticsStorage ? "granted" : "denied"
    });
}

export function identify(id, sessionId, pageId, friendlyName) {
    window.clarity("identify", id, sessionId || undefined, pageId || undefined, friendlyName || undefined);
}

export function setTag(key, value) {
    window.clarity("set", key, value);
}

export function trackEvent(name) {
    window.clarity("event", name);
}
