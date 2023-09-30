/**
 * jQuery gMap
 *
 * @url		http://gmap.nurtext.de/
 * @author	Cedric Kastner <cedric@nur-text.de>
 * @version	1.1.0
 */
(function ($) {
    $.fn.gMap = function (d) {
        if (!window.GBrowserIsCompatible || !GBrowserIsCompatible()) {
            return this
        }
        var e = $.extend({}, $.fn.gMap.defaults, d);
        return this.each(function () {
            $gmap = new GMap2(this);
            $geocoder = new GClientGeocoder();
            if (e.address) {
                $geocoder.getLatLng(e.address, function (a) {
                    $gmap.setCenter(a, e.zoom)
                })
            } else {
                if (e.latitude && e.longitude) {
                    $gmap.setCenter(new GLatLng(e.latitude, e.longitude), e.zoom)
                } else {
                    if ($.isArray(e.markers) && e.markers.length > 0) {
                        if (e.markers[0].address) {
                            $geocoder.getLatLng(e.markers[0].address, function (a) {
                                $gmap.setCenter(a, e.zoom)
                            })
                        } else {
                            $gmap.setCenter(new GLatLng(e.markers[0].latitude, e.markers[0].longitude), e.zoom)
                        }
                    } else {
                        $gmap.setCenter(new GLatLng(34.885931, 9.84375), e.zoom)
                    }
                }
            }
            $gmap.setMapType(e.maptype);
            if (e.controls.length == 0) {
                $gmap.setUIToDefault()
            } else {
                for (var i = 0; i < e.controls.length; i++) {
                    eval("$gmap.addControl(new " + e.controls[i] + "());")
                }
            }
            if (e.scrollwheel == true && e.controls.length != 0) {
                $gmap.enableScrollWheelZoom()
            }
            for (var j = 0; j < e.markers.length; j++) {
                marker = e.markers[j];
                gicon = new GIcon();
                gicon.image = e.icon.image;
                gicon.shadow = e.icon.shadow;
                gicon.iconSize = ($.isArray(e.icon.iconsize)) ? new GSize(e.icon.iconsize[0], e.icon.iconsize[1]) : e.icon.iconsize;
                gicon.shadowSize = ($.isArray(e.icon.shadowsize)) ? new GSize(e.icon.shadowsize[0], e.icon.shadowsize[1]) : e.icon.shadowsize;
                gicon.iconAnchor = ($.isArray(e.icon.iconanchor)) ? new GPoint(e.icon.iconanchor[0], e.icon.iconanchor[1]) : e.icon.iconanchor;
                gicon.infoWindowAnchor = ($.isArray(e.icon.infowindowanchor)) ? new GPoint(e.icon.infowindowanchor[0], e.icon.infowindowanchor[1]) : e.icon.infowindowanchor;
                if (marker.icon) {
                    gicon.image = marker.icon.image;
                    gicon.shadow = marker.icon.shadow;
                    gicon.iconSize = ($.isArray(marker.icon.iconsize)) ? new GSize(marker.icon.iconsize[0], marker.icon.iconsize[1]) : marker.icon.iconsize;
                    gicon.shadowSize = ($.isArray(marker.icon.shadowsize)) ? new GSize(marker.icon.shadowsize[0], marker.icon.shadowsize[1]) : marker.icon.shadowsize;
                    gicon.iconAnchor = ($.isArray(marker.icon.iconanchor)) ? new GPoint(marker.icon.iconanchor[0], marker.icon.iconanchor[1]) : marker.icon.iconanchor;
                    gicon.infoWindowAnchor = ($.isArray(marker.icon.infowindowanchor)) ? new GPoint(marker.icon.infowindowanchor[0], marker.icon.infowindowanchor[1]) : marker.icon.infowindowanchor
                }
                if (marker.address) {
                    if (marker.html == "_address") {
                        marker.html = marker.address
                    }
                    $geocoder.getLatLng(marker.address, function (b, c) {
                        return function (a) {
                            gmarker = new GMarker(a, b);
                            if (c.html) {
                                gmarker.bindInfoWindowHtml(e.html_prepend + c.html + e.html_append)
                            }
                            if (c.html && c.popup) {
                                gmarker.openInfoWindowHtml(e.html_prepend + c.html + e.html_append)
                            }
                            if (gmarker) {
                                $gmap.addOverlay(gmarker)
                            }
                        }
                    }(gicon, marker))
                } else {
                    if (marker.html == "_latlng") {
                        marker.html = marker.latitude + ", " + marker.longitude
                    }
                    gmarker = new GMarker(new GPoint(marker.longitude, marker.latitude), gicon);
                    if (marker.html) {
                        gmarker.bindInfoWindowHtml(e.html_prepend + marker.html + e.html_append)
                    }
                    if (marker.html && marker.popup) {
                        gmarker.openInfoWindowHtml(e.html_prepend + marker.html + e.html_append)
                    }
                    if (gmarker) {
                        $gmap.addOverlay(gmarker)
                    }
                }
            }
        })
    };
    $.fn.gMap.defaults = {
        address: "",
        latitude: 0,
        longitude: 0,
        zoom: 1,
        markers: [],
        controls: [],
        scrollwheel: true,
        maptype: G_NORMAL_MAP,
        html_prepend: '<div class="gmap_marker">',
        html_append: "</div>",
        icon: {
            image: "http://www.google.com/mapfiles/marker.png",
            shadow: "http://www.google.com/mapfiles/shadow50.png",
            iconsize: [20, 34],
            shadowsize: [37, 34],
            iconanchor: [9, 34],
            infowindowanchor: [9, 2]
        }
    }
})(jQuery);