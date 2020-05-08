using CalendarLight.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CalendarLight.Calendar
{
    class DisplayInfo
    {
        readonly string SID = string.Empty;
        int mon;
        int year;
        int[] sizeMon;
        double dayLeft;
        double dayTop;

        Canvas Canvas1;
        Border borderInfo;
        Border borderCalendar; // border календаря при вызовет Note, чтобы вернуться
        Canvas canvasBorders = new Canvas { Width = 280, MinHeight = 345 };
        ScrollViewer scrollViewer = new ScrollViewer { Width = 285, Height = 345 };
        Popup popup = new Popup { Placement = PlacementMode.Mouse, MaxWidth = 180, AllowsTransparency = true, IsOpen = false };
        Border borderPopup = new Border { Background = new SolidColorBrush(Color.FromRgb(245, 245, 245)), BorderBrush = Brushes.Black, BorderThickness = new Thickness(1) };
        System.Windows.Threading.DispatcherTimer popupTimer;

        public DisplayInfo(int _year, int _mon, int[] _sizeMon, Border _border,string sid)
        {
            SID = sid;

            Canvas1 = new Canvas();
            Canvas1.Background = Brushes.White;

            borderCalendar = _border;
            borderInfo = _border;
            AlgFind algFind = new AlgFind();
            mon = _mon;
            year = _year;
            sizeMon = _sizeMon;

            scrollViewer.CanContentScroll = true;
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

            borderInfo.Child = Canvas1;
        }

        private void displayCalendar_Button_Click(object sender, RoutedEventArgs e)
        {
            borderInfo.Child = null;
            DisplayCalendar displayCalendar = new DisplayCalendar(borderCalendar, year, mon, sizeMon,SID);
        }
        private void exit_button_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
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
        private void timerTick(object sender, EventArgs e)
        {
            popup.IsOpen = true;
            popupTimer.IsEnabled = false;
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
        private void displayHead()
        {
            dayLeft = 30;
            dayTop = 10;

            Label lbHead = new Label { Content = "Информация", Height = 40, Width = 240, FontSize = 16 };
            lbHead.Background = Brushes.White;
            lbHead.FontFamily = new FontFamily("Arial");
            lbHead.FontWeight = FontWeights.DemiBold;
            lbHead.HorizontalContentAlignment = HorizontalAlignment.Center;
            Canvas.SetLeft(lbHead, dayLeft);
            Canvas.SetTop(lbHead, dayTop);
            Canvas1.Children.Add(lbHead);
        }
        private void displayText()
        {
            dayTop = 35;

            AlgFind algFind = new AlgFind();
            List<DataNote> dataNote = new List<DataNote>();
            SerializableNote serializableNote = new SerializableNote(SID);
            dataNote = serializableNote.deserializable(new Uri("pack://application:,,,/CalendarLight;component/Resources/infoText/Info.json"));
            Canvas.SetLeft(scrollViewer, 5);
            Canvas.SetTop(scrollViewer, dayTop);
            Canvas.SetTop(canvasBorders, 0);
            dayTop = 5;
            canvasBorders.Height = 50;
            for (int i = 0; i < dataNote.Count; i++)
            {
                dayLeft = 8;
                Label thema = new Label { Content = dataNote[i].thema, FontSize = 10, FontWeight = FontWeights.Black };

                Label text = new Label { Content = dataNote[i].text, Width = 270, FontSize = 14 };

                string textLable = text.Content.ToString();
                int sizeText = algFind.findSizeText(textLable);
                canvasBorders.Height += sizeText * 20 + 30 + 10;
                Border border = new Border { Height = sizeText * 20 + 10 + 30, Width = 260, BorderBrush = Brushes.Black, BorderThickness = new Thickness(1) };
                border.CornerRadius = new CornerRadius(2);
                Canvas.SetLeft(border, dayLeft);
                Canvas.SetTop(border, dayTop);

                Grid grid = new Grid {};

                grid.RowDefinitions.Add(new RowDefinition());
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(sizeText * 20 + 10) });

                grid.ColumnDefinitions.Add(new ColumnDefinition { });

                Grid.SetRow(thema, 0);
                Grid.SetColumn(thema, 0);
                grid.Children.Add(thema);

                Grid.SetRow(text, 1);
                Grid.SetColumn(text, 0);
                grid.Children.Add(text);

                border.Child = grid;

                dayTop = Canvas.GetTop(border);
                dayTop += sizeText * 20 + 15 + 30;

                canvasBorders.Children.Add(border);
            }
            scrollViewer.Content = canvasBorders;
            Canvas1.Children.Add(scrollViewer);
        }
        private void displayBack()
        {
            Button back = new Button { Width = 30, Height = 30 };
            back.BorderThickness = new Thickness(0);
            ImageBrush content = new ImageBrush();
            content.ImageSource = new BitmapImage(new Uri("pack://application:,,,/CalendarLight;component/Resources/icon/leftarrow1_120649.png"));
            back.Background = content;
            Canvas.SetLeft(back, 45); // 10
            Canvas.SetTop(back, 5);
            back.Click += displayCalendar_Button_Click;
            Canvas1.Children.Add(back);
        }

        public Border display()
        {
            Canvas1.Children.Clear();
            scrollViewer.Content = null;
            canvasBorders.Children.Clear();

            displayExit();
            displayHead(); 
            displayText();
            displayBack();
            return borderInfo;
        }
    }
}
