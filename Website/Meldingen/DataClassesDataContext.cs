using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Meldingen
{
  public partial class DataClassesDataContext
  {

    partial void UpdateMelding(Melding instance)
    {
      instance.GewijzigdOp = System.DateTime.Now;
      instance.GewijzigdDoor = HttpContext.Current.User.Identity.Name.ToString().Replace("PROVGRON\\", "");
      ExecuteDynamicUpdate(instance);
    }
  }
}