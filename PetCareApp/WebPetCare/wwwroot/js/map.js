let map;
let markers = [];
let mapInterop = null;

function registerMapInterop(dotNetObject) {
    mapInterop = dotNetObject;
}
function initMap(mapId, markerData) {
    map = new google.maps.Map(document.getElementById(mapId), {
        center: { lat: markerData[0].latitude, lng: markerData[0].longitude },
        zoom: 12,
    });

    markers = markerData.map((marker, index) => {
        const mapMarker = new google.maps.Marker({
            position: { lat: marker.latitude, lng: marker.longitude },
            map: map,
            title: marker.name,
            id: marker.id,
        });

        mapMarker.addListener("click", () => {
            selectMarker(mapMarker.id);
        });

        return mapMarker;
    });
}

function selectMarker(markerId) {
    markers.forEach(marker => {
        marker.setIcon(null); 
    });

    const selectedMarker = markers.find(marker => marker.id === markerId);
    if (selectedMarker) {
        selectedMarker.setIcon({
            url: "http://maps.google.com/mapfiles/ms/icons/red-dot.png",
            scaledSize: new google.maps.Size(50, 50), // Enlarged icon
        });

        if (mapInterop) {
            console.log("Attempting to call .NET method...");
            mapInterop.invokeMethodAsync("MarkerSelected", markerId)
                .then(() => console.log("Successfully called .NET method"))
                .catch(error => console.error("Error calling .NET method:", error));
        } else {
            console.error("DotNetObject is not registered.");
        }
    }
}

let marker, searchBox, geocoder;

window.initEditMap = () => {
    const mapOptions = {
        center: { lat: 50.453186751694375, lng: 30.528526928029315 }, //Kyiv
        zoom: 13,
    };
    map = new google.maps.Map(document.getElementById("map"), mapOptions);

    const input = document.getElementById("searchBox");
    searchBox = new google.maps.places.SearchBox(input);
    map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);

    geocoder = new google.maps.Geocoder();

    marker = new google.maps.Marker({
        map: map,
        draggable: true,
    });

    searchBox.addListener("places_changed", () => {
        const places = searchBox.getPlaces();
        if (places.length === 0) return;

        const place = places[0];
        if (!place.geometry || !place.geometry.location) return;

        map.setCenter(place.geometry.location);
        map.setZoom(15);

        marker.setPosition(place.geometry.location);
        marker.setVisible(true);
    });

    marker.addListener("dragend", () => {
        const position = marker.getPosition();
        map.setCenter(position);
    });
};

window.getLocationDetails = async () => {
    const position = marker.getPosition();
    if (!position) return null;

    const lat = position.lat();
    const lng = position.lng();

    const response = await geocoder.geocode({ location: { lat, lng } });
    if (response.results.length > 0) {
        const addressComponents = response.results[0].address_components;
        const cityComponent = addressComponents.find(c => c.types.includes("locality")) || {};
        const city = cityComponent.long_name || "";

        return {
            City: city,
            Address: response.results[0].formatted_address,
            Latitude: lat,
            Longitude: lng,
        };
    }

    return null;
};