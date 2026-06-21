// PARA USO COM O GOOGLE MAPS SOMENTE

var map;
var geocoder;
var markers = [];
var RJ_LAT = -22.25;
var RJ_LONG = -42.5;

function initMap() {
    geocoder = new google.maps.Geocoder();

    var mapOptions = {
        center: new google.maps.LatLng(RJ_LAT, RJ_LONG),
        zoom: 8,
        disableDefaultUI: true
    };

    map = new google.maps.Map(document.getElementById('map_canvas'), mapOptions);
}

function getLatLng(location, callback) {
    var coordsObj = {
        lat: 0,
        lng: 0
    };
    
    if (location != undefined) {
        geocoder.geocode(
            { 'address': location },
            function(results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    addMarker(results[0].geometry.location);

                    coordsObj.lat = results[0].geometry.location.lat().toFixed(6);
                    coordsObj.lng = results[0].geometry.location.lng().toFixed(6);

                    callback(coordsObj);
                }
                else {
                    alert('Endereço não encontrado no mapa.');
                }
            }
        );
    }
    else {
        alert('Endereço inválido');
    }
}

// Add a marker to the map and push to the array.
function addMarker(position) {
    map.setCenter(position);
    map.setZoom(16);    
    
    var marker = new google.maps.Marker({
        position: position,
        animation: google.maps.Animation.DROP,
        icon: {
            url: '../img/ico_mark.png',
            // porque a imagem é de 24x36, distanciando em 4 px no eixo y.
            anchor: new google.maps.Point(0, 40)
        }
    });

    deleteMarkers();
    markers[markers.length] = marker;
    setAllMap(map);
}

function setAllMap(map) {
    for (i = 0; i < markers.length; i++)
        markers[i].setMap(map);
}

function deleteMarkers() {
    setAllMap(null);
    markers = [];
}

function getFormatAddress(address, number, compl, district, town, state) {
    var addr = [];
    var args = [].slice.call(arguments);

    addr[addr.length] = address + ', ' + number;

    for (var i = 2; i < args.length; i++) {
        if (args[i].length > 0) {
            addr[addr.length] = args[i];
        }
    }

    return addr.join(' - ');
}

function onCenterChanged() {
    window.setTimeout(function() {
        if (map) {
            map.panTo(markers[markers.length].getPosition());
        }
    }, 2000);
}

function onClick() {
    if (map) {
        map.setZoom(8);
        map.setCenter(markers[markers.length].getPosition());
    }
}

google.maps.event.addDomListener(window, 'load', initMap);
google.maps.event.addListener(map, 'center_changed', onCenterChanged);
google.maps.event.addListener(marker, 'click', onClick);
