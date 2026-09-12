let mapInstance;

window.mapInterop = {

    initializeMap: function (elementId, lat, lng, zoomLevel, locationName) {

        if (mapInstance) {
            mapInstance.remove();
        }

        mapInstance = L.map(elementId)
            .setView([lat, lng], zoomLevel);

        setTimeout(() => {
            mapInstance.invalidateSize();
        }, 300);

        setTimeout(() => {
            mapInstance.invalidateSize();
        }, 1000);

        L.tileLayer(
            'https://tile.openstreetmap.org/{z}/{x}/{y}.png',
            {
                maxZoom: 19,
                attribution: '© OpenStreetMap contributors'
            }).addTo(mapInstance);

        L.marker([lat, lng])
            .addTo(mapInstance)
            .bindPopup(locationName)
            .openPopup();

        requestAnimationFrame(() => {
            mapInstance.invalidateSize();
        });
    },

    panToLocation: function (lat, lng) {

        if (mapInstance) {

            mapInstance.panTo(
                new L.LatLng(lat, lng)
            );
        }
    }
};