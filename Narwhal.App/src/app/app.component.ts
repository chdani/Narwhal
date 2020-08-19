import { Component } from '@angular/core';

import * as mapboxgl from 'mapbox-gl';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss']
})

export class AppComponent {

    map: mapboxgl.Map;
    hardwareRendering = mapboxgl.supported({ failIfMajorPerformanceCaveat: true });

    ngOnInit() {

        let mapboxOptions : mapboxgl.MapboxOptions = {
            container: 'map',
            style: {
                version: 8,
                sources: {
                    'osm-source': { type: 'raster', tiles: ['https://a.tile.openstreetmap.org/{z}/{x}/{y}.png'], tileSize: 256 }
                },
                layers: [
                    {
                        'id': 'osm-layer',
                        'type': 'raster',
                        'source': 'osm-source',
                        'minzoom': 0,
                        'maxzoom': 22
                    }
                ],
            },

            attributionControl: false,
            antialias: this.hardwareRendering
        };

        mapboxOptions.center = [ 0, 30 ];
        mapboxOptions.zoom = 2;
        
        // *******************************************************************
        // Mapbox GL initialization
        // *******************************************************************

        this.map = new mapboxgl.Map(mapboxOptions);
        this.map.addControl(new mapboxgl.NavigationControl());
        
        this.map.on('mousemove', (e: mapboxgl.MapMouseEvent) => {
            const level = Math.floor(e.target.getZoom());
            const divider = Math.pow(2, level);

            const resultX = (e.lngLat.lng + 180) / (360 / divider);
            const resultY = (1 - Math.log(Math.tan(e.lngLat.lat * Math.PI / 180) + 1 / Math.cos(e.lngLat.lat * Math.PI / 180)) / Math.PI) / 2 * divider;
            const resultScale = 500000000 / Math.pow(2, level + 1);

            document.getElementById('info').innerHTML =
                JSON.stringify({ lat: Math.round(e.lngLat.lat * 10000) / 10000, lon: Math.round(e.lngLat.lng * 10000) / 10000 }) + '\n' +
                JSON.stringify({ z: level, x: Math.floor(resultX), y: Math.floor(resultY), scale: Math.floor(resultScale) });
        });

        // *******************************************************************
        // WebGL support
        // *******************************************************************

        const banner = document.getElementsByClassName('banner-webgl')[0];

        if (mapboxgl.supported({ failIfMajorPerformanceCaveat: true })) {
            banner.getElementsByClassName('status')[0].innerHTML = 'WebGL GPU';
            banner.className = 'banner-webgl valid';
        } else if (mapboxgl.supported({ failIfMajorPerformanceCaveat: false })) {
            banner.getElementsByClassName('status')[0].innerHTML = 'WebGL CPU';
            banner.className = 'banner-webgl warning';
        } else {
            banner.getElementsByClassName('status')[0].innerHTML = 'WebGL not supported';
            banner.className = 'banner-webgl danger';
        }

        // *******************************************************************
        // NavWarnings source
        // *******************************************************************

        this.map.on('load', function() {

            this.addSource('navwarnings-source', {
                'type': 'geojson',
                'data': {
                    'type': 'FeatureCollection',
                    'features': []
                }
            });

            this.addLayer({
                'id': 'navwanings-layer',
                'type': 'circle',
                'source': 'navwarnings-source',
                'paint': {
                    'circle-radius': 6,
                    'circle-color': '#007cbf'
                }
            });

            fetch('/api/navwarnings/get')
                .then(response => response.json())
                .then(data => {
                    data = data.map(d => d.data);

                    var geoJson = {
                        'type': 'FeatureCollection',
                        'features': data
                    };

                    this.getSource('navwarnings-source').setData(geoJson);
                });
        });
    }
}
