using System;

namespace Services.Common.DTO
{
    public class RawFile
    {
        public string OriginalFileName { get; set; }

        public string FileExtension { get; set; }

        public byte[] Content { get; set; }
    }
}
