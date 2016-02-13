namespace Services.Common
{
    using System;
    using System.Threading.Tasks;
    using DTO;
    using Extensions;
    using Uspelite.Data;
    using Uspelite.Data.Models;
    using Uspelite.Web.Infrastructure.Contracts;

    public abstract class FileInfoService
    {
        private const char WhiteSpace = ' ';

        private readonly IObjectFactory objectFactory;

        protected FileInfoService(IObjectFactory objectFactory)
        {
            this.objectFactory = objectFactory;
        }

        public async Task<T> SaveFileInfo<T>(RawFile file)
            where T : FileInfo, new()
        {
            var processedFileName = string.Join(WhiteSpace.ToString(), file.OriginalFileName.Split(new[] { WhiteSpace }, StringSplitOptions.RemoveEmptyEntries));
            var databaseFile = new T { OriginalFileName = processedFileName, FileExtension = file.FileExtension };

            var filesContext = this.objectFactory.GetInstance<UspeliteDbContext>();
            filesContext.Set<T>().Add(databaseFile);
            await filesContext.SaveChangesAsync();

            databaseFile.UrlPath = databaseFile.Id.ToUrlPath();
            await filesContext.SaveChangesAsync();

            return databaseFile;
        }
    }
}
