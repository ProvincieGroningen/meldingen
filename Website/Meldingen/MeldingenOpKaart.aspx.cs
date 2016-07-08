using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Meldingen
{
  public partial class MeldingenOpKaart : System.Web.UI.Page
  {

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void Melding_Selected(object sender, EventArgs e)
    {
      ToonMelding();
    }

    private void ToonMelding()
    {
      if (string.IsNullOrEmpty(TextBoxIdMelding.Text))
        meldingDetails.ShowMelding(null);
      else
        meldingDetails.ShowMelding(int.Parse(TextBoxIdMelding.Text));
    }
  }
}