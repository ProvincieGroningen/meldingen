using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Meldingen
{
  /// <summary>
  /// Ping, this is used to keep the application/application pool running
  /// This is needed otherwise mail processing will stop
  /// </summary>
  public class Ping : IHttpHandler
  {

    public void ProcessRequest(HttpContext context)
    {
      context.Response.ContentType = "text/plain";
      context.Response.Write("OK");
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