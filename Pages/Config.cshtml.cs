using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.IO;

namespace MacReportViewer.Pages
{
    public class ConfigModel : PageModel
    {
        [BindProperty]
        public string MacAddress { get; set; }

        [BindProperty]
        public DeviceConfig Config { get; set; }

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

                    if (deviceData != null && deviceData.Config != null)
                    {
                        Config = deviceData.Config;
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Configuration not found in the file.");
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

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                string filePath = Path.Combine("../garlow/csharp/GarlowTcpServer/bin/Debug/net8.0/devices", $"{MacAddress}.json");

                if (System.IO.File.Exists(filePath))
                {
                    string jsonContent = System.IO.File.ReadAllText(filePath);
                    var deviceData = JsonConvert.DeserializeObject<DeviceData>(jsonContent);

                    if (deviceData != null)
                    {
                        deviceData.Config = Config;

                        jsonContent = JsonConvert.SerializeObject(deviceData, Formatting.Indented);
                        System.IO.File.WriteAllText(filePath, jsonContent);

                        TempData["SuccessMessage"] = "Configuration saved successfully.";
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Error loading the configuration.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "File not found.");
                }
            }

            return Page();
        }
    }

    // public class DeviceConfig
    // {
    //     public string State { get; set; }
    //     public string NetworkApn { get; set; }
    //     public string NetworkUser { get; set; }
    //     public string NetworkPassword { get; set; }
    //     public string NetworkServer { get; set; }
    //     public int NetworkPort { get; set; }
    //     public int ComunicationTimeOfDay1 { get; set; }
    //     public int ComunicationTimeOfDay2 { get; set; }
    //     public int ComunicationTimeOfDay3 { get; set; }
    //     public int ComunicationTimeOfDay4 { get; set; }
    //     public int Periodicity { get; set; }
    // }

    // public class DeviceData
    // {
    //     public DeviceConfig Config { get; set; }
    //     public List<Report> Reports { get; set; }
    // }

    // public class Report
    // {
    //     public int UsageLevel { get; set; }
    //     public int BatteryLevel { get; set; }
    //     public int NetworkLevel { get; set; }
    //     public DateTime Timestamp { get; set; }
    // }
}
