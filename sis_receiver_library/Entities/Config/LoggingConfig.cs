using System;
using System.Collections.Generic;
using System.Text;

namespace sis_receiver_library.Entities.Config
{
    public class LoggingConfig
    {
        public string PathFormat { get; set; }
        public LogLevelConfig LogLevel { get; set; }
    }
}
