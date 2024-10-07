using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
using DBDesign.PosiStageDotNet;


namespace PSNtool
{


    public partial class MainWindow : Window
    {
        public PsnServer PsnServer { get; set; }
        public PsnClient PsnClient { get; set; }

        public PsnTracker[] trackers = new[]
{
                new PsnTracker(0, "T 1",
                    position: Tuple.Create(0f, 0f, 0f),
                    speed: Tuple.Create(0f, 0f, 0f),
                    orientation: Tuple.Create(0f, 0f, 0f)),
                new PsnTracker(1, "T 2",
                    position: Tuple.Create(0f, 0f, 0f),
                    speed: Tuple.Create(0f, 0f, 0f),
                    orientation: Tuple.Create(0f, 0f, 0f)),
                new PsnTracker(2, "T 3",
                    position: Tuple.Create(0f, 0f, 0f),
                    speed: Tuple.Create(0f, 0f, 0f),
                    orientation: Tuple.Create(0f, 0f, 0f)),
                new PsnTracker(3, "T 4",
                    position: Tuple.Create(0f, 0f, 0f),
                    speed: Tuple.Create(0f, 0f, 0f),
                    orientation: Tuple.Create(0f, 0f, 0f))
        };

        public PsnServer psnServer;

        public float x1 = 0;
        public float x2 = 0;
        public float x3 = 0;
        public float x4 = 0;

        public float y1 = 0;
        public float y2 = 0;
        public float y3 = 0;
        public float y4 = 0;

        public float z1 = 0;
        public float z2 = 0;
        public float z3 = 0;
        public float z4 = 0;

        public double X = 0;
        public double Y = 0;
        public double Z = 0;



        public MainWindow()
        {
            InitializeComponent();
            GetLocalIp();
            OpenBnt.Tag="off";

        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void CloseBnt_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void MiniBtn_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void AboutBnt_Click(object sender, RoutedEventArgs e)
        {

        }
        private void TgBnt_Click(object sender, RoutedEventArgs e)
        {
            if (TgBnt.IsChecked.Value)
            {
                ModeLabel.Content = "输出/OUTPUT";
                ModeLabel.Foreground = System.Windows.Media.Brushes.DodgerBlue;
            }
            else
            {
                ModeLabel.Content = "输入/INPUT";
                ModeLabel.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }
        public  void GetLocalIp()
        {
            IPbox.Items.Add("127.0.0.1");
            IPHostEntry IpEntry = Dns.GetHostEntry(Dns.GetHostName());
            for (int i = 0; i < IpEntry.AddressList.Length; i++)
            {
                if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                {
                    string strMyIP = IpEntry.AddressList[i].ToString();

                    IPbox.Items.Add(strMyIP);
                }
            }

        }
        private void OpenBnt_Click(object sender, RoutedEventArgs e)
        {
            string  s=OpenBnt.Tag.ToString();
            if (s == "off")
            {
                if (TgBnt.IsChecked.Value)
                {
                    OpenServer(IPbox.SelectedItem.ToString());
                }
                else
                {
                    OpenClient(IPbox.SelectedItem.ToString());
                }
                OpenBnt.Background = System.Windows.Media.Brushes.DarkOrange;
                OpenBnt.Content = "停止/STOP";
                OpenBnt.Tag = "on";
            } else if (s=="on")
            {
                OpenBnt.Background = System.Windows.Media.Brushes.DodgerBlue;
                OpenBnt.Content = "开启/OPEN";
                OpenBnt.Tag = "off";
             // Offpsn();
    
            }
        

        }
        public  void OpenServer(string ipAddress)
       {

            IPAddress HIPAddress = IPAddress.Parse(ipAddress);
            psnServer=new("PSNtool", HIPAddress);
            psnServer.SetTrackers(trackers);
            psnServer.StartSending();


        }
        public static void OpenClient(string Clientip) 
        {
            IPAddress CIPaddress = IPAddress.Parse(Clientip);
            var psnClient = new PsnClient(CIPaddress);
            psnClient.TrackersUpdated += (s,e) =>
            {
                foreach (var t in e.Values)
                Console.WriteLine(t);
            };
            psnClient.StartListening();
        }
        public void Offpsn() 
        {   
            if (TgBnt.IsChecked.Value)
            {

                PsnServer.RemoveAllTrackers();
                PsnServer.Dispose();
                PsnServer.StopSending();
       
            } else
            {
                PsnClient.StopListening();
                PsnClient.Dispose();
           
            }
          
        }

        private void SX_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            X=SX.Value;
            Tboxchange();
            psnchange();

        }

        private void SY_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Y = SY.Value;
            Tboxchange();
            psnchange();

        }

