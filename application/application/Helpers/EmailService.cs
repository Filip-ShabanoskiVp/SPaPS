using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Postal;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace DataAccess.Services
{
    public class MessageModel : Postal.Email
    {
        protected MessageModel() : base()
        {

        }

        public MessageModel(string viewName)
            : base(viewName)
        {

        }

        public MessageModel(string viewName, string areaName)
            : base(viewName, areaName, new EmptyModelMetadataProvider())
        {

        }

        public string To
        {
            get
            {
                return (string)ViewData["to"];
            }
            set
            {
                ViewData["to"] = value;
            }
        }
        public string Cc
        {
            get
            {
                return (string)ViewData["cc"];
            }
            set
            {
                ViewData["cc"] = value;
            }
        }
        public string Bcc
        {
            get
            {
                return (string)ViewData["bcc"];
            }
            set
            {
                ViewData["bcc"] = value;
            }
        }
        public string Subject
        {
            get
            {
                return (string)ViewData["Subject"];
            }
            set
            {
                ViewData["Subject"] = value;
            }
        }
        public string DisplayName
        {
            get
            {
                return (string)ViewData["DisplayName"];
            }
            set
            {
                ViewData["DisplayName"] = value;
            }
        }
    }
    public class EmailSetUp
    {
        public string To { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Template { get; set; }
        public string Callback { get; set; }
        public RequestPath RequestPath { get; set; }
        public string Token { get; set; }
        public List<Attachment> Attachments { get; set; } = new List<Attachment>();
    }

    public class EmailSenderOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSSL { get; set; }
        public string FromAddress { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
    }

    public interface IEmailSenderEnhance : IEmailSender
    {
        Task SendEmailAsync(MailMessage mailMessage);
        Task SendEmailAsync(EmailSetUp model);
        RequestPath PostalRequest(HttpRequest Request);
    }


    public class EmailSender : IEmailSenderEnhance
    {
        // Our private configuration variables
        private readonly EmailSenderOptions _emailOptions;
        private readonly IEmailService _emailService;
        private readonly IWebHostEnvironment _env;


        // Get our parameterized configuration
        public EmailSender(IEmailService emailService,
            IWebHostEnvironment env,
            IOptions<EmailSenderOptions> emailOptions)
        {
            _emailOptions = emailOptions.Value;
            _emailService = emailService;
            _env = env;
        }

        private SmtpClient CreateSmtpClient()
        {
            var client = new SmtpClient(_emailOptions.Host, _emailOptions.Port)
            {
                EnableSsl = _emailOptions.EnableSSL,
                UseDefaultCredentials = true,
                Credentials = new NetworkCredential(_emailOptions.Username, _emailOptions.Password),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            return client;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using (var client = CreateSmtpClient())
            {
                await client.SendMailAsync(
                    new MailMessage(_emailOptions.FromAddress, email, subject, htmlMessage) { IsBodyHtml = true }
                );
            }
        }

        public async Task SendEmailAsync(MailMessage mailMessage)
        {
            mailMessage.From = new MailAddress(_emailOptions.FromAddress, _emailOptions.DisplayName);
            using (var client = CreateSmtpClient())
            {
                await client.SendMailAsync(mailMessage);
            }
        }

        public async Task SendEmailAsync(EmailSetUp model)
        {
            var emailData = new Email(model.Template);
            emailData.RequestPath = model.RequestPath;
            emailData.ViewData["To"] = model.To;
            emailData.ViewData["Email"] = model.To;
            emailData.ViewData["Username"] = model.Username;
            emailData.ViewData["Password"] = model.Password;
            emailData.ViewData["Callback"] = model.Callback;

            if (model.Attachments.Count > 0)
                emailData.Attachments = model.Attachments;

            MailMessage message = await _emailService.CreateMailMessageAsync(emailData);
            message.From = new MailAddress(_emailOptions.FromAddress);
            await SendEmailAsync(message);
        }

        public RequestPath PostalRequest(HttpRequest Request)
        {
            var requestPath = new Postal.RequestPath();
            requestPath.PathBase = Request.PathBase.ToString();
            requestPath.Host = Request.Host.ToString();
            requestPath.IsHttps = Request.IsHttps;
            requestPath.Scheme = Request.Scheme;
            requestPath.Method = Request.Method;

            return requestPath;
        }
    }
}