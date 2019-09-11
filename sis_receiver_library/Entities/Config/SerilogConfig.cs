using System;
using System.Collections.Generic;
using System.Text;

namespace sis_receiver_library.Entities.Config
{
    public class SeriLogConfig
    {
        public string MinimumLevel { get; set; }
        public List<WriteToConfig> WriteTo { get; set; }
    }
}
