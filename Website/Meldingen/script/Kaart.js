var map;

function toonKaart(divName) {
    map = new OpenLayers.Map(divName, {
        projection: new OpenLayers.Projection("EPSG:28992"),
        displayProjection: new OpenLayers.Projection("EPSG:28992"),
        units: "m",
        maxExtent: maxExtentMeldingen(),
        eventListeners: { zoomend: function () { mapZoomEnd(); } }
    });
    map.addLayer(getLayerBRT());
    map.addLayer(getLayerLuchtfoto());
    map.addLayer(getLayerWegenbestand());
    map.addLayer(getLayerVaarWegenbestand());
    map.addLayer(getLayerPercelen());
    addOrReplaceLayerMeldingen();

    addLayerSwitcher();
    map.addControl(new OpenLayers.Control.MousePosition());
    map.addControl(new OpenLayers.Control.Navigation());

    map.zoomToMaxExtent = function () { map.zoomToExtent(boundsGroningen()); };

    var meldingId = getParameterByName("meldingId");
    if ((meldingId != null) && (meldingId != "")) {
        layerMeldingen.events.register("loadend", null, function () {
            zoomToMelding(meldingId);
            layerMeldingen.events.unregister("loadend");
        });
    } else {
        map.zoomToExtent(boundsGroningen());
    }
}

var layerMeldingen;
function addOrReplaceLayerMeldingen() {
    if (layerMeldingen != null) map.removeLayer(layerMeldingen);
    layerMeldingen = getLayerMeldingen();
    styleLayerMeldingen();
    map.addLayer(layerMeldingen);
    addLayerMeldingenInteraction();
}

function addLayerSwitcher() {
    var layerSwitcher = new OpenLayers.Control.LayerSwitcher();
    map.addControl(layerSwitcher);
    layerSwitcher.baseLbl.innerHTML = "Basislagen";
    layerSwitcher.dataLbl.innerHTML = "Themalagen"
}

function getLayerPercelen() {
    return new OpenLayers.Layer.WMS("Perceelsgrenzen", "http://o-geoportaal/ArcGIS/services/Basisregistraties/BRK/MapServer/WMSServer?service=WMS&version=1.1.0",
      {
          layers: '1', //Percelen_Provincie
          format: 'image/png',
          transparent: true,
          projection: "EPSG:28992"
      },
      { isBaseLayer: false,
          visibility: false,
          style: "Percelen_Provincie",
          transitionEffect: "resize"
      });

}

//doetniet  http://geodata.nationaalgeoregister.nl/nwbwegen/ows?LAYERS=hectopunten&FORMAT=image%2Fpng&TRANSPARENT=TRUE&SERVICE=WMS&VERSION=1.1.1&REQUEST=GetMap&STYLES=&SRS=EPSG%3A28992&BBOX=200000,575050.24,255050.24,630100.48&WIDTH=256&HEIGHT=256
//doetwel   http://geodata.nationaalgeoregister.nl/nwbwegen/ows?LAYERS=hectopunten&TRANSPARENT=TRUE&FORMAT=image%2Fpng&SERVICE=WMS&VERSION=1.1.1&REQUEST=GetMap&STYLES=&SRS=EPSG%3A28992&BBOX=218432.6,585389.68,222202.52,587824&WIDTH=1122&HEIGHT=724
function getLayerWegenbestand() {
    //sld = "<StyledLayerDescriptor xmlns:ogc=\"http://www.opengis.net/ogc\" ><NamedLayer><UserStyle><FeatureTypeStyle><Rule><PointSymbolizer><Graphic><Mark><WellKnownName>square</WellKnownName><Fill><CssParameter name=\"fill\">#FF0000</CssParameter></Fill></Mark><Size>6</Size></Graphic></PointSymbolizer><TextSymbolizer><Label><ogc:PropertyName>hectomtrng</ogc:PropertyName></Label><Fill><CssParameter name=\"fill\">#0000FF</CssParameter></Fill></TextSymbolizer></Rule></FeatureTypeStyle></UserStyle></NamedLayer></StyledLayerDescriptor>";
    return new OpenLayers.Layer.WMS("Wegen: hectopunten", "http://geodata.nationaalgeoregister.nl/nwbwegen/ows?",
      {
          layers: 'nwbwegen:hectopunten',
          format: 'image/png',
          transparent: true,
          style: 'hectopunten'
      },
      { isBaseLayer: false,
          visibility: false
      });
}

