using Microsoft.Kinect;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Collections.Generic;
using Coding4Fun.Kinect.Wpf.Controls;


namespace Kinect_Architecture
{
    class KinectMain
    {
        private KinectSensor sensor;
        private SkeletonManagement[] SkeletonManagementData;
        public Curseur curseur;
        public GestureCamera gestureCamera;


        private Canvas StickMen;
        private int nearestId;
        private DateTime highlightTime;
        private int highlightId;

        public KinectMain(Grid global, HoverButton kinectButton, List<Button> buttons)
        {
            curseur = new Curseur(global, kinectButton, buttons);
            gestureCamera = new GestureCamera();
            SkeletonManagementData = new SkeletonManagement[0];
            nearestId = -1;
            highlightId = -1;
            highlightTime = DateTime.MinValue;

          
        }

        private void HighlightSkeleton(Skeleton skeleton)
        {
            // Set the highlight time to be a short time from now.
            highlightTime = DateTime.UtcNow + TimeSpan.FromSeconds(0.5);

            // Record the ID of the skeleton.
            highlightId = skeleton.TrackingId;
        }

        //For Skeleton
        public void sensor_SkeletonFramesReady(Object sender, SkeletonFrameReadyEventArgs e)
        {
            int i;

            Skeleton skeletonFocus;
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {

                    if ((this.SkeletonManagementData.Length != skeletonFrame.SkeletonArrayLength))
                    {
                        // Actualise le tableau de SkeletonManagement en fonction du nombre de Skeleton
                        SkeletonManagementData = new SkeletonManagement[skeletonFrame.SkeletonArrayLength];
                    }

                    for (i = 0; i < SkeletonManagementData.Length; i++)
                    {
                        // Initialise les skeletons du tableau skeletonManagementData à partir des skeletons détectés
                        SkeletonManagementData[i] = new SkeletonManagement(skeletonFrame, i, StickMen);
                    }

                    /////////////////////// SELECTION SKELETON //////////////////////

                    // Assume no nearest skeleton and that the nearest skeleton is a long way away.
                    float nearestDistance = (float)double.MaxValue;

                    for (i = 0; i < SkeletonManagementData.Length; i++)
                    {
                        // Only consider tracked skeletons.
                        if (SkeletonManagementData[i].skeleton.TrackingState == SkeletonTrackingState.Tracked)
                        {
                            if (SkeletonManagementData[i].isSkeletonNearest(nearestDistance))
                            {
                                // Récupère la plus proche distance
                                nearestDistance = SkeletonManagementData[i].distance;

                                // Récupère l'id du skeleton correspondant
                                nearestId = SkeletonManagementData[i].skeleton.TrackingId;
                            }
                        }
                    }

                    ///////////////////////// STICKMAN //////////////////////////

                    // Remove any previous skeletons.
                    StickMen.Children.Clear();

                    for (i = 0; i < SkeletonManagementData.Length; i++)
                    {
                        if (SkeletonManagementData[i].skeleton.TrackingState == SkeletonTrackingState.Tracked)
                        {
                            // Dessine un stickman en fond
                            SkeletonManagementData[i].stickman.DrawStickMan(Brushes.WhiteSmoke, 7);
                        }
                    }

                    for (i = 0; i < SkeletonManagementData.Length; i++)
                    {
                        // Only draw tracked skeletons.
                        if (SkeletonManagementData[i].skeleton.TrackingState == SkeletonTrackingState.Tracked)
                        {
                            // Pick a brush, Red for a skeleton that has recently gestures, black for the nearest, gray otherwise.
                            var brush = DateTime.UtcNow < highlightTime && SkeletonManagementData[i].skeleton.TrackingId == highlightId ? Brushes.Red :
                                SkeletonManagementData[i].skeleton.TrackingId == nearestId ? Brushes.Black : Brushes.Gray;

                            // Draw the individual skeleton.
                            SkeletonManagementData[i].stickman.DrawStickMan(brush, 3);
                        }


                        ////////////////////////// CURSEUR ///////////////////////////////

                        if (SkeletonManagementData[i].skeleton.TrackingId == nearestId)
                        {
                            skeletonFocus = SkeletonManagementData[i].skeleton;

                            if (skeletonFocus == null)
                            {
                                this.curseur.kinectButton.Visibility = Visibility.Collapsed;
                            }
                            else
                            {
                                /*this.curseur.timer.Tick += new EventHandler(this.curseur.TimerStop);
                                if (!this.curseur.timer.Enabled)
                                {
                                    this.curseur.TrackHand(sensor, skeletonFocus);
                                }*/
                                this.curseur.TrackHand(sensor, skeletonFocus);

                                if (gestureCamera.isGestureOn)
                                {
                                    HighlightSkeleton(skeletonFocus);
                                    gestureCamera.isGestureOn = false;
                                }
                            }

                            gestureCamera.OnGesture(SkeletonManagementData[i].skeleton);
                            nearestId = SkeletonManagementData[i].skeleton.TrackingId;


                        }
                    }
                }
            }
        }


        //When your window is loaded
        public void InitKinect(Canvas stickMen)
        {
            this.sensor = KinectSensor.KinectSensors[0];

            if (this.sensor.Status == KinectStatus.Connected)
            {

                var parameters = new TransformSmoothParameters
                {
                    Correction = 0.5f,
                    Prediction = 0.5f,
                    Smoothing = 0.05f,
                    JitterRadius = 0.8f,
                    MaxDeviationRadius = 0.2f
                    /*Smoothing = 0.5f,
                    Correction = 0.5f,
                    Prediction = 0.5f,
                    JitterRadius = 0.8f,
                    MaxDeviationRadius = 0.8f*/
                };

                // Set the settings to the Kinect
                this.sensor.SkeletonStream.Enable(parameters);
                this.sensor.DepthStream.Enable();
                this.sensor.SkeletonFrameReady += this.sensor_SkeletonFramesReady;
                this.sensor.Start();
                //sensor.ElevationAngle = Convert.ToInt32("10");
                sensor.ElevationAngle = Convert.ToInt32("0");

            }

            this.StickMen = stickMen;
        }
    }



}
