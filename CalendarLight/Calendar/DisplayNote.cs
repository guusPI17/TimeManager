using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using CalendarLight.Logic;
using CalendarLight.TimeForm;
using System.Windows.Input;
using System.Windows.Controls.Primitives;
using System.Threading;
using System.Windows.Media.Imaging;

namespace CalendarLight.Calendar
{
    public class DisplayNote
    {
        readonly string SID = string.Empty;
        System.Windows.Threading.DispatcherTimer popupTimer;
        Canvas Canvas1;
        Border borderNote;
        string day;
        string mon;
        string year;
        int[] sizeMon;
        List<string> strYear;

        Border borderCalendar; // border календаря при вызовет Note, чтобы вернуться
        Canvas canvasBorder = new Canvas { Width = 280, MinHeight = 345 };
        ScrollViewer scrollViewer = new ScrollViewer { Width = 285, Height = 345};

        double dayLeft;
        double dayTop;

        int indexEditNote = -1;

        Popup popup = new Popup { Placement = PlacementMode.Mouse, MaxWidth = 180, AllowsTransparency = true, IsOpen = false };
        Border borderPopup = new Border { Background = new SolidColorBrush(Color.FromRgb(245, 245, 245)), BorderBrush = Brushes.Black, BorderThickness = new Thickness(1) };

        public DisplayNote(string date, int[] _sizeMon, Border _border, List<string> _strYear, string sid)
        {
            SID = sid;

            Canvas1 = new Canvas();
            Canvas1.Background = Brushes.White;

            borderCalendar = _border;
            borderNote = _border;
            AlgFind algFind = new AlgFind();
            day = algFind.findDayInName(date);
            mon = algFind.findMonInName(date);
            year = algFind.findYearInName(date);
            sizeMon = _sizeMon;
            strYear = _strYear;

            scrollViewer.CanContentScroll = true;
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

            borderNote.Child = Canvas1;
        }

        private void addMarkerNote()
        {
            Button leftNote = new Button { Width = 30, Height = 30 };
            leftNote.BorderThickness = new Thickness(0);
            ImageBrush content = new ImageBrush();
            content.ImageSource = new BitmapImage(new Uri("pack://application:,,,/CalendarLight;component/Resources/icon/leftarrow1_120649.png"));
            leftNote.Background = content;
            Canvas.SetLeft(leftNote, 45);
            Canvas.SetTop(leftNote, 5);
            leftNote.Click += leftNote_Button_Click;
            Canvas1.Children.Add(leftNote);

            Button rightNote = new Button {  Width = 30, Height = 30 };
            rightNote.BorderThickness = new Thickness(0);
            content = new ImageBrush();
            content.ImageSource = new BitmapImage(new Uri("pack://application:,,,/CalendarLight;component/Resources/icon/rightarrow_120622.png"));
            rightNote.Background = content;
            Canvas.SetLeft(rightNote, 225);
            Canvas.SetTop(rightNote, 5);
            rightNote.Click += rightNote_Button_Click;
            Canvas1.Children.Add(rightNote);
        }

