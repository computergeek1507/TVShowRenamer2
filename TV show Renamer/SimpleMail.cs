/*****************************************************************************************/
/* ***************************************************************************************
 * SimpleMail.cs
 * ***************************************************************************************/
/*****************************************************************************************/
//--
//-- Writtrn by Scott
//--
using System;
using System.Diagnostics;
using System.Text;
using System.Net.Mail;
//
//-- .NET 4 made this class worthless 
//
namespace TV_Show_Renamer
{
    public class SimpleMail
    {
        public class SMTPMailMessage
        {
            public string From;
            public string To;
            public string Subject;
            public string Body;
            public string BodyHTML;
            public string AttachmentPath;
        }

        public class SMTPClient
        {
            private string _strServer = "smtp.1and1.com";

            private int _intPort = 587;
            private string _strUserName = "bugs@scottnation.com";

            private bool _useSSL = true;

            private string _strUserPassword = "computer";            

            //private string _strLastResponse;

            public string AuthUser
            {
                get { return _strUserName; }
                set { _strUserName = value; }
            }
            public string AuthPassword
            {
                get { return _strUserPassword; }
                set { _strUserPassword = value; }
            }
            public int Port
            {
                get { return _intPort; }
                set { _intPort = value; }
            }
            public bool UseSSL
            {
                get { return _useSSL; }
                set { _useSSL = value; }
            }
            public string Server
            {
                get { return _strServer; }
                set { _strServer = value; }
            }
            //Send Mail
            public void SendMail(SMTPMailMessage mail)
            {
                try
                {
                    MailMessage Newmail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient(_strServer);
                  
                    Newmail.From = new MailAddress(_strUserName);
                    Newmail.To.Add(mail.To);
                    Newmail.Subject = mail.Subject;
                    Newmail.Body = mail.Body;

                    System.Net.Mail.Attachment attachment;
                    attachment = new System.Net.Mail.Attachment(mail.AttachmentPath);
                    Newmail.Attachments.Add(attachment);

                    SmtpServer.EnableSsl = _useSSL;
                    SmtpServer.UseDefaultCredentials = true;
                    SmtpServer.Port = _intPort;
                    SmtpServer.Credentials = new System.Net.NetworkCredential(_strUserName, _strUserPassword);
                    
                    SmtpServer.Send(Newmail);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    return;
                }
            }            
        }
    }
}