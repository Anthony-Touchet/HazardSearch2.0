using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class EmailScoreManager : MonoBehaviour
{
    public static string teacherMasterScoreEmail = "";

    public static void SendTeacherMasterScoreEmail()
    {
        SendMail("no-reply@tantrumlab.com", PlayerPrefs.GetString("teacherEmail"),
        "Hazard Awareness Scores", teacherMasterScoreEmail,
        "1jd7Y^%rg)j6%#209");
    }

    public void SendMailToStudent()
    {
        var randomHazards = FindObjectOfType<RandomHazardManager>();
        string score = randomHazards.MakeResultString();
        teacherMasterScoreEmail += PlayerPrefs.GetString("studentEmail") + ": " + score + "\n\n";

        SendMail("no-reply@tantrumlab.com", PlayerPrefs.GetString("studentEmail"),
        "Hazard Awareness Score", score,
        "1jd7Y^%rg)j6%#209");
    }

    static void SendMail(string aFrom, string aTo, string aSubject, string aBody, string aPassword)
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