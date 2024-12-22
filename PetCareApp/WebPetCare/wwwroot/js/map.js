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
