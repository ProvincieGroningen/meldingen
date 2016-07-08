using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Meldingen
{
    /// <summary>
    /// Class Orphan genereert coördinaten in een configureerbaar extent. 
    /// Deze coordinaten kunnen worden gebruikt om meldingen zonder coordinaat
    /// toch op de kaart te zetten.
    /// 
    /// configuratie-paramters:
    /// - WeesExtentNoordWestLatLon
    /// - WeesExtentZuidOostLatLon
    /// </summary>
    public class Orphan
    {

        private static bool valuesRead = false;
        private static Random random = new Random();

        private static void ReadCoordinates()
        {
            if (valuesRead)
            {
                return;
            }
            string[] noordwest = Meldingen.Properties.Settings.Default.WeesExtentNoordWestLatLon.Split(',');
            string[] zuidoost = Meldingen.Properties.Settings.Default.WeesExtentZuidOostLatLon.Split(',');
            System.Globalization.CultureInfo latLonCulture = new System.Globalization.CultureInfo("en-US");
            MinLat = decimal.Parse(zuidoost[0], latLonCulture);
            MaxLat = decimal.Parse(noordwest[0], latLonCulture);
            MaxLon = decimal.Parse(zuidoost[1], latLonCulture);
            MinLon = decimal.Parse(noordwest[1], latLonCulture);
        }

        private static decimal MinLat { get; set; }

        private static decimal MaxLat { get; set; }

        private static decimal MinLon { get; set; }

        private static decimal MaxLon { get; set; }

        public static decimal GetLat
        {
            get
            {
                ReadCoordinates();
                return MinLat + ((decimal)random.NextDouble()) * (MaxLat - MinLat);
            }
        }

        public static decimal GetLon
        {
            get
            {
                ReadCoordinates();
                return MinLon + ((decimal)random.NextDouble()) * (MaxLon - MinLon);
            }
        }

    }
}