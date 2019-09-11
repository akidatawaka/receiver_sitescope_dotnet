using System;
using System.Collections.Generic;
using System.Text;

namespace sis_receiver_library.Entities.Config
{
    public class ArgsConfig
    {
        public string LogDirectory { get; set; }
        public int FileSizeLimitBytes { get; set; }
        public string PathFormat { get; set; }
        public string OutputTemplate { get; set; }
    }
}
