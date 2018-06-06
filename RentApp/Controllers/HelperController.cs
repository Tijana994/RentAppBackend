using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace RentApp.Controllers
{
    public class HelperController
    {
        private HelperController()
        {
            // Prevent outside instantiation
        }

        private static readonly HelperController _helper = new HelperController();

        public static HelperController GetHelperController()
        {
            return _helper;
        }

        public static int sendAccountConfirmationEmail(string userEmail)
        {
            string email_to = userEmail;
            string email_from = "forumblok@gmail.com";
            string email_from_sifra = "sifra123";

            try
            {
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(email_from, email_from_sifra);

                MailMessage mm = new MailMessage(email_from, email_to);
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.Subject = @"Account Confirmation [MT RentAVehicle]";
                mm.Body = "Dear,\n\nYour account on MT RentAVehicle service is confirmed\n\nThe best regards,\nMT RentAVehicle Admin tim";
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                client.Send(mm);
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Greska, email nije poslat");
                return -1;
            }
        }

    }
}