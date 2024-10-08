window.sessionStorageSetItem = function (key, value) {
    sessionStorage.setItem(key, value);
};

window.sessionStorageGetItem = function (key) {
    return sessionStorage.getItem(key);
};

window.sessionStorageRemoveItem = function (key) {
    sessionStorage.removeItem(key);
};