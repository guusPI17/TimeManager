using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using CalendarLight.Logic;
using CalendarLight.SettingEmail;
using System.Windows.Media.Imaging;
using CalendarLight.WordDoc;
using System.Windows.Controls.Primitives;
using System.Threading;

namespace CalendarLight.Calendar
{
    public class DisplayCalendar
    {
        readonly string SID = string.Empty;
        System.Windows.Threading.DispatcherTimer popupTimer;
        int nomerYear;
        int nomerMon;
        int[] sizeMon;
        List<string> strYear;
        Canvas Canvas1;

        Popup popup = new Popup { Placement = PlacementMode.Mouse, MaxWidth = 250, AllowsTransparency = true, IsOpen = false };
        Border borderPopup = new Border { Background = new SolidColorBrush(Color.FromRgb(245, 245, 245)), BorderBrush = Brushes.Black, BorderThickness = new Thickness(1) };

        Border mainBorder;
        Border downPanel;
        Canvas canvasPanel;
        public DisplayCalendar(string sid)
        {
            SID = sid;
            AlgFind algFind = new AlgFind();
            sizeMon = algFind.findingYear(DateTime.Now.Year);
            nomerMon = Convert.ToInt32(DateTime.Now.Month) - 1;

            Canvas1 = new Canvas();
            Canvas1.Background = Brushes.White;

            mainBorder = new Border { Height = 405, Width = 294, CornerRadius = new CornerRadius(2), BorderThickness = new Thickness(1), BorderBrush = Brushes.Black };
            strYear = new List<string>();
            for (int i = 2018; i < DateTime.Now.Year + 7; i++)
            {
                strYear.Add(i.ToString());
                if (i.ToString() == DateTime.Now.Year.ToString())
                {
                    nomerYear = strYear.Count - 1;
                }
            }
            CheckMessage checkMessage = new CheckMessage(SID);
        }
        public DisplayCalendar(Border _mainBorder, int _nomerYear, int _nomerMon, int[] _sizeMon,string sid)
        {
            SID = sid;

            Canvas1 = new Canvas();
            Canvas1.Background = Brushes.White;
            strYear = new List<string>();
            for (int i = 2018; i < Convert.ToInt32(DateTime.Now.Year) + 7; i++)
            {
                strYear.Add(i.ToString());
                if (i == _nomerYear)
                {
                    nomerYear = strYear.Count - 1;
                }
            }
            mainBorder = null;
            mainBorder = _mainBorder;
            nomerMon = _nomerMon;
            sizeMon = _sizeMon;
            display();
        }
        // добавление маркров перелистывания
        private void addMarkerMon()
        {
            Button leftMon = new Button { Width = 30, Height = 30 };
            leftMon.BorderThickness = new Thickness(0);
            ImageBrush content = new ImageBrush();
            content.ImageSource = new BitmapImage(new Uri("pack://application:,,,/CalendarLight;component/Resources/icon/leftarrow1_120649.png"));
            leftMon.Background = content;
            Canvas.SetLeft(leftMon, 45); // 10
            Canvas.SetTop(leftMon, 5);
            leftMon.Click += leftMon_Button_Click;
            Canvas1.Children.Add(leftMon);

            Button rightMon = new Button { Width = 30, Height = 30 };
            rightMon.BorderThickness = new Thickness(0);
            content = new ImageBrush();
            content.ImageSource = new BitmapImage(new Uri("pack://application:,,,/CalendarLight;component/Resources/icon/rightarrow_120622.png"));
            rightMon.Background = content;
            Canvas.SetLeft(rightMon, 225); // 250
            Canvas.SetTop(rightMon, 5);
            rightMon.Click += rightMon_Button_Click;
            Canvas1.Children.Add(rightMon);
        }
        private void addMarkerYear()
        {
            Button leftYear = new Button { Width = 30, Height = 30 };
            leftYear.BorderThickness = new Thickness(0);
            ImageBrush content = new ImageBrush();
            content.ImageSource = new BitmapImage(new Uri("pack://application:,,,/CalendarLight;component/Resources/icon/leftarrow1_120649.png"));
            leftYear.Background = content;
            Canvas.SetLeft(leftYear, 45); // 10
            Canvas.SetTop(leftYear, 5);
            leftYear.Click += leftYear_Button_Click;
            Canvas1.Children.Add(leftYear);

            Button rightYear = new Button { Width = 30, Height = 30 };
            rightYear.BorderThickness = new Thickness(0);
            content = new ImageBrush();
            content.ImageSource = new BitmapImage(new Uri("pack://application:,,,/CalendarLight;component/Resources/icon/rightarrow_120622.png"));
            rightYear.Background = content;
            Canvas.SetLeft(rightYear, 225); // 250
            Canvas.SetTop(rightYear, 5);
            rightYear.Click += rightYear_Button_Click;
            Canvas1.Children.Add(rightYear);
        }

