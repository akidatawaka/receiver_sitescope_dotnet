using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using sis_receiver_api.Entities.Config;
using Serilog;
using System.IO;
using System.Net.Http;
using System.Text;

namespace sis_receiver_api.Controllers
{
    [Route("sitescope/[controller]/")]
    [ApiController]
    public class Receiver : ControllerBase
    {
        private readonly IOptions<SisReceiverApiConfig> _configuration;

        public Receiver(IOptions<SisReceiverApiConfig> configuration)
        {
            _configuration = configuration;
        }

        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        // POST api/values
        //[HttpPost]        
        //public async Task<IActionResult> Post()
        //{
        //    try
        //    {
        //        string date = DateTime.Now.ToString("MM\\/dd\\/yyyy h\\:mm tt");
        //        string directoryOutput = _configuration.Value.DirectoryOutput;

        //        using (StreamWriter outputFile = new StreamWriter(Path.Combine(directoryOutput, date + "File.txt")))
        //        {
        //            outputFile.WriteLine();
        //        }

        //        //using (StreamReader stream = new StreamReader(Request.Body))
        //        //{
        //        //    string content = stream.ReadToEnd();
        //        //    System.IO.File.WriteAllBytes(directoryOutput + "/" + date  +  "tes.log", Encoding.UTF8.GetBytes(content));
        //        //}

        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error("Sis Receiver Api Receiver : Post : " + ex.ToString());
        //    }

        //    return Ok();
        //}

        public async Task<IActionResult> ReadRawData()
        {
            try
            {
                string date = DateTime.Now.ToString("yyyyMMddhmmtt");
                string outputPath = _configuration.Value.DirectoryOutput;

                using (BinaryReader stream = new BinaryReader(Request.Body))
                {
                    var content = stream.ReadBytes(Convert.ToInt16(Request.ContentLength));
                    await System.IO.File.WriteAllBytesAsync(outputPath + "/" + date + ".sis.log.gz", content);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
