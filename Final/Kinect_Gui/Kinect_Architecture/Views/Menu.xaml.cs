﻿using System;
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
using CameraFinal;
using ManagedUPnP;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect.Toolkit.Controls;

namespace Kinect_Architecture.Views
{
    /// <summary>
    /// Logique d'interaction pour Menu.xaml
    /// </summary>
    public partial class Menu : UserControl
    {

        /* Variables */
        private KinectMain kinect; 
        private readonly KinectSensorChooser sensorChooser;
        private static Camera[] cameraArray = new Camera[3];

        public Menu(KinectSensorChooser sensorChooser)
        {
            InitializeComponent();

            // initialize the sensor chooser and UI
            this.sensorChooser = sensorChooser;
            this.sensorChooser.KinectChanged += SensorChooserOnKinectChanged;
            this.sensorChooserUi.KinectSensorChooser = this.sensorChooser;
            this.sensorChooser.Start();

            // Bind the sensor chooser's current sensor to the KinectRegion
            var regionSensorBinding = new Binding("Kinect") { Source = this.sensorChooser };
            BindingOperations.SetBinding(this.kinectRegion, KinectRegion.KinectSensorProperty, regionSensorBinding);

            // Use KinectMain class
            //this.kinect = new KinectMain(this.sensorChooser.Kinect);
            

           

            /*Instruction.Text = "Select a CCTV";
            this.buttons = new List<System.Windows.Controls.Button> { buttonOne, buttonTwo, buttonThree };
            kinect = new KinectMain(MenuGrid, kinectButton, buttons);
            kinectButton.Click += new RoutedEventHandler(this.kinect.curseur.kinectButton_Click);

            /*cameraArray[0] = new CameraPTZ(new VlcControl(), cameraOne);
            cameraArray[1] = new CameraSTD(new VlcControl(), cameraTwo);
            cameraArray[2] = new CameraSTD(new VlcControl(), cameraThree);

            Discovery disc = new Discovery(null, AddressFamilyFlags.IPv4, false);
            disc.DeviceAdded += new DeviceAddedEventHandler(discDeviceAdded);
            disc.Start();/*

            kinect.gestureCamera.OnSwipeLeftEvent += new GestureCamera.SwipeLeftEvent(moveToCameraOne);
            kinect.gestureCamera.OnSwipeRightEvent += new GestureCamera.SwipeRightEvent(moveToCameraOne);*/

        }

        /// <summary>
        /// Called when the KinectSensorChooser gets a new sensor
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="args">event arguments</param>
        private static void SensorChooserOnKinectChanged(object sender, KinectChangedEventArgs args)
        {
            if (args.OldSensor != null)
            {
                try
                {
                    args.OldSensor.DepthStream.Range = DepthRange.Default;
                    args.OldSensor.SkeletonStream.EnableTrackingInNearRange = false;
                    args.OldSensor.DepthStream.Disable();
                    args.OldSensor.SkeletonStream.Disable();
                }
                catch (InvalidOperationException)
                {
                    // KinectSensor might enter an invalid state while enabling/disabling streams or stream features.
                    // E.g.: sensor might be abruptly unplugged.
                }
            }

            if (args.NewSensor != null)
            {
                try
                {
                    args.NewSensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
                    args.NewSensor.SkeletonStream.Enable();

                    try
                    {
                        args.NewSensor.DepthStream.Range = DepthRange.Near;
                        args.NewSensor.SkeletonStream.EnableTrackingInNearRange = true;
                    }
                    catch (InvalidOperationException)
                    {
                        // Non Kinect for Windows devices do not support Near mode, so reset back to default mode.
                        args.NewSensor.DepthStream.Range = DepthRange.Default;
                        args.NewSensor.SkeletonStream.EnableTrackingInNearRange = false;
                    }
                }
                catch (InvalidOperationException)
                {
                    // KinectSensor might enter an invalid state while enabling/disabling streams or stream features.
                    // E.g.: sensor might be abruptly unplugged.
                }
            }
        }

        private void moveToCameraOne()
        {
            Views.CameraOne CameraPage = new Views.CameraOne(this.sensorChooser);
            this.Content = CameraPage;
        }

        ////When the window is loaded
        private void Menu_Window_Loaded(Object sender, RoutedEventArgs e)
        {
           
        }

        public static void discDeviceAdded(object sender, DeviceAddedEventArgs a)
        {
            if (a.Device.FriendlyName.Contains("AXIS 214"))
            {
                cameraArray[0].initCamera(a.Device.RootHostAddresses[0].ToString());
                cameraArray[0].Play();
                /*if (cameraArray[0] is CameraPTZ)
                ((CameraPTZ)cameraArray[0]).zoomOn();*/
            }

            else if (a.Device.FriendlyName.Contains("AXIS M1054"))
            {
                cameraArray[1].initCamera(a.Device.RootHostAddresses[0].ToString());
                cameraArray[1].Play();
            }

            else if (a.Device.FriendlyName.Contains("AXIS 54645"))
            {
                cameraArray[2].initCamera(a.Device.RootHostAddresses[0].ToString());
                cameraArray[2].Play();
            }
        }


        public void button_One(object sender, RoutedEventArgs e)
        {
            
        }

        public void button_Two(object sender, RoutedEventArgs e)
        {
            Views.CameraOne CameraOnePage = new Views.CameraOne(this.sensorChooser);
            this.Content = CameraOnePage;
        }

        public void button_Three(object sender, RoutedEventArgs e)
        {
            
        }

    }
}
