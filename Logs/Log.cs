using System;
using System.IO;
using System.Text.RegularExpressions;
using ZyxMeSocket.Entities;

namespace ZyxMeSocket.Logs
{
    public class Log
    {
        public static void Write(AppSettings appSettings, string Level, string Label, string Class, string Method, string Msg, string Path = "Logs")
        {
            try
            {
                LogPaths settings = new LogPaths();
                if (appSettings.LogPaths.Exists(x => x.Label == Label))
                {
                    settings = appSettings.LogPaths.Find(x => x.Label == Label).Parameters;
                }
                else
                {
                    settings = appSettings.LogPaths.Find(x => x.Label == "Default").Parameters;
                }

                if (settings.Enabled &&
                    (Level == LogEnum.DEBUG.ToString() && appSettings.LogSettings.Debug
                    || Level == LogEnum.INFO.ToString() && appSettings.LogSettings.Info
                    || Level == LogEnum.ERROR.ToString() && appSettings.LogSettings.Error))
                {
                    Msg = Regex.Replace(Msg, "\n", "");
                    Msg = Regex.Replace(Msg, "\r", "");
                    Msg = Regex.Replace(Msg, "    ", " ");
                    Msg = Regex.Replace(Msg, "   ", " ");
                    Msg = Regex.Replace(Msg, "  ", " ");

                    if (Directory.Exists(Path) == false)
                    {
                        Directory.CreateDirectory(Path);
                    }
                    string pathFile = (Directory.GetCurrentDirectory() + "\\" + Path + "\\" + DateTime.Now.ToString("yyyyMMdd") + "." + Label + "." + Class + ".log").Replace("\\\\", "\\");

                    if (File.Exists(pathFile))
                    {
                        using (StreamWriter sw = File.AppendText(pathFile))
                        {
                            sw.WriteLine(DateTime.Now.ToString("yyyyMMdd") + "." + DateTime.Now.ToString("HHmmss") + $" {Level} " + Method.Trim() + " --> " + Msg);
                        }
                    }
                    else
                    {
                        using (StreamWriter sw = File.CreateText(pathFile))
                        {
                            sw.WriteLine(DateTime.Now.ToString("yyyyMMdd") + "." + DateTime.Now.ToString("HHmmss") + $" {Level} " + Method.Trim() + " --> " + Msg);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
