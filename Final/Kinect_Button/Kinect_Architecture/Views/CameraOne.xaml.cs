using System;
using Kinect_Architecture;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Coding4Fun.Kinect.Wpf.Controls;
using CameraFinal;
using ManagedUPnP;
using Vlc.DotNet.Wpf;


namespace Kinect_Architecture.Views
{
    /// <summary>
    /// Logique d'interaction pour CameraOne.xaml
    /// </summary>
    public partial class CameraOne : UserControl
    {

        /* Variables */
        private KinectMain kinect;
        public List<Button> buttons;
        private static CameraPTZ cameraOne;
        public System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        public CameraOne()
        {
            InitializeComponent();
            this.buttons = new List<System.Windows.Controls.Button> { quitButton, buttonDown, buttonDownLeft, buttonDownRight, buttonLeft, buttonRight, buttonTop, buttonTopLeft, buttonTopRight };
            kinect = new KinectMain(CameraOneGrid, kinectButton, buttons);
            kinectButton.Click += new RoutedEventHandler(this.kinect.curseur.kinectButton_Click);

            cameraOne = new CameraPTZ(new VlcControl(), CameraOnePlayer);

            Discovery disc = new Discovery(null, AddressFamilyFlags.IPv4, false);
            disc.DeviceAdded += new DeviceAddedEventHandler(discDeviceAdded);
            disc.Start();

            kinect.gestureCamera.OnSwipeLeftEvent += new GestureCamera.SwipeLeftEvent(moveToMenu);
            kinect.gestureCamera.OnSwipeRightEvent += new GestureCamera.SwipeRightEvent(moveToMenu);   
        }

        private void moveToMenu()
        {
            Views.Menu MenuPage = new Views.Menu();
            this.Content = MenuPage;
        }

        ////When the window is loaded
        private void CameraOne_Window_Loaded(Object sender, RoutedEventArgs e)
        {
            kinect.InitKinect(StickMen);
        }

        public static void discDeviceAdded(object sender, DeviceAddedEventArgs a)
        {
            cameraOne.initCamera("172.18.255.100");
            System.Console.WriteLine(a.Device.RootHostAddresses[0].ToString());
            cameraOne.Play();
            if (a.Device.FriendlyName.Contains("AXIS 214"))
            {
                cameraOne.initCamera(a.Device.RootHostAddresses[0].ToString());
                cameraOne.Play();
                /*if (cameraArray[0] is CameraPTZ)
                ((CameraPTZ)cameraArray[0]).zoomOn();*/
            }
        }

        public void TimerStop(Object myObject, EventArgs myEventArgs)
        {
            this.timer.Stop();
        }

        public void button_Right(object sender, RoutedEventArgs e)
        {
            this.timer.Tick += new EventHandler(this.TimerStop);
            if (!this.timer.Enabled)
            {
                this.timer.Interval = 2000;
                this.timer.Start();
                System.Console.WriteLine("Button Right");
                message.Content = "Button Right";
                cameraOne.goRight();
            }
        }

        public void button_Left(object sender, RoutedEventArgs e)
        {
            this.timer.Tick += new EventHandler(this.TimerStop);
            if (!this.timer.Enabled)
            {
                this.timer.Interval = 2000;
                this.timer.Start();
                System.Console.WriteLine("Button Left");
                message.Content = "Button Left";
                cameraOne.goLeft();
            }
        }

        public void button_Top(object sender, RoutedEventArgs e)
        {
            this.timer.Tick += new EventHandler(this.TimerStop);
            if (!this.timer.Enabled)
            {
                this.timer.Interval = 2000;
                this.timer.Start();
                System.Console.WriteLine("Button Top");
                message.Content = "Button Top";
                cameraOne.goUp();
            }
        }

        public void button_TopLeft(object sender, RoutedEventArgs e)
        {
            this.timer.Tick += new EventHandler(this.TimerStop);
            if (!this.timer.Enabled)
            {
                this.timer.Interval = 3000;
                this.timer.Start();
                System.Console.WriteLine("Button TopLeft");
                message.Content = "Button TopLeft";
                cameraOne.goUp();
                cameraOne.goLeft();
            }
        }

        public void button_TopRight(object sender, RoutedEventArgs e)
        {
            this.timer.Tick += new EventHandler(this.TimerStop);
            if (!this.timer.Enabled)
            {
                this.timer.Interval = 3000;
                this.timer.Start();
                System.Console.WriteLine("Button TopRight");
                message.Content = "Button TopRight";
                cameraOne.goUp();
                cameraOne.goRight();
            }
            
        }

        public void button_Down(object sender, RoutedEventArgs e)
        {
            this.timer.Tick += new EventHandler(this.TimerStop);
            if (!this.timer.Enabled)
            {
                this.timer.Interval = 2000;
                this.timer.Start();
                System.Console.WriteLine("Button Down");
                message.Content = "Button Down";
                cameraOne.goDown();
            }
        }

        public void button_DownRight(object sender, RoutedEventArgs e)
        {
            this.timer.Tick += new EventHandler(this.TimerStop);
            if (!this.timer.Enabled)
            {
                this.timer.Interval = 3000;
                this.timer.Start();
                System.Console.WriteLine("Button DownRight");
                message.Content = "Button DownRight";
                cameraOne.goDown();
                cameraOne.goRight();
            }
            
        }

        public void button_DownLeft(object sender, RoutedEventArgs e)
        {
            this.timer.Tick += new EventHandler(this.TimerStop);
            if (!this.timer.Enabled)
            {
                this.timer.Interval = 3000;
                this.timer.Start();
                System.Console.WriteLine("Button DownLeft");
                message.Content = "Button  DownLeft";
                cameraOne.goDown();
                cameraOne.goLeft();
            }
        }

        public void quitButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        public void button_Menu(object sender, RoutedEventArgs e)
        {
            /*Views.Menu MenuPage = new Views.Menu();
            this.Content = MenuPage;*/
        }

        private void button_Screenshot(object sender, RoutedEventArgs e)
        {
            cameraOne.takeScreenshot();
        }

        private void button_Start(object sender, RoutedEventArgs e)
        {
            cameraOne.startCapture();
        }

        private void button_Stop(object sender, RoutedEventArgs e)
        {
            cameraOne.stopCapture();
        }
    }
}
