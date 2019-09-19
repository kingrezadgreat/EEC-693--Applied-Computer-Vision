﻿using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace lecture_2_button
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KinectSensor sensor;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            if (KinectSensor.KinectSensors.Count > 0)
            {
                this.sensor = KinectSensor.KinectSensors[0]; if (this.sensor != null && !this.sensor.IsRunning)
                {
                    this.sensor.Start(); displayInfo();
                }
            }
            else
            {
                MessageBox.Show("No device is connected with system!"); this.Close();
            }
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.sensor != null && this.sensor.IsRunning)
            {
                this.sensor.Stop();
            }
        }

        private void displayInfo()
        {
            this.textBlock1.Text = this.sensor.DeviceConnectionId;
            this.textBlock2.Text = this.sensor.Status.ToString();
            this.textBlock3.Text = this.sensor.ElevationAngle.ToString();
        }



        private void Decrease_Click(object sender, RoutedEventArgs e)
        {
            this.sensor.ElevationAngle = this.sensor.ElevationAngle - 1;
            displayInfo();
        }

        private void Increase_Click(object sender, RoutedEventArgs e)
        {
            this.sensor.ElevationAngle = this.sensor.ElevationAngle + 1;
            displayInfo();
        }
    }
}
