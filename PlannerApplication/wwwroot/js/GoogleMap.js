function initMap(latitude, longtitude) {
    // Map options
    console.log("InitMap");

    var options = {
        zoom: 16,
        center: { lat: latitude, lng: longtitude }
    }
    // New map
    var map = new google.maps.Map(document.getElementById('map'), options);
    // Add marker
    var marker = new google.maps.Marker({
        position: { lat: latitude, lng: longtitude },
        map: map
    });
}