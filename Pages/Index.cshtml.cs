using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MacReportViewer.Pages
{
    public class IndexModel : PageModel
    {
        public List<DeviceInfo> Devices { get; set; } = new List<DeviceInfo>();

        public void OnGet()
        {
            string dataFolderPath = Path.Combine("../garlow/csharp/GarlowTcpServer/bin/Debug/net8.0", "devices");

            if (Directory.Exists(dataFolderPath))
            {
                var files = Directory.GetFiles(dataFolderPath, "*.json");

                foreach (var file in files)
                {
                    string jsonContent = System.IO.File.ReadAllText(file);
                    var deviceData = JsonConvert.DeserializeObject<DeviceData>(jsonContent);

                    if (deviceData != null && deviceData.Reports != null && deviceData.Reports.Count > 0)
                    {
                        var lastReport = deviceData.Reports.OrderByDescending(r => r.Timestamp).FirstOrDefault();
                        if (lastReport != null)
                        {
                            Devices.Add(new DeviceInfo
                            {
                                MacAddress = Path.GetFileNameWithoutExtension(file),
                                LastReportTimestamp = lastReport.Timestamp
                            });
                        }
                    }
                    else
                    {
                        Devices.Add(new DeviceInfo
                        {
                            MacAddress = Path.GetFileNameWithoutExtension(file),
                            LastReportTimestamp = null
                        });
                    }
                }
            }
        }
    }

    public class DeviceInfo
    {
        public string MacAddress { get; set; }
        public DateTime? LastReportTimestamp { get; set; }
    }

    public enum ContainerAccessControllerConfigState
    {
        UpdatePending,
        Commited,
    }

    public class DeviceConfig
    {
        public required ContainerAccessControllerConfigState State { get; set; }
        public string? NetworkApn { get; set; }
        public string? NetworkUser { get; set; }
        public string? NetworkPassword { get; set; }
        public required string NetworkServer { get; set; }
        public int NetworkPort { get; set; }
        public int ComunicationTimeOfDay1 { get; set; }
        public int ComunicationTimeOfDay2 { get; set; }
        public int ComunicationTimeOfDay3 { get; set; }
        public int ComunicationTimeOfDay4 { get; set; }
        public int Periodicity { get; set; }
    }

    // public class DeviceConfig
    // {
    //     public required string State { get; set; }
    //     public string? NetworkApn { get; set; }
    //     public string? NetworkUser { get; set; }
    //     public string? NetworkPassword { get; set; }
    //     public required string NetworkServer { get; set; }
    //     public int NetworkPort { get; set; }
    //     public int ComunicationTimeOfDay1 { get; set; }
    //     public int ComunicationTimeOfDay2 { get; set; }
    //     public int ComunicationTimeOfDay3 { get; set; }
    //     public int ComunicationTimeOfDay4 { get; set; }
    //     public int Periodicity { get; set; }
    // }

    public class DeviceData
    {
        public DeviceConfig Config { get; set; }
        public List<Report> Reports { get; set; }
    }

    public class Report
    {
        public int UsageLevel { get; set; }
        public int BatteryLevel { get; set; }
        public int NetworkLevel { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
