namespace Uspelite.Web.Models.Common
{
    public interface IRateable
    {
        int Id { get; set; }

        int RateType { get; }

        int? Rating { get; }
    }
}
