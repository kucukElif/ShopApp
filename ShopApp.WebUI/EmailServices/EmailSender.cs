using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Net.Mail;

namespace ShopApp.WebUI.EmailServices
{
    public class EmailSender : IEmailSender
    {
        
            private readonly IConfiguration _configuration;

            public EmailSender(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            public async Task SendEmailAsync(string email, string subject, string htmlMessage)
            {
                var settings = _configuration.GetSection("EmailSettings");

                var smtpClient = new SmtpClient
                {
                    Host = settings["Host"],
                    Port = int.Parse(settings["Port"]),
                    EnableSsl = bool.Parse(settings["EnableSsl"]),
                    Credentials = new NetworkCredential(
                        settings["UserName"],
                        settings["Password"]
                    )
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(settings["UserName"], "Shop App"),
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(email);

                await smtpClient.SendMailAsync(mailMessage);
            }
        }
    }

