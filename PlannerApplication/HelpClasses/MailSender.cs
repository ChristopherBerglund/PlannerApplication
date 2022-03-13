using MailKit.Net.Smtp;
using MimeKit;
using PlannerApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PlannerApplication.HelpClasses
{
    public class MailSender
    {
        public static void Send(string _email, string Sub, string messageText )
        {
            string toEmail = _email;
            var mailAddress = "bobertestar@gmail.com";
            var password = "Hejsan123!";

            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("PlannerApplication", mailAddress));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = Sub.ToString();
            message.Body = new TextPart("plain")
            {
                Text = messageText.ToString()
            };
            SmtpClient client = new SmtpClient();
            try
            {
                client.CheckCertificateRevocation = false;
                client.Connect("smtp.gmail.com", 465, true);
                client.Authenticate(mailAddress, password);
                client.Send(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }

        public static void EventDeletedMessage(participant user, newactivity thisActivity, string? _reason)
        {
            var sw = new StringBuilder();
            sw.Append($"Hej, {user.Name}! \n");
            sw.Append($"\n");
            sw.Append($"Vi måste tyvärr meddela att {thisActivity.User.firstName} har ställt in eventet med {thisActivity.Activity.Name} som skulle ha ägt rum ");
            sw.Append($"{thisActivity.When} vid {thisActivity.Where}.\n");
            sw.Append($"Orsak: {_reason}.\n");
            sw.Append($"\n");
            sw.Append($"\n");
            sw.Append($"Här skulle det kunna komma upp förslag på andra event inom samma kategori?");
            sw.Append($"\n");
            sw.Append($"\n");
            sw.Append($"Vi på \"Vad ska vi hitt på\" önskar dig en fortsatt fin dag! ");

            var sub = $"Eventet: {thisActivity.Activity.Name}, har blivit inställt.";

            Send(user.User.Email, sub, sw.ToString());
        }

        public static void TwoHoursMessage(participant user, newactivity thisActivity)
        {
            var sw = new StringBuilder();
            sw.Append($"Hej, {user.Name}! \n");
            sw.Append($"\n");
            sw.Append($"Vi vill bara påminna dig om att {thisActivity.User.firstName}s event med {thisActivity.Activity.Name} startar om 2 timmar!");
            sw.Append($"{thisActivity.When} vid {thisActivity.Where}.\n");
            sw.Append($"\n");
            sw.Append($"\n");
            sw.Append($"Vi på \"Vad ska vi hitt på\" önskar dig en fortsatt fin dag! ");

            var sub = $"2 timmar kvar till {thisActivity.Activity.Name}!";

            Send(user.User.Email, sub, sw.ToString());
        }

        public static void MinimumMessage(participant user, newactivity thisActivity)
        {
            var sw = new StringBuilder();
            sw.Append($"Hej, {user}! \n");
            sw.Append($"\n");
            sw.Append($"Vi vill bara göra dig uppmärksam om att {thisActivity.User.firstName}s event med {thisActivity.Activity.Name} har nått sitt minimum antal av deltagare, kul!");
            sw.Append($"{thisActivity.When} vid {thisActivity.Where}.\n");
            sw.Append($"\n");
            sw.Append($"\n");
            sw.Append($"Vi på \"Vad ska vi hitt på\" önskar dig en fortsatt fin dag! ");

            var sub = $"Minimum antal deltagar har nåtts på {thisActivity.Activity.Name}!";

            Send(user.User.Email, sub, sw.ToString());
        }

        public static void WantsToJoinYourGroupNotifikation(group _group, planneruser _user)
        {
            var sw = new StringBuilder();
            sw.Append($"Hej, {_group.user.firstName}! \n");
            sw.Append($"\n");
            sw.Append($"Vi vill bara göra dig uppmärksam om att {_user.firstName} vill gå med i din grupp, {_group.groupTitle}!");
            sw.Append($"Klicka på länken för att se vem: https://localhost:7282/Group/GroupHandler !\n");
            sw.Append($"\n");
            sw.Append($"Vi på \"Vad ska vi hitt på\" önskar dig en fortsatt fin dag! ");

            var sub = $"{_user.firstName} vill gå med i {_group.groupTitle}!";
            planneruser User = _group.user;
            Send(_group.user.Email, sub, sw.ToString());
        }

        public static void YouWhereDeniedEntranceToGroup(group _group, planneruser _user)
        {
            var sw = new StringBuilder();
            sw.Append($"Hej, {_user.firstName}! \n");
            sw.Append($"\n");
            sw.Append($"Vi måste tyvärr meddela att du blivit nekad till gruppen, {_group.groupTitle}.");
            sw.Append($"\n");
            sw.Append($"Vi på \"Vad ska vi hitt på\" önskar dig en fortsatt fin dag! ");

            var sub = $"Du har tyvärr blivit nekad till gruppen, {_group.groupTitle}.";
            planneruser User = _group.user;
            Send(_user.Email, sub, sw.ToString());
        }

        public static void YouWhereAcceptedToGroup(group _group, planneruser _user)
        {
            var sw = new StringBuilder();
            sw.Append($"Hej, {_user.firstName}! \n");
            sw.Append($"\n");
            sw.Append($"Vi måste är glada att meddela att du blivit accepterad till gruppen, {_group.groupTitle}!");
            sw.Append($"\n");
            sw.Append($"Vi på \"Vad ska vi hitt på\" önskar dig en fortsatt fin dag! ");

            var sub = $"Du har blivit Accepterad till gruppen, {_group.groupTitle}!";
            planneruser User = _group.user;
            Send(_user.Email, sub, sw.ToString());
        }
    }
}
