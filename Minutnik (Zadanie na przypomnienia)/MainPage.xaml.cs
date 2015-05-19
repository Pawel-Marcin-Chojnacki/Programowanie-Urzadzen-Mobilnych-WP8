using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Minutnik__Zadanie_na_przypomnienia_.Resources;
using Microsoft.Phone.Scheduler;

namespace Minutnik__Zadanie_na_przypomnienia_
{
    public partial class MainPage : PhoneApplicationPage
    {

        private int RemainderTime;
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void Start_Countdown(object sender, RoutedEventArgs e)
        {
            if (ReminderNameTextBox.Text == "")
            {
                MessageBox.Show("Musisz podac nazwe przypomnienia.");
                return;
            }
            if (CountdownSlider == null || CountdownSlider.Value == 0)
            {
                MessageBox.Show("Musisz nastwić czas suwakiem.");
                return;
            }
            var customReminder = new Reminder(ReminderNameTextBox.Text)
            {
                BeginTime = DateTime.Now + new TimeSpan(0, RemainderTime, 0),
                Content = "Zegar odliczyl czas na: " + ReminderNameTextBox.Text,
                NavigationUri = new Uri("/Page2.xaml", UriKind.Relative),
                RecurrenceType = RecurrenceInterval.None
            };
            if (ScheduledActionService.Find(ReminderNameTextBox.Text) != null)
            {
                ScheduledActionService.Remove(ReminderNameTextBox.Text);
            }
            ScheduledActionService.Add(customReminder);

        }

        private void Update_Textblock(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int newValueOfSlider = (int) e.NewValue;
            CountdownSlider.Value = newValueOfSlider;
            MinutesLeftTextBlock.Text = newValueOfSlider.ToString();
            RemainderTime = newValueOfSlider;
        }

    }
} ;