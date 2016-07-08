using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Text;
using Common.Logging;

namespace Meldingen
{
    /// <summary>
    /// Summary description for GetKML
    /// </summary>
    public class GetKML : IHttpHandler
    {
        private static ILog log = LogManager.GetCurrentClassLogger();
        private static System.Globalization.CultureInfo KML_CULTURE = new System.Globalization.CultureInfo("en-US");

        private string FilterOnderwerp { get; set; }
        private string FilterStatus { get; set; }
        private string FilterAfzender { get; set; }
        private string FilterOpmerking { get; set; }
        private string FilterToegewezenAan { get; set; }

        private int FilterMeldingId { get; set; }

        public void ProcessRequest(HttpContext context)
        {
            log.Info(m => m("ProcessRequest(HttpContext context={0})", context));
            try
            {
                var response = context.Response;
                response.ContentType = "application/vnd.google-earth.kml+xml";
                response.AddHeader("Content-Disposition", "attachment; filename=Meldingen.kml");
                response.CacheControl = "No-Cache";
                XmlTextWriter writer = new XmlTextWriter(response.OutputStream, UTF8Encoding.Unicode);
                if (context.Request.QueryString.Count == 0)
                {
                    log.Trace(m => m("ProcessRequest : WriteKMLNetworkLink"));
                    WriteKMLNetworkLink(writer, context.Request.Url.GetLeftPart(UriPartial.Path));
                }
                else
                {
                    log.Trace(m => m("ProcessRequest : WriteKML"));
                    this.FilterOnderwerp = ParameterValue(context.Request, "Onderwerp", "");
                    this.FilterStatus = ParameterValue(context.Request, "Status", "Open");
                    this.FilterAfzender = ParameterValue(context.Request, "Afzender", "");
                    this.FilterOpmerking = ParameterValue(context.Request, "Opmerking", "");
                    this.FilterToegewezenAan = ParameterValue(context.Request, "ToegewezenAan", "");
                    int meldingId = int.MinValue;
                    if (int.TryParse(ParameterValue(context.Request, "MeldingId", "-1"), out meldingId))
                    {
                        this.FilterMeldingId = meldingId;
                    }
                    WriteKML(writer);
                }
                writer.Close();
            }
            catch (Exception ex)
            {
                log.Error(m => m("ProcessRequest(HttpContext context={0})", context), ex);
            }
        }

        private void WriteKMLNetworkLink(XmlTextWriter writer, string path)
        {
            writer.WriteStartElement("kml", "http://www.opengis.net/kml/2.2");
            writer.WriteStartElement("Document");
            writer.WriteElementString("name", "Meldingen");
            writer.WriteElementString("description", "Meldingen op de kaart");

            writer.WriteStartElement("NetworkLink");
            writer.WriteElementString("name", "Meldingen op de kaart");
            writer.WriteElementString("visibility", "1");
            writer.WriteElementString("flyToView", "1");

            writer.WriteStartElement("Link");
            writer.WriteAttributeString("id", "Meldingen");
            writer.WriteElementString("refreshMode", "onInterval");
            writer.WriteElementString("refreshInterval", "300");
            writer.WriteElementString("href", string.Format("{0}?Onderwerp=&Status=Open&Afzender=", path));
            writer.WriteEndElement(); // Link

            writer.WriteEndElement(); // NetworkLink

            writer.WriteEndElement(); // Document
        }

        private string ParameterValue(HttpRequest request, string name, string defaultValue)
        {
            string value = request.Params[name];
            return value == null ? defaultValue : value;
        }

        private void WriteKML(XmlTextWriter writer)
        {
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 3;

            WriteDocument(writer);
        }

        private void WriteDocument(XmlTextWriter writer)
        {
            writer.WriteStartDocument();

            writer.WriteStartElement("kml", "http://www.opengis.net/kml/2.2");
            writer.WriteStartElement("Document");
            writer.WriteElementString("name", "Meldingen");
            writer.WriteElementString("description", "Uitgebreide uitleg en eventuele beschrijving van het huidige filter");

            WritePoints(writer);

            writer.WriteEndElement(); // Document
            writer.WriteEndDocument(); // kml
        }

        private void WritePoints(XmlTextWriter writer)
        {
            using (Meldingen.DataClassesDataContext db = new DataClassesDataContext())
            {
                var mld = from m in db.Meldings
                          where (m.Id == FilterMeldingId)
                          || (m.Onderwerp.ToLower().Contains(this.FilterOnderwerp.ToLower())
                            && m.Status.Naam.Contains(this.FilterStatus)
                            && m.Melder.ToLower().Contains(this.FilterAfzender.ToLower())
                            && (/* er is geen opmerkingenfilter */ (this.FilterOpmerking.Equals(""))
                              /* of */ ||
                              /* er zijn opmerkingen die de zoektekst bevatten */ m.Opmerkings.Any(o => o.Tekst.ToLower().Contains(this.FilterOpmerking.ToLower())))
                            && (/* er is geen toegewezenAanfilter */ (this.FilterToegewezenAan.Equals(""))
                            /* of */ ||
                            /* er zijn opmerkingen die de zoektekst bevatten */ m.ToegewezenAan.ToLower().Contains(this.FilterToegewezenAan.ToLower()))
                            && m.Latitude != null
                            && m.Longitude != null)
                          select m;

                foreach (Meldingen.Melding melding in mld)
                {
                    writer.WriteStartElement("Placemark");
                    writer.WriteAttributeString("id", string.Format("{0:00000}", melding.Id));
                    writer.WriteElementString("name", string.Format("{0:00000}\n{1}", melding.Id, melding.Onderwerp));

                    writer.WriteStartElement("ExtendedData");
                    WriteExtendedData(writer, melding);
                    writer.WriteEndElement(); // ExtendedData

                    writer.WriteStartElement("Point");
                    writer.WriteElementString("coordinates", string.Format(KML_CULTURE, "{0},{1}", melding.Longitude, melding.Latitude));

                    writer.WriteEndElement(); // Point
                    writer.WriteEndElement(); // Placemark
                }
            }
        }

        private static void WriteExtendedData(XmlWriter writer, Melding melding)
        {
            writer.WriteStartElement("Data");
            writer.WriteElementString("displayName", "melder");
            writer.WriteElementString("value", melding.Melder);
            writer.WriteEndElement(); // Data

            writer.WriteStartElement("Data");
            writer.WriteElementString("displayName", "verzondenOp");
            writer.WriteElementString("value", string.Format("{0:s}", melding.VerzondenOp));
            writer.WriteEndElement(); // Data

            writer.WriteStartElement("Data");
            writer.WriteElementString("displayName", "opmerkingen");
            writer.WriteElementString("value", GetOpmerkingen(melding));
            writer.WriteEndElement(); // Data
        }

        private static string GetOpmerkingen(Melding melding)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Meldingen.Opmerking opmerking in melding.Opmerkings)
            {
                sb.AppendLine(string.Format("{0}: {1}", opmerking.AangemaaktDoor, opmerking.Tekst));
            }
            return sb.ToString();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}