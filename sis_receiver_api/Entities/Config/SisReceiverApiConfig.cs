using sis_receiver_library.Entities.Config;

namespace sis_receiver_api.Entities.Config
{
    public class SisReceiverApiConfig
    {
        public string ServerUrls { get; set; }
        public string DirectoryOutput { get; set; }
        public SeriLogConfig Serilog { get; set; }
        public string AllowedHosts { get; set; }
    }
}
