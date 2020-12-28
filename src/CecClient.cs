using System;
using CecSharp;

namespace kek
{
    class CecClient : CecCallbackMethods
    {
        private int LogLevel;
        private LibCecSharp Lib;
        private LibCECConfiguration Config;

        public CecClient()
        {
            Config = new LibCECConfiguration();
            Config.DeviceTypes.Types[0] = CecDeviceType.PlaybackDevice;
            Config.DeviceName = "kek";
            Config.ClientVersion = LibCECConfiguration.CurrentVersion;
            Config.HDMIPort = 2; // TODO: make this configurable
            LogLevel = (int)CecLogLevel.All;

            Lib = new LibCecSharp(this, Config);
        }

        public override int ReceiveLogMessage(CecLogMessage message)
        {
            if (((int)message.Level & LogLevel) == (int)message.Level)
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
            CecAdapter[] adapters = Lib.FindAdapters(string.Empty);
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
            return Lib.Open(port, timeout);
        }

        public void Close()
        {
            Lib.Close();
        }

        public void SetActiveSource()
        {
            Lib.SetActiveSource(CecDeviceType.PlaybackDevice);
        }
    }
}
