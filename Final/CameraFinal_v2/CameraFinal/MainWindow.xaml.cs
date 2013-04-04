using ManagedUPnP;
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
using Vlc.DotNet.Wpf;


namespace CameraFinal
{
    /// <summary> 
    /// Logique d'interaction pour MainWindow.xaml 
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Camera[] cameraArray = new Camera[3];

        public MainWindow()
        {
            InitializeComponent();

            cameraArray[0] = new CameraPTZ(new VlcControl(), player0);
            cameraArray[1] = new CameraSTD(new VlcControl(), player1);
            cameraArray[2] = new CameraSTD(new VlcControl(), player2);

            Discovery disc = new Discovery(null, AddressFamilyFlags.IPv4, false);
            disc.DeviceAdded += new DeviceAddedEventHandler(discDeviceAdded);
            disc.Start();
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

        private void Screenshot_Click(object sender, RoutedEventArgs e)
        {
            cameraArray[0].takeScreenshot();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            cameraArray[0].startCapture();
        }

        private void stopCapture_Click(object sender, RoutedEventArgs e)
        {
            cameraArray[0].stopCapture();
        }
    }
}

                /*ipList.Add(a.Device.RootHostAddresses[0].ToString());
                nameList.Add(a.Device.FriendlyName.ToString());*/