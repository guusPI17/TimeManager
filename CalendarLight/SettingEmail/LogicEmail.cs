using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CalendarLight.SettingEmail
{
    class LogicEmail
    {
        string email = "email@gmail.com";
        MailMessage objectMes;

        public LogicEmail(string _email)
        {
            if (_email != string.Empty || _email.Length != 0)
            {
                email = _email;
            }
            MailAddress from = new MailAddress("guus493@gmail.com", "Time Menedger");
            MailAddress to = new MailAddress(email);
            objectMes = new MailMessage(from, to);
        }
        public LogicEmail() { }
        public void emailInFile()
        {
            if (File.Exists(@"email.txt"))
            {
                File.Delete(@"email.txt");
            }
            using (FileStream fs = new FileStream(@"email.txt", FileMode.OpenOrCreate))
            {
                byte[] array = Encoding.Default.GetBytes(email);
                fs.Write(array, 0, array.Length);
            }

        }
        public string emailFromFile()
        {
            if (File.Exists(@"email.txt"))
            {
                using (FileStream fs = File.OpenRead(@"email.txt"))
                {
                    // преобразуем строку в байты
                    byte[] array = new byte[fs.Length];
                    // считываем данные
                    fs.Read(array, 0, array.Length);
                    // декодируем байты в строку
                    email = Encoding.Default.GetString(array);
                }
            }
            return email;
        }
        public void outputMessage(string thema,string text)
        {
 
            objectMes.Subject = thema;
            objectMes.Body = "<h4>"+text+"</h4>";
            objectMes.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("guus493@gmail.com", "270320000");
            smtp.EnableSsl = true;
            smtp.Send(objectMes);
        }
    }
}
