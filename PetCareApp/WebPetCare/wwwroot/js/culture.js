window.blazorCulture = {
    get: () => ['BlazorCulture'],
    set: (value) => localStorage['BlazorCulture'] = value
};


function setCultureCookie(culture, cookieName) {
    var cookieValue = culture;
    document.cookie = cookieName + "=" + encodeURIComponent(cookieValue);
}