        private void leftNote_Button_Click(object sender, RoutedEventArgs e)
        {
            AlgFind algFind = new AlgFind();
            int indexMon = Convert.ToInt32(algFind.findNomerMonInString(mon)) - 1;
            int dayInt = Convert.ToInt32(day);
            int yearInt = Convert.ToInt32(year);

            if (dayInt - 1 < 1)
            {
                if (indexMon - 1 < 0)
                {
                    indexMon = 11;
                    if (yearInt - 1 < Convert.ToInt32(strYear[0]))
                    {
                        yearInt = Convert.ToInt32(strYear[0]);
                        indexMon = 0;
                        dayInt = 1; 
                    }
                    else
                    {
                        yearInt--;
                        sizeMon = algFind.findingYear(yearInt);
                        dayInt = sizeMon[indexMon];
                    }
                }
                else
                {
                    indexMon--;
                    dayInt = sizeMon[indexMon];
                }
            }
            else
            {
                dayInt--;
            }
            day = Convert.ToString(dayInt);
            year = Convert.ToString(yearInt);
            mon = algFind.findMonForIndex(indexMon);
            display();
        }
        private void rightNote_Button_Click(object sender, RoutedEventArgs e)
        {
            AlgFind algFind = new AlgFind();
            int indexMon = Convert.ToInt32(algFind.findNomerMonInString(mon)) - 1;
            int dayInt = Convert.ToInt32(day);
            int yearInt = Convert.ToInt32(year);

            if (dayInt + 1 > sizeMon[indexMon])
            {
                if (indexMon + 1 > 11)
                {
                    indexMon = 0;
                    if (yearInt + 1 > Convert.ToInt32(strYear[strYear.Count-1]))
                    {
                        yearInt = Convert.ToInt32(strYear[strYear.Count - 1]);
                        indexMon = 11;
                        dayInt = sizeMon[indexMon]; 
                    }
                    else
                    {
                        yearInt++;
                        sizeMon = algFind.findingYear(yearInt);
                        dayInt = 1;
                    }
                }
                else
                {
                    indexMon++;
                    dayInt = 1;
                }
            }
            else
            {
                dayInt++;
            }
            day = Convert.ToString(dayInt);
            year = Convert.ToString(yearInt);
            mon = algFind.findMonForIndex(indexMon);
            display();
        }
        private void displayCalendar_Button_Click(object sender, RoutedEventArgs e)
        {
            borderNote.Child = null;
            AlgFind algFind = new AlgFind();
            int nomerMon = Convert.ToInt32(algFind.findNomerMonInString(mon))-1;
            DisplayCalendar displayCalendar = new DisplayCalendar(borderCalendar, Convert.ToInt32(year),nomerMon,sizeMon,SID);

        }
        private void addNote_Button_Click(object sender, RoutedEventArgs e)
        {
            displayNewFormNote();
        }
        private void saveNote_Button_Click(object sender, RoutedEventArgs e)
        {
            List<DataNote> dataNote = new List<DataNote>();
            SerializableNote serializableNote = new SerializableNote(SID);
            dataNote = serializableNote.deserializable(year, mon, day);

            IEnumerable<TextBox> collectionCbTime = Canvas1.Children.OfType<TextBox>().Where(s=>s.Name == "textBoxTime");
            TextBox ptrTime = collectionCbTime.Last();
            IEnumerable<TextBox> collectionTbText = Canvas1.Children.OfType<TextBox>().Where(s => s.Name == "textBoxNote");
            TextBox ptrText = collectionTbText.Last();
            IEnumerable<TextBox> collectionTbThema = Canvas1.Children.OfType<TextBox>().Where(s => s.Name == "textBoxThema");
            TextBox ptrThema = collectionTbThema.Last();

            AlgFind algFind = new AlgFind();
            string goodText = algFind.findGoodTextFromTextBox(ptrText);
            Clock clock = new Clock();
            dataNote.Add(new DataNote(clock.regulationsTime(ptrTime.Text), ptrThema.Text, goodText));


            serializableNote.serializable(dataNote,year,mon,day);

            display();
        }
        private void noSaveNote_Button_Click(object sender, RoutedEventArgs e)
        {
            List<DataNote> dataNote = new List<DataNote>();
            SerializableNote serializableNote = new SerializableNote(SID);
            dataNote = serializableNote.deserializable(year, mon, day);

            display();
        }
        private void editNote_Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            IEnumerable<ScrollViewer> collectionScr = Canvas1.Children.OfType<ScrollViewer>();
            ScrollViewer scrollView = collectionScr.Last();
            Canvas canvas = (Canvas)scrollView.Content;
            List<Border> collectionBorder = canvas.Children.OfType<Border>().ToList();
            for (int i = 0; i < collectionBorder.Count; i++)
            {
                Grid ptrGrid = (Grid)collectionBorder[i].Child;
                List<Canvas> collectionCanvas = ptrGrid.Children.OfType<Canvas>().ToList();
                for (int j = 0; j < collectionCanvas.Count; j++)
                {
                    List<Button> collectionButton = collectionCanvas[j].Children.OfType<Button>().ToList();
                    for (int p = 0; p < collectionButton.Count; p++)
                    {
                        if (btn == collectionButton[p])
                        {
                            Grid grid = ptrGrid;
                            indexEditNote = i;
                            displayEditFormNote();
                            IEnumerable<Label> collectionLb = grid.Children.OfType<Label>();
                            List<Label> lb = collectionLb.ToList();
                            IEnumerable<TextBox> collectionTb = Canvas1.Children.OfType<TextBox>();
                            List<TextBox> tb = collectionTb.ToList();
                            AlgFind algFind = new AlgFind();

                            tb[0].Text = lb[1].Content.ToString();
                            tb[1].Text = lb[3].Content.ToString();
                            tb[2].Text = lb[4].Content.ToString();

                            return;
                        }
                    }
                }
            } 
        }
        private void deliteNote_Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            IEnumerable<ScrollViewer> collectionScr = Canvas1.Children.OfType<ScrollViewer>();
            ScrollViewer scrollView = collectionScr.Last();
            Canvas canvas = (Canvas)scrollView.Content;
            List<Border> collectionBorder = canvas.Children.OfType<Border>().ToList();
            for (int i = 0; i < collectionBorder.Count; i++)
            {
                Grid ptrGrid = (Grid)collectionBorder[i].Child;
                List<Canvas> collectionCanvas = ptrGrid.Children.OfType<Canvas>().ToList();
                for (int j = 0; j < collectionCanvas.Count; j++)
                {
                    List<Button> collectionButton = collectionCanvas[j].Children.OfType<Button>().ToList();
                    for (int p = 0; p < collectionButton.Count; p++)
                    {
                        if (btn == collectionButton[p])
                        {
                            indexEditNote = i;

                            List<DataNote> dataNote = new List<DataNote>();
                            SerializableNote serializableNote = new SerializableNote(SID);
                            dataNote = serializableNote.deserializable(year, mon, day);

                            AlgFind algFind = new AlgFind();
                            string textLable = dataNote[indexEditNote].text;
                            int sizeText = algFind.findSizeText(textLable);
                            canvasBorder.Height -= sizeText * 20 + 10 + 30;

                            dataNote.RemoveAt(indexEditNote);

                            serializableNote.serializable(dataNote, year, mon, day);

                            algFind.presenceSizeNote(dataNote.Count, day, mon, year,SID);
;
                            display();

                            return;
                        }
                    }
                }
            }
        }
        private void saveNoteForEdit_Button_Click(object sender, RoutedEventArgs e)
        {
            List<DataNote> dataNote = new List<DataNote>();
            SerializableNote serializableNote = new SerializableNote(SID);
            dataNote = serializableNote.deserializable(year, mon, day);

            IEnumerable<TextBox> collectionCbTime = Canvas1.Children.OfType<TextBox>().Where(s => s.Name == "textBoxTime");
            TextBox ptrTime = collectionCbTime.Last();
            IEnumerable<TextBox> collectionTbText = Canvas1.Children.OfType<TextBox>().Where(s => s.Name == "textBoxNote");
            TextBox ptrText = collectionTbText.Last();
            IEnumerable<TextBox> collectionTbThema = Canvas1.Children.OfType<TextBox>().Where(s => s.Name == "textBoxThema");
            TextBox ptrThema = collectionTbThema.Last();

            AlgFind algFind = new AlgFind();
            string goodText = algFind.findGoodTextFromTextBox(ptrText);
            goodText = algFind.deleteRubricInString(goodText);
            //
            dataNote[indexEditNote].time = ptrTime.Text;
            dataNote[indexEditNote].thema = ptrThema.Text;
            dataNote[indexEditNote].text = goodText;
            //
            serializableNote.serializable(dataNote, year, mon, day);

            display();
        }
        private void exit_button_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        const int maxLineCount = 5;
        const int maxLenght = 120;
        private void textBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.LineCount > maxLineCount)
            {
                textBox.MaxLength = textBox.Text.Length;
            }
            else
            {
                textBox.MaxLength = maxLenght;
            }
        }
        private void textBoxThema_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            List<Button> collectionBt = Canvas1.Children.OfType<Button>().ToList();
            Button btSave = collectionBt[1];
            btSave.IsEnabled = false;
            if (textBox.Text == string.Empty)
            {
                btSave.IsEnabled = false;
            }
            else
            {
                btSave.IsEnabled = true;
            }
        }
        private void textBoxTime_LostFocus(object sender, RoutedEventArgs e)
        {
            Clock clock = new Clock();
            TextBox textBox = (TextBox)sender;
            textBox.Text = clock.regulationsTime(textBox.Text);
        }
        private void textBoxThema_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == string.Empty)
            {
                textBox.BorderThickness = new Thickness(2);
                textBox.BorderBrush = Brushes.Red;
            }
            else
            {
                textBox.BorderThickness = new Thickness(1);
                textBox.BorderBrush = Brushes.Gray;
            }
        }
        private void textBoxThema_Loaded(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == string.Empty)
            {
                textBox.BorderThickness = new Thickness(2);
                textBox.BorderBrush = Brushes.Red;
            }
            else
            {
                textBox.BorderThickness = new Thickness(1);
                textBox.BorderBrush = Brushes.Gray;
            }
        }


        //счетчик времени
        /*private void displayTime_loaded(object sender, RoutedEventArgs e)
        {
            Label label = (Label)sender;
            AlgFind algFind = new AlgFind();
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.IsEnabled = true;
            timer.Tick += (o, t) => { label.Content = "Время " + algFind.timeWithoutSec(); };
            timer.Start();
        }*/
        private void textBoxPopup_MouseEnter(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            popupTimer = new System.Windows.Threading.DispatcherTimer();
            if (tb.Name == "textBoxTime")
            {
                borderPopup.Child = new Label { Content = "Формат: \"чч:мм\"", Foreground = Brushes.Gray };
                popup.Child = borderPopup;
                popupTimer.Tick += new EventHandler(timerTick);
                popupTimer.Interval = new TimeSpan(0, 0, 1);
                popupTimer.Start();
            }
            else if(tb.Name == "textBoxThema")
            {
                if(tb.BorderBrush == Brushes.Red)
                {
                    borderPopup.Child = new Label { Content = "Заполните поле", Foreground = Brushes.Gray };
                    popup.Child = borderPopup;
                    popupTimer.Tick += new EventHandler(timerTick);
                    popupTimer.Interval = new TimeSpan(0, 0, 1);
                    popupTimer.Start();
                }
            }
        }
        private void timerTick(object sender, EventArgs e)
        {
            popup.IsOpen = true;
            popupTimer.IsEnabled = false;
        }
        private void textBoxPopup_MouseLeave(object sender, RoutedEventArgs e)
        {
            popup.IsOpen = false;
            popupTimer.Stop();
        }
        private void buttonPopup_MouseEnter(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            popupTimer = new System.Windows.Threading.DispatcherTimer();
            if (btn.Name == "btnExit")
            {
                borderPopup.Child = new Label { Content = "Выход", Foreground = Brushes.Gray };
                popup.Child = borderPopup;
                popupTimer.Tick += new EventHandler(timerTick); ;
                popupTimer.Interval = new TimeSpan(0, 0, 1);
                popupTimer.Start();
            }
        }
        private void buttonPopup_MouseLeave(object sender, RoutedEventArgs e)
        {
            popup.IsOpen = false;
            popupTimer.Stop();
        }

        private void displayExit()
        {
            dayLeft = 270;
            dayTop = 2;
            Button btn = new Button { Name = "btnExit", Height = 20, Width = 20, };
            btn.BorderThickness = new Thickness(0);
            ImageBrush content = new ImageBrush();
            content.ImageSource = new BitmapImage(new Uri("pack://application:,,,/CalendarLight;component/Resources/icon/crosscircleregular_106260.png"));
            btn.Background = content;
            btn.MouseEnter += buttonPopup_MouseEnter;
            btn.MouseLeave += buttonPopup_MouseLeave;
            Canvas.SetLeft(btn, dayLeft);
            Canvas.SetTop(btn, dayTop);
            btn.Click += exit_button_Click;
            Canvas1.Children.Add(btn);
        }
        private void displayEditFormNote()
        {
            Canvas1.Children.Clear();
            scrollViewer.Content = null;
            canvasBorder.Children.Clear();

            displayExit();

            dayLeft = 24;
            dayTop = 10;


            Label lbTime = new Label { Content = "Редактирование заметки", Height = 40, Width = 245, FontSize = 16 };
            lbTime.Background = Brushes.White;
            lbTime.FontFamily = new FontFamily("Arial");
            lbTime.FontWeight = FontWeights.DemiBold;
            lbTime.HorizontalContentAlignment = HorizontalAlignment.Center;
            Canvas.SetLeft(lbTime, dayLeft);
            Canvas.SetTop(lbTime, dayTop);
            Canvas1.Children.Add(lbTime);

            dayTop = 40;
            dayLeft = 65;

            AlgFind algFind = new AlgFind();
            string dayPtr = day;
            if (Convert.ToInt32(day) < 10)
            {
                dayPtr = day.Insert(0, "0");
            }
            Label ltDate = new Label { Content = dayPtr + "." + algFind.findNomerMonInString(mon) + "." + year, Height = 40, Width = 294, FontSize = 16, HorizontalContentAlignment = HorizontalAlignment.Center };
            ltDate.FontFamily = new FontFamily("Arial");
            ltDate.FontWeight = FontWeights.DemiBold;
            // Canvas.SetLeft(ltDate, dayLeft);
            Canvas.SetTop(ltDate, dayTop);
            Canvas1.Children.Add(ltDate);

            dayLeft = 10;
            dayTop += 55;
            Label lbInfoTime = new Label { Content = "Время: ", Height = 40, Width = 65, FontSize = 16 };
            lbInfoTime.FontFamily = new FontFamily("Arial");
            Canvas.SetLeft(lbInfoTime, dayLeft);
            Canvas.SetTop(lbInfoTime, dayTop);
            Canvas1.Children.Add(lbInfoTime);

            dayLeft += 65;
            dayTop += 3;

            TextBox textBoxTime = new TextBox { Name = "textBoxTime", MaxLength = 5, Height = 25, Width = 45, VerticalContentAlignment = VerticalAlignment.Center, FontSize = 16 };
            textBoxTime.Text = algFind.timeWithoutSec();
            Canvas.SetLeft(textBoxTime, dayLeft);
            Canvas.SetTop(textBoxTime, dayTop);

            textBoxTime.MouseEnter += textBoxPopup_MouseEnter;
            textBoxTime.MouseLeave += textBoxPopup_MouseLeave;
            textBoxTime.LostFocus += textBoxTime_LostFocus;
            Canvas1.Children.Add(textBoxTime);

            dayLeft = 10;
            dayTop += 50;
            Label lbThema = new Label { Content = "Тема: ", Height = 40, Width = 65, FontSize = 16 };
            lbThema.FontFamily = new FontFamily("Arial");
            Canvas.SetLeft(lbThema, dayLeft);
            Canvas.SetTop(lbThema, dayTop);
            Canvas1.Children.Add(lbThema);


            dayLeft += 65;
            dayTop += 3;
            TextBox textBoxThema = new TextBox { Name = "textBoxThema", Height = 25, Width = 120, MaxLength = 15, VerticalContentAlignment = VerticalAlignment.Center, FontSize = 14 };
            Canvas.SetLeft(textBoxThema, dayLeft);
            Canvas.SetTop(textBoxThema, dayTop);
            textBoxThema.TextChanged += textBoxThema_TextChanged;
            textBoxThema.LostFocus += textBoxThema_LostFocus;
            textBoxThema.Loaded += textBoxThema_Loaded;
            textBoxThema.MouseEnter += textBoxPopup_MouseEnter;
            textBoxThema.MouseLeave += textBoxPopup_MouseLeave;
            Canvas1.Children.Add(textBoxThema);

            dayLeft = 10;
            dayTop += 50;
            Label lbInfoText = new Label { Content = "Заметка: ", Height = 35, Width = 75, FontSize = 16 };
            lbInfoText.FontFamily = new FontFamily("Arial");
            Canvas.SetLeft(lbInfoText, dayLeft);
            Canvas.SetTop(lbInfoText, dayTop);
            Canvas1.Children.Add(lbInfoText);

            dayTop += 30;
            dayLeft += 5;
            TextBox textBoxNote = new TextBox { Name = "textBoxNote", Height = 100, Width = 260, MaxLines = 4, MaxLength = 120, FontSize = 14, TextWrapping = TextWrapping.Wrap, AcceptsReturn = true };
            Canvas.SetLeft(textBoxNote, dayLeft);
            Canvas.SetTop(textBoxNote, dayTop);
            textBoxNote.TextChanged += textBoxBase_OnTextChanged;
            Canvas1.Children.Add(textBoxNote);

            dayTop += 110;
            Button btAdd = new Button { Content = "Сохранить", Height = 40, Width = 85, FontSize = 14 };
            btAdd.FontFamily = new FontFamily("Arial");
            btAdd.FontWeight = FontWeights.Bold;
            btAdd.Background = new SolidColorBrush(Color.FromRgb(245, 245, 245));
            Canvas.SetLeft(btAdd, dayLeft);
            Canvas.SetTop(btAdd, dayTop);
            btAdd.Click += saveNoteForEdit_Button_Click;
            btAdd.IsEnabled = false;
            Canvas1.Children.Add(btAdd);

            dayLeft += 185;
            Button btEnd = new Button { Content = "Отмена", Height = 40, Width = 75, FontSize = 14 };
            btEnd.FontFamily = new FontFamily("Arial");
            btEnd.Background = new SolidColorBrush(Color.FromRgb(245, 245, 245));
            btEnd.FontWeight = FontWeights.Bold;
            Canvas.SetLeft(btEnd, dayLeft);
            Canvas.SetTop(btEnd, dayTop);
            btEnd.Click += noSaveNote_Button_Click;
            Canvas1.Children.Add(btEnd);

        }
        private void displayNewFormNote()
        {

            Canvas1.Children.Clear();
            scrollViewer.Content = null;
            canvasBorder.Children.Clear();

            displayExit();

            dayLeft = 24;
            dayTop = 10;

            Label lbTime = new Label { Content = "Добавление заметки", Height = 40, Width = 240, FontSize = 16 };
            lbTime.Background = Brushes.White;
            lbTime.FontFamily = new FontFamily("Arial");
            lbTime.FontWeight = FontWeights.DemiBold;
            lbTime.HorizontalContentAlignment = HorizontalAlignment.Center;
            Canvas.SetLeft(lbTime, dayLeft);
            Canvas.SetTop(lbTime, dayTop);
            Canvas1.Children.Add(lbTime);

            dayTop = 40;
            dayLeft = 65;

            AlgFind algFind = new AlgFind();
            string dayPtr = day;
            if (Convert.ToInt32(day) < 10)
            {
                dayPtr = day.Insert(0, "0");
            }
            Label ltDate = new Label { Content = dayPtr + "." + algFind.findNomerMonInString(mon) + "." + year, Height = 40, Width = 294, FontSize = 16, HorizontalContentAlignment=HorizontalAlignment.Center };
            ltDate.FontFamily = new FontFamily("Arial");
            ltDate.FontWeight = FontWeights.DemiBold;
            // Canvas.SetLeft(ltDate, dayLeft);
            Canvas.SetTop(ltDate, dayTop);
            Canvas1.Children.Add(ltDate);

            dayLeft = 10;
            dayTop += 55;
            Label lbInfoTime = new Label { Content = "Время: ", Height = 40, Width = 65, FontSize = 16 };
            lbInfoTime.FontFamily = new FontFamily("Arial");
            Canvas.SetLeft(lbInfoTime, dayLeft);
            Canvas.SetTop(lbInfoTime, dayTop);
            Canvas1.Children.Add(lbInfoTime);

            dayLeft += 65;
            dayTop += 3;

            TextBox textBoxTime = new TextBox { Name = "textBoxTime", MaxLength = 5,Height = 25,Width=45,VerticalContentAlignment = VerticalAlignment.Center,FontSize=16 };
            textBoxTime.Text = algFind.timeWithoutSec();
            Canvas.SetLeft(textBoxTime, dayLeft);
            Canvas.SetTop(textBoxTime, dayTop);

            textBoxTime.MouseEnter += textBoxPopup_MouseEnter;
            textBoxTime.MouseLeave += textBoxPopup_MouseLeave;
            textBoxTime.LostFocus += textBoxTime_LostFocus;
            Canvas1.Children.Add(textBoxTime);

            dayLeft = 10;
            dayTop += 50;
            Label lbThema = new Label { Content = "Тема: ", Height = 40, Width = 65, FontSize = 16 };
            lbThema.FontFamily = new FontFamily("Arial");
            Canvas.SetLeft(lbThema, dayLeft);
            Canvas.SetTop(lbThema, dayTop);
            Canvas1.Children.Add(lbThema);


            dayLeft += 65;
            dayTop += 3;
            TextBox textBoxThema = new TextBox { Name = "textBoxThema", Height = 25, Width = 120, MaxLength = 15, VerticalContentAlignment = VerticalAlignment.Center, FontSize = 14 };
            Canvas.SetLeft(textBoxThema, dayLeft);
            Canvas.SetTop(textBoxThema, dayTop);
            textBoxThema.TextChanged += textBoxThema_TextChanged;
            textBoxThema.LostFocus += textBoxThema_LostFocus;
            textBoxThema.Loaded += textBoxThema_Loaded;
            textBoxThema.MouseEnter += textBoxPopup_MouseEnter;
            textBoxThema.MouseLeave += textBoxPopup_MouseLeave;
            Canvas1.Children.Add(textBoxThema);

            dayLeft = 10;
            dayTop += 50;
            Label lbInfoText = new Label { Content = "Заметка: ", Height = 35, Width = 75, FontSize = 16 };
            lbInfoText.FontFamily = new FontFamily("Arial");
            Canvas.SetLeft(lbInfoText, dayLeft);
            Canvas.SetTop(lbInfoText, dayTop);
            Canvas1.Children.Add(lbInfoText);

            dayTop += 30;
            dayLeft += 5;
            TextBox textBoxNote = new TextBox { Name = "textBoxNote", Height = 100, MaxLines = 4, MaxLength = 120, Width = 260,FontSize=14, TextWrapping = TextWrapping.Wrap, AcceptsReturn = true };
            Canvas.SetLeft(textBoxNote, dayLeft);
            Canvas.SetTop(textBoxNote, dayTop);
            textBoxNote.TextChanged += textBoxBase_OnTextChanged;
            Canvas1.Children.Add(textBoxNote);

            dayTop += 110;
            Button btAdd = new Button { Content = "Добавить", Height = 40, Width = 75 , FontSize = 14 };
            btAdd.FontFamily = new FontFamily("Arial");
            btAdd.FontWeight = FontWeights.Bold;
            btAdd.Background = new SolidColorBrush(Color.FromRgb(245, 245, 245));
            Canvas.SetLeft(btAdd, dayLeft);
            Canvas.SetTop(btAdd, dayTop);
            btAdd.Click += saveNote_Button_Click;
            btAdd.IsEnabled = false;
            Canvas1.Children.Add(btAdd);

            dayLeft +=185;
            Button btEnd = new Button { Content = "Отмена", Height = 40, Width = 75, FontSize = 14 };
            btEnd.FontFamily = new FontFamily("Arial");
            btEnd.Background = new SolidColorBrush(Color.FromRgb(245, 245, 245));
            btEnd.FontWeight = FontWeights.Bold;
            Canvas.SetLeft(btEnd, dayLeft);
            Canvas.SetTop(btEnd, dayTop);
            btEnd.Click += noSaveNote_Button_Click;
            Canvas1.Children.Add(btEnd);


        }
        private void displayDate()
        {
            dayLeft = 105;
            dayTop = 5;

            AlgFind algFind = new AlgFind();
            string dayPtr = day;
            if(Convert.ToInt32(day) < 10)
            {
                dayPtr = day.Insert(0, "0");
            }
            Button btDate = new Button { Content = dayPtr + "." + algFind.findNomerMonInString(mon) + "." + year, FontSize = 16, Height = 30, Width = 95,HorizontalContentAlignment = HorizontalAlignment.Center };
            btDate.BorderThickness = new Thickness(1);
            btDate.Padding = new Thickness(5);
            btDate.BorderBrush = Brushes.LightGray;
            btDate.Background = Brushes.White;
            btDate.FontFamily = new FontFamily("Arial");
            btDate.FontWeight = FontWeights.DemiBold;
            Canvas.SetLeft(btDate, dayLeft);
            Canvas.SetTop(btDate, dayTop);
            btDate.Click += displayCalendar_Button_Click;
            Canvas1.Children.Add(btDate);
        }
        private void displayText()
        {
            dayTop = 35;

            AlgFind algFind = new AlgFind();
            List<DataNote> dataNote = new List<DataNote>();
            SerializableNote serializableNote = new SerializableNote(SID);
            dataNote = serializableNote.deserializable(year, mon, day);
            DataNote[] ptr = new DataNote[dataNote.Count];
            dataNote.CopyTo(ptr);
            dataNote.Clear();
            dataNote = algFind.sortTime(ptr);
            algFind.presenceSizeNote(dataNote.Count, day, mon, year,SID);
            if (algFind.presenceNote(day, mon, year,SID))
            {
                serializableNote.serializable(dataNote, year, mon, day);
            }
            Canvas.SetLeft(scrollViewer, 5);
            Canvas.SetTop(scrollViewer, dayTop);
            Canvas.SetTop(canvasBorder, 0);
            dayTop = 5;
            canvasBorder.Height = 50;
            for (int i = 0; i < dataNote.Count; i++)
            {
                dayLeft = 8;
                Label index = new Label { Content = "№" + (i + 1), FontSize = 10 };
                Label thema = new Label { Content = dataNote[i].thema, FontSize = 10, FontWeight = FontWeights.Black };

                Label text = new Label { Content = dataNote[i].text,Width=270, FontSize = 14 };

                Label time = new Label { Content = "Время: ", FontSize = 10 };
                Label timeMy = new Label { Content = dataNote[i].time, FontSize = 10, FontWeight = FontWeights.Black };

                string textLable = text.Content.ToString();
                int sizeText = algFind.findSizeText(textLable);
                canvasBorder.Height += sizeText * 20 + 30 + 15;
                Border border = new Border {Height = sizeText * 20+10+30, Width = 260, BorderBrush = Brushes.Black, BorderThickness = new Thickness(1)};
                border.CornerRadius = new CornerRadius(2);
                Canvas.SetLeft(border, dayLeft);
                Canvas.SetTop(border, dayTop);
                WebElement webElement = new WebElement();

                border = webElement.editBackground(border, dataNote[i].time);

                Grid grid = new Grid { Name = "Note_" + i };

                grid.RowDefinitions.Add(new RowDefinition());
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(sizeText * 20 + 10) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(40) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(40) });
                grid.ColumnDefinitions.Add(new ColumnDefinition {});

                Grid.SetRow(time, 0);
                Grid.SetColumn(time, 2);
                grid.Children.Add(time);

                Grid.SetRow(timeMy, 0);
                Grid.SetColumn(timeMy, 3);
                grid.Children.Add(timeMy);

                Grid.SetRow(index, 0);
                Grid.SetColumn(index, 0);
                grid.Children.Add(index);

                Grid.SetRow(thema, 0);
                Grid.SetColumn(thema, 1);
                grid.Children.Add(thema);

                Grid.SetRow(text, 1);
                Grid.SetColumn(text, 0);
                Grid.SetColumnSpan(text, 5);
                grid.Children.Add(text);

                border.Child = grid;

                dayTop = Canvas.GetTop(border);
                dayTop += sizeText * 20 + 15+30;

                displayAddFuncNote(grid);


                canvasBorder.Children.Add(border);
            }
            scrollViewer.Content = canvasBorder;
            Canvas1.Children.Add(scrollViewer);
        }
        private void displayAddFuncNote(Grid _grid)
        {
            Grid grid = _grid;
            Canvas canvas = new Canvas();
            Button btEdit = new Button { Width = 20, Height = 20 };
            btEdit.BorderThickness = new Thickness(0);
            ImageBrush content = new ImageBrush();
            content.ImageSource = new BitmapImage(new Uri("pack://application:,,,/CalendarLight;component/Resources/icon/pencil_106497.png"));
            btEdit.Background = content;
            btEdit.Click += editNote_Button_Click;
            Canvas.SetTop(btEdit, 2);
            canvas.Children.Add(btEdit);

            dayLeft += 15;
            Button btDel = new Button { Width = 20, Height = 20 };
            btDel.BorderThickness = new Thickness(0);
            content = new ImageBrush();
            content.ImageSource = new BitmapImage(new Uri("pack://application:,,,/CalendarLight;component/Resources/icon/trashbinsymbol_120594.png"));
            btDel.Background = content;
            btDel.Click += deliteNote_Button_Click;
            Canvas.SetTop(btDel, 2);
            Canvas.SetLeft(btDel, dayLeft);
            canvas.Children.Add(btDel);


            Grid.SetRow(canvas, 0);
            Grid.SetColumn(canvas, 4);
            grid.Children.Add(canvas);

        }
        private void displayAddNote()
        {
            dayTop = 5;
            dayLeft = 5;
            IEnumerable<ScrollViewer> collectionScr = Canvas1.Children.OfType<ScrollViewer>();
            ScrollViewer scrollView = collectionScr.Last();
            Canvas canvas = (Canvas)scrollView.Content;
            IEnumerable<Border> collectionBord = canvas.Children.OfType<Border>();
            if (collectionBord.Count() != 0)
            {
                Border border = collectionBord.Last();
                Grid grid = (Grid)border.Child;
                IEnumerable<Label> collectionInGrid = grid.Children.OfType<Label>();
                List<Label> listlbPtr = collectionInGrid.ToList();
                Label lbPtr = listlbPtr[4];
                string textLable = lbPtr.Content.ToString();
                AlgFind algFind = new AlgFind();
                int sizeText = algFind.findSizeText(textLable);
                dayTop = Canvas.GetTop(border);
                dayTop += sizeText * 20+15+35;
            }
            dayLeft = 127;
            Button addNote = new Button { Width = 30, Height = 30 };
            addNote.BorderThickness = new Thickness(0);
            ImageBrush content = new ImageBrush();
            content.ImageSource = new BitmapImage(new Uri("pack://application:,,,/CalendarLight;component/Resources/icon/plus-black-symbol_icon-icons.com_73453.png"));
            addNote.Background = content;
            Canvas.SetLeft(addNote, dayLeft);
            Canvas.SetTop(addNote, dayTop);
            addNote.Click += addNote_Button_Click;
            canvasBorder.Children.Add(addNote);
        }
        public Border display()
        {
            Canvas1.Children.Clear();
            scrollViewer.Content = null;
            canvasBorder.Children.Clear();

            displayExit();
            addMarkerNote(); // стрелки
            displayDate(); // дату
            displayText(); // текс
            displayAddNote();
           // scrollViewer.Content = Canvas1;
            return borderNote;
        }

    }
}
