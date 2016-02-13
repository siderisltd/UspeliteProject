
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Common.Contracts
{
    using DTO;
    using Uspelite.Data.Models;

    public interface IDownloadableFilesService 
    {
        Task<File> FileById(int id);

        Task<IEnumerable<File>> AddNew(IEnumerable<RawFile> rawFiles);
    }
}
