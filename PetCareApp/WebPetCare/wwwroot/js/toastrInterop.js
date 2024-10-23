//window.toastrInterop = {
//    showSuccess: function (message, title) {
//        toastr.success(message, title);
//    },
//    showError: function (message, title) {
//        toastr.error(message, title);
//    },
//    showWarning: function (message, title) {
//        toastr.warning(message, title);
//    },
//    showInfo: function (message, title) {
//        toastr.info(message, title);
//    }
//};

window.toastrInterop = {
    showSuccess: function (message, title) {
        if (window.toastr) {
            toastr.success(message, title);
        } else {
            console.error("Toastr is not defined");
        }
    },
    showError: function (message, title) {
        if (window.toastr) {
            toastr.error(message, title);
        } else {
            console.error("Toastr is not defined");
        }
    }
};