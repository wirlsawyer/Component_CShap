using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Component
{
    public class NSSerialPortInfo
    {
        public String ComPort = "";
        public String DeviceID = "";
        public String Caption = "";
        public String Description = "";
    }
    public class NSSerialPort
    {
        static public List<NSSerialPortInfo> Search()
        {
            return NSSerialPort.Search("");
        }

        static public List<NSSerialPortInfo> Search(String keypoint)
        {
            List<NSSerialPortInfo> result = new List<NSSerialPortInfo>();

            var check = new System.Text.RegularExpressions.Regex("(COM[1-9][0-9]?[0-9]?)");

            ManagementClass mcPnPEntity = new ManagementClass("Win32_PnPEntity");
            ManagementObjectCollection manageObjCol = mcPnPEntity.GetInstances();

            foreach (ManagementObject manageObj in manageObjCol)
            {
                String namePropertyValue = (String)manageObj.GetPropertyValue("Name");
                String PropertyValue1 = (String)manageObj.GetPropertyValue("DeviceID");
                String PropertyValue2 = (String)manageObj.GetPropertyValue("Caption");
                String PropertyValue3 = (String)manageObj.GetPropertyValue("Description");

                if (namePropertyValue == null)
                {
                    continue;
                }

                string name = namePropertyValue.ToString();
                if (check.IsMatch(name))
                {
                    
                    NSSerialPortInfo info = new NSSerialPortInfo();


                    String pattern = @"COM[0-9]*[\)]$";
                    info.ComPort = NSRegex.Do(namePropertyValue, pattern).Replace(")", "");
  
                    info.DeviceID = PropertyValue1;
                    info.Caption = PropertyValue2;
                    info.Description = PropertyValue3;
                    if (keypoint.Length == 0)
                    {
                        result.Add(info);
                    }
                    else if (namePropertyValue.Contains(keypoint))
                    {
                        result.Add(info);
                    }

                }
            }

            return result;
        }
        /*
        static public List<NSSerialPortInfo> Search(String keypoint)
        {
            List<NSSerialPortInfo> result = new List<NSSerialPortInfo>();

            using (var searcher = new ManagementObjectSearcher("SELECT * FROM WIN32_SerialPort"))
            //using (var searcher = new ManagementObjectSearcher("root\\WMI", "SELECT * FROM WIN32_SerialPort"))
            {
                string[] portnames = SerialPort.GetPortNames();
                var ports = searcher.Get().Cast<ManagementBaseObject>().ToList();
                var tList = (from n in portnames
                             join p in ports on n equals p["DeviceID"].ToString()
                             select n + "●" + p["Caption"] + "●" + p["Description"]).ToList();

                foreach (string s in tList)
                {

                    NSSerialPortInfo info = new NSSerialPortInfo();


                    String pattern = @"^\D{3}\S";
                    info.ComPort = NSRegex.Do(s, pattern);
                    String[] items = s.Split('●');
                    info.DeviceID = items[0];
                    info.Caption = items[1];
                    info.Description = items[2];
                    if (keypoint.Length == 0)
                    {
                        result.Add(info);
                    }
                    else if (s.Contains(keypoint))
                    {
                        result.Add(info);
                    }
                    //Console.WriteLine(s);
                }
            }

            return result;
        }
        */
    }
}
