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
window.showToastr = function (type, message) {
    if (type === "success") {
        toastr.success(message);
    } else if (type === "error") {
        toastr.error(message);
    } else if (type === "warning") {
        toastr.warning(message);
    } else if (type === "info") {
        toastr.info(message);
    }
};


window.hideDeleteButton = function () {
    setTimeout(() => {
        let deleteButton = document.querySelector('.e-event-delete');
        if (deleteButton) {
            deleteButton.style.display = 'none';
        }
    }, 100);
};

window.showDeleteButton = function () {
    setTimeout(() => {
        let deleteButton = document.querySelector('.e-event-delete');
        if (deleteButton) {
            deleteButton.style.display = 'block';
        }
    }, 100);
};

