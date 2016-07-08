using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Meldingen
{
    public partial class Beheer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                using (Meldingen.DataClassesDataContext db = new DataClassesDataContext())
                {
                    var toeTeWijzenAan = string.Join(Environment.NewLine, db.ToeTeWijzenAans.Select(t => t.Naam));
                    this.TextBoxToeTeWijzenAan.Text = toeTeWijzenAan;
                    this.TextBoxMailadresMaandrapport.Text = db.Configuraties.MailadresMaandrapport();
                    var x = db.Configuraties.MailBodyMaandrapport();
                    this.TextBoxMailBodyMaandrapport.Text = x;
                }
            }
        }

        protected void ButtonBewaar_Click(object sender, EventArgs e)
        {
            var items = this.TextBoxToeTeWijzenAan.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            using (Meldingen.DataClassesDataContext db = new DataClassesDataContext())
            {
                var weg = db.ToeTeWijzenAans.Where(t => !items.Contains(t.Naam));
                foreach (var w in weg) { db.ToeTeWijzenAans.DeleteOnSubmit(w); }

                var nieuw = items.Where(i => !db.ToeTeWijzenAans.Any(t => t.Naam == i));
                foreach (var n in nieuw) { db.ToeTeWijzenAans.InsertOnSubmit(new ToeTeWijzenAan() { Naam = n }); }

                db.Configuraties.MailadresMaandrapport(this.TextBoxMailadresMaandrapport.Text);
                db.Configuraties.MailBodyMaandrapport(this.TextBoxMailBodyMaandrapport.Text);
                db.SubmitChanges();
            }
            string scriptBlock =
               @"<script language=""JavaScript"">
               <!--
                  alert(""De wijzigingen zijn bewaard."");
               // -->
               </script>";
            string scriptKey = "intoPopupMessage:" + this.UniqueID;
            ClientScript.RegisterClientScriptBlock(this.GetType(), scriptKey, scriptBlock);
        }

        protected void ButtonVerstuurMaandrapport_Click(object sender, EventArgs e)
        {
            using (Meldingen.DataClassesDataContext db = new DataClassesDataContext())
            {
                Maandrapport.Verstuur(db);
            }
            string scriptBlock =
           @"<script language=""JavaScript"">
               <!--
                  alert(""Het maandrapport is verstuurd"");
               // -->
               </script>";
            string scriptKey = "intoPopupMessage:" + this.UniqueID;
            ClientScript.RegisterClientScriptBlock(this.GetType(), scriptKey, scriptBlock);
        }
    }
}