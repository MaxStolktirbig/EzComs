using EzComs.Common.CustomExceptions;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace EzComs.Model.ActionContext.ConcreteActions
{
    public class SendEmailAction : IAction
    {
        public Guid Id { get; set; }
        public object ActionOptions { get; set; } =  new EmailDetails(
                fromAddress:    "maxstoltenborgh@hotmail.com",
                toAddress:      "maxstoltenborgh@hotmail.com",
                smtpUsername:   "hverhoed@gmail.com",
                smtpPassword:   "p@55_word15" ,
                body:           "This is the body message",
                subject:        "This is a test email subject",
                smtpClientUrl:  "smtp.gmail.com"
                );
        public List<IAction> dependsOn { get; set; } = new();
        public List<IAction> nextActions { get; set; } = new();
        public ActionState State { get; set; }
        public bool NextActionsCompleted { get; set; }

        private bool ValidateOptions()
        {
            return ActionOptions is EmailDetails;
        }

        //very simple example of an action taking place
        public async Task<ActionState> Execute()
        {
            try
            {
                if (!ValidateOptions()) throw new Exception("Required Options were invalid");
                var emailDetails = ActionOptions as EmailDetails;
                if (emailDetails == null) throw new Exception("Something went wrong converting to EmailDetails");

                var smtpClient = new SmtpClient(emailDetails.smtpClientUrl)
                {
                    Port = 587,
                    Credentials = new NetworkCredential(emailDetails.smtpUsername, emailDetails.smtpPassword),
                    EnableSsl = true,
                };
                
                smtpClient.Send(emailDetails.fromAddress, emailDetails.toAddress, emailDetails.subject, emailDetails.body);
                State = ActionState.DONE;
            }
            catch (Exception)
            {
                State=ActionState.FAILED;
                throw;
            }
            return State;
        }
    }
    public class EmailDetails
    {
        public string fromAddress { get; set; }
        public string toAddress { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public string smtpPassword { get; set; }
        public string smtpUsername { get; set; }
        public string smtpClientUrl { get; set; }

        public EmailDetails(string fromAddress, string toAddress, string subject, string body, string smtpPassword, string smtpUsername, string smtpClientUrl)
        {
            this.fromAddress    = fromAddress;
            this.toAddress      = toAddress;
            this.subject        = subject;
            this.body           = body;
            this.smtpPassword   = smtpPassword;
            this.smtpUsername   = smtpUsername;
            this.smtpClientUrl  = smtpClientUrl;
        }
    }
}
