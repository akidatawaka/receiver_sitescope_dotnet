using sis_receiver.Entities.Config;
using sis_receiver_library.Helper;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using sis_receiver_library;
using System.Collections.Generic;
using System.Xml;

namespace sis_receiver.Services
{
    public class SisReceiverService : IHostedService, IDisposable
    {
        private readonly IOptions<SisReceiverConfig> _configuration;
        private static readonly HttpClient client = new HttpClient();
        private Timer timer;


        public SisReceiverService(IOptions<SisReceiverConfig> configuration)
        {
            _configuration = configuration;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Log.Information("[SisReceiverService::StartAsync] Starting daemon");
            Log.Information("[SisReceiverService::StartAsync] Monitoring Directory : " + _configuration.Value.InputDirectory);

            try
            {
                timer = new Timer(RunJob, null, TimeSpan.Zero, TimeSpan.FromSeconds(Convert.ToInt16(_configuration.Value.Interval)));
            }
            catch (Exception Ex)
            {
                Log.Error("[SisReceiverService::ExecuteAsync] " + Ex.ToString());
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Log.Information("[SisReceiverService::StopAsync] Stopping daemon");
            timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
            Log.Information("[SisReceiverService::Dispose] Disposing daemon");
        }

        string type;
        string target;
        string targetIP;
        string time;
        string quality;
        string name;
        string value;
        string counterQuality;
        string counterName;

        


        // Define the event handlers.
        private void RunJob(object state)
        {
            List<String> metric = new List<String>();
            try
            {
                string inputDirectory = _configuration.Value.InputDirectory;
                string decompressDirectory = _configuration.Value.DecompressDirectory;
                string outputDirectory = _configuration.Value.OutputDirectory;

                DirectoryInfo inputDirectoryInfo = new DirectoryInfo(inputDirectory);
                FileInfo[] _filesInfo = inputDirectoryInfo.GetFiles();

                //string fileForTesting = decompressDirectory + @"\20190910164908SIS.xml";
                //string fileForTesting = decompressDirectory + @"\20190910611PM.sis.xml";

                string[] allFileNames = Directory.GetFiles(decompressDirectory);

                foreach(string fileName in allFileNames)
                {                    
                    string completeFilePath = fileName;

                    Console.WriteLine(completeFilePath);

                    XmlDocument doc = new XmlDocument();
                    doc.Load(completeFilePath);

                    //XmlNode rootNode = doc.DocumentElement;

                    XmlNodeList elementList = doc.GetElementsByTagName("monitor");

                    foreach (XmlNode element in elementList)
                    {
                        foreach (XmlAttribute attribute in element.Attributes)
                        {
                            if (attribute.Name == "type")
                            {
                                type = attribute.Value;
                            }
                            else if (attribute.Name == "target")
                            {
                                target = attribute.Value;
                            }
                            else if (attribute.Name == "targetIP")
                            {
                                targetIP = attribute.Value;
                            }
                            else if (attribute.Name == "time")
                            {
                                time = attribute.Value;
                            }
                            else if (attribute.Name == "quality")
                            {
                                quality = attribute.Value;
                            }
                            else if (attribute.Name == "name")
                            {
                                name = attribute.Value;
                            }
                        }

                        foreach (XmlNode counter in element.ChildNodes)
                        {
                            foreach (XmlAttribute counterAttribute in counter.Attributes)
                            {
                                if (counterAttribute.Name == "value")
                                {
                                    value = counterAttribute.Value;
                                }
                                else if (counterAttribute.Name == "quality")
                                {
                                    counterQuality = counterAttribute.Value;
                                }
                                else if (counterAttribute.Name == "name")
                                {
                                    counterName = counterAttribute.Value;
                                }
                            }
                            metric.Add(type + "|" + target + "|" + targetIP + "|" + time + "|" + quality + "|" + name + "|" + value + "|" + counterQuality + "|" + counterName);
                            //Console.WriteLine(type + "|" + target + "|" + targetIP + "|" + time + "|" + quality + "|" + name + "|" + value + "|" + counterQuality + "|" + counterName);
                        }
                    }
                }

                //SitescopeLibrary.Decompress(_filesInfo);
                //string date = DateTime.Now.ToString("yyyyMMddhmmtt");
                string date = DateTime.Now.ToString("yyyyMMddHHmmssFFF");

                using (var file = File.CreateText(outputDirectory + @"/" + date + ".csv"))
                {
                    foreach(string line in metric)
                    {
                        file.WriteLine(line);
                    }
                }
            }
            catch (Exception Ex)
            {
                Log.Error("[SisReceiverService::RunJob] " + Ex.ToString());
            }

        }

    }
}
