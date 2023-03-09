using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BRIMOPulseSettings
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        enum PulseSel
        {
            PS0,
            PS1,
            PS2,
            PS3,
            PS4,
            PS5,
            PS6,
            PS7,
            PS8,
            PS9
        }
        SerialPort? _port;
        Button[]? btnSel;
        PulseSel? _pulseSel;
        bool isConnected = false;
        AppSettings? _appSettings;

        public MainWindow()
        {
            InitializeComponent();

            //_appSettings = new AppSettings();
            btnCheck.Visibility = Visibility.Collapsed;
            //txtResponse.Visibility = Visibility.Collapsed;
            btnSel = new Button[] { btn25ns, btn50ns, btn75ns, btn100ns, btn125ns, btn150ns, btn175ns, btn200ns, btn225ns, btn250ns };
            //_pulseSel = PulseSel.P25ns;
            //btnSel[0].Background = Brushes.Lime;
            _port = new SerialPort();

            Loaded += delegate 
            {
                try
                {
                    using var stream = new FileStream("settings.json", FileMode.Open, FileAccess.Read);
                    _appSettings = JsonSerializer.Deserialize<AppSettings>(stream);
                    

                }
                catch
                {
                    _appSettings = new AppSettings();
                }
                for (var i = 0; i < 10; i++)
                {
                    btnSel[i].Content = _appSettings?.PulseSettings[i].Split(':')[0];
                }
            };

            Closed += delegate 
            {
                if(_port.IsOpen)
                {
                    _port.Close();
                }
                using var stream = new FileStream("settings.json", FileMode.Create, FileAccess.Write);
                JsonSerializer.Serialize(stream, _appSettings);
            };

            btnConnect.Click += delegate 
            {
                if (string.IsNullOrEmpty(_appSettings?.PortName))
                    throw new ApplicationException();

                if(isConnected)
                {
                    _port.Close();
                    isConnected = false;
                    btnConnect.Background = Brushes.Gray;
                    return;
                }
                try
                {
                    _port = new SerialPort();
                    _port.PortName = _appSettings.PortName;
                    _port.BaudRate = 9600;
                    _port.NewLine = "\r";
                    _port.Open();
                    btnConnect.Background = Brushes.Lime;
                    isConnected = true;
                }
                catch
                {
                    MessageBox.Show("発振器接続エラー.\r\nケーブルの接続やポート番号を確認してください.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    isConnected = false;
                    btnConnect.Background = Brushes.DarkGray;
                }
            };
            
            void ChangePulseSel(int index, PulseSel sel)
            {
                if (_pulseSel == sel)
                {
                    _pulseSel = null;
                    txtCommand.Text = "";
                    btnSel[index].Background = Brushes.LightGray;

                    return;
                }
                _pulseSel = sel;
                for (var i = 0; i < btnSel.Length; i++)
                {
                    if (i == index)
                    {
                        btnSel[i].Background = Brushes.Lime;
                    }
                    else
                    {
                        btnSel[i].Background = Brushes.LightGray;
                    }
                }
                switch (sel)
                {
                    case PulseSel.PS0:
                        txtCommand.Text = _appSettings?.PulseSettings[0].Split(':')[1];
                        break;
                    case PulseSel.PS1:
                        txtCommand.Text = _appSettings?.PulseSettings[1].Split(':')[1];
                        break;
                    case PulseSel.PS2:
                        txtCommand.Text = _appSettings?.PulseSettings[2].Split(':')[1];
                        break;
                    case PulseSel.PS3:
                        txtCommand.Text = _appSettings?.PulseSettings[3].Split(':')[1];
                        break;
                    case PulseSel.PS4:
                        txtCommand.Text = _appSettings?.PulseSettings[4].Split(':')[1];
                        break;
                    case PulseSel.PS5:
                        txtCommand.Text = _appSettings?.PulseSettings[5].Split(':')[1];
                        break;
                    case PulseSel.PS6:
                        txtCommand.Text = _appSettings?.PulseSettings[6].Split(':')[1];
                        break;
                    case PulseSel.PS7:
                        txtCommand.Text = _appSettings?.PulseSettings[7].Split(':')[1];
                        break;
                    case PulseSel.PS8:
                        txtCommand.Text = _appSettings?.PulseSettings[8].Split(':')[1];
                        break;
                    case PulseSel.PS9:
                        txtCommand.Text = _appSettings?.PulseSettings[9].Split(':')[1];
                        break;
                }
                
            }
            //void ChangePulseSelBackgrandColor(Button btn)
            //{
            //    for (var i = 0; i < btnSel.Length; i++)
            //    {
            //        if (btnSel[i] == btn)
            //        {
            //            btnSel[i].Background = Brushes.Lime;
            //        }
            //        else
            //        {
            //            btnSel[i].Background = Brushes.LightGray;
            //        }
            //    }
            //}
            btnSel[0].Click += delegate 
            {
                //_pulseSel = PulseSel.P25ns;
                //ChangePulseSelBackgrandColor(btnSel[0]);
                //txtCommand.Text = "S20;0";
                ChangePulseSel(0, PulseSel.PS0);
                
            };
            btnSel[1].Click += delegate
            {
                ChangePulseSel(1, PulseSel.PS1);
            };
            btnSel[2].Click += delegate
            {
                ChangePulseSel(2, PulseSel.PS2);
            };
            btnSel[3].Click += delegate
            {
                ChangePulseSel(3, PulseSel.PS3);
            };
            btnSel[4].Click += delegate
            {
                ChangePulseSel(4, PulseSel.PS4);
            };
            btnSel[5].Click += delegate
            {
                ChangePulseSel(5, PulseSel.PS5);
            };
            btnSel[6].Click += delegate
            {
                ChangePulseSel(6, PulseSel.PS6);
            };
            btnSel[7].Click += delegate
            {
                ChangePulseSel(7, PulseSel.PS7);
            };
            btnSel[8].Click += delegate
            {
                ChangePulseSel(8, PulseSel.PS8);
            };
            btnSel[9].Click += delegate
            {
                ChangePulseSel(9, PulseSel.PS9);
            };

            btnSend.Click += delegate 
            {
                if(_port == null)
                {
                    throw new ApplicationException("");
                }
                if(_pulseSel == null)
                {
                    MessageBox.Show("パルス幅が選択されていません.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (!isConnected)
                {
                    MessageBox.Show("発振器に接続されていません.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                var cmd = txtCommand.Text.Trim();
                _port.WriteLine(cmd);
                var res = _port.ReadLine();
                txtResponse.Text = res;
            };
            btnCheck.Click += delegate
            {
                if (_port == null)
                {
                    throw new ApplicationException("");
                }
                if(!isConnected)
                {
                    MessageBox.Show("発振器に接続されていません.", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                _port.WriteLine("S19;");
                var res = _port.ReadLine();
                txtResponse.Text = res;
            };

            btnSettings.Click += delegate 
            {
                var w = new Window1(_appSettings);
                w.ShowDialog();
            };
        }
    }

    public class AppSettings
    {
        public string PortName { get; set; } = "COM1";
        public IList<string> PulseSettings { get; set; } = new string[] 
            { 
                "25ns:$19;0",
                "50ns:$19;1",
                "75ns:$19;2",
                "100ns:$19;3",
                "125ns:$19;4",
                "150ns:$19;5",
                "175ns:$19;6",
                "200ns:$19;7",
                "250ns:$19;8",
                "300ns:$19;9",
            }.ToList();
    }
}
