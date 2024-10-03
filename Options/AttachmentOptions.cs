using System.Reflection.PortableExecutable;

namespace WebAppApi.Options
{
    public class AttachmentOptions
    {
        public string AllowedExtensions { get; set; }

        public int MaxSizeInMegaBytes { get; set; }
        public bool EnableCompression { get; set; }
    }
}
