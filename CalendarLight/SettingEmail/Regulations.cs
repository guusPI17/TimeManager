using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarLight.SettingEmail
{
    class Regulations
    {
        public string regulationsEmail(string _email)
        {
            string email = _email;
            int count = 0;
            for(int i = 0; i < email.Length; i++)
            {
                if (email[i] == '@')
                {
                    count++;
                }
            }
            if(count == 0 || count > 1)
            {
                return "email@gmail.com";
            }
            string left = string.Empty;
            string right = string.Empty;
            Boolean answer = false;
            for(int i=0;i<email.Length;i++)
            {
                if (email[i] == '@')
                {
                    answer = true;
                    continue;
                }
                if (answer)
                {
                    right += email[i];
                }
                else
                {
                    left += email[i];
                }
            }
            if (left.Length < 1)
            {
                return "email@gmail.com";
            }
            answer = false;
            for(int i = 0; i < left.Length; i++)
            {
                answer = left.ToUpper().StartsWith(Convert.ToString(i));
            }
            if (answer)
            {
                return "email@gmail.com";
            }
            if(right!="gmail.com" && right!="yandex.ru" && right != "mail.ru")
            {
                return "email@gmail.com";
            }
            return email;
        }
    }
}
