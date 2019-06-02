using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Templates;
using Templates.ViewModels;

namespace Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class EmailsController : ControllerBase
  {
    private readonly IRazorViewToStringRenderer _renderer;

    public EmailsController(IRazorViewToStringRenderer renderer)
    {
      _renderer = renderer;
    }
    
    // GET api/emails/send
    [HttpGet, Route("send")]
    public async Task<IActionResult> Send()
    {
      try
      {
        var from = new MailAddress("from@mail.com", "Derek Arends");
        var to = new MailAddress("to@mail.com");

        var model = new HelloWorldViewModel("https://www.google.com");

        const string view = "/Views/Emails/HelloWorld/HelloWorld";
        var htmlBody = await _renderer.RenderViewToStringAsync($"{view}Html.cshtml", model);
        var textBody = await _renderer.RenderViewToStringAsync($"{view}Text.cshtml", model);

        var message = new MailMessage(from, to)
        {
          Subject = "Hello World!",
          Body = textBody
        };

        message.AlternateViews.Add(
          AlternateView.CreateAlternateViewFromString(htmlBody, Encoding.UTF8, MediaTypeNames.Text.Html));

        using (var smtp = new SmtpClient("smtp.mailserver.com", 587))
        {
          smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
          smtp.UseDefaultCredentials = false;
          smtp.EnableSsl = true;
          smtp.Credentials = new NetworkCredential("smtp_user","smtp_password");
          await smtp.SendMailAsync(message);
        }
      }
      catch (Exception e)
      {
        return StatusCode(500, $"Failed to send email: {e.Message}");
      }

      return Ok("Email Sent!");
    }
  }
}