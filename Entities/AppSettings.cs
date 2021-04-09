using System;
using System.Collections.Generic;

namespace ZyxMeSocket.Entities
{
    public class AppSettings
    {
        public string SymmetricKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Environment { get; set; }
        public List<Environments> Environments { get; set; }
        public LogSettings LogSettings { get; set; }
        public List<LogSettings> LogPaths { get; set; }
        public Dictionary<string, string> Queries { get; set; }
        public string ReloadTime { get; set; }
        public string[] PersonFields { get; set; }
    }

    public class Environments
    {
        public string Label { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
        public ExternalPlatforms ExternalPlatforms { get; set; }
        public string FtpAttachment { get; set; }
    }

    public class ConnectionStrings
    {
        public string PostgresSQL { get; set; }
    }

    public class ExternalPlatforms
    {
        public string App { get; set; }
        public string Api { get; set; }
    }

    public class LogSettings
    {
        public bool Debug { get; set; }
        public bool Info { get; set; }
        public bool Error { get; set; }
        public string Label { get; set; }
        public LogPaths Parameters { get; set; }
    }
    public class LogPaths
    {
        public bool Enabled { get; set; }
        public string Path { get; set; }
    }

    public enum LogEnum
    {
        DEBUG,
        INFO,
        ERROR
    }
}
