namespace CalendarLight.Calendar
{
    public class DataNote
    {
        public string time { get; set; }
        public string thema { get; set; }
        public string text { get; set; }

        public DataNote(string _time, string _thema,string _text)
        {
            time = _time;
            thema = _thema;
            text = _text;
        }
    }
}