        private void SZ_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Z = SZ.Value;
            Tboxchange();
            psnchange();

        }

        private void Xrbnt_Click(object sender, RoutedEventArgs e)
        {
            SX.Value = 0;
            if (T1R.IsChecked.Value)
            {
                T1txtX.Text = "0";
            } else if (T2R.IsChecked.Value) 
            {
                T2txtX.Text = "0";
            }
            else if(T3R.IsChecked.Value)
            {
                T3txtX.Text = "0";
            }
            else if (T4R.IsChecked.Value)
            {
                T4txtX.Text = "0";
            }
        }

        private void Yrbnt_Click(object sender, RoutedEventArgs e)
        {
            SY.Value = 0;
            if (T1R.IsChecked.Value)
            {
                T1txtY.Text = "0";
            }
            else if (T2R.IsChecked.Value)
            {
                T2txtY.Text = "0";
            }
            else if (T3R.IsChecked.Value)
            {
                T3txtY.Text = "0";
            }
            else if (T4R.IsChecked.Value)
            {
                T4txtY.Text = "0";
            }
        }

        private void Zrbnt_Click(object sender, RoutedEventArgs e)
        {
            SZ.Value = 0;
            if (T1R.IsChecked.Value)
            {
                T1txtZ.Text = "0";
            }
            else if (T2R.IsChecked.Value)
            {
                T2txtZ.Text = "0";
            }
            else if (T3R.IsChecked.Value)
            {
                T3txtZ.Text = "0";
            }
            else if (T4R.IsChecked.Value)
            {
                T4txtZ.Text = "0";
            }
        }

        public void Tboxchange()
        {
           

            if (T1R.IsChecked.Value)
            {
                T1txtX.Text = Convert.ToString(X);
                x1 = (float)X;
                T1txtY.Text = Convert.ToString(Y);
                y1 = (float)Y;
                T1txtZ.Text = Convert.ToString(Z);
                z1 = (float)Z;
               

            }
            else if (T2R.IsChecked.Value)
            {
                T2txtX.Text = Convert.ToString(X);
                x2 = (float)X;
                T2txtY.Text = Convert.ToString(Y);
                y2 = (float)Y;
                T2txtZ.Text = Convert.ToString(Z);
                z2 = (float)Z;
              

            }
            else if (T3R.IsChecked.Value)
            {
                T3txtX.Text = Convert.ToString(X);
                x3 = (float)X;
                T3txtY.Text = Convert.ToString(Y);
                y3 = (float)Y;
                T3txtZ.Text = Convert.ToString(Z);
                z3 = (float)Z;
            

            }
            else if (T4R.IsChecked.Value)
            {
                T4txtX.Text = Convert.ToString(X);
                x4 = (float)X;
                T4txtY.Text = Convert.ToString(Y);
                y4 = (float)Y;
                T4txtZ.Text = Convert.ToString(Z);
                z4 = (float)Z;
               

            }

        }

        public void psnchange() 
        {
            var tracker1Update = trackers[0].WithPosition(Tuple.Create(x1, y1, z1));

            var tracker2Update = trackers[1].WithPosition(Tuple.Create(x2, y2, z2));

            var tracker3Update = trackers[2].WithPosition(Tuple.Create(x3, y3, z3));

            var tracker4Update = trackers[3].WithPosition(Tuple.Create(x4, y4, z4));

            string go= OpenBnt.Tag.ToString();

            if (go=="on")
            {
                if (T1R.IsChecked.Value)
                {
                    psnServer.UpdateTrackers(tracker1Update);
                }
                else if (T2R.IsChecked.Value)
                {
                    psnServer.UpdateTrackers(tracker2Update);
                }
                else if (T3R.IsChecked.Value)
                {
                    psnServer.UpdateTrackers(tracker3Update);
                }
                else if (T4R.IsChecked.Value)
                {
                    psnServer.UpdateTrackers(tracker4Update);
                }
            }
        }

        private void SuJi_Click(object sender, RoutedEventArgs e)
        {
            randoms();

        }
        public void randoms() 
        {
            Random r = new Random();
            double num1 = r.Next(-100, 100);
            double num2 = r.Next(-100, 100);
            double num3 = r.Next(-100, 100);
            SX.Value = num1;
            SY.Value = num2;
            SZ.Value = num3;
        }

    }
}
   
