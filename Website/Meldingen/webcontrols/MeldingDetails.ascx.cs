using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Meldingen.webcontrols
{
  public partial class MeldingDetails : System.Web.UI.UserControl, IScriptControl
  {
    public int? IdMelding
    {
      get
      {
        return (int?)ViewState["IdMelding"];
      }
      private set
      {
        ViewState["IdMelding"] = value;
      }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }


    protected void NieuweOpmerking(object sender, EventArgs e)
    {
      using (DataClassesDataContext context = new DataClassesDataContext())
      {
        Melding melding = context.Meldings.First(m => m.Id == this.IdMelding);
        Opmerking opmerking = new Opmerking();
        opmerking.AangemaaktOp = DateTime.Now;
        opmerking.AangemaaktDoor = HttpContext.Current.User.Identity.Name.ToString().Replace("PROVGRON\\", "");
        opmerking.Tekst = TextBoxNieuweOpmerking.Text;
        TextBoxNieuweOpmerking.Text = "Nieuwe opmerking";
        melding.Opmerkings.Add(opmerking);
        context.SubmitChanges();
        ShowMelding(melding, context.ToeTeWijzenAans.Select(t => t.Naam));
      }
    }

    public void ShowMelding(int? idMelding)
    {
      IdMelding = idMelding;
      if (idMelding == null)
      {
        SetVisibility(false);
        return;
      }
      SetVisibility(true);
      using (DataClassesDataContext context = new DataClassesDataContext())
      {
        Melding melding = context.Meldings.First(m => m.Id == idMelding);
        ShowMelding(melding, context.ToeTeWijzenAans.Select(t => t.Naam));
      }
    }

    private void ShowMelding(Melding melding, IEnumerable<string> toeTeWijzenAan)
    {
      string urlBase = Request.Url.GetLeftPart(UriPartial.Path).ToLower().Replace("/meldingenopkaart.aspx", "");
      this.LabelMeldingId.Text = melding.Id.ToString("00000");

      this.HyperLinkInNewWindow.NavigateUrl = string.Format("{0}/?meldingId={1:00000}", urlBase, melding.Id);
      this.LabelOmschrijving.Text = melding.Onderwerp;

      this.LinkMelder.NavigateUrl = "mailto://" + melding.Melder;
      this.LinkMelder.Text = melding.Melder;
      this.LabelVerzondenOp.Text = melding.VerzondenOp.ToString("d");
      this.LabelStatus.Text = melding.Status.Naam;
      TextBoxNieuweOpmerking.Text = "Nieuwe opmerking";
      switch (melding.Status.Naam)
      {
        case Status.StatusOpen:
          this.LinkButtonChangeStatus.Text = "melding sluiten";
          break;
        case Status.StatusGesloten:
          this.LinkButtonChangeStatus.Text = "melding heropenen";
          break;
        default:
          throw new InvalidOperationException("Status " + melding.Status.Naam + " is not handled.");
      }
      this.ImageThumb.ImageUrl = "../GetPrimaireFoto.ashx?Size=ThumbBig&meldingId=" + melding.Id.ToString();
      this.ImageThumb.NavigateUrl = "../GetPrimaireFoto.ashx?Size=Normal&meldingId=" + melding.Id.ToString();

      System.Globalization.CultureInfo latLonCulture = new System.Globalization.CultureInfo("en-US");
      string latLon = string.Format(latLonCulture, "{0:0.00000},{1:0.00000}", melding.Latitude, melding.Longitude);
      this.LabelCoordinaat.Text = string.Format("WGS-84: {0}", latLon);
      ScriptManager.RegisterClientScriptBlock(LabelCoordinaat, typeof(Label), "coordinates", string.Format("var c = wgs84ToRDNew({0}); $('#meldingDetails_LabelCoordinaat').append('; RDNew: '+ Math.round(100*c.x)/100 + ', ' + Math.round(100*c.y)/100);", latLon), true);

      //zie voor de Google Maps parameters: http://mapki.com/wiki/Google_Map_Parameters
      this.HyperLinkMailMe.NavigateUrl = string.Format("mailto:?subject=Melding {0:00000}: {1}&body=Kijk voor meldingdetails hier: {2}/?meldingId={0:00000}%0D%0A%0D%0AKlik hier om de locatie in Google Maps te zien: http://maps.google.nl/?q=Melding+{0:00000}@{3}%26z=17%26t=h", melding.Id, melding.Onderwerp, urlBase, latLon);

      this.GridViewOpmerkingen.DataSource = melding.Opmerkings.OrderByDescending(o => o.AangemaaktOp);
      this.GridViewOpmerkingen.DataBind();

      this.GridViewBijlagen.DataSource = melding.Bijlages.Where(b => !b.IsPrimaireFoto).OrderByDescending(b => b.Naam);
      this.GridViewBijlagen.DataBind();

      var items = toeTeWijzenAan.Union(new string[] { null }).OrderBy(t => t).Select(t => new ListItem(t) { Selected = t == melding.ToegewezenAan });
      this.DropDownListToeTeWijzenAan.Items.Clear();
      this.DropDownListToeTeWijzenAan.Items.AddRange(items.ToArray());
    }

    private void SetVisibility(bool visibility)
    {
      this.PanelDetails.Visible = visibility;
    }

    protected void LinkButtonChangeStatus_OnClick(object sender, EventArgs e)
    {
      using (DataClassesDataContext context = new DataClassesDataContext())
      {
        Melding melding = context.Meldings.First(m => m.Id == this.IdMelding);
        if (melding.Status.Naam == Status.StatusOpen) melding.Status = context.Status.First(s => s.Naam == Status.StatusGesloten);
        else if (melding.Status.Naam == Status.StatusGesloten) melding.Status = context.Status.First(s => s.Naam == Status.StatusOpen);
        melding.GewijzigdDoor = HttpContext.Current.User.Identity.Name.ToString().Replace("PROVGRON\\", "");
        melding.GewijzigdOp = DateTime.Now;
        context.SubmitChanges();
        ShowMelding(melding, context.ToeTeWijzenAans.Select(t => t.Naam));
      }
    }

    protected void ButtonChangeCoordinate_OnClick(object sender, EventArgs e)
    {
      using (DataClassesDataContext context = new DataClassesDataContext())
      {
        Melding melding = context.Meldings.First(m => m.Id == this.IdMelding);
        System.Globalization.CultureInfo latLonCulture = new System.Globalization.CultureInfo("en-US");
        melding.Latitude = decimal.Parse(TextboxLat.Text, latLonCulture);
        melding.Longitude = decimal.Parse(TextboxLon.Text, latLonCulture);
        melding.GewijzigdDoor = HttpContext.Current.User.Identity.Name.ToString().Replace("PROVGRON\\", "");
        melding.GewijzigdOp = DateTime.Now;
        context.SubmitChanges();
        ShowMelding(melding, context.ToeTeWijzenAans.Select(t => t.Naam));
      }
    }

    public IEnumerable<ScriptDescriptor> GetScriptDescriptors()
    {
      throw new NotImplementedException();
    }

    public IEnumerable<ScriptReference> GetScriptReferences()
    {
      throw new NotImplementedException();
    }

    protected void GridViewOpmerkingen_RowDataBound(object sender, GridViewRowEventArgs e)
    {
      if (e.Row.RowType != DataControlRowType.DataRow) return;
      Opmerking opmerking = (Opmerking)e.Row.DataItem;
      TimeSpan leeftijd = DateTime.Now.Date.Subtract(opmerking.AangemaaktOp.Date);
      switch (leeftijd.Days)
      {
        case 0:
          ((Label)e.Row.FindControl("LabelAangemaaktOp")).Text = "vandaag";
          break;
        case 1:
          ((Label)e.Row.FindControl("LabelAangemaaktOp")).Text = "gisteren";
          break;
        case 2:
          ((Label)e.Row.FindControl("LabelAangemaaktOp")).Text = "eergisteren";
          break;
        default:
          ((Label)e.Row.FindControl("LabelAangemaaktOp")).Text = opmerking.AangemaaktOp.ToString("d");
          break;
      }
    }

    protected void GridViewBijlagen_RowDataBound(object sender, GridViewRowEventArgs e)
    {
      if (e.Row.RowType != DataControlRowType.DataRow) return;
      Bijlage bijlage = (Bijlage)e.Row.DataItem;
      HyperLink hyperlinkToonDocument = (HyperLink)e.Row.FindControl("HyperlinkToonDocument");
      if (bijlage.IsPrimaireFoto)
      {
        hyperlinkToonDocument.Text = "Meldingfoto";
        hyperlinkToonDocument.NavigateUrl = "../GetBijlage.ashx?bijlageId=" + bijlage.Id.ToString();
      }
      else
      {
        hyperlinkToonDocument.Text = "Extra bijlage: " + bijlage.Naam;
        hyperlinkToonDocument.NavigateUrl = "../GetBijlage.ashx?bijlageId=" + bijlage.Id.ToString();
      }
    }

    protected void ButtonWijsToe_Click(object sender, EventArgs e)
    {
      using (DataClassesDataContext context = new DataClassesDataContext())
      {
        var melding = context.Meldings.First(m => m.Id == this.IdMelding);
        melding.ToegewezenAan = this.DropDownListToeTeWijzenAan.SelectedValue;
        melding.DatumToegewezenAanGewijzigd = DateTime.Today;
        melding.GewijzigdDoor = HttpContext.Current.User.Identity.Name.ToString().Replace("PROVGRON\\", "");
        melding.GewijzigdOp = DateTime.Now;
        context.SubmitChanges();
        ShowMelding(melding, context.ToeTeWijzenAans.Select(t => t.Naam));
      }
    }
  }
}