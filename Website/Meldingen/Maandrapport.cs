using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Exchange.WebServices.Data;

namespace Meldingen
{
    public class Maandrapport
    {
        public static void Verstuur(DataClassesDataContext db)
        {
            /*
                Meldingnummer
                Omschrijving
                AangemaaktOp
                Toegewezen aan
                DatumToegewezenAanGewijzigd

                De lijst bevat de meldingen, gefilterd op:
                - Toegewezen aan <te configureren
                - DatumToegewezenAanGewijzigd is in de afgelopen maand
             */ 

            var filterdatumTot = new DateTime(DateTime.Today.Year, DateTime.Today.Month,1);
            var filterdatumVan = filterdatumTot.AddMonths(-1);
            var meldingen = db
                .Meldings
                .Where(m => m.DatumToegewezenAanGewijzigd >= filterdatumVan)
                .Where(m => m.DatumToegewezenAanGewijzigd <= filterdatumTot)
                .Where(m => m.ToegewezenAan == "Rijkswaterstaat")
                .Select(m => string.Format("{0}\t{1}\t{2:dd-MM-yyyy}\t{3}\t{4:dd-MM-yyyy}", m.Id, m.Onderwerp.Replace('\t', ' '), m.VerzondenOp, m.ToegewezenAan.Replace('\t', ' '), m.DatumToegewezenAanGewijzigd));

            var header = "Nummer\tOnderwerp\tAangemaakt op\tToegewezen aan\tDatum toewijzing" + Environment.NewLine;
            var rapport = header + string.Join(Environment.NewLine, meldingen);

            MailClient.Send(db.Configuraties.MailadresMaandrapport(), "Maandrapport Meldingen", db.Configuraties.MailBodyMaandrapport(), rapport);
        }
    }
}