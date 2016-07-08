using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Meldingen
{
    public static class Config
    {

        private static string ConfigWaarde(this System.Data.Linq.Table<Configuratie> configs, string naam)
        {
            return configs.Where(c => c.Naam == naam).Select( c => c.Waarde).FirstOrDefault();
        }

        private const string ConfigMailadresMaandrapport = "MailadresMaandrapport";
        public static string MailadresMaandrapport(this System.Data.Linq.Table<Configuratie> configs)
        {
            return configs.ConfigWaarde(ConfigMailadresMaandrapport);
        }

        public static void MailadresMaandrapport(this System.Data.Linq.Table<Configuratie> configs, string value)
        {
            var config = configs.Where(c => c.Naam == ConfigMailadresMaandrapport).FirstOrDefault();
            if (config == null)
            {
                config = new Configuratie { Naam = ConfigMailadresMaandrapport, Waarde = value };
                configs.InsertOnSubmit(config);
            }   
            else
            {
                config.Waarde = value;
            }
            config.Waarde = value;
        }


        private const string ConfigMailBodyMaandrapport = "MailBodyMaandrapport";
        public static string MailBodyMaandrapport(this System.Data.Linq.Table<Configuratie> configs)
        {
            return configs.ConfigWaarde(ConfigMailBodyMaandrapport);
        }

        public static void MailBodyMaandrapport(this System.Data.Linq.Table<Configuratie> configs, string value)
        {
            var config = configs.Where(c => c.Naam == ConfigMailBodyMaandrapport).FirstOrDefault();
            if (config == null)
            {
                config = new Configuratie { Naam = ConfigMailBodyMaandrapport, Waarde = value };
                configs.InsertOnSubmit(config);
            }
            else
            {
                config.Waarde = value;
            }
            config.Waarde = value;
        }

        private const string ConfigMaandrapportLastSent = "MaandrapportLastSent";
        public static DateTime MaandrapportLastSent(this System.Data.Linq.Table<Configuratie> configs)
        {
            var waarde = configs.ConfigWaarde(ConfigMaandrapportLastSent);
            if (waarde == null)
                return DateTime.MinValue;
            return new DateTime(long.Parse(waarde));
        }

        public static void MaandrapportLastSent(this System.Data.Linq.Table<Configuratie> configs, DateTime value)
        {
            var config = configs.Where(c => c.Naam == ConfigMaandrapportLastSent).FirstOrDefault();
            if (config == null)
            {
                config = new Configuratie { Naam = ConfigMaandrapportLastSent, Waarde = value.Ticks.ToString() };
                configs.InsertOnSubmit(config);
            }
            else
            {
                config.Waarde = value.Ticks.ToString();
            }
            config.Waarde = value.Ticks.ToString();
        }

    }
}