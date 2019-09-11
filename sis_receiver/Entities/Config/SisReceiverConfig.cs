using sis_receiver_library.Entities.Config;

namespace sis_receiver.Entities.Config
{
    public class SisReceiverConfig
    {
        public string InputDirectory{ get; set; }
        public string ProcessDirectory { get; set; }
        public string DecompressDirectory { get; set; }
        public string OutputDirectory { get; set; }
        public string EnableArchive { get; set; }
        public string Interval { get; set; }
        public SeriLogConfig Serilog { get; set; }
    }
}
