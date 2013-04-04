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


namespace Kinect_Architecture
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window    
    {

       /* Variables */
        private KinectMain kinect;
        public List<Button> buttons;

        public MainWindow()
        {
            InitializeComponent();
            Instruction.Text = "Hold your hand over the start button";
            this.buttons = new List<System.Windows.Controls.Button> { buttonStart, quitButton };
            kinect = new KinectMain(global, kinectButton, buttons);
            kinectButton.Click += new RoutedEventHandler(this.kinect.curseur.kinectButton_Click);
            
            kinect.gestureCamera.OnSwipeLeftEvent += new GestureCamera.SwipeLeftEvent(moveToMenu);
            kinect.gestureCamera.OnSwipeRightEvent += new GestureCamera.SwipeRightEvent(moveToMenu);
        }

        private void moveToMenu()
        {
            Views.Menu MenuPage = new Views.Menu();
            this.Content = MenuPage;
        }

        ////When the window is loaded
        private void Window_Loaded(Object sender, RoutedEventArgs e)
        {
            kinect.InitKinect(StickMen);

            // get the main screen size
            double height = System.Windows.SystemParameters.PrimaryScreenHeight;
            double width = System.Windows.SystemParameters.PrimaryScreenWidth;

            // if the main screen is not 1920 x 1080 then warn the user it is not the optimal experience 
            if (width != 1920 || height != 1080)
            {
                MessageBoxResult continueResult = MessageBox.Show("This screen is not 1920 x 1080.\nThis sample has been optimized for a screen resolution of 1920 x 1080.\nDo you wish to continue?", "Suboptimal Screen Resolution", MessageBoxButton.YesNo);
                if (continueResult == MessageBoxResult.No)
                {
                    this.Close();
                }
            }
        }


        // Methode pour un zoom/dezoom 'fluide'

        //private bool ZoomDezoom(Skeleton skeleton)
        //{

        //    CoordinateMapper cmLeft = new CoordinateMapper(this.sensor);
        //    CoordinateMapper cmRight = new CoordinateMapper(this.sensor);
        //    ColorImagePoint Left = cmLeft.MapSkeletonPointToColorPoint(skeleton.Joints[JointType.HandLeft].Position, ColorImageFormat.RgbResolution1280x960Fps12);
        //    ColorImagePoint Right = cmRight.MapSkeletonPointToColorPoint(skeleton.Joints[JointType.HandRight].Position, ColorImageFormat.RgbResolution1280x960Fps12);
        //    // // Coordonnée en Z proche 
        //    //  if ((skeleton.Joints[JointType.HandRight].Position.Z <= skeleton.Joints[JointType.HandLeft].Position.Z + 1 && skeleton.Joints[JointType.HandRight].Position.Z >= skeleton.Joints[JointType.HandLeft].Position.Z - 1))
        //    //  {
        //    // Coordonnée en Y proche 
        //    if ((Right.Y <= Left.Y + 40 && Right.Y >= Left.Y - 40)) // rajouté une condition sur la position par rapport au corps
        //    {
        //        // Coordonnée en X 'centré'
        //        if (Right.X - 1280 / 2 < 1280 / 2 - Left.X + 40 && Right.X - 1280 / 2 > 1280 / 2 - Left.X - 40)
        //        {
        //            xZoomL = Left.X;
        //            xZoomR = Right.X;

        //            Canvas.SetLeft(rectZoom, 2 * Left.X);
        //            rectZoom.Width = 2 * Right.X - 2 * Left.X;
        //            return true;
        //        }

        //    }

        //    xZoomL = -1;
        //    xZoomR = -1;
        //    return false;

        //}

        public void button_Start(object sender, RoutedEventArgs e)
        {
            Views.CameraOne CameraPage = new Views.CameraOne();
            Views.Menu MenuPage = new Views.Menu();
            this.Content = CameraPage;
        }

        public void quitButton_Click(object sender, RoutedEventArgs e)
        {
            //System.Windows.Application.Current.Shutdown();
            
        }

       
    }
}