function getLayerVaarWegenbestand() {
    return new OpenLayers.Layer.WMS("Vaarwegen: km markeringen", "http://geodata.nationaalgeoregister.nl/nwbvaarwegen/wms?",
      {
          layers: 'nwbvaarwegen:kmmarkeringen',
          format: 'image/png',
          transparent: true,
          projection: "EPSG:28992"
      },
      { isBaseLayer: false,
          visibility: false,
          transitionEffect: "resize"
      });
}

function getLayerLuchtfoto() {
    return new OpenLayers.Layer.WMS("Luchtfoto", "http://luchtfoto.services.gbo-provincies.nl/mapserv.cgi?map=luchtfoto.map",
      {
          layers: 'ipo_luchtfoto_actueel',
          format: 'image/png',
          alpha: 40,
          units: "m",
          srs: "EPSG:28992"
      },
      { isBaseLayer: false,
          visibility: false
      });
}


function getBrtMatrixIds() {
    var brtMatrixIds = new Array(14);
    for (var i = 0; i < 14; ++i) {
        brtMatrixIds[i] = "EPSG:28992:" + i;
    }
    return brtMatrixIds;
}

function getLayerBRT() {
    var brtResolutions = [3440.64, 1720.32, 860.16, 430.08, 215.04, 107.52, 53.76, 26.88, 13.44, 6.72, 3.36, 1.68, 0.84, 0.42, 0.21];
    return new OpenLayers.Layer.TMS(
        "Topografie",
        "http://geodata.nationaalgeoregister.nl/tiles/service/tms/",
        {
            layername: "brtachtergrondkaart",
            type: "png8",
            projection: "EPSG:28992",
            resolutions: brtResolutions,
            serverResolutions: brtResolutions,
            maxExtent: new OpenLayers.Bounds(-285401.92, 22598.08, 595401.9199999999, 903401.9199999999),
            tileOrigin: new OpenLayers.LonLat(-285401.92, 22598.08)
        },
      { isBaseLayer: true,
          visibility: true
      });
}

function getLayerBRT2() {
    return new OpenLayers.Layer.WMS(
      {
          name: "Topografie",
          url: "http://geodata.nationaalgeoregister.nl/wmsc?tiled=true",
          layer: "bgt",
          format: "image/png8"
      },
      { isBaseLayer: true,
          visibility: true
      });
}

function maxExtentMeldingen() {
    return new OpenLayers.Bounds(202911, 540354, 278026, 642000)
}

function getLayerMeldingen() {
    return new OpenLayers.Layer.Vector("Meldingen", {
        projection: "EPSG:4326",
        displayInLayerSwitcher: false,
        strategies: [new OpenLayers.Strategy.Fixed()],
        maxExtent: maxExtentMeldingen(),
        protocol: new OpenLayers.Protocol.HTTP({
            url: kmlUrl(),
            format: new OpenLayers.Format.KML({
                extractStyles: true,
                extractAttributes: true,
                maxDepth: 2
            })
        })
    });
}

function mapZoomEnd() {
    styleLayerMeldingen();
}

function styleLayerMeldingen() {
    if (map.zoom >= 7) {
        layerMeldingen.styleMap = new OpenLayers.StyleMap({
            'default': {
                strokeColor: "#00FF00",
                strokeOpacity: 1,
                strokeWidth: 3,
                fillColor: "#FF5500",
                fillOpacity: 0,
                pointRadius: 6,
                label: "${name}",
                fontColor: "maroon",
                fontSize: "12px",
                fontFamily: "Corbel",
                fontWeight: "bold",
                labelAlign: "lm",
                labelXOffset: "10",
                labelYOffset: "0",
                labelOutlineColor: "white",
                labelOutlineWidth: 10
            }
        });
    } else {
        layerMeldingen.styleMap = new OpenLayers.StyleMap({
            'default': {
                strokeColor: "#00FF00",
                strokeOpacity: 0.6,
                strokeWidth: 2,
                fillColor: "#FF5500",
                fillOpacity: 0.6,
                pointRadius: 4
            }
        });
    }
    layerMeldingen.redraw();
}

