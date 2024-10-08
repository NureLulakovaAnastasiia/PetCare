window.sessionStorageSetItem = function (key, value) {
    sessionStorage.setItem(key, value);
};

window.sessionStorageGetItem = function (key) {
    return sessionStorage.getItem(key);
};

window.sessionStorageRemoveItem = function (key) {
    sessionStorage.removeItem(key);
};

window.focusNextInput = function (elementId) {
    const element = document.getElementById(elementId);
    if (element) {
        element.focus();
    }
};