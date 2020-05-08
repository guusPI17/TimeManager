using CalendarLight.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarLight.TimeForm
{
    public class Clock
    {
        public string regulationsTime(string _time)
        {
            AlgFind algFind = new AlgFind();
            string newtime = _time;
            if(newtime.Length > 5 || newtime.Length < 5) // длина
            {
                return algFind.timeWithoutSec();
            }
            if (newtime[2] != ':') // на наличие ':' под индексом 2
            {
                return algFind.timeWithoutSec();
            }
            for (int i = 0; i < newtime.Length; i++) // все кроме цифр и :
            {
                if(newtime[i] < '0' || newtime[i] > '9' && newtime[i]!=':')
                {
                    return algFind.timeWithoutSec();
                }
            }
            string ptr;
            ptr = newtime.Substring(0, 2); // первые две цифры
            if(Convert.ToInt32(ptr)>23 || Convert.ToInt32(ptr) < 0)
            {
                return algFind.timeWithoutSec();
            }
            ptr = newtime.Substring(3, 2); // вторые последние две цифры
            if (Convert.ToInt32(ptr) > 59 || Convert.ToInt32(ptr) < 0)
            {
                return algFind.timeWithoutSec();
            }
            return newtime;
        }
    }
}
