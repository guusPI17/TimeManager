using CalendarLight.Calendar;
using CalendarLight.Logic;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Words.NET;

namespace CalendarLight.WordDoc
{
    class Report
    {
        readonly string SID = string.Empty;
        public Report(string sid)
        {
            SID = sid;
        }
        public void generationReport()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                string fileName = getGoodFileName(saveFileDialog.FileName) + ".docx";
                int countNote = 0;
                var doc = DocX.Create(fileName);
                SerializableNote serializableNote = new SerializableNote(SID);
                AlgFind algFind = new AlgFind();
                for (int year = 2018; year < DateTime.Now.Year + 7; year++) 
                {
                    if (!Directory.Exists(SID + $@"\note\{year.ToString()}"))
                    {
                        continue;
                    }
                    doc.InsertParagraph("Год: " + year.ToString() + "\n");
                    for (int mon = 0; mon < 12; mon++)
                    {
                        string _mon = algFind.findMonForIndex(mon);
                        if (!Directory.Exists(SID + $@"\note\{year.ToString()}\{_mon}"))
                        {
                            continue;
                        }
                        doc.InsertParagraph("Месяц: " + _mon + "\n");
                        for (int day = 0; day < 31; day++)
                        {
                            if (!File.Exists(SID + $@"\note\{year.ToString()}\{_mon}\{day.ToString()}.json"))
                            {
                                continue;
                            }
                            doc.InsertParagraph("Число: " + day.ToString());
                            List<DataNote> dataNotes = serializableNote.deserializable(year.ToString(), _mon, day.ToString());
                            for (int i = 0; i < dataNotes.Count; i++)
                            {
                                doc.InsertParagraph("Время: " + dataNotes[i].time);
                                doc.InsertParagraph("Тема: " + dataNotes[i].thema);
                                doc.InsertParagraph("Заметка:\n" + dataNotes[i].text);
                                doc.InsertParagraph("\n");
                                countNote++;
                            }
                        }
                    }
                }
                doc.InsertParagraph("----------------------------");
                doc.InsertParagraph("Всего было сделано: " + countNote.ToString() + " заметок.");
                doc.Save();
            }

        }
        private string getGoodFileName(string _name)
        {
            string name = _name;
            string ptr = string.Empty;
            for(int i=name.Length-1,j=0;j!=4;i--,j++)
            {
                ptr += name[i];
            }
            var ptrNew = ptr.ToCharArray().Reverse();
            ptr = new string(ptrNew.ToArray());
            if (ptr == "docx")
            {
                return name.Substring(0, name.Length - 5);
            }
            if (ptr == ".doc")
            {
                return name.Substring(0, name.Length - 4);
            }
            return name;
        }
    }
}
