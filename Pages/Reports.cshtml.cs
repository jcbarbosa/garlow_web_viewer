using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace MacReportViewer.Pages
{
    public class ReportsModel : PageModel
    {
        public string MacAddress { get; set; }

        public List<Report> Reports { get; set; } = new List<Report>();

        public void OnGet(string macAddress)
        {
            MacAddress = macAddress;

            if (!string.IsNullOrEmpty(macAddress))
            {
                string filePath = Path.Combine("../garlow/csharp/GarlowTcpServer/bin/Debug/net8.0/devices", $"{macAddress}.json");

                if (System.IO.File.Exists(filePath))
                {
                    string jsonContent = System.IO.File.ReadAllText(filePath);
                    var deviceData = JsonConvert.DeserializeObject<DeviceData>(jsonContent);

                    if (deviceData != null && deviceData.Reports != null)
                    {
                        Reports = deviceData.Reports.OrderByDescending(r => r.Timestamp).ToList();
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "No reports found in the file.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "File not found.");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "MAC address is required.");
            }
        }
    }

    // public class Report
    // {
    //     public int UsageLevel { get; set; }
    //     public int BatteryLevel { get; set; }
    //     public int NetworkLevel { get; set; }
    //     public DateTime Timestamp { get; set; }
    // }

    // public class DeviceData
    // {
    //     public List<Report> Reports { get; set; }
    // }
}
