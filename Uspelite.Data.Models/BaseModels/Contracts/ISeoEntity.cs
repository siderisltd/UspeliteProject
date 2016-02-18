namespace Uspelite.Data.Models.BaseModels.Contracts
{
    public interface ISeoEntity : IAuditInfo
    {
        string Title { get; set; }

        string Slug { get; set; }
    }
}
