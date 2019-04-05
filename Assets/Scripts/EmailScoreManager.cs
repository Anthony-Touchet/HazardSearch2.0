using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class EmailScoreManager : MonoBehaviour
{
    public void SendMailToStudent()
    {
        SendMail(PlayerPrefs.GetString("teacherEmail"), PlayerPrefs.GetString("studentEmail"),
        "Hazard Awareness Score", RandomHazardManager.instance.MakeResultString(),
        PlayerPrefs.GetString("teachePassword"));
    }

    void SendMail(string aFrom, string aTo, string aSubject, string aBody, string aPassword)
    {
        if (!aTo.Contains("@") && !aTo.ToLower().Contains(".com"))
            return;

        aTo = aTo.Trim();

        MailMessage mail = new MailMessage();

        mail.From = new MailAddress(aFrom);
        mail.To.Add(aTo);
        mail.Subject = aSubject;
        mail.Body = aBody;

        SmtpClient smtpServer = new SmtpClient();
        smtpServer.Host = "smtp.gmail.com";
        smtpServer.Port = 587;
        smtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtpServer.Credentials = new System.Net.NetworkCredential(aFrom, aPassword) as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback =
            delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            { return true; };
        smtpServer.Send(mail);
    }
}
