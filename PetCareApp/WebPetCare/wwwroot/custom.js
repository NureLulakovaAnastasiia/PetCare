window.localStorageSetItem = function (key, value) {
    localStorage.setItem(key, value);
};

window.localStorageGetItem = function (key) {
    return localStorage.getItem(key);
};

window.localStorageRemoveItem = function (key) {
    localStorage.removeItem(key);
};
