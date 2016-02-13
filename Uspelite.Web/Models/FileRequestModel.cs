using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RawFile = Services.Common.DTO.RawFile;

namespace Uspelite.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    public class FileRequestModel
    {
        public static Func<FileRequestModel, RawFile> ToRawFile
        {
            get
            {
                return file => new RawFile
                {
                    OriginalFileName = file.OriginalFileName,
                    FileExtension = file.FileExtension,
                    Content = file.ByteArrayContent
                };
            }
        }

        [Required]
        [MaxLength(100)]
        public string OriginalFileName { get; set; }

        [Required]
        [MaxLength(50)]
        public string FileExtension { get; set; }

        [Required]
        public string Base64Content { get; set; }

        public byte[] ByteArrayContent
        {
            get
            {
                return Convert.FromBase64String(this.Base64Content);
            }
        }
    }
}