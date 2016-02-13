namespace Services.Common
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DTO;
    using Uspelite.Data.Models;
    using Uspelite.Data.Repositories;
    using Uspelite.Web.Infrastructure;
    using Uspelite.Web.Infrastructure.Contracts;

    public class DownloadableFilesService : FileInfoService, IDownloadableFilesService
    {
        private readonly IRepository<File> files;

        public DownloadableFilesService(IObjectFactory objectFactory, IRepository<File> files)
            : base(objectFactory)
        {
            this.files = files;
        }

        public async Task<File> FileById(int id)
        {
            return await this.files
                .All()
                .Where(f => f.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<File>> AddNew(IEnumerable<RawFile> rawFiles)
        {
            var addedFiles = await rawFiles.ForEachAsync(async rawFile =>
            {
                var file = await this.SaveFileInfo<File>(rawFile);
                file.Content = rawFile.Content;
                return file;
            });

            return addedFiles;
        }
    }
}