function boundsGroningen() {
    return new OpenLayers.Bounds(202911.781, 540354.187, 278026.094, 609712.25)
}

function kmlUrl() {
    var queryMeldingId = getParameterByName("meldingId");
    if (queryMeldingId != null) {
        queryMeldingId = "&MeldingId=" + encodeURI(queryMeldingId)
    } else {
        queryMeldingId = "";
    }

    return "GetKml.ashx"
          + "?Onderwerp=" + encodeURI($("#FilterOnderwerp").val())
          + "&Status=" + encodeURI($("#FilterStatus").val())
          + "&Opmerking=" + encodeURI($("#FilterOpmerking").val())
          + "&Afzender=" + encodeURI($("#FilterAfzender").val())
          + "&ToegewezenAan=" + encodeURI($("#FilterToegewezenAan").val())
          + queryMeldingId;
}

function wijzigFilter() {
    addOrReplaceLayerMeldingen();
}

var meldingSelector;
var modifyFeatureWijzigMeldingCoordinaat;
function addLayerMeldingenInteraction() {
    meldingSelector = new OpenLayers.Control.SelectFeature(layerMeldingen);
    map.addControl(meldingSelector);
    meldingSelector.activate();

    modifyFeatureWijzigMeldingCoordinaat = new OpenLayers.Control.ModifyFeature(layerMeldingen, {
        mode: OpenLayers.Control.ModifyFeature.DRAG
    });
    map.addControl(modifyFeatureWijzigMeldingCoordinaat);
    modifyFeatureWijzigMeldingCoordinaat.activate();

    layerMeldingen.events.on({
        'featureselected': meldingSelected
   , 'featureunselected': meldingUnSelected
   , 'afterfeaturemodified': eindeWijzigenMeldingCoordinaat
   , 'featuremodified': eindeWijzigenMeldingCoordinaat
    });
}

function eindeWijzigenMeldingCoordinaat(featureObject) {
    var feature = featureObject.feature;
    var geom = feature.geometry.transform(map.projection, layerMeldingen.projection);
    wijzigCoordinaat(feature.fid, geom.y, geom.x);
    return true;
}

function meldingSelected(eventObject) {
    selectMelding(eventObject.feature.fid);
}

function meldingUnSelected(eventObject) {
    selectMelding("");
    wijzigFilter();
}

function selectMelding(id) {
    $("#TextBoxIdMelding").val(id);
    $("#ButtonMeldingSelected").click();
}

function toonAfbeelding(id) {
    window.open("GetPrimaireFoto.ashx?Size=Normal&meldingId=" + id, "Foto");
}

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.search);
    if (results == null)
        return "";
    else
        return decodeURIComponent(results[1].replace(/\+/g, " "));
}

function zoomToMelding(id) {
    var point = layerMeldingen.getFeatureByFid(id).geometry;
    map.moveTo(new OpenLayers.LonLat(point.x, point.y), 8);
    meldingSelector.select(layerMeldingen.getFeatureByFid(id));
    //selectMelding(id);
}

var timeout;
function ajaxWijzigFilter() {
    clearTimeout(timeout);
    $("#filterStatusDiv").text("filter aangepast");
    timeout = setTimeout(function () {
        wijzigFilter();
        $("#filterStatusDiv").text("");
    }, 500);
}

function resetFilter() {
    $("#FilterOnderwerp").val("");
    $("#FilterAfzender").val("");
    $("#FilterOpmerking").val("");
    $("#FilterStatus").val("Open");
    wijzigFilter();
}