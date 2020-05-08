using CalendarLight.Calendar;
using CalendarLight.Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;

namespace CalendarLight.SettingEmail
{
    class CheckMessage
    {
        readonly string SID = string.Empty;
        public CheckMessage(string sid)
        {
            SID = sid;
            Thread myThread = new Thread(new ThreadStart(checkForEmail));
            myThread.Start(); 
        }
        private void checkForEmail()
        {
            while (true)
            {
                AlgFind algFind = new AlgFind();
                string year = Convert.ToString(DateTime.Now.Year);
                string mon = algFind.findMonForIndex(DateTime.Now.Month - 1);
                string day = Convert.ToString(DateTime.Now.Day);
                string time = algFind.timeWithoutSec();
                string filePATH = checkFile(year,mon,day);
                if(filePATH == null)
                {
                    Thread.Sleep(60000);
                    continue;
                }
                SerializableNote serializableNote = new SerializableNote(SID);
                List<DataNote> dataNote = serializableNote.deserializable(year, mon, day);
                for(int i = 0; i < dataNote.Count; i++)
                {
                    if(dataNote[i].time == time)
                    {
                        string email = string.Empty;
                        using (FileStream fs = File.OpenRead(@"email.txt"))
                        {
                            // преобразуем строку в байты
                            byte[] array = new byte[fs.Length];
                            // считываем данные
                            fs.Read(array, 0, array.Length);
                            // декодируем байты в строку
                            email = Encoding.Default.GetString(array);
                        }

                        LogicEmail logicEmail = new LogicEmail(email);
                        logicEmail.outputMessage(dataNote[i].thema+"["+time+"]", dataNote[i].text);
                    }
                }
                Thread.Sleep(60000);
            }
        }
        private string checkFile(string _year,string _mon,string _day)
        {
            string year = _year;
            string mon = _mon;
            string day = _day;
            if (!Directory.Exists(SID))
            {
                return null;
            }
            if (!Directory.Exists(SID + @"\note"))
            {
                return null;
            }
            if (!Directory.Exists(SID + $@"\note\{year}"))
            {
                return null;
            }
            if (!Directory.Exists(SID + $@"\note\{year}\{mon}"))
            {
                return null;
            }
            if (!File.Exists(SID + $@"\note\{year}\{mon}\{day}.json"))
            {
                return null;
            }
            if (!File.Exists(@"email.txt"))
            {
                return null;
            }
            return SID + $@"\note\{year}\{mon}\{day}.json";

        }
    }
}
