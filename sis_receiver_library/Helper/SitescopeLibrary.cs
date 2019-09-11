using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace sis_receiver_library.Helper
{
    public class SitescopeLibrary
    {
        public static void Hello()
        {
            Console.WriteLine("Hello Method from SitescopeLibrary");
        }

        public static void _move_key(Dictionary<String, String> xml,string find)
        {
            foreach(KeyValuePair<String, String> key in xml)
            {
                if(string.Compare(key.Key, find) == 0)
                {

                }                
            }
        }

        public static void Decompress(FileInfo[] fileToDecompress)
        {
            try
            {
                foreach (FileInfo _fileInfo in fileToDecompress)
                {
                    using (FileStream fileToDecompressAsStream = _fileInfo.OpenRead())
                    {
                        string decompressedFileName = @"C:/data/receiver/sitescope/decompressed/" + _fileInfo.Name.Replace("log.gz", "xml");
                        using (FileStream decompressedStream = File.Create(decompressedFileName))
                        {
                            using (GZipStream decompressionStream = new GZipStream(fileToDecompressAsStream, CompressionMode.Decompress))
                            {
                                try
                                {
                                    decompressionStream.CopyTo(decompressedStream);
                                    Console.WriteLine("Decompressed : " + decompressedFileName);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