        // события
        private void rightYear_Button_Click(object sender, RoutedEventArgs e)
        {
            if (nomerYear + 1 > strYear.Count - 1)
            {
                nomerYear = strYear.Count - 1;
            }
            else
            {
                nomerYear++;
            }
            Canvas1.Children.Clear();
            addMarkerYear();
            displayCoolectionMon();
        }
        private void leftYear_Button_Click(object sender, RoutedEventArgs e)
        {
            if (nomerYear - 1 < 0)
            {
                nomerYear = 0;
            }
            else
            {
                nomerYear--;
            }
            Canvas1.Children.Clear();
            addMarkerYear();
            displayCoolectionMon();
        }
        private void leftMon_Button_Click(object sender, RoutedEventArgs e)
        {
            AlgFind algFind = new AlgFind();
            Canvas1.Children.Clear();
            if (nomerMon - 1 < 0)
            {
                nomerMon = 11;
                if (nomerYear - 1 < 0)
                {
                    nomerYear = 0;
                    nomerMon = 0;
                }
                else
                {
                    nomerYear--;
                }
                sizeMon = algFind.findingYear(Convert.ToInt32(strYear[nomerYear]));
            }
            else
            {
                nomerMon--;
            }
            display();
        }
        private void rightMon_Button_Click(object sender, RoutedEventArgs e)
        {
            AlgFind algFind = new AlgFind();
            Canvas1.Children.Clear();
            if (nomerMon + 1 > 11)
            {
                nomerMon = 0;
                if (nomerYear + 1 > strYear.Count - 1)
                {
                    nomerYear = strYear.Count - 1;
                    nomerMon = 11;
                }
                else
                {
                    nomerYear++;
                }
                sizeMon = algFind.findingYear(Convert.ToInt32(strYear[nomerYear]));
            }
            else
            {
                nomerMon++;
            }
            display();
        }
        private void day_Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Canvas1.Children.Clear();
            AlgFind algFind = new AlgFind();
            DisplayNote note = new DisplayNote(btn.Name.ToString(), sizeMon, mainBorder, strYear,SID);
            mainBorder = note.display();
        }
        private void collectionMon_button_Click(object sender, RoutedEventArgs e)
        {
            Canvas1.Children.Clear();
            addMarkerYear();
            displayCoolectionMon();
        }
        private void mon_Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Canvas canvas = (Canvas)btn.Content;
            IEnumerable<Label> collectionLbName = canvas.Children.OfType<Label>().Where(s => s.Name == "lbnameMon");
            Label label = collectionLbName.Last();
            AlgFind algFind = new AlgFind();

