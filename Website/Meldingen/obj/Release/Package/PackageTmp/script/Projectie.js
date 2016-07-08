Proj4js.defs["EPSG:28992"] = "+proj=sterea +lat_0=52.15616055555555 +lon_0=5.38763888888889 +k=0.9999079 +x_0=155000 +y_0=463000 +ellps=bessel          +towgs84=565.417,50.3319,465.552,-0.398957,0.343988,-1.8774,4.0725 +units=m +no_def"

var projectionWgs84 = new Proj4js.Proj("EPGS:4326");
var projectionRdNew = new Proj4js.Proj("EPSG:28992");

/*
var sourceProjection = new Proj4js.Proj("EPGS:4326");
var destProjection = new Proj4js.Proj("EPSG:28992");

function toRD(latLng) {
  var point = new Proj4js.Point(latLng.lng(), latLng.lat());
  return Proj4js.transform(sourceProjection, destProjection, point);
}

function toRD(lat, lng) {
  var point = new Proj4js.Point(lng, lat);
  return Proj4js.transform(sourceProjection, destProjection, point);
}
*/

function wgs84ToRDNew(lat, lng) {
  var point = new Proj4js.Point(lng, lat);
  return Proj4js.transform(projectionWgs84, projectionRdNew, point);
}
