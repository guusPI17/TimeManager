using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using CalendarLight.Logic;

namespace CalendarLight.Calendar
{
    public class WebElement
    {
        const int limitWork = 3;
        public Button editBackground(Button _button,int day,int mon,int year)
        {
            Button button = _button;
            if (DateTime.Now.Year < year)
            {
                button.Background = new SolidColorBrush(Color.FromRgb(130, 244, 122));
            }
            else if(DateTime.Now.Year > year)
            {
                button.Background = new SolidColorBrush(Color.FromRgb(240, 240, 240));
            }
            else
            {
                if (DateTime.Now.Month < mon)
                {
                    button.Background = new SolidColorBrush(Color.FromRgb(130, 244, 122));
                }
                else if(DateTime.Now.Month > mon)
                {
                    button.Background = new SolidColorBrush(Color.FromRgb(240, 240, 240));
                }
                else
                {
                    if (day - DateTime.Now.Day > limitWork)
                    {
                        button.Background = new SolidColorBrush(Color.FromRgb(130, 244, 122));
                    }
                    else if (DateTime.Now.Day > day)
                    {
                        button.Background = new SolidColorBrush(Color.FromRgb(240, 240, 240));
                    }
                    else if ((day - DateTime.Now.Day <= limitWork) && (DateTime.Now.Day != day))
                    {
                        button.Background = new SolidColorBrush(Color.FromRgb(92, 249, 44));
                    }
                    else if (DateTime.Now.Day == day)
                    {
                        button.Background = new SolidColorBrush(Color.FromRgb(16, 196, 3));
                    }
                }
            }
            return button;
        }
        public Label editBackground(Label _label, int day, int mon, int year)
        {
            Label label = _label;
            if (DateTime.Now.Year < year)
            {
                label.Background = new SolidColorBrush(Color.FromRgb(130, 244, 122));
            }
            else if (DateTime.Now.Year > year)
            {
                label.Background = new SolidColorBrush(Color.FromRgb(227, 227, 229));
            }
            else
            {
                if (DateTime.Now.Month < mon)
                {
                    label.Background = new SolidColorBrush(Color.FromRgb(130, 244, 122));
                }
                else if (DateTime.Now.Month > mon)
                {
                    label.Background = new SolidColorBrush(Color.FromRgb(227, 227, 229));
                }
                else
                {
                    if (day - DateTime.Now.Day > limitWork)
                    {
                        label.Background = new SolidColorBrush(Color.FromRgb(130, 244, 122));
                    }
                    else if (DateTime.Now.Day > day)
                    {
                        label.Background = new SolidColorBrush(Color.FromRgb(227, 227, 229));
                    }
                    else if ((day - DateTime.Now.Day <= limitWork) && (DateTime.Now.Day != day))
                    {
                        label.Background = new SolidColorBrush(Color.FromRgb(92, 249, 44));
                    }
                    else if (DateTime.Now.Day == day)
                    {
                        label.Background = new SolidColorBrush(Color.FromRgb(16, 196, 3));
                    }
                }
            }
            return label;
        }
        public Border editBackground(Border _border, string time)
        {
            AlgFind algFind = new AlgFind();
            string timeCompSt = algFind.timeWithoutSec();
            string[] divisTime = algFind.divideTime(timeCompSt);
            Border border = _border;
            if (String.Compare(time, timeCompSt) < 0)
            {
                border.Background = new SolidColorBrush(Color.FromRgb(250, 250, 250));
            }
            else if (String.Compare(time, Convert.ToString((Convert.ToInt32(divisTime[0]) + 1))) < 0) // не выше 1 часа
            {
                border.Background = new SolidColorBrush(Color.FromRgb(16, 196, 3));
            }
            else if (String.Compare(time, Convert.ToString((Convert.ToInt32(divisTime[0]) + 5))) < 0) // выше 1 часа и ниже 5 часов
            {
                border.Background = new SolidColorBrush(Color.FromRgb(92, 249, 44));
            }
            else // выше 5 часов
            {
                border.Background = new SolidColorBrush(Color.FromRgb(130, 244, 122));
            }
            return border;
        }
        public Button editBorderThickness(Button _button)
        {
            Button button = _button;
            button.BorderThickness = new Thickness(2);
            button.BorderBrush = Brushes.Red;
            return button;
        }
        public Label editBorderThickness(Label _label)
        {
            Label label = _label;
            label.BorderThickness = new Thickness(0.5);
            label.BorderBrush = Brushes.Red;
            return label;
        }

    }
}