            for (int i = 0; i < 12; i++)
            {
                if (algFind.findMonForIndex(i) == label.Content.ToString())
                {
                    nomerMon = i;
                    break;
                }
            }
            Canvas1.Children.Clear();
            display();
        }

        string email = string.Empty;
        private void textBoxEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            Regulations regulations = new Regulations();
            email = regulations.regulationsEmail(textBox.Text);
            textBox.Text = regulations.regulationsEmail(textBox.Text);
            if (textBox.Text == "email@gmail.com")
            {
                textBox.BorderThickness = new Thickness(2);
                textBox.BorderBrush = Brushes.Red;
            }
            else
            {
                textBox.BorderThickness = new Thickness(0);
                textBox.BorderBrush = Brushes.White;
            }
        }
        private void mon_Button_MouseEnter(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Canvas canvas = (Canvas)btn.Content;
            IEnumerable<Label> collectionLbName = canvas.Children.OfType<Label>().Where(s => s.Name == "lbnameMon");
            Label label = collectionLbName.Last();
            label.Foreground = Brushes.Transparent;
        }
        private void mon_Button_MouseLeave(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Canvas canvas = (Canvas)btn.Content;
            IEnumerable<Label> collectionLbName = canvas.Children.OfType<Label>().Where(s => s.Name == "lbnameMon");
            Label label = collectionLbName.Last();
            label.Foreground = Brushes.Black;
        }
        private void buttonPopup_MouseEnter(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            popupTimer = new System.Windows.Threading.DispatcherTimer();
            if (btn.Name == "btnSettingEmail")
            {
                borderPopup.Child = new Label { Content = "Настройка уведомления", Foreground = Brushes.Gray };
                popup.Child = borderPopup;
                popupTimer.Tick += new EventHandler(timerTick);
                popupTimer.Interval = new TimeSpan(0, 0, 1);
                popupTimer.Start();
            }
            else if(btn.Name == "btnReport")
            {
                borderPopup.Child = new Label { Content = "Экспорт заметок", Foreground = Brushes.Gray };
                popup.Child = borderPopup;
                popupTimer.Tick += new EventHandler(timerTick);
                popupTimer.Interval = new TimeSpan(0, 0, 1);
                popupTimer.Start();
            }
            else if(btn.Name == "btnExit")
            {
                borderPopup.Child = new Label { Content = "Выход", Foreground = Brushes.Gray };
                popup.Child = borderPopup;
                popupTimer.Tick += new EventHandler(timerTick);;
                popupTimer.Interval = new TimeSpan(0, 0, 1);
                popupTimer.Start();
            }
            else if(btn.Name == "btnInfo")
            {
                borderPopup.Child = new Label { Content = "Информация о программе", Foreground = Brushes.Gray };
                popup.Child = borderPopup;
                popupTimer.Tick += new EventHandler(timerTick); ;
                popupTimer.Interval = new TimeSpan(0, 0, 1);
                popupTimer.Start();
            }
            else if(btn.Name == "btnToday")
            {
                borderPopup.Child = new Label { Content = "Сегоднешнее число", Foreground = Brushes.Gray };
                popup.Child = borderPopup;
                popupTimer.Tick += new EventHandler(timerTick); ;
                popupTimer.Interval = new TimeSpan(0, 0, 1);
                popupTimer.Start();
            }
        }
        private void timerTick(object sender, EventArgs e)
        {
            popup.IsOpen = true;
            popupTimer.IsEnabled = false; 
        }
        private void buttonPopup_MouseLeave(object sender, RoutedEventArgs e)
        {
             popup.IsOpen = false;
             popupTimer.Stop();
        }
        private void textBoxPopup_MouseEnter(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            popupTimer = new System.Windows.Threading.DispatcherTimer();
            if (tb.Name == "tbEmail")
            {
                if(tb.BorderBrush == Brushes.Red)
                {
                    borderPopup.Child = new Label { Content = "Неверный формат", Foreground = Brushes.Gray };
                    popup.Child = borderPopup;
                    popupTimer.Tick += new EventHandler(timerTick);
                    popupTimer.Interval = new TimeSpan(0, 0, 1);
                    popupTimer.Start();
                }
            }
        }
        private void textBoxPopup_MouseLeave(object sender, RoutedEventArgs e)
        {
            popup.IsOpen = false;
            popupTimer.Stop();
        }

        private bool IsToggle;
        private void settingEmail_button_Click(object sender, RoutedEventArgs e)
        {
            downPanel.Height = 0;
            DoubleAnimation da = new DoubleAnimation();
            if (!IsToggle)
            {
                da.To = 33;
                da.Duration = TimeSpan.FromSeconds(0.8);
                downPanel.BeginAnimation(Border.HeightProperty, da);
                displayButSet();

                IEnumerable<Button> collectionBt;
                Button bt;
                IEnumerable<Label> collectionLb;
                Label lb;
                IEnumerable<TextBox> collectionTb;
                TextBox tb;

                da.Duration = TimeSpan.FromSeconds(1);
                collectionBt = canvasPanel.Children.OfType<Button>().Where(s => s.Name == "btEmail");
                bt = collectionBt.Last();
                bt.Height = 0;
                bt.BeginAnimation(Border.HeightProperty, da);

                da.Duration = TimeSpan.FromSeconds(0.8);
                collectionLb = canvasPanel.Children.OfType<Label>().Where(s => s.Name == "lbEmail");
                lb = collectionLb.Last();
                lb.Height = 0;
                lb.BeginAnimation(Border.HeightProperty, da);

                da.Duration = TimeSpan.FromSeconds(1);
                collectionTb = canvasPanel.Children.OfType<TextBox>().Where(s => s.Name == "tbEmail");
                tb = collectionTb.Last();
                tb.Height = 0;
                tb.BeginAnimation(Border.HeightProperty, da);
                
                IsToggle = true;
            }
            else
            {
                da.To = 0;
                da.Duration = TimeSpan.FromSeconds(1);
                downPanel.BeginAnimation(Border.HeightProperty, da);

                IEnumerable<Button> collectionBt;
                Button bt;
                IEnumerable<Label> collectionLb;
                Label lb;
                IEnumerable<TextBox> collectionTb;
                TextBox tb;

                da.Duration = TimeSpan.FromSeconds(0.8);
                collectionBt = canvasPanel.Children.OfType<Button>().Where(s => s.Name == "btEmail");
                bt = collectionBt.Last();
                bt.BeginAnimation(Border.HeightProperty, da);

                da.Duration = TimeSpan.FromSeconds(1);
                collectionLb = canvasPanel.Children.OfType<Label>().Where(s => s.Name == "lbEmail");
                lb = collectionLb.Last();
                lb.BeginAnimation(Border.HeightProperty, da);

                da.Duration = TimeSpan.FromSeconds(0.8);
                collectionTb = canvasPanel.Children.OfType<TextBox>().Where(s => s.Name == "tbEmail");
                tb = collectionTb.Last();
                tb.BeginAnimation(Border.HeightProperty, da);

                IsToggle = false;

            }
        }
        private void exit_button_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
        private void report_button_Click(object sender, RoutedEventArgs e)
        {
            Report report = new Report(SID);
            report.generationReport();
        }
        private void editEmail_button_click(object sender,RoutedEventArgs e)
        {
            LogicEmail logicEmail = new LogicEmail(email);
            logicEmail.emailInFile();

            if (email != "email@gmail.com")
            {
                downPanel.Height = 0;
                DoubleAnimation da = new DoubleAnimation();
                da.To = 0;
                da.Duration = TimeSpan.FromSeconds(1);
                downPanel.BeginAnimation(Border.HeightProperty, da);

                IEnumerable<Button> collectionBt;
                Button bt;
                IEnumerable<Label> collectionLb;
                Label lb;
                IEnumerable<TextBox> collectionTb;
                TextBox tb;

                da.Duration = TimeSpan.FromSeconds(0.8);
                collectionBt = canvasPanel.Children.OfType<Button>().Where(s => s.Name == "btEmail");
                bt = collectionBt.Last();
                bt.BeginAnimation(Border.HeightProperty, da);

                da.Duration = TimeSpan.FromSeconds(1);
                collectionLb = canvasPanel.Children.OfType<Label>().Where(s => s.Name == "lbEmail");
                lb = collectionLb.Last();
                lb.BeginAnimation(Border.HeightProperty, da);

                da.Duration = TimeSpan.FromSeconds(0.8);
                collectionTb = canvasPanel.Children.OfType<TextBox>().Where(s => s.Name == "tbEmail");
                tb = collectionTb.Last();
                tb.BeginAnimation(Border.HeightProperty, da);

                IsToggle = false;
            }
        }
        private void info_button_Click(object sender, RoutedEventArgs e)
        {
            AlgFind algFind = new AlgFind();
            DisplayInfo displayInfo = new DisplayInfo(Convert.ToInt32(strYear[nomerYear]), nomerMon, sizeMon, mainBorder,SID);
            mainBorder = displayInfo.display();
        }
        private void today_button_Click(object sender, RoutedEventArgs e)
        {
            AlgFind algFind = new AlgFind();
            sizeMon = algFind.findingYear(DateTime.Now.Year);
            string date = "B_" + DateTime.Now.Day + '_' + algFind.findMonForIndex(DateTime.Now.Month-1) + '_' + DateTime.Now.Year;
            Canvas1.Children.Clear();
            DisplayNote note = new DisplayNote(date, sizeMon, mainBorder, strYear,SID);
            mainBorder = note.display();
        }

        // вывод элементов
        private void displayMon(int dayLeft)
        {
            StackPanel stackPanel = new StackPanel { Width = 150};
            Canvas.SetLeft(stackPanel, dayLeft);
            Canvas.SetTop(stackPanel, 5);
            AlgFind algFind = new AlgFind();
            string ptrMonYear = algFind.findMonForIndex(nomerMon) + " " + strYear[nomerYear] + " год";
            Button btnMon = new Button { Content = ptrMonYear, Height = 40, FontSize = 15 };
            btnMon.Background = Brushes.White;
            btnMon.BorderThickness = new Thickness(1);
            btnMon.Padding = new Thickness(5);
            btnMon.BorderBrush = Brushes.LightGray;
            btnMon.FontFamily = new FontFamily("Arial");
            btnMon.FontWeight = FontWeights.DemiBold;
            btnMon.HorizontalContentAlignment = HorizontalAlignment.Center;
            btnMon.HorizontalAlignment = HorizontalAlignment.Center;
            btnMon.Click += collectionMon_button_Click;
            stackPanel.Children.Add(btnMon);
            Canvas1.Children.Add(stackPanel);
        }
        private void displayDayWerk(int dayLeft, int dayTop)
        {
            string[] str = { "Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Вс" };
            Label[] lbs = new Label[7];
            for (int i = 0; i < lbs.Length; i++, dayLeft += 40)
            {
                var lb = new Label
                {
                    Content = str[i],
                    Width = 40,
                    Height = 40,
                    FontWeight = FontWeights.Black
                };
                Canvas.SetLeft(lb, dayLeft);
                Canvas.SetTop(lb, dayTop);
                Canvas1.Children.Add(lb);
            }
        }
        private int[] displayMainMon(int dayLeft)
        {
            AlgFind algFind = new AlgFind();
            WebElement webElement = new WebElement();
            Button[] btns = new Button[sizeMon[nomerMon]];
            int[] arrTopLeft = new int[2];
            int dayTop = 0;
            if (dayLeft == 0) // если понедельник первое число, то надо вниз на одну строчку отпустить
            {
                dayTop = 125;
            }
            else
            {
                dayTop = 85;
            }
            for (int i = 0, j = (algFind.findDayWeek(1, nomerMon + 1, Convert.ToInt32(strYear[nomerYear]))) / 40; i < btns.Length; i++, j++)
            {
                dayLeft = algFind.findDayWeek(i + 1, nomerMon + 1, Convert.ToInt32(strYear[nomerYear]));
                if (j == 7)
                {
                    dayTop += 40;
                    j = 0;
                }
                var btn = new Button
                {
                    Content = i + 1,
                    Width = 40,
                    Height = 40,
                    FontWeight = FontWeights.DemiBold,
                    Background = Brushes.White,
                    BorderThickness = new Thickness(0)
                };
                btn.Name = "B_" + btn.Content.ToString() + '_' + algFind.findMonForIndex(nomerMon) + '_' + strYear[nomerYear].ToString();
                Boolean answer = algFind.presenceNote(btn.Content.ToString(), algFind.findMonForIndex(nomerMon), strYear[nomerYear].ToString(),SID);
                if (answer)
                {
                    btn = webElement.editBackground(btn, i + 1, nomerMon + 1, Convert.ToInt32(strYear[nomerYear]));
                }
                if(i+1 == DateTime.Now.Day && nomerMon + 1 == DateTime.Now.Month && Convert.ToInt32(strYear[nomerYear]) == DateTime.Now.Year)
                {
                    webElement.editBorderThickness(btn);
                }
                Canvas.SetLeft(btn, dayLeft);
                Canvas.SetTop(btn, dayTop);
                btn.Click += day_Button_Click;
                Canvas1.Children.Add(btn);
            }
            arrTopLeft[0] = dayTop;
            arrTopLeft[1] = dayLeft;
            return arrTopLeft;
        }
        private void displayNewMon(int dayLeft,int dayTop)
        {
            WebElement webElement = new WebElement();
            AlgFind algFind = new AlgFind();
            int countBut = 0;
            if (dayTop == 245)
            {
                countBut = ((245 - dayLeft) / 40) + 8;
            }
            else
            {
                countBut = (245 - dayLeft) / 40 + 1;
            }
            Button[] btns = new Button[countBut];
            for (int i = 0, j = dayLeft / 40; i < btns.Length; i++, dayLeft += 40, j++)
            {
                if (j == 7)
                {
                    dayTop += 40;
                    j = 0;
                    dayLeft = 5;
                }
                var btn = new Button
                {
                    Content = i + 1,
                    Width = 40,
                    Height = 40,
                    Background = Brushes.White,
                    Foreground = Brushes.Gray,
                    BorderThickness = new Thickness(0)

                };
                int ptrNomerMon = nomerMon + 1;
                int ptrNomerYear = nomerYear;
                if (nomerMon + 1 > sizeMon.Length - 1)
                {
                    ptrNomerMon = 0;
                    if (nomerYear + 1 > strYear.Count - 1)
                    {
                        ptrNomerYear = strYear.Count - 1;
                    }
                    else
                    {
                        ptrNomerYear = nomerYear + 1;
                    }
                }
                btn.Name = "B_" + btn.Content.ToString() + '_' + algFind.findMonForIndex(ptrNomerMon) + '_' + strYear[ptrNomerYear].ToString();
                Boolean answer = algFind.presenceNote(btn.Content.ToString(), algFind.findMonForIndex(ptrNomerMon), strYear[ptrNomerYear].ToString(),SID);
                if (answer)
                {
                    btn = webElement.editBackground(btn, i + 1, ptrNomerMon + 1, Convert.ToInt32(strYear[ptrNomerYear]));
                }
                if (i + 1 == DateTime.Now.Day && ptrNomerMon + 1 == DateTime.Now.Month && Convert.ToInt32(strYear[ptrNomerYear]) == DateTime.Now.Year)
                {
                    webElement.editBorderThickness(btn);
                }
                Canvas.SetLeft(btn, dayLeft);
                Canvas.SetTop(btn, dayTop);
                btn.Click += day_Button_Click;
               // btn.Opacity = 0.7;
                Canvas1.Children.Add(btn);
            }
        }
        private void displayOldMon(int dayLeft)
        {
            WebElement webElement = new WebElement();
            AlgFind algFind = new AlgFind();
            int countBut = 0;
            if (dayLeft == 0) // если понедельник первое число, то надо вниз на одну строчку отпустить
            {
                countBut = (dayLeft / 40) + 7;
            }
            else
            {
                countBut = dayLeft / 40;
            }
            Button[] btns = new Button[countBut];
            dayLeft = 5;
            int dayTop = 85;
            int ptrM = nomerMon - 1;
            if (ptrM == -1)
            {
                ptrM = 11;
            }
            int sizeDay = sizeMon[ptrM];
            for (int i = 0, t = sizeDay - countBut + 1, j = 0; i < btns.Length; t++, i++, j++, dayLeft += 40)
            {
                if (j == 7)
                {
                    dayTop += 40;
                    j = 0;
                    dayLeft = 5;
                }
                var btn = new Button
                {
                    Content = t,
                    Width = 40,
                    Height = 40,
                    Background = Brushes.White,
                    Foreground = Brushes.Gray,
                    BorderThickness = new Thickness(0)

                };
                int ptrNomerMon = nomerMon - 1;
                int ptrNomerYear = nomerYear;
                if (nomerMon - 1 < 0)
                {
                    ptrNomerMon = 11;
                    if (nomerYear - 1 < 0)
                    {
                        ptrNomerYear = 0;
                    }
                    else
                    {
                        ptrNomerYear = nomerYear - 1;
                    }

                }
                btn.Name = "B_" + btn.Content.ToString() + '_' + algFind.findMonForIndex(ptrNomerMon) + '_' + strYear[ptrNomerYear].ToString();
                Boolean answer = algFind.presenceNote(btn.Content.ToString(), algFind.findMonForIndex(ptrNomerMon), strYear[ptrNomerYear].ToString(),SID);
                if (answer)
                {
                    btn = webElement.editBackground(btn, t, ptrNomerMon + 1, Convert.ToInt32(strYear[ptrNomerYear]));
                }
                if (t == DateTime.Now.Day && ptrNomerMon + 1 == DateTime.Now.Month && Convert.ToInt32(strYear[ptrNomerYear]) == DateTime.Now.Year)
                {
                    webElement.editBorderThickness(btn);
                }
                Canvas.SetLeft(btn, dayLeft);
                Canvas.SetTop(btn, dayTop);
                btn.Click += day_Button_Click;
                //btn.Opacity = 0.7;
                Canvas1.Children.Add(btn);
            }
        }

        private void displayButSet()
        {
            LogicEmail logicEmail = new LogicEmail();
            email = logicEmail.emailFromFile();
            canvasPanel = new Canvas { MaxHeight = 30};

            downPanel.Child = null;

            Label lbEmail = new Label { Name = "lbEmail", Content = "E-mail: ", MaxHeight = 30, Width = 55,FontSize=14 };
            Canvas.SetLeft(lbEmail,3);

            TextBox tbEmail = new TextBox { Name = "tbEmail", Text = email, MaxHeight = 30, Width = 175, MaxLength = 40 , FontSize = 14 };
            tbEmail.MouseEnter += textBoxPopup_MouseEnter;
            tbEmail.MouseLeave += textBoxPopup_MouseLeave;
            tbEmail.VerticalContentAlignment = VerticalAlignment.Center;
            Canvas.SetLeft(tbEmail, 60);
            tbEmail.LostFocus += textBoxEmail_LostFocus;

            Button btEmail = new Button { Name = "btEmail", MaxHeight = 30 ,Width=30,BorderThickness = new Thickness(0)};
            ImageBrush content = new ImageBrush();
            content.ImageSource = new BitmapImage(new Uri("pack://application:,,,/CalendarLight;component/Resources/icon/check_email.png"));
            btEmail.Background = content;
            Canvas.SetLeft(btEmail, 245);
            btEmail.Click += editEmail_button_click;

            canvasPanel.Children.Add(lbEmail);
            canvasPanel.Children.Add(tbEmail);
            canvasPanel.Children.Add(btEmail);

            downPanel.Child = canvasPanel;
        }
        private void displaySettingEmail(int dayLeft,int dayTop)
        {
            Button btn = new Button { Name = "btnSettingEmail",Height = 35, Width = 35 };
            btn.BorderThickness = new Thickness(0);
            ImageBrush content = new ImageBrush();
            content.ImageSource = new BitmapImage(new Uri("pack://application:,,,/CalendarLight;component/Resources/icon/emailfilledclosedenvelope_119640.png"));
            btn.Background = content;

            Canvas.SetLeft(btn, dayLeft);
            Canvas.SetTop(btn, dayTop);
            btn.Click += settingEmail_button_Click;
            btn.MouseEnter += buttonPopup_MouseEnter; 
            btn.MouseLeave += buttonPopup_MouseLeave;
            Canvas1.Children.Add(btn);

            downPanel = new Border { Background = Brushes.White, BorderBrush = Brushes.Black,CornerRadius = new CornerRadius(0),BorderThickness = new Thickness(1)};
            downPanel.Width = 280;
            downPanel.Height = 0;
            Canvas.SetLeft(downPanel, 5);
            Canvas.SetTop(downPanel, 390);
            Canvas1.Children.Add(downPanel);

        }
        private void displayReport(int dayLeft, int dayTop)
        {
            Button btn = new Button { Name = "btnReport", Height = 35, Width = 35 };
            btn.BorderThickness = new Thickness(0);
            ImageBrush content = new ImageBrush();
            content.ImageSource = new BitmapImage(new Uri("pack://application:,,,/CalendarLight;component/Resources/icon/fileinterfacesymboloftextpapersheet_79740.png"));
            btn.Background = content;
            btn.MouseEnter += buttonPopup_MouseEnter;
            btn.MouseLeave += buttonPopup_MouseLeave;
            Canvas.SetLeft(btn, dayLeft);
            Canvas.SetTop(btn, dayTop);
            btn.Click += report_button_Click;
            Canvas1.Children.Add(btn);
        }
        private void displayExit(int dayLeft, int dayTop)
        {
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
        private void displayInfo(int dayLeft,int dayTop)
        {
            Button btn = new Button { Name = "btnInfo", Height = 35, Width = 35 };
            btn.BorderThickness = new Thickness(0);
            ImageBrush content = new ImageBrush();
            content.ImageSource = new BitmapImage(new Uri("pack://application:,,,/CalendarLight;component/Resources/icon/issueopened_106531.png"));
            btn.Background = content;
            btn.MouseEnter += buttonPopup_MouseEnter;
            btn.MouseLeave += buttonPopup_MouseLeave;
            Canvas.SetLeft(btn, dayLeft);
            Canvas.SetTop(btn, dayTop);
            btn.Click += info_button_Click;
            Canvas1.Children.Add(btn);
        }
        private void displayToday(int dayLeft,int dayTop)
        {
            Button btn = new Button { Name = "btnToday", Height = 35, Width = 35 };
            btn.BorderThickness = new Thickness(0);
            ImageBrush content = new ImageBrush();
            content.ImageSource = new BitmapImage(new Uri("pack://application:,,,/CalendarLight;component/Resources/icon/today_118542.png"));
            btn.Background = content;
            btn.MouseEnter += buttonPopup_MouseEnter;
            btn.MouseLeave += buttonPopup_MouseLeave;
            Canvas.SetLeft(btn, dayLeft);
            Canvas.SetTop(btn, dayTop);
            btn.Click += today_button_Click;
            Canvas1.Children.Add(btn);
        }
        private void displaySignature(int dayLeft,int dayTop)
        {
            Label label = new Label { Content = "――――――――――――――――――――――――――――――――――――――――――――", Height = 30, Width = 285, HorizontalContentAlignment = HorizontalAlignment.Center };
            Canvas.SetLeft(label, dayLeft);
            Canvas.SetTop(label, dayTop);
            Canvas1.Children.Add(label);
        }

        public Border display()
        {
            mainBorder.Child = null;
            IsToggle = false;
            AlgFind algFind = new AlgFind();
            addMarkerMon();
            int dayLeft = 0;
            int dayTop = 0;

            dayLeft = 75;
            displayMon(dayLeft);

            dayTop = 50;
            dayLeft = 10;
            displayDayWerk(dayLeft, dayTop);

            dayLeft = algFind.findDayWeek(1, nomerMon + 1, Convert.ToInt32(strYear[nomerYear]));
            int[] arrLefyTop = displayMainMon(dayLeft);

            dayTop = arrLefyTop[0];
            dayLeft = arrLefyTop[1];
            dayLeft += 40;
            displayNewMon(dayLeft, dayTop);

            dayLeft = algFind.findDayWeek(1, nomerMon + 1, Convert.ToInt32(strYear[nomerYear]));
            displayOldMon(dayLeft);

            dayLeft = 5;
            dayTop = 330;
            displaySignature(dayLeft, dayTop);

            dayTop = 352;
            dayLeft = 10;
            displaySettingEmail(dayLeft,dayTop);

            dayLeft = 55;
            displayReport(dayLeft, dayTop);

            dayLeft = 95;
            displayInfo(dayLeft,dayTop);

            dayLeft = 240;
            displayToday(dayLeft, dayTop);

            dayTop = 2;
            dayLeft = 270;
            displayExit(dayLeft, dayTop);


            mainBorder.Child = Canvas1;
            return mainBorder;
        }

        public void displayCoolectionMon()
        {
            AlgFind algFind = new AlgFind();
            int dayTop = 7;
            int dayLeft = 105;
            Label lbYear = new Label { Content = strYear[nomerYear] + " год", Height = 40, Width = 95, FontSize = 18 };
            lbYear.FontFamily = new FontFamily("Arial");
            lbYear.FontWeight = FontWeights.DemiBold;
            Canvas.SetLeft(lbYear, dayLeft);
            Canvas.SetTop(lbYear, dayTop);
            Canvas1.Children.Add(lbYear);
            Button[] btns = new Button[sizeMon.Length];
            dayTop = 40;
            dayLeft = 5;
            for (int i = 0, j = 0; i < btns.Length; i++, dayLeft += 93, j++)
            {
                if (j == 3)
                {
                    j = 0;
                    dayTop += 90;
                    dayLeft = 5;
                }
                var btn = new Button
                {
                   // Content = algFind.findMonForIndex(i),
                    Width = 93,
                    Height = 90,
                    Margin = new Thickness(0),
                   // FontWeight = FontWeights.Black,
                    Background = Brushes.White,
                    BorderThickness = new Thickness(0)
                };
                btn.MouseEnter += mon_Button_MouseEnter;
                btn.MouseLeave += mon_Button_MouseLeave;
                btn.Click += mon_Button_Click;
                Canvas.SetLeft(btn, dayLeft);
                Canvas.SetTop(btn, dayTop);

                Canvas canvas = new Canvas { Width = 93, Height = 90 };
                int dayLeftWork = 0;
                int dayTopWork = 0;
                string[] str = { "Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Вс" };
                Label[] lbs = new Label[7];
                for (int t = 0; t < lbs.Length; t++, dayLeftWork += 13)
                {
                    var lb = new Label
                    {
                        Content = str[t],
                        FontSize = 3,
                        Background = Brushes.Transparent,
                        FontWeight = FontWeights.Black
                    };
                    Canvas.SetLeft(lb, dayLeftWork);
                    Canvas.SetTop(lb, dayTopWork);
                    canvas.Children.Add(lb);
                }
                Label[] lbsDay = new Label[sizeMon[i]];
                WebElement webElement = new WebElement();
                dayLeftWork = 0;
                dayTopWork = 13;
                for (int p = 0, y = (algFind.findDayWeek(1, i + 1, Convert.ToInt32(strYear[nomerYear]))) / 40; p < lbsDay.Length; p++, y++)
                {
                     dayLeftWork = ((algFind.findDayWeek(p + 1, i + 1, Convert.ToInt32(strYear[nomerYear]))) / 40 )*13;
                    if (y == 7)
                    {
                        dayTopWork += 13;
                        y = 0;
                    }
                    var lb = new Label
                    {
                        Content = p + 1,
                        FontSize = 3,
                        Height = 13,
                        Width = 13,
                        FontWeight = FontWeights.Bold,
                        Background = Brushes.Transparent,

                     };
                    Boolean answer = algFind.presenceNote(lb.Content.ToString(), algFind.findMonForIndex(i), strYear[nomerYear].ToString(),SID);
                    if (answer)
                    {
                        lb = webElement.editBackground(lb, p + 1, i + 1, Convert.ToInt32(strYear[nomerYear]));
                    }
                    if (p + 1 == DateTime.Now.Day && i + 1 == DateTime.Now.Month && Convert.ToInt32(strYear[nomerYear]) == DateTime.Now.Year)
                    {
                        webElement.editBorderThickness(lb);
                    }
                    Canvas.SetLeft(lb, dayLeftWork);
                    Canvas.SetTop(lb, dayTopWork);
                    canvas.Children.Add(lb);
                }
                Label lbNameMon = new Label { Name="lbnameMon", Content = algFind.findMonForIndex(i), Background = Brushes.Transparent,FontSize = 16, FontWeight = FontWeights.Black };
                lbNameMon.Foreground = Brushes.Black;
                canvas.Children.Add(lbNameMon);
                btn.Content = canvas;
                Canvas1.Children.Add(btn);
            }

            dayTop = 2;
            dayLeft = 270;
            displayExit(dayLeft, dayTop);
        }
    }
}
