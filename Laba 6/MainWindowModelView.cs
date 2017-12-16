using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Laba_6
{
    public class MainWindowModelView : INotifyPropertyChanged
    {
        #region Fields

        private string _programTitle = "Device manager";
        public string H1Title => "Devices Manager";

        #endregion


        #region Properties

        public string ProgramTitle
        {
            get
            {
                return _programTitle;
            }
            private set
            {
                _programTitle = value;
                OnPropertyChanged(nameof(ProgramTitle));
            }
        }

        public ObservableCollection<Device> DevicesList
        {
            get
            {
                return _deviceList;
            }
        }

        #endregion




        DeviceManager manager = new DeviceManager();
        ObservableCollection<Device> _deviceList = new ObservableCollection<Device>();

        

        Device _selectedDevice = new Device();
        public int SelectedDeviceIndex
        {
            get;
            set;
        } 
        public Device SelectedDevice
        {
            get
            {
                return _selectedDevice;
            }
            set
            {
                _selectedDevice = value;
                OnPropertyChanged(nameof(SelectedDevice));
            }
        }

        private void UpdateInfoInAnotherThread()
        {
            new Thread(() =>
            {
                _deviceList = manager.Devices;

                OnPropertyChanged(nameof(DevicesList));
                OnPropertyChanged(nameof(SelectedDevice));
            }).Start();
        }

        public MainWindowModelView()
        {
            MainWindow.OnSelectedDevice += MainWindow_OnSelectedDevice;
            MainWindow.OnMouseDeviceDoubleClick += MainWindow_OnMouseDeviceDoubleClick;


            UpdateInfoInAnotherThread();
        }

        private void MainWindow_OnSelectedDevice(Device device)
        {
            SelectedDevice = device;
        }

        private void MainWindow_OnMouseDeviceDoubleClick(Device device)
        {
            if (device.IsEnable)
            {
                if (!device.Disconnect())
                {
                    MessageBox.Show( "Не могу отключить устройство", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Устройство отключено","Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                    UpdateInfoInAnotherThread();
                }
            }
            else
            {

                if (!device.Connect())
                {
                    MessageBox.Show("Не могу включить устройство", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show( "Устройство включено", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                    UpdateInfoInAnotherThread();
                }
            }

        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
