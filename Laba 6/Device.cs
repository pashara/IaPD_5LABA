using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Laba_6
{
    public class Device
    {

        private ManagementObject _device;
        private ManagementObject _driver;


        public string DeviceID
        {
            get
            {
                return asdasd(_device["DeviceID"]);
            }
        }
        public string Title
        {
            get
            {
                return asdasd(_device["Name"]);
            }
        }

        private string asdasd(object a)
        {
            if (a != null)
            {
                return a.ToString();
            }
            return "";
        }


        public string GuID
        {
            get
            {
                return asdasd(_device["ClassGuid"]);
            }
        }



        public string Hardware
        {
            get
            {
                var hardware = _device["HardwareID"] as string[];
                string result = string.Empty;
                foreach (var temp in hardware)
                {
                    if (temp != "(null)")
                    {
                        result += temp + "    ";
                    }
                }
                return result;
            }
        }


        public string Manufacturer
        {
            get
            {
                return asdasd(_device["Manufacturer"]);
            }
        }

        public string DriverDescription
        {
            get
            {
                if (_driver != null)
                {

                    return asdasd(_driver["Description"]);
                }
                return "";
            }
        }
        public string DriverPath
        {
            get
            {
                if (_driver != null)
                {
                    return asdasd(_driver["PathName"]);
                }
                return "";
            }
        }

        public bool IsEnable
        {
            get
            {
                var code = _device["ConfigManagerErrorCode"];
                return !(Convert.ToInt32(code) == 22);
            }
        }



        public Device(ManagementObject device, ManagementObject driver)
        {
            _device = device;
            _driver = driver;
        }

        public Device()
        {
        }

        public bool Disconnect()
        {
            try
            {
                _device.InvokeMethod("Disable", null);
            }
            catch (ManagementException)
            {
                return false;
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
            }
            return true;
        }

        public bool Connect()
        {
            try
            {
                _device.InvokeMethod("Enable", null);
            }
            catch (ManagementException)
            {
                return false;
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e);
            }
            return true;
        }

    }
}
