﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace DiasComputer.Utility.SendEmail
{
    public class SendEmail
    {
        public static void Send(string To,string Subject,string Body)
        {
            MailMessage mail = new MailMessage();

            //For google
            //SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");


            SmtpClient SmtpServer = new SmtpClient("smtp-mail.outlook.com");
            mail.From = new MailAddress("diascomputertest@hotmail.com", "DiasComputer");
            mail.To.Add(To);
            mail.Subject = Subject;
            mail.Body = Body;
            mail.IsBodyHtml = true;

            //System.Net.Mail.Attachment attachment;
            // attachment = new System.Net.Mail.Attachment("c:/textfile.txt");
            // mail.Attachments.Add(attachment);




            SmtpServer.Port = 587;

            //Note : for sending email password should be changed !

            SmtpServer.Credentials = new System.Net.NetworkCredential("diascomputertest@hotmail.com", "********");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
            SmtpServer.Dispose();
        }
    }
}