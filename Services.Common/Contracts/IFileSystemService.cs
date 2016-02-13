namespace Services.Common.Contracts
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using DTO;
    using DownloadableFile = Uspelite.Data.Models.File;

    public interface IFileSystemService
    {
        Task SaveImages(IEnumerable<ProcessedImage> images);

        Task SaveDownloadableFiles(IEnumerable<DownloadableFile> files);

        FileStream GetFileStream(string filePath, string fileExtension);
    }
}
