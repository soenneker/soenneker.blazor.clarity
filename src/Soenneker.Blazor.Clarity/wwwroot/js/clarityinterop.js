export function init(key) {
    (function (c, l, a, r, i, t, y) {
        c[a] = c[a] || function () { (c[a].q = c[a].q || []).push(arguments) };
        t = l.createElement(r);
        t.async = 1;
        t.src = "https://www.clarity.ms/tag/" + i;
        y = l.getElementsByTagName(r)[0];
        y.parentNode.insertBefore(t, y);
    })(window, document, "clarity", "script", key);
}

export function consent() {
    window.clarity("consent");
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
