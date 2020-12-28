using System;
using CecSharp;

namespace kek
{
    class CecClient : CecCallbackMethods
    {
        private int logLevel;
        private LibCecSharp lib;
        private LibCECConfiguration config;

        public CecClient()
        {
            config = new LibCECConfiguration();
            config.DeviceTypes.Types[0] = CecDeviceType.PlaybackDevice;
            config.DeviceName = "kek";
            config.ClientVersion = LibCECConfiguration.CurrentVersion;
            config.HDMIPort = 2; // TODO: make this configurable
            logLevel = (int)CecLogLevel.All;

            lib = new LibCecSharp(this, config);
        }

        public override int ReceiveLogMessage(CecLogMessage message)
        {
            if (((int)message.Level & logLevel) == (int)message.Level)
            {
                string strLevel = "";
                switch (message.Level)
                {
                    case CecLogLevel.Error:
                        strLevel = "ERROR:   ";
                        break;
                    case CecLogLevel.Warning:
                        strLevel = "WARNING: ";
                        break;
                    case CecLogLevel.Notice:
                        strLevel = "NOTICE:  ";
                        break;
                    case CecLogLevel.Traffic:
                        strLevel = "TRAFFIC: ";
                        break;
                    case CecLogLevel.Debug:
                        strLevel = "DEBUG:   ";
                        break;
                    default:
                        break;
                }
                string strLog = string.Format("{0} {1,16} {2}", strLevel, message.Time, message.Message);
                Console.WriteLine(strLog);
            }
            return 1;
        }

        public bool Connect(int timeout)
        {
            CecAdapter[] adapters = lib.FindAdapters(string.Empty);
            if (adapters.Length > 0)
            {
                return Connect(adapters[0].ComPort, timeout);
            }
            else
            {
                Console.WriteLine("Did not find any CEC adapters");
                return false;
            }
        }

        public bool Connect(string port, int timeout)
        {
            return lib.Open(port, timeout);
        }

        public void Close()
        {
            lib.Close();
        }

        public void SetActiveSource()
        {
            lib.SetActiveSource(CecDeviceType.PlaybackDevice);
        }
    }
}
