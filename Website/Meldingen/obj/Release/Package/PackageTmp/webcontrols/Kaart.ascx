<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Kaart.ascx.cs" Inherits="Meldingen.webcontrols.Kaart" %>
<script src="openlayers/OpenLayers.js" type="text/javascript"></script>
<script src="script/proj4js-combined.js" type="text/javascript"></script>
<script src="script/Projectie.js" type="text/javascript"></script>
<script src="script/Kaart.js" type="text/javascript"></script>
<div id="kaartDiv">
</div>
<script type="text/javascript">
  $(document).ready(function () {
    toonKaart('kaartDiv');
  });
</script>
