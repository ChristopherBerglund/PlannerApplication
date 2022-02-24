//call geocode
//geocode();

//Get location form
var locationForm = document.getElementById('location-form');

//Listen for submit
locationForm.addEventListener('submit', geocode);

function geocode(e) {
    // Prevent actual submit
    e.preventDefault();
    var location = document.getElementById('location-input').value;
    axios.get('https://maps.googleapis.com/maps/api/geocode/json', {
        params: {
            address: location,
            key: 'AIzaSyD0ZXdv31nRynoPkkDjq8iLP6DzWSn-0q8'
        }
    })
        .then(function (response) {
            // Log full respone
            console.log(response);

            //Formatted Address
            var formattedAddress = response.data.results[0].formatted_address;
            var formattedAddressOutput = `
    <ul class="list-group">
        <li class="list-group-item">${formattedAddress}</li>
    </ul>
    `;

            // Address Components
            var addressComponents = response.data.results[0].address_components;
            var addressComponentsOutput = '<ul class="list-group">';
            for (var i = 0; i < addressComponents.length; i++) {
                addressComponentsOutput += `
        <li class="list-group-item"><strong>${addressComponents[i].types[0]
                    }</strong>: ${addressComponents[i].long_name}</li>
        `;

            }
     /*       addressComponentsOutput += '</ul>';*/
            // geometry
            var lat = response.data.results[0].geometry.location.lat;
            var lng = response.data.results[0].geometry.location.lng;
        //    var geometryOutput = `
        //<ul class="list-group">
        //    <li class="list-group-item"><strong>Latitude</strong>:
        //    ${lat}</li>
        //    <li class="list-group-item"><strong>Longitude</strong>:
        //    ${lng}</li>
        //</ul>
        //`;

            console.log(lat);
            console.log(lng);
            //console.log(formattedAddress);

      /*      initMap(lat, lng);*/

            // Output to app
            //document.getElementById('formatted-address').innerHTML =
            //    formattedAddressOutput;

            //document.getElementById('address-components').innerHTML =
            //    addressComponentsOutput;

            //document.getElementById('geometry').innerHTML =
            //    geometryOutput;

            //document.getElementById('latitude').value = product(lat);
            //document.getElementById('longtitude').value = product(lng);
            //document.getElementById('address').value = product(formattedAddress);

            setInputValue('latitude', lat);
            setInputValue('longtitude', lng);
            setInputValue('streetAddress', formattedAddress);

            //setInputValue('address', formattedAddress);

        })
        .catch(function (error) {
            console.log(error);
        });
}



//Hämtar karta
//function initMap(Latitude, Longitude) {
//    // Map options
//    var options = {
//        zoom: 16,
//        center: { lat: Latitude, lng: Longitude }
//    }
//    // New map
//    var map = new google.maps.Map(document.getElementById('map'), options);
//    // Add marker
//    var marker = new google.maps.Marker({
//        position: { lat: Latitude, lng: Longitude },
//        map: map
//    });
//}



function setInputValue(input_id, val) {
    document.getElementById(input_id).setAttribute('value', val);
}