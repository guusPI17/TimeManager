using CalendarLight.Calendar;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CalendarLight.Logic
{
    public class AlgFind
    {
        public int[] findingYear(int _nowYear)
        {
            int nowYear = _nowYear;
            if ((nowYear % 4 == 0 && nowYear % 100 != 0) || nowYear % 400 == 0)
            {
                int[] ptrSizeMon = { 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
                return ptrSizeMon;
            }
            else
            {
                int[] ptrSizeMon = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
                return ptrSizeMon;
            }
        }
        public int findDayWeek(int day, int mon, int year)
        {
            int leftDay = 0;
            if (mon <= 2)
            {
                year--;
                day += 3;
            }
            leftDay = 1 + (day + year + (year / 4) - (year / 100) + (year / 400) + ((31 * mon + 10) / 12)) % 7;
            leftDay--;
            leftDay *= 40;
            return leftDay+5;
        }
        public string findYearInName(string _name)
        {
            int ptr = 0;
            string str = string.Empty;
            for (int i = 0; i < _name.Length; i++)
            {
                if (ptr == 3)
                {
                    str += _name[i];
                    if (i + 1 > _name.Length - 1)
                    {
                        break;
                    }
                }
                if (_name[i] == '_' && ptr < 3)
                {
                    ptr++;
                }
            }
            return str;
        }
        public string findMonInName(string _name)
        {
            int ptr = 0;
            string str = string.Empty;
            for (int i = 0; i < _name.Length; i++)
            {
                if (ptr == 2)
                {
                    str += _name[i];
                    if (_name[i + 1] == '_')
                    {
                        break;
                    }
                }
                if (_name[i] == '_' && ptr < 2)
                {
                    ptr++;
                }
            }
            return str;
        }
        public string findDayInName(string _name)
        {
            int ptr = 0;
            string str = string.Empty;
            for (int i = 0; i < _name.Length; i++)
            {
                if (ptr == 1)
                {
                    str += _name[i];
                    if (_name[i + 1] == '_')
                    {
                        break;
                    }
                }
                if (_name[i] == '_' && ptr < 1)
                {
                    ptr++;
                }
            }
            return str;
        }
        public string findNomerMonInString(string _mon)
        {
            string[] strMon = { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };
            for(int i = 0; i < strMon.Length; i++)
            {
                if (_mon == strMon[i])
                {
                    if (i < 9)
                    {
                        string ptr = "0" + Convert.ToString(i+1);
                        return ptr;
                    }
                    else
                    {
                        return Convert.ToString(i+1);
                    }
                }
            }
            return Convert.ToString(0);
        }
        public string findMonForIndex(int index)
        {
            string[] strMon = { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };
            return strMon[index];
        }
        public int findSizeText(string _text)
        {
            int count = 0;
            for(int i = 0; i < _text.Length; i++)
            {
                if(_text[i] == '\n')
                {
                    count++;
                }
            }
            return count;
        }
        public string findGoodTextFromTextBox(TextBox textBox)
        {
            StringCollection lines = new StringCollection();
            int lineCount = textBox.LineCount;

            for (int line = 0; line < lineCount; line++)
            {
                lines.Add(textBox.GetLineText(line));
            }
            string[] ptr = new string[lines.Count];
            string retText = string.Empty;
            lines.CopyTo(ptr, 0);
            for (int i = 0; i < ptr.Length; i++)
            {

                int count = -1;
                count = ptr[i].IndexOf("\n");
                if (count == -1)
                {
                    retText += ptr[i] + '\n';
                }
                else
                {
                    retText += ptr[i];
                }
            }
            return retText;
        }
        public string deleteRubricInString(string _str)
        {
            int index = -1;
            for (int i = 0; i < _str.Length; i++)
            {
                if (_str[i] != '\n' && _str[i]!='\r')
                {
                    index = i;
                }
            }
            string str = _str.Substring(0, index + 1);
            str += '\n';
            return str;
        }
        public Boolean presenceNote(string day,string mon,string year,string SID)
        {
            string filePATH = SID + $@"\note\{year}\{mon}\{day}.json";
            if (File.Exists(filePATH))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void presenceSizeNote(int _sizeArray, string day, string mon, string year,string SID)
        {
            string filePATH = SID + $@"\note\{year}\{mon}\{day}.json";
            if (_sizeArray == 0)
            {
                if (File.Exists(filePATH))
                {
                    File.Delete(filePATH);
                }
            }
        }
        public List<DataNote> sortTime(DataNote[] _dataNote)
        {
            DataNote[] dataNote = _dataNote;
            for(int i=0;i< dataNote.Count(); i++)
            {
                dataNote[i].thema = dataNote[i].thema;
            }
            var result = from user in dataNote
                         orderby user.time
                         select user;
            List<DataNote> ptr = new List<DataNote>();

            foreach (DataNote u in result)
            {
                ptr.Add(u);
            }

            for(int i = 0; i < ptr.Count; i++)
            {
                ptr[i].thema = ptr[i].thema;
                ptr[i].time = ptr[i].time;
            }
            return ptr;
        }
        public string[] divideTime(string _time)
        {
            string[] newTime = new string[2];
            string time = _time;
            newTime[0] = time.Substring(0, 2);
            newTime[1] = time.Substring(3, 2);
            return newTime;
        }
        public string timeWithoutSec()
        {
            TimeSpan timeComp = DateTime.Now.TimeOfDay;
            string timeCompSt = Convert.ToString(timeComp);
            timeCompSt = timeCompSt.Substring(0, 5);
            return timeCompSt;
        }
    }
}
