using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Meldingen
{
    public class McMainMelding
    {
        public McMainMeldingItem[] MeldingItems;
        public decimal? Lat;
        public decimal? Lon;
        public string Melder;

        public static McMainMelding GenerateFromMessageBody(string bodyPart)
        {
            var items = ParseItems(bodyPart);
            if (items == null) return null;

            var lat = items.SingleOrDefault(i => i.Naam == "Latitude")?.Waarde;
            var lon = items.SingleOrDefault(i => i.Naam == "Longitude")?.Waarde;
            var melder = items.SingleOrDefault(i => i.Naam == "E-mail melder:")?.Waarde;

            return new McMainMelding
            {
                MeldingItems = items,
                Lat = lat == null ? default(decimal?) : decimal.Parse(lat),
                Lon = lon == null ? default(decimal?) : decimal.Parse(lon),
                Melder = melder,
            };
        }

        static McMainMeldingItem[] ParseItems(string bodyPart)
        {
            if (bodyPart == null) return null;

            const string startTag = "<table class=\"general-info\">";
            const string endTag = "</table>";

            var items = new List<McMainMeldingItem>();
            var position = bodyPart.IndexOf(startTag, StringComparison.Ordinal);
            while (position > 0)
            {
                var last = bodyPart.IndexOf(endTag, position + 1, StringComparison.Ordinal) + endTag.Length;

                var xmlText = bodyPart.Substring(position, last - position);
                try
                {
                    var xml = XElement.Parse(xmlText);

                    var itemsIntern = xml.Elements("tr").Select(i => i.Elements().ToArray()).Select(e => McMainMeldingItem.Create(e[0].Value, e[1].Value)).ToArray();
                    items.AddRange(itemsIntern);
                }
                catch (Exception ex)
                {
                    items.Add(McMainMeldingItem.Create("Fout bij verwerking", ex.Message));
                }
                position = bodyPart.IndexOf(startTag, last, StringComparison.Ordinal);
            }
            return items.ToArray();
        }
    }

    public class McMainMeldingItem
    {
        public string Naam;
        public string Waarde;

        public static McMainMeldingItem Create(string naam, string waarde)
        {
            return new McMainMeldingItem
            {
                Naam = naam,
                Waarde = waarde,
            };
        }
    }
}