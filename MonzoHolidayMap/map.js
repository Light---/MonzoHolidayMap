// Initialize and add the map
function initMap() {
    // The location of Uluru

    var locations = [
        { lat: -25.344, lng: 131.036 },
        { lat: 54.5973, lng: 5.9301 },
        { lat: 40.7128, lng: 74.0060 }
    ]

    // The map, centered at Uluru
    var map = new google.maps.Map(
        document.getElementById('map'), { zoom: 4, center: locations[0] });

    // The marker, positioned at Uluru

    for (i = 0; i < locations.length; i++) {
        var marker = new google.maps.Marker({ position: locations[i], map: map });
    }
    
}