using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CalendarLight.Calendar;
using Newtonsoft.Json;

namespace CalendarLight.Logic
{
    public class SerializableNote
    {
        string filePATH = string.Empty;
        readonly string SID = string.Empty;

        public SerializableNote(string sid)
        {
            SID = sid;
            filePATH = SID + @"\note\";
        }
        public void serializable(List<DataNote> _dataNotes,string year,string mon,string day)
        {
            List<DataNote> dataNotes = _dataNotes;
            string serialized = JsonConvert.SerializeObject(dataNotes);

            if (!Directory.Exists(SID))
            {
                Directory.CreateDirectory(SID);
            }
            if (!Directory.Exists(SID + @"\note"))
            {
                Directory.CreateDirectory(SID + @"\note");
            }
            if (!Directory.Exists(SID + $@"\note\{year}"))
            {
                Directory.CreateDirectory(SID + $@"\note\{year}");
            }
            if (!Directory.Exists(SID + $@"\note\{year}\{mon}"))
            {
                Directory.CreateDirectory(SID + $@"\note\{year}\{mon}");
            }
            filePATH = SID + $@"\note\{year}\{mon}\{day}.json";
            if (File.Exists(filePATH))
            {
                File.Delete(filePATH);
            }
            using (FileStream fs = new FileStream(filePATH, FileMode.OpenOrCreate))
            {
                byte[] array = Encoding.Default.GetBytes(serialized);
                fs.Write(array, 0, array.Length);
            }
        }

        public List<DataNote> deserializable(string year, string mon, string day)
        {
            List<DataNote> dataNoteZero = new List<DataNote>();

            if (!Directory.Exists(SID))
            {
                return dataNoteZero;
            }
            if (!Directory.Exists(SID + @"\note"))
            {
                return dataNoteZero;
            }
            if (!Directory.Exists(SID + $@"\note\{year}"))
            {
                return dataNoteZero;
            }
            if (!Directory.Exists(SID + $@"\note\{year}\{mon}"))
            {
                return dataNoteZero;
            }
            if (!File.Exists(SID + $@"\note\{year}\{mon}\{day}.json"))
            {
                return dataNoteZero;
            }
            List<DataNote> dataNote;
            filePATH = SID + $@"\note\{year}\{mon}\{day}.json";
            try
            {
                using (FileStream fs = File.OpenRead(filePATH))
                {
                    // преобразуем строку в байты
                    byte[] array = new byte[fs.Length];
                    // считываем данные
                    fs.Read(array, 0, array.Length);
                    // декодируем байты в строку
                    string textFromFile = Encoding.Default.GetString(array);
                    dataNote = JsonConvert.DeserializeObject<List<DataNote>>(textFromFile);
                    //newDataNote.Add(dataNote);
                }
                return dataNote;
            }
            catch
            {
                MessageBox.Show($@"Ошибка при считывание заметок в '{year}\{mon}\{day}'. Считывание отменено. Файл будет удален после нажатия на ОК! ", "Ошибка считывания данных", MessageBoxButton.OK, MessageBoxImage.Error);
                return dataNote = new List<DataNote>();
            }
        }

        public List<DataNote> deserializable(Uri fileName)
        {
            List<DataNote> dataNoteZero = new List<DataNote>();
            List<DataNote> dataNote;
            Stream stream = Application.GetResourceStream(fileName).Stream;

            using (StreamReader sr = new StreamReader(stream, Encoding.GetEncoding(1251)))
            {
                dataNote = JsonConvert.DeserializeObject<List<DataNote>>(sr.ReadLine());
            }
            return dataNote;
        }
    }
